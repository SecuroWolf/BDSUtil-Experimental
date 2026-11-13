using BDSUtil.Modals.Content_Controls.CfgHelper;
using BDSUtil.UI;
using Material3.Core;
using Material3.WinForms.Drawing;
using System;
using System.IO;
using System.Windows.Forms;

namespace BDSUtil.Modals
{
    public partial class ServerConfig : Material3Form
    {
        private readonly MainWindow _parentForm;
        private string _serverRootPath;
        private int _prevConfigTabIndex = 0;

        private bool _isPropertiesLoaded = false;
        private bool _isWhitelistLoaded = false;
        private bool _isOperatorsLoaded = false;
        private bool _isNetworkLoaded = false;

        public ServerConfig(MainWindow parent)
        {
            InitializeComponent();
            _parentForm = parent;
            this.Text = "Server Settings Configuration";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;
            this.ShowInTaskbar = true;
            this.StartPosition = FormStartPosition.CenterParent;
            SyncTitleBarMetadata();

            Material3NavigationHelper.Bind(
                _navRail,
                CfgTabControl,
                ("Properties", MaterialIcons.Settings),
                ("Whitelist", MaterialIcons.CheckCircle),
                ("Permissions", MaterialIcons.DeployedCode),
                ("Networking", MaterialIcons.TravelExplore),
                ("Exit", MaterialIcons.Close)
            );

            if (CfgTabControl != null)
            {
                CfgTabControl.SelectedIndexChanged += CfgTabControl_SelectedIndexChanged;
            }
        }

        private void ServerConfig_Load(object sender, EventArgs e)
        {
            string storedPath = AppSettings.Instance.General.ServerExecutablePath;
            if (string.IsNullOrWhiteSpace(storedPath)) return;

            _serverRootPath = storedPath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)
                ? Path.GetDirectoryName(storedPath)
                : storedPath;

            if (CfgTabControl != null)
            {
                if (CfgTabControl.SelectedTab == PropTab)
                {
                    InitializeChildControls();
                }
                else if (CfgTabControl.SelectedTab.Name == "WhitelistTab" || CfgTabControl.SelectedTab.Text.Contains("Whitelist"))
                {
                    InitializeWhitelistControl();
                }
            }
        }
        private void CfgTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CfgTabControl == null || CfgTabControl.SelectedTab == null) return;

            if (CfgTabControl.SelectedTab == PropTab)
            {
                _prevConfigTabIndex = CfgTabControl.SelectedIndex;
                InitializeChildControls();
            }
            else if (CfgTabControl.SelectedTab == WLTab || CfgTabControl.SelectedTab.Name.Contains("Whitelist"))
            {
                _prevConfigTabIndex = CfgTabControl.SelectedIndex;
                InitializeWhitelistControl();
            }
            else if (CfgTabControl.SelectedTab == PermTab || CfgTabControl.SelectedTab.Name == "PermTab")
            {
                _prevConfigTabIndex = CfgTabControl.SelectedIndex;
                InitializeOperatorsControl();
            }
            else if (CfgTabControl.SelectedTab == NetTab || CfgTabControl.SelectedTab.Name == "NetTab")
            {
                _prevConfigTabIndex = CfgTabControl.SelectedIndex;
                InitializeNetworkControl();
            }
            else if (CfgTabControl.SelectedTab == ExitTab)
            {
                CloseWindow();
            }
        }
        private void InitializeChildControls()
        {
            if (_isPropertiesLoaded || string.IsNullOrEmpty(_serverRootPath)) return;
            if (serverPropertiesControl1 != null)
            {
                serverPropertiesControl1.InitializeControl(_serverRootPath);
                _isPropertiesLoaded = true;
            }
        }

        private void InitializeWhitelistControl()
        {
            if (_isWhitelistLoaded || string.IsNullOrEmpty(_serverRootPath)) return;
            if (whitelistControl1 != null)
            {
                whitelistControl1.InitializeControl(_serverRootPath);
                _isWhitelistLoaded = true;
            }
        }

        private void InitializeOperatorsControl()
        {
            if (_isOperatorsLoaded || string.IsNullOrEmpty(_serverRootPath)) return;
            if (permissionsControl1 != null)
            {
                permissionsControl1.InitializeControl(_serverRootPath);
                _isOperatorsLoaded = true;
            }
        }

        private void InitializeNetworkControl()
        {
            if (_isNetworkLoaded || string.IsNullOrEmpty(_serverRootPath)) return;
            if (networkingControl1 != null)
            {
                networkingControl1.InitializeControl(_serverRootPath);
                _isNetworkLoaded = true;
            }
        }

        private void CloseWindow()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
