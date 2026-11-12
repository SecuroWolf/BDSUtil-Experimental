using BDSUtil.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BDSUtil.Configuration
{
    /// <summary>
    /// Handles one-time migration from legacy flat config.json and conf_am.json schemas into the unified v2 AppConfig.
    /// </summary>
    internal static class LegacyConfigMigrator
    {
        /// <summary>
        /// Returns true if the parsed JSON document already conforms to the v2 modular schema.
        /// </summary>
        public static bool IsUnifiedV2Config(JsonElement root)
        {
            if (root.ValueKind != JsonValueKind.Object) return false;

            if (JsonHelper.TryGetPropertyIgnoreCase(root, nameof(AppConfig.ConfigVersion), out JsonElement versionElem) &&
                JsonHelper.GetIntValue(versionElem, 0) >= AppConfig.CurrentConfigVersion)
            {
                return true;
            }

            // Also treat as v2 if core modular sections exist as JSON objects.
            return (JsonHelper.TryGetPropertyIgnoreCase(root, nameof(AppConfig.General), out JsonElement gen) && gen.ValueKind == JsonValueKind.Object)
                || (JsonHelper.TryGetPropertyIgnoreCase(root, nameof(AppConfig.AutoMod), out JsonElement am) && am.ValueKind == JsonValueKind.Object)
                || (JsonHelper.TryGetPropertyIgnoreCase(root, nameof(AppConfig.Networking), out JsonElement net) && net.ValueKind == JsonValueKind.Object)
                || (JsonHelper.TryGetPropertyIgnoreCase(root, nameof(AppConfig.RemoteApi), out JsonElement api) && api.ValueKind == JsonValueKind.Object);
        }

        /// <summary>
        /// Maps legacy flat config.json key/value pairs into the structured AppConfig model.
        /// </summary>
        public static AppConfig MigrateFlatConfig(JsonElement root)
        {
            var config = new AppConfig();
            if (root.ValueKind != JsonValueKind.Object) return config;

            foreach (JsonProperty prop in root.EnumerateObject())
            {
                string key = prop.Name;
                JsonElement val = prop.Value;

                if (key.Equals("ServerExecutablePath", StringComparison.OrdinalIgnoreCase))
                {
                    config.General.ServerExecutablePath = JsonHelper.GetStringValue(val, string.Empty);
                }
                else if (key.Equals("CompressionMode", StringComparison.OrdinalIgnoreCase))
                {
                    config.General.CompressionMode = JsonHelper.GetIntValue(val, 0);
                }
                else if (key.Equals("WorldEditorEnabled", StringComparison.OrdinalIgnoreCase))
                {
                    config.General.WorldEditorEnabled = JsonHelper.GetBoolValue(val, false);
                }
                else if (key.Equals("ExperimentsWarned", StringComparison.OrdinalIgnoreCase))
                {
                    config.General.ExperimentsWarned = JsonHelper.GetBoolValue(val, false);
                }
                else if (key.Equals("HideTelemetryWarnings", StringComparison.OrdinalIgnoreCase))
                {
                    config.General.HideTelemetryWarnings = JsonHelper.GetBoolValue(val, false);
                }
                else if (key.Equals("EnableUPnP", StringComparison.OrdinalIgnoreCase))
                {
                    config.Networking.EnableUPnP = JsonHelper.GetBoolValue(val, false);
                }
                else if (key.Equals("HasAcceptedNetworkRisks", StringComparison.OrdinalIgnoreCase))
                {
                    config.Networking.HasAcceptedNetworkRisks = JsonHelper.GetBoolValue(val, false);
                }
                else if (key.Equals("AutoModEnabled", StringComparison.OrdinalIgnoreCase))
                {
                    config.AutoMod.Enabled = JsonHelper.GetBoolValue(val, false);
                }
                else if (key.Equals("utilpack_op_commands", StringComparison.OrdinalIgnoreCase))
                {
                    config.UtilPack.OpCommands = JsonHelper.GetBoolValue(val, false);
                }
                else if (key.Equals("utilpack_pvp_awards", StringComparison.OrdinalIgnoreCase))
                {
                    config.UtilPack.PvpAwards = JsonHelper.GetBoolValue(val, false);
                }
                else if (key.Equals("utilpack_anti_grief", StringComparison.OrdinalIgnoreCase))
                {
                    config.UtilPack.AntiGrief = JsonHelper.GetBoolValue(val, false);
                }
                else if (key.Equals("utilpack_economy", StringComparison.OrdinalIgnoreCase))
                {
                    config.UtilPack.Economy = JsonHelper.GetBoolValue(val, false);
                }
                else if (key.Equals("utilpack_optimization", StringComparison.OrdinalIgnoreCase))
                {
                    config.UtilPack.Optimization = JsonHelper.GetBoolValue(val, false);
                }
                else if (key.StartsWith("utilpack_", StringComparison.OrdinalIgnoreCase))
                {
                    config.UtilPack.SetFlag(key, JsonHelper.GetBoolValue(val, false));
                }
                else
                {
                    // Preserve any custom legacy keys in Modules["Custom"]
                    PreserveCustomProperty(config, key, val);
                }
            }

            return config;
        }

        /// <summary>
        /// Merges legacy conf_am.json (or am_conf.json) key/value pairs into <see cref="AppConfig.AutoMod"/>.
        /// </summary>
        public static void MergeLegacyAutoModConfig(JsonElement root, AutoModSettings target)
        {
            if (root.ValueKind != JsonValueKind.Object || target == null) return;

            foreach (JsonProperty prop in root.EnumerateObject())
            {
                string key = prop.Name;
                JsonElement val = prop.Value;

                if (key.Equals("EnableChatEnforcement", StringComparison.OrdinalIgnoreCase))
                {
                    target.EnableChatEnforcement = JsonHelper.GetBoolValue(val, target.EnableChatEnforcement);
                }
                else if (key.Equals("PrimaryEnforcementType", StringComparison.OrdinalIgnoreCase))
                {
                    string str = JsonHelper.GetStringValue(val, target.PrimaryEnforcementType);
                    if (!string.IsNullOrWhiteSpace(str)) target.PrimaryEnforcementType = str;
                }
                else if (key.Equals("PrimaryEnforcementThreshold", StringComparison.OrdinalIgnoreCase))
                {
                    target.PrimaryEnforcementThreshold = JsonHelper.GetNullableIntValue(val);
                }
                else if (key.Equals("SecondaryEnforcementType", StringComparison.OrdinalIgnoreCase))
                {
                    string str = JsonHelper.GetStringValue(val, target.SecondaryEnforcementType);
                    if (!string.IsNullOrWhiteSpace(str)) target.SecondaryEnforcementType = str;
                }
                else if (key.Equals("SecondaryEnforcementThreshold", StringComparison.OrdinalIgnoreCase))
                {
                    target.SecondaryEnforcementThreshold = JsonHelper.GetNullableIntValue(val);
                }
                else if (key.Equals("AntiSpamEnabled", StringComparison.OrdinalIgnoreCase))
                {
                    target.AntiSpamEnabled = JsonHelper.GetBoolValue(val, target.AntiSpamEnabled);
                }
                else if (key.Equals("AntiSpamMessages", StringComparison.OrdinalIgnoreCase))
                {
                    int parsed = JsonHelper.GetIntValue(val, target.AntiSpamMessages);
                    if (parsed > 0) target.AntiSpamMessages = parsed;
                }
                else if (key.Equals("AntiSpamTime", StringComparison.OrdinalIgnoreCase))
                {
                    int parsed = JsonHelper.GetIntValue(val, target.AntiSpamTime);
                    if (parsed > 0) target.AntiSpamTime = parsed;
                }
                else if (key.Equals("AntiSpamEnforcementType", StringComparison.OrdinalIgnoreCase))
                {
                    string str = JsonHelper.GetStringValue(val, target.AntiSpamEnforcementType);
                    if (!string.IsNullOrWhiteSpace(str)) target.AntiSpamEnforcementType = str;
                }
                else if (key.Equals("ChatStrikeReset", StringComparison.OrdinalIgnoreCase))
                {
                    string str = JsonHelper.GetStringValue(val, target.ChatStrikeReset);
                    if (!string.IsNullOrWhiteSpace(str)) target.ChatStrikeReset = str;
                }
                else if (key.Equals("AntiSpamStrikeReset", StringComparison.OrdinalIgnoreCase))
                {
                    string str = JsonHelper.GetStringValue(val, target.AntiSpamStrikeReset);
                    if (!string.IsNullOrWhiteSpace(str)) target.AntiSpamStrikeReset = str;
                }
                else
                {
                    target.ExtensionData ??= new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
                    target.ExtensionData[key] = val.Clone();
                }
            }
        }

        private static void PreserveCustomProperty(AppConfig config, string key, JsonElement val)
        {
            var customMap = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
            if (config.Modules.TryGetValue("Custom", out JsonElement existingCustom) &&
                existingCustom.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty existingProp in existingCustom.EnumerateObject())
                {
                    customMap[existingProp.Name] = existingProp.Value.Clone();
                }
            }

            customMap[key] = val.Clone();
            config.Modules["Custom"] = JsonSerializer.SerializeToElement(customMap, JsonHelper.CompactOptions);
        }
    }
}

