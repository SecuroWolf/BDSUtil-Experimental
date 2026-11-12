using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using BDSUtil.Helpers;

namespace BDSUtil.Configuration
{
    /// <summary>
    /// Network routing, UPnP, and connectivity security settings.
    /// </summary>
    public sealed class NetworkingSettings
    {
        public bool EnableUPnP { get; set; } = false;
        public bool ConsolidateUdpPorts { get; set; } = false;
        public bool HasAcceptedNetworkRisks { get; set; } = false;
        public string ExternalPortCheckProvider { get; set; } = string.Empty;
        public string ExternalPortCheckUrl { get; set; } = string.Empty;

        /// <summary>
        /// Preserves unrecognized properties inside the Networking section for forward/backward compatibility.
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }

        /// <summary>
        /// Normalizes networking defaults and migrates legacy ExternalPortCheckUrl values into ExternalPortCheckProvider.
        /// </summary>
        public void Normalize()
        {
            ExternalPortCheckUrl = ExternalPortCheckUrl?.Trim() ?? string.Empty;
            ExternalPortCheckProvider = ExternalPortChecker.NormalizeProviderKey(ExternalPortCheckProvider, ExternalPortCheckUrl);
        }

        /// <summary>
        /// Returns the effective external port-checker URL or preset spec, or empty when set to Off.
        /// </summary>
        public string ResolveEffectiveExternalProbeUrl()
        {
            return ExternalPortChecker.ResolveEffectiveProbeSpec(ExternalPortCheckProvider, ExternalPortCheckUrl);
        }
    }
}

