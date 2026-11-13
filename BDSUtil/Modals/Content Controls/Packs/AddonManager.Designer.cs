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
            PM_TabSelector = new BDSUtil.UI.MaterialTabSelector();
            PM_TabControl = new BDSUtil.UI.MaterialTabControl();
            tabResourcePacks = new System.Windows.Forms.TabPage();
            RP_DeleteBTN = new Material3.WinForms.Controls.MaterialButton();
            PM_CloseBTN = new Material3.WinForms.Controls.MaterialButton();
            RP_ListView = new BDSUtil.UI.MaterialListView();
            colRPName = new System.Windows.Forms.ColumnHeader();
            colRPVersion = new System.Windows.Forms.ColumnHeader();
            RP_ImportBTN = new Material3.WinForms.Controls.MaterialButton();
            RP_EnableBTN = new Material3.WinForms.Controls.MaterialButton();
            RP_DisableBTN = new Material3.WinForms.Controls.MaterialButton();
            tabBehaviorPacks = new System.Windows.Forms.TabPage();
            materialButton1 = new Material3.WinForms.Controls.MaterialButton();
            BP_DeleteBTN = new Material3.WinForms.Controls.MaterialButton();
            BP_ListView = new BDSUtil.UI.MaterialListView();
            colBPName = new System.Windows.Forms.ColumnHeader();
            colBPVersion = new System.Windows.Forms.ColumnHeader();
            BP_ImportBTN = new Material3.WinForms.Controls.MaterialButton();
            BP_EnableBTN = new Material3.WinForms.Controls.MaterialButton();
            BP_DisableBTN = new Material3.WinForms.Controls.MaterialButton();
            tabCustomFeatures = new System.Windows.Forms.TabPage();
            CF_PlaceholderCard2 = new Material3.WinForms.Controls.MaterialCard();
            CF_Placeholder2Switch = new Material3.WinForms.Controls.MaterialSwitch();
            CF_Placeholder2HeaderLBL = new Material3.WinForms.Controls.MaterialLabel();
            CF_PlaceholderCard1 = new Material3.WinForms.Controls.MaterialCard();
            CF_Placeholder1Switch = new Material3.WinForms.Controls.MaterialSwitch();
            CF_Placeholder1HeaderLBL = new Material3.WinForms.Controls.MaterialLabel();
            CF_CommandsCard = new Material3.WinForms.Controls.MaterialCard();
            CF_AddCommandBTN = new Material3.WinForms.Controls.MaterialButton();
            CF_CommandsDescLBL = new Material3.WinForms.Controls.MaterialLabel();
            CF_CommandsHeaderLBL = new Material3.WinForms.Controls.MaterialLabel();
            CF_FeaturesCard = new Material3.WinForms.Controls.MaterialCard();
            CF_AntiGriefSwitch = new Material3.WinForms.Controls.MaterialSwitch();
            CF_PvpAwardsSwitch = new Material3.WinForms.Controls.MaterialSwitch();
            CF_OpCommandsSwitch = new Material3.WinForms.Controls.MaterialSwitch();
            CF_HeaderLBL = new Material3.WinForms.Controls.MaterialLabel();
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
            PM_TabSelector.Dock = System.Windows.Forms.DockStyle.Top;
            PM_TabSelector.Location = new System.Drawing.Point(3, 40);
            PM_TabSelector.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PM_TabSelector.Name = "PM_TabSelector";
            PM_TabSelector.Size = new System.Drawing.Size(758, 48);
            PM_TabSelector.TabIndex = 0;
            // 
            // PM_TabControl
            // 
            PM_TabControl.Controls.Add(tabResourcePacks);
            PM_TabControl.Controls.Add(tabBehaviorPacks);
            PM_TabControl.Controls.Add(tabCustomFeatures);
            PM_TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            PM_TabControl.Location = new System.Drawing.Point(3, 88);
            PM_TabControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PM_TabControl.Multiline = true;
            PM_TabControl.Name = "PM_TabControl";
            PM_TabControl.SelectedIndex = 0;
            PM_TabControl.Size = new System.Drawing.Size(758, 498);
            PM_TabControl.TabIndex = 1;
            // 
            // tabResourcePacks
            // 
            tabResourcePacks.BackColor = Material3.WinForms.Theming.MaterialColors.Surface;
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
            tabResourcePacks.Size = new System.Drawing.Size(750, 470);
            tabResourcePacks.TabIndex = 0;
            tabResourcePacks.Text = "Resource Packs";
            // 
            // RP_DeleteBTN
            // 
            RP_DeleteBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            RP_DeleteBTN.AutoSize = false;
            RP_DeleteBTN.Location = new System.Drawing.Point(167, 368);
            RP_DeleteBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            RP_DeleteBTN.Name = "RP_DeleteBTN";
            RP_DeleteBTN.Size = new System.Drawing.Size(130, 38);
            RP_DeleteBTN.TabIndex = 5;
            RP_DeleteBTN.Text = "DELETE PACK";
            RP_DeleteBTN.Tone = Material3.WinForms.Controls.MaterialTone.Error;
            RP_DeleteBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Tonal;
            RP_DeleteBTN.Click += DeleteBTN_Click;
            // 
            // PM_CloseBTN
            // 
            PM_CloseBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            PM_CloseBTN.AutoSize = false;
            PM_CloseBTN.Location = new System.Drawing.Point(642, 418);
            PM_CloseBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            PM_CloseBTN.Name = "PM_CloseBTN";
            PM_CloseBTN.Size = new System.Drawing.Size(88, 38);
            PM_CloseBTN.TabIndex = 2;
            PM_CloseBTN.Text = "CLOSE";
            PM_CloseBTN.Tone = Material3.WinForms.Controls.MaterialTone.Neutral;
            PM_CloseBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Outlined;
            PM_CloseBTN.Click += PM_CloseBTN_Click;
            // 
            // RP_ListView
            // 
            RP_ListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            RP_ListView.AutoSizeTable = false;
            RP_ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            RP_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { colRPName, colRPVersion });
            RP_ListView.FullRowSelect = true;
            RP_ListView.Location = new System.Drawing.Point(20, 20);
            RP_ListView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            RP_ListView.MinimumSize = new System.Drawing.Size(233, 115);
            RP_ListView.MouseLocation = 5;
            RP_ListView.Name = "RP_ListView";
            RP_ListView.OwnerDraw = true;
            RP_ListView.Size = new System.Drawing.Size(710, 336);
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
            RP_ImportBTN.AutoSize = false;
            RP_ImportBTN.Location = new System.Drawing.Point(20, 368);
            RP_ImportBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            RP_ImportBTN.Name = "RP_ImportBTN";
            RP_ImportBTN.Size = new System.Drawing.Size(134, 38);
            RP_ImportBTN.TabIndex = 1;
            RP_ImportBTN.Text = "IMPORT PACK";
            RP_ImportBTN.Tone = Material3.WinForms.Controls.MaterialTone.Accent;
            RP_ImportBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Filled;
            RP_ImportBTN.Click += ImportBTN_Click;
            // 
            // RP_EnableBTN
            // 
            RP_EnableBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            RP_EnableBTN.AutoSize = false;
            RP_EnableBTN.Location = new System.Drawing.Point(524, 368);
            RP_EnableBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            RP_EnableBTN.Name = "RP_EnableBTN";
            RP_EnableBTN.Size = new System.Drawing.Size(96, 38);
            RP_EnableBTN.TabIndex = 2;
            RP_EnableBTN.Text = "ENABLE";
            RP_EnableBTN.Tone = Material3.WinForms.Controls.MaterialTone.Success;
            RP_EnableBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Tonal;
            RP_EnableBTN.Click += RP_EnableBTN_Click;
            // 
            // RP_DisableBTN
            // 
            RP_DisableBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            RP_DisableBTN.AutoSize = false;
            RP_DisableBTN.Location = new System.Drawing.Point(632, 368);
            RP_DisableBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            RP_DisableBTN.Name = "RP_DisableBTN";
            RP_DisableBTN.Size = new System.Drawing.Size(98, 38);
            RP_DisableBTN.TabIndex = 3;
            RP_DisableBTN.Text = "DISABLE";
            RP_DisableBTN.Tone = Material3.WinForms.Controls.MaterialTone.Neutral;
            RP_DisableBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Outlined;
            RP_DisableBTN.Click += RP_DisableBTN_Click;
            // 
            // tabBehaviorPacks
            // 
            tabBehaviorPacks.BackColor = Material3.WinForms.Theming.MaterialColors.Surface;
            tabBehaviorPacks.Controls.Add(materialButton1);
            tabBehaviorPacks.Controls.Add(BP_DeleteBTN);
            tabBehaviorPacks.Controls.Add(BP_ListView);
            tabBehaviorPacks.Controls.Add(BP_ImportBTN);
            tabBehaviorPacks.Controls.Add(BP_EnableBTN);
            tabBehaviorPacks.Controls.Add(BP_DisableBTN);
            tabBehaviorPacks.Location = new System.Drawing.Point(4, 24);
            tabBehaviorPacks.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabBehaviorPacks.Name = "tabBehaviorPacks";
            tabBehaviorPacks.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabBehaviorPacks.Size = new System.Drawing.Size(750, 470);
            tabBehaviorPacks.TabIndex = 1;
            tabBehaviorPacks.Text = "Behavior Packs";
            // 
            // materialButton1
            // 
            materialButton1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            materialButton1.AutoSize = false;
            materialButton1.Location = new System.Drawing.Point(642, 418);
            materialButton1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            materialButton1.Name = "materialButton1";
            materialButton1.Size = new System.Drawing.Size(88, 38);
            materialButton1.TabIndex = 5;
            materialButton1.Text = "CLOSE";
            materialButton1.Tone = Material3.WinForms.Controls.MaterialTone.Neutral;
            materialButton1.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Outlined;
            materialButton1.Click += PM_CloseBTN_Click;
            // 
            // BP_DeleteBTN
            // 
            BP_DeleteBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BP_DeleteBTN.AutoSize = false;
            BP_DeleteBTN.Location = new System.Drawing.Point(167, 368);
            BP_DeleteBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            BP_DeleteBTN.Name = "BP_DeleteBTN";
            BP_DeleteBTN.Size = new System.Drawing.Size(130, 38);
            BP_DeleteBTN.TabIndex = 4;
            BP_DeleteBTN.Text = "DELETE PACK";
            BP_DeleteBTN.Tone = Material3.WinForms.Controls.MaterialTone.Error;
            BP_DeleteBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Tonal;
            BP_DeleteBTN.Click += DeleteBTN_Click;
            // 
            // BP_ListView
            // 
            BP_ListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            BP_ListView.AutoSizeTable = false;
            BP_ListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            BP_ListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { colBPName, colBPVersion });
            BP_ListView.FullRowSelect = true;
            BP_ListView.Location = new System.Drawing.Point(20, 20);
            BP_ListView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            BP_ListView.MinimumSize = new System.Drawing.Size(233, 115);
            BP_ListView.MouseLocation = 5;
            BP_ListView.Name = "BP_ListView";
            BP_ListView.OwnerDraw = true;
            BP_ListView.Size = new System.Drawing.Size(710, 336);
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
            BP_ImportBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            BP_ImportBTN.AutoSize = false;
            BP_ImportBTN.Location = new System.Drawing.Point(20, 368);
            BP_ImportBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            BP_ImportBTN.Name = "BP_ImportBTN";
            BP_ImportBTN.Size = new System.Drawing.Size(134, 38);
            BP_ImportBTN.TabIndex = 1;
            BP_ImportBTN.Text = "IMPORT PACK";
            BP_ImportBTN.Tone = Material3.WinForms.Controls.MaterialTone.Accent;
            BP_ImportBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Filled;
            BP_ImportBTN.Click += ImportBTN_Click;
            // 
            // BP_EnableBTN
            // 
            BP_EnableBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            BP_EnableBTN.AutoSize = false;
            BP_EnableBTN.Location = new System.Drawing.Point(524, 368);
            BP_EnableBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            BP_EnableBTN.Name = "BP_EnableBTN";
            BP_EnableBTN.Size = new System.Drawing.Size(96, 38);
            BP_EnableBTN.TabIndex = 2;
            BP_EnableBTN.Text = "ENABLE";
            BP_EnableBTN.Tone = Material3.WinForms.Controls.MaterialTone.Success;
            BP_EnableBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Tonal;
            BP_EnableBTN.Click += BP_EnableBTN_Click;
            // 
            // BP_DisableBTN
            // 
            BP_DisableBTN.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            BP_DisableBTN.AutoSize = false;
            BP_DisableBTN.Location = new System.Drawing.Point(632, 368);
            BP_DisableBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            BP_DisableBTN.Name = "BP_DisableBTN";
            BP_DisableBTN.Size = new System.Drawing.Size(98, 38);
            BP_DisableBTN.TabIndex = 3;
            BP_DisableBTN.Text = "DISABLE";
            BP_DisableBTN.Tone = Material3.WinForms.Controls.MaterialTone.Neutral;
            BP_DisableBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Outlined;
            BP_DisableBTN.Click += BP_DisableBTN_Click;
            // 
            // tabCustomFeatures
            // 
            tabCustomFeatures.AutoScroll = true;
            tabCustomFeatures.BackColor = Material3.WinForms.Theming.MaterialColors.Surface;
            tabCustomFeatures.Controls.Add(CF_PlaceholderCard2);
            tabCustomFeatures.Controls.Add(CF_PlaceholderCard1);
            tabCustomFeatures.Controls.Add(CF_CommandsCard);
            tabCustomFeatures.Controls.Add(CF_FeaturesCard);
            tabCustomFeatures.Location = new System.Drawing.Point(4, 24);
            tabCustomFeatures.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabCustomFeatures.Name = "tabCustomFeatures";
            tabCustomFeatures.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabCustomFeatures.Size = new System.Drawing.Size(750, 470);
            tabCustomFeatures.TabIndex = 2;
            tabCustomFeatures.Text = "Custom Features";
            // 
            // CF_PlaceholderCard2
            // 
            CF_PlaceholderCard2.Controls.Add(CF_Placeholder2Switch);
            CF_PlaceholderCard2.Controls.Add(CF_Placeholder2HeaderLBL);
            CF_PlaceholderCard2.Location = new System.Drawing.Point(20, 623);
            CF_PlaceholderCard2.Margin = new System.Windows.Forms.Padding(16);
            CF_PlaceholderCard2.Name = "CF_PlaceholderCard2";
            CF_PlaceholderCard2.Padding = new System.Windows.Forms.Padding(16);
            CF_PlaceholderCard2.Size = new System.Drawing.Size(822, 127);
            CF_PlaceholderCard2.TabIndex = 3;
            CF_PlaceholderCard2.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            CF_PlaceholderCard2.Visible = false;
            // 
            // CF_Placeholder2Switch
            // 
            CF_Placeholder2Switch.AutoSize = false;
            CF_Placeholder2Switch.Location = new System.Drawing.Point(20, 63);
            CF_Placeholder2Switch.Margin = new System.Windows.Forms.Padding(0);
            CF_Placeholder2Switch.Name = "CF_Placeholder2Switch";
            CF_Placeholder2Switch.Size = new System.Drawing.Size(340, 38);
            CF_Placeholder2Switch.TabIndex = 1;
            CF_Placeholder2Switch.Text = "Enable Aggressive Entity Culling";
            // 
            // CF_Placeholder2HeaderLBL
            // 
            CF_Placeholder2HeaderLBL.AutoSize = true;
            CF_Placeholder2HeaderLBL.BackColor = System.Drawing.Color.Transparent;
            CF_Placeholder2HeaderLBL.Font = Material3.WinForms.Typography.TypeScale.TitleMedium;
            CF_Placeholder2HeaderLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            CF_Placeholder2HeaderLBL.Location = new System.Drawing.Point(20, 16);
            CF_Placeholder2HeaderLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CF_Placeholder2HeaderLBL.Name = "CF_Placeholder2HeaderLBL";
            CF_Placeholder2HeaderLBL.Size = new System.Drawing.Size(210, 24);
            CF_Placeholder2HeaderLBL.TabIndex = 0;
            CF_Placeholder2HeaderLBL.Text = "Server Optimization Module";
            // 
            // CF_PlaceholderCard1
            // 
            CF_PlaceholderCard1.Controls.Add(CF_Placeholder1Switch);
            CF_PlaceholderCard1.Controls.Add(CF_Placeholder1HeaderLBL);
            CF_PlaceholderCard1.Location = new System.Drawing.Point(20, 473);
            CF_PlaceholderCard1.Margin = new System.Windows.Forms.Padding(16);
            CF_PlaceholderCard1.Name = "CF_PlaceholderCard1";
            CF_PlaceholderCard1.Padding = new System.Windows.Forms.Padding(16);
            CF_PlaceholderCard1.Size = new System.Drawing.Size(822, 127);
            CF_PlaceholderCard1.TabIndex = 2;
            CF_PlaceholderCard1.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            CF_PlaceholderCard1.Visible = false;
            // 
            // CF_Placeholder1Switch
            // 
            CF_Placeholder1Switch.AutoSize = false;
            CF_Placeholder1Switch.Location = new System.Drawing.Point(20, 63);
            CF_Placeholder1Switch.Margin = new System.Windows.Forms.Padding(0);
            CF_Placeholder1Switch.Name = "CF_Placeholder1Switch";
            CF_Placeholder1Switch.Size = new System.Drawing.Size(340, 38);
            CF_Placeholder1Switch.TabIndex = 1;
            CF_Placeholder1Switch.Text = "Enable Automated Player Economy";
            // 
            // CF_Placeholder1HeaderLBL
            // 
            CF_Placeholder1HeaderLBL.AutoSize = true;
            CF_Placeholder1HeaderLBL.BackColor = System.Drawing.Color.Transparent;
            CF_Placeholder1HeaderLBL.Font = Material3.WinForms.Typography.TypeScale.TitleMedium;
            CF_Placeholder1HeaderLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            CF_Placeholder1HeaderLBL.Location = new System.Drawing.Point(20, 16);
            CF_Placeholder1HeaderLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CF_Placeholder1HeaderLBL.Name = "CF_Placeholder1HeaderLBL";
            CF_Placeholder1HeaderLBL.Size = new System.Drawing.Size(210, 24);
            CF_Placeholder1HeaderLBL.TabIndex = 0;
            CF_Placeholder1HeaderLBL.Text = "Economy / Trading Module";
            // 
            // CF_CommandsCard
            // 
            CF_CommandsCard.Controls.Add(CF_AddCommandBTN);
            CF_CommandsCard.Controls.Add(CF_CommandsDescLBL);
            CF_CommandsCard.Controls.Add(CF_CommandsHeaderLBL);
            CF_CommandsCard.Location = new System.Drawing.Point(20, 306);
            CF_CommandsCard.Margin = new System.Windows.Forms.Padding(16);
            CF_CommandsCard.Name = "CF_CommandsCard";
            CF_CommandsCard.Padding = new System.Windows.Forms.Padding(16);
            CF_CommandsCard.Size = new System.Drawing.Size(822, 144);
            CF_CommandsCard.TabIndex = 1;
            CF_CommandsCard.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            CF_CommandsCard.Visible = false;
            // 
            // CF_AddCommandBTN
            // 
            CF_AddCommandBTN.AutoSize = false;
            CF_AddCommandBTN.Location = new System.Drawing.Point(20, 87);
            CF_AddCommandBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            CF_AddCommandBTN.Name = "CF_AddCommandBTN";
            CF_AddCommandBTN.Size = new System.Drawing.Size(196, 38);
            CF_AddCommandBTN.TabIndex = 2;
            CF_AddCommandBTN.Text = "BUILD NEW COMMAND";
            CF_AddCommandBTN.Tone = Material3.WinForms.Controls.MaterialTone.Accent;
            CF_AddCommandBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Filled;
            CF_AddCommandBTN.Click += CF_AddCommandBTN_Click;
            // 
            // CF_CommandsDescLBL
            // 
            CF_CommandsDescLBL.AutoSize = true;
            CF_CommandsDescLBL.BackColor = System.Drawing.Color.Transparent;
            CF_CommandsDescLBL.Font = Material3.WinForms.Typography.TypeScale.BodyMedium;
            CF_CommandsDescLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurfaceVariant;
            CF_CommandsDescLBL.Location = new System.Drawing.Point(20, 58);
            CF_CommandsDescLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CF_CommandsDescLBL.Name = "CF_CommandsDescLBL";
            CF_CommandsDescLBL.Size = new System.Drawing.Size(448, 20);
            CF_CommandsDescLBL.TabIndex = 1;
            CF_CommandsDescLBL.Text = "Build tailored commands, responses, and execution logic flows.";
            // 
            // CF_CommandsHeaderLBL
            // 
            CF_CommandsHeaderLBL.AutoSize = true;
            CF_CommandsHeaderLBL.BackColor = System.Drawing.Color.Transparent;
            CF_CommandsHeaderLBL.Font = Material3.WinForms.Typography.TypeScale.TitleMedium;
            CF_CommandsHeaderLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            CF_CommandsHeaderLBL.Location = new System.Drawing.Point(20, 16);
            CF_CommandsHeaderLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CF_CommandsHeaderLBL.Name = "CF_CommandsHeaderLBL";
            CF_CommandsHeaderLBL.Size = new System.Drawing.Size(185, 24);
            CF_CommandsHeaderLBL.TabIndex = 0;
            CF_CommandsHeaderLBL.Text = "Custom Command Logic";
            // 
            // CF_FeaturesCard
            // 
            CF_FeaturesCard.Controls.Add(CF_AntiGriefSwitch);
            CF_FeaturesCard.Controls.Add(CF_PvpAwardsSwitch);
            CF_FeaturesCard.Controls.Add(CF_OpCommandsSwitch);
            CF_FeaturesCard.Controls.Add(CF_HeaderLBL);
            CF_FeaturesCard.Location = new System.Drawing.Point(20, 20);
            CF_FeaturesCard.Margin = new System.Windows.Forms.Padding(16);
            CF_FeaturesCard.Name = "CF_FeaturesCard";
            CF_FeaturesCard.Padding = new System.Windows.Forms.Padding(16);
            CF_FeaturesCard.Size = new System.Drawing.Size(822, 265);
            CF_FeaturesCard.TabIndex = 0;
            CF_FeaturesCard.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            CF_FeaturesCard.Visible = false;
            // 
            // CF_AntiGriefSwitch
            // 
            CF_AntiGriefSwitch.AutoSize = false;
            CF_AntiGriefSwitch.Location = new System.Drawing.Point(20, 185);
            CF_AntiGriefSwitch.Margin = new System.Windows.Forms.Padding(0);
            CF_AntiGriefSwitch.Name = "CF_AntiGriefSwitch";
            CF_AntiGriefSwitch.Size = new System.Drawing.Size(440, 38);
            CF_AntiGriefSwitch.TabIndex = 3;
            CF_AntiGriefSwitch.Text = "Enforce Anti-Griefing && Spawn-Kill Protection";
            // 
            // CF_PvpAwardsSwitch
            // 
            CF_PvpAwardsSwitch.AutoSize = false;
            CF_PvpAwardsSwitch.Location = new System.Drawing.Point(20, 127);
            CF_PvpAwardsSwitch.Margin = new System.Windows.Forms.Padding(0);
            CF_PvpAwardsSwitch.Name = "CF_PvpAwardsSwitch";
            CF_PvpAwardsSwitch.Size = new System.Drawing.Size(400, 38);
            CF_PvpAwardsSwitch.TabIndex = 2;
            CF_PvpAwardsSwitch.Text = "Enable Automated PVP Killstreak Awards";
            // 
            // CF_OpCommandsSwitch
            // 
            CF_OpCommandsSwitch.AutoSize = false;
            CF_OpCommandsSwitch.Location = new System.Drawing.Point(20, 69);
            CF_OpCommandsSwitch.Margin = new System.Windows.Forms.Padding(0);
            CF_OpCommandsSwitch.Name = "CF_OpCommandsSwitch";
            CF_OpCommandsSwitch.Size = new System.Drawing.Size(580, 38);
            CF_OpCommandsSwitch.TabIndex = 1;
            CF_OpCommandsSwitch.Text = "OP Command Interpreter (Reject non-OP execution with generic error)";
            // 
            // CF_HeaderLBL
            // 
            CF_HeaderLBL.AutoSize = true;
            CF_HeaderLBL.BackColor = System.Drawing.Color.Transparent;
            CF_HeaderLBL.Font = Material3.WinForms.Typography.TypeScale.TitleMedium;
            CF_HeaderLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            CF_HeaderLBL.Location = new System.Drawing.Point(20, 16);
            CF_HeaderLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            CF_HeaderLBL.Name = "CF_HeaderLBL";
            CF_HeaderLBL.Size = new System.Drawing.Size(180, 24);
            CF_HeaderLBL.TabIndex = 0;
            CF_HeaderLBL.Text = "Custom Pack Directives";
            // 
            // AddonManager
            // 
            AllowDrop = true;
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(765, 590);
            Controls.Add(PM_TabControl);
            Controls.Add(PM_TabSelector);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AddonManager";
            Padding = new System.Windows.Forms.Padding(3, 0, 4, 3);
            Sizable = false;
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

        private BDSUtil.UI.MaterialTabSelector PM_TabSelector;
        private BDSUtil.UI.MaterialTabControl PM_TabControl;

        private System.Windows.Forms.TabPage tabResourcePacks;
        private System.Windows.Forms.TabPage tabBehaviorPacks;
        private System.Windows.Forms.TabPage tabCustomFeatures;

        private BDSUtil.UI.MaterialListView RP_ListView;
        private System.Windows.Forms.ColumnHeader colRPName;
        private System.Windows.Forms.ColumnHeader colRPVersion;
        private Material3.WinForms.Controls.MaterialButton RP_ImportBTN;
        private Material3.WinForms.Controls.MaterialButton RP_EnableBTN;
        private Material3.WinForms.Controls.MaterialButton RP_DisableBTN;

        private BDSUtil.UI.MaterialListView BP_ListView;
        private System.Windows.Forms.ColumnHeader colBPName;
        private System.Windows.Forms.ColumnHeader colBPVersion;
        private Material3.WinForms.Controls.MaterialButton BP_ImportBTN;
        private Material3.WinForms.Controls.MaterialButton BP_EnableBTN;
        private Material3.WinForms.Controls.MaterialButton BP_DisableBTN;

        private Material3.WinForms.Controls.MaterialCard CF_FeaturesCard;
        private Material3.WinForms.Controls.MaterialLabel CF_HeaderLBL;
        private Material3.WinForms.Controls.MaterialSwitch CF_OpCommandsSwitch;
        private Material3.WinForms.Controls.MaterialSwitch CF_PvpAwardsSwitch;
        private Material3.WinForms.Controls.MaterialSwitch CF_AntiGriefSwitch;

        private Material3.WinForms.Controls.MaterialCard CF_CommandsCard;
        private Material3.WinForms.Controls.MaterialLabel CF_CommandsHeaderLBL;
        private Material3.WinForms.Controls.MaterialLabel CF_CommandsDescLBL;
        private Material3.WinForms.Controls.MaterialButton CF_AddCommandBTN;

        private Material3.WinForms.Controls.MaterialCard CF_PlaceholderCard1;
        private Material3.WinForms.Controls.MaterialLabel CF_Placeholder1HeaderLBL;
        private Material3.WinForms.Controls.MaterialSwitch CF_Placeholder1Switch;

        private Material3.WinForms.Controls.MaterialCard CF_PlaceholderCard2;
        private Material3.WinForms.Controls.MaterialLabel CF_Placeholder2HeaderLBL;
        private Material3.WinForms.Controls.MaterialSwitch CF_Placeholder2Switch;

        private Material3.WinForms.Controls.MaterialButton PM_CloseBTN;
        private Material3.WinForms.Controls.MaterialButton RP_DeleteBTN;
        private Material3.WinForms.Controls.MaterialButton BP_DeleteBTN;
        private Material3.WinForms.Controls.MaterialButton materialButton1;
    }
}