namespace BDSUtil.Modals
{
    partial class ServerUpdateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerUpdateForm));
            StatusLabel = new ReaLTaiizor.Controls.MaterialLabel();
            UpdateProgressBar = new ReaLTaiizor.Controls.MaterialProgressBar();
            UpdateCloseBTN = new ReaLTaiizor.Controls.MaterialButton();
            SuspendLayout();
            // 
            // StatusLabel
            // 
            StatusLabel.AutoSize = true;
            StatusLabel.Depth = 0;
            StatusLabel.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            StatusLabel.Location = new System.Drawing.Point(23, 90);
            StatusLabel.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new System.Drawing.Size(140, 19);
            StatusLabel.TabIndex = 0;
            StatusLabel.Text = "Status: Initializing...";
            // 
            // UpdateProgressBar
            // 
            UpdateProgressBar.Depth = 0;
            UpdateProgressBar.Location = new System.Drawing.Point(26, 130);
            UpdateProgressBar.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            UpdateProgressBar.Name = "UpdateProgressBar";
            UpdateProgressBar.Size = new System.Drawing.Size(550, 5);
            UpdateProgressBar.TabIndex = 1;
            UpdateProgressBar.UseAccentColor = false;
            // 
            // UpdateCloseBTN
            // 
            UpdateCloseBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            UpdateCloseBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            UpdateCloseBTN.Depth = 0;
            UpdateCloseBTN.HighEmphasis = true;
            UpdateCloseBTN.Icon = null;
            UpdateCloseBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            UpdateCloseBTN.Location = new System.Drawing.Point(245, 185);
            UpdateCloseBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            UpdateCloseBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            UpdateCloseBTN.Name = "UpdateCloseBTN";
            UpdateCloseBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            UpdateCloseBTN.Size = new System.Drawing.Size(66, 36);
            UpdateCloseBTN.TabIndex = 2;
            UpdateCloseBTN.Text = "CLOSE";
            UpdateCloseBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            UpdateCloseBTN.UseAccentColor = false;
            UpdateCloseBTN.UseVisualStyleBackColor = true;
            UpdateCloseBTN.Click += UpdateCloseBTN_Click;
            // 
            // ServerUpdateForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(600, 230);
            Controls.Add(UpdateCloseBTN);
            Controls.Add(UpdateProgressBar);
            Controls.Add(StatusLabel);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Name = "ServerUpdateForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Bedrock Server Updater";
            Load += ServerUpdateForm_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private ReaLTaiizor.Controls.MaterialLabel StatusLabel;
        private ReaLTaiizor.Controls.MaterialProgressBar UpdateProgressBar;
        private ReaLTaiizor.Controls.MaterialButton UpdateCloseBTN;
    }
}