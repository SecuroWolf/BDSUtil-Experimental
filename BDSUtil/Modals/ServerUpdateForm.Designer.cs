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
            StatusLabel = new Material3.WinForms.Controls.MaterialLabel();
            UpdateProgressBar = new BDSUtil.UI.MaterialMarqueeProgressBar();
            UpdateCloseBTN = new Material3.WinForms.Controls.MaterialButton();
            SuspendLayout();
            // 
            // StatusLabel
            // 
            StatusLabel.AutoSize = true;
            StatusLabel.Font = Material3.WinForms.Typography.TypeScale.BodyLarge;
            StatusLabel.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            StatusLabel.Location = new System.Drawing.Point(24, 68);
            StatusLabel.Name = "StatusLabel";
            StatusLabel.Size = new System.Drawing.Size(160, 20);
            StatusLabel.TabIndex = 0;
            StatusLabel.Text = "Status: Initializing...";
            // 
            // UpdateProgressBar
            // 
            UpdateProgressBar.Location = new System.Drawing.Point(24, 108);
            UpdateProgressBar.Name = "UpdateProgressBar";
            UpdateProgressBar.Size = new System.Drawing.Size(552, 8);
            UpdateProgressBar.TabIndex = 1;
            // 
            // UpdateCloseBTN
            // 
            UpdateCloseBTN.Location = new System.Drawing.Point(245, 146);
            UpdateCloseBTN.Name = "UpdateCloseBTN";
            UpdateCloseBTN.Size = new System.Drawing.Size(110, 40);
            UpdateCloseBTN.TabIndex = 2;
            UpdateCloseBTN.Text = "CLOSE";
            UpdateCloseBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Filled;
            UpdateCloseBTN.Click += UpdateCloseBTN_Click;
            // 
            // ServerUpdateForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            ClientSize = new System.Drawing.Size(600, 205);
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

        private Material3.WinForms.Controls.MaterialLabel StatusLabel;
        private BDSUtil.UI.MaterialMarqueeProgressBar UpdateProgressBar;
        private Material3.WinForms.Controls.MaterialButton UpdateCloseBTN;
    }
}