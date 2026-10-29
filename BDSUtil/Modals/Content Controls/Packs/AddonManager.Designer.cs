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
            this.PM_TabSelector = new ReaLTaiizor.Controls.MaterialTabSelector();
            this.PM_TabControl = new ReaLTaiizor.Controls.MaterialTabControl();
            this.tabResourcePacks = new System.Windows.Forms.TabPage();
            this.RP_ListView = new ReaLTaiizor.Controls.MaterialListView();
            this.colRPName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colRPVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.RP_ImportBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.RP_EnableBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.RP_DisableBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.tabBehaviorPacks = new System.Windows.Forms.TabPage();
            this.BP_ListView = new ReaLTaiizor.Controls.MaterialListView();
            this.colBPName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colBPVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BP_ImportBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.BP_EnableBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.BP_DisableBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.tabCustomFeatures = new System.Windows.Forms.TabPage();
            this.CF_PlaceholderCard2 = new ReaLTaiizor.Controls.MaterialCard();
            this.CF_Placeholder2Switch = new ReaLTaiizor.Controls.MaterialSwitch();
            this.CF_Placeholder2HeaderLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.CF_PlaceholderCard1 = new ReaLTaiizor.Controls.MaterialCard();
            this.CF_Placeholder1Switch = new ReaLTaiizor.Controls.MaterialSwitch();
            this.CF_Placeholder1HeaderLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.CF_CommandsCard = new ReaLTaiizor.Controls.MaterialCard();
            this.CF_AddCommandBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.CF_CommandsDescLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.CF_CommandsHeaderLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.CF_FeaturesCard = new ReaLTaiizor.Controls.MaterialCard();
            this.CF_AntiGriefSwitch = new ReaLTaiizor.Controls.MaterialSwitch();
            this.CF_PvpAwardsSwitch = new ReaLTaiizor.Controls.MaterialSwitch();
            this.CF_OpCommandsSwitch = new ReaLTaiizor.Controls.MaterialSwitch();
            this.CF_HeaderLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.PM_CloseBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.BP_DeleteBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.RP_DeleteBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.PM_TabControl.SuspendLayout();
            this.tabResourcePacks.SuspendLayout();
            this.tabBehaviorPacks.SuspendLayout();
            this.tabCustomFeatures.SuspendLayout();
            this.CF_PlaceholderCard2.SuspendLayout();
            this.CF_PlaceholderCard1.SuspendLayout();
            this.CF_CommandsCard.SuspendLayout();
            this.CF_FeaturesCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // PM_TabSelector
            // 
            this.PM_TabSelector.BaseTabControl = this.PM_TabControl;
            this.PM_TabSelector.CharacterCasing = ReaLTaiizor.Controls.MaterialTabSelector.CustomCharacterCasing.Normal;
            this.PM_TabSelector.Depth = 0;
            this.PM_TabSelector.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.PM_TabSelector.HeadAlignment = ReaLTaiizor.Controls.MaterialTabSelector.Alignment.Left;
            this.PM_TabSelector.Location = new System.Drawing.Point(0, 64);
            this.PM_TabSelector.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.PM_TabSelector.Name = "PM_TabSelector";
            this.PM_TabSelector.SelectorHideTabName = new string[0];
            this.PM_TabSelector.SelectorNonClickTabPage = new System.Windows.Forms.TabPage[0];
            this.PM_TabSelector.Size = new System.Drawing.Size(800, 48);
            this.PM_TabSelector.TabIndex = 0;
            // 
            // PM_TabControl
            // 
            this.PM_TabControl.Controls.Add(this.tabResourcePacks);
            this.PM_TabControl.Controls.Add(this.tabBehaviorPacks);
            this.PM_TabControl.Controls.Add(this.tabCustomFeatures);
            this.PM_TabControl.Depth = 0;
            this.PM_TabControl.Location = new System.Drawing.Point(17, 125);
            this.PM_TabControl.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.PM_TabControl.Multiline = true;
            this.PM_TabControl.Name = "PM_TabControl";
            this.PM_TabControl.SelectedIndex = 0;
            this.PM_TabControl.Size = new System.Drawing.Size(766, 400);
            this.PM_TabControl.TabIndex = 1;
            // 
            // tabResourcePacks
            // 
            this.tabResourcePacks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabResourcePacks.Controls.Add(this.RP_DeleteBTN);
            this.tabResourcePacks.Controls.Add(this.RP_ListView);
            this.tabResourcePacks.Controls.Add(this.RP_ImportBTN);
            this.tabResourcePacks.Controls.Add(this.RP_EnableBTN);
            this.tabResourcePacks.Controls.Add(this.RP_DisableBTN);
            this.tabResourcePacks.Location = new System.Drawing.Point(4, 22);
            this.tabResourcePacks.Name = "tabResourcePacks";
            this.tabResourcePacks.Padding = new System.Windows.Forms.Padding(3);
            this.tabResourcePacks.Size = new System.Drawing.Size(758, 374);
            this.tabResourcePacks.TabIndex = 0;
            this.tabResourcePacks.Text = "Resource Packs";
            // 
            // RP_ListView
            // 
            this.RP_ListView.AutoSizeTable = false;
            this.RP_ListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RP_ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RP_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colRPName,
            this.colRPVersion});
            this.RP_ListView.Depth = 0;
            this.RP_ListView.FullRowSelect = true;
            this.RP_ListView.HideSelection = false;
            this.RP_ListView.Location = new System.Drawing.Point(17, 17);
            this.RP_ListView.MinimumSize = new System.Drawing.Size(200, 100);
            this.RP_ListView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.RP_ListView.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.RP_ListView.Name = "RP_ListView";
            this.RP_ListView.OwnerDraw = true;
            this.RP_ListView.Size = new System.Drawing.Size(720, 280);
            this.RP_ListView.TabIndex = 0;
            this.RP_ListView.UseCompatibleStateImageBehavior = false;
            this.RP_ListView.View = System.Windows.Forms.View.Details;
            // 
            // colRPName
            // 
            this.colRPName.Text = "Pack Name";
            this.colRPName.Width = 520;
            // 
            // colRPVersion
            // 
            this.colRPVersion.Text = "Version";
            this.colRPVersion.Width = 200;
            // 
            // RP_ImportBTN
            // 
            this.RP_ImportBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RP_ImportBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.RP_ImportBTN.Depth = 0;
            this.RP_ImportBTN.HighEmphasis = true;
            this.RP_ImportBTN.Icon = null;
            this.RP_ImportBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.RP_ImportBTN.Location = new System.Drawing.Point(17, 315);
            this.RP_ImportBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.RP_ImportBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.RP_ImportBTN.Name = "RP_ImportBTN";
            this.RP_ImportBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.RP_ImportBTN.Size = new System.Drawing.Size(118, 36);
            this.RP_ImportBTN.TabIndex = 1;
            this.RP_ImportBTN.Text = "IMPORT PACK";
            this.RP_ImportBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.RP_ImportBTN.UseAccentColor = true;
            this.RP_ImportBTN.Click += new System.EventHandler(this.ImportBTN_Click);
            // 
            // RP_EnableBTN
            // 
            this.RP_EnableBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RP_EnableBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.RP_EnableBTN.Depth = 0;
            this.RP_EnableBTN.HighEmphasis = true;
            this.RP_EnableBTN.Icon = null;
            this.RP_EnableBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.RP_EnableBTN.Location = new System.Drawing.Point(568, 315);
            this.RP_EnableBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.RP_EnableBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.RP_EnableBTN.Name = "RP_EnableBTN";
            this.RP_EnableBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.RP_EnableBTN.Size = new System.Drawing.Size(76, 36);
            this.RP_EnableBTN.TabIndex = 2;
            this.RP_EnableBTN.Text = "ENABLE";
            this.RP_EnableBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.RP_EnableBTN.UseAccentColor = false;
            this.RP_EnableBTN.Click += new System.EventHandler(this.RP_EnableBTN_Click);
            // 
            // RP_DisableBTN
            // 
            this.RP_DisableBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RP_DisableBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.RP_DisableBTN.Depth = 0;
            this.RP_DisableBTN.HighEmphasis = true;
            this.RP_DisableBTN.Icon = null;
            this.RP_DisableBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.RP_DisableBTN.Location = new System.Drawing.Point(658, 315);
            this.RP_DisableBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.RP_DisableBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.RP_DisableBTN.Name = "RP_DisableBTN";
            this.RP_DisableBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.RP_DisableBTN.Size = new System.Drawing.Size(79, 36);
            this.RP_DisableBTN.TabIndex = 3;
            this.RP_DisableBTN.Text = "DISABLE";
            this.RP_DisableBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.RP_DisableBTN.UseAccentColor = false;
            this.RP_DisableBTN.Click += new System.EventHandler(this.RP_DisableBTN_Click);
            // 
            // tabBehaviorPacks
            // 
            this.tabBehaviorPacks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabBehaviorPacks.Controls.Add(this.BP_DeleteBTN);
            this.tabBehaviorPacks.Controls.Add(this.BP_ListView);
            this.tabBehaviorPacks.Controls.Add(this.BP_ImportBTN);
            this.tabBehaviorPacks.Controls.Add(this.BP_EnableBTN);
            this.tabBehaviorPacks.Controls.Add(this.BP_DisableBTN);
            this.tabBehaviorPacks.Location = new System.Drawing.Point(4, 22);
            this.tabBehaviorPacks.Name = "tabBehaviorPacks";
            this.tabBehaviorPacks.Padding = new System.Windows.Forms.Padding(3);
            this.tabBehaviorPacks.Size = new System.Drawing.Size(758, 374);
            this.tabBehaviorPacks.TabIndex = 1;
            this.tabBehaviorPacks.Text = "Behavior Packs";
            // 
            // BP_ListView
            // 
            this.BP_ListView.AutoSizeTable = false;
            this.BP_ListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.BP_ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BP_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colBPName,
            this.colBPVersion});
            this.BP_ListView.Depth = 0;
            this.BP_ListView.FullRowSelect = true;
            this.BP_ListView.HideSelection = false;
            this.BP_ListView.Location = new System.Drawing.Point(17, 17);
            this.BP_ListView.MinimumSize = new System.Drawing.Size(200, 100);
            this.BP_ListView.MouseLocation = new System.Drawing.Point(-1, -1);
            this.BP_ListView.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.BP_ListView.Name = "BP_ListView";
            this.BP_ListView.OwnerDraw = true;
            this.BP_ListView.Size = new System.Drawing.Size(720, 280);
            this.BP_ListView.TabIndex = 0;
            this.BP_ListView.UseCompatibleStateImageBehavior = false;
            this.BP_ListView.View = System.Windows.Forms.View.Details;
            // 
            // colBPName
            // 
            this.colBPName.Text = "Pack Name";
            this.colBPName.Width = 520;
            // 
            // colBPVersion
            // 
            this.colBPVersion.Text = "Version";
            this.colBPVersion.Width = 200;
            // 
            // BP_ImportBTN
            // 
            this.BP_ImportBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BP_ImportBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.BP_ImportBTN.Depth = 0;
            this.BP_ImportBTN.HighEmphasis = true;
            this.BP_ImportBTN.Icon = null;
            this.BP_ImportBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.BP_ImportBTN.Location = new System.Drawing.Point(17, 315);
            this.BP_ImportBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.BP_ImportBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.BP_ImportBTN.Name = "BP_ImportBTN";
            this.BP_ImportBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.BP_ImportBTN.Size = new System.Drawing.Size(118, 36);
            this.BP_ImportBTN.TabIndex = 1;
            this.BP_ImportBTN.Text = "IMPORT PACK";
            this.BP_ImportBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.BP_ImportBTN.UseAccentColor = true;
            this.BP_ImportBTN.Click += new System.EventHandler(this.ImportBTN_Click);
            // 
            // BP_EnableBTN
            // 
            this.BP_EnableBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BP_EnableBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.BP_EnableBTN.Depth = 0;
            this.BP_EnableBTN.HighEmphasis = true;
            this.BP_EnableBTN.Icon = null;
            this.BP_EnableBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.BP_EnableBTN.Location = new System.Drawing.Point(568, 315);
            this.BP_EnableBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.BP_EnableBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.BP_EnableBTN.Name = "BP_EnableBTN";
            this.BP_EnableBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.BP_EnableBTN.Size = new System.Drawing.Size(76, 36);
            this.BP_EnableBTN.TabIndex = 2;
            this.BP_EnableBTN.Text = "ENABLE";
            this.BP_EnableBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.BP_EnableBTN.UseAccentColor = false;
            this.BP_EnableBTN.Click += new System.EventHandler(this.BP_EnableBTN_Click);
            // 
            // BP_DisableBTN
            // 
            this.BP_DisableBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BP_DisableBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.BP_DisableBTN.Depth = 0;
            this.BP_DisableBTN.HighEmphasis = true;
            this.BP_DisableBTN.Icon = null;
            this.BP_DisableBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.BP_DisableBTN.Location = new System.Drawing.Point(658, 315);
            this.BP_DisableBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.BP_DisableBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.BP_DisableBTN.Name = "BP_DisableBTN";
            this.BP_DisableBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.BP_DisableBTN.Size = new System.Drawing.Size(79, 36);
            this.BP_DisableBTN.TabIndex = 3;
            this.BP_DisableBTN.Text = "DISABLE";
            this.BP_DisableBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.BP_DisableBTN.UseAccentColor = false;
            this.BP_DisableBTN.Click += new System.EventHandler(this.BP_DisableBTN_Click);
            // 
            // tabCustomFeatures
            // 
            this.tabCustomFeatures.AutoScroll = true;
            this.tabCustomFeatures.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.tabCustomFeatures.Controls.Add(this.CF_PlaceholderCard2);
            this.tabCustomFeatures.Controls.Add(this.CF_PlaceholderCard1);
            this.tabCustomFeatures.Controls.Add(this.CF_CommandsCard);
            this.tabCustomFeatures.Controls.Add(this.CF_FeaturesCard);
            this.tabCustomFeatures.Location = new System.Drawing.Point(4, 22);
            this.tabCustomFeatures.Name = "tabCustomFeatures";
            this.tabCustomFeatures.Padding = new System.Windows.Forms.Padding(3);
            this.tabCustomFeatures.Size = new System.Drawing.Size(758, 374);
            this.tabCustomFeatures.TabIndex = 2;
            this.tabCustomFeatures.Text = "Custom Features";
            // 
            // CF_PlaceholderCard2
            // 
            this.CF_PlaceholderCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.CF_PlaceholderCard2.Controls.Add(this.CF_Placeholder2Switch);
            this.CF_PlaceholderCard2.Controls.Add(this.CF_Placeholder2HeaderLBL);
            this.CF_PlaceholderCard2.Depth = 0;
            this.CF_PlaceholderCard2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CF_PlaceholderCard2.Location = new System.Drawing.Point(17, 540);
            this.CF_PlaceholderCard2.Margin = new System.Windows.Forms.Padding(14);
            this.CF_PlaceholderCard2.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_PlaceholderCard2.Name = "CF_PlaceholderCard2";
            this.CF_PlaceholderCard2.Padding = new System.Windows.Forms.Padding(14);
            this.CF_PlaceholderCard2.Size = new System.Drawing.Size(705, 110);
            this.CF_PlaceholderCard2.TabIndex = 3;
            this.CF_PlaceholderCard2.Visible = false;
            // 
            // CF_Placeholder2Switch
            // 
            this.CF_Placeholder2Switch.AutoSize = true;
            this.CF_Placeholder2Switch.Depth = 0;
            this.CF_Placeholder2Switch.Location = new System.Drawing.Point(17, 55);
            this.CF_Placeholder2Switch.Margin = new System.Windows.Forms.Padding(0);
            this.CF_Placeholder2Switch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.CF_Placeholder2Switch.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_Placeholder2Switch.Name = "CF_Placeholder2Switch";
            this.CF_Placeholder2Switch.Ripple = true;
            this.CF_Placeholder2Switch.Size = new System.Drawing.Size(284, 37);
            this.CF_Placeholder2Switch.TabIndex = 1;
            this.CF_Placeholder2Switch.Text = "Enable Aggressive Entity Culling";
            this.CF_Placeholder2Switch.UseAccentColor = false;
            this.CF_Placeholder2Switch.UseVisualStyleBackColor = true;
            // 
            // CF_Placeholder2HeaderLBL
            // 
            this.CF_Placeholder2HeaderLBL.AutoSize = true;
            this.CF_Placeholder2HeaderLBL.Depth = 0;
            this.CF_Placeholder2HeaderLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CF_Placeholder2HeaderLBL.Location = new System.Drawing.Point(17, 14);
            this.CF_Placeholder2HeaderLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_Placeholder2HeaderLBL.Name = "CF_Placeholder2HeaderLBL";
            this.CF_Placeholder2HeaderLBL.Size = new System.Drawing.Size(197, 19);
            this.CF_Placeholder2HeaderLBL.TabIndex = 0;
            this.CF_Placeholder2HeaderLBL.Text = "Server Optimization Module";
            // 
            // CF_PlaceholderCard1
            // 
            this.CF_PlaceholderCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.CF_PlaceholderCard1.Controls.Add(this.CF_Placeholder1Switch);
            this.CF_PlaceholderCard1.Controls.Add(this.CF_Placeholder1HeaderLBL);
            this.CF_PlaceholderCard1.Depth = 0;
            this.CF_PlaceholderCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CF_PlaceholderCard1.Location = new System.Drawing.Point(17, 410);
            this.CF_PlaceholderCard1.Margin = new System.Windows.Forms.Padding(14);
            this.CF_PlaceholderCard1.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_PlaceholderCard1.Name = "CF_PlaceholderCard1";
            this.CF_PlaceholderCard1.Padding = new System.Windows.Forms.Padding(14);
            this.CF_PlaceholderCard1.Size = new System.Drawing.Size(705, 110);
            this.CF_PlaceholderCard1.TabIndex = 2;
            this.CF_PlaceholderCard1.Visible = false;
            // 
            // CF_Placeholder1Switch
            // 
            this.CF_Placeholder1Switch.AutoSize = true;
            this.CF_Placeholder1Switch.Depth = 0;
            this.CF_Placeholder1Switch.Location = new System.Drawing.Point(17, 55);
            this.CF_Placeholder1Switch.Margin = new System.Windows.Forms.Padding(0);
            this.CF_Placeholder1Switch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.CF_Placeholder1Switch.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_Placeholder1Switch.Name = "CF_Placeholder1Switch";
            this.CF_Placeholder1Switch.Ripple = true;
            this.CF_Placeholder1Switch.Size = new System.Drawing.Size(306, 37);
            this.CF_Placeholder1Switch.TabIndex = 1;
            this.CF_Placeholder1Switch.Text = "Enable Automated Player Economy";
            this.CF_Placeholder1Switch.UseAccentColor = false;
            this.CF_Placeholder1Switch.UseVisualStyleBackColor = true;
            // 
            // CF_Placeholder1HeaderLBL
            // 
            this.CF_Placeholder1HeaderLBL.AutoSize = true;
            this.CF_Placeholder1HeaderLBL.Depth = 0;
            this.CF_Placeholder1HeaderLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CF_Placeholder1HeaderLBL.Location = new System.Drawing.Point(17, 14);
            this.CF_Placeholder1HeaderLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_Placeholder1HeaderLBL.Name = "CF_Placeholder1HeaderLBL";
            this.CF_Placeholder1HeaderLBL.Size = new System.Drawing.Size(194, 19);
            this.CF_Placeholder1HeaderLBL.TabIndex = 0;
            this.CF_Placeholder1HeaderLBL.Text = "Economy / Trading Module";
            // 
            // CF_CommandsCard
            // 
            this.CF_CommandsCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.CF_CommandsCard.Controls.Add(this.CF_AddCommandBTN);
            this.CF_CommandsCard.Controls.Add(this.CF_CommandsDescLBL);
            this.CF_CommandsCard.Controls.Add(this.CF_CommandsHeaderLBL);
            this.CF_CommandsCard.Depth = 0;
            this.CF_CommandsCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CF_CommandsCard.Location = new System.Drawing.Point(17, 265);
            this.CF_CommandsCard.Margin = new System.Windows.Forms.Padding(14);
            this.CF_CommandsCard.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_CommandsCard.Name = "CF_CommandsCard";
            this.CF_CommandsCard.Padding = new System.Windows.Forms.Padding(14);
            this.CF_CommandsCard.Size = new System.Drawing.Size(705, 125);
            this.CF_CommandsCard.TabIndex = 1;
            this.CF_CommandsCard.Visible = false;
            // 
            // CF_AddCommandBTN
            // 
            this.CF_AddCommandBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CF_AddCommandBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.CF_AddCommandBTN.Depth = 0;
            this.CF_AddCommandBTN.HighEmphasis = true;
            this.CF_AddCommandBTN.Icon = null;
            this.CF_AddCommandBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.CF_AddCommandBTN.Location = new System.Drawing.Point(17, 75);
            this.CF_AddCommandBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.CF_AddCommandBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_AddCommandBTN.Name = "CF_AddCommandBTN";
            this.CF_AddCommandBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.CF_AddCommandBTN.Size = new System.Drawing.Size(177, 36);
            this.CF_AddCommandBTN.TabIndex = 2;
            this.CF_AddCommandBTN.Text = "BUILD NEW COMMAND";
            this.CF_AddCommandBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.CF_AddCommandBTN.UseAccentColor = false;
            this.CF_AddCommandBTN.Click += new System.EventHandler(this.CF_AddCommandBTN_Click);
            // 
            // CF_CommandsDescLBL
            // 
            this.CF_CommandsDescLBL.AutoSize = true;
            this.CF_CommandsDescLBL.Depth = 0;
            this.CF_CommandsDescLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CF_CommandsDescLBL.Location = new System.Drawing.Point(17, 50);
            this.CF_CommandsDescLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_CommandsDescLBL.Name = "CF_CommandsDescLBL";
            this.CF_CommandsDescLBL.Size = new System.Drawing.Size(448, 19);
            this.CF_CommandsDescLBL.TabIndex = 1;
            this.CF_CommandsDescLBL.Text = "Build tailored commands, responses, and execution logic flows.";
            // 
            // CF_CommandsHeaderLBL
            // 
            this.CF_CommandsHeaderLBL.AutoSize = true;
            this.CF_CommandsHeaderLBL.Depth = 0;
            this.CF_CommandsHeaderLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CF_CommandsHeaderLBL.Location = new System.Drawing.Point(17, 14);
            this.CF_CommandsHeaderLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_CommandsHeaderLBL.Name = "CF_CommandsHeaderLBL";
            this.CF_CommandsHeaderLBL.Size = new System.Drawing.Size(177, 19);
            this.CF_CommandsHeaderLBL.TabIndex = 0;
            this.CF_CommandsHeaderLBL.Text = "Custom Command Logic";
            // 
            // CF_FeaturesCard
            // 
            this.CF_FeaturesCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.CF_FeaturesCard.Controls.Add(this.CF_AntiGriefSwitch);
            this.CF_FeaturesCard.Controls.Add(this.CF_PvpAwardsSwitch);
            this.CF_FeaturesCard.Controls.Add(this.CF_OpCommandsSwitch);
            this.CF_FeaturesCard.Controls.Add(this.CF_HeaderLBL);
            this.CF_FeaturesCard.Depth = 0;
            this.CF_FeaturesCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CF_FeaturesCard.Location = new System.Drawing.Point(17, 17);
            this.CF_FeaturesCard.Margin = new System.Windows.Forms.Padding(14);
            this.CF_FeaturesCard.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_FeaturesCard.Name = "CF_FeaturesCard";
            this.CF_FeaturesCard.Padding = new System.Windows.Forms.Padding(14);
            this.CF_FeaturesCard.Size = new System.Drawing.Size(705, 230);
            this.CF_FeaturesCard.TabIndex = 0;
            this.CF_FeaturesCard.Visible = false;
            // 
            // CF_AntiGriefSwitch
            // 
            this.CF_AntiGriefSwitch.AutoSize = true;
            this.CF_AntiGriefSwitch.Depth = 0;
            this.CF_AntiGriefSwitch.Location = new System.Drawing.Point(17, 160);
            this.CF_AntiGriefSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.CF_AntiGriefSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.CF_AntiGriefSwitch.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_AntiGriefSwitch.Name = "CF_AntiGriefSwitch";
            this.CF_AntiGriefSwitch.Ripple = true;
            this.CF_AntiGriefSwitch.Size = new System.Drawing.Size(383, 37);
            this.CF_AntiGriefSwitch.TabIndex = 3;
            this.CF_AntiGriefSwitch.Text = "Enforce Anti-Griefing && Spawn-Kill Protection";
            this.CF_AntiGriefSwitch.UseAccentColor = false;
            this.CF_AntiGriefSwitch.UseVisualStyleBackColor = true;
            // 
            // CF_PvpAwardsSwitch
            // 
            this.CF_PvpAwardsSwitch.AutoSize = true;
            this.CF_PvpAwardsSwitch.Depth = 0;
            this.CF_PvpAwardsSwitch.Location = new System.Drawing.Point(17, 110);
            this.CF_PvpAwardsSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.CF_PvpAwardsSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.CF_PvpAwardsSwitch.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_PvpAwardsSwitch.Name = "CF_PvpAwardsSwitch";
            this.CF_PvpAwardsSwitch.Ripple = true;
            this.CF_PvpAwardsSwitch.Size = new System.Drawing.Size(348, 37);
            this.CF_PvpAwardsSwitch.TabIndex = 2;
            this.CF_PvpAwardsSwitch.Text = "Enable Automated PVP Killstreak Awards";
            this.CF_PvpAwardsSwitch.UseAccentColor = false;
            this.CF_PvpAwardsSwitch.UseVisualStyleBackColor = true;
            // 
            // CF_OpCommandsSwitch
            // 
            this.CF_OpCommandsSwitch.AutoSize = true;
            this.CF_OpCommandsSwitch.Depth = 0;
            this.CF_OpCommandsSwitch.Location = new System.Drawing.Point(17, 60);
            this.CF_OpCommandsSwitch.Margin = new System.Windows.Forms.Padding(0);
            this.CF_OpCommandsSwitch.MouseLocation = new System.Drawing.Point(-1, -1);
            this.CF_OpCommandsSwitch.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_OpCommandsSwitch.Name = "CF_OpCommandsSwitch";
            this.CF_OpCommandsSwitch.Ripple = true;
            this.CF_OpCommandsSwitch.Size = new System.Drawing.Size(543, 37);
            this.CF_OpCommandsSwitch.TabIndex = 1;
            this.CF_OpCommandsSwitch.Text = "OP Command Interpreter (Reject non-OP execution with generic error)";
            this.CF_OpCommandsSwitch.UseAccentColor = false;
            this.CF_OpCommandsSwitch.UseVisualStyleBackColor = true;
            // 
            // CF_HeaderLBL
            // 
            this.CF_HeaderLBL.AutoSize = true;
            this.CF_HeaderLBL.Depth = 0;
            this.CF_HeaderLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.CF_HeaderLBL.Location = new System.Drawing.Point(17, 14);
            this.CF_HeaderLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CF_HeaderLBL.Name = "CF_HeaderLBL";
            this.CF_HeaderLBL.Size = new System.Drawing.Size(168, 19);
            this.CF_HeaderLBL.TabIndex = 0;
            this.CF_HeaderLBL.Text = "Custom Pack Directives";
            // 
            // PM_CloseBTN
            // 
            this.PM_CloseBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PM_CloseBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.PM_CloseBTN.Depth = 0;
            this.PM_CloseBTN.HighEmphasis = true;
            this.PM_CloseBTN.Icon = null;
            this.PM_CloseBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.PM_CloseBTN.Location = new System.Drawing.Point(709, 540);
            this.PM_CloseBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.PM_CloseBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.PM_CloseBTN.Name = "PM_CloseBTN";
            this.PM_CloseBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.PM_CloseBTN.Size = new System.Drawing.Size(66, 36);
            this.PM_CloseBTN.TabIndex = 2;
            this.PM_CloseBTN.Text = "CLOSE";
            this.PM_CloseBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.PM_CloseBTN.UseAccentColor = true;
            this.PM_CloseBTN.Click += new System.EventHandler(this.PM_CloseBTN_Click);
            // 
            // BP_DeleteBTN
            // 
            this.BP_DeleteBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BP_DeleteBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.BP_DeleteBTN.Depth = 0;
            this.BP_DeleteBTN.HighEmphasis = true;
            this.BP_DeleteBTN.Icon = null;
            this.BP_DeleteBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.BP_DeleteBTN.Location = new System.Drawing.Point(143, 315);
            this.BP_DeleteBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.BP_DeleteBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.BP_DeleteBTN.Name = "BP_DeleteBTN";
            this.BP_DeleteBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.BP_DeleteBTN.Size = new System.Drawing.Size(115, 36);
            this.BP_DeleteBTN.TabIndex = 4;
            this.BP_DeleteBTN.Text = "DELETE PACK";
            this.BP_DeleteBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.BP_DeleteBTN.UseAccentColor = true;
            this.BP_DeleteBTN.Click += new System.EventHandler(this.DeleteBTN_Click);
            // 
            // RP_DeleteBTN
            // 
            this.RP_DeleteBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.RP_DeleteBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.RP_DeleteBTN.Depth = 0;
            this.RP_DeleteBTN.HighEmphasis = true;
            this.RP_DeleteBTN.Icon = null;
            this.RP_DeleteBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.RP_DeleteBTN.Location = new System.Drawing.Point(143, 315);
            this.RP_DeleteBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.RP_DeleteBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.RP_DeleteBTN.Name = "RP_DeleteBTN";
            this.RP_DeleteBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.RP_DeleteBTN.Size = new System.Drawing.Size(115, 36);
            this.RP_DeleteBTN.TabIndex = 5;
            this.RP_DeleteBTN.Text = "DELETE PACK";
            this.RP_DeleteBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.RP_DeleteBTN.UseAccentColor = true;
            this.RP_DeleteBTN.Click += new System.EventHandler(this.DeleteBTN_Click);
            // 
            // AddonManager
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.PM_TabSelector);
            this.Controls.Add(this.PM_TabControl);
            this.Controls.Add(this.PM_CloseBTN);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AddonManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Addon & Pack Management";
            this.Load += new System.EventHandler(this.AddonManager_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.AddonManager_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.AddonManager_DragEnter);
            this.PM_TabControl.ResumeLayout(false);
            this.tabResourcePacks.ResumeLayout(false);
            this.tabResourcePacks.PerformLayout();
            this.tabBehaviorPacks.ResumeLayout(false);
            this.tabBehaviorPacks.PerformLayout();
            this.tabCustomFeatures.ResumeLayout(false);
            this.CF_PlaceholderCard2.ResumeLayout(false);
            this.CF_PlaceholderCard2.PerformLayout();
            this.CF_PlaceholderCard1.ResumeLayout(false);
            this.CF_PlaceholderCard1.PerformLayout();
            this.CF_CommandsCard.ResumeLayout(false);
            this.CF_CommandsCard.PerformLayout();
            this.CF_FeaturesCard.ResumeLayout(false);
            this.CF_FeaturesCard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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