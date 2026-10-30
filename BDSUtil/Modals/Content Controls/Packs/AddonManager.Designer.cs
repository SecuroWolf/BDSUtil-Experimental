namespace BDSUtil.Modals.Content_Controls.Packs
{
    partial class AddonManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddonManager));
            PM_TabSelector = new ReaLTaiizor.Controls.MaterialTabSelector();
            PM_TabControl = new ReaLTaiizor.Controls.MaterialTabControl();
            tabResourcePacks = new System.Windows.Forms.TabPage();
            RP_DeleteBTN = new ReaLTaiizor.Controls.MaterialButton();
            PM_CloseBTN = new ReaLTaiizor.Controls.MaterialButton();
            RP_ListView = new ReaLTaiizor.Controls.MaterialListView();
            colRPName = new System.Windows.Forms.ColumnHeader();
            colRPVersion = new System.Windows.Forms.ColumnHeader();
            RP_ImportBTN = new ReaLTaiizor.Controls.MaterialButton();
            RP_EnableBTN = new ReaLTaiizor.Controls.MaterialButton();
            RP_DisableBTN = new ReaLTaiizor.Controls.MaterialButton();
            tabBehaviorPacks = new System.Windows.Forms.TabPage();
            BP_DeleteBTN = new ReaLTaiizor.Controls.MaterialButton();
            BP_ListView = new ReaLTaiizor.Controls.MaterialListView();
            colBPName = new System.Windows.Forms.ColumnHeader();
            colBPVersion = new System.Windows.Forms.ColumnHeader();
            BP_ImportBTN = new ReaLTaiizor.Controls.MaterialButton();
            BP_EnableBTN = new ReaLTaiizor.Controls.MaterialButton();
            BP_DisableBTN = new ReaLTaiizor.Controls.MaterialButton();
            tabCustomFeatures = new System.Windows.Forms.TabPage();
            CF_PlaceholderCard2 = new ReaLTaiizor.Controls.MaterialCard();
            CF_Placeholder2Switch = new ReaLTaiizor.Controls.MaterialSwitch();
            CF_Placeholder2HeaderLBL = new ReaLTaiizor.Controls.MaterialLabel();
            CF_PlaceholderCard1 = new ReaLTaiizor.Controls.MaterialCard();
            CF_Placeholder1Switch = new ReaLTaiizor.Controls.MaterialSwitch();
            CF_Placeholder1HeaderLBL = new ReaLTaiizor.Controls.MaterialLabel();
            CF_CommandsCard = new ReaLTaiizor.Controls.MaterialCard();
            CF_AddCommandBTN = new ReaLTaiizor.Controls.MaterialButton();
            CF_CommandsDescLBL = new ReaLTaiizor.Controls.MaterialLabel();
            CF_CommandsHeaderLBL = new ReaLTaiizor.Controls.MaterialLabel();
            CF_FeaturesCard = new ReaLTaiizor.Controls.MaterialCard();
            CF_AntiGriefSwitch = new ReaLTaiizor.Controls.MaterialSwitch();
            CF_PvpAwardsSwitch = new ReaLTaiizor.Controls.MaterialSwitch();
            CF_OpCommandsSwitch = new ReaLTaiizor.Controls.MaterialSwitch();
            CF_HeaderLBL = new ReaLTaiizor.Controls.MaterialLabel();
            PM_TabControl.SuspendLayout();
            tabResourcePacks.SuspendLayout();
            tabBehaviorPacks.SuspendLayout();
            tabCustomFeatures.SuspendLayout();
            CF_PlaceholderCard2.SuspendLayout();
            CF_PlaceholderCard1.SuspendLayout();
            CF_CommandsCard.SuspendLayout();
            CF_FeaturesCard.SuspendLayout();
            SuspendLayout();
            // 
            // PM_TabSelector
            // 
            PM_TabSelector.BaseTabControl = PM_TabControl;
            PM_TabSelector.CharacterCasing = ReaLTaiizor.Controls.MaterialTabSelector.CustomCharacterCasing.Normal;
            PM_TabSelector.Depth = 0;
            PM_TabSelector.Dock = System.Windows.Forms.DockStyle.Top;
            PM_TabSelector.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            PM_TabSelector.HeadAlignment = ReaLTaiizor.Controls.MaterialTabSelector.Alignment.Left;
            PM_TabSelector.Location = new System.Drawing.Point(3, 0);
            PM_TabSelector.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PM_TabSelector.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            PM_TabSelector.Name = "PM_TabSelector";
            PM_TabSelector.Size = new System.Drawing.Size(758, 55);
            PM_TabSelector.TabIndex = 0;
            // 
            // PM_TabControl
            // 
            PM_TabControl.Controls.Add(tabResourcePacks);
            PM_TabControl.Controls.Add(tabBehaviorPacks);
            PM_TabControl.Controls.Add(tabCustomFeatures);
            PM_TabControl.Depth = 0;
            PM_TabControl.Dock = System.Windows.Forms.DockStyle.Top;
            PM_TabControl.Location = new System.Drawing.Point(3, 55);
            PM_TabControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PM_TabControl.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            PM_TabControl.Multiline = true;
            PM_TabControl.Name = "PM_TabControl";
            PM_TabControl.SelectedIndex = 0;
            PM_TabControl.Size = new System.Drawing.Size(758, 489);
            PM_TabControl.TabIndex = 1;
            // 
            // tabResourcePacks
            // 
            tabResourcePacks.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            tabResourcePacks.Controls.Add(RP_DeleteBTN);
            tabResourcePacks.Controls.Add(PM_CloseBTN);
            tabResourcePacks.Controls.Add(RP_ListView);
            tabResourcePacks.Controls.Add(RP_ImportBTN);
            tabResourcePacks.Controls.Add(RP_EnableBTN);
            tabResourcePacks.Controls.Add(RP_DisableBTN);
            tabResourcePacks.Location = new System.Drawing.Point(4, 24);
            tabResourcePacks.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabResourcePacks.Name = "tabResourcePacks";
            tabResourcePacks.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabResourcePacks.Size = new System.Drawing.Size(750, 461);
            tabResourcePacks.TabIndex = 0;
            tabResourcePacks.Text = "Resource Packs";
            // 
            // RP_DeleteBTN
            // 
            RP_DeleteBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            RP_DeleteBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            RP_DeleteBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            RP_DeleteBTN.Depth = 0;
            RP_DeleteBTN.HighEmphasis = true;
            RP_DeleteBTN.Icon = null;
            RP_DeleteBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            RP_DeleteBTN.Location = new System.Drawing.Point(167, 361);
            RP_DeleteBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            RP_DeleteBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            RP_DeleteBTN.Name = "RP_DeleteBTN";
            RP_DeleteBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            RP_DeleteBTN.Size = new System.Drawing.Size(115, 36);
            RP_DeleteBTN.TabIndex = 5;
            RP_DeleteBTN.Text = "DELETE PACK";
            RP_DeleteBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            RP_DeleteBTN.UseAccentColor = true;
            RP_DeleteBTN.Click += DeleteBTN_Click;
            // 
            // PM_CloseBTN
            // 
            PM_CloseBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            PM_CloseBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            PM_CloseBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            PM_CloseBTN.Depth = 0;
            PM_CloseBTN.HighEmphasis = true;
            PM_CloseBTN.Icon = null;
            PM_CloseBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            PM_CloseBTN.Location = new System.Drawing.Point(664, 411);
            PM_CloseBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            PM_CloseBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            PM_CloseBTN.Name = "PM_CloseBTN";
            PM_CloseBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            PM_CloseBTN.Size = new System.Drawing.Size(66, 36);
            PM_CloseBTN.TabIndex = 2;
            PM_CloseBTN.Text = "CLOSE";
            PM_CloseBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            PM_CloseBTN.UseAccentColor = true;
            PM_CloseBTN.Click += PM_CloseBTN_Click;
            // 
            // RP_ListView
            // 
            RP_ListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            RP_ListView.AutoSizeTable = false;
            RP_ListView.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            RP_ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            RP_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { colRPName, colRPVersion });
            RP_ListView.Depth = 0;
            RP_ListView.FullRowSelect = true;
            RP_ListView.Location = new System.Drawing.Point(20, 20);
            RP_ListView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RP_ListView.MinimumSize = new System.Drawing.Size(233, 115);
            RP_ListView.MouseLocation = new System.Drawing.Point(-1, -1);
            RP_ListView.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            RP_ListView.Name = "RP_ListView";
            RP_ListView.OwnerDraw = true;
            RP_ListView.Size = new System.Drawing.Size(710, 331);
            RP_ListView.TabIndex = 0;
            RP_ListView.UseCompatibleStateImageBehavior = false;
            RP_ListView.View = System.Windows.Forms.View.Details;
            // 
            // colRPName
            // 
            colRPName.Text = "Pack Name";
            colRPName.Width = 520;
            // 
            // colRPVersion
            // 
            colRPVersion.Text = "Version";
            colRPVersion.Width = 200;
            // 
            // RP_ImportBTN
            // 
            RP_ImportBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            RP_ImportBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            RP_ImportBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            RP_ImportBTN.Depth = 0;
            RP_ImportBTN.HighEmphasis = true;
            RP_ImportBTN.Icon = null;
            RP_ImportBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            RP_ImportBTN.Location = new System.Drawing.Point(20, 361);
            RP_ImportBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            RP_ImportBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            RP_ImportBTN.Name = "RP_ImportBTN";
            RP_ImportBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            RP_ImportBTN.Size = new System.Drawing.Size(118, 36);
            RP_ImportBTN.TabIndex = 1;
            RP_ImportBTN.Text = "IMPORT PACK";
            RP_ImportBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            RP_ImportBTN.UseAccentColor = true;
            RP_ImportBTN.Click += ImportBTN_Click;
            // 
            // RP_EnableBTN
            // 
            RP_EnableBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            RP_EnableBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            RP_EnableBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            RP_EnableBTN.Depth = 0;
            RP_EnableBTN.HighEmphasis = true;
            RP_EnableBTN.Icon = null;
            RP_EnableBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            RP_EnableBTN.Location = new System.Drawing.Point(545, 361);
            RP_EnableBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            RP_EnableBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            RP_EnableBTN.Name = "RP_EnableBTN";
            RP_EnableBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            RP_EnableBTN.Size = new System.Drawing.Size(76, 36);
            RP_EnableBTN.TabIndex = 2;
            RP_EnableBTN.Text = "ENABLE";
            RP_EnableBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            RP_EnableBTN.UseAccentColor = false;
            RP_EnableBTN.Click += RP_EnableBTN_Click;
            // 
            // RP_DisableBTN
            // 
            RP_DisableBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            RP_DisableBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            RP_DisableBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            RP_DisableBTN.Depth = 0;
            RP_DisableBTN.HighEmphasis = true;
            RP_DisableBTN.Icon = null;
            RP_DisableBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            RP_DisableBTN.Location = new System.Drawing.Point(651, 361);
            RP_DisableBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            RP_DisableBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            RP_DisableBTN.Name = "RP_DisableBTN";
            RP_DisableBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            RP_DisableBTN.Size = new System.Drawing.Size(79, 36);
            RP_DisableBTN.TabIndex = 3;
            RP_DisableBTN.Text = "DISABLE";
            RP_DisableBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            RP_DisableBTN.UseAccentColor = false;
            RP_DisableBTN.Click += RP_DisableBTN_Click;
            // 
            // tabBehaviorPacks
            // 
            tabBehaviorPacks.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            tabBehaviorPacks.Controls.Add(BP_DeleteBTN);
            tabBehaviorPacks.Controls.Add(BP_ListView);
            tabBehaviorPacks.Controls.Add(BP_ImportBTN);
            tabBehaviorPacks.Controls.Add(BP_EnableBTN);
            tabBehaviorPacks.Controls.Add(BP_DisableBTN);
            tabBehaviorPacks.Location = new System.Drawing.Point(4, 24);
            tabBehaviorPacks.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabBehaviorPacks.Name = "tabBehaviorPacks";
            tabBehaviorPacks.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabBehaviorPacks.Size = new System.Drawing.Size(750, 491);
            tabBehaviorPacks.TabIndex = 1;
            tabBehaviorPacks.Text = "Behavior Packs";
            // 
            // BP_DeleteBTN
            // 
            BP_DeleteBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BP_DeleteBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            BP_DeleteBTN.Depth = 0;
            BP_DeleteBTN.HighEmphasis = true;
            BP_DeleteBTN.Icon = null;
            BP_DeleteBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            BP_DeleteBTN.Location = new System.Drawing.Point(167, 363);
            BP_DeleteBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            BP_DeleteBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            BP_DeleteBTN.Name = "BP_DeleteBTN";
            BP_DeleteBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            BP_DeleteBTN.Size = new System.Drawing.Size(115, 36);
            BP_DeleteBTN.TabIndex = 4;
            BP_DeleteBTN.Text = "DELETE PACK";
            BP_DeleteBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            BP_DeleteBTN.UseAccentColor = true;
            BP_DeleteBTN.Click += DeleteBTN_Click;
            // 
            // BP_ListView
            // 
            BP_ListView.AutoSizeTable = false;
            BP_ListView.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            BP_ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            BP_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { colBPName, colBPVersion });
            BP_ListView.Depth = 0;
            BP_ListView.FullRowSelect = true;
            BP_ListView.Location = new System.Drawing.Point(20, 20);
            BP_ListView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BP_ListView.MinimumSize = new System.Drawing.Size(233, 115);
            BP_ListView.MouseLocation = new System.Drawing.Point(-1, -1);
            BP_ListView.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            BP_ListView.Name = "BP_ListView";
            BP_ListView.OwnerDraw = true;
            BP_ListView.Size = new System.Drawing.Size(840, 323);
            BP_ListView.TabIndex = 0;
            BP_ListView.UseCompatibleStateImageBehavior = false;
            BP_ListView.View = System.Windows.Forms.View.Details;
            // 
            // colBPName
            // 
            colBPName.Text = "Pack Name";
            colBPName.Width = 520;
            // 
            // colBPVersion
            // 
            colBPVersion.Text = "Version";
            colBPVersion.Width = 200;
            // 
            // BP_ImportBTN
            // 
            BP_ImportBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BP_ImportBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            BP_ImportBTN.Depth = 0;
            BP_ImportBTN.HighEmphasis = true;
            BP_ImportBTN.Icon = null;
            BP_ImportBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            BP_ImportBTN.Location = new System.Drawing.Point(20, 363);
            BP_ImportBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            BP_ImportBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            BP_ImportBTN.Name = "BP_ImportBTN";
            BP_ImportBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            BP_ImportBTN.Size = new System.Drawing.Size(118, 36);
            BP_ImportBTN.TabIndex = 1;
            BP_ImportBTN.Text = "IMPORT PACK";
            BP_ImportBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            BP_ImportBTN.UseAccentColor = true;
            BP_ImportBTN.Click += ImportBTN_Click;
            // 
            // BP_EnableBTN
            // 
            BP_EnableBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BP_EnableBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            BP_EnableBTN.Depth = 0;
            BP_EnableBTN.HighEmphasis = true;
            BP_EnableBTN.Icon = null;
            BP_EnableBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            BP_EnableBTN.Location = new System.Drawing.Point(663, 363);
            BP_EnableBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            BP_EnableBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            BP_EnableBTN.Name = "BP_EnableBTN";
            BP_EnableBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            BP_EnableBTN.Size = new System.Drawing.Size(76, 36);
            BP_EnableBTN.TabIndex = 2;
            BP_EnableBTN.Text = "ENABLE";
            BP_EnableBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            BP_EnableBTN.UseAccentColor = false;
            BP_EnableBTN.Click += BP_EnableBTN_Click;
            // 
            // BP_DisableBTN
            // 
            BP_DisableBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            BP_DisableBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            BP_DisableBTN.Depth = 0;
            BP_DisableBTN.HighEmphasis = true;
            BP_DisableBTN.Icon = null;
            BP_DisableBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            BP_DisableBTN.Location = new System.Drawing.Point(768, 363);
            BP_DisableBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            BP_DisableBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            BP_DisableBTN.Name = "BP_DisableBTN";
            BP_DisableBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            BP_DisableBTN.Size = new System.Drawing.Size(79, 36);
            BP_DisableBTN.TabIndex = 3;
            BP_DisableBTN.Text = "DISABLE";
            BP_DisableBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            BP_DisableBTN.UseAccentColor = false;
            BP_DisableBTN.Click += BP_DisableBTN_Click;
            // 
            // tabCustomFeatures
            // 
            tabCustomFeatures.AutoScroll = true;
            tabCustomFeatures.BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            tabCustomFeatures.Controls.Add(CF_PlaceholderCard2);
            tabCustomFeatures.Controls.Add(CF_PlaceholderCard1);
            tabCustomFeatures.Controls.Add(CF_CommandsCard);
            tabCustomFeatures.Controls.Add(CF_FeaturesCard);
            tabCustomFeatures.Location = new System.Drawing.Point(4, 24);
            tabCustomFeatures.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabCustomFeatures.Name = "tabCustomFeatures";
            tabCustomFeatures.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabCustomFeatures.Size = new System.Drawing.Size(750, 491);
            tabCustomFeatures.TabIndex = 2;
            tabCustomFeatures.Text = "Custom Features";
            // 
            // CF_PlaceholderCard2
            // 
            CF_PlaceholderCard2.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            CF_PlaceholderCard2.Controls.Add(CF_Placeholder2Switch);
            CF_PlaceholderCard2.Controls.Add(CF_Placeholder2HeaderLBL);
            CF_PlaceholderCard2.Depth = 0;
            CF_PlaceholderCard2.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            CF_PlaceholderCard2.Location = new System.Drawing.Point(20, 623);
            CF_PlaceholderCard2.Margin = new System.Windows.Forms.Padding(16);
            CF_PlaceholderCard2.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_PlaceholderCard2.Name = "CF_PlaceholderCard2";
            CF_PlaceholderCard2.Padding = new System.Windows.Forms.Padding(16);
            CF_PlaceholderCard2.Size = new System.Drawing.Size(822, 127);
            CF_PlaceholderCard2.TabIndex = 3;
            CF_PlaceholderCard2.Visible = false;
            // 
            // CF_Placeholder2Switch
            // 
            CF_Placeholder2Switch.AutoSize = true;
            CF_Placeholder2Switch.Depth = 0;
            CF_Placeholder2Switch.Location = new System.Drawing.Point(20, 63);
            CF_Placeholder2Switch.Margin = new System.Windows.Forms.Padding(0);
            CF_Placeholder2Switch.MouseLocation = new System.Drawing.Point(-1, -1);
            CF_Placeholder2Switch.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_Placeholder2Switch.Name = "CF_Placeholder2Switch";
            CF_Placeholder2Switch.Ripple = true;
            CF_Placeholder2Switch.Size = new System.Drawing.Size(284, 37);
            CF_Placeholder2Switch.TabIndex = 1;
            CF_Placeholder2Switch.Text = "Enable Aggressive Entity Culling";
            CF_Placeholder2Switch.UseAccentColor = false;
            CF_Placeholder2Switch.UseVisualStyleBackColor = true;
            // 
            // CF_Placeholder2HeaderLBL
            // 
            CF_Placeholder2HeaderLBL.AutoSize = true;
            CF_Placeholder2HeaderLBL.Depth = 0;
            CF_Placeholder2HeaderLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            CF_Placeholder2HeaderLBL.Location = new System.Drawing.Point(20, 16);
            CF_Placeholder2HeaderLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CF_Placeholder2HeaderLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_Placeholder2HeaderLBL.Name = "CF_Placeholder2HeaderLBL";
            CF_Placeholder2HeaderLBL.Size = new System.Drawing.Size(197, 19);
            CF_Placeholder2HeaderLBL.TabIndex = 0;
            CF_Placeholder2HeaderLBL.Text = "Server Optimization Module";
            // 
            // CF_PlaceholderCard1
            // 
            CF_PlaceholderCard1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            CF_PlaceholderCard1.Controls.Add(CF_Placeholder1Switch);
            CF_PlaceholderCard1.Controls.Add(CF_Placeholder1HeaderLBL);
            CF_PlaceholderCard1.Depth = 0;
            CF_PlaceholderCard1.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            CF_PlaceholderCard1.Location = new System.Drawing.Point(20, 473);
            CF_PlaceholderCard1.Margin = new System.Windows.Forms.Padding(16);
            CF_PlaceholderCard1.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_PlaceholderCard1.Name = "CF_PlaceholderCard1";
            CF_PlaceholderCard1.Padding = new System.Windows.Forms.Padding(16);
            CF_PlaceholderCard1.Size = new System.Drawing.Size(822, 127);
            CF_PlaceholderCard1.TabIndex = 2;
            CF_PlaceholderCard1.Visible = false;
            // 
            // CF_Placeholder1Switch
            // 
            CF_Placeholder1Switch.AutoSize = true;
            CF_Placeholder1Switch.Depth = 0;
            CF_Placeholder1Switch.Location = new System.Drawing.Point(20, 63);
            CF_Placeholder1Switch.Margin = new System.Windows.Forms.Padding(0);
            CF_Placeholder1Switch.MouseLocation = new System.Drawing.Point(-1, -1);
            CF_Placeholder1Switch.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_Placeholder1Switch.Name = "CF_Placeholder1Switch";
            CF_Placeholder1Switch.Ripple = true;
            CF_Placeholder1Switch.Size = new System.Drawing.Size(306, 37);
            CF_Placeholder1Switch.TabIndex = 1;
            CF_Placeholder1Switch.Text = "Enable Automated Player Economy";
            CF_Placeholder1Switch.UseAccentColor = false;
            CF_Placeholder1Switch.UseVisualStyleBackColor = true;
            // 
            // CF_Placeholder1HeaderLBL
            // 
            CF_Placeholder1HeaderLBL.AutoSize = true;
            CF_Placeholder1HeaderLBL.Depth = 0;
            CF_Placeholder1HeaderLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            CF_Placeholder1HeaderLBL.Location = new System.Drawing.Point(20, 16);
            CF_Placeholder1HeaderLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CF_Placeholder1HeaderLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_Placeholder1HeaderLBL.Name = "CF_Placeholder1HeaderLBL";
            CF_Placeholder1HeaderLBL.Size = new System.Drawing.Size(194, 19);
            CF_Placeholder1HeaderLBL.TabIndex = 0;
            CF_Placeholder1HeaderLBL.Text = "Economy / Trading Module";
            // 
            // CF_CommandsCard
            // 
            CF_CommandsCard.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            CF_CommandsCard.Controls.Add(CF_AddCommandBTN);
            CF_CommandsCard.Controls.Add(CF_CommandsDescLBL);
            CF_CommandsCard.Controls.Add(CF_CommandsHeaderLBL);
            CF_CommandsCard.Depth = 0;
            CF_CommandsCard.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            CF_CommandsCard.Location = new System.Drawing.Point(20, 306);
            CF_CommandsCard.Margin = new System.Windows.Forms.Padding(16);
            CF_CommandsCard.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_CommandsCard.Name = "CF_CommandsCard";
            CF_CommandsCard.Padding = new System.Windows.Forms.Padding(16);
            CF_CommandsCard.Size = new System.Drawing.Size(822, 144);
            CF_CommandsCard.TabIndex = 1;
            CF_CommandsCard.Visible = false;
            // 
            // CF_AddCommandBTN
            // 
            CF_AddCommandBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            CF_AddCommandBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            CF_AddCommandBTN.Depth = 0;
            CF_AddCommandBTN.HighEmphasis = true;
            CF_AddCommandBTN.Icon = null;
            CF_AddCommandBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            CF_AddCommandBTN.Location = new System.Drawing.Point(20, 87);
            CF_AddCommandBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            CF_AddCommandBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_AddCommandBTN.Name = "CF_AddCommandBTN";
            CF_AddCommandBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            CF_AddCommandBTN.Size = new System.Drawing.Size(177, 36);
            CF_AddCommandBTN.TabIndex = 2;
            CF_AddCommandBTN.Text = "BUILD NEW COMMAND";
            CF_AddCommandBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            CF_AddCommandBTN.UseAccentColor = false;
            CF_AddCommandBTN.Click += CF_AddCommandBTN_Click;
            // 
            // CF_CommandsDescLBL
            // 
            CF_CommandsDescLBL.AutoSize = true;
            CF_CommandsDescLBL.Depth = 0;
            CF_CommandsDescLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            CF_CommandsDescLBL.Location = new System.Drawing.Point(20, 58);
            CF_CommandsDescLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CF_CommandsDescLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_CommandsDescLBL.Name = "CF_CommandsDescLBL";
            CF_CommandsDescLBL.Size = new System.Drawing.Size(448, 19);
            CF_CommandsDescLBL.TabIndex = 1;
            CF_CommandsDescLBL.Text = "Build tailored commands, responses, and execution logic flows.";
            // 
            // CF_CommandsHeaderLBL
            // 
            CF_CommandsHeaderLBL.AutoSize = true;
            CF_CommandsHeaderLBL.Depth = 0;
            CF_CommandsHeaderLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            CF_CommandsHeaderLBL.Location = new System.Drawing.Point(20, 16);
            CF_CommandsHeaderLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CF_CommandsHeaderLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_CommandsHeaderLBL.Name = "CF_CommandsHeaderLBL";
            CF_CommandsHeaderLBL.Size = new System.Drawing.Size(177, 19);
            CF_CommandsHeaderLBL.TabIndex = 0;
            CF_CommandsHeaderLBL.Text = "Custom Command Logic";
            // 
            // CF_FeaturesCard
            // 
            CF_FeaturesCard.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            CF_FeaturesCard.Controls.Add(CF_AntiGriefSwitch);
            CF_FeaturesCard.Controls.Add(CF_PvpAwardsSwitch);
            CF_FeaturesCard.Controls.Add(CF_OpCommandsSwitch);
            CF_FeaturesCard.Controls.Add(CF_HeaderLBL);
            CF_FeaturesCard.Depth = 0;
            CF_FeaturesCard.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            CF_FeaturesCard.Location = new System.Drawing.Point(20, 20);
            CF_FeaturesCard.Margin = new System.Windows.Forms.Padding(16);
            CF_FeaturesCard.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_FeaturesCard.Name = "CF_FeaturesCard";
            CF_FeaturesCard.Padding = new System.Windows.Forms.Padding(16);
            CF_FeaturesCard.Size = new System.Drawing.Size(822, 265);
            CF_FeaturesCard.TabIndex = 0;
            CF_FeaturesCard.Visible = false;
            // 
            // CF_AntiGriefSwitch
            // 
            CF_AntiGriefSwitch.AutoSize = true;
            CF_AntiGriefSwitch.Depth = 0;
            CF_AntiGriefSwitch.Location = new System.Drawing.Point(20, 185);
            CF_AntiGriefSwitch.Margin = new System.Windows.Forms.Padding(0);
            CF_AntiGriefSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            CF_AntiGriefSwitch.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_AntiGriefSwitch.Name = "CF_AntiGriefSwitch";
            CF_AntiGriefSwitch.Ripple = true;
            CF_AntiGriefSwitch.Size = new System.Drawing.Size(383, 37);
            CF_AntiGriefSwitch.TabIndex = 3;
            CF_AntiGriefSwitch.Text = "Enforce Anti-Griefing && Spawn-Kill Protection";
            CF_AntiGriefSwitch.UseAccentColor = false;
            CF_AntiGriefSwitch.UseVisualStyleBackColor = true;
            // 
            // CF_PvpAwardsSwitch
            // 
            CF_PvpAwardsSwitch.AutoSize = true;
            CF_PvpAwardsSwitch.Depth = 0;
            CF_PvpAwardsSwitch.Location = new System.Drawing.Point(20, 127);
            CF_PvpAwardsSwitch.Margin = new System.Windows.Forms.Padding(0);
            CF_PvpAwardsSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            CF_PvpAwardsSwitch.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_PvpAwardsSwitch.Name = "CF_PvpAwardsSwitch";
            CF_PvpAwardsSwitch.Ripple = true;
            CF_PvpAwardsSwitch.Size = new System.Drawing.Size(348, 37);
            CF_PvpAwardsSwitch.TabIndex = 2;
            CF_PvpAwardsSwitch.Text = "Enable Automated PVP Killstreak Awards";
            CF_PvpAwardsSwitch.UseAccentColor = false;
            CF_PvpAwardsSwitch.UseVisualStyleBackColor = true;
            // 
            // CF_OpCommandsSwitch
            // 
            CF_OpCommandsSwitch.AutoSize = true;
            CF_OpCommandsSwitch.Depth = 0;
            CF_OpCommandsSwitch.Location = new System.Drawing.Point(20, 69);
            CF_OpCommandsSwitch.Margin = new System.Windows.Forms.Padding(0);
            CF_OpCommandsSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            CF_OpCommandsSwitch.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_OpCommandsSwitch.Name = "CF_OpCommandsSwitch";
            CF_OpCommandsSwitch.Ripple = true;
            CF_OpCommandsSwitch.Size = new System.Drawing.Size(543, 37);
            CF_OpCommandsSwitch.TabIndex = 1;
            CF_OpCommandsSwitch.Text = "OP Command Interpreter (Reject non-OP execution with generic error)";
            CF_OpCommandsSwitch.UseAccentColor = false;
            CF_OpCommandsSwitch.UseVisualStyleBackColor = true;
            // 
            // CF_HeaderLBL
            // 
            CF_HeaderLBL.AutoSize = true;
            CF_HeaderLBL.Depth = 0;
            CF_HeaderLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            CF_HeaderLBL.Location = new System.Drawing.Point(20, 16);
            CF_HeaderLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CF_HeaderLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            CF_HeaderLBL.Name = "CF_HeaderLBL";
            CF_HeaderLBL.Size = new System.Drawing.Size(168, 19);
            CF_HeaderLBL.TabIndex = 0;
            CF_HeaderLBL.Text = "Custom Pack Directives";
            // 
            // AddonManager
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            ClientSize = new System.Drawing.Size(765, 550);
            Controls.Add(PM_TabControl);
            Controls.Add(PM_TabSelector);
            ForeColor = System.Drawing.SystemColors.ControlText;
            FormStyle = ReaLTaiizor.Enum.Material.FormStyles.StatusAndActionBar_None;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "AddonManager";
            Padding = new System.Windows.Forms.Padding(3, 0, 4, 3);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Addon & Pack Management";
            Load += AddonManager_Load;
            DragDrop += AddonManager_DragDrop;
            DragEnter += AddonManager_DragEnter;
            PM_TabControl.ResumeLayout(false);
            tabResourcePacks.ResumeLayout(false);
            tabResourcePacks.PerformLayout();
            tabBehaviorPacks.ResumeLayout(false);
            tabBehaviorPacks.PerformLayout();
            tabCustomFeatures.ResumeLayout(false);
            CF_PlaceholderCard2.ResumeLayout(false);
            CF_PlaceholderCard2.PerformLayout();
            CF_PlaceholderCard1.ResumeLayout(false);
            CF_PlaceholderCard1.PerformLayout();
            CF_CommandsCard.ResumeLayout(false);
            CF_CommandsCard.PerformLayout();
            CF_FeaturesCard.ResumeLayout(false);
            CF_FeaturesCard.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.MaterialTabSelector PM_TabSelector;
        private ReaLTaiizor.Controls.MaterialTabControl PM_TabControl;

        private System.Windows.Forms.TabPage tabResourcePacks;
        private System.Windows.Forms.TabPage tabBehaviorPacks;
        private System.Windows.Forms.TabPage tabCustomFeatures;

        private ReaLTaiizor.Controls.MaterialListView RP_ListView;
        private System.Windows.Forms.ColumnHeader colRPName;
        private System.Windows.Forms.ColumnHeader colRPVersion;
        private ReaLTaiizor.Controls.MaterialButton RP_ImportBTN;
        private ReaLTaiizor.Controls.MaterialButton RP_EnableBTN;
        private ReaLTaiizor.Controls.MaterialButton RP_DisableBTN;

        private ReaLTaiizor.Controls.MaterialListView BP_ListView;
        private System.Windows.Forms.ColumnHeader colBPName;
        private System.Windows.Forms.ColumnHeader colBPVersion;
        private ReaLTaiizor.Controls.MaterialButton BP_ImportBTN;
        private ReaLTaiizor.Controls.MaterialButton BP_EnableBTN;
        private ReaLTaiizor.Controls.MaterialButton BP_DisableBTN;

        private ReaLTaiizor.Controls.MaterialCard CF_FeaturesCard;
        private ReaLTaiizor.Controls.MaterialLabel CF_HeaderLBL;
        private ReaLTaiizor.Controls.MaterialSwitch CF_OpCommandsSwitch;
        private ReaLTaiizor.Controls.MaterialSwitch CF_PvpAwardsSwitch;
        private ReaLTaiizor.Controls.MaterialSwitch CF_AntiGriefSwitch;

        private ReaLTaiizor.Controls.MaterialCard CF_CommandsCard;
        private ReaLTaiizor.Controls.MaterialLabel CF_CommandsHeaderLBL;
        private ReaLTaiizor.Controls.MaterialLabel CF_CommandsDescLBL;
        private ReaLTaiizor.Controls.MaterialButton CF_AddCommandBTN;

        private ReaLTaiizor.Controls.MaterialCard CF_PlaceholderCard1;
        private ReaLTaiizor.Controls.MaterialLabel CF_Placeholder1HeaderLBL;
        private ReaLTaiizor.Controls.MaterialSwitch CF_Placeholder1Switch;

        private ReaLTaiizor.Controls.MaterialCard CF_PlaceholderCard2;
        private ReaLTaiizor.Controls.MaterialLabel CF_Placeholder2HeaderLBL;
        private ReaLTaiizor.Controls.MaterialSwitch CF_Placeholder2Switch;

        private ReaLTaiizor.Controls.MaterialButton PM_CloseBTN;
        private ReaLTaiizor.Controls.MaterialButton RP_DeleteBTN;
        private ReaLTaiizor.Controls.MaterialButton BP_DeleteBTN;
    }
}