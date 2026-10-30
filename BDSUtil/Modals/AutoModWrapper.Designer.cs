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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoModWrapper));
            AMTabControl = new ReaLTaiizor.Controls.MaterialTabControl();
            CFTab = new System.Windows.Forms.TabPage();
            filterTab1 = new BDSUtil.Modals.Content_Controls.AutoMod.FilterTab();
            AMTab = new System.Windows.Forms.TabPage();
            autoModOptions1 = new BDSUtil.Modals.Content_Controls.AutoMod.AutoModOptions();
            AMATab = new System.Windows.Forms.TabPage();
            activeEnforcement1 = new BDSUtil.Modals.Content_Controls.AutoMod.ActiveEnforcement();
            SaveTab = new System.Windows.Forms.TabPage();
            IconList_Cfg = new System.Windows.Forms.ImageList(components);
            AMTabControl.SuspendLayout();
            CFTab.SuspendLayout();
            AMTab.SuspendLayout();
            AMATab.SuspendLayout();
            SuspendLayout();
            // 
            // AMTabControl
            // 
            AMTabControl.Controls.Add(CFTab);
            AMTabControl.Controls.Add(AMTab);
            AMTabControl.Controls.Add(AMATab);
            AMTabControl.Controls.Add(SaveTab);
            AMTabControl.Depth = 0;
            AMTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            AMTabControl.ImageList = IconList_Cfg;
            AMTabControl.Location = new System.Drawing.Point(0, 0);
            AMTabControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AMTabControl.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            AMTabControl.Multiline = true;
            AMTabControl.Name = "AMTabControl";
            AMTabControl.SelectedIndex = 0;
            AMTabControl.ShowToolTips = true;
            AMTabControl.Size = new System.Drawing.Size(929, 689);
            AMTabControl.TabIndex = 0;
            // 
            // CFTab
            // 
            CFTab.Controls.Add(filterTab1);
            CFTab.ImageKey = "chat_info_24dp.png";
            CFTab.Location = new System.Drawing.Point(4, 31);
            CFTab.Margin = new System.Windows.Forms.Padding(0);
            CFTab.Name = "CFTab";
            CFTab.Size = new System.Drawing.Size(921, 654);
            CFTab.TabIndex = 0;
            CFTab.Text = "Chat Filter";
            CFTab.ToolTipText = "Server.Properties";
            CFTab.UseVisualStyleBackColor = true;
            // 
            // filterTab1
            // 
            filterTab1.AutoScroll = true;
            filterTab1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            filterTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            filterTab1.Location = new System.Drawing.Point(0, 0);
            filterTab1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            filterTab1.MaximumSize = new System.Drawing.Size(861, 11538);
            filterTab1.Name = "filterTab1";
            filterTab1.Size = new System.Drawing.Size(861, 654);
            filterTab1.TabIndex = 0;
            // 
            // AMTab
            // 
            AMTab.Controls.Add(autoModOptions1);
            AMTab.ImageKey = "ic_settings_white_24dp_1x.png";
            AMTab.Location = new System.Drawing.Point(4, 31);
            AMTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AMTab.Name = "AMTab";
            AMTab.Size = new System.Drawing.Size(921, 654);
            AMTab.TabIndex = 1;
            AMTab.Text = "AutoMod";
            AMTab.ToolTipText = "Whitelist";
            AMTab.UseVisualStyleBackColor = true;
            // 
            // autoModOptions1
            // 
            autoModOptions1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            autoModOptions1.Dock = System.Windows.Forms.DockStyle.Fill;
            autoModOptions1.Location = new System.Drawing.Point(0, 0);
            autoModOptions1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            autoModOptions1.MaximumSize = new System.Drawing.Size(861, 11538);
            autoModOptions1.Name = "autoModOptions1";
            autoModOptions1.Size = new System.Drawing.Size(861, 654);
            autoModOptions1.TabIndex = 0;
            // 
            // AMATab
            // 
            AMATab.Controls.Add(activeEnforcement1);
            AMATab.ImageKey = "admin_panel_settings_24dp.png";
            AMATab.Location = new System.Drawing.Point(4, 31);
            AMATab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AMATab.Name = "AMATab";
            AMATab.Size = new System.Drawing.Size(921, 654);
            AMATab.TabIndex = 2;
            AMATab.Text = "Actions Taken";
            AMATab.UseVisualStyleBackColor = true;
            // 
            // activeEnforcement1
            // 
            activeEnforcement1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            activeEnforcement1.Dock = System.Windows.Forms.DockStyle.Fill;
            activeEnforcement1.Location = new System.Drawing.Point(0, 0);
            activeEnforcement1.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            activeEnforcement1.Name = "activeEnforcement1";
            activeEnforcement1.Size = new System.Drawing.Size(921, 654);
            activeEnforcement1.TabIndex = 0;
            // 
            // SaveTab
            // 
            SaveTab.ImageKey = "save_24dp.png";
            SaveTab.Location = new System.Drawing.Point(4, 31);
            SaveTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SaveTab.Name = "SaveTab";
            SaveTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SaveTab.Size = new System.Drawing.Size(921, 654);
            SaveTab.TabIndex = 6;
            SaveTab.Text = "Save and Exit";
            SaveTab.UseVisualStyleBackColor = true;
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
            // 
            // AutoModWrapper
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            ClientSize = new System.Drawing.Size(933, 692);
            ControlBox = false;
            Controls.Add(AMTabControl);
            DrawerShowIconsWhenHidden = true;
            DrawerTabControl = AMTabControl;
            FormStyle = ReaLTaiizor.Enum.Material.FormStyles.StatusAndActionBar_None;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "AutoModWrapper";
            Padding = new System.Windows.Forms.Padding(0, 0, 4, 3);
            Text = "AutoModWrapper";
            Load += AutoModWrapper_Load;
            AMTabControl.ResumeLayout(false);
            CFTab.ResumeLayout(false);
            AMTab.ResumeLayout(false);
            AMATab.ResumeLayout(false);
            ResumeLayout(false);

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
        private Content_Controls.AutoMod.ActiveEnforcement activeEnforcement1;
    }
}