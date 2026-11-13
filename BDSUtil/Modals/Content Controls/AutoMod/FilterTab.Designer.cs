namespace BDSUtil.Modals.Content_Controls.AutoMod
{
    partial class FilterTab
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
            this.materialDivider2 = new Material3.WinForms.Controls.MaterialDivider();
            this.materialCard1 = new Material3.WinForms.Controls.MaterialCard();
            this.FilterLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.materialCard2 = new Material3.WinForms.Controls.MaterialCard();
            this.materialCard5 = new Material3.WinForms.Controls.MaterialCard();
            this.RemBTN = new Material3.WinForms.Controls.MaterialButton();
            this.AddBTN = new Material3.WinForms.Controls.MaterialButton();
            this.materialDivider4 = new Material3.WinForms.Controls.MaterialDivider();
            this.materialCard3 = new Material3.WinForms.Controls.MaterialCard();
            this.FilterCheckListBox = new BDSUtil.UI.MaterialCheckListBox();
            this.SABTN = new Material3.WinForms.Controls.MaterialButton();
            this.DSABTN = new Material3.WinForms.Controls.MaterialButton();
            this.materialCard1.SuspendLayout();
            this.materialCard2.SuspendLayout();
            this.materialCard5.SuspendLayout();
            this.materialCard3.SuspendLayout();
            this.SuspendLayout();
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
            // materialCard1
            // 
            this.materialCard1.Controls.Add(this.FilterLBL);
            this.materialCard1.Location = new System.Drawing.Point(14, 14);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(690, 51);
            this.materialCard1.TabIndex = 7;
            this.materialCard1.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
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
            this.FilterLBL.Text = "Chat Filter";
            this.FilterLBL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // materialCard2
            // 
            this.materialCard2.Controls.Add(this.materialCard5);
            this.materialCard2.Controls.Add(this.materialDivider4);
            this.materialCard2.Controls.Add(this.materialCard3);
            this.materialCard2.Controls.Add(this.materialDivider2);
            this.materialCard2.Location = new System.Drawing.Point(14, 75);
            this.materialCard2.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard2.Name = "materialCard2";
            this.materialCard2.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard2.Size = new System.Drawing.Size(690, 504);
            this.materialCard2.TabIndex = 8;
            this.materialCard2.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            // 
            // materialCard5
            // 
            this.materialCard5.Controls.Add(this.DSABTN);
            this.materialCard5.Controls.Add(this.SABTN);
            this.materialCard5.Controls.Add(this.RemBTN);
            this.materialCard5.Controls.Add(this.AddBTN);
            this.materialCard5.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialCard5.Location = new System.Drawing.Point(14, 426);
            this.materialCard5.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard5.Name = "materialCard5";
            this.materialCard5.Padding = new System.Windows.Forms.Padding(0);
            this.materialCard5.Size = new System.Drawing.Size(662, 65);
            this.materialCard5.TabIndex = 7;
            this.materialCard5.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            // 
            // RemBTN
            // 
            this.RemBTN.AutoSize = false;
            this.RemBTN.Location = new System.Drawing.Point(518, 14);
            this.RemBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.RemBTN.Name = "RemBTN";
            this.RemBTN.Size = new System.Drawing.Size(136, 38);
            this.RemBTN.TabIndex = 1;
            this.RemBTN.Text = "Remove Word";
            this.RemBTN.Tone = Material3.WinForms.Controls.MaterialTone.Error;
            this.RemBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Tonal;
            // 
            // AddBTN
            // 
            this.AddBTN.AutoSize = false;
            this.AddBTN.Location = new System.Drawing.Point(8, 14);
            this.AddBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.AddBTN.Name = "AddBTN";
            this.AddBTN.Size = new System.Drawing.Size(112, 38);
            this.AddBTN.TabIndex = 0;
            this.AddBTN.Text = "Add Word";
            this.AddBTN.Tone = Material3.WinForms.Controls.MaterialTone.Accent;
            this.AddBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Filled;
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
            this.materialCard3.Controls.Add(this.FilterCheckListBox);
            this.materialCard3.Dock = System.Windows.Forms.DockStyle.Top;
            this.materialCard3.Location = new System.Drawing.Point(14, 24);
            this.materialCard3.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard3.Name = "materialCard3";
            this.materialCard3.Padding = new System.Windows.Forms.Padding(5);
            this.materialCard3.Size = new System.Drawing.Size(662, 392);
            this.materialCard3.TabIndex = 5;
            this.materialCard3.Variant = Material3.WinForms.Controls.MaterialCardVariant.Outlined;
            // 
            // FilterCheckListBox
            // 
            this.FilterCheckListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FilterCheckListBox.Location = new System.Drawing.Point(5, 5);
            this.FilterCheckListBox.Name = "FilterCheckListBox";
            this.FilterCheckListBox.Size = new System.Drawing.Size(652, 382);
            this.FilterCheckListBox.TabIndex = 0;
            // 
            // SABTN
            // 
            this.SABTN.AutoSize = false;
            this.SABTN.Location = new System.Drawing.Point(190, 14);
            this.SABTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.SABTN.Name = "SABTN";
            this.SABTN.Size = new System.Drawing.Size(112, 38);
            this.SABTN.TabIndex = 2;
            this.SABTN.Text = "Select All";
            this.SABTN.Tone = Material3.WinForms.Controls.MaterialTone.Neutral;
            this.SABTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Outlined;
            this.SABTN.Click += new System.EventHandler(this.SABTN_Click);
            // 
            // DSABTN
            // 
            this.DSABTN.AutoSize = false;
            this.DSABTN.Location = new System.Drawing.Point(310, 14);
            this.DSABTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.DSABTN.Name = "DSABTN";
            this.DSABTN.Size = new System.Drawing.Size(128, 38);
            this.DSABTN.TabIndex = 3;
            this.DSABTN.Text = "Deselect All";
            this.DSABTN.Tone = Material3.WinForms.Controls.MaterialTone.Neutral;
            this.DSABTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Outlined;
            this.DSABTN.Click += new System.EventHandler(this.DSABTN_Click);
            // 
            // FilterTab
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = Material3.WinForms.Theming.MaterialColors.Surface;
            this.Controls.Add(this.materialCard1);
            this.Controls.Add(this.materialCard2);
            this.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.MaximumSize = new System.Drawing.Size(738, 10000);
            this.Name = "FilterTab";
            this.Size = new System.Drawing.Size(738, 600);
            this.materialCard1.ResumeLayout(false);
            this.materialCard2.ResumeLayout(false);
            this.materialCard5.ResumeLayout(false);
            this.materialCard3.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private Material3.WinForms.Controls.MaterialDivider materialDivider2;
        private Material3.WinForms.Controls.MaterialCard materialCard1;
        private Material3.WinForms.Controls.MaterialLabel FilterLBL;
        private Material3.WinForms.Controls.MaterialCard materialCard2;
        private Material3.WinForms.Controls.MaterialCard materialCard5;
        private Material3.WinForms.Controls.MaterialButton RemBTN;
        private Material3.WinForms.Controls.MaterialButton AddBTN;
        private Material3.WinForms.Controls.MaterialDivider materialDivider4;
        private Material3.WinForms.Controls.MaterialCard materialCard3;
        private BDSUtil.UI.MaterialCheckListBox FilterCheckListBox;
        private Material3.WinForms.Controls.MaterialButton DSABTN;
        private Material3.WinForms.Controls.MaterialButton SABTN;
    }
}
