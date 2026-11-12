using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BDSUtil.Configuration
{
    /// <summary>
    /// Unified AutoMod configuration (merged from legacy conf_am.json and AutoModEnabled).
    /// </summary>
    public sealed class AutoModSettings
    {
        public bool Enabled { get; set; } = false;
        public bool EnableChatEnforcement { get; set; } = false;
        public string PrimaryEnforcementType { get; set; } = "Kick";
        public int? PrimaryEnforcementThreshold { get; set; } = null;
        public string SecondaryEnforcementType { get; set; } = "Temp-Ban(24h)";
        public int? SecondaryEnforcementThreshold { get; set; } = null;
        public bool AntiSpamEnabled { get; set; } = false;
        public int AntiSpamMessages { get; set; } = 5;
        public int AntiSpamTime { get; set; } = 3;
        public string AntiSpamEnforcementType { get; set; } = "Mute(5m)";
        public string ChatStrikeReset { get; set; } = "12 Hours";
        public string AntiSpamStrikeReset { get; set; } = "1 Hour";

        /// <summary>
        /// Preserves unrecognized properties inside the AutoMod section for forward/backward compatibility.
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }

        internal void Normalize()
        {
            if (string.IsNullOrWhiteSpace(PrimaryEnforcementType)) PrimaryEnforcementType = "Kick";
            if (string.IsNullOrWhiteSpace(SecondaryEnforcementType)) SecondaryEnforcementType = "Temp-Ban(24h)";
            if (string.IsNullOrWhiteSpace(AntiSpamEnforcementType)) AntiSpamEnforcementType = "Mute(5m)";
            if (string.IsNullOrWhiteSpace(ChatStrikeReset)) ChatStrikeReset = "12 Hours";
            if (string.IsNullOrWhiteSpace(AntiSpamStrikeReset)) AntiSpamStrikeReset = "1 Hour";
            if (AntiSpamMessages <= 0) AntiSpamMessages = 5;
            if (AntiSpamTime <= 0) AntiSpamTime = 3;
        }
    }
}

