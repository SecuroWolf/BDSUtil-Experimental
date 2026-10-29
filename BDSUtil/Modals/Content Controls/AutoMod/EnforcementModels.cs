using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

        public string GetFormattedListEntry()
        {
            if (ActiveSanctionType.ToLower() == "inactive")
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
        public string ActionTaken { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string MessageContext { get; set; } = string.Empty; // New field for the raw chat text
    }

    // --- 2. The Dependency-Free File Manager ---
    public static class ProfileManager
    {
        private static readonly string ProfilesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "enforcement_profiles");

        private static void EnsureDirectoryExists()
        {
            if (!Directory.Exists(ProfilesDirectory))
                Directory.CreateDirectory(ProfilesDirectory);
        }

        public static PlayerEnforcementProfile LoadProfile(string xuid)
        {
            EnsureDirectoryExists();
            string filePath = Path.Combine(ProfilesDirectory, $"{xuid}.ep");
            PlayerEnforcementProfile profile = new PlayerEnforcementProfile { Xuid = xuid };

            if (!File.Exists(filePath)) return profile;

            try
            {
                string[] lines = File.ReadAllLines(filePath);
                bool inHistoryBlock = false;
                EnforcementRecord currentRecord = null;

                foreach (string rawLine in lines)
                {
                    string line = rawLine.Trim();

                    // ONLY skip empty lines and array brackets globally. 
                    // We need '{' and '}' to pass through so the History block can instantiate records!
                    if (string.IsNullOrEmpty(line) || line == "[" || line == "]") continue;

                    if (line.StartsWith("\"History\":"))
                    {
                        inHistoryBlock = true;
                        continue;
                    }

                    if (inHistoryBlock)
                    {
                        if (line == "{")
                        {
                            currentRecord = new EnforcementRecord();
                            continue;
                        }

                        if (line.StartsWith("}") || line.StartsWith("},"))
                        {
                            if (currentRecord != null) profile.History.Add(currentRecord);
                            currentRecord = null;
                            continue;
                        }

                        if (currentRecord != null) ParseRecordLine(line, currentRecord);
                    }
                    else
                    {
                        // Skip the outer wrapper brackets for the main profile block safely down here
                        if (line == "{" || line == "}") continue;

                        ParseProfileLine(line, profile);
                    }
                }
            }
            catch { }

            return profile;
        }

        private static void ParseProfileLine(string line, PlayerEnforcementProfile profile)
        {
            int colonIdx = line.IndexOf(':');
            if (colonIdx > 0)
            {
                string key = line.Substring(0, colonIdx).Trim(' ', '"', ',');
                string val = line.Substring(colonIdx + 1).Trim(' ', '"', ',');

                switch (key)
                {
                    case "Gamertag": profile.Gamertag = val; break;
                    case "Xuid": profile.Xuid = val; break;
                    case "ActiveSanctionType": profile.ActiveSanctionType = val; break;
                    case "SanctionExpiration":
                        if (DateTime.TryParse(val, out DateTime dt)) profile.SanctionExpiration = dt;
                        break;
                }
            }
        }

        private static void ParseRecordLine(string line, EnforcementRecord record)
        {
            int colonIdx = line.IndexOf(':');
            if (colonIdx > 0)
            {
                string key = line.Substring(0, colonIdx).Trim(' ', '"', ',');
                string val = line.Substring(colonIdx + 1).Trim(' ', '"', ',');

                switch (key)
                {
                    case "Date":
                        if (DateTime.TryParse(val, out DateTime dt)) record.Date = dt;
                        break;
                    case "ActionTaken": record.ActionTaken = val.Replace("\\\\", "\\"); break;
                    case "Reason": record.Reason = val.Replace("\\\\", "\\"); break;
                    case "MessageContext": record.MessageContext = val.Replace("\\\\", "\\"); break;
                }
            }
        }

        public static void SaveProfile(PlayerEnforcementProfile profile)
        {
            EnsureDirectoryExists();
            if (string.IsNullOrWhiteSpace(profile.Xuid)) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  \"Gamertag\": \"{profile.Gamertag}\",");
            sb.AppendLine($"  \"Xuid\": \"{profile.Xuid}\",");
            sb.AppendLine($"  \"ActiveSanctionType\": \"{profile.ActiveSanctionType}\",");

            string expStr = profile.SanctionExpiration.HasValue ? profile.SanctionExpiration.Value.ToString("O") : "";
            sb.AppendLine($"  \"SanctionExpiration\": \"{expStr}\",");

            sb.AppendLine("  \"History\": [");
            for (int i = 0; i < profile.History.Count; i++)
            {
                EnforcementRecord rec = profile.History[i];
                sb.AppendLine("    {");
                sb.AppendLine($"      \"Date\": \"{rec.Date:O}\",");
                sb.AppendLine($"      \"ActionTaken\": \"{rec.ActionTaken.Replace("\\", "\\\\")}\",");
                sb.AppendLine($"      \"Reason\": \"{rec.Reason.Replace("\\", "\\\\")}\",");
                sb.AppendLine($"      \"MessageContext\": \"{rec.MessageContext.Replace("\\", "\\\\")}\"");

                if (i < profile.History.Count - 1)
                    sb.AppendLine("    },");
                else
                    sb.AppendLine("    }");
            }
            sb.AppendLine("  ]");
            sb.AppendLine("}");

            string filePath = Path.Combine(ProfilesDirectory, $"{profile.Xuid}.ep");
            File.WriteAllText(filePath, sb.ToString());
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
            if (File.Exists(filePath))
            {
                try { File.Delete(filePath); } catch { }
            }
        }
    }
}