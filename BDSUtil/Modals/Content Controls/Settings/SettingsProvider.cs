using BDSUtil.Helpers;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace BDSUtil.Modals.Content_Controls.Settings
{
    public partial class SettingsProvider : UserControl
    {
        private bool _isInitializing = true;
        private Label _focusTrapLabel; // ✅ Holds focus invisibly to prevent input highlighting

        public SettingsProvider()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
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

            // Explicitly bind layout event triggers to avoid visual designer dropped hooks
            if (BrowseBTN != null)
                BrowseBTN.Click += BrowseBTN_Click;

            if (CompressionTypeComboBox != null)
                CompressionTypeComboBox.SelectedIndexChanged += CompressionTypeComboBox_SelectedIndexChanged;

            if (CfgWorldMod != null)
                CfgWorldMod.CheckedChanged += CfgWorldMod_CheckedChanged;

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

            // 1. Sync the bedrock executable root directory location string
            if (ServerPathTextBox != null)
            {
                ServerPathTextBox.Text = AppSettings.Instance.ServerExecutablePath;
            }

            // 2. Sync active archive sorting format selection dropdown indices securely
            if (CompressionTypeComboBox != null)
            {
                int savedMode = AppSettings.Instance.CompressionMode;
                if (savedMode >= 0 && savedMode < CompressionTypeComboBox.Items.Count)
                {
                    CompressionTypeComboBox.SelectedIndex = savedMode;
                }
            }

            // 3. Sync master file system consent authorization status switches
            if (CfgWorldMod != null)
            {
                CfgWorldMod.Checked = AppSettings.Instance.WorldEditorEnabled;
            }

            if (CfgHTel != null) CfgHTel.Checked = AppSettings.Instance.HideTelemetryWarnings;

            _isInitializing = false;

            // ✅ START WITH NOTHING SELECTED
            ClearSelectionFocus();
        }

        /// <summary>
        /// Telemetry Hider toggle
        /// </summary>
        private void CfgHTel_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing || CfgHTel == null) return;

            AppSettings.Instance.HideTelemetryWarnings = CfgHTel.Checked;
            AppSettings.Instance.SaveSettings();
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
                if (this.FindForm() is MaterialForm parentMaterialForm)
                {
                    DialogResult riskConsentResult = MaterialMessageBox.Show(
                        parentMaterialForm,
                        "CRITICAL DATA SAFETY WARNING:\n\nUnlocking the World Editor tools grants permission to rewrite level parameters.\n\n" +
                        "Improper values or directory write errors can corrupt active worlds or cause database crashes. Always maintain independent save folder copies.\n\n" +
                        "Do you completely acknowledge these operational risks and authorize file manipulations?\n",
                        "Confirm Data Modification Rights",
                        MessageBoxButtons.YesNo,
                        false,
                        MaterialFlexibleForm.ButtonsPosition.Center
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
            AppSettings.Instance.WorldEditorEnabled = isEnabled;
            AppSettings.Instance.SaveSettings();

            // ✅ GO BACK TO NOTHING SELECTED (On Modification Complete)
            ClearSelectionFocus();
        }

        /// <summary>
        /// Saves backup compression format modifications to config.json instantly when a dropdown option is clicked.
        /// </summary>
        private void CompressionTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isInitializing || CompressionTypeComboBox == null) return;

            AppSettings.Instance.CompressionMode = CompressionTypeComboBox.SelectedIndex;
            AppSettings.Instance.SaveSettings();

            // ✅ GO BACK TO NOTHING SELECTED (After Dropdown Closes)
            ClearSelectionFocus();
        }

        /// <summary>
        /// Spawns a platform explorer folder selection window to locate and store the bedrock_server root installation path.
        /// </summary>
        private void BrowseBTN_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select the base installation directory hosting your Bedrock Dedicated Server files (bedrock_server.exe).";
                folderDialog.ShowNewFolderButton = false;

                // Pre-seed search coordinates to current configuration if it evaluates as structurally valid
                if (!string.IsNullOrWhiteSpace(AppSettings.Instance.ServerExecutablePath) && Directory.Exists(AppSettings.Instance.ServerExecutablePath))
                {
                    folderDialog.SelectedPath = AppSettings.Instance.ServerExecutablePath;
                }

                if (folderDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string selectedDirectory = folderDialog.SelectedPath;

                    if (ServerPathTextBox != null)
                    {
                        ServerPathTextBox.Text = selectedDirectory;
                    }

                    AppSettings.Instance.ServerExecutablePath = selectedDirectory;
                    AppSettings.Instance.SaveSettings();
                }
            }

            // ✅ GO BACK TO NOTHING SELECTED (After Folder Picker Shuts Down)
            ClearSelectionFocus();
        }
    }
}
