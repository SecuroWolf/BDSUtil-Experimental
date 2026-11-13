using BDSUtil.Helpers;
using BDSUtil.UI;
using Material3.WinForms.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BDSUtil.Modals.Content_Controls.Packs
{
    public partial class AddonManager : Material3Form
    {
        private readonly string _activeWorldName = "Bedrock level";

        public AddonManager(MainWindow mainWindow)
        {
            InitializeComponent();
        }

        private void AddonManager_Load(object sender, EventArgs e)
        {
            PM_TabSelector?.RebuildTabs();
            var utilPack = AppSettings.Instance.UtilPack;
            CF_OpCommandsSwitch.Checked = utilPack.OpCommands;
            CF_PvpAwardsSwitch.Checked = utilPack.PvpAwards;
            CF_AntiGriefSwitch.Checked = utilPack.AntiGrief;
            CF_Placeholder1Switch.Checked = utilPack.Economy;
            CF_Placeholder2Switch.Checked = utilPack.Optimization;

            CF_OpCommandsSwitch.CheckedChanged += CustomFeatureSwitch_CheckedChanged;
            CF_PvpAwardsSwitch.CheckedChanged += CustomFeatureSwitch_CheckedChanged;
            CF_AntiGriefSwitch.CheckedChanged += CustomFeatureSwitch_CheckedChanged;
            CF_Placeholder1Switch.CheckedChanged += CustomFeatureSwitch_CheckedChanged;
            CF_Placeholder2Switch.CheckedChanged += CustomFeatureSwitch_CheckedChanged;

            // Hook the custom renderer for Minecraft color codes safely
            RP_ListView.DrawSubItem += PackListView_DrawSubItem;
            BP_ListView.DrawSubItem += PackListView_DrawSubItem;

            PopulatePackLists();
        }

        private void PopulatePackLists()
        {
            RP_ListView.Items.Clear();
            BP_ListView.Items.Clear();

            string serverRoot = AppSettings.Instance.General.ServerExecutablePath;

            List<PackManifestInfo> resourcePacks = PackManager.GetAvailablePacks(serverRoot, _activeWorldName, false);
            foreach (var pack in resourcePacks)
            {
                ListViewItem item = new ListViewItem(string.Empty); // Column 0 text cleared
                item.SubItems.Add(string.Empty);                    // Column 1 text cleared
                item.Tag = pack;
                RP_ListView.Items.Add(item);
            }

            List<PackManifestInfo> behaviorPacks = PackManager.GetAvailablePacks(serverRoot, _activeWorldName, true);
            foreach (var pack in behaviorPacks)
            {
                ListViewItem item = new ListViewItem(string.Empty); // Column 0 text cleared
                item.SubItems.Add(string.Empty);                    // Column 1 text cleared
                item.Tag = pack;
                BP_ListView.Items.Add(item);
            }
        }

        private void CustomFeatureSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (sender is MaterialSwitch toggle)
            {
                AppSettings.Instance.Update(cfg =>
                {
                    if (toggle == CF_OpCommandsSwitch) cfg.UtilPack.OpCommands = toggle.Checked;
                    else if (toggle == CF_PvpAwardsSwitch) cfg.UtilPack.PvpAwards = toggle.Checked;
                    else if (toggle == CF_AntiGriefSwitch) cfg.UtilPack.AntiGrief = toggle.Checked;
                    else if (toggle == CF_Placeholder1Switch) cfg.UtilPack.Economy = toggle.Checked;
                    else if (toggle == CF_Placeholder2Switch) cfg.UtilPack.Optimization = toggle.Checked;
                });
            }
        }

        private void AddonManager_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    string ext = System.IO.Path.GetExtension(file).ToLower();
                    if (ext == ".mcpack" || ext == ".mcaddon" || ext == ".zip")
                    {
                        e.Effect = DragDropEffects.Copy;
                        return;
                    }
                }
            }
            e.Effect = DragDropEffects.None;
        }

        private void AddonManager_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                string ext = System.IO.Path.GetExtension(file).ToLower();
                if (ext == ".mcpack" || ext == ".mcaddon" || ext == ".zip")
                {
                    ProcessPackImport(file);
                }
            }
        }

        private void ImportBTN_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Select Minecraft Pack";
                ofd.Filter = "Minecraft Packs (*.mcpack;*.mcaddon;*.zip)|*.mcpack;*.mcaddon;*.zip|All files (*.*)|*.*";
                ofd.Multiselect = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        ProcessPackImport(file);
                    }
                }
            }
        }

        private void DeleteBTN_Click(object sender, EventArgs e)
        {
            MaterialButton clickedButton = sender as MaterialButton;
            bool isBehavior = (clickedButton == BP_DeleteBTN);
            ListView targetListView = isBehavior ? BP_ListView : RP_ListView;

            if (targetListView.SelectedItems.Count == 0) return;

            if (targetListView.SelectedItems[0].Tag is PackManifestInfo packInfo)
            {
                DialogResult confirm = MaterialDialogBox.Show(this,
                    $"Are you sure you want to permanently delete '{packInfo.Name}'? This action cannot be undone.",
                    "Confirm Deletion", MessageBoxButtons.YesNo);

                if (confirm == DialogResult.Yes)
                {
                    string serverRoot = AppSettings.Instance.General.ServerExecutablePath;
                    bool success = PackManager.DeletePack(serverRoot, _activeWorldName, packInfo.FolderName, packInfo.Uuid, isBehavior);

                    if (success)
                    {
                        PopulatePackLists();
                    }
                    else
                    {
                        MaterialDialogBox.Show(this, $"Failed to delete {packInfo.Name}.", "Error");
                    }
                }
            }
        }

        private void ProcessPackImport(string filePath)
        {
            string serverRoot = AppSettings.Instance.General.ServerExecutablePath;
            bool success = PackManager.InstallPack(serverRoot, filePath);

            if (success)
            {
                PopulatePackLists();
            }
            else
            {
                MaterialDialogBox.Show(this, $"Failed to import: {System.IO.Path.GetFileName(filePath)}", "Import Error");
            }
        }

        private void RP_EnableBTN_Click(object sender, EventArgs e)
        {
            if (RP_ListView.SelectedItems.Count == 0) return;
            if (RP_ListView.SelectedItems[0].Tag is PackManifestInfo packInfo)
            {
                // Check if already enabled
                if (packInfo.IsEnabled)
                {
                    MaterialDialogBox.Show(this, $"{packInfo.Name} is already enabled.", "Notice");
                    return;
                }

                string serverRoot = AppSettings.Instance.General.ServerExecutablePath;
                bool success = PackManager.EnablePackInWorld(serverRoot, _activeWorldName, packInfo.Uuid, packInfo.Version, false);
                if (success) PopulatePackLists();
            }
        }

        private void RP_DisableBTN_Click(object sender, EventArgs e)
        {
            if (RP_ListView.SelectedItems.Count == 0) return;
            if (RP_ListView.SelectedItems[0].Tag is PackManifestInfo packInfo)
            {
                // Check if already disabled / not enabled
                if (!packInfo.IsEnabled)
                {
                    MaterialDialogBox.Show(this, $"{packInfo.Name} is not currently enabled.", "Notice");
                    return;
                }

                string serverRoot = AppSettings.Instance.General.ServerExecutablePath;
                bool success = PackManager.DisablePackInWorld(serverRoot, _activeWorldName, packInfo.Uuid, false);
                if (success) PopulatePackLists();
            }
        }

        private void BP_EnableBTN_Click(object sender, EventArgs e)
        {
            if (BP_ListView.SelectedItems.Count == 0) return;
            if (BP_ListView.SelectedItems[0].Tag is PackManifestInfo packInfo)
            {
                // Check if already enabled
                if (packInfo.IsEnabled)
                {
                    MaterialDialogBox.Show(this, $"{packInfo.Name} is already enabled.", "Notice");
                    return;
                }

                string serverRoot = AppSettings.Instance.General.ServerExecutablePath;
                bool success = PackManager.EnablePackInWorld(serverRoot, _activeWorldName, packInfo.Uuid, packInfo.Version, true);
                if (success) PopulatePackLists();
            }
        }

        private void BP_DisableBTN_Click(object sender, EventArgs e)
        {
            if (BP_ListView.SelectedItems.Count == 0) return;
            if (BP_ListView.SelectedItems[0].Tag is PackManifestInfo packInfo)
            {
                // Check if already disabled / not enabled
                if (!packInfo.IsEnabled)
                {
                    MaterialDialogBox.Show(this, $"{packInfo.Name} is not currently enabled.", "Notice");
                    return;
                }

                string serverRoot = AppSettings.Instance.General.ServerExecutablePath;
                bool success = PackManager.DisablePackInWorld(serverRoot, _activeWorldName, packInfo.Uuid, true);
                if (success) PopulatePackLists();
            }
        }

        private void CF_AddCommandBTN_Click(object sender, EventArgs e)
        {
            MaterialDialogBox.Show(this, "The custom command logic builder will launch here.", "Feature Pending", MessageBoxButtons.OK);
        }

        private void PM_CloseBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // --- MINECRAFT COLOR RENDERER ---
        private void PackListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            PackManifestInfo pack = e.Item.Tag as PackManifestInfo;
            if (pack == null)
            {
                return;
            }

            // MaterialListView.OnDrawSubItem already fills the M3 row background, selection tint, and divider.
            TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix;
            Color defaultTextColor = e.Item.Selected ? Material3.WinForms.Theming.MaterialColors.OnSecondaryContainer : Material3.WinForms.Theming.MaterialColors.OnSurface;

            // Column 0: Status & Pack Name / Studio (Minecraft Color Parsed)
            if (e.ColumnIndex == 0)
            {
                string status = pack.IsEnabled ? "§a[ENABLED]§r " : "§f[INSTALLED]§r ";
                string fullText = status + pack.Name;

                int currentX = e.Bounds.Left + 6;
                Color currentColor = defaultTextColor;
                string[] segments = fullText.Split('§');

                for (int i = 0; i < segments.Length; i++)
                {
                    string segment = segments[i];

                    if (i == 0 && !fullText.StartsWith("§"))
                    {
                        DrawTextSegment(e.Graphics, segment, e.Item.Font, currentColor, e.Bounds, ref currentX);
                        continue;
                    }

                    if (segment.Length > 0)
                    {
                        char colorCode = segment[0];
                        currentColor = GetMinecraftColor(colorCode, defaultTextColor);
                        string textToDraw = segment.Substring(1);
                        DrawTextSegment(e.Graphics, textToDraw, e.Item.Font, currentColor, e.Bounds, ref currentX);
                    }
                }
            }
            // Column 1: Version Number (Uniform x.x.x format)
            else if (e.ColumnIndex == 1)
            {
                Rectangle versionBounds = e.Bounds;
                versionBounds.X += 6;
                TextRenderer.DrawText(e.Graphics, pack.FormattedVersion, e.Item.Font, versionBounds, defaultTextColor, flags);
            }
        }

        private void DrawTextSegment(Graphics g, string text, Font font, Color color, Rectangle bounds, ref int currentX)
        {
            if (string.IsNullOrEmpty(text)) return;

            // Use NoPadding to stitch segments together cleanly without native Windows gap margins
            TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding;

            Rectangle segmentBounds = new Rectangle(currentX, bounds.Top, bounds.Width - (currentX - bounds.Left), bounds.Height);
            TextRenderer.DrawText(g, text, font, segmentBounds, color, flags);

            // Measure text accurately without default padding to advance X coordinate seamlessly
            Size textSize = TextRenderer.MeasureText(g, text, font, new Size(int.MaxValue, bounds.Height), flags);
            currentX += textSize.Width;
        }

        private Color GetMinecraftColor(char code, Color defaultColor)
        {
            switch (char.ToLower(code))
            {
                case '0': return Color.FromArgb(0, 0, 0);
                case '1': return Color.FromArgb(0, 0, 170);
                case '2': return Color.FromArgb(0, 170, 0);
                case '3': return Color.FromArgb(0, 170, 170);
                case '4': return Color.FromArgb(170, 0, 0);
                case '5': return Color.FromArgb(170, 0, 170);
                case '6': return Color.FromArgb(255, 170, 0);
                case '7': return Color.FromArgb(170, 170, 170);
                case '8': return Color.FromArgb(140, 140, 140);  // Brightened Dark Gray so it doesn't vanish on the dark background
                case '9': return Color.FromArgb(85, 85, 255);
                case 'a': return Color.FromArgb(85, 255, 85);
                case 'b': return Color.FromArgb(85, 255, 255);
                case 'c': return Color.FromArgb(255, 85, 85);
                case 'd': return Color.FromArgb(255, 85, 255);
                case 'e': return Color.FromArgb(255, 255, 85);
                case 'f': return Color.FromArgb(255, 255, 255);
                case 'r': return defaultColor;
                default: return defaultColor;
            }
        }
    }
}