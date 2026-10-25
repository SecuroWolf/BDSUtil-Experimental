using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    public partial class ServerPropertiesControl : UserControl
    {
        private bool _isInitializing = true;
        private readonly Label _focusTrapLabel;
        private string _configPath;
        private readonly System.Collections.Generic.Dictionary<string, string> _propertiesCache;
        private readonly System.Collections.Generic.List<string> _fileLineLayout;

        public ServerPropertiesControl()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            InitializeComponent();
            this.AutoSize = false;

            _propertiesCache = new System.Collections.Generic.Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _fileLineLayout = new System.Collections.Generic.List<string>();

            _focusTrapLabel = new Label { Size = new System.Drawing.Size(0, 0), Location = new System.Drawing.Point(0, 0), TabStop = true };
            this.Controls.Add(_focusTrapLabel);

            WireControlRestrictions();
            WireRealTimeSavingEvents();
        }

        public void InitializeControl(string serverRoot)
        {
            _configPath = Path.Combine(serverRoot, "server.properties");
            LoadPropertiesConfig();
        }

        public void ClearSelectionFocus()
        {
            if (_focusTrapLabel != null && _focusTrapLabel.CanFocus) _focusTrapLabel.Focus();
        }

        private void WireControlRestrictions()
        {
            PropSvName.KeyPress += PropSvName_KeyPress;
            PropMP.KeyPress += NumericTextBox_KeyPress;
            PropPortV4.KeyPress += NumericTextBox_KeyPress;
            PropPortV6.KeyPress += NumericTextBox_KeyPress;
        }

        private void WireRealTimeSavingEvents()
        {
            PropSvName.TextChanged += AutoSave_Event;
            PropSvSeed.TextChanged += AutoSave_Event;
            PropMP.TextChanged += AutoSave_Event;
            PropPortV4.TextChanged += AutoSave_Event;
            PropPortV6.TextChanged += AutoSave_Event;

            PropGM.SelectedIndexChanged += AutoSave_Event;
            PropDiff.SelectedIndexChanged += AutoSave_Event;
            PropSD.SelectedIndexChanged += AutoSave_Event;
            PropAFK.SelectedIndexChanged += AutoSave_Event;

            PropFGM.CheckedChanged += AutoSave_Event;
            PropAlCh.CheckedChanged += AutoSave_Event;
            PropOMode.CheckedChanged += AutoSave_Event;
            PropWList.CheckedChanged += AutoSave_Event;
            PropReqPacks.CheckedChanged += AutoSave_Event;
            PropCastLAN.CheckedChanged += AutoSave_Event;
            PropCCG.CheckedChanged += AutoSave_Event;
        }

        private void AutoSave_Event(object sender, EventArgs e)
        {
            if (_isInitializing) return;
            SaveConfig();
        }

        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }
        private void PropSvName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                ClearSelectionFocus();
            }
        }

        private void LoadPropertiesConfig()
        {
            if (!File.Exists(_configPath)) return;
            _isInitializing = true;

            _propertiesCache.Clear();
            _fileLineLayout.Clear();
            string[] rawLines = File.ReadAllLines(_configPath, Encoding.UTF8);

            foreach (string currentLine in rawLines)
            {
                _fileLineLayout.Add(currentLine);
                string trimmed = currentLine.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#")) continue;

                int delimiterIndex = trimmed.IndexOf('=');
                if (delimiterIndex <= 0) continue;

                string key = trimmed.Substring(0, delimiterIndex).Trim().ToLower();
                string value = trimmed.Substring(delimiterIndex + 1).Trim();
                if (!string.IsNullOrEmpty(key)) _propertiesCache[key] = value;
            }

            PopulateWorldsDropdown();
            PopulateInterfaceElements();
            _isInitializing = false;
            ClearSelectionFocus();
        }

        public void SaveConfig()
        {
            if (string.IsNullOrEmpty(_configPath)) return;
            HarvestInterfaceElements();

            System.Collections.Generic.List<string> finalizedOutputLines = new System.Collections.Generic.List<string>();
            System.Collections.Generic.HashSet<string> writtenKeys = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string originalLine in _fileLineLayout)
            {
                string trimmed = originalLine.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#")) { finalizedOutputLines.Add(originalLine); continue; }

                int delimiterIndex = trimmed.IndexOf('=');
                if (delimiterIndex <= 0) { finalizedOutputLines.Add(originalLine); continue; }

                string key = trimmed.Substring(0, delimiterIndex).Trim();
                if (!string.IsNullOrEmpty(key) && _propertiesCache.ContainsKey(key))
                {
                    finalizedOutputLines.Add($"{key}={_propertiesCache[key]}");
                    writtenKeys.Add(key);
                }
                else finalizedOutputLines.Add(originalLine);
            }

            foreach (System.Collections.Generic.KeyValuePair<string, string> propertyPair in _propertiesCache)
            {
                if (!writtenKeys.Contains(propertyPair.Key)) finalizedOutputLines.Add($"{propertyPair.Key}={propertyPair.Value}");
            }

            try { File.WriteAllLines(_configPath, finalizedOutputLines, Encoding.UTF8); }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Auto-save failed: {ex.Message}"); }
        }
        private void PopulateWorldsDropdown()
        {
            if (PropWorldSel == null) return;
            PropWorldSel.Items.Clear();

            string serverRootDir = Path.GetDirectoryName(_configPath);
            string worldsDirectoryPath = Path.Combine(serverRootDir, "worlds");

            if (Directory.Exists(worldsDirectoryPath))
            {
                try
                {
                    string[] discoveredFolders = Directory.GetDirectories(worldsDirectoryPath);
                    foreach (string folderPath in discoveredFolders)
                    {
                        string folderName = Path.GetFileName(folderPath);
                        if (!string.IsNullOrWhiteSpace(folderName)) PropWorldSel.Items.Add(folderName);
                    }
                }
                catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Failed crawling worlds: {ex.Message}"); }
            }

            string activeLevelName = GetCachedProperty("level-name", "Bedrock level");
            if (!PropWorldSel.Items.Contains(activeLevelName)) PropWorldSel.Items.Insert(0, activeLevelName);
            PropWorldSel.Items.Add("Add New");
        }

        private string GetCachedProperty(string key, string defaultValue = "")
        {
            string cleanKey = key.Trim().ToLower();
            return _propertiesCache.ContainsKey(cleanKey) ? _propertiesCache[cleanKey] : defaultValue;
        }

        private void PopulateInterfaceElements()
        {
            PropSvName.Text = GetCachedProperty("server-name", "Dedicated Server");
            PropSvSeed.Text = GetCachedProperty("level-seed", "");
            PropMP.Text = GetCachedProperty("max-players", "10");
            PropPortV4.Text = GetCachedProperty("server-port", "19132");
            PropPortV6.Text = GetCachedProperty("server-portv6", "19133");

            PropGM.Text = CapitalizeFirstLetter(GetCachedProperty("gamemode", "survival"));
            PropDiff.Text = CapitalizeFirstLetter(GetCachedProperty("difficulty", "easy"));
            PropWorldSel.Text = GetCachedProperty("level-name", "Bedrock level");

            ParseChunkDistanceToUI(PropVD, GetCachedProperty("view-distance", "32"));
            ParseChunkDistanceToUI(PropSD, GetCachedProperty("tick-distance", "4"));
            ParseTimeoutToUI(PropAFK, GetCachedProperty("player-idle-timeout", "30"));

            PropFGM.Checked = GetCachedProperty("force-gamemode", "false").Equals("true", StringComparison.OrdinalIgnoreCase);
            PropAlCh.Checked = GetCachedProperty("allow-cheats", "false").Equals("true", StringComparison.OrdinalIgnoreCase);
            PropOMode.Checked = GetCachedProperty("online-mode", "true").Equals("true", StringComparison.OrdinalIgnoreCase);
            PropWList.Checked = GetCachedProperty("allow-list", "true").Equals("true", StringComparison.OrdinalIgnoreCase);
            PropReqPacks.Checked = GetCachedProperty("texturepack-required", "false").Equals("true", StringComparison.OrdinalIgnoreCase);
            PropCastLAN.Checked = GetCachedProperty("enable-lan-visibility", "true").Equals("true", StringComparison.OrdinalIgnoreCase);
            PropCCG.Checked = GetCachedProperty("client-side-chunk-generation-enabled", "true").Equals("true", StringComparison.OrdinalIgnoreCase);
        }
        private void HarvestInterfaceElements()
        {
            _propertiesCache["server-name"] = PropSvName.Text.Trim();
            _propertiesCache["level-name"] = PropWorldSel.Text.Trim();
            _propertiesCache["level-seed"] = PropSvSeed.Text.Trim();
            _propertiesCache["max-players"] = PropMP.Text.Trim();
            _propertiesCache["server-port"] = PropPortV4.Text.Trim();
            _propertiesCache["server-portv6"] = PropPortV6.Text.Trim();

            _propertiesCache["gamemode"] = PropGM.Text.Trim().ToLower();
            _propertiesCache["difficulty"] = PropDiff.Text.Trim().ToLower();

            _propertiesCache["view-distance"] = ExtractNumericValue(PropVD.Text);
            _propertiesCache["tick-distance"] = ExtractNumericValue(PropSD.Text);
            _propertiesCache["player-idle-timeout"] = ExtractTimeoutValue(PropAFK.Text);

            _propertiesCache["force-gamemode"] = PropFGM.Checked ? "true" : "false";
            _propertiesCache["allow-cheats"] = PropAlCh.Checked ? "true" : "false";
            _propertiesCache["online-mode"] = PropOMode.Checked ? "true" : "false";
            _propertiesCache["allow-list"] = PropWList.Checked ? "true" : "false";
            _propertiesCache["texturepack-required"] = PropReqPacks.Checked ? "true" : "false";
            _propertiesCache["enable-lan-visibility"] = PropCastLAN.Checked ? "true" : "false";
            _propertiesCache["client-side-chunk-generation-enabled"] = PropCCG.Checked ? "true" : "false";
        }

        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        private void ParseChunkDistanceToUI(ComboBox box, string rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue)) return;
            string cleanValue = rawValue.Replace("Chunks", "").Replace("(Custom)", "").Trim();
            string standardOption = $"{cleanValue} Chunks";

            bool matchFound = false;
            foreach (var item in box.Items)
            {
                if (item.ToString().Equals(standardOption, StringComparison.OrdinalIgnoreCase)) { box.Text = standardOption; matchFound = true; break; }
            }

            if (!matchFound)
            {
                string customOptionString = $"{cleanValue} Chunks (Custom)";
                if (!box.Items.Contains(customOptionString)) box.Items.Insert(box.Items.Count - 1, customOptionString);
                box.Text = customOptionString;
            }
        }

        private void ParseTimeoutToUI(ComboBox box, string rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue)) return;
            string cleanValue = rawValue.Replace("Mins", "").Replace("(Custom)", "").Trim();

            if (cleanValue == "0") { box.Text = "Off"; return; }
            if (cleanValue == "60") { box.Text = "1 Hour"; return; }

            string standardOption = $"{cleanValue} Mins";
            bool matchFound = false;
            foreach (var item in box.Items)
            {
                if (item.ToString().Equals(standardOption, StringComparison.OrdinalIgnoreCase)) { box.Text = standardOption; matchFound = true; break; }
            }

            if (!matchFound)
            {
                string customTimeoutString = $"{cleanValue} Mins (Custom)";
                if (!box.Items.Contains(customTimeoutString)) box.Items.Insert(box.Items.Count - 1, customTimeoutString);
                box.Text = customTimeoutString;
            }
        }
        private string ExtractNumericValue(string uiText)
        {
            if (string.IsNullOrEmpty(uiText)) return "4";
            string clean = uiText.Replace("Chunks", "").Replace("(Custom)", "").Trim();
            return string.IsNullOrEmpty(clean) ? "4" : clean;
        }

        private string ExtractTimeoutValue(string uiText)
        {
            if (string.IsNullOrEmpty(uiText)) return "30";
            string standardized = uiText.Trim();
            if (standardized.Equals("Off", StringComparison.OrdinalIgnoreCase)) return "0";
            if (standardized.Equals("1 Hour", StringComparison.OrdinalIgnoreCase)) return "60";

            string clean = standardized.Replace("Mins", "").Replace("(Custom)", "").Trim();
            return string.IsNullOrEmpty(clean) ? "30" : clean;
        }

        private void PropWorldSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PropWorldSel.SelectedItem == null) return;
            string selectedOption = PropWorldSel.SelectedItem.ToString();

            if (selectedOption == "Add New")
            {
                string newWorldName = VisualInputPrompt.ShowDialog("Create New World Target", "Enter unique world directory filename:");
                if (!string.IsNullOrWhiteSpace(newWorldName))
                {
                    foreach (char illegalChar in Path.GetInvalidFileNameChars()) newWorldName = newWorldName.Replace(illegalChar.ToString(), "");
                    newWorldName = newWorldName.Trim();
                }

                if (!string.IsNullOrWhiteSpace(newWorldName))
                {
                    int targetInsertIndex = PropWorldSel.Items.Count - 1;
                    PropWorldSel.Items.Insert(targetInsertIndex, newWorldName);
                    _isInitializing = true; PropWorldSel.SelectedIndex = targetInsertIndex; _isInitializing = false;
                    PropSvSeed.Text = string.Empty; PropSvSeed.ReadOnly = false; PropSvSeed.Focus();
                }
                else { _isInitializing = true; PropWorldSel.SelectedIndex = 0; _isInitializing = false; }
            }
            else
            {
                PropSvSeed.ReadOnly = true;
                string serverRootDir = Path.GetDirectoryName(_configPath);
                string worldFolderDir = Path.Combine(serverRootDir, "worlds", selectedOption);
                string targetBreadcrumbFile = Path.Combine(worldFolderDir, ".bdsseed");
                string configLevelName = GetCachedProperty("level-name", "Bedrock level");
                string configSeedValue = GetCachedProperty("level-seed", "");

                if (Directory.Exists(worldFolderDir) && File.Exists(targetBreadcrumbFile))
                {
                    try { PropSvSeed.Text = File.ReadAllText(targetBreadcrumbFile, Encoding.UTF8).Trim(); }
                    catch { PropSvSeed.Text = string.Empty; }
                }
                else if (selectedOption.Equals(configLevelName, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(configSeedValue))
                {
                    PropSvSeed.Text = configSeedValue;
                    try { if (Directory.Exists(worldFolderDir)) File.WriteAllText(targetBreadcrumbFile, configSeedValue, Encoding.UTF8); }
                    catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Breadcrumb bypass: {ex.Message}"); }
                }
                else PropSvSeed.Text = string.Empty;
            }

            if (!_isInitializing) { SaveConfig(); ClearSelectionFocus(); }
        }

        private void PropVD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PropVD.SelectedItem != null && PropVD.SelectedItem.ToString() == "Custom")
            {
                string customDistanceInput = VisualInputPrompt.ShowDialog("Custom Chunk Rendering Setup", "Enter custom view distance value (Chunks):");
                if (int.TryParse(customDistanceInput, out int cleanChunkCount) && cleanChunkCount > 0)
                {
                    string dynamicCustomOptionText = $"{cleanChunkCount} Chunks (Custom)";
                    int insertPoint = PropVD.Items.Count - 1;
                    if (!PropVD.Items.Contains(dynamicCustomOptionText))
                    {
                        _isInitializing = true; PropVD.Items.Insert(insertPoint, dynamicCustomOptionText); _isInitializing = false;
                    }
                    PropVD.Text = dynamicCustomOptionText;
                }
                else PropVD.Text = "32 Chunks";
            }

            if (!_isInitializing) { SaveConfig(); ClearSelectionFocus(); }
        }

        private void ServerPropertiesControl_Load(object sender, EventArgs e)
        {
            WireControlRestrictions();
            ClearSelectionFocus();
        }
    }

    public static class VisualInputPrompt
    {
        public static string ShowDialog(string titleText, string promptLabelText)
        {
            Form promptWindow = new Form() { Width = 400, Height = 160, FormBorderStyle = FormBorderStyle.FixedDialog, Text = titleText, StartPosition = FormStartPosition.CenterParent, MaximizeBox = false, MinimizeBox = false };
            Label textLabel = new Label() { Left = 20, Top = 15, Text = promptLabelText, Width = 360 };
            TextBox inputTextBox = new TextBox() { Left = 20, Top = 40, Width = 340 };
            Button confirmationButton = new Button() { Text = "Confirm", Left = 260, Width = 100, Top = 80, DialogResult = DialogResult.OK };

            confirmationButton.Click += (sender, e) => { promptWindow.Close(); };
            promptWindow.Controls.Add(inputTextBox);
            promptWindow.Controls.Add(textLabel);
            promptWindow.Controls.Add(confirmationButton);
            promptWindow.AcceptButton = confirmationButton;

            return promptWindow.ShowDialog() == DialogResult.OK ? inputTextBox.Text.Trim() : string.Empty;
        }
    }
}
