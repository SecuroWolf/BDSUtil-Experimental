namespace BDSUtil.Modals
{
    partial class ServerConfig
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfig));
            _navRail = new Material3.WinForms.Controls.MaterialNavigationRail();
            CfgTabControl = new BDSUtil.UI.MaterialTabControl();
            PropTab = new System.Windows.Forms.TabPage();
            serverPropertiesControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.ServerPropertiesControl();
            WLTab = new System.Windows.Forms.TabPage();
            whitelistControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.WhitelistControl();
            PermTab = new System.Windows.Forms.TabPage();
            permissionsControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.PermissionsControl();
            NetTab = new System.Windows.Forms.TabPage();
            networkingControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.NetworkingControl();
            ExitTab = new System.Windows.Forms.TabPage();
            CfgTabControl.SuspendLayout();
            PropTab.SuspendLayout();
            WLTab.SuspendLayout();
            PermTab.SuspendLayout();
            NetTab.SuspendLayout();
            SuspendLayout();
            // 
            // _navRail
            // 
            _navRail.Dock = System.Windows.Forms.DockStyle.Left;
            _navRail.Name = "_navRail";
            _navRail.RailWidth = 88;
            _navRail.TabIndex = 0;
            // 
            // CfgTabControl
            // 
            CfgTabControl.Controls.Add(PropTab);
            CfgTabControl.Controls.Add(WLTab);
            CfgTabControl.Controls.Add(PermTab);
            CfgTabControl.Controls.Add(NetTab);
            CfgTabControl.Controls.Add(ExitTab);
            CfgTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            CfgTabControl.Name = "CfgTabControl";
            CfgTabControl.SelectedIndex = 0;
            CfgTabControl.TabIndex = 1;
            // 
            // PropTab
            // 
            PropTab.AutoScroll = true;
            PropTab.Controls.Add(serverPropertiesControl1);
            PropTab.Name = "PropTab";
            PropTab.TabIndex = 0;
            PropTab.Text = "Server Properties";
            // 
            // serverPropertiesControl1
            // 
            serverPropertiesControl1.AutoScroll = true;
            serverPropertiesControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            serverPropertiesControl1.Location = new System.Drawing.Point(0, 0);
            serverPropertiesControl1.MaximumSize = new System.Drawing.Size(861, 0);
            serverPropertiesControl1.Name = "serverPropertiesControl1";
            serverPropertiesControl1.Size = new System.Drawing.Size(861, 654);
            serverPropertiesControl1.TabIndex = 0;
            // 
            // WLTab
            // 
            WLTab.Controls.Add(whitelistControl1);
            WLTab.Name = "WLTab";
            WLTab.TabIndex = 1;
            WLTab.Text = "Whitelist";
            // 
            // whitelistControl1
            // 
            whitelistControl1.AutoScroll = true;
            whitelistControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            whitelistControl1.Location = new System.Drawing.Point(0, 0);
            whitelistControl1.MaximumSize = new System.Drawing.Size(861, 11538);
            whitelistControl1.Name = "whitelistControl1";
            whitelistControl1.Size = new System.Drawing.Size(861, 654);
            whitelistControl1.TabIndex = 0;
            // 
            // PermTab
            // 
            PermTab.Controls.Add(permissionsControl1);
            PermTab.Name = "PermTab";
            PermTab.TabIndex = 2;
            PermTab.Text = "Permissions";
            // 
            // permissionsControl1
            // 
            permissionsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            permissionsControl1.Location = new System.Drawing.Point(0, 0);
            permissionsControl1.MaximumSize = new System.Drawing.Size(861, 11538);
            permissionsControl1.Name = "permissionsControl1";
            permissionsControl1.Size = new System.Drawing.Size(861, 654);
            permissionsControl1.TabIndex = 0;
            // 
            // NetTab
            // 
            NetTab.Controls.Add(networkingControl1);
            NetTab.Name = "NetTab";
            NetTab.TabIndex = 3;
            NetTab.Text = "Networking";
            // 
            // networkingControl1
            // 
            networkingControl1.AutoScroll = true;
            networkingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            networkingControl1.Location = new System.Drawing.Point(0, 0);
            networkingControl1.MaximumSize = new System.Drawing.Size(861, 11538);
            networkingControl1.MinimumSize = new System.Drawing.Size(861, 58);
            networkingControl1.Name = "networkingControl1";
            networkingControl1.Size = new System.Drawing.Size(861, 654);
            networkingControl1.TabIndex = 0;
            // 
            // ExitTab
            // 
            ExitTab.Name = "ExitTab";
            ExitTab.TabIndex = 4;
            ExitTab.Text = "Exit";
            // 
            // ServerConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(965, 720);
            ControlBox = true;
            Controls.Add(CfgTabControl);
            Controls.Add(_navRail);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "ServerConfig";
            Text = "Server Settings Configuration";
            Load += ServerConfig_Load;
            CfgTabControl.ResumeLayout(false);
            PropTab.ResumeLayout(false);
            WLTab.ResumeLayout(false);
            PermTab.ResumeLayout(false);
            NetTab.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Material3.WinForms.Controls.MaterialNavigationRail _navRail;
        private System.Windows.Forms.TabPage PropTab;
        private System.Windows.Forms.TabPage WLTab;
        private BDSUtil.UI.MaterialTabControl CfgTabControl;
        private System.Windows.Forms.TabPage PermTab;
        private Content_Controls.CfgHelper.ServerPropertiesControl serverPropertiesControl1;
        private Content_Controls.CfgHelper.PermissionsControl permissionsControl1;
        private Content_Controls.CfgHelper.WhitelistControl whitelistControl1;
        private System.Windows.Forms.TabPage NetTab;
        private System.Windows.Forms.TabPage ExitTab;
        private Content_Controls.CfgHelper.NetworkingControl networkingControl1;
    }
}
