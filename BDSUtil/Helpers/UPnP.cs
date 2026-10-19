using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BDSUtil.Helpers
{
    public static class UPnPEngine
    {
        // TRACKING MATRIX: Keeps track of exactly what ports we leased during this execution loop session
        private static readonly HashSet<(int Port, string Protocol)> _activeLeases = new HashSet<(int Port, string Protocol)>();
        private static readonly object _leaseLock = new object();

        public static string GetLocalIPAddress()
        {
            try
            {
                using (System.Net.Sockets.Socket socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    System.Net.IPEndPoint endPoint = socket.LocalEndPoint as System.Net.IPEndPoint;
                    return endPoint?.Address.ToString() ?? "127.0.0.1";
                }
            }
            catch
            {
                return "127.0.0.1";
            }
        }

        public static string GetExternalIPAddress()
        {
            try
            {
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(3);
                    // REAL FIX: Changed 'ipify.org' to 'api.ipify.org' to get raw text instead of HTML
                    string ip = client.GetStringAsync("https://api.ipify.org").GetAwaiter().GetResult();
                    return ip.Trim();
                }
            }
            catch
            {
                return "127.0.0.1";
            }
        }



        /// <summary>
        /// Standard 3-parameter variant (Defaults to UDP for dynamic multi-node tracking pools)
        /// </summary>
        public static bool ForwardPort(int externalPort, int internalPort, string localIp)
        {
            return ForwardPort(externalPort, internalPort, localIp, "UDP");
        }

        /// <summary>
        /// 4-parameter overload to handle explicit TCP and UDP allocations cleanly
        /// </summary>
        public static bool ForwardPort(int externalPort, int internalPort, string localIp, string protocolType)
        {
            try
            {
                Open.Nat.Protocol targetProtocol = protocolType.Equals("TCP", StringComparison.OrdinalIgnoreCase)
                    ? Open.Nat.Protocol.Tcp
                    : Open.Nat.Protocol.Udp;

                var discoverer = new Open.Nat.NatDiscoverer();
                var cts = new CancellationTokenSource(2500);

                var deviceTask = discoverer.DiscoverDeviceAsync(Open.Nat.PortMapper.Upnp, cts);
                var device = deviceTask.GetAwaiter().GetResult();

                if (device != null)
                {
                    var mapping = new Open.Nat.Mapping(targetProtocol, internalPort, externalPort, $"BDSUtil Node {externalPort}");
                    device.CreatePortMapAsync(mapping).GetAwaiter().GetResult();

                    // Track the lease successfully so we can dismantle it on application close
                    lock (_leaseLock)
                    {
                        _activeLeases.Add((externalPort, protocolType.ToUpper()));
                    }
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Explicit single-port removal module supporting manual interface control list deletions.
        /// </summary>
        public static bool ClosePort(int externalPort)
        {
            try
            {
                var discoverer = new Open.Nat.NatDiscoverer();
                var cts = new System.Threading.CancellationTokenSource(2500); // 2.5 second timeout deadline
                var deviceTask = discoverer.DiscoverDeviceAsync(Open.Nat.PortMapper.Upnp, cts);
                var device = deviceTask.GetAwaiter().GetResult();

                if (device != null)
                {
                    // Dismantle standard UDP routing assignments directly on target candidate indices
                    var oldMapping = new Open.Nat.Mapping(Open.Nat.Protocol.Udp, externalPort, externalPort);
                    device.DeletePortMapAsync(oldMapping).GetAwaiter().GetResult();

                    // Keep our internal global tracking registry cleaned up synchronously
                    lock (_leaseLock)
                    {
                        _activeLeases.Remove((externalPort, "UDP"));
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UPnP Manual Close Port Failure]: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Programmatic teardown engine: Forcefully deletes every lease created by this execution instance from the router hardware
        /// </summary>
        public static void TearDownPorts()
        {
            List<(int Port, string Protocol)> leasesToPurge;
            lock (_leaseLock)
            {
                if (_activeLeases.Count == 0) return;
                leasesToPurge = new List<(int Port, string Protocol)>(_activeLeases);
            }

            try
            {
                var discoverer = new Open.Nat.NatDiscoverer();
                var cts = new CancellationTokenSource(3000); // 3 second deadline allocation window
                var deviceTask = discoverer.DiscoverDeviceAsync(Open.Nat.PortMapper.Upnp, cts);
                var device = deviceTask.GetAwaiter().GetResult();

                if (device != null)
                {
                    foreach (var lease in leasesToPurge)
                    {
                        try
                        {
                            Open.Nat.Protocol targetProtocol = lease.Protocol == "TCP"
                                ? Open.Nat.Protocol.Tcp
                                : Open.Nat.Protocol.Udp;

                            var oldMapping = new Open.Nat.Mapping(targetProtocol, lease.Port, lease.Port);
                            device.DeletePortMapAsync(oldMapping).GetAwaiter().GetResult();
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[UPnP Individual Purge Failure]: Port {lease.Port} ({lease.Protocol}) - {ex.Message}");
                        }
                    }

                    lock (_leaseLock)
                    {
                        _activeLeases.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[UPnP Global Hardware Disconnect Failure]: {ex.Message}");
            }
        }
    }
}
