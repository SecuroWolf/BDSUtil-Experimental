using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.IO.Compression;

namespace BDSUtil.Helpers
{
    public static class PackManager
    {
        public static bool IsPackInstalled(string serverRootPath, string packFolderName, bool isBehaviorPack)
        {
            if (string.IsNullOrWhiteSpace(serverRootPath) || !Directory.Exists(serverRootPath))
                return false;

            string packCategory = isBehaviorPack ? "behavior_packs" : "resource_packs";
            string devPackCategory = isBehaviorPack ? "development_behavior_packs" : "development_resource_packs";

            string devPackPath = Path.Combine(serverRootPath, devPackCategory, packFolderName);
            string prodPackPath = Path.Combine(serverRootPath, packCategory, packFolderName);

            return File.Exists(Path.Combine(devPackPath, "manifest.json")) ||
                   File.Exists(Path.Combine(prodPackPath, "manifest.json"));
        }

        public static bool IsPackEnabledInWorld(string serverRootPath, string activeWorldName, string uuid, bool isBehaviorPack)
        {
            if (string.IsNullOrWhiteSpace(serverRootPath) || string.IsNullOrWhiteSpace(activeWorldName))
                return false;

            string fileName = isBehaviorPack ? "world_behavior_packs.json" : "world_resource_packs.json";
            string worldPackJsonPath = Path.Combine(serverRootPath, "worlds", activeWorldName, fileName);

            if (!File.Exists(worldPackJsonPath))
                return false;

            try
            {
                string rawJsonContent = File.ReadAllText(worldPackJsonPath);
                return rawJsonContent.Contains(uuid);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PackManager Read Exception]: {ex.Message}");
                return false;
            }
        }

