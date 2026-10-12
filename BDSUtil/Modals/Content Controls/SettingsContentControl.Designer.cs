namespace BDSUtil
{
    partial class SettingsContentControl
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

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsContentControl));
            this.ServerPathTextBox = new ReaLTaiizor.Controls.MaterialTextBoxEdit();
            this.BrowseBtn = new ReaLTaiizor.Controls.MaterialButton();
            this.CompressionTypeComboBox = new ReaLTaiizor.Controls.MaterialComboBox();
            this.compressionLabel = new ReaLTaiizor.Controls.MaterialLabel();
            this.materialLabel1 = new ReaLTaiizor.Controls.MaterialLabel();
            this.materialLabel2 = new ReaLTaiizor.Controls.MaterialLabel();
            this.SuspendLayout();
            // 
            // ServerPathTextBox
            // 
            this.ServerPathTextBox.AnimateReadOnly = false;
            this.ServerPathTextBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ServerPathTextBox.Depth = 0;
            this.ServerPathTextBox.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ServerPathTextBox.HideSelection = true;
            this.ServerPathTextBox.Hint = "Server Directory";
            this.ServerPathTextBox.LeadingIcon = null;
            this.ServerPathTextBox.Location = new System.Drawing.Point(6, 89);
            this.ServerPathTextBox.MaxLength = 32767;
            this.ServerPathTextBox.Name = "ServerPathTextBox";
            this.ServerPathTextBox.ReadOnly = true;
            this.ServerPathTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ServerPathTextBox.SelectedText = "";
            this.ServerPathTextBox.SelectionLength = 0;
            this.ServerPathTextBox.SelectionStart = 0;
            this.ServerPathTextBox.ShortcutsEnabled = true;
            this.ServerPathTextBox.Size = new System.Drawing.Size(662, 48);
            this.ServerPathTextBox.TabIndex = 1;
            this.ServerPathTextBox.TabStop = false;
            this.ServerPathTextBox.TrailingIcon = null;
            // 
            // BrowseBtn
            // 
            this.BrowseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowseBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BrowseBtn.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.BrowseBtn.Depth = 0;
            this.BrowseBtn.HighEmphasis = true;
            this.BrowseBtn.Icon = null;
            this.BrowseBtn.Location = new System.Drawing.Point(706, 96);
            this.BrowseBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.BrowseBtn.Name = "BrowseBtn";
            this.BrowseBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.BrowseBtn.Size = new System.Drawing.Size(80, 36);
            this.BrowseBtn.TabIndex = 2;
            this.BrowseBtn.Text = "Browse";
            this.BrowseBtn.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.BrowseBtn.UseAccentColor = false;
            this.BrowseBtn.UseVisualStyleBackColor = true;
            this.BrowseBtn.Click += new System.EventHandler(this.BrowseBtn_Click);
            // 
            // CompressionTypeComboBox
            // 
            this.CompressionTypeComboBox.AutoResize = false;
            this.CompressionTypeComboBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.CompressionTypeComboBox.Depth = 0;
            this.CompressionTypeComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CompressionTypeComboBox.DropDownHeight = 174;
            this.CompressionTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CompressionTypeComboBox.DropDownWidth = 121;
            this.CompressionTypeComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.CompressionTypeComboBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CompressionTypeComboBox.FormattingEnabled = true;
            this.CompressionTypeComboBox.Hint = "Default Backup Compression Format";
            this.CompressionTypeComboBox.IntegralHeight = false;
            this.CompressionTypeComboBox.ItemHeight = 43;
            this.CompressionTypeComboBox.Items.AddRange(new object[] {
            "WinRAR (.rar)",
            "WinRAR (.zip) [Optimized High-Speed Zip]",
            "Windows Native (.zip)"});
            this.CompressionTypeComboBox.Location = new System.Drawing.Point(401, 297);
            this.CompressionTypeComboBox.MaxDropDownItems = 4;
            this.CompressionTypeComboBox.Name = "CompressionTypeComboBox";
            this.CompressionTypeComboBox.Size = new System.Drawing.Size(384, 49);
            this.CompressionTypeComboBox.StartIndex = 0;
            this.CompressionTypeComboBox.TabIndex = 4;
            // 
            // compressionLabel
            // 
            this.compressionLabel.AutoSize = true;
            this.compressionLabel.Depth = 0;
            this.compressionLabel.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.compressionLabel.Location = new System.Drawing.Point(23, 312);
            this.compressionLabel.Name = "compressionLabel";
            this.compressionLabel.Size = new System.Drawing.Size(133, 19);
            this.compressionLabel.TabIndex = 5;
            this.compressionLabel.Text = "Compression Type";
            // 
            // materialLabel1
            // 
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(23, 9);
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(676, 86);
            this.materialLabel1.TabIndex = 6;
            this.materialLabel1.Text = resources.GetString("materialLabel1.Text");
            // 
            // materialLabel2
            // 
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.Location = new System.Drawing.Point(23, 195);
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(645, 86);
            this.materialLabel2.TabIndex = 7;
            this.materialLabel2.Text = resources.GetString("materialLabel2.Text");
            // 
            // SettingsContentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.ServerPathTextBox);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.compressionLabel);
            this.Controls.Add(this.CompressionTypeComboBox);
            this.Controls.Add(this.BrowseBtn);
            this.Name = "SettingsContentControl";
            this.Size = new System.Drawing.Size(790, 364);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ReaLTaiizor.Controls.MaterialTextBoxEdit ServerPathTextBox;
        private ReaLTaiizor.Controls.MaterialButton BrowseBtn;
        private ReaLTaiizor.Controls.MaterialComboBox CompressionTypeComboBox;
        private ReaLTaiizor.Controls.MaterialLabel compressionLabel;
        private ReaLTaiizor.Controls.MaterialLabel materialLabel1;
        private ReaLTaiizor.Controls.MaterialLabel materialLabel2;
    }
}
