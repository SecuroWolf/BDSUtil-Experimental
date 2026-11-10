using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BDSUtil.Helpers.Updates
{
    internal class ServerUpdates
    {
        private const string API_URL = "https://net-secondary.web.minecraft-services.net/api/v1.0/download/links";

        public static async Task<string> GetServerUpdateUrlAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/129.0.0.0 Safari/537.36");
                string jsonResponse = await client.GetStringAsync(API_URL);

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    var links = doc.RootElement.GetProperty("result").GetProperty("links").EnumerateArray();
                    foreach (var link in links)
                    {
                        if (link.GetProperty("downloadType").GetString() == "serverBedrockWindows")
                        {
                            return link.GetProperty("downloadUrl").GetString();
                        }
                    }
                }
                throw new Exception("Failed to locate the Windows release link in the Mojang API JSON.");
            }
        }

        public static string ExtractVersionFromUrl(string downloadUrl)
        {
            Match match = Regex.Match(downloadUrl, @"bedrock-server-([\d\.]+)\.zip");
            return match.Success ? match.Groups[1].Value : "Unknown";
        }

        /// <summary>
        /// Performs a complete startup check: fetches the latest web URL/version and compares it against the installed binary.
        /// </summary>
        public static async Task<(bool UpdateAvailable, string LatestVersion, string DownloadUrl)> DoUpdateCheck(string serverRoot)
        {
            try
            {
                string downloadUrl = await GetServerUpdateUrlAsync();
                string latestVersionStr = ExtractVersionFromUrl(downloadUrl);
                string installedVersionStr = GetServerVersion(serverRoot);

                if (Version.TryParse(latestVersionStr, out Version latestVersion) &&
                    Version.TryParse(installedVersionStr, out Version installedVersion))
                {
                    // Only trigger if the web version is strictly higher than what's installed
                    if (latestVersion > installedVersion)
                    {
                        return (true, latestVersionStr, downloadUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Update Check Failed]: {ex.Message}");
            }

            return (false, string.Empty, string.Empty);
        }

        /// <summary>
        /// Extracts the current version number natively from the bedrock_server.exe file metadata.
        /// </summary>
        public static string GetServerVersion(string serverRoot)
        {
            string exePath = Path.Combine(serverRoot, "bedrock_server.exe");
            if (File.Exists(exePath))
            {
                var fvi = FileVersionInfo.GetVersionInfo(exePath);
                return fvi.FileVersion?.Replace(", ", ".") ?? "0.0.0.0";
            }
            return "0.0.0.0";
        }

        /// <summary>
        /// Evaluates if the official site version is strictly greater than the installed version.
        /// </summary>
        public static async Task<(bool UpdateAvailable, string LatestVersion, string DownloadUrl)> CheckForUpdatesAsync(string serverRoot)
        {
            string downloadUrl = await GetServerUpdateUrlAsync();
            string latestVersionStr = ExtractVersionFromUrl(downloadUrl);
            string installedVersionStr = GetServerVersion(serverRoot);

            if (Version.TryParse(latestVersionStr, out Version latestVersion) &&
                Version.TryParse(installedVersionStr, out Version installedVersion))
            {
                // Only return true if the site is definitively higher than the installed version
                if (latestVersion > installedVersion)
                {
                    return (true, latestVersionStr, downloadUrl);
                }
            }
            return (false, latestVersionStr, downloadUrl);
        }

        /// <summary>
        /// Downloads the update zip asynchronously to the specified temporary staging path.
        /// </summary>
        public static async Task DownloadUpdateAsync(string updateUrl, string destinationZipPath, IProgress<int> progress)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(updateUrl, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode();
                    long? totalBytes = response.Content.Headers.ContentLength;

                    using (Stream contentStream = await response.Content.ReadAsStreamAsync())
                    using (FileStream fileStream = new FileStream(destinationZipPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        var buffer = new byte[8192];
                        long totalRead = 0;
                        int read;
                        while ((read = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, read);
                            totalRead += read;
                            if (totalBytes.HasValue && progress != null)
                            {
                                progress.Report((int)((double)totalRead / totalBytes.Value * 100));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates a version-stamped pre-update backup utilizing your existing native .zip engine.
        /// </summary>
        public static async Task DoBackupsAsync(string serverRoot, string currentVersion)
        {
            await Task.Run(() =>
            {
                string backupsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
                Directory.CreateDirectory(backupsDir);

                string timestamp = DateTime.Now.ToString("MM_dd_yyyy_HHmmss");
                string archiveName = $"Update_Backup_v{currentVersion}_{timestamp}.zip";
                string finalArchivePath = Path.Combine(backupsDir, archiveName);

                ZipFile.CreateFromDirectory(serverRoot, finalArchivePath, CompressionLevel.Optimal, false);
            });
        }

        /// <summary>
        /// Executes your isolated update strategy: Moves protected files to a temp folder, nukes the rest, extracts the update, and restores protected files.
        /// </summary>
        public static async Task InstallUpdateAsync(string serverRoot, string downloadedZipPath)
        {
            await Task.Run(() =>
            {
                string tempRoot = Path.Combine(serverRoot, "_bdsutil_update_temp");
                string keptFilesDir = Path.Combine(tempRoot, "kept_data");
                Directory.CreateDirectory(keptFilesDir);

                // 1. Identify files to protect
                string[] protectedRootFiles = {
                    "allowlist.json", "permissions.json", "profanity_filter.cache",
                    "profanity_filter.wlist", "server.properties"
                };
                string[] protectedRootFolders = { "worlds", "world_templates" };

                // 2. Move protected root files
                foreach (string file in protectedRootFiles)
                {
                    string sourcePath = Path.Combine(serverRoot, file);
                    if (File.Exists(sourcePath))
                    {
                        File.Copy(sourcePath, Path.Combine(keptFilesDir, file), true);
                    }
                }

                // 3. Move protected root folders
                foreach (string folder in protectedRootFolders)
                {
                    string sourcePath = Path.Combine(serverRoot, folder);
                    if (Directory.Exists(sourcePath))
                    {
                        MoveDirectory(sourcePath, Path.Combine(keptFilesDir, folder));
                    }
                }

                // 4. Surgically extract custom packs
                string[] packContainers = { "behavior_packs", "resource_packs" };
                foreach (string container in packContainers)
                {
                    string sourceContainer = Path.Combine(serverRoot, container);
                    if (Directory.Exists(sourceContainer))
                    {
                        foreach (string packDir in Directory.GetDirectories(sourceContainer))
                        {
                            string folderName = Path.GetFileName(packDir);
                            if (folderName.StartsWith("custom_", StringComparison.OrdinalIgnoreCase) ||
                                folderName.StartsWith("bdsutil_", StringComparison.OrdinalIgnoreCase))
                            {
                                string destPath = Path.Combine(keptFilesDir, container, folderName);
                                Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                                MoveDirectory(packDir, destPath);
                            }
                        }
                    }
                }

                // 5. Nuke the main folder (ignoring our temp folder and the downloaded zip if it sits in the root)
                foreach (string dir in Directory.GetDirectories(serverRoot))
                {
                    if (!dir.Equals(tempRoot, StringComparison.OrdinalIgnoreCase))
                        Directory.Delete(dir, true);
                }
                foreach (string file in Directory.GetFiles(serverRoot))
                {
                    if (!file.Equals(downloadedZipPath, StringComparison.OrdinalIgnoreCase))
                        File.Delete(file);
                }

                // 6. Extract the new BDS binaries
                ZipFile.ExtractToDirectory(downloadedZipPath, serverRoot, true);

                // 7. Restore kept files over the vanilla defaults
                MoveDirectory(keptFilesDir, serverRoot, overwrite: true);

                // 8. Clean up
                Directory.Delete(tempRoot, true);
                File.Delete(downloadedZipPath);
            });
        }

        // Helper for deep directory moving
        private static void MoveDirectory(string sourceDir, string destDir, bool overwrite = false)
        {
            Directory.CreateDirectory(destDir);
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                File.Copy(file, Path.Combine(destDir, Path.GetFileName(file)), overwrite);
            }
            foreach (string dir in Directory.GetDirectories(sourceDir))
            {
                MoveDirectory(dir, Path.Combine(destDir, Path.GetFileName(dir)), overwrite);
            }
        }
    }
}