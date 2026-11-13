using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Material3.WinForms.Controls;
using Material3.WinForms.Theming;

namespace BDSUtil.UI
{
    public class MaterialListBoxItem
    {
        public MaterialListBoxItem()
        {
        }

        public MaterialListBoxItem(string text, string secondaryText = "", object tag = null)
        {
            Text = text ?? string.Empty;
            SecondaryText = secondaryText ?? string.Empty;
            Tag = tag;
        }

        public string Text { get; set; } = string.Empty;
        public string SecondaryText { get; set; } = string.Empty;
        public object Tag { get; set; }

        public override string ToString() => Text;
    }

    /// <summary>
    /// Material 3 styled scrollable single-select list host backed by <see cref="MaterialListItem"/> rows.
    /// </summary>
    public class MaterialListBox : MaterialRoundedPanel
    {
        private readonly Panel _scrollHost;
        private readonly List<MaterialListBoxItem> _items = new List<MaterialListBoxItem>();
        private readonly List<MaterialListItem> _rowControls = new List<MaterialListItem>();
        private int _selectedIndex = -1;

        public delegate void SelectedIndexChangedHandler(object sender, MaterialListBoxItem selectedItem);
        public event SelectedIndexChangedHandler SelectedIndexChanged;

        public MaterialListBox() : base(10)
        {
            Material3AppTheme.EnsureInitialized();
            Padding = new Padding(4);

            _scrollHost = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BorderStyle = BorderStyle.None
            };

            Items = new ListItemCollection(this);
            _scrollHost.SizeChanged += (s, e) => LayoutRows();
            Controls.Add(_scrollHost);

            ApplyPalette();
            ThemeHook.Attach(this, ApplyPalette);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public ListItemCollection Items { get; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SelectIndex(value);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MaterialListBoxItem SelectedItem =>
            (_selectedIndex >= 0 && _selectedIndex < _items.Count) ? _items[_selectedIndex] : null;

        [DefaultValue(BorderStyle.None)]
        public new BorderStyle BorderStyle { get; set; } = BorderStyle.None;

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            LayoutRows();
        }

        private void ApplyPalette()
        {
            if (IsDisposed)
            {
                return;
            }

            BackColor = MaterialColors.SurfaceContainerLow;
            OutlineColor = MaterialColors.OutlineVariant;
            _scrollHost.BackColor = MaterialColors.SurfaceContainerLow;
            _scrollHost.ForeColor = MaterialColors.OnSurface;
            Invalidate();
        }

        internal void AddRow(MaterialListBoxItem item)
        {
            int idx = _items.Count;
            _items.Add(item);

            var row = new MaterialListItem
            {
                Headline = item.Text ?? string.Empty,
                SupportingText = item.SecondaryText ?? string.Empty,
                CornerRadius = 8,
                Height = string.IsNullOrEmpty(item.SecondaryText) ? 44 : 58,
                Tag = item
            };

            row.Click += (s, e) => SelectIndex(idx);
            _rowControls.Add(row);
            _scrollHost.Controls.Add(row);
            LayoutRows();
        }

        internal void ClearRows()
        {
            _selectedIndex = -1;
            _items.Clear();
            _rowControls.Clear();
            _scrollHost.SuspendLayout();
            try
            {
                while (_scrollHost.Controls.Count > 0)
                {
                    Control c = _scrollHost.Controls[0];
                    _scrollHost.Controls.RemoveAt(0);
                    c.Dispose();
                }
            }
            finally
            {
                _scrollHost.ResumeLayout(true);
            }
        }

        private void SelectIndex(int index)
        {
            if (index < -1 || index >= _items.Count)
            {
                return;
            }

            _selectedIndex = index;
            for (int i = 0; i < _rowControls.Count; i++)
            {
                _rowControls[i].IsSelected = (i == _selectedIndex);
            }

            SelectedIndexChanged?.Invoke(this, SelectedItem);
        }

        private void LayoutRows()
        {
            if (_scrollHost.IsDisposed || _rowControls.Count == 0)
            {
                return;
            }

            _scrollHost.SuspendLayout();
            try
            {
                int scrollBarWidth = _scrollHost.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0;
                int rowWidth = Math.Max(80, _scrollHost.ClientSize.Width - scrollBarWidth - 8);
                int y = 4 + _scrollHost.AutoScrollPosition.Y;

                foreach (MaterialListItem row in _rowControls)
                {
                    if (!row.IsDisposed)
                    {
                        int h = string.IsNullOrEmpty(row.SupportingText) ? 44 : 58;
                        row.SetBounds(4, y, rowWidth, h);
                        y += h + 2;
                    }
                }
            }
            finally
            {
                _scrollHost.ResumeLayout(false);
            }
        }

        public sealed class ListItemCollection : IList, IEnumerable<MaterialListBoxItem>
        {
            private readonly MaterialListBox _owner;

            internal ListItemCollection(MaterialListBox owner)
            {
                _owner = owner;
            }

            public int Count => _owner._items.Count;
            public bool IsFixedSize => false;
            public bool IsReadOnly => false;
            public bool IsSynchronized => false;
            public object SyncRoot => ((ICollection)_owner._items).SyncRoot;

            object IList.this[int index]
            {
                get => _owner._items[index];
                set => throw new NotSupportedException();
            }

            public MaterialListBoxItem this[int index] => _owner._items[index];

            public void Add(MaterialListBoxItem item)
            {
                if (item != null)
                {
                    _owner.AddRow(item);
                }
            }

            public int Add(object value)
            {
                if (value is MaterialListBoxItem item)
                {
                    Add(item);
                }
                else
                {
                    Add(new MaterialListBoxItem(value?.ToString() ?? string.Empty));
                }

                return _owner._items.Count - 1;
            }

            public void Clear() => _owner.ClearRows();

            public bool Contains(object value) => IndexOf(value) >= 0;

            public int IndexOf(object value)
            {
                if (value is MaterialListBoxItem item)
                {
                    return _owner._items.IndexOf(item);
                }

                return -1;
            }

            public void Insert(int index, object value) => throw new NotSupportedException();
            public void Remove(object value) => throw new NotSupportedException();
            public void RemoveAt(int index) => throw new NotSupportedException();
            public void CopyTo(Array array, int index) => ((ICollection)_owner._items).CopyTo(array, index);
            public IEnumerator<MaterialListBoxItem> GetEnumerator() => _owner._items.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => _owner._items.GetEnumerator();
        }
    }
}
