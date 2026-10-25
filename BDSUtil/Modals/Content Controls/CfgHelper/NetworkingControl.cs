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
        private bool _isManualChange = true;

        public NetworkingControl()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            InitializeComponent();
            this.AutoSize = false;

            _focusTrapLabel = new Label { Size = new System.Drawing.Size(0, 0), Location = new System.Drawing.Point(0, 0), TabStop = true };
            this.Controls.Add(_focusTrapLabel);

            if (CfgUPnP != null) CfgUPnP.CheckedChanged += CfgUPnP_CheckedChanged;
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

        private void CfgUPnP_CheckedChanged(object sender, EventArgs e)
        {
            ToggleManualInputContainers(!CfgUPnP.Checked);
            if (_isManualChange) SaveConfig();
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

            if (!File.Exists(_propertiesPath)) { ClearSelectionFocus(); return; }

            try
            {
                _isManualChange = false;

                string[] lines = File.ReadAllLines(_propertiesPath);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#")) continue;

                    int equalsIdx = line.IndexOf('=');
                    if (equalsIdx != -1)
                    {
                        string key = line.Substring(0, equalsIdx).Trim();
                        string val = line.Substring(equalsIdx + 1).Trim();

                        if (key.Equals("bdsutil-enable-upnp", StringComparison.OrdinalIgnoreCase)) continue;
                        _serverPropertiesMap[key] = val;
                    }
                }

                if (AppSettings.Instance != null) CfgUPnP.Checked = AppSettings.Instance.EnableUPnP;
                else CfgUPnP.Checked = false;

                ToggleManualInputContainers(!CfgUPnP.Checked);

                if (_serverPropertiesMap.TryGetValue("server-udp-ports", out string rawPorts) && !string.IsNullOrWhiteSpace(rawPorts))
                {
                    string[] nodes = rawPorts.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string node in nodes)
                    {
                        string cleanNode = node.Trim();
                        if (string.IsNullOrEmpty(cleanNode)) continue;

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
            finally
            {
                _isManualChange = true;
            }

            ClearSelectionFocus();
        }

        private void APBTN_Click(object sender, EventArgs e)
        {
            using (var addDialog = new BDSUtil.Modals.AddPortDialogue())
            {
                if (addDialog.ShowDialog(this) == DialogResult.OK && addDialog.IsConfirmed)
                {
                    string wanIpAddress = UPnPEngine.GetExternalIPAddress();
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
                            System.Threading.Tasks.Task.Run(() => {
                                try { UPnPEngine.ClosePort(portTarget); } catch { }
                            });
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
                _isManualChange = false;

                // Commit the target parameters clean state down to disk
                SaveConfig();

                // Re-verify and rebuild list layout items from disk explicitly 
                LoadConfig();
            }
            finally
            {
                _isManualChange = true;
            }

            ClearSelectionFocus();
        }

        public void SaveConfig()
        {
            if (string.IsNullOrEmpty(_propertiesPath) || !File.Exists(_propertiesPath)) return;

            try
            {
                bool historicalUpnpState = AppSettings.Instance?.GetSetting("EnableUPnP", false) ?? false;
                bool activeUiUpnpState = CfgUPnP.Checked;

                if (AppSettings.Instance != null)
                {
                    AppSettings.Instance.EnableUPnP = activeUiUpnpState;
                    AppSettings.Instance.SaveSettingsAsync().GetAwaiter().GetResult();
                }

                _serverPropertiesMap["server-udp-ports"] = string.Join(",", _activeUdpNodesCache);

                string[] lines = File.ReadAllLines(_propertiesPath);
                List<string> outputBufferLines = new List<string>();
                bool portsFieldHandled = false;

                foreach (string line in lines)
                {
                    string trimmedLine = line.Trim();

                    // ✅ FIX: Only transform the VERY FIRST instance found. Pass all subsequent examples through cleanly.
                    if (!portsFieldHandled && trimmedLine.IndexOf("server-udp-ports", StringComparison.OrdinalIgnoreCase) != -1)
                    {
                        outputBufferLines.Add($"server-udp-ports={_serverPropertiesMap["server-udp-ports"]}");
                        portsFieldHandled = true;
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(line) || trimmedLine.StartsWith("#"))
                    {
                        outputBufferLines.Add(line);
                        continue;
                    }

                    int equalsIdx = line.IndexOf('=');
                    if (equalsIdx != -1)
                    {
                        string key = line.Substring(0, equalsIdx).Trim();

                        if (key.Equals("bdsutil-enable-upnp", StringComparison.OrdinalIgnoreCase)) continue;

                        if (key.Equals("server-udp-ports", StringComparison.OrdinalIgnoreCase))
                        {
                            // Safety backup in case an uncommented line slipped past the indexer check above
                            if (!portsFieldHandled)
                            {
                                outputBufferLines.Add($"{key}={_serverPropertiesMap["server-udp-ports"]}");
                                portsFieldHandled = true;
                            }
                            else
                            {
                                // If already handled up top, strip out extra active lines so bedrock_server doesn't crash
                                continue;
                            }
                            continue;
                        }

                        if (_serverPropertiesMap.ContainsKey(key))
                        {
                            outputBufferLines.Add($"{key}={_serverPropertiesMap[key]}");
                            continue;
                        }
                    }
                    outputBufferLines.Add(line);
                }

                // FALLBACK: If no line containing 'server-udp-ports' was found anywhere, append it safely to the bottom
                if (!portsFieldHandled)
                {
                    outputBufferLines.Add($"server-udp-ports={_serverPropertiesMap["server-udp-ports"]}");
                }

                File.WriteAllLines(_propertiesPath, outputBufferLines, Encoding.UTF8);

                if (historicalUpnpState != activeUiUpnpState)
                {
                    if (ReaLTaiizor.Controls.MaterialMessageBox.Show(
                        this,
                        "[UPnP CAPABILITY INVERSION DETECTED]\n\n" +
                        "You have modified the core network translation engine configuration state.\n\n" +
                        "To map network interface ports securely and align background execution threads, " +
                        "BDSUtil requires a clean environmental application initialization loop cycle.\n\n" +
                        "Would you like to restart the manager app right now, or continue working manually?",
                        "Infrastructure Realignment Required",
                        true) == DialogResult.OK)
                    {
                        string serverDirectory = AppSettings.Instance?.ServerExecutablePath;

                        using (UPnPCloseProgressForm progressModal = new UPnPCloseProgressForm(serverDirectory))
                        {
                            progressModal.ShowDialog(this);
                        }

                        Application.Restart();
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Network save file violation error: {ex.Message}");
            }

            ClearSelectionFocus();
        }
    }
}
