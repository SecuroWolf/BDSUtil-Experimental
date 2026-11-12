using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

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
            if (string.IsNullOrWhiteSpace(serverRootPath) || string.IsNullOrWhiteSpace(activeWorldName) || string.IsNullOrWhiteSpace(uuid))
                return false;

            string fileName = isBehaviorPack ? "world_behavior_packs.json" : "world_resource_packs.json";
            string worldPackJsonPath = Path.Combine(serverRootPath, "worlds", activeWorldName, fileName);

            if (!File.Exists(worldPackJsonPath))
                return false;

            try
            {
                string rawJsonContent = File.ReadAllText(worldPackJsonPath, Encoding.UTF8);
                if (string.IsNullOrWhiteSpace(rawJsonContent))
                    return false;

                using JsonDocument doc = JsonDocument.Parse(rawJsonContent, JsonHelper.DocumentOptions);
                if (doc.RootElement.ValueKind != JsonValueKind.Array)
                    return false;

                foreach (JsonElement entry in doc.RootElement.EnumerateArray())
                {
                    if (JsonHelper.TryGetPropertyIgnoreCase(entry, "pack_id", out JsonElement packIdProp) &&
                        packIdProp.ValueKind == JsonValueKind.String &&
                        string.Equals(packIdProp.GetString(), uuid, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[PackManager Read Exception]: {ex.Message}");
                return false;
            }
        }

        public static bool EnablePackInWorld(string serverRootPath, string activeWorldName, string uuid, string versionString, bool isBehaviorPack)
        {
            if (string.IsNullOrWhiteSpace(serverRootPath) || string.IsNullOrWhiteSpace(activeWorldName) || string.IsNullOrWhiteSpace(uuid))
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

                List<int> versionNumbers = ParseWorldPackVersion(versionString);
                JsonArray entries = LoadWorldPackArray(worldPacksFile);

                foreach (JsonNode node in entries)
                {
                    if (TryGetWorldPackId(node, out string existingId) &&
                        string.Equals(existingId, uuid, StringComparison.OrdinalIgnoreCase))
                    {
                        return true; // Already enabled
                    }
                }

                var versionArray = new JsonArray();
                foreach (int num in versionNumbers)
                {
                    versionArray.Add(num);
                }

                entries.Add(new JsonObject
                {
                    ["pack_id"] = uuid,
                    ["version"] = versionArray
                });

                JsonHelper.WriteTextAtomically(worldPacksFile, entries.ToJsonString(JsonHelper.IndentedOptions));
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
                    string jsonContent = File.ReadAllText(manifestPath, Encoding.UTF8);
                    ManifestMetadata metadata = ParseManifest(jsonContent);

                    string finalName = GetLocalizedPackText(packFolder, metadata.Name, folderName);
                    string finalDesc = GetLocalizedPackText(packFolder, metadata.Description, string.Empty);

                    if (!string.IsNullOrEmpty(metadata.Uuid))
                    {
                        bool isEnabledInWorld = IsPackEnabledInWorld(serverRootPath, activeWorldName, metadata.Uuid, getBehaviorPacks);

                        parsedPacks.Add(new PackManifestInfo
                        {
                            FolderName = folderName,
                            Name = finalName,
                            Description = finalDesc,
                            Uuid = metadata.Uuid,
                            Version = string.IsNullOrEmpty(metadata.Version) ? "1, 0, 0" : metadata.Version,
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

        private sealed class ManifestMetadata
        {
            public string Name { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public string Uuid { get; set; } = string.Empty;
            public string Version { get; set; } = string.Empty;
            public bool IsBehaviorPack { get; set; }
        }

        private static ManifestMetadata ParseManifest(string json)
        {
            var metadata = new ManifestMetadata();
            if (string.IsNullOrWhiteSpace(json))
                return metadata;

            using JsonDocument doc = JsonDocument.Parse(json, JsonHelper.DocumentOptions);
            JsonElement root = doc.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
                return metadata;

            JsonElement header = JsonHelper.TryGetPropertyIgnoreCase(root, "header", out JsonElement headerEl) &&
                                 headerEl.ValueKind == JsonValueKind.Object
                ? headerEl
                : root;

            metadata.Name = JsonHelper.GetPropertyString(header, "name");
            metadata.Description = JsonHelper.GetPropertyString(header, "description");
            metadata.Uuid = JsonHelper.GetPropertyString(header, "uuid");
            metadata.Version = FormatVersionElement(header);

            if (JsonHelper.TryGetPropertyIgnoreCase(root, "modules", out JsonElement modulesEl) &&
                modulesEl.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement module in modulesEl.EnumerateArray())
                {
                    string moduleType = JsonHelper.GetPropertyString(module, "type");
                    if (moduleType.Equals("data", StringComparison.OrdinalIgnoreCase) ||
                        moduleType.Equals("script", StringComparison.OrdinalIgnoreCase) ||
                        moduleType.Equals("javascript", StringComparison.OrdinalIgnoreCase))
                    {
                        metadata.IsBehaviorPack = true;
                        break;
                    }
                }
            }

            return metadata;
        }

        private static string FormatVersionElement(JsonElement header)
        {
            if (!JsonHelper.TryGetPropertyIgnoreCase(header, "version", out JsonElement versionEl))
                return string.Empty;

            if (versionEl.ValueKind == JsonValueKind.String)
                return versionEl.GetString()?.Trim() ?? string.Empty;

            if (versionEl.ValueKind == JsonValueKind.Array)
            {
                var parts = new List<string>();
                foreach (JsonElement item in versionEl.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.Number)
                    {
                        parts.Add(item.GetRawText());
                    }
                    else if (item.ValueKind == JsonValueKind.String)
                    {
                        string val = item.GetString()?.Trim();
                        if (!string.IsNullOrEmpty(val))
                            parts.Add(val);
                    }
                }

                return string.Join(", ", parts);
            }

            return string.Empty;
        }

        private static List<int> ParseWorldPackVersion(string versionString)
        {
            var versionNumbers = new List<int>();
            if (!string.IsNullOrWhiteSpace(versionString))
            {
                string cleanVersion = versionString.Replace("[", string.Empty).Replace("]", string.Empty).Trim();
                int preReleaseIdx = cleanVersion.IndexOf('-');
                if (preReleaseIdx >= 0)
                {
                    cleanVersion = cleanVersion.Substring(0, preReleaseIdx);
                }

                string[] parts = cleanVersion.Split(new[] { ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string part in parts)
                {
                    if (int.TryParse(part.Trim(), out int num))
                    {
                        versionNumbers.Add(num);
                    }
                }
            }

            if (versionNumbers.Count == 0)
            {
                versionNumbers.AddRange(new[] { 1, 0, 0 });
            }

            return versionNumbers;
        }

        private static JsonArray LoadWorldPackArray(string worldPacksFile)
        {
            if (!File.Exists(worldPacksFile))
                return new JsonArray();

            string rawJson = File.ReadAllText(worldPacksFile, Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(rawJson))
                return new JsonArray();

            try
            {
                return JsonNode.Parse(rawJson, documentOptions: JsonHelper.DocumentOptions) as JsonArray ?? new JsonArray();
            }
            catch (JsonException)
            {
                return new JsonArray();
            }
        }

        private static bool TryGetWorldPackId(JsonNode node, out string packId)
        {
            packId = null;
            if (node is not JsonObject obj)
                return false;

            foreach (var kvp in obj)
            {
                if (kvp.Key.Equals("pack_id", StringComparison.OrdinalIgnoreCase) &&
                    kvp.Value is JsonValue val &&
                    val.TryGetValue(out string str))
                {
                    packId = str;
                    return true;
                }
            }

            return false;
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
                    string jsonContent = File.ReadAllText(manifestPath, Encoding.UTF8);
                    ManifestMetadata metadata = ParseManifest(jsonContent);

                    bool isBehavior = metadata.IsBehaviorPack;
                    string rawName = metadata.Name;
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
            if (string.IsNullOrWhiteSpace(serverRootPath) || string.IsNullOrWhiteSpace(activeWorldName) || string.IsNullOrWhiteSpace(uuid))
                return false;

            try
            {
                string fileName = isBehaviorPack ? "world_behavior_packs.json" : "world_resource_packs.json";
                string worldPacksFile = Path.Combine(serverRootPath, "worlds", activeWorldName, fileName);

                if (!File.Exists(worldPacksFile))
                    return true;

                JsonArray entries = LoadWorldPackArray(worldPacksFile);
                bool removed = false;

                for (int i = entries.Count - 1; i >= 0; i--)
                {
                    if (TryGetWorldPackId(entries[i], out string existingId) &&
                        string.Equals(existingId, uuid, StringComparison.OrdinalIgnoreCase))
                    {
                        entries.RemoveAt(i);
                        removed = true;
                    }
                }

                if (!removed)
                    return true;

                JsonHelper.WriteTextAtomically(worldPacksFile, entries.ToJsonString(JsonHelper.IndentedOptions));
                return true;
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
