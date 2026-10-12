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
            this.materialLabel1 = new ReaLTaiizor.Controls.MaterialLabel();
            this.YesBtn = new ReaLTaiizor.Controls.MaterialButton();
            this.ForceStopBtn = new ReaLTaiizor.Controls.MaterialButton();
            this.NoBtn = new ReaLTaiizor.Controls.MaterialButton();
            this.SuspendLayout();
            // 
            // materialLabel1
            // 
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(6, 24);
            this.materialLabel1.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(424, 69);
            this.materialLabel1.TabIndex = 0;
            this.materialLabel1.Text = "The Minecraft Bedrock Server is currently running.\r\nHow would you like to handle " +
    "shutdown before exiting?";
            this.materialLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // YesBtn
            // 
            this.YesBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.YesBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.YesBtn.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.YesBtn.Depth = 0;
            this.YesBtn.HighEmphasis = true;
            this.YesBtn.Icon = null;
            this.YesBtn.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.YesBtn.Location = new System.Drawing.Point(48, 99);
            this.YesBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.YesBtn.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.YesBtn.Name = "YesBtn";
            this.YesBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.YesBtn.Size = new System.Drawing.Size(109, 36);
            this.YesBtn.TabIndex = 1;
            this.YesBtn.Text = "Clean Stop";
            this.YesBtn.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.YesBtn.UseAccentColor = false;
            this.YesBtn.UseVisualStyleBackColor = true;
            this.YesBtn.Click += new System.EventHandler(this.YesBtn_Click);
            // 
            // ForceStopBtn
            // 
            this.ForceStopBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForceStopBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ForceStopBtn.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.ForceStopBtn.Depth = 0;
            this.ForceStopBtn.HighEmphasis = true;
            this.ForceStopBtn.Icon = null;
            this.ForceStopBtn.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.ForceStopBtn.Location = new System.Drawing.Point(165, 99);
            this.ForceStopBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.ForceStopBtn.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.ForceStopBtn.Name = "ForceStopBtn";
            this.ForceStopBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.ForceStopBtn.Size = new System.Drawing.Size(108, 36);
            this.ForceStopBtn.TabIndex = 2;
            this.ForceStopBtn.Text = "Force Stop";
            this.ForceStopBtn.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.ForceStopBtn.UseAccentColor = false;
            this.ForceStopBtn.UseVisualStyleBackColor = true;
            this.ForceStopBtn.Click += new System.EventHandler(this.ForceStopBtn_Click);
            // 
            // NoBtn
            // 
            this.NoBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.NoBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.NoBtn.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.NoBtn.Depth = 0;
            this.NoBtn.HighEmphasis = true;
            this.NoBtn.Icon = null;
            this.NoBtn.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.NoBtn.Location = new System.Drawing.Point(281, 99);
            this.NoBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.NoBtn.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.NoBtn.Name = "NoBtn";
            this.NoBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.NoBtn.Size = new System.Drawing.Size(77, 36);
            this.NoBtn.TabIndex = 3;
            this.NoBtn.Text = "Cancel";
            this.NoBtn.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.NoBtn.UseAccentColor = false;
            this.NoBtn.UseVisualStyleBackColor = true;
            this.NoBtn.Click += new System.EventHandler(this.NoBtn_Click);
            // 
            // CloseConfirmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 150);
            this.Controls.Add(this.NoBtn);
            this.Controls.Add(this.ForceStopBtn);
            this.Controls.Add(this.YesBtn);
            this.Controls.Add(this.materialLabel1);
            this.FormStyle = ReaLTaiizor.Enum.Material.FormStyles.ActionBar_None;
            this.Name = "CloseConfirmForm";
            this.Padding = new System.Windows.Forms.Padding(3, 24, 3, 3);
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ReaLTaiizor.Controls.MaterialLabel materialLabel1;
        private ReaLTaiizor.Controls.MaterialButton YesBtn;
        private ReaLTaiizor.Controls.MaterialButton ForceStopBtn;
        private ReaLTaiizor.Controls.MaterialButton NoBtn;
    }
}