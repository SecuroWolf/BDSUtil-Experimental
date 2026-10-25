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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfig));
            this.CfgTabControl = new ReaLTaiizor.Controls.MaterialTabControl();
            this.PropTab = new System.Windows.Forms.TabPage();
            this.WLTab = new System.Windows.Forms.TabPage();
            this.PermTab = new System.Windows.Forms.TabPage();
            this.NetTab = new System.Windows.Forms.TabPage();
            this.ExitTab = new System.Windows.Forms.TabPage();
            this.IconList_Cfg = new System.Windows.Forms.ImageList(this.components);
            this.serverPropertiesControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.ServerPropertiesControl();
            this.whitelistControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.WhitelistControl();
            this.permissionsControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.PermissionsControl();
            this.networkingControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.NetworkingControl();
            this.CfgTabControl.SuspendLayout();
            this.PropTab.SuspendLayout();
            this.WLTab.SuspendLayout();
            this.PermTab.SuspendLayout();
            this.NetTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // CfgTabControl
            // 
            this.CfgTabControl.Controls.Add(this.PropTab);
            this.CfgTabControl.Controls.Add(this.WLTab);
            this.CfgTabControl.Controls.Add(this.PermTab);
            this.CfgTabControl.Controls.Add(this.NetTab);
            this.CfgTabControl.Controls.Add(this.ExitTab);
            this.CfgTabControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.CfgTabControl.Depth = 0;
            this.CfgTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CfgTabControl.ImageList = this.IconList_Cfg;
            this.CfgTabControl.Location = new System.Drawing.Point(0, 0);
            this.CfgTabControl.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CfgTabControl.Multiline = true;
            this.CfgTabControl.Name = "CfgTabControl";
            this.CfgTabControl.SelectedIndex = 0;
            this.CfgTabControl.ShowToolTips = true;
            this.CfgTabControl.Size = new System.Drawing.Size(797, 597);
            this.CfgTabControl.TabIndex = 0;
            // 
            // PropTab
            // 
            this.PropTab.AutoScroll = true;
            this.PropTab.Controls.Add(this.serverPropertiesControl1);
            this.PropTab.ImageKey = "ic_settings_white_24dp_1x.png";
            this.PropTab.Location = new System.Drawing.Point(4, 31);
            this.PropTab.Margin = new System.Windows.Forms.Padding(0);
            this.PropTab.Name = "PropTab";
            this.PropTab.Size = new System.Drawing.Size(789, 562);
            this.PropTab.TabIndex = 0;
            this.PropTab.Text = "Server Properties";
            this.PropTab.ToolTipText = "Server.Properties";
            this.PropTab.UseVisualStyleBackColor = true;
            // 
            // WLTab
            // 
            this.WLTab.Controls.Add(this.whitelistControl1);
            this.WLTab.ImageKey = "gate_24dp.png";
            this.WLTab.Location = new System.Drawing.Point(4, 31);
            this.WLTab.Name = "WLTab";
            this.WLTab.Size = new System.Drawing.Size(789, 562);
            this.WLTab.TabIndex = 1;
            this.WLTab.Text = "Whitelist";
            this.WLTab.ToolTipText = "Whitelist";
            this.WLTab.UseVisualStyleBackColor = true;
            // 
            // PermTab
            // 
            this.PermTab.Controls.Add(this.permissionsControl1);
            this.PermTab.ImageKey = "admin_panel_settings_24dp.png";
            this.PermTab.Location = new System.Drawing.Point(4, 31);
            this.PermTab.Name = "PermTab";
            this.PermTab.Size = new System.Drawing.Size(789, 562);
            this.PermTab.TabIndex = 2;
            this.PermTab.Text = "Permissions";
            this.PermTab.UseVisualStyleBackColor = true;
            // 
            // NetTab
            // 
            this.NetTab.Controls.Add(this.networkingControl1);
            this.NetTab.ImageKey = "network_node_24dp.png";
            this.NetTab.Location = new System.Drawing.Point(4, 31);
            this.NetTab.Name = "NetTab";
            this.NetTab.Size = new System.Drawing.Size(789, 562);
            this.NetTab.TabIndex = 5;
            this.NetTab.Text = "Networking";
            this.NetTab.UseVisualStyleBackColor = true;
            // 
            // ExitTab
            // 
            this.ExitTab.ImageKey = "close_24dp.png";
            this.ExitTab.Location = new System.Drawing.Point(4, 31);
            this.ExitTab.Name = "ExitTab";
            this.ExitTab.Padding = new System.Windows.Forms.Padding(3);
            this.ExitTab.Size = new System.Drawing.Size(789, 562);
            this.ExitTab.TabIndex = 6;
            this.ExitTab.Text = "Exit";
            this.ExitTab.UseVisualStyleBackColor = true;
            // 
            // IconList_Cfg
            // 
            this.IconList_Cfg.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IconList_Cfg.ImageStream")));
            this.IconList_Cfg.TransparentColor = System.Drawing.Color.Transparent;
            this.IconList_Cfg.Images.SetKeyName(0, "ic_settings_white_24dp_1x.png");
            this.IconList_Cfg.Images.SetKeyName(1, "admin_panel_settings_24dp.png");
            this.IconList_Cfg.Images.SetKeyName(2, "chat_info_24dp.png");
            this.IconList_Cfg.Images.SetKeyName(3, "gate_24dp.png");
            this.IconList_Cfg.Images.SetKeyName(4, "key_24dp.png");
            this.IconList_Cfg.Images.SetKeyName(5, "");
            this.IconList_Cfg.Images.SetKeyName(6, "save_24dp.png");
            this.IconList_Cfg.Images.SetKeyName(7, "network_node_24dp.png");
            this.IconList_Cfg.Images.SetKeyName(8, "close_24dp.png");
            // 
            // serverPropertiesControl1
            // 
            this.serverPropertiesControl1.AutoScroll = true;
            this.serverPropertiesControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.serverPropertiesControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverPropertiesControl1.Location = new System.Drawing.Point(0, 0);
            this.serverPropertiesControl1.Margin = new System.Windows.Forms.Padding(0);
            this.serverPropertiesControl1.MaximumSize = new System.Drawing.Size(738, 0);
            this.serverPropertiesControl1.Name = "serverPropertiesControl1";
            this.serverPropertiesControl1.Size = new System.Drawing.Size(738, 0);
            this.serverPropertiesControl1.TabIndex = 0;
            // 
            // whitelistControl1
            // 
            this.whitelistControl1.AutoScroll = true;
            this.whitelistControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.whitelistControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whitelistControl1.Location = new System.Drawing.Point(0, 0);
            this.whitelistControl1.MaximumSize = new System.Drawing.Size(738, 10000);
            this.whitelistControl1.Name = "whitelistControl1";
            this.whitelistControl1.Size = new System.Drawing.Size(738, 562);
            this.whitelistControl1.TabIndex = 0;
            // 
            // permissionsControl1
            // 
            this.permissionsControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.permissionsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.permissionsControl1.Location = new System.Drawing.Point(0, 0);
            this.permissionsControl1.MaximumSize = new System.Drawing.Size(738, 10000);
            this.permissionsControl1.Name = "permissionsControl1";
            this.permissionsControl1.Size = new System.Drawing.Size(738, 562);
            this.permissionsControl1.TabIndex = 0;
            // 
            // networkingControl1
            // 
            this.networkingControl1.AutoScroll = true;
            this.networkingControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.networkingControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.networkingControl1.Location = new System.Drawing.Point(0, 0);
            this.networkingControl1.MaximumSize = new System.Drawing.Size(738, 10000);
            this.networkingControl1.MinimumSize = new System.Drawing.Size(738, 50);
            this.networkingControl1.Name = "networkingControl1";
            this.networkingControl1.Size = new System.Drawing.Size(738, 562);
            this.networkingControl1.TabIndex = 0;
            // 
            // ServerConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.ControlBox = false;
            this.Controls.Add(this.CfgTabControl);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.CfgTabControl;
            this.FormStyle = ReaLTaiizor.Enum.Material.FormStyles.StatusAndActionBar_None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ServerConfig";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.Text = "ServerConfig";
            this.Load += new System.EventHandler(this.ServerConfig_Load);
            this.CfgTabControl.ResumeLayout(false);
            this.PropTab.ResumeLayout(false);
            this.WLTab.ResumeLayout(false);
            this.PermTab.ResumeLayout(false);
            this.NetTab.ResumeLayout(false);
            this.ResumeLayout(false);

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
