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
        private int _prevConfigTabIndex = 0;
        // Track whether we have successfully initialized the child controls to avoid redundant file reads and writes
        private bool _isPropertiesLoaded = false;
        private bool _isWhitelistLoaded = false;
        private bool _isOperatorsLoaded = false;
        private bool _isNetworkLoaded = false; // Added tracking block state lock field for network configurations

        public ServerConfigModalForm(MainWindow parent)
        {
            InitializeComponent();
            _parentForm = parent;
            this.Text = "Server Settings Configuration";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;

            // FIXED: Set this to true so the window gets its own dedicated taskbar node while MainWindow is hidden!
            this.ShowInTaskbar = true;

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

            // Force initial checking loops if the window spawns on either active control pane
            if (CfgTabControl != null)
            {
                if (CfgTabControl.SelectedTab == PropTab)
                {
                    InitializeChildControls();
                }
                // FIXED: Catch initial boot edge cases if the form default index shifts
                else if (CfgTabControl.SelectedTab.Name == "WhitelistTab" || CfgTabControl.SelectedTab.Text.Contains("Whitelist"))
                {
                    InitializeWhitelistControl();
                }
            }
        }

        /// <summary>
        /// Catches and processes user tab navigation choices to intercept hidden initialization routines.
        /// </summary>
        private void CfgTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CfgTabControl == null || CfgTabControl.SelectedTab == null) return;

            // 1. If navigating to a standard safe tab panel, update our active historical tracking index snapshot
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
            // initialize the intercept target tracking the Networking tab ("NetTab")
            else if (CfgTabControl.SelectedTab == NetTab || CfgTabControl.SelectedTab.Name == "NetTab")
            {
                if (!AppSettings.Instance.HasAcceptedNetworkRisks)
                {
                    using (var alertDialogue = new NetworkSecurityDialogue())
                    {
                        if (alertDialogue.ShowDialog(this) == DialogResult.OK && alertDialogue.Confirmed)
                        {
                            // CHAIN-LINK GATING: Exiting the first dialog opens the next instruction panel seamlessly
                            using (var instructionsDialogue = new AdvancedNetworkingDialogue())
                            {
                                if (instructionsDialogue.ShowDialog(this) == DialogResult.OK && instructionsDialogue.InstructionsAccepted)
                                {
                                    // Both barriers successfully cleared: commit permissions rule directly to disk
                                    AppSettings.Instance.HasAcceptedNetworkRisks = true;
                                    AppSettings.Instance.SaveSettingsAsync().GetAwaiter().GetResult();

                                    _prevConfigTabIndex = CfgTabControl.SelectedIndex;
                                    InitializeNetworkControl();
                                }
                                else
                                {
                                    // Bounced right out of the instruction window
                                    ExecuteBounceBackMacro();
                                    return;
                                }
                            }
                        }
                        else
                        {
                            // Bounced right out of the primary warning window
                            ExecuteBounceBackMacro();
                            return;
                        }
                    }
                }
                else
                {
                    _prevConfigTabIndex = CfgTabControl.SelectedIndex;
                    InitializeNetworkControl();
                }
            }

            // 3. Intercept save/close macro request channels
            else if (CfgTabControl.SelectedTab == SaveTab)
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

        /// <summary>
        /// FIXED: Safely wakes up your custom whitelist manager component and feeds it the resolved base folder path.
        /// </summary>
        private void InitializeWhitelistControl()
        {
            if (_isWhitelistLoaded || string.IsNullOrEmpty(_serverRootPath)) return;

            // NOTE: Replace 'whitelistControl1' with the exact name of the component inside your WinForms Designer layout
            if (whitelistControl1 != null)
            {
                whitelistControl1.InitializeControl(_serverRootPath);
                _isWhitelistLoaded = true;
            }
        }

        /// <summary>
        /// Wakes up your custom permission manager component and feeds it the resolved base folder path.
        /// </summary>
        private void InitializeOperatorsControl()
        {
            if (_isOperatorsLoaded || string.IsNullOrEmpty(_serverRootPath)) return;

            // FIXED: Pointing directly to your clean designer component control instance name
            if (permissionsControl1 != null)
            {
                permissionsControl1.InitializeControl(_serverRootPath);
                _isOperatorsLoaded = true;
            }
        }

        /// <summary>
        /// Wakes up your custom network manager component and feeds it the resolved base folder path.
        /// </summary>
        /// <summary>
        /// Wakes up your custom network manager component and feeds it the resolved base folder path.
        /// </summary>
        private void InitializeNetworkControl()
        {
            if (_isNetworkLoaded || string.IsNullOrEmpty(_serverRootPath)) return;

            // FIXED: Pointing directly to your authentic designer name mapping
            if (networkingControl1 != null)
            {
                networkingControl1.InitializeControl(_serverRootPath);
                _isNetworkLoaded = true;
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

                if (_isWhitelistLoaded && whitelistControl1 != null)
                {
                    whitelistControl1.SaveConfig();
                }

                // COMMIT OPERATOR MUTATIONS TO PERMISSIONS.JSON ON MODAL CLOSE
                if (_isOperatorsLoaded && permissionsControl1 != null)
                {
                    permissionsControl1.SaveConfig();
                }

                // FIXED: Automatically saves modified network properties straight into server.properties on window close
                if (_isNetworkLoaded && networkingControl1 != null)
                {
                    networkingControl1.SaveConfig();
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
        /// <summary>
        /// Programmatic timer macro to bounce the selected tab index backward without causing visual drawing lockouts.
        /// </summary>
        private void ExecuteBounceBackMacro()
        {
            int snapshotIndex = _prevConfigTabIndex;
            Timer bounceTimer = new Timer { Interval = 1 };
            bounceTimer.Tick += (s, args) =>
            {
                bounceTimer.Stop();
                CfgTabControl.SelectedIndex = snapshotIndex;
                CfgTabControl.Update();
                bounceTimer.Dispose();
            };
            bounceTimer.Start();
        }
    }
}
