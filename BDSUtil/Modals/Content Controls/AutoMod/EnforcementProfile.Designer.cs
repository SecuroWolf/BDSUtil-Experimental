namespace BDSUtil.Modals.Content_Controls.AutoMod
{
    partial class EnforcementProfile
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
            this.EP_IdentityCard = new ReaLTaiizor.Controls.MaterialCard();
            this.EP_IconLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.EP_GamertagLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.EP_XuidLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.EP_StatusCard = new ReaLTaiizor.Controls.MaterialCard();
            this.EP_StatusHeaderLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.EP_StatusLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.EP_ExpiryLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.EP_InfractionsLBL = new ReaLTaiizor.Controls.MaterialLabel();
            this.EP_HistoryList = new ReaLTaiizor.Controls.MaterialListView();
            this.colDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colReason = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.EP_CloseBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.EP_ViewMsgBTN = new ReaLTaiizor.Controls.MaterialButton();
            this.materialDivider1 = new ReaLTaiizor.Controls.MaterialDivider();
            this.EP_IdentityCard.SuspendLayout();
            this.EP_StatusCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // EP_IdentityCard
            // 
            this.EP_IdentityCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.EP_IdentityCard.Controls.Add(this.EP_IconLBL);
            this.EP_IdentityCard.Controls.Add(this.EP_GamertagLBL);
            this.EP_IdentityCard.Controls.Add(this.EP_XuidLBL);
            this.EP_IdentityCard.Depth = 0;
            this.EP_IdentityCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.EP_IdentityCard.Location = new System.Drawing.Point(24, 88);
            this.EP_IdentityCard.Margin = new System.Windows.Forms.Padding(14);
            this.EP_IdentityCard.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.EP_IdentityCard.Name = "EP_IdentityCard";
            this.EP_IdentityCard.Padding = new System.Windows.Forms.Padding(14);
            this.EP_IdentityCard.Size = new System.Drawing.Size(360, 115);
            this.EP_IdentityCard.TabIndex = 0;
            // 
            // EP_IconLBL
            // 
            this.EP_IconLBL.AutoSize = true;
            this.EP_IconLBL.Depth = 0;
            this.EP_IconLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.EP_IconLBL.Location = new System.Drawing.Point(17, 30);
            this.EP_IconLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.EP_IconLBL.Name = "EP_IconLBL";
            this.EP_IconLBL.Size = new System.Drawing.Size(17, 19);
            this.EP_IconLBL.TabIndex = 3;
            this.EP_IconLBL.Text = "👤";
            // 
            // EP_GamertagLBL
            // 
            this.EP_GamertagLBL.AutoSize = true;
            this.EP_GamertagLBL.Depth = 0;
            this.EP_GamertagLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.EP_GamertagLBL.Location = new System.Drawing.Point(75, 26);
            this.EP_GamertagLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.EP_GamertagLBL.Name = "EP_GamertagLBL";
            this.EP_GamertagLBL.Size = new System.Drawing.Size(71, 19);
            this.EP_GamertagLBL.TabIndex = 0;
            this.EP_GamertagLBL.Text = "Gamertag";
            // 
            // EP_XuidLBL
            // 
            this.EP_XuidLBL.AutoSize = true;
            this.EP_XuidLBL.Depth = 0;
            this.EP_XuidLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.EP_XuidLBL.Location = new System.Drawing.Point(78, 62);
            this.EP_XuidLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.EP_XuidLBL.Name = "EP_XuidLBL";
            this.EP_XuidLBL.Size = new System.Drawing.Size(44, 19);
            this.EP_XuidLBL.TabIndex = 1;
            this.EP_XuidLBL.Text = "XUID: ";
            // 
            // EP_StatusCard
            // 
            this.EP_StatusCard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.EP_StatusCard.Controls.Add(this.EP_StatusHeaderLBL);
            this.EP_StatusCard.Controls.Add(this.EP_StatusLBL);
            this.EP_StatusCard.Controls.Add(this.EP_ExpiryLBL);
            this.EP_StatusCard.Controls.Add(this.EP_InfractionsLBL);
            this.EP_StatusCard.Depth = 0;
            this.EP_StatusCard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.EP_StatusCard.Location = new System.Drawing.Point(395, 88);
            this.EP_StatusCard.Margin = new System.Windows.Forms.Padding(14);
            this.EP_StatusCard.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.EP_StatusCard.Name = "EP_StatusCard";
            this.EP_StatusCard.Padding = new System.Windows.Forms.Padding(14);
            this.EP_StatusCard.Size = new System.Drawing.Size(360, 115);
            this.EP_StatusCard.TabIndex = 4;
            // 
            // EP_StatusHeaderLBL
            // 
            this.EP_StatusHeaderLBL.AutoSize = true;
            this.EP_StatusHeaderLBL.Depth = 0;
            this.EP_StatusHeaderLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.EP_StatusHeaderLBL.Location = new System.Drawing.Point(17, 14);
            this.EP_StatusHeaderLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.EP_StatusHeaderLBL.Name = "EP_StatusHeaderLBL";
            this.EP_StatusHeaderLBL.Size = new System.Drawing.Size(119, 19);
            this.EP_StatusHeaderLBL.TabIndex = 4;
            this.EP_StatusHeaderLBL.Text = "Current Sanction";
            // 
            // EP_StatusLBL
            // 
            this.EP_StatusLBL.AutoSize = true;
            this.EP_StatusLBL.Depth = 0;
            this.EP_StatusLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.EP_StatusLBL.Location = new System.Drawing.Point(17, 33);
            this.EP_StatusLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.EP_StatusLBL.Name = "EP_StatusLBL";
            this.EP_StatusLBL.Size = new System.Drawing.Size(69, 19);
            this.EP_StatusLBL.TabIndex = 2;
            this.EP_StatusLBL.Text = "INACTIVE";
            // 
            // EP_ExpiryLBL
            // 
            this.EP_ExpiryLBL.AutoSize = true;
            this.EP_ExpiryLBL.Depth = 0;
            this.EP_ExpiryLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.EP_ExpiryLBL.Location = new System.Drawing.Point(17, 52);
            this.EP_ExpiryLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.EP_ExpiryLBL.Name = "EP_ExpiryLBL";
            this.EP_ExpiryLBL.Size = new System.Drawing.Size(153, 19);
            this.EP_ExpiryLBL.TabIndex = 3;
            this.EP_ExpiryLBL.Text = "No active restrictions.";
            // 
            // EP_InfractionsLBL
            // 
            this.EP_InfractionsLBL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EP_InfractionsLBL.AutoSize = true;
            this.EP_InfractionsLBL.Depth = 0;
            this.EP_InfractionsLBL.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.EP_InfractionsLBL.Location = new System.Drawing.Point(17, 89);
            this.EP_InfractionsLBL.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.EP_InfractionsLBL.Name = "EP_InfractionsLBL";
            this.EP_InfractionsLBL.Size = new System.Drawing.Size(160, 19);
            this.EP_InfractionsLBL.TabIndex = 5;
            this.EP_InfractionsLBL.Text = "Enforcement Entries: 0";
            this.EP_InfractionsLBL.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // EP_HistoryList
            // 
            this.EP_HistoryList.AutoSizeTable = false;
            this.EP_HistoryList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.EP_HistoryList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EP_HistoryList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDate,
            this.colAction,
            this.colReason});
            this.EP_HistoryList.Depth = 0;
            this.EP_HistoryList.FullRowSelect = true;
            this.EP_HistoryList.HideSelection = false;
            this.EP_HistoryList.Location = new System.Drawing.Point(24, 227);
            this.EP_HistoryList.MinimumSize = new System.Drawing.Size(200, 100);
            this.EP_HistoryList.MouseLocation = new System.Drawing.Point(-1, -1);
            this.EP_HistoryList.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.OUT;
            this.EP_HistoryList.Name = "EP_HistoryList";
            this.EP_HistoryList.OwnerDraw = true;
            this.EP_HistoryList.Size = new System.Drawing.Size(731, 304);
            this.EP_HistoryList.TabIndex = 1;
            this.EP_HistoryList.UseCompatibleStateImageBehavior = false;
            this.EP_HistoryList.View = System.Windows.Forms.View.Details;
            // 
            // colDate
            // 
            this.colDate.Text = "Date / Time";
            this.colDate.Width = 140;
            // 
            // colAction
            // 
            this.colAction.Text = "Action Taken";
            this.colAction.Width = 150;
            // 
            // colReason
            // 
            this.colReason.Text = "Reason for Enforcement";
            this.colReason.Width = 441;
            // 
            // EP_CloseBTN
            // 
            this.EP_CloseBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EP_CloseBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.EP_CloseBTN.Depth = 0;
            this.EP_CloseBTN.HighEmphasis = true;
            this.EP_CloseBTN.Icon = null;
            this.EP_CloseBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.EP_CloseBTN.Location = new System.Drawing.Point(689, 540);
            this.EP_CloseBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.EP_CloseBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.EP_CloseBTN.Name = "EP_CloseBTN";
            this.EP_CloseBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.EP_CloseBTN.Size = new System.Drawing.Size(66, 36);
            this.EP_CloseBTN.TabIndex = 2;
            this.EP_CloseBTN.Text = "CLOSE";
            this.EP_CloseBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.EP_CloseBTN.UseAccentColor = false;
            this.EP_CloseBTN.UseVisualStyleBackColor = true;
            this.EP_CloseBTN.Click += new System.EventHandler(this.EP_CloseBTN_Click);
            // 
            // EP_ViewMsgBTN
            // 
            this.EP_ViewMsgBTN.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EP_ViewMsgBTN.Density = ReaLTaiizor.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.EP_ViewMsgBTN.Depth = 0;
            this.EP_ViewMsgBTN.HighEmphasis = true;
            this.EP_ViewMsgBTN.Icon = null;
            this.EP_ViewMsgBTN.IconType = ReaLTaiizor.Controls.MaterialButton.MaterialIconType.Rebase;
            this.EP_ViewMsgBTN.Location = new System.Drawing.Point(24, 540);
            this.EP_ViewMsgBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.EP_ViewMsgBTN.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.EP_ViewMsgBTN.Name = "EP_ViewMsgBTN";
            this.EP_ViewMsgBTN.NoAccentTextColor = System.Drawing.Color.Empty;
            this.EP_ViewMsgBTN.Size = new System.Drawing.Size(127, 36);
            this.EP_ViewMsgBTN.TabIndex = 4;
            this.EP_ViewMsgBTN.Text = "VIEW MESSAGE";
            this.EP_ViewMsgBTN.Type = ReaLTaiizor.Controls.MaterialButton.MaterialButtonType.Contained;
            this.EP_ViewMsgBTN.UseAccentColor = false;
            this.EP_ViewMsgBTN.UseVisualStyleBackColor = true;
            this.EP_ViewMsgBTN.Click += new System.EventHandler(this.EP_ViewMsgBTN_Click);
            // 
            // materialDivider1
            // 
            this.materialDivider1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider1.Depth = 0;
            this.materialDivider1.Location = new System.Drawing.Point(24, 215);
            this.materialDivider1.MouseState = ReaLTaiizor.Helper.MaterialDrawHelper.MaterialMouseState.HOVER;
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(731, 3);
            this.materialDivider1.TabIndex = 3;
            this.materialDivider1.Text = "materialDivider1";
            // 
            // EnforcementProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(780, 595);
            this.Controls.Add(this.EP_ViewMsgBTN);
            this.Controls.Add(this.EP_CloseBTN);
            this.Controls.Add(this.EP_HistoryList);
            this.Controls.Add(this.materialDivider1);
            this.Controls.Add(this.EP_StatusCard);
            this.Controls.Add(this.EP_IdentityCard);
            this.Name = "EnforcementProfile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enforcement Profile";
            this.Load += new System.EventHandler(this.EnforcementProfile_Load);
            this.EP_IdentityCard.ResumeLayout(false);
            this.EP_IdentityCard.PerformLayout();
            this.EP_StatusCard.ResumeLayout(false);
            this.EP_StatusCard.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ReaLTaiizor.Controls.MaterialCard EP_IdentityCard;
        private ReaLTaiizor.Controls.MaterialLabel EP_IconLBL;
        private ReaLTaiizor.Controls.MaterialLabel EP_GamertagLBL;
        private ReaLTaiizor.Controls.MaterialLabel EP_XuidLBL;

        private ReaLTaiizor.Controls.MaterialCard EP_StatusCard;
        private ReaLTaiizor.Controls.MaterialLabel EP_StatusHeaderLBL;
        private ReaLTaiizor.Controls.MaterialLabel EP_StatusLBL;
        private ReaLTaiizor.Controls.MaterialLabel EP_ExpiryLBL;
        private ReaLTaiizor.Controls.MaterialLabel EP_InfractionsLBL;

        private ReaLTaiizor.Controls.MaterialListView EP_HistoryList;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colAction;
        private System.Windows.Forms.ColumnHeader colReason;

        private ReaLTaiizor.Controls.MaterialDivider materialDivider1;
        private ReaLTaiizor.Controls.MaterialButton EP_CloseBTN;
        private ReaLTaiizor.Controls.MaterialButton EP_ViewMsgBTN;
    }
}