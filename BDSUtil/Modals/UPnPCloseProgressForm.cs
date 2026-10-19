using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Manager;

namespace BDSUtil.Modals
{
    public partial class UPnPCloseProgressForm : MaterialForm
    {
        private readonly string _serverDirectory;
        private MaterialLabel _statusLabel;

        public UPnPCloseProgressForm(string serverDirectory)
        {
            _serverDirectory = serverDirectory;

            // Explicitly inherit your main application's theme colors right on creation
            var skinManager = MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);

            // Slim visual dimensions optimized for a clean, single-label alert prompt layout
            this.Width = 380;
            this.Height = 140;
            this.Text = "Releasing Ports..."; // FIXED: Clean, concise operation context title
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;
            this.ControlBox = false; // Prevents the user from closing out this modal mid-operation
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;

            _statusLabel = new MaterialLabel()
            {
                Text = "Scrubbing active UPnP leases from hardware...",
                Left = 24,
                Top = 85,
                Width = 330,
                FontType = MaterialSkinManager.FontType.Body2
            };

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

                        if (System.IO.File.Exists(propertiesFilePath))
                        {
                            string[] currentLines = System.IO.File.ReadAllLines(propertiesFilePath);
                            List<string> cleanOutputBuffer = new List<string>();
                            string strictKeyAssignmentPattern = @"^\s*#?\s*server-udp-ports\s*=";

                            foreach (string line in currentLines)
                            {
                                if (System.Text.RegularExpressions.Regex.IsMatch(line, strictKeyAssignmentPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                                {
                                    cleanOutputBuffer.Add("# server-udp-ports=");
                                }
                                else
                                {
                                    cleanOutputBuffer.Add(line);
                                }
                            }

                            System.IO.File.WriteAllLines(propertiesFilePath, cleanOutputBuffer, System.Text.Encoding.UTF8);
                        }
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
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    });
                }
            });
        }
    }
}
