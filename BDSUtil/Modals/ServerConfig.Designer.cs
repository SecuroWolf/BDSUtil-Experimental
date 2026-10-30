namespace BDSUtil.Modals
{
    // ✅ FIX: Designer partial declaration mapped to renamed ServerConfig class
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfig));
            CfgTabControl = new ReaLTaiizor.Controls.MaterialTabControl();
            PropTab = new System.Windows.Forms.TabPage();
            serverPropertiesControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.ServerPropertiesControl();
            WLTab = new System.Windows.Forms.TabPage();
            whitelistControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.WhitelistControl();
            PermTab = new System.Windows.Forms.TabPage();
            permissionsControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.PermissionsControl();
            NetTab = new System.Windows.Forms.TabPage();
            networkingControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.NetworkingControl();
            ExitTab = new System.Windows.Forms.TabPage();
            IconList_Cfg = new System.Windows.Forms.ImageList(components);
            CfgTabControl.SuspendLayout();
            PropTab.SuspendLayout();
            WLTab.SuspendLayout();
            PermTab.SuspendLayout();
            NetTab.SuspendLayout();
            SuspendLayout();
            // 
            // CfgTabControl
            // 
            CfgTabControl.Controls.Add(PropTab);
            CfgTabControl.Controls.Add(WLTab);
            CfgTabControl.Controls.Add(PermTab);
            CfgTabControl.Controls.Add(NetTab);
            CfgTabControl.Controls.Add(ExitTab);
            CfgTabControl.Depth = 0;
            CfgTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            CfgTabControl.ImageList = IconList_Cfg;
            CfgTabControl.Location = new System.Drawing.Point(0, 0);
            CfgTabControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CfgTabControl.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CfgTabControl.Multiline = true;
            CfgTabControl.Name = "CfgTabControl";
            CfgTabControl.SelectedIndex = 0;
            CfgTabControl.ShowToolTips = true;
            CfgTabControl.Size = new System.Drawing.Size(929, 689);
            CfgTabControl.TabIndex = 0;
            // 
            // PropTab
            // 
            PropTab.AutoScroll = true;
            PropTab.Controls.Add(serverPropertiesControl1);
            PropTab.ImageKey = "ic_settings_white_24dp_1x.png";
            PropTab.Location = new System.Drawing.Point(4, 31);
            PropTab.Margin = new System.Windows.Forms.Padding(0);
            PropTab.Name = "PropTab";
            PropTab.Size = new System.Drawing.Size(921, 654);
            PropTab.TabIndex = 0;
            PropTab.Text = "Server Properties";
            PropTab.ToolTipText = "Server.Properties";
            PropTab.UseVisualStyleBackColor = true;
            // 
            // serverPropertiesControl1
            // 
            serverPropertiesControl1.AutoScroll = true;
            serverPropertiesControl1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            serverPropertiesControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            serverPropertiesControl1.Location = new System.Drawing.Point(0, 0);
            serverPropertiesControl1.Margin = new System.Windows.Forms.Padding(0);
            serverPropertiesControl1.MaximumSize = new System.Drawing.Size(861, 0);
            serverPropertiesControl1.Name = "serverPropertiesControl1";
            serverPropertiesControl1.Size = new System.Drawing.Size(861, 654);
            serverPropertiesControl1.TabIndex = 0;
            // 
            // WLTab
            // 
            WLTab.Controls.Add(whitelistControl1);
            WLTab.ImageKey = "gate_24dp.png";
            WLTab.Location = new System.Drawing.Point(4, 31);
            WLTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            WLTab.Name = "WLTab";
            WLTab.Size = new System.Drawing.Size(922, 654);
            WLTab.TabIndex = 1;
            WLTab.Text = "Whitelist";
            WLTab.ToolTipText = "Whitelist";
            WLTab.UseVisualStyleBackColor = true;
            // 
            // whitelistControl1
            // 
            whitelistControl1.AutoScroll = true;
            whitelistControl1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            whitelistControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            whitelistControl1.Location = new System.Drawing.Point(0, 0);
            whitelistControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            whitelistControl1.MaximumSize = new System.Drawing.Size(861, 11538);
            whitelistControl1.Name = "whitelistControl1";
            whitelistControl1.Size = new System.Drawing.Size(861, 654);
            whitelistControl1.TabIndex = 0;
            // 
            // PermTab
            // 
            PermTab.Controls.Add(permissionsControl1);
            PermTab.ImageKey = "admin_panel_settings_24dp.png";
            PermTab.Location = new System.Drawing.Point(4, 31);
            PermTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PermTab.Name = "PermTab";
            PermTab.Size = new System.Drawing.Size(922, 654);
            PermTab.TabIndex = 2;
            PermTab.Text = "Permissions";
            PermTab.UseVisualStyleBackColor = true;
            // 
            // permissionsControl1
            // 
            permissionsControl1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            permissionsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            permissionsControl1.Location = new System.Drawing.Point(0, 0);
            permissionsControl1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            permissionsControl1.MaximumSize = new System.Drawing.Size(861, 11538);
            permissionsControl1.Name = "permissionsControl1";
            permissionsControl1.Size = new System.Drawing.Size(861, 654);
            permissionsControl1.TabIndex = 0;
            // 
            // NetTab
            // 
            NetTab.Controls.Add(networkingControl1);
            NetTab.ImageKey = "network_node_24dp.png";
            NetTab.Location = new System.Drawing.Point(4, 31);
            NetTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NetTab.Name = "NetTab";
            NetTab.Size = new System.Drawing.Size(922, 654);
            NetTab.TabIndex = 5;
            NetTab.Text = "Networking";
            NetTab.UseVisualStyleBackColor = true;
            // 
            // networkingControl1
            // 
            networkingControl1.AutoScroll = true;
            networkingControl1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            networkingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            networkingControl1.Location = new System.Drawing.Point(0, 0);
            networkingControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            networkingControl1.MaximumSize = new System.Drawing.Size(861, 11538);
            networkingControl1.MinimumSize = new System.Drawing.Size(861, 58);
            networkingControl1.Name = "networkingControl1";
            networkingControl1.Size = new System.Drawing.Size(861, 654);
            networkingControl1.TabIndex = 0;
            // 
            // ExitTab
            // 
            ExitTab.ImageKey = "close_24dp.png";
            ExitTab.Location = new System.Drawing.Point(4, 31);
            ExitTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ExitTab.Name = "ExitTab";
            ExitTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ExitTab.Size = new System.Drawing.Size(922, 654);
            ExitTab.TabIndex = 6;
            ExitTab.Text = "Exit";
            ExitTab.UseVisualStyleBackColor = true;
            // 
            // IconList_Cfg
            // 
            IconList_Cfg.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            IconList_Cfg.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("IconList_Cfg.ImageStream");
            IconList_Cfg.TransparentColor = System.Drawing.Color.Transparent;
            IconList_Cfg.Images.SetKeyName(0, "ic_settings_white_24dp_1x.png");
            IconList_Cfg.Images.SetKeyName(1, "admin_panel_settings_24dp.png");
            IconList_Cfg.Images.SetKeyName(2, "chat_info_24dp.png");
            IconList_Cfg.Images.SetKeyName(3, "gate_24dp.png");
            IconList_Cfg.Images.SetKeyName(4, "key_24dp.png");
            IconList_Cfg.Images.SetKeyName(5, "");
            IconList_Cfg.Images.SetKeyName(6, "save_24dp.png");
            IconList_Cfg.Images.SetKeyName(7, "network_node_24dp.png");
            IconList_Cfg.Images.SetKeyName(8, "close_24dp.png");
            // 
            // ServerConfig
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            ClientSize = new System.Drawing.Size(933, 692);
            ControlBox = false;
            Controls.Add(CfgTabControl);
            DrawerShowIconsWhenHidden = true;
            DrawerTabControl = CfgTabControl;
            FormStyle = ReaLTaiizor.Enum.Material.FormStyles.StatusAndActionBar_None;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ServerConfig";
            Padding = new System.Windows.Forms.Padding(0, 0, 4, 3);
            Text = "ServerConfig";
            Load += ServerConfig_Load;
            CfgTabControl.ResumeLayout(false);
            PropTab.ResumeLayout(false);
            WLTab.ResumeLayout(false);
            PermTab.ResumeLayout(false);
            NetTab.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage PropTab;
        private System.Windows.Forms.TabPage WLTab;
        private ReaLTaiizor.Controls.MaterialTabControl CfgTabControl;
        private System.Windows.Forms.ImageList IconList_Cfg;
        private System.Windows.Forms.TabPage PermTab;
        private Content_Controls.CfgHelper.ServerPropertiesControl serverPropertiesControl1;
        private Content_Controls.CfgHelper.PermissionsControl permissionsControl1;
        private Content_Controls.CfgHelper.WhitelistControl whitelistControl1;
        private System.Windows.Forms.TabPage NetTab;
        private System.Windows.Forms.TabPage ExitTab;
        private Content_Controls.CfgHelper.NetworkingControl networkingControl1;
    }
}