        public static bool EnablePackInWorld(string serverRootPath, string activeWorldName, string uuid, string versionString, bool isBehaviorPack)
        {
            if (string.IsNullOrWhiteSpace(serverRootPath) || string.IsNullOrWhiteSpace(activeWorldName))
                return false;

            try
            {
                string worldFolder = Path.Combine(serverRootPath, "worlds", activeWorldName);
                if (!Directory.Exists(worldFolder))
                {
                    Directory.CreateDirectory(worldFolder);
                }

                string fileName = isBehaviorPack ? "world_behavior_packs.json" : "world_resource_packs.json";
                string worldPacksFile = Path.Combine(worldFolder, fileName);

                // Parse version string into an integer array list (e.g., "3.1.1" -> [3, 1, 1])
                List<int> versionNumbers = new List<int>();
                if (!string.IsNullOrEmpty(versionString))
                {
                    string cleanVersion = versionString.Replace("[", "").Replace("]", "").Trim();
                    string[] parts = cleanVersion.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string part in parts)
                    {
                        if (int.TryParse(part.Trim(), out int num))
                        {
                            versionNumbers.Add(num);
                        }
                    }
                }
                if (versionNumbers.Count == 0) versionNumbers.AddRange(new int[] { 1, 0, 0 });

                // Read existing entries or start a new list block
                List<string> existingEntries = new List<string>();
                if (File.Exists(worldPacksFile))
                {
                    string rawJson = File.ReadAllText(worldPacksFile, Encoding.UTF8);
                    if (rawJson.Contains(uuid))
                    {
                        return true; // Already enabled
                    }

                    // Simple block extractor to preserve any custom entries already in the file
                    int idx = 0;
                    while ((idx = rawJson.IndexOf("\"pack_id\"", idx, StringComparison.OrdinalIgnoreCase)) >= 0)
                    {
                        int startObj = rawJson.LastIndexOf('{', idx);
                        int endObj = rawJson.IndexOf('}', idx);
                        if (startObj >= 0 && endObj > startObj)
                        {
                            existingEntries.Add(rawJson.Substring(startObj, (endObj - startObj) + 1));
                            idx = endObj;
                        }
                        else
                        {
                            idx++;
                        }
                    }
                }

                // Add the new pack object block
                string formattedVersionArray = string.Join(", ", versionNumbers);
                string newEntry =
                    "    {\n" +
                    $"        \"pack_id\" : \"{uuid}\",\n" +
                    $"        \"version\": [{formattedVersionArray}]\n" +
                    "    }";

                existingEntries.Add(newEntry);

                // Rebuild clean, perfectly formatted JSON array content
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("[");
                for (int i = 0; i < existingEntries.Count; i++)
                {
                    sb.Append(existingEntries[i]);
                    if (i < existingEntries.Count - 1)
                    {
                        sb.AppendLine(",");
                    }
                    else
                    {
                        sb.AppendLine();
                    }
                }
                sb.Append("]");

                File.WriteAllText(worldPacksFile, sb.ToString(), Encoding.UTF8);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PackManager Mutation Exception]: {ex.Message}");
                return false;
            }
        }

        public static List<PackManifestInfo> GetAvailablePacks(string serverRootPath, string activeWorldName, bool getBehaviorPacks)
        {
            List<PackManifestInfo> parsedPacks = new List<PackManifestInfo>();

            if (string.IsNullOrWhiteSpace(serverRootPath) || !Directory.Exists(serverRootPath))
                return parsedPacks;

            string standardDir = Path.Combine(serverRootPath, getBehaviorPacks ? "behavior_packs" : "resource_packs");
            string devDir = Path.Combine(serverRootPath, getBehaviorPacks ? "development_behavior_packs" : "development_resource_packs");

            List<string> directoriesToScan = new List<string>();
            if (Directory.Exists(standardDir)) directoriesToScan.AddRange(Directory.GetDirectories(standardDir));
            if (Directory.Exists(devDir)) directoriesToScan.AddRange(Directory.GetDirectories(devDir));

            foreach (string packFolder in directoriesToScan)
            {
                string folderName = new DirectoryInfo(packFolder).Name;

                if (folderName.Equals("bds_monitor", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!folderName.StartsWith("custom_", StringComparison.OrdinalIgnoreCase))
                    continue;

                string manifestPath = Path.Combine(packFolder, "manifest.json");
                if (!File.Exists(manifestPath))
                    continue;

                try
                {
                    string jsonContent = File.ReadAllText(manifestPath);

                    string rawName = GetStrictManifestValue(jsonContent, "\"name\"");
                    string rawDesc = GetStrictManifestValue(jsonContent, "\"description\"");
                    string parsedUuid = GetStrictManifestValue(jsonContent, "\"uuid\"");

                    // Universal version extractor: checks for string version first, then array version
                    string parsedVersion = GetExactVersionValue(jsonContent);

                    string finalName = GetLocalizedPackText(packFolder, rawName, folderName);
                    string finalDesc = GetLocalizedPackText(packFolder, rawDesc, string.Empty);

                    if (!string.IsNullOrEmpty(parsedUuid) && !parsedUuid.StartsWith("{"))
                    {
                        bool isEnabledInWorld = IsPackEnabledInWorld(serverRootPath, activeWorldName, parsedUuid, getBehaviorPacks);

                        parsedPacks.Add(new PackManifestInfo
                        {
                            FolderName = folderName,
                            Name = finalName,
                            Description = finalDesc,
                            Uuid = parsedUuid,
                            Version = string.IsNullOrEmpty(parsedVersion) ? "1, 0, 0" : parsedVersion,
                            IsBehaviorPack = getBehaviorPacks,
                            IsEnabled = isEnabledInWorld
                        });
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[Manifest Parse Error in {folderName}]: {ex.Message}");
                }
            }

            return parsedPacks;
        }

        private static string GetExactVersionValue(string json)
        {
            // Search specifically for "version" as an exact key (ignoring "min_engine_version")
            string targetKey = "\"version\"";
            int searchIdx = 0;

            while (true)
            {
                int keyIdx = json.IndexOf(targetKey, searchIdx, StringComparison.OrdinalIgnoreCase);
                if (keyIdx < 0) break;

                // Ensure it's not preceded by letters (like min_engine_version)
                if (keyIdx > 0 && char.IsLetter(json[keyIdx - 1]))
                {
                    searchIdx = keyIdx + targetKey.Length;
                    continue;
                }

                int colonIdx = json.IndexOf(':', keyIdx);
                if (colonIdx < 0) break;

                // Look ahead to see if it's a string version ("3.1.1") or an array version ([1, 0, 0])
                int nextQuote = json.IndexOf('"', colonIdx);
                int nextBracket = json.IndexOf('[', colonIdx);

                // If bracket comes first (or no quote found), it's an array version
                if (nextBracket > 0 && (nextQuote < 0 || nextBracket < nextQuote))
                {
                    int endBracket = json.IndexOf(']', nextBracket);
                    if (endBracket > nextBracket)
                    {
                        return json.Substring(nextBracket + 1, endBracket - nextBracket - 1).Trim();
                    }
                }
                // Otherwise if quote comes first, it's a string version
                else if (nextQuote > 0)
                {
                    string val = ExtractStringAt(json, keyIdx);
                    if (!string.IsNullOrEmpty(val)) return val;
                }

                searchIdx = keyIdx + targetKey.Length;
            }

            // Fallback to standard array extractor if exact key scan fails
            return ExtractJsonArrayValue(json, "\"version\"");
        }

        private static string GetStrictManifestValue(string json, string targetKey)
        {
            int searchIdx = 0;
            while (true)
            {
                int keyIdx = json.IndexOf(targetKey, searchIdx, StringComparison.OrdinalIgnoreCase);
                if (keyIdx < 0) break;

                // Prevent partial matches like min_engine_version matching "version"
                if (targetKey.Equals("\"version\"", StringComparison.OrdinalIgnoreCase) && keyIdx > 0 && char.IsLetter(json[keyIdx - 1]))
                {
                    searchIdx = keyIdx + targetKey.Length;
                    continue;
                }

                string val = ExtractStringAt(json, keyIdx);

                if (!string.IsNullOrEmpty(val) && !val.StartsWith("{") && !val.Contains("\"type\"") && !val.Contains("\"uuid\""))
                {
                    if (targetKey.Equals("\"uuid\"", StringComparison.OrdinalIgnoreCase))
                    {
                        int modulesIdx = json.IndexOf("\"modules\"", StringComparison.OrdinalIgnoreCase);
                        if (modulesIdx > 0 && keyIdx > modulesIdx)
                        {
                            searchIdx = keyIdx + targetKey.Length;
                            continue;
                        }
                    }
                    return val;
                }

                searchIdx = keyIdx + targetKey.Length;
            }

            return string.Empty;
        }

        private static string ExtractStringAt(string json, int keyIndex)
        {
            int colonIdx = json.IndexOf(':', keyIndex);
            if (colonIdx < 0) return string.Empty;

            int startQuoteIdx = json.IndexOf('"', colonIdx);
            if (startQuoteIdx < 0) return string.Empty;

            int endQuoteIdx = startQuoteIdx + 1;
            while (endQuoteIdx < json.Length)
            {
                if (json[endQuoteIdx] == '"' && json[endQuoteIdx - 1] != '\\')
                {
                    break;
                }
                endQuoteIdx++;
            }

            if (endQuoteIdx < json.Length && endQuoteIdx > startQuoteIdx)
            {
                return json.Substring(startQuoteIdx + 1, endQuoteIdx - startQuoteIdx - 1).Trim();
            }

            return string.Empty;
        }

        private static string GetLocalizedPackText(string packFolderPath, string rawText, string fallback)
        {
            if (string.IsNullOrEmpty(rawText))
                return fallback;

            if (!rawText.Contains(".") && !rawText.StartsWith("pack.", StringComparison.OrdinalIgnoreCase))
            {
                return rawText;
            }

            try
            {
                string textsDir = Path.Combine(packFolderPath, "texts");
                if (Directory.Exists(textsDir))
                {
                    string langFilePath = Path.Combine(textsDir, "en_US.lang");
                    if (!File.Exists(langFilePath))
                    {
                        string[] langFiles = Directory.GetFiles(textsDir, "*.lang");
                        if (langFiles.Length > 0) langFilePath = langFiles[0];
                    }

                    if (File.Exists(langFilePath))
                    {
                        string[] lines = File.ReadAllLines(langFilePath);
                        foreach (string line in lines)
                        {
                            int equalIndex = line.IndexOf('=');
                            if (equalIndex > 0)
                            {
                                string key = line.Substring(0, equalIndex).Trim();
                                if (key.Equals(rawText.Trim(), StringComparison.OrdinalIgnoreCase))
                                {
                                    return line.Substring(equalIndex + 1).Trim();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Localization Parse Exception]: {ex.Message}");
            }

            return string.IsNullOrEmpty(fallback) ? rawText : fallback;
        }

        private static string ExtractJsonArrayValue(string json, string key)
        {
            int keyIdx = json.IndexOf(key, StringComparison.OrdinalIgnoreCase);
            if (keyIdx < 0) return string.Empty;

            int colonIdx = json.IndexOf(':', keyIdx);
            int startBracket = json.IndexOf('[', colonIdx);
            int endBracket = json.IndexOf(']', startBracket);

            if (startBracket > 0 && endBracket > startBracket)
            {
                return json.Substring(startBracket + 1, endBracket - startBracket - 1).Trim();
            }
            return string.Empty;
        }

        public static bool InstallPack(string serverRootPath, string archivePath)
        {
            if (string.IsNullOrWhiteSpace(serverRootPath) || !File.Exists(archivePath))
                return false;

            string tempExtractPath = Path.Combine(Path.GetTempPath(), "BDSUtil_Import_" + Guid.NewGuid().ToString());
            bool installedAny = false;

            try
            {
                ZipFile.ExtractToDirectory(archivePath, tempExtractPath);

                string[] manifests = Directory.GetFiles(tempExtractPath, "manifest.json", SearchOption.AllDirectories);

                foreach (string manifestPath in manifests)
                {
                    string packDir = Path.GetDirectoryName(manifestPath);
                    string jsonContent = File.ReadAllText(manifestPath);

                    string strippedJson = jsonContent.Replace(" ", "").Replace("\t", "").ToLower();
                    bool isBehavior = strippedJson.Contains("\"type\":\"data\"");

                    string rawName = GetStrictManifestValue(jsonContent, "\"name\"");
                    string parsedName = GetLocalizedPackText(packDir, rawName, new DirectoryInfo(packDir).Name);
                    if (string.IsNullOrWhiteSpace(parsedName))
                    {
                        parsedName = new DirectoryInfo(packDir).Name;
                    }

                    parsedName = System.Text.RegularExpressions.Regex.Replace(parsedName, "§.", "");
                    string safeName = string.Join("_", parsedName.Split(Path.GetInvalidFileNameChars())).Replace(" ", "_");
                    safeName = System.Text.RegularExpressions.Regex.Replace(safeName, "_+", "_").Trim('_');

                    string finalFolderName = "custom_" + safeName;

                    string targetCategory = isBehavior ? "behavior_packs" : "resource_packs";
                    string finalDestPath = Path.Combine(serverRootPath, targetCategory, finalFolderName);

                    if (Directory.Exists(finalDestPath))
                    {
                        Directory.Delete(finalDestPath, true);
                    }

                    Directory.Move(packDir, finalDestPath);
                    installedAny = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Pack Import Error]: {ex.Message}");
            }
            finally
            {
                if (Directory.Exists(tempExtractPath))
                {
                    try { Directory.Delete(tempExtractPath, true); } catch { }
                }
            }

            return installedAny;
        }

        public static bool DisablePackInWorld(string serverRootPath, string activeWorldName, string uuid, bool isBehaviorPack)
        {
            if (string.IsNullOrWhiteSpace(serverRootPath) || string.IsNullOrWhiteSpace(activeWorldName))
                return false;

            try
            {
                string fileName = isBehaviorPack ? "world_behavior_packs.json" : "world_resource_packs.json";
                string worldPacksFile = Path.Combine(serverRootPath, "worlds", activeWorldName, fileName);

                if (!File.Exists(worldPacksFile))
                    return true;

                string rawJson = File.ReadAllText(worldPacksFile, Encoding.UTF8);

                int uuidIndex = rawJson.IndexOf(uuid, StringComparison.OrdinalIgnoreCase);
                if (uuidIndex < 0)
                    return true;

                int startBracket = rawJson.LastIndexOf('{', uuidIndex);
                int endBracket = rawJson.IndexOf('}', uuidIndex);

                if (startBracket >= 0 && endBracket > startBracket)
                {
                    string beforeChunk = rawJson.Substring(0, startBracket);
                    string afterChunk = rawJson.Substring(endBracket + 1);
                    string cleanedJson = beforeChunk + afterChunk;

                    cleanedJson = System.Text.RegularExpressions.Regex.Replace(cleanedJson, @",\s*,", ",");
                    cleanedJson = System.Text.RegularExpressions.Regex.Replace(cleanedJson, @"\[\s*,", "[");
                    cleanedJson = System.Text.RegularExpressions.Regex.Replace(cleanedJson, @",\s*\]", "]");

                    File.WriteAllText(worldPacksFile, cleanedJson, Encoding.UTF8);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PackManager Deletion Exception]: {ex.Message}");
                return false;
            }
        }

        public static bool DeletePack(string serverRootPath, string activeWorldName, string folderName, string uuid, bool isBehaviorPack)
        {
            if (string.IsNullOrWhiteSpace(serverRootPath) || string.IsNullOrWhiteSpace(folderName))
                return false;

            if (folderName.Equals("bds_monitor", StringComparison.OrdinalIgnoreCase))
                return false;

            if (!folderName.StartsWith("custom_", StringComparison.OrdinalIgnoreCase))
                return false;

            try
            {
                if (!string.IsNullOrEmpty(activeWorldName) && !string.IsNullOrEmpty(uuid))
                {
                    DisablePackInWorld(serverRootPath, activeWorldName, uuid, isBehaviorPack);
                }

                string packCategory = isBehaviorPack ? "behavior_packs" : "resource_packs";
                string packPath = Path.Combine(serverRootPath, packCategory, folderName);
                if (Directory.Exists(packPath))
                {
                    Directory.Delete(packPath, true);
                }

                string devPackCategory = isBehaviorPack ? "development_behavior_packs" : "development_resource_packs";
                string devPackPath = Path.Combine(serverRootPath, devPackCategory, folderName);
                if (Directory.Exists(devPackPath))
                {
                    Directory.Delete(devPackPath, true);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PackManager Delete Exception]: {ex.Message}");
                return false;
            }
        }
    }

    public class PackManifestInfo
    {
        public string FolderName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Uuid { get; set; }
        public string Version { get; set; }
        public bool IsBehaviorPack { get; set; }
        public bool IsEnabled { get; set; }

        public string FormattedVersion
        {
            get
            {
                if (string.IsNullOrEmpty(Version)) return "1.0.0";

                // If version is a clean dot-notation string like "3.1.1", return it directly
                if (!Version.Contains(",") && Version.Contains("."))
                {
                    return Version.Trim();
                }

                string[] parts = Version.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i] = parts[i].Trim();
                }

                return parts.Length > 0 ? string.Join(".", parts) : Version.Replace(" ", "");
            }
        }
    }
}