using BDSUtil.Configuration;
using BDSUtil.Helpers;
using BDSUtil.Helpers.RemoteApi;
using BDSUtil.Helpers.Updates;
using BDSUtil.Modals.Content_Controls.CfgHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BDSUtil
{
    public partial class MainWindow : IRemoteManagementHost
    {
        private const int MaxConsoleRingBufferEntries = 1000;

        private readonly object _consoleLogLock = new object();
        private readonly Queue<ConsoleLogEntry> _consoleRingBuffer = new Queue<ConsoleLogEntry>();
        private long _nextConsoleSequence = 1;

        private readonly object _onlinePlayersLock = new object();
        private readonly Dictionary<string, ConnectedPlayerInfo> _onlinePlayers = new Dictionary<string, ConnectedPlayerInfo>(StringComparer.OrdinalIgnoreCase);

        private readonly object _serverFilesLock = new object();
        private DateTimeOffset? _serverStartedAtUtc = null;
        private bool _suppressNextBackupExplorerPrompt = false;

        internal void RecordConsoleEntry(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            lock (_consoleLogLock)
            {
                _consoleRingBuffer.Enqueue(new ConsoleLogEntry
                {
                    Sequence = _nextConsoleSequence++,
                    TimestampUtc = DateTimeOffset.UtcNow,
                    Line = text
                });

                while (_consoleRingBuffer.Count > MaxConsoleRingBufferEntries)
                {
                    _consoleRingBuffer.Dequeue();
                }
            }
        }

        internal void TrackPlayerConnectionLine(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            bool isConnect = text.IndexOf("Player connected:", StringComparison.OrdinalIgnoreCase) >= 0;
            bool isDisconnect = !isConnect && text.IndexOf("Player disconnected:", StringComparison.OrdinalIgnoreCase) >= 0;
            if (!isConnect && !isDisconnect) return;

            string marker = isConnect ? "Player connected:" : "Player disconnected:";
            int markerIdx = text.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
            if (markerIdx < 0) return;

            string payload = text.Substring(markerIdx + marker.Length).Trim();
            int commaIdx = payload.IndexOf(',');
            string gamertag = (commaIdx >= 0 ? payload.Substring(0, commaIdx) : payload).Trim();
            if (string.IsNullOrEmpty(gamertag)) return;

            string xuid = string.Empty;
            int xuidIdx = payload.IndexOf("xuid:", StringComparison.OrdinalIgnoreCase);
            if (xuidIdx >= 0)
            {
                xuid = payload.Substring(xuidIdx + 5).Trim();
                int trailingComma = xuid.IndexOf(',');
                if (trailingComma >= 0) xuid = xuid.Substring(0, trailingComma).Trim();
            }

            lock (_onlinePlayersLock)
            {
                if (isConnect)
                {
                    _onlinePlayers[gamertag] = new ConnectedPlayerInfo
                    {
                        Gamertag = gamertag,
                        Xuid = xuid,
                        ConnectedAtUtc = DateTimeOffset.UtcNow
                    };
                }
                else
                {
                    _onlinePlayers.Remove(gamertag);
                }
            }
        }

        internal void ClearTrackedOnlinePlayers()
        {
            lock (_onlinePlayersLock)
            {
                _onlinePlayers.Clear();
            }
            _serverStartedAtUtc = null;
        }

        private Task<T> InvokeOnUiAsync<T>(Func<T> func)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
            {
                return Task.FromException<T>(new ObjectDisposedException(nameof(MainWindow)));
            }

            if (!this.InvokeRequired)
            {
                return Task.FromResult(func());
            }

            var tcs = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        tcs.TrySetResult(func());
                    }
                    catch (Exception ex)
                    {
                        tcs.TrySetException(ex);
                    }
                }));
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }

            return tcs.Task;
        }

        private Task InvokeOnUiAsync(Action action) =>
            InvokeOnUiAsync(() => { action(); return true; });

        public RemoteServerStatusSnapshot GetStatusSnapshot()
        {
            string serverRoot = AppSettings.Instance?.General?.ServerExecutablePath ?? string.Empty;
            bool pathValid = !string.IsNullOrWhiteSpace(serverRoot) &&
                             Directory.Exists(serverRoot) &&
                             File.Exists(Path.Combine(serverRoot, "bedrock_server.exe"));

            ServerPortSnapshot portSnap = ServerPortDiscovery.ReadServerPortSnapshot(serverRoot);

            bool running = false;
            bool initialized = _isServerFullyInitialized;
            int? pid = null;
            double? memoryMb = null;
            double? uptimeSec = null;

            Process proc = _serverProcess;
            if (proc != null)
            {
                try
                {
                    if (!proc.HasExited)
                    {
                        running = true;
                        pid = proc.Id;
                        memoryMb = Math.Round(proc.WorkingSet64 / (1024.0 * 1024.0), 1);
                        if (_serverStartedAtUtc.HasValue)
                        {
                            uptimeSec = Math.Round(Math.Max(0, (DateTimeOffset.UtcNow - _serverStartedAtUtc.Value).TotalSeconds), 1);
                        }
                    }
                }
                catch { }
            }

            List<ConnectedPlayerInfo> online;
            lock (_onlinePlayersLock)
            {
                online = _onlinePlayers.Values
                    .OrderBy(p => p.Gamertag, StringComparer.OrdinalIgnoreCase)
                    .Select(p => new ConnectedPlayerInfo
                    {
                        Gamertag = p.Gamertag,
                        Xuid = p.Xuid,
                        ConnectedAtUtc = p.ConnectedAtUtc
                    })
                    .ToList();
            }

            var api = RemoteApiManager.Instance;
            string localIp = string.IsNullOrWhiteSpace(api.LocalIp) ? UPnPEngine.GetLocalIPAddress() : api.LocalIp;
            string externalIp = api.ExternalIp;
            int apiPort = api.BoundPort;

            return new RemoteServerStatusSnapshot
            {
                WrapperVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0",
                ServerVersion = pathValid ? ServerUpdates.GetServerVersion(serverRoot) : string.Empty,
                ServerRunning = running,
                ServerInitialized = initialized,
                ProcessId = pid,
                UptimeSeconds = uptimeSec,
                MemoryMb = memoryMb,
                ServerPathValid = pathValid,
                BackupInProgress = _backupWorker != null && _backupWorker.IsBusy,
                AutoModEnabled = AppSettings.Instance?.AutoMod?.Enabled ?? false,
                OnlinePlayerCount = online.Count,
                OnlinePlayers = online,
                Ports = new RemoteServerPortsInfo
                {
                    PrimaryIpv4 = portSnap.PrimaryPortV4,
                    LowestServerPort = portSnap.LowestServerPort,
                    MaxPlayers = portSnap.MaxPlayers,
                    UdpNodes = new List<int>(portSnap.ExplicitUdpNodePorts)
                },
                RemoteApi = new RemoteApiNetworkingInfo
                {
                    ApiRunning = api.IsRunning,
                    ApiPort = apiPort,
                    UpnpEnabled = AppSettings.Instance?.Networking?.EnableUPnP ?? false,
                    ApiUpnpForwarded = api.IsUpnpForwarded,
                    LeasedUpnpPort = api.LeasedUpnpPort,
                    ExternalIp = externalIp,
                    LocalIp = localIp,
                    ExternalUrl = (apiPort > 0 && !string.IsNullOrWhiteSpace(externalIp)) ? $"http://{externalIp}:{apiPort}/api/v1" : string.Empty,
                    LanUrl = (apiPort > 0 && !string.IsNullOrWhiteSpace(localIp)) ? $"http://{localIp}:{apiPort}/api/v1" : string.Empty,
                    UpnpVerifyState = api.LastVerification?.AccessibilityState.ToString() ?? string.Empty,
                    StatusMessage = api.StatusMessage
                },
                TimestampUtc = DateTimeOffset.UtcNow
            };
        }

        public ConsoleLogPage GetConsoleLogs(long sinceSequence, int limit)
        {
            lock (_consoleLogLock)
            {
                var matching = _consoleRingBuffer
                    .Where(e => e.Sequence > sinceSequence)
                    .Take(limit)
                    .ToList();

                return new ConsoleLogPage
                {
                    LatestSequence = _nextConsoleSequence - 1,
                    ReturnedFromSequence = matching.Count > 0 ? matching[0].Sequence : sinceSequence,
                    Entries = matching
                };
            }
        }

        public async Task<RemoteActionResult> StartServerAsync(CancellationToken ct)
        {
            string serverDir = AppSettings.Instance?.General?.ServerExecutablePath ?? string.Empty;
            if (string.IsNullOrWhiteSpace(serverDir) || !Directory.Exists(serverDir))
            {
                return RemoteActionResult.Fail(400, "Bedrock server directory is not configured or does not exist.");
            }

            string exePath = Path.Combine(serverDir, "bedrock_server.exe");
            if (!File.Exists(exePath))
            {
                return RemoteActionResult.Fail(400, "bedrock_server.exe could not be found in the configured server directory.");
            }

            if (_backupWorker != null && _backupWorker.IsBusy)
            {
                return RemoteActionResult.Fail(423, "Cannot start server while a backup job is running.");
            }

            return await InvokeOnUiAsync(() =>
            {
                if (_serverProcess != null && !_serverProcess.HasExited)
                {
                    return RemoteActionResult.Fail(409, "Server is already running.", new { processId = _serverProcess.Id });
                }

                AppendTerminalLine("[REMOTE API] Authorized client requested server startup.");
                StartServerBTN_New_Click(this, EventArgs.Empty);

                bool started = _serverProcess != null && !_serverProcess.HasExited;
                return started
                    ? RemoteActionResult.Ok("Bedrock server process started.", new { processId = _serverProcess.Id })
                    : RemoteActionResult.Fail(500, "Server process did not start.");
            }).ConfigureAwait(false);
        }

        public async Task<RemoteActionResult> StopServerAsync(bool force, CancellationToken ct)
        {
            return await InvokeOnUiAsync(() =>
            {
                if (_serverProcess == null || _serverProcess.HasExited)
                {
                    return RemoteActionResult.Fail(409, "Server is not currently running.");
                }

                if (force)
                {
                    AppendTerminalLine("[REMOTE API] Authorized client forced server termination.");
                    _serverProcess.Kill();
                    UpdateControlButtonStates();
                    return RemoteActionResult.Ok("Server process forcefully terminated.");
                }

                AppendTerminalLine("[REMOTE API] Authorized client issued administrative 'stop'.");
                _serverProcess.StandardInput.WriteLine("stop");
                UpdateControlButtonStates();
                return RemoteActionResult.Accepted("Graceful server shutdown requested ('stop').");
            }).ConfigureAwait(false);
        }

        public async Task<RemoteActionResult> RestartServerAsync(CancellationToken ct)
        {
            Process existingProc = _serverProcess;
            if (existingProc != null && !existingProc.HasExited)
            {
                await InvokeOnUiAsync(() =>
                {
                    AppendTerminalLine("[REMOTE API] Authorized client requested server restart. Stopping server...");
                    if (_serverProcess != null && !_serverProcess.HasExited)
                    {
                        _serverProcess.StandardInput.WriteLine("stop");
                    }
                }).ConfigureAwait(false);

                using var waitCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                waitCts.CancelAfter(TimeSpan.FromSeconds(25));
                try
                {
                    await existingProc.WaitForExitAsync(waitCts.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    return RemoteActionResult.Fail(409, "Timed out waiting for server to stop cleanly before restart.");
                }

                await Task.Delay(300, ct).ConfigureAwait(false);
            }

            return await StartServerAsync(ct).ConfigureAwait(false);
        }

        public async Task<RemoteActionResult> SendConsoleCommandAsync(string command, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return RemoteActionResult.Fail(400, "Console command cannot be empty.");
            }

            string sanitized = command.Replace("\r", "").Replace("\n", "").Trim();
            if (string.IsNullOrEmpty(sanitized))
            {
                return RemoteActionResult.Fail(400, "Console command cannot be empty.");
            }

            return await InvokeOnUiAsync(() =>
            {
                if (_serverProcess == null || _serverProcess.HasExited)
                {
                    return RemoteActionResult.Fail(409, "Cannot run console command because server is not running.");
                }

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                AppendTerminalLine($"[{timestamp} REMOTE CONSOLE] {sanitized}");
                _serverProcess.StandardInput.WriteLine(sanitized);
                return RemoteActionResult.Ok("Command dispatched to server console.", new { command = sanitized });
            }).ConfigureAwait(false);
        }

        public RemotePlayersSnapshot GetPlayersSnapshot()
        {
            var result = new RemotePlayersSnapshot();

            lock (_onlinePlayersLock)
            {
                result.OnlinePlayers = _onlinePlayers.Values
                    .OrderBy(p => p.Gamertag, StringComparer.OrdinalIgnoreCase)
                    .Select(p => new ConnectedPlayerInfo
                    {
                        Gamertag = p.Gamertag,
                        Xuid = p.Xuid,
                        ConnectedAtUtc = p.ConnectedAtUtc
                    })
                    .ToList();
            }

            result.KnownPlayers = PlayerIdentityStore.GetKnownPlayersSnapshot()
                .Select(kvp => new KnownPlayerInfo
                {
                    Xuid = kvp.Key,
                    Gamertag = kvp.Value
                })
                .ToList();

            return result;
        }

        public Task<RemoteActionResult> ExecutePlayerActionAsync(PlayerActionRequest request, CancellationToken ct)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Action) || string.IsNullOrWhiteSpace(request.Player))
            {
                return Task.FromResult(RemoteActionResult.Fail(400, "Both 'action' and 'player' are required."));
            }

            string cleanPlayer = request.Player.Replace("\r", "").Replace("\n", "").Replace("\"", "").Trim();
            string cleanMsg = (request.Message ?? string.Empty).Replace("\r", "").Replace("\n", "").Trim();
            string action = request.Action.Trim().ToLowerInvariant();

            string cmd;
            switch (action)
            {
                case "kick":
                    cmd = string.IsNullOrEmpty(cleanMsg)
                        ? $"kick \"{cleanPlayer}\""
                        : $"kick \"{cleanPlayer}\" {cleanMsg}";
                    break;
                case "tell":
                case "msg":
                case "whisper":
                    if (string.IsNullOrEmpty(cleanMsg))
                        return Task.FromResult(RemoteActionResult.Fail(400, "Message text is required for tell/whisper actions."));
                    cmd = $"tell \"{cleanPlayer}\" {cleanMsg}";
                    break;
                case "op":
                    cmd = $"op \"{cleanPlayer}\"";
                    break;
                case "deop":
                    cmd = $"deop \"{cleanPlayer}\"";
                    break;
                default:
                    return Task.FromResult(RemoteActionResult.Fail(400, $"Unsupported player action '{request.Action}'. Supported: kick, tell, op, deop."));
            }

            return SendConsoleCommandAsync(cmd, ct);
        }

        public IReadOnlyList<WhitelistEntryDto> GetWhitelist()
        {
            string allowlistPath = GetServerFilePath("allowlist.json");
            if (string.IsNullOrEmpty(allowlistPath) || !File.Exists(allowlistPath))
            {
                return Array.Empty<WhitelistEntryDto>();
            }

            lock (_serverFilesLock)
            {
                try
                {
                    string raw = File.ReadAllText(allowlistPath, Encoding.UTF8);
                    if (string.IsNullOrWhiteSpace(raw)) return Array.Empty<WhitelistEntryDto>();

                    var loaded = JsonSerializer.Deserialize<List<WhitelistPlayerEntry>>(raw, JsonHelper.CompactOptions);
                    if (loaded == null) return Array.Empty<WhitelistEntryDto>();

                    return loaded
                        .Where(p => p != null && !string.IsNullOrWhiteSpace(p.Name))
                        .Select(p => new WhitelistEntryDto
                        {
                            Name = p.Name.Trim(),
                            Xuid = string.IsNullOrWhiteSpace(p.Xuid) ? null : p.Xuid.Trim(),
                            IgnoresPlayerLimit = p.IgnoresPlayerLimit
                        })
                        .ToList();
                }
                catch
                {
                    return Array.Empty<WhitelistEntryDto>();
                }
            }
        }

        public async Task<RemoteActionResult> AddWhitelistPlayerAsync(string name, bool ignoresPlayerLimit, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return RemoteActionResult.Fail(400, "Player gamertag 'name' is required.");
            }

            string allowlistPath = GetServerFilePath("allowlist.json");
            if (string.IsNullOrEmpty(allowlistPath))
            {
                return RemoteActionResult.Fail(400, "Server directory is not configured.");
            }

            string cleanName = name.Replace("\"", "").Replace("\r", "").Replace("\n", "").Trim();

            lock (_serverFilesLock)
            {
                var entries = ReadRawAllowlistLocked(allowlistPath);
                if (entries.Any(e => e.Name.Equals(cleanName, StringComparison.OrdinalIgnoreCase)))
                {
                    return RemoteActionResult.Fail(409, $"'{cleanName}' is already on allowlist.json.");
                }

                entries.Add(new WhitelistPlayerEntry
                {
                    Name = cleanName,
                    Xuid = null,
                    IgnoresPlayerLimit = ignoresPlayerLimit
                });

                string json = JsonSerializer.Serialize(entries, JsonHelper.IndentedOptions);
                JsonHelper.WriteTextAtomically(allowlistPath, json);
            }

            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                await SendConsoleCommandAsync("allowlist reload", ct).ConfigureAwait(false);
            }

            return RemoteActionResult.Ok($"Added '{cleanName}' to whitelist.", new { name = cleanName, ignoresPlayerLimit });
        }

        public async Task<RemoteActionResult> RemoveWhitelistPlayerAsync(string name, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return RemoteActionResult.Fail(400, "Player gamertag is required.");
            }

            string allowlistPath = GetServerFilePath("allowlist.json");
            if (string.IsNullOrEmpty(allowlistPath) || !File.Exists(allowlistPath))
            {
                return RemoteActionResult.Fail(404, "allowlist.json not found.");
            }

            string cleanName = name.Replace("\"", "").Replace("\r", "").Replace("\n", "").Trim();
            int removed;

            lock (_serverFilesLock)
            {
                var entries = ReadRawAllowlistLocked(allowlistPath);
                removed = entries.RemoveAll(e => e.Name.Equals(cleanName, StringComparison.OrdinalIgnoreCase));
                if (removed == 0)
                {
                    return RemoteActionResult.Fail(404, $"Player '{cleanName}' was not found on allowlist.json.");
                }

                string json = JsonSerializer.Serialize(entries, JsonHelper.IndentedOptions);
                JsonHelper.WriteTextAtomically(allowlistPath, json);
            }

            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                await SendConsoleCommandAsync("allowlist reload", ct).ConfigureAwait(false);
            }

            return RemoteActionResult.Ok($"Removed '{cleanName}' from whitelist.");
        }

        public IReadOnlyList<OperatorEntryDto> GetOperators()
        {
            string permsPath = GetServerFilePath("permissions.json");
            if (string.IsNullOrEmpty(permsPath) || !File.Exists(permsPath))
            {
                return Array.Empty<OperatorEntryDto>();
            }

            lock (_serverFilesLock)
            {
                try
                {
                    string raw = File.ReadAllText(permsPath, Encoding.UTF8);
                    if (string.IsNullOrWhiteSpace(raw)) return Array.Empty<OperatorEntryDto>();

                    var entries = JsonSerializer.Deserialize<List<OperatorEntry>>(raw, JsonHelper.CompactOptions);
                    if (entries == null) return Array.Empty<OperatorEntryDto>();

                    var results = new List<OperatorEntryDto>();
                    foreach (var e in entries)
                    {
                        if (e == null || string.IsNullOrWhiteSpace(e.Xuid)) continue;
                        PlayerIdentityStore.TryGetGamertag(e.Xuid, out string gamertag);

                        results.Add(new OperatorEntryDto
                        {
                            Xuid = e.Xuid.Trim(),
                            Gamertag = gamertag ?? string.Empty,
                            Permission = string.IsNullOrWhiteSpace(e.Permission) ? "operator" : e.Permission.Trim()
                        });
                    }
                    return results;
                }
                catch
                {
                    return Array.Empty<OperatorEntryDto>();
                }
            }
        }

        public IReadOnlyDictionary<string, string> GetServerProperties()
        {
            string serverRoot = AppSettings.Instance?.General?.ServerExecutablePath ?? string.Empty;
            var snap = ServerPortDiscovery.ReadServerPortSnapshot(serverRoot);
            return snap.RawProperties;
        }

        public async Task<RemoteActionResult> UpdateServerPropertiesAsync(IReadOnlyDictionary<string, string> updates, CancellationToken ct)
        {
            if (updates == null || updates.Count == 0)
            {
                return RemoteActionResult.Fail(400, "No server.properties updates were provided.");
            }

            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                return RemoteActionResult.Fail(409, "Stop the Bedrock server before editing server.properties.");
            }

            string propsPath = GetServerFilePath("server.properties");
            if (string.IsNullOrEmpty(propsPath) || !File.Exists(propsPath))
            {
                return RemoteActionResult.Fail(404, "server.properties was not found in the configured server directory.");
            }

            await InvokeOnUiAsync(() => { }).ConfigureAwait(false); // sanity check form still alive

            bool ignoredIpv6 = false;
            int updatedCount = 0;
            lock (_serverFilesLock)
            {
                var pending = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var kvp in updates)
                {
                    if (string.IsNullOrWhiteSpace(kvp.Key)) continue;
                    string cleanKey = kvp.Key.Replace("\r", "").Replace("\n", "").Replace("=", "").Trim();
                    string cleanVal = (kvp.Value ?? string.Empty).Replace("\r", "").Replace("\n", "").Trim();
                    if (cleanKey.Equals("server-portv6", StringComparison.OrdinalIgnoreCase))
                    {
                        ignoredIpv6 = true;
                        continue;
                    }
                    if (!string.IsNullOrEmpty(cleanKey))
                    {
                        pending[cleanKey] = cleanVal;
                    }
                }

                updatedCount = pending.Count;
                if (updatedCount > 0)
                {
                    ServerPropertiesDocument.ApplyUpdates(propsPath, pending);
                }

                ServerPropertiesDocument.CommentOutServerPortV6(propsPath);
            }

            if (updatedCount == 0)
            {
                string msg = ignoredIpv6
                    ? "Ignored server-portv6 (ignored under NetherNet; kept commented out in server.properties)."
                    : "No valid server.properties updates were provided.";
                return ignoredIpv6 ? RemoteActionResult.Ok(msg) : RemoteActionResult.Fail(400, msg);
            }

            // If ports were modified while remote API & UPnP are active, re-align remote API port below lowest server port if needed.
            _ = Task.Run(() => RemoteApiManager.Instance.SynchronizeFromSettingsAsync(allowUpnpDiscovery: true));

            return RemoteActionResult.Ok($"Updated {updatedCount} server.properties setting(s).");
        }

        public IReadOnlyList<BackupArchiveDto> ListBackups()
        {
            string backupRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
            if (!Directory.Exists(backupRoot))
            {
                return Array.Empty<BackupArchiveDto>();
            }

            var list = new List<BackupArchiveDto>();
            try
            {
                foreach (string path in Directory.GetFiles(backupRoot, "*.*", SearchOption.TopDirectoryOnly))
                {
                    var info = new FileInfo(path);
                    string name = info.Name;
                    string category = "Other";
                    if (name.StartsWith("Worlds_Backup_", StringComparison.OrdinalIgnoreCase)) category = "Worlds";
                    else if (name.StartsWith("Packs_Backup_", StringComparison.OrdinalIgnoreCase)) category = "Packs";
                    else if (name.StartsWith("Server_Backup_", StringComparison.OrdinalIgnoreCase)) category = "Server";

                    list.Add(new BackupArchiveDto
                    {
                        FileName = name,
                        Type = category,
                        SizeBytes = info.Length,
                        CreatedUtc = info.CreationTimeUtc
                    });
                }
            }
            catch { }

            return list.OrderByDescending(b => b.CreatedUtc).ToList();
        }

        public async Task<RemoteActionResult> StartBackupAsync(string backupType, bool stopServerFirst, CancellationToken ct)
        {
            string normalized = string.IsNullOrWhiteSpace(backupType) ? "worlds" : backupType.Trim().ToLowerInvariant();
            int mode = normalized switch
            {
                "worlds" or "world" or "1" => 1,
                "packs" or "pack" or "2" => 2,
                "server" or "full" or "3" => 3,
                _ => -1
            };

            if (mode < 0)
            {
                return RemoteActionResult.Fail(400, $"Unsupported backup type '{backupType}'. Expected: worlds, packs, or server.");
            }

            string serverDir = AppSettings.Instance?.General?.ServerExecutablePath ?? string.Empty;
            if (string.IsNullOrWhiteSpace(serverDir) || !Directory.Exists(serverDir))
            {
                return RemoteActionResult.Fail(400, "Server directory is not configured.");
            }

            return await InvokeOnUiAsync(() =>
            {
                if (_backupWorker != null && _backupWorker.IsBusy)
                {
                    return RemoteActionResult.Fail(423, "A backup job is already running.");
                }

                _activeBackupType = mode;
                _suppressNextBackupExplorerPrompt = true;

                if (_serverProcess != null && !_serverProcess.HasExited)
                {
                    if (!stopServerFirst)
                    {
                        _suppressNextBackupExplorerPrompt = false;
                        return RemoteActionResult.Fail(409, "Server is running. Stop the server first or pass stopServerFirst=true.");
                    }

                    _isBackupPendingAfterStop = true;
                    BackupWorldBTN.Enabled = false;
                    BackupPacksBTN.Enabled = false;
                    BackupServerBTN.Enabled = false;

                    AppendTerminalLine($"[REMOTE API] Stopping server cleanly before '{normalized}' backup...");
                    _serverProcess.StandardInput.WriteLine("stop");
                    return RemoteActionResult.Accepted($"Stopping server and queuing '{normalized}' backup.");
                }

                AppendTerminalLine($"[REMOTE API] Starting background '{normalized}' backup...");
                ExecuteBackgroundBackupPipeline();
                return RemoteActionResult.Accepted($"Started '{normalized}' backup job.");
            }).ConfigureAwait(false);
        }

        public RemoteConfigSummaryDto GetConfigSummary()
        {
            var cfg = AppSettings.Instance;
            return new RemoteConfigSummaryDto
            {
                ConfigVersion = cfg.Config.ConfigVersion,
                EnableUPnP = cfg.Networking.EnableUPnP,
                ConsolidateUdpPorts = cfg.Networking.ConsolidateUdpPorts,
                HideTelemetryWarnings = cfg.General.HideTelemetryWarnings,
                WorldEditorEnabled = cfg.General.WorldEditorEnabled,
                CompressionMode = cfg.General.CompressionMode,
                AutoModEnabled = cfg.AutoMod.Enabled,
                AutoModChatEnforcement = cfg.AutoMod.EnableChatEnforcement,
                AutoModAntiSpam = cfg.AutoMod.AntiSpamEnabled,
                RemoteApiEnabled = cfg.RemoteApi.Enabled
            };
        }

        public RemoteConfigSummaryDto UpdateConfigToggles(AutoModTogglePatch patch)
        {
            if (patch != null)
            {
                AppSettings.Instance.Update(cfg =>
                {
                    if (patch.Enabled.HasValue) cfg.AutoMod.Enabled = patch.Enabled.Value;
                    if (patch.EnableChatEnforcement.HasValue) cfg.AutoMod.EnableChatEnforcement = patch.EnableChatEnforcement.Value;
                    if (patch.AntiSpamEnabled.HasValue) cfg.AutoMod.AntiSpamEnabled = patch.AntiSpamEnabled.Value;
                    if (patch.HideTelemetryWarnings.HasValue) cfg.General.HideTelemetryWarnings = patch.HideTelemetryWarnings.Value;
                });
            }

            return GetConfigSummary();
        }

        public async Task<RemoteUpnpStatusDto> GetUpnpStatusAsync(bool runVerification, CancellationToken ct)
        {
            var networking = AppSettings.Instance?.Networking;
            var remoteApi = AppSettings.Instance?.RemoteApi;
            string serverRoot = AppSettings.Instance?.General?.ServerExecutablePath ?? string.Empty;
            ServerPortSnapshot portSnap = ServerPortDiscovery.ReadServerPortSnapshot(serverRoot);

            string localIp = UPnPEngine.GetLocalIPAddress();
            string externalIp = await UPnPEngine.GetExternalIPAddressAsync(ct).ConfigureAwait(false);
            UpnpGatewayClient gateway = networking != null && networking.EnableUPnP
                ? await UPnPEngine.DiscoverGatewayAsync(TimeSpan.FromMilliseconds(2000), cancellationToken: ct).ConfigureAwait(false)
                : null;

            var dto = new RemoteUpnpStatusDto
            {
                UpnpEnabled = networking?.EnableUPnP ?? false,
                GatewayFound = gateway != null,
                GatewayDescription = gateway?.FriendlyName ?? string.Empty,
                ExternalIp = externalIp,
                LocalIp = localIp,
                ApiPort = RemoteApiManager.Instance.BoundPort
            };

            if (gateway != null)
            {
                try
                {
                    var all = await gateway.GetAllPortMappingsAsync(cancellationToken: ct).ConfigureAwait(false);
                    foreach (var m in all)
                    {
                        dto.ActiveGatewayMappings.Add(new RemoteUpnpMappingSummaryDto
                        {
                            ExternalPort = m.ExternalPort,
                            InternalPort = m.InternalPort,
                            Protocol = m.Protocol == UpnpProtocol.Tcp ? "TCP" : "UDP",
                            InternalClient = m.InternalClient,
                            Description = m.Description,
                            Enabled = m.Enabled
                        });
                    }
                }
                catch { }
            }

            if (runVerification)
            {
                var verifyOpts = new PortVerificationOptions
                {
                    Gateway = gateway,
                    ExternalProbeUrl = networking?.ResolveEffectiveExternalProbeUrl() ?? string.Empty,
                    PublicExternalIpOverride = externalIp,
                    LocalIpOverride = localIp
                };

                dto.PrimaryPortVerification = await UPnPEngine.VerifyPortAsync(
                    portSnap.PrimaryPortV4,
                    portSnap.PrimaryPortV4,
                    "UDP",
                    verifyOpts,
                    ct).ConfigureAwait(false);

                if (remoteApi != null && remoteApi.Enabled && RemoteApiManager.Instance.BoundPort > 0)
                {
                    int apiPort = RemoteApiManager.Instance.BoundPort;
                    dto.ApiPortVerification = await UPnPEngine.VerifyPortAsync(
                        apiPort,
                        apiPort,
                        "TCP",
                        verifyOpts,
                        ct).ConfigureAwait(false);
                }
            }
            else
            {
                dto.ApiPortVerification = RemoteApiManager.Instance.LastVerification;
            }

            return dto;
        }

        private static string GetServerFilePath(string fileName)
        {
            string serverDir = AppSettings.Instance?.General?.ServerExecutablePath;
            if (string.IsNullOrWhiteSpace(serverDir)) return string.Empty;
            return Path.Combine(serverDir, fileName);
        }

        private static List<WhitelistPlayerEntry> ReadRawAllowlistLocked(string allowlistPath)
        {
            if (!File.Exists(allowlistPath)) return new List<WhitelistPlayerEntry>();
            try
            {
                string raw = File.ReadAllText(allowlistPath, Encoding.UTF8).Trim();
                if (string.IsNullOrEmpty(raw)) return new List<WhitelistPlayerEntry>();
                return JsonSerializer.Deserialize<List<WhitelistPlayerEntry>>(raw, JsonHelper.CompactOptions)
                       ?? new List<WhitelistPlayerEntry>();
            }
            catch
            {
                return new List<WhitelistPlayerEntry>();
            }
        }
    }
}
