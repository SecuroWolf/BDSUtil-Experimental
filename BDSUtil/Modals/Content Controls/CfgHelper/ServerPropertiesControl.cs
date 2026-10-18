using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    public partial class ServerPropertiesControl : UserControl
    {
        // Tracks the target path to the server.properties file location
        private string _configPath;

        // Stores key-value configuration pairs in a case-insensitive dictionary map
        private Dictionary<string, string> _propertiesCache;

        // Stores a raw historical line-by-line layout copy to preserve comments and formatting spacing on save
        private List<string> _fileLineLayout;

        public ServerPropertiesControl()
        {
            InitializeComponent();
            _propertiesCache = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            _fileLineLayout = new List<string>();

            // Intercept user typing loops to filter out invalid characters
            WireControlRestrictions();
        }

        /// <summary>
        /// Combines the verified base folder path with the configuration file name and begins parsing text lines.
        /// </summary>
        public void InitializeControl(string serverRoot)
        {
            _configPath = Path.Combine(serverRoot, "server.properties");
            MessageBox.Show("server config found at : " + _configPath, "Configuration Loaded", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadPropertiesConfig();
        }

        /// <summary>
        /// Binds custom KeyPress events to our text inputs to block non-numeric characters natively.
        /// </summary>
        private void WireControlRestrictions()
        {
            // Lock inputs to numerical whole values to satisfy port and player boundaries safely
            PropMP.KeyPress += NumericTextBox_KeyPress;
            PropPortV4.KeyPress += NumericTextBox_KeyPress;
            PropPortV6.KeyPress += NumericTextBox_KeyPress;
        }

        /// <summary>
        /// Standard event handler used to discard non-digit characters at the input gate.
        /// </summary>
        private void NumericTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow backspaces and numbers only
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }



        /// <summary>
        /// Reads configuration tokens directly from the file stream while skipping empty lines or global comments.
        /// </summary>
        private void LoadPropertiesConfig()
        {
            if (!File.Exists(_configPath)) return;

            _propertiesCache.Clear();
            _fileLineLayout.Clear();

            // Load the entire target property sheet safely utilizing UTF-8 character arrays
            string[] rawLines = File.ReadAllLines(_configPath, Encoding.UTF8);

            foreach (string currentLine in rawLines)
            {
                // Cache the line verbatim to protect structural design comments from deletions
                _fileLineLayout.Add(currentLine);

                string trimmed = currentLine.Trim();
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#")) continue;

                // Split each active row exactly on the first assignment operator sequence
                int delimiterIndex = trimmed.IndexOf('=');
                if (delimiterIndex <= 0) continue;

                string key = trimmed.Substring(0, delimiterIndex).Trim().ToLower();
                string value = trimmed.Substring(delimiterIndex + 1).Trim();

                if (!string.IsNullOrEmpty(key))
                {
                    _propertiesCache[key] = value;
                }
            }

            // FIRST: Scan the local file system to fill the world selection dropdown with actual directory nodes
            PopulateWorldsDropdown();

            // SECOND: Flush the freshly structured memory caches directly out into your visual component layers
            PopulateInterfaceElements();
        }

        /// <summary>
        /// Crawls the local server directory structure to discover valid world folders and binds them to the selection element.
        /// </summary>
        private void PopulateWorldsDropdown()
        {
            if (PropWorldSel == null) return;

            // Clear any old items to prevent messy duplications on reload triggers
            PropWorldSel.Items.Clear();

            // Resolve the absolute path target straight to the local "worlds" directory container
            string serverRootDir = Path.GetDirectoryName(_configPath);
            string worldsDirectoryPath = Path.Combine(serverRootDir, "worlds");

            // Look up and inject actual directory names if the worlds master folder exists on disk
            if (Directory.Exists(worldsDirectoryPath))
            {
                try
                {
                    // Collect all immediate subdirectories inside the worlds folder scope
                    string[] discoveredFolders = Directory.GetDirectories(worldsDirectoryPath);

                    foreach (string folderPath in discoveredFolders)
                    {
                        string folderName = Path.GetFileName(folderPath);
                        if (!string.IsNullOrWhiteSpace(folderName))
                        {
                            PropWorldSel.Items.Add(folderName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Fail gracefully if an OS security permission layer blocks file-system access loops
                    System.Diagnostics.Debug.WriteLine($"Failed crawling world directories: {ex.Message}");
                }
            }

            // Fallback safety layer: if no folders were found, ensure the current level-name is at least present
            string activeLevelName = GetCachedProperty("level-name", "Bedrock level");
            if (!PropWorldSel.Items.Contains(activeLevelName))
            {
                PropWorldSel.Items.Insert(0, activeLevelName);
            }

            // Append your custom "Add New" item macro option cleanly at the absolute end of the list choices
            PropWorldSel.Items.Add("Add New");
        }


        /// <summary>
        /// Pulls values safely out of our cache, defaulting to safe strings if a variable is missing.
        /// </summary>
        private string GetCachedProperty(string key, string defaultValue = "")
        {
            // Normalize the requested key string token to ensure safe matching
            string cleanKey = key.Trim().ToLower();
            return _propertiesCache.ContainsKey(cleanKey) ? _propertiesCache[cleanKey] : defaultValue;
        }

        /// <summary>
        /// Standardizes cached entries and binds them directly to their designated visual layout targets.
        /// </summary>
        private void PopulateInterfaceElements()
        {
            // --- TEXT CONFIGURATION INPUT FIELDS ---
            PropSvName.Text = GetCachedProperty("server-name", "Dedicated Server");
            PropSvSeed.Text = GetCachedProperty("level-seed", "");
            PropMP.Text = GetCachedProperty("max-players", "10");
            PropPortV4.Text = GetCachedProperty("server-port", "19132");
            PropPortV6.Text = GetCachedProperty("server-portv6", "19133");

            // --- STANDARD SELECTION DROPDOWNS ---
            PropGM.Text = CapitalizeFirstLetter(GetCachedProperty("gamemode", "survival"));
            PropDiff.Text = CapitalizeFirstLetter(GetCachedProperty("difficulty", "easy"));

            // --- WORLD DROPDOWN PICKER INITIAL SELECTION ---
            // Because the items are populated dynamically, setting .Text will match up smoothly with the active folder row
            PropWorldSel.Text = GetCachedProperty("level-name", "Bedrock level");

            // --- COMPLEX COMBOBOXES COMBINED WITH UNIT EXTENSIONS ---
            ParseChunkDistanceToUI(PropVD, GetCachedProperty("view-distance", "32"));
            ParseChunkDistanceToUI(PropSD, GetCachedProperty("tick-distance", "4"));
            ParseTimeoutToUI(PropAFK, GetCachedProperty("player-idle-timeout", "30"));

            // --- INTERFACE SLIDING SWITCH TOGGLES ---
            PropFGM.Checked = GetCachedProperty("force-gamemode", "false").Equals("true", StringComparison.OrdinalIgnoreCase);
            PropAlCh.Checked = GetCachedProperty("allow-cheats", "false").Equals("true", StringComparison.OrdinalIgnoreCase);
            PropOMode.Checked = GetCachedProperty("online-mode", "true").Equals("true", StringComparison.OrdinalIgnoreCase);
            PropWList.Checked = GetCachedProperty("allow-list", "true").Equals("true", StringComparison.OrdinalIgnoreCase);
            PropReqPacks.Checked = GetCachedProperty("texturepack-required", "false").Equals("true", StringComparison.OrdinalIgnoreCase);
            PropCastLAN.Checked = GetCachedProperty("enable-lan-visibility", "true").Equals("true", StringComparison.OrdinalIgnoreCase);
            PropCCG.Checked = GetCachedProperty("client-side-chunk-generation-enabled", "true").Equals("true", StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// Reads state parameters back out of active UI inputs and updates the dictionary mapping model.
        /// </summary>
        private void HarvestInterfaceElements()
        {
            // Harvest native textbox components
            _propertiesCache["server-name"] = PropSvName.Text.Trim();
            _propertiesCache["level-name"] = PropWorldSel.Text.Trim();
            _propertiesCache["level-seed"] = PropSvSeed.Text.Trim();
            _propertiesCache["max-players"] = PropMP.Text.Trim();
            _propertiesCache["server-port"] = PropPortV4.Text.Trim();
            _propertiesCache["server-portv6"] = PropPortV6.Text.Trim();

            // Downcase options to fit standard bedrock server configuration properties formatting
            _propertiesCache["gamemode"] = PropGM.Text.Trim().ToLower();
            _propertiesCache["difficulty"] = PropDiff.Text.Trim().ToLower();

            // Strip descriptive layout strings (" Chunks", " Mins") out to store raw integer data rules
            _propertiesCache["view-distance"] = ExtractNumericValue(PropVD.Text);
            _propertiesCache["tick-distance"] = ExtractNumericValue(PropSD.Text);
            _propertiesCache["player-idle-timeout"] = ExtractTimeoutValue(PropAFK.Text);

            // Translate interface bool ticks directly back to true/false string lines
            _propertiesCache["force-gamemode"] = PropFGM.Checked ? "true" : "false";
            _propertiesCache["allow-cheats"] = PropAlCh.Checked ? "true" : "false";
            _propertiesCache["online-mode"] = PropOMode.Checked ? "true" : "false";
            _propertiesCache["allow-list"] = PropWList.Checked ? "true" : "false";
            _propertiesCache["texturepack-required"] = PropReqPacks.Checked ? "true" : "false";
            _propertiesCache["enable-lan-visibility"] = PropCastLAN.Checked ? "true" : "false";
            _propertiesCache["client-side-chunk-generation-enabled"] = PropCCG.Checked ? "true" : "false";
        }
        /// <summary>
        /// Synchronizes changes back to disk sequentially while accurately preserving original layout formatting templates.
        /// </summary>
        public void SaveConfig()
        {
            if (string.IsNullOrEmpty(_configPath)) return;

            // Harvest the active control adjustments right before writing data out to the text track
            HarvestInterfaceElements();

            List<string> finalizedOutputLines = new List<string>();
            HashSet<string> writtenKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Reconstruct the file systematically using original line allocations to maintain layout structures
            foreach (string originalLine in _fileLineLayout)
            {
                string trimmed = originalLine.Trim();

                // Keep historical blanks and header remarks completely un-touched
                if (string.IsNullOrEmpty(trimmed) || trimmed.StartsWith("#"))
                {
                    finalizedOutputLines.Add(originalLine);
                    continue;
                }

                int delimiterIndex = trimmed.IndexOf('=');
                if (delimiterIndex <= 0)
                {
                    finalizedOutputLines.Add(originalLine);
                    continue;
                }

                string key = trimmed.Substring(0, delimiterIndex).Trim();

                if (!string.IsNullOrEmpty(key) && _propertiesCache.ContainsKey(key))
                {
                    // Emit our freshly modified field variable data right next to its original key tracking assignment
                    finalizedOutputLines.Add($"{key}={_propertiesCache[key]}");
                    writtenKeys.Add(key);
                }
                else
                {
                    finalizedOutputLines.Add(originalLine);
                }
            }

            // Append any dynamically added or structural parameters that were missing in the original file skeleton
            foreach (KeyValuePair<string, string> propertyPair in _propertiesCache)
            {
                if (!writtenKeys.Contains(propertyPair.Key))
                {
                    finalizedOutputLines.Add($"{propertyPair.Key}={propertyPair.Value}");
                }
            }

            // Overwrite and execute a clean disk commit stream sequence at position byte zero
            File.WriteAllLines(_configPath, finalizedOutputLines, Encoding.UTF8);
        }

        // --- CUSTOM STRING FORMATTING HELPER SYSTEMS ---

        // --- CUSTOM STRING FORMATTING HELPER SYSTEMS ---

        /// <summary>
        /// Converts the first character of a string to uppercase to align text inputs smoothly with selection collections.
        /// </summary>
        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }

        /// <summary>
        /// Appends units or dynamically handles unmatched custom file values for Distance dropdown selectors.
        /// </summary>
        private void ParseChunkDistanceToUI(ComboBox box, string rawValue)
        {
            if (string.IsNullOrWhiteSpace(rawValue)) return;

            // Strip out extra string labels to process the bare configuration integer token cleanly
            string cleanValue = rawValue.Replace("Chunks", "").Replace("(Custom)", "").Trim();
            string standardOption = $"{cleanValue} Chunks";

            // Loop over existing item choices to see if this matches one of your standard predefined dropdown rows
            bool matchFound = false;
            foreach (var item in box.Items)
            {
                if (item.ToString().Equals(standardOption, StringComparison.OrdinalIgnoreCase))
                {
                    box.Text = standardOption;
                    matchFound = true;
                    break;
                }
            }

            // FALLBACK PATHWAY: If the config file holds an unmapped integer, inject a temporary option track so it loads safely
            if (!matchFound)
            {
                string customOptionString = $"{cleanValue} Chunks (Custom)";

                // Add the unmapped setting option dynamically to the dropdown items array list tracking sequence
                if (!box.Items.Contains(customOptionString))
                {
                    box.Items.Insert(box.Items.Count - 1, customOptionString);
                }

                box.Text = customOptionString;
            }
        }

        /// <summary>
        /// Decodes numeric configurations into clean, human-readable timing units inside the dropdown list.
        /// </summary>
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
                if (item.ToString().Equals(standardOption, StringComparison.OrdinalIgnoreCase))
                {
                    box.Text = standardOption;
                    matchFound = true;
                    break;
                }
            }

            // Custom non-standard tracking fallback logic injection point for user timeouts
            if (!matchFound)
            {
                string customTimeoutString = $"{cleanValue} Mins (Custom)";
                if (!box.Items.Contains(customTimeoutString))
                {
                    box.Items.Insert(box.Items.Count - 1, customTimeoutString);
                }
                box.Text = customTimeoutString;
            }
        }

        /// <summary>
        /// Strips formatting strings and custom tags away to return a raw numeric value string for file emission.
        /// </summary>
        private string ExtractNumericValue(string uiText)
        {
            if (string.IsNullOrEmpty(uiText)) return "4";

            // Systematically wipe text extensions to leave behind the raw configuration digits
            string clean = uiText.Replace("Chunks", "").Replace("(Custom)", "").Trim();
            return string.IsNullOrEmpty(clean) ? "4" : clean;
        }

        /// <summary>
        /// Translates visual time frames back to exact bedrock config integers.
        /// </summary>
        private string ExtractTimeoutValue(string uiText)
        {
            if (string.IsNullOrEmpty(uiText)) return "30";
            string standardized = uiText.Trim();

            if (standardized.Equals("Off", StringComparison.OrdinalIgnoreCase)) return "0";
            if (standardized.Equals("1 Hour", StringComparison.OrdinalIgnoreCase)) return "60";

            string clean = standardized.Replace("Mins", "").Replace("(Custom)", "").Trim();
            return string.IsNullOrEmpty(clean) ? "30" : clean;
        }

        /// <summary>
        /// Intercepts world choices to clear or repopulate configuration seeds, dropping local breadcrumb files retroactively.
        /// </summary>
        private void PropWorldSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PropWorldSel.SelectedItem == null) return;

            string selectedOption = PropWorldSel.SelectedItem.ToString();

            if (selectedOption == "Add New")
            {
                // Pull a fresh world subdirectory name straight from an overlay input box prompt
                string newWorldName = VisualInputPrompt.ShowDialog("Create New World Target", "Enter unique world directory filename:");

                if (!string.IsNullOrWhiteSpace(newWorldName))
                {
                    // Clean off illegal file characters to safely allow storage creation blocks
                    foreach (char illegalChar in Path.GetInvalidFileNameChars())
                    {
                        newWorldName = newWorldName.Replace(illegalChar.ToString(), "");
                    }
                    newWorldName = newWorldName.Trim();
                }

                if (!string.IsNullOrWhiteSpace(newWorldName))
                {
                    // Insert the user-specified world name cleanly right above our static 'Add New' anchor index tracking spot
                    int targetInsertIndex = PropWorldSel.Items.Count - 1;
                    PropWorldSel.Items.Insert(targetInsertIndex, newWorldName);
                    PropWorldSel.SelectedIndex = targetInsertIndex;

                    // Clear any lingering text from previous selections and unlock the control for typing
                    PropSvSeed.Text = string.Empty;
                    PropSvSeed.ReadOnly = false;
                    PropSvSeed.Focus();
                }
                else
                {
                    // Revert selection tracking instantly if the prompt tracking input matrix is discarded or empty
                    PropWorldSel.SelectedIndex = 0;
                }
            }
            else
            {
                // LOCKDOWN SAFETY LAYER: Secure the field against arbitrary typing edits for existing configurations
                PropSvSeed.ReadOnly = true;

                // Resolve absolute pathing variables to target the world's internal folder structure
                string serverRootDir = Path.GetDirectoryName(_configPath);
                string worldFolderDir = Path.Combine(serverRootDir, "worlds", selectedOption);
                string targetBreadcrumbFile = Path.Combine(worldFolderDir, ".bdsseed");

                // Pull current active server properties configuration data values from memory cache
                string configLevelName = GetCachedProperty("level-name", "Bedrock level");
                string configSeedValue = GetCachedProperty("level-seed", "");

                // RULE 1: If a local tracking breadcrumb already exists, read and display it immediately
                if (Directory.Exists(worldFolderDir) && File.Exists(targetBreadcrumbFile))
                {
                    try
                    {
                        PropSvSeed.Text = File.ReadAllText(targetBreadcrumbFile, Encoding.UTF8).Trim();
                    }
                    catch
                    {
                        PropSvSeed.Text = string.Empty;
                    }
                }
                // RULE 2: If no breadcrumb exists, but this world is the active server config target and has a populated seed
                else if (selectedOption.Equals(configLevelName, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(configSeedValue))
                {
                    PropSvSeed.Text = configSeedValue;

                    // Automatically generate the missing breadcrumb retroactively so it is preserved locally
                    try
                    {
                        if (Directory.Exists(worldFolderDir))
                        {
                            File.WriteAllText(targetBreadcrumbFile, configSeedValue, Encoding.UTF8);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Retroactive breadcrumb generation bypassed: {ex.Message}");
                    }
                }
                // RULE 3: Race Condition Fallback — If no breadcrumb exists and it doesn't match the active properties configuration, clear the field entirely
                else
                {
                    PropSvSeed.Text = string.Empty;
                }
            }
        }



        /// <summary>
        /// Catches View Distance indexing adjustments to enable arbitrary user custom number string insertions.
        /// </summary>
        private void PropVD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PropVD.SelectedItem != null && PropVD.SelectedItem.ToString() == "Custom")
            {
                string customDistanceInput = VisualInputPrompt.ShowDialog("Custom Chunk Rendering Setup", "Enter custom view distance value (Chunks):");

                // Parse strings to ensure the numerical entry inputs satisfy healthy integer bounds
                if (int.TryParse(customDistanceInput, out int cleanChunkCount) && cleanChunkCount > 0)
                {
                    string dynamicCustomOptionText = $"{cleanChunkCount} Chunks (Custom)";

                    // Inject the custom choice right above the 'Custom' macro toggle tracking slot array
                    int insertPoint = PropVD.Items.Count - 1;

                    if (!PropVD.Items.Contains(dynamicCustomOptionText))
                    {
                        PropVD.Items.Insert(insertPoint, dynamicCustomOptionText);
                    }

                    PropVD.Text = dynamicCustomOptionText;
                }
                else
                {
                    // Bounce field states fallback standard defaults securely on illegal non-digit entry drops
                    PropVD.Text = "32 Chunks";
                }
            }
        }



        private void ServerPropertiesControl_Load(object sender, EventArgs e)
        {
            // Arm the numeric typing filters immediately when the visual canvas initializes
            WireControlRestrictions();
        }


    }
}
/// <summary>
/// A clean, minimalist prompt helper window to securely capture custom user string string variables.
/// </summary>
public static class VisualInputPrompt
{
    public static string ShowDialog(string titleText, string promptLabelText)
    {
        Form promptWindow = new Form()
        {
            Width = 400,
            Height = 160,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = titleText,
            StartPosition = FormStartPosition.CenterParent,
            MaximizeBox = false,
            MinimizeBox = false
        };
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
