using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    public partial class PermissionsControl : UserControl
    {
        private string _configPath;
        private readonly List<OperatorEntry> _cachedOps = new List<OperatorEntry>();
        private readonly Label _focusTrapLabel;

        public PermissionsControl()
        {
            InitializeComponent();

            _focusTrapLabel = new Label
            {
                Size = new System.Drawing.Size(0, 0),
                Location = new System.Drawing.Point(0, 0),
                TabStop = true
            };
            this.Controls.Add(_focusTrapLabel);
        }

        public void InitializeControl(string serverRoot)
        {
            _configPath = Path.Combine(serverRoot, "permissions.json");
            LoadConfig();
        }

        public void ClearSelectionFocus()
        {
            if (_focusTrapLabel != null && _focusTrapLabel.CanFocus)
            {
                _focusTrapLabel.Focus();
            }
        }

        private void PermissionsControl_Load(object sender, EventArgs e)
        {
            materialCheckListBox1.Items.Clear();
            ClearSelectionFocus();
        }

        private void LoadConfig()
        {
            _cachedOps.Clear();
            materialCheckListBox1.Items.Clear();

            if (File.Exists(_configPath))
            {
                try
                {
                    string rawJson = File.ReadAllText(_configPath, System.Text.Encoding.UTF8).Trim();
                    if (!string.IsNullOrEmpty(rawJson) && rawJson != "[]")
                    {
                        List<string> quotedTokens = new List<string>();
                        int idx = 0;
                        while ((idx = rawJson.IndexOf('"', idx)) != -1)
                        {
                            int endIdx = rawJson.IndexOf('"', idx + 1);
                            if (endIdx == -1) break;
                            quotedTokens.Add(rawJson.Substring(idx + 1, endIdx - idx - 1));
                            idx = endIdx + 1;
                        }

                        for (int i = 0; i < quotedTokens.Count - 1; i++)
                        {
                            if (quotedTokens[i].Equals("permission", StringComparison.OrdinalIgnoreCase) &&
                                quotedTokens[i + 1].Equals("operator", StringComparison.OrdinalIgnoreCase))
                            {
                                int searchStart = Math.Max(0, i - 4);
                                int searchEnd = Math.Min(quotedTokens.Count - 1, i + 4);

                                for (int j = searchStart; j < searchEnd; j++)
                                {
                                    if (quotedTokens[j].Equals("xuid", StringComparison.OrdinalIgnoreCase) && j + 1 < quotedTokens.Count)
                                    {
                                        string foundXuid = quotedTokens[j + 1];
                                        if (!string.IsNullOrEmpty(foundXuid) && !_cachedOps.Any(o => o.Xuid == foundXuid))
                                        {
                                            _cachedOps.Add(new OperatorEntry { Xuid = foundXuid, Permission = "operator" });
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Permissions load exception: {ex.Message}");
                }
            }

            HashSet<string> renderedXuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var op in _cachedOps)
            {
                string displayLabel = string.Empty;
                lock (PlayerIdentityStore.LockObject)
                {
                    if (PlayerIdentityStore.DiscoveredCache.ContainsKey(op.Xuid))
                    {
                        displayLabel = $"{PlayerIdentityStore.DiscoveredCache[op.Xuid]} [Operator]";
                    }
                    else
                    {
                        displayLabel = $"{op.Xuid} [Operator/Name Not Cached]";
                    }
                }
                materialCheckListBox1.Items.Add(displayLabel, false);
                renderedXuids.Add(op.Xuid);
            }

            lock (PlayerIdentityStore.LockObject)
            {
                var sortedPlayers = PlayerIdentityStore.DiscoveredCache.OrderBy(p => p.Value).ToList();
                foreach (KeyValuePair<string, string> player in sortedPlayers)
                {
                    if (renderedXuids.Contains(player.Key)) continue;
                    materialCheckListBox1.Items.Add(player.Value, false);
                }
            }

            ClearSelectionFocus();
        }
        private void OpPlayerBTN_Click(object sender, EventArgs e)
        {
            bool changesMade = false;
            foreach (object item in materialCheckListBox1.Items)
            {
                if (item is ReaLTaiizor.Controls.MaterialCheckBox checkbox && checkbox.Checked)
                {
                    string bareName = checkbox.Text.Replace(" [Operator]", "").Trim();
                    string resolvedXuid = string.Empty;

                    lock (PlayerIdentityStore.LockObject)
                    {
                        resolvedXuid = PlayerIdentityStore.DiscoveredCache.FirstOrDefault(
                            x => x.Value.Equals(bareName, StringComparison.OrdinalIgnoreCase)).Key;
                    }

                    if (string.IsNullOrEmpty(resolvedXuid)) continue;
                    if (_cachedOps.Any(o => o.Xuid == resolvedXuid)) continue;

                    _cachedOps.Add(new OperatorEntry { Xuid = resolvedXuid, Permission = "operator" });
                    checkbox.Text = $"{bareName} [Operator]";
                    checkbox.Checked = false;
                    changesMade = true;
                }
            }

            if (changesMade) SaveConfig();
            else ReaLTaiizor.Controls.MaterialMessageBox.Show("No checked players were eligible for operator promotion.", "Operation Intercepted");
            ClearSelectionFocus();
        }

        private void DeOpPlayerBTN_Click(object sender, EventArgs e)
        {
            bool changesMade = false;
            foreach (object item in materialCheckListBox1.Items)
            {
                if (item is ReaLTaiizor.Controls.MaterialCheckBox checkbox && checkbox.Checked)
                {
                    string bareName = checkbox.Text.Replace(" [Operator]", "").Trim();
                    string resolvedXuid = string.Empty;

                    lock (PlayerIdentityStore.LockObject)
                    {
                        resolvedXuid = PlayerIdentityStore.DiscoveredCache.FirstOrDefault(
                            x => x.Value.Equals(bareName, StringComparison.OrdinalIgnoreCase)).Key;
                    }

                    if (string.IsNullOrEmpty(resolvedXuid)) continue;

                    var opRecord = _cachedOps.FirstOrDefault(o => o.Xuid == resolvedXuid);
                    if (opRecord != null)
                    {
                        _cachedOps.Remove(opRecord);
                        checkbox.Text = bareName;
                        checkbox.Checked = false;
                        changesMade = true;
                    }
                }
            }

            if (changesMade) SaveConfig();
            else ReaLTaiizor.Controls.MaterialMessageBox.Show("No active operators were selected for role revocation.", "Operation Intercepted");
            ClearSelectionFocus();
        }

        public void SaveConfig()
        {
            if (string.IsNullOrEmpty(_configPath)) return;
            try
            {
                List<string> jsonLines = new List<string> { "[" };
                for (int i = 0; i < _cachedOps.Count; i++)
                {
                    var op = _cachedOps[i];
                    jsonLines.Add("   {");
                    jsonLines.Add($"      \"permission\" : \"{op.Permission}\",");
                    jsonLines.Add($"      \"xuid\" : \"{op.Xuid}\"");
                    jsonLines.Add(i == _cachedOps.Count - 1 ? "   }" : "   },");
                }
                jsonLines.Add("]");
                File.WriteAllLines(_configPath, jsonLines, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Permissions write exception: {ex.Message}");
            }
        }
    }
    public class OperatorEntry
    {
        public string Permission { get; set; } = "operator";
        public string Xuid { get; set; } = string.Empty;
    }

    public static class PlayerIdentityStore
    {
        public static readonly object LockObject = new object();
        public static readonly Dictionary<string, string> DiscoveredCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private static readonly string StoragePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bds_players.dat");

        static PlayerIdentityStore()
        {
            LoadFromDisk();
        }

        private static void LoadFromDisk()
        {
            if (!File.Exists(StoragePath)) return;
            try
            {
                string[] lines = File.ReadAllLines(StoragePath, System.Text.Encoding.UTF8);
                lock (LockObject)
                {
                    foreach (string line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] segments = line.Split(new string[] { "||" }, StringSplitOptions.None);
                        if (segments.Length == 2)
                        {
                            string targetXuid = segments[0].Trim();
                            string targetName = segments[1].Trim();
                            if (!string.IsNullOrEmpty(targetXuid) && !string.IsNullOrEmpty(targetName))
                            {
                                DiscoveredCache[targetXuid] = targetName;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        public static void SavePlayerNode(string xuid, string gamertag)
        {
            if (string.IsNullOrEmpty(xuid) || string.IsNullOrEmpty(gamertag)) return;
            try
            {
                List<string> outputLines = new List<string>();
                lock (LockObject)
                {
                    DiscoveredCache[xuid] = gamertag;
                    foreach (KeyValuePair<string, string> kvp in DiscoveredCache)
                    {
                        outputLines.Add($"{kvp.Key}||{kvp.Value}");
                    }
                }
                File.WriteAllLines(StoragePath, outputLines, System.Text.Encoding.UTF8);
            }
            catch { }
        }
    }
}
