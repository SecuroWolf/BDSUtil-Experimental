using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BDSUtil.Helpers.AutoMod
{
    public class AMConfHelper
    {
        private static AMConfHelper _instance;
        private static readonly object _lock = new object();
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "conf_am.json");

        // Storage backing indexer for AutoMod settings
        private readonly Dictionary<string, string> _settingsRegistry = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private AMConfHelper() { }

        public static AMConfHelper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AMConfHelper();
                        _instance.LoadSettingsSync(); // Completely sidesteps the async thread pool deadlock
                    }
                    return _instance;
                }
            }
        }

        public T GetSetting<T>(string settingKey, T defaultFallbackValue = default(T))
        {
            lock (_lock)
            {
                if (!_settingsRegistry.TryGetValue(settingKey, out string rawValue))
                {
                    SetSetting(settingKey, defaultFallbackValue);
                    return defaultFallbackValue;
                }

                try
                {
                    if (typeof(T) == typeof(string))
                        return (T)(object)rawValue;

                    return (T)Convert.ChangeType(rawValue, typeof(T));
                }
                catch
                {
                    return defaultFallbackValue;
                }
            }
        }

        public void SetSetting<T>(string settingKey, T inputVal)
        {
            lock (_lock)
            {
                if (inputVal == null)
                {
                    _settingsRegistry[settingKey] = string.Empty;
                    return;
                }

                if (inputVal is bool boolVal)
                {
                    _settingsRegistry[settingKey] = boolVal.ToString().ToLower();
                }
                else
                {
                    _settingsRegistry[settingKey] = inputVal.ToString();
                }
            }
        }

        // --- AUTOMOD CONFIG VARIABLES ---

        public bool EnableChatEnforcement
        {
            get => GetSetting("EnableChatEnforcement", false);
            set => SetSetting("EnableChatEnforcement", value);
        }

        public string PrimaryEnforcementType
        {
            get => GetSetting("PrimaryEnforcementType", "Kick");
            set => SetSetting("PrimaryEnforcementType", value);
        }

        public string PrimaryEnforcementThreshold
        {
            get => GetSetting("PrimaryEnforcementThreshold", string.Empty);
            set => SetSetting("PrimaryEnforcementThreshold", value);
        }

        public string SecondaryEnforcementType
        {
            get => GetSetting("SecondaryEnforcementType", "Temp-Ban(24h)");
            set => SetSetting("SecondaryEnforcementType", value);
        }

        public string SecondaryEnforcementThreshold
        {
            get => GetSetting("SecondaryEnforcementThreshold", string.Empty);
            set => SetSetting("SecondaryEnforcementThreshold", value);
        }

        public bool AntiSpamEnabled
        {
            get => GetSetting("AntiSpamEnabled", false);
            set => SetSetting("AntiSpamEnabled", value);
        }

        public string AntiSpamMessages
        {
            get => GetSetting("AntiSpamMessages", "5");
            set => SetSetting("AntiSpamMessages", value);
        }

        public string AntiSpamTime
        {
            get => GetSetting("AntiSpamTime", "3");
            set => SetSetting("AntiSpamTime", value);
        }

        public string AntiSpamEnforcementType
        {
            get => GetSetting("AntiSpamEnforcementType", "Mute(5m)");
            set => SetSetting("AntiSpamEnforcementType", value);
        }

        public string ChatStrikeReset
        {
            get => GetSetting("ChatStrikeReset", "12 Hours");
            set => SetSetting("ChatStrikeReset", value);
        }

        public string AntiSpamStrikeReset
        {
            get => GetSetting("AntiSpamStrikeReset", "1 Hour");
            set => SetSetting("AntiSpamStrikeReset", value);
        }

        // --- END AUTOMOD CONFIG VARIABLES ---

        private void LoadSettingsSync()
        {
            bool configFileExists = File.Exists(ConfigPath);

            if (configFileExists)
            {
                try
                {
                    // Read synchronously. It is perfectly fast enough for a small config on the UI thread.
                    string[] lines = File.ReadAllLines(ConfigPath);
                    foreach (string rawLine in lines)
                    {
                        string trimmed = rawLine.Trim();
                        if (string.IsNullOrEmpty(trimmed) || trimmed == "{" || trimmed == "}") continue;

                        int colonIdx = trimmed.IndexOf(':');
                        if (colonIdx > 0)
                        {
                            string rawKey = trimmed.Substring(0, colonIdx).Trim(' ', '"', ',');
                            string rawValue = trimmed.Substring(colonIdx + 1).Trim(' ', '"', ',');

                            if (!string.IsNullOrEmpty(rawKey))
                            {
                                string cleanValue = rawValue.Replace("\\\\", "\\");
                                _settingsRegistry[rawKey] = cleanValue;
                            }
                        }
                    }
                }
                catch { }
            }
            else
            {
                // Generate the default file if it doesn't exist yet
                try { SaveSettings(); } catch { }
            }
        }

        public void SaveSettings()
        {
            try
            {
                StringBuilder jsonBuilder = new StringBuilder();
                jsonBuilder.AppendLine("{");

                lock (_lock)
                {
                    int trackerIdx = 0;
                    foreach (KeyValuePair<string, string> settingPair in _settingsRegistry)
                    {
                        string escapedValue = settingPair.Value.Replace("\\", "\\\\");
                        jsonBuilder.AppendFormat("  \"{0}\": \"{1}\"", settingPair.Key, escapedValue);

                        if (trackerIdx < _settingsRegistry.Count - 1)
                        {
                            jsonBuilder.AppendLine(",");
                        }
                        else
                        {
                            jsonBuilder.AppendLine();
                        }
                        trackerIdx++;
                    }
                }

                jsonBuilder.Append("}");

                // Synchronously writes the file instantly
                File.WriteAllText(ConfigPath, jsonBuilder.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AMConfHelper Save Violation]: {ex.Message}");
            }
        }
    }
}