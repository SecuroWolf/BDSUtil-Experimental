namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    partial class NetworkingControl
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
            materialCard1 = new Material3.WinForms.Controls.MaterialCard();
            OpLBL = new Material3.WinForms.Controls.MaterialLabel();
            materialCard2 = new Material3.WinForms.Controls.MaterialCard();
            PortBTNCard = new Material3.WinForms.Controls.MaterialCard();
            RPBTN = new Material3.WinForms.Controls.MaterialButton();
            APBTN = new Material3.WinForms.Controls.MaterialButton();
            materialDivider1 = new Material3.WinForms.Controls.MaterialDivider();
            ManPMCtrls = new Material3.WinForms.Controls.MaterialCard();
            PMList = new BDSUtil.UI.MaterialCheckListBox();
            PMLBL = new Material3.WinForms.Controls.MaterialLabel();
            materialDivider4 = new Material3.WinForms.Controls.MaterialDivider();
            materialCard3 = new Material3.WinForms.Controls.MaterialCard();
            UPnPLBL = new Material3.WinForms.Controls.MaterialLabel();
            materialDivider2 = new Material3.WinForms.Controls.MaterialDivider();
            materialCard1.SuspendLayout();
            materialCard2.SuspendLayout();
            PortBTNCard.SuspendLayout();
            ManPMCtrls.SuspendLayout();
            materialCard3.SuspendLayout();
            SuspendLayout();
            // 
            // materialCard1
            // 
            materialCard1.Controls.Add(OpLBL);
            materialCard1.Location = new System.Drawing.Point(16, 16);
            materialCard1.Margin = new System.Windows.Forms.Padding(16);
            materialCard1.Name = "materialCard1";
            materialCard1.Padding = new System.Windows.Forms.Padding(16);
            materialCard1.Size = new System.Drawing.Size(795, 59);
            materialCard1.TabIndex = 9;
            materialCard1.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            // 
            // OpLBL
            // 
            OpLBL.BackColor = System.Drawing.Color.Transparent;
            OpLBL.Dock = System.Windows.Forms.DockStyle.Fill;
            OpLBL.Font = Material3.WinForms.Typography.TypeScale.TitleMedium;
            OpLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            OpLBL.Location = new System.Drawing.Point(16, 16);
            OpLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            OpLBL.Name = "OpLBL";
            OpLBL.Size = new System.Drawing.Size(763, 27);
            OpLBL.TabIndex = 0;
            OpLBL.Text = "Server Networking";
            OpLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialCard2
            // 
            materialCard2.AutoScroll = true;
            materialCard2.Controls.Add(PortBTNCard);
            materialCard2.Controls.Add(materialDivider1);
            materialCard2.Controls.Add(ManPMCtrls);
            materialCard2.Controls.Add(materialDivider4);
            materialCard2.Controls.Add(materialCard3);
            materialCard2.Controls.Add(materialDivider2);
            materialCard2.Location = new System.Drawing.Point(16, 87);
            materialCard2.Margin = new System.Windows.Forms.Padding(16);
            materialCard2.Name = "materialCard2";
            materialCard2.Padding = new System.Windows.Forms.Padding(16);
            materialCard2.Size = new System.Drawing.Size(795, 590);
            materialCard2.TabIndex = 10;
            materialCard2.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            // 
            // PortBTNCard
            // 
            PortBTNCard.Controls.Add(RPBTN);
            PortBTNCard.Controls.Add(APBTN);
            PortBTNCard.Dock = System.Windows.Forms.DockStyle.Top;
            PortBTNCard.Location = new System.Drawing.Point(16, 509);
            PortBTNCard.Margin = new System.Windows.Forms.Padding(16);
            PortBTNCard.Name = "PortBTNCard";
            PortBTNCard.Padding = new System.Windows.Forms.Padding(0);
            PortBTNCard.Size = new System.Drawing.Size(763, 66);
            PortBTNCard.TabIndex = 9;
            PortBTNCard.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            // 
            // RPBTN
            // 
            RPBTN.AutoSize = false;
            RPBTN.Location = new System.Drawing.Point(328, 14);
            RPBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            RPBTN.Name = "RPBTN";
            RPBTN.Size = new System.Drawing.Size(228, 38);
            RPBTN.TabIndex = 1;
            RPBTN.Text = "Remove Selected Port(s)";
            RPBTN.Tone = Material3.WinForms.Controls.MaterialTone.Error;
            RPBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Tonal;
            // 
            // APBTN
            // 
            APBTN.AutoSize = false;
            APBTN.Location = new System.Drawing.Point(190, 14);
            APBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            APBTN.Name = "APBTN";
            APBTN.Size = new System.Drawing.Size(118, 38);
            APBTN.TabIndex = 0;
            APBTN.Text = "Add Port";
            APBTN.Tone = Material3.WinForms.Controls.MaterialTone.Accent;
            APBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Filled;
            // 
            // materialDivider1
            // 
            materialDivider1.BackColor = System.Drawing.Color.Transparent;
            materialDivider1.Dock = System.Windows.Forms.DockStyle.Top;
            materialDivider1.Location = new System.Drawing.Point(16, 497);
            materialDivider1.Margin = new System.Windows.Forms.Padding(12);
            materialDivider1.Name = "materialDivider1";
            materialDivider1.Size = new System.Drawing.Size(763, 12);
            materialDivider1.TabIndex = 8;
            materialDivider1.TabStop = false;
            // 
            // ManPMCtrls
            // 
            ManPMCtrls.Controls.Add(PMList);
            ManPMCtrls.Controls.Add(PMLBL);
            ManPMCtrls.Dock = System.Windows.Forms.DockStyle.Top;
            ManPMCtrls.Location = new System.Drawing.Point(16, 106);
            ManPMCtrls.Margin = new System.Windows.Forms.Padding(16);
            ManPMCtrls.Name = "ManPMCtrls";
            ManPMCtrls.Padding = new System.Windows.Forms.Padding(6);
            ManPMCtrls.Size = new System.Drawing.Size(763, 391);
            ManPMCtrls.TabIndex = 7;
            ManPMCtrls.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            // 
            // PMList
            // 
            PMList.Dock = System.Windows.Forms.DockStyle.Top;
            PMList.Location = new System.Drawing.Point(6, 31);
            PMList.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PMList.Name = "PMList";
            PMList.Size = new System.Drawing.Size(751, 345);
            PMList.TabIndex = 3;
            // 
            // PMLBL
            // 
            PMLBL.BackColor = System.Drawing.Color.Transparent;
            PMLBL.Dock = System.Windows.Forms.DockStyle.Top;
            PMLBL.Font = Material3.WinForms.Typography.TypeScale.TitleSmall;
            PMLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            PMLBL.Location = new System.Drawing.Point(6, 6);
            PMLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            PMLBL.Name = "PMLBL";
            PMLBL.Size = new System.Drawing.Size(751, 25);
            PMLBL.TabIndex = 2;
            PMLBL.Text = "Port Map";
            PMLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialDivider4
            // 
            materialDivider4.BackColor = System.Drawing.Color.Transparent;
            materialDivider4.Dock = System.Windows.Forms.DockStyle.Top;
            materialDivider4.Location = new System.Drawing.Point(16, 94);
            materialDivider4.Margin = new System.Windows.Forms.Padding(12);
            materialDivider4.Name = "materialDivider4";
            materialDivider4.Size = new System.Drawing.Size(763, 12);
            materialDivider4.TabIndex = 6;
            materialDivider4.TabStop = false;
            // 
            // materialCard3
            // 
            materialCard3.Controls.Add(UPnPLBL);
            materialCard3.Dock = System.Windows.Forms.DockStyle.Top;
            materialCard3.Location = new System.Drawing.Point(16, 28);
            materialCard3.Margin = new System.Windows.Forms.Padding(16);
            materialCard3.Name = "materialCard3";
            materialCard3.Padding = new System.Windows.Forms.Padding(6);
            materialCard3.Size = new System.Drawing.Size(763, 66);
            materialCard3.TabIndex = 5;
            materialCard3.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            // 
            // UPnPLBL
            // 
            UPnPLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            UPnPLBL.BackColor = System.Drawing.Color.Transparent;
            UPnPLBL.Font = Material3.WinForms.Typography.TypeScale.BodyMedium;
            UPnPLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            UPnPLBL.Location = new System.Drawing.Point(10, 6);
            UPnPLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            UPnPLBL.Name = "UPnPLBL";
            UPnPLBL.Size = new System.Drawing.Size(600, 54);
            UPnPLBL.TabIndex = 1;
            UPnPLBL.Text = "UPnP automation is configured in Settings -> Networking";
            UPnPLBL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // materialDivider2
            // 
            materialDivider2.BackColor = System.Drawing.Color.Transparent;
            materialDivider2.Dock = System.Windows.Forms.DockStyle.Top;
            materialDivider2.Location = new System.Drawing.Point(16, 16);
            materialDivider2.Margin = new System.Windows.Forms.Padding(12);
            materialDivider2.Name = "materialDivider2";
            materialDivider2.Size = new System.Drawing.Size(763, 12);
            materialDivider2.TabIndex = 4;
            materialDivider2.TabStop = false;
            // 
            // NetworkingControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            AutoScroll = true;
            BackColor = Material3.WinForms.Theming.MaterialColors.Surface;
            Controls.Add(materialCard1);
            Controls.Add(materialCard2);
            ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximumSize = new System.Drawing.Size(861, 11538);
            MinimumSize = new System.Drawing.Size(861, 58);
            Name = "NetworkingControl";
            Size = new System.Drawing.Size(861, 692);
            Load += NetworkingControl_Load;
            materialCard1.ResumeLayout(false);
            materialCard2.ResumeLayout(false);
            PortBTNCard.ResumeLayout(false);
            ManPMCtrls.ResumeLayout(false);
            materialCard3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Material3.WinForms.Controls.MaterialCard materialCard1;
        private Material3.WinForms.Controls.MaterialLabel OpLBL;
        private Material3.WinForms.Controls.MaterialCard materialCard2;
        private Material3.WinForms.Controls.MaterialCard ManPMCtrls;
        private Material3.WinForms.Controls.MaterialDivider materialDivider4;
        private Material3.WinForms.Controls.MaterialCard materialCard3;
        private Material3.WinForms.Controls.MaterialDivider materialDivider2;
        private Material3.WinForms.Controls.MaterialCard PortBTNCard;
        private Material3.WinForms.Controls.MaterialDivider materialDivider1;
        private Material3.WinForms.Controls.MaterialLabel UPnPLBL;
        private BDSUtil.UI.MaterialCheckListBox PMList;
        private Material3.WinForms.Controls.MaterialLabel PMLBL;
        private Material3.WinForms.Controls.MaterialButton APBTN;
        private Material3.WinForms.Controls.MaterialButton RPBTN;
    }
}
