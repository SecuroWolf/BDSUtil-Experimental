using System;
using System.IO;
using System.Windows.Forms;
using BDSUtil.UI;
using Material3.Core;
using Material3.WinForms.Drawing;

namespace BDSUtil.Modals
{
    public partial class AutoModWrapper : Material3Form
    {
        private readonly MainWindow _parentForm;
        private string _targetRootPath;
        private bool _isFilterLoaded = false;
        private bool _isOptionsLoaded = false;

        public AutoModWrapper(MainWindow parent)
        {
            InitializeComponent();

            _parentForm = parent;
            this.Text = "AutoMod Management";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;
            this.ShowInTaskbar = true;
            this.StartPosition = FormStartPosition.CenterParent;
            SyncTitleBarMetadata();

            Material3NavigationHelper.Bind(
                _navRail,
                AMTabControl,
                ("Filter", MaterialIcons.Search),
                ("Options", MaterialIcons.Settings),
                ("Actions", MaterialIcons.Warning),
                ("Save & Exit", MaterialIcons.CheckCircle)
            );

            if (AMTabControl != null)
            {
                AMTabControl.SelectedIndexChanged += AMTabControl_SelectedIndexChanged;
            }
        }

        private void AutoModWrapper_Load(object sender, EventArgs e)
        {
            string storedPath = AppSettings.Instance?.General.ServerExecutablePath;
            if (string.IsNullOrWhiteSpace(storedPath)) return;

            _targetRootPath = storedPath.EndsWith(".exe", StringComparison.OrdinalIgnoreCase)
                ? Path.GetDirectoryName(storedPath)
                : storedPath;

            if (AMTabControl != null && AMTabControl.SelectedTab == CFTab)
            {
                InitializeFilterTabControl();
            }
        }
        private void AMTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AMTabControl == null || AMTabControl.SelectedTab == null || string.IsNullOrEmpty(_targetRootPath)) return;

            if (AMTabControl.SelectedTab == CFTab)
            {
                InitializeFilterTabControl();
            }
            else if (AMTabControl.SelectedTab == AMTab)
            {
                InitializeOptionsTabControl();
            }
            else if (AMTabControl.SelectedTab == SaveTab)
            {
                CloseWindow();
            }
        }

        private void InitializeFilterTabControl()
        {
            if (_isFilterLoaded || string.IsNullOrEmpty(_targetRootPath)) return;

            if (filterTab1 != null)
            {
                filterTab1.InitializeControl(_targetRootPath);
                _isFilterLoaded = true;
            }
        }
        private void InitializeOptionsTabControl()
        {
            if (_isOptionsLoaded) return;

            if (autoModOptions1 != null)
            {
                autoModOptions1.InitializeControl();
                _isOptionsLoaded = true;
            }
        }
        private void CloseWindow()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
