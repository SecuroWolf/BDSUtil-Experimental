namespace BDSUtil.Modals
{
    partial class ServerConfigModalForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerConfigModalForm));
            this.CfgTabControl = new ReaLTaiizor.Controls.MaterialTabControl();
            this.PropTab = new System.Windows.Forms.TabPage();
            this.WLTab = new System.Windows.Forms.TabPage();
            this.PermTab = new System.Windows.Forms.TabPage();
            this.ChatTab = new System.Windows.Forms.TabPage();
            this.SaveDummyTab = new System.Windows.Forms.TabPage();
            this.PHCl = new ReaLTaiizor.Controls.MaterialButton();
            this.IconList_Cfg = new System.Windows.Forms.ImageList(this.components);
            this.serverPropertiesControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.ServerPropertiesControl();
            this.whitelistControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.WhitelistControl();
            this.permissionsControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.PermissionsControl();
            this.chatFilterControl1 = new BDSUtil.Modals.Content_Controls.CfgHelper.ChatFilterControl();
            this.CfgTabControl.SuspendLayout();
            this.PropTab.SuspendLayout();
            this.WLTab.SuspendLayout();
            this.PermTab.SuspendLayout();
            this.ChatTab.SuspendLayout();
            this.SaveDummyTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // CfgTabControl
            // 
            this.CfgTabControl.Controls.Add(this.PropTab);
            this.CfgTabControl.Controls.Add(this.WLTab);
            this.CfgTabControl.Controls.Add(this.PermTab);
            this.CfgTabControl.Controls.Add(this.ChatTab);
            this.CfgTabControl.Controls.Add(this.SaveDummyTab);
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
            this.CfgTabControl.Size = new System.Drawing.Size(797, 447);
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
            this.PropTab.Size = new System.Drawing.Size(789, 412);
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
            this.WLTab.Size = new System.Drawing.Size(789, 412);
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
            this.PermTab.Size = new System.Drawing.Size(789, 412);
            this.PermTab.TabIndex = 2;
            this.PermTab.Text = "Permissions";
            this.PermTab.UseVisualStyleBackColor = true;
            // 
            // ChatTab
            // 
            this.ChatTab.AutoScroll = true;
            this.ChatTab.Controls.Add(this.chatFilterControl1);
            this.ChatTab.ImageKey = "chat_info_24dp.png";
            this.ChatTab.Location = new System.Drawing.Point(4, 31);
            this.ChatTab.Name = "ChatTab";
            this.ChatTab.Size = new System.Drawing.Size(789, 412);
            this.ChatTab.TabIndex = 3;
            this.ChatTab.Text = "Chat Filter";
            this.ChatTab.ToolTipText = "Chat Filter";
            this.ChatTab.UseVisualStyleBackColor = true;
            // 
            // SaveDummyTab
            // 
            this.SaveDummyTab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.SaveDummyTab.Controls.Add(this.PHCl);
            this.SaveDummyTab.ImageKey = "save_24dp.png";
            this.SaveDummyTab.Location = new System.Drawing.Point(4, 31);
            this.SaveDummyTab.Name = "SaveDummyTab";
            this.SaveDummyTab.Padding = new System.Windows.Forms.Padding(3);
            this.SaveDummyTab.Size = new System.Drawing.Size(789, 412);
            this.SaveDummyTab.TabIndex = 4;
            this.SaveDummyTab.Text = "Save";
            this.SaveDummyTab.ToolTipText = "Save and Exit";
            // 
            // PHCl
            // 
            this.PHCl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PHCl.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.PHCl.Depth = 0;
            this.PHCl.HighEmphasis = true;
            this.PHCl.Icon = null;
            this.PHCl.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.PHCl.Location = new System.Drawing.Point(304, 79);
            this.PHCl.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.PHCl.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.PHCl.Name = "PHCl";
            this.PHCl.NoAccentTextColor = System.Drawing.Color.Empty;
            this.PHCl.Size = new System.Drawing.Size(171, 36);
            this.PHCl.TabIndex = 0;
            this.PHCl.Text = "Placeholder Close";
            this.PHCl.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.PHCl.UseAccentColor = false;
            this.PHCl.UseVisualStyleBackColor = true;
            this.PHCl.Click += new System.EventHandler(this.PHCl_Click);
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
            this.whitelistControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.whitelistControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.whitelistControl1.Location = new System.Drawing.Point(0, 0);
            this.whitelistControl1.Name = "whitelistControl1";
            this.whitelistControl1.Size = new System.Drawing.Size(789, 412);
            this.whitelistControl1.TabIndex = 0;
            // 
            // permissionsControl1
            // 
            this.permissionsControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.permissionsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.permissionsControl1.Location = new System.Drawing.Point(0, 0);
            this.permissionsControl1.Name = "permissionsControl1";
            this.permissionsControl1.Size = new System.Drawing.Size(789, 412);
            this.permissionsControl1.TabIndex = 0;
            // 
            // chatFilterControl1
            // 
            this.chatFilterControl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.chatFilterControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chatFilterControl1.Location = new System.Drawing.Point(0, 0);
            this.chatFilterControl1.Name = "chatFilterControl1";
            this.chatFilterControl1.Size = new System.Drawing.Size(789, 412);
            this.chatFilterControl1.TabIndex = 0;
            // 
            // ServerConfigModalForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.CfgTabControl);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.CfgTabControl;
            this.FormStyle = ReaLTaiizor.Enum.Material.FormStyles.StatusAndActionBar_None;
            this.Name = "ServerConfigModalForm";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.Text = "ServerConfigModalForm";
            this.Load += new System.EventHandler(this.ServerConfigModalForm_Load);
            this.CfgTabControl.ResumeLayout(false);
            this.PropTab.ResumeLayout(false);
            this.WLTab.ResumeLayout(false);
            this.PermTab.ResumeLayout(false);
            this.ChatTab.ResumeLayout(false);
            this.SaveDummyTab.ResumeLayout(false);
            this.SaveDummyTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage PropTab;
        private System.Windows.Forms.TabPage WLTab;
        private ReaLTaiizor.Controls.MaterialTabControl CfgTabControl;
        private System.Windows.Forms.ImageList IconList_Cfg;
        private System.Windows.Forms.TabPage PermTab;
        private System.Windows.Forms.TabPage ChatTab;
        private Content_Controls.CfgHelper.ServerPropertiesControl serverPropertiesControl1;
        private Content_Controls.CfgHelper.WhitelistControl whitelistControl1;
        private Content_Controls.CfgHelper.PermissionsControl permissionsControl1;
        private Content_Controls.CfgHelper.ChatFilterControl chatFilterControl1;
        private System.Windows.Forms.TabPage SaveDummyTab;
        private ReaLTaiizor.Controls.MaterialButton PHCl;
    }
}