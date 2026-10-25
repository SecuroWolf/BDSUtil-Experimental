using System;
using System.Drawing;
using System.Windows.Forms;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Controls;

namespace BDSUtil.Modals
{
    public partial class AdvancedNetworkingDialogue : MaterialForm
    {
        public bool InstructionsAccepted { get; private set; } = false;

        public AdvancedNetworkingDialogue()
        {
            this.Width = 620;
            this.Height = 520;
            this.Text = "Advanced Networking Instructions";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;

            RichTextBox infoBox = new RichTextBox()
            {
                Left = 20,
                Top = 80,
                Width = 580,
                Height = 350,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.FromArgb(50, 50, 50), // Material Dark Surface Accent
                ForeColor = Color.FromArgb(222, 255, 255, 255),
                Font = new Font("Roboto", 9.5f, FontStyle.Regular)
            };

            // Section 1 Header
            infoBox.SelectionFont = new Font("Roboto", 11f, FontStyle.Bold);
            infoBox.SelectionColor = Color.FromArgb(0, 150, 136); // Material Teal Accent
            infoBox.AppendText("1. LOCAL ROUTING BASICS\n");
            infoBox.SelectionFont = new Font("Roboto", 9.5f, FontStyle.Regular);
            infoBox.SelectionColor = Color.FromArgb(178, 255, 255, 255);
            infoBox.AppendText("For closed Local Area Network (LAN) deployment profiles, complex multi-port routing rules are completely unnecessary. The underlying server binary automatically maps local interfaces flawlessly to handle same-network client discoveries. Advanced network configurations are strictly required for opening edge routing bounds to external internet traffic layers.\n\n");

            // Section 2 Header
            infoBox.SelectionFont = new Font("Roboto", 11f, FontStyle.Bold);
            infoBox.SelectionColor = Color.FromArgb(244, 67, 54); // Red Alert Accent
            infoBox.AppendText("2. TRANSPORT ARCHITECTURE SHIFTS & THE NETHERNET BUG\n");
            infoBox.SelectionFont = new Font("Roboto", 9.5f, FontStyle.Regular);
            infoBox.SelectionColor = Color.FromArgb(178, 255, 255, 255);
            infoBox.AppendText("In recent Bedrock Dedicated Server builds, Mojang has transitioned core client data streams from RakNet over to the modern Nethernet protocol layer. This architectural migration introduces a major, persistent bug affecting how server traffic handles inbound routing parameters:\n\n");
            infoBox.AppendText(" • BLOCK BOUNDARY FAILURE: Standard syntax arguments allocating integer port blocks (e.g., 19132-19142) or ranged external maps cause the server to fail silently, resulting in an immediate connection refusal error for all external players.\n");
            infoBox.AppendText(" • LISTING COLLISION: Attempting to list individual integers sequentially also results in a total external connection blackout.\n");
            infoBox.AppendText(" • ARCHITECTURAL WORKAROUND: To maintain an active connection gate, the server configuration must be populated with an explicit array of fully articulated, comma-separated internal/external map nodes specifying your public IP targets directly:\n   ");

            // Format Code Example Block Text
            // FIXED: Swapped out private network values for a clean, generic architectural syntax placeholder
            infoBox.SelectionFont = new Font("Consolas", 9.5f, FontStyle.Bold);
            infoBox.SelectionColor = Color.FromArgb(255, 235, 59); // Yellow highlights
            infoBox.AppendText("server-udp-ports=[YourPublicIP]:19134:19134,[YourPublicIP]:19135:19135...\n\n");


            // Section 3 Header
            infoBox.SelectionFont = new Font("Roboto", 11f, FontStyle.Bold);
            infoBox.SelectionColor = Color.FromArgb(255, 152, 0); // Orange sub-header
            infoBox.AppendText("3. INTEGRATED COMPLEMENTARY STACK SOLVERS\n");
            infoBox.SelectionFont = new Font("Roboto", 9.5f, FontStyle.Regular);
            infoBox.SelectionColor = Color.FromArgb(178, 255, 255, 255);
            infoBox.AppendText("To completely eliminate the tedious work of manually typing out these configurations, BDSUtil includes an automated, programmatic network integration stack:\n\n");
            infoBox.AppendText(" • DYNAMIC AUTOMATION (UPnP): When activated, the manager program hooks directly into your edge gateway's Universal Plug and Play framework. It automatically calculates and requests clean, temporary port translations on your local router, dynamically modifying the server configurations on the fly based on your specified Max Player capacity.\n");
            infoBox.AppendText(" • SECURITY OVERVIEW: UPnP execution is disabled by default to protect network security. Manual port configuration inside your hardware router's dashboard remains the recommended deployment strategy because it prevents background reconfigurations.\n\n");

            // Section 4 Header
            infoBox.SelectionFont = new Font("Roboto", 11f, FontStyle.Bold);
            infoBox.SelectionColor = Color.FromArgb(255, 152, 0);
            infoBox.AppendText("4. CAPACITY AND PLAYER CONSTRAINTS\n");
            infoBox.SelectionFont = new Font("Roboto", 9.5f, FontStyle.Regular);
            infoBox.SelectionColor = Color.FromArgb(178, 255, 255, 255);
            infoBox.AppendText("By default, BDSUtil maps an initial 10-port block sequence starting from 19134 through 19143. This allocation strictly supports up to 10 active external concurrent player profiles. You must scale the available port allocation array to precisely match your intended Max Player cap. If the concurrent user traffic exceeds the number of individual port maps declared inside your configuration file, the Nethernet engine will instantly drop any subsequent inbound client connection handshakes.\n\n");

            infoBox.SelectionFont = new Font("Roboto", 10f, FontStyle.Italic);
            infoBox.SelectionColor = Color.FromArgb(255, 255, 255);
            infoBox.AppendText("Please accept these structural constraints to open the Networking configuration editor canvas panel.");

            // Material Button Controls
            MaterialButton btnAccept = new MaterialButton()
            {
                Text = "Accept and Continue",
                Left = 20,
                Top = 455,
                Width = 220,
                Height = 36,
                Type = MaterialButton.MaterialButtonType.Contained,
                UseAccentColor = false
            };

            MaterialButton btnCancel = new MaterialButton()
            {
                Text = "Cancel",
                Left = 380,
                Top = 455,
                Width = 220,
                Height = 36,
                Type = MaterialButton.MaterialButtonType.Outlined,
                UseAccentColor = false
            };

            btnAccept.Click += (s, e) => { this.InstructionsAccepted = true; this.DialogResult = DialogResult.OK; this.Close(); };
            btnCancel.Click += (s, e) => { this.InstructionsAccepted = false; this.DialogResult = DialogResult.Cancel; this.Close(); };

            this.Controls.Add(infoBox);
            this.Controls.Add(btnAccept);
            this.Controls.Add(btnCancel);

            // FIXED: Deselects the active cursor node block to prevent the window from autoscrolling to the bottom!
            infoBox.SelectionLength = 0;
            infoBox.SelectionStart = 0;

            // Forces initial system window focal control down onto the Cancel button node instead
            this.ActiveControl = btnCancel;
        }
    }
}
