using BDSUtil.Helpers;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace BDSUtil.Modals.Content_Controls.Level_Editor
{
    public partial class EditorProvider : UserControl
    {
        private bool _isInitializing = true;
        private Label _focusTrapLabel; // ✅ Holds focus invisibly to prevent input highlighting

        public EditorProvider()
        {
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            InitializeComponent();

            _focusTrapLabel = new Label { Size = new System.Drawing.Size(0, 0), Location = new System.Drawing.Point(0, 0), TabStop = true };
            this.Controls.Add(_focusTrapLabel);

            if (LEBetaAPI != null) LEBetaAPI.CheckedChanged += ExperimentSwitch_CheckedChanged;
            if (LEUCF != null) LEUCF.CheckedChanged += ExperimentSwitch_CheckedChanged;
            if (LEDTP != null) LEDTP.CheckedChanged += ExperimentSwitch_CheckedChanged;

            this.Load += (s, e) => { InitializeControl(); };
        }

        public void ClearSelectionFocus()
        {
            if (_focusTrapLabel != null && _focusTrapLabel.CanFocus) _focusTrapLabel.Focus();
        }

        public void InitializeControl()
        {
            _isInitializing = true;

            bool isAllowed = AppSettings.Instance.WorldEditorEnabled;
            this.Enabled = isAllowed;

            if (isAllowed)
            {
                RefreshWorldValues();
            }

            _isInitializing = false;
            ClearSelectionFocus();
        }
        private string ResolveActiveLevelDatPath()
        {
            string rootPath = AppSettings.Instance.ServerExecutablePath;
            if (string.IsNullOrWhiteSpace(rootPath) || !Directory.Exists(rootPath)) return null;

            string propsPath = Path.Combine(rootPath, "server.properties");
            string worldFolder = "Dedicated Server";

            if (File.Exists(propsPath))
            {
                foreach (string rawLine in File.ReadLines(propsPath))
                {
                    string line = rawLine.Trim();
                    if (line.StartsWith("level-name=", StringComparison.OrdinalIgnoreCase))
                    {
                        string val = line.Substring(11).Trim();
                        if (!string.IsNullOrWhiteSpace(val)) worldFolder = val;
                        break;
                    }
                }
            }

            string fullPath = Path.Combine(rootPath, "worlds", worldFolder, "level.dat");
            return File.Exists(fullPath) ? fullPath : null;
        }

        private void RefreshWorldValues()
        {
            string targetFile = ResolveActiveLevelDatPath();
            if (string.IsNullOrWhiteSpace(targetFile)) return;

            bool betaEnabled = LevelDatPatcher.IsExperimentEnabled(targetFile, "gametest");
            bool ucfEnabled = LevelDatPatcher.IsExperimentEnabled(targetFile, "upcoming_creator_features");
            bool dtpEnabled = LevelDatPatcher.IsExperimentEnabled(targetFile, "deferred_technical_preview");

            // ✅ INITIALIZATION GUARDED: Switches won't trip changing events while reading file states
            if (LEBetaAPI != null) { LEBetaAPI.Checked = betaEnabled; LEBetaAPI.Enabled = !betaEnabled; }
            if (LEUCF != null) { LEUCF.Checked = ucfEnabled; LEUCF.Enabled = !ucfEnabled; }
            if (LEDTP != null) { LEDTP.Checked = dtpEnabled; LEDTP.Enabled = !dtpEnabled; }
        }

        private void ExperimentSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing || !(sender is MaterialSwitch targetedSwitch)) return;

            if (targetedSwitch.Checked && !AppSettings.Instance.ExperimentsWarned)
            {
                if (this.FindForm() is MaterialForm parentMaterialForm)
                {
                    DialogResult warningResult = MaterialMessageBox.Show(
                        parentMaterialForm,
                        "PERMANENT EXPERIMENTAL LOCKOUT WARNING:\n\n" +
                        "Enabling experimental features irreversibly alters this world file. " +
                        "Achievements will be permanently disabled for this world, and it cannot be reverted back to a vanilla state.\n\n" +
                        "Are you absolutely certain you want to apply these permanent changes to the world binary?\n",
                        "Irreversible World Modification Warning",
                        MessageBoxButtons.YesNo,
                        false,
                        MaterialFlexibleForm.ButtonsPosition.Center
                    );

                    if (warningResult == DialogResult.No)
                    {
                        _isInitializing = true;
                        targetedSwitch.Checked = false;
                        _isInitializing = false;
                        ClearSelectionFocus();
                        return;
                    }

                    AppSettings.Instance.ExperimentsWarned = true;
                    AppSettings.Instance.SaveSettings();
                }
            }

            string targetFile = ResolveActiveLevelDatPath();
            if (string.IsNullOrWhiteSpace(targetFile)) return;

            string experimentToken = "gametest";
            if (targetedSwitch == LEUCF) experimentToken = "upcoming_creator_features";
            if (targetedSwitch == LEDTP) experimentToken = "deferred_technical_preview";

            LevelDatPatcher.SetExperimentState(targetFile, experimentToken, targetedSwitch.Checked);

            if (targetedSwitch.Checked)
            {
                targetedSwitch.Enabled = false;
            }

            ClearSelectionFocus();
        }
    }
}
