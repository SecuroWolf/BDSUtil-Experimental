using BDSUtil.UI;
using System;
using System.IO;
using System.Windows.Forms;

namespace BDSUtil.Helpers.AutoMod
{
    public static class AutoModValidator
    {
        public static void ValidateStateOnDirectoryChange(string newServerRoot, Form parentForm)
        {
            // If AutoMod is already off, we don't need to validate anything.
            if (!AppSettings.Instance.AutoMod.Enabled) return;

            if (string.IsNullOrWhiteSpace(newServerRoot) || !Directory.Exists(newServerRoot))
            {
                DisableAutoMod(parentForm, "The server directory is invalid or missing.");
                return;
            }

            string targetWorldName = "Dedicated Server";
            string serverPropsPath = Path.Combine(newServerRoot, "server.properties");

            if (ServerPropertiesDocument.TryGetProperty(serverPropsPath, "level-name", out string configuredWorld) &&
                !string.IsNullOrWhiteSpace(configuredWorld))
            {
                targetWorldName = configuredWorld;
            }

            string levelDatPath = Path.Combine(newServerRoot, "worlds", targetWorldName, "level.dat");

            // Scan for prerequisites
            bool isApiActive = LevelDatPatcher.IsExperimentEnabled(levelDatPath, "gametest");
            bool isFolderInstalled = PackManager.IsPackInstalled(newServerRoot, BDSPackInstaller.MONITOR_PACK_FOLDER, isBehaviorPack: true);
            bool isPackEnabled = PackManager.IsPackEnabledInWorld(newServerRoot, targetWorldName, BDSPackInstaller.MONITOR_PACK_UUID, isBehaviorPack: true);

            if (!isApiActive || !isFolderInstalled || !isPackEnabled)
            {
                DisableAutoMod(parentForm,
                    "You have switched server directories.\n\n" +
                    "AutoMod has been temporarily disabled because the target world lacks the required monitor pack or Beta APIs. " +
                    "Navigate back to the AutoMod tab to re-enable and apply the prerequisites to this new environment.");
            }
        }

        private static void DisableAutoMod(Form parentForm, string message)
        {
            AppSettings.Instance.Update(cfg => cfg.AutoMod.Enabled = false);

            if (parentForm != null && !parentForm.IsDisposed)
            {
                MaterialDialogBox.Show(
                    parentForm,
                    message,
                    "AutoMod Disabled",
                    MessageBoxButtons.OK
                );
            }
        }
    }
}

