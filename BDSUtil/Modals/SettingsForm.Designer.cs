namespace BDSUtil
{
    partial class SettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.SettingsWrapper = new ReaLTaiizor.Controls.MaterialCard();
            this.SettingsContentCard = new ReaLTaiizor.Controls.MaterialCard();
            this.CloseButton = new ReaLTaiizor.Controls.MaterialButton();
            this.SettingsWrapper.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsWrapper
            // 
            this.SettingsWrapper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SettingsWrapper.Controls.Add(this.SettingsContentCard);
            this.SettingsWrapper.Controls.Add(this.CloseButton);
            this.SettingsWrapper.Depth = 0;
            this.SettingsWrapper.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsWrapper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.SettingsWrapper.Location = new System.Drawing.Point(3, 0);
            this.SettingsWrapper.Margin = new System.Windows.Forms.Padding(14);
            this.SettingsWrapper.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.SettingsWrapper.Name = "SettingsWrapper";
            this.SettingsWrapper.Padding = new System.Windows.Forms.Padding(14);
            this.SettingsWrapper.Size = new System.Drawing.Size(794, 447);
            this.SettingsWrapper.TabIndex = 0;
            // 
            // SettingsContentCard
            // 
            this.SettingsContentCard.AutoScroll = true;
            this.SettingsContentCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.SettingsContentCard.Depth = 0;
            this.SettingsContentCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.SettingsContentCard.Location = new System.Drawing.Point(3, 3);
            this.SettingsContentCard.Margin = new System.Windows.Forms.Padding(14);
            this.SettingsContentCard.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.SettingsContentCard.Name = "SettingsContentCard";
            this.SettingsContentCard.Padding = new System.Windows.Forms.Padding(14);
            this.SettingsContentCard.Size = new System.Drawing.Size(790, 383);
            this.SettingsContentCard.TabIndex = 2;
            // 
            // CloseButton
            // 
            this.CloseButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CloseButton.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.CloseButton.Depth = 0;
            this.CloseButton.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.CloseButton.HighEmphasis = true;
            this.CloseButton.Icon = null;
            this.CloseButton.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.CloseButton.Location = new System.Drawing.Point(14, 397);
            this.CloseButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.CloseButton.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.NoAccentTextColor = System.Drawing.Color.Empty;
            this.CloseButton.Size = new System.Drawing.Size(766, 36);
            this.CloseButton.TabIndex = 0;
            this.CloseButton.Text = "Save && Close";
            this.CloseButton.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.CloseButton.UseAccentColor = false;
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.SettingsWrapper);
            this.FormStyle = ReaLTaiizor.Enum.Material.FormStyles.StatusAndActionBar_None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm";
            this.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Text = "SettingsForm";
            this.SettingsWrapper.ResumeLayout(false);
            this.SettingsWrapper.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ReaLTaiizor.Controls.MaterialCard SettingsWrapper;
        private ReaLTaiizor.Controls.MaterialButton CloseButton;
        private ReaLTaiizor.Controls.MaterialCard SettingsContentCard;
    }
}
