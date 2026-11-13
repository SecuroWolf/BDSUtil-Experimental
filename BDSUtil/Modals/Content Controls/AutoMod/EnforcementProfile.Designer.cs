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
            this.EP_IdentityCard = new Material3.WinForms.Controls.MaterialCard();
            this.EP_IconLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.EP_GamertagLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.EP_XuidLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.EP_StatusCard = new Material3.WinForms.Controls.MaterialCard();
            this.EP_StatusHeaderLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.EP_StatusLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.EP_ExpiryLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.EP_InfractionsLBL = new Material3.WinForms.Controls.MaterialLabel();
            this.EP_HistoryList = new BDSUtil.UI.MaterialListView();
            this.colDate = new System.Windows.Forms.ColumnHeader();
            this.colAction = new System.Windows.Forms.ColumnHeader();
            this.colReason = new System.Windows.Forms.ColumnHeader();
            this.EP_CloseBTN = new Material3.WinForms.Controls.MaterialButton();
            this.EP_ViewMsgBTN = new Material3.WinForms.Controls.MaterialButton();
            this.materialDivider1 = new Material3.WinForms.Controls.MaterialDivider();
            this.EP_IdentityCard.SuspendLayout();
            this.EP_StatusCard.SuspendLayout();
            this.SuspendLayout();
            // 
            // EP_IdentityCard
            // 
            this.EP_IdentityCard.Controls.Add(this.EP_IconLBL);
            this.EP_IdentityCard.Controls.Add(this.EP_GamertagLBL);
            this.EP_IdentityCard.Controls.Add(this.EP_XuidLBL);
            this.EP_IdentityCard.Location = new System.Drawing.Point(24, 68);
            this.EP_IdentityCard.Margin = new System.Windows.Forms.Padding(14);
            this.EP_IdentityCard.Name = "EP_IdentityCard";
            this.EP_IdentityCard.Padding = new System.Windows.Forms.Padding(14);
            this.EP_IdentityCard.Size = new System.Drawing.Size(360, 120);
            this.EP_IdentityCard.TabIndex = 0;
            this.EP_IdentityCard.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            // 
            // EP_IconLBL
            // 
            this.EP_IconLBL.AutoSize = true;
            this.EP_IconLBL.BackColor = System.Drawing.Color.Transparent;
            this.EP_IconLBL.Font = Material3.WinForms.Typography.TypeScale.HeadlineMedium;
            this.EP_IconLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.Primary;
            this.EP_IconLBL.Location = new System.Drawing.Point(17, 26);
            this.EP_IconLBL.Name = "EP_IconLBL";
            this.EP_IconLBL.Size = new System.Drawing.Size(36, 36);
            this.EP_IconLBL.TabIndex = 3;
            this.EP_IconLBL.Text = "ðŸ‘¤";
            // 
            // EP_GamertagLBL
            // 
            this.EP_GamertagLBL.AutoSize = true;
            this.EP_GamertagLBL.BackColor = System.Drawing.Color.Transparent;
            this.EP_GamertagLBL.Font = Material3.WinForms.Typography.TypeScale.TitleMedium;
            this.EP_GamertagLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.EP_GamertagLBL.Location = new System.Drawing.Point(75, 24);
            this.EP_GamertagLBL.Name = "EP_GamertagLBL";
            this.EP_GamertagLBL.Size = new System.Drawing.Size(80, 24);
            this.EP_GamertagLBL.TabIndex = 0;
            this.EP_GamertagLBL.Text = "Gamertag";
            // 
            // EP_XuidLBL
            // 
            this.EP_XuidLBL.AutoSize = true;
            this.EP_XuidLBL.BackColor = System.Drawing.Color.Transparent;
            this.EP_XuidLBL.Font = Material3.WinForms.Typography.TypeScale.BodyMedium;
            this.EP_XuidLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurfaceVariant;
            this.EP_XuidLBL.Location = new System.Drawing.Point(75, 60);
            this.EP_XuidLBL.Name = "EP_XuidLBL";
            this.EP_XuidLBL.Size = new System.Drawing.Size(44, 20);
            this.EP_XuidLBL.TabIndex = 1;
            this.EP_XuidLBL.Text = "XUID: ";
            // 
            // EP_StatusCard
            // 
            this.EP_StatusCard.Controls.Add(this.EP_StatusHeaderLBL);
            this.EP_StatusCard.Controls.Add(this.EP_StatusLBL);
            this.EP_StatusCard.Controls.Add(this.EP_ExpiryLBL);
            this.EP_StatusCard.Controls.Add(this.EP_InfractionsLBL);
            this.EP_StatusCard.Location = new System.Drawing.Point(395, 68);
            this.EP_StatusCard.Margin = new System.Windows.Forms.Padding(14);
            this.EP_StatusCard.Name = "EP_StatusCard";
            this.EP_StatusCard.Padding = new System.Windows.Forms.Padding(14);
            this.EP_StatusCard.Size = new System.Drawing.Size(360, 120);
            this.EP_StatusCard.TabIndex = 4;
            this.EP_StatusCard.Variant = Material3.WinForms.Controls.MaterialCardVariant.Elevated;
            // 
            // EP_StatusHeaderLBL
            // 
            this.EP_StatusHeaderLBL.AutoSize = true;
            this.EP_StatusHeaderLBL.BackColor = System.Drawing.Color.Transparent;
            this.EP_StatusHeaderLBL.Font = Material3.WinForms.Typography.TypeScale.LabelLarge;
            this.EP_StatusHeaderLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurfaceVariant;
            this.EP_StatusHeaderLBL.Location = new System.Drawing.Point(17, 14);
            this.EP_StatusHeaderLBL.Name = "EP_StatusHeaderLBL";
            this.EP_StatusHeaderLBL.Size = new System.Drawing.Size(119, 20);
            this.EP_StatusHeaderLBL.TabIndex = 4;
            this.EP_StatusHeaderLBL.Text = "Current Sanction";
            // 
            // EP_StatusLBL
            // 
            this.EP_StatusLBL.AutoSize = true;
            this.EP_StatusLBL.BackColor = System.Drawing.Color.Transparent;
            this.EP_StatusLBL.Font = Material3.WinForms.Typography.TypeScale.TitleMedium;
            this.EP_StatusLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurface;
            this.EP_StatusLBL.Location = new System.Drawing.Point(17, 36);
            this.EP_StatusLBL.Name = "EP_StatusLBL";
            this.EP_StatusLBL.Size = new System.Drawing.Size(75, 24);
            this.EP_StatusLBL.TabIndex = 2;
            this.EP_StatusLBL.Text = "INACTIVE";
            // 
            // EP_ExpiryLBL
            // 
            this.EP_ExpiryLBL.AutoSize = true;
            this.EP_ExpiryLBL.BackColor = System.Drawing.Color.Transparent;
            this.EP_ExpiryLBL.Font = Material3.WinForms.Typography.TypeScale.BodyMedium;
            this.EP_ExpiryLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurfaceVariant;
            this.EP_ExpiryLBL.Location = new System.Drawing.Point(17, 62);
            this.EP_ExpiryLBL.Name = "EP_ExpiryLBL";
            this.EP_ExpiryLBL.Size = new System.Drawing.Size(153, 20);
            this.EP_ExpiryLBL.TabIndex = 3;
            this.EP_ExpiryLBL.Text = "No active restrictions.";
            // 
            // EP_InfractionsLBL
            // 
            this.EP_InfractionsLBL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EP_InfractionsLBL.AutoSize = true;
            this.EP_InfractionsLBL.BackColor = System.Drawing.Color.Transparent;
            this.EP_InfractionsLBL.Font = Material3.WinForms.Typography.TypeScale.BodyMedium;
            this.EP_InfractionsLBL.ForeColor = Material3.WinForms.Theming.MaterialColors.OnSurfaceVariant;
            this.EP_InfractionsLBL.Location = new System.Drawing.Point(17, 89);
            this.EP_InfractionsLBL.Name = "EP_InfractionsLBL";
            this.EP_InfractionsLBL.Size = new System.Drawing.Size(160, 20);
            this.EP_InfractionsLBL.TabIndex = 5;
            this.EP_InfractionsLBL.Text = "Enforcement Entries: 0";
            this.EP_InfractionsLBL.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // EP_HistoryList
            // 
            this.EP_HistoryList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.EP_HistoryList.AutoSizeTable = false;
            this.EP_HistoryList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.EP_HistoryList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colDate,
            this.colAction,
            this.colReason});
            this.EP_HistoryList.FullRowSelect = true;
            this.EP_HistoryList.HideSelection = false;
            this.EP_HistoryList.Location = new System.Drawing.Point(24, 214);
            this.EP_HistoryList.MinimumSize = new System.Drawing.Size(200, 100);
            this.EP_HistoryList.Name = "EP_HistoryList";
            this.EP_HistoryList.OwnerDraw = true;
            this.EP_HistoryList.Size = new System.Drawing.Size(731, 314);
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
            this.EP_CloseBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.EP_CloseBTN.AutoSize = false;
            this.EP_CloseBTN.Location = new System.Drawing.Point(659, 540);
            this.EP_CloseBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.EP_CloseBTN.Name = "EP_CloseBTN";
            this.EP_CloseBTN.Size = new System.Drawing.Size(96, 38);
            this.EP_CloseBTN.TabIndex = 2;
            this.EP_CloseBTN.Text = "CLOSE";
            this.EP_CloseBTN.Tone = Material3.WinForms.Controls.MaterialTone.Neutral;
            this.EP_CloseBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Outlined;
            this.EP_CloseBTN.Click += new System.EventHandler(this.EP_CloseBTN_Click);
            // 
            // EP_ViewMsgBTN
            // 
            this.EP_ViewMsgBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EP_ViewMsgBTN.AutoSize = false;
            this.EP_ViewMsgBTN.Location = new System.Drawing.Point(24, 540);
            this.EP_ViewMsgBTN.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.EP_ViewMsgBTN.Name = "EP_ViewMsgBTN";
            this.EP_ViewMsgBTN.Size = new System.Drawing.Size(148, 38);
            this.EP_ViewMsgBTN.TabIndex = 4;
            this.EP_ViewMsgBTN.Text = "VIEW MESSAGE";
            this.EP_ViewMsgBTN.Tone = Material3.WinForms.Controls.MaterialTone.Accent;
            this.EP_ViewMsgBTN.Variant = Material3.WinForms.Controls.MaterialButtonVariant.Filled;
            this.EP_ViewMsgBTN.Click += new System.EventHandler(this.EP_ViewMsgBTN_Click);
            // 
            // materialDivider1
            // 
            this.materialDivider1.BackColor = System.Drawing.Color.Transparent;
            this.materialDivider1.Location = new System.Drawing.Point(24, 200);
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(731, 6);
            this.materialDivider1.TabIndex = 3;
            this.materialDivider1.TabStop = false;
            // 
            // EnforcementProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(780, 595);
            this.Controls.Add(this.EP_ViewMsgBTN);
            this.Controls.Add(this.EP_CloseBTN);
            this.Controls.Add(this.EP_HistoryList);
            this.Controls.Add(this.materialDivider1);
            this.Controls.Add(this.EP_StatusCard);
            this.Controls.Add(this.EP_IdentityCard);
            this.MaximizeBox = false;
            this.Name = "EnforcementProfile";
            this.Sizable = false;
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

        private Material3.WinForms.Controls.MaterialCard EP_IdentityCard;
        private Material3.WinForms.Controls.MaterialLabel EP_IconLBL;
        private Material3.WinForms.Controls.MaterialLabel EP_GamertagLBL;
        private Material3.WinForms.Controls.MaterialLabel EP_XuidLBL;

        private Material3.WinForms.Controls.MaterialCard EP_StatusCard;
        private Material3.WinForms.Controls.MaterialLabel EP_StatusHeaderLBL;
        private Material3.WinForms.Controls.MaterialLabel EP_StatusLBL;
        private Material3.WinForms.Controls.MaterialLabel EP_ExpiryLBL;
        private Material3.WinForms.Controls.MaterialLabel EP_InfractionsLBL;

        private BDSUtil.UI.MaterialListView EP_HistoryList;
        private System.Windows.Forms.ColumnHeader colDate;
        private System.Windows.Forms.ColumnHeader colAction;
        private System.Windows.Forms.ColumnHeader colReason;

        private Material3.WinForms.Controls.MaterialDivider materialDivider1;
        private Material3.WinForms.Controls.MaterialButton EP_CloseBTN;
        private Material3.WinForms.Controls.MaterialButton EP_ViewMsgBTN;
    }
}