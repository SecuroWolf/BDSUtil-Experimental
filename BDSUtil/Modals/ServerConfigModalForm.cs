using System;
using System.IO;
using System.Windows.Forms;
using ReaLTaiizor.Forms;

namespace BDSUtil.Modals
{
    public partial class ServerConfigModalForm : MaterialForm
    {
        private MainWindow _parentForm;
        private string _serverRootPath;
        // Track whether we have successfully initialized the file parser state cache
        private bool _isPropertiesLoaded = false;

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

            // Tie the tab selection change loop straight to our runtime initializer hook
            if (CfgTabControl != null)
            {
                CfgTabControl.SelectedIndexChanged += CfgTabControl_SelectedIndexChanged;
            }
        }

        private void ServerConfigModalForm_Load(object sender, EventArgs e)
        {
            // Extract the server execution target path from your application settings loader
            string storedPath = AppSettings.Instance.ServerExecutablePath;
            if (string.IsNullOrWhiteSpace(storedPath)) return;

            // Strip the binary filename suffix if the tracked config path points directly to the executable
            if (storedPath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                _serverRootPath = Path.GetDirectoryName(storedPath);
            }
            else
            {
                _serverRootPath = storedPath;
            }

            // If the modal form defaults to opening straight on the properties tab index, execute the initializer immediately
            if (CfgTabControl != null && CfgTabControl.SelectedTab == PropTab)
            {
                InitializeChildControls();
            }
        }

        /// <summary>
        /// Catches and processes user tab navigation choices to intercept hidden initialization routines.
        /// </summary>
        private void CfgTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CfgTabControl == null) return;

            // If the operator lands on the standard property card sheet, force the configuration data read loop
            if (CfgTabControl.SelectedTab == PropTab)
            {
                InitializeChildControls();
            }
            // Intercept your placeholder save and close navigation macro
            else if (CfgTabControl.SelectedTab == SaveDummyTab)
            {
                ExecuteConfigSaveAndClose();
            }
        }

        private void InitializeChildControls()
        {
            // Block repeat iterations if the file track has already been loaded into memory bounds
            if (_isPropertiesLoaded || string.IsNullOrEmpty(_serverRootPath)) return;

            // Transmit the verified path down to your properties manager user control
            if (serverPropertiesControl1 != null)
            {
                serverPropertiesControl1.InitializeControl(_serverRootPath);
                _isPropertiesLoaded = true; // Set block lock true on success
            }
        }

        private void ExecuteConfigSaveAndClose()
        {
            try
            {
                // Only issue file write operations if the user control was initialized and has active state data changes
                if (_isPropertiesLoaded && serverPropertiesControl1 != null)
                {
                    serverPropertiesControl1.SaveConfig();
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unexpected exception fouled up file structure IO saves: " + ex.Message,
                                "Write Violation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PHCl_Click(object sender, EventArgs e)
        {
            ExecuteConfigSaveAndClose();
        }
    }
}
