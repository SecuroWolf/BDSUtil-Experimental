using System;
using System.IO;
using System.Windows.Forms;

namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    public partial class ChatFilterControl : UserControl
    {
        private string _configPath;

        public ChatFilterControl()
        {
            InitializeComponent();
        }

        public void InitializeControl(string serverRoot)
        {
            _configPath = Path.Combine(serverRoot, "chatfilter.cfg");
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
