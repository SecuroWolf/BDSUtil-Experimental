using System;
using System.IO;
using System.Windows.Forms;

namespace BDSUtil
{
    public partial class SettingsContentControl : UserControl
    {
        public string StagedServerPath { get; set; } = string.Empty;

        public SettingsContentControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Populates the layout fields on instantiation by reading current global AppSettings parameters.
        /// </summary>
        public void LoadCurrentConfigurationState()
        {
            var currentSettings = AppSettings.Instance;

            // 1. Pull the path parameters straight out of your updated text parser instance
            StagedServerPath = currentSettings.ServerExecutablePath;

            // 2. Direct string value assignment
            if (string.IsNullOrWhiteSpace(StagedServerPath))
            {
                ServerPathTextBox.Text = "No directory selected...";
            }
            else
            {
                ServerPathTextBox.Text = StagedServerPath;
            }

            // 3. Queue the index selection directly to your Crown dropdown
            if (currentSettings.IsWinRarInstalled)
            {
                CompressionTypeComboBox.Enabled = true;
                int savedMode = currentSettings.CompressionMode;

                this.BeginInvoke((MethodInvoker)delegate
                {
                    if (CompressionTypeComboBox.Items.Count > savedMode)
                    {
                        CompressionTypeComboBox.SelectedIndex = savedMode;
                    }
                });
            }
            else
            {
                if (CompressionTypeComboBox.Items.Count >= 3)
                {
                    CompressionTypeComboBox.SelectedIndex = 2;
                }
                CompressionTypeComboBox.Enabled = false;
            }
        }



        /// <summary>
        /// Reads the dropdown control state to synchronize data variables right before the save operation.
        /// </summary>
        public void SyncStagedPropertiesBeforeSave()
        {
            AppSettings.Instance.CompressionMode = CompressionTypeComboBox.SelectedIndex;
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Bedrock Server Executable (bedrock_server.exe)|bedrock_server.exe";
                openFileDialog.Title = "Locate Minecraft Bedrock Dedicated Server Target Executable";

                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string fullPath = openFileDialog.FileName;
                    StagedServerPath = Path.GetDirectoryName(fullPath);
                    ServerPathTextBox.Text = StagedServerPath;
                }
            }
        }
    }
}
