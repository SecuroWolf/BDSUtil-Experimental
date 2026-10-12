using System;
using System.Windows.Forms;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Manager;
using ReaLTaiizor.Controls;

namespace BDSUtil
{
    public partial class SettingsForm : MaterialForm
    {
        private readonly MaterialSkinManager _materialSkinManager;
        private SettingsContentControl _contentControl;

        public SettingsForm()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterParent;

            _materialSkinManager = MaterialSkinManager.Instance;
            _materialSkinManager.AddFormToManage(this);

            // 1. Build and dock the visual canvas layout first
            InitializeSettingsCanvas();

            // 2. CRITICAL SEQUENTIAL HOOK: Force the configuration parameters to load 
            // strictly during the parent form's own Load execution window timeline.
            this.Load += SettingsForm_Load;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            _contentControl.LoadCurrentConfigurationState();
            if (_contentControl != null)
            {
                this.Invalidate();
                this.Update();
            }
        }


        private void InitializeSettingsCanvas()
        {
            _contentControl = new SettingsContentControl();
            _contentControl.Dock = DockStyle.Fill;

            // Append the child canvas directly inside your scrollable designer card container
            SettingsContentCard.Controls.Add(_contentControl);
        }

        private async void CloseButton_Click(object sender, EventArgs e)
        {
            CloseButton.Enabled = false;

            // Direct our child canvas sheet instance to save its visual selection states down to AppSettings memory maps
            _contentControl.SyncStagedPropertiesBeforeSave();

            // Pull configuration path parameters straight into the global execution manager
            AppSettings.Instance.ServerExecutablePath = _contentControl.StagedServerPath;

            try
            {
                // Fire off the unblocked asynchronous disk serialization write loop
                await AppSettings.Instance.SaveSettingsAsync();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                // Corrected to ReaLTaiizor's native explicit parameter syntax maps
                MaterialMessageBox.Show(
                    $"An unexpected disk operations fault occurred while attempting to write settings changes:\n\n{ex.Message}",
                    "Disk Configuration Write Failure",
                    MessageBoxButtons.OK,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );



                CloseButton.Enabled = true;
            }
        }
    }
}
