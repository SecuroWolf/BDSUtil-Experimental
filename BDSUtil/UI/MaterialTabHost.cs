using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Material3.WinForms.Controls;
using Material3.WinForms.Theming;

namespace BDSUtil.UI
{
    /// <summary>
    /// Dark-themed headerless <see cref="TabControl"/> paired with <see cref="MaterialTabSelector"/>
    /// or <see cref="MaterialNavigationDrawer"/>.
    /// </summary>
    public class MaterialTabControl : TabControl
    {
        private const int TCM_ADJUSTRECT = 0x1328;

        public MaterialTabControl()
        {
            Material3AppTheme.EnsureInitialized();
            DoubleBuffered = true;
            Multiline = true;

            ControlAdded += (s, e) => StyleTabPage(e.Control as TabPage);
            ThemeHook.Attach(this, ApplyPalette);
            ApplyPalette();
        }

        private void ApplyPalette()
        {
            if (IsDisposed)
            {
                return;
            }

            BackColor = MaterialColors.Surface;
            ForeColor = MaterialColors.OnSurface;
            foreach (TabPage page in TabPages)
            {
                StyleTabPage(page);
            }
        }

        private static void StyleTabPage(TabPage page)
        {
            if (page == null || page.IsDisposed)
            {
                return;
            }

            page.UseVisualStyleBackColor = false;
            page.BackColor = MaterialColors.Surface;
            page.ForeColor = MaterialColors.OnSurface;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == TCM_ADJUSTRECT && !DesignMode)
            {
                m.Result = (IntPtr)1;
                return;
            }

