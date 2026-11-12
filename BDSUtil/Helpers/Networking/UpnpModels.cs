using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace BDSUtil.Helpers
{
    /// <summary>
    /// Optional inputs and test overrides for <see cref="UPnPEngine.VerifyPortAsync"/>.
    /// </summary>
    public sealed class PortVerificationOptions
    {
        public UpnpGatewayClient Gateway { get; set; }
        public string ExternalProbeUrl { get; set; } = string.Empty;
        public int TimeoutMs { get; set; } = 1500;

        internal string PublicExternalIpOverride { get; set; }
        internal string RouterExternalIpOverride { get; set; }
        internal string LocalIpOverride { get; set; }
        internal string ExternalProbeTargetIpOverride { get; set; }
        internal IPAddress LocalEchoBindIpOverride { get; set; }
        internal HttpClient CustomHttpClient { get; set; }
        internal bool SkipGatewayDiscovery { get; set; }
    }
    public enum UpnpProtocol
    {
        Udp,
        Tcp
    }

    public enum PortAccessibilityState
    {
        Unknown,
        NotForwarded,
        ConflictWithAnotherHost,
        GatewayUnavailable,
        BlockedByDoubleNatOrCgnat,
        LocalListenerUnavailable,
        ForwardedButLoopbackBlocked,
        VerifiedAccessible,
        UpnpDisabledOrUnresponsive
    }

    public sealed class UpnpPortSpec
    {
        public int ExternalPort { get; set; }
        public int InternalPort { get; set; }
        public UpnpProtocol Protocol { get; set; } = UpnpProtocol.Udp;
        public string Label { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsGameUdpNode { get; set; }
    }

    public sealed class UpnpPortSetupResult
    {
        public int ExternalPort { get; set; }
        public int InternalPort { get; set; }
        public UpnpProtocol Protocol { get; set; } = UpnpProtocol.Udp;
        public string Label { get; set; } = string.Empty;
        public bool IsGameUdpNode { get; set; }
        public bool RequisitionSucceeded { get; set; }
        public bool ReusedExistingLease { get; set; }
        public bool VerifiedAccessible { get; set; }
        public bool ConflictWithAnotherHost { get; set; }
        public string ConflictHost { get; set; } = string.Empty;
        public bool CheckedRouterLeaseTable { get; set; }
        public bool LeasePresentInRouterTable { get; set; }
        public bool RetriedRequisition { get; set; }
        public bool RetriedVerification { get; set; }
        public PortAccessibilityState AccessibilityState { get; set; } = PortAccessibilityState.Unknown;
        public PortForwardVerificationResult Verification { get; set; }
        public string DiagnosticMessage { get; set; } = string.Empty;
    }

    public sealed class UpnpBatchSetupResult
    {
        public bool GatewayFound { get; set; }
        public string LocalIp { get; set; } = string.Empty;
        public string PublicExternalIp { get; set; } = string.Empty;
        public string RouterExternalIp { get; set; } = string.Empty;
        public List<UpnpPortSetupResult> PortResults { get; set; } = new List<UpnpPortSetupResult>();
        public List<string> SuccessfulUdpNodeTokens { get; set; } = new List<string>();
        public bool AllPortsVerified { get; set; }
        public bool AllPortsInRouterTable { get; set; }
        public PortAccessibilityState PrimaryFailureState { get; set; } = PortAccessibilityState.Unknown;
        public string SummaryMessage { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a single port-forwarding rule stored on a UPnP IGD router.
    /// </summary>
    public sealed class UpnpPortMapping
    {
        public string RemoteHost { get; set; } = string.Empty;
        public int ExternalPort { get; set; }
        public int InternalPort { get; set; }
        public UpnpProtocol Protocol { get; set; } = UpnpProtocol.Udp;
        public string InternalClient { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public string Description { get; set; } = string.Empty;
        public int LeaseDuration { get; set; }
    }

    /// <summary>
    /// Describes the verification outcome for both local host forwarding and external IP accessibility.
    /// </summary>
    public sealed class PortForwardVerificationResult
    {
        public int ExternalPort { get; set; }
        public int InternalPort { get; set; }
        public UpnpProtocol Protocol { get; set; } = UpnpProtocol.Udp;

        public string LocalIp { get; set; } = string.Empty;
        public string RouterExternalIp { get; set; } = string.Empty;
        public string PublicExternalIp { get; set; } = string.Empty;
        public string GatewayDescription { get; set; } = string.Empty;

        public bool GatewayFound { get; set; }
        public bool IsForwardedToHost { get; set; }
        public bool ForwardedViaManualOrUnknownRule { get; set; }
        public bool IsAssignedToAnotherHost { get; set; }
        public string MappedInternalClient { get; set; } = string.Empty;

        public bool IsBehindDoubleNatOrCgnat { get; set; }
        public bool LocalListenerDetected { get; set; }
        public bool UsedEphemeralProbeListener { get; set; }
        public bool ExternalProbeSucceeded { get; set; }
        public bool ExternalRelayChecked { get; set; }

        /// <summary>
        /// True when confirmed reachable from the external IP; false when confirmed blocked
        /// (e.g., CGNAT/double-NAT, conflict, or external relay rejection); null when router
        /// forwarding succeeded but LAN hairpin/loopback timed out without an external relay.
        /// </summary>
        public bool? IsExternallyAccessible { get; set; }

        public PortAccessibilityState AccessibilityState { get; set; } = PortAccessibilityState.Unknown;
        public string ServerMotd { get; set; } = string.Empty;
        public TimeSpan? RoundTripTime { get; set; }
        public string DiagnosticMessage { get; set; } = string.Empty;
        public bool CheckedRouterLeaseTable { get; set; }
        public bool LeasePresentInRouterTable { get; set; }
        public bool RetriedRequisition { get; set; }
        public bool RetriedVerification { get; set; }
    }

    /// <summary>
    /// Exception raised when a UPnP gateway returns a SOAP fault response.
    /// </summary>
    public sealed class UpnpSoapException : Exception
    {
        public int ErrorCode { get; }
        public string UpnpErrorDescription { get; }

        public UpnpSoapException(int errorCode, string errorDescription, string message)
            : base(message)
        {
            ErrorCode = errorCode;
            UpnpErrorDescription = errorDescription ?? string.Empty;
        }
    }
}
