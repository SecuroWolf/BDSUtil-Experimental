using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using BDSUtil.Helpers;

namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    public partial class PermissionsControl : UserControl
    {
        private string _configPath;
        private readonly List<OperatorEntry> _cachedOps = new List<OperatorEntry>();
        private readonly List<OperatorEntry> _preservedEntries = new List<OperatorEntry>();
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
            _preservedEntries.Clear();
            materialCheckListBox1.Items.Clear();

            if (JsonHelper.TryDeserializeFile(_configPath, out List<OperatorEntry> entries, JsonHelper.IndentedOptions) &&
                entries != null)
            {
                foreach (var entry in entries)
                {
                    if (entry == null || string.IsNullOrWhiteSpace(entry.Xuid))
                        continue;

                    entry.Xuid = entry.Xuid.Trim();
                    entry.Permission = string.IsNullOrWhiteSpace(entry.Permission)
                        ? "operator"
                        : entry.Permission.Trim();

                    if (entry.Permission.Equals("operator", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!_cachedOps.Any(o => string.Equals(o.Xuid, entry.Xuid, StringComparison.OrdinalIgnoreCase)))
                        {
                            _cachedOps.Add(entry);
                        }
                    }
                    else
                    {
                        if (!_preservedEntries.Any(o => string.Equals(o.Xuid, entry.Xuid, StringComparison.OrdinalIgnoreCase)))
                        {
                            _preservedEntries.Add(entry);
                        }
                    }
                }
            }

            HashSet<string> renderedXuids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var op in _cachedOps)
            {
                string displayLabel = PlayerIdentityStore.TryGetGamertag(op.Xuid, out string cachedName)
                    ? $"{cachedName} [Operator]"
                    : $"{op.Xuid} [Operator/Name Not Cached]";
                materialCheckListBox1.Items.Add(displayLabel, false);
                renderedXuids.Add(op.Xuid);
            }

            foreach (KeyValuePair<string, string> player in PlayerIdentityStore.GetKnownPlayersSnapshot())
            {
                if (renderedXuids.Contains(player.Key)) continue;
                materialCheckListBox1.Items.Add(player.Value, false);
            }

            ClearSelectionFocus();
        }

        private static bool TryResolveCheckboxTarget(string labelText, out string resolvedXuid, out string bareDisplayName)
        {
            resolvedXuid = string.Empty;
            bareDisplayName = string.Empty;
            if (string.IsNullOrWhiteSpace(labelText)) return false;

            const string uncachedSuffix = "[Operator/Name Not Cached]";
            if (labelText.EndsWith(uncachedSuffix, StringComparison.OrdinalIgnoreCase))
            {
                resolvedXuid = labelText.Substring(0, labelText.Length - uncachedSuffix.Length).Trim();
                bareDisplayName = resolvedXuid;
                return !string.IsNullOrEmpty(resolvedXuid);
            }

            bareDisplayName = labelText.Replace(" [Operator]", "").Trim();
            return PlayerIdentityStore.TryResolveXuid(bareDisplayName, out resolvedXuid);
        }

        private void OpPlayerBTN_Click(object sender, EventArgs e)
        {
            bool changesMade = false;
            foreach (object item in materialCheckListBox1.Items)
            {
                if (item is ReaLTaiizor.Controls.MaterialCheckBox checkbox && checkbox.Checked)
                {
                    if (!TryResolveCheckboxTarget(checkbox.Text, out string resolvedXuid, out string bareName)) continue;
                    if (_cachedOps.Any(o => string.Equals(o.Xuid, resolvedXuid, StringComparison.OrdinalIgnoreCase))) continue;

                    _preservedEntries.RemoveAll(o => string.Equals(o.Xuid, resolvedXuid, StringComparison.OrdinalIgnoreCase));
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
                    if (!TryResolveCheckboxTarget(checkbox.Text, out string resolvedXuid, out string bareName)) continue;

                    var opRecord = _cachedOps.FirstOrDefault(o => string.Equals(o.Xuid, resolvedXuid, StringComparison.OrdinalIgnoreCase));
                    if (opRecord != null)
                    {
                        _cachedOps.Remove(opRecord);
                        checkbox.Text = PlayerIdentityStore.TryGetGamertag(resolvedXuid, out string knownName)
                            ? knownName
                            : bareName;
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
                var allEntries = new List<OperatorEntry>(_cachedOps);
                foreach (var entry in _preservedEntries)
                {
                    if (!allEntries.Any(o => string.Equals(o.Xuid, entry.Xuid, StringComparison.OrdinalIgnoreCase)))
                    {
                        allEntries.Add(entry);
                    }
                }

                JsonHelper.WriteJsonFileAtomically(_configPath, allEntries, JsonHelper.IndentedOptions);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Permissions write exception: {ex.Message}");
            }
        }
    }
    public class OperatorEntry
    {
        [JsonPropertyName("permission")]
        public string Permission { get; set; } = "operator";

        [JsonPropertyName("xuid")]
        public string Xuid { get; set; } = string.Empty;

        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }
    }
}