            base.WndProc(ref m);
        }
    }

    /// <summary>
    /// Material 3 horizontal tab strip wrapping <see cref="MaterialTabs"/> and bound two-way to a <see cref="TabControl"/>.
    /// </summary>
    public class MaterialTabSelector : Control
    {
        private readonly MaterialTabs _tabs;
        private TabControl _baseTabControl;
        private bool _syncing;

        public MaterialTabSelector()
        {
            Material3AppTheme.EnsureInitialized();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = MaterialColors.Surface;

            _tabs = new MaterialTabs
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                Variant = MaterialTabVariant.Primary,
                BarHeight = 44
            };

            _tabs.SelectedIndexChanged += OnTabsSelectedIndexChanged;
            Controls.Add(_tabs);
            ThemeHook.Attach(this, () =>
            {
                if (!IsDisposed)
                {
                    BackColor = MaterialColors.Surface;
                }
            });
        }

        [DefaultValue(null)]
        public TabControl BaseTabControl
        {
            get => _baseTabControl;
            set
            {
                if (ReferenceEquals(_baseTabControl, value))
                {
                    return;
                }

                if (_baseTabControl != null)
                {
                    _baseTabControl.SelectedIndexChanged -= OnBaseTabControlSelectedIndexChanged;
                    _baseTabControl.ControlAdded -= OnTabPagesMutated;
                    _baseTabControl.ControlRemoved -= OnTabPagesMutated;
                    foreach (TabPage page in _baseTabControl.TabPages)
                    {
                        page.TextChanged -= OnTabPageTextChanged;
                    }
                }

                _baseTabControl = value;

                if (_baseTabControl != null)
                {
                    _baseTabControl.SelectedIndexChanged += OnBaseTabControlSelectedIndexChanged;
                    _baseTabControl.ControlAdded += OnTabPagesMutated;
                    _baseTabControl.ControlRemoved += OnTabPagesMutated;
                    foreach (TabPage page in _baseTabControl.TabPages)
                    {
                        page.TextChanged -= OnTabPageTextChanged;
                        page.TextChanged += OnTabPageTextChanged;
                    }
                }

                RebuildTabs();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            RebuildTabs();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            RebuildTabs();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Height > 0)
            {
                _tabs.BarHeight = Math.Max(36, Height);
            }
        }

        public void RebuildTabs()
        {
            if (_syncing)
            {
                return;
            }

            _syncing = true;
            try
            {
                _tabs.ClearTabs();
                if (_baseTabControl == null)
                {
                    return;
                }

                foreach (TabPage page in _baseTabControl.TabPages)
                {
                    page.TextChanged -= OnTabPageTextChanged;
                    page.TextChanged += OnTabPageTextChanged;
                    string label = !string.IsNullOrEmpty(page.Text) ? page.Text : page.Name;
                    _tabs.AddTab(label ?? string.Empty);
                }

                if (_baseTabControl.TabPages.Count > 0)
                {
                    int target = Math.Max(0, Math.Min(_baseTabControl.SelectedIndex, _baseTabControl.TabPages.Count - 1));
                    _tabs.SelectedIndex = target;
                }
            }
            finally
            {
                _syncing = false;
            }
        }

        private void OnTabPageTextChanged(object sender, EventArgs e) => RebuildTabs();

        private void OnTabPagesMutated(object sender, ControlEventArgs e)
        {
            if (e.Control is TabPage page)
            {
                page.TextChanged -= OnTabPageTextChanged;
                page.TextChanged += OnTabPageTextChanged;
            }

            RebuildTabs();
        }

        private void OnTabsSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_syncing || _baseTabControl == null)
            {
                return;
            }

            _syncing = true;
            try
            {
                int idx = _tabs.SelectedIndex;
                if (idx >= 0 && idx < _baseTabControl.TabCount && _baseTabControl.SelectedIndex != idx)
                {
                    _baseTabControl.SelectedIndex = idx;
                }
            }
            finally
            {
                _syncing = false;
            }
        }

        private void OnBaseTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            if (_syncing || _baseTabControl == null)
            {
                return;
            }

            _syncing = true;
            try
            {
                int idx = _baseTabControl.SelectedIndex;
                if (idx >= 0 && idx < _tabs.TabCount && _tabs.SelectedIndex != idx)
                {
                    _tabs.SelectedIndex = idx;
                }
            }
            finally
            {
                _syncing = false;
            }
        }
    }

    /// <summary>
    /// Keeps a <see cref="MaterialNavigationBase"/> rail or drawer synchronized two-way with a <see cref="TabControl"/>.
    /// </summary>
    public static class Material3NavigationHelper
    {
        public static void Bind(
            MaterialNavigationBase navigation,
            TabControl tabControl,
            params (string label, string glyph)[] items)
        {
            if (navigation == null || tabControl == null)
            {
                return;
            }

            navigation.ClearItems();
            if (items != null)
            {
                foreach ((string label, string glyph) in items)
                {
                    navigation.AddItem(label ?? string.Empty, glyph ?? string.Empty);
                }
            }

            bool syncing = false;

            if (tabControl.TabCount > 0 && tabControl.SelectedIndex >= 0 && tabControl.SelectedIndex < navigation.Destinations.Count)
            {
                syncing = true;
                navigation.SelectedIndex = tabControl.SelectedIndex;
                syncing = false;
            }

            navigation.SelectedIndexChanged += (s, e) =>
            {
                if (syncing)
                {
                    return;
                }

                syncing = true;
                try
                {
                    int idx = navigation.SelectedIndex;
                    if (idx >= 0 && idx < tabControl.TabCount && tabControl.SelectedIndex != idx)
                    {
                        tabControl.SelectedIndex = idx;
                    }
                }
                finally
                {
                    syncing = false;
                }
            };

            tabControl.SelectedIndexChanged += (s, e) =>
            {
                if (syncing)
                {
                    return;
                }

                syncing = true;
                try
                {
                    int idx = tabControl.SelectedIndex;
                    if (idx >= 0 && idx < navigation.Destinations.Count && navigation.SelectedIndex != idx)
                    {
                        navigation.SelectedIndex = idx;
                    }
                }
                finally
                {
                    syncing = false;
                }
            };
        }
    }
}
