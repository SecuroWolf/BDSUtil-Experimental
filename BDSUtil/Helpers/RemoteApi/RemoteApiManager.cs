using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BDSUtil.Configuration;

namespace BDSUtil.Helpers.RemoteApi
{
    public sealed class RemoteApiReconcileResult
    {
        public bool Enabled { get; set; }
        public bool ListenerRunning { get; set; }
        public int BoundPort { get; set; }
        public int LowestServerPort { get; set; }
        public bool UpnpRequested { get; set; }
        public bool UpnpLeased { get; set; }
        public string ConflictHost { get; set; } = string.Empty;
        public string ExternalIp { get; set; } = string.Empty;
        public string LocalIp { get; set; } = string.Empty;
        public PortForwardVerificationResult Verification { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    /// <summary>
    /// Coordinates the lifecycle and UPnP port requisitioning of BDSUtil's Remote Management API.
    /// Guarantees that when RemoteApi is disabled, no listener starts and no UPnP port is requisitioned.
    /// </summary>
    public sealed class RemoteApiManager : IDisposable
    {
        public static RemoteApiManager Instance { get; } = new RemoteApiManager();

        private readonly SemaphoreSlim _gate = new SemaphoreSlim(1, 1);
        private readonly RemoteManagementHttpServer _httpServer = new RemoteManagementHttpServer();
        private readonly IPAddress _bindAddress;

        private IRemoteManagementHost _host;
        private Action<string> _logCallback;

        public RemoteApiManager(IPAddress bindAddress = null)
        {
            _bindAddress = bindAddress ?? IPAddress.Any;
            _httpServer.StateChanged += (running, port, error) =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    StatusMessage = error;
                }
                StateChanged?.Invoke();
            };
        }

        public bool IsRunning => _httpServer.IsRunning;
        public int BoundPort => _httpServer.BoundPort;
        public int LeasedUpnpPort { get; private set; }
        public bool IsUpnpForwarded { get; private set; }
        public string ExternalIp { get; private set; } = string.Empty;
        public string LocalIp { get; private set; } = string.Empty;
        public int LowestServerPort { get; private set; } = ServerPortDiscovery.DefaultPrimaryPortV4;
        public PortForwardVerificationResult LastVerification { get; private set; }
        public string StatusMessage { get; private set; } = "Disabled";

        public event Action StateChanged;

        public void AttachHost(IRemoteManagementHost host, Action<string> logCallback = null)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
            _logCallback = logCallback;
        }

