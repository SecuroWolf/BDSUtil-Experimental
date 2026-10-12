using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using ReaLTaiizor.Controls;

namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    public partial class ServerPropertiesControl : UserControl
    {
        private string _configPath;
        private Dictionary<string, string> _properties;

        public ServerPropertiesControl()
        {
            InitializeComponent();
            _properties = new Dictionary<string, string>();
        }

        public void InitializeControl(string serverRoot)
        {
            _configPath = Path.Combine(serverRoot, "server.properties");
            LoadConfig();
        }

        private void LoadConfig()
        {
            if (!File.Exists(_configPath)) return;
            _properties.Clear();
            string[] lines = File.ReadAllLines(_configPath);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                int index = line.IndexOf('=');
                if (index <= 0) continue;
                string key = line.Substring(0, index).Trim();
                string value = line.Substring(index + 1).Trim();
                _properties[index.ToString()] = value;
            }
            BindDataToUI();
        }

        private void BindDataToUI()
        {
        }

        public void SaveConfig()
        {
            List<string> lines = new List<string>();
            foreach (KeyValuePair<string, string> kvp in _properties)
            {
                lines.Add($"{kvp.Key}={kvp.Value}");
            }
            File.WriteAllLines(_configPath, lines);
        }

        private void ServerPropertiesControl_Load(object sender, EventArgs e)
        {

        }
    }
}

