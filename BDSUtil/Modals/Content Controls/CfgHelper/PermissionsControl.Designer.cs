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
            materialCard1 = new ReaLTaiizor.Controls.MaterialCard();
            OpLBL = new ReaLTaiizor.Controls.MaterialLabel();
            materialCard2 = new ReaLTaiizor.Controls.MaterialCard();
            materialCard5 = new ReaLTaiizor.Controls.MaterialCard();
            DerOpPlayerBTN = new ReaLTaiizor.Controls.MaterialButton();
            OpPlayerBTN = new ReaLTaiizor.Controls.MaterialButton();
            materialDivider4 = new ReaLTaiizor.Controls.MaterialDivider();
            materialCard3 = new ReaLTaiizor.Controls.MaterialCard();
            materialCheckListBox1 = new ReaLTaiizor.Controls.MaterialCheckListBox();
            materialDivider2 = new ReaLTaiizor.Controls.MaterialDivider();
            materialCard1.SuspendLayout();
            materialCard2.SuspendLayout();
            materialCard5.SuspendLayout();
            materialCard3.SuspendLayout();
            SuspendLayout();
            // 
            // materialCard1
            // 
            materialCard1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            materialCard1.Controls.Add(OpLBL);
            materialCard1.Depth = 0;
            materialCard1.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialCard1.Location = new System.Drawing.Point(16, 16);
            materialCard1.Margin = new System.Windows.Forms.Padding(16);
            materialCard1.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            materialCard1.Name = "materialCard1";
            materialCard1.Padding = new System.Windows.Forms.Padding(16);
            materialCard1.Size = new System.Drawing.Size(795, 59);
            materialCard1.TabIndex = 7;
            // 
            // OpLBL
            // 
            OpLBL.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            OpLBL.Depth = 0;
            OpLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            OpLBL.Location = new System.Drawing.Point(273, 21);
            OpLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            OpLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            OpLBL.Name = "OpLBL";
            OpLBL.Size = new System.Drawing.Size(217, 22);
            OpLBL.TabIndex = 0;
            OpLBL.Text = "Server Operators";
            OpLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialCard2
            // 
            materialCard2.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            materialCard2.Controls.Add(materialCard5);
            materialCard2.Controls.Add(materialDivider4);
            materialCard2.Controls.Add(materialCard3);
            materialCard2.Controls.Add(materialDivider2);
            materialCard2.Depth = 0;
            materialCard2.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialCard2.Location = new System.Drawing.Point(16, 87);
            materialCard2.Margin = new System.Windows.Forms.Padding(16);
            materialCard2.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            materialCard2.Name = "materialCard2";
            materialCard2.Padding = new System.Windows.Forms.Padding(16);
            materialCard2.Size = new System.Drawing.Size(795, 590);
            materialCard2.TabIndex = 8;
            // 
            // materialCard5
            // 
            materialCard5.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            materialCard5.Controls.Add(DerOpPlayerBTN);
            materialCard5.Controls.Add(OpPlayerBTN);
            materialCard5.Depth = 0;
            materialCard5.Dock = System.Windows.Forms.DockStyle.Top;
            materialCard5.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialCard5.Location = new System.Drawing.Point(16, 499);
            materialCard5.Margin = new System.Windows.Forms.Padding(16);
            materialCard5.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            materialCard5.Name = "materialCard5";
            materialCard5.Padding = new System.Windows.Forms.Padding(6);
            materialCard5.Size = new System.Drawing.Size(763, 75);
            materialCard5.TabIndex = 7;
            // 
            // DerOpPlayerBTN
            // 
            DerOpPlayerBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            DerOpPlayerBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            DerOpPlayerBTN.Depth = 0;
            DerOpPlayerBTN.HighEmphasis = true;
            DerOpPlayerBTN.Icon = null;
            DerOpPlayerBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            DerOpPlayerBTN.Location = new System.Drawing.Point(388, 18);
            DerOpPlayerBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            DerOpPlayerBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            DerOpPlayerBTN.Name = "DerOpPlayerBTN";
            DerOpPlayerBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            DerOpPlayerBTN.Size = new System.Drawing.Size(210, 36);
            DerOpPlayerBTN.TabIndex = 1;
            DerOpPlayerBTN.Text = "DeOp Selected Player(s)";
            DerOpPlayerBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            DerOpPlayerBTN.UseAccentColor = false;
            DerOpPlayerBTN.UseVisualStyleBackColor = true;
            DerOpPlayerBTN.Click += DeOpPlayerBTN_Click;
            // 
            // OpPlayerBTN
            // 
            OpPlayerBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            OpPlayerBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            OpPlayerBTN.Depth = 0;
            OpPlayerBTN.HighEmphasis = true;
            OpPlayerBTN.Icon = null;
            OpPlayerBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            OpPlayerBTN.Location = new System.Drawing.Point(148, 18);
            OpPlayerBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            OpPlayerBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            OpPlayerBTN.Name = "OpPlayerBTN";
            OpPlayerBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            OpPlayerBTN.Size = new System.Drawing.Size(198, 36);
            OpPlayerBTN.TabIndex = 0;
            OpPlayerBTN.Text = "Make Player Operator";
            OpPlayerBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            OpPlayerBTN.UseAccentColor = false;
            OpPlayerBTN.UseVisualStyleBackColor = true;
            OpPlayerBTN.Click += OpPlayerBTN_Click;
            // 
            // materialDivider4
            // 
            materialDivider4.BackColor = System.Drawing.Color.Transparent;
            materialDivider4.Depth = 0;
            materialDivider4.Dock = System.Windows.Forms.DockStyle.Top;
            materialDivider4.Enabled = false;
            materialDivider4.Location = new System.Drawing.Point(16, 487);
            materialDivider4.Margin = new System.Windows.Forms.Padding(12);
            materialDivider4.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            materialDivider4.Name = "materialDivider4";
            materialDivider4.Size = new System.Drawing.Size(763, 12);
            materialDivider4.TabIndex = 6;
            materialDivider4.Text = "materialDivider4";
            // 
            // materialCard3
            // 
            materialCard3.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            materialCard3.Controls.Add(materialCheckListBox1);
            materialCard3.Depth = 0;
            materialCard3.Dock = System.Windows.Forms.DockStyle.Top;
            materialCard3.ForeColor = System.Drawing.Color.FromArgb(222, 0, 0, 0);
            materialCard3.Location = new System.Drawing.Point(16, 28);
            materialCard3.Margin = new System.Windows.Forms.Padding(16);
            materialCard3.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            materialCard3.Name = "materialCard3";
            materialCard3.Padding = new System.Windows.Forms.Padding(6);
            materialCard3.Size = new System.Drawing.Size(763, 459);
            materialCard3.TabIndex = 5;
            // 
            // materialCheckListBox1
            // 
            materialCheckListBox1.AutoScroll = true;
            materialCheckListBox1.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            materialCheckListBox1.Depth = 0;
            materialCheckListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            materialCheckListBox1.Location = new System.Drawing.Point(6, 6);
            materialCheckListBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialCheckListBox1.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            materialCheckListBox1.Name = "materialCheckListBox1";
            materialCheckListBox1.Size = new System.Drawing.Size(751, 447);
            materialCheckListBox1.Striped = false;
            materialCheckListBox1.StripeDarkColor = System.Drawing.Color.Empty;
            materialCheckListBox1.TabIndex = 0;
            // 
            // materialDivider2
            // 
            materialDivider2.BackColor = System.Drawing.Color.Transparent;
            materialDivider2.Depth = 0;
            materialDivider2.Dock = System.Windows.Forms.DockStyle.Top;
            materialDivider2.Enabled = false;
            materialDivider2.Location = new System.Drawing.Point(16, 16);
            materialDivider2.Margin = new System.Windows.Forms.Padding(12);
            materialDivider2.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            materialDivider2.Name = "materialDivider2";
            materialDivider2.Size = new System.Drawing.Size(763, 12);
            materialDivider2.TabIndex = 4;
            materialDivider2.Text = "materialDivider2";
            // 
            // PermissionsControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(50, 50, 50);
            Controls.Add(materialCard1);
            Controls.Add(materialCard2);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximumSize = new System.Drawing.Size(861, 11538);
            Name = "PermissionsControl";
            Size = new System.Drawing.Size(861, 692);
            Load += PermissionsControl_Load;
            materialCard1.ResumeLayout(false);
            materialCard2.ResumeLayout(false);
            materialCard5.ResumeLayout(false);
            materialCard5.PerformLayout();
            materialCard3.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.MaterialCard materialCard1;
        private ReaLTaiizor.Controls.MaterialLabel OpLBL;
        private ReaLTaiizor.Controls.MaterialCard materialCard2;
        private ReaLTaiizor.Controls.MaterialCard materialCard5;
        private ReaLTaiizor.Controls.MaterialButton DerOpPlayerBTN;
        private ReaLTaiizor.Controls.MaterialButton OpPlayerBTN;
        private ReaLTaiizor.Controls.MaterialDivider materialDivider4;
        private ReaLTaiizor.Controls.MaterialCard materialCard3;
        private ReaLTaiizor.Controls.MaterialCheckListBox materialCheckListBox1;
        private ReaLTaiizor.Controls.MaterialDivider materialDivider2;
    }
}
