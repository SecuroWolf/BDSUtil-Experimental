using BDSUtil.Models.AutoMod;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Forms;
using System;
using System.Windows.Forms;

namespace BDSUtil.Modals.Content_Controls.AutoMod
{
    public partial class EnforcementProfile : MaterialForm
    {
        private readonly string _xuid;

        public EnforcementProfile(string xuid)
        {
            InitializeComponent();
            _xuid = xuid;
        }

        private void EP_ViewMsgBTN_Click(object sender, EventArgs e)
        {
            if (EP_HistoryList.SelectedIndices.Count == 0)
            {
                MaterialMessageBox.Show(this, "Please select an infraction from the list first.", "No Selection", MessageBoxButtons.OK, false, MaterialFlexibleForm.ButtonsPosition.Center);
                return;
            }

            int index = EP_HistoryList.SelectedIndices[0];
            PlayerEnforcementProfile profile = ProfileManager.LoadProfile(_xuid);

            if (index >= 0 && index < profile.History.Count)
            {
                string context = profile.History[index].MessageContext;
                string action = profile.History[index].ActionTaken;

                string messageToShow = string.IsNullOrWhiteSpace(context)
                    ? "No chat context was recorded for this specific infraction."
                    : context;

                MaterialMessageBox.Show(this, messageToShow, $"Context: {action}", MessageBoxButtons.OK, false, MaterialFlexibleForm.ButtonsPosition.Center);
            }
        }

        private void EnforcementProfile_Load(object sender, EventArgs e)
        {
            LoadProfileData();
        }

        private void LoadProfileData()
        {
            PlayerEnforcementProfile profile = ProfileManager.LoadProfile(_xuid);

            this.Text = $"Enforcement Profile - {profile.Gamertag}";
            EP_GamertagLBL.Text = profile.Gamertag;
            EP_XuidLBL.Text = $"XUID: {profile.Xuid}";

            string expiry = profile.SanctionExpiration.HasValue
                ? profile.SanctionExpiration.Value.ToString("MM/dd/yy HH:mm")
                : "Indefinite";

            EP_StatusLBL.Text = profile.ActiveSanctionType.ToUpper();
            EP_ExpiryLBL.Text = profile.ActiveSanctionType.ToLower() != "inactive"
                ? $"Expires: {expiry}"
                : "No active restrictions.";

            EP_InfractionsLBL.Text = $"Enforcement Entries: {profile.History.Count}";

            EP_HistoryList.Items.Clear();
            foreach (var record in profile.History)
            {
                var item = new ListViewItem(record.Date.ToString("MM/dd/yy HH:mm"));
                item.SubItems.Add(record.ActionTaken);
                item.SubItems.Add(record.Reason);
                EP_HistoryList.Items.Add(item);
            }
        }

        private void EP_CloseBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}