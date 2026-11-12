using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("BDSUtilVerification")]

namespace BDSUtil.Helpers
{
    /// <summary>
    /// Native .NET 8 UPnP Internet Gateway and External Port Reachability Engine (replaces Open.NAT).
    /// </summary>
    public static class UPnPEngine
    {
        private static readonly TimeSpan GatewayCacheTtl = TimeSpan.FromSeconds(30);
        private static readonly byte[] RakNetOfflineMagic =
        {
            0x00, 0xFF, 0xFF, 0x00, 0xFE, 0xFE, 0xFE, 0xFE,
            0xFD, 0xFD, 0xFD, 0xFD, 0x12, 0x34, 0x56, 0x78
        };

        private static readonly HttpClient _wanHttpClient = new HttpClient(new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2),
            ConnectTimeout = TimeSpan.FromSeconds(3)
        })
        {
            Timeout = TimeSpan.FromSeconds(3)
        };

        // Tracks active leases opened during this session so application exit can tear them down cleanly.
        private static readonly HashSet<(int Port, UpnpProtocol Protocol)> _activeLeases = new HashSet<(int Port, UpnpProtocol Protocol)>();
        private static readonly object _leaseLock = new object();
        private static int _activePrimaryGamePort;
        private static List<string> _activeGameUdpTokens = new List<string>();

        private static readonly object _gatewayLock = new object();
        private static UpnpGatewayClient _cachedGateway;
        private static DateTime _gatewayCachedAtUtc = DateTime.MinValue;

        public static string GetLocalIPAddress()
        {
            try
            {
                using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.Connect("8.8.8.8", 65530);
                if (socket.LocalEndPoint is IPEndPoint endPoint)
                {
                    return endPoint.Address.ToString();
                }
            }
            catch { }

            foreach (string ip in GetLocalIPAddresses())
            {
                return ip;
            }

            return "127.0.0.1";
        }

        /// <summary>
        /// Resolves the local IPv4 interface address used to communicate with the discovered UPnP gateway,
        /// avoiding VPN or virtual adapter mismatches when issuing AddPortMapping.
        /// </summary>
        public static string GetLocalIpForGateway(UpnpGatewayClient gateway)
        {
            Uri targetUri = gateway?.ControlUri ?? gateway?.LocationUri;
            if (targetUri != null && !string.IsNullOrWhiteSpace(targetUri.Host))
            {
                try
                {
                    int port = targetUri.Port > 0 ? targetUri.Port : 80;
                    using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    socket.Connect(targetUri.Host, port);
                    if (socket.LocalEndPoint is IPEndPoint endPoint &&
                        endPoint.Address.AddressFamily == AddressFamily.InterNetwork &&
                        !IPAddress.IsLoopback(endPoint.Address))
                    {
                        return endPoint.Address.ToString();
                    }
                }
                catch { }
            }

            return GetLocalIPAddress();
        }

        public static IReadOnlyCollection<string> GetLocalIPAddresses()
        {
            var addresses = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus != OperationalStatus.Up ||
                        nic.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                        nic.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                    {
                        continue;
                    }

                    foreach (UnicastIPAddressInformation unicast in nic.GetIPProperties().UnicastAddresses)
                    {
                        if (unicast.Address.AddressFamily == AddressFamily.InterNetwork &&
                            !IPAddress.IsLoopback(unicast.Address))
                        {
                            addresses.Add(unicast.Address.ToString());
                        }
                    }
                }
            }
            catch { }

            if (addresses.Count == 0)
            {
                addresses.Add("127.0.0.1");
            }

            return addresses;
        }

        public static string GetExternalIPAddress() =>
            GetExternalIPAddressAsync().GetAwaiter().GetResult();

        public static async Task<string> GetExternalIPAddressAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                string trace = await _wanHttpClient.GetStringAsync("https://cloudflare.com/cdn-cgi/trace", cancellationToken).ConfigureAwait(false);
                foreach (string line in trace.Split('\n'))
                {
                    if (line.StartsWith("ip=", StringComparison.OrdinalIgnoreCase))
                    {
                        string candidate = line.Substring(3).Trim();
                        if (IsValidPublicIPv4(candidate))
                            return candidate;
                    }
                }
            }
            catch { }

            string[] fallbackEndpoints =
            {
                "https://api.ipify.org",
                "https://checkip.amazonaws.com"
            };

            foreach (string endpoint in fallbackEndpoints)
            {
                try
                {
                    string raw = (await _wanHttpClient.GetStringAsync(endpoint, cancellationToken).ConfigureAwait(false)).Trim();
                    if (IsValidPublicIPv4(raw))
                        return raw;
                }
                catch { }
            }

            return "127.0.0.1";
        }

        public static bool IsPrivateOrCgnatIPv4(string ipString)
        {
            if (!IPAddress.TryParse(ipString?.Trim(), out IPAddress ip) || ip.AddressFamily != AddressFamily.InterNetwork)
                return false;

            byte[] b = ip.GetAddressBytes();
            if (b[0] == 0) return true;                                  // 0.0.0.0/8
            if (b[0] == 10) return true;                                 // 10.0.0.0/8 (RFC 1918)
            if (b[0] == 127) return true;                                // 127.0.0.0/8 (Loopback)
            if (b[0] == 169 && b[1] == 254) return true;                 // 169.254.0.0/16 (Link-local)
            if (b[0] == 172 && b[1] >= 16 && b[1] <= 31) return true;    // 172.16.0.0/12 (RFC 1918)
            if (b[0] == 192 && b[1] == 168) return true;                 // 192.168.0.0/16 (RFC 1918)
            if (b[0] == 100 && b[1] >= 64 && b[1] <= 127) return true;   // 100.64.0.0/10 (Carrier-Grade NAT RFC 6598)
            return false;
        }

        public static bool IsValidPublicIPv4(string ipString)
        {
            return IPAddress.TryParse(ipString?.Trim(), out IPAddress ip) &&
                   ip.AddressFamily == AddressFamily.InterNetwork &&
                   !IsPrivateOrCgnatIPv4(ipString);
        }

        public static async Task<UpnpGatewayClient> DiscoverGatewayAsync(
            TimeSpan? timeout = null,
            bool forceRefresh = false,
            CancellationToken cancellationToken = default)
        {
            if (!forceRefresh)
            {
                lock (_gatewayLock)
                {
                    if (_cachedGateway != null && (DateTime.UtcNow - _gatewayCachedAtUtc) < GatewayCacheTtl)
                    {
                        return _cachedGateway;
                    }
                }
            }

            UpnpGatewayClient discovered = await UpnpGatewayClient.DiscoverAsync(
                timeout ?? TimeSpan.FromMilliseconds(2500),
                cancellationToken: cancellationToken).ConfigureAwait(false);

            if (discovered != null)
            {
                lock (_gatewayLock)
                {
                    _cachedGateway = discovered;
                    _gatewayCachedAtUtc = DateTime.UtcNow;
                }
            }

            return discovered;
        }

        public static void InvalidateGatewayCache()
        {
            lock (_gatewayLock)
            {
                _cachedGateway = null;
                _gatewayCachedAtUtc = DateTime.MinValue;
            }
        }

        public static async Task<IReadOnlyList<UpnpPortMapping>> GetAllMappingsAsync(
            UpnpGatewayClient gatewayOverride = null,
            CancellationToken cancellationToken = default)
        {
            UpnpGatewayClient gateway = gatewayOverride ?? await DiscoverGatewayAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            if (gateway == null) return Array.Empty<UpnpPortMapping>();

            try
            {
                return await gateway.GetAllPortMappingsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                if (gatewayOverride == null)
                {
                    InvalidateGatewayCache();
                }
                return Array.Empty<UpnpPortMapping>();
            }
        }

        public static async Task<UpnpPortMapping> GetSpecificMappingAsync(
            int externalPort,
            string protocolType = "UDP",
            UpnpGatewayClient gatewayOverride = null,
            CancellationToken cancellationToken = default)
        {
            UpnpGatewayClient gateway = gatewayOverride ?? await DiscoverGatewayAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            if (gateway == null) return null;

            return await QueryPortMappingAsync(gateway, externalPort, ParseProtocol(protocolType), cancellationToken).ConfigureAwait(false);
        }

        public static async Task<bool> IsPortForwardedToHostAsync(
            int externalPort,
            int? expectedInternalPort = null,
            string protocolType = "UDP",
            UpnpGatewayClient gatewayOverride = null,
            CancellationToken cancellationToken = default)
        {
            UpnpProtocol protocol = ParseProtocol(protocolType);
            UpnpPortMapping mapping = await GetSpecificMappingAsync(externalPort, protocolType, gatewayOverride, cancellationToken).ConfigureAwait(false);
            if (mapping == null || !mapping.Enabled || mapping.Protocol != protocol)
                return false;

            if (expectedInternalPort.HasValue && mapping.InternalPort != expectedInternalPort.Value)
                return false;

            var hostIps = new HashSet<string>(GetLocalIPAddresses(), StringComparer.OrdinalIgnoreCase);
            hostIps.Add(GetLocalIPAddress());
            return hostIps.Contains(mapping.InternalClient);
        }

        /// <summary>
        /// Standard 3-parameter variant (Defaults to UDP for dynamic multi-node tracking pools).
        /// </summary>
        public static bool ForwardPort(int externalPort, int internalPort, string localIp) =>
            ForwardPort(externalPort, internalPort, localIp, "UDP");

        /// <summary>
        /// 4-parameter overload to handle explicit TCP and UDP allocations synchronously.
        /// </summary>
        public static bool ForwardPort(int externalPort, int internalPort, string localIp, string protocolType) =>
            ForwardPortAsync(externalPort, internalPort, localIp, protocolType).GetAwaiter().GetResult();

        public static void RegisterSessionLease(int externalPort, string protocolType = "UDP")
        {
            if (externalPort <= 0 || externalPort > 65535) return;
            UpnpProtocol protocol = ParseProtocol(protocolType);
            lock (_leaseLock)
            {
                _activeLeases.Add((externalPort, protocol));
            }
        }

        /// <summary>
        /// Forwards a port on the UPnP gateway and verifies the router persisted the lease targeting this host.
        /// </summary>
        public static async Task<bool> ForwardPortAsync(
            int externalPort,
            int internalPort,
            string localIp = null,
            string protocolType = "UDP",
            bool verifyAfterForward = true,
            UpnpGatewayClient gatewayOverride = null,
            CancellationToken cancellationToken = default,
            string description = null)
        {
            if (externalPort <= 0 || externalPort > 65535 || internalPort <= 0 || internalPort > 65535)
                return false;

            string explicitIp = string.IsNullOrWhiteSpace(localIp) ? null : localIp.Trim();
            UpnpProtocol protocol = ParseProtocol(protocolType);
            string effectiveDescription = string.IsNullOrWhiteSpace(description)
                ? $"BDSUtil Node {externalPort}"
                : description.Trim();

            for (int attempt = 0; attempt < 2; attempt++)
            {
                bool forceRefresh = attempt > 0 && gatewayOverride == null;
                UpnpGatewayClient gateway = gatewayOverride ?? await DiscoverGatewayAsync(forceRefresh: forceRefresh, cancellationToken: cancellationToken).ConfigureAwait(false);
                if (gateway == null) return false;

                string targetIp = explicitIp ?? GetLocalIpForGateway(gateway);

                try
                {
                    await gateway.AddPortMappingAsync(
                        externalPort,
                        internalPort,
                        targetIp,
                        protocol,
                        effectiveDescription,
                        leaseDurationSeconds: 0,
                        cancellationToken: cancellationToken).ConfigureAwait(false);

                    if (verifyAfterForward)
                    {
                        UpnpPortMapping verified = await QueryPortMappingAsync(gateway, externalPort, protocol, cancellationToken).ConfigureAwait(false);
                        if (verified != null)
                        {
                            var allowedIps = new HashSet<string>(GetLocalIPAddresses(), StringComparer.OrdinalIgnoreCase) { targetIp };
                            if (!verified.Enabled || !allowedIps.Contains(verified.InternalClient) || verified.InternalPort != internalPort)
                            {
                                return false;
                            }
                        }
                    }

                    lock (_leaseLock)
                    {
                        _activeLeases.Add((externalPort, protocol));
                    }
                    return true;
                }
                catch (HttpRequestException) when (attempt == 0 && gatewayOverride == null)
                {
                    // Cached gateway might have changed port/IP; retry once with fresh SSDP discovery.
                    InvalidateGatewayCache();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[UPnP ForwardPort ({externalPort}/{protocol})]: {ex.Message}");
                    return false;
                }
            }

            return false;
        }

        public static bool ClosePort(int externalPort) =>
            ClosePort(externalPort, "UDP");

        public static bool ClosePort(int externalPort, string protocolType) =>
            ClosePortAsync(externalPort, protocolType).GetAwaiter().GetResult();

        public static async Task<bool> ClosePortAsync(
            int externalPort,
            string protocolType = "UDP",
            UpnpGatewayClient gatewayOverride = null,
            CancellationToken cancellationToken = default)
        {
            if (externalPort <= 0 || externalPort > 65535)
                return false;

            UpnpProtocol protocol = ParseProtocol(protocolType);

            try
            {
                UpnpGatewayClient gateway = gatewayOverride ?? await DiscoverGatewayAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
                if (gateway == null) return false;

                bool deleted = await gateway.DeletePortMappingAsync(externalPort, protocol, cancellationToken: cancellationToken).ConfigureAwait(false);
                if (deleted)
                {
                    lock (_leaseLock)
                    {
                        _activeLeases.Remove((externalPort, protocol));
                    }
                }
                return deleted;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UPnP Manual Close Port Failure ({externalPort}/{protocol})]: {ex.Message}");
                return false;
            }
        }

        public static void TearDownPorts() =>
            TearDownPortsAsync().GetAwaiter().GetResult();

        public static async Task TearDownPortsAsync(
            UpnpGatewayClient gatewayOverride = null,
            CancellationToken cancellationToken = default)
        {
            List<(int Port, UpnpProtocol Protocol)> leasesToPurge;
            lock (_leaseLock)
            {
                _activePrimaryGamePort = 0;
                _activeGameUdpTokens.Clear();
                if (_activeLeases.Count == 0) return;
                leasesToPurge = new List<(int Port, UpnpProtocol Protocol)>(_activeLeases);
            }

            try
            {
                UpnpGatewayClient gateway = gatewayOverride ?? await DiscoverGatewayAsync(TimeSpan.FromMilliseconds(3000), cancellationToken: cancellationToken).ConfigureAwait(false);
                if (gateway == null) return;

                foreach (var (port, protocol) in leasesToPurge)
                {
                    try
                    {
                        await gateway.DeletePortMappingAsync(port, protocol, cancellationToken: cancellationToken).ConfigureAwait(false);
                        lock (_leaseLock)
                        {
                            _activeLeases.Remove((port, protocol));
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[UPnP Individual Purge Failure]: Port {port} ({protocol}) - {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UPnP Global Hardware Disconnect Failure]: {ex.Message}");
            }
        }

        /// <summary>
        /// Records the primary game port and <c>server-udp-ports</c> tokens requisitioned during this session.
        /// </summary>
        public static void RecordActiveGameServerMapping(int primaryPort, IEnumerable<string> udpNodeTokens)
        {
            lock (_leaseLock)
            {
                _activePrimaryGamePort = primaryPort > 0 && primaryPort <= 65535 ? primaryPort : 0;
                _activeGameUdpTokens = udpNodeTokens != null
                    ? udpNodeTokens.Where(t => !string.IsNullOrWhiteSpace(t)).ToList()
                    : new List<string>();
            }
        }

        /// <summary>
        /// Retrieves a snapshot of the currently active session game port mappings, if any have been requisitioned.
        /// </summary>
        public static bool TryGetActiveGameServerMapping(out int primaryPort, out IReadOnlyList<string> udpNodeTokens)
        {
            lock (_leaseLock)
            {
                primaryPort = _activePrimaryGamePort;
                if (_activePrimaryGamePort <= 0 || _activeGameUdpTokens.Count == 0)
                {
                    udpNodeTokens = Array.Empty<string>();
                    return false;
                }

                udpNodeTokens = _activeGameUdpTokens.ToArray();
                return true;
            }
        }

        /// <summary>
        /// Attempts to apply the session's already-requisitioned UPnP UDP ports directly to a newly selected server directory's
        /// <c>server.properties</c>. Returns <c>false</c> when no game ports are leased yet or when the new server requires different ports.
        /// </summary>
        public static bool TryApplyRequisitionedPortsToDirectory(
            string newServerDirectory,
            bool consolidateUdpPorts,
            out int appliedNodeCount,
            out string statusDetail)
        {
            appliedNodeCount = 0;
            statusDetail = string.Empty;

            if (string.IsNullOrWhiteSpace(newServerDirectory))
            {
                statusDetail = "Target server directory is empty.";
                return false;
            }

            string propsPath = Path.Combine(newServerDirectory, "server.properties");
            if (!File.Exists(propsPath))
            {
                statusDetail = "server.properties was not found in the selected server directory.";
                return false;
            }

            if (!TryGetActiveGameServerMapping(out int leasedPrimaryPort, out IReadOnlyList<string> activeTokens))
            {
                statusDetail = "No game server UPnP ports have been requisitioned yet in this session.";
                return false;
            }

            var targetSnapshot = RemoteApi.ServerPortDiscovery.ReadServerPortSnapshot(newServerDirectory);
            if (targetSnapshot.PrimaryPortV4 != leasedPrimaryPort)
            {
                statusDetail = $"The new server uses server-port={targetSnapshot.PrimaryPortV4}, whereas active UPnP leases are bound to port {leasedPrimaryPort}.";
                return false;
            }

            int requiredNodeCount = RemoteApi.ServerPortDiscovery.GetTargetUdpNodeCount(targetSnapshot.MaxPlayers, consolidateUdpPorts);
            if (activeTokens.Count < requiredNodeCount)
            {
                statusDetail = $"The new server requires {requiredNodeCount} UDP node port(s), but only {activeTokens.Count} port(s) are currently leased.";
                return false;
            }

            string joinedTokens = string.Join(",", activeTokens.Take(requiredNodeCount));
            ServerPropertiesDocument.SetServerUdpPorts(propsPath, joinedTokens);
            ServerPropertiesDocument.CommentOutServerPortV6(propsPath);

            appliedNodeCount = requiredNodeCount;
            statusDetail = $"Applied {appliedNodeCount} already-requisitioned UDP port mapping(s) (primary port {leasedPrimaryPort}) to server.properties.";
            return true;
        }

        /// <summary>
        /// Verifies both whether the port is forwarded to this host machine and whether it is accessible via the external IP.
        /// </summary>
        public static Task<PortForwardVerificationResult> VerifyPortAsync(
            int externalPort,
            int? internalPort = null,
            string protocolType = "UDP",
            string externalProbeUrl = null,
            CancellationToken cancellationToken = default)
        {
            return VerifyPortAsync(
                externalPort,
                internalPort,
                protocolType,
                new PortVerificationOptions { ExternalProbeUrl = externalProbeUrl ?? string.Empty },
                cancellationToken);
        }

        public static async Task<PortForwardVerificationResult> VerifyPortAsync(
            int externalPort,
            int? internalPort,
            string protocolType,
            PortVerificationOptions options,
            CancellationToken cancellationToken = default)
        {
            options ??= new PortVerificationOptions();

            int effectiveInternalPort = internalPort ?? externalPort;
            UpnpProtocol protocol = ParseProtocol(protocolType);
            string protoLabel = protocol == UpnpProtocol.Tcp ? "TCP" : "UDP";

            string localIp = string.IsNullOrWhiteSpace(options.LocalIpOverride)
                ? GetLocalIPAddress()
                : options.LocalIpOverride.Trim();

            var hostIps = new HashSet<string>(GetLocalIPAddresses(), StringComparer.OrdinalIgnoreCase) { localIp };

            var result = new PortForwardVerificationResult
            {
                ExternalPort = externalPort,
                InternalPort = effectiveInternalPort,
                Protocol = protocol,
                LocalIp = localIp
            };

            if (externalPort <= 0 || externalPort > 65535 || effectiveInternalPort <= 0 || effectiveInternalPort > 65535)
            {
                result.IsExternallyAccessible = false;
                result.AccessibilityState = PortAccessibilityState.Unknown;
                result.DiagnosticMessage = $"Invalid port specification ({externalPort} -> {effectiveInternalPort}).";
                return result;
            }

            Task<string> publicIpTask = string.IsNullOrWhiteSpace(options.PublicExternalIpOverride)
                ? GetExternalIPAddressAsync(cancellationToken)
                : Task.FromResult(options.PublicExternalIpOverride.Trim());

            Task<UpnpGatewayClient> gatewayTask = options.Gateway != null
                ? Task.FromResult(options.Gateway)
                : (options.SkipGatewayDiscovery
                    ? Task.FromResult<UpnpGatewayClient>(null)
                    : DiscoverGatewayAsync(TimeSpan.FromMilliseconds(2500), cancellationToken: cancellationToken));

            await Task.WhenAll(publicIpTask, gatewayTask).ConfigureAwait(false);

            string publicIp = publicIpTask.Result;
            UpnpGatewayClient gateway = gatewayTask.Result;

            result.PublicExternalIp = publicIp;
            result.GatewayFound = gateway != null;
            result.GatewayDescription = gateway?.FriendlyName ?? string.Empty;

            UpnpPortMapping mapping = null;
            if (gateway != null)
            {
                try
                {
                    result.RouterExternalIp = !string.IsNullOrWhiteSpace(options.RouterExternalIpOverride)
                        ? options.RouterExternalIpOverride.Trim()
                        : await gateway.GetExternalIPAddressAsync(cancellationToken).ConfigureAwait(false);
                }
                catch { }

                mapping = await QueryPortMappingAsync(gateway, externalPort, protocol, cancellationToken).ConfigureAwait(false);
                if (mapping != null && mapping.Enabled)
                {
                    result.MappedInternalClient = mapping.InternalClient;

                    if (hostIps.Contains(mapping.InternalClient))
                    {
                        if (mapping.InternalPort == effectiveInternalPort)
                        {
                            result.IsForwardedToHost = true;
                        }
                        else
                        {
                            result.IsExternallyAccessible = false;
                            result.AccessibilityState = PortAccessibilityState.NotForwarded;
                            result.DiagnosticMessage =
                                $"Router maps {protoLabel} {externalPort} to this host ({mapping.InternalClient}), but on internal port {mapping.InternalPort} instead of {effectiveInternalPort}.";
                            return result;
                        }
                    }
                    else
                    {
                        result.IsAssignedToAnotherHost = true;
                        result.IsExternallyAccessible = false;
                        result.AccessibilityState = PortAccessibilityState.ConflictWithAnotherHost;
                        result.DiagnosticMessage =
                            $"Port {protoLabel} {externalPort} is currently forwarded to another machine on your LAN ({mapping.InternalClient}:{mapping.InternalPort}).";
                        return result;
                    }
                }
            }

            // Detect CGNAT / Double NAT whenever the router reports a WAN IP.
            if (!string.IsNullOrWhiteSpace(result.RouterExternalIp))
            {
                bool routerWanPrivateOrCgnat = IsPrivateOrCgnatIPv4(result.RouterExternalIp);
                bool wanDiffersFromPublic = IsValidPublicIPv4(publicIp) &&
                                            !string.Equals(result.RouterExternalIp, publicIp, StringComparison.OrdinalIgnoreCase);

                if (routerWanPrivateOrCgnat || wanDiffersFromPublic)
                {
                    result.IsBehindDoubleNatOrCgnat = true;
                    result.IsExternallyAccessible = false;
                    result.AccessibilityState = PortAccessibilityState.BlockedByDoubleNatOrCgnat;
                    result.DiagnosticMessage = routerWanPrivateOrCgnat
                        ? $"Double-NAT / CGNAT detected: Gateway WAN IP is {result.RouterExternalIp} while public IP is {publicIp}. Traffic from the external IP cannot reach this host without upstream forwarding."
                        : $"WAN IP mismatch: Gateway reports WAN IP {result.RouterExternalIp}, but external lookup sees {publicIp}. Upstream NAT/firewall blocks direct external access.";
                    return result;
                }
            }

            string probeTargetIp = !string.IsNullOrWhiteSpace(options.ExternalProbeTargetIpOverride)
                ? options.ExternalProbeTargetIpOverride.Trim()
                : (IsValidPublicIPv4(publicIp) ? publicIp : result.RouterExternalIp);

            int timeoutMs = Math.Max(400, options.TimeoutMs);
            bool connectionRejected = false;

            // Stage 1: Direct external IP probe (echo responder when port is idle, or RakNet/TCP connect when bound).
            if (!string.IsNullOrWhiteSpace(probeTargetIp) &&
                (!probeTargetIp.Equals("127.0.0.1", StringComparison.Ordinal) || !string.IsNullOrWhiteSpace(options.ExternalProbeTargetIpOverride)))
            {
                IPAddress localBindIp = options.LocalEchoBindIpOverride ?? IPAddress.Any;
                var probeOutcome = await ProbeEndpointAsync(
                    probeTargetIp,
                    externalPort,
                    effectiveInternalPort,
                    protocol,
                    localBindIp,
                    timeoutMs,
                    cancellationToken).ConfigureAwait(false);

                result.LocalListenerDetected = probeOutcome.LocalListenerDetected;
                result.UsedEphemeralProbeListener = probeOutcome.UsedEphemeralListener;
                result.ExternalProbeSucceeded = probeOutcome.Succeeded;
                result.RoundTripTime = probeOutcome.RoundTripTime;
                if (!string.IsNullOrWhiteSpace(probeOutcome.ServerMotd))
                {
                    result.ServerMotd = probeOutcome.ServerMotd;
                }
                connectionRejected = probeOutcome.RejectedByPeer;
            }

            // Stage 2: Optional external verification relay endpoint (only needed when Stage 1 did not succeed).
            if (!result.ExternalProbeSucceeded &&
                !string.IsNullOrWhiteSpace(options.ExternalProbeUrl) &&
                !string.IsNullOrWhiteSpace(probeTargetIp) &&
                ExternalPortChecker.SupportsProtocol(options.ExternalProbeUrl, protocol))
            {
                bool? relayAccessible = await RunExternalRelayWithEphemeralListenerAsync(
                    options.ExternalProbeUrl,
                    probeTargetIp,
                    externalPort,
                    effectiveInternalPort,
                    protocol,
                    options.LocalEchoBindIpOverride ?? IPAddress.Any,
                    options.CustomHttpClient,
                    cancellationToken).ConfigureAwait(false);

                if (relayAccessible.HasValue)
                {
                    result.ExternalRelayChecked = true;
                    result.ExternalProbeSucceeded = relayAccessible.Value;
                    connectionRejected = !relayAccessible.Value;
                }
            }

            string displayExternalIp = !string.IsNullOrWhiteSpace(probeTargetIp) ? probeTargetIp : publicIp;

            if (result.ExternalProbeSucceeded)
            {
                if (!result.IsForwardedToHost)
                {
                    result.IsForwardedToHost = true;
                    result.ForwardedViaManualOrUnknownRule = true;
                }

                result.IsExternallyAccessible = true;
                result.AccessibilityState = PortAccessibilityState.VerifiedAccessible;

                string rttText = result.RoundTripTime.HasValue ? $" ({(int)result.RoundTripTime.Value.TotalMilliseconds} ms)" : string.Empty;
                string motdNote = !string.IsNullOrWhiteSpace(result.ServerMotd) ? $" [{result.ServerMotd}]" : string.Empty;
                string ruleNote = result.ForwardedViaManualOrUnknownRule
                    ? " (manual router rule or DMZ; no UPnP lease required)"
                    : $" -> {localIp}:{effectiveInternalPort}";

                result.DiagnosticMessage =
                    $"Port {protoLabel} {externalPort}{ruleNote} is verified accessible from external IP {displayExternalIp}{rttText}.{motdNote}";
                return result;
            }

            if (result.ExternalRelayChecked)
            {
                result.IsExternallyAccessible = false;
                result.AccessibilityState = result.IsForwardedToHost
                    ? PortAccessibilityState.LocalListenerUnavailable
                    : (result.GatewayFound ? PortAccessibilityState.NotForwarded : PortAccessibilityState.GatewayUnavailable);

                result.DiagnosticMessage = result.IsForwardedToHost
                    ? $"Router maps {protoLabel} {externalPort} -> {localIp}:{effectiveInternalPort}, but the external probe could not reach {displayExternalIp}:{externalPort} (check Windows Firewall or server process)."
                    : $"External probe confirmed {displayExternalIp}:{externalPort} ({protoLabel}) is NOT accessible from the internet.";
                return result;
            }

            if (result.IsForwardedToHost)
            {
                if (connectionRejected)
                {
                    result.IsExternallyAccessible = false;
                    result.AccessibilityState = PortAccessibilityState.LocalListenerUnavailable;
                    result.DiagnosticMessage =
                        $"Router forwards {protoLabel} {externalPort} -> {localIp}:{effectiveInternalPort}, but probe to {displayExternalIp}:{externalPort} was actively rejected (check local firewall/listener).";
                }
                else
                {
                    result.IsExternallyAccessible = null;
                    result.AccessibilityState = PortAccessibilityState.ForwardedButLoopbackBlocked;
                    string localStatusNote = result.LocalListenerDetected
                        ? "Local service is listening cleanly."
                        : "Local test listener bound cleanly.";
                    bool skippedTcpOnlyCheckerOnUdp = !string.IsNullOrWhiteSpace(options.ExternalProbeUrl) &&
                                                      !ExternalPortChecker.SupportsProtocol(options.ExternalProbeUrl, protocol);
                    string externalNote = skippedTcpOnlyCheckerOnUdp
                        ? $"Selected external checker ({ExternalPortChecker.GetDisplayNameForSpec(options.ExternalProbeUrl)}) tests TCP only, so UDP loopback timeout could not be externally cross-checked."
                        : "Direct probe to " + displayExternalIp + " timed out (many routers block LAN-to-WAN NAT loopback; external players can typically connect if Windows Firewall allows it).";
                    result.DiagnosticMessage =
                        $"Router confirmed forwarding {protoLabel} {externalPort} -> {localIp}:{effectiveInternalPort} with public WAN IP {displayExternalIp}. {localStatusNote} {externalNote}";
                }
                return result;
            }

            if (!result.GatewayFound)
            {
                result.IsExternallyAccessible = connectionRejected ? false : (bool?)null;
                result.AccessibilityState = PortAccessibilityState.GatewayUnavailable;
                result.DiagnosticMessage =
                    $"No UPnP gateway responded on the LAN, and external probe to {displayExternalIp}:{externalPort} ({protoLabel}) did not receive a reply.";
                return result;
            }

            result.IsExternallyAccessible = false;
            result.AccessibilityState = PortAccessibilityState.NotForwarded;
            result.DiagnosticMessage =
                $"Port {protoLabel} {externalPort} has no active UPnP mapping to this host ({localIp}) on the gateway and did not respond via {displayExternalIp}:{externalPort}.";
            return result;
        }

        private static async Task<UpnpPortMapping> QueryPortMappingAsync(
            UpnpGatewayClient gateway,
            int externalPort,
            UpnpProtocol protocol,
            CancellationToken cancellationToken)
        {
            try
            {
                return await gateway.GetSpecificPortMappingEntryAsync(externalPort, protocol, cancellationToken: cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                try
                {
                    IReadOnlyList<UpnpPortMapping> all = await gateway.GetAllPortMappingsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
                    return all.FirstOrDefault(m => m.ExternalPort == externalPort && m.Protocol == protocol);
                }
                catch
                {
                    return null;
                }
            }
        }

        private readonly struct ProbeOutcome
        {
            public bool Succeeded { get; init; }
            public bool LocalListenerDetected { get; init; }
            public bool UsedEphemeralListener { get; init; }
            public bool RejectedByPeer { get; init; }
            public string ServerMotd { get; init; }
            public TimeSpan? RoundTripTime { get; init; }
        }

        private static async Task<ProbeOutcome> ProbeEndpointAsync(
            string targetIp,
            int externalPort,
            int internalPort,
            UpnpProtocol protocol,
            IPAddress localBindIp,
            int timeoutMs,
            CancellationToken cancellationToken)
        {
            return protocol == UpnpProtocol.Tcp
                ? await ProbeTcpEndpointAsync(targetIp, externalPort, internalPort, localBindIp, timeoutMs, cancellationToken).ConfigureAwait(false)
                : await ProbeUdpEndpointAsync(targetIp, externalPort, internalPort, localBindIp, timeoutMs, cancellationToken).ConfigureAwait(false);
        }

        private static async Task<ProbeOutcome> ProbeUdpEndpointAsync(
            string targetIp,
            int externalPort,
            int internalPort,
            IPAddress localBindIp,
            int timeoutMs,
            CancellationToken cancellationToken)
        {
            UdpClient ephemeralListener = null;
            using var echoCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            try
            {
                try
                {
                    ephemeralListener = new UdpClient(new IPEndPoint(localBindIp, internalPort));
                }
                catch (SocketException)
                {
                    ephemeralListener = null;
                }

                if (ephemeralListener != null)
                {
                    string nonce = Guid.NewGuid().ToString("N");
                    byte[] expectedProbe = Encoding.UTF8.GetBytes("BDSUTIL_PROBE:" + nonce);
                    byte[] ackBytes = Encoding.UTF8.GetBytes("BDSUTIL_ACK:" + nonce);

                    // Post the first ReceiveAsync before sending so no scheduler delay races the initial UDP echo.
                    Task<UdpReceiveResult> initialReceiveTask = ephemeralListener.ReceiveAsync(echoCts.Token).AsTask();
                    Task echoServerTask = Task.Run(async () =>
                    {
                        Task<UdpReceiveResult> pendingReceive = initialReceiveTask;
                        while (!echoCts.IsCancellationRequested)
                        {
                            try
                            {
                                UdpReceiveResult received = await pendingReceive.ConfigureAwait(false);
                                string text = Encoding.UTF8.GetString(received.Buffer);
                                if (text == "BDSUTIL_PROBE:" + nonce)
                                {
                                    await ephemeralListener.SendAsync(ackBytes, ackBytes.Length, received.RemoteEndPoint).ConfigureAwait(false);
                                    break;
                                }
                                pendingReceive = ephemeralListener.ReceiveAsync(echoCts.Token).AsTask();
                            }
                            catch
                            {
                                break;
                            }
                        }
                    }, CancellationToken.None);

                    using var senderClient = new UdpClient(new IPEndPoint(localBindIp, 0));
                    var targetEp = new IPEndPoint(IPAddress.Parse(targetIp), externalPort);
                    var sw = Stopwatch.StartNew();

                    for (int attempt = 0; attempt < 2; attempt++)
                    {
                        await senderClient.SendAsync(expectedProbe, expectedProbe.Length, targetEp).ConfigureAwait(false);
                        using var waitCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                        waitCts.CancelAfter(Math.Max(250, timeoutMs / 2));

                        try
                        {
                            UdpReceiveResult reply = await senderClient.ReceiveAsync(waitCts.Token).ConfigureAwait(false);
                            sw.Stop();
                            if (Encoding.UTF8.GetString(reply.Buffer) == "BDSUTIL_ACK:" + nonce)
                            {
                                echoCts.Cancel();
                                return new ProbeOutcome
                                {
                                    Succeeded = true,
                                    UsedEphemeralListener = true,
                                    RoundTripTime = sw.Elapsed
                                };
                            }
                        }
                        catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset || ex.SocketErrorCode == SocketError.ConnectionRefused)
                        {
                            echoCts.Cancel();
                            return new ProbeOutcome { UsedEphemeralListener = true, RejectedByPeer = true };
                        }
                        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                        {
                            // Retry once inside budget.
                        }
                    }

                    echoCts.Cancel();
                    try { await Task.WhenAny(echoServerTask, Task.Delay(50, CancellationToken.None)).ConfigureAwait(false); } catch { }
                    return new ProbeOutcome { UsedEphemeralListener = true };
                }

                // Port already in use (e.g., bedrock_server.exe is running): probe with RakNet Unconnected Ping!
                var rakExternal = await SendRakNetPingAsync(targetIp, externalPort, timeoutMs, cancellationToken).ConfigureAwait(false);
                if (rakExternal.Succeeded)
                {
                    return new ProbeOutcome
                    {
                        Succeeded = true,
                        LocalListenerDetected = true,
                        ServerMotd = rakExternal.ServerMotd,
                        RoundTripTime = rakExternal.RoundTripTime
                    };
                }

                // Confirm whether BDS responds locally on the host even if NAT loopback fails.
                var rakLocal = await SendRakNetPingAsync(
                    localBindIp.Equals(IPAddress.Any) ? "127.0.0.1" : localBindIp.ToString(),
                    internalPort,
                    Math.Min(500, timeoutMs),
                    cancellationToken).ConfigureAwait(false);

                return new ProbeOutcome
                {
                    LocalListenerDetected = true,
                    RejectedByPeer = rakExternal.RejectedByPeer,
                    ServerMotd = rakLocal.ServerMotd
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UPnP UDP Probe Error]: {ex.Message}");
                return new ProbeOutcome();
            }
            finally
            {
                echoCts.Cancel();
                ephemeralListener?.Dispose();
            }
        }

        private static async Task<ProbeOutcome> ProbeTcpEndpointAsync(
            string targetIp,
            int externalPort,
            int internalPort,
            IPAddress localBindIp,
            int timeoutMs,
            CancellationToken cancellationToken)
        {
            TcpListener ephemeralListener = null;
            using var acceptCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            try
            {
                try
                {
                    ephemeralListener = new TcpListener(localBindIp, internalPort);
                    ephemeralListener.Start(1);
                }
                catch (SocketException)
                {
                    ephemeralListener = null;
                }

                if (ephemeralListener != null)
                {
                    string nonce = Guid.NewGuid().ToString("N");
                    Task serverTask = Task.Run(async () =>
                    {
                        try
                        {
                            using TcpClient accepted = await ephemeralListener.AcceptTcpClientAsync(acceptCts.Token).ConfigureAwait(false);
                            using NetworkStream stream = accepted.GetStream();
                            using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);
                            using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };

                            string line = await reader.ReadLineAsync(acceptCts.Token).ConfigureAwait(false);
                            if (line == "BDSUTIL_PROBE:" + nonce)
                            {
                                await writer.WriteLineAsync("BDSUTIL_ACK:" + nonce).ConfigureAwait(false);
                            }
                        }
                        catch { }
                    }, CancellationToken.None);

                    using var client = new TcpClient(AddressFamily.InterNetwork);
                    using var connectCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                    connectCts.CancelAfter(timeoutMs);

                    var sw = Stopwatch.StartNew();
                    try
                    {
                        await client.ConnectAsync(IPAddress.Parse(targetIp), externalPort, connectCts.Token).ConfigureAwait(false);
                        using NetworkStream clientStream = client.GetStream();
                        using var writer = new StreamWriter(clientStream, Encoding.UTF8, leaveOpen: true) { AutoFlush = true };
                        using var reader = new StreamReader(clientStream, Encoding.UTF8, leaveOpen: true);

                        await writer.WriteLineAsync("BDSUTIL_PROBE:" + nonce).ConfigureAwait(false);
                        string ack = await reader.ReadLineAsync(connectCts.Token).ConfigureAwait(false);
                        sw.Stop();

                        if (ack == "BDSUTIL_ACK:" + nonce)
                        {
                            return new ProbeOutcome
                            {
                                Succeeded = true,
                                UsedEphemeralListener = true,
                                RoundTripTime = sw.Elapsed
                            };
                        }
                    }
                    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionRefused || ex.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        return new ProbeOutcome { UsedEphemeralListener = true, RejectedByPeer = true };
                    }
                    catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                    {
                        return new ProbeOutcome { UsedEphemeralListener = true };
                    }
                    finally
                    {
                        acceptCts.Cancel();
                        try { await Task.WhenAny(serverTask, Task.Delay(50, CancellationToken.None)).ConfigureAwait(false); } catch { }
                    }

                    return new ProbeOutcome { UsedEphemeralListener = true };
                }

                // Existing TCP listener bound: attempt direct TCP handshake against externalIp:externalPort.
                using var probeClient = new TcpClient(AddressFamily.InterNetwork);
                using var handshakeCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                handshakeCts.CancelAfter(timeoutMs);

                var connectSw = Stopwatch.StartNew();
                try
                {
                    await probeClient.ConnectAsync(IPAddress.Parse(targetIp), externalPort, handshakeCts.Token).ConfigureAwait(false);
                    connectSw.Stop();
                    return new ProbeOutcome
                    {
                        Succeeded = true,
                        LocalListenerDetected = true,
                        RoundTripTime = connectSw.Elapsed
                    };
                }
                catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionRefused || ex.SocketErrorCode == SocketError.ConnectionReset)
                {
                    return new ProbeOutcome { LocalListenerDetected = true, RejectedByPeer = true };
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    return new ProbeOutcome { LocalListenerDetected = true };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UPnP TCP Probe Error]: {ex.Message}");
                return new ProbeOutcome();
            }
            finally
            {
                acceptCts.Cancel();
                try { ephemeralListener?.Stop(); } catch { }
            }
        }

        private static async Task<ProbeOutcome> SendRakNetPingAsync(
            string targetIp,
            int port,
            int timeoutMs,
            CancellationToken cancellationToken)
        {
            try
            {
                using var udp = new UdpClient(AddressFamily.InterNetwork);
                var endpoint = new IPEndPoint(IPAddress.Parse(targetIp), port);

                byte[] packet = new byte[33];
                packet[0] = 0x01; // RakNet ID_UNCONNECTED_PING
                BinaryPrimitives.WriteInt64BigEndian(packet.AsSpan(1, 8), Environment.TickCount64);
                Buffer.BlockCopy(RakNetOfflineMagic, 0, packet, 9, RakNetOfflineMagic.Length);
                BinaryPrimitives.WriteInt64BigEndian(packet.AsSpan(25, 8), 0x4244535554494C01L);

                var sw = Stopwatch.StartNew();
                for (int attempt = 0; attempt < 2; attempt++)
                {
                    await udp.SendAsync(packet, packet.Length, endpoint).ConfigureAwait(false);
                    using var waitCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                    waitCts.CancelAfter(Math.Max(200, timeoutMs / 2));

                    try
                    {
                        UdpReceiveResult response = await udp.ReceiveAsync(waitCts.Token).ConfigureAwait(false);
                        sw.Stop();
                        if (TryParseRakNetPongMotd(response.Buffer, out string motd))
                        {
                            return new ProbeOutcome
                            {
                                Succeeded = true,
                                ServerMotd = motd,
                                RoundTripTime = sw.Elapsed
                            };
                        }
                    }
                    catch (SocketException ex) when (ex.SocketErrorCode == SocketError.ConnectionReset || ex.SocketErrorCode == SocketError.ConnectionRefused)
                    {
                        return new ProbeOutcome { RejectedByPeer = true };
                    }
                    catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                    {
                        // Retry once.
                    }
                }
            }
            catch { }

            return new ProbeOutcome();
        }

        internal static bool TryParseRakNetPongMotd(byte[] buffer, out string summary)
        {
            summary = string.Empty;
            if (buffer == null || buffer.Length < 35 || buffer[0] != 0x1C)
                return false;

            ushort payloadLength = BinaryPrimitives.ReadUInt16BigEndian(buffer.AsSpan(33, 2));
            if (buffer.Length < 35 + payloadLength || payloadLength == 0)
            {
                summary = "Bedrock RakNet Pong";
                return true;
            }

            string rawId = Encoding.UTF8.GetString(buffer, 35, payloadLength);
            string[] parts = rawId.Split(';');
            if (parts.Length >= 6)
            {
                // MCPE;<motd>;<protocol>;<version>;<players>;<maxPlayers>;...
                summary = $"{parts[1]} v{parts[3]} ({parts[4]}/{parts[5]})";
            }
            else
            {
                summary = rawId;
            }
            return true;
        }

        private static async Task<bool?> RunExternalRelayWithEphemeralListenerAsync(
            string externalProbeUrl,
            string externalIp,
            int externalPort,
            int internalPort,
            UpnpProtocol protocol,
            IPAddress localBindIp,
            HttpClient customHttpClient,
            CancellationToken cancellationToken)
        {
            using var listenerCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            TcpListener ephemeralTcp = null;
            UdpClient ephemeralUdp = null;
            Task acceptTask = Task.CompletedTask;

            try
            {
                if (protocol == UpnpProtocol.Tcp)
                {
                    try
                    {
                        ephemeralTcp = new TcpListener(localBindIp, internalPort);
                        ephemeralTcp.Start(4);
                    }
                    catch (SocketException)
                    {
                        ephemeralTcp = null;
                    }

                    if (ephemeralTcp != null)
                    {
                        acceptTask = Task.Run(async () =>
                        {
                            while (!listenerCts.IsCancellationRequested)
                            {
                                try
                                {
                                    TcpClient accepted = await ephemeralTcp.AcceptTcpClientAsync(listenerCts.Token).ConfigureAwait(false);
                                    _ = HandleExternalRelayTcpPeerAsync(accepted, listenerCts.Token);
                                }
                                catch
                                {
                                    break;
                                }
                            }
                        }, CancellationToken.None);
                    }
                }
                else
                {
                    try
                    {
                        ephemeralUdp = new UdpClient(new IPEndPoint(localBindIp, internalPort));
                    }
                    catch (SocketException)
                    {
                        ephemeralUdp = null;
                    }

                    if (ephemeralUdp != null)
                    {
                        acceptTask = Task.Run(async () =>
                        {
                            while (!listenerCts.IsCancellationRequested)
                            {
                                try
                                {
                                    UdpReceiveResult received = await ephemeralUdp.ReceiveAsync(listenerCts.Token).ConfigureAwait(false);
                                    string text = Encoding.UTF8.GetString(received.Buffer);
                                    if (text.StartsWith("BDSUTIL_PROBE:", StringComparison.Ordinal))
                                    {
                                        string nonce = text.Substring("BDSUTIL_PROBE:".Length);
                                        byte[] ackBytes = Encoding.UTF8.GetBytes("BDSUTIL_ACK:" + nonce);
                                        await ephemeralUdp.SendAsync(ackBytes, ackBytes.Length, received.RemoteEndPoint).ConfigureAwait(false);
                                    }
                                }
                                catch
                                {
                                    break;
                                }
                            }
                        }, CancellationToken.None);
                    }
                }

                string protoLabel = FormatProtocol(protocol);
                return await QueryExternalRelayAsync(
                    externalProbeUrl,
                    externalIp,
                    externalPort,
                    protoLabel,
                    customHttpClient,
                    cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                listenerCts.Cancel();
                try { ephemeralTcp?.Stop(); } catch { }
                try { ephemeralUdp?.Dispose(); } catch { }
                try { await Task.WhenAny(acceptTask, Task.Delay(50, CancellationToken.None)).ConfigureAwait(false); } catch { }
            }
        }

        private static async Task HandleExternalRelayTcpPeerAsync(TcpClient accepted, CancellationToken cancellationToken)
        {
            try
            {
                using (accepted)
                using (NetworkStream stream = accepted.GetStream())
                using (var readCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken))
                {
                    readCts.CancelAfter(250);
                    byte[] buffer = new byte[128];
                    int read = await stream.ReadAsync(buffer, 0, buffer.Length, readCts.Token).ConfigureAwait(false);
                    if (read > 0)
                    {
                        string text = Encoding.UTF8.GetString(buffer, 0, read).Trim();
                        if (text.StartsWith("BDSUTIL_PROBE:", StringComparison.Ordinal))
                        {
                            string nonce = text.Substring("BDSUTIL_PROBE:".Length);
                            byte[] ack = Encoding.UTF8.GetBytes("BDSUTIL_ACK:" + nonce + "\n");
                            await stream.WriteAsync(ack, 0, ack.Length, readCts.Token).ConfigureAwait(false);
                        }
                    }
                }
            }
            catch
            {
                // Standard port checkers (ifconfig.co / ipconfig.io / portchecker.io) disconnect immediately after TCP handshake.
            }
        }

        private static Task<bool?> QueryExternalRelayAsync(
            string externalProbeUrl,
            string externalIp,
            int externalPort,
            string protocol,
            HttpClient httpClient,
            CancellationToken cancellationToken)
        {
            return ExternalPortChecker.QueryAsync(
                externalProbeUrl,
                externalIp,
                externalPort,
                protocol,
                httpClient,
                cancellationToken);
        }

        public static Task<UpnpPortSetupResult> RequisitionAndVerifyPortWithRetryAsync(
            UpnpPortSpec portSpec,
            PortVerificationOptions options = null,
            IReadOnlyList<UpnpPortMapping> existingMappingsOverride = null,
            CancellationToken cancellationToken = default)
        {
            if (portSpec == null) throw new ArgumentNullException(nameof(portSpec));
            return RequisitionAndVerifySingleFromBatchAsync(portSpec, options, existingMappingsOverride, cancellationToken);
        }

        private static async Task<UpnpPortSetupResult> RequisitionAndVerifySingleFromBatchAsync(
            UpnpPortSpec portSpec,
            PortVerificationOptions options,
            IReadOnlyList<UpnpPortMapping> existingMappingsOverride,
            CancellationToken cancellationToken)
        {
            UpnpBatchSetupResult batch = await RequisitionAndVerifyPortsBatchAsync(
                new[] { portSpec },
                options,
                existingMappingsOverride,
                cancellationToken).ConfigureAwait(false);

            return batch.PortResults.Count > 0
                ? batch.PortResults[0]
                : new UpnpPortSetupResult
                {
                    ExternalPort = portSpec.ExternalPort,
                    InternalPort = portSpec.InternalPort,
                    Protocol = portSpec.Protocol,
                    Label = portSpec.Label ?? string.Empty,
                    IsGameUdpNode = portSpec.IsGameUdpNode,
                    AccessibilityState = batch.PrimaryFailureState,
                    DiagnosticMessage = batch.SummaryMessage
                };
        }

        public static async Task<UpnpBatchSetupResult> RequisitionAndVerifyPortsBatchAsync(
            IReadOnlyList<UpnpPortSpec> ports,
            PortVerificationOptions options = null,
            IReadOnlyList<UpnpPortMapping> existingMappingsOverride = null,
            CancellationToken cancellationToken = default)
        {
            ports ??= Array.Empty<UpnpPortSpec>();
            options ??= new PortVerificationOptions();

            var batch = new UpnpBatchSetupResult();
            if (ports.Count == 0)
            {
                batch.AllPortsVerified = true;
                batch.AllPortsInRouterTable = true;
                batch.SummaryMessage = "No UPnP ports were requested.";
                return batch;
            }

            Task<string> publicIpTask = !string.IsNullOrWhiteSpace(options.PublicExternalIpOverride)
                ? Task.FromResult(options.PublicExternalIpOverride.Trim())
                : GetExternalIPAddressAsync(cancellationToken);

            Task<UpnpGatewayClient> gatewayTask = options.Gateway != null
                ? Task.FromResult(options.Gateway)
                : (options.SkipGatewayDiscovery
                    ? Task.FromResult<UpnpGatewayClient>(null)
                    : DiscoverGatewayAsync(TimeSpan.FromMilliseconds(2500), cancellationToken: cancellationToken));

            await Task.WhenAll(publicIpTask, gatewayTask).ConfigureAwait(false);

            string publicIp = publicIpTask.Result;
            UpnpGatewayClient gateway = gatewayTask.Result;

            batch.GatewayFound = gateway != null;
            batch.PublicExternalIp = publicIp;

            if (gateway == null)
            {
                batch.AllPortsVerified = false;
                batch.AllPortsInRouterTable = false;
                batch.PrimaryFailureState = PortAccessibilityState.GatewayUnavailable;
                batch.SummaryMessage =
                    "UPnP gateway not found on the local network. Either UPnP is disabled on your router or your router is not responding to UPnP API requests.";

                foreach (UpnpPortSpec spec in ports)
                {
                    batch.PortResults.Add(new UpnpPortSetupResult
                    {
                        ExternalPort = spec.ExternalPort,
                        InternalPort = spec.InternalPort,
                        Protocol = spec.Protocol,
                        Label = spec.Label ?? string.Empty,
                        IsGameUdpNode = spec.IsGameUdpNode,
                        AccessibilityState = PortAccessibilityState.GatewayUnavailable,
                        DiagnosticMessage = batch.SummaryMessage
                    });
                }

                return batch;
            }

            string localIp = !string.IsNullOrWhiteSpace(options.LocalIpOverride)
                ? options.LocalIpOverride.Trim()
                : GetLocalIpForGateway(gateway);
            batch.LocalIp = localIp;

            var hostIps = new HashSet<string>(GetLocalIPAddresses(), StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrWhiteSpace(localIp))
            {
                hostIps.Add(localIp);
            }

            string routerWanIp = !string.IsNullOrWhiteSpace(options.RouterExternalIpOverride)
                ? options.RouterExternalIpOverride.Trim()
                : string.Empty;
            if (string.IsNullOrWhiteSpace(routerWanIp))
            {
                try
                {
                    routerWanIp = await gateway.GetExternalIPAddressAsync(cancellationToken).ConfigureAwait(false);
                }
                catch
                {
                    routerWanIp = string.Empty;
                }
            }
            batch.RouterExternalIp = routerWanIp;

            if (!IsValidPublicIPv4(publicIp) && IsValidPublicIPv4(routerWanIp))
            {
                publicIp = routerWanIp;
                batch.PublicExternalIp = publicIp;
            }

            var verifyOptions = new PortVerificationOptions
            {
                Gateway = gateway,
                ExternalProbeUrl = options.ExternalProbeUrl ?? string.Empty,
                TimeoutMs = options.TimeoutMs > 0 ? options.TimeoutMs : (ports.Count > 4 ? 1000 : 1500),
                PublicExternalIpOverride = publicIp,
                RouterExternalIpOverride = routerWanIp,
                LocalIpOverride = localIp,
                ExternalProbeTargetIpOverride = options.ExternalProbeTargetIpOverride,
                LocalEchoBindIpOverride = options.LocalEchoBindIpOverride,
                CustomHttpClient = options.CustomHttpClient
            };

            IReadOnlyList<UpnpPortMapping> initialMappings = existingMappingsOverride
                ?? (await TryQueryRouterLeaseTableAsync(gateway, cancellationToken).ConfigureAwait(false)).Mappings;
            var initialLookup = BuildMappingLookup(initialMappings);

            var pendingTcpIndices = new List<int>();
            var pendingUdpIndices = new List<int>();
            for (int i = 0; i < ports.Count; i++)
            {
                UpnpPortSpec spec = ports[i];
                string protoStr = FormatProtocol(spec.Protocol);
                var entry = new UpnpPortSetupResult
                {
                    ExternalPort = spec.ExternalPort,
                    InternalPort = spec.InternalPort,
                    Protocol = spec.Protocol,
                    Label = spec.Label ?? string.Empty,
                    IsGameUdpNode = spec.IsGameUdpNode
                };

                if (initialLookup.TryGetValue((spec.ExternalPort, spec.Protocol), out UpnpPortMapping existing) &&
                    existing != null &&
                    existing.Enabled)
                {
                    if (IsMappingOwnedByHost(existing, hostIps, spec.InternalPort))
                    {
                        RegisterSessionLease(spec.ExternalPort, protoStr);
                        entry.RequisitionSucceeded = true;
                        entry.ReusedExistingLease = true;
                    }
                    else if (!hostIps.Contains(existing.InternalClient))
                    {
                        entry.ConflictWithAnotherHost = true;
                        entry.ConflictHost = existing.InternalClient ?? string.Empty;
                        entry.AccessibilityState = PortAccessibilityState.ConflictWithAnotherHost;
                        entry.DiagnosticMessage =
                            $"Port {spec.ExternalPort}/{protoStr} is already mapped to another device on the LAN ({entry.ConflictHost}).";
                        batch.PortResults.Add(entry);
                        continue;
                    }
                    else
                    {
                        entry.RequisitionSucceeded = await ForwardPortAsync(
                            spec.ExternalPort,
                            spec.InternalPort,
                            localIp,
                            protoStr,
                            verifyAfterForward: true,
                            gatewayOverride: gateway,
                            cancellationToken: cancellationToken,
                            description: spec.Description).ConfigureAwait(false);
                    }
                }
                else
                {
                    entry.RequisitionSucceeded = await ForwardPortAsync(
                        spec.ExternalPort,
                        spec.InternalPort,
                        localIp,
                        protoStr,
                        verifyAfterForward: true,
                        gatewayOverride: gateway,
                        cancellationToken: cancellationToken,
                        description: spec.Description).ConfigureAwait(false);
                }

                batch.PortResults.Add(entry);
                if (spec.Protocol == UpnpProtocol.Tcp)
                {
                    pendingTcpIndices.Add(i);
                }
                else
                {
                    pendingUdpIndices.Add(i);
                }
            }

            // External reachability verification runs on TCP ports; UDP ports are verified directly via the router UPnP lease table.
            await VerifyBatchIndicesAsync(batch.PortResults, pendingTcpIndices, verifyOptions, cancellationToken).ConfigureAwait(false);

            var needTcpTableCheckIndices = new List<int>();
            foreach (int idx in pendingTcpIndices)
            {
                UpnpPortSetupResult entry = batch.PortResults[idx];
                if (entry.AccessibilityState == PortAccessibilityState.ConflictWithAnotherHost)
                {
                    entry.ConflictWithAnotherHost = true;
                    entry.ConflictHost = entry.Verification?.MappedInternalClient ?? entry.ConflictHost;
                    entry.VerifiedAccessible = false;
                    continue;
                }

                if (entry.AccessibilityState == PortAccessibilityState.VerifiedAccessible)
                {
                    entry.VerifiedAccessible = true;
                    entry.RequisitionSucceeded = true;
                    continue;
                }

                needTcpTableCheckIndices.Add(idx);
            }

            if (pendingUdpIndices.Count > 0 || needTcpTableCheckIndices.Count > 0)
            {
                // Ask the router to list its active UPnP leases to confirm all UDP ports and diagnose any failed TCP checks.
                var tableQuery1 = await TryQueryRouterLeaseTableAsync(gateway, cancellationToken).ConfigureAwait(false);

                var presentTcpInTableIndices = new List<int>();
                var missingFromTableIndices = new List<int>();

                foreach (int idx in pendingUdpIndices)
                {
                    UpnpPortSetupResult entry = batch.PortResults[idx];
                    entry.CheckedRouterLeaseTable = true;
                    bool inTable = tableQuery1.QuerySucceeded &&
                                   ContainsMatchingLease(tableQuery1.Mappings, entry.ExternalPort, entry.InternalPort, entry.Protocol, hostIps);
                    if (inTable)
                    {
                        MarkUdpLeaseVerified(entry, localIp);
                    }
                    else
                    {
                        entry.LeasePresentInRouterTable = false;
                        missingFromTableIndices.Add(idx);
                    }
                }

                foreach (int idx in needTcpTableCheckIndices)
                {
                    UpnpPortSetupResult entry = batch.PortResults[idx];
                    entry.CheckedRouterLeaseTable = true;
                    bool inTable = tableQuery1.QuerySucceeded &&
                                   ContainsMatchingLease(tableQuery1.Mappings, entry.ExternalPort, entry.InternalPort, entry.Protocol, hostIps);
                    entry.LeasePresentInRouterTable = inTable;
                    if (inTable)
                    {
                        presentTcpInTableIndices.Add(idx);
                    }
                    else
                    {
                        missingFromTableIndices.Add(idx);
                    }
                }

                // Branch A: TCP lease IS present in router table -> re-verify reachability; if still unreachable,
                // report Double-NAT / CGNAT, external blocker, or router disabling NAT loopback.
                if (presentTcpInTableIndices.Count > 0)
                {
                    foreach (int idx in presentTcpInTableIndices)
                    {
                        batch.PortResults[idx].RetriedVerification = true;
                    }

                    await VerifyBatchIndicesAsync(batch.PortResults, presentTcpInTableIndices, verifyOptions, cancellationToken).ConfigureAwait(false);

                    foreach (int idx in presentTcpInTableIndices)
                    {
                        UpnpPortSetupResult entry = batch.PortResults[idx];
                        ApplySecondAttemptVerificationOutcome(entry, inRouterTable: true, afterSecondRequisition: false);
                    }
                }

                // Branch B: Lease is NOT present in router table -> requisition a second time, re-verify TCP / re-check table.
                if (missingFromTableIndices.Count > 0)
                {
                    var retryTcpIndices = new List<int>();
                    foreach (int idx in missingFromTableIndices)
                    {
                        UpnpPortSpec spec = ports[idx];
                        UpnpPortSetupResult entry = batch.PortResults[idx];
                        entry.RetriedRequisition = true;
                        entry.RetriedVerification = spec.Protocol == UpnpProtocol.Tcp;

                        bool secondOk = await ForwardPortAsync(
                            spec.ExternalPort,
                            spec.InternalPort,
                            localIp,
                            FormatProtocol(spec.Protocol),
                            verifyAfterForward: true,
                            gatewayOverride: gateway,
                            cancellationToken: cancellationToken,
                            description: spec.Description).ConfigureAwait(false);

                        entry.RequisitionSucceeded |= secondOk;
                        if (spec.Protocol == UpnpProtocol.Tcp)
                        {
                            retryTcpIndices.Add(idx);
                        }
                    }

                    if (retryTcpIndices.Count > 0)
                    {
                        await VerifyBatchIndicesAsync(batch.PortResults, retryTcpIndices, verifyOptions, cancellationToken).ConfigureAwait(false);
                    }

                    var needSecondTableCheck = new List<int>();
                    foreach (int idx in missingFromTableIndices)
                    {
                        UpnpPortSetupResult entry = batch.PortResults[idx];
                        if (entry.Protocol == UpnpProtocol.Tcp)
                        {
                            if (entry.AccessibilityState == PortAccessibilityState.VerifiedAccessible)
                            {
                                entry.VerifiedAccessible = true;
                                entry.RequisitionSucceeded = true;
                                entry.LeasePresentInRouterTable = true;
                                if (entry.Verification != null)
                                {
                                    entry.Verification.CheckedRouterLeaseTable = true;
                                    entry.Verification.LeasePresentInRouterTable = true;
                                    entry.Verification.RetriedRequisition = true;
                                    entry.Verification.RetriedVerification = true;
                                }
                            }
                            else if (entry.AccessibilityState == PortAccessibilityState.ConflictWithAnotherHost)
                            {
                                entry.ConflictWithAnotherHost = true;
                                entry.ConflictHost = entry.Verification?.MappedInternalClient ?? entry.ConflictHost;
                                entry.VerifiedAccessible = false;
                            }
                            else
                            {
                                needSecondTableCheck.Add(idx);
                            }
                        }
                        else
                        {
                            needSecondTableCheck.Add(idx);
                        }
                    }

                    if (needSecondTableCheck.Count > 0)
                    {
                        var tableQuery2 = await TryQueryRouterLeaseTableAsync(gateway, cancellationToken).ConfigureAwait(false);
                        foreach (int idx in needSecondTableCheck)
                        {
                            UpnpPortSetupResult entry = batch.PortResults[idx];
                            bool inTable2 = tableQuery2.QuerySucceeded &&
                                            ContainsMatchingLease(tableQuery2.Mappings, entry.ExternalPort, entry.InternalPort, entry.Protocol, hostIps);
                            if (entry.Protocol == UpnpProtocol.Udp && inTable2)
                            {
                                MarkUdpLeaseVerified(entry, localIp);
                            }
                            else
                            {
                                entry.LeasePresentInRouterTable = inTable2;
                                ApplySecondAttemptVerificationOutcome(entry, inRouterTable: inTable2, afterSecondRequisition: true);
                            }
                        }
                    }
                }
            }

            FinalizeBatchSummary(batch);
            return batch;
        }

        private static void MarkUdpLeaseVerified(UpnpPortSetupResult entry, string localIp)
        {
            entry.RequisitionSucceeded = true;
            entry.LeasePresentInRouterTable = true;
            entry.VerifiedAccessible = true;
            entry.AccessibilityState = PortAccessibilityState.VerifiedAccessible;
            entry.DiagnosticMessage =
                $"Port {entry.ExternalPort}/UDP -> {localIp}:{entry.InternalPort} is mapped in the router's active UPnP lease table.";
        }

        private static async Task VerifyBatchIndicesAsync(
            IList<UpnpPortSetupResult> results,
            IReadOnlyList<int> indices,
            PortVerificationOptions verifyOptions,
            CancellationToken cancellationToken)
        {
            if (indices == null || indices.Count == 0) return;

            foreach (int index in indices)
            {
                cancellationToken.ThrowIfCancellationRequested();

                UpnpPortSetupResult entry = results[index];
                PortForwardVerificationResult verification = await VerifyPortAsync(
                    entry.ExternalPort,
                    entry.InternalPort,
                    FormatProtocol(entry.Protocol),
                    verifyOptions,
                    cancellationToken).ConfigureAwait(false);

                entry.Verification = verification;
                entry.AccessibilityState = verification?.AccessibilityState ?? PortAccessibilityState.Unknown;
                entry.DiagnosticMessage = verification?.DiagnosticMessage ?? string.Empty;
            }
        }

        private static void ApplySecondAttemptVerificationOutcome(
            UpnpPortSetupResult entry,
            bool inRouterTable,
            bool afterSecondRequisition)
        {
            if (entry.AccessibilityState == PortAccessibilityState.VerifiedAccessible)
            {
                entry.VerifiedAccessible = true;
                entry.RequisitionSucceeded = true;
                entry.LeasePresentInRouterTable = true;
                if (entry.Verification != null)
                {
                    entry.Verification.CheckedRouterLeaseTable = true;
                    entry.Verification.LeasePresentInRouterTable = true;
                    entry.Verification.RetriedRequisition = entry.RetriedRequisition;
                    entry.Verification.RetriedVerification = entry.RetriedVerification;
                }
                return;
            }

            if (entry.AccessibilityState == PortAccessibilityState.ConflictWithAnotherHost)
            {
                entry.ConflictWithAnotherHost = true;
                entry.ConflictHost = entry.Verification?.MappedInternalClient ?? entry.ConflictHost;
                entry.VerifiedAccessible = false;
                return;
            }

            entry.VerifiedAccessible = false;
            string protoStr = FormatProtocol(entry.Protocol);

            if (inRouterTable)
            {
                entry.RequisitionSucceeded = true;
                bool externalRelayRejected = entry.Verification != null &&
                                             entry.Verification.ExternalRelayChecked &&
                                             !entry.Verification.ExternalProbeSucceeded;

                entry.AccessibilityState = entry.Verification != null && entry.Verification.IsBehindDoubleNatOrCgnat
                    ? PortAccessibilityState.BlockedByDoubleNatOrCgnat
                    : (externalRelayRejected
                        ? PortAccessibilityState.LocalListenerUnavailable
                        : PortAccessibilityState.ForwardedButLoopbackBlocked);

                string stageText = afterSecondRequisition
                    ? "appeared in the router's UPnP table after the second requisition attempt"
                    : "is active in the router's UPnP lease table";
                entry.DiagnosticMessage = externalRelayRejected
                    ? $"Port {entry.ExternalPort}/{protoStr} {stageText}, but the external port checker confirmed it is unreachable from the internet (check Windows Firewall, ISP blocks, or Double-NAT / CGNAT)."
                    : $"Port {entry.ExternalPort}/{protoStr} {stageText}, but external reachability verification failed. You are likely under Double-NAT / CGNAT or using a router that disables NAT loopback.";
            }
            else
            {
                entry.RequisitionSucceeded = false;
                entry.AccessibilityState = PortAccessibilityState.UpnpDisabledOrUnresponsive;
                entry.DiagnosticMessage =
                    $"Port {entry.ExternalPort}/{protoStr} did not appear in the router's active UPnP lease table after retry. Either UPnP is disabled on your router or your router is not responding to UPnP API requests.";
            }

            if (entry.Verification != null)
            {
                entry.Verification.CheckedRouterLeaseTable = entry.CheckedRouterLeaseTable;
                entry.Verification.LeasePresentInRouterTable = inRouterTable;
                entry.Verification.RetriedRequisition = entry.RetriedRequisition;
                entry.Verification.RetriedVerification = entry.RetriedVerification;
                entry.Verification.AccessibilityState = entry.AccessibilityState;
                entry.Verification.DiagnosticMessage = entry.DiagnosticMessage;
                if (!inRouterTable)
                {
                    entry.Verification.IsForwardedToHost = false;
                    entry.Verification.IsExternallyAccessible = false;
                }
            }
        }

        private static void FinalizeBatchSummary(UpnpBatchSetupResult batch)
        {
            bool anyUnverified = false;
            bool allInTable = true;
            bool anyUnresponsive = false;
            bool anyConflict = false;
            bool anyCgnatOrLoopback = false;
            bool anyExternalRelayRejected = false;
            bool confirmedCgnat = false;
            UpnpPortSetupResult firstFailure = null;
            int verifiedCount = 0;
            int inTableCount = 0;

            var unresponsivePorts = new List<string>();
            var cgnatOrLoopbackPorts = new List<string>();
            var externalRejectedPorts = new List<string>();

            bool validPublicIp = IsValidPublicIPv4(batch.PublicExternalIp);

            foreach (UpnpPortSetupResult entry in batch.PortResults)
            {
                string portTag = $"{entry.ExternalPort}/{FormatProtocol(entry.Protocol)}";

                if (entry.VerifiedAccessible)
                {
                    verifiedCount++;
                }
                else
                {
                    anyUnverified = true;
                    firstFailure ??= entry;
                }

                bool presentOrVerified = entry.VerifiedAccessible || entry.LeasePresentInRouterTable || entry.RequisitionSucceeded;
                if (presentOrVerified)
                {
                    inTableCount++;
                }
                else
                {
                    allInTable = false;
                }

                if (entry.IsGameUdpNode && presentOrVerified && validPublicIp)
                {
                    batch.SuccessfulUdpNodeTokens.Add($"{batch.PublicExternalIp}:{entry.ExternalPort}:{entry.InternalPort}");
                }

                if (entry.AccessibilityState == PortAccessibilityState.UpnpDisabledOrUnresponsive)
                {
                    anyUnresponsive = true;
                    unresponsivePorts.Add(portTag);
                }
                else if (entry.AccessibilityState == PortAccessibilityState.ConflictWithAnotherHost)
                {
                    anyConflict = true;
                }
                else if (entry.AccessibilityState == PortAccessibilityState.LocalListenerUnavailable &&
                         entry.Verification?.ExternalRelayChecked == true)
                {
                    anyExternalRelayRejected = true;
                    externalRejectedPorts.Add(portTag);
                }
                else if (entry.AccessibilityState == PortAccessibilityState.BlockedByDoubleNatOrCgnat ||
                         entry.AccessibilityState == PortAccessibilityState.ForwardedButLoopbackBlocked)
                {
                    anyCgnatOrLoopback = true;
                    cgnatOrLoopbackPorts.Add(portTag);
                    if (entry.AccessibilityState == PortAccessibilityState.BlockedByDoubleNatOrCgnat)
                    {
                        confirmedCgnat = true;
                    }
                }
            }

            batch.AllPortsVerified = !anyUnverified;
            batch.AllPortsInRouterTable = allInTable;

            if (!anyUnverified)
            {
                batch.PrimaryFailureState = PortAccessibilityState.VerifiedAccessible;
                batch.SummaryMessage = "Ports successfully mapped and service is accessible externally";
                return;
            }

            if (anyUnresponsive)
            {
                batch.PrimaryFailureState = PortAccessibilityState.UpnpDisabledOrUnresponsive;
                batch.SummaryMessage =
                    $"UPnP verification failed ({verifiedCount}/{batch.PortResults.Count} verified, {inTableCount}/{batch.PortResults.Count} active on router; missing: {string.Join(", ", unresponsivePorts)}): leases could not be confirmed in your router's active UPnP table after retrying. Either UPnP is disabled on your router or your router is not responding to UPnP API requests.";
            }
            else if (anyExternalRelayRejected)
            {
                batch.PrimaryFailureState = PortAccessibilityState.LocalListenerUnavailable;
                batch.SummaryMessage =
                    $"External reachability verification failed ({verifiedCount}/{batch.PortResults.Count} verified, {inTableCount}/{batch.PortResults.Count} active in router UPnP lease table; external checker blocked/unreachable on: {string.Join(", ", externalRejectedPorts)}). Leases are active on the router, so check Windows Firewall, ISP blocks, or Double-NAT / CGNAT.";
            }
            else if (anyCgnatOrLoopback)
            {
                batch.PrimaryFailureState = confirmedCgnat
                    ? PortAccessibilityState.BlockedByDoubleNatOrCgnat
                    : PortAccessibilityState.ForwardedButLoopbackBlocked;

                batch.SummaryMessage =
                    $"External reachability verification failed ({verifiedCount}/{batch.PortResults.Count} verified, {inTableCount}/{batch.PortResults.Count} active in router UPnP lease table; unreachable: {string.Join(", ", cgnatOrLoopbackPorts)}). Your leases are registered on the router, so this is caused either by Double-NAT / CGNAT or by a router that disables NAT loopback.";
            }
            else if (anyConflict)
            {
                batch.PrimaryFailureState = PortAccessibilityState.ConflictWithAnotherHost;
                batch.SummaryMessage =
                    $"UPnP setup failed: {firstFailure?.DiagnosticMessage ?? "One or more requested ports are already mapped to another device on your LAN."}";
            }
            else
            {
                batch.PrimaryFailureState = firstFailure?.AccessibilityState ?? PortAccessibilityState.Unknown;
                batch.SummaryMessage =
                    $"UPnP verification failed: {firstFailure?.DiagnosticMessage ?? "External reachability could not be confirmed."}";
            }
        }

        private static async Task<(bool QuerySucceeded, IReadOnlyList<UpnpPortMapping> Mappings)> TryQueryRouterLeaseTableAsync(
            UpnpGatewayClient gateway,
            CancellationToken cancellationToken)
        {
            if (gateway == null) return (false, Array.Empty<UpnpPortMapping>());

            try
            {
                IReadOnlyList<UpnpPortMapping> mappings = await gateway.GetAllPortMappingsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
                return (true, mappings ?? Array.Empty<UpnpPortMapping>());
            }
            catch
            {
                return (false, Array.Empty<UpnpPortMapping>());
            }
        }

        public static List<UpnpPortSpec> PlanGameServerPorts(
            int primaryPortV4,
            int maxPlayersCount,
            bool consolidateUdpPorts,
            int excludedApiPort = 0,
            IReadOnlyList<UpnpPortMapping> existingMappings = null,
            ISet<string> hostIps = null)
        {
            var specs = new List<UpnpPortSpec>();
            if (primaryPortV4 <= 0 || primaryPortV4 > 65535)
                return specs;

            specs.Add(new UpnpPortSpec
            {
                ExternalPort = primaryPortV4,
                InternalPort = primaryPortV4,
                Protocol = UpnpProtocol.Tcp,
                Label = "Primary Server Port",
                Description = $"BDSUtil Primary TCP {primaryPortV4}",
                IsGameUdpNode = false
            });
            specs.Add(new UpnpPortSpec
            {
                ExternalPort = primaryPortV4,
                InternalPort = primaryPortV4,
                Protocol = UpnpProtocol.Udp,
                Label = "Primary Server Port",
                Description = $"BDSUtil Primary UDP {primaryPortV4}",
                IsGameUdpNode = false
            });

            int targetUdpNodeCount = BDSUtil.Helpers.RemoteApi.ServerPortDiscovery.GetTargetUdpNodeCount(maxPlayersCount, consolidateUdpPorts);
            int candidate = BDSUtil.Helpers.RemoteApi.ServerPortDiscovery.GetAdditionalUdpStartPort(primaryPortV4);
            var lookup = BuildMappingLookup(existingMappings);

            int addedNodes = 0;
            int scanAttempts = 0;
            const int maxScanAttempts = 64;

            while (addedNodes < targetUdpNodeCount && scanAttempts < maxScanAttempts && candidate <= 65535)
            {
                if (candidate == primaryPortV4 || (excludedApiPort > 0 && candidate == excludedApiPort))
                {
                    candidate++;
                    continue;
                }

                scanAttempts++;

                if (lookup.TryGetValue((candidate, UpnpProtocol.Udp), out UpnpPortMapping existing) &&
                    existing != null &&
                    existing.Enabled &&
                    hostIps != null &&
                    hostIps.Count > 0 &&
                    !hostIps.Contains(existing.InternalClient))
                {
                    candidate++;
                    continue;
                }

                addedNodes++;
                specs.Add(new UpnpPortSpec
                {
                    ExternalPort = candidate,
                    InternalPort = candidate,
                    Protocol = UpnpProtocol.Udp,
                    Label = $"Server UDP Node {addedNodes}",
                    Description = $"BDSUtil Node {candidate}",
                    IsGameUdpNode = true
                });
                candidate++;
            }

            return specs;
        }

        private static Dictionary<(int ExternalPort, UpnpProtocol Protocol), UpnpPortMapping> BuildMappingLookup(
            IReadOnlyList<UpnpPortMapping> mappings)
        {
            var dict = new Dictionary<(int, UpnpProtocol), UpnpPortMapping>();
            if (mappings == null) return dict;

            foreach (UpnpPortMapping mapping in mappings)
            {
                if (mapping == null) continue;
                dict[(mapping.ExternalPort, mapping.Protocol)] = mapping;
            }
            return dict;
        }

        internal static bool ContainsMatchingLease(
            IReadOnlyList<UpnpPortMapping> mappings,
            int externalPort,
            int internalPort,
            UpnpProtocol protocol,
            ISet<string> hostIps)
        {
            if (mappings == null || mappings.Count == 0) return false;

            foreach (UpnpPortMapping m in mappings)
            {
                if (m == null || !m.Enabled) continue;
                if (m.ExternalPort != externalPort || m.Protocol != protocol) continue;
                if (internalPort > 0 && m.InternalPort != internalPort) continue;
                if (hostIps != null && hostIps.Count > 0 && !hostIps.Contains(m.InternalClient)) continue;
                return true;
            }

            return false;
        }

        private static bool IsMappingOwnedByHost(UpnpPortMapping mapping, ISet<string> hostIps, int expectedInternalPort)
        {
            if (mapping == null || !mapping.Enabled) return false;
            if (expectedInternalPort > 0 && mapping.InternalPort != expectedInternalPort) return false;
            return hostIps != null && hostIps.Contains(mapping.InternalClient);
        }

        private static string FormatProtocol(UpnpProtocol protocol) =>
            protocol == UpnpProtocol.Tcp ? "TCP" : "UDP";

        private static UpnpProtocol ParseProtocol(string protocolType)
        {
            return !string.IsNullOrWhiteSpace(protocolType) &&
                   protocolType.Trim().Equals("TCP", StringComparison.OrdinalIgnoreCase)
                ? UpnpProtocol.Tcp
                : UpnpProtocol.Udp;
        }
    }
}
