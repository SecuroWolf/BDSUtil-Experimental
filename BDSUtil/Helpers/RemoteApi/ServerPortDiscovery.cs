using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace BDSUtil.Helpers.RemoteApi
{
    /// <summary>
    /// Snapshot of Bedrock Dedicated Server ports and key parameters read from server.properties.
    /// </summary>
    public sealed class ServerPortSnapshot
    {
        public string PropertiesFilePath { get; set; } = string.Empty;
        public bool PropertiesFileExists { get; set; }
        public string[] OriginalLines { get; set; } = Array.Empty<string>();
        public Dictionary<string, string> RawProperties { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public int PrimaryPortV4 { get; set; } = 19132;
        public int MaxPlayers { get; set; } = 10;
        public List<int> ExplicitUdpNodePorts { get; } = new List<int>();
        public HashSet<int> ReservedServerPorts { get; } = new HashSet<int>();
        public int LowestServerPort { get; set; } = 19132;
    }

    /// <summary>
    /// Reads BDS server.properties port assignments and resolves available ports strictly below the lowest server port.
    /// </summary>
    public static class ServerPortDiscovery
    {
        public const int DefaultPrimaryPortV4 = 19132;
        public const int MinimumUnprivilegedPort = 1024;

        /// <summary>
        /// Parses server.properties (when available) and calculates all reserved server ports plus the lowest configured server port.
        /// Defaults to 19132 when server.properties has not been configured yet.
        /// </summary>
        public static ServerPortSnapshot ReadServerPortSnapshot(string serverDirectory)
        {
            var snapshot = new ServerPortSnapshot();

            if (!string.IsNullOrWhiteSpace(serverDirectory))
            {
                string propsPath = Path.Combine(serverDirectory, "server.properties");
                snapshot.PropertiesFilePath = propsPath;

                if (File.Exists(propsPath))
                {
                    snapshot.PropertiesFileExists = true;
                    var doc = ServerPropertiesDocument.Load(propsPath);
                    snapshot.OriginalLines = doc.OriginalLines.ToArray();

                    foreach (KeyValuePair<string, string> kvp in doc.Properties)
                    {
                        string key = kvp.Key;
                        string val = kvp.Value;
                        snapshot.RawProperties[key] = val;

                        if (key.Equals("server-port", StringComparison.OrdinalIgnoreCase) &&
                            int.TryParse(val, out int parsedV4) && IsValidPort(parsedV4))
                        {
                            snapshot.PrimaryPortV4 = parsedV4;
                        }
                        else if (key.Equals("max-players", StringComparison.OrdinalIgnoreCase) &&
                                 int.TryParse(val, out int parsedMax) && parsedMax > 0)
                        {
                            snapshot.MaxPlayers = parsedMax;
                        }
                        else if (key.Equals("server-udp-ports", StringComparison.OrdinalIgnoreCase) &&
                                 !string.IsNullOrWhiteSpace(val))
                        {
                            ParseUdpNodePorts(val, snapshot.ExplicitUdpNodePorts);
                        }
                    }
                }
            }

            snapshot.ReservedServerPorts.Add(snapshot.PrimaryPortV4);
            foreach (int nodePort in snapshot.ExplicitUdpNodePorts)
            {
                snapshot.ReservedServerPorts.Add(nodePort);
            }

            snapshot.LowestServerPort = snapshot.ReservedServerPorts.Count > 0
                ? snapshot.ReservedServerPorts.Min()
                : DefaultPrimaryPortV4;

            return snapshot;
        }

        /// <summary>
        /// Selects the highest available TCP port strictly below <paramref name="lowestServerPort"/>,
        /// skipping reserved server ports, ports claimed by another LAN host on the UPnP gateway, and locally bound ports.
        /// Returns 0 if no valid port below <paramref name="lowestServerPort"/> can be allocated.
        /// </summary>
        public static int FindAvailableApiPortBelow(
            int lowestServerPort,
            ISet<int> excludedPorts = null,
            IReadOnlyDictionary<int, UpnpPortMapping> existingRouterTcpMappings = null,
            ISet<string> hostIps = null,
            int? preferredPort = null,
            int currentBoundPort = 0,
            IPAddress localBindAddress = null,
            Func<int, bool> customLocalAvailabilityCheck = null)
        {
            if (lowestServerPort <= 1) return 0;

            IPAddress effectiveBind = localBindAddress ?? IPAddress.Any;
            int maxCandidate = Math.Min(65535, lowestServerPort - 1);
            int minCandidate = lowestServerPort > MinimumUnprivilegedPort ? MinimumUnprivilegedPort : 1;

            bool IsCandidateUsable(int candidate)
            {
                if (candidate < 1 || candidate >= lowestServerPort || candidate > 65535)
                    return false;

                if (excludedPorts != null && excludedPorts.Contains(candidate))
                    return false;

                if (existingRouterTcpMappings != null &&
                    existingRouterTcpMappings.TryGetValue(candidate, out UpnpPortMapping existing) &&
                    existing.Enabled)
                {
                    bool claimedByUs = hostIps != null &&
                                       hostIps.Contains(existing.InternalClient) &&
                                       existing.InternalPort == candidate;
                    if (!claimedByUs)
                        return false;
                }

                if (candidate == currentBoundPort && currentBoundPort > 0)
                    return true;

                return customLocalAvailabilityCheck != null
                    ? customLocalAvailabilityCheck(candidate)
                    : IsLocalTcpPortAvailable(candidate, effectiveBind);
            }

            if (preferredPort.HasValue && IsCandidateUsable(preferredPort.Value))
            {
                return preferredPort.Value;
            }

            for (int candidate = maxCandidate; candidate >= minCandidate; candidate--)
            {
                if (IsCandidateUsable(candidate))
                {
                    return candidate;
                }
            }

            // Last resort below 1024 only if every unprivileged port below lowestServerPort is exhausted.
            if (minCandidate > 1)
            {
                for (int candidate = MinimumUnprivilegedPort - 1; candidate >= 1; candidate--)
                {
                    if (candidate >= lowestServerPort) continue;
                    if (IsCandidateUsable(candidate))
                    {
                        return candidate;
                    }
                }
            }

            return 0;
        }

        public static bool IsLocalTcpPortAvailable(int port, IPAddress bindAddress = null)
        {
            if (!IsValidPort(port)) return false;

            TcpListener probe = null;
            try
            {
                probe = new TcpListener(bindAddress ?? IPAddress.Any, port);
                probe.Server.ExclusiveAddressUse = true;
                probe.Start();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                try { probe?.Stop(); } catch { }
            }
        }

        private static bool IsValidPort(int port) => port >= 1 && port <= 65535;

        /// <summary>
        /// Computes the starting UDP port immediately above the primary Bedrock port (default 19133).
        /// </summary>
        public static int GetAdditionalUdpStartPort(int primaryPort)
        {
            if (!IsValidPort(primaryPort))
            {
                return DefaultPrimaryPortV4 + 1;
            }

            return Math.Min(65535, primaryPort + 1);
        }

        /// <summary>
        /// Computes how many additional UDP ports to lease for <c>server-udp-ports</c>.
        /// Consolidated mode ignores <paramref name="maxPlayersCount"/> and allocates a single UDP port.
        /// </summary>
        public static int GetTargetUdpNodeCount(int maxPlayersCount, bool consolidateUdpPorts)
        {
            if (consolidateUdpPorts)
            {
                return 1;
            }

            return Math.Clamp(maxPlayersCount, 1, 16);
        }

        private static void ParseUdpNodePorts(string rawNodes, List<int> destination)
        {
            string[] tokens = rawNodes.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string token in tokens)
            {
                string[] parts = token.Trim().Split(':');
                foreach (string part in parts)
                {
                    // Skip IPv4 tokens (contain '.') and parse integer port numbers cleanly.
                    if (part.Contains('.')) continue;
                    if (int.TryParse(part.Trim(), out int port) && IsValidPort(port) && !destination.Contains(port))
                    {
                        destination.Add(port);
                    }
                }
            }
        }
    }
}
