using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Material3.WinForms.Controls;
using Material3.WinForms.Theming;
using Material3.WinForms.Typography;

namespace BDSUtil.UI
{
    /// <summary>
    /// Material 3 styled scrollable checklist container exposing <see cref="Material3.WinForms.Controls.MaterialCheckBox"/> items
    /// while matching the legacy <see cref="Items"/> API across BDSUtil panels.
    /// </summary>
    public class MaterialCheckListBox : MaterialRoundedPanel
    {
        private readonly Panel _scrollHost;
        private readonly CheckListItemCollection _items;

        public MaterialCheckListBox() : base(10)
        {
            Material3AppTheme.EnsureInitialized();
            Padding = new Padding(4);

            _scrollHost = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BorderStyle = BorderStyle.None
            };

            _items = new CheckListItemCollection(this);
            _scrollHost.SizeChanged += (s, e) => LayoutItems();
            Controls.Add(_scrollHost);

            ApplyPalette();
            ThemeHook.Attach(this, ApplyPalette);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public CheckListItemCollection Items => _items;

        [DefaultValue(BorderStyle.None)]
        public new BorderStyle BorderStyle { get; set; } = BorderStyle.None;

        [DefaultValue(true)]
        public bool CheckOnClick { get; set; } = true;

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            LayoutItems();
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            if (!IsDisposed && ReferenceEquals(e.Control, _scrollHost) && !Controls.Contains(_scrollHost))
            {
                Controls.Add(_scrollHost);
            }
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

        internal void AddCheckBox(Material3.WinForms.Controls.MaterialCheckBox checkBox)
        {
            if (checkBox == null)
            {
                return;
            }

            checkBox.AutoSize = false;
            checkBox.Height = 36;
            checkBox.Margin = new Padding(0, 2, 0, 2);
            checkBox.Padding = new Padding(6, 0, 6, 0);
            checkBox.Font = TypeScale.BodyMedium;

            _scrollHost.Controls.Add(checkBox);
            LayoutItems();
        }

        internal void RemoveCheckBox(Material3.WinForms.Controls.MaterialCheckBox checkBox)
        {
            if (checkBox == null)
            {
                return;
            }

            _scrollHost.Controls.Remove(checkBox);
            checkBox.Dispose();
            LayoutItems();
        }

        internal void ClearCheckBoxes()
        {
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

        private void LayoutItems()
        {
            if (_scrollHost.IsDisposed || _items.Count == 0)
            {
                return;
            }

            _scrollHost.SuspendLayout();
            try
            {
                int scrollBarWidth = _scrollHost.VerticalScroll.Visible ? SystemInformation.VerticalScrollBarWidth : 0;
                int itemWidth = Math.Max(80, _scrollHost.ClientSize.Width - scrollBarWidth - 8);
                int y = 4 + _scrollHost.AutoScrollPosition.Y;

                foreach (Material3.WinForms.Controls.MaterialCheckBox item in _items.InternalList)
                {
                    if (!item.IsDisposed)
                    {
                        item.SetBounds(4, y, itemWidth, 36);
                        y += 38;
                    }
                }
            }
            finally
            {
                _scrollHost.ResumeLayout(false);
            }
        }

        public sealed class CheckListItemCollection : IList, IEnumerable<Material3.WinForms.Controls.MaterialCheckBox>
        {
            private readonly MaterialCheckListBox _owner;
            internal readonly List<Material3.WinForms.Controls.MaterialCheckBox> InternalList = new List<Material3.WinForms.Controls.MaterialCheckBox>();

            internal CheckListItemCollection(MaterialCheckListBox owner)
            {
                _owner = owner;
            }

            public int Count => InternalList.Count;
            public bool IsFixedSize => false;
            public bool IsReadOnly => false;
            public bool IsSynchronized => false;
            public object SyncRoot => ((ICollection)InternalList).SyncRoot;

            object IList.this[int index]
            {
                get => InternalList[index];
                set => throw new NotSupportedException();
            }

            public Material3.WinForms.Controls.MaterialCheckBox this[int index] => InternalList[index];

            public Material3.WinForms.Controls.MaterialCheckBox Add(string text, bool isChecked = false)
            {
                var cb = new Material3.WinForms.Controls.MaterialCheckBox
                {
                    Text = text ?? string.Empty,
                    Checked = isChecked
                };
                InternalList.Add(cb);
                _owner.AddCheckBox(cb);
                return cb;
            }

            public int Add(object value)
            {
                if (value is Material3.WinForms.Controls.MaterialCheckBox cb)
                {
                    InternalList.Add(cb);
                    _owner.AddCheckBox(cb);
                    return InternalList.Count - 1;
                }

                Add(value?.ToString() ?? string.Empty, false);
                return InternalList.Count - 1;
            }

            public void Clear()
            {
                InternalList.Clear();
                _owner.ClearCheckBoxes();
            }

            public bool Contains(object value) => IndexOf(value) >= 0;

            public int IndexOf(object value)
            {
                if (value is Material3.WinForms.Controls.MaterialCheckBox cb)
                {
                    return InternalList.IndexOf(cb);
                }

                string text = value?.ToString();
                for (int i = 0; i < InternalList.Count; i++)
                {
                    if (string.Equals(InternalList[i].Text, text, StringComparison.Ordinal))
                    {
                        return i;
                    }
                }

                return -1;
            }

            public void Insert(int index, object value) => throw new NotSupportedException();

            public void Remove(object value)
            {
                int idx = IndexOf(value);
                if (idx >= 0)
                {
                    RemoveAt(idx);
                }
            }

            public void RemoveAt(int index)
            {
                if (index < 0 || index >= InternalList.Count)
                {
                    return;
                }

                Material3.WinForms.Controls.MaterialCheckBox target = InternalList[index];
                InternalList.RemoveAt(index);
                _owner.RemoveCheckBox(target);
            }

            public void CopyTo(Array array, int index) => ((ICollection)InternalList).CopyTo(array, index);
            public IEnumerator<Material3.WinForms.Controls.MaterialCheckBox> GetEnumerator() => InternalList.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => InternalList.GetEnumerator();
        }
    }
}
