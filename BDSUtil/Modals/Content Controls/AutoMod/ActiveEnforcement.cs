using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BDSUtil.Models.AutoMod;
using ReaLTaiizor.Controls;

namespace BDSUtil.Modals.Content_Controls.AutoMod
{
    public partial class ActiveEnforcement : UserControl
    {
        private List<PlayerEnforcementProfile> _loadedProfiles = new List<PlayerEnforcementProfile>();

        public ActiveEnforcement()
        {
            InitializeComponent();

            // Fire reliably the moment the control is actually displayed to the user
            this.VisibleChanged += (s, e) =>
            {
                if (this.Visible) RefreshProfileList();
            };

            if (AM_OEP_BTN != null) AM_OEP_BTN.Click += AM_OEP_BTN_Click;
            if (AM_RAE_BTN != null) AM_RAE_BTN.Click += AM_RAE_BTN_Click;
            if (AM_CS_BTN != null) AM_CS_BTN.Click += AM_CS_BTN_Click;
        }

        private bool _isUpdatingChecks = false;

        public void RefreshProfileList()
        {
            if (AM_EnforcementHistory == null) return;

            this.BeginInvoke((MethodInvoker)delegate
            {
                AM_EnforcementHistory.Items.Clear();
                AM_EnforcementHistory.Controls.Clear();

                _loadedProfiles = ProfileManager.GetAllProfiles();

                this.SuspendLayout();
                AM_EnforcementHistory.SuspendLayout();

                try
                {
                    foreach (PlayerEnforcementProfile profile in _loadedProfiles)
                    {
                        AM_EnforcementHistory.Items.Add(profile.GetFormattedListEntry(), false);
                    }

                    // Hook the single-selection logic directly into the generated checkboxes
                    foreach (object item in AM_EnforcementHistory.Items)
                    {
                        if (item is MaterialCheckBox chk)
                        {
                            chk.CheckedChanged += (s, ev) =>
                            {
                                if (_isUpdatingChecks || !chk.Checked) return;

                                _isUpdatingChecks = true;
                                foreach (object otherItem in AM_EnforcementHistory.Items)
                                {
                                    if (otherItem is MaterialCheckBox otherChk && otherChk != chk)
                                    {
                                        otherChk.Checked = false;
                                    }
                                }
                                _isUpdatingChecks = false;
                            };
                        }
                    }
                }
                finally
                {
                    AM_EnforcementHistory.ResumeLayout(true);
                    this.ResumeLayout(true);
                }
            });
        }

        private string GetSelectedXuid()
        {
            if (AM_EnforcementHistory == null) return null;

            int index = 0;

            // Iterate over Items (not Controls) because .Add(string, bool) stores the MaterialCheckBox there
            foreach (object item in AM_EnforcementHistory.Items)
            {
                if (item is MaterialCheckBox chk && chk.Checked)
                {
                    if (index < _loadedProfiles.Count) return _loadedProfiles[index].Xuid;
                }
                index++;
            }
            return null;
        }

        // --- BUTTON EVENT HANDLERS ---

        private void AM_OEP_BTN_Click(object sender, EventArgs e)
        {
            string xuid = GetSelectedXuid();
            if (string.IsNullOrEmpty(xuid))
            {
                MessageBox.Show("Please check a user profile from the list first.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            EnforcementProfile profileForm = new EnforcementProfile(xuid);
            profileForm.ShowDialog();

            RefreshProfileList();
        }

        private void AM_RAE_BTN_Click(object sender, EventArgs e)
        {
            string xuid = GetSelectedXuid();
            if (string.IsNullOrEmpty(xuid)) return;

            PlayerEnforcementProfile profile = ProfileManager.LoadProfile(xuid);

            if (profile.ActiveSanctionType != "inactive")
            {
                profile.ActiveSanctionType = "inactive";
                profile.SanctionExpiration = null;

                profile.History.Add(new EnforcementRecord
                {
                    Date = DateTime.Now,
                    ActionTaken = "Admin Pardon",
                    Reason = "Active enforcement manually cleared viadashboard."
                });

                ProfileManager.SaveProfile(profile);
                RefreshProfileList();
            }
        }

        private void AM_CS_BTN_Click(object sender, EventArgs e)
        {
            string xuid = GetSelectedXuid();
            if (string.IsNullOrEmpty(xuid)) return;

            DialogResult result = MessageBox.Show(
                "Are you sure you want to grant this user a Clean Slate? This will permanently delete their entire enforcement history profile from the disk.",
                "Confirm Clean Slate",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                ProfileManager.DeleteProfile(xuid);
                RefreshProfileList();
            }
        }
    }
}