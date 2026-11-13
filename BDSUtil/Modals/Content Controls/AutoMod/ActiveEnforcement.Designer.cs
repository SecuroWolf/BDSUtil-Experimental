namespace BDSUtil.Modals.Content_Controls.AutoMod
{
    partial class ActiveEnforcement
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
            this.AM_EA_Title = new Material3.WinForms.Controls.MaterialCard();
            this.FilterLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.materialCard2 = new Material3.WinForms.Controls.MaterialCard();
            this.materialCard5 = new Material3.WinForms.Controls.MaterialCard();
            this.AM_RAE_BTN = new Material3.WinForms.Controls.MaterialButton();
            this.AM_CS_BTN = new Material3.WinForms.Controls.MaterialButton();
            this.AM_OEP_BTN = new Material3.WinForms.Controls.MaterialButton();
            this.materialDivider4 = new Material3.WinForms.Controls.MaterialDivider();
            this.materialCard3 = new Material3.WinForms.Controls.MaterialCard();
            this.AM_EnforcementHistory = new BDSUtil.UI.MaterialCheckListBox();
            this.materialDivider2 = new Material3.WinForms.Controls.MaterialDivider();
            this.AM_EA_Title.SuspendLayout();
            this.materialCard2.SuspendLayout();
            this.materialCard5.SuspendLayout();
            this.materialCard3.SuspendLayout();
            this.SuspendLayout();
            // 
            // AM_EA_Title
            // 
            this.AM_EA_Title.Controls.Add(this.FilterLBL);
            this.AM_EA_Title.Location = new System.Drawing.Point(24, 18);
            this.AM_EA_Title.Margin = new System.Windows.Forms.Padding(14);
            this.AM_EA_Title.Name = "AM_EA_Title";
            this.AM_EA_Title.Padding = new System.Windows.Forms.Padding(14);
            this.AM_EA_Title.Size = new System.Drawing.Size(690, 51);
            this.AM_EA_Title.TabIndex = 9;
            this.AM_EA_Title.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            // 
            // FilterLBL
            // 
            this.FilterLBL.BackColor = System.Drawing.Color.Transparent;
            this.FilterLBL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilterLBL.Font = Material3.WinForms.Typography.TypeScale.TitleMedium;
            this.FilterLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.FilterLBL.Location = new System.Drawing.Point(14, 14);
            this.FilterLBL.Name = "FilterLBL";
            this.FilterLBL.Size = new System.Drawing.Size(662, 23);
            this.FilterLBL.TabIndex = 0;
            this.FilterLBL.Text = "Enforcement Actions";
            this.FilterLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialCard2
            // 
            this.materialCard2.Controls.Add(this.materialCard5);
            this.materialCard2.Controls.Add(this.materialDivider4);
            this.materialCard2.Controls.Add(this.materialCard3);
            this.materialCard2.Controls.Add(this.materialDivider2);
            this.materialCard2.Location = new System.Drawing.Point(24, 79);
            this.materialCard2.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard2.Name = "materialCard2";
            this.materialCard2.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard2.Size = new System.Drawing.Size(690, 504);
            this.materialCard2.TabIndex = 10;
            this.materialCard2.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            // 
            // materialCard5
            // 
            this.materialCard5.Controls.Add(this.AM_RAE_BTN);
            this.materialCard5.Controls.Add(this.AM_CS_BTN);
            this.materialCard5.Controls.Add(this.AM_OEP_BTN);
            this.materialCard5.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialCard5.Location = new System.Drawing.Point(14, 426);
            this.materialCard5.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard5.Name = "materialCard5";
            this.materialCard5.Padding = new System.Windows.Forms.Padding(0);
            this.materialCard5.Size = new System.Drawing.Size(662, 65);
            this.materialCard5.TabIndex = 7;
            this.materialCard5.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            // 
            // AM_RAE_BTN
            // 
            this.AM_RAE_BTN.AutoSize = false;
            this.AM_RAE_BTN.Location = new System.Drawing.Point(262, 14);
            this.AM_RAE_BTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.AM_RAE_BTN.Name = "AM_RAE_BTN";
            this.AM_RAE_BTN.Size = new System.Drawing.Size(256, 38);
            this.AM_RAE_BTN.TabIndex = 3;
            this.AM_RAE_BTN.Text = "Remove Active Enforcement";
            this.AM_RAE_BTN.Tone = Material3.WinForms.Controls.MaterialTone.Warning;
            this.AM_RAE_BTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Tonal;
            // 
            // AM_CS_BTN
            // 
            this.AM_CS_BTN.AutoSize = false;
            this.AM_CS_BTN.Location = new System.Drawing.Point(526, 14);
            this.AM_CS_BTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.AM_CS_BTN.Name = "AM_CS_BTN";
            this.AM_CS_BTN.Size = new System.Drawing.Size(128, 38);
            this.AM_CS_BTN.TabIndex = 1;
            this.AM_CS_BTN.Text = "Clean Slate";
            this.AM_CS_BTN.Tone = Material3.WinForms.Controls.MaterialTone.Error;
            this.AM_CS_BTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Tonal;
            // 
            // AM_OEP_BTN
            // 
            this.AM_OEP_BTN.AutoSize = false;
            this.AM_OEP_BTN.Location = new System.Drawing.Point(8, 14);
            this.AM_OEP_BTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.AM_OEP_BTN.Name = "AM_OEP_BTN";
            this.AM_OEP_BTN.Size = new System.Drawing.Size(244, 38);
            this.AM_OEP_BTN.TabIndex = 0;
            this.AM_OEP_BTN.Text = "Open Enforcement Profile";
            this.AM_OEP_BTN.Tone = Material3.WinForms.Controls.MaterialTone.Accent;
            this.AM_OEP_BTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Filled;
            // 
            // materialDivider4
            // 
            this.materialDivider4.BackColor = System.Drawing.Color.Transparent;
            this.materialDivider4.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialDivider4.Location = new System.Drawing.Point(14, 416);
            this.materialDivider4.Margin = new System.Windows.Forms.Padding(10);
            this.materialDivider4.Name = "materialDivider4";
            this.materialDivider4.Size = new System.Drawing.Size(662, 10);
            this.materialDivider4.TabIndex = 6;
            this.materialDivider4.TabStop = false;
            // 
            // materialCard3
            // 
            this.materialCard3.Controls.Add(this.AM_EnforcementHistory);
            this.materialCard3.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialCard3.Location = new System.Drawing.Point(14, 24);
            this.materialCard3.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard3.Name = "materialCard3";
            this.materialCard3.Padding = new System.Windows.Forms.Padding(5);
            this.materialCard3.Size = new System.Drawing.Size(662, 392);
            this.materialCard3.TabIndex = 5;
            this.materialCard3.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            // 
            // AM_EnforcementHistory
            // 
            this.AM_EnforcementHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AM_EnforcementHistory.Location = new System.Drawing.Point(5, 5);
            this.AM_EnforcementHistory.Name = "AM_EnforcementHistory";
            this.AM_EnforcementHistory.Size = new System.Drawing.Size(652, 382);
            this.AM_EnforcementHistory.TabIndex = 0;
            // 
            // materialDivider2
            // 
            this.materialDivider2.BackColor = System.Drawing.Color.Transparent;
            this.materialDivider2.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialDivider2.Location = new System.Drawing.Point(14, 14);
            this.materialDivider2.Margin = new System.Windows.Forms.Padding(10);
            this.materialDivider2.Name = "materialDivider2";
            this.materialDivider2.Size = new System.Drawing.Size(662, 10);
            this.materialDivider2.TabIndex = 4;
            this.materialDivider2.TabStop = false;
            // 
            // ActiveEnforcement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = Material3.WinForms.Theming.MaterialColors.Surface;
            this.Controls.Add(this.AM_EA_Title);
            this.Controls.Add(this.materialCard2);
            this.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.Name = "ActiveEnforcement";
            this.Size = new System.Drawing.Size(738, 600);
            this.AM_EA_Title.ResumeLayout(false);
            this.materialCard2.ResumeLayout(false);
            this.materialCard5.ResumeLayout(false);
            this.materialCard3.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Material3.WinForms.Controls.MaterialCard AM_EA_Title;
        private Material3.WinForms.Controls.MaterialLabel FilterLBL;
        private Material3.WinForms.Controls.MaterialCard materialCard2;
        private Material3.WinForms.Controls.MaterialCard materialCard5;
        private Material3.WinForms.Controls.MaterialButton AM_RAE_BTN;
        private Material3.WinForms.Controls.MaterialButton AM_CS_BTN;
        private Material3.WinForms.Controls.MaterialButton AM_OEP_BTN;
        private Material3.WinForms.Controls.MaterialDivider materialDivider4;
        private Material3.WinForms.Controls.MaterialCard materialCard3;
        private BDSUtil.UI.MaterialCheckListBox AM_EnforcementHistory;
        private Material3.WinForms.Controls.MaterialDivider materialDivider2;
    }
}
