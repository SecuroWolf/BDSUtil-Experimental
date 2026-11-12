using BDSUtil.Configuration;
using BDSUtil.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BDSUtil
{
    /// <summary>
    /// Thread-safe singleton manager for the unified modular config.json file.
    /// </summary>
    public sealed class AppSettings
    {
        private static AppSettings _instance;
        private static readonly object _instanceLock = new object();

        private readonly object _configLock = new object();
        private readonly string _baseDirectory;
        private readonly string _configPath;
        private AppConfig _config = new AppConfig();

        // --- RUNTIME ENVIRONMENT FLAGS ---
        public bool IsWinRarInstalled { get; private set; }
        public string WinRarExecutablePath { get; private set; } = string.Empty;

        public event Action<AppConfig> SettingsSaved;

        private AppSettings(string baseDirectory)
        {
            _baseDirectory = string.IsNullOrWhiteSpace(baseDirectory)
                ? AppContext.BaseDirectory
                : baseDirectory;
            _configPath = Path.Combine(_baseDirectory, "config.json");
        }

        public static AppSettings Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        var created = new AppSettings(AppContext.BaseDirectory);
                        created.LoadInternal();
                        created.RunStartupCheck();
                        _instance = created;
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// Creates an isolated AppSettings instance rooted at a specific directory (useful for testing or multi-profile setups).
        /// </summary>
        public static AppSettings LoadFromDirectory(string baseDirectory)
        {
            var settings = new AppSettings(baseDirectory);
            settings.LoadInternal();
            settings.RunStartupCheck();
            return settings;
        }

        public string ConfigFilePath => _configPath;

        // --- MODULAR SECTION ACCESSORS ---
        public AppConfig Config => _config;
        public GeneralSettings General => _config.General;
        public NetworkingSettings Networking => _config.Networking;
        public RemoteApiSettings RemoteApi => _config.RemoteApi;
        public AutoModSettings AutoMod => _config.AutoMod;
        public UtilPackSettings UtilPack => _config.UtilPack;

        /// <summary>
        /// Loads or migrates config.json (and any legacy conf_am.json) synchronously at application startup.
        /// </summary>
        public static void LoadSettings()
        {
            var localSettings = new AppSettings(AppContext.BaseDirectory);
            localSettings.LoadInternal();
            localSettings.RunStartupCheck();

            lock (_instanceLock)
            {
                _instance = localSettings;
            }
        }

        public static Task LoadSettingsAsync()
        {
            LoadSettings();
            return Task.CompletedTask;
        }

        public void Reload()
        {
            lock (_configLock)
            {
                LoadInternal();
            }
        }

        /// <summary>
        /// Atomically applies one or more mutations to the configuration and persists changes to disk.
        /// </summary>
        public void Update(Action<AppConfig> applyChanges)
        {
            if (applyChanges == null) throw new ArgumentNullException(nameof(applyChanges));

            lock (_configLock)
            {
                applyChanges(_config);
                _config.Normalize();
                SaveInternal();
            }

            SettingsSaved?.Invoke(_config);
        }

        public void Save()
        {
            lock (_configLock)
            {
                _config.Normalize();
                SaveInternal();
            }

            SettingsSaved?.Invoke(_config);
        }

        public Task SaveAsync()
        {
            Save();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Compatibility alias for <see cref="Save"/>.
        /// </summary>
        public void SaveSettings() => Save();

        /// <summary>
        /// Compatibility alias for <see cref="SaveAsync"/>.
        /// </summary>
        public Task SaveSettingsAsync() => SaveAsync();

        public void ResetToDefaults()
        {
            lock (_configLock)
            {
                _config = new AppConfig();
                _config.Normalize();
                SaveInternal();
            }

            SettingsSaved?.Invoke(_config);
        }

        /// <summary>
        /// Retrieves a typed modular config object stored under <see cref="AppConfig.Modules"/>,
        /// initializing and registering its default state if not yet present.
        /// </summary>
        public T GetModule<T>(string moduleKey, Func<T> defaultFactory = null) where T : class
        {
            if (string.IsNullOrWhiteSpace(moduleKey))
                throw new ArgumentException("Module key cannot be empty.", nameof(moduleKey));

            lock (_configLock)
            {
                if (_config.Modules.TryGetValue(moduleKey, out JsonElement element) &&
                    element.ValueKind != JsonValueKind.Null &&
                    element.ValueKind != JsonValueKind.Undefined)
                {
                    try
                    {
                        T deserialized = element.Deserialize<T>(JsonHelper.IndentedOptions);
                        if (deserialized != null) return deserialized;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"[AppSettings Module Read Error ({moduleKey})]: {ex.Message}");
                    }
                }

                T fallback = defaultFactory != null ? defaultFactory() : Activator.CreateInstance<T>();
                _config.Modules[moduleKey] = JsonSerializer.SerializeToElement(fallback, JsonHelper.IndentedOptions);
                return fallback;
            }
        }

        /// <summary>
        /// Stores or replaces a typed modular config object under <see cref="AppConfig.Modules"/> without automatically saving.
        /// </summary>
        public void SetModule<T>(string moduleKey, T moduleConfig)
        {
            if (string.IsNullOrWhiteSpace(moduleKey))
                throw new ArgumentException("Module key cannot be empty.", nameof(moduleKey));

            lock (_configLock)
            {
                _config.Modules[moduleKey] = JsonSerializer.SerializeToElement(moduleConfig, JsonHelper.IndentedOptions);
            }
        }

        /// <summary>
        /// Reads, mutates, stores, and immediately saves a modular config section under <see cref="AppConfig.Modules"/>.
        /// </summary>
        public void UpdateModule<T>(string moduleKey, Action<T> updater, Func<T> defaultFactory = null) where T : class
        {
            if (updater == null) throw new ArgumentNullException(nameof(updater));

            lock (_configLock)
            {
                T module = GetModule(moduleKey, defaultFactory);
                updater(module);
                SetModule(moduleKey, module);
                SaveInternal();
            }

            SettingsSaved?.Invoke(_config);
        }

        /// <summary>
        /// Reads a strongly-typed dynamic key from the extensible Modules["Custom"] dictionary.
        /// Automatically registers the fallback default if the key does not exist yet.
        /// </summary>
        public T GetSetting<T>(string settingKey, T defaultFallbackValue = default)
        {
            if (string.IsNullOrWhiteSpace(settingKey)) return defaultFallbackValue;

            lock (_configLock)
            {
                var customMap = GetCustomDictionaryLocked();
                if (!customMap.TryGetValue(settingKey, out JsonElement element))
                {
                    SetSetting(settingKey, defaultFallbackValue);
                    return defaultFallbackValue;
                }

                try
                {
                    if (typeof(T) == typeof(string))
                        return (T)(object)JsonHelper.GetStringValue(element, defaultFallbackValue as string ?? string.Empty);

                    if (typeof(T) == typeof(bool))
                        return (T)(object)JsonHelper.GetBoolValue(element, defaultFallbackValue is bool b && b);

                    if (typeof(T) == typeof(int))
                        return (T)(object)JsonHelper.GetIntValue(element, defaultFallbackValue is int i ? i : 0);

                    T deserialized = element.Deserialize<T>(JsonHelper.IndentedOptions);
                    return deserialized != null ? deserialized : defaultFallbackValue;
                }
                catch
                {
                    return defaultFallbackValue;
                }
            }
        }

        /// <summary>
        /// Writes a strongly-typed dynamic key into the extensible Modules["Custom"] dictionary.
        /// </summary>
        public void SetSetting<T>(string settingKey, T inputVal)
        {
            if (string.IsNullOrWhiteSpace(settingKey)) return;

            lock (_configLock)
            {
                var customMap = GetCustomDictionaryLocked();
                customMap[settingKey] = JsonSerializer.SerializeToElement(inputVal, JsonHelper.CompactOptions);
                _config.Modules["Custom"] = JsonSerializer.SerializeToElement(customMap, JsonHelper.IndentedOptions);
            }
        }

        private Dictionary<string, JsonElement> GetCustomDictionaryLocked()
        {
            var result = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);

            if (_config.Modules.TryGetValue("Custom", out JsonElement existing) &&
                existing.ValueKind == JsonValueKind.Object)
            {
                foreach (JsonProperty prop in existing.EnumerateObject())
                {
                    result[prop.Name] = prop.Value.Clone();
                }
            }

            return result;
        }

        public void RunStartupCheck()
        {
            string defaultPath = @"C:\Program Files\WinRAR\WinRAR.exe";
            if (File.Exists(defaultPath))
            {
                IsWinRarInstalled = true;
                WinRarExecutablePath = defaultPath;
                return;
            }

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe"))
                {
                    if (key != null)
                    {
                        string regPath = key.GetValue(string.Empty) as string;
                        if (!string.IsNullOrWhiteSpace(regPath) && File.Exists(regPath))
                        {
                            IsWinRarInstalled = true;
                            WinRarExecutablePath = regPath;
                            return;
                        }

                        IsWinRarInstalled = true;
                        WinRarExecutablePath = defaultPath;
                        return;
                    }
                }
            }
            catch
            {
                IsWinRarInstalled = false;
                WinRarExecutablePath = string.Empty;
            }
        }

        private void LoadInternal()
        {
            bool configFileExists = File.Exists(_configPath);
            bool needsSave = !configFileExists;

            AppConfig loaded = new AppConfig();

            if (configFileExists)
            {
                try
                {
                    string rawJson = File.ReadAllText(_configPath);
                    if (!string.IsNullOrWhiteSpace(rawJson))
                    {
                        using (JsonDocument doc = JsonDocument.Parse(rawJson, JsonHelper.DocumentOptions))
                        {
                            if (LegacyConfigMigrator.IsUnifiedV2Config(doc.RootElement))
                            {
                                loaded = doc.RootElement.Deserialize<AppConfig>(JsonHelper.IndentedOptions) ?? new AppConfig();
                            }
                            else
                            {
                                loaded = LegacyConfigMigrator.MigrateFlatConfig(doc.RootElement);
                                needsSave = true;
                            }
                        }
                    }
                    else
                    {
                        needsSave = true;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[AppSettings Load Error]: {ex.Message}");
                    TryBackupCorruptConfig(_configPath);
                    loaded = new AppConfig();
                    needsSave = true;
                }
            }
            else
            {
                // Auto-detect local bedrock_server.exe on fresh blank boots.
                string localServerCheck = Path.Combine(_baseDirectory, "bedrock_server.exe");
                if (File.Exists(localServerCheck))
                {
                    loaded.General.ServerExecutablePath = _baseDirectory;
                }
            }

            // Merge legacy AutoMod configuration file if present and not yet unified into v2.
            var migratedLegacyFiles = new List<string>();
            string[] legacyAutoModCandidates =
            {
                Path.Combine(_baseDirectory, "conf_am.json"),
                Path.Combine(_baseDirectory, "am_conf.json")
            };

            foreach (string legacyAmPath in legacyAutoModCandidates)
            {
                if (!File.Exists(legacyAmPath)) continue;

                try
                {
                    string amRaw = File.ReadAllText(legacyAmPath);
                    if (!string.IsNullOrWhiteSpace(amRaw))
                    {
                        using (JsonDocument amDoc = JsonDocument.Parse(amRaw, JsonHelper.DocumentOptions))
                        {
                            LegacyConfigMigrator.MergeLegacyAutoModConfig(amDoc.RootElement, loaded.AutoMod);
                        }
                    }
                    migratedLegacyFiles.Add(legacyAmPath);
                    needsSave = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[AppSettings Legacy AutoMod Migration Error]: {ex.Message}");
                }
            }

            loaded.Normalize();
            _config = loaded;

            if (needsSave)
            {
                try
                {
                    SaveInternal();

                    // Only retire legacy AutoMod files after config.json has been atomically written.
                    foreach (string legacyFile in migratedLegacyFiles)
                    {
                        try { File.Delete(legacyFile); } catch { }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[AppSettings Initial Save Error]: {ex.Message}");
                }
            }
        }

        private void SaveInternal()
        {
            string json = JsonSerializer.Serialize(_config, JsonHelper.IndentedOptions);
            JsonHelper.WriteTextAtomically(_configPath, json);
        }

        private static void TryBackupCorruptConfig(string configPath)
        {
            try
            {
                string backupPath = $"{configPath}.corrupt_{DateTime.Now:yyyyMMdd_HHmmss}";
                File.Copy(configPath, backupPath, overwrite: true);
            }
            catch { }
        }
    }
}
