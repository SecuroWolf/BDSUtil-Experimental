namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    partial class PermissionsControl
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
            materialCard5 = new Material3.WinForms.Controls.MaterialCard();
            DerOpPlayerBTN = new Material3.WinForms.Controls.MaterialButton();
            OpPlayerBTN = new Material3.WinForms.Controls.MaterialButton();
            materialDivider4 = new Material3.WinForms.Controls.MaterialDivider();
            materialCard3 = new Material3.WinForms.Controls.MaterialCard();
            materialCheckListBox1 = new BDSUtil.UI.MaterialCheckListBox();
            materialDivider2 = new Material3.WinForms.Controls.MaterialDivider();
            materialCard1.SuspendLayout();
            materialCard2.SuspendLayout();
            materialCard5.SuspendLayout();
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
            materialCard1.TabIndex = 7;
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
            OpLBL.Text = "Server Operators";
            OpLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialCard2
            // 
            materialCard2.Controls.Add(materialCard5);
            materialCard2.Controls.Add(materialDivider4);
            materialCard2.Controls.Add(materialCard3);
            materialCard2.Controls.Add(materialDivider2);
            materialCard2.Location = new System.Drawing.Point(16, 87);
            materialCard2.Margin = new System.Windows.Forms.Padding(16);
            materialCard2.Name = "materialCard2";
            materialCard2.Padding = new System.Windows.Forms.Padding(16);
            materialCard2.Size = new System.Drawing.Size(795, 590);
            materialCard2.TabIndex = 8;
            materialCard2.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            // 
            // materialCard5
            // 
            materialCard5.Controls.Add(DerOpPlayerBTN);
            materialCard5.Controls.Add(OpPlayerBTN);
            materialCard5.Dock = System.Windows.Forms.DockStyle.Top;
            materialCard5.Location = new System.Drawing.Point(16, 499);
            materialCard5.Margin = new System.Windows.Forms.Padding(16);
            materialCard5.Name = "materialCard5";
            materialCard5.Padding = new System.Windows.Forms.Padding(0);
            materialCard5.Size = new System.Drawing.Size(763, 75);
            materialCard5.TabIndex = 7;
            materialCard5.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            // 
            // DerOpPlayerBTN
            // 
            DerOpPlayerBTN.AutoSize = false;
            DerOpPlayerBTN.Location = new System.Drawing.Point(392, 18);
            DerOpPlayerBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            DerOpPlayerBTN.Name = "DerOpPlayerBTN";
            DerOpPlayerBTN.Size = new System.Drawing.Size(224, 38);
            DerOpPlayerBTN.TabIndex = 1;
            DerOpPlayerBTN.Text = "DeOp Selected Player(s)";
            DerOpPlayerBTN.Tone = Material3.WinForms.Controls.MaterialTone.Error;
            DerOpPlayerBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Tonal;
            DerOpPlayerBTN.Click += DeOpPlayerBTN_Click;
            // 
            // OpPlayerBTN
            // 
            OpPlayerBTN.AutoSize = false;
            OpPlayerBTN.Location = new System.Drawing.Point(148, 18);
            OpPlayerBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            OpPlayerBTN.Name = "OpPlayerBTN";
            OpPlayerBTN.Size = new System.Drawing.Size(224, 38);
            OpPlayerBTN.TabIndex = 0;
            OpPlayerBTN.Text = "Make Player Operator";
            OpPlayerBTN.Tone = Material3.WinForms.Controls.MaterialTone.Accent;
            OpPlayerBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Filled;
            OpPlayerBTN.Click += OpPlayerBTN_Click;
            // 
            // materialDivider4
            // 
            materialDivider4.BackColor = System.Drawing.Color.Transparent;
            materialDivider4.Dock = System.Windows.Forms.DockStyle.Top;
            materialDivider4.Location = new System.Drawing.Point(16, 487);
            materialDivider4.Margin = new System.Windows.Forms.Padding(12);
            materialDivider4.Name = "materialDivider4";
            materialDivider4.Size = new System.Drawing.Size(763, 12);
            materialDivider4.TabIndex = 6;
            materialDivider4.TabStop = false;
            // 
            // materialCard3
            // 
            materialCard3.Controls.Add(materialCheckListBox1);
            materialCard3.Dock = System.Windows.Forms.DockStyle.Top;
            materialCard3.Location = new System.Drawing.Point(16, 28);
            materialCard3.Margin = new System.Windows.Forms.Padding(16);
            materialCard3.Name = "materialCard3";
            materialCard3.Padding = new System.Windows.Forms.Padding(6);
            materialCard3.Size = new System.Drawing.Size(763, 459);
            materialCard3.TabIndex = 5;
            materialCard3.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            // 
            // materialCheckListBox1
            // 
            materialCheckListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            materialCheckListBox1.Location = new System.Drawing.Point(6, 6);
            materialCheckListBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialCheckListBox1.Name = "materialCheckListBox1";
            materialCheckListBox1.Size = new System.Drawing.Size(751, 447);
            materialCheckListBox1.TabIndex = 0;
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
            // PermissionsControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            BackColor = Material3.WinForms.Theming.MaterialColors.Surface;
            Controls.Add(materialCard1);
            Controls.Add(materialCard2);
            ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximumSize = new System.Drawing.Size(861, 11538);
            Name = "PermissionsControl";
            Size = new System.Drawing.Size(861, 692);
            Load += PermissionsControl_Load;
            materialCard1.ResumeLayout(false);
            materialCard2.ResumeLayout(false);
            materialCard5.ResumeLayout(false);
            materialCard3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Material3.WinForms.Controls.MaterialCard materialCard1;
        private Material3.WinForms.Controls.MaterialLabel OpLBL;
        private Material3.WinForms.Controls.MaterialCard materialCard2;
        private Material3.WinForms.Controls.MaterialCard materialCard5;
        private Material3.WinForms.Controls.MaterialButton DerOpPlayerBTN;
        private Material3.WinForms.Controls.MaterialButton OpPlayerBTN;
        private Material3.WinForms.Controls.MaterialDivider materialDivider4;
        private Material3.WinForms.Controls.MaterialCard materialCard3;
        private BDSUtil.UI.MaterialCheckListBox materialCheckListBox1;
        private Material3.WinForms.Controls.MaterialDivider materialDivider2;
    }
}
