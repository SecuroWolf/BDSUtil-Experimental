using BDSUtil.Helpers;
using BDSUtil.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using M3MaterialCheckBox = Material3.WinForms.Controls.MaterialCheckBox;

namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    public partial class WhitelistControl : Material3UserControl
    {
        private string _configPath;
        private readonly Label _focusTrapLabel;
        private readonly List<WhitelistPlayerEntry> _cachedPlayers = new List<WhitelistPlayerEntry>();

        public WhitelistControl()
        {
            InitializeComponent();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = false;

            // =========================================================================
            // 🛡️ UNSELECTED BASELINE FOCUS TRAP
            // =========================================================================
            _focusTrapLabel = new Label
            {
                Size = new System.Drawing.Size(0, 0),
                Location = new System.Drawing.Point(0, 0),
                TabStop = true
            };
            this.Controls.Add(_focusTrapLabel);
            // =========================================================================
        }

        public void InitializeControl(string serverRoot)
        {
            _configPath = Path.Combine(serverRoot, "allowlist.json");
            LoadConfig();
        }

        public void ClearSelectionFocus()
        {
            if (_focusTrapLabel != null && _focusTrapLabel.CanFocus)
            {
                _focusTrapLabel.Focus();
            }
        }

        private void WhitelistControl_Load(object sender, EventArgs e)
        {
            materialCheckListBox1.Items.Clear();
            ClearSelectionFocus();
        }
        private void LoadConfig()
        {
            _cachedPlayers.Clear();
            materialCheckListBox1.Items.Clear();

            if (!File.Exists(_configPath))
            {
                try { JsonHelper.WriteJsonFileAtomically(_configPath, _cachedPlayers, JsonHelper.IndentedOptions); } catch { }
                ClearSelectionFocus();
                return;
            }

            try
            {
                if (JsonHelper.TryDeserializeFile(_configPath, out List<WhitelistPlayerEntry> loaded, JsonHelper.IndentedOptions) &&
                    loaded != null)
                {
                    foreach (var player in loaded)
                    {
                        if (player != null && !string.IsNullOrWhiteSpace(player.Name))
                        {
                            player.Xuid = string.IsNullOrWhiteSpace(player.Xuid) ? null : player.Xuid.Trim();
                            _cachedPlayers.Add(player);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Allowlist read exception: {ex.Message}");
            }

            var sortedPlayers = _cachedPlayers.OrderBy(p => p.Name).ToList();

            foreach (var player in sortedPlayers)
            {
                string displayLabel = player.IgnoresPlayerLimit ? $"{player.Name} [Staff Bypass]" : player.Name;
                materialCheckListBox1.Items.Add(displayLabel, false);
            }

            ClearSelectionFocus();
        }
        private void AddPlayerBTN_Click(object sender, EventArgs e)
        {
            string targetGamertag = VisualInputPrompt.ShowDialog("Whitelist Profile Wizard", "Enter exact Xbox Live Gamertag:");

            if (string.IsNullOrWhiteSpace(targetGamertag)) { ClearSelectionFocus(); return; }
            targetGamertag = targetGamertag.Trim();

            bool profileExists = _cachedPlayers.Any(p => p.Name.Equals(targetGamertag, StringComparison.OrdinalIgnoreCase));
            if (profileExists)
            {
                MaterialDialogBox.Show(this.FindForm(), "That player profile is already registered on the whitelist.", "Duplicate Profile Entry");
                ClearSelectionFocus();
                return;
            }

            DialogResult bypassPrompt = MaterialDialogBox.Show(
                this.FindForm(),
                $"Should '{targetGamertag}' bypass the maximum player connection limits?\n\n(Useful for administrative staff entry on full servers)",
                "Configure Staff Role Bounds",
                MessageBoxButtons.YesNo
            );
            bool requiresBypass = (bypassPrompt == DialogResult.Yes);

            var newPlayerRecord = new WhitelistPlayerEntry()
            {
                Name = targetGamertag,
                Xuid = null,
                IgnoresPlayerLimit = requiresBypass
            };

            _cachedPlayers.Add(newPlayerRecord);

            string visualStringText = requiresBypass ? $"{targetGamertag} [Staff Bypass]" : targetGamertag;
            materialCheckListBox1.Items.Add(visualStringText, false);

            SaveConfig();
            ClearSelectionFocus();
        }

        private void RemovePlayerBTN_Click(object sender, EventArgs e)
        {
            var entriesToRemove = new List<M3MaterialCheckBox>();
            var playersToPurge = new List<string>();

            foreach (object item in materialCheckListBox1.Items)
            {
                if (item is M3MaterialCheckBox checkbox && checkbox.Checked)
                {
                    entriesToRemove.Add(checkbox);
                    string cleanedName = checkbox.Text.Replace(" [Staff Bypass]", "").Trim();
                    playersToPurge.Add(cleanedName);
                }
            }

            if (entriesToRemove.Count == 0)
            {
                MaterialDialogBox.Show(this.FindForm(), "No player listings are currently checked for removal.", "Selection Trace Empty");
                ClearSelectionFocus();
                return;
            }

            _cachedPlayers.RemoveAll(p => playersToPurge.Contains(p.Name, StringComparer.OrdinalIgnoreCase));

            foreach (var referenceItem in entriesToRemove)
            {
                materialCheckListBox1.Items.Remove(referenceItem);
            }

            SaveConfig();
            ClearSelectionFocus();
        }

        public void SaveConfig()
        {
            if (string.IsNullOrEmpty(_configPath)) return;

            try
            {
                JsonHelper.WriteJsonFileAtomically(_configPath, _cachedPlayers, JsonHelper.IndentedOptions);
            }
            catch (Exception ex)
            {
                MaterialDialogBox.Show(
                    this.FindForm(),
                    $"An operating system file exception blocked the disk write operation:\n\n{ex.Message}",
                    "Disk Operation Exception",
                    MessageBoxButtons.OK
                );
            }
        }
    }

    public class WhitelistPlayerEntry
    {
        [JsonPropertyName("ignoresPlayerLimit")]
        public bool IgnoresPlayerLimit { get; set; } = false;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("xuid")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Xuid { get; set; } = null;

        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }
    }
}
