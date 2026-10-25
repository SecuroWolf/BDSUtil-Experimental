namespace BDSUtil.Modals
{
    partial class AutoModWrapper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoModWrapper));
            this.AMTabControl = new ReaLTaiizor.Controls.MaterialTabControl();
            this.CFTab = new System.Windows.Forms.TabPage();
            this.filterTab1 = new BDSUtil.Modals.Content_Controls.AutoMod.FilterTab();
            this.AMTab = new System.Windows.Forms.TabPage();
            this.autoModOptions1 = new BDSUtil.Modals.Content_Controls.AutoMod.AutoModOptions();
            this.AMATab = new System.Windows.Forms.TabPage();
            this.SaveTab = new System.Windows.Forms.TabPage();
            this.IconList_Cfg = new System.Windows.Forms.ImageList(this.components);
            this.AMTabControl.SuspendLayout();
            this.CFTab.SuspendLayout();
            this.AMTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // AMTabControl
            // 
            this.AMTabControl.Controls.Add(this.CFTab);
            this.AMTabControl.Controls.Add(this.AMTab);
            this.AMTabControl.Controls.Add(this.AMATab);
            this.AMTabControl.Controls.Add(this.SaveTab);
            this.AMTabControl.Cursor = System.Windows.Forms.Cursors.Default;
            this.AMTabControl.Depth = 0;
            this.AMTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AMTabControl.ImageList = this.IconList_Cfg;
            this.AMTabControl.Location = new System.Drawing.Point(0, 0);
            this.AMTabControl.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.AMTabControl.Multiline = true;
            this.AMTabControl.Name = "AMTabControl";
            this.AMTabControl.SelectedIndex = 0;
            this.AMTabControl.ShowToolTips = true;
            this.AMTabControl.Size = new System.Drawing.Size(797, 597);
            this.AMTabControl.TabIndex = 0;
            // 
            // CFTab
            // 
            this.CFTab.Controls.Add(this.filterTab1);
            this.CFTab.ImageKey = "chat_info_24dp.png";
            this.CFTab.Location = new System.Drawing.Point(4, 31);
            this.CFTab.Margin = new System.Windows.Forms.Padding(0);
            this.CFTab.Name = "CFTab";
            this.CFTab.Size = new System.Drawing.Size(789, 562);
            this.CFTab.TabIndex = 0;
            this.CFTab.Text = "Chat Filter";
            this.CFTab.ToolTipText = "Server.Properties";
            this.CFTab.UseVisualStyleBackColor = true;
            // 
            // filterTab1
            // 
            this.filterTab1.AutoScroll = true;
            this.filterTab1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.filterTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filterTab1.Location = new System.Drawing.Point(0, 0);
            this.filterTab1.MaximumSize = new System.Drawing.Size(738, 10000);
            this.filterTab1.Name = "filterTab1";
            this.filterTab1.Size = new System.Drawing.Size(738, 562);
            this.filterTab1.TabIndex = 0;
            // 
            // AMTab
            // 
            this.AMTab.Controls.Add(this.autoModOptions1);
            this.AMTab.ImageKey = "ic_settings_white_24dp_1x.png";
            this.AMTab.Location = new System.Drawing.Point(4, 31);
            this.AMTab.Name = "AMTab";
            this.AMTab.Size = new System.Drawing.Size(789, 562);
            this.AMTab.TabIndex = 1;
            this.AMTab.Text = "AutoMod";
            this.AMTab.ToolTipText = "Whitelist";
            this.AMTab.UseVisualStyleBackColor = true;
            // 
            // autoModOptions1
            // 
            this.autoModOptions1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.autoModOptions1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.autoModOptions1.Location = new System.Drawing.Point(0, 0);
            this.autoModOptions1.MaximumSize = new System.Drawing.Size(738, 10000);
            this.autoModOptions1.Name = "autoModOptions1";
            this.autoModOptions1.Size = new System.Drawing.Size(738, 562);
            this.autoModOptions1.TabIndex = 0;
            // 
            // AMATab
            // 
            this.AMATab.ImageKey = "admin_panel_settings_24dp.png";
            this.AMATab.Location = new System.Drawing.Point(4, 31);
            this.AMATab.Name = "AMATab";
            this.AMATab.Size = new System.Drawing.Size(789, 562);
            this.AMATab.TabIndex = 2;
            this.AMATab.Text = "Actions Taken";
            this.AMATab.UseVisualStyleBackColor = true;
            // 
            // SaveTab
            // 
            this.SaveTab.ImageKey = "save_24dp.png";
            this.SaveTab.Location = new System.Drawing.Point(4, 31);
            this.SaveTab.Name = "SaveTab";
            this.SaveTab.Padding = new System.Windows.Forms.Padding(3);
            this.SaveTab.Size = new System.Drawing.Size(789, 562);
            this.SaveTab.TabIndex = 6;
            this.SaveTab.Text = "Save and Exit";
            this.SaveTab.UseVisualStyleBackColor = true;
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
            // 
            // AutoModWrapper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.ControlBox = false;
            this.Controls.Add(this.AMTabControl);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.AMTabControl;
            this.FormStyle = ReaLTaiizor.Enum.Material.FormStyles.StatusAndActionBar_None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AutoModWrapper";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.Text = "AutoModWrapper";
            this.Load += new System.EventHandler(this.AutoModWrapper_Load);
            this.AMTabControl.ResumeLayout(false);
            this.CFTab.ResumeLayout(false);
            this.AMTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion
        private System.Windows.Forms.TabPage CFTab;
        private System.Windows.Forms.TabPage AMTab;
        private ReaLTaiizor.Controls.MaterialTabControl AMTabControl;
        private System.Windows.Forms.ImageList IconList_Cfg;
        private System.Windows.Forms.TabPage AMATab;
        private System.Windows.Forms.TabPage SaveTab;
        private Content_Controls.AutoMod.FilterTab filterTab1;
        private Content_Controls.AutoMod.AutoModOptions autoModOptions1;
    }
}