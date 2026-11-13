using BDSUtil.Configuration;
using BDSUtil.Helpers;
using BDSUtil.Helpers.AutoMod;
using BDSUtil.Helpers.RemoteApi;
using BDSUtil.Helpers.Updates;
using BDSUtil.Modals;
using BDSUtil.UI;
using Material3.WinForms.Controls;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BDSUtil.Modals.Content_Controls.Settings
{
    public partial class SettingsProvider : Material3UserControl
    {
        private bool _isInitializing = true;
        private Label _focusTrapLabel; // ✅ Holds focus invisibly to prevent input highlighting

        public SettingsProvider()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            InitializeComponent();

            // =========================================================================
            // 🛡️ FOCUS REDIRECTION TRAP
            // =========================================================================
            _focusTrapLabel = new Label
            {
                Size = new System.Drawing.Size(0, 0),
                Location = new System.Drawing.Point(0, 0),
                TabStop = true
            };
            this.Controls.Add(_focusTrapLabel);
            // =========================================================================

            this.Resize += (s, e) => UpdateNetworkingCardsLayout();

            if (CompressionTypeComboBox != null)
                CompressionTypeComboBox.SelectedIndexChanged += CompressionTypeComboBox_SelectedIndexChanged;

            PopulatePortCheckerProviders();
            if (PortCheckerProviderComboBox != null)
                PortCheckerProviderComboBox.SelectedIndexChanged += PortCheckerProviderComboBox_SelectedIndexChanged;

            if (CustomPortCheckerTextBox != null)
            {
                CustomPortCheckerTextBox.TextChanged += CustomPortCheckerTextBox_TextChanged;
                CustomPortCheckerTextBox.Leave += CustomPortCheckerTextBox_Leave;
            }

            if (CfgWorldMod != null)
                CfgWorldMod.CheckedChanged += CfgWorldMod_CheckedChanged;

            RemoteApiManager.Instance.StateChanged += OnRemoteApiStateChanged;
            this.Disposed += (s, e) => RemoteApiManager.Instance.StateChanged -= OnRemoteApiStateChanged;

            this.Load += (s, e) => { InitializeControl(); };
        }

        /// <summary>
        /// Programmatically forces focus onto our zero-size element to drop all input highlights.
        /// </summary>
        public void ClearSelectionFocus()
        {
            if (_focusTrapLabel != null && _focusTrapLabel.CanFocus)
            {
                _focusTrapLabel.Focus();
            }
        }

        /// <summary>
        /// Populates the custom material designer controls using parameters retrieved from AppSettings.
        /// </summary>
        public void InitializeControl()
        {
            _isInitializing = true;

            var general = AppSettings.Instance.General;
            var networking = AppSettings.Instance.Networking;
            var remoteApi = AppSettings.Instance.RemoteApi;

            // 1. Sync the bedrock executable root directory location string
            if (ServerPathTextBox != null)
            {
                ServerPathTextBox.Text = general.ServerExecutablePath;
            }

            // 2. Sync active archive sorting format selection dropdown indices securely
            if (CompressionTypeComboBox != null)
            {
                int savedMode = general.CompressionMode;
                if (savedMode >= 0 && savedMode < CompressionTypeComboBox.Items.Count)
                {
                    CompressionTypeComboBox.SelectedIndex = savedMode;
                }
            }

            // 3. Sync master file system and networking switches
            if (CfgWorldMod != null)
            {
                CfgWorldMod.Checked = general.WorldEditorEnabled;
            }

            if (CfgHTel != null) CfgHTel.Checked = general.HideTelemetryWarnings;

            if (CfgUPnP != null) CfgUPnP.Checked = networking.EnableUPnP;
            if (PortCheckerProviderComboBox != null && PortCheckerProviderComboBox.Items.Count > 0)
            {
                int providerIdx = ExternalPortChecker.GetProviderIndex(networking.ExternalPortCheckProvider);
                PortCheckerProviderComboBox.SelectedIndex = Math.Clamp(providerIdx, 0, PortCheckerProviderComboBox.Items.Count - 1);
            }
            if (CustomPortCheckerTextBox != null)
            {
                CustomPortCheckerTextBox.Text = networking.ExternalPortCheckUrl ?? string.Empty;
            }
            UpdateCustomPortCheckerHint();

            if (CfgConsolidateUdp != null) CfgConsolidateUdp.Checked = networking.ConsolidateUdpPorts;

            if (CfgRemoteApi != null) CfgRemoteApi.Checked = remoteApi.Enabled;
            RefreshRemoteApiUi();
            UpdateNetworkingCardsLayout();

            _isInitializing = false;

            // ✅ START WITH NOTHING SELECTED
            ClearSelectionFocus();
        }

        private void PopulatePortCheckerProviders()
        {
            if (PortCheckerProviderComboBox == null) return;

            PortCheckerProviderComboBox.Items.Clear();
            foreach (var option in ExternalPortChecker.Providers)
            {
                PortCheckerProviderComboBox.Items.Add(option.DisplayName);
            }
        }

        private string GetSelectedPortCheckerProviderKey()
        {
            int idx = PortCheckerProviderComboBox?.SelectedIndex ?? 0;
            if (idx >= 0 && idx < ExternalPortChecker.Providers.Count)
            {
                return ExternalPortChecker.Providers[idx].Key;
            }
            return ExternalPortChecker.ProviderOff;
        }

        private void UpdateCustomPortCheckerHint()
        {
            if (CustomPortCheckerHintLBL == null) return;
            CustomPortCheckerHintLBL.Text = ExternalPortChecker.GetCustomTemplateHint(CustomPortCheckerTextBox?.Text);
        }

        private void UpdateNetworkingCardsLayout()
        {
            if (materialCard1 == null || materialCard3 == null || UpnpSettingsCard == null || ConsolidateUdpCard == null || materialCard5 == null)
                return;

            bool upnpOn = CfgUPnP?.Checked == true;
            bool isCustomProvider = upnpOn && GetSelectedPortCheckerProviderKey() == ExternalPortChecker.ProviderCustom;

            this.SuspendLayout();
            try
            {
                int left = 14;
                int cardWidth = Math.Max(280, this.ClientSize.Width - (left * 2));

                // 1. Global App Settings Header Card
                materialCard1.Location = new System.Drawing.Point(left, 14);
                materialCard1.Width = cardWidth;

                // 2. Server Directory & Compression Card
                materialCard3.Location = new System.Drawing.Point(left, materialCard1.Bottom + 14);
                materialCard3.Size = new System.Drawing.Size(cardWidth, 150);

                if (BrowseBTN != null)
                {
                    BrowseBTN.ElevationLevel = 1;
                    BrowseBTN.IconSize = 28;
                    BrowseBTN.Size = new System.Drawing.Size(56, 56);
                    BrowseBTN.Location = new System.Drawing.Point(Math.Max(300, materialCard3.ClientSize.Width - 14 - 56), 10);
                }

                if (ServerPathTextBox != null)
                {
                    int browseLeft = BrowseBTN != null ? BrowseBTN.Left : (materialCard3.ClientSize.Width - 14 - 56);
                    ServerPathTextBox.Location = new System.Drawing.Point(301, 14);
                    ServerPathTextBox.Size = new System.Drawing.Size(Math.Max(120, browseLeft - 301 - 12), 48);
                }

                if (materialLabel2 != null)
                {
                    materialLabel2.Location = new System.Drawing.Point(14, 82);
                }

                if (CompressionTypeComboBox != null)
                {
                    CompressionTypeComboBox.Location = new System.Drawing.Point(395, 82);
                    CompressionTypeComboBox.Size = new System.Drawing.Size(Math.Max(160, materialCard3.ClientSize.Width - 395 - 14), 49);
                }

                // 3. Enable World Modification Card
                if (materialCard4 != null)
                {
                    materialCard4.Location = new System.Drawing.Point(left, materialCard3.Bottom + 8);
                    materialCard4.Size = new System.Drawing.Size(cardWidth, 75);
                }

                // 4. Hide Telemetry Card
                if (materialCard2 != null)
                {
                    int top4 = materialCard4 != null ? materialCard4.Bottom : materialCard3.Bottom;
                    materialCard2.Location = new System.Drawing.Point(left, top4 + 8);
                    materialCard2.Size = new System.Drawing.Size(cardWidth, 75);
                }

                // 5. Networking Settings Header Card
                if (NetworkingSectionCard != null)
                {
                    int top2 = materialCard2 != null ? materialCard2.Bottom : materialCard3.Bottom;
                    NetworkingSectionCard.Location = new System.Drawing.Point(left, top2 + 16);
                    NetworkingSectionCard.Size = new System.Drawing.Size(cardWidth, 51);
                }

                // 6. UPnP Card
                int netHeaderBottom = NetworkingSectionCard != null ? NetworkingSectionCard.Bottom : (materialCard2?.Bottom ?? 380);
                UpnpSettingsCard.Location = new System.Drawing.Point(left, netHeaderBottom + 8);
                UpnpSettingsCard.Size = new System.Drawing.Size(cardWidth, 75);

                int nextTop = UpnpSettingsCard.Bottom + 8;

                // 7. External Port Checker Card
                if (ExternalPortCheckerCard != null)
                {
                    ExternalPortCheckerCard.Visible = upnpOn;
                    if (upnpOn)
                    {
                        ExternalPortCheckerCard.Location = new System.Drawing.Point(left, nextTop);
                        ExternalPortCheckerCard.Width = cardWidth;
                        ExternalPortCheckerCard.Height = isCustomProvider ? 186 : 88;

                        int dropdownWidth = Math.Min(310, Math.Max(220, ExternalPortCheckerCard.ClientSize.Width - 380));
                        int dropdownLeft = Math.Max(220, ExternalPortCheckerCard.ClientSize.Width - 14 - dropdownWidth);

                        if (PortCheckerProviderComboBox != null)
                        {
                            PortCheckerProviderComboBox.Location = new System.Drawing.Point(dropdownLeft, 19);
                            PortCheckerProviderComboBox.Size = new System.Drawing.Size(dropdownWidth, 49);
                        }

                        if (ExternalPortCheckerLBL != null)
                        {
                            ExternalPortCheckerLBL.Location = new System.Drawing.Point(14, 10);
                            ExternalPortCheckerLBL.Size = new System.Drawing.Size(Math.Max(180, dropdownLeft - 14 - 12), 68);
                        }

                        if (CustomPortCheckerTextBox != null)
                        {
                            CustomPortCheckerTextBox.Visible = isCustomProvider;
                            CustomPortCheckerTextBox.Location = new System.Drawing.Point(14, 84);
                            CustomPortCheckerTextBox.Size = new System.Drawing.Size(Math.Max(220, ExternalPortCheckerCard.ClientSize.Width - 28), 48);
                        }

                        if (CustomPortCheckerHintLBL != null)
                        {
                            CustomPortCheckerHintLBL.Visible = isCustomProvider;
                            CustomPortCheckerHintLBL.Location = new System.Drawing.Point(14, 138);
                            CustomPortCheckerHintLBL.Size = new System.Drawing.Size(Math.Max(220, ExternalPortCheckerCard.ClientSize.Width - 28), 36);
                        }

                        nextTop = ExternalPortCheckerCard.Bottom + 8;
                    }
                }

                // 8. Consolidate UDP Ports Card
                ConsolidateUdpCard.Location = new System.Drawing.Point(left, nextTop);
                ConsolidateUdpCard.Size = new System.Drawing.Size(cardWidth, 88);

                if (ConsolidateUdpLBL != null)
                {
                    ConsolidateUdpLBL.Location = new System.Drawing.Point(14, 10);
                    ConsolidateUdpLBL.Size = new System.Drawing.Size(Math.Max(200, ConsolidateUdpCard.ClientSize.Width - 14 - 84), 68);
                }

                if (CfgConsolidateUdp != null)
                {
                    CfgConsolidateUdp.Location = new System.Drawing.Point(ConsolidateUdpCard.ClientSize.Width - 14 - 64, 25);
                }

                nextTop = ConsolidateUdpCard.Bottom + 8;

                // 9. Remote API Card
                materialCard5.Location = new System.Drawing.Point(left, nextTop);
                materialCard5.Size = new System.Drawing.Size(cardWidth, 126);
                nextTop = materialCard5.Bottom + 14;

                int logicalBottom = nextTop - this.AutoScrollPosition.Y;
                this.AutoScrollMinSize = new System.Drawing.Size(0, Math.Max(765, logicalBottom));
            }
            finally
            {
                this.ResumeLayout(performLayout: true);
            }
        }

        private void OnRemoteApiStateChanged()
        {
            if (this.IsDisposed || !this.IsHandleCreated) return;
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(RefreshRemoteApiUi));
                }
                else
                {
                    RefreshRemoteApiUi();
                }
            }
            catch { }
        }

        private void RefreshRemoteApiUi()
        {
            var remoteSettings = AppSettings.Instance?.RemoteApi;
            bool enabled = remoteSettings != null && remoteSettings.Enabled;
            string apiKey = remoteSettings?.ApiKey ?? string.Empty;
            var mgr = RemoteApiManager.Instance;

            if (CopyApiKeyBTN != null) CopyApiKeyBTN.Enabled = !string.IsNullOrEmpty(apiKey);
            if (RotateApiKeyBTN != null) RotateApiKeyBTN.Enabled = enabled || !string.IsNullOrEmpty(apiKey);

            if (RemoteApiStatusLBL == null) return;

            if (!enabled)
            {
                RemoteApiStatusLBL.Text = "Status: Disabled — API listener stopped; no UPnP port requisitioned.";
                return;
            }

            string maskedKey = MaskApiKey(apiKey);
            if (mgr.IsRunning && mgr.BoundPort > 0)
            {
                string upnpTag = mgr.IsUpnpForwarded
                    ? $"UPnP TCP {mgr.BoundPort} forwarded"
                    : (AppSettings.Instance?.Networking?.EnableUPnP == true ? "LAN only (UPnP unavailable/pending)" : "LAN / manual forward");
                RemoteApiStatusLBL.Text = $"Status: Listening on TCP {mgr.BoundPort} ({upnpTag})\r\nAPI Key: {maskedKey}";
            }
            else
            {
                RemoteApiStatusLBL.Text = $"Status: {mgr.StatusMessage}\r\nAPI Key: {maskedKey}";
            }
        }

        private static string MaskApiKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return "(not generated yet)";
            if (key.Length <= 8) return "********";
            return $"{key.Substring(0, 4)}...{key.Substring(key.Length - 4)}";
        }

        private async void CfgRemoteApi_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing || CfgRemoteApi == null) return;

            bool isEnabled = CfgRemoteApi.Checked;

            if (isEnabled && !AppSettings.Instance.RemoteApi.HasAcceptedApiRisks)
            {
                if (this.FindForm() is Form parentMaterialForm)
                {
                    DialogResult consent = MaterialDialogBox.Show(
                        parentMaterialForm,
                        "REMOTE MANAGEMENT API SECURITY NOTICE:\n\n" +
                        "Enabling the Remote Management API binds an HTTP/JSON listener below your lowest Bedrock server port and accepts commands from Android or desktop companion apps.\n\n" +
                        "When UPnP is enabled, this port is also requisitioned on your router for external WAN access.\n\n" +
                        "Always keep your generated API key private. Enable Remote Management API now?\n",
                        "Enable Remote Management API",
                        MessageBoxButtons.YesNo);

                    if (consent == DialogResult.No)
                    {
                        _isInitializing = true;
                        CfgRemoteApi.Checked = false;
                        _isInitializing = false;
                        RefreshRemoteApiUi();
                        ClearSelectionFocus();
                        return;
                    }
                }
            }

            AppSettings.Instance.Update(cfg =>
            {
                cfg.RemoteApi.Enabled = isEnabled;
                if (isEnabled)
                {
                    cfg.RemoteApi.HasAcceptedApiRisks = true;
                    if (string.IsNullOrWhiteSpace(cfg.RemoteApi.ApiKey))
                    {
                        cfg.RemoteApi.ApiKey = RemoteApiSettings.GenerateApiKey();
                    }
                }
            });

            if (RemoteApiStatusLBL != null)
            {
                RemoteApiStatusLBL.Text = isEnabled
                    ? "Status: Starting API listener and checking port allocation..."
                    : "Status: Stopping API listener and releasing UPnP port...";
            }

            try
            {
                await RemoteApiManager.Instance.SynchronizeFromSettingsAsync(allowUpnpDiscovery: true);
            }
            catch (Exception ex)
            {
                if (RemoteApiStatusLBL != null)
                {
                    RemoteApiStatusLBL.Text = $"Status: Error — {ex.Message}";
                }
            }
            finally
            {
                RefreshRemoteApiUi();
                ClearSelectionFocus();
            }
        }

        private void CopyApiKeyBTN_Click(object sender, EventArgs e)
        {
            string key = AppSettings.Instance?.RemoteApi?.ApiKey;
            if (string.IsNullOrWhiteSpace(key))
            {
                key = RemoteApiSettings.GenerateApiKey();
                AppSettings.Instance.Update(cfg => cfg.RemoteApi.ApiKey = key);
                RefreshRemoteApiUi();
            }

            try
            {
                Clipboard.SetText(key);
            }
            catch { }

            var mgr = RemoteApiManager.Instance;
            int port = mgr.BoundPort > 0 ? mgr.BoundPort : (ServerPortDiscovery.ReadServerPortSnapshot(AppSettings.Instance?.General?.ServerExecutablePath).LowestServerPort - 1);
            string localIp = string.IsNullOrWhiteSpace(mgr.LocalIp) ? BDSUtil.Helpers.UPnPEngine.GetLocalIPAddress() : mgr.LocalIp;
            string externalUrl = (!string.IsNullOrWhiteSpace(mgr.ExternalIp) && port > 0)
                ? $"http://{mgr.ExternalIp}:{port}/api/v1"
                : "(Enable UPnP or check external IP)";

            if (this.FindForm() is Form parentForm)
            {
                MaterialDialogBox.Show(
                    parentForm,
                    $"API Key copied to clipboard!\n\n" +
                    $"• Header: Authorization: Bearer <key> (or X-API-Key: <key>)\n" +
                    $"• LAN URL: http://{localIp}:{port}/api/v1\n" +
                    $"• External URL: {externalUrl}\n" +
                    $"• Health Ping: GET /api/v1/ping\n" +
                    $"• Status: GET /api/v1/status",
                    "Remote API Connection Info",
                    MessageBoxButtons.OK);
            }

            ClearSelectionFocus();
        }

        private void RotateApiKeyBTN_Click(object sender, EventArgs e)
        {
            if (this.FindForm() is Form parentForm)
            {
                DialogResult confirm = MaterialDialogBox.Show(
                    parentForm,
                    "Generate a new Remote Management API key?\n\nExisting companion apps using the old key will immediately lose access until updated.",
                    "Rotate API Key",
                    MessageBoxButtons.YesNo);

                if (confirm != DialogResult.Yes)
                {
                    ClearSelectionFocus();
                    return;
                }
            }

            string freshKey = RemoteApiSettings.GenerateApiKey();
            AppSettings.Instance.Update(cfg => cfg.RemoteApi.ApiKey = freshKey);
            RefreshRemoteApiUi();
            ClearSelectionFocus();
        }

        /// <summary>
        /// Telemetry Hider toggle
        /// </summary>
        private void CfgHTel_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing || CfgHTel == null) return;

            AppSettings.Instance.General.HideTelemetryWarnings = CfgHTel.Checked;
            AppSettings.Instance.Save();
        }

        /// <summary>
        /// Prompts users with an explicit Material confirmation warning before saving changes to the World Modification switch.
        /// </summary>
        private void CfgWorldMod_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing || CfgWorldMod == null) return;

            bool isEnabled = CfgWorldMod.Checked;

            if (isEnabled)
            {
                if (this.FindForm() is Form parentMaterialForm)
                {
                    DialogResult riskConsentResult = MaterialDialogBox.Show(
                        parentMaterialForm,
                        "CRITICAL DATA SAFETY WARNING:\n\nUnlocking the World Editor tools grants permission to rewrite level parameters.\n\n" +
                        "Improper values or directory write errors can corrupt active worlds or cause database crashes. Always maintain independent save folder copies.\n\n" +
                        "Do you completely acknowledge these operational risks and authorize file manipulations?\n",
                        "Confirm Data Modification Rights",
                        MessageBoxButtons.YesNo
                    );

                    if (riskConsentResult == DialogResult.No)
                    {
                        // Roll back the switch toggle immediately if safety consent is rejected
                        _isInitializing = true;
                        CfgWorldMod.Checked = false;
                        _isInitializing = false;

                        // ✅ GO BACK TO NOTHING SELECTED (On Abort)
                        ClearSelectionFocus();
                        return;
                    }
                }
            }

            // On-demand configuration commitment save
            AppSettings.Instance.Update(cfg => cfg.General.WorldEditorEnabled = isEnabled);

            // ✅ GO BACK TO NOTHING SELECTED (On Modification Complete)
            ClearSelectionFocus();
        }

        /// <summary>
        /// Saves backup compression format modifications to config.json instantly when a dropdown option is clicked.
        /// </summary>
        private void CompressionTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitializing || CompressionTypeComboBox == null) return;

            AppSettings.Instance.Update(cfg => cfg.General.CompressionMode = CompressionTypeComboBox.SelectedIndex);

            // ✅ GO BACK TO NOTHING SELECTED (After Dropdown Closes)
            ClearSelectionFocus();
        }

        private async void CfgUPnP_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing || CfgUPnP == null) return;

            bool newState = CfgUPnP.Checked;
            if (newState && !AppSettings.Instance.Networking.HasAcceptedNetworkRisks)
            {
                if (!ConfirmNetworkingSecurityConsent())
                {
                    _isInitializing = true;
                    CfgUPnP.Checked = false;
                    _isInitializing = false;
                    ClearSelectionFocus();
                    return;
                }
            }

            AppSettings.Instance.Update(cfg => cfg.Networking.EnableUPnP = newState);
            UpdateNetworkingCardsLayout();
            RefreshRemoteApiUi();

            if (AppSettings.Instance.RemoteApi.Enabled)
            {
                await RemoteApiManager.Instance.SynchronizeFromSettingsAsync(allowUpnpDiscovery: newState);
                RefreshRemoteApiUi();
            }

            PromptRestartForUpnpRealignment();
            ClearSelectionFocus();
        }

        private void PortCheckerProviderComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitializing || PortCheckerProviderComboBox == null) return;

            string selectedKey = GetSelectedPortCheckerProviderKey();
            AppSettings.Instance.Update(cfg => cfg.Networking.ExternalPortCheckProvider = selectedKey);

            UpdateCustomPortCheckerHint();
            UpdateNetworkingCardsLayout();

            if (selectedKey == ExternalPortChecker.ProviderCustom &&
                CustomPortCheckerTextBox != null &&
                CustomPortCheckerTextBox.Visible &&
                CustomPortCheckerTextBox.CanFocus)
            {
                CustomPortCheckerTextBox.Focus();
            }
            else
            {
                ClearSelectionFocus();
            }
        }

        private void CustomPortCheckerTextBox_TextChanged(object sender, EventArgs e)
        {
            if (_isInitializing || CustomPortCheckerTextBox == null) return;

            UpdateCustomPortCheckerHint();

            string candidateUrl = CustomPortCheckerTextBox.Text.Trim();
            if (!string.Equals(AppSettings.Instance.Networking.ExternalPortCheckUrl, candidateUrl, StringComparison.Ordinal))
            {
                AppSettings.Instance.Update(cfg => cfg.Networking.ExternalPortCheckUrl = candidateUrl);
            }
        }

        private void CustomPortCheckerTextBox_Leave(object sender, EventArgs e)
        {
            if (_isInitializing || CustomPortCheckerTextBox == null) return;

            string trimmed = CustomPortCheckerTextBox.Text.Trim();
            if (!string.Equals(CustomPortCheckerTextBox.Text, trimmed, StringComparison.Ordinal))
            {
                _isInitializing = true;
                CustomPortCheckerTextBox.Text = trimmed;
                _isInitializing = false;
            }

            if (!string.Equals(AppSettings.Instance.Networking.ExternalPortCheckUrl, trimmed, StringComparison.Ordinal))
            {
                AppSettings.Instance.Update(cfg => cfg.Networking.ExternalPortCheckUrl = trimmed);
            }

            UpdateCustomPortCheckerHint();
        }

        private void CfgConsolidateUdp_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing || CfgConsolidateUdp == null) return;

            bool isConsolidated = CfgConsolidateUdp.Checked;
            AppSettings.Instance.Update(cfg => cfg.Networking.ConsolidateUdpPorts = isConsolidated);

            if (AppSettings.Instance.Networking.EnableUPnP)
            {
                PromptRestartForUpnpRealignment(
                    "You updated [BETA] Consolidate UDP Ports while UPnP is active.\n\n" +
                    "To rebuild router port mappings and rewrite server-udp-ports cleanly, BDSUtil needs to restart.\n\n" +
                    "Would you like to restart the manager app right now?");
            }

            ClearSelectionFocus();
        }

        private bool ConfirmNetworkingSecurityConsent()
        {
            IWin32Window owner = (IWin32Window)this.FindForm() ?? this;
            using (var alertDialogue = new NetworkSecurityDialogue())
            {
                if (alertDialogue.ShowDialog(owner) == DialogResult.OK && alertDialogue.Confirmed)
                {
                    using (var instructionsDialogue = new AdvancedNetworkingDialogue())
                    {
                        if (instructionsDialogue.ShowDialog(owner) == DialogResult.OK && instructionsDialogue.InstructionsAccepted)
                        {
                            AppSettings.Instance.Update(cfg => cfg.Networking.HasAcceptedNetworkRisks = true);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool PromptRestartForUpnpRealignment(string customMessage = null)
        {
            IWin32Window owner = (IWin32Window)this.FindForm() ?? this;
            string prompt = customMessage ??
                "[UPnP CAPABILITY INVERSION DETECTED]\n\n" +
                "You have modified the core network translation engine configuration state.\n\n" +
                "To map network interface ports securely and align background execution threads, " +
                "BDSUtil requires a clean application restart.\n\n" +
                "Would you like to restart the manager app right now, or continue working manually?";

            if (MaterialDialogBox.Show(
                    owner,
                    prompt,
                    "Infrastructure Realignment Required",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string serverDirectory = AppSettings.Instance?.General.ServerExecutablePath;
                using (var progressModal = new UPnPCloseProgressForm(serverDirectory))
                {
                    progressModal.ShowDialog(owner);
                }

                Application.Restart();
                Environment.Exit(0);
                return true;
            }

            return false;
        }

        private static DialogResult ShowPrompt(
            IWin32Window owner,
            Form materialOwner,
            string text,
            string caption,
            MessageBoxButtons buttons,
            MessageBoxIcon fallbackIcon = MessageBoxIcon.Information)
        {
            return MaterialDialogBox.Show(materialOwner ?? owner, text, caption, buttons);
        }

        private async void BrowseBTN_Click(object sender, EventArgs e)
        {
            IWin32Window owner = (IWin32Window)this.FindForm() ?? this;
            Form materialOwner = this.FindForm();
            string originalWorkingDir = Directory.GetCurrentDirectory();

            try
            {
                DialogResult hasServerResult = ShowPrompt(
                    owner,
                    materialOwner,
                    "Do you already have Minecraft Bedrock Dedicated Server (bedrock_server.exe) downloaded/installed?\n\n" +
                    "• Yes — Choose your existing server folder\n" +
                    "• No — Set up a new Bedrock Server automatically\n",
                    "Select or Set Up Server Directory",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (hasServerResult == DialogResult.Yes)
                {
                    SelectExistingServerDirectory(owner, materialOwner, originalWorkingDir);
                }
                else if (hasServerResult == DialogResult.No)
                {
                    DialogResult autoSetupConsent = ShowPrompt(
                        owner,
                        materialOwner,
                        "Would you like BDSUtil to automatically detect the latest official Minecraft Bedrock Dedicated Server version, download it, and install it wherever you'd like?\n",
                        "Automatic Server Setup",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (autoSetupConsent == DialogResult.Yes)
                    {
                        await RunAutomaticServerSetupAsync(owner, materialOwner, originalWorkingDir);
                    }
                }
            }
            finally
            {
                try { Directory.SetCurrentDirectory(originalWorkingDir); } catch { }
                ClearSelectionFocus();
            }
        }

        private void SelectExistingServerDirectory(IWin32Window owner, Form materialOwner, string originalWorkingDir)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select the base installation directory hosting your Bedrock Dedicated Server files (bedrock_server.exe).";
                folderDialog.ShowNewFolderButton = false;

                if (!string.IsNullOrWhiteSpace(AppSettings.Instance.General.ServerExecutablePath) &&
                    Directory.Exists(AppSettings.Instance.General.ServerExecutablePath))
                {
                    folderDialog.SelectedPath = AppSettings.Instance.General.ServerExecutablePath;
                }

                if (folderDialog.ShowDialog(owner) == DialogResult.OK)
                {
                    string selectedDirectory = folderDialog.SelectedPath;
                    try { Directory.SetCurrentDirectory(originalWorkingDir); } catch { }

                    if (!ServerUpdates.IsValidServerDirectory(selectedDirectory))
                    {
                        DialogResult linkAnyway = ShowPrompt(
                            owner,
                            materialOwner,
                            $"bedrock_server.exe was not found in:\n{selectedDirectory}\n\nLink this directory anyway?",
                            "Executable Not Found",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (linkAnyway != DialogResult.Yes) return;
                    }

                    ApplySelectedServerDirectory(selectedDirectory);
                }
            }
        }

        private async Task RunAutomaticServerSetupAsync(IWin32Window owner, Form materialOwner, string originalWorkingDir)
        {
            string chosenRoot;
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select or create the folder where BDSUtil should download and install Minecraft Bedrock Dedicated Server.";
                folderDialog.ShowNewFolderButton = true;

                if (!string.IsNullOrWhiteSpace(AppSettings.Instance.General.ServerExecutablePath) &&
                    Directory.Exists(AppSettings.Instance.General.ServerExecutablePath))
                {
                    folderDialog.SelectedPath = AppSettings.Instance.General.ServerExecutablePath;
                }

                if (folderDialog.ShowDialog(owner) != DialogResult.OK)
                {
                    return;
                }

                chosenRoot = folderDialog.SelectedPath;
                try { Directory.SetCurrentDirectory(originalWorkingDir); } catch { }
            }

            if (string.IsNullOrWhiteSpace(chosenRoot)) return;

            if (ServerUpdates.IsValidServerDirectory(chosenRoot))
            {
                DialogResult linkExisting = ShowPrompt(
                    owner,
                    materialOwner,
                    $"A Bedrock Dedicated Server installation already exists in:\n{chosenRoot}\n\nLink this existing server directory instead of overwriting it?",
                    "Existing Server Detected",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information);

                if (linkExisting == DialogResult.Yes)
                {
                    ApplySelectedServerDirectory(chosenRoot);
                }
                return;
            }

            string installDirectory = chosenRoot;
            if (!ServerUpdates.IsDirectoryEmpty(chosenRoot))
            {
                DialogResult createSubfolder = ShowPrompt(
                    owner,
                    materialOwner,
                    $"The selected folder is not empty:\n{chosenRoot}\n\nWould you like to create a dedicated 'Bedrock Dedicated Server' subfolder inside it?\n" +
                    "• Yes — Install into 'Bedrock Dedicated Server' subfolder (Recommended)\n" +
                    "• No — Extract directly into the selected folder\n" +
                    "• Cancel — Abort setup",
                    "Folder Is Not Empty",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (createSubfolder == DialogResult.Cancel) return;
                if (createSubfolder == DialogResult.Yes)
                {
                    installDirectory = Path.Combine(chosenRoot, "Bedrock Dedicated Server");
                }
            }

            if (BrowseBTN != null) BrowseBTN.Enabled = false;
            this.UseWaitCursor = true;

            (string Version, string DownloadUrl) latest;
            try
            {
                latest = await ServerUpdates.GetLatestReleaseInfoAsync();
            }
            catch (Exception ex)
            {
                this.UseWaitCursor = false;
                if (BrowseBTN != null) BrowseBTN.Enabled = true;

                ShowPrompt(
                    owner,
                    materialOwner,
                    $"Could not detect the latest Bedrock Dedicated Server download:\n\n{ex.Message}",
                    "Download Lookup Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            finally
            {
                this.UseWaitCursor = false;
                if (BrowseBTN != null) BrowseBTN.Enabled = true;
            }

            using (var setupForm = new ServerUpdateForm(installDirectory, latest.Version, latest.DownloadUrl, isFreshInstall: true))
            {
                if (setupForm.ShowDialog(owner) == DialogResult.OK && ServerUpdates.IsValidServerDirectory(installDirectory))
                {
                    ApplySelectedServerDirectory(installDirectory);
                }
            }
        }

        private void ApplySelectedServerDirectory(string selectedDirectory)
        {
            if (string.IsNullOrWhiteSpace(selectedDirectory)) return;

            string previousDirectory = AppSettings.Instance?.General?.ServerExecutablePath;
            bool directoryChanged = !string.Equals(
                NormalizeDirectoryPath(previousDirectory),
                NormalizeDirectoryPath(selectedDirectory),
                StringComparison.OrdinalIgnoreCase);

            if (ServerPathTextBox != null)
            {
                ServerPathTextBox.Text = selectedDirectory;
            }

            AppSettings.Instance.Update(cfg => cfg.General.ServerExecutablePath = selectedDirectory);
            ServerPropertiesDocument.CommentOutServerPortV6(selectedDirectory);

            bool restartedForUpnp = false;
            if (AppSettings.Instance.Networking.EnableUPnP &&
                (directoryChanged || !UPnPEngine.TryGetActiveGameServerMapping(out _, out _)))
            {
                if (directoryChanged && !string.IsNullOrWhiteSpace(previousDirectory))
                {
                    string previousProps = Path.Combine(previousDirectory, "server.properties");
                    ServerPropertiesDocument.CommentOutProperty(previousProps, "server-udp-ports");
                }

                bool consolidateUdp = AppSettings.Instance.Networking.ConsolidateUdpPorts;
                if (UPnPEngine.TryApplyRequisitionedPortsToDirectory(selectedDirectory, consolidateUdp, out _, out string applyDetail))
                {
                    IWin32Window owner = (IWin32Window)this.FindForm() ?? this;
                    ShowPrompt(
                        owner,
                        this.FindForm(),
                        $"{applyDetail}\n\nYour server directory is ready without requiring a restart.",
                        "UPnP Port Mappings Applied",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    restartedForUpnp = PromptRestartForUpnpRealignment(
                        $"The server directory configuration changed while UPnP is active.\n\n{applyDetail}\n\n" +
                        "To requisition the required router ports and update server-udp-ports in the new server.properties file, BDSUtil needs to restart.\n\n" +
                        "Would you like to restart the manager app right now?");
                }
            }

            AutoModValidator.ValidateStateOnDirectoryChange(selectedDirectory, this.FindForm());

            if (!restartedForUpnp && AppSettings.Instance.RemoteApi.Enabled)
            {
                _ = Task.Run(() => RemoteApiManager.Instance.SynchronizeFromSettingsAsync(allowUpnpDiscovery: true));
            }
        }

        private static string NormalizeDirectoryPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            try
            {
                return Path.GetFullPath(path.Trim()).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
            catch
            {
                return path.Trim().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            }
        }
    }
}
