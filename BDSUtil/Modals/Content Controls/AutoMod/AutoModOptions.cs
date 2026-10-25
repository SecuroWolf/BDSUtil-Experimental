using BDSUtil.Helpers;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using System;
using System.IO;
using System.Windows.Forms;

namespace BDSUtil.Modals.Content_Controls.AutoMod
{
    public partial class AutoModOptions : UserControl
    {
        private bool _isInitializing = true;

        public AutoModOptions()
        {
            InitializeComponent();

            if (CfgEAM != null)
            {
                CfgEAM.CheckedChanged -= CfgEAM_CheckedChanged;
                CfgEAM.CheckedChanged += CfgEAM_CheckedChanged;
            }

            this.Load += (s, e) => { InitializeControl(); };
        }

        public void InitializeControl()
        {
            _isInitializing = true;

            if (CfgEAM != null)
                CfgEAM.Checked = AppSettings.Instance.AutoModEnabled;

            if (AMCCard != null)
                AMCCard.Visible = AppSettings.Instance.AutoModEnabled;

            _isInitializing = false;
        }

        private void CfgEAM_CheckedChanged(object sender, EventArgs e)
        {
            if (_isInitializing || CfgEAM == null || AMCCard == null) return;

            if (!CfgEAM.Checked)
            {
                AMCCard.Visible = false;
                AppSettings.Instance.AutoModEnabled = false;
                AppSettings.Instance.SaveSettings();
                return;
            }

            // --- STEP 1: Verify World Editor Master Safety Authorization Flag ---
            if (!AppSettings.Instance.WorldEditorEnabled)
            {
                _isInitializing = true;
                CfgEAM.Checked = false;
                AMCCard.Visible = false;
                _isInitializing = false;

                if (this.FindForm() is MaterialForm parentForm)
                {
                    DialogResult redirectResult = MaterialMessageBox.Show(
                        parentForm,
                        "The World Editor tools are currently disabled.\n\nWould you like to go to the App Settings configuration menu to review the safety warning and enable world modifications now?",
                        "World Editor Inactive",
                        MessageBoxButtons.YesNo,
                        false,
                        MaterialFlexibleForm.ButtonsPosition.Center
                    );

                    if (redirectResult == DialogResult.Yes)
                    {
                        parentForm.DialogResult = DialogResult.Retry;
                        parentForm.Close();
                    }
                }
                return;
            }
            // Resolve active server execution environment path configurations
            string serverRoot = AppSettings.Instance.ServerExecutablePath;
            string targetWorldName = "Dedicated Server"; // Default vanilla matching token
            string serverPropsPath = Path.Combine(serverRoot, "server.properties");

            if (File.Exists(serverPropsPath))
            {
                try
                {
                    foreach (string rawLine in File.ReadLines(serverPropsPath))
                    {
                        string line = rawLine.Trim();
                        if (line.StartsWith("level-name=", StringComparison.OrdinalIgnoreCase))
                        {
                            string val = line.Substring(11).Trim();
                            if (!string.IsNullOrWhiteSpace(val)) targetWorldName = val;
                            break;
                        }
                    }
                }
                catch { }
            }

            string levelDatPath = Path.Combine(serverRoot, "worlds", targetWorldName, "level.dat");

            // --- STEP 2: Use Production Patcher to Verify and Inject Beta APIs ---
            bool isApiActive = LevelDatPatcher.IsExperimentEnabled(levelDatPath, "gametest");

            if (!isApiActive)
            {
                if (this.FindForm() is MaterialForm parentForm)
                {
                    DialogResult apiDialog = MaterialMessageBox.Show(
                        parentForm,
                        "AutoMod requires active Minecraft Beta Scripting APIs to capture chat log event string variables.\n\n" +
                        "CRITICAL INFO: Once Beta APIs are enabled, they CANNOT be turned off on this world, and world experiments will permanently disable game achievements.\n\n" +
                        "Would you like BDSUtil to auto-patch your level.dat tags to enable them now?",
                        "Beta APIs Activation Required",
                        MessageBoxButtons.YesNo,
                        false,
                        MaterialFlexibleForm.ButtonsPosition.Center
                    );

                    if (apiDialog == DialogResult.Yes)
                    {
                        if (!LevelDatPatcher.EnableBetaApis(levelDatPath))
                        {
                            ResetToggleToOff();
                            MaterialMessageBox.Show(parentForm, "Failed to patch world level.dat tags. Check if files are currently locked by the bedrock server process.", "Patch Aborted", MessageBoxButtons.OK, false, MaterialFlexibleForm.ButtonsPosition.Center);
                            return;
                        }

                        // Sync safety preferences globally so Level Editor panel skips redundant prompts
                        AppSettings.Instance.ExperimentsWarned = true;
                        AppSettings.Instance.SaveSettings();
                    }
                    else
                    {
                        ResetToggleToOff();
                        return;
                    }
                }
            }
            // --- STEP 3: Decoupled Folder Presence and World Manifest Verifications ---
            string globalPackPath = Path.Combine(serverRoot, "behavior_packs", "bds_monitor");
            bool isFolderInstalled = Directory.Exists(globalPackPath);
            bool isPackEnabled = PackManager.IsPackEnabledInWorld(serverRoot, targetWorldName);

            if (isFolderInstalled && isPackEnabled)
            {
                AMCCard.Visible = true;
                AppSettings.Instance.AutoModEnabled = true;
                AppSettings.Instance.SaveSettings();
            }
            else
            {
                if (this.FindForm() is MaterialForm parentForm)
                {
                    // ✅ FIXED: String optimized and newline appended to guarantee no layout canvas clipping
                    DialogResult packDialog = MaterialMessageBox.Show(
                        parentForm,
                        "The 'bds_monitor' backend script addon folder is missing or disabled for the currently selected server world save.\n\n" +
                        "Would you like BDSUtil to automatically install the required plugin directories and register the pack entries now?\n",
                        "Addon Configuration Required",
                        MessageBoxButtons.YesNo,
                        false,
                        MaterialFlexibleForm.ButtonsPosition.Center
                    );

                    if (packDialog == DialogResult.Yes)
                    {
                        if (!isFolderInstalled)
                        {
                            if (!BDSPackInstaller.InstallMonitorPackFiles(serverRoot))
                            {
                                ResetToggleToOff();
                                MaterialMessageBox.Show(parentForm, "File system write block. Unable to write pack files down onto disk nodes.", "Installation Failed", MessageBoxButtons.OK, false, MaterialFlexibleForm.ButtonsPosition.Center);
                                return;
                            }
                        }

                        if (!isPackEnabled)
                        {
                            if (!PackManager.EnablePackInWorld(serverRoot, targetWorldName))
                            {
                                ResetToggleToOff();
                                MaterialMessageBox.Show(parentForm, "Failed to update world_behavior_packs.json array configuration mappings safely.", "Activation Failed", MessageBoxButtons.OK, false, MaterialFlexibleForm.ButtonsPosition.Center);
                                return;
                            }
                        }

                        AMCCard.Visible = true;
                        AppSettings.Instance.AutoModEnabled = true;
                        AppSettings.Instance.SaveSettings();

                        MaterialMessageBox.Show(parentForm, "The monitor plugin addon has been successfully built and registered.\n\nA Bedrock Dedicated Server reboot is required to activate the chat bridge pipeline.", "Setup Complete", MessageBoxButtons.OK, false, MaterialFlexibleForm.ButtonsPosition.Center);
                    }
                    else
                    {
                        ResetToggleToOff();
                    }
                }
            }
        }


        private void ResetToggleToOff()
        {
            _isInitializing = true;
            if (CfgEAM != null) CfgEAM.Checked = false;
            if (AMCCard != null) AMCCard.Visible = false;
            _isInitializing = false;

            AppSettings.Instance.AutoModEnabled = false;
            AppSettings.Instance.SaveSettings();
        }
    }
}
