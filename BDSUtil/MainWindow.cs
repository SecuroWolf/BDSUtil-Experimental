using BDSUtil.Helpers;
using BDSUtil.Modals;
using BDSUtil.UI;
using Material3.WinForms.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BDSUtil
{
    public partial class MainWindow : Material3Form
    {
        private Process _serverProcess;
        private string _currentLogPath;
        private bool _isServerFullyInitialized = false;
        private int _previousActiveTabIndex = 0;
        private bool _isRedirecting = false;
        private bool _isSkippingTelemetryBlock = false;

        private bool _isBackupPendingAfterStop = false;
        private System.ComponentModel.BackgroundWorker _backupWorker;
        private int _activeBackupType = 0;
        private string _backupDestinationRoot = string.Empty;
        private readonly string _winRarRegistryPath = @"C:\Program Files\WinRAR\WinRAR.exe";

        private bool _forceSystemExit = false;
        private bool _isUpnpCleanupComplete = false;

        public MainWindow()
        {
            InitializeComponent();
            SyncTitleBarMetadata();

            Material3AppTheme.StyleNativeTextBox(ConsoleOutputTextBox, recessedContainer: true);
            Material3NavigationHelper.Bind(
                MainNavDrawer,
                MaterialTabControl1,
                ("Server Console", MaterialIcons.Play),
                ("Backup & Restore", MaterialIcons.Restore),
                ("Chat AutoMod", MaterialIcons.Tips),
                ("Level Editor", MaterialIcons.Layers),
                ("Settings", MaterialIcons.Settings));

            _previousActiveTabIndex = 0;
            MaterialTabControl1.SelectedIndex = 0;

            InitializeBackupEngine();

            if (tabLiveMonitor != null)
            {
                tabLiveMonitor.Resize += (s, e) => LayoutConsoleControls();
            }
            LayoutConsoleControls();
        }

        private void LayoutConsoleControls()
        {
            if (tabLiveMonitor == null || StartServerBTN_New == null)
            {
                return;
            }

            int tabW = tabLiveMonitor.ClientSize.Width;
            int tabH = tabLiveMonitor.ClientSize.Height;
            if (tabW <= 100 || tabH <= 100)
            {
                return;
            }

            const int rightMargin = 20;
            const int bottomMargin = 20;
            const int btnSize = 72;
            const int powerSize = 74;
            const int gap = 12;

            int btnY = tabH - bottomMargin - btnSize;
            int puzzleX = tabW - rightMargin - btnSize;
            int wrenchX = puzzleX - gap - btnSize;
            int powerY = btnY - gap - powerSize;
            int powerX = (wrenchX + puzzleX + btnSize - powerSize) / 2;

            StartServerBTN_New.Location = new Point(powerX, powerY);
            StartServerBTN_New.Size = new Size(powerSize, powerSize);
            StartServerBTN_New.ElevationLevel = 1;
            StartServerBTN_New.IconSize = 32;

            StopServerBTN_New.Location = new Point(powerX, powerY);
            StopServerBTN_New.Size = new Size(powerSize, powerSize);
            StopServerBTN_New.ElevationLevel = 1;
            StopServerBTN_New.IconSize = 32;

            SvConfigButton.Location = new Point(wrenchX, btnY);
            SvConfigButton.Size = new Size(btnSize, btnSize);
            SvConfigButton.ElevationLevel = 1;
            SvConfigButton.IconSize = 30;

            SV_PM_BTN.Location = new Point(puzzleX, btnY);
            SV_PM_BTN.Size = new Size(btnSize, btnSize);
            SV_PM_BTN.ElevationLevel = 1;
            SV_PM_BTN.IconSize = 30;

            int contentW = Math.Max(200, wrenchX - 14 - gap);

            if (CommandInputTextBox != null)
            {
                int inputY = tabH - bottomMargin - CommandInputTextBox.Height;
                CommandInputTextBox.Location = new Point(14, inputY);
                CommandInputTextBox.Width = contentW;
            }

            if (ConsoleOutputTextBox != null)
            {
                int inputTop = CommandInputTextBox != null ? CommandInputTextBox.Top : (tabH - 70);
                int consoleH = Math.Max(100, inputTop - 14 - 10);
                ConsoleOutputTextBox.Location = new Point(14, 14);
                ConsoleOutputTextBox.Size = new Size(contentW, consoleH);
            }
        }

        private async void MainWindow_Load(object sender, EventArgs e)
        {

            // --- BDS CORE STARTUP UPDATE CHECK ---
            string serverRoot = AppSettings.Instance?.General.ServerExecutablePath;
            if (!string.IsNullOrWhiteSpace(serverRoot) && Directory.Exists(serverRoot))
            {
                if (ServerPropertiesDocument.CommentOutServerPortV6(serverRoot))
                {
                    AppendTerminalLine("[MANAGER INFO] Commented out 'server-portv6' in server.properties (ignored under NetherNet).");
                }

                string currentVersion = BDSUtil.Helpers.Updates.ServerUpdates.GetServerVersion(serverRoot);
                var updateInfo = await BDSUtil.Helpers.Updates.ServerUpdates.DoUpdateCheck(serverRoot);

                if (updateInfo.UpdateAvailable)
                {
                    DialogResult updateResult = MaterialDialogBox.Show(
                        this,
                        $"A new version of the Bedrock Dedicated Server is available.\n\n" +
                        $"• Current Version: v{currentVersion}\n" +
                        $"• Latest Version: v{updateInfo.LatestVersion}\n\n" +
                        $"Would you like to backup your current server and install the update now?\n",
                        "Update Available",
                        MessageBoxButtons.YesNo
                    );

                    if (updateResult == DialogResult.Yes)
                    {
                        using (var updateForm = new BDSUtil.Modals.ServerUpdateForm(serverRoot, updateInfo.LatestVersion, updateInfo.DownloadUrl))
                        {
                            updateForm.ShowDialog(this);
                        }
                    }
                }
            }



            UpdateControlButtonStates();
            LayoutConsoleControls();

            BDSUtil.Helpers.RemoteApi.RemoteApiManager.Instance.AttachHost(this, msg => AppendTerminalLine(msg));

            //#################################################
            //##            UPnP NETWORK MAPPING             ##
            //#################################################
            if (AppSettings.Instance.Networking.EnableUPnP)
            {
                AppendTerminalLine("[UPnP] Setting up ports...");

                this.Invoke((MethodInvoker)delegate
                {
                    StartServerBTN_New.Visible = false;
                });

                _ = Task.Run(async () =>
                {
                    void LogSafe(string msg)
                    {
                        if (this.IsDisposed || !this.IsHandleCreated) return;
                        try
                        {
                            this.Invoke((MethodInvoker)delegate { AppendTerminalLine(msg); });
                        }
                        catch { }
                    }

                    void RestoreStartButtonSafe()
                    {
                        if (this.IsDisposed || !this.IsHandleCreated) return;
                        try
                        {
                            this.Invoke((MethodInvoker)delegate { StartServerBTN_New.Visible = true; });
                        }
                        catch { }
                    }

                    try
                    {
                        string serverDirectory = AppSettings.Instance.General.ServerExecutablePath;
                        ServerPropertiesDocument.CommentOutServerPortV6(serverDirectory);
                        var portSnapshot = BDSUtil.Helpers.RemoteApi.ServerPortDiscovery.ReadServerPortSnapshot(serverDirectory);
                        bool remoteApiEnabled = AppSettings.Instance.RemoteApi.Enabled;

                        Task<string> externalIpTask = UPnPEngine.GetExternalIPAddressAsync();
                        Task<UpnpGatewayClient> gatewayTask = UPnPEngine.DiscoverGatewayAsync(TimeSpan.FromMilliseconds(2500), forceRefresh: true);
                        await Task.WhenAll(externalIpTask, gatewayTask).ConfigureAwait(false);

                        string externalPublicIp = externalIpTask.Result;
                        UpnpGatewayClient gateway = gatewayTask.Result;

                        string localSubnetIp = gateway != null
                            ? UPnPEngine.GetLocalIpForGateway(gateway)
                            : UPnPEngine.GetLocalIPAddress();

                        var hostIpSet = new HashSet<string>(UPnPEngine.GetLocalIPAddresses(), StringComparer.OrdinalIgnoreCase)
                        {
                            localSubnetIp,
                            UPnPEngine.GetLocalIPAddress()
                        };

                        if (gateway != null && !UPnPEngine.IsValidPublicIPv4(externalPublicIp))
                        {
                            try
                            {
                                string gatewayWanIp = await gateway.GetExternalIPAddressAsync().ConfigureAwait(false);
                                if (UPnPEngine.IsValidPublicIPv4(gatewayWanIp))
                                {
                                    externalPublicIp = gatewayWanIp;
                                }
                            }
                            catch { }
                        }

                        IReadOnlyList<UpnpPortMapping> existingMappings = null;
                        var activeTcpMappings = new Dictionary<int, UpnpPortMapping>();

                        if (gateway != null)
                        {
                            try
                            {
                                existingMappings = await gateway.GetAllPortMappingsAsync().ConfigureAwait(false);
                                foreach (var mapping in existingMappings)
                                {
                                    if (mapping.Protocol == UpnpProtocol.Tcp)
                                    {
                                        activeTcpMappings[mapping.ExternalPort] = mapping;
                                    }
                                }
                            }
                            catch
                            {
                                existingMappings = null;
                            }
                        }

                        int apiPort = 0;
                        if (remoteApiEnabled)
                        {
                            apiPort = await BDSUtil.Helpers.RemoteApi.RemoteApiManager.Instance.BindListenerForBatchUpnpAsync(
                                AppSettings.Instance.RemoteApi,
                                portSnapshot,
                                activeTcpMappings.Count > 0 ? activeTcpMappings : null,
                                hostIpSet,
                                localSubnetIp,
                                externalPublicIp).ConfigureAwait(false);
                        }
                        else
                        {
                            await BDSUtil.Helpers.RemoteApi.RemoteApiManager.Instance.StopAsync(releaseUpnpLease: false).ConfigureAwait(false);
                        }

                        var plannedPorts = new List<UpnpPortSpec>();
                        bool consolidateUdpPorts = AppSettings.Instance?.Networking?.ConsolidateUdpPorts ?? false;

                        if (portSnapshot.PropertiesFileExists)
                        {
                            plannedPorts.AddRange(UPnPEngine.PlanGameServerPorts(
                                portSnapshot.PrimaryPortV4,
                                portSnapshot.MaxPlayers,
                                consolidateUdpPorts,
                                excludedApiPort: apiPort,
                                existingMappings: existingMappings,
                                hostIps: hostIpSet));
                        }

                        if (remoteApiEnabled && apiPort > 0)
                        {
                            plannedPorts.Add(new UpnpPortSpec
                            {
                                ExternalPort = apiPort,
                                InternalPort = apiPort,
                                Protocol = UpnpProtocol.Tcp,
                                Label = "Remote Management API",
                                Description = $"BDSUtil Remote API {apiPort}",
                                IsGameUdpNode = false
                            });
                        }

                        if (plannedPorts.Count == 0)
                        {
                            LogSafe("[UPnP INFO] server.properties not yet linked; skipping game server port mappings.");
                            return;
                        }

                        LogSafe("[UPnP] Ports queued for requisition & verification:");
                        if (portSnapshot.PropertiesFileExists)
                        {
                            LogSafe($"  - {portSnapshot.PrimaryPortV4} (TCP, UDP) [Server Port]");
                            var udpNodePorts = new List<int>();
                            foreach (UpnpPortSpec spec in plannedPorts)
                            {
                                if (spec.IsGameUdpNode)
                                {
                                    udpNodePorts.Add(spec.ExternalPort);
                                }
                            }
                            if (udpNodePorts.Count > 0)
                            {
                                LogSafe($"  - {string.Join(", ", udpNodePorts)} (UDP) [server-udp-ports]");
                            }
                        }
                        if (remoteApiEnabled && apiPort > 0)
                        {
                            LogSafe($"  - {apiPort} (TCP) [Remote Management API]");
                        }

                        LogSafe("[UPnP] Verifying...");

                        var batch = await UPnPEngine.RequisitionAndVerifyPortsBatchAsync(
                            plannedPorts,
                            new PortVerificationOptions
                            {
                                Gateway = gateway,
                                SkipGatewayDiscovery = gateway == null,
                                LocalIpOverride = localSubnetIp,
                                PublicExternalIpOverride = UPnPEngine.IsValidPublicIPv4(externalPublicIp) ? externalPublicIp : null,
                                ExternalProbeUrl = AppSettings.Instance.Networking.ResolveEffectiveExternalProbeUrl()
                            },
                            existingMappingsOverride: existingMappings).ConfigureAwait(false);

                        if (remoteApiEnabled && apiPort > 0)
                        {
                            UpnpPortSetupResult apiOutcome = null;
                            foreach (UpnpPortSetupResult result in batch.PortResults)
                            {
                                if (!result.IsGameUdpNode && result.ExternalPort == apiPort && result.Protocol == UpnpProtocol.Tcp)
                                {
                                    apiOutcome = result;
                                    break;
                                }
                            }

                            await BDSUtil.Helpers.RemoteApi.RemoteApiManager.Instance.ApplyBatchUpnpOutcomeAsync(
                                portSnapshot.LowestServerPort,
                                batch.LocalIp,
                                batch.PublicExternalIp,
                                batch.GatewayFound,
                                apiOutcome).ConfigureAwait(false);
                        }

                        if (portSnapshot.PropertiesFileExists && batch.SuccessfulUdpNodeTokens.Count > 0)
                        {
                            List<string> tokens = batch.SuccessfulUdpNodeTokens.Count > 16
                                ? batch.SuccessfulUdpNodeTokens.GetRange(0, 16)
                                : batch.SuccessfulUdpNodeTokens;

                            ServerPropertiesDocument.SetServerUdpPorts(portSnapshot.PropertiesFilePath, string.Join(",", tokens));
                            UPnPEngine.RecordActiveGameServerMapping(portSnapshot.PrimaryPortV4, tokens);
                        }

                        if (batch.AllPortsVerified)
                        {
                            LogSafe($"[UPNP Success] {batch.SummaryMessage}");
                        }
                        else
                        {
                            LogSafe($"[UPnP FAILED] {batch.SummaryMessage}");
                        }
                    }
                    catch (Exception ex)
                    {
                        LogSafe($"[UPnP SYSTEM EXCEPTION] Boot pipeline failure: {ex.Message}");
                    }
                    finally
                    {
                        RestoreStartButtonSafe();
                    }
                });
            }
            else if (AppSettings.Instance.RemoteApi.Enabled)
            {
                _ = Task.Run(async () =>
                {
                    var localApiRes = await BDSUtil.Helpers.RemoteApi.RemoteApiManager.Instance.SynchronizeFromSettingsAsync(allowUpnpDiscovery: false).ConfigureAwait(false);
                    if (!this.IsDisposed && this.IsHandleCreated)
                    {
                        try { this.BeginInvoke((MethodInvoker)delegate { AppendTerminalLine($"[REMOTE API] {localApiRes.Message}"); }); } catch { }
                    }
                });
            }
        }

        public static void ExecuteUpnpTeardown()
        {
            try
            {
                string serverDirectory = AppSettings.Instance?.General.ServerExecutablePath;
                if (!string.IsNullOrWhiteSpace(serverDirectory))
                {
                    string propertiesFilePath = System.IO.Path.Combine(serverDirectory, "server.properties");
                    ServerPropertiesDocument.CommentOutProperty(propertiesFilePath, "server-udp-ports");
                }

                try { BDSUtil.Helpers.RemoteApi.RemoteApiManager.Instance.StopAsync(releaseUpnpLease: true).GetAwaiter().GetResult(); } catch { }
                BDSUtil.Helpers.UPnPEngine.TearDownPorts();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failover restart network cleanup violation: {ex.Message}");
            }
        }

        private void UpdateControlButtonStates()
        {
            string configuredPath = AppSettings.Instance?.General.ServerExecutablePath;
            bool isPathConfigured = false;

            if (!string.IsNullOrWhiteSpace(configuredPath))
            {
                if (configuredPath.EndsWith("bedrock_server.exe", StringComparison.OrdinalIgnoreCase))
                {
                    configuredPath = Path.GetDirectoryName(configuredPath);
                }

                if (!string.IsNullOrEmpty(configuredPath) && Directory.Exists(configuredPath))
                {
                    string absoluteBinaryTarget = Path.Combine(configuredPath, "bedrock_server.exe");
                    string absolutePropertiesTarget = Path.Combine(configuredPath, "server.properties");

                    isPathConfigured = File.Exists(absoluteBinaryTarget) && File.Exists(absolutePropertiesTarget);
                }
            }

            bool isProcessRunning = _serverProcess != null && !_serverProcess.HasExited;

            if (isProcessRunning)
            {
                StartServerBTN_New.Visible = false;
                StopServerBTN_New.Visible = _isServerFullyInitialized;
            }
            else
            {
                StartServerBTN_New.Visible = true;
                StopServerBTN_New.Visible = false;
                _isServerFullyInitialized = false;
            }
        }

        private void InitializeBackupEngine()
        {
            _backupWorker = new System.ComponentModel.BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = false
            };

            _backupWorker.DoWork += BackupWorker_DoWork;
            _backupWorker.ProgressChanged += BackupWorker_ProgressChanged;
            _backupWorker.RunWorkerCompleted += BackupWorker_RunWorkerCompleted;
        }

        private void BackupWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            BackupProgressBar.Value = e.ProgressPercentage;
        }

        private void CommandInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string cmd = CommandInputTextBox.Text;

                if (string.IsNullOrWhiteSpace(cmd)) return;

                if (_serverProcess != null && !_serverProcess.HasExited)
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                    AppendTerminalLine($"[{timestamp} CONSOLE] {cmd}");
                    _serverProcess.StandardInput.WriteLine(cmd);
                    CommandInputTextBox.Text = string.Empty;
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void CommandInputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Outputs log text strings to the terminal box while monitoring for explicit server initialization flags.
        /// Catches and translates raw noisy Bedrock telemetry into clean UI displays dynamically.
        /// </summary>
        private void AppendTerminalLine(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            TrackPlayerConnectionLine(text);

            // Intercept player connection to enforce bans instantly on join
            if (text.Contains("Player connected:"))
            {
                try
                {
                    // Format: "Player connected: Wolfe4322, xuid: 2535467664359860"
                    string payload = text.Substring(text.IndexOf("Player connected:") + 17).Trim();
                    string[] parts = payload.Split(new[] { ", xuid:" }, StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length >= 2)
                    {
                        string gamertag = parts[0].Trim();
                        string xuid = parts[1].Trim();

                        // Fire the authoritative C# ban enforcer
                        BDSUtil.Helpers.AutoMod.TelemetryInterceptor.EnforceProfileOnJoin(gamertag, xuid, _serverProcess);
                    }
                }
                catch { }
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(AppendTerminalLine), text);
                return;
            }

            // =========================================================================
            // --- BDSUTIL_BRIDGE REAL-TIME PARSING & INTERCEPTION ENGINE ---
            // =========================================================================
            if (text.Contains("[BDSUTIL_BRIDGE]"))
            {
                ProcessTelemetryLine(text);

                // Write the raw payload to the internal .log file for diagnostic history, 
                // but SKIP pushing the raw noisy string to the UI console
                try { File.AppendAllText(_currentLogPath, text + Environment.NewLine); } catch { }
                return;
            }

            // Suppress the initial noisy pack discovery broadcast from the Bedrock engine
            if (text.Contains("[Scripting][verbose]-Plugin Discovered [bdsutil_monitor]"))
            {
                try { File.AppendAllText(_currentLogPath, text + Environment.NewLine); } catch { }
                return;
            }

            // 1. Outright drop the LevelDB AutoCompaction spam
            if (text.Contains("Running AutoCompaction..."))
            {
                try { File.AppendAllText(_currentLogPath, text + Environment.NewLine); } catch { }
                return;
            }

            // 2. Stateful block interceptor for the multi-line Telemetry warning
            if (AppSettings.Instance.General.HideTelemetryWarnings)
            {
                // Detect the start of the block
                if (text.Contains("======== TELEMETRY MESSAGE ========"))
                {
                    _isSkippingTelemetryBlock = true;
                    try { File.AppendAllText(_currentLogPath, text + Environment.NewLine); } catch { }
                    return;
                }

                // If we are currently inside the block, swallow the line
                if (_isSkippingTelemetryBlock)
                {
                    try { File.AppendAllText(_currentLogPath, text + Environment.NewLine); } catch { }

                    // Detect the end of the block (a line of just equals signs with no words)
                    if (text.Contains("=============================") && !text.Contains("TELEMETRY"))
                    {
                        _isSkippingTelemetryBlock = false; // Turn the filter back off for normal logs
                    }
                    return;
                }
            }

            // Write to UI
            RecordConsoleEntry(text);
            ConsoleOutputTextBox.AppendText(text + Environment.NewLine);
            ConsoleOutputTextBox.SelectionStart = ConsoleOutputTextBox.Text.Length;
            ConsoleOutputTextBox.ScrollToCaret();

            // --- SERVER INITIALIZATION FLAG DETECTION ---
            if (text.IndexOf("Server Started.", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                _isServerFullyInitialized = true;
                UpdateControlButtonStates();
            LayoutConsoleControls();
            }

            try
            {
                File.AppendAllText(_currentLogPath, text + Environment.NewLine);
            }
            catch { }

            // --- LIVE PLAYER IDENTITY STREAM CAPTURE ---
            if (text.IndexOf("Player connected:", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                try
                {
                    int labelLen = "Player connected:".Length;
                    int nameStartIndex = text.IndexOf("Player connected:") + labelLen;
                    int commaIndex = text.IndexOf(",", nameStartIndex);
                    int xuidLabelIndex = text.IndexOf("xuid:", commaIndex);
                    int xuidStartIndex = xuidLabelIndex + "xuid:".Length;

                    if (commaIndex > nameStartIndex && xuidLabelIndex > 0)
                    {
                        string gamertag = text.Substring(nameStartIndex, commaIndex - nameStartIndex).Trim();
                        string xuidValue = text.Substring(xuidStartIndex).Trim();

                        if (!string.IsNullOrEmpty(xuidValue) && !string.IsNullOrEmpty(gamertag))
                        {
                            BDSUtil.Modals.Content_Controls.CfgHelper.PlayerIdentityStore.SavePlayerNode(xuidValue, gamertag);
                        }
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// Extracts and cleans raw JSON telemetry strings coming from the console output interceptor, 
        /// pushing beautifully formatted strings into the main UI terminal.
        /// </summary>
        private void ProcessTelemetryLine(string text)
        {
            try
            {
                // 1. REAL-TIME CHAT INTERCEPTION & MODERATION VECTOR
                if (text.Contains("[CHAT]"))
                {
                    string player = JsonHelper.ExtractBridgeTokenValue(text, "player=");
                    string message = JsonHelper.ExtractBridgeTokenValue(text, "message=");

                    if (!string.IsNullOrEmpty(player) && !string.IsNullOrEmpty(message))
                    {
                        // Clean, readable translation pushed to UI directly
                        string formattedChat = $"[AutoMod] 💬 {player}: {message}";
                        RecordConsoleEntry(formattedChat);
                        ConsoleOutputTextBox.AppendText(formattedChat + "\r\n");
                        ConsoleOutputTextBox.SelectionStart = ConsoleOutputTextBox.Text.Length;
                        ConsoleOutputTextBox.ScrollToCaret();

                        // Hand over to the autonomous evaluation array
                        EvaluateAutoModChatRules(player, message);
                    }
                }
                // 2. REAL-TIME DEATH & PVP GRIEF TELEMETRY CAPTURE
                else if (text.Contains("[DEATH][PVP]"))
                {
                    string victim = JsonHelper.ExtractBridgeTokenValue(text, "victim=");
                    string killer = JsonHelper.ExtractBridgeTokenValue(text, "killer=");
                    string cause = JsonHelper.ExtractBridgeTokenValue(text, "cause=");

                    if (!string.IsNullOrEmpty(killer) && !string.IsNullOrEmpty(victim))
                    {
                        string formattedPvp = $"[AutoMod] ⚔️ PVP: {killer} killed {victim} ({cause})";
                        RecordConsoleEntry(formattedPvp);
                        ConsoleOutputTextBox.AppendText(formattedPvp + "\r\n");
                        ConsoleOutputTextBox.SelectionStart = ConsoleOutputTextBox.Text.Length;
                        ConsoleOutputTextBox.ScrollToCaret();

                        if (_serverProcess != null && !_serverProcess.HasExited)
                        {
                            _serverProcess.StandardInput.WriteLine($"say §c[Security Hub] Player {killer} killed {victim} using {cause}!");
                        }
                    }
                }
                // 3. ENVIRONMENT DEATH CAPTURE
                else if (text.Contains("[DEATH][ENV]"))
                {
                    string victim = JsonHelper.ExtractBridgeTokenValue(text, "victim=");
                    string source = JsonHelper.ExtractBridgeTokenValue(text, "source=");
                    string cause = JsonHelper.ExtractBridgeTokenValue(text, "cause=");

                    if (!string.IsNullOrEmpty(victim))
                    {
                        string formattedEnv = $"[AutoMod] 💀 ENV: {victim} died to {source} ({cause})";
                        RecordConsoleEntry(formattedEnv);
                        ConsoleOutputTextBox.AppendText(formattedEnv + "\r\n");
                        ConsoleOutputTextBox.SelectionStart = ConsoleOutputTextBox.Text.Length;
                        ConsoleOutputTextBox.ScrollToCaret();
                    }
                }
                // 4. CLEAN INITIALIZATION FEEDBACK
                else if (text.Contains("[INIT]"))
                {
                    int initIndex = text.IndexOf("[INIT]");
                    string cleanInit = text.Substring(initIndex).Replace("[INIT]", "").Trim();

                    string formattedInit = $"[AutoMod] ⚙️ {cleanInit}";
                    RecordConsoleEntry(formattedInit);
                    ConsoleOutputTextBox.AppendText(formattedInit + "\r\n");
                    ConsoleOutputTextBox.SelectionStart = ConsoleOutputTextBox.Text.Length;
                    ConsoleOutputTextBox.ScrollToCaret();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Bridge Dynamic Parser Exception]: {ex.Message}");
            }
        }

        /// <summary>
        /// Forwards real-time chat strings to the background AutoMod Telemetry Interceptor engine.
        /// </summary>
        private void EvaluateAutoModChatRules(string player, string message)
        {
            // Fire the payload into the background thread pool interceptor.
            // We pass an anonymous Action delegate that safely Invokes the UI thread to print action logs.
            BDSUtil.Helpers.AutoMod.TelemetryInterceptor.EvaluateChatEventAsync(
                player,
                message,
                _serverProcess,
                (logActionStr) =>
                {
                    // This callback allows the background thread to safely print to the UI Console 
                    // without causing a Cross-Thread Exception.
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            ConsoleOutputTextBox.AppendText(logActionStr + "\r\n");
                            ConsoleOutputTextBox.SelectionStart = ConsoleOutputTextBox.Text.Length;
                            ConsoleOutputTextBox.ScrollToCaret();
                        }));
                    }
                    else
                    {
                        ConsoleOutputTextBox.AppendText(logActionStr + "\r\n");
                        ConsoleOutputTextBox.SelectionStart = ConsoleOutputTextBox.Text.Length;
                        ConsoleOutputTextBox.ScrollToCaret();
                    }
                }
            );
        }

        private void ServerProcess_Exited(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(ServerProcess_Exited), sender, e);
                return;
            }

            _isServerFullyInitialized = false;
            ClearTrackedOnlinePlayers();
            UpdateControlButtonStates();
            LayoutConsoleControls();

            ConsoleOutputTextBox.AppendText(Environment.NewLine + "==================================================" + Environment.NewLine + "Server has shutdown successfully." + Environment.NewLine + "==================================================" + Environment.NewLine);

            ConsoleOutputTextBox.SelectionStart = ConsoleOutputTextBox.Text.Length;
            ConsoleOutputTextBox.ScrollToCaret();

            if (_isBackupPendingAfterStop)
            {
                _isBackupPendingAfterStop = false;
                ExecuteBackgroundBackupPipeline();
            }
        }

        private void LaunchModalAndRestoreTab(Func<Form> formFactory)
        {
            int snapshotIndex = _previousActiveTabIndex;
            DialogResult outcomeResult;

            this.Hide();
            using (Form modalWindow = formFactory.Invoke())
            {
                outcomeResult = modalWindow.ShowDialog(this);
            }
            this.Show();

            if (outcomeResult == DialogResult.Retry)
            {
                if (MaterialTabControl1 != null)
                {
                    int settingsIndex = MaterialTabControl1.TabCount - 1;

                    System.Windows.Forms.Timer redirectTimer = new System.Windows.Forms.Timer { Interval = 1 };
                    redirectTimer.Tick += (s, args) =>
                    {
                        redirectTimer.Stop();

                        _isRedirecting = true;
                        MaterialTabControl1.SelectedIndex = settingsIndex;
                        _isRedirecting = false;

                        MaterialTabControl1.Update();

                        if (this.settingsProvider1 != null)
                        {
                            this.settingsProvider1.InitializeControl();
                            this.settingsProvider1.ClearSelectionFocus();
                        }

                        redirectTimer.Dispose();
                    };
                    redirectTimer.Start();
                }
                return;
            }

            System.Windows.Forms.Timer restorationTimer = new System.Windows.Forms.Timer { Interval = 1 };
            restorationTimer.Tick += (s, args) =>
            {
                restorationTimer.Stop();
                MaterialTabControl1.SelectedIndex = snapshotIndex;
                MaterialTabControl1.Update();
                restorationTimer.Dispose();
            };
            restorationTimer.Start();
        }

        private void MaterialTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BKCompleteLBL != null) BKCompleteLBL.Visible = false;
            if (BackupProgressBar != null) BackupProgressBar.Visible = false;

            if (_isRedirecting) return;

            int activeIndex = MaterialTabControl1.SelectedIndex;

            if (activeIndex == 2)
            {
                LaunchModalAndRestoreTab(() => new AutoModWrapper(this));
                return;
            }

            if (activeIndex == 3)
            {
                if (AppSettings.Instance.General.WorldEditorEnabled)
                {
                    if (this.editorProvider1 != null)
                    {
                        this.editorProvider1.Visible = true;
                        this.editorProvider1.InitializeControl();
                        this.editorProvider1.ClearSelectionFocus();
                    }
                }
                else
                {
                    if (this.editorProvider1 != null)
                    {
                        this.editorProvider1.Visible = false;
                    }

                    EvaluateLevelEditorAccessAndRedirect();
                    return;
                }
            }

            int settingsTabIndex = MaterialTabControl1.TabCount - 1;
            if (activeIndex == settingsTabIndex)
            {
                if (this.settingsProvider1 != null)
                {
                    this.settingsProvider1.InitializeControl();
                    this.settingsProvider1.ClearSelectionFocus();
                }
            }

            _previousActiveTabIndex = activeIndex;
        }

        private void EvaluateLevelEditorAccessAndRedirect()
        {
            int snapshotIndex = _previousActiveTabIndex;
            int settingsIndex = MaterialTabControl1.TabCount - 1;

            DialogResult routingConsentResult = MaterialDialogBox.Show(
                this,
                "ACCESS RESTRICTED:\n\n" +
                "World Editor tools are currently locked down under global safety profile configurations.\n\n" +
                "Would you like to navigate directly to the Settings panel to authorize file transformations?\n\n",
                "Permissions Consent Required",
                MessageBoxButtons.YesNo
            );

            if (routingConsentResult == DialogResult.Yes)
            {
                System.Windows.Forms.Timer redirectTimer = new System.Windows.Forms.Timer { Interval = 1 };
                redirectTimer.Tick += (s, args) =>
                {
                    redirectTimer.Stop();

                    _isRedirecting = true;
                    MaterialTabControl1.SelectedIndex = settingsIndex;
                    _isRedirecting = false;

                    MaterialTabControl1.Update();

                    if (this.settingsProvider1 != null)
                    {
                        this.settingsProvider1.InitializeControl();
                        this.settingsProvider1.ClearSelectionFocus();
                    }

                    redirectTimer.Dispose();
                };
                redirectTimer.Start();
            }
            else
            {
                System.Windows.Forms.Timer restorationTimer = new System.Windows.Forms.Timer { Interval = 1 };
                restorationTimer.Tick += (s, args) =>
                {
                    restorationTimer.Stop();
                    MaterialTabControl1.SelectedIndex = snapshotIndex;
                    MaterialTabControl1.Update();
                    restorationTimer.Dispose();
                };
                restorationTimer.Start();
            }
        }


        //#############################################################################
        //############################ MAIN BACKUP PIPELINE ###########################
        //#############################################################################

        private void BackupWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var worker = sender as System.ComponentModel.BackgroundWorker;
            string serverDir = AppSettings.Instance.General.ServerExecutablePath;
            string timestamp = DateTime.Now.ToString("MM_dd_yyyy_HHmmss");
            int compressionType = AppSettings.Instance.General.CompressionMode;
            bool winRarExists = File.Exists(_winRarRegistryPath);

            if (!winRarExists && (compressionType == 0 || compressionType == 1))
            {
                compressionType = 2; // Auto-fallback lock if missing
            }

            string fileExtension = (compressionType == 0) ? ".rar" : ".zip";

            string stagingFolderName = "Worlds_Backup_";
            if (_activeBackupType == 2) stagingFolderName = "Packs_Backup_";
            else if (_activeBackupType == 3) stagingFolderName = "Server_Backup_";

            string archiveName = stagingFolderName + timestamp + fileExtension;
            string finalArchiveDestination = Path.Combine(_backupDestinationRoot, archiveName);

            worker?.ReportProgress(5);
            System.Threading.Thread.Sleep(1500);

            string tempBackupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp_bk_" + timestamp);

            try
            {
                if (_activeBackupType == 2)
                {
                    string[] targetPackFolders = new string[] { "behavior_packs", "resource_packs" };
                    bool foundAnyCustomPacks = false;

                    foreach (string packDirName in targetPackFolders)
                    {
                        string sourceRoot = Path.Combine(serverDir, packDirName);
                        if (!Directory.Exists(sourceRoot)) continue;

                        string[] customPackDirs = Directory.GetDirectories(sourceRoot, "custom_*", SearchOption.TopDirectoryOnly);

                        foreach (string packSourcePath in customPackDirs)
                        {
                            foundAnyCustomPacks = true;
                            string folderName = Path.GetFileName(packSourcePath);
                            string targetStagingRoot = Path.Combine(tempBackupPath, packDirName, folderName);

                            foreach (string filePath in Directory.GetFiles(packSourcePath, "*.*", SearchOption.AllDirectories))
                            {
                                string destFilePath = filePath.Replace(packSourcePath, targetStagingRoot);
                                string destFileDir = Path.GetDirectoryName(destFilePath);
                                if (!string.IsNullOrEmpty(destFileDir)) Directory.CreateDirectory(destFileDir);

                                using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    using (FileStream destStream = new FileStream(destFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                                    {
                                        sourceStream.CopyTo(destStream);
                                    }
                                }
                            }
                        }
                    }

                    if (!foundAnyCustomPacks)
                    {
                        throw new DirectoryNotFoundException("No active directory nodes starting with your prefix naming scheme 'custom_' were detected inside behavior or resource containers.");
                    }
                }
                else if (_activeBackupType == 3)
                {
                    string[] whitelistedItems = new string[]
                    {
                        "allowlist.json",
                        "bedrock_server.exe",
                        "packetlimitconfig.json",
                        "permissions.json",
                        "profanity_filter.wlist",
                        "server.properties",
                        "behavior_packs",
                        "config",
                        "data",
                        "definitions",
                        "resource_packs",
                        "development_behavior_packs",
                        "development_resource_packs",
                        "development_skin_packs",
                        "world_templates",
                        "worlds"
                    };

                    foreach (string item in whitelistedItems)
                    {
                        string sourceFullPath = Path.Combine(serverDir, item);
                        string targetStagingRoot = Path.Combine(tempBackupPath, item);

                        if (File.Exists(sourceFullPath))
                        {
                            string destFileDir = Path.GetDirectoryName(targetStagingRoot);
                            if (!string.IsNullOrEmpty(destFileDir)) Directory.CreateDirectory(destFileDir);

                            using (FileStream sourceStream = new FileStream(sourceFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (FileStream destStream = new FileStream(targetStagingRoot, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    sourceStream.CopyTo(destStream);
                                }
                            }
                        }
                        else if (Directory.Exists(sourceFullPath))
                        {
                            foreach (string filePath in Directory.GetFiles(sourceFullPath, "*.*", SearchOption.AllDirectories))
                            {
                                string destFilePath = filePath.Replace(sourceFullPath, targetStagingRoot);
                                string destFileDir = Path.GetDirectoryName(destFilePath);
                                if (!string.IsNullOrEmpty(destFileDir)) Directory.CreateDirectory(destFileDir);

                                using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    using (FileStream destStream = new FileStream(destFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                                    {
                                        sourceStream.CopyTo(destStream);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    string worldsSourceFolder = Path.Combine(serverDir, "worlds");
                    if (!Directory.Exists(worldsSourceFolder))
                    {
                        throw new DirectoryNotFoundException("The target 'worlds' directory could not be located inside the server root folder.");
                    }

                    foreach (string filePath in Directory.GetFiles(worldsSourceFolder, "*.*", SearchOption.AllDirectories))
                    {
                        string destFilePath = filePath.Replace(worldsSourceFolder, tempBackupPath);
                        string destFileDir = Path.GetDirectoryName(destFilePath);
                        if (!string.IsNullOrEmpty(destFileDir)) Directory.CreateDirectory(destFileDir);

                        using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (FileStream destStream = new FileStream(destFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                sourceStream.CopyTo(destStream);
                            }
                        }
                    }
                }

                worker?.ReportProgress(50);

                if (compressionType == 0 || compressionType == 1)
                {
                    string archiveFlag = (compressionType == 1) ? " -afzip" : "";
                    string winRarArgs = "a -ep1 -r -ibck -dh" + archiveFlag + " \"" + finalArchiveDestination + "\" \"" + tempBackupPath + "\\*\"";

                    var rarInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = _winRarRegistryPath,
                        Arguments = winRarArgs,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (System.Diagnostics.Process rarProcess = System.Diagnostics.Process.Start(rarInfo))
                    {
                        if (rarProcess != null)
                        {
                            int fauxProgress = 55;
                            while (!rarProcess.HasExited)
                            {
                                System.Threading.Thread.Sleep(200);
                                fauxProgress += 5;
                                if (fauxProgress >= 95) fauxProgress = 55;
                                worker?.ReportProgress(fauxProgress);
                            }
                            rarProcess.WaitForExit();
                        }
                    }
                }
                else
                {
                    System.IO.Compression.ZipFile.CreateFromDirectory(tempBackupPath, finalArchiveDestination);
                }
            }
            finally
            {
                if (Directory.Exists(tempBackupPath))
                {
                    Directory.Delete(tempBackupPath, true);
                }
            }

            worker?.ReportProgress(100);
            e.Result = archiveName;
        }

        private void BackupWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            BackupWorldBTN.Enabled = true;
            BackupPacksBTN.Enabled = true;
            BackupServerBTN.Enabled = true;

            BackupProgressBar.Visible = false;

            if (e.Error != null)
            {
                _suppressNextBackupExplorerPrompt = false;
                AppendTerminalLine($"[MANAGER ERROR] Background compression failed: {e.Error.Message}");
            }
            else
            {
                string savedName = e.Result as string;
                AppendTerminalLine($"[CONSOLE] [SUCCESS] Backup package '{savedName}' successfully committed.");

                BKCompleteLBL.Visible = true;

                bool suppressPrompt = _suppressNextBackupExplorerPrompt;
                _suppressNextBackupExplorerPrompt = false;

                if (!suppressPrompt)
                {
                    DialogResult promptResult = MaterialDialogBox.Show(
                        this,
                        $"Backup package successfully created:\n'{savedName}'\n\nWould you like to open the local Backups archive directory in File Explorer now?",
                        "Backup Operations Successful",
                        MessageBoxButtons.YesNo
                    );

                    if (promptResult == DialogResult.Yes)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(_backupDestinationRoot) && Directory.Exists(_backupDestinationRoot))
                            {
                                Process.Start("explorer.exe", _backupDestinationRoot);
                            }
                        }
                        catch (Exception ex)
                        {
                            AppendTerminalLine($"[MANAGER ERROR] Failed to automatically initialize File Explorer context loop: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void ExecuteBackgroundBackupPipeline()
        {
            BackupWorldBTN.Enabled = false;
            BackupPacksBTN.Enabled = false;
            BackupServerBTN.Enabled = false;

            BKCompleteLBL.Visible = false;
            BackupProgressBar.Value = 0;
            BackupProgressBar.Visible = true;

            _backupDestinationRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
            if (!Directory.Exists(_backupDestinationRoot))
            {
                Directory.CreateDirectory(_backupDestinationRoot);
            }

            AppendTerminalLine("[MANAGER INFO] Initializing background storage compilation loop...");
            _backupWorker.RunWorkerAsync();
        }

        //#############################################################################
        //############################## Restore Pipeline #############################
        //#############################################################################

        private class BackupRestoreItem
        {
            public string FileFullPath { get; set; }
            public string VisualTextString { get; set; }

            public override string ToString()
            {
                return VisualTextString;
            }
        }

        public void PopulateRestoreSnapshotsList()
        {
            BackupPointsListBox.Items.Clear();
            RunRestoreBTN.Visible = false;

            string backupRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
            if (!Directory.Exists(backupRoot)) return;

            int targetType = RestoreTypeComboBox.SelectedIndex;
            string matchPrefix = "Worlds_Backup_";
            if (targetType == 1) matchPrefix = "Packs_Backup_";
            else if (targetType == 2) matchPrefix = "Server_Backup_";

            try
            {
                string[] files = Directory.GetFiles(backupRoot, matchPrefix + "*.*", SearchOption.TopDirectoryOnly);

                foreach (string filePath in files)
                {
                    string filename = Path.GetFileNameWithoutExtension(filePath);
                    string rawTimestamp = filename.Replace(matchPrefix, "");
                    string[] timeTokens = rawTimestamp.Split('_');

                    if (timeTokens.Length >= 4)
                    {
                        string month = timeTokens[0];
                        string day = timeTokens[1];
                        string year = timeTokens[2];

                        string timeString = timeTokens[3];
                        string hourStr = timeString.Substring(0, 2);
                        string minStr = timeString.Substring(2, 2);
                        string secStr = timeString.Substring(4, 2);

                        string readableDate = month + "/" + day + "/" + year + " @ " + hourStr + ":" + minStr + ":" + secStr;
                        string extension = Path.GetExtension(filePath).ToUpper().Replace(".", "");

                        var item = new BDSUtil.UI.MaterialListBoxItem
                        {
                            Text = "📅  " + readableDate,
                            SecondaryText = filePath
                        };

                        BackupPointsListBox.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                AppendTerminalLine("[MANAGER ERROR] Failed to load restore points index map: " + ex.Message);
            }
        }

        private void RestoreTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateRestoreSnapshotsList();
        }

        private void BackupPointsListBox_SelectedIndexChanged(object sender, BDSUtil.UI.MaterialListBoxItem selectedItem)
        {
            if (selectedItem == null || string.IsNullOrEmpty(selectedItem.SecondaryText))
            {
                RunRestoreBTN.Visible = false;
                return;
            }

            RunRestoreBTN.Visible = true;
        }

        //#############################################################################
        //########################### Snapshot Page BTNs ##############################
        //#############################################################################

        private void BackupWorldBTN_Click(object sender, EventArgs e)
        {
            if (_backupWorker.IsBusy) return;

            string serverDir = AppSettings.Instance.General.ServerExecutablePath;
            if (string.IsNullOrWhiteSpace(serverDir) || !System.IO.Directory.Exists(serverDir))
            {
                MaterialDialogBox.Show(
                    this,
                    "Please configure a valid server path in Settings before running backup operations.",
                    "Missing Configuration Parameters",
                    MessageBoxButtons.OK
                );
                return;
            }

            _activeBackupType = 1;

            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                DialogResult result = MaterialDialogBox.Show(
                    this,
                    "The Minecraft Bedrock server must be completely stopped before a backup can safely compile to prevent LevelDB file corruption.\n\n" +
                    "Would you like to issue a clean 'stop' command to the server now and automatically proceed with the backup once it fully exits?",
                    "Server Shutdown Required",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    _isBackupPendingAfterStop = true;
                    BackupWorldBTN.Enabled = false;
                    BackupServerBTN.Enabled = false;

                    AppendTerminalLine("[CONSOLE] [MANAGER INTERCEPT] Issuing server shutdown request for pending backup...");
                    _serverProcess.StandardInput.WriteLine("stop");
                }
                return;
            }

            ExecuteBackgroundBackupPipeline();
        }

        private void BackupPacksBTN_Click(object sender, EventArgs e)
        {
            if (_backupWorker.IsBusy) return;

            string serverDir = AppSettings.Instance.General.ServerExecutablePath;
            if (string.IsNullOrWhiteSpace(serverDir) || !System.IO.Directory.Exists(serverDir))
            {
                MaterialDialogBox.Show(
                    this,
                    "Please configure a valid server path in Settings before running backup operations.",
                    "Missing Configuration Parameters",
                    MessageBoxButtons.OK
                );
                return;
            }

            _activeBackupType = 2;

            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                DialogResult result = MaterialDialogBox.Show(
                    this,
                    "The Minecraft Bedrock server must be completely stopped before a backup can safely compile to prevent file corruption.\n\n" +
                    "Would you like to issue a clean 'stop' command to the server now and automatically proceed with the backup once it fully exits?",
                    "Server Shutdown Required",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    _isBackupPendingAfterStop = true;
                    BackupWorldBTN.Enabled = false;
                    BackupPacksBTN.Enabled = false;
                    BackupServerBTN.Enabled = false;

                    AppendTerminalLine("[CONSOLE] [MANAGER INTERCEPT] Issuing server shutdown request for pending packs backup...");
                    _serverProcess.StandardInput.WriteLine("stop");
                }
                return;
            }

            ExecuteBackgroundBackupPipeline();
        }

        private void BackupServerBTN_Click(object sender, EventArgs e)
        {
            if (_backupWorker.IsBusy) return;

            string serverDir = AppSettings.Instance.General.ServerExecutablePath;
            if (string.IsNullOrWhiteSpace(serverDir) || !System.IO.Directory.Exists(serverDir))
            {
                MaterialDialogBox.Show(
                    this,
                    "Please configure a valid server path in Settings before running backup operations.",
                    "Missing Configuration Parameters",
                    MessageBoxButtons.OK
                );
                return;
            }

            _activeBackupType = 3;

            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                DialogResult result = MaterialDialogBox.Show(
                    this,
                    "The Minecraft Bedrock server must be completely stopped before a backup can safely compile to prevent file corruption.\n\n" +
                    "Would you like to issue a clean 'stop' command to the server now and automatically proceed with the backup once it fully exits?",
                    "Server Shutdown Required",
                    MessageBoxButtons.YesNo
                );

                if (result == DialogResult.Yes)
                {
                    _isBackupPendingAfterStop = true;
                    BackupWorldBTN.Enabled = false;
                    BackupPacksBTN.Enabled = false;
                    BackupServerBTN.Enabled = false;

                    AppendTerminalLine("[CONSOLE] [MANAGER INTERCEPT] Issuing server shutdown request for full server backup...");
                    _serverProcess.StandardInput.WriteLine("stop");
                }
                return;
            }

            ExecuteBackgroundBackupPipeline();
        }

        private void RunRestoreBTN_Click(object sender, EventArgs e)
        {
            if (!(BackupPointsListBox.SelectedItem is BDSUtil.UI.MaterialListBoxItem selectedItem) || string.IsNullOrEmpty(selectedItem.SecondaryText))
            {
                RunRestoreBTN.Visible = false;
                return;
            }

            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                MaterialDialogBox.Show(
                    this,
                    "The Bedrock Server process is currently active. You must completely 'Stop Server' before overwriting server states to avoid catastrophic data corruption.",
                    "Active Server Process Blocked",
                    MessageBoxButtons.OK
                );
                return;
            }

            DialogResult confirmResult = MaterialDialogBox.Show(
                this,
                "WARNING: Proceeding will completely OVERWRITE your existing data matching the snapshot type chosen.\n\nAre you absolutely sure you want to execute recovery?",
                "Confirm Snapshot Overwrite",
                MessageBoxButtons.YesNo
            );

            if (confirmResult != DialogResult.Yes) return;

            string serverDir = AppSettings.Instance.General.ServerExecutablePath;
            int targetType = RestoreTypeComboBox.SelectedIndex;
            string archivePath = selectedItem.SecondaryText;
            string extension = Path.GetExtension(archivePath).ToLower();

            try
            {
                AppendTerminalLine("[RESTORE INITIALIZED] Preparing target environment directories...");

                if (targetType == 0)
                {
                    string targetFolder = Path.Combine(serverDir, "worlds");
                    if (Directory.Exists(targetFolder)) Directory.Delete(targetFolder, true);
                }
                else if (targetType == 1)
                {
                    string[] parentContainers = new string[] { "behavior_packs", "resource_packs" };
                    foreach (string container in parentContainers)
                    {
                        string containerPath = Path.Combine(serverDir, container);
                        if (!Directory.Exists(containerPath)) continue;

                        string[] customDirs = Directory.GetDirectories(containerPath, "custom_*", SearchOption.TopDirectoryOnly);
                        foreach (string dir in customDirs) Directory.Delete(dir, true);
                    }
                }
                else if (targetType == 2)
                {
                    string[] serverFolders = new string[] {
                        "behavior_packs", "config", "data", "definitions", "resource_packs",
                        "development_behavior_packs", "development_resource_packs", "development_skin_packs",
                        "world_templates", "worlds"
                    };
                    foreach (string folder in serverFolders)
                    {
                        string targetFolder = Path.Combine(serverDir, folder);
                        if (Directory.Exists(targetFolder)) Directory.Delete(targetFolder, true);
                    }

                    string[] rootConfigFileNames = new string[] {
                        "allowlist.json", "packetlimitconfig.json", "permissions.json",
                        "profanity_filter.wlist", "server.properties"
                    };
                    foreach (string fileName in rootConfigFileNames)
                    {
                        string targetFile = Path.Combine(serverDir, fileName);
                        if (File.Exists(targetFile)) File.Delete(targetFile);
                    }
                }

                AppendTerminalLine("[RESTORE EXTRACTION] Extracting archive file bytes down to disk blocks...");

                if (extension == ".rar" || (extension == ".zip" && File.Exists(_winRarRegistryPath) && AppSettings.Instance.General.CompressionMode != 2))
                {
                    string winRarArgs = "x -o+ \"" + archivePath + "\" \"" + serverDir + "\\\"";

                    var rarInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = _winRarRegistryPath,
                        Arguments = winRarArgs,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (System.Diagnostics.Process rarProcess = System.Diagnostics.Process.Start(rarInfo))
                    {
                        rarProcess?.WaitForExit();
                    }
                }
                else
                {
                    using (System.IO.Compression.ZipArchive archive = System.IO.Compression.ZipFile.OpenRead(archivePath))
                    {
                        foreach (System.IO.Compression.ZipArchiveEntry entry in archive.Entries)
                        {
                            string destinationPath = Path.GetFullPath(Path.Combine(serverDir, entry.FullName));

                            if (entry.FullName.EndsWith("/") || entry.FullName.EndsWith("\\"))
                            {
                                if (!Directory.Exists(destinationPath))
                                {
                                    Directory.CreateDirectory(destinationPath);
                                }
                            }
                            else
                            {
                                string directoryPath = Path.GetDirectoryName(destinationPath);
                                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }

                                entry.ExtractToFile(destinationPath, true);
                            }
                        }
                    }
                }

                AppendTerminalLine("[CONSOLE] [SUCCESS] Snapshot recovery operation successfully committed.");

                RunRestoreBTN.Visible = false;
                PopulateRestoreSnapshotsList();

                MaterialDialogBox.Show(
                    this,
                    "Snapshot recovery extraction loop finished successfully! Your active server environment has been rolled back.",
                    "Recovery Process Complete",
                    MessageBoxButtons.OK
                );
            }
            catch (Exception ex)
            {
                AppendTerminalLine("[MANAGER ERROR] Recovery execution pipeline failed: " + ex.Message);
                MaterialDialogBox.Show(
                    this,
                    "A critical exception error occurred during restoration loops:\n\n" + ex.Message,
                    "Recovery Fatal Failure",
                    MessageBoxButtons.OK
                );
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_forceSystemExit && (AppSettings.Instance.Networking.EnableUPnP || BDSUtil.Helpers.RemoteApi.RemoteApiManager.Instance.IsRunning) && !_isUpnpCleanupComplete)
            {
                e.Cancel = true;
                this.Enabled = false;

                Task.Run(() =>
                {
                    if (AppSettings.Instance.Networking.EnableUPnP)
                    {
                        ExecuteUpnpTeardown();
                    }
                    else
                    {
                        try { BDSUtil.Helpers.RemoteApi.RemoteApiManager.Instance.StopAsync(releaseUpnpLease: false).GetAwaiter().GetResult(); } catch { }
                    }

                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        _isUpnpCleanupComplete = true;
                        this.Close();
                    });
                });

                return;
            }

            if (_forceSystemExit)
            {
                return;
            }

            if (_serverProcess == null || _serverProcess.HasExited)
            {
                return;
            }

            using (CloseConfirmForm confirmDialog = new CloseConfirmForm())
            {
                if (confirmDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string action = confirmDialog.SelectedExitAction;

                    if (action == "CLEAN_STOP")
                    {
                        e.Cancel = true;
                        this.Enabled = false;

                        try
                        {
                            AppendTerminalLine("[EXIT CONSOLE INTERCEPT] Processing clean administrative shutdown sequence...");

                            _serverProcess.Exited -= ServerProcess_Exited;

                            _serverProcess.Exited += (s, args) =>
                            {
                                _forceSystemExit = true;
                                Environment.Exit(0);
                            };

                            _serverProcess.StandardInput.WriteLine("stop");
                        }
                        catch
                        {
                            try { _serverProcess.Kill(); } catch { }
                            _forceSystemExit = true;
                            Environment.Exit(0);
                        }
                    }
                    else if (action == "FORCE_KILL")
                    {
                        try
                        {
                            AppendTerminalLine("[EXIT CONSOLE INTERCEPT] Forcefully terminating active background server tree...");
                            _serverProcess.Kill();
                        }
                        catch { }

                        _forceSystemExit = true;
                        Application.Exit();
                    }
                    else
                    {
                        e.Cancel = true;
                        this.Enabled = true;
                    }
                }
                else
                {
                    e.Cancel = true;
                    this.Enabled = true;
                }
            }
        }

        private void StartServerBTN_New_Click(object sender, EventArgs e)
        {
            if (_serverProcess != null && !_serverProcess.HasExited) return;

            string serverDirectory = AppSettings.Instance.General.ServerExecutablePath;
            if (string.IsNullOrWhiteSpace(serverDirectory) || !System.IO.Directory.Exists(serverDirectory))
            {
                DialogResult promptResult = MaterialDialogBox.Show(
                    this,
                    "The Minecraft Bedrock Server path has not been configured, or 'bedrock_server.exe' could not be found.\n\n" +
                    "Would you like to open the application Settings panel now to link a valid server directory target path?",
                    "Server Routing Target Missing",
                    MessageBoxButtons.YesNo
                );

                if (promptResult == DialogResult.Yes)
                {
                    if (MaterialTabControl1 != null)
                    {
                        int settingsIndex = MaterialTabControl1.TabCount - 1;

                        System.Windows.Forms.Timer redirectTimer = new System.Windows.Forms.Timer { Interval = 1 };
                        redirectTimer.Tick += (s, args) =>
                        {
                            redirectTimer.Stop();

                            _isRedirecting = true;
                            MaterialTabControl1.SelectedIndex = settingsIndex;
                            _isRedirecting = false;

                            MaterialTabControl1.Update();

                            if (this.settingsProvider1 != null)
                            {
                                this.settingsProvider1.InitializeControl();
                                this.settingsProvider1.ClearSelectionFocus();
                            }

                            AppendTerminalLine("[MANAGER INFO] Configuration paths updated. Click 'Start Server' again once path is linked.");
                            UpdateControlButtonStates();
            LayoutConsoleControls();

                            redirectTimer.Dispose();
                        };
                        redirectTimer.Start();
                    }
                }
                return;
            }

            string fullBinaryPath = System.IO.Path.Combine(serverDirectory, "bedrock_server.exe");
            if (!System.IO.File.Exists(fullBinaryPath))
            {
                MaterialDialogBox.Show(
                    this,
                    "The binary execution module 'bedrock_server.exe' could not be found in the specified server root path.",
                    "Target Executable Missing",
                    MessageBoxButtons.OK
                );
                return;
            }

            ConsoleOutputTextBox.Clear();
            ConsoleOutputTextBox.AppendText("Initializing Server Boot Pipeline...\r\n");

            if (ServerPropertiesDocument.CommentOutServerPortV6(serverDirectory))
            {
                AppendTerminalLine("[MANAGER INFO] Commented out 'server-portv6' in server.properties (ignored under NetherNet).");
            }

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmm");
            System.IO.Directory.CreateDirectory("logs");
            _currentLogPath = $@"logs\server_{timestamp}.log";

            _serverProcess = new Process();
            _serverProcess.StartInfo.FileName = fullBinaryPath;
            _serverProcess.StartInfo.WorkingDirectory = serverDirectory;
            _serverProcess.StartInfo.UseShellExecute = false;
            _serverProcess.StartInfo.CreateNoWindow = true;
            _serverProcess.StartInfo.RedirectStandardOutput = true;
            _serverProcess.StartInfo.RedirectStandardError = true;
            _serverProcess.StartInfo.RedirectStandardInput = true;

            _serverProcess.OutputDataReceived += (s, args) => AppendTerminalLine(args.Data);
            _serverProcess.ErrorDataReceived += (s, args) => AppendTerminalLine(args.Data);

            _serverProcess.EnableRaisingEvents = true;
            _serverProcess.Exited += ServerProcess_Exited;

            _serverProcess.Start();
            _serverStartedAtUtc = DateTimeOffset.UtcNow;
            _serverProcess.BeginOutputReadLine();
            _serverProcess.BeginErrorReadLine();

            UpdateControlButtonStates();
            LayoutConsoleControls();
        }

        private void StopServerBTN_New_Click(object sender, EventArgs e)
        {
            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                AppendTerminalLine("[MANAGER INFO] Injected administrative 'stop' command stream...");
                _serverProcess.StandardInput.WriteLine("stop");
                UpdateControlButtonStates();
            LayoutConsoleControls();
            }
        }

        private void SvConfigButton_Click(object sender, EventArgs e)
        {
            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                MaterialDialogBox.Show(
                    this,
                    "The Bedrock Server process is currently active. You must completely 'Stop Server' before editing core server configurations to prevent disk write collisions.",
                    "Active Server Process Blocked",
                    MessageBoxButtons.OK
                );
                return;
            }

            string targetDir = AppSettings.Instance?.General.ServerExecutablePath;
            bool isEnvironmentValid = false;

            if (!string.IsNullOrWhiteSpace(targetDir))
            {
                if (targetDir.EndsWith("bedrock_server.exe", StringComparison.OrdinalIgnoreCase))
                {
                    targetDir = Path.GetDirectoryName(targetDir);
                }

                if (!string.IsNullOrEmpty(targetDir) && Directory.Exists(targetDir))
                {
                    string binaryCheck = Path.Combine(targetDir, "bedrock_server.exe");
                    string propertiesCheck = Path.Combine(targetDir, "server.properties");

                    isEnvironmentValid = File.Exists(binaryCheck) && File.Exists(propertiesCheck);
                }
            }

            if (!isEnvironmentValid)
            {
                DialogResult promptResult = MaterialDialogBox.Show(
                    this,
                    "You cant edit the server configuration since you didnt set its path...\n\n" +
                    "Would you like to open the application Settings panel now to link a valid server directory target path?",
                    "Silly Goose :D",
                    MessageBoxButtons.YesNo
                );

                if (promptResult == DialogResult.Yes)
                {
                    if (MaterialTabControl1 != null)
                    {
                        int settingsIndex = MaterialTabControl1.TabCount - 1;

                        System.Windows.Forms.Timer redirectTimer = new System.Windows.Forms.Timer { Interval = 1 };
                        redirectTimer.Tick += (s, args) =>
                        {
                            redirectTimer.Stop();

                            _isRedirecting = true;
                            MaterialTabControl1.SelectedIndex = settingsIndex;
                            _isRedirecting = false;

                            MaterialTabControl1.Update();

                            if (this.settingsProvider1 != null)
                            {
                                this.settingsProvider1.InitializeControl();
                                this.settingsProvider1.ClearSelectionFocus();
                            }

                            AppendTerminalLine("[MANAGER INFO] Swapped to Settings panel. Please link a valid server directory path.");
                            UpdateControlButtonStates();
            LayoutConsoleControls();

                            redirectTimer.Dispose();
                        };
                        redirectTimer.Start();
                    }
                }

                return;
            }

            ProceedToConfigModal();
        }

        private void ProceedToConfigModal()
        {
            this.Hide();

            using (var configForm = new ServerConfig(this))
            {
                configForm.ShowDialog(this);
            }

            this.Show();
            this.Update();
        }

        private void SV_PM_BTN_Click(object sender, EventArgs e)
        {
            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                MaterialDialogBox.Show(
                    this,
                    "The Bedrock Server process is currently active. You must completely 'Stop Server' before accessing the Pack Manager.",
                    "Active Server Process Blocked",
                    MessageBoxButtons.OK
                );
                return;
            }

            string targetDir = AppSettings.Instance?.General.ServerExecutablePath;
            bool isEnvironmentValid = false;

            if (!string.IsNullOrWhiteSpace(targetDir))
            {
                if (targetDir.EndsWith("bedrock_server.exe", StringComparison.OrdinalIgnoreCase))
                {
                    targetDir = Path.GetDirectoryName(targetDir);
                }

                if (!string.IsNullOrEmpty(targetDir) && Directory.Exists(targetDir))
                {
                    string bpDirCheck = Path.Combine(targetDir, "behavior_packs");
                    string rpDirCheck = Path.Combine(targetDir, "resource_packs");

                    isEnvironmentValid = Directory.Exists(bpDirCheck) && Directory.Exists(rpDirCheck);
                }
            }

            if (!isEnvironmentValid)
            {
                DialogResult promptResult = MaterialDialogBox.Show(
                    this,
                    "You cant use the Pack Manager without a valid server directory path.\n\n" +
                    "Would you like to open the application Settings panel now to link a valid server directory target path?",
                    "Silly Goose :D",
                    MessageBoxButtons.YesNo
                );

                if (promptResult == DialogResult.Yes)
                {
                    if (MaterialTabControl1 != null)
                    {
                        int settingsIndex = MaterialTabControl1.TabCount - 1;

                        System.Windows.Forms.Timer redirectTimer = new System.Windows.Forms.Timer { Interval = 1 };
                        redirectTimer.Tick += (s, args) =>
                        {
                            redirectTimer.Stop();

                            _isRedirecting = true;
                            MaterialTabControl1.SelectedIndex = settingsIndex;
                            _isRedirecting = false;

                            MaterialTabControl1.Update();

                            if (this.settingsProvider1 != null)
                            {
                                this.settingsProvider1.InitializeControl();
                                this.settingsProvider1.ClearSelectionFocus();
                            }

                            AppendTerminalLine("[MANAGER INFO] Swapped to Settings panel. Please link a valid server directory path.");
                            UpdateControlButtonStates();
            LayoutConsoleControls();

                            redirectTimer.Dispose();
                        };
                        redirectTimer.Start();
                    }
                }

                return;
            }

            ProceedToPackManager();
        }

        private void ProceedToPackManager()
        {
            this.Hide();

            using (var packManager = new Modals.Content_Controls.Packs.AddonManager(this))
            {
                packManager.ShowDialog(this);
            }

            this.Show();
            this.Update();
        }


    }
}