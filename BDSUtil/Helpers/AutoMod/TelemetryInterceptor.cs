using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BDSUtil.Models.AutoMod;

namespace BDSUtil.Helpers.AutoMod
{
    public static class TelemetryInterceptor
    {
        // Maintains a local queue of recent chat timestamps for the Anti-Spam engine
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, List<DateTime>> _spamTracker
            = new System.Collections.Concurrent.ConcurrentDictionary<string, List<DateTime>>();

        // Thread-safe memory cache for the Base64 decoded profanity filter
        private static List<string> _bannedWordsCache = null;
        private static readonly object _filterLock = new object();

        /// <summary>
        /// Asynchronously evaluates a chat message against all active AutoMod rules.
        /// </summary>
        public static void EvaluateChatEventAsync(string player, string message, Process activeServerProcess, Action<string> uiConsoleCallback)
        {
            Task.Run(() =>
            {
                if (!AMConfHelper.Instance.EnableChatEnforcement && !AMConfHelper.Instance.AntiSpamEnabled) return;

                bool isViolation = false;
                string violationReason = string.Empty;
                string enforcementAction = string.Empty;

                // --- 1. Evaluate Anti-Spam Rules ---
                if (AMConfHelper.Instance.AntiSpamEnabled)
                {
                    if (IsSpamming(player))
                    {
                        isViolation = true;
                        violationReason = "Automated Chat Spam Detection";
                        enforcementAction = AMConfHelper.Instance.AntiSpamEnforcementType;
                    }
                }

                // --- 2. Evaluate Chat Enforcement (Banned Words) ---
                if (!isViolation && AMConfHelper.Instance.EnableChatEnforcement)
                {
                    EnsureProfanityFilterLoaded();

                    lock (_filterLock)
                    {
                        if (_bannedWordsCache != null)
                        {
                            foreach (string word in _bannedWordsCache)
                            {
                                // We pad the message with spaces to prevent the 'Scunthorpe Problem' 
                                // (e.g., banning 'ass' shouldn't ban the word 'class')
                                // Using a basic string check since Regex can be heavy on a hot chat loop.
                                if (message.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)
                                {
                                    isViolation = true;
                                    violationReason = "Chat Filter Policy Violation";
                                    enforcementAction = DeterminePrimaryEnforcement(player);
                                    break;
                                }
                            }
                        }
                    }
                }

                // --- 3. Execute Enforcement ---
                if (isViolation)
                {
                    ExecuteEnforcement(player, violationReason, enforcementAction, message, activeServerProcess, uiConsoleCallback);
                }
            });
        }

