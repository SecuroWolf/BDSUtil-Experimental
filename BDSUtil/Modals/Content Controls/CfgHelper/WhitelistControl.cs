using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    public partial class WhitelistControl : UserControl
    {
        private string _configPath;

        // Track the data cache cleanly using a flat tracking model
        private List<WhitelistPlayerEntry> _cachedPlayers = new List<WhitelistPlayerEntry>();

        public WhitelistControl()
        {
            InitializeComponent();
        }

        public void InitializeControl(string serverRoot)
        {
            _configPath = Path.Combine(serverRoot, "allowlist.json");
            LoadConfig();
        }

        /// <summary>
        /// Reads allowlist tokens from disk using a lightweight manual parser to support older .NET Frameworks.
        /// </summary>
        private void LoadConfig()
        {
            _cachedPlayers.Clear();
            materialCheckListBox1.Items.Clear();

            if (!File.Exists(_configPath))
            {
                try { File.WriteAllText(_configPath, "[]", System.Text.Encoding.UTF8); } catch { }
                return;
            }

            try
            {
                string rawJson = File.ReadAllText(_configPath, System.Text.Encoding.UTF8).Trim();

                // Lightweight manual parser to extract records flawlessly without external library dependencies
                if (!string.IsNullOrEmpty(rawJson) && rawJson != "[]")
                {
                    string[] blocks = rawJson.Split(new string[] { "}," }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string block in blocks)
                    {
                        string name = ParseJsonValue(block, "name");
                        if (!string.IsNullOrEmpty(name))
                        {
                            string xuid = ParseJsonValue(block, "xuid");
                            string ignoreLimitStr = ParseJsonValue(block, "ignoresPlayerLimit");
                            bool ignoresLimit = ignoreLimitStr.Equals("true", StringComparison.OrdinalIgnoreCase);

                            _cachedPlayers.Add(new WhitelistPlayerEntry
                            {
                                Name = name,
                                Xuid = string.IsNullOrEmpty(xuid) ? null : xuid,
                                IgnoresPlayerLimit = ignoresLimit
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Allowlist read exception: {ex.Message}");
            }

            // Alpha-sort the listings to keep everything neat and structured
            var sortedPlayers = _cachedPlayers.OrderBy(p => p.Name).ToList();

            foreach (var player in sortedPlayers)
            {
                string displayLabel = player.IgnoresPlayerLimit ? $"{player.Name} [Staff Bypass]" : player.Name;
                materialCheckListBox1.Items.Add(displayLabel, false);
            }
        }

        /// <summary>
        /// Manual indentation builder that outputs perfectly formatted JSON array layouts to disk.
        /// </summary>
        /// <summary>
        /// Collects current active memory entries and commits them to disk, verifying path accuracy.
        /// </summary>
        public void SaveConfig()
        {
            if (string.IsNullOrEmpty(_configPath))
            {
                ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    "Allowlist save pipeline failed: Target file configuration path is uninitialized or null.",
                    "Save Execution Fault",
                    MessageBoxButtons.OK,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );
                return;
            }

            try
            {
                List<string> jsonLines = new List<string> { "[" };

                for (int i = 0; i < _cachedPlayers.Count; i++)
                {
                    var player = _cachedPlayers[i];
                    jsonLines.Add("  {");
                    jsonLines.Add($"    \"ignoresPlayerLimit\": {(player.IgnoresPlayerLimit ? "true" : "false")},");
                    jsonLines.Add($"    \"name\": \"{player.Name}\"{(player.Xuid != null ? "," : "")}");

                    if (player.Xuid != null)
                    {
                        jsonLines.Add($"    \"xuid\": \"{player.Xuid}\"");
                    }

                    jsonLines.Add(i == _cachedPlayers.Count - 1 ? "  }" : "  },");
                }

                jsonLines.Add("]");

                // Ensure the destination subdirectory container actually exists before invoking standard IO write pipelines
                string targetDir = Path.GetDirectoryName(_configPath);
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                // Force a synchronous overwrite pass straight to the absolute path target
                File.WriteAllLines(_configPath, jsonLines, System.Text.Encoding.UTF8);

                // --- PATH INTEGRITY VERIFICATION ---
                // If it claims it wrote successfully but you don't see it, this trace window reveals exactly where Windows dropped it
                System.Diagnostics.Debug.WriteLine($"[MANAGER SUCCESS] Allowlist changes written to target path: {_configPath}");
            }
            catch (Exception ex)
            {
                // Unmask underlying OS file system locks or security boundary errors immediately
                ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    $"An operating system file exception blocked the disk write operation:\n\n{ex.Message}",
                    "Disk Operation Exception",
                    MessageBoxButtons.OK,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );
            }
        }


        private void WhitelistControl_Load(object sender, EventArgs e)
        {
            materialCheckListBox1.Items.Clear();
        }

        /// <summary>
        /// Event hook for the ADD PLAYER button interaction.
        /// </summary>
        private void AddPlayerBTN_Click(object sender, EventArgs e)
        {
            string targetGamertag = VisualInputPrompt.ShowDialog("Whitelist Profile Wizard", "Enter exact Xbox Live Gamertag:");

            if (string.IsNullOrWhiteSpace(targetGamertag)) return;
            targetGamertag = targetGamertag.Trim();

            bool profileExists = _cachedPlayers.Any(p => p.Name.Equals(targetGamertag, StringComparison.OrdinalIgnoreCase));
            if (profileExists)
            {
                ReaLTaiizor.Controls.MaterialMessageBox.Show("That player profile is already registered on the whitelist.", "Duplicate Profile Entry");
                return;
            }

            // Fixed the ambiguity error by specifying the parameter positions explicitly
            DialogResult bypassPrompt = ReaLTaiizor.Controls.MaterialMessageBox.Show(
                $"Should '{targetGamertag}' bypass the maximum player connection limits?\n\n(Useful for administrative staff entry on full servers)",
                "Configure Staff Role Bounds",
                MessageBoxButtons.YesNo,
                ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
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
        }

        /// <summary>
        /// Event hook for the REMOVE SELECTED PLAYERS button execution.
        /// </summary>
        private void RemovePlayerBTN_Click(object sender, EventArgs e)
        {
            var entriesToRemove = new List<ReaLTaiizor.Controls.MaterialCheckBox>();
            var playersToPurge = new List<string>();

            // FIXED: MaterialCheckListBox holds a collection of native MaterialCheckBox controls inside its Items track
            foreach (object item in materialCheckListBox1.Items)
            {
                if (item is ReaLTaiizor.Controls.MaterialCheckBox checkbox && checkbox.Checked)
                {
                    entriesToRemove.Add(checkbox);
                    string cleanedName = checkbox.Text.Replace(" [Staff Bypass]", "").Trim();
                    playersToPurge.Add(cleanedName);
                }
            }

            if (entriesToRemove.Count == 0)
            {
                ReaLTaiizor.Controls.MaterialMessageBox.Show("No player listings are currently checked for removal.", "Selection Trace Empty");
                return;
            }

            _cachedPlayers.RemoveAll(p => playersToPurge.Contains(p.Name, StringComparer.OrdinalIgnoreCase));

            // FIXED: Removed the casting conversion error by ensuring types align perfectly during loop execution passes
            foreach (var referenceItem in entriesToRemove)
            {
                materialCheckListBox1.Items.Remove(referenceItem);
            }

            SaveConfig();
        }

        /// <summary>
        /// Internal string helper that cleans and strips away text fragments from json string objects.
        /// </summary>
        private string ParseJsonValue(string jsonBlock, string key)
        {
            string quoteKey = $"\"{key}\"";
            int keyIndex = jsonBlock.IndexOf(quoteKey, StringComparison.OrdinalIgnoreCase);
            if (keyIndex == -1) return string.Empty;

            int valueStartIndex = jsonBlock.IndexOf(':', keyIndex);
            if (valueStartIndex == -1) return string.Empty;
            valueStartIndex++;

            int commaIndex = jsonBlock.IndexOf(',', valueStartIndex);
            if (commaIndex == -1) commaIndex = jsonBlock.IndexOf('}', valueStartIndex);
            if (commaIndex == -1) commaIndex = jsonBlock.Length;

            string rawValue = jsonBlock.Substring(valueStartIndex, commaIndex - valueStartIndex).Trim();
            return rawValue.Replace("\"", "").Replace("}", "").Replace("]", "").Trim();
        }
    }

    // --- SYSTEM FLAT REGISTRATION ENTRY PROFILE MODEL ---

    public class WhitelistPlayerEntry
    {
        public string Name { get; set; } = string.Empty;
        public string Xuid { get; set; } = null;
        public bool IgnoresPlayerLimit { get; set; } = false;
    }
}
