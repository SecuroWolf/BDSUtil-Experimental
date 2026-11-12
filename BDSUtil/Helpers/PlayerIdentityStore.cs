using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BDSUtil.Helpers;

namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    /// <summary>
    /// Thread-safe JSON-backed store mapping Xbox Live XUIDs to discovered gamertags,
    /// with automatic one-time import from legacy pipe-delimited <c>bds_players.dat</c> files.
    /// </summary>
    public sealed class PlayerIdentityFileStore
    {
        private readonly string _jsonPath;
        private readonly string _legacyDatPath;

        public object LockObject { get; } = new object();
        public Dictionary<string, string> DiscoveredCache { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public PlayerIdentityFileStore(string jsonPath, string legacyDatPath = null)
        {
            _jsonPath = jsonPath ?? throw new ArgumentNullException(nameof(jsonPath));
            _legacyDatPath = legacyDatPath;
            LoadFromDisk();
        }

        /// <summary>
        /// Reloads player identities from <c>bds_players.json</c> (migrating from legacy <c>bds_players.dat</c> if needed).
        /// </summary>
        public void LoadFromDisk()
        {
            lock (LockObject)
            {
                DiscoveredCache.Clear();

                bool loadedJson = TryLoadJsonUnlocked();
                if (!loadedJson && ImportLegacyDatUnlocked())
                {
                    PersistUnlocked();
                }
            }
        }

        /// <summary>
        /// Saves or updates a discovered player's XUID-to-gamertag mapping atomically as JSON.
        /// </summary>
        public void SavePlayerNode(string xuid, string gamertag)
        {
            if (string.IsNullOrWhiteSpace(xuid) || string.IsNullOrWhiteSpace(gamertag))
            {
                return;
            }

            string normalizedXuid = xuid.Trim();
            string normalizedGamertag = gamertag.Trim();

            lock (LockObject)
            {
                if (DiscoveredCache.TryGetValue(normalizedXuid, out string existing) &&
                    string.Equals(existing, normalizedGamertag, StringComparison.Ordinal) &&
                    File.Exists(_jsonPath))
                {
                    return;
                }

                DiscoveredCache[normalizedXuid] = normalizedGamertag;
                PersistUnlocked();
            }
        }

        /// <summary>
        /// Attempts to look up a player's gamertag by XUID.
        /// </summary>
        public bool TryGetGamertag(string xuid, out string gamertag)
        {
            gamertag = null;
            if (string.IsNullOrWhiteSpace(xuid))
            {
                return false;
            }

            lock (LockObject)
            {
                return DiscoveredCache.TryGetValue(xuid.Trim(), out gamertag);
            }
        }

        /// <summary>
        /// Attempts to resolve a player's XUID from their gamertag (case-insensitive).
        /// </summary>
        public bool TryResolveXuid(string gamertag, out string xuid)
        {
            xuid = null;
            if (string.IsNullOrWhiteSpace(gamertag))
            {
                return false;
            }

            string target = gamertag.Trim();
            lock (LockObject)
            {
                foreach (KeyValuePair<string, string> kvp in DiscoveredCache)
                {
                    if (string.Equals(kvp.Value, target, StringComparison.OrdinalIgnoreCase))
                    {
                        xuid = kvp.Key;
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Resolves a player's XUID from their gamertag, returning <paramref name="gamertag"/> unchanged if unknown.
        /// </summary>
        public string ResolveXuidOrFallback(string gamertag)
        {
            return TryResolveXuid(gamertag, out string xuid) ? xuid : gamertag;
        }

        /// <summary>
        /// Returns a snapshot of known players sorted alphabetically by gamertag.
        /// </summary>
        public List<KeyValuePair<string, string>> GetKnownPlayersSnapshot()
        {
            lock (LockObject)
            {
                return DiscoveredCache
                    .OrderBy(kvp => kvp.Value, StringComparer.OrdinalIgnoreCase)
                    .ToList();
            }
        }

        private bool TryLoadJsonUnlocked()
        {
            if (!JsonHelper.TryDeserializeFile(_jsonPath, out Dictionary<string, string> loaded, JsonHelper.IndentedOptions) ||
                loaded == null)
            {
                return false;
            }

            foreach (KeyValuePair<string, string> kvp in loaded)
            {
                if (!string.IsNullOrWhiteSpace(kvp.Key) && !string.IsNullOrWhiteSpace(kvp.Value))
                {
                    DiscoveredCache[kvp.Key.Trim()] = kvp.Value.Trim();
                }
            }

            return true;
        }

        private bool ImportLegacyDatUnlocked()
        {
            if (string.IsNullOrWhiteSpace(_legacyDatPath) || !File.Exists(_legacyDatPath))
            {
                return false;
            }

            bool importedAny = false;
            try
            {
                foreach (string line in File.ReadLines(_legacyDatPath))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split(new[] { "||" }, 2, StringSplitOptions.None);
                    if (parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[0]) && !string.IsNullOrWhiteSpace(parts[1]))
                    {
                        DiscoveredCache[parts[0].Trim()] = parts[1].Trim();
                        importedAny = true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Legacy player identity import failed: {ex.Message}");
            }

            return importedAny;
        }

        private void PersistUnlocked()
        {
            try
            {
                var ordered = new SortedDictionary<string, string>(DiscoveredCache, StringComparer.OrdinalIgnoreCase);
                JsonHelper.WriteJsonFileAtomically(_jsonPath, ordered, JsonHelper.IndentedOptions);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Player cache write failed: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Global singleton facade preserving the existing static API over <see cref="PlayerIdentityFileStore"/>.
    /// </summary>
    public static class PlayerIdentityStore
    {
        private static readonly string StoragePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bds_players.json");
        private static readonly string LegacyStoragePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bds_players.dat");
        private static readonly PlayerIdentityFileStore Store = new PlayerIdentityFileStore(StoragePath, LegacyStoragePath);

        public static object LockObject => Store.LockObject;
        public static Dictionary<string, string> DiscoveredCache => Store.DiscoveredCache;

        public static void SavePlayerNode(string xuid, string gamertag) => Store.SavePlayerNode(xuid, gamertag);
        public static bool TryGetGamertag(string xuid, out string gamertag) => Store.TryGetGamertag(xuid, out gamertag);
        public static bool TryResolveXuid(string gamertag, out string xuid) => Store.TryResolveXuid(gamertag, out xuid);
        public static string ResolveXuidOrFallback(string gamertag) => Store.ResolveXuidOrFallback(gamertag);
        public static List<KeyValuePair<string, string>> GetKnownPlayersSnapshot() => Store.GetKnownPlayersSnapshot();
    }
}

