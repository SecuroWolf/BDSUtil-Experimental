using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BDSUtil.Configuration
{
    /// <summary>
    /// Configuration for BDSUtil's built-in HTTP/JSON Remote Management API.
    /// Off by default; when disabled, no port is requisitioned via UPnP and no listener is started.
    /// </summary>
    public sealed class RemoteApiSettings
    {
        /// <summary>
        /// Enables or disables the Remote Management API listener and its UPnP port requisition.
        /// Strictly off by default.
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// Tracks whether the operator acknowledged the security notice for remote management access.
        /// </summary>
        public bool HasAcceptedApiRisks { get; set; } = false;

        /// <summary>
        /// Bearer / X-API-Key secret required by authenticated Remote Management API endpoints.
        /// </summary>
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        /// Optional preferred port below the lowest server port. When null, the highest free port below the lowest server port is selected.
        /// </summary>
        public int? PreferredPort { get; set; } = null;

        /// <summary>
        /// Preserves unrecognized properties inside the RemoteApi section for forward/backward compatibility.
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }

        internal void Normalize()
        {
            ApiKey = string.IsNullOrWhiteSpace(ApiKey) ? string.Empty : ApiKey.Trim();

            if (PreferredPort.HasValue && (PreferredPort.Value <= 0 || PreferredPort.Value > 65535))
            {
                PreferredPort = null;
            }

            if (Enabled && string.IsNullOrEmpty(ApiKey))
            {
                ApiKey = GenerateApiKey();
            }
        }

        /// <summary>
        /// Generates a URL-safe, header-safe cryptographically random API key.
        /// </summary>
        public static string GenerateApiKey()
        {
            byte[] randomBytes = RandomNumberGenerator.GetBytes(24);
            return Convert.ToBase64String(randomBytes)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}

