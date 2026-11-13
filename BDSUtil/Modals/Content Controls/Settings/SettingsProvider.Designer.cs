using BDSUtil.UI;
using Material3.WinForms.Controls;
using Material3.WinForms.Theming;
using Material3.WinForms.Typography;

namespace BDSUtil.Modals.Content_Controls.Settings
{
    partial class SettingsProvider
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            materialCard1 = new MaterialCard();
            AMConLBL = new MaterialLabel();
            materialCard4 = new MaterialCard();
            CfgWorldMod = new MaterialSwitch();
            WorldModLBL = new MaterialLabel();
            materialCard3 = new MaterialCard();
            EAMLBL = new MaterialLabel();
            ServerPathTextBox = new MaterialTextBoxEdit();
            BrowseBTN = new MaterialFab();
            materialLabel2 = new MaterialLabel();
            CompressionTypeComboBox = new MaterialComboBox();
            materialCard2 = new MaterialCard();
            CfgHTel = new MaterialSwitch();
            TelHideLBL = new MaterialLabel();
            materialCard5 = new MaterialCard();
            RotateApiKeyBTN = new MaterialButton();
            CopyApiKeyBTN = new MaterialButton();
            RemoteApiStatusLBL = new MaterialLabel();
            CfgRemoteApi = new MaterialSwitch();
            RemoteApiLBL = new MaterialLabel();
            NetworkingSectionCard = new MaterialCard();
            NetworkingSectionLBL = new MaterialLabel();
            UpnpSettingsCard = new MaterialCard();
            CfgUPnP = new MaterialSwitch();
            UpnpLBL = new MaterialLabel();
            ConsolidateUdpCard = new MaterialCard();
            CfgConsolidateUdp = new MaterialSwitch();
            ConsolidateUdpLBL = new MaterialLabel();
            ExternalPortCheckerCard = new MaterialCard();
            CustomPortCheckerHintLBL = new MaterialLabel();
            CustomPortCheckerTextBox = new MaterialTextBoxEdit();
            PortCheckerProviderComboBox = new MaterialComboBox();
            ExternalPortCheckerLBL = new MaterialLabel();
            materialCard1.SuspendLayout();
            materialCard4.SuspendLayout();
            materialCard3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)CompressionTypeComboBox).BeginInit();
            materialCard2.SuspendLayout();
            materialCard5.SuspendLayout();
            NetworkingSectionCard.SuspendLayout();
            UpnpSettingsCard.SuspendLayout();
            ConsolidateUdpCard.SuspendLayout();
            ExternalPortCheckerCard.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PortCheckerProviderComboBox).BeginInit();
            SuspendLayout();
            // 
            // materialCard1
            // 
            materialCard1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialCard1.BackColor = System.Drawing.Color.FromArgb(29, 37, 41);
            materialCard1.Controls.Add(AMConLBL);
            materialCard1.Location = new System.Drawing.Point(14, 14);
            materialCard1.Margin = new System.Windows.Forms.Padding(14);
            materialCard1.Name = "materialCard1";
            materialCard1.Padding = new System.Windows.Forms.Padding(14);
            materialCard1.Size = new System.Drawing.Size(859, 51);
            materialCard1.TabIndex = 11;
            materialCard1.Variant = MaterialCardVariant.Elevated;
            // 
            // AMConLBL
            // 
            AMConLBL.BackColor = System.Drawing.Color.Transparent;
            AMConLBL.Dock = System.Windows.Forms.DockStyle.Fill;
            AMConLBL.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            AMConLBL.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            AMConLBL.Location = new System.Drawing.Point(14, 14);
            AMConLBL.Name = "AMConLBL";
            AMConLBL.Size = new System.Drawing.Size(831, 23);
            AMConLBL.TabIndex = 0;
            AMConLBL.Text = "Global App Settings";
            AMConLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialCard4
            // 
            materialCard4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialCard4.BackColor = System.Drawing.Color.FromArgb(29, 37, 41);
            materialCard4.Controls.Add(CfgWorldMod);
            materialCard4.Controls.Add(WorldModLBL);
            materialCard4.Location = new System.Drawing.Point(14, 222);
            materialCard4.Margin = new System.Windows.Forms.Padding(14);
            materialCard4.Name = "materialCard4";
            materialCard4.Padding = new System.Windows.Forms.Padding(14);
            materialCard4.Size = new System.Drawing.Size(859, 75);
            materialCard4.TabIndex = 13;
            materialCard4.Variant = MaterialCardVariant.Elevated;
            // 
            // CfgWorldMod
            // 
            CfgWorldMod.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CfgWorldMod.AutoSize = false;
            CfgWorldMod.BackColor = System.Drawing.Color.Transparent;
            CfgWorldMod.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            CfgWorldMod.Location = new System.Drawing.Point(781, 19);
            CfgWorldMod.Margin = new System.Windows.Forms.Padding(0);
            CfgWorldMod.Name = "CfgWorldMod";
            CfgWorldMod.Size = new System.Drawing.Size(64, 38);
            CfgWorldMod.TabIndex = 14;
            // 
            // WorldModLBL
            // 
            WorldModLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            WorldModLBL.BackColor = System.Drawing.Color.Transparent;
            WorldModLBL.Font = new System.Drawing.Font("Segoe UI", 12F);
            WorldModLBL.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            WorldModLBL.Location = new System.Drawing.Point(14, 14);
            WorldModLBL.Name = "WorldModLBL";
            WorldModLBL.Size = new System.Drawing.Size(750, 48);
            WorldModLBL.TabIndex = 13;
            WorldModLBL.Text = "Enable World Modification\r\n(level.dat editing is very dangerous)";
            WorldModLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialCard3
            // 
            materialCard3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialCard3.BackColor = System.Drawing.Color.FromArgb(29, 37, 41);
            materialCard3.Controls.Add(EAMLBL);
            materialCard3.Controls.Add(ServerPathTextBox);
            materialCard3.Controls.Add(BrowseBTN);
            materialCard3.Controls.Add(materialLabel2);
            materialCard3.Controls.Add(CompressionTypeComboBox);
            materialCard3.Location = new System.Drawing.Point(14, 79);
            materialCard3.Margin = new System.Windows.Forms.Padding(14);
            materialCard3.Name = "materialCard3";
            materialCard3.Padding = new System.Windows.Forms.Padding(14);
            materialCard3.Size = new System.Drawing.Size(859, 150);
            materialCard3.TabIndex = 12;
            materialCard3.Variant = MaterialCardVariant.Elevated;
            // 
            // EAMLBL
            // 
            EAMLBL.BackColor = System.Drawing.Color.Transparent;
            EAMLBL.Font = new System.Drawing.Font("Segoe UI", 12F);
            EAMLBL.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            EAMLBL.Location = new System.Drawing.Point(14, 14);
            EAMLBL.Name = "EAMLBL";
            EAMLBL.Size = new System.Drawing.Size(280, 48);
            EAMLBL.TabIndex = 13;
            EAMLBL.Text = "Server Directory\r\n(Where bedrock_server.exe is located)";
            EAMLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ServerPathTextBox
            // 
            ServerPathTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ServerPathTextBox.BackColor = System.Drawing.Color.Transparent;
            ServerPathTextBox.Hint = "Server Directory";
            ServerPathTextBox.Location = new System.Drawing.Point(301, 14);
            ServerPathTextBox.Name = "ServerPathTextBox";
            ServerPathTextBox.ReadOnly = true;
            ServerPathTextBox.Size = new System.Drawing.Size(476, 48);
            ServerPathTextBox.TabIndex = 14;
            ServerPathTextBox.TabStop = false;
            // 
            // BrowseBTN
            // 
            BrowseBTN.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BrowseBTN.AutoSize = false;
            BrowseBTN.BackColor = System.Drawing.Color.Transparent;
            BrowseBTN.ElevationLevel = 1;
            BrowseBTN.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            BrowseBTN.IconImage = Properties.Resources.folder_open_24dp;
            BrowseBTN.IconSize = 28;
            BrowseBTN.Location = new System.Drawing.Point(789, 10);
            BrowseBTN.Name = "BrowseBTN";
            BrowseBTN.Size = new System.Drawing.Size(56, 56);
            BrowseBTN.TabIndex = 15;
            BrowseBTN.Click += BrowseBTN_Click;
            // 
            // materialLabel2
            // 
            materialLabel2.BackColor = System.Drawing.Color.Transparent;
            materialLabel2.Font = new System.Drawing.Font("Segoe UI", 12F);
            materialLabel2.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            materialLabel2.Location = new System.Drawing.Point(14, 82);
            materialLabel2.Name = "materialLabel2";
            materialLabel2.Size = new System.Drawing.Size(372, 48);
            materialLabel2.TabIndex = 17;
            materialLabel2.Text = "Compression Format\r\n(WinRAR disabled if not installed)";
            materialLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CompressionTypeComboBox
            // 
            CompressionTypeComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CompressionTypeComboBox.BackColor = System.Drawing.Color.Transparent;
            CompressionTypeComboBox.Hint = "Default Backup Compression Format";
            CompressionTypeComboBox.Items.AddRange(new object[] { "WinRAR (.rar)", "WinRAR (.zip) [Optimized High-Speed Zip]", "Windows Native (.zip)" });
            CompressionTypeComboBox.Location = new System.Drawing.Point(395, 82);
            CompressionTypeComboBox.Name = "CompressionTypeComboBox";
            CompressionTypeComboBox.Size = new System.Drawing.Size(448, 49);
            CompressionTypeComboBox.TabIndex = 18;
            // 
            // materialCard2
            // 
            materialCard2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialCard2.BackColor = System.Drawing.Color.FromArgb(29, 37, 41);
            materialCard2.Controls.Add(CfgHTel);
            materialCard2.Controls.Add(TelHideLBL);
            materialCard2.Location = new System.Drawing.Point(14, 305);
            materialCard2.Margin = new System.Windows.Forms.Padding(14, 200, 14, 14);
            materialCard2.Name = "materialCard2";
            materialCard2.Padding = new System.Windows.Forms.Padding(14);
            materialCard2.Size = new System.Drawing.Size(859, 75);
            materialCard2.TabIndex = 14;
            materialCard2.Variant = MaterialCardVariant.Elevated;
            // 
            // CfgHTel
            // 
            CfgHTel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CfgHTel.AutoSize = false;
            CfgHTel.BackColor = System.Drawing.Color.Transparent;
            CfgHTel.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            CfgHTel.Location = new System.Drawing.Point(781, 19);
            CfgHTel.Margin = new System.Windows.Forms.Padding(0);
            CfgHTel.Name = "CfgHTel";
            CfgHTel.Size = new System.Drawing.Size(64, 38);
            CfgHTel.TabIndex = 14;
            CfgHTel.CheckedChanged += CfgHTel_CheckedChanged;
            // 
            // TelHideLBL
            // 
            TelHideLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            TelHideLBL.BackColor = System.Drawing.Color.Transparent;
            TelHideLBL.Font = new System.Drawing.Font("Segoe UI", 12F);
            TelHideLBL.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            TelHideLBL.Location = new System.Drawing.Point(14, 14);
            TelHideLBL.Name = "TelHideLBL";
            TelHideLBL.Size = new System.Drawing.Size(750, 48);
            TelHideLBL.TabIndex = 13;
            TelHideLBL.Text = "Hide \"Telemetry not enabled\" messages in the console\r\n(Privacy: Hides the message/warning without enabling Telemetry)";
            TelHideLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialCard5
            // 
            materialCard5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialCard5.BackColor = System.Drawing.Color.FromArgb(29, 37, 41);
            materialCard5.Controls.Add(RotateApiKeyBTN);
            materialCard5.Controls.Add(CopyApiKeyBTN);
            materialCard5.Controls.Add(RemoteApiStatusLBL);
            materialCard5.Controls.Add(CfgRemoteApi);
            materialCard5.Controls.Add(RemoteApiLBL);
            materialCard5.Location = new System.Drawing.Point(14, 621);
            materialCard5.Margin = new System.Windows.Forms.Padding(14);
            materialCard5.Name = "materialCard5";
            materialCard5.Padding = new System.Windows.Forms.Padding(14);
            materialCard5.Size = new System.Drawing.Size(859, 126);
            materialCard5.TabIndex = 19;
            materialCard5.Variant = MaterialCardVariant.Elevated;
            // 
            // RotateApiKeyBTN
            // 
            RotateApiKeyBTN.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            RotateApiKeyBTN.BackColor = System.Drawing.Color.Transparent;
            RotateApiKeyBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            RotateApiKeyBTN.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            RotateApiKeyBTN.ForeColor = System.Drawing.Color.FromArgb(101, 211, 255);
            RotateApiKeyBTN.Location = new System.Drawing.Point(708, 72);
            RotateApiKeyBTN.Name = "RotateApiKeyBTN";
            RotateApiKeyBTN.Size = new System.Drawing.Size(132, 36);
            RotateApiKeyBTN.TabIndex = 18;
            RotateApiKeyBTN.Text = "Rotate Key";
            RotateApiKeyBTN.UseVisualStyleBackColor = false;
            RotateApiKeyBTN.Variant = MaterialButtonVariant.Outlined;
            RotateApiKeyBTN.Click += RotateApiKeyBTN_Click;
            // 
            // CopyApiKeyBTN
            // 
            CopyApiKeyBTN.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CopyApiKeyBTN.BackColor = System.Drawing.Color.Transparent;
            CopyApiKeyBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            CopyApiKeyBTN.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            CopyApiKeyBTN.ForeColor = System.Drawing.Color.FromArgb(101, 211, 255);
            CopyApiKeyBTN.Location = new System.Drawing.Point(568, 72);
            CopyApiKeyBTN.Name = "CopyApiKeyBTN";
            CopyApiKeyBTN.Size = new System.Drawing.Size(130, 36);
            CopyApiKeyBTN.TabIndex = 17;
            CopyApiKeyBTN.Text = "Copy Key";
            CopyApiKeyBTN.UseVisualStyleBackColor = false;
            CopyApiKeyBTN.Variant = MaterialButtonVariant.Outlined;
            CopyApiKeyBTN.Click += CopyApiKeyBTN_Click;
            // 
            // RemoteApiStatusLBL
            // 
            RemoteApiStatusLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            RemoteApiStatusLBL.BackColor = System.Drawing.Color.Transparent;
            RemoteApiStatusLBL.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            RemoteApiStatusLBL.ForeColor = System.Drawing.Color.FromArgb(192, 200, 205);
            RemoteApiStatusLBL.Location = new System.Drawing.Point(14, 68);
            RemoteApiStatusLBL.Name = "RemoteApiStatusLBL";
            RemoteApiStatusLBL.Size = new System.Drawing.Size(540, 44);
            RemoteApiStatusLBL.TabIndex = 16;
            RemoteApiStatusLBL.Text = "Status: Disabled (Listener stopped; no UPnP port requisitioned)";
            RemoteApiStatusLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CfgRemoteApi
            // 
            CfgRemoteApi.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CfgRemoteApi.AutoSize = false;
            CfgRemoteApi.BackColor = System.Drawing.Color.Transparent;
            CfgRemoteApi.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            CfgRemoteApi.Location = new System.Drawing.Point(781, 18);
            CfgRemoteApi.Margin = new System.Windows.Forms.Padding(0);
            CfgRemoteApi.Name = "CfgRemoteApi";
            CfgRemoteApi.Size = new System.Drawing.Size(64, 38);
            CfgRemoteApi.TabIndex = 15;
            CfgRemoteApi.CheckedChanged += CfgRemoteApi_CheckedChanged;
            // 
            // RemoteApiLBL
            // 
            RemoteApiLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            RemoteApiLBL.BackColor = System.Drawing.Color.Transparent;
            RemoteApiLBL.Font = new System.Drawing.Font("Segoe UI", 12F);
            RemoteApiLBL.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            RemoteApiLBL.Location = new System.Drawing.Point(14, 14);
            RemoteApiLBL.Name = "RemoteApiLBL";
            RemoteApiLBL.Size = new System.Drawing.Size(750, 44);
            RemoteApiLBL.TabIndex = 13;
            RemoteApiLBL.Text = "Enable Remote Management API\r\n(Exposes authenticated HTTP/JSON control API below lowest server port; uses UPnP when enabled)";
            RemoteApiLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NetworkingSectionCard
            // 
            NetworkingSectionCard.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            NetworkingSectionCard.BackColor = System.Drawing.Color.FromArgb(29, 37, 41);
            NetworkingSectionCard.Controls.Add(NetworkingSectionLBL);
            NetworkingSectionCard.Location = new System.Drawing.Point(14, 396);
            NetworkingSectionCard.Margin = new System.Windows.Forms.Padding(14);
            NetworkingSectionCard.Name = "NetworkingSectionCard";
            NetworkingSectionCard.Padding = new System.Windows.Forms.Padding(14);
            NetworkingSectionCard.Size = new System.Drawing.Size(859, 51);
            NetworkingSectionCard.TabIndex = 15;
            NetworkingSectionCard.Variant = MaterialCardVariant.Elevated;
            // 
            // NetworkingSectionLBL
            // 
            NetworkingSectionLBL.BackColor = System.Drawing.Color.Transparent;
            NetworkingSectionLBL.Dock = System.Windows.Forms.DockStyle.Fill;
            NetworkingSectionLBL.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            NetworkingSectionLBL.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            NetworkingSectionLBL.Location = new System.Drawing.Point(14, 14);
            NetworkingSectionLBL.Name = "NetworkingSectionLBL";
            NetworkingSectionLBL.Size = new System.Drawing.Size(831, 23);
            NetworkingSectionLBL.TabIndex = 0;
            NetworkingSectionLBL.Text = "Networking Settings";
            NetworkingSectionLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UpnpSettingsCard
            // 
            UpnpSettingsCard.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            UpnpSettingsCard.BackColor = System.Drawing.Color.FromArgb(29, 37, 41);
            UpnpSettingsCard.Controls.Add(CfgUPnP);
            UpnpSettingsCard.Controls.Add(UpnpLBL);
            UpnpSettingsCard.Location = new System.Drawing.Point(14, 455);
            UpnpSettingsCard.Margin = new System.Windows.Forms.Padding(14);
            UpnpSettingsCard.Name = "UpnpSettingsCard";
            UpnpSettingsCard.Padding = new System.Windows.Forms.Padding(14);
            UpnpSettingsCard.Size = new System.Drawing.Size(859, 75);
            UpnpSettingsCard.TabIndex = 16;
            UpnpSettingsCard.Variant = MaterialCardVariant.Elevated;
            // 
            // CfgUPnP
            // 
            CfgUPnP.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CfgUPnP.AutoSize = false;
            CfgUPnP.BackColor = System.Drawing.Color.Transparent;
            CfgUPnP.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            CfgUPnP.Location = new System.Drawing.Point(781, 19);
            CfgUPnP.Margin = new System.Windows.Forms.Padding(0);
            CfgUPnP.Name = "CfgUPnP";
            CfgUPnP.Size = new System.Drawing.Size(64, 38);
            CfgUPnP.TabIndex = 14;
            CfgUPnP.CheckedChanged += CfgUPnP_CheckedChanged;
            // 
            // UpnpLBL
            // 
            UpnpLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            UpnpLBL.BackColor = System.Drawing.Color.Transparent;
            UpnpLBL.Font = new System.Drawing.Font("Segoe UI", 12F);
            UpnpLBL.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            UpnpLBL.Location = new System.Drawing.Point(14, 14);
            UpnpLBL.Name = "UpnpLBL";
            UpnpLBL.Size = new System.Drawing.Size(750, 48);
            UpnpLBL.TabIndex = 13;
            UpnpLBL.Text = "Enable UPnP\r\n(Automatically forwards server-port, server-udp-ports, and Remote API ports when enabled)";
            UpnpLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ConsolidateUdpCard
            // 
            ConsolidateUdpCard.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ConsolidateUdpCard.BackColor = System.Drawing.Color.FromArgb(29, 37, 41);
            ConsolidateUdpCard.Controls.Add(CfgConsolidateUdp);
            ConsolidateUdpCard.Controls.Add(ConsolidateUdpLBL);
            ConsolidateUdpCard.Location = new System.Drawing.Point(14, 538);
            ConsolidateUdpCard.Margin = new System.Windows.Forms.Padding(14);
            ConsolidateUdpCard.Name = "ConsolidateUdpCard";
            ConsolidateUdpCard.Padding = new System.Windows.Forms.Padding(14);
            ConsolidateUdpCard.Size = new System.Drawing.Size(859, 88);
            ConsolidateUdpCard.TabIndex = 18;
            ConsolidateUdpCard.Variant = MaterialCardVariant.Elevated;
            // 
            // CfgConsolidateUdp
            // 
            CfgConsolidateUdp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            CfgConsolidateUdp.AutoSize = false;
            CfgConsolidateUdp.BackColor = System.Drawing.Color.Transparent;
            CfgConsolidateUdp.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            CfgConsolidateUdp.Location = new System.Drawing.Point(781, 25);
            CfgConsolidateUdp.Margin = new System.Windows.Forms.Padding(0);
            CfgConsolidateUdp.Name = "CfgConsolidateUdp";
            CfgConsolidateUdp.Size = new System.Drawing.Size(64, 38);
            CfgConsolidateUdp.TabIndex = 14;
            CfgConsolidateUdp.CheckedChanged += CfgConsolidateUdp_CheckedChanged;
            // 
            // ConsolidateUdpLBL
            // 
            ConsolidateUdpLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ConsolidateUdpLBL.BackColor = System.Drawing.Color.Transparent;
            ConsolidateUdpLBL.Font = new System.Drawing.Font("Segoe UI", 12F);
            ConsolidateUdpLBL.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            ConsolidateUdpLBL.Location = new System.Drawing.Point(14, 10);
            ConsolidateUdpLBL.Name = "ConsolidateUdpLBL";
            ConsolidateUdpLBL.Size = new System.Drawing.Size(750, 68);
            ConsolidateUdpLBL.TabIndex = 13;
            ConsolidateUdpLBL.Text = "[BETA] Consolidate UDP Ports\r\n(Ignore max player count and assign only a single extra UDP port above server-port for server-udp-ports)";
            ConsolidateUdpLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ExternalPortCheckerCard
            // 
            ExternalPortCheckerCard.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ExternalPortCheckerCard.BackColor = System.Drawing.Color.FromArgb(29, 37, 41);
            ExternalPortCheckerCard.Controls.Add(CustomPortCheckerHintLBL);
            ExternalPortCheckerCard.Controls.Add(CustomPortCheckerTextBox);
            ExternalPortCheckerCard.Controls.Add(PortCheckerProviderComboBox);
            ExternalPortCheckerCard.Controls.Add(ExternalPortCheckerLBL);
            ExternalPortCheckerCard.Location = new System.Drawing.Point(14, 538);
            ExternalPortCheckerCard.Margin = new System.Windows.Forms.Padding(14);
            ExternalPortCheckerCard.Name = "ExternalPortCheckerCard";
            ExternalPortCheckerCard.Padding = new System.Windows.Forms.Padding(14);
            ExternalPortCheckerCard.Size = new System.Drawing.Size(859, 88);
            ExternalPortCheckerCard.TabIndex = 17;
            ExternalPortCheckerCard.Variant = MaterialCardVariant.Elevated;
            ExternalPortCheckerCard.Visible = false;
            // 
            // CustomPortCheckerHintLBL
            // 
            CustomPortCheckerHintLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CustomPortCheckerHintLBL.BackColor = System.Drawing.Color.Transparent;
            CustomPortCheckerHintLBL.Font = new System.Drawing.Font("Segoe UI", 9F);
            CustomPortCheckerHintLBL.ForeColor = System.Drawing.Color.FromArgb(192, 200, 205);
            CustomPortCheckerHintLBL.Location = new System.Drawing.Point(14, 138);
            CustomPortCheckerHintLBL.Name = "CustomPortCheckerHintLBL";
            CustomPortCheckerHintLBL.Size = new System.Drawing.Size(826, 36);
            CustomPortCheckerHintLBL.TabIndex = 3;
            CustomPortCheckerHintLBL.Text = "Put {port} where the port goes (optional: {ip}, {protocol}). Example: https://ifconfig.co/port/{port}";
            CustomPortCheckerHintLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            CustomPortCheckerHintLBL.Visible = false;
            // 
            // CustomPortCheckerTextBox
            // 
            CustomPortCheckerTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CustomPortCheckerTextBox.BackColor = System.Drawing.Color.Transparent;
            CustomPortCheckerTextBox.Hint = "Custom Port Checker API URL (include {port})";
            CustomPortCheckerTextBox.Location = new System.Drawing.Point(14, 84);
            CustomPortCheckerTextBox.MaxLength = 2048;
            CustomPortCheckerTextBox.Name = "CustomPortCheckerTextBox";
            CustomPortCheckerTextBox.Size = new System.Drawing.Size(826, 48);
            CustomPortCheckerTextBox.TabIndex = 2;
            CustomPortCheckerTextBox.Visible = false;
            // 
            // PortCheckerProviderComboBox
            // 
            PortCheckerProviderComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PortCheckerProviderComboBox.BackColor = System.Drawing.Color.Transparent;
            PortCheckerProviderComboBox.Hint = "External Port Checker API";
            PortCheckerProviderComboBox.Location = new System.Drawing.Point(535, 19);
            PortCheckerProviderComboBox.Name = "PortCheckerProviderComboBox";
            PortCheckerProviderComboBox.Size = new System.Drawing.Size(310, 49);
            PortCheckerProviderComboBox.TabIndex = 1;
            // 
            // ExternalPortCheckerLBL
            // 
            ExternalPortCheckerLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ExternalPortCheckerLBL.BackColor = System.Drawing.Color.Transparent;
            ExternalPortCheckerLBL.Font = new System.Drawing.Font("Segoe UI", 12F);
            ExternalPortCheckerLBL.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            ExternalPortCheckerLBL.Location = new System.Drawing.Point(14, 10);
            ExternalPortCheckerLBL.Name = "ExternalPortCheckerLBL";
            ExternalPortCheckerLBL.Size = new System.Drawing.Size(509, 68);
            ExternalPortCheckerLBL.TabIndex = 0;
            ExternalPortCheckerLBL.Text = "External Port Checker API\r\n(Optional WAN TCP check; Off by default for privacy)";
            ExternalPortCheckerLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SettingsProvider
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            AutoScroll = true;
            AutoScrollMinSize = new System.Drawing.Size(0, 761);
            BackColor = System.Drawing.Color.FromArgb(17, 20, 21);
            Controls.Add(materialCard5);
            Controls.Add(ConsolidateUdpCard);
            Controls.Add(ExternalPortCheckerCard);
            Controls.Add(UpnpSettingsCard);
            Controls.Add(NetworkingSectionCard);
            Controls.Add(materialCard2);
            Controls.Add(materialCard4);
            Controls.Add(materialCard3);
            Controls.Add(materialCard1);
            Name = "SettingsProvider";
            Size = new System.Drawing.Size(888, 765);
            materialCard1.ResumeLayout(false);
            materialCard4.ResumeLayout(false);
            materialCard3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)CompressionTypeComboBox).EndInit();
            materialCard2.ResumeLayout(false);
            materialCard5.ResumeLayout(false);
            NetworkingSectionCard.ResumeLayout(false);
            UpnpSettingsCard.ResumeLayout(false);
            ConsolidateUdpCard.ResumeLayout(false);
            ExternalPortCheckerCard.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)PortCheckerProviderComboBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private MaterialCard materialCard1;
        private MaterialLabel AMConLBL;
        private MaterialCard materialCard4;
        private MaterialSwitch CfgWorldMod;
        private MaterialLabel WorldModLBL;
        private MaterialCard materialCard3;
        private BDSUtil.UI.MaterialComboBox CompressionTypeComboBox;
        private MaterialLabel materialLabel2;
        private MaterialFab BrowseBTN;
        private BDSUtil.UI.MaterialTextBoxEdit ServerPathTextBox;
        private MaterialLabel EAMLBL;
        private MaterialCard materialCard2;
        private MaterialSwitch CfgHTel;
        private MaterialLabel TelHideLBL;
        private MaterialCard materialCard5;
        private MaterialSwitch CfgRemoteApi;
        private MaterialLabel RemoteApiLBL;
        private MaterialLabel RemoteApiStatusLBL;
        private MaterialButton CopyApiKeyBTN;
        private MaterialButton RotateApiKeyBTN;
        private MaterialCard NetworkingSectionCard;
        private MaterialLabel NetworkingSectionLBL;
        private MaterialCard UpnpSettingsCard;
        private MaterialSwitch CfgUPnP;
        private MaterialLabel UpnpLBL;
        private MaterialCard ConsolidateUdpCard;
        private MaterialSwitch CfgConsolidateUdp;
        private MaterialLabel ConsolidateUdpLBL;
        private MaterialCard ExternalPortCheckerCard;
        private MaterialLabel ExternalPortCheckerLBL;
        private BDSUtil.UI.MaterialComboBox PortCheckerProviderComboBox;
        private BDSUtil.UI.MaterialTextBoxEdit CustomPortCheckerTextBox;
        private MaterialLabel CustomPortCheckerHintLBL;
    }
}
