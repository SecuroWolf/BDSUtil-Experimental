using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BDSUtil.Configuration
{
    /// <summary>
    /// Feature toggles and extensible flags for built-in BDSUtil addon modules.
    /// </summary>
    public sealed class UtilPackSettings
    {
        public bool OpCommands { get; set; } = false;
        public bool PvpAwards { get; set; } = false;
        public bool AntiGrief { get; set; } = false;
        public bool Economy { get; set; } = false;
        public bool Optimization { get; set; } = false;

        /// <summary>
        /// Extensible dictionary for additional or dynamic UtilPack feature switches.
        /// </summary>
        public Dictionary<string, bool> CustomFlags { get; set; } = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }

        public bool GetFlag(string flagKey, bool defaultValue = false)
        {
            if (string.IsNullOrWhiteSpace(flagKey)) return defaultValue;
            return CustomFlags != null && CustomFlags.TryGetValue(flagKey, out bool val) ? val : defaultValue;
        }

        public void SetFlag(string flagKey, bool value)
        {
            if (string.IsNullOrWhiteSpace(flagKey)) return;
            CustomFlags ??= new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            CustomFlags[flagKey] = value;
        }

        internal void Normalize()
        {
            CustomFlags = CustomFlags != null
                ? new Dictionary<string, bool>(CustomFlags, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
        }
    }
}

