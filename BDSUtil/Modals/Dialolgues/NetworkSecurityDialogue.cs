using System;
using System.Drawing;
using System.Windows.Forms;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Controls;

namespace BDSUtil.Modals
{
    public partial class NetworkSecurityDialogue : MaterialForm
    {
        public bool Confirmed { get; private set; } = false;

        public NetworkSecurityDialogue()
        {
            this.Width = 560;
            this.Height = 460;
            this.Text = "Network Security Advisory";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // Rich TextBox Canvas Setup for the advisory content
            RichTextBox warningBox = new RichTextBox()
            {
                Left = 20,
                Top = 80, // Offset cleanly below the Material window header title bar
                Width = 520,
                Height = 290,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(50, 50, 50), // Standard Material Dark Surface color
                ForeColor = Color.FromArgb(222, 255, 255, 255), // High-contrast text accent
                Font = new Font("Roboto", 10f, FontStyle.Regular)
            };

            // High-Visibility Warning Header Styling Pass
            warningBox.SelectionFont = new Font("Roboto", 11f, FontStyle.Bold);
            warningBox.SelectionColor = Color.FromArgb(244, 67, 54); // Soft Red Alert Color
            warningBox.AppendText("⚠️ SECURITY NOTICE: INBOUND PORT ROUTING EXPOSURE ANALYSIS\n");
            warningBox.AppendText("======================================================================\n\n");

            warningBox.SelectionFont = new Font("Roboto", 10f, FontStyle.Regular);
            warningBox.SelectionColor = Color.FromArgb(178, 255, 255, 255); // Secondary muted text
            warningBox.AppendText("Before initializing network mapping configurations, system administrators must understand the inherent exposure profiles associated with automated and manual inbound traffic routing protocols.\n\n");

            warningBox.SelectionFont = new Font("Roboto", 10f, FontStyle.Bold);
            warningBox.SelectionColor = Color.FromArgb(255, 152, 0); // Material Amber accent
            warningBox.AppendText("1. UNIVERSAL PLUG AND PLAY (UPnP) THREAT PROFILE\n");
            warningBox.SelectionFont = new Font("Roboto", 10f, FontStyle.Regular);
            warningBox.SelectionColor = Color.FromArgb(178, 255, 255, 255);
            warningBox.AppendText("The Universal Plug and Play (UPnP) protocol provides seamless, automated port configuration by allowing BDSUtil to dynamically request temporary inbound holes through your local network gateway's hardware firewall layer. While this configuration eliminates manual routing friction, it introduces significant architectural vulnerabilities:\n\n");
            warningBox.AppendText(" • UNVERIFIED TRUST LIFECYCLE: Standard UPnP implementations lack robust cryptographic validation engines. The gateway blindly trusts local execution handles.\n");
            warningBox.AppendText(" • COVERT CHANNEL RISK: If an adversary gains lateral entry to your local network segment via a secondary vector, they can abuse the open UPnP service to map arbitrary backdoor access channels straight back to their command servers, completely bypassing standard operating system packet filters.\n\n");
            warningBox.AppendText("Regardless of the rigorous internal code limits and validation blocks implemented within BDSUtil to prevent local permission exploitation, the underlying UPnP endpoint itself remains an appealing entry vector if an adversarial trace maps your external routing bounds.\n\n");

            warningBox.SelectionFont = new Font("Roboto", 10f, FontStyle.Bold);
            warningBox.SelectionColor = Color.FromArgb(255, 152, 0);
            warningBox.AppendText("2. MANUAL PORT FORWARDING COMPLEMENTARY STACK\n");
            warningBox.SelectionFont = new Font("Roboto", 10f, FontStyle.Regular);
            warningBox.SelectionColor = Color.FromArgb(178, 255, 255, 255);
            warningBox.AppendText("Manually pinning absolute port translation paths right inside your gateway's security dashboard offers a significantly more secure operational model:\n\n");
            warningBox.AppendText(" • APPLICATION ISOLATION: Manual mappings isolate traffic exclusively to your designated Bedrock Dedicated Server port track (typically UDP 19132/19133), meaning stray applications cannot dynamically open random ports elsewhere on the system.\n");
            warningBox.AppendText(" • RECONNAISSANCE MITIGATION: Adversaries scanning your public IP boundary can still locate and target the open server port directly, but the overall attack surface is vastly smaller because your system's edge gateway remains closed to automated reconfiguration loops.\n\n");
            warningBox.AppendText("Please click 'I understand the risks' to open the advanced networking documentation Dialogue.");

            // Material Button Controls
            MaterialButton btnConfirm = new MaterialButton()
            {
                Text = "I understand the risks",
                Left = 20,
                Top = 390,
                Width = 200,
                Height = 36,
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false
            };

            MaterialButton btnCancel = new MaterialButton()
            {
                Text = "Go Back",
                Left = 340,
                Top = 390,
                Width = 200,
                Height = 36,
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false
            };

            // Wire up event triggers to drive the dialog result callbacks
            btnConfirm.Click += (s, e) => { this.Confirmed = true; this.DialogResult = DialogResult.OK; this.Close(); };
            btnCancel.Click += (s, e) => { this.Confirmed = false; this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.Add(warningBox);
            this.Controls.Add(btnConfirm);
            this.Controls.Add(btnCancel);
        }
    }
}
