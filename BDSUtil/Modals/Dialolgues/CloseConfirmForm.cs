using System;
using System.Windows.Forms;
using BDSUtil.UI;

namespace BDSUtil
{
    public partial class CloseConfirmForm : Material3Form
    {
        // Public property to pass the selected action token back up to MainWindow
        public string SelectedExitAction { get; private set; } = "CANCEL";

        public CloseConfirmForm()
        {
            InitializeComponent();

            this.ShowTitleBar = false;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            SyncTitleBarMetadata();
        }

        private void YesBtn_Click(object sender, EventArgs e)
        {
            SelectedExitAction = "CLEAN_STOP";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ForceStopBtn_Click(object sender, EventArgs e)
        {
            SelectedExitAction = "FORCE_KILL";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void NoBtn_Click(object sender, EventArgs e)
        {
            SelectedExitAction = "CANCEL";
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}