        /// <summary>
        /// Reads, decodes, and caches the Base64 profanity filter into memory securely.
        /// </summary>
        private static void EnsureProfanityFilterLoaded()
        {
            lock (_filterLock)
            {
                if (_bannedWordsCache != null) return;

                _bannedWordsCache = new List<string>();

                try
                {
                    string serverDir = AppSettings.Instance.ServerExecutablePath;
                    if (string.IsNullOrEmpty(serverDir)) return;

                    string wlistPath = Path.Combine(serverDir, "profanity_filter.wlist");
                    if (File.Exists(wlistPath))
                    {
                        string base64Content = File.ReadAllText(wlistPath).Trim();
                        byte[] rawBytes = Convert.FromBase64String(base64Content);
                        string decodedText = Encoding.UTF8.GetString(rawBytes);

                        // Split by both carriage returns and newlines
                        string[] lines = decodedText.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string line in lines)
                        {
                            // Strip out the hidden UTF-8 BOM (\uFEFF) that occurs at the start of the decoded Base64 string
                            string cleanWord = line.Replace("\uFEFF", "").Trim();

                            if (!string.IsNullOrEmpty(cleanWord))
                            {
                                _bannedWordsCache.Add(cleanWord);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[AutoMod Filter Load Error]: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Call this method if you ever add a GUI button to reload the profanity filter while the server is running!
        /// </summary>
        public static void ForceReloadProfanityFilter()
        {
            lock (_filterLock)
            {
                _bannedWordsCache = null;
            }
        }

        private static bool IsSpamming(string player)
        {
            if (!int.TryParse(AMConfHelper.Instance.AntiSpamMessages, out int maxMessages)) maxMessages = 5;
            if (!int.TryParse(AMConfHelper.Instance.AntiSpamTime, out int timeWindowSeconds)) timeWindowSeconds = 3;

            DateTime now = DateTime.Now;
            var userStamps = _spamTracker.GetOrAdd(player, new List<DateTime>());

            lock (userStamps)
            {
                userStamps.RemoveAll(t => (now - t).TotalSeconds > timeWindowSeconds);
                userStamps.Add(now);

                if (userStamps.Count >= maxMessages)
                {
                    userStamps.Clear();
                    return true;
                }
            }
            return false;
        }

        private static string DeterminePrimaryEnforcement(string player)
        {
            string trueXuid = ResolveXuid(player);
            PlayerEnforcementProfile profile = ProfileManager.LoadProfile(trueXuid);

            int recentStrikes = profile.History.Count;

            // Safely parse strictly from the dynamic config. If the config is empty or invalid, 
            // it defaults to int.MaxValue so it doesn't accidentally trigger a false ban.
            int primaryThresh = int.TryParse(AMConfHelper.Instance.PrimaryEnforcementThreshold, out int pVal) ? pVal : int.MaxValue;
            int secondaryThresh = int.TryParse(AMConfHelper.Instance.SecondaryEnforcementThreshold, out int sVal) ? sVal : int.MaxValue;

            if (recentStrikes >= secondaryThresh)
                return AMConfHelper.Instance.SecondaryEnforcementType;

            if (recentStrikes >= primaryThresh)
                return AMConfHelper.Instance.PrimaryEnforcementType;

            return "Warning";
        }

        private static void ExecuteEnforcement(string player, string reason, string actionType, string offendingMessage, Process server, Action<string> uiConsoleCallback)
        {
            string trueXuid = ResolveXuid(player);
            PlayerEnforcementProfile profile = ProfileManager.LoadProfile(trueXuid);
            string commandToInject = string.Empty;

            // Ensure the gamertag populates cleanly on new file generation
            if (string.IsNullOrEmpty(profile.Gamertag))
            {
                profile.Gamertag = player;
            }

            if (string.IsNullOrEmpty(profile.Gamertag))
            {
                profile.Gamertag = player;
            }

            if (actionType.StartsWith("Mute"))
            {
                profile.ActiveSanctionType = "Mute";
                profile.SanctionExpiration = actionType.Contains("10m") ? DateTime.Now.AddMinutes(10) : DateTime.Now.AddMinutes(5);
                commandToInject = MessageProvider.GetMuteCommand(player, actionType.Contains("10m") ? "10 minutes" : "5 minutes");
            }
            else if (actionType.StartsWith("Temp-Ban"))
            {
                profile.ActiveSanctionType = "Ban";
                profile.SanctionExpiration = DateTime.Now.AddHours(24);
                commandToInject = MessageProvider.GetBanCommand(player, "24 Hours", reason);
            }
            else if (actionType == "Ban")
            {
                profile.ActiveSanctionType = "Ban";
                profile.SanctionExpiration = null;
                commandToInject = MessageProvider.GetBanCommand(player, "Permanent", reason);
            }
            else if (actionType == "Kick")
            {
                commandToInject = MessageProvider.GetKickCommand(player, reason);
            }
            else
            {
                commandToInject = MessageProvider.GetWarningCommand(player, reason);
            }

            profile.History.Add(new EnforcementRecord
            {
                Date = DateTime.Now,
                ActionTaken = actionType,
                Reason = reason,
                MessageContext = offendingMessage
            });

            ProfileManager.SaveProfile(profile);

            uiConsoleCallback?.Invoke($"[AutoMod Action] Executed {actionType} on {player}. Reason: {reason}");

            if (server != null && !server.HasExited && !string.IsNullOrEmpty(commandToInject))
            {
                string[] commands = commandToInject.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string cmd in commands)
                {
                    server.StandardInput.WriteLine(cmd);
                }
            }
        }

        private static string ResolveXuid(string gamertag)
        {
            string datPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bds_players.dat");
            if (!File.Exists(datPath)) return gamertag;

            try
            {
                foreach (string line in File.ReadLines(datPath))
                {
                    // Match the exact XUID||Gamertag format
                    if (line.EndsWith($"||{gamertag}", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] parts = line.Split(new[] { "||" }, StringSplitOptions.None);
                        if (parts.Length == 2)
                        {
                            return parts[0].Trim(); // Returns the 16-digit XUID
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[XUID Resolver Error]: {ex.Message}");
            }

            return gamertag;
        }

        // Tracks recent join evaluations to prevent double-firing while the client disconnects
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, DateTime> _joinCooldownTracker
            = new System.Collections.Concurrent.ConcurrentDictionary<string, DateTime>();

        /// <summary>
        /// Evaluates a player's enforcement profile the moment they connect. 
        /// Waits briefly so the player sees the ban reason, and enforces a cooldown to prevent spam loops.
        /// </summary>
        public static void EnforceProfileOnJoin(string gamertag, string xuid, Process activeServerProcess)
        {
            string trueXuid = string.IsNullOrEmpty(xuid) ? ResolveXuid(gamertag) : xuid;

            // Cooldown Guard: If we evaluated this XUID in the last 10 seconds, ignore duplicate log triggers
            var now = DateTime.Now;
            if (_joinCooldownTracker.TryGetValue(trueXuid, out var lastChecked) && (now - lastChecked).TotalSeconds < 10)
            {
                return;
            }
            _joinCooldownTracker[trueXuid] = now;

            Task.Run(async () =>
            {
                if (activeServerProcess == null || activeServerProcess.HasExited) return;

                // Wait 1.5 seconds so the client finishes handshaking and can actually receive the kick packet
                await Task.Delay(1500);

                PlayerEnforcementProfile profile = ProfileManager.LoadProfile(trueXuid);

                // Ensure gamertag is synced
                if (string.IsNullOrEmpty(profile.Gamertag))
                {
                    profile.Gamertag = gamertag;
                    ProfileManager.SaveProfile(profile);
                }

                if (profile.ActiveSanctionType.ToLower() == "inactive") return;

                bool isExpired = profile.SanctionExpiration.HasValue && DateTime.Now >= profile.SanctionExpiration.Value;

                if (isExpired)
                {
                    // Sanction expired while offline. Automatically pardon.
                    string previousSanction = profile.ActiveSanctionType;
                    profile.ActiveSanctionType = "inactive";
                    profile.SanctionExpiration = null;

                    profile.History.Add(new EnforcementRecord
                    {
                        Date = DateTime.Now,
                        ActionTaken = "Auto-Pardon",
                        Reason = $"{previousSanction} time duration expired."
                    });

                    ProfileManager.SaveProfile(profile);

                    // Strip the tags from the server
                    activeServerProcess.StandardInput.WriteLine($"tag \"{gamertag}\" remove automod_mute");
                    activeServerProcess.StandardInput.WriteLine($"tag \"{gamertag}\" remove automod_ban");
                }
                else
                {
                    // Sanction is still active
                    if (profile.ActiveSanctionType.ToLower() == "ban")
                    {
                        string durationStr = profile.SanctionExpiration.HasValue
                            ? $"Ends {profile.SanctionExpiration.Value:MM/dd HH:mm}"
                            : "Permanent";

                        string banKick = MessageProvider.GetBanCommand(gamertag, durationStr, "Active Ban Enforced");

                        string[] commands = banKick.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string cmd in commands)
                        {
                            activeServerProcess.StandardInput.WriteLine(cmd);
                        }
                    }
                    else if (profile.ActiveSanctionType.ToLower() == "mute")
                    {
                        activeServerProcess.StandardInput.WriteLine($"tag \"{gamertag}\" add automod_mute");
                    }
                }
            });
        }

    }
}