        /// <summary>
        /// Synchronizes the Remote Management API with the provided UPnP gateway and existing router TCP mappings.
        /// Strictly does nothing (stops any active listener and releases any lease) when <paramref name="settings"/> is disabled.
        /// </summary>
        public async Task<RemoteApiReconcileResult> ReconcileWithUpnpAsync(
            RemoteApiSettings settings,
            bool upnpEnabled,
            ServerPortSnapshot portSnapshot,
            UpnpGatewayClient gateway,
            IReadOnlyDictionary<int, UpnpPortMapping> existingRouterTcpMappings,
            ISet<string> hostIps,
            string localIp,
            string externalIp,
            string externalProbeUrl = null,
            bool verifyExternalReachability = true,
            HttpClient customProbeHttpClient = null,
            string probeTargetIpOverride = null,
            IPAddress localEchoBindOverride = null,
            CancellationToken ct = default)
        {
            await _gate.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                portSnapshot ??= new ServerPortSnapshot();
                LowestServerPort = portSnapshot.LowestServerPort;
                LocalIp = localIp ?? UPnPEngine.GetLocalIPAddress();
                ExternalIp = externalIp ?? string.Empty;

                if (settings == null || !settings.Enabled)
                {
                    await StopAndReleaseInternalAsync(upnpEnabled ? gateway : null, ct).ConfigureAwait(false);
                    StatusMessage = "Disabled (No UPnP port requisitioned; listener stopped)";
                    StateChanged?.Invoke();

                    return new RemoteApiReconcileResult
                    {
                        Enabled = false,
                        ListenerRunning = false,
                        BoundPort = 0,
                        LowestServerPort = portSnapshot.LowestServerPort,
                        UpnpRequested = false,
                        UpnpLeased = false,
                        LocalIp = LocalIp,
                        ExternalIp = ExternalIp,
                        Message = StatusMessage
                    };
                }

                (bool boundOk, int actualPort, string bindErrorMessage) = await EnsureListenerBoundInternalAsync(
                    settings,
                    portSnapshot,
                    upnpEnabled ? existingRouterTcpMappings : null,
                    upnpEnabled ? hostIps : null,
                    releaseOldUpnpLeaseOnRebind: upnpEnabled ? gateway : null,
                    ct: ct).ConfigureAwait(false);

                if (!boundOk)
                {
                    return new RemoteApiReconcileResult
                    {
                        Enabled = true,
                        ListenerRunning = false,
                        LowestServerPort = portSnapshot.LowestServerPort,
                        Message = bindErrorMessage
                    };
                }

                bool leasedOnRouter = false;
                string conflictHost = string.Empty;
                PortForwardVerificationResult verifyResult = null;

                if (upnpEnabled && gateway != null)
                {
                    if (verifyExternalReachability)
                    {
                        IReadOnlyList<UpnpPortMapping> existingList = existingRouterTcpMappings != null
                            ? new List<UpnpPortMapping>(existingRouterTcpMappings.Values)
                            : null;

                        UpnpPortSetupResult setupOutcome = await UPnPEngine.RequisitionAndVerifyPortWithRetryAsync(
                            new UpnpPortSpec
                            {
                                ExternalPort = actualPort,
                                InternalPort = actualPort,
                                Protocol = UpnpProtocol.Tcp,
                                Label = "Remote Management API",
                                Description = $"BDSUtil Remote API {actualPort}",
                                IsGameUdpNode = false
                            },
                            new PortVerificationOptions
                            {
                                Gateway = gateway,
                                ExternalProbeUrl = externalProbeUrl ?? string.Empty,
                                PublicExternalIpOverride = externalIp,
                                LocalIpOverride = LocalIp,
                                ExternalProbeTargetIpOverride = probeTargetIpOverride,
                                LocalEchoBindIpOverride = localEchoBindOverride,
                                CustomHttpClient = customProbeHttpClient
                            },
                            existingMappingsOverride: existingList,
                            cancellationToken: ct).ConfigureAwait(false);

                        leasedOnRouter = setupOutcome.RequisitionSucceeded;
                        conflictHost = setupOutcome.ConflictHost ?? string.Empty;
                        verifyResult = setupOutcome.Verification;
                    }
                    else
                    {
                        UpnpPortMapping existing = null;
                        if (existingRouterTcpMappings != null)
                        {
                            existingRouterTcpMappings.TryGetValue(actualPort, out existing);
                        }
                        else
                        {
                            existing = await UPnPEngine.GetSpecificMappingAsync(actualPort, "TCP", gateway, ct).ConfigureAwait(false);
                        }

                        if (existing != null &&
                            existing.Enabled &&
                            hostIps != null &&
                            hostIps.Contains(existing.InternalClient) &&
                            existing.InternalPort == actualPort)
                        {
                            leasedOnRouter = true;
                            UPnPEngine.RegisterSessionLease(actualPort, "TCP");
                        }
                        else if (existing != null &&
                                 existing.Enabled &&
                                 (hostIps == null || !hostIps.Contains(existing.InternalClient)))
                        {
                            conflictHost = existing.InternalClient;
                        }
                        else
                        {
                            leasedOnRouter = await UPnPEngine.ForwardPortAsync(
                                actualPort,
                                actualPort,
                                LocalIp,
                                "TCP",
                                verifyAfterForward: true,
                                gatewayOverride: gateway,
                                cancellationToken: ct,
                                description: $"BDSUtil Remote API {actualPort}").ConfigureAwait(false);
                        }
                    }

                    LeasedUpnpPort = leasedOnRouter ? actualPort : 0;
                    IsUpnpForwarded = leasedOnRouter;
                }
                else
                {
                    LeasedUpnpPort = 0;
                    IsUpnpForwarded = false;
                }

                LastVerification = verifyResult;

                if (upnpEnabled && gateway == null)
                {
                    StatusMessage = $"Listening locally on TCP {actualPort} (below server port {portSnapshot.LowestServerPort}); UPnP gateway unavailable.";
                }
                else if (upnpEnabled && leasedOnRouter)
                {
                    StatusMessage = $"Listening on TCP {actualPort} (< {portSnapshot.LowestServerPort}); UPnP forwarded ({verifyResult?.AccessibilityState.ToString() ?? "Leased"}).";
                }
                else if (upnpEnabled)
                {
                    StatusMessage = $"Listening locally on TCP {actualPort} (< {portSnapshot.LowestServerPort}); UPnP mapping failed.";
                }
                else
                {
                    StatusMessage = $"Listening locally on TCP {actualPort} (below server port {portSnapshot.LowestServerPort}; UPnP off).";
                }

                StateChanged?.Invoke();

                return new RemoteApiReconcileResult
                {
                    Enabled = true,
                    ListenerRunning = _httpServer.IsRunning,
                    BoundPort = actualPort,
                    LowestServerPort = portSnapshot.LowestServerPort,
                    UpnpRequested = upnpEnabled && gateway != null,
                    UpnpLeased = leasedOnRouter,
                    ConflictHost = conflictHost,
                    LocalIp = LocalIp,
                    ExternalIp = ExternalIp,
                    Verification = verifyResult,
                    Message = StatusMessage
                };
            }
            finally
            {
                _gate.Release();
            }
        }

        public async Task<int> BindListenerForBatchUpnpAsync(
            RemoteApiSettings settings,
            ServerPortSnapshot portSnapshot,
            IReadOnlyDictionary<int, UpnpPortMapping> existingRouterTcpMappings,
            ISet<string> hostIps,
            string localIp,
            string externalIp,
            CancellationToken ct = default)
        {
            await _gate.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                portSnapshot ??= new ServerPortSnapshot();
                LowestServerPort = portSnapshot.LowestServerPort;
                LocalIp = localIp ?? UPnPEngine.GetLocalIPAddress();
                ExternalIp = externalIp ?? string.Empty;

                if (settings == null || !settings.Enabled)
                {
                    await StopAndReleaseInternalAsync(null, ct).ConfigureAwait(false);
                    StatusMessage = "Disabled (No UPnP port requisitioned; listener stopped)";
                    StateChanged?.Invoke();
                    return 0;
                }

                (bool ok, int boundPort, _) = await EnsureListenerBoundInternalAsync(
                    settings,
                    portSnapshot,
                    existingRouterTcpMappings,
                    hostIps,
                    releaseOldUpnpLeaseOnRebind: null,
                    ct: ct).ConfigureAwait(false);

                if (!ok)
                {
                    return 0;
                }

                StatusMessage = $"Listening locally on TCP {boundPort} (< {portSnapshot.LowestServerPort}); verifying UPnP...";
                StateChanged?.Invoke();
                return boundPort;
            }
            finally
            {
                _gate.Release();
            }
        }

        private async Task<(bool Success, int BoundPort, string ErrorMessage)> EnsureListenerBoundInternalAsync(
            RemoteApiSettings settings,
            ServerPortSnapshot portSnapshot,
            IReadOnlyDictionary<int, UpnpPortMapping> existingRouterTcpMappings,
            ISet<string> hostIps,
            UpnpGatewayClient releaseOldUpnpLeaseOnRebind,
            CancellationToken ct)
        {
            if (_host == null)
            {
                StatusMessage = "Remote Management API host is not attached.";
                StateChanged?.Invoke();
                return (false, 0, StatusMessage);
            }

            int targetPort = ServerPortDiscovery.FindAvailableApiPortBelow(
                portSnapshot.LowestServerPort,
                portSnapshot.ReservedServerPorts,
                existingRouterTcpMappings,
                hostIps,
                settings.PreferredPort,
                _httpServer.IsRunning ? _httpServer.BoundPort : 0,
                _bindAddress);

            if (targetPort <= 0)
            {
                await StopAndReleaseInternalAsync(releaseOldUpnpLeaseOnRebind, ct).ConfigureAwait(false);
                StatusMessage = $"Unable to locate a free TCP port strictly below lowest server port {portSnapshot.LowestServerPort}.";
                StateChanged?.Invoke();
                return (false, 0, StatusMessage);
            }

            // If bound on a different port (e.g., UPnP revealed a LAN collision or lowest server port shifted), rebind cleanly.
            if (_httpServer.IsRunning && _httpServer.BoundPort != targetPort)
            {
                int oldLeased = LeasedUpnpPort;
                await _httpServer.StopAsync().ConfigureAwait(false);
                if (oldLeased > 0 && oldLeased != targetPort && releaseOldUpnpLeaseOnRebind != null)
                {
                    await UPnPEngine.ClosePortAsync(oldLeased, "TCP", releaseOldUpnpLeaseOnRebind, ct).ConfigureAwait(false);
                    LeasedUpnpPort = 0;
                    IsUpnpForwarded = false;
                }
            }

            if (!_httpServer.IsRunning)
            {
                bool useGlobalSettings = object.ReferenceEquals(settings, AppSettings.Instance?.RemoteApi);
                bool started = await _httpServer.StartAsync(
                    targetPort,
                    () => useGlobalSettings ? (AppSettings.Instance?.RemoteApi?.ApiKey ?? string.Empty) : (settings.ApiKey ?? string.Empty),
                    _host,
                    _bindAddress,
                    _logCallback).ConfigureAwait(false);

                if (!started)
                {
                    StatusMessage = $"Failed to bind Remote Management API on port {targetPort}: {_httpServer.LastError}";
                    StateChanged?.Invoke();
                    return (false, 0, StatusMessage);
                }
            }

            return (true, _httpServer.BoundPort, string.Empty);
        }

        public async Task ApplyBatchUpnpOutcomeAsync(
            int lowestServerPort,
            string localIp,
            string externalIp,
            bool gatewayFound,
            UpnpPortSetupResult outcome,
            CancellationToken ct = default)
        {
            await _gate.WaitAsync(ct).ConfigureAwait(false);
            try
            {
                LowestServerPort = lowestServerPort;
                LocalIp = localIp ?? UPnPEngine.GetLocalIPAddress();
                ExternalIp = externalIp ?? string.Empty;

                if (!_httpServer.IsRunning)
                {
                    return;
                }

                int actualPort = _httpServer.BoundPort;
                bool leased = outcome != null && outcome.RequisitionSucceeded;
                LeasedUpnpPort = leased ? actualPort : 0;
                IsUpnpForwarded = leased;
                LastVerification = outcome?.Verification;

                if (!gatewayFound)
                {
                    StatusMessage = $"Listening locally on TCP {actualPort} (below server port {lowestServerPort}); UPnP gateway unavailable.";
                }
                else if (leased)
                {
                    StatusMessage = $"Listening on TCP {actualPort} (< {lowestServerPort}); UPnP forwarded ({outcome.AccessibilityState}).";
                }
                else
                {
                    StatusMessage = $"Listening locally on TCP {actualPort} (< {lowestServerPort}); UPnP mapping failed.";
                }

                StateChanged?.Invoke();
            }
            finally
            {
                _gate.Release();
            }
        }

        /// <summary>
        /// Synchronizes runtime state from AppSettings (used when Remote API is toggled in Settings or server directory changes).
        /// </summary>
        public async Task<RemoteApiReconcileResult> SynchronizeFromSettingsAsync(
            bool allowUpnpDiscovery,
            CancellationToken ct = default)
        {
            var networking = AppSettings.Instance?.Networking;
            var remoteApi = AppSettings.Instance?.RemoteApi;
            string serverRoot = AppSettings.Instance?.General?.ServerExecutablePath;
            bool upnpEnabled = networking != null && networking.EnableUPnP;

            ServerPortSnapshot portSnapshot = ServerPortDiscovery.ReadServerPortSnapshot(serverRoot);

            if (remoteApi == null || !remoteApi.Enabled)
            {
                UpnpGatewayClient existingGateway = null;
                if (allowUpnpDiscovery && upnpEnabled && LeasedUpnpPort > 0)
                {
                    existingGateway = await UPnPEngine.DiscoverGatewayAsync(TimeSpan.FromMilliseconds(2000), cancellationToken: ct).ConfigureAwait(false);
                }

                return await ReconcileWithUpnpAsync(
                    remoteApi,
                    upnpEnabled,
                    portSnapshot,
                    existingGateway,
                    null,
                    null,
                    UPnPEngine.GetLocalIPAddress(),
                    string.Empty,
                    cancellationTokenOrFallback: ct).ConfigureAwait(false);
            }

            string localIp = UPnPEngine.GetLocalIPAddress();
            var hostIps = new HashSet<string>(UPnPEngine.GetLocalIPAddresses(), StringComparer.OrdinalIgnoreCase) { localIp };

            UpnpGatewayClient gateway = null;
            IReadOnlyDictionary<int, UpnpPortMapping> tcpMappings = null;
            string externalIp = string.Empty;

            if (allowUpnpDiscovery && upnpEnabled)
            {
                Task<UpnpGatewayClient> gwTask = UPnPEngine.DiscoverGatewayAsync(TimeSpan.FromMilliseconds(2500), cancellationToken: ct);
                Task<string> extIpTask = UPnPEngine.GetExternalIPAddressAsync(ct);
                await Task.WhenAll(gwTask, extIpTask).ConfigureAwait(false);

                gateway = gwTask.Result;
                externalIp = extIpTask.Result;

                if (gateway != null)
                {
                    localIp = UPnPEngine.GetLocalIpForGateway(gateway);
                    hostIps.Add(localIp);

                    if (!UPnPEngine.IsValidPublicIPv4(externalIp))
                    {
                        try
                        {
                            string wanIp = await gateway.GetExternalIPAddressAsync(ct).ConfigureAwait(false);
                            if (UPnPEngine.IsValidPublicIPv4(wanIp))
                            {
                                externalIp = wanIp;
                            }
                        }
                        catch { }
                    }

                    try
                    {
                        var allMappings = await gateway.GetAllPortMappingsAsync(cancellationToken: ct).ConfigureAwait(false);
                        var mapDict = new Dictionary<int, UpnpPortMapping>();
                        foreach (var m in allMappings)
                        {
                            if (m.Protocol == UpnpProtocol.Tcp)
                            {
                                mapDict[m.ExternalPort] = m;
                            }
                        }
                        tcpMappings = mapDict;
                    }
                    catch { }
                }
            }

            return await ReconcileWithUpnpAsync(
                remoteApi,
                upnpEnabled,
                portSnapshot,
                gateway,
                tcpMappings,
                hostIps,
                localIp,
                externalIp,
                externalProbeUrl: networking?.ResolveEffectiveExternalProbeUrl(),
                verifyExternalReachability: allowUpnpDiscovery && upnpEnabled && gateway != null,
                ct: ct).ConfigureAwait(false);
        }

        // Helper overload for cleaner internal cancellation passing
        private Task<RemoteApiReconcileResult> ReconcileWithUpnpAsync(
            RemoteApiSettings settings,
            bool upnpEnabled,
            ServerPortSnapshot portSnapshot,
            UpnpGatewayClient gateway,
            IReadOnlyDictionary<int, UpnpPortMapping> existingRouterTcpMappings,
            ISet<string> hostIps,
            string localIp,
            string externalIp,
            CancellationToken cancellationTokenOrFallback) =>
            ReconcileWithUpnpAsync(
                settings,
                upnpEnabled,
                portSnapshot,
                gateway,
                existingRouterTcpMappings,
                hostIps,
                localIp,
                externalIp,
                externalProbeUrl: null,
                verifyExternalReachability: false,
                customProbeHttpClient: null,
                probeTargetIpOverride: null,
                localEchoBindOverride: null,
                ct: cancellationTokenOrFallback);

        public async Task StopAsync(bool releaseUpnpLease = false, UpnpGatewayClient gatewayOverride = null)
        {
            await _gate.WaitAsync().ConfigureAwait(false);
            try
            {
                await StopAndReleaseInternalAsync(releaseUpnpLease ? gatewayOverride : null, CancellationToken.None, forceDiscoverIfMissing: releaseUpnpLease).ConfigureAwait(false);
                StatusMessage = "Stopped";
                StateChanged?.Invoke();
            }
            finally
            {
                _gate.Release();
            }
        }

        private async Task StopAndReleaseInternalAsync(UpnpGatewayClient gateway, CancellationToken ct, bool forceDiscoverIfMissing = false)
        {
            int leased = LeasedUpnpPort;
            await _httpServer.StopAsync().ConfigureAwait(false);
            LeasedUpnpPort = 0;
            IsUpnpForwarded = false;
            LastVerification = null;

            if (leased > 0)
            {
                UpnpGatewayClient gw = gateway;
                if (gw == null && forceDiscoverIfMissing)
                {
                    try { gw = await UPnPEngine.DiscoverGatewayAsync(TimeSpan.FromMilliseconds(2000), cancellationToken: ct).ConfigureAwait(false); } catch { }
                }

                if (gw != null)
                {
                    try { await UPnPEngine.ClosePortAsync(leased, "TCP", gw, ct).ConfigureAwait(false); } catch { }
                }
            }
        }

        public void Dispose()
        {
            _httpServer.Dispose();
            _gate.Dispose();
        }
    }
}
