using Material3.WinForms.Controls;
using Material3.WinForms.Theming;
using Material3.WinForms.Typography;

namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    partial class ServerPropertiesControl
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
            materialCard1 = new MaterialCard();
            PropLBL = new MaterialLabel();
            materialCard2 = new MaterialCard();
            materialDivider5 = new MaterialDivider();
            materialCard5 = new MaterialCard();
            PropAFK = new BDSUtil.UI.MaterialComboBox();
            PropSD = new BDSUtil.UI.MaterialComboBox();
            PropVD = new BDSUtil.UI.MaterialComboBox();
            PropCCG = new MaterialSwitch();
            CCGLBL = new MaterialLabel();
            IdleTOLBL = new MaterialLabel();
            SimDisLBL = new MaterialLabel();
            VDistLBL = new MaterialLabel();
            materialDivider4 = new MaterialDivider();
            materialCard3 = new MaterialCard();
            PropCastLAN = new MaterialSwitch();
            LanLBL = new MaterialLabel();
            PropPortV4 = new BDSUtil.UI.MaterialTextBoxEdit();
            PortV4LBL = new MaterialLabel();
            materialDivider2 = new MaterialDivider();
            materialCard4 = new MaterialCard();
            PropReqPacks = new MaterialSwitch();
            ReqPacksLBL = new MaterialLabel();
            PropWList = new MaterialSwitch();
            WListLBL = new MaterialLabel();
            PropOMode = new MaterialSwitch();
            PropAlCh = new MaterialSwitch();
            PropFGM = new MaterialSwitch();
            OModeLBL = new MaterialLabel();
            AlChLBL = new MaterialLabel();
            FGmLBL = new MaterialLabel();
            PropMP = new BDSUtil.UI.MaterialTextBoxEdit();
            PropDiff = new BDSUtil.UI.MaterialComboBox();
            PropGM = new BDSUtil.UI.MaterialComboBox();
            MaxPlayerLBL = new MaterialLabel();
            DiffLBL = new MaterialLabel();
            GamemodeLBL = new MaterialLabel();
            materialDivider3 = new MaterialDivider();
            CoreProps = new MaterialCard();
            materialDivider1 = new MaterialDivider();
            PropSvSeed = new BDSUtil.UI.MaterialTextBoxEdit();
            PropWorldSel = new BDSUtil.UI.MaterialComboBox();
            SeedLBL = new MaterialLabel();
            WorldLBL = new MaterialLabel();
            PropSvName = new BDSUtil.UI.MaterialTextBoxEdit();
            SvNameLBL = new MaterialLabel();
            materialCard1.SuspendLayout();
            materialCard2.SuspendLayout();
            materialCard5.SuspendLayout();
            materialCard3.SuspendLayout();
            materialCard4.SuspendLayout();
            CoreProps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(PropAFK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(PropSD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(PropVD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(PropDiff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(PropGM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(PropWorldSel)).BeginInit();
            SuspendLayout();
            // 
            // materialCard1
            // 
            materialCard1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialCard1.Controls.Add(PropLBL);
            materialCard1.Location = new System.Drawing.Point(16, 16);
            materialCard1.Margin = new System.Windows.Forms.Padding(16);
            materialCard1.Name = "materialCard1";
            materialCard1.Padding = new System.Windows.Forms.Padding(16);
            materialCard1.Size = new System.Drawing.Size(795, 59);
            materialCard1.TabIndex = 3;
            materialCard1.Variant = MaterialCardVariant.Elevated;
            // 
            // PropLBL
            // 
            PropLBL.AutoSize = false;
            PropLBL.BackColor = System.Drawing.Color.Transparent;
            PropLBL.Dock = System.Windows.Forms.DockStyle.Fill;
            PropLBL.Font = TypeScale.TitleMedium;
            PropLBL.ForeColor = MaterialColors.OnSurface;
            PropLBL.Location = new System.Drawing.Point(16, 16);
            PropLBL.Name = "PropLBL";
            PropLBL.Size = new System.Drawing.Size(763, 27);
            PropLBL.TabIndex = 0;
            PropLBL.Text = "Server Properties";
            PropLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialCard2
            // 
            materialCard2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            materialCard2.AutoScroll = true;
            materialCard2.Controls.Add(materialDivider5);
            materialCard2.Controls.Add(materialCard5);
            materialCard2.Controls.Add(materialDivider4);
            materialCard2.Controls.Add(materialCard3);
            materialCard2.Controls.Add(materialDivider2);
            materialCard2.Controls.Add(materialCard4);
            materialCard2.Controls.Add(materialDivider3);
            materialCard2.Controls.Add(CoreProps);
            materialCard2.Location = new System.Drawing.Point(16, 87);
            materialCard2.Margin = new System.Windows.Forms.Padding(16);
            materialCard2.Name = "materialCard2";
            materialCard2.Padding = new System.Windows.Forms.Padding(16);
            materialCard2.Size = new System.Drawing.Size(795, 590);
            materialCard2.TabIndex = 4;
            materialCard2.Variant = MaterialCardVariant.Elevated;
            // 
            // materialDivider5
            // 
            materialDivider5.Dock = System.Windows.Forms.DockStyle.Top;
            materialDivider5.Location = new System.Drawing.Point(16, 1270);
            materialDivider5.Name = "materialDivider5";
            materialDivider5.Size = new System.Drawing.Size(746, 12);
            materialDivider5.TabIndex = 7;
            // 
            // materialCard5
            // 
            materialCard5.Controls.Add(PropAFK);
            materialCard5.Controls.Add(PropSD);
            materialCard5.Controls.Add(PropVD);
            materialCard5.Controls.Add(PropCCG);
            materialCard5.Controls.Add(CCGLBL);
            materialCard5.Controls.Add(IdleTOLBL);
            materialCard5.Controls.Add(SimDisLBL);
            materialCard5.Controls.Add(VDistLBL);
            materialCard5.Dock = System.Windows.Forms.DockStyle.Top;
            materialCard5.Location = new System.Drawing.Point(16, 1036);
            materialCard5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialCard5.Name = "materialCard5";
            materialCard5.Padding = new System.Windows.Forms.Padding(6);
            materialCard5.Size = new System.Drawing.Size(746, 234);
            materialCard5.TabIndex = 5;
            materialCard5.Variant = MaterialCardVariant.Outlined;
            // 
            // PropAFK
            // 
            PropAFK.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropAFK.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            PropAFK.Hint = "AFK Kick";
            PropAFK.Items.AddRange(new object[] {
                "Off",
                "10 Mins",
                "20 Mins",
                "30 Mins",
                "40 Mins",
                "50 Mins",
                "1 Hour"
            });
            PropAFK.Location = new System.Drawing.Point(398, 118);
            PropAFK.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropAFK.Name = "PropAFK";
            PropAFK.SelectedIndex = 0;
            PropAFK.Size = new System.Drawing.Size(332, 46);
            PropAFK.TabIndex = 6;
            // 
            // PropSD
            // 
            PropSD.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropSD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            PropSD.Hint = "Simulation Distance";
            PropSD.Items.AddRange(new object[] {
                "4 Chunks",
                "5 Chunks",
                "6 Chunks",
                "7 Chunks",
                "8 Chunks",
                "9 Chunks",
                "10 Chunks",
                "11 Chunks",
                "12 Chunks"
            });
            PropSD.Location = new System.Drawing.Point(398, 62);
            PropSD.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropSD.Name = "PropSD";
            PropSD.SelectedIndex = 0;
            PropSD.Size = new System.Drawing.Size(332, 46);
            PropSD.TabIndex = 5;
            // 
            // PropVD
            // 
            PropVD.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropVD.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            PropVD.Hint = "View Distance";
            PropVD.Items.AddRange(new object[] {
                "5 Chunks",
                "10 Chunks",
                "15 Chunks",
                "20 Chunks",
                "25 Chunks",
                "30 Chunks",
                "35 Chunks",
                "40 Chunks",
                "45 Chunks",
                "50 Chunks",
                "Custom"
            });
            PropVD.Location = new System.Drawing.Point(398, 5);
            PropVD.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropVD.Name = "PropVD";
            PropVD.SelectedIndex = 0;
            PropVD.Size = new System.Drawing.Size(332, 46);
            PropVD.TabIndex = 4;
            PropVD.SelectedIndexChanged += PropVD_SelectedIndexChanged;
            // 
            // PropCCG
            // 
            PropCCG.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropCCG.AutoSize = false;
            PropCCG.Location = new System.Drawing.Point(664, 180);
            PropCCG.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropCCG.Name = "PropCCG";
            PropCCG.Size = new System.Drawing.Size(64, 38);
            PropCCG.TabIndex = 8;
            PropCCG.Text = "";
            // 
            // CCGLBL
            // 
            CCGLBL.AutoSize = false;
            CCGLBL.BackColor = System.Drawing.Color.Transparent;
            CCGLBL.Dock = System.Windows.Forms.DockStyle.Top;
            CCGLBL.Font = TypeScale.BodyLarge;
            CCGLBL.ForeColor = MaterialColors.OnSurface;
            CCGLBL.Location = new System.Drawing.Point(6, 171);
            CCGLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CCGLBL.Name = "CCGLBL";
            CCGLBL.Size = new System.Drawing.Size(734, 55);
            CCGLBL.TabIndex = 3;
            CCGLBL.Text = "Client Chunk Generation";
            CCGLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // IdleTOLBL
            // 
            IdleTOLBL.AutoSize = false;
            IdleTOLBL.BackColor = System.Drawing.Color.Transparent;
            IdleTOLBL.Dock = System.Windows.Forms.DockStyle.Top;
            IdleTOLBL.Font = TypeScale.BodyLarge;
            IdleTOLBL.ForeColor = MaterialColors.OnSurface;
            IdleTOLBL.Location = new System.Drawing.Point(6, 116);
            IdleTOLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            IdleTOLBL.Name = "IdleTOLBL";
            IdleTOLBL.Size = new System.Drawing.Size(734, 55);
            IdleTOLBL.TabIndex = 2;
            IdleTOLBL.Text = "AFK Kick";
            IdleTOLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SimDisLBL
            // 
            SimDisLBL.AutoSize = false;
            SimDisLBL.BackColor = System.Drawing.Color.Transparent;
            SimDisLBL.Dock = System.Windows.Forms.DockStyle.Top;
            SimDisLBL.Font = TypeScale.BodyLarge;
            SimDisLBL.ForeColor = MaterialColors.OnSurface;
            SimDisLBL.Location = new System.Drawing.Point(6, 61);
            SimDisLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SimDisLBL.Name = "SimDisLBL";
            SimDisLBL.Size = new System.Drawing.Size(734, 55);
            SimDisLBL.TabIndex = 1;
            SimDisLBL.Text = "Simulation Distance";
            SimDisLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VDistLBL
            // 
            VDistLBL.AutoSize = false;
            VDistLBL.BackColor = System.Drawing.Color.Transparent;
            VDistLBL.Dock = System.Windows.Forms.DockStyle.Top;
            VDistLBL.Font = TypeScale.BodyLarge;
            VDistLBL.ForeColor = MaterialColors.OnSurface;
            VDistLBL.Location = new System.Drawing.Point(6, 6);
            VDistLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            VDistLBL.Name = "VDistLBL";
            VDistLBL.Size = new System.Drawing.Size(734, 55);
            VDistLBL.TabIndex = 0;
            VDistLBL.Text = "View Distance";
            VDistLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialDivider4
            // 
            materialDivider4.Dock = System.Windows.Forms.DockStyle.Top;
            materialDivider4.Location = new System.Drawing.Point(16, 1024);
            materialDivider4.Name = "materialDivider4";
            materialDivider4.Size = new System.Drawing.Size(746, 12);
            materialDivider4.TabIndex = 6;
            // 
            // materialCard3
            // 
            materialCard3.Controls.Add(PropCastLAN);
            materialCard3.Controls.Add(LanLBL);
            materialCard3.Controls.Add(PropPortV4);
            materialCard3.Controls.Add(PortV4LBL);
            materialCard3.Dock = System.Windows.Forms.DockStyle.Top;
            materialCard3.Location = new System.Drawing.Point(16, 901);
            materialCard3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialCard3.Name = "materialCard3";
            materialCard3.Padding = new System.Windows.Forms.Padding(6);
            materialCard3.Size = new System.Drawing.Size(746, 123);
            materialCard3.TabIndex = 3;
            materialCard3.Variant = MaterialCardVariant.Outlined;
            // 
            // PropCastLAN
            // 
            PropCastLAN.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropCastLAN.AutoSize = false;
            PropCastLAN.Location = new System.Drawing.Point(664, 70);
            PropCastLAN.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropCastLAN.Name = "PropCastLAN";
            PropCastLAN.Size = new System.Drawing.Size(64, 38);
            PropCastLAN.TabIndex = 7;
            PropCastLAN.Text = "";
            // 
            // LanLBL
            // 
            LanLBL.AutoSize = false;
            LanLBL.BackColor = System.Drawing.Color.Transparent;
            LanLBL.Dock = System.Windows.Forms.DockStyle.Top;
            LanLBL.Font = TypeScale.BodyLarge;
            LanLBL.ForeColor = MaterialColors.OnSurface;
            LanLBL.Location = new System.Drawing.Point(6, 61);
            LanLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            LanLBL.Name = "LanLBL";
            LanLBL.Size = new System.Drawing.Size(734, 55);
            LanLBL.TabIndex = 6;
            LanLBL.Text = "Broadcast to LAN";
            LanLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PropPortV4
            // 
            PropPortV4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropPortV4.Hint = "Server Port";
            PropPortV4.Location = new System.Drawing.Point(398, 5);
            PropPortV4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropPortV4.MaxLength = 5;
            PropPortV4.Name = "PropPortV4";
            PropPortV4.Size = new System.Drawing.Size(332, 46);
            PropPortV4.TabIndex = 5;
            // 
            // PortV4LBL
            // 
            PortV4LBL.AutoSize = false;
            PortV4LBL.BackColor = System.Drawing.Color.Transparent;
            PortV4LBL.Dock = System.Windows.Forms.DockStyle.Top;
            PortV4LBL.Font = TypeScale.BodyLarge;
            PortV4LBL.ForeColor = MaterialColors.OnSurface;
            PortV4LBL.Location = new System.Drawing.Point(6, 6);
            PortV4LBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PortV4LBL.Name = "PortV4LBL";
            PortV4LBL.Size = new System.Drawing.Size(734, 55);
            PortV4LBL.TabIndex = 0;
            PortV4LBL.Text = "Server Port";
            PortV4LBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialDivider2
            // 
            materialDivider2.Dock = System.Windows.Forms.DockStyle.Top;
            materialDivider2.Location = new System.Drawing.Point(16, 889);
            materialDivider2.Name = "materialDivider2";
            materialDivider2.Size = new System.Drawing.Size(746, 12);
            materialDivider2.TabIndex = 4;
            // 
            // materialCard4
            // 
            materialCard4.Controls.Add(PropReqPacks);
            materialCard4.Controls.Add(ReqPacksLBL);
            materialCard4.Controls.Add(PropWList);
            materialCard4.Controls.Add(WListLBL);
            materialCard4.Controls.Add(PropOMode);
            materialCard4.Controls.Add(PropAlCh);
            materialCard4.Controls.Add(PropFGM);
            materialCard4.Controls.Add(OModeLBL);
            materialCard4.Controls.Add(AlChLBL);
            materialCard4.Controls.Add(FGmLBL);
            materialCard4.Controls.Add(PropMP);
            materialCard4.Controls.Add(PropDiff);
            materialCard4.Controls.Add(PropGM);
            materialCard4.Controls.Add(MaxPlayerLBL);
            materialCard4.Controls.Add(DiffLBL);
            materialCard4.Controls.Add(GamemodeLBL);
            materialCard4.Dock = System.Windows.Forms.DockStyle.Top;
            materialCard4.Location = new System.Drawing.Point(16, 425);
            materialCard4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialCard4.Name = "materialCard4";
            materialCard4.Padding = new System.Windows.Forms.Padding(6);
            materialCard4.Size = new System.Drawing.Size(746, 464);
            materialCard4.TabIndex = 2;
            materialCard4.Variant = MaterialCardVariant.Outlined;
            // 
            // PropReqPacks
            // 
            PropReqPacks.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropReqPacks.AutoSize = false;
            PropReqPacks.Location = new System.Drawing.Point(664, 401);
            PropReqPacks.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropReqPacks.Name = "PropReqPacks";
            PropReqPacks.Size = new System.Drawing.Size(64, 38);
            PropReqPacks.TabIndex = 15;
            PropReqPacks.Text = "";
            // 
            // ReqPacksLBL
            // 
            ReqPacksLBL.AutoSize = false;
            ReqPacksLBL.BackColor = System.Drawing.Color.Transparent;
            ReqPacksLBL.Dock = System.Windows.Forms.DockStyle.Top;
            ReqPacksLBL.Font = TypeScale.BodyLarge;
            ReqPacksLBL.ForeColor = MaterialColors.OnSurface;
            ReqPacksLBL.Location = new System.Drawing.Point(6, 391);
            ReqPacksLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ReqPacksLBL.Name = "ReqPacksLBL";
            ReqPacksLBL.Size = new System.Drawing.Size(734, 55);
            ReqPacksLBL.TabIndex = 14;
            ReqPacksLBL.Text = "Require Texture/Behavior Packs";
            ReqPacksLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PropWList
            // 
            PropWList.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropWList.AutoSize = false;
            PropWList.Location = new System.Drawing.Point(664, 346);
            PropWList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropWList.Name = "PropWList";
            PropWList.Size = new System.Drawing.Size(64, 38);
            PropWList.TabIndex = 13;
            PropWList.Text = "";
            // 
            // WListLBL
            // 
            WListLBL.AutoSize = false;
            WListLBL.BackColor = System.Drawing.Color.Transparent;
            WListLBL.Dock = System.Windows.Forms.DockStyle.Top;
            WListLBL.Font = TypeScale.BodyLarge;
            WListLBL.ForeColor = MaterialColors.OnSurface;
            WListLBL.Location = new System.Drawing.Point(6, 336);
            WListLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            WListLBL.Name = "WListLBL";
            WListLBL.Size = new System.Drawing.Size(734, 55);
            WListLBL.TabIndex = 12;
            WListLBL.Text = "Enable Whitelist";
            WListLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PropOMode
            // 
            PropOMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropOMode.AutoSize = false;
            PropOMode.Location = new System.Drawing.Point(664, 291);
            PropOMode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropOMode.Name = "PropOMode";
            PropOMode.Size = new System.Drawing.Size(64, 38);
            PropOMode.TabIndex = 11;
            PropOMode.Text = "";
            // 
            // PropAlCh
            // 
            PropAlCh.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropAlCh.AutoSize = false;
            PropAlCh.Location = new System.Drawing.Point(664, 235);
            PropAlCh.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropAlCh.Name = "PropAlCh";
            PropAlCh.Size = new System.Drawing.Size(64, 38);
            PropAlCh.TabIndex = 10;
            PropAlCh.Text = "";
            // 
            // PropFGM
            // 
            PropFGM.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropFGM.AutoSize = false;
            PropFGM.Location = new System.Drawing.Point(664, 180);
            PropFGM.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropFGM.Name = "PropFGM";
            PropFGM.Size = new System.Drawing.Size(64, 38);
            PropFGM.TabIndex = 9;
            PropFGM.Text = "";
            // 
            // OModeLBL
            // 
            OModeLBL.AutoSize = false;
            OModeLBL.BackColor = System.Drawing.Color.Transparent;
            OModeLBL.Dock = System.Windows.Forms.DockStyle.Top;
            OModeLBL.Font = TypeScale.BodyLarge;
            OModeLBL.ForeColor = MaterialColors.OnSurface;
            OModeLBL.Location = new System.Drawing.Point(6, 281);
            OModeLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            OModeLBL.Name = "OModeLBL";
            OModeLBL.Size = new System.Drawing.Size(734, 55);
            OModeLBL.TabIndex = 8;
            OModeLBL.Text = "Online Mode";
            OModeLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AlChLBL
            // 
            AlChLBL.AutoSize = false;
            AlChLBL.BackColor = System.Drawing.Color.Transparent;
            AlChLBL.Dock = System.Windows.Forms.DockStyle.Top;
            AlChLBL.Font = TypeScale.BodyLarge;
            AlChLBL.ForeColor = MaterialColors.OnSurface;
            AlChLBL.Location = new System.Drawing.Point(6, 226);
            AlChLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AlChLBL.Name = "AlChLBL";
            AlChLBL.Size = new System.Drawing.Size(734, 55);
            AlChLBL.TabIndex = 7;
            AlChLBL.Text = "Allow Cheats";
            AlChLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FGmLBL
            // 
            FGmLBL.AutoSize = false;
            FGmLBL.BackColor = System.Drawing.Color.Transparent;
            FGmLBL.Dock = System.Windows.Forms.DockStyle.Top;
            FGmLBL.Font = TypeScale.BodyLarge;
            FGmLBL.ForeColor = MaterialColors.OnSurface;
            FGmLBL.Location = new System.Drawing.Point(6, 171);
            FGmLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FGmLBL.Name = "FGmLBL";
            FGmLBL.Size = new System.Drawing.Size(734, 55);
            FGmLBL.TabIndex = 6;
            FGmLBL.Text = "Force Gamemode";
            FGmLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PropMP
            // 
            PropMP.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropMP.Hint = "Numbers only(1-100)";
            PropMP.Location = new System.Drawing.Point(398, 118);
            PropMP.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropMP.MaxLength = 3;
            PropMP.Name = "PropMP";
            PropMP.Size = new System.Drawing.Size(332, 46);
            PropMP.TabIndex = 5;
            // 
            // PropDiff
            // 
            PropDiff.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropDiff.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            PropDiff.Hint = "Difficulty";
            PropDiff.Items.AddRange(new object[] {
                "Peaceful",
                "Easy",
                "Normal",
                "Hard"
            });
            PropDiff.Location = new System.Drawing.Point(398, 62);
            PropDiff.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropDiff.Name = "PropDiff";
            PropDiff.SelectedIndex = 0;
            PropDiff.Size = new System.Drawing.Size(332, 46);
            PropDiff.TabIndex = 4;
            // 
            // PropGM
            // 
            PropGM.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropGM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            PropGM.Hint = "Default Gamemode";
            PropGM.Items.AddRange(new object[] {
                "Survival",
                "Creative",
                "Adventure",
                "Spectator"
            });
            PropGM.Location = new System.Drawing.Point(398, 5);
            PropGM.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropGM.Name = "PropGM";
            PropGM.SelectedIndex = 0;
            PropGM.Size = new System.Drawing.Size(332, 46);
            PropGM.TabIndex = 3;
            // 
            // MaxPlayerLBL
            // 
            MaxPlayerLBL.AutoSize = false;
            MaxPlayerLBL.BackColor = System.Drawing.Color.Transparent;
            MaxPlayerLBL.Dock = System.Windows.Forms.DockStyle.Top;
            MaxPlayerLBL.Font = TypeScale.BodyLarge;
            MaxPlayerLBL.ForeColor = MaterialColors.OnSurface;
            MaxPlayerLBL.Location = new System.Drawing.Point(6, 116);
            MaxPlayerLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaxPlayerLBL.Name = "MaxPlayerLBL";
            MaxPlayerLBL.Size = new System.Drawing.Size(734, 55);
            MaxPlayerLBL.TabIndex = 2;
            MaxPlayerLBL.Text = "Max Players";
            MaxPlayerLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DiffLBL
            // 
            DiffLBL.AutoSize = false;
            DiffLBL.BackColor = System.Drawing.Color.Transparent;
            DiffLBL.Dock = System.Windows.Forms.DockStyle.Top;
            DiffLBL.Font = TypeScale.BodyLarge;
            DiffLBL.ForeColor = MaterialColors.OnSurface;
            DiffLBL.Location = new System.Drawing.Point(6, 61);
            DiffLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DiffLBL.Name = "DiffLBL";
            DiffLBL.Size = new System.Drawing.Size(734, 55);
            DiffLBL.TabIndex = 1;
            DiffLBL.Text = "Difficulty";
            DiffLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // GamemodeLBL
            // 
            GamemodeLBL.AutoSize = false;
            GamemodeLBL.BackColor = System.Drawing.Color.Transparent;
            GamemodeLBL.Dock = System.Windows.Forms.DockStyle.Top;
            GamemodeLBL.Font = TypeScale.BodyLarge;
            GamemodeLBL.ForeColor = MaterialColors.OnSurface;
            GamemodeLBL.Location = new System.Drawing.Point(6, 6);
            GamemodeLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GamemodeLBL.Name = "GamemodeLBL";
            GamemodeLBL.Size = new System.Drawing.Size(734, 55);
            GamemodeLBL.TabIndex = 0;
            GamemodeLBL.Text = "Default Gamemode";
            GamemodeLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialDivider3
            // 
            materialDivider3.Dock = System.Windows.Forms.DockStyle.Top;
            materialDivider3.Location = new System.Drawing.Point(16, 413);
            materialDivider3.Name = "materialDivider3";
            materialDivider3.Size = new System.Drawing.Size(746, 12);
            materialDivider3.TabIndex = 3;
            // 
            // CoreProps
            // 
            CoreProps.Controls.Add(materialDivider1);
            CoreProps.Controls.Add(PropSvSeed);
            CoreProps.Controls.Add(PropWorldSel);
            CoreProps.Controls.Add(SeedLBL);
            CoreProps.Controls.Add(WorldLBL);
            CoreProps.Controls.Add(PropSvName);
            CoreProps.Controls.Add(SvNameLBL);
            CoreProps.Dock = System.Windows.Forms.DockStyle.Top;
            CoreProps.Location = new System.Drawing.Point(16, 16);
            CoreProps.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CoreProps.Name = "CoreProps";
            CoreProps.Padding = new System.Windows.Forms.Padding(6);
            CoreProps.Size = new System.Drawing.Size(746, 179);
            CoreProps.TabIndex = 1;
            CoreProps.Variant = MaterialCardVariant.Outlined;
            // 
            // materialDivider1
            // 
            materialDivider1.Dock = System.Windows.Forms.DockStyle.Top;
            materialDivider1.Location = new System.Drawing.Point(6, 171);
            materialDivider1.Name = "materialDivider1";
            materialDivider1.Size = new System.Drawing.Size(734, 2);
            materialDivider1.TabIndex = 6;
            // 
            // PropSvSeed
            // 
            PropSvSeed.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropSvSeed.Hint = "Server Seed";
            PropSvSeed.Location = new System.Drawing.Point(398, 118);
            PropSvSeed.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropSvSeed.Name = "PropSvSeed";
            PropSvSeed.ReadOnly = true;
            PropSvSeed.Size = new System.Drawing.Size(332, 46);
            PropSvSeed.TabIndex = 5;
            // 
            // PropWorldSel
            // 
            PropWorldSel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropWorldSel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            PropWorldSel.Hint = "World";
            PropWorldSel.Location = new System.Drawing.Point(398, 62);
            PropWorldSel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropWorldSel.Name = "PropWorldSel";
            PropWorldSel.Size = new System.Drawing.Size(332, 46);
            PropWorldSel.TabIndex = 4;
            PropWorldSel.SelectedIndexChanged += PropWorldSel_SelectedIndexChanged;
            // 
            // SeedLBL
            // 
            SeedLBL.AutoSize = false;
            SeedLBL.BackColor = System.Drawing.Color.Transparent;
            SeedLBL.Dock = System.Windows.Forms.DockStyle.Top;
            SeedLBL.Font = TypeScale.BodyLarge;
            SeedLBL.ForeColor = MaterialColors.OnSurface;
            SeedLBL.Location = new System.Drawing.Point(6, 116);
            SeedLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SeedLBL.Name = "SeedLBL";
            SeedLBL.Size = new System.Drawing.Size(734, 55);
            SeedLBL.TabIndex = 2;
            SeedLBL.Text = "Seed";
            SeedLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WorldLBL
            // 
            WorldLBL.AutoSize = false;
            WorldLBL.BackColor = System.Drawing.Color.Transparent;
            WorldLBL.Dock = System.Windows.Forms.DockStyle.Top;
            WorldLBL.Font = TypeScale.BodyLarge;
            WorldLBL.ForeColor = MaterialColors.OnSurface;
            WorldLBL.Location = new System.Drawing.Point(6, 61);
            WorldLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            WorldLBL.Name = "WorldLBL";
            WorldLBL.Size = new System.Drawing.Size(734, 55);
            WorldLBL.TabIndex = 1;
            WorldLBL.Text = "World";
            WorldLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PropSvName
            // 
            PropSvName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PropSvName.Hint = "Server Name";
            PropSvName.Location = new System.Drawing.Point(398, 5);
            PropSvName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PropSvName.Name = "PropSvName";
            PropSvName.Size = new System.Drawing.Size(332, 46);
            PropSvName.TabIndex = 3;
            PropSvName.KeyPress += PropSvName_KeyPress;
            // 
            // SvNameLBL
            // 
            SvNameLBL.AutoSize = false;
            SvNameLBL.BackColor = System.Drawing.Color.Transparent;
            SvNameLBL.Dock = System.Windows.Forms.DockStyle.Top;
            SvNameLBL.Font = TypeScale.BodyLarge;
            SvNameLBL.ForeColor = MaterialColors.OnSurface;
            SvNameLBL.Location = new System.Drawing.Point(6, 6);
            SvNameLBL.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SvNameLBL.Name = "SvNameLBL";
            SvNameLBL.Size = new System.Drawing.Size(734, 55);
            SvNameLBL.TabIndex = 0;
            SvNameLBL.Text = "Server Name";
            SvNameLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ServerPropertiesControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = MaterialColors.Surface;
            Controls.Add(materialCard2);
            Controls.Add(materialCard1);
            ForeColor = MaterialColors.OnSurface;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximumSize = new System.Drawing.Size(861, 11538);
            Name = "ServerPropertiesControl";
            Size = new System.Drawing.Size(861, 692);
            Load += ServerPropertiesControl_Load;
            materialCard1.ResumeLayout(false);
            materialCard2.ResumeLayout(false);
            materialCard5.ResumeLayout(false);
            materialCard3.ResumeLayout(false);
            materialCard4.ResumeLayout(false);
            CoreProps.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(PropAFK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(PropSD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(PropVD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(PropDiff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(PropGM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(PropWorldSel)).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private MaterialCard materialCard1;
        private MaterialLabel PropLBL;
        private MaterialCard materialCard2;
        private MaterialCard CoreProps;
        private MaterialLabel SvNameLBL;
        public BDSUtil.UI.MaterialTextBoxEdit PropSvName;
        private MaterialLabel SeedLBL;
        private MaterialLabel WorldLBL;
        public BDSUtil.UI.MaterialComboBox PropWorldSel;
        public BDSUtil.UI.MaterialTextBoxEdit PropSvSeed;
        private MaterialDivider materialDivider1;
        private MaterialDivider materialDivider2;
        private MaterialCard materialCard4;
        public BDSUtil.UI.MaterialTextBoxEdit PropMP;
        public BDSUtil.UI.MaterialComboBox PropDiff;
        public BDSUtil.UI.MaterialComboBox PropGM;
        private MaterialLabel MaxPlayerLBL;
        private MaterialLabel DiffLBL;
        private MaterialLabel GamemodeLBL;
        private MaterialDivider materialDivider3;
        public MaterialSwitch PropWList;
        private MaterialLabel WListLBL;
        public MaterialSwitch PropOMode;
        public MaterialSwitch PropAlCh;
        public MaterialSwitch PropFGM;
        private MaterialLabel OModeLBL;
        private MaterialLabel AlChLBL;
        private MaterialLabel FGmLBL;
        private MaterialCard materialCard3;
        public MaterialSwitch PropCastLAN;
        private MaterialLabel LanLBL;
        public BDSUtil.UI.MaterialTextBoxEdit PropPortV4;
        private MaterialLabel PortV4LBL;
        private MaterialDivider materialDivider4;
        private MaterialCard materialCard5;
        public BDSUtil.UI.MaterialComboBox PropAFK;
        public BDSUtil.UI.MaterialComboBox PropSD;
        public BDSUtil.UI.MaterialComboBox PropVD;
        private MaterialLabel IdleTOLBL;
        private MaterialLabel SimDisLBL;
        private MaterialLabel VDistLBL;
        private MaterialDivider materialDivider5;
        public MaterialSwitch PropCCG;
        private MaterialLabel CCGLBL;
        public MaterialSwitch PropReqPacks;
        private MaterialLabel ReqPacksLBL;
    }
}
