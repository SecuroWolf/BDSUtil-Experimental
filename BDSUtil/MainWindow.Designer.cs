namespace BDSUtil
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            MaterialTabControl1 = new ReaLTaiizor.Controls.MaterialTabControl();
            tabLiveMonitor = new System.Windows.Forms.TabPage();
            SV_PM_BTN = new ReaLTaiizor.Controls.MaterialFloatingActionButton();
            IconList = new System.Windows.Forms.ImageList(components);
            StartServerBTN_New = new ReaLTaiizor.Controls.MaterialFloatingActionButton();
            SvConfigButton = new ReaLTaiizor.Controls.MaterialFloatingActionButton();
            StopServerBTN_New = new ReaLTaiizor.Controls.MaterialFloatingActionButton();
            CommandInputTextBox = new ReaLTaiizor.Controls.MaterialTextBoxEdit();
            ConsoleOutputTextBox = new System.Windows.Forms.RichTextBox();
            BkRsTab = new System.Windows.Forms.TabPage();
            BackupWrapper = new ReaLTaiizor.Controls.MaterialCard();
            BackupProgressBar = new ReaLTaiizor.Controls.MaterialProgressBar();
            BKCompleteLBL = new ReaLTaiizor.Controls.MaterialLabel();
            BackupServerBTN = new ReaLTaiizor.Controls.MaterialButton();
            BackupPacksBTN = new ReaLTaiizor.Controls.MaterialButton();
            BackupWorldBTN = new ReaLTaiizor.Controls.MaterialButton();
            RestoreWrapper = new ReaLTaiizor.Controls.MaterialCard();
            RunRestoreBTN = new ReaLTaiizor.Controls.MaterialButton();
            BackupPointsListBox = new ReaLTaiizor.Controls.MaterialListBox();
            RestoreTypeComboBox = new ReaLTaiizor.Controls.MaterialComboBox();
            RestorTitleLBL = new ReaLTaiizor.Controls.MaterialLabel();
            AutoModTab = new System.Windows.Forms.TabPage();
            label2 = new System.Windows.Forms.Label();
            LETab = new System.Windows.Forms.TabPage();
            editorProvider1 = new BDSUtil.Modals.Content_Controls.Level_Editor.EditorProvider();
            SettingsTabPage = new System.Windows.Forms.TabPage();
            settingsProvider1 = new BDSUtil.Modals.Content_Controls.Settings.SettingsProvider();
            object_7f028359_dd1b_422d_83c9_6faf6dccb105 = new ReaLTaiizor.Controls.MaterialTabControl();
            MaterialTabControl1.SuspendLayout();
            tabLiveMonitor.SuspendLayout();
            BkRsTab.SuspendLayout();
            BackupWrapper.SuspendLayout();
            RestoreWrapper.SuspendLayout();
            AutoModTab.SuspendLayout();
            LETab.SuspendLayout();
            SettingsTabPage.SuspendLayout();
            SuspendLayout();
            // 
            // MaterialTabControl1
            // 
            MaterialTabControl1.Controls.Add(tabLiveMonitor);
            MaterialTabControl1.Controls.Add(BkRsTab);
            MaterialTabControl1.Controls.Add(AutoModTab);
            MaterialTabControl1.Controls.Add(LETab);
            MaterialTabControl1.Controls.Add(SettingsTabPage);
            MaterialTabControl1.Depth = 0;
            MaterialTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            MaterialTabControl1.ImageList = IconList;
            MaterialTabControl1.Location = new System.Drawing.Point(0, 74);
            MaterialTabControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaterialTabControl1.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            MaterialTabControl1.Multiline = true;
            MaterialTabControl1.Name = "MaterialTabControl1";
            MaterialTabControl1.SelectedIndex = 0;
            MaterialTabControl1.Size = new System.Drawing.Size(950, 558);
            MaterialTabControl1.TabIndex = 0;
            MaterialTabControl1.SelectedIndexChanged += MaterialTabControl1_SelectedIndexChanged;
            // 
            // tabLiveMonitor
            // 
            tabLiveMonitor.Controls.Add(SV_PM_BTN);
            tabLiveMonitor.Controls.Add(StartServerBTN_New);
            tabLiveMonitor.Controls.Add(SvConfigButton);
            tabLiveMonitor.Controls.Add(StopServerBTN_New);
            tabLiveMonitor.Controls.Add(CommandInputTextBox);
            tabLiveMonitor.Controls.Add(ConsoleOutputTextBox);
            tabLiveMonitor.ImageKey = "terminal_24dp.png";
            tabLiveMonitor.Location = new System.Drawing.Point(4, 31);
            tabLiveMonitor.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabLiveMonitor.Name = "tabLiveMonitor";
            tabLiveMonitor.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabLiveMonitor.Size = new System.Drawing.Size(942, 523);
            tabLiveMonitor.TabIndex = 0;
            tabLiveMonitor.Text = "Server Console";
            tabLiveMonitor.UseVisualStyleBackColor = true;
            // 
            // SV_PM_BTN
            // 
            SV_PM_BTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            SV_PM_BTN.Depth = 0;
            SV_PM_BTN.Icon = Properties.Resources.extension_24dp;
            SV_PM_BTN.ImageList = IconList;
            SV_PM_BTN.Location = new System.Drawing.Point(873, 449);
            SV_PM_BTN.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SV_PM_BTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            SV_PM_BTN.Name = "SV_PM_BTN";
            SV_PM_BTN.Size = new System.Drawing.Size(65, 65);
            SV_PM_BTN.TabIndex = 24;
            SV_PM_BTN.Text = "materialFloatingActionButton1";
            SV_PM_BTN.UseVisualStyleBackColor = true;
            SV_PM_BTN.Click += SV_PM_BTN_Click;
            // 
            // IconList
            // 
            IconList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            IconList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("IconList.ImageStream");
            IconList.TransparentColor = System.Drawing.Color.Transparent;
            IconList.Images.SetKeyName(0, "baseline_bluetooth_black_24dp.png");
            IconList.Images.SetKeyName(1, "baseline_build_black_24dp.png");
            IconList.Images.SetKeyName(2, "baseline_favorite_border_black_24dp.png");
            IconList.Images.SetKeyName(3, "baseline_fingerprint_black_24dp.png");
            IconList.Images.SetKeyName(4, "baseline_thumb_up_black_24dp.png");
            IconList.Images.SetKeyName(5, "ic_settings_black_24dp_1x.png");
            IconList.Images.SetKeyName(6, "ic_settings_white_24dp_1x.png");
            IconList.Images.SetKeyName(7, "minus.png");
            IconList.Images.SetKeyName(8, "plus.png");
            IconList.Images.SetKeyName(9, "round_add_black_24dp.png");
            IconList.Images.SetKeyName(10, "round_assessment_white_24dp.png");
            IconList.Images.SetKeyName(11, "round_backup_white_24dp.png");
            IconList.Images.SetKeyName(12, "round_bluetooth_white_24dp.png");
            IconList.Images.SetKeyName(13, "round_bookmark_white_24dp.png");
            IconList.Images.SetKeyName(14, "round_build_white_24dp.png");
            IconList.Images.SetKeyName(15, "round_gps_fixed_white_24dp.png");
            IconList.Images.SetKeyName(16, "round_http_white_24dp.png");
            IconList.Images.SetKeyName(17, "round_phone_black_24dp.png");
            IconList.Images.SetKeyName(18, "round_report_problem_white_24dp.png");
            IconList.Images.SetKeyName(19, "round_swap_vert_white_24dp.png");
            IconList.Images.SetKeyName(20, "terminal_24dp.png");
            IconList.Images.SetKeyName(21, "chat_info_24dp.png");
            IconList.Images.SetKeyName(22, "globe_24dp.png");
            // 
            // StartServerBTN_New
            // 
            StartServerBTN_New.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            StartServerBTN_New.Depth = 0;
            StartServerBTN_New.Icon = Properties.Resources.power_settings_new_24dp_green;
            StartServerBTN_New.ImageList = IconList;
            StartServerBTN_New.Location = new System.Drawing.Point(835, 376);
            StartServerBTN_New.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            StartServerBTN_New.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            StartServerBTN_New.Name = "StartServerBTN_New";
            StartServerBTN_New.Size = new System.Drawing.Size(65, 65);
            StartServerBTN_New.TabIndex = 23;
            StartServerBTN_New.Text = "materialFloatingActionButton3";
            StartServerBTN_New.UseVisualStyleBackColor = true;
            StartServerBTN_New.Click += StartServerBTN_New_Click;
            // 
            // SvConfigButton
            // 
            SvConfigButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            SvConfigButton.Depth = 0;
            SvConfigButton.Icon = Properties.Resources.round_build_white_24dp;
            SvConfigButton.ImageList = IconList;
            SvConfigButton.Location = new System.Drawing.Point(800, 449);
            SvConfigButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SvConfigButton.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            SvConfigButton.Name = "SvConfigButton";
            SvConfigButton.Size = new System.Drawing.Size(65, 65);
            SvConfigButton.TabIndex = 22;
            SvConfigButton.Text = "SvConfigButton";
            SvConfigButton.UseVisualStyleBackColor = true;
            SvConfigButton.Click += SvConfigButton_Click;
            // 
            // StopServerBTN_New
            // 
            StopServerBTN_New.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            StopServerBTN_New.Depth = 0;
            StopServerBTN_New.Icon = Properties.Resources.power_settings_new_24dp_red;
            StopServerBTN_New.ImageList = IconList;
            StopServerBTN_New.Location = new System.Drawing.Point(835, 376);
            StopServerBTN_New.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            StopServerBTN_New.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            StopServerBTN_New.Name = "StopServerBTN_New";
            StopServerBTN_New.Size = new System.Drawing.Size(65, 65);
            StopServerBTN_New.TabIndex = 21;
            StopServerBTN_New.Text = "materialFloatingActionButton1";
            StopServerBTN_New.UseVisualStyleBackColor = true;
            StopServerBTN_New.Visible = false;
            StopServerBTN_New.Click += StopServerBTN_New_Click;
            // 
            // CommandInputTextBox
            // 
            CommandInputTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            CommandInputTextBox.AnimateReadOnly = false;
            CommandInputTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            CommandInputTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            CommandInputTextBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            CommandInputTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            CommandInputTextBox.Depth = 0;
            CommandInputTextBox.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            CommandInputTextBox.HideSelection = true;
            CommandInputTextBox.Hint = "Enter Console Command";
            CommandInputTextBox.LeadingIcon = null;
            CommandInputTextBox.Location = new System.Drawing.Point(14, 449);
            CommandInputTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CommandInputTextBox.MaxLength = 32767;
            CommandInputTextBox.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            CommandInputTextBox.Name = "CommandInputTextBox";
            CommandInputTextBox.PasswordChar = '\0';
            CommandInputTextBox.PrefixSuffixText = null;
            CommandInputTextBox.ReadOnly = false;
            CommandInputTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            CommandInputTextBox.SelectedText = "";
            CommandInputTextBox.SelectionLength = 0;
            CommandInputTextBox.SelectionStart = 0;
            CommandInputTextBox.ShortcutsEnabled = true;
            CommandInputTextBox.Size = new System.Drawing.Size(770, 48);
            CommandInputTextBox.TabIndex = 1;
            CommandInputTextBox.TabStop = false;
            CommandInputTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            CommandInputTextBox.TrailingIcon = null;
            CommandInputTextBox.UseSystemPasswordChar = false;
            CommandInputTextBox.KeyDown += CommandInputTextBox_KeyDown;
            CommandInputTextBox.KeyPress += CommandInputTextBox_KeyPress;
            // 
            // ConsoleOutputTextBox
            // 
            ConsoleOutputTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            ConsoleOutputTextBox.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            ConsoleOutputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            ConsoleOutputTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            ConsoleOutputTextBox.ForeColor = System.Drawing.Color.White;
            ConsoleOutputTextBox.Location = new System.Drawing.Point(14, 14);
            ConsoleOutputTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ConsoleOutputTextBox.Name = "ConsoleOutputTextBox";
            ConsoleOutputTextBox.ReadOnly = true;
            ConsoleOutputTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            ConsoleOutputTextBox.Size = new System.Drawing.Size(770, 416);
            ConsoleOutputTextBox.TabIndex = 0;
            ConsoleOutputTextBox.Text = "";
            // 
            // BkRsTab
            // 
            BkRsTab.Controls.Add(BackupWrapper);
            BkRsTab.Controls.Add(RestoreWrapper);
            BkRsTab.ImageKey = "round_backup_white_24dp.png";
            BkRsTab.Location = new System.Drawing.Point(4, 31);
            BkRsTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BkRsTab.Name = "BkRsTab";
            BkRsTab.Padding = new System.Windows.Forms.Padding(4, 15, 4, 15);
            BkRsTab.Size = new System.Drawing.Size(942, 523);
            BkRsTab.TabIndex = 1;
            BkRsTab.Text = "Backup & Restore";
            BkRsTab.UseVisualStyleBackColor = true;
            // 
            // BackupWrapper
            // 
            BackupWrapper.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            BackupWrapper.Controls.Add(BackupProgressBar);
            BackupWrapper.Controls.Add(BKCompleteLBL);
            BackupWrapper.Controls.Add(BackupServerBTN);
            BackupWrapper.Controls.Add(BackupPacksBTN);
            BackupWrapper.Controls.Add(BackupWorldBTN);
            BackupWrapper.Depth = 0;
            BackupWrapper.Dock = System.Windows.Forms.DockStyle.Top;
            BackupWrapper.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            BackupWrapper.Location = new System.Drawing.Point(4, 15);
            BackupWrapper.Margin = new System.Windows.Forms.Padding(16);
            BackupWrapper.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            BackupWrapper.Name = "BackupWrapper";
            BackupWrapper.Padding = new System.Windows.Forms.Padding(16);
            BackupWrapper.Size = new System.Drawing.Size(934, 105);
            BackupWrapper.TabIndex = 0;
            // 
            // BackupProgressBar
            // 
            BackupProgressBar.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            BackupProgressBar.Depth = 0;
            BackupProgressBar.Location = new System.Drawing.Point(-142, 96);
            BackupProgressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BackupProgressBar.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            BackupProgressBar.Name = "BackupProgressBar";
            BackupProgressBar.Size = new System.Drawing.Size(1071, 6);
            BackupProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            BackupProgressBar.TabIndex = 5;
            BackupProgressBar.UseAccentColor = false;
            BackupProgressBar.Value = 75;
            BackupProgressBar.Visible = false;
            // 
            // BKCompleteLBL
            // 
            BKCompleteLBL.AutoSize = true;
            BKCompleteLBL.Depth = 0;
            BKCompleteLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            BKCompleteLBL.Location = new System.Drawing.Point(723, 44);
            BKCompleteLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            BKCompleteLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            BKCompleteLBL.Name = "BKCompleteLBL";
            BKCompleteLBL.Size = new System.Drawing.Size(129, 19);
            BKCompleteLBL.TabIndex = 3;
            BKCompleteLBL.Text = "Backup Complete!";
            BKCompleteLBL.Visible = false;
            // 
            // BackupServerBTN
            // 
            BackupServerBTN.AutoSize = false;
            BackupServerBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackupServerBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            BackupServerBTN.Depth = 0;
            BackupServerBTN.HighEmphasis = true;
            BackupServerBTN.Icon = null;
            BackupServerBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            BackupServerBTN.Location = new System.Drawing.Point(471, 16);
            BackupServerBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            BackupServerBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            BackupServerBTN.Name = "BackupServerBTN";
            BackupServerBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            BackupServerBTN.Size = new System.Drawing.Size(202, 73);
            BackupServerBTN.TabIndex = 2;
            BackupServerBTN.Text = "Full Server Backup";
            BackupServerBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            BackupServerBTN.UseAccentColor = false;
            BackupServerBTN.UseVisualStyleBackColor = true;
            BackupServerBTN.Click += BackupServerBTN_Click;
            // 
            // BackupPacksBTN
            // 
            BackupPacksBTN.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            BackupPacksBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackupPacksBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            BackupPacksBTN.Depth = 0;
            BackupPacksBTN.HighEmphasis = true;
            BackupPacksBTN.Icon = null;
            BackupPacksBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            BackupPacksBTN.Location = new System.Drawing.Point(260, 16);
            BackupPacksBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            BackupPacksBTN.MaximumSize = new System.Drawing.Size(202, 73);
            BackupPacksBTN.MinimumSize = new System.Drawing.Size(202, 73);
            BackupPacksBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            BackupPacksBTN.Name = "BackupPacksBTN";
            BackupPacksBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            BackupPacksBTN.Padding = new System.Windows.Forms.Padding(6);
            BackupPacksBTN.Size = new System.Drawing.Size(202, 73);
            BackupPacksBTN.TabIndex = 1;
            BackupPacksBTN.Text = "Backup Packs";
            BackupPacksBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            BackupPacksBTN.UseAccentColor = false;
            BackupPacksBTN.UseVisualStyleBackColor = true;
            BackupPacksBTN.Click += BackupPacksBTN_Click;
            // 
            // BackupWorldBTN
            // 
            BackupWorldBTN.AutoSize = false;
            BackupWorldBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BackupWorldBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            BackupWorldBTN.Depth = 0;
            BackupWorldBTN.Dock = System.Windows.Forms.DockStyle.Left;
            BackupWorldBTN.HighEmphasis = true;
            BackupWorldBTN.Icon = null;
            BackupWorldBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            BackupWorldBTN.Location = new System.Drawing.Point(16, 16);
            BackupWorldBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            BackupWorldBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            BackupWorldBTN.Name = "BackupWorldBTN";
            BackupWorldBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            BackupWorldBTN.Size = new System.Drawing.Size(234, 73);
            BackupWorldBTN.TabIndex = 0;
            BackupWorldBTN.Text = "Backup World(s)";
            BackupWorldBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            BackupWorldBTN.UseAccentColor = false;
            BackupWorldBTN.UseVisualStyleBackColor = true;
            BackupWorldBTN.Click += BackupWorldBTN_Click;
            // 
            // RestoreWrapper
            // 
            RestoreWrapper.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            RestoreWrapper.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            RestoreWrapper.Controls.Add(RunRestoreBTN);
            RestoreWrapper.Controls.Add(BackupPointsListBox);
            RestoreWrapper.Controls.Add(RestoreTypeComboBox);
            RestoreWrapper.Controls.Add(RestorTitleLBL);
            RestoreWrapper.Depth = 0;
            RestoreWrapper.Dock = System.Windows.Forms.DockStyle.Bottom;
            RestoreWrapper.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            RestoreWrapper.Location = new System.Drawing.Point(4, 103);
            RestoreWrapper.Margin = new System.Windows.Forms.Padding(16);
            RestoreWrapper.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            RestoreWrapper.Name = "RestoreWrapper";
            RestoreWrapper.Padding = new System.Windows.Forms.Padding(16);
            RestoreWrapper.Size = new System.Drawing.Size(934, 405);
            RestoreWrapper.TabIndex = 1;
            // 
            // RunRestoreBTN
            // 
            RunRestoreBTN.AutoSize = false;
            RunRestoreBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            RunRestoreBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            RunRestoreBTN.Depth = 0;
            RunRestoreBTN.HighEmphasis = true;
            RunRestoreBTN.Icon = null;
            RunRestoreBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            RunRestoreBTN.Location = new System.Drawing.Point(16, 329);
            RunRestoreBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            RunRestoreBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            RunRestoreBTN.Name = "RunRestoreBTN";
            RunRestoreBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            RunRestoreBTN.Size = new System.Drawing.Size(334, 57);
            RunRestoreBTN.TabIndex = 3;
            RunRestoreBTN.Text = "Restore Selected Snapshot";
            RunRestoreBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            RunRestoreBTN.UseAccentColor = false;
            RunRestoreBTN.UseVisualStyleBackColor = true;
            RunRestoreBTN.Visible = false;
            RunRestoreBTN.Click += RunRestoreBTN_Click;
            // 
            // BackupPointsListBox
            // 
            BackupPointsListBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            BackupPointsListBox.BackColor = System.Drawing.Color.White;
            BackupPointsListBox.BorderColor = System.Drawing.Color.LightGray;
            BackupPointsListBox.Depth = 0;
            BackupPointsListBox.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            BackupPointsListBox.Location = new System.Drawing.Point(373, 48);
            BackupPointsListBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BackupPointsListBox.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            BackupPointsListBox.Name = "BackupPointsListBox";
            BackupPointsListBox.SelectedIndex = -1;
            BackupPointsListBox.SelectedItem = null;
            BackupPointsListBox.Size = new System.Drawing.Size(541, 333);
            BackupPointsListBox.TabIndex = 2;
            BackupPointsListBox.SelectedIndexChanged += BackupPointsListBox_SelectedIndexChanged;
            // 
            // RestoreTypeComboBox
            // 
            RestoreTypeComboBox.AutoResize = false;
            RestoreTypeComboBox.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            RestoreTypeComboBox.Depth = 0;
            RestoreTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            RestoreTypeComboBox.DropDownHeight = 174;
            RestoreTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            RestoreTypeComboBox.DropDownWidth = 121;
            RestoreTypeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            RestoreTypeComboBox.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            RestoreTypeComboBox.FormattingEnabled = true;
            RestoreTypeComboBox.IntegralHeight = false;
            RestoreTypeComboBox.ItemHeight = 43;
            RestoreTypeComboBox.Items.AddRange(new object[] { "Worlds Backups", "Packs/Addons Backups", "Full Server Backups" });
            RestoreTypeComboBox.Location = new System.Drawing.Point(20, 48);
            RestoreTypeComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RestoreTypeComboBox.MaxDropDownItems = 4;
            RestoreTypeComboBox.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            RestoreTypeComboBox.Name = "RestoreTypeComboBox";
            RestoreTypeComboBox.Size = new System.Drawing.Size(330, 49);
            RestoreTypeComboBox.StartIndex = 0;
            RestoreTypeComboBox.TabIndex = 1;
            RestoreTypeComboBox.SelectedIndexChanged += RestoreTypeComboBox_SelectedIndexChanged;
            // 
            // RestorTitleLBL
            // 
            RestorTitleLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            RestorTitleLBL.AutoSize = true;
            RestorTitleLBL.Depth = 0;
            RestorTitleLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            RestorTitleLBL.Location = new System.Drawing.Point(410, 12);
            RestorTitleLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            RestorTitleLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            RestorTitleLBL.Name = "RestorTitleLBL";
            RestorTitleLBL.Size = new System.Drawing.Size(185, 19);
            RestorTitleLBL.TabIndex = 0;
            RestorTitleLBL.Text = "Server Snapshot Recovery";
            // 
            // AutoModTab
            // 
            AutoModTab.Controls.Add(label2);
            AutoModTab.ImageKey = "chat_info_24dp.png";
            AutoModTab.Location = new System.Drawing.Point(4, 31);
            AutoModTab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AutoModTab.Name = "AutoModTab";
            AutoModTab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AutoModTab.Size = new System.Drawing.Size(942, 523);
            AutoModTab.TabIndex = 2;
            AutoModTab.Text = "Chat AutoMod";
            AutoModTab.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            label2.Location = new System.Drawing.Point(201, 203);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(598, 96);
            label2.TabIndex = 2;
            label2.Text = "This is a dummy tab(AutoMod).\r\nIn release you should never see this. \r\nIf you see this you're somewhere you shouldnt be.\r\nMaybe you should come work for me :D";
            // 
            // LETab
            // 
            LETab.Controls.Add(editorProvider1);
            LETab.ImageKey = "globe_24dp.png";
            LETab.Location = new System.Drawing.Point(4, 31);
            LETab.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            LETab.Name = "LETab";
            LETab.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            LETab.Size = new System.Drawing.Size(942, 523);
            LETab.TabIndex = 4;
            LETab.Text = "Level Editor";
            LETab.UseVisualStyleBackColor = true;
            // 
            // editorProvider1
            // 
            editorProvider1.AutoScroll = true;
            editorProvider1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            editorProvider1.Dock = System.Windows.Forms.DockStyle.Fill;
            editorProvider1.Location = new System.Drawing.Point(4, 3);
            editorProvider1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            editorProvider1.Name = "editorProvider1";
            editorProvider1.Size = new System.Drawing.Size(934, 517);
            editorProvider1.TabIndex = 0;
            // 
            // SettingsTabPage
            // 
            SettingsTabPage.Controls.Add(settingsProvider1);
            SettingsTabPage.ImageKey = "ic_settings_white_24dp_1x.png";
            SettingsTabPage.Location = new System.Drawing.Point(4, 31);
            SettingsTabPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SettingsTabPage.Name = "SettingsTabPage";
            SettingsTabPage.Size = new System.Drawing.Size(942, 523);
            SettingsTabPage.TabIndex = 3;
            SettingsTabPage.Text = "Settings";
            SettingsTabPage.UseVisualStyleBackColor = true;
            // 
            // settingsProvider1
            // 
            settingsProvider1.AutoScroll = true;
            settingsProvider1.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            settingsProvider1.Dock = System.Windows.Forms.DockStyle.Fill;
            settingsProvider1.Location = new System.Drawing.Point(0, 0);
            settingsProvider1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            settingsProvider1.Name = "settingsProvider1";
            settingsProvider1.Size = new System.Drawing.Size(942, 523);
            settingsProvider1.TabIndex = 0;
            // 
            // object_7f028359_dd1b_422d_83c9_6faf6dccb105
            // 
            object_7f028359_dd1b_422d_83c9_6faf6dccb105.Depth = 0;
            object_7f028359_dd1b_422d_83c9_6faf6dccb105.Dock = System.Windows.Forms.DockStyle.Fill;
            object_7f028359_dd1b_422d_83c9_6faf6dccb105.Location = new System.Drawing.Point(0, 64);
            object_7f028359_dd1b_422d_83c9_6faf6dccb105.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            object_7f028359_dd1b_422d_83c9_6faf6dccb105.Multiline = true;
            object_7f028359_dd1b_422d_83c9_6faf6dccb105.Name = "object_7f028359_dd1b_422d_83c9_6faf6dccb105";
            object_7f028359_dd1b_422d_83c9_6faf6dccb105.SelectedIndex = 0;
            object_7f028359_dd1b_422d_83c9_6faf6dccb105.Size = new System.Drawing.Size(950, 483);
            object_7f028359_dd1b_422d_83c9_6faf6dccb105.TabIndex = 0;
            object_7f028359_dd1b_422d_83c9_6faf6dccb105.Visible = false;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(950, 635);
            Controls.Add(MaterialTabControl1);
            DrawerShowIconsWhenHidden = true;
            DrawerTabControl = MaterialTabControl1;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "MainWindow";
            Padding = new System.Windows.Forms.Padding(0, 74, 0, 3);
            Sizable = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Minecraft BDS Utility";
            FormClosing += MainWindow_FormClosing;
            Load += MainWindow_Load;
            MaterialTabControl1.ResumeLayout(false);
            tabLiveMonitor.ResumeLayout(false);
            BkRsTab.ResumeLayout(false);
            BackupWrapper.ResumeLayout(false);
            BackupWrapper.PerformLayout();
            RestoreWrapper.ResumeLayout(false);
            RestoreWrapper.PerformLayout();
            AutoModTab.ResumeLayout(false);
            AutoModTab.PerformLayout();
            LETab.ResumeLayout(false);
            SettingsTabPage.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.MaterialTabControl MaterialTabControl1;
        private System.Windows.Forms.TabPage tabLiveMonitor;
        private System.Windows.Forms.TabPage BkRsTab;
        private System.Windows.Forms.RichTextBox ConsoleOutputTextBox;
        private ReaLTaiizor.Controls.MaterialTextBoxEdit CommandInputTextBox;
        private System.Windows.Forms.TabPage SettingsTabPage;
        private ReaLTaiizor.Controls.MaterialCard RestoreWrapper;
        private ReaLTaiizor.Controls.MaterialCard BackupWrapper;
        private ReaLTaiizor.Controls.MaterialButton BackupServerBTN;
        private ReaLTaiizor.Controls.MaterialButton BackupPacksBTN;
        private ReaLTaiizor.Controls.MaterialButton BackupWorldBTN;
        private ReaLTaiizor.Controls.MaterialLabel BKCompleteLBL;
        private ReaLTaiizor.Controls.MaterialProgressBar BackupProgressBar;
        private ReaLTaiizor.Controls.MaterialLabel RestorTitleLBL;
        private ReaLTaiizor.Controls.MaterialListBox BackupPointsListBox;
        private ReaLTaiizor.Controls.MaterialComboBox RestoreTypeComboBox;
        private ReaLTaiizor.Controls.MaterialButton RunRestoreBTN;
        private ReaLTaiizor.Controls.MaterialFloatingActionButton StopServerBTN_New;
        private ReaLTaiizor.Controls.MaterialFloatingActionButton StartServerBTN_New;
        private ReaLTaiizor.Controls.MaterialFloatingActionButton SvConfigButton;
        private System.Windows.Forms.ImageList IconList;
        private System.Windows.Forms.TabPage AutoModTab;
        private System.Windows.Forms.Label label2;
        private Modals.Content_Controls.Settings.SettingsProvider settingsProvider1;
        private System.Windows.Forms.TabPage LETab;
        private ReaLTaiizor.Controls.MaterialTabControl object_7f028359_dd1b_422d_83c9_6faf6dccb105;
        private Modals.Content_Controls.Level_Editor.EditorProvider editorProvider1;
        private ReaLTaiizor.Controls.MaterialFloatingActionButton SV_PM_BTN;
    }
}
