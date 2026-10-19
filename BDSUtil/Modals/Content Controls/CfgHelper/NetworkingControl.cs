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

        public NetworkingControl()
        {
            InitializeComponent();

            // Wire the layout container disabling trigger hook right to your dynamic theme switch
            if (CfgUPnP != null)
            {
                CfgUPnP.CheckedChanged += CfgUPnP_CheckedChanged;
            }

            // Explicitly link your button clicks to their event handlers
            if (APBTN != null) APBTN.Click += APBTN_Click;
            if (RPBTN != null) RPBTN.Click += RPBTN_Click;
        }

        /// <summary>
        /// Captures the server root execution path and kicks off the initial property file loading.
        /// </summary>
        public void InitializeControl(string serverRoot)
        {
            _propertiesPath = Path.Combine(serverRoot, "server.properties");
            LoadConfig();
        }

        /// <summary>
        /// Reads network configuration strings from disk and binds values directly into visual canvas elements.
        /// </summary>
        private void LoadConfig()
        {
            _serverPropertiesMap.Clear();
            PMList.Items.Clear();

            if (!File.Exists(_propertiesPath)) return;

            try
            {
                string[] lines = File.ReadAllLines(_propertiesPath);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#")) continue;

                    int equalsIdx = line.IndexOf('=');
                    if (equalsIdx != -1)
                    {
                        string key = line.Substring(0, equalsIdx).Trim();
                        string val = line.Substring(equalsIdx + 1).Trim();

                        // CRITICAL FIX: Skip and ignore the legacy key completely so it never enters the memory cache
                        if (key.Equals("bdsutil-enable-upnp", StringComparison.OrdinalIgnoreCase)) continue;

                        _serverPropertiesMap[key] = val;
                    }
                }


                // 1. Process UPnP Configuration State directly from our own configuration variables
                // FIXED: Reading from AppSettings.Instance instead of parsing server.properties
                if (AppSettings.Instance != null)
                {
                    CfgUPnP.Checked = AppSettings.Instance.EnableUPnP;
                }
                else
                {
                    CfgUPnP.Checked = false; // Safety fallback default setting
                }

                // Force initial visual card state evaluation right away on file load
                ToggleManualInputContainers(!CfgUPnP.Checked);

                // 2. Parse out the complex server-udp-ports map lines array
                if (_serverPropertiesMap.TryGetValue("server-udp-ports", out string rawPorts) && !string.IsNullOrWhiteSpace(rawPorts))
                {
                    string[] nodes = rawPorts.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string node in nodes)
                    {
                        string cleanNode = node.Trim();
                        if (string.IsNullOrEmpty(cleanNode)) continue;

                        // Check for fully articulated map configurations ([IP]:Ext:Int)
                        string[] segments = cleanNode.Split(':');
                        if (segments.Length >= 2)
                        {
                            string internalPort = segments[segments.Length - 1];
                            string externalPort = segments[segments.Length - 2];

                            string listDisplayLabel = (externalPort == internalPort)
                                ? $"UDP Forward Node: {externalPort}"
                                : $"UDP Port Map: {externalPort} mapped to internal port {internalPort}";

                            // FIXED: PMList.Items.Add returns void in ReaLTaiizor. Append it, then apply your tag to the final index tracking slot.
                            PMList.Items.Add(listDisplayLabel, false);
                            int addedIndex = PMList.Items.Count - 1;
                            if (addedIndex >= 0)
                            {
                                PMList.Items[addedIndex].Tag = cleanNode;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Network load file violation error: {ex.Message}");
            }
        }

        /// <summary>
        /// Assembles internal lists and flushes clean string nodes back down into server.properties file bytes.
        /// </summary>
        public void SaveConfig()
        {
            if (string.IsNullOrEmpty(_propertiesPath) || !File.Exists(_propertiesPath)) return;

            try
            {
                // Capture the initial saved database configuration state right before evaluating live UI toggles
                bool historicalUpnpState = AppSettings.Instance?.GetSetting("EnableUPnP", false) ?? false;
                bool activeUiUpnpState = CfgUPnP.Checked;

                // Commit the toggle selection back to our own internal configuration state variables
                if (AppSettings.Instance != null)
                {
                    AppSettings.Instance.EnableUPnP = activeUiUpnpState;
                    AppSettings.Instance.SaveSettingsAsync().GetAwaiter().GetResult();
                }

                // Formulate the multi-node comma-separated string sequence directly from live list views
                List<string> activeUdpStrings = new List<string>();
                for (int i = 0; i < PMList.Items.Count; i++)
                {
                    var item = PMList.Items[i];
                    if (item != null && item.Tag is string savedStringNode)
                    {
                        activeUdpStrings.Add(savedStringNode);
                    }
                }

                _serverPropertiesMap["server-udp-ports"] = string.Join(",", activeUdpStrings);

                // Read structural layout commentary lines and cleanly append modified values matching explicit keys
                string[] lines = File.ReadAllLines(_propertiesPath);
                List<string> outputBufferLines = new List<string>();

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#"))
                    {
                        outputBufferLines.Add(line);
                        continue;
                    }

                    int equalsIdx = line.IndexOf('=');
                    if (equalsIdx != -1)
                    {
                        string key = line.Substring(0, equalsIdx).Trim();

                        if (key.Equals("bdsutil-enable-upnp", StringComparison.OrdinalIgnoreCase)) continue;

                        if (_serverPropertiesMap.ContainsKey(key))
                        {
                            outputBufferLines.Add($"{key}={_serverPropertiesMap[key]}");
                            continue;
                        }
                    }
                    outputBufferLines.Add(line);
                }

                File.WriteAllLines(_propertiesPath, outputBufferLines, Encoding.UTF8);

                // --- DETECT INFRASTRUCTURE ADJUSTMENTS & FORCE RESTART INTERCEPT ---
                if (historicalUpnpState != activeUiUpnpState)
                {
                    // Frame out the configuration alignment check prompt
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
                        // FIXED: Open our lightweight cleanup modal to offload network operations safely off the UI thread!
                        string serverDirectory = AppSettings.Instance?.ServerExecutablePath;

                        using (UPnPCloseProgressForm progressModal = new UPnPCloseProgressForm(serverDirectory))
                        {
                            // ShowDialog runs the network drops safely while maintaining full visual feedback updates
                            progressModal.ShowDialog(this);
                        }

                        // The exact millisecond the router confirms cleanup is done, the modal drops and we bounce the app instantly!
                        Application.Restart();
                        Environment.Exit(0);
                    }
                }
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Network save file violation error: {ex.Message}");
            }
        }

        private void CfgUPnP_CheckedChanged(object sender, EventArgs e)
        {
            ToggleManualInputContainers(!CfgUPnP.Checked);
        }

        /// <summary>
        /// Toggles target layout visibility panels instantly based on UPnP validation selections.
        /// </summary>
        private void ToggleManualInputContainers(bool enableManual)
        {
            if (ManPMCtrls != null) ManPMCtrls.Enabled = enableManual;
            if (PortBTNCard != null) PortBTNCard.Enabled = enableManual;
        }

        /// <summary>
        /// Event hook linked straight to your APBTN (Add Port Button) trigger loop channel.
        /// </summary>
        /// <summary>
        /// Event hook linked straight to your APBTN (Add Port Button) trigger loop channel.
        /// </summary>
        /// <summary>
        /// Event hook linked straight to your APBTN (Add Port Button) trigger loop channel.
        /// </summary>
        private void APBTN_Click(object sender, EventArgs e)
        {
            // FIXED: Explicitly targeting the dialogue utilizing its absolute modal namespace path
            using (var addDialog = new BDSUtil.Modals.AddPortDialogue())
            {
                if (addDialog.ShowDialog(this) == DialogResult.OK && addDialog.IsConfirmed)
                {
                    // Dynamic public lookup handles Nethernet mappings perfectly for external users!
                    string wanIpAddress = UPnPEngine.GetExternalIPAddress();

                    // Re-formulate raw property token block formatting matching Mojang parameters ([IP]:Ext:Int)
                    string formattedPropertyToken = $"{wanIpAddress}:{addDialog.ExternalPort}:{addDialog.InternalPort}";

                    // Build user descriptive dashboard visualization item text string rows
                    string cleanDisplayLabel = (addDialog.ExternalPort == addDialog.InternalPort)
                        ? $"UDP Forward Node: {addDialog.ExternalPort}"
                        : $"UDP Port Map: {addDialog.ExternalPort} mapped to internal port {addDialog.InternalPort}";

                    // Check for duplications inside our current canvas checklist tracks
                    for (int i = 0; i < PMList.Items.Count; i++)
                    {
                        if (PMList.Items[i].Tag is string existingToken)
                        {
                            if (existingToken.Equals(formattedPropertyToken, StringComparison.OrdinalIgnoreCase))
                            {
                                ReaLTaiizor.Controls.MaterialMessageBox.Show(this, "That specific port translation map rule node already exists inside your active grid.", "Duplicate Conflict");
                                return;
                            }
                        }
                    }

                    // FIXED: PMList.Items.Add returns void in ReaLTaiizor. We append it, then fetch the index by count.
                    PMList.Items.Add(cleanDisplayLabel, false);
                    int newIndex = PMList.Items.Count - 1;
                    if (newIndex >= 0)
                    {
                        PMList.Items[newIndex].Tag = formattedPropertyToken;
                    }
                }
            }
        }

        /// <summary>
        /// Event hook linked straight to your RPBTN (Remove Selected Port Button) trigger loop channel.
        /// </summary>
        private void RPBTN_Click(object sender, EventArgs e)
        {
            // Gather all checked items for removal
            var indicesToRemove = new List<int>();

            for (int i = 0; i < PMList.Items.Count; i++)
            {
                if (PMList.Items[i].Checked)
                {
                    indicesToRemove.Add(i);

                    // Cleanly tear port forward maps down from your physical hardware router live if UPnP handles it
                    if (PMList.Items[i].Tag is string tokenString)
                    {
                        string[] parts = tokenString.Split(':');
                        if (parts.Length >= 2 && int.TryParse(parts[parts.Length - 2], out int extPortToPurge))
                        {
                            UPnPEngine.ClosePort(extPortToPurge);
                        }
                    }
                }
            }

            if (indicesToRemove.Count == 0)
            {
                ReaLTaiizor.Controls.MaterialMessageBox.Show(this, "No specific routing configuration rows are checked for deletion tracking.", "Selection Track Empty");
                return;
            }

            // Remove items in reverse order to preserve indexing constraints during the loop pass
            foreach (int index in indicesToRemove.OrderByDescending(v => v))
            {
                PMList.Items.RemoveAt(index);
            }
        }

        private void NetworkingControl_Load(object sender, EventArgs e)
        {

        }
    }
}
