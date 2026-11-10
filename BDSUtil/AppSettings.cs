using BDSUtil.Helpers;
using Microsoft.Win32;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BDSUtil
{
    public class AppSettings
    {
        private static AppSettings _instance;
        private static readonly object _lock = new object();
        private static readonly string AppBaseDirectory = Path.GetDirectoryName(Environment.ProcessPath) ?? AppDomain.CurrentDomain.BaseDirectory;
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

        /// <summary>
        /// CONFIG VARIABLES: These are the actual settings that will be saved to disk and loaded from disk.
        /// </summary>

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

        public bool EnableUPnP
        {
            get => GetSetting("EnableUPnP", false);
            set => SetSetting("EnableUPnP", value);
        }

        public bool AutoModEnabled
        {
            get => GetSetting("AutoModEnabled", false);
            set => SetSetting("AutoModEnabled", value);
        }

        public bool WorldEditorEnabled
        {
            get => GetSetting("WorldEditorEnabled", false);
            set => SetSetting("WorldEditorEnabled", value);
        }

        public bool ExperimentsWarned
        {
            get => GetSetting("ExperimentsWarned", false);
            set => SetSetting("ExperimentsWarned", value);
        }

        public bool HideTelemetryWarnings
        {
            get => GetSetting("HideTelemetryWarnings", false);
            set => SetSetting("HideTelemetryWarnings", value);
        }

        // --- END OF CONFIG VARIABLES ---


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
        /// NEW Settings Saving Method: Forces the thread pool to execute the serialization routine synchronously to keep writes isolated.
        /// </summary>
        public void SaveSettings()
        {
            try
            {
                // Forces the thread pool to execute the serialization routine synchronously to keep writes isolated
                Task.Run(async () => await SaveSettingsAsync()).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AppSettings Sync Save Violation]: {ex.Message}");
            }
        }

        /// <summary>
        /// OLD Settings Saving Method: Asynchronously writes the current configuration registry to disk in a structured JSON format.
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

    public static class AutoModValidator
    {
        public static void ValidateStateOnDirectoryChange(string newServerRoot, Form parentForm)
        {
            // If AutoMod is already off, we don't need to validate anything
            if (!AppSettings.Instance.AutoModEnabled) return;

            if (string.IsNullOrWhiteSpace(newServerRoot) || !Directory.Exists(newServerRoot))
            {
                DisableAutoMod(parentForm, "The server directory is invalid or missing.");
                return;
            }

            string targetWorldName = "Dedicated Server";
            string serverPropsPath = Path.Combine(newServerRoot, "server.properties");

            if (File.Exists(serverPropsPath))
            {
                try
                {
                    foreach (string rawLine in File.ReadLines(serverPropsPath))
                    {
                        string line = rawLine.Trim();
                        if (line.StartsWith("level-name=", StringComparison.OrdinalIgnoreCase))
                        {
                            string val = line.Substring(11).Trim();
                            if (!string.IsNullOrWhiteSpace(val)) targetWorldName = val;
                            break;
                        }
                    }
                }
                catch { }
            }

            string levelDatPath = Path.Combine(newServerRoot, "worlds", targetWorldName, "level.dat");

            // Scan for prerequisites
            bool isApiActive = LevelDatPatcher.IsExperimentEnabled(levelDatPath, "gametest");
            bool isFolderInstalled = PackManager.IsPackInstalled(newServerRoot, BDSPackInstaller.MONITOR_PACK_FOLDER, isBehaviorPack: true);
            bool isPackEnabled = PackManager.IsPackEnabledInWorld(newServerRoot, targetWorldName, BDSPackInstaller.MONITOR_PACK_UUID, isBehaviorPack: true);

            if (!isApiActive || !isFolderInstalled || !isPackEnabled)
            {
                DisableAutoMod(parentForm,
                    "You have switched server directories.\n\n" +
                    "AutoMod has been temporarily disabled because the target world lacks the required monitor pack or Beta APIs. " +
                    "Navigate back to the AutoMod tab to re-enable and apply the prerequisites to this new environment.");
            }
        }

        private static void DisableAutoMod(Form parentForm, string message)
        {
            AppSettings.Instance.AutoModEnabled = false;
            AppSettings.Instance.SaveSettings();

            if (parentForm is MaterialForm materialForm)
            {
                MaterialMessageBox.Show(
                    materialForm,
                    message,
                    "AutoMod Disabled",
                    MessageBoxButtons.OK,
                    false,
                    MaterialFlexibleForm.ButtonsPosition.Center
                );
            }
        }
    }
}
