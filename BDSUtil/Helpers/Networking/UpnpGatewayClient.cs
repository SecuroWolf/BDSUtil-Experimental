using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BDSUtil.Helpers
{
    /// <summary>
    /// Native .NET 8 UPnP Internet Gateway Device (IGD v1/v2) client implementing SSDP discovery,
    /// device description parsing, and SOAP WANIPConnection / WANPPPConnection control actions
    /// without external NAT libraries.
    /// </summary>
    public sealed class UpnpGatewayClient
    {
        private static readonly TimeSpan DefaultSoapTimeout = TimeSpan.FromSeconds(4);

        private static readonly string[] SsdpSearchTargets =
        {
            "urn:schemas-upnp-org:device:InternetGatewayDevice:1",
            "urn:schemas-upnp-org:device:InternetGatewayDevice:2",
            "urn:schemas-upnp-org:service:WANIPConnection:2",
            "urn:schemas-upnp-org:service:WANIPConnection:1",
            "urn:schemas-upnp-org:service:WANPPPConnection:1"
        };

        private sealed class WanServiceCandidate
        {
            public string ServiceType { get; set; } = string.Empty;
            public Uri ControlUri { get; set; }
            public int Priority { get; set; }
        }

        private readonly HttpClient _httpClient;
        private readonly List<WanServiceCandidate> _wanServices;

        internal static readonly HttpClient SharedLanHttpClient = CreateLanHttpClient();

        public Uri LocationUri { get; }
        public Uri ControlUri { get; private set; }
        public string ServiceType { get; private set; }
        public string FriendlyName { get; private set; }

        private UpnpGatewayClient(
            Uri locationUri,
            Uri controlUri,
            string serviceType,
            string friendlyName,
            HttpClient httpClient,
            List<WanServiceCandidate> wanServices = null)
        {
            LocationUri = locationUri;
            ControlUri = controlUri;
            ServiceType = serviceType;
            FriendlyName = friendlyName ?? string.Empty;
            _httpClient = httpClient;
            _wanServices = wanServices ?? new List<WanServiceCandidate>
            {
                new WanServiceCandidate
                {
                    ControlUri = controlUri,
                    ServiceType = serviceType,
                    Priority = 1
                }
            };
        }

        internal static HttpClient CreateLanHttpClient(HttpMessageHandler customHandler = null)
        {
            var handler = customHandler ?? new SocketsHttpHandler
            {
                UseProxy = false,
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                ConnectTimeout = TimeSpan.FromSeconds(3)
            };

            var client = new HttpClient(handler, disposeHandler: customHandler == null)
            {
                Timeout = DefaultSoapTimeout
            };
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.DefaultRequestHeaders.ConnectionClose = true;
            return client;
        }

        /// <summary>
        /// Broadcasts SSDP M-SEARCH probes across active local IPv4 interfaces to locate a UPnP IGD router,
        /// preferring the active OS default gateway and public-WAN gateway when multiple UPnP devices exist on the LAN.
        /// </summary>
        public static async Task<UpnpGatewayClient> DiscoverAsync(
            TimeSpan timeout,
            IPAddress multicastAddress = null,
            int multicastPort = 1900,
            IReadOnlyList<IPAddress> localInterfacesOverride = null,
            HttpClient customHttpClient = null,
            CancellationToken cancellationToken = default)
        {
            var gateways = await DiscoverAllAsync(
                timeout,
                multicastAddress,
                multicastPort,
                localInterfacesOverride,
                customHttpClient,
                cancellationToken).ConfigureAwait(false);

            return gateways.Count > 0 ? gateways[0] : null;
        }

        internal static async Task<IReadOnlyList<UpnpGatewayClient>> DiscoverAllAsync(
            TimeSpan timeout,
            IPAddress multicastAddress = null,
            int multicastPort = 1900,
            IReadOnlyList<IPAddress> localInterfacesOverride = null,
            HttpClient customHttpClient = null,
            CancellationToken cancellationToken = default)
        {
            var effectiveMulticast = multicastAddress ?? IPAddress.Parse("239.255.255.250");
            var candidates = localInterfacesOverride ?? GetCandidateMulticastInterfaces();
            if (candidates.Count == 0)
            {
                candidates = new[] { IPAddress.Any };
            }

            HashSet<string> defaultGatewayIps = localInterfacesOverride == null
                ? GetDefaultGatewayIPv4Addresses()
                : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var locations = await ProbeSsdpLocationsAsync(
                candidates,
                effectiveMulticast,
                multicastPort,
                timeout,
                defaultGatewayIps,
                cancellationToken).ConfigureAwait(false);

            if (locations.Count == 0)
                return Array.Empty<UpnpGatewayClient>();

            HttpClient httpClient = customHttpClient ?? SharedLanHttpClient;
            var orderedLocations = locations
                .OrderByDescending(u => defaultGatewayIps.Contains(u.Host))
                .ToList();

            var discovered = new List<(UpnpGatewayClient Gateway, bool IsDefaultGateway, bool HasPublicWanIp)>();
            foreach (Uri locationUri in orderedLocations)
            {
                try
                {
                    var gateway = await FromDescriptionUriAsync(locationUri, httpClient, cancellationToken).ConfigureAwait(false);
                    if (gateway == null) continue;

                    bool isDefaultGw = defaultGatewayIps.Contains(locationUri.Host);

                    // Fast path: when there is only one UPnP device on the LAN, no WAN-IP tie-breaker is needed.
                    if (orderedLocations.Count == 1)
                    {
                        return new[] { gateway };
                    }

                    bool hasPublicWanIp = false;
                    try
                    {
                        string wanIp = await gateway.GetExternalIPAddressAsync(cancellationToken).ConfigureAwait(false);
                        hasPublicWanIp = UPnPEngine.IsValidPublicIPv4(wanIp);
                    }
                    catch { }

                    discovered.Add((gateway, isDefaultGw, hasPublicWanIp));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[UPnP Discovery Description Error ({locationUri})]: {ex.Message}");
                }
            }

            return discovered
                .OrderByDescending(d => d.IsDefaultGateway && d.HasPublicWanIp)
                .ThenByDescending(d => d.IsDefaultGateway)
                .ThenByDescending(d => d.HasPublicWanIp)
                .Select(d => d.Gateway)
                .ToList();
        }

        /// <summary>
        /// Fetches and parses a UPnP IGD device description XML directly from a known LOCATION URI.
        /// </summary>
        public static async Task<UpnpGatewayClient> FromDescriptionUriAsync(
            Uri locationUri,
            HttpClient customHttpClient = null,
            CancellationToken cancellationToken = default)
        {
            if (locationUri == null) throw new ArgumentNullException(nameof(locationUri));

            HttpClient httpClient = customHttpClient ?? SharedLanHttpClient;
            string xmlContent = SanitizeXmlPayload(await httpClient.GetStringAsync(locationUri, cancellationToken).ConfigureAwait(false));
            if (string.IsNullOrWhiteSpace(xmlContent))
                return null;

            XDocument doc = XDocument.Parse(xmlContent);
            if (doc.Root == null)
                return null;

            Uri baseUri = ResolveBaseUri(doc.Root, locationUri);
            string friendlyName = GetFirstDescendantValue(doc.Root, "friendlyName");

            var services = ExtractWanServices(doc.Root, baseUri);
            if (services.Count == 0)
                return null;

            WanServiceCandidate selected = await SelectActiveServiceAsync(services, httpClient, cancellationToken).ConfigureAwait(false);
            var orderedServices = new List<WanServiceCandidate>(services.Count) { selected };
            foreach (WanServiceCandidate candidate in services)
            {
                if (!ReferenceEquals(candidate, selected))
                {
                    orderedServices.Add(candidate);
                }
            }

            return new UpnpGatewayClient(locationUri, selected.ControlUri, selected.ServiceType, friendlyName, httpClient, orderedServices);
        }

        public async Task<string> GetExternalIPAddressAsync(CancellationToken cancellationToken = default)
        {
            XElement response = await InvokeSoapActionAsync("GetExternalIPAddress", null, cancellationToken).ConfigureAwait(false);
            return GetFirstDescendantValue(response, "NewExternalIPAddress").Trim();
        }

        public async Task<string> GetConnectionStatusAsync(CancellationToken cancellationToken = default)
        {
            XElement response = await InvokeSoapActionAsync("GetStatusInfo", null, cancellationToken).ConfigureAwait(false);
            return GetFirstDescendantValue(response, "NewConnectionStatus").Trim();
        }

        public async Task<UpnpPortMapping> GetSpecificPortMappingEntryAsync(
            int externalPort,
            UpnpProtocol protocol,
            string remoteHost = "",
            CancellationToken cancellationToken = default)
        {
            try
            {
                XElement response = await InvokeSoapActionAsync(
                    "GetSpecificPortMappingEntry",
                    new[]
                    {
                        new XElement("NewRemoteHost", remoteHost ?? string.Empty),
                        new XElement("NewExternalPort", externalPort),
                        new XElement("NewProtocol", FormatProtocol(protocol))
                    },
                    cancellationToken).ConfigureAwait(false);

                string internalClient = GetFirstDescendantValue(response, "NewInternalClient").Trim();
                int internalPort = ParseIntOrDefault(GetFirstDescendantValue(response, "NewInternalPort"), externalPort);
                bool enabled = ParseBoolOrDefault(GetFirstDescendantValue(response, "NewEnabled"), defaultValue: true);
                string description = GetFirstDescendantValue(response, "NewPortMappingDescription").Trim();
                int leaseDuration = ParseIntOrDefault(GetFirstDescendantValue(response, "NewLeaseDuration"), 0);

                return new UpnpPortMapping
                {
                    RemoteHost = remoteHost ?? string.Empty,
                    ExternalPort = externalPort,
                    InternalPort = internalPort,
                    Protocol = protocol,
                    InternalClient = internalClient,
                    Enabled = enabled,
                    Description = description,
                    LeaseDuration = leaseDuration
                };
            }
            catch (UpnpSoapException ex) when (IsMappingNotFoundOrUnsupported(ex))
            {
                // 714 = NoSuchEntryInArray (and equivalent router-specific missing/index errors).
                return null;
            }
        }

        public async Task<IReadOnlyList<UpnpPortMapping>> GetAllPortMappingsAsync(
            int maxEntries = 512,
            CancellationToken cancellationToken = default)
        {
            var results = new List<UpnpPortMapping>();

            for (int index = 0; index < maxEntries; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    XElement response = await InvokeSoapActionAsync(
                        "GetGenericPortMappingEntry",
                        new[] { new XElement("NewPortMappingIndex", index) },
                        cancellationToken).ConfigureAwait(false);

                    int extPort = ParseIntOrDefault(GetFirstDescendantValue(response, "NewExternalPort"), 0);
                    if (extPort <= 0)
                        break;

                    string protoStr = GetFirstDescendantValue(response, "NewProtocol").Trim();
                    UpnpProtocol proto = protoStr.Equals("TCP", StringComparison.OrdinalIgnoreCase)
                        ? UpnpProtocol.Tcp
                        : UpnpProtocol.Udp;

                    results.Add(new UpnpPortMapping
                    {
                        RemoteHost = GetFirstDescendantValue(response, "NewRemoteHost").Trim(),
                        ExternalPort = extPort,
                        InternalPort = ParseIntOrDefault(GetFirstDescendantValue(response, "NewInternalPort"), extPort),
                        Protocol = proto,
                        InternalClient = GetFirstDescendantValue(response, "NewInternalClient").Trim(),
                        Enabled = ParseBoolOrDefault(GetFirstDescendantValue(response, "NewEnabled"), defaultValue: true),
                        Description = GetFirstDescendantValue(response, "NewPortMappingDescription").Trim(),
                        LeaseDuration = ParseIntOrDefault(GetFirstDescendantValue(response, "NewLeaseDuration"), 0)
                    });
                }
                catch (UpnpSoapException ex) when (IsGenericEnumerationEndOrUnsupported(ex))
                {
                    // Some routers use 1-based NewPortMappingIndex and reject index 0 with 713/402/501.
                    if (index == 0)
                        continue;

                    // End of gateway mapping table or router does not support enumeration.
                    break;
                }
                catch (HttpRequestException)
                {
                    // Some consumer gateways reply with raw HTTP 500/501/404 at table end or when unsupported.
                    break;
                }
            }

            return results;
        }

        public async Task AddPortMappingAsync(
            int externalPort,
            int internalPort,
            string internalClient,
            UpnpProtocol protocol,
            string description,
            int leaseDurationSeconds = 0,
            CancellationToken cancellationToken = default)
        {
            Exception primaryFailure = null;

            try
            {
                await AddPortMappingOnCurrentServiceAsync(
                    externalPort,
                    internalPort,
                    internalClient,
                    protocol,
                    description,
                    leaseDurationSeconds,
                    cancellationToken).ConfigureAwait(false);
                return;
            }
            catch (UpnpSoapException ex) when (!IsPortConflictError(ex) && _wanServices.Count > 1 && ShouldRetryAlternateService(ex))
            {
                primaryFailure = ex;
            }
            catch (HttpRequestException ex) when (_wanServices.Count > 1)
            {
                primaryFailure = ex;
            }

            // On dual-service gateways (e.g., IGDv2 + IGDv1 or WANIP + WANPPP), try alternate WAN services.
            Uri originalControlUri = ControlUri;
            string originalServiceType = ServiceType;

            foreach (WanServiceCandidate candidate in _wanServices)
            {
                if (candidate.ControlUri == originalControlUri &&
                    candidate.ServiceType.Equals(originalServiceType, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                try
                {
                    ControlUri = candidate.ControlUri;
                    ServiceType = candidate.ServiceType;

                    await AddPortMappingOnCurrentServiceAsync(
                        externalPort,
                        internalPort,
                        internalClient,
                        protocol,
                        description,
                        leaseDurationSeconds,
                        cancellationToken).ConfigureAwait(false);
                    return;
                }
                catch
                {
                    // Keep trying remaining candidates before restoring original and propagating primary failure.
                }
            }

            ControlUri = originalControlUri;
            ServiceType = originalServiceType;
            throw primaryFailure;
        }

        private async Task AddPortMappingOnCurrentServiceAsync(
            int externalPort,
            int internalPort,
            string internalClient,
            UpnpProtocol protocol,
            string description,
            int leaseDurationSeconds,
            CancellationToken cancellationToken,
            bool allowConflictResolution = true)
        {
            UpnpSoapException terminalSoapException = null;

            try
            {
                await SendAddPortMappingSoapAsync(
                    externalPort,
                    internalPort,
                    internalClient,
                    protocol,
                    description,
                    leaseDurationSeconds,
                    cancellationToken).ConfigureAwait(false);
                return;
            }
            catch (UpnpSoapException ex) when (ex.ErrorCode == 725 && leaseDurationSeconds > 0)
            {
                // 725 = OnlyPermanentLeasesSupported: retry with permanent lease (0).
                await SendAddPortMappingSoapAsync(
                    externalPort,
                    internalPort,
                    internalClient,
                    protocol,
                    description,
                    0,
                    cancellationToken).ConfigureAwait(false);
                return;
            }
            catch (UpnpSoapException ex) when (leaseDurationSeconds == 0 && !IsPortConflictError(ex) && IsLeaseDurationRejected(ex))
            {
                terminalSoapException = ex;

                // Some IGDv2/ISP routers forbid permanent lease (0) and require a bounded TTL (24h or 1h).
                foreach (int candidateLease in new[] { 86400, 3600 })
                {
                    try
                    {
                        await SendAddPortMappingSoapAsync(
                            externalPort,
                            internalPort,
                            internalClient,
                            protocol,
                            description,
                            candidateLease,
                            cancellationToken).ConfigureAwait(false);
                        return;
                    }
                    catch (UpnpSoapException retryEx)
                    {
                        terminalSoapException = retryEx;
                        if (IsPortConflictError(retryEx) || (!IsLeaseDurationRejected(retryEx) && retryEx.ErrorCode != 402))
                            break;
                    }
                }
            }
            catch (UpnpSoapException ex)
            {
                terminalSoapException = ex;
            }

            if (terminalSoapException != null &&
                allowConflictResolution &&
                (IsPortConflictError(terminalSoapException) || terminalSoapException.ErrorCode == 501))
            {
                bool handled = await TryResolveExistingMappingConflictAsync(
                    externalPort,
                    internalPort,
                    internalClient,
                    protocol,
                    description,
                    leaseDurationSeconds,
                    cancellationToken).ConfigureAwait(false);

                if (handled)
                    return;
            }

            if (terminalSoapException != null)
                throw terminalSoapException;
        }

        private async Task<bool> TryResolveExistingMappingConflictAsync(
            int externalPort,
            int internalPort,
            string internalClient,
            UpnpProtocol protocol,
            string description,
            int leaseDurationSeconds,
            CancellationToken cancellationToken)
        {
            UpnpPortMapping existing = null;
            try
            {
                existing = await GetSpecificPortMappingEntryAsync(externalPort, protocol, cancellationToken: cancellationToken).ConfigureAwait(false);
            }
            catch
            {
                existing = null;
            }

            if (existing == null)
                return false;

            bool sameClient = string.Equals(existing.InternalClient?.Trim(), internalClient?.Trim(), StringComparison.OrdinalIgnoreCase);
            if (!sameClient)
                return false;

            if (existing.Enabled && existing.InternalPort == internalPort)
                return true;

            bool ownedByBdsUtil = existing.Description != null &&
                                  existing.Description.StartsWith("BDSUtil", StringComparison.OrdinalIgnoreCase);
            if (!ownedByBdsUtil && existing.InternalPort != internalPort)
                return false;

            await DeletePortMappingAsync(externalPort, protocol, cancellationToken: cancellationToken).ConfigureAwait(false);
            await AddPortMappingOnCurrentServiceAsync(
                externalPort,
                internalPort,
                internalClient,
                protocol,
                description,
                leaseDurationSeconds,
                cancellationToken,
                allowConflictResolution: false).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> DeletePortMappingAsync(
            int externalPort,
            UpnpProtocol protocol,
            string remoteHost = "",
            CancellationToken cancellationToken = default)
        {
            try
            {
                await InvokeSoapActionAsync(
                    "DeletePortMapping",
                    new[]
                    {
                        new XElement("NewRemoteHost", remoteHost ?? string.Empty),
                        new XElement("NewExternalPort", externalPort),
                        new XElement("NewProtocol", FormatProtocol(protocol))
                    },
                    cancellationToken).ConfigureAwait(false);

                return true;
            }
            catch (UpnpSoapException ex) when (IsMappingNotFoundOrUnsupported(ex))
            {
                // Entry was already absent.
                return true;
            }
        }

        private Task SendAddPortMappingSoapAsync(
            int externalPort,
            int internalPort,
            string internalClient,
            UpnpProtocol protocol,
            string description,
            int leaseDurationSeconds,
            CancellationToken cancellationToken)
        {
            return InvokeSoapActionAsync(
                "AddPortMapping",
                new[]
                {
                    new XElement("NewRemoteHost", string.Empty),
                    new XElement("NewExternalPort", externalPort),
                    new XElement("NewProtocol", FormatProtocol(protocol)),
                    new XElement("NewInternalPort", internalPort),
                    new XElement("NewInternalClient", internalClient),
                    new XElement("NewEnabled", 1),
                    new XElement("NewPortMappingDescription", description ?? $"BDSUtil Node {externalPort}"),
                    new XElement("NewLeaseDuration", Math.Max(0, leaseDurationSeconds))
                },
                cancellationToken);
        }

        private async Task<XElement> InvokeSoapActionAsync(
            string actionName,
            IEnumerable<XElement> parameters,
            CancellationToken cancellationToken)
        {
            XNamespace s = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace u = ServiceType;

            var actionElement = new XElement(
                u + actionName,
                new XAttribute(XNamespace.Xmlns + "u", u.NamespaceName));
            if (parameters != null)
            {
                foreach (XElement param in parameters)
                {
                    actionElement.Add(new XElement(param.Name.LocalName, param.Value));
                }
            }

            var envelope = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement(s + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "s", s.NamespaceName),
                    new XAttribute(s + "encodingStyle", "http://schemas.xmlsoap.org/soap/encoding/"),
                    new XElement(s + "Body", actionElement)));

            string payloadXml;
            using (var sw = new Utf8StringWriter())
            {
                envelope.Save(sw, SaveOptions.DisableFormatting);
                payloadXml = sw.ToString();
            }

            using var request = new HttpRequestMessage(HttpMethod.Post, ControlUri);
            request.Headers.TryAddWithoutValidation("SOAPACTION", $"\"{ServiceType}#{actionName}\"");
            request.Content = new StringContent(payloadXml, Encoding.UTF8, "text/xml");
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml") { CharSet = "utf-8" };

            using HttpResponseMessage response = await _httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
            string responseBody = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            string sanitizedBody = SanitizeXmlPayload(responseBody);

            if (!response.IsSuccessStatusCode)
            {
                ThrowIfSoapFault(sanitizedBody, response.StatusCode, actionName);
                response.EnsureSuccessStatusCode();
            }

            if (string.IsNullOrWhiteSpace(sanitizedBody))
                return new XElement(actionName + "Response");

            XDocument responseDoc = XDocument.Parse(sanitizedBody);
            ThrowIfSoapFault(responseDoc, response.StatusCode, actionName);

            XElement responseNode = responseDoc
                .Descendants()
                .FirstOrDefault(e => e.Name.LocalName.Equals(actionName + "Response", StringComparison.OrdinalIgnoreCase));

            return responseNode ?? responseDoc.Root;
        }

        private static void ThrowIfSoapFault(string responseBody, HttpStatusCode statusCode, string actionName)
        {
            if (string.IsNullOrWhiteSpace(responseBody))
                return;

            try
            {
                XDocument doc = XDocument.Parse(responseBody);
                ThrowIfSoapFault(doc, statusCode, actionName);
            }
            catch (UpnpSoapException)
            {
                throw;
            }
            catch
            {
                // Ignore XML parse failure for non-XML error pages.
            }
        }

        private static void ThrowIfSoapFault(XDocument doc, HttpStatusCode statusCode, string actionName)
        {
            XElement fault = doc?.Descendants()
                .FirstOrDefault(e => e.Name.LocalName.Equals("Fault", StringComparison.OrdinalIgnoreCase));
            if (fault == null)
                return;

            int errorCode = ParseIntOrDefault(GetFirstDescendantValue(fault, "errorCode"), (int)statusCode);
            string errorDesc = GetFirstDescendantValue(fault, "errorDescription");
            if (string.IsNullOrWhiteSpace(errorDesc))
            {
                errorDesc = GetFirstDescendantValue(fault, "faultstring");
            }

            throw new UpnpSoapException(
                errorCode,
                errorDesc,
                $"UPnP SOAP action '{actionName}' failed with code {errorCode}: {errorDesc}");
        }

        private sealed class SsdpDiscoveryState
        {
            public object SyncLock { get; } = new object();
            public List<Uri> FoundLocations { get; } = new List<Uri>();
            public HashSet<string> SeenKeys { get; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            public bool PreferredMatched { get; set; }
        }

        private static async Task<List<Uri>> ProbeSsdpLocationsAsync(
            IReadOnlyList<IPAddress> interfaces,
            IPAddress multicastAddress,
            int multicastPort,
            TimeSpan timeout,
            HashSet<string> preferredGatewayIps,
            CancellationToken cancellationToken)
        {
            var state = new SsdpDiscoveryState();
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            linkedCts.CancelAfter(timeout);

            var tasks = interfaces.Select(localIp =>
                ProbeSingleInterfaceSsdpAsync(
                    localIp,
                    multicastAddress,
                    multicastPort,
                    timeout,
                    preferredGatewayIps,
                    state,
                    linkedCts));

            await Task.WhenAll(tasks).ConfigureAwait(false);
            return state.FoundLocations;
        }

        private static async Task ProbeSingleInterfaceSsdpAsync(
            IPAddress localIp,
            IPAddress multicastAddress,
            int multicastPort,
            TimeSpan totalTimeout,
            HashSet<string> preferredGatewayIps,
            SsdpDiscoveryState state,
            CancellationTokenSource sharedCts)
        {
            try
            {
                using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                socket.Bind(new IPEndPoint(localIp, 0));

                if (!localIp.Equals(IPAddress.Any))
                {
                    try
                    {
                        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, localIp.GetAddressBytes());
                    }
                    catch { }
                }

                try
                {
                    socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 4);
                }
                catch { }

                var multicastEndpoint = new IPEndPoint(multicastAddress, multicastPort);

                foreach (string target in SsdpSearchTargets)
                {
                    string request =
                        "M-SEARCH * HTTP/1.1\r\n" +
                        $"HOST: {multicastAddress}:{multicastPort}\r\n" +
                        "MAN: \"ssdp:discover\"\r\n" +
                        "MX: 2\r\n" +
                        $"ST: {target}\r\n" +
                        "USER-AGENT: BDSUtil/1.0 UPnP/1.1 .NET/8\r\n" +
                        "\r\n";

                    byte[] bytes = Encoding.ASCII.GetBytes(request);
                    await socket.SendToAsync(bytes, SocketFlags.None, multicastEndpoint, sharedCts.Token).ConfigureAwait(false);
                }

                byte[] buffer = new byte[4096];
                EndPoint remoteEp = new IPEndPoint(IPAddress.Any, 0);

                while (!sharedCts.IsCancellationRequested)
                {
                    SocketReceiveFromResult received = await socket.ReceiveFromAsync(buffer, SocketFlags.None, remoteEp, sharedCts.Token).ConfigureAwait(false);
                    if (received.ReceivedBytes <= 0)
                        continue;

                    string packetText = Encoding.UTF8.GetString(buffer, 0, received.ReceivedBytes);
                    if (TryParseSsdpLocation(packetText, out Uri locationUri))
                    {
                        TimeSpan? nextCancelAfter = null;
                        lock (state.SyncLock)
                        {
                            if (state.SeenKeys.Add(locationUri.AbsoluteUri))
                            {
                                state.FoundLocations.Add(locationUri);
                                bool isFirstDiscovery = state.FoundLocations.Count == 1;
                                bool hasPreferred = preferredGatewayIps != null && preferredGatewayIps.Count > 0;
                                bool matchesPreferred = hasPreferred && preferredGatewayIps.Contains(locationUri.Host);

                                if (matchesPreferred && !state.PreferredMatched)
                                {
                                    state.PreferredMatched = true;
                                    nextCancelAfter = TimeSpan.FromMilliseconds(200);
                                }
                                else if (isFirstDiscovery)
                                {
                                    nextCancelAfter = !hasPreferred
                                        ? TimeSpan.FromMilliseconds(200)
                                        : TimeSpan.FromMilliseconds(Math.Min(1000, Math.Max(300, (int)totalTimeout.TotalMilliseconds)));
                                }
                            }
                        }

                        if (nextCancelAfter.HasValue)
                        {
                            try { sharedCts.CancelAfter(nextCancelAfter.Value); } catch { }
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Discovery window completed.
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[SSDP Probe ({localIp})]: {ex.Message}");
            }
        }

        internal static bool TryParseSsdpLocation(string responseText, out Uri locationUri)
        {
            locationUri = null;
            if (string.IsNullOrWhiteSpace(responseText))
                return false;

            string[] lines = responseText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0 || !lines[0].StartsWith("HTTP/1.", StringComparison.OrdinalIgnoreCase))
                return false;

            string locationHeader = null;
            string stHeader = string.Empty;

            for (int i = 1; i < lines.Length; i++)
            {
                int colon = lines[i].IndexOf(':');
                if (colon <= 0) continue;

                string name = lines[i].Substring(0, colon).Trim();
                string value = lines[i].Substring(colon + 1).Trim();

                if (name.Equals("LOCATION", StringComparison.OrdinalIgnoreCase))
                {
                    locationHeader = value;
                }
                else if (name.Equals("ST", StringComparison.OrdinalIgnoreCase))
                {
                    stHeader = value;
                }
            }

            if (string.IsNullOrWhiteSpace(locationHeader))
                return false;

            if (!string.IsNullOrEmpty(stHeader) &&
                !stHeader.Contains("InternetGatewayDevice", StringComparison.OrdinalIgnoreCase) &&
                !stHeader.Contains("WANIPConnection", StringComparison.OrdinalIgnoreCase) &&
                !stHeader.Contains("WANPPPConnection", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return Uri.TryCreate(locationHeader, UriKind.Absolute, out locationUri);
        }

        internal static HashSet<string> GetDefaultGatewayIPv4Addresses()
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
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

                    foreach (GatewayIPAddressInformation gwInfo in nic.GetIPProperties().GatewayAddresses)
                    {
                        IPAddress addr = gwInfo?.Address;
                        if (addr != null &&
                            addr.AddressFamily == AddressFamily.InterNetwork &&
                            !IPAddress.IsLoopback(addr) &&
                            !addr.Equals(IPAddress.Any))
                        {
                            result.Add(addr.ToString());
                        }
                    }
                }
            }
            catch { }

            return result;
        }

        private static List<IPAddress> GetCandidateMulticastInterfaces()
        {
            var ordered = new List<IPAddress>();
            string primaryIpStr = UPnPEngine.GetLocalIPAddress();
            if (IPAddress.TryParse(primaryIpStr, out IPAddress primaryIp) &&
                !IPAddress.IsLoopback(primaryIp) &&
                primaryIp.AddressFamily == AddressFamily.InterNetwork)
            {
                ordered.Add(primaryIp);
            }

            try
            {
                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus != OperationalStatus.Up)
                        continue;

                    if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                        nic.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                        continue;

                    IPInterfaceProperties props = nic.GetIPProperties();
                    if (props.GatewayAddresses.Count == 0)
                        continue;

                    foreach (UnicastIPAddressInformation unicast in props.UnicastAddresses)
                    {
                        if (unicast.Address.AddressFamily == AddressFamily.InterNetwork &&
                            !IPAddress.IsLoopback(unicast.Address) &&
                            !ordered.Contains(unicast.Address))
                        {
                            ordered.Add(unicast.Address);
                        }
                    }
                }
            }
            catch { }

            return ordered;
        }

        private static Uri ResolveBaseUri(XElement root, Uri locationUri)
        {
            string urlBase = GetFirstDescendantValue(root, "URLBase").Trim();
            if (!string.IsNullOrWhiteSpace(urlBase) && Uri.TryCreate(urlBase, UriKind.Absolute, out Uri parsed))
            {
                // Guard against buggy router descriptions advertising 0.0.0.0 or 127.0.0.1 as URLBase.
                if (!locationUri.IsLoopback &&
                    (parsed.IsLoopback || string.Equals(parsed.Host, "0.0.0.0", StringComparison.OrdinalIgnoreCase)))
                {
                    return locationUri;
                }
                return parsed;
            }

            return locationUri;
        }

        private static List<WanServiceCandidate> ExtractWanServices(XElement root, Uri baseUri)
        {
            var candidates = new List<WanServiceCandidate>();

            foreach (XElement service in root.Descendants().Where(e => e.Name.LocalName.Equals("service", StringComparison.OrdinalIgnoreCase)))
            {
                string serviceType = GetFirstDescendantValue(service, "serviceType").Trim();
                string controlUrl = GetFirstDescendantValue(service, "controlURL").Trim();
                if (string.IsNullOrWhiteSpace(serviceType) || string.IsNullOrWhiteSpace(controlUrl))
                    continue;

                int priority = GetWanServicePriority(serviceType);
                if (priority <= 0)
                    continue;

                if (Uri.TryCreate(baseUri, controlUrl, out Uri resolvedControl))
                {
                    candidates.Add(new WanServiceCandidate
                    {
                        ServiceType = serviceType,
                        ControlUri = resolvedControl,
                        Priority = priority
                    });
                }
            }

            return candidates.OrderByDescending(c => c.Priority).ToList();
        }

        private static int GetWanServicePriority(string serviceType)
        {
            if (serviceType.Equals("urn:schemas-upnp-org:service:WANIPConnection:2", StringComparison.OrdinalIgnoreCase))
                return 3;
            if (serviceType.Equals("urn:schemas-upnp-org:service:WANIPConnection:1", StringComparison.OrdinalIgnoreCase))
                return 2;
            if (serviceType.Equals("urn:schemas-upnp-org:service:WANPPPConnection:1", StringComparison.OrdinalIgnoreCase))
                return 1;
            return 0;
        }

        private static async Task<WanServiceCandidate> SelectActiveServiceAsync(
            List<WanServiceCandidate> candidates,
            HttpClient httpClient,
            CancellationToken cancellationToken)
        {
            if (candidates.Count == 1)
                return candidates[0];

            foreach (WanServiceCandidate candidate in candidates)
            {
                try
                {
                    var probe = new UpnpGatewayClient(candidate.ControlUri, candidate.ControlUri, candidate.ServiceType, string.Empty, httpClient);
                    string status = await probe.GetConnectionStatusAsync(cancellationToken).ConfigureAwait(false);
                    if (status.Equals("Connected", StringComparison.OrdinalIgnoreCase))
                    {
                        return candidate;
                    }
                }
                catch
                {
                    // Try next WAN service if GetStatusInfo isn't supported or interface isn't active.
                }
            }

            return candidates[0];
        }

        private static string GetFirstDescendantValue(XElement parent, string localName)
        {
            if (parent == null) return string.Empty;
            XElement match = parent.Descendants().FirstOrDefault(e => e.Name.LocalName.Equals(localName, StringComparison.OrdinalIgnoreCase));
            return match?.Value ?? string.Empty;
        }

        private static string FormatProtocol(UpnpProtocol protocol) =>
            protocol == UpnpProtocol.Tcp ? "TCP" : "UDP";

        private static int ParseIntOrDefault(string raw, int fallback) =>
            int.TryParse(raw?.Trim(), out int parsed) ? parsed : fallback;

        private static bool ParseBoolOrDefault(string raw, bool defaultValue)
        {
            if (string.IsNullOrWhiteSpace(raw)) return defaultValue;
            string trimmed = raw.Trim();
            if (trimmed == "1") return true;
            if (trimmed == "0") return false;
            return bool.TryParse(trimmed, out bool parsed) ? parsed : defaultValue;
        }

        private static bool IsGenericEnumerationEndOrUnsupported(UpnpSoapException ex)
        {
            if (ex == null) return false;
            if (ex.ErrorCode is 713 or 714 or 402 or 501 or 606 or 401 or 711 or 899)
                return true;

            return ContainsAnyIgnoreCase(
                ex.UpnpErrorDescription,
                "Index",
                "Array",
                "NoSuchEntry",
                "Out of range",
                "Invalid arg",
                "Not Implemented",
                "Unsupported");
        }

        private static bool IsMappingNotFoundOrUnsupported(UpnpSoapException ex)
        {
            if (ex == null) return false;
            if (ex.ErrorCode is 714 or 713)
                return true;

            if (ex.ErrorCode is 501 or 402 or 401)
            {
                return ContainsAnyIgnoreCase(
                    ex.UpnpErrorDescription,
                    "NoSuchEntry",
                    "not found",
                    "not exist",
                    "Invalid NewPortMappingIndex",
                    "Invalid Action",
                    "Not Implemented");
            }

            return false;
        }

        private static bool IsLeaseDurationRejected(UpnpSoapException ex)
        {
            if (ex == null) return false;
            if (ex.ErrorCode is 402 or 501 or 606 or 899 or 726)
                return true;

            return ContainsAnyIgnoreCase(ex.UpnpErrorDescription, "Lease", "Duration", "Permanent");
        }

        private static bool IsPortConflictError(UpnpSoapException ex)
        {
            if (ex == null) return false;
            if (ex.ErrorCode == 718)
                return true;

            return ContainsAnyIgnoreCase(ex.UpnpErrorDescription, "Conflict", "already exist", "duplicate");
        }

        private static bool ShouldRetryAlternateService(UpnpSoapException ex)
        {
            if (ex == null) return false;
            return ex.ErrorCode is 401 or 501 or 602 or 606;
        }

        private static bool ContainsAnyIgnoreCase(string text, params string[] terms)
        {
            if (string.IsNullOrEmpty(text) || terms == null) return false;
            foreach (string term in terms)
            {
                if (text.Contains(term, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }

        private static string SanitizeXmlPayload(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return string.Empty;
            return raw.Trim('\0', '\uFEFF', ' ', '\t', '\r', '\n');
        }

        private sealed class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
