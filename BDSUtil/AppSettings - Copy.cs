using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace BDSUtil
{
    public class AppSettings
    {
        private static AppSettings _instance;
        private static readonly object _lock = new object();
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

        // --- PERSISTENT DATA FIELDS ---
        public string ServerExecutablePath { get; set; } = string.Empty;
        public int CompressionMode { get; set; } = 0; // 0 = WinRAR RAR, 1 = WinRAR ZIP, 2 = Windows Native ZIP
        public bool HasAcceptedNetworkRisks { get; set; } = false; // FIXED Persistent Authorization tracking gate flag

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
        /// Reads the local config file using pure native text stream parsing.
        /// Completely avoids JavaScriptSerializer reflection type collisions!
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
                        string rawText = await reader.ReadToEndAsync();

                        if (!string.IsNullOrWhiteSpace(rawText))
                        {
                            // --- EXPLICIT NATIVE TEXT PARSING ENGINE ---
                            // Extract ServerExecutablePath parameter cleanly
                            int pathKeyIndex = rawText.IndexOf("\"ServerExecutablePath\"");
                            if (pathKeyIndex != -1)
                            {
                                int startQuote = rawText.IndexOf("\"", pathKeyIndex + 22);
                                if (startQuote != -1)
                                {
                                    int endQuote = rawText.IndexOf("\"", startQuote + 1);
                                    if (endQuote != -1)
                                    {
                                        string extractedPath = rawText.Substring(startQuote + 1, endQuote - startQuote - 1);
                                        // Unescape JSON forward slash path characters cleanly
                                        localSettings.ServerExecutablePath = extractedPath.Replace("\\\\", "\\");
                                    }
                                }
                            }

                            // Extract CompressionMode parameter cleanly
                            int modeKeyIndex = rawText.IndexOf("\"CompressionMode\"");
                            if (modeKeyIndex != -1)
                            {
                                int colonIndex = rawText.IndexOf(":", modeKeyIndex);
                                if (colonIndex != -1)
                                {
                                    string searchBlock = rawText.Substring(colonIndex + 1).Trim(new char[] { ' ', '}', ',', '\r', '\n' });
                                    int nextComma = searchBlock.IndexOf(",");
                                    if (nextComma != -1) searchBlock = searchBlock.Substring(0, nextComma).Trim();
                                    int nextBrace = searchBlock.IndexOf("}");
                                    if (nextBrace != -1) searchBlock = searchBlock.Substring(0, nextBrace).Trim();

                                    if (int.TryParse(searchBlock, out int parsedMode))
                                    {
                                        localSettings.CompressionMode = parsedMode;
                                    }
                                }
                            }

                            // FIXED EXPLICIT NATIVE PARSING ENGINE: Read HasAcceptedNetworkRisks safely from the JSON file stream
                            int riskKeyIndex = rawText.IndexOf("\"HasAcceptedNetworkRisks\"");
                            if (riskKeyIndex != -1)
                            {
                                int colonIndex = rawText.IndexOf(":", riskKeyIndex);
                                if (colonIndex != -1)
                                {
                                    string searchBlock = rawText.Substring(colonIndex + 1).Trim(new char[] { ' ', '}', ',', '\r', '\n' });
                                    int nextComma = searchBlock.IndexOf(",");
                                    if (nextComma != -1) searchBlock = searchBlock.Substring(0, nextComma).Trim();
                                    int nextBrace = searchBlock.IndexOf("}");
                                    if (nextBrace != -1) searchBlock = searchBlock.Substring(0, nextBrace).Trim();

                                    if (bool.TryParse(searchBlock, out bool parsedRisk))
                                    {
                                        localSettings.HasAcceptedNetworkRisks = parsedRisk;
                                    }
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

                // If config.json already exists and was read successfully, do not overwrite fields
                if (configFileExists)
                {
                    return;
                }

                // --- AUTOFILL DETECTION ENGINE (ONLY FIRES ON FRESH BLANK BOOTS) ---
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
        /// Emits a flat structural JSON text stream straight to disk byte zero.
        /// </summary>
        public async Task SaveSettingsAsync()
        {
            try
            {
                // Escape backslashes cleanly so standard text editors read the directories perfectly
                string escapedPath = ServerExecutablePath.Replace("\\", "\\\\");

                // FIXED FORMAT PATTERN: Formulate the raw JSON layout structure explicitly manually, handling string serialization strings
                string flatJsonText = string.Format(
                    "{{\r\n  \"ServerExecutablePath\": \"{0}\",\r\n  \"CompressionMode\": {1},\r\n  \"HasAcceptedNetworkRisks\": {2}\r\n}}",
                    escapedPath,
                    CompressionMode,
                    HasAcceptedNetworkRisks.ToString().ToLower() // Forces standard JSON boolean lowercase strings (true/false)
                );

                using (FileStream fs = new FileStream(ConfigPath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    await writer.WriteAsync(flatJsonText);
                }
            }
            catch (Exception ex)
            {
                throw new IOException("Failed to write manual text configuration changes to storage bounds.", ex);
            }
        }
    }
}
