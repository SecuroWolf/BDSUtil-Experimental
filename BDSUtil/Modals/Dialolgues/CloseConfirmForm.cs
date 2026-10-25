using System;
using System.Windows.Forms;
using ReaLTaiizor.Forms;

namespace BDSUtil
{
    public partial class CloseConfirmForm : MaterialForm
    {
        // Public property to pass the selected action token back up to MainWindow
        public string SelectedExitAction { get; private set; } = "CANCEL";

        public CloseConfirmForm()
        {
            InitializeComponent();

            // Hook up ReaLTaiizor's skin manager to keep the custom dialog dark-themed
            var skinManager = ReaLTaiizor.Manager.MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);

            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent; // Center perfectly locked to main window!
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
