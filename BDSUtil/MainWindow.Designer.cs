using BDSUtil.UI;
using Material3.WinForms.Controls;
using Material3.WinForms.Theming;
using Material3.WinForms.Typography;

namespace BDSUtil
{
    partial class MainWindow
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            MainNavDrawer = new MaterialNavigationDrawer();
            MaterialTabControl1 = new MaterialTabControl();
            tabLiveMonitor = new System.Windows.Forms.TabPage();
            SV_PM_BTN = new MaterialFab();
            StartServerBTN_New = new MaterialFab();
            SvConfigButton = new MaterialFab();
            StopServerBTN_New = new MaterialFab();
            CommandInputTextBox = new MaterialTextBoxEdit();
            ConsoleOutputTextBox = new System.Windows.Forms.RichTextBox();
            BkRsTab = new System.Windows.Forms.TabPage();
            RestoreWrapper = new MaterialCard();
            RunRestoreBTN = new MaterialButton();
            BackupPointsListBox = new MaterialListBox();
            RestoreTypeComboBox = new MaterialComboBox();
            RestorTitleLBL = new MaterialLabel();
            BackupWrapper = new MaterialCard();
            BackupProgressBar = new MaterialProgressBar();
            BKCompleteLBL = new MaterialLabel();
            BackupServerBTN = new MaterialButton();
            BackupPacksBTN = new MaterialButton();
            BackupWorldBTN = new MaterialButton();
            AutoModTab = new System.Windows.Forms.TabPage();
            label2 = new System.Windows.Forms.Label();
            LETab = new System.Windows.Forms.TabPage();
            editorProvider1 = new BDSUtil.Modals.Content_Controls.Level_Editor.EditorProvider();
            SettingsTabPage = new System.Windows.Forms.TabPage();
            settingsProvider1 = new BDSUtil.Modals.Content_Controls.Settings.SettingsProvider();
            MaterialTabControl1.SuspendLayout();
            tabLiveMonitor.SuspendLayout();
            BkRsTab.SuspendLayout();
            RestoreWrapper.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)RestoreTypeComboBox).BeginInit();
            BackupWrapper.SuspendLayout();
            AutoModTab.SuspendLayout();
            LETab.SuspendLayout();
            SettingsTabPage.SuspendLayout();
            SuspendLayout();
            // 
            // MainNavDrawer
            // 
            MainNavDrawer.BackColor = System.Drawing.Color.FromArgb(25, 28, 30);
            MainNavDrawer.Dock = System.Windows.Forms.DockStyle.Left;
            MainNavDrawer.DrawerWidth = 220;
            MainNavDrawer.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            MainNavDrawer.Headline = "NAVIGATION";
            MainNavDrawer.Location = new System.Drawing.Point(1, 41);
            MainNavDrawer.Name = "MainNavDrawer";
            MainNavDrawer.Padding = new System.Windows.Forms.Padding(12);
            MainNavDrawer.Size = new System.Drawing.Size(220, 812);
            MainNavDrawer.TabIndex = 0;
            // 
            // MaterialTabControl1
            // 
            MaterialTabControl1.Controls.Add(tabLiveMonitor);
            MaterialTabControl1.Controls.Add(BkRsTab);
            MaterialTabControl1.Controls.Add(AutoModTab);
            MaterialTabControl1.Controls.Add(LETab);
            MaterialTabControl1.Controls.Add(SettingsTabPage);
            MaterialTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            MaterialTabControl1.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            MaterialTabControl1.Location = new System.Drawing.Point(221, 41);
            MaterialTabControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaterialTabControl1.Multiline = true;
            MaterialTabControl1.Name = "MaterialTabControl1";
            MaterialTabControl1.SelectedIndex = 0;
            MaterialTabControl1.Size = new System.Drawing.Size(1044, 812);
            MaterialTabControl1.TabIndex = 1;
            MaterialTabControl1.SelectedIndexChanged += MaterialTabControl1_SelectedIndexChanged;
            // 
            // tabLiveMonitor
            // 
            tabLiveMonitor.BackColor = System.Drawing.Color.FromArgb(17, 20, 21);
            tabLiveMonitor.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            tabLiveMonitor.Location = new System.Drawing.Point(4, 24);
            tabLiveMonitor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabLiveMonitor.Name = "tabLiveMonitor";
            tabLiveMonitor.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabLiveMonitor.Size = new System.Drawing.Size(1036, 784);
            tabLiveMonitor.TabIndex = 0;
            tabLiveMonitor.Text = "Server Console";
            // 
            // SV_PM_BTN
            // 
            SV_PM_BTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            SV_PM_BTN.AutoSize = false;
            SV_PM_BTN.BackColor = System.Drawing.Color.Transparent;
            SV_PM_BTN.ElevationLevel = 1;
            SV_PM_BTN.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            SV_PM_BTN.IconImage = Properties.Resources.extension_24dp;
            SV_PM_BTN.IconSize = 30;
            SV_PM_BTN.Location = new System.Drawing.Point(944, 694);
            SV_PM_BTN.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SV_PM_BTN.Name = "SV_PM_BTN";
            SV_PM_BTN.Size = new System.Drawing.Size(72, 72);
            SV_PM_BTN.TabIndex = 24;
            SV_PM_BTN.Click += SV_PM_BTN_Click;
            // 
            // StartServerBTN_New
            // 
            StartServerBTN_New.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            StartServerBTN_New.AutoSize = false;
            StartServerBTN_New.BackColor = System.Drawing.Color.Transparent;
            StartServerBTN_New.ElevationLevel = 1;
            StartServerBTN_New.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            StartServerBTN_New.IconImage = Properties.Resources.power_settings_new_24dp_green;
            StartServerBTN_New.IconSize = 32;
            StartServerBTN_New.Location = new System.Drawing.Point(901, 608);
            StartServerBTN_New.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            StartServerBTN_New.Name = "StartServerBTN_New";
            StartServerBTN_New.Size = new System.Drawing.Size(74, 74);
            StartServerBTN_New.TabIndex = 23;
            StartServerBTN_New.Click += StartServerBTN_New_Click;
            // 
            // SvConfigButton
            // 
            SvConfigButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            SvConfigButton.AutoSize = false;
            SvConfigButton.BackColor = System.Drawing.Color.Transparent;
            SvConfigButton.ElevationLevel = 1;
            SvConfigButton.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            SvConfigButton.IconImage = Properties.Resources.round_build_white_24dp;
            SvConfigButton.IconSize = 30;
            SvConfigButton.Location = new System.Drawing.Point(860, 694);
            SvConfigButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SvConfigButton.Name = "SvConfigButton";
            SvConfigButton.Size = new System.Drawing.Size(72, 72);
            SvConfigButton.TabIndex = 22;
            SvConfigButton.Click += SvConfigButton_Click;
            // 
            // StopServerBTN_New
            // 
            StopServerBTN_New.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            StopServerBTN_New.AutoSize = false;
            StopServerBTN_New.BackColor = System.Drawing.Color.Transparent;
            StopServerBTN_New.ElevationLevel = 1;
            StopServerBTN_New.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            StopServerBTN_New.IconImage = Properties.Resources.power_settings_new_24dp_red;
            StopServerBTN_New.IconSize = 32;
            StopServerBTN_New.Location = new System.Drawing.Point(901, 608);
            StopServerBTN_New.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            StopServerBTN_New.Name = "StopServerBTN_New";
            StopServerBTN_New.Size = new System.Drawing.Size(74, 74);
            StopServerBTN_New.TabIndex = 21;
            StopServerBTN_New.Visible = false;
            StopServerBTN_New.Click += StopServerBTN_New_Click;
            // 
            // CommandInputTextBox
            // 
            CommandInputTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CommandInputTextBox.BackColor = System.Drawing.Color.Transparent;
            CommandInputTextBox.Hint = "Enter Console Command";
            CommandInputTextBox.Location = new System.Drawing.Point(14, 716);
            CommandInputTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CommandInputTextBox.Name = "CommandInputTextBox";
            CommandInputTextBox.Size = new System.Drawing.Size(858, 50);
            CommandInputTextBox.TabIndex = 1;
            CommandInputTextBox.TabStop = false;
            CommandInputTextBox.KeyPress += CommandInputTextBox_KeyPress;
            CommandInputTextBox.KeyDown += CommandInputTextBox_KeyDown;
            // 
            // ConsoleOutputTextBox
            // 
            ConsoleOutputTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ConsoleOutputTextBox.BackColor = System.Drawing.Color.FromArgb(12, 15, 16);
            ConsoleOutputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            ConsoleOutputTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            ConsoleOutputTextBox.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            ConsoleOutputTextBox.Location = new System.Drawing.Point(14, 14);
            ConsoleOutputTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ConsoleOutputTextBox.Name = "ConsoleOutputTextBox";
            ConsoleOutputTextBox.ReadOnly = true;
            ConsoleOutputTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            ConsoleOutputTextBox.Size = new System.Drawing.Size(858, 690);
            ConsoleOutputTextBox.TabIndex = 0;
            ConsoleOutputTextBox.Text = "";
            // 
            // tabLiveMonitor Controls addition
            // 
            tabLiveMonitor.Controls.Add(SV_PM_BTN);
            tabLiveMonitor.Controls.Add(StartServerBTN_New);
            tabLiveMonitor.Controls.Add(SvConfigButton);
            tabLiveMonitor.Controls.Add(StopServerBTN_New);
            tabLiveMonitor.Controls.Add(CommandInputTextBox);
            tabLiveMonitor.Controls.Add(ConsoleOutputTextBox);
            // 
            // BkRsTab
            // 
            BkRsTab.BackColor = System.Drawing.Color.FromArgb(17, 20, 21);
            BkRsTab.Controls.Add(RestoreWrapper);
            BkRsTab.Controls.Add(BackupWrapper);
            BkRsTab.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            BkRsTab.Location = new System.Drawing.Point(4, 24);
            BkRsTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BkRsTab.Name = "BkRsTab";
            BkRsTab.Padding = new System.Windows.Forms.Padding(12);
            BkRsTab.Size = new System.Drawing.Size(1036, 784);
            BkRsTab.TabIndex = 1;
            BkRsTab.Text = "Backup & Restore";
            // 
            // RestoreWrapper
            // 
            RestoreWrapper.BackColor = System.Drawing.Color.FromArgb(29, 37, 41);
            RestoreWrapper.Controls.Add(RunRestoreBTN);
            RestoreWrapper.Controls.Add(BackupPointsListBox);
            RestoreWrapper.Controls.Add(RestoreTypeComboBox);
            RestoreWrapper.Controls.Add(RestorTitleLBL);
            RestoreWrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            RestoreWrapper.Location = new System.Drawing.Point(12, 122);
            RestoreWrapper.Margin = new System.Windows.Forms.Padding(16);
            RestoreWrapper.Name = "RestoreWrapper";
            RestoreWrapper.Padding = new System.Windows.Forms.Padding(16);
            RestoreWrapper.Size = new System.Drawing.Size(1012, 650);
            RestoreWrapper.TabIndex = 1;
            RestoreWrapper.Variant = MaterialCardVariant.Elevated;
            // 
            // RunRestoreBTN
            // 
            RunRestoreBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            RunRestoreBTN.BackColor = System.Drawing.Color.Transparent;
            RunRestoreBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            RunRestoreBTN.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            RunRestoreBTN.ForeColor = System.Drawing.Color.FromArgb(0, 53, 70);
            RunRestoreBTN.Location = new System.Drawing.Point(20, 570);
            RunRestoreBTN.Name = "RunRestoreBTN";
            RunRestoreBTN.Size = new System.Drawing.Size(330, 56);
            RunRestoreBTN.TabIndex = 3;
            RunRestoreBTN.Text = "Restore Selected Snapshot";
            RunRestoreBTN.UseVisualStyleBackColor = false;
            RunRestoreBTN.Visible = false;
            RunRestoreBTN.Click += RunRestoreBTN_Click;
            // 
            // BackupPointsListBox
            // 
            BackupPointsListBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            BackupPointsListBox.BackColor = System.Drawing.Color.FromArgb(25, 28, 30);
            BackupPointsListBox.Location = new System.Drawing.Point(370, 56);
            BackupPointsListBox.Name = "BackupPointsListBox";
            BackupPointsListBox.OutlineColor = System.Drawing.Color.FromArgb(64, 72, 76);
            BackupPointsListBox.Padding = new System.Windows.Forms.Padding(4);
            BackupPointsListBox.Size = new System.Drawing.Size(624, 572);
            BackupPointsListBox.TabIndex = 2;
            BackupPointsListBox.SelectedIndexChanged += BackupPointsListBox_SelectedIndexChanged;
            // 
            // RestoreTypeComboBox
            // 
            RestoreTypeComboBox.BackColor = System.Drawing.Color.Transparent;
            RestoreTypeComboBox.Hint = "Snapshot Category";
            RestoreTypeComboBox.Items.AddRange(new object[] { "Worlds Backups", "Packs/Addons Backups", "Full Server Backups" });
            RestoreTypeComboBox.Location = new System.Drawing.Point(20, 56);
            RestoreTypeComboBox.Name = "RestoreTypeComboBox";
            RestoreTypeComboBox.Size = new System.Drawing.Size(330, 49);
            RestoreTypeComboBox.TabIndex = 1;
            RestoreTypeComboBox.SelectedIndexChanged += RestoreTypeComboBox_SelectedIndexChanged;
            // 
            // RestorTitleLBL
            // 
            RestorTitleLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            RestorTitleLBL.BackColor = System.Drawing.Color.Transparent;
            RestorTitleLBL.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            RestorTitleLBL.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            RestorTitleLBL.Location = new System.Drawing.Point(16, 14);
            RestorTitleLBL.Name = "RestorTitleLBL";
            RestorTitleLBL.Size = new System.Drawing.Size(980, 28);
            RestorTitleLBL.TabIndex = 0;
            RestorTitleLBL.Text = "Server Snapshot Recovery";
            RestorTitleLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BackupWrapper
            // 
            BackupWrapper.BackColor = System.Drawing.Color.FromArgb(29, 37, 41);
            BackupWrapper.Controls.Add(BackupProgressBar);
            BackupWrapper.Controls.Add(BKCompleteLBL);
            BackupWrapper.Controls.Add(BackupServerBTN);
            BackupWrapper.Controls.Add(BackupPacksBTN);
            BackupWrapper.Controls.Add(BackupWorldBTN);
            BackupWrapper.Dock = System.Windows.Forms.DockStyle.Top;
            BackupWrapper.Location = new System.Drawing.Point(12, 12);
            BackupWrapper.Margin = new System.Windows.Forms.Padding(16);
            BackupWrapper.Name = "BackupWrapper";
            BackupWrapper.Padding = new System.Windows.Forms.Padding(16);
            BackupWrapper.Size = new System.Drawing.Size(1012, 110);
            BackupWrapper.TabIndex = 0;
            BackupWrapper.Variant = MaterialCardVariant.Elevated;
            // 
            // BackupProgressBar
            // 
            BackupProgressBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            BackupProgressBar.AutoSize = false;
            BackupProgressBar.BackColor = System.Drawing.Color.Transparent;
            BackupProgressBar.Location = new System.Drawing.Point(16, 98);
            BackupProgressBar.Name = "BackupProgressBar";
            BackupProgressBar.Size = new System.Drawing.Size(980, 6);
            BackupProgressBar.TabIndex = 5;
            BackupProgressBar.TrackThickness = 6;
            BackupProgressBar.Value = 75;
            BackupProgressBar.Visible = false;
            // 
            // BKCompleteLBL
            // 
            BKCompleteLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            BKCompleteLBL.AutoSize = true;
            BKCompleteLBL.BackColor = System.Drawing.Color.Transparent;
            BKCompleteLBL.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            BKCompleteLBL.ForeColor = System.Drawing.Color.FromArgb(101, 211, 255);
            BKCompleteLBL.Location = new System.Drawing.Point(804, 42);
            BKCompleteLBL.Name = "BKCompleteLBL";
            BKCompleteLBL.Size = new System.Drawing.Size(126, 19);
            BKCompleteLBL.TabIndex = 3;
            BKCompleteLBL.Text = "Backup Complete!";
            BKCompleteLBL.Visible = false;
            // 
            // BackupServerBTN
            // 
            BackupServerBTN.BackColor = System.Drawing.Color.Transparent;
            BackupServerBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BackupServerBTN.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            BackupServerBTN.ForeColor = System.Drawing.Color.FromArgb(0, 53, 70);
            BackupServerBTN.Location = new System.Drawing.Point(476, 16);
            BackupServerBTN.Name = "BackupServerBTN";
            BackupServerBTN.Size = new System.Drawing.Size(202, 72);
            BackupServerBTN.TabIndex = 2;
            BackupServerBTN.Text = "Full Server Backup";
            BackupServerBTN.UseVisualStyleBackColor = false;
            BackupServerBTN.Click += BackupServerBTN_Click;
            // 
            // BackupPacksBTN
            // 
            BackupPacksBTN.BackColor = System.Drawing.Color.Transparent;
            BackupPacksBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BackupPacksBTN.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            BackupPacksBTN.ForeColor = System.Drawing.Color.FromArgb(0, 53, 70);
            BackupPacksBTN.Location = new System.Drawing.Point(262, 16);
            BackupPacksBTN.Name = "BackupPacksBTN";
            BackupPacksBTN.Size = new System.Drawing.Size(202, 72);
            BackupPacksBTN.TabIndex = 1;
            BackupPacksBTN.Text = "Backup Packs";
            BackupPacksBTN.UseVisualStyleBackColor = false;
            BackupPacksBTN.Click += BackupPacksBTN_Click;
            // 
            // BackupWorldBTN
            // 
            BackupWorldBTN.BackColor = System.Drawing.Color.Transparent;
            BackupWorldBTN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            BackupWorldBTN.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F);
            BackupWorldBTN.ForeColor = System.Drawing.Color.FromArgb(0, 53, 70);
            BackupWorldBTN.Location = new System.Drawing.Point(16, 16);
            BackupWorldBTN.Name = "BackupWorldBTN";
            BackupWorldBTN.Size = new System.Drawing.Size(234, 72);
            BackupWorldBTN.TabIndex = 0;
            BackupWorldBTN.Text = "Backup World(s)";
            BackupWorldBTN.UseVisualStyleBackColor = false;
            BackupWorldBTN.Click += BackupWorldBTN_Click;
            // 
            // AutoModTab
            // 
            AutoModTab.BackColor = System.Drawing.Color.FromArgb(17, 20, 21);
            AutoModTab.Controls.Add(label2);
            AutoModTab.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            AutoModTab.Location = new System.Drawing.Point(4, 24);
            AutoModTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AutoModTab.Name = "AutoModTab";
            AutoModTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AutoModTab.Size = new System.Drawing.Size(1004, 706);
            AutoModTab.TabIndex = 2;
            AutoModTab.Text = "Chat AutoMod";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label2.ForeColor = System.Drawing.Color.FromArgb(192, 200, 205);
            label2.Location = new System.Drawing.Point(180, 200);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(598, 96);
            label2.TabIndex = 2;
            label2.Text = "This is a dummy tab(AutoMod).\r\nIn release you should never see this. \r\nIf you see this you're somewhere you shouldnt be.\r\nMaybe you should come work for me :D";
            // 
            // LETab
            // 
            LETab.BackColor = System.Drawing.Color.FromArgb(17, 20, 21);
            LETab.Controls.Add(editorProvider1);
            LETab.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            LETab.Location = new System.Drawing.Point(4, 24);
            LETab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            LETab.Name = "LETab";
            LETab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            LETab.Size = new System.Drawing.Size(1004, 706);
            LETab.TabIndex = 4;
            LETab.Text = "Level Editor";
            // 
            // editorProvider1
            // 
            editorProvider1.AutoScroll = true;
            editorProvider1.BackColor = System.Drawing.Color.FromArgb(17, 20, 21);
            editorProvider1.Dock = System.Windows.Forms.DockStyle.Fill;
            editorProvider1.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            editorProvider1.Location = new System.Drawing.Point(4, 3);
            editorProvider1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            editorProvider1.Name = "editorProvider1";
            editorProvider1.Size = new System.Drawing.Size(996, 700);
            editorProvider1.TabIndex = 0;
            // 
            // SettingsTabPage
            // 
            SettingsTabPage.BackColor = System.Drawing.Color.FromArgb(17, 20, 21);
            SettingsTabPage.Controls.Add(settingsProvider1);
            SettingsTabPage.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            SettingsTabPage.Location = new System.Drawing.Point(4, 24);
            SettingsTabPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SettingsTabPage.Name = "SettingsTabPage";
            SettingsTabPage.Size = new System.Drawing.Size(1004, 706);
            SettingsTabPage.TabIndex = 3;
            SettingsTabPage.Text = "Settings";
            // 
            // settingsProvider1
            // 
            settingsProvider1.AutoScroll = true;
            settingsProvider1.AutoScrollMinSize = new System.Drawing.Size(0, 765);
            settingsProvider1.BackColor = System.Drawing.Color.FromArgb(17, 20, 21);
            settingsProvider1.Dock = System.Windows.Forms.DockStyle.Fill;
            settingsProvider1.ForeColor = System.Drawing.Color.FromArgb(225, 226, 228);
            settingsProvider1.Location = new System.Drawing.Point(0, 0);
            settingsProvider1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            settingsProvider1.Name = "settingsProvider1";
            settingsProvider1.Size = new System.Drawing.Size(1004, 706);
            settingsProvider1.TabIndex = 0;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(1266, 854);
            Controls.Add(MaterialTabControl1);
            Controls.Add(MainNavDrawer);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "MainWindow";
            Sizable = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Minecraft BDS Utility";
            FormClosing += MainWindow_FormClosing;
            Load += MainWindow_Load;
            Controls.SetChildIndex(MainNavDrawer, 0);
            Controls.SetChildIndex(MaterialTabControl1, 0);
            MaterialTabControl1.ResumeLayout(false);
            tabLiveMonitor.ResumeLayout(false);
            BkRsTab.ResumeLayout(false);
            RestoreWrapper.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)RestoreTypeComboBox).EndInit();
            BackupWrapper.ResumeLayout(false);
            BackupWrapper.PerformLayout();
            AutoModTab.ResumeLayout(false);
            AutoModTab.PerformLayout();
            LETab.ResumeLayout(false);
            SettingsTabPage.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MaterialNavigationDrawer MainNavDrawer;
        private BDSUtil.UI.MaterialTabControl MaterialTabControl1;
        private System.Windows.Forms.TabPage tabLiveMonitor;
        private System.Windows.Forms.TabPage BkRsTab;
        private System.Windows.Forms.RichTextBox ConsoleOutputTextBox;
        private BDSUtil.UI.MaterialTextBoxEdit CommandInputTextBox;
        private System.Windows.Forms.TabPage SettingsTabPage;
        private MaterialCard RestoreWrapper;
        private MaterialCard BackupWrapper;
        private MaterialButton BackupServerBTN;
        private MaterialButton BackupPacksBTN;
        private MaterialButton BackupWorldBTN;
        private MaterialLabel BKCompleteLBL;
        private MaterialProgressBar BackupProgressBar;
        private MaterialLabel RestorTitleLBL;
        private BDSUtil.UI.MaterialListBox BackupPointsListBox;
        private BDSUtil.UI.MaterialComboBox RestoreTypeComboBox;
        private MaterialButton RunRestoreBTN;
        private MaterialFab StopServerBTN_New;
        private MaterialFab StartServerBTN_New;
        private MaterialFab SvConfigButton;
        private System.Windows.Forms.TabPage AutoModTab;
        private System.Windows.Forms.Label label2;
        private Modals.Content_Controls.Settings.SettingsProvider settingsProvider1;
        private System.Windows.Forms.TabPage LETab;
        private Modals.Content_Controls.Level_Editor.EditorProvider editorProvider1;
        private MaterialFab SV_PM_BTN;
    }
}
