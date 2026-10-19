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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.MaterialTabControl1 = new ReaLTaiizor.Controls.MaterialTabControl();
            this.tabLiveMonitor = new System.Windows.Forms.TabPage();
            this.StartServerBTN_New = new ReaLTaiizor.Controls.MaterialFloatingActionButton();
            this.IconList = new System.Windows.Forms.ImageList(this.components);
            this.SvConfigButton = new ReaLTaiizor.Controls.MaterialFloatingActionButton();
            this.StopServerBTN_New = new ReaLTaiizor.Controls.MaterialFloatingActionButton();
            this.CommandInputTextBox = new ReaLTaiizor.Controls.MaterialTextBoxEdit();
            this.ConsoleOutputTextBox = new System.Windows.Forms.RichTextBox();
            this.BkRsTab = new System.Windows.Forms.TabPage();
            this.BackupWrapper = new ReaLTaiizor.Controls.MaterialCard();
            this.BackupProgressBar = new ReaLTaiizor.Controls.MaterialProgressBar();
            this.BKCompleteLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.BackupServerBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.BackupPacksBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.BackupWorldBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.RestoreWrapper = new ReaLTaiizor.Controls.MaterialCard();
            this.RunRestoreBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.BackupPointsListBox = new ReaLTaiizor.Controls.MaterialListBox();
            this.RestoreTypeComboBox = new ReaLTaiizor.Controls.MaterialComboBox();
            this.RestorTitleLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.SettingsTabPage = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.MaterialTabControl1.SuspendLayout();
            this.tabLiveMonitor.SuspendLayout();
            this.BkRsTab.SuspendLayout();
            this.BackupWrapper.SuspendLayout();
            this.RestoreWrapper.SuspendLayout();
            this.SettingsTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // MaterialTabControl1
            // 
            this.MaterialTabControl1.Controls.Add(this.tabLiveMonitor);
            this.MaterialTabControl1.Controls.Add(this.BkRsTab);
            this.MaterialTabControl1.Controls.Add(this.SettingsTabPage);
            this.MaterialTabControl1.Depth = 0;
            this.MaterialTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaterialTabControl1.ImageList = this.IconList;
            this.MaterialTabControl1.Location = new System.Drawing.Point(0, 64);
            this.MaterialTabControl1.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.MaterialTabControl1.Multiline = true;
            this.MaterialTabControl1.Name = "MaterialTabControl1";
            this.MaterialTabControl1.SelectedIndex = 0;
            this.MaterialTabControl1.Size = new System.Drawing.Size(950, 483);
            this.MaterialTabControl1.TabIndex = 0;
            this.MaterialTabControl1.SelectedIndexChanged += new System.EventHandler(this.MaterialTabControl1_SelectedIndexChanged);
            // 
            // tabLiveMonitor
            // 
            this.tabLiveMonitor.Controls.Add(this.StartServerBTN_New);
            this.tabLiveMonitor.Controls.Add(this.SvConfigButton);
            this.tabLiveMonitor.Controls.Add(this.StopServerBTN_New);
            this.tabLiveMonitor.Controls.Add(this.CommandInputTextBox);
            this.tabLiveMonitor.Controls.Add(this.ConsoleOutputTextBox);
            this.tabLiveMonitor.ImageKey = "terminal_24dp.png";
            this.tabLiveMonitor.Location = new System.Drawing.Point(4, 31);
            this.tabLiveMonitor.Name = "tabLiveMonitor";
            this.tabLiveMonitor.Padding = new System.Windows.Forms.Padding(3);
            this.tabLiveMonitor.Size = new System.Drawing.Size(942, 448);
            this.tabLiveMonitor.TabIndex = 0;
            this.tabLiveMonitor.Text = "Server Console";
            this.tabLiveMonitor.UseVisualStyleBackColor = true;
            // 
            // StartServerBTN_New
            // 
            this.StartServerBTN_New.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StartServerBTN_New.Depth = 0;
            this.StartServerBTN_New.Icon = global::BDSUtil.Properties.Resources.power_settings_new_24dp_green;
            this.StartServerBTN_New.ImageList = this.IconList;
            this.StartServerBTN_New.Location = new System.Drawing.Point(834, 316);
            this.StartServerBTN_New.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.StartServerBTN_New.Name = "StartServerBTN_New";
            this.StartServerBTN_New.Size = new System.Drawing.Size(56, 56);
            this.StartServerBTN_New.TabIndex = 23;
            this.StartServerBTN_New.Text = "materialFloatingActionButton3";
            this.StartServerBTN_New.UseVisualStyleBackColor = true;
            this.StartServerBTN_New.Click += new System.EventHandler(this.StartServerBTN_New_Click);
            // 
            // IconList
            // 
            this.IconList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("IconList.ImageStream")));
            this.IconList.TransparentColor = System.Drawing.Color.Transparent;
            this.IconList.Images.SetKeyName(0, "baseline_bluetooth_black_24dp.png");
            this.IconList.Images.SetKeyName(1, "baseline_build_black_24dp.png");
            this.IconList.Images.SetKeyName(2, "baseline_favorite_border_black_24dp.png");
            this.IconList.Images.SetKeyName(3, "baseline_fingerprint_black_24dp.png");
            this.IconList.Images.SetKeyName(4, "baseline_thumb_up_black_24dp.png");
            this.IconList.Images.SetKeyName(5, "ic_settings_black_24dp_1x.png");
            this.IconList.Images.SetKeyName(6, "ic_settings_white_24dp_1x.png");
            this.IconList.Images.SetKeyName(7, "minus.png");
            this.IconList.Images.SetKeyName(8, "plus.png");
            this.IconList.Images.SetKeyName(9, "round_add_black_24dp.png");
            this.IconList.Images.SetKeyName(10, "round_assessment_white_24dp.png");
            this.IconList.Images.SetKeyName(11, "round_backup_white_24dp.png");
            this.IconList.Images.SetKeyName(12, "round_bluetooth_white_24dp.png");
            this.IconList.Images.SetKeyName(13, "round_bookmark_white_24dp.png");
            this.IconList.Images.SetKeyName(14, "round_build_white_24dp.png");
            this.IconList.Images.SetKeyName(15, "round_gps_fixed_white_24dp.png");
            this.IconList.Images.SetKeyName(16, "round_http_white_24dp.png");
            this.IconList.Images.SetKeyName(17, "round_phone_black_24dp.png");
            this.IconList.Images.SetKeyName(18, "round_report_problem_white_24dp.png");
            this.IconList.Images.SetKeyName(19, "round_swap_vert_white_24dp.png");
            this.IconList.Images.SetKeyName(20, "terminal_24dp.png");
            // 
            // SvConfigButton
            // 
            this.SvConfigButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SvConfigButton.Depth = 0;
            this.SvConfigButton.Icon = global::BDSUtil.Properties.Resources.round_build_white_24dp;
            this.SvConfigButton.ImageList = this.IconList;
            this.SvConfigButton.Location = new System.Drawing.Point(834, 380);
            this.SvConfigButton.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.SvConfigButton.Name = "SvConfigButton";
            this.SvConfigButton.Size = new System.Drawing.Size(56, 56);
            this.SvConfigButton.TabIndex = 22;
            this.SvConfigButton.Text = "SvConfigButton";
            this.SvConfigButton.UseVisualStyleBackColor = true;
            this.SvConfigButton.Click += new System.EventHandler(this.SvConfigButton_Click);
            // 
            // StopServerBTN_New
            // 
            this.StopServerBTN_New.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.StopServerBTN_New.Depth = 0;
            this.StopServerBTN_New.Icon = global::BDSUtil.Properties.Resources.power_settings_new_24dp_red;
            this.StopServerBTN_New.ImageList = this.IconList;
            this.StopServerBTN_New.Location = new System.Drawing.Point(834, 316);
            this.StopServerBTN_New.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.StopServerBTN_New.Name = "StopServerBTN_New";
            this.StopServerBTN_New.Size = new System.Drawing.Size(56, 56);
            this.StopServerBTN_New.TabIndex = 21;
            this.StopServerBTN_New.Text = "materialFloatingActionButton1";
            this.StopServerBTN_New.UseVisualStyleBackColor = true;
            this.StopServerBTN_New.Visible = false;
            this.StopServerBTN_New.Click += new System.EventHandler(this.StopServerBTN_New_Click);
            // 
            // CommandInputTextBox
            // 
            this.CommandInputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CommandInputTextBox.AnimateReadOnly = false;
            this.CommandInputTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.CommandInputTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.CommandInputTextBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CommandInputTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.CommandInputTextBox.Depth = 0;
            this.CommandInputTextBox.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CommandInputTextBox.HideSelection = true;
            this.CommandInputTextBox.Hint = "Enter Console Command";
            this.CommandInputTextBox.LeadingIcon = null;
            this.CommandInputTextBox.Location = new System.Drawing.Point(12, 388);
            this.CommandInputTextBox.MaxLength = 32767;
            this.CommandInputTextBox.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.CommandInputTextBox.Name = "CommandInputTextBox";
            this.CommandInputTextBox.PasswordChar = '\0';
            this.CommandInputTextBox.PrefixSuffixText = null;
            this.CommandInputTextBox.ReadOnly = false;
            this.CommandInputTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CommandInputTextBox.SelectedText = "";
            this.CommandInputTextBox.SelectionLength = 0;
            this.CommandInputTextBox.SelectionStart = 0;
            this.CommandInputTextBox.ShortcutsEnabled = true;
            this.CommandInputTextBox.Size = new System.Drawing.Size(760, 48);
            this.CommandInputTextBox.TabIndex = 1;
            this.CommandInputTextBox.TabStop = false;
            this.CommandInputTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.CommandInputTextBox.TrailingIcon = null;
            this.CommandInputTextBox.UseSystemPasswordChar = false;
            this.CommandInputTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CommandInputTextBox_KeyDown);
            this.CommandInputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CommandInputTextBox_KeyPress);
            // 
            // ConsoleOutputTextBox
            // 
            this.ConsoleOutputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConsoleOutputTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ConsoleOutputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ConsoleOutputTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConsoleOutputTextBox.ForeColor = System.Drawing.Color.White;
            this.ConsoleOutputTextBox.Location = new System.Drawing.Point(12, 12);
            this.ConsoleOutputTextBox.Name = "ConsoleOutputTextBox";
            this.ConsoleOutputTextBox.ReadOnly = true;
            this.ConsoleOutputTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.ConsoleOutputTextBox.Size = new System.Drawing.Size(760, 360);
            this.ConsoleOutputTextBox.TabIndex = 0;
            this.ConsoleOutputTextBox.Text = "";
            // 
            // BkRsTab
            // 
            this.BkRsTab.Controls.Add(this.BackupWrapper);
            this.BkRsTab.Controls.Add(this.RestoreWrapper);
            this.BkRsTab.ImageKey = "round_backup_white_24dp.png";
            this.BkRsTab.Location = new System.Drawing.Point(4, 31);
            this.BkRsTab.Name = "BkRsTab";
            this.BkRsTab.Padding = new System.Windows.Forms.Padding(3, 13, 3, 13);
            this.BkRsTab.Size = new System.Drawing.Size(942, 448);
            this.BkRsTab.TabIndex = 1;
            this.BkRsTab.Text = "Backup & Restore";
            this.BkRsTab.UseVisualStyleBackColor = true;
            // 
            // BackupWrapper
            // 
            this.BackupWrapper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BackupWrapper.Controls.Add(this.BackupProgressBar);
            this.BackupWrapper.Controls.Add(this.BKCompleteLBL);
            this.BackupWrapper.Controls.Add(this.BackupServerBTN);
            this.BackupWrapper.Controls.Add(this.BackupPacksBTN);
            this.BackupWrapper.Controls.Add(this.BackupWorldBTN);
            this.BackupWrapper.Depth = 0;
            this.BackupWrapper.Dock = System.Windows.Forms.DockStyle.Top;
            this.BackupWrapper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BackupWrapper.Location = new System.Drawing.Point(3, 13);
            this.BackupWrapper.Margin = new System.Windows.Forms.Padding(14);
            this.BackupWrapper.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.BackupWrapper.Name = "BackupWrapper";
            this.BackupWrapper.Padding = new System.Windows.Forms.Padding(14);
            this.BackupWrapper.Size = new System.Drawing.Size(936, 91);
            this.BackupWrapper.TabIndex = 0;
            // 
            // BackupProgressBar
            // 
            this.BackupProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BackupProgressBar.Depth = 0;
            this.BackupProgressBar.Location = new System.Drawing.Point(14, 83);
            this.BackupProgressBar.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.BackupProgressBar.Name = "BackupProgressBar";
            this.BackupProgressBar.Size = new System.Drawing.Size(918, 5);
            this.BackupProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.BackupProgressBar.TabIndex = 5;
            this.BackupProgressBar.UseAccentColor = false;
            this.BackupProgressBar.Value = 75;
            this.BackupProgressBar.Visible = false;
            // 
            // BKCompleteLBL
            // 
            this.BKCompleteLBL.AutoSize = true;
            this.BKCompleteLBL.Depth = 0;
            this.BKCompleteLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.BKCompleteLBL.Location = new System.Drawing.Point(692, 38);
            this.BKCompleteLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.BKCompleteLBL.Name = "BKCompleteLBL";
            this.BKCompleteLBL.Size = new System.Drawing.Size(129, 19);
            this.BKCompleteLBL.TabIndex = 3;
            this.BKCompleteLBL.Text = "Backup Complete!";
            this.BKCompleteLBL.Visible = false;
            // 
            // BackupServerBTN
            // 
            this.BackupServerBTN.AutoSize = false;
            this.BackupServerBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackupServerBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.BackupServerBTN.Depth = 0;
            this.BackupServerBTN.HighEmphasis = true;
            this.BackupServerBTN.Icon = null;
            this.BackupServerBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.BackupServerBTN.Location = new System.Drawing.Point(404, 14);
            this.BackupServerBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.BackupServerBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.BackupServerBTN.Name = "BackupServerBTN";
            this.BackupServerBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.BackupServerBTN.Size = new System.Drawing.Size(173, 63);
            this.BackupServerBTN.TabIndex = 2;
            this.BackupServerBTN.Text = "Full Server Backup";
            this.BackupServerBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.BackupServerBTN.UseAccentColor = false;
            this.BackupServerBTN.UseVisualStyleBackColor = true;
            this.BackupServerBTN.Click += new System.EventHandler(this.BackupServerBTN_Click);
            // 
            // BackupPacksBTN
            // 
            this.BackupPacksBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.BackupPacksBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackupPacksBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.BackupPacksBTN.Depth = 0;
            this.BackupPacksBTN.HighEmphasis = true;
            this.BackupPacksBTN.Icon = null;
            this.BackupPacksBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.BackupPacksBTN.Location = new System.Drawing.Point(223, 14);
            this.BackupPacksBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.BackupPacksBTN.MaximumSize = new System.Drawing.Size(173, 63);
            this.BackupPacksBTN.MinimumSize = new System.Drawing.Size(173, 63);
            this.BackupPacksBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.BackupPacksBTN.Name = "BackupPacksBTN";
            this.BackupPacksBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.BackupPacksBTN.Padding = new System.Windows.Forms.Padding(5);
            this.BackupPacksBTN.Size = new System.Drawing.Size(173, 63);
            this.BackupPacksBTN.TabIndex = 1;
            this.BackupPacksBTN.Text = "Backup Packs";
            this.BackupPacksBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.BackupPacksBTN.UseAccentColor = false;
            this.BackupPacksBTN.UseVisualStyleBackColor = true;
            this.BackupPacksBTN.Click += new System.EventHandler(this.BackupPacksBTN_Click);
            // 
            // BackupWorldBTN
            // 
            this.BackupWorldBTN.AutoSize = false;
            this.BackupWorldBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackupWorldBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.BackupWorldBTN.Depth = 0;
            this.BackupWorldBTN.Dock = System.Windows.Forms.DockStyle.Left;
            this.BackupWorldBTN.HighEmphasis = true;
            this.BackupWorldBTN.Icon = null;
            this.BackupWorldBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.BackupWorldBTN.Location = new System.Drawing.Point(14, 14);
            this.BackupWorldBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.BackupWorldBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.BackupWorldBTN.Name = "BackupWorldBTN";
            this.BackupWorldBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.BackupWorldBTN.Size = new System.Drawing.Size(201, 63);
            this.BackupWorldBTN.TabIndex = 0;
            this.BackupWorldBTN.Text = "Backup World(s)";
            this.BackupWorldBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.BackupWorldBTN.UseAccentColor = false;
            this.BackupWorldBTN.UseVisualStyleBackColor = true;
            this.BackupWorldBTN.Click += new System.EventHandler(this.BackupWorldBTN_Click);
            // 
            // RestoreWrapper
            // 
            this.RestoreWrapper.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RestoreWrapper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RestoreWrapper.Controls.Add(this.RunRestoreBTN);
            this.RestoreWrapper.Controls.Add(this.BackupPointsListBox);
            this.RestoreWrapper.Controls.Add(this.RestoreTypeComboBox);
            this.RestoreWrapper.Controls.Add(this.RestorTitleLBL);
            this.RestoreWrapper.Depth = 0;
            this.RestoreWrapper.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.RestoreWrapper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.RestoreWrapper.Location = new System.Drawing.Point(3, 84);
            this.RestoreWrapper.Margin = new System.Windows.Forms.Padding(14);
            this.RestoreWrapper.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.RestoreWrapper.Name = "RestoreWrapper";
            this.RestoreWrapper.Padding = new System.Windows.Forms.Padding(14);
            this.RestoreWrapper.Size = new System.Drawing.Size(936, 351);
            this.RestoreWrapper.TabIndex = 1;
            // 
            // RunRestoreBTN
            // 
            this.RunRestoreBTN.AutoSize = false;
            this.RunRestoreBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RunRestoreBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.RunRestoreBTN.Depth = 0;
            this.RunRestoreBTN.HighEmphasis = true;
            this.RunRestoreBTN.Icon = null;
            this.RunRestoreBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.RunRestoreBTN.Location = new System.Drawing.Point(14, 285);
            this.RunRestoreBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.RunRestoreBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.RunRestoreBTN.Name = "RunRestoreBTN";
            this.RunRestoreBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.RunRestoreBTN.Size = new System.Drawing.Size(286, 49);
            this.RunRestoreBTN.TabIndex = 3;
            this.RunRestoreBTN.Text = "Restore Selected Snapshot";
            this.RunRestoreBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.RunRestoreBTN.UseAccentColor = false;
            this.RunRestoreBTN.UseVisualStyleBackColor = true;
            this.RunRestoreBTN.Visible = false;
            this.RunRestoreBTN.Click += new System.EventHandler(this.RunRestoreBTN_Click);
            // 
            // BackupPointsListBox
            // 
            this.BackupPointsListBox.BackColor = System.Drawing.Color.White;
            this.BackupPointsListBox.BorderColor = System.Drawing.Color.LightGray;
            this.BackupPointsListBox.Depth = 0;
            this.BackupPointsListBox.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.BackupPointsListBox.Location = new System.Drawing.Point(306, 42);
            this.BackupPointsListBox.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.BackupPointsListBox.Name = "BackupPointsListBox";
            this.BackupPointsListBox.SelectedIndex = -1;
            this.BackupPointsListBox.SelectedItem = null;
            this.BackupPointsListBox.Size = new System.Drawing.Size(571, 289);
            this.BackupPointsListBox.TabIndex = 2;
            this.BackupPointsListBox.SelectedIndexChanged += new ReaLTaiizor.Controls.MaterialListBox.SelectedIndexChangedEventHandler(this.BackupPointsListBox_SelectedIndexChanged);
            // 
            // RestoreTypeComboBox
            // 
            this.RestoreTypeComboBox.AutoResize = false;
            this.RestoreTypeComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RestoreTypeComboBox.Depth = 0;
            this.RestoreTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.RestoreTypeComboBox.DropDownHeight = 174;
            this.RestoreTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RestoreTypeComboBox.DropDownWidth = 121;
            this.RestoreTypeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.RestoreTypeComboBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.RestoreTypeComboBox.FormattingEnabled = true;
            this.RestoreTypeComboBox.IntegralHeight = false;
            this.RestoreTypeComboBox.ItemHeight = 43;
            this.RestoreTypeComboBox.Items.AddRange(new object[] {
            "Worlds Backups",
            "Packs/Addons Backups",
            "Full Server Backups"});
            this.RestoreTypeComboBox.Location = new System.Drawing.Point(17, 42);
            this.RestoreTypeComboBox.MaxDropDownItems = 4;
            this.RestoreTypeComboBox.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.RestoreTypeComboBox.Name = "RestoreTypeComboBox";
            this.RestoreTypeComboBox.Size = new System.Drawing.Size(283, 49);
            this.RestoreTypeComboBox.StartIndex = 0;
            this.RestoreTypeComboBox.TabIndex = 1;
            this.RestoreTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.RestoreTypeComboBox_SelectedIndexChanged);
            // 
            // RestorTitleLBL
            // 
            this.RestorTitleLBL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RestorTitleLBL.AutoSize = true;
            this.RestorTitleLBL.Depth = 0;
            this.RestorTitleLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.RestorTitleLBL.Location = new System.Drawing.Point(351, 10);
            this.RestorTitleLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.RestorTitleLBL.Name = "RestorTitleLBL";
            this.RestorTitleLBL.Size = new System.Drawing.Size(185, 19);
            this.RestorTitleLBL.TabIndex = 0;
            this.RestorTitleLBL.Text = "Server Snapshot Recovery";
            // 
            // SettingsTabPage
            // 
            this.SettingsTabPage.Controls.Add(this.label1);
            this.SettingsTabPage.ImageKey = "ic_settings_white_24dp_1x.png";
            this.SettingsTabPage.Location = new System.Drawing.Point(4, 31);
            this.SettingsTabPage.Name = "SettingsTabPage";
            this.SettingsTabPage.Size = new System.Drawing.Size(942, 448);
            this.SettingsTabPage.TabIndex = 2;
            this.SettingsTabPage.Text = "Settings";
            this.SettingsTabPage.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(176, 137);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(598, 96);
            this.label1.TabIndex = 1;
            this.label1.Text = "This is a dummy tab.\r\nIn release you should never see this. \r\nIf you see this you" +
    "\'re somewhere you shouldnt be.\r\nMaybe you should come work for me :D";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 550);
            this.Controls.Add(this.MaterialTabControl1);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerTabControl = this.MaterialTabControl1;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Padding = new System.Windows.Forms.Padding(0, 64, 0, 3);
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Minecraft BDS Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.MaterialTabControl1.ResumeLayout(false);
            this.tabLiveMonitor.ResumeLayout(false);
            this.BkRsTab.ResumeLayout(false);
            this.BackupWrapper.ResumeLayout(false);
            this.BackupWrapper.PerformLayout();
            this.RestoreWrapper.ResumeLayout(false);
            this.RestoreWrapper.PerformLayout();
            this.SettingsTabPage.ResumeLayout(false);
            this.SettingsTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.MaterialTabControl MaterialTabControl1;
        private System.Windows.Forms.TabPage tabLiveMonitor;
        private System.Windows.Forms.TabPage BkRsTab;
        private System.Windows.Forms.RichTextBox ConsoleOutputTextBox;
        private ReaLTaiizor.Controls.MaterialTextBoxEdit CommandInputTextBox;
        private System.Windows.Forms.TabPage SettingsTabPage;
        private System.Windows.Forms.Label label1;
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
    }
}
