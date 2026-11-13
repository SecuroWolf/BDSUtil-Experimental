namespace BDSUtil.Modals.Content_Controls.AutoMod
{
    partial class AutoModOptions
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.materialCard1 = new Material3.WinForms.Controls.MaterialCard();
            this.AMConLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.materialCard2 = new Material3.WinForms.Controls.MaterialCard();
            this.AMCCard = new Material3.WinForms.Controls.MaterialCard();
            this.SEnTypeLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.AM_PEnThresh = new BDSUtil.UI.MaterialTextBoxEdit();
            this.AM_PEnType = new BDSUtil.UI.MaterialComboBox();
            this.PEThLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.PEnTypeLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.ChEnLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.materialDivider3 = new Material3.WinForms.Controls.MaterialDivider();
            this.materialCard3 = new Material3.WinForms.Controls.MaterialCard();
            this.CfgEAM = new Material3.WinForms.Controls.MaterialSwitch();
            this.EAMLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.SEThLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.AntiSpamLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.STHMLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.AM_Enforce = new Material3.WinForms.Controls.MaterialSwitch();
            this.AM_SEnThresh = new BDSUtil.UI.MaterialTextBoxEdit();
            this.AM_SEnType = new BDSUtil.UI.MaterialComboBox();
            this.STHSLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.AM_AntiSpam = new Material3.WinForms.Controls.MaterialSwitch();
            this.AM_ASThreshMessages = new BDSUtil.UI.MaterialTextBoxEdit();
            this.AM_ASThreshTime = new BDSUtil.UI.MaterialComboBox();
            this.ASEnforcement = new Material3.WinForms.Controls.MaterialLabel();
            this.AMCEStrikeReset = new Material3.WinForms.Controls.MaterialLabel();
            this.AMASStrikeReset = new Material3.WinForms.Controls.MaterialLabel();
            this.AM_ASEnType = new BDSUtil.UI.MaterialComboBox();
            this.AM_CESR = new BDSUtil.UI.MaterialComboBox();
            this.AM_ASSR = new BDSUtil.UI.MaterialComboBox();
            this.materialCard1.SuspendLayout();
            this.materialCard2.SuspendLayout();
            this.AMCCard.SuspendLayout();
            this.materialCard3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AM_PEnType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AM_SEnType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AM_ASThreshTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AM_ASEnType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AM_CESR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AM_ASSR)).BeginInit();
            this.SuspendLayout();
            // 
            // materialCard1
            // 
            this.materialCard1.Controls.Add(this.AMConLBL);
            this.materialCard1.Location = new System.Drawing.Point(24, 18);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(690, 51);
            this.materialCard1.TabIndex = 9;
            this.materialCard1.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            // 
            // AMConLBL
            // 
            this.AMConLBL.BackColor = System.Drawing.Color.Transparent;
            this.AMConLBL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AMConLBL.Font = Material3.WinForms.Typography.TypeScale.TitleMedium;
            this.AMConLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.AMConLBL.Location = new System.Drawing.Point(14, 14);
            this.AMConLBL.Name = "AMConLBL";
            this.AMConLBL.Size = new System.Drawing.Size(662, 23);
            this.AMConLBL.TabIndex = 0;
            this.AMConLBL.Text = "AutoMod Controls";
            this.AMConLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialCard2
            // 
            this.materialCard2.AutoScroll = true;
            this.materialCard2.AutoSize = true;
            this.materialCard2.Controls.Add(this.AMCCard);
            this.materialCard2.Controls.Add(this.materialDivider3);
            this.materialCard2.Controls.Add(this.materialCard3);
            this.materialCard2.Location = new System.Drawing.Point(24, 79);
            this.materialCard2.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard2.Name = "materialCard2";
            this.materialCard2.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard2.Size = new System.Drawing.Size(690, 504);
            this.materialCard2.TabIndex = 10;
            this.materialCard2.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            // 
            // AMCCard
            // 
            this.AMCCard.AutoScroll = true;
            this.AMCCard.Controls.Add(this.AM_ASSR);
            this.AMCCard.Controls.Add(this.AM_CESR);
            this.AMCCard.Controls.Add(this.AM_ASEnType);
            this.AMCCard.Controls.Add(this.AMASStrikeReset);
            this.AMCCard.Controls.Add(this.AMCEStrikeReset);
            this.AMCCard.Controls.Add(this.ASEnforcement);
            this.AMCCard.Controls.Add(this.AM_ASThreshTime);
            this.AMCCard.Controls.Add(this.AM_ASThreshMessages);
            this.AMCCard.Controls.Add(this.AM_AntiSpam);
            this.AMCCard.Controls.Add(this.STHSLBL);
            this.AMCCard.Controls.Add(this.AM_SEnThresh);
            this.AMCCard.Controls.Add(this.AM_SEnType);
            this.AMCCard.Controls.Add(this.AM_Enforce);
            this.AMCCard.Controls.Add(this.STHMLBL);
            this.AMCCard.Controls.Add(this.AntiSpamLBL);
            this.AMCCard.Controls.Add(this.SEThLBL);
            this.AMCCard.Controls.Add(this.SEnTypeLBL);
            this.AMCCard.Controls.Add(this.AM_PEnThresh);
            this.AMCCard.Controls.Add(this.AM_PEnType);
            this.AMCCard.Controls.Add(this.PEThLBL);
            this.AMCCard.Controls.Add(this.PEnTypeLBL);
            this.AMCCard.Controls.Add(this.ChEnLBL);
            this.AMCCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.AMCCard.Location = new System.Drawing.Point(14, 99);
            this.AMCCard.Margin = new System.Windows.Forms.Padding(14);
            this.AMCCard.Name = "AMCCard";
            this.AMCCard.Padding = new System.Windows.Forms.Padding(5);
            this.AMCCard.Size = new System.Drawing.Size(662, 385);
            this.AMCCard.TabIndex = 4;
            this.AMCCard.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            this.AMCCard.Visible = false;
            // 
            // SEnTypeLBL
            // 
            this.SEnTypeLBL.BackColor = System.Drawing.Color.Transparent;
            this.SEnTypeLBL.Dock = System.Windows.Forms.DockStyle.Top;
            this.SEnTypeLBL.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.SEnTypeLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.SEnTypeLBL.Location = new System.Drawing.Point(5, 149);
            this.SEnTypeLBL.Name = "SEnTypeLBL";
            this.SEnTypeLBL.Size = new System.Drawing.Size(635, 48);
            this.SEnTypeLBL.TabIndex = 9;
            this.SEnTypeLBL.Text = "Secondary Enforcement Type";
            this.SEnTypeLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AM_PEnThresh
            // 
            this.AM_PEnThresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AM_PEnThresh.Hint = "Numbers only(1-100)";
            this.AM_PEnThresh.Location = new System.Drawing.Point(458, 102);
            this.AM_PEnThresh.MaxLength = 3;
            this.AM_PEnThresh.Name = "AM_PEnThresh";
            this.AM_PEnThresh.ReadOnly = false;
            this.AM_PEnThresh.Size = new System.Drawing.Size(188, 46);
            this.AM_PEnThresh.TabIndex = 8;
            this.AM_PEnThresh.TabStop = false;
            this.AM_PEnThresh.Text = "3";
            // 
            // AM_PEnType
            // 
            this.AM_PEnType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AM_PEnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AM_PEnType.Items.AddRange(new object[] {
            "Kick",
            "Temp-Ban(24h)",
            "Ban"});
            this.AM_PEnType.Location = new System.Drawing.Point(458, 54);
            this.AM_PEnType.Name = "AM_PEnType";
            this.AM_PEnType.SelectedIndex = 0;
            this.AM_PEnType.Size = new System.Drawing.Size(188, 44);
            this.AM_PEnType.TabIndex = 7;
            // 
            // PEThLBL
            // 
            this.PEThLBL.BackColor = System.Drawing.Color.Transparent;
            this.PEThLBL.Dock = System.Windows.Forms.DockStyle.Top;
            this.PEThLBL.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.PEThLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.PEThLBL.Location = new System.Drawing.Point(5, 101);
            this.PEThLBL.Name = "PEThLBL";
            this.PEThLBL.Size = new System.Drawing.Size(635, 48);
            this.PEThLBL.TabIndex = 4;
            this.PEThLBL.Text = "Primary Enforcement Strike Threshold";
            this.PEThLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PEnTypeLBL
            // 
            this.PEnTypeLBL.BackColor = System.Drawing.Color.Transparent;
            this.PEnTypeLBL.Dock = System.Windows.Forms.DockStyle.Top;
            this.PEnTypeLBL.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.PEnTypeLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.PEnTypeLBL.Location = new System.Drawing.Point(5, 53);
            this.PEnTypeLBL.Name = "PEnTypeLBL";
            this.PEnTypeLBL.Size = new System.Drawing.Size(635, 48);
            this.PEnTypeLBL.TabIndex = 2;
            this.PEnTypeLBL.Text = "Primary Enforcement Type\r\n(Each strike until this results in a Warning.)";
            this.PEnTypeLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ChEnLBL
            // 
            this.ChEnLBL.BackColor = System.Drawing.Color.Transparent;
            this.ChEnLBL.Dock = System.Windows.Forms.DockStyle.Top;
            this.ChEnLBL.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.ChEnLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.ChEnLBL.Location = new System.Drawing.Point(5, 5);
            this.ChEnLBL.Name = "ChEnLBL";
            this.ChEnLBL.Size = new System.Drawing.Size(635, 48);
            this.ChEnLBL.TabIndex = 0;
            this.ChEnLBL.Text = "Enable Automated Chat Enforcement";
            this.ChEnLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialDivider3
            // 
            this.materialDivider3.BackColor = System.Drawing.Color.Transparent;
            this.materialDivider3.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialDivider3.Location = new System.Drawing.Point(14, 89);
            this.materialDivider3.Margin = new System.Windows.Forms.Padding(10);
            this.materialDivider3.Name = "materialDivider3";
            this.materialDivider3.Size = new System.Drawing.Size(662, 10);
            this.materialDivider3.TabIndex = 2;
            this.materialDivider3.TabStop = false;
            // 
            // materialCard3
            // 
            this.materialCard3.Controls.Add(this.CfgEAM);
            this.materialCard3.Controls.Add(this.EAMLBL);
            this.materialCard3.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialCard3.Location = new System.Drawing.Point(14, 14);
            this.materialCard3.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard3.Name = "materialCard3";
            this.materialCard3.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard3.Size = new System.Drawing.Size(662, 75);
            this.materialCard3.TabIndex = 0;
            this.materialCard3.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            // 
            // CfgEAM
            // 
            this.CfgEAM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CfgEAM.AutoSize = false;
            this.CfgEAM.Location = new System.Drawing.Point(584, 18);
            this.CfgEAM.Margin = new System.Windows.Forms.Padding(0);
            this.CfgEAM.Name = "CfgEAM";
            this.CfgEAM.Size = new System.Drawing.Size(64, 38);
            this.CfgEAM.TabIndex = 14;
            this.CfgEAM.CheckedChanged += new System.EventHandler(this.CfgEAM_CheckedChanged);
            // 
            // EAMLBL
            // 
            this.EAMLBL.BackColor = System.Drawing.Color.Transparent;
            this.EAMLBL.Dock = System.Windows.Forms.DockStyle.Top;
            this.EAMLBL.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.EAMLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.EAMLBL.Location = new System.Drawing.Point(14, 14);
            this.EAMLBL.Name = "EAMLBL";
            this.EAMLBL.Size = new System.Drawing.Size(634, 48);
            this.EAMLBL.TabIndex = 13;
            this.EAMLBL.Text = "Enable Automod";
            this.EAMLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SEThLBL
            // 
            this.SEThLBL.BackColor = System.Drawing.Color.Transparent;
            this.SEThLBL.Dock = System.Windows.Forms.DockStyle.Top;
            this.SEThLBL.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.SEThLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.SEThLBL.Location = new System.Drawing.Point(5, 197);
            this.SEThLBL.Name = "SEThLBL";
            this.SEThLBL.Size = new System.Drawing.Size(635, 48);
            this.SEThLBL.TabIndex = 13;
            this.SEThLBL.Text = "Secondary Enforcement Strike Threshold";
            this.SEThLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AntiSpamLBL
            // 
            this.AntiSpamLBL.BackColor = System.Drawing.Color.Transparent;
            this.AntiSpamLBL.Dock = System.Windows.Forms.DockStyle.Top;
            this.AntiSpamLBL.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.AntiSpamLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.AntiSpamLBL.Location = new System.Drawing.Point(5, 245);
            this.AntiSpamLBL.Name = "AntiSpamLBL";
            this.AntiSpamLBL.Size = new System.Drawing.Size(635, 48);
            this.AntiSpamLBL.TabIndex = 14;
            this.AntiSpamLBL.Text = "Enable Anti-Spam\r\n(You must set both threshold levels.)";
            this.AntiSpamLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // STHMLBL
            // 
            this.STHMLBL.BackColor = System.Drawing.Color.Transparent;
            this.STHMLBL.Dock = System.Windows.Forms.DockStyle.Top;
            this.STHMLBL.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.STHMLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.STHMLBL.Location = new System.Drawing.Point(5, 293);
            this.STHMLBL.Name = "STHMLBL";
            this.STHMLBL.Size = new System.Drawing.Size(635, 48);
            this.STHMLBL.TabIndex = 15;
            this.STHMLBL.Text = "Spam Threshold(Messages)";
            this.STHMLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AM_Enforce
            // 
            this.AM_Enforce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AM_Enforce.AutoSize = false;
            this.AM_Enforce.Location = new System.Drawing.Point(580, 10);
            this.AM_Enforce.Margin = new System.Windows.Forms.Padding(0);
            this.AM_Enforce.Name = "AM_Enforce";
            this.AM_Enforce.Size = new System.Drawing.Size(64, 38);
            this.AM_Enforce.TabIndex = 19;
            // 
            // AM_SEnThresh
            // 
            this.AM_SEnThresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AM_SEnThresh.Hint = "Numbers only(1-100)";
            this.AM_SEnThresh.Location = new System.Drawing.Point(458, 198);
            this.AM_SEnThresh.MaxLength = 3;
            this.AM_SEnThresh.Name = "AM_SEnThresh";
            this.AM_SEnThresh.ReadOnly = false;
            this.AM_SEnThresh.Size = new System.Drawing.Size(188, 46);
            this.AM_SEnThresh.TabIndex = 21;
            this.AM_SEnThresh.TabStop = false;
            this.AM_SEnThresh.Text = "5";
            // 
            // AM_SEnType
            // 
            this.AM_SEnType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AM_SEnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AM_SEnType.Items.AddRange(new object[] {
            "Kick",
            "Temp-Ban(24h)",
            "Ban"});
            this.AM_SEnType.Location = new System.Drawing.Point(458, 150);
            this.AM_SEnType.Name = "AM_SEnType";
            this.AM_SEnType.SelectedIndex = 1;
            this.AM_SEnType.Size = new System.Drawing.Size(188, 44);
            this.AM_SEnType.TabIndex = 20;
            // 
            // STHSLBL
            // 
            this.STHSLBL.BackColor = System.Drawing.Color.Transparent;
            this.STHSLBL.Dock = System.Windows.Forms.DockStyle.Top;
            this.STHSLBL.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.STHSLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.STHSLBL.Location = new System.Drawing.Point(5, 341);
            this.STHSLBL.Name = "STHSLBL";
            this.STHSLBL.Size = new System.Drawing.Size(635, 48);
            this.STHSLBL.TabIndex = 22;
            this.STHSLBL.Text = "Spam Threshold(Seconds)";
            this.STHSLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AM_AntiSpam
            // 
            this.AM_AntiSpam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AM_AntiSpam.AutoSize = false;
            this.AM_AntiSpam.Location = new System.Drawing.Point(580, 250);
            this.AM_AntiSpam.Margin = new System.Windows.Forms.Padding(0);
            this.AM_AntiSpam.Name = "AM_AntiSpam";
            this.AM_AntiSpam.Size = new System.Drawing.Size(64, 38);
            this.AM_AntiSpam.TabIndex = 23;
            // 
            // AM_ASThreshMessages
            // 
            this.AM_ASThreshMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AM_ASThreshMessages.Hint = "Numbers only(1-100)";
            this.AM_ASThreshMessages.Location = new System.Drawing.Point(458, 294);
            this.AM_ASThreshMessages.MaxLength = 3;
            this.AM_ASThreshMessages.Name = "AM_ASThreshMessages";
            this.AM_ASThreshMessages.ReadOnly = false;
            this.AM_ASThreshMessages.Size = new System.Drawing.Size(188, 46);
            this.AM_ASThreshMessages.TabIndex = 24;
            this.AM_ASThreshMessages.TabStop = false;
            this.AM_ASThreshMessages.Text = "5";
            // 
            // AM_ASThreshTime
            // 
            this.AM_ASThreshTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AM_ASThreshTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AM_ASThreshTime.Items.AddRange(new object[] {
            "3",
            "5",
            "7",
            "9",
            "Custom"});
            this.AM_ASThreshTime.Location = new System.Drawing.Point(458, 342);
            this.AM_ASThreshTime.Name = "AM_ASThreshTime";
            this.AM_ASThreshTime.SelectedIndex = 0;
            this.AM_ASThreshTime.Size = new System.Drawing.Size(188, 44);
            this.AM_ASThreshTime.TabIndex = 25;
            // 
            // ASEnforcement
            // 
            this.ASEnforcement.BackColor = System.Drawing.Color.Transparent;
            this.ASEnforcement.Dock = System.Windows.Forms.DockStyle.Top;
            this.ASEnforcement.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.ASEnforcement.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.ASEnforcement.Location = new System.Drawing.Point(5, 389);
            this.ASEnforcement.Name = "ASEnforcement";
            this.ASEnforcement.Size = new System.Drawing.Size(635, 48);
            this.ASEnforcement.TabIndex = 26;
            this.ASEnforcement.Text = "Anti-Spam Enforcement Action\r\n(First strike is a warn. Second initiates this acti" +
    "on)";
            this.ASEnforcement.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AMCEStrikeReset
            // 
            this.AMCEStrikeReset.BackColor = System.Drawing.Color.Transparent;
            this.AMCEStrikeReset.Dock = System.Windows.Forms.DockStyle.Top;
            this.AMCEStrikeReset.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.AMCEStrikeReset.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.AMCEStrikeReset.Location = new System.Drawing.Point(5, 437);
            this.AMCEStrikeReset.Name = "AMCEStrikeReset";
            this.AMCEStrikeReset.Size = new System.Drawing.Size(635, 48);
            this.AMCEStrikeReset.TabIndex = 27;
            this.AMCEStrikeReset.Text = "Chat Enforcement Strike Reset";
            this.AMCEStrikeReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AMASStrikeReset
            // 
            this.AMASStrikeReset.BackColor = System.Drawing.Color.Transparent;
            this.AMASStrikeReset.Dock = System.Windows.Forms.DockStyle.Top;
            this.AMASStrikeReset.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.AMASStrikeReset.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.AMASStrikeReset.Location = new System.Drawing.Point(5, 485);
            this.AMASStrikeReset.Name = "AMASStrikeReset";
            this.AMASStrikeReset.Size = new System.Drawing.Size(635, 48);
            this.AMASStrikeReset.TabIndex = 28;
            this.AMASStrikeReset.Text = "Anti-Spam Strike Reset";
            this.AMASStrikeReset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AM_ASEnType
            // 
            this.AM_ASEnType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AM_ASEnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AM_ASEnType.Items.AddRange(new object[] {
            "Mute(5m)",
            "Mute(10m)",
            "Kick",
            "Temp-Ban(24h)",
            "Ban"});
            this.AM_ASEnType.Location = new System.Drawing.Point(458, 390);
            this.AM_ASEnType.Name = "AM_ASEnType";
            this.AM_ASEnType.SelectedIndex = 0;
            this.AM_ASEnType.Size = new System.Drawing.Size(188, 44);
            this.AM_ASEnType.TabIndex = 31;
            // 
            // AM_CESR
            // 
            this.AM_CESR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AM_CESR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AM_CESR.Items.AddRange(new object[] {
            "1 Hour",
            "6 Hours",
            "12 Hours",
            "24 Hours",
            "Custom"});
            this.AM_CESR.Location = new System.Drawing.Point(458, 438);
            this.AM_CESR.Name = "AM_CESR";
            this.AM_CESR.SelectedIndex = 2;
            this.AM_CESR.Size = new System.Drawing.Size(188, 44);
            this.AM_CESR.TabIndex = 32;
            // 
            // AM_ASSR
            // 
            this.AM_ASSR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AM_ASSR.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AM_ASSR.Items.AddRange(new object[] {
            "1 Hour",
            "6 Hours",
            "12 Hours",
            "24 Hours",
            "Custom"});
            this.AM_ASSR.Location = new System.Drawing.Point(458, 486);
            this.AM_ASSR.Name = "AM_ASSR";
            this.AM_ASSR.SelectedIndex = 0;
            this.AM_ASSR.Size = new System.Drawing.Size(188, 44);
            this.AM_ASSR.TabIndex = 33;
            // 
            // AutoModOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = Material3.WinForms.Theming.MaterialColors.Surface;
            this.Controls.Add(this.materialCard1);
            this.Controls.Add(this.materialCard2);
            this.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.MaximumSize = new System.Drawing.Size(738, 10000);
            this.Name = "AutoModOptions";
            this.Size = new System.Drawing.Size(738, 600);
            this.materialCard1.ResumeLayout(false);
            this.materialCard2.ResumeLayout(false);
            this.AMCCard.ResumeLayout(false);
            this.AMCCard.PerformLayout();
            this.materialCard3.ResumeLayout(false);
            this.materialCard3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AM_PEnType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AM_SEnType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AM_ASThreshTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AM_ASEnType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AM_CESR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AM_ASSR)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Material3.WinForms.Controls.MaterialCard materialCard1;
        private Material3.WinForms.Controls.MaterialLabel AMConLBL;
        private Material3.WinForms.Controls.MaterialCard materialCard2;
        private Material3.WinForms.Controls.MaterialCard materialCard3;
        private Material3.WinForms.Controls.MaterialSwitch CfgEAM;
        private Material3.WinForms.Controls.MaterialLabel EAMLBL;
        private Material3.WinForms.Controls.MaterialDivider materialDivider3;
        private Material3.WinForms.Controls.MaterialCard AMCCard;
        private Material3.WinForms.Controls.MaterialLabel SEnTypeLBL;
        private BDSUtil.UI.MaterialTextBoxEdit AM_PEnThresh;
        private BDSUtil.UI.MaterialComboBox AM_PEnType;
        private Material3.WinForms.Controls.MaterialLabel PEThLBL;
        private Material3.WinForms.Controls.MaterialLabel PEnTypeLBL;
        private Material3.WinForms.Controls.MaterialLabel ChEnLBL;
        private Material3.WinForms.Controls.MaterialLabel SEThLBL;
        private Material3.WinForms.Controls.MaterialLabel AntiSpamLBL;
        private Material3.WinForms.Controls.MaterialLabel STHMLBL;
        private Material3.WinForms.Controls.MaterialSwitch AM_Enforce;
        private BDSUtil.UI.MaterialTextBoxEdit AM_SEnThresh;
        private BDSUtil.UI.MaterialComboBox AM_SEnType;
        private BDSUtil.UI.MaterialComboBox AM_ASThreshTime;
        private BDSUtil.UI.MaterialTextBoxEdit AM_ASThreshMessages;
        private Material3.WinForms.Controls.MaterialSwitch AM_AntiSpam;
        private Material3.WinForms.Controls.MaterialLabel STHSLBL;
        private Material3.WinForms.Controls.MaterialLabel AMASStrikeReset;
        private Material3.WinForms.Controls.MaterialLabel AMCEStrikeReset;
        private Material3.WinForms.Controls.MaterialLabel ASEnforcement;
        private BDSUtil.UI.MaterialComboBox AM_ASEnType;
        private BDSUtil.UI.MaterialComboBox AM_ASSR;
        private BDSUtil.UI.MaterialComboBox AM_CESR;
    }
}
