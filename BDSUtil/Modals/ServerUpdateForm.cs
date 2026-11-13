using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using BDSUtil.Helpers.Updates;
using BDSUtil.UI;

namespace BDSUtil.Modals
{
    public partial class ServerUpdateForm : Material3Form
    {
        private readonly string _serverRoot;
        private readonly string _targetVersion;
        private readonly string _downloadUrl;
        private readonly bool _isFreshInstall;

        public ServerUpdateForm(string serverRoot, string targetVersion, string downloadUrl, bool isFreshInstall = false)
        {
            InitializeComponent();

            _serverRoot = serverRoot;
            _targetVersion = targetVersion;
            _downloadUrl = downloadUrl;
            _isFreshInstall = isFreshInstall;

            this.Text = isFreshInstall ? "Bedrock Server Installer" : "Bedrock Server Updater";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;
            SyncTitleBarMetadata();
        }

        private async void ServerUpdateForm_Load(object sender, EventArgs e)
        {
            // Lock the UI so the user can't close the form mid-update
            this.ControlBox = false;
            this.TitleBar.ShowClose = false;
            UpdateCloseBTN.Visible = false;

            await RunUpdatePipelineAsync();
        }

        private async Task RunUpdatePipelineAsync()
        {
            string tempZipPath = _isFreshInstall
                ? Path.Combine(Path.GetTempPath(), $"bds_setup_{_targetVersion}_{Guid.NewGuid():N}.zip")
                : Path.Combine(_serverRoot, $"bds_update_{_targetVersion}.zip");

            try
            {
                if (!_isFreshInstall)
                {
                    // STEP 1: Backup existing server before updating
                    StatusLabel.Text = "Status: Generating pre-update backup archive...";
                    UpdateProgressBar.Value = 0;
                    UpdateProgressBar.Style = ProgressBarStyle.Marquee;

                    await ServerUpdates.DoBackupsAsync(_serverRoot, _targetVersion);
                }

                // STEP 2: Download
                StatusLabel.Text = $"Status: Downloading Bedrock Server v{_targetVersion}...";
                UpdateProgressBar.Style = ProgressBarStyle.Continuous;
                UpdateProgressBar.Value = 0;

                var progress = new Progress<int>(percent =>
                {
                    UpdateProgressBar.Value = Math.Clamp(percent, 0, 100);
                });

                await ServerUpdates.DownloadUpdateAsync(_downloadUrl, tempZipPath, progress);

                // STEP 3: Install/Extract
                StatusLabel.Text = _isFreshInstall
                    ? $"Status: Extracting Bedrock Server v{_targetVersion}..."
                    : "Status: Extracting binaries and preserving custom data...";
                UpdateProgressBar.Style = ProgressBarStyle.Marquee;

                if (_isFreshInstall)
                {
                    await ServerUpdates.InstallFreshServerAsync(_serverRoot, tempZipPath);
                }
                else
                {
                    await ServerUpdates.InstallUpdateAsync(_serverRoot, tempZipPath);
                }

                // STEP 4: Complete
                StatusLabel.Text = _isFreshInstall
                    ? "Status: Server setup completed successfully!"
                    : "Status: Update successfully completed!";
                UpdateProgressBar.Style = ProgressBarStyle.Continuous;
                UpdateProgressBar.Value = 100;

                string completionMessage = _isFreshInstall
                    ? $"Minecraft Bedrock Dedicated Server v{_targetVersion} has been installed to:\n\n{_serverRoot}"
                    : $"The server has been successfully updated to v{_targetVersion}.\nYour worlds and custom packs were preserved.";
                string completionTitle = _isFreshInstall ? "Server Setup Complete" : "Update Complete";

                MaterialDialogBox.Show(
                    this,
                    completionMessage,
                    completionTitle,
                    MessageBoxButtons.OK
                );

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                try { if (File.Exists(tempZipPath)) File.Delete(tempZipPath); } catch { }

                StatusLabel.Text = _isFreshInstall ? "Status: Server setup failed." : "Status: Update failed.";
                UpdateProgressBar.Style = ProgressBarStyle.Continuous;
                UpdateProgressBar.Value = 0;

                MaterialDialogBox.Show(
                    this,
                    $"A critical error occurred during the download/install pipeline:\n\n{ex.Message}",
                    _isFreshInstall ? "Server Setup Failure" : "Update Failure",
                    MessageBoxButtons.OK
                );

                // Allow the user to close the form if it fails
                UpdateCloseBTN.Visible = true;
                this.ControlBox = true;
                this.TitleBar.ShowClose = true;
            }
        }

        private void UpdateCloseBTN_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}