using System;
using System.IO;
using System.Windows.Forms;

namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    public partial class PermissionsControl : UserControl
    {
        private string _configPath;

        public PermissionsControl()
        {
            InitializeComponent();
        }

        public void InitializeControl(string serverRoot)
        {
            _configPath = Path.Combine(serverRoot, "permissions.json");
            LoadConfig();
        }

        private void LoadConfig()
        {
            if (!File.Exists(_configPath)) return;
        }

        public void SaveConfig()
        {
        }
    }
}
