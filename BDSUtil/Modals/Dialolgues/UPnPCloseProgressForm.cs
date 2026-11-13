using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using BDSUtil.UI;
using Material3.WinForms.Controls;
using Material3.WinForms.Typography;
using M3MaterialLabel = Material3.WinForms.Controls.MaterialLabel;

namespace BDSUtil.Modals
{
    public partial class UPnPCloseProgressForm : Material3Form
    {
        private readonly string _serverDirectory;
        private readonly M3MaterialLabel _statusLabel;
        private readonly MaterialCircularProgress _spinner;

        public UPnPCloseProgressForm(string serverDirectory)
        {
            _serverDirectory = serverDirectory;

            this.Width = 400;
            this.Height = 132;
            this.Text = "Releasing Ports...";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            SyncTitleBarMetadata();

            _spinner = new MaterialCircularProgress
            {
                Left = 24,
                Top = 64,
                Diameter = 28,
                StrokeWidth = 3,
                IsIndeterminate = true
            };

            _statusLabel = new M3MaterialLabel()
            {
                Text = "Scrubbing active UPnP leases from hardware...",
                Left = 64,
                Top = 67,
                Width = 312,
                Height = 24
            };
            Material3AppTheme.ConfigureLabel(_statusLabel, TypeScale.BodyMedium);

            this.Controls.Add(_spinner);
            this.Controls.Add(_statusLabel);

            this.Shown += UPnPCloseProgressForm_Shown;
        }

        private void UPnPCloseProgressForm_Shown(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                try
                {
                    // PHASE 1: Re-comment out the server-udp-ports entry in server.properties
                    if (!string.IsNullOrWhiteSpace(_serverDirectory))
                    {
                        string propertiesFilePath = System.IO.Path.Combine(_serverDirectory, "server.properties");
                        BDSUtil.Helpers.ServerPropertiesDocument.CommentOutProperty(propertiesFilePath, "server-udp-ports");
                    }

                    // PHASE 2: Terminate active dynamic router leases completely
                    BDSUtil.Helpers.UPnPEngine.TearDownPorts();

                    // VISUAL CUSHION: Short delay to let the user see the system working before the form drops
                    await Task.Delay(600);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[UPnP Modal Teardown Failure]: {ex.Message}");
                }
                finally
                {
                    // Marshall back onto the main UI context thread to close out the form layout bounds
                    if (!IsDisposed && IsHandleCreated)
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        });
                    }
                }
            });
        }
    }
}
