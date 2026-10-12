using System;
using System.Windows.Forms;
using ReaLTaiizor.Forms;

namespace BDSUtil.Modals
{
    public partial class ServerConfigModalForm : MaterialForm
    {
        private MainWindow _parentForm;

        public ServerConfigModalForm(MainWindow parent)
        {
            InitializeComponent();
            _parentForm = parent;
            this.Text = "Server Settings Configuration";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void ServerConfigModalForm_Load(object sender, EventArgs e)
        {
        }

        private void PHCl_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
