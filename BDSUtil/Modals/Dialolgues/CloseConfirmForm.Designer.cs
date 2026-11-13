namespace BDSUtil
{
    partial class CloseConfirmForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.materialLabel1 = new Material3.WinForms.Controls.MaterialLabel();
            this.YesBtn = new Material3.WinForms.Controls.MaterialButton();
            this.ForceStopBtn = new Material3.WinForms.Controls.MaterialButton();
            this.NoBtn = new Material3.WinForms.Controls.MaterialButton();
            this.SuspendLayout();
            // 
            // materialLabel1
            // 
            this.materialLabel1.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            this.materialLabel1.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.materialLabel1.Location = new System.Drawing.Point(16, 18);
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(412, 66);
            this.materialLabel1.TabIndex = 0;
            this.materialLabel1.Text = "The Minecraft Bedrock Server is currently running.\r\nHow would you like to handle shutdown before exiting?";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // YesBtn
            // 
            this.YesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.YesBtn.Location = new System.Drawing.Point(28, 98);
            this.YesBtn.Name = "YesBtn";
            this.YesBtn.Size = new System.Drawing.Size(124, 38);
            this.YesBtn.TabIndex = 1;
            this.YesBtn.Text = "Clean Stop";
            this.YesBtn.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Filled;
            this.YesBtn.Click += new System.EventHandler(this.YesBtn_Click);
            // 
            // ForceStopBtn
            // 
            this.ForceStopBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ForceStopBtn.Location = new System.Drawing.Point(162, 98);
            this.ForceStopBtn.Name = "ForceStopBtn";
            this.ForceStopBtn.Size = new System.Drawing.Size(120, 38);
            this.ForceStopBtn.TabIndex = 2;
            this.ForceStopBtn.Text = "Force Stop";
            this.ForceStopBtn.Tone = Material3.WinForms.Controls.MaterialTone.Error;
            this.ForceStopBtn.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Filled;
            this.ForceStopBtn.Click += new System.EventHandler(this.ForceStopBtn_Click);
            // 
            // NoBtn
            // 
            this.NoBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NoBtn.Location = new System.Drawing.Point(292, 98);
            this.NoBtn.Name = "NoBtn";
            this.NoBtn.Size = new System.Drawing.Size(124, 38);
            this.NoBtn.TabIndex = 3;
            this.NoBtn.Text = "Cancel";
            this.NoBtn.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Outlined;
            this.NoBtn.Click += new System.EventHandler(this.NoBtn_Click);
            // 
            // CloseConfirmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(444, 154);
            this.Controls.Add(this.NoBtn);
            this.Controls.Add(this.ForceStopBtn);
            this.Controls.Add(this.YesBtn);
            this.Controls.Add(this.materialLabel1);
            this.Name = "CloseConfirmForm";
            this.Text = "Close Confirmation";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private Material3.WinForms.Controls.MaterialLabel materialLabel1;
        private Material3.WinForms.Controls.MaterialButton YesBtn;
        private Material3.WinForms.Controls.MaterialButton ForceStopBtn;
        private Material3.WinForms.Controls.MaterialButton NoBtn;
    }
}