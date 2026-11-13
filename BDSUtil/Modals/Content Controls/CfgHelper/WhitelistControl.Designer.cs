namespace BDSUtil.Modals.Content_Controls.CfgHelper
{
    partial class WhitelistControl
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
            materialDivider2 = new Material3.WinForms.Controls.MaterialDivider();
            materialCard1 = new Material3.WinForms.Controls.MaterialCard();
            WhitelistLBL = new Material3.WinForms.Controls.MaterialLabel();
            materialCard2 = new Material3.WinForms.Controls.MaterialCard();
            materialCard5 = new Material3.WinForms.Controls.MaterialCard();
            RemovePlayerBTN = new Material3.WinForms.Controls.MaterialButton();
            AddPlayerBTN = new Material3.WinForms.Controls.MaterialButton();
            materialDivider4 = new Material3.WinForms.Controls.MaterialDivider();
            materialCard3 = new Material3.WinForms.Controls.MaterialCard();
            materialCheckListBox1 = new BDSUtil.UI.MaterialCheckListBox();
            materialCard1.SuspendLayout();
            materialCard2.SuspendLayout();
            materialCard5.SuspendLayout();
            materialCard3.SuspendLayout();
            SuspendLayout();
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
            // materialCard1
            // 
            materialCard1.Controls.Add(WhitelistLBL);
            materialCard1.Location = new System.Drawing.Point(16, 16);
            materialCard1.Margin = new System.Windows.Forms.Padding(16);
            materialCard1.Name = "materialCard1";
            materialCard1.Padding = new System.Windows.Forms.Padding(16);
            materialCard1.Size = new System.Drawing.Size(795, 59);
            materialCard1.TabIndex = 5;
            materialCard1.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            // 
            // WhitelistLBL
            // 
            WhitelistLBL.BackColor = System.Drawing.Color.Transparent;
            WhitelistLBL.Dock = System.Windows.Forms.DockStyle.Fill;
            WhitelistLBL.Font = Material3.WinForms.Typography.TypeScale.TitleMedium;
            WhitelistLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            WhitelistLBL.Location = new System.Drawing.Point(16, 16);
            WhitelistLBL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            WhitelistLBL.Name = "WhitelistLBL";
            WhitelistLBL.Size = new System.Drawing.Size(763, 27);
            WhitelistLBL.TabIndex = 0;
            WhitelistLBL.Text = "Whitelisted Players";
            WhitelistLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            materialCard2.TabIndex = 6;
            materialCard2.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            // 
            // materialCard5
            // 
            materialCard5.Controls.Add(RemovePlayerBTN);
            materialCard5.Controls.Add(AddPlayerBTN);
            materialCard5.Dock = System.Windows.Forms.DockStyle.Top;
            materialCard5.Location = new System.Drawing.Point(16, 496);
            materialCard5.Margin = new System.Windows.Forms.Padding(16);
            materialCard5.Name = "materialCard5";
            materialCard5.Padding = new System.Windows.Forms.Padding(0);
            materialCard5.Size = new System.Drawing.Size(763, 75);
            materialCard5.TabIndex = 7;
            materialCard5.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            // 
            // RemovePlayerBTN
            // 
            RemovePlayerBTN.AutoSize = false;
            RemovePlayerBTN.Location = new System.Drawing.Point(363, 18);
            RemovePlayerBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            RemovePlayerBTN.Name = "RemovePlayerBTN";
            RemovePlayerBTN.Size = new System.Drawing.Size(230, 38);
            RemovePlayerBTN.TabIndex = 1;
            RemovePlayerBTN.Text = "Remove Selected Players";
            RemovePlayerBTN.Tone = Material3.WinForms.Controls.MaterialTone.Error;
            RemovePlayerBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Tonal;
            RemovePlayerBTN.Click += RemovePlayerBTN_Click;
            // 
            // AddPlayerBTN
            // 
            AddPlayerBTN.AutoSize = false;
            AddPlayerBTN.Location = new System.Drawing.Point(196, 18);
            AddPlayerBTN.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            AddPlayerBTN.Name = "AddPlayerBTN";
            AddPlayerBTN.Size = new System.Drawing.Size(134, 38);
            AddPlayerBTN.TabIndex = 0;
            AddPlayerBTN.Text = "Add Player";
            AddPlayerBTN.Tone = Material3.WinForms.Controls.MaterialTone.Accent;
            AddPlayerBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Filled;
            AddPlayerBTN.Click += AddPlayerBTN_Click;
            // 
            // materialDivider4
            // 
            materialDivider4.BackColor = System.Drawing.Color.Transparent;
            materialDivider4.Dock = System.Windows.Forms.DockStyle.Top;
            materialDivider4.Location = new System.Drawing.Point(16, 484);
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
            materialCard3.Size = new System.Drawing.Size(763, 456);
            materialCard3.TabIndex = 5;
            materialCard3.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            // 
            // materialCheckListBox1
            // 
            materialCheckListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            materialCheckListBox1.Location = new System.Drawing.Point(6, 6);
            materialCheckListBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            materialCheckListBox1.Name = "materialCheckListBox1";
            materialCheckListBox1.Size = new System.Drawing.Size(751, 444);
            materialCheckListBox1.TabIndex = 0;
            // 
            // WhitelistControl
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
            Name = "WhitelistControl";
            Size = new System.Drawing.Size(861, 692);
            Load += WhitelistControl_Load;
            materialCard1.ResumeLayout(false);
            materialCard2.ResumeLayout(false);
            materialCard5.ResumeLayout(false);
            materialCard3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Material3.WinForms.Controls.MaterialDivider materialDivider2;
        private Material3.WinForms.Controls.MaterialCard materialCard1;
        private Material3.WinForms.Controls.MaterialLabel WhitelistLBL;
        private Material3.WinForms.Controls.MaterialCard materialCard2;
        private Material3.WinForms.Controls.MaterialCard materialCard5;
        private Material3.WinForms.Controls.MaterialDivider materialDivider4;
        private Material3.WinForms.Controls.MaterialCard materialCard3;
        private BDSUtil.UI.MaterialCheckListBox materialCheckListBox1;
        private Material3.WinForms.Controls.MaterialButton RemovePlayerBTN;
        private Material3.WinForms.Controls.MaterialButton AddPlayerBTN;
    }
}
