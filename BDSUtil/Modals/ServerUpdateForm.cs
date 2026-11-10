using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using BDSUtil.Helpers.Updates;

namespace BDSUtil.Modals
{
    public partial class ServerUpdateForm : MaterialForm
    {
        private readonly string _serverRoot;
        private readonly string _targetVersion;
        private readonly string _downloadUrl;

        public ServerUpdateForm(string serverRoot, string targetVersion, string downloadUrl)
        {
            InitializeComponent();

            _serverRoot = serverRoot;
            _targetVersion = targetVersion;
            _downloadUrl = downloadUrl;

            // Optional: Force the form canvas to match your dark slate theme natively
            this.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
        }

        private async void ServerUpdateForm_Load(object sender, EventArgs e)
        {
            // Lock the UI so the user can't close the form mid-update
            this.ControlBox = false;
            UpdateCloseBTN.Visible = false;

            await RunUpdatePipelineAsync();
        }

        private async Task RunUpdatePipelineAsync()
        {
            try
            {
                // STEP 1: Backup
                StatusLabel.Text = "Status: Generating pre-update backup archive...";
                UpdateProgressBar.Value = 0;
                UpdateProgressBar.Style = ProgressBarStyle.Marquee; // Indeterminate loading for zip creation

                await ServerUpdates.DoBackupsAsync(_serverRoot, _targetVersion);

                // STEP 2: Download
                StatusLabel.Text = $"Status: Downloading Bedrock Server v{_targetVersion}...";
                UpdateProgressBar.Style = ProgressBarStyle.Continuous;
                UpdateProgressBar.Value = 0;

                string tempZipPath = Path.Combine(_serverRoot, $"bds_update_{_targetVersion}.zip");

                // Progress reporter to feed the UI thread
                var progress = new Progress<int>(percent =>
                {
                    UpdateProgressBar.Value = percent;
                });

                await ServerUpdates.DownloadUpdateAsync(_downloadUrl, tempZipPath, progress);

                // STEP 3: Install/Extract
                StatusLabel.Text = "Status: Extracting binaries and preserving custom data...";
                UpdateProgressBar.Style = ProgressBarStyle.Marquee;

                await ServerUpdates.InstallUpdateAsync(_serverRoot, tempZipPath);

                // STEP 4: Complete
                StatusLabel.Text = "Status: Update successfully completed!";
                UpdateProgressBar.Style = ProgressBarStyle.Continuous;
                UpdateProgressBar.Value = 100;

                MaterialMessageBox.Show(
                    this,
                    $"The server has been successfully updated to v{_targetVersion}.\nYour worlds and custom packs were preserved.",
                    "Update Complete",
                    MessageBoxButtons.OK,
                    false,
                    MaterialFlexibleForm.ButtonsPosition.Center
                );

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                StatusLabel.Text = "Status: Update failed.";
                UpdateProgressBar.Style = ProgressBarStyle.Continuous;
                UpdateProgressBar.Value = 0;

                MaterialMessageBox.Show(
                    this,
                    $"A critical error occurred during the update pipeline:\n\n{ex.Message}",
                    "Update Failure",
                    MessageBoxButtons.OK,
                    false,
                    MaterialFlexibleForm.ButtonsPosition.Center
                );

                // Allow the user to close the form if it fails
                UpdateCloseBTN.Visible = true;
                this.ControlBox = true;
            }
        }

        private void UpdateCloseBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}