using BDSUtil.Helpers;
using BDSUtil.Helpers.AutoMod;
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

            // --- Load AutoMod Configurations ---
            if (AM_Enforce != null) AM_Enforce.Checked = AMConfHelper.Instance.EnableChatEnforcement;
            if (AM_PEnType != null) AM_PEnType.Text = AMConfHelper.Instance.PrimaryEnforcementType;
            if (AM_PEnThresh != null) AM_PEnThresh.Text = AMConfHelper.Instance.PrimaryEnforcementThreshold;

            if (AM_SEnType != null) AM_SEnType.Text = AMConfHelper.Instance.SecondaryEnforcementType;
            if (AM_SEnThresh != null) AM_SEnThresh.Text = AMConfHelper.Instance.SecondaryEnforcementThreshold;

            if (AM_AntiSpam != null) AM_AntiSpam.Checked = AMConfHelper.Instance.AntiSpamEnabled;
            if (AM_ASThreshMessages != null) AM_ASThreshMessages.Text = AMConfHelper.Instance.AntiSpamMessages;
            if (AM_ASThreshTime != null) AM_ASThreshTime.Text = AMConfHelper.Instance.AntiSpamTime;
            if (AM_ASEnType != null) AM_ASEnType.Text = AMConfHelper.Instance.AntiSpamEnforcementType;

            if (AM_CESR != null) AM_CESR.Text = AMConfHelper.Instance.ChatStrikeReset;
            if (AM_ASSR != null) AM_ASSR.Text = AMConfHelper.Instance.AntiSpamStrikeReset;

            // Hook up the save events dynamically
            WireUpSaveEvents();

            _isInitializing = false;
        }

        private void WireUpSaveEvents()
        {
            if (AM_Enforce != null) AM_Enforce.CheckedChanged += SaveAutoModSettings;
            if (AM_AntiSpam != null) AM_AntiSpam.CheckedChanged += SaveAutoModSettings;

            if (AM_PEnType != null) AM_PEnType.SelectedIndexChanged += SaveAutoModSettings;
            if (AM_SEnType != null) AM_SEnType.SelectedIndexChanged += SaveAutoModSettings;
            if (AM_ASThreshTime != null) AM_ASThreshTime.SelectedIndexChanged += SaveAutoModSettings;
            if (AM_ASEnType != null) AM_ASEnType.SelectedIndexChanged += SaveAutoModSettings;
            if (AM_CESR != null) AM_CESR.SelectedIndexChanged += SaveAutoModSettings;
            if (AM_ASSR != null) AM_ASSR.SelectedIndexChanged += SaveAutoModSettings;

            if (AM_PEnThresh != null) AM_PEnThresh.Leave += SaveAutoModSettings;
            if (AM_SEnThresh != null) AM_SEnThresh.Leave += SaveAutoModSettings;
            if (AM_ASThreshMessages != null) AM_ASThreshMessages.Leave += SaveAutoModSettings;
        }

        private void SaveAutoModSettings(object sender, EventArgs e)
        {
            if (_isInitializing) return;

            if (AM_Enforce != null) AMConfHelper.Instance.EnableChatEnforcement = AM_Enforce.Checked;
            if (AM_PEnType != null) AMConfHelper.Instance.PrimaryEnforcementType = AM_PEnType.Text;
            if (AM_PEnThresh != null) AMConfHelper.Instance.PrimaryEnforcementThreshold = AM_PEnThresh.Text;

            if (AM_SEnType != null) AMConfHelper.Instance.SecondaryEnforcementType = AM_SEnType.Text;
            if (AM_SEnThresh != null) AMConfHelper.Instance.SecondaryEnforcementThreshold = AM_SEnThresh.Text;

            if (AM_AntiSpam != null) AMConfHelper.Instance.AntiSpamEnabled = AM_AntiSpam.Checked;
            if (AM_ASThreshMessages != null) AMConfHelper.Instance.AntiSpamMessages = AM_ASThreshMessages.Text;
            if (AM_ASThreshTime != null) AMConfHelper.Instance.AntiSpamTime = AM_ASThreshTime.Text;
            if (AM_ASEnType != null) AMConfHelper.Instance.AntiSpamEnforcementType = AM_ASEnType.Text;

            if (AM_CESR != null) AMConfHelper.Instance.ChatStrikeReset = AM_CESR.Text;
            if (AM_ASSR != null) AMConfHelper.Instance.AntiSpamStrikeReset = AM_ASSR.Text;

            AMConfHelper.Instance.SaveSettings();
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
            string targetWorldName = "Dedicated Server";
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
            bool isFolderInstalled = PackManager.IsPackInstalled(serverRoot, BDSPackInstaller.MONITOR_PACK_FOLDER, isBehaviorPack: true);
            bool isPackEnabled = PackManager.IsPackEnabledInWorld(serverRoot, targetWorldName, BDSPackInstaller.MONITOR_PACK_UUID, isBehaviorPack: true);

            if (isFolderInstalled && isPackEnabled)
            {
                ConfigureServerPropertiesForAutoMod(serverRoot);
                AMCCard.Visible = true;
                AppSettings.Instance.AutoModEnabled = true;
                AppSettings.Instance.SaveSettings();
            }
            else
            {
                if (this.FindForm() is MaterialForm parentForm)
                {
                    DialogResult packDialog = MaterialMessageBox.Show(
                        parentForm,
                        $"The '{BDSPackInstaller.MONITOR_PACK_FOLDER}' backend script addon folder is missing or disabled for the currently selected server world save.\n\n" +
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
                            if (!BDSPackInstaller.EnableMonitorPack(serverRoot, targetWorldName))
                            {
                                ResetToggleToOff();
                                MaterialMessageBox.Show(parentForm, "Failed to update world_behavior_packs.json array configuration mappings safely.", "Activation Failed", MessageBoxButtons.OK, false, MaterialFlexibleForm.ButtonsPosition.Center);
                                return;
                            }
                        }

                        ConfigureServerPropertiesForAutoMod(serverRoot);
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

        private void ConfigureServerPropertiesForAutoMod(string serverRoot)
        {
            string propsPath = Path.Combine(serverRoot, "server.properties");
            if (!File.Exists(propsPath)) return;

            string content = File.ReadAllText(propsPath);
            bool modified = false;

            // Enforce console output routing so our standard output stream intercepts the telemetry
            if (content.Contains("content-log-console-output-enabled=false"))
            {
                content = content.Replace("content-log-console-output-enabled=false", "content-log-console-output-enabled=true");
                modified = true;
            }

            // Disable file writing to save disk I/O cycles since the Bedrock engine buffers it incorrectly anyway
            if (content.Contains("content-log-file-enabled=true"))
            {
                content = content.Replace("content-log-file-enabled=true", "content-log-file-enabled=false");
                modified = true;
            }

            if (modified)
            {
                File.WriteAllText(propsPath, content);
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