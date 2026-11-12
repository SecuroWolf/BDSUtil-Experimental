using BDSUtil.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BDSUtil.Models.AutoMod
{
    // --- 1. The Data Models ---
    public class PlayerEnforcementProfile
    {
        public string Gamertag { get; set; } = string.Empty;
        public string Xuid { get; set; } = string.Empty;

        public string ActiveSanctionType { get; set; } = "inactive"; // "inactive", "mute", "ban"
        public DateTime? SanctionExpiration { get; set; } // Null means Indefinite

        public List<EnforcementRecord> History { get; set; } = new List<EnforcementRecord>();

        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }

        public string GetFormattedListEntry()
        {
            if (string.Equals(ActiveSanctionType, "inactive", StringComparison.OrdinalIgnoreCase))
                return $"{Gamertag} [inactive]";

            string endDate = SanctionExpiration.HasValue
                ? SanctionExpiration.Value.ToString("MM/dd/yy HH:mm:ss")
                : "Indefinite";

            return $"{Gamertag} [{ActiveSanctionType}][Ends {endDate}]";
        }
    }

    public class EnforcementRecord
    {
        public DateTime Date { get; set; }
        public string ActionTaken { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string MessageContext { get; set; } = string.Empty;

        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }
    }

    // --- 2. The System.Text.Json File Manager ---
    public static class ProfileManager
    {
        private static readonly object _fileLock = new object();
        private static readonly string ProfilesDirectory = Path.Combine(AppContext.BaseDirectory, "enforcement_profiles");

        private static void EnsureDirectoryExists()
        {
            if (!Directory.Exists(ProfilesDirectory))
                Directory.CreateDirectory(ProfilesDirectory);
        }

        public static PlayerEnforcementProfile LoadProfile(string xuid)
        {
            EnsureDirectoryExists();
            string filePath = Path.Combine(ProfilesDirectory, $"{xuid}.ep");
            PlayerEnforcementProfile fallback = new PlayerEnforcementProfile { Xuid = xuid };

            if (!File.Exists(filePath)) return fallback;

            try
            {
                string rawJson;
                lock (_fileLock)
                {
                    rawJson = File.ReadAllText(filePath);
                }

                if (string.IsNullOrWhiteSpace(rawJson)) return fallback;

                PlayerEnforcementProfile profile = JsonSerializer.Deserialize<PlayerEnforcementProfile>(rawJson, JsonHelper.IndentedOptions);
                if (profile == null) return fallback;

                profile.Gamertag ??= string.Empty;
                profile.Xuid = string.IsNullOrWhiteSpace(profile.Xuid) ? xuid : profile.Xuid;
                profile.ActiveSanctionType = string.IsNullOrWhiteSpace(profile.ActiveSanctionType) ? "inactive" : profile.ActiveSanctionType;
                profile.History ??= new List<EnforcementRecord>();
                return profile;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[ProfileManager Load Error ({xuid})]: {ex.Message}");
                return fallback;
            }
        }

        public static void SaveProfile(PlayerEnforcementProfile profile)
        {
            if (profile == null || string.IsNullOrWhiteSpace(profile.Xuid)) return;

            EnsureDirectoryExists();
            profile.History ??= new List<EnforcementRecord>();

            string filePath = Path.Combine(ProfilesDirectory, $"{profile.Xuid}.ep");
            string json = JsonSerializer.Serialize(profile, JsonHelper.IndentedOptions);

            lock (_fileLock)
            {
                JsonHelper.WriteTextAtomically(filePath, json);
            }
        }

        public static List<PlayerEnforcementProfile> GetAllProfiles()
        {
            EnsureDirectoryExists();
            List<PlayerEnforcementProfile> profiles = new List<PlayerEnforcementProfile>();

            string[] files = Directory.GetFiles(ProfilesDirectory, "*.ep");
            foreach (string file in files)
            {
                string xuid = Path.GetFileNameWithoutExtension(file);
                profiles.Add(LoadProfile(xuid));
            }
            return profiles;
        }

        public static void DeleteProfile(string xuid)
        {
            string filePath = Path.Combine(ProfilesDirectory, $"{xuid}.ep");
            lock (_fileLock)
            {
                if (File.Exists(filePath))
                {
                    try { File.Delete(filePath); } catch { }
                }
            }
        }
    }
}