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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoModWrapper));
            _navRail = new Material3.WinForms.Controls.MaterialNavigationRail();
            AMTabControl = new BDSUtil.UI.MaterialTabControl();
            CFTab = new System.Windows.Forms.TabPage();
            filterTab1 = new BDSUtil.Modals.Content_Controls.AutoMod.FilterTab();
            AMTab = new System.Windows.Forms.TabPage();
            autoModOptions1 = new BDSUtil.Modals.Content_Controls.AutoMod.AutoModOptions();
            AMATab = new System.Windows.Forms.TabPage();
            activeEnforcement1 = new BDSUtil.Modals.Content_Controls.AutoMod.ActiveEnforcement();
            SaveTab = new System.Windows.Forms.TabPage();
            AMTabControl.SuspendLayout();
            CFTab.SuspendLayout();
            AMTab.SuspendLayout();
            AMATab.SuspendLayout();
            SuspendLayout();
            // 
            // _navRail
            // 
            _navRail.Dock = System.Windows.Forms.DockStyle.Left;
            _navRail.Name = "_navRail";
            _navRail.RailWidth = 88;
            _navRail.TabIndex = 0;
            // 
            // AMTabControl
            // 
            AMTabControl.Controls.Add(CFTab);
            AMTabControl.Controls.Add(AMTab);
            AMTabControl.Controls.Add(AMATab);
            AMTabControl.Controls.Add(SaveTab);
            AMTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            AMTabControl.Name = "AMTabControl";
            AMTabControl.SelectedIndex = 0;
            AMTabControl.TabIndex = 1;
            // 
            // CFTab
            // 
            CFTab.Controls.Add(filterTab1);
            CFTab.Name = "CFTab";
            CFTab.TabIndex = 0;
            CFTab.Text = "Chat Filter";
            // 
            // filterTab1
            // 
            filterTab1.AutoScroll = true;
            filterTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            filterTab1.Location = new System.Drawing.Point(0, 0);
            filterTab1.MaximumSize = new System.Drawing.Size(861, 11538);
            filterTab1.Name = "filterTab1";
            filterTab1.Size = new System.Drawing.Size(861, 654);
            filterTab1.TabIndex = 0;
            // 
            // AMTab
            // 
            AMTab.Controls.Add(autoModOptions1);
            AMTab.Name = "AMTab";
            AMTab.TabIndex = 1;
            AMTab.Text = "AutoMod";
            // 
            // autoModOptions1
            // 
            autoModOptions1.Dock = System.Windows.Forms.DockStyle.Fill;
            autoModOptions1.Location = new System.Drawing.Point(0, 0);
            autoModOptions1.MaximumSize = new System.Drawing.Size(861, 11538);
            autoModOptions1.Name = "autoModOptions1";
            autoModOptions1.Size = new System.Drawing.Size(861, 654);
            autoModOptions1.TabIndex = 0;
            // 
            // AMATab
            // 
            AMATab.Controls.Add(activeEnforcement1);
            AMATab.Name = "AMATab";
            AMATab.TabIndex = 2;
            AMATab.Text = "Actions Taken";
            // 
            // activeEnforcement1
            // 
            activeEnforcement1.Dock = System.Windows.Forms.DockStyle.Fill;
            activeEnforcement1.Location = new System.Drawing.Point(0, 0);
            activeEnforcement1.Name = "activeEnforcement1";
            activeEnforcement1.Size = new System.Drawing.Size(861, 654);
            activeEnforcement1.TabIndex = 0;
            // 
            // SaveTab
            // 
            SaveTab.Name = "SaveTab";
            SaveTab.TabIndex = 3;
            SaveTab.Text = "Save and Exit";
            // 
            // AutoModWrapper
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(965, 720);
            ControlBox = true;
            Controls.Add(AMTabControl);
            Controls.Add(_navRail);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "AutoModWrapper";
            Text = "AutoMod Management";
            Load += AutoModWrapper_Load;
            AMTabControl.ResumeLayout(false);
            CFTab.ResumeLayout(false);
            AMTab.ResumeLayout(false);
            AMATab.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion

        private Material3.WinForms.Controls.MaterialNavigationRail _navRail;
        private System.Windows.Forms.TabPage CFTab;
        private System.Windows.Forms.TabPage AMTab;
        private BDSUtil.UI.MaterialTabControl AMTabControl;
        private System.Windows.Forms.TabPage AMATab;
        private System.Windows.Forms.TabPage SaveTab;
        private Content_Controls.AutoMod.FilterTab filterTab1;
        private Content_Controls.AutoMod.AutoModOptions autoModOptions1;
        private Content_Controls.AutoMod.ActiveEnforcement activeEnforcement1;
    }
}