using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BDSUtil.Helpers
{
    /// <summary>
    /// Shared reader and atomic comment-preserving updater for Bedrock Dedicated Server <c>server.properties</c> files.
    /// Eliminates duplicated ad-hoc file parsing and cross-tab overwrites across configuration controls.
    /// </summary>
    public sealed class ServerPropertiesDocument
    {
        private static readonly object SyncLock = new object();
        private static readonly string[] DefaultLegacyRemovedKeys = { "bdsutil-enable-upnp" };

        public IReadOnlyList<string> OriginalLines { get; }
        public Dictionary<string, string> Properties { get; }

        private ServerPropertiesDocument(IReadOnlyList<string> originalLines, Dictionary<string, string> properties)
        {
            OriginalLines = originalLines;
            Properties = properties;
        }

        /// <summary>
        /// Loads and parses <paramref name="path"/> from disk. Returns an empty document when missing.
        /// </summary>
        public static ServerPropertiesDocument Load(string path, IEnumerable<string> ignoredKeys = null)
        {
            var props = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return new ServerPropertiesDocument(Array.Empty<string>(), props);
            }

            var ignoredSet = BuildIgnoreSet(ignoredKeys);
            string[] lines;
            lock (SyncLock)
            {
                lines = File.ReadAllLines(path, Encoding.UTF8);
            }

            foreach (string rawLine in lines)
            {
                if (TryParsePropertyAssignment(rawLine, includeCommented: false, out string key, out string val))
                {
                    if (!ignoredSet.Contains(key))
                    {
                        props[key] = val;
                    }
                }
            }

            return new ServerPropertiesDocument(lines, props);
        }

        /// <summary>
        /// Returns a case-insensitive dictionary of active properties from <paramref name="path"/>.
        /// </summary>
        public static Dictionary<string, string> ReadProperties(string path)
        {
            return Load(path).Properties;
        }

        /// <summary>
        /// Looks up a single active property value in <paramref name="path"/>.
        /// </summary>
        public static bool TryGetProperty(string path, string key, out string value)
        {
            value = null;
            if (string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return Load(path).Properties.TryGetValue(key.Trim(), out value);
        }

        /// <summary>
        /// Splits a comma-separated <c>server-udp-ports</c> value into trimmed non-empty node tokens.
        /// </summary>
        public static List<string> ParseUdpNodeTokens(string rawPorts)
        {
            var tokens = new List<string>();
            if (string.IsNullOrWhiteSpace(rawPorts))
            {
                return tokens;
            }

            foreach (string part in rawPorts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string trimmed = part.Trim();
                if (!string.IsNullOrWhiteSpace(trimmed))
                {
                    tokens.Add(trimmed);
                }
            }

            return tokens;
        }

        /// <summary>
        /// Atomically patches <paramref name="path"/> against its latest on-disk lines while preserving comments and untouched keys.
        /// </summary>
        public static void ApplyUpdates(
            string path,
            IDictionary<string, string> updates,
            IEnumerable<string> commentKeys = null,
            IEnumerable<string> removeKeys = null,
            bool matchCommentedForUpdatedKeys = false)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            var updateMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (updates != null)
            {
                foreach (KeyValuePair<string, string> kvp in updates)
                {
                    if (!string.IsNullOrWhiteSpace(kvp.Key))
                    {
                        updateMap[kvp.Key.Trim()] = kvp.Value ?? string.Empty;
                    }
                }
            }

            var commentSet = new HashSet<string>(commentKeys ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);
            var removeSet = BuildIgnoreSet(removeKeys);

            lock (SyncLock)
            {
                string[] existingLines = File.Exists(path)
                    ? File.ReadAllLines(path, Encoding.UTF8)
                    : Array.Empty<string>();

                var outputLines = new List<string>(existingLines.Length + updateMap.Count);
                var writtenKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                var commentedKeysWritten = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (string rawLine in existingLines)
                {
                    string trimmed = rawLine.Trim();
                    bool isComment = trimmed.StartsWith("#", StringComparison.Ordinal);

                    if (TryParsePropertyAssignment(rawLine, includeCommented: true, out string key, out _))
                    {
                        if (removeSet.Contains(key))
                        {
                            continue;
                        }

                        if (commentSet.Contains(key))
                        {
                            if (commentedKeysWritten.Add(key))
                            {
                                outputLines.Add($"# {key}=");
                            }
                            continue;
                        }

                        bool shouldUpdateThisLine = updateMap.TryGetValue(key, out string newVal) &&
                                                    (!isComment || matchCommentedForUpdatedKeys);
                        if (shouldUpdateThisLine)
                        {
                            if (writtenKeys.Add(key))
                            {
                                outputLines.Add($"{key}={newVal}");
                            }
                            continue;
                        }
                    }

                    outputLines.Add(rawLine);
                }

                foreach (KeyValuePair<string, string> kvp in updateMap)
                {
                    if (!removeSet.Contains(kvp.Key) &&
                        !commentSet.Contains(kvp.Key) &&
                        writtenKeys.Add(kvp.Key))
                    {
                        outputLines.Add($"{kvp.Key}={kvp.Value}");
                    }
                }

                string serialized = outputLines.Count > 0
                    ? string.Join(Environment.NewLine, outputLines) + Environment.NewLine
                    : string.Empty;
                JsonHelper.WriteTextAtomically(path, serialized);
            }
        }

        /// <summary>
        /// Atomically sets (and uncomments if needed) the <c>server-udp-ports</c> entry in <paramref name="path"/>.
        /// </summary>
        public static void SetServerUdpPorts(string path, string consolidatedPortsValue)
        {
            var updates = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["server-udp-ports"] = consolidatedPortsValue ?? string.Empty
            };
            ApplyUpdates(path, updates, matchCommentedForUpdatedKeys: true);
        }

        /// <summary>
        /// Atomically comments out <paramref name="key"/> (e.g. <c># server-udp-ports=</c>) when present in <paramref name="path"/>.
        /// </summary>
        public static void CommentOutProperty(string path, string key)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(key) || !File.Exists(path))
            {
                return;
            }

            ApplyUpdates(path, updates: null, commentKeys: new[] { key.Trim() });
        }

        /// <summary>
        /// Atomically comments out any active (uncommented) assignment for <paramref name="key"/> in <paramref name="path"/>
        /// while leaving already-commented lines untouched. Returns <c>true</c> if the file was modified.
        /// </summary>
        public static bool CommentOutPropertyIfUncommented(string path, string key)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrWhiteSpace(key) || !File.Exists(path))
            {
                return false;
            }

            string targetKey = key.Trim();

            lock (SyncLock)
            {
                string[] existingLines = File.ReadAllLines(path, Encoding.UTF8);
                var outputLines = new List<string>(existingLines.Length);
                bool changed = false;

                foreach (string rawLine in existingLines)
                {
                    if (TryParsePropertyAssignment(rawLine, includeCommented: false, out string parsedKey, out _) &&
                        parsedKey.Equals(targetKey, StringComparison.OrdinalIgnoreCase))
                    {
                        outputLines.Add($"# {rawLine.Trim()}");
                        changed = true;
                        continue;
                    }

                    outputLines.Add(rawLine);
                }

                if (!changed)
                {
                    return false;
                }

                string serialized = outputLines.Count > 0
                    ? string.Join(Environment.NewLine, outputLines) + Environment.NewLine
                    : string.Empty;
                JsonHelper.WriteTextAtomically(path, serialized);
                return true;
            }
        }

        /// <summary>
        /// Resolves <c>server.properties</c> from a server directory, <c>bedrock_server.exe</c> path, or file path,
        /// and comments out <c>server-portv6</c> if found uncommented (ignored under NetherNet).
        /// </summary>
        public static bool CommentOutServerPortV6(string serverDirectoryOrPropertiesPath)
        {
            if (string.IsNullOrWhiteSpace(serverDirectoryOrPropertiesPath))
            {
                return false;
            }

            string candidate = serverDirectoryOrPropertiesPath.Trim();
            if (candidate.EndsWith("bedrock_server.exe", StringComparison.OrdinalIgnoreCase))
            {
                candidate = Path.GetDirectoryName(candidate);
            }

            if (string.IsNullOrWhiteSpace(candidate))
            {
                return false;
            }

            string propsPath = Path.GetFileName(candidate).Equals("server.properties", StringComparison.OrdinalIgnoreCase)
                ? candidate
                : Path.Combine(candidate, "server.properties");

            return CommentOutPropertyIfUncommented(propsPath, "server-portv6");
        }

        private static HashSet<string> BuildIgnoreSet(IEnumerable<string> extraKeys)
        {
            var set = new HashSet<string>(DefaultLegacyRemovedKeys, StringComparer.OrdinalIgnoreCase);
            if (extraKeys != null)
            {
                foreach (string key in extraKeys)
                {
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        set.Add(key.Trim());
                    }
                }
            }

            return set;
        }

        private static bool TryParsePropertyAssignment(string rawLine, bool includeCommented, out string key, out string value)
        {
            key = null;
            value = null;
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                return false;
            }

            string candidate = rawLine.Trim();
            if (candidate.StartsWith("#", StringComparison.Ordinal))
            {
                if (!includeCommented)
                {
                    return false;
                }

                candidate = candidate.Substring(1).TrimStart();
                // Ignore prose comments such as "# Sets the server name." that are not commented key=value assignments.
                if (string.IsNullOrEmpty(candidate) || candidate.Contains(' ') && !StartsWithKeyEquals(candidate))
                {
                    return false;
                }
            }

            int eqIdx = candidate.IndexOf('=');
            if (eqIdx <= 0)
            {
                return false;
            }

            string parsedKey = candidate.Substring(0, eqIdx).Trim();
            if (string.IsNullOrEmpty(parsedKey) || parsedKey.IndexOfAny(new[] { ' ', '\t' }) >= 0)
            {
                return false;
            }

            key = parsedKey;
            value = candidate.Substring(eqIdx + 1).Trim();
            return true;
        }

        private static bool StartsWithKeyEquals(string candidate)
        {
            int eqIdx = candidate.IndexOf('=');
            if (eqIdx <= 0)
            {
                return false;
            }

            string beforeEq = candidate.Substring(0, eqIdx).Trim();
            return beforeEq.Length > 0 && beforeEq.IndexOfAny(new[] { ' ', '\t' }) < 0;
        }
    }
}

