using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BDSUtil
{
    public class AppSettings
    {
        private static AppSettings _instance;
        private static readonly object _lock = new object();
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        // FIXED: Infinite storage backing indexer tracking array handles any settings dynamically
        private readonly Dictionary<string, string> _settingsRegistry = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // --- RUNTIME ENVIRONMENT FLAGS ---
        public bool IsWinRarInstalled { get; private set; } = false;

        private AppSettings() { }

        public static AppSettings Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new AppSettings();
                        _instance.RunStartupCheck();
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// DYNAMIC INDEXER METHOD: Pulls and casts any preference parameter securely on the fly.
        /// If the item isn't in config.json yet, it caches the default value automatically!
        /// </summary>
        public T GetSetting<T>(string settingKey, T defaultFallbackValue = default(T))
        {
            lock (_lock)
            {
                if (!_settingsRegistry.TryGetValue(settingKey, out string rawValue))
                {
                    // Cache the default fallback option on the fly so it saves cleanly later
                    SetSetting(settingKey, defaultFallbackValue);
                    return defaultFallbackValue;
                }

                try
                {
                    // Direct string conversion shortcut pass
                    if (typeof(T) == typeof(string))
                    {
                        return (T)(object)rawValue;
                    }

                    // Native framework type parser conversion matrix
                    return (T)Convert.ChangeType(rawValue, typeof(T));
                }
                catch
                {
                    return defaultFallbackValue;
                }
            }
        }

        /// <summary>
        /// DYNAMIC SETTER METHOD: Modifies or initializes configuration preferences in the memory registry.
        /// </summary>
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
                    _settingsRegistry[settingKey] = boolVal.ToString().ToLower(); // Keeps standard JSON boolean formats
                }
                else
                {
                    _settingsRegistry[settingKey] = inputVal.ToString();
                }
            }
        }

        // --- EXPLICIT COMPATIBILITY WRAPPERS FOR YOUR EXISTING FORMS ---
        // (Keeps your main forms and path buttons compiling without modifying their current code)

        public string ServerExecutablePath
        {
            get => GetSetting("ServerExecutablePath", string.Empty);
            set => SetSetting("ServerExecutablePath", value);
        }

        public int CompressionMode
        {
            get => GetSetting("CompressionMode", 0);
            set => SetSetting("CompressionMode", value);
        }

        public bool HasAcceptedNetworkRisks
        {
            get => GetSetting("HasAcceptedNetworkRisks", false);
            set => SetSetting("HasAcceptedNetworkRisks", value);
        }

        // NEW WORKSPACE PROPERTY: Automatically integrated for your newly updated Networking Control Switch
        public bool EnableUPnP
        {
            get => GetSetting("EnableUPnP", false);
            set => SetSetting("EnableUPnP", value);
        }

        // --- END OF COMPATIBILITY LAYER ---

        public void RunStartupCheck()
        {
            string defaultPath = @"C:\Program Files\WinRAR\WinRAR.exe";
            if (File.Exists(defaultPath))
            {
                IsWinRarInstalled = true;
                return;
            }

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe"))
                {
                    if (key != null)
                    {
                        IsWinRarInstalled = true;
                        return;
                    }
                }
            }
            catch
            {
                IsWinRarInstalled = false;
            }
        }

        /// <summary>
        /// MODERNIZED NATIVE PARSER ENGINE: Reads any arbitrary key/value pair 
        /// completely dependency-free without string index positioning hardcoding.
        /// </summary>
        public static async Task LoadSettingsAsync()
        {
            AppSettings localSettings = new AppSettings();
            bool configFileExists = File.Exists(ConfigPath);

            if (configFileExists)
            {
                try
                {
                    using (StreamReader reader = new StreamReader(ConfigPath))
                    {
                        string line;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            string trimmed = line.Trim();
                            if (string.IsNullOrEmpty(trimmed) || trimmed == "{" || trimmed == "}") continue;

                            int colonIdx = trimmed.IndexOf(':');
                            if (colonIdx > 0)
                            {
                                // Clean up the key string segment bounds
                                string rawKey = trimmed.Substring(0, colonIdx).Trim(new char[] { ' ', '"', ',' });

                                // Clean up the value string segment bounds
                                string rawValue = trimmed.Substring(colonIdx + 1).Trim(new char[] { ' ', '"', ',' });

                                if (!string.IsNullOrEmpty(rawKey))
                                {
                                    // Unescape string path delimiters back to normal slashes
                                    string cleanValue = rawValue.Replace("\\\\", "\\");
                                    localSettings._settingsRegistry[rawKey] = cleanValue;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    localSettings = new AppSettings();
                }
            }

            lock (_lock)
            {
                _instance = localSettings;
                _instance.RunStartupCheck();

                if (configFileExists)
                {
                    return;
                }

                // AUTOFILL DETECTION ENGINE (ONLY FIRES ON FRESH BLANK BOOTS)
                if (string.IsNullOrWhiteSpace(_instance.ServerExecutablePath))
                {
                    string localServerCheck = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bedrock_server.exe");
                    if (File.Exists(localServerCheck))
                    {
                        _instance.ServerExecutablePath = AppDomain.CurrentDomain.BaseDirectory;
                    }
                }
            }

            if (!configFileExists)
            {
                try
                {
                    await AppSettings.Instance.SaveSettingsAsync();
                }
                catch { }
            }
        }

        /// <summary>
        /// DYNAMIC EMISSION GENERATOR: Loops your internal settings cache list 
        /// and converts it to a standard, beautifully aligned JSON text profile file.
        /// </summary>
        public async Task SaveSettingsAsync()
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
                        // Escape backslashes inside value parameters securely for standard text formatting
                        string escapedValue = settingPair.Value.Replace("\\", "\\\\");

                        // Format structural indentation layers matching standard layouts
                        jsonBuilder.AppendFormat("  \"{0}\": \"{1}\"", settingPair.Key, escapedValue);

                        // Keep standard JSON trailing commas trailing down until the final line element loop
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

                using (FileStream fs = new FileStream(ConfigPath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (StreamWriter writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    await writer.WriteAsync(jsonBuilder.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Failed to write dynamic manual text configuration changes to storage bounds.", ex);
            }
        }
    }
}
