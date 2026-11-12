using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using BDSUtil.Helpers;

namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    public partial class NetworkingControl : UserControl
    {
        private string _propertiesPath;
        private readonly Dictionary<string, string> _serverPropertiesMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly List<string> _activeUdpNodesCache = new List<string>();
        private readonly Label _focusTrapLabel;
        private readonly ReaLTaiizor.Controls.MaterialButton _verifyPortsBtn;

        public NetworkingControl()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            InitializeComponent();
            this.AutoSize = false;

            _focusTrapLabel = new Label { Size = new System.Drawing.Size(0, 0), Location = new System.Drawing.Point(0, 0), TabStop = true };
            this.Controls.Add(_focusTrapLabel);

            _verifyPortsBtn = new ReaLTaiizor.Controls.MaterialButton
            {
                Text = "Verify Ports",
                Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new System.Drawing.Point(618, 14),
                Size = new System.Drawing.Size(130, 36),
                TabIndex = 12
            };
            _verifyPortsBtn.Click += VerifyPortsBtn_Click;
            if (materialCard3 != null)
            {
                materialCard3.Controls.Add(_verifyPortsBtn);
                _verifyPortsBtn.BringToFront();
            }

            if (APBTN != null) APBTN.Click += APBTN_Click;
            if (RPBTN != null) RPBTN.Click += RPBTN_Click;
        }

        public void InitializeControl(string serverRoot)
        {
            _propertiesPath = Path.Combine(serverRoot, "server.properties");
            LoadConfig();
        }

        public void ClearSelectionFocus()
        {
            if (_focusTrapLabel != null && _focusTrapLabel.CanFocus) _focusTrapLabel.Focus();
        }

        private void NetworkingControl_Load(object sender, EventArgs e)
        {
            ClearSelectionFocus();
        }

        private void ToggleManualInputContainers(bool enableManual)
        {
            if (ManPMCtrls != null) ManPMCtrls.Enabled = enableManual;
            if (PortBTNCard != null) PortBTNCard.Enabled = enableManual;
        }
        private void LoadConfig()
        {
            _serverPropertiesMap.Clear();
            _activeUdpNodesCache.Clear();

            // ✅ FIX: Purge the tracking data AND physically vaporize old UI checkbox handles
            PMList.Items.Clear();
            PMList.Controls.Clear();

            bool upnpEnabled = AppSettings.Instance?.Networking?.EnableUPnP ?? false;
            bool consolidateEnabled = AppSettings.Instance?.Networking?.ConsolidateUdpPorts ?? false;
            ToggleManualInputContainers(!upnpEnabled);
            UpdateUpnpBannerText(upnpEnabled, consolidateEnabled);

            if (!File.Exists(_propertiesPath)) { ClearSelectionFocus(); return; }

            try
            {
                ServerPropertiesDocument.CommentOutServerPortV6(_propertiesPath);
                foreach (KeyValuePair<string, string> kvp in ServerPropertiesDocument.ReadProperties(_propertiesPath))
                {
                    _serverPropertiesMap[kvp.Key] = kvp.Value;
                }

                if (_serverPropertiesMap.TryGetValue("server-udp-ports", out string rawPorts) && !string.IsNullOrWhiteSpace(rawPorts))
                {
                    foreach (string cleanNode in ServerPropertiesDocument.ParseUdpNodeTokens(rawPorts))
                    {
                        _activeUdpNodesCache.Add(cleanNode);
                        string[] segments = cleanNode.Split(':');

                        if (segments.Length >= 2)
                        {
                            string internalPort = segments[segments.Length - 1];
                            string externalPort = segments[segments.Length - 2];

                            string listDisplayLabel = (externalPort == internalPort)
                                ? $"UDP Forward Node: {externalPort}"
                                : $"UDP Port Map: {externalPort} mapped to internal port {internalPort}";

                            PMList.Items.Add(listDisplayLabel, false);
                        }
                        else
                        {
                            PMList.Items.Add($"UDP Port Node: {cleanNode}", false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Network load file violation error: {ex.Message}");
            }

            ClearSelectionFocus();
        }

        private async void APBTN_Click(object sender, EventArgs e)
        {
            using (var addDialog = new BDSUtil.Modals.AddPortDialogue())
            {
                if (addDialog.ShowDialog(this) == DialogResult.OK && addDialog.IsConfirmed)
                {
                    string wanIpAddress = await UPnPEngine.GetExternalIPAddressAsync();
                    string formattedPropertyToken = $"{wanIpAddress}:{addDialog.ExternalPort}:{addDialog.InternalPort}";

                    string cleanDisplayLabel = (addDialog.ExternalPort == addDialog.InternalPort)
                        ? $"UDP Forward Node: {addDialog.ExternalPort}"
                        : $"UDP Port Map: {addDialog.ExternalPort} mapped to internal port {addDialog.InternalPort}";

                    if (_activeUdpNodesCache.Contains(formattedPropertyToken, StringComparer.OrdinalIgnoreCase))
                    {
                        ReaLTaiizor.Controls.MaterialMessageBox.Show(this, "That specific port translation map rule node already exists inside your active grid.", "Duplicate Conflict");
                        ClearSelectionFocus();
                        return;
                    }

                    _activeUdpNodesCache.Add(formattedPropertyToken);
                    PMList.Items.Add(cleanDisplayLabel, false);

                    SaveConfig();
                }
            }
            ClearSelectionFocus();
        }

        private async void VerifyPortsBtn_Click(object sender, EventArgs e)
        {
            _verifyPortsBtn.Enabled = false;
            string originalText = _verifyPortsBtn.Text;
            _verifyPortsBtn.Text = "Checking...";

            try
            {
                int primaryPort = 19132;
                if (_serverPropertiesMap.TryGetValue("server-port", out string rawPrimary) &&
                    int.TryParse(rawPrimary, out int parsedPrimary) &&
                    parsedPrimary > 0 && parsedPrimary <= 65535)
                {
                    primaryPort = parsedPrimary;
                }

                var targets = new List<(int ExternalPort, int InternalPort, string Protocol)>();
                if (AppSettings.Instance?.Networking?.EnableUPnP == true)
                {
                    targets.Add((primaryPort, primaryPort, "TCP"));
                }
                targets.Add((primaryPort, primaryPort, "UDP"));

                foreach (string nodeToken in _activeUdpNodesCache)
                {
                    string[] parts = nodeToken.Split(':');
                    if (parts.Length >= 2 &&
                        int.TryParse(parts[parts.Length - 2], out int extPort) &&
                        int.TryParse(parts[parts.Length - 1], out int intPort) &&
                        extPort > 0 && extPort <= 65535 &&
                        intPort > 0 && intPort <= 65535 &&
                        !targets.Any(t => t.ExternalPort == extPort && t.InternalPort == intPort && t.Protocol == "UDP"))
                    {
                        targets.Add((extPort, intPort, "UDP"));
                        if (targets.Count >= 5) break;
                    }
                }

                if (AppSettings.Instance?.RemoteApi?.Enabled == true &&
                    BDSUtil.Helpers.RemoteApi.RemoteApiManager.Instance.BoundPort > 0)
                {
                    int apiPort = BDSUtil.Helpers.RemoteApi.RemoteApiManager.Instance.BoundPort;
                    targets.Add((apiPort, apiPort, "TCP"));
                }

                var options = new PortVerificationOptions
                {
                    ExternalProbeUrl = AppSettings.Instance?.Networking?.ResolveEffectiveExternalProbeUrl() ?? string.Empty
                };

                var gateway = await UPnPEngine.DiscoverGatewayAsync(TimeSpan.FromMilliseconds(2500));
                options.Gateway = gateway;

                var report = new StringBuilder();
                foreach (var target in targets)
                {
                    PortForwardVerificationResult res = await UPnPEngine.VerifyPortAsync(
                        target.ExternalPort,
                        target.InternalPort,
                        target.Protocol,
                        options);

                    if (report.Length == 0)
                    {
                        report.AppendLine($"Local IP: {res.LocalIp}");
                        report.AppendLine($"Public External IP: {res.PublicExternalIp}");
                        report.AppendLine($"Router WAN IP: {(string.IsNullOrWhiteSpace(res.RouterExternalIp) ? "(UPnP Gateway Not Found)" : res.RouterExternalIp)}");
                        report.AppendLine();
                    }

                    string forwardStatus = res.IsForwardedToHost
                        ? (res.ForwardedViaManualOrUnknownRule ? "Confirmed (Manual/DMZ)" : "Confirmed to Host")
                        : (res.IsAssignedToAnotherHost ? $"Conflict ({res.MappedInternalClient})" : "Not Confirmed");

                    string externalStatus = res.IsExternallyAccessible switch
                    {
                        true => "Accessible",
                        false => "Blocked / Unreachable",
                        null => "Indeterminate (Loopback Timeout)"
                    };

                    report.AppendLine($"Port {target.Protocol} {target.ExternalPort} -> {target.InternalPort}:");
                    report.AppendLine($"  • Forwarded to Host: {forwardStatus}");
                    report.AppendLine($"  • External IP Reachable: {externalStatus} ({res.AccessibilityState})");
                    report.AppendLine($"  • Details: {res.DiagnosticMessage}");
                    report.AppendLine();
                }

                ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    this,
                    report.ToString().TrimEnd(),
                    "Port Forwarding & External Reachability Report",
                    MessageBoxButtons.OK,
                    false,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center);
            }
            catch (Exception ex)
            {
                ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    this,
                    $"Port verification encountered an error:\n\n{ex.Message}",
                    "Verification Error",
                    MessageBoxButtons.OK,
                    false,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center);
            }
            finally
            {
                _verifyPortsBtn.Text = originalText;
                _verifyPortsBtn.Enabled = true;
                ClearSelectionFocus();
            }
        }

        private void UpdateUpnpBannerText(bool upnpEnabled, bool consolidateEnabled)
        {
            if (UPnPLBL == null) return;

            if (upnpEnabled)
            {
                string modeTag = consolidateEnabled ? " (Consolidated single-UDP mode)" : "";
                UPnPLBL.Text = $"UPnP Automation: Enabled in Settings -> Networking{modeTag}\r\nManual port editing is locked while UPnP manages server-udp-ports.";
            }
            else
            {
                UPnPLBL.Text = "UPnP Automation: Disabled (toggle in Settings -> Networking)\r\nConfigure manual server-udp-ports mappings below.";
            }
        }

        private void RPBTN_Click(object sender, EventArgs e)
        {
            var indicesToRemove = new List<int>();

            for (int i = 0; i < PMList.Items.Count; i++)
            {
                if (PMList.Items[i].Checked)
                {
                    indicesToRemove.Add(i);

                    if (i < _activeUdpNodesCache.Count)
                    {
                        string tokenString = _activeUdpNodesCache[i];
                        string[] parts = tokenString.Split(':');
                        if (parts.Length >= 2 && int.TryParse(parts[parts.Length - 2], out int extPortToPurge))
                        {
                            int portTarget = extPortToPurge;
                            _ = System.Threading.Tasks.Task.Run(() => UPnPEngine.ClosePortAsync(portTarget, "UDP"));
                        }
                    }
                }
            }

            if (indicesToRemove.Count == 0)
            {
                ReaLTaiizor.Controls.MaterialMessageBox.Show(this, "No specific routing configuration rows are checked for deletion tracking.", "Selection Track Empty");
                ClearSelectionFocus();
                return;
            }

            // Remove sequentially from backend memory track array
            foreach (int index in indicesToRemove.OrderByDescending(v => v))
            {
                if (index < _activeUdpNodesCache.Count) _activeUdpNodesCache.RemoveAt(index);
            }

            try
            {
                // Commit the target parameters clean state down to disk
                SaveConfig();

                // Re-verify and rebuild list layout items from disk explicitly 
                LoadConfig();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Network remove error: {ex.Message}");
            }

            ClearSelectionFocus();
        }

        public void SaveConfig()
        {
            if (string.IsNullOrEmpty(_propertiesPath) || !File.Exists(_propertiesPath)) return;

            bool upnpEnabled = AppSettings.Instance?.Networking?.EnableUPnP ?? false;
            if (upnpEnabled)
            {
                ClearSelectionFocus();
                return;
            }

            try
            {
                string serializedPorts = string.Join(",", _activeUdpNodesCache);
                _serverPropertiesMap["server-udp-ports"] = serializedPorts;
                ServerPropertiesDocument.SetServerUdpPorts(_propertiesPath, serializedPorts);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Network save file violation error: {ex.Message}");
            }

            ClearSelectionFocus();
        }
    }
}
