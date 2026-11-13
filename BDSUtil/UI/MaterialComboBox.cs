using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Material3.WinForms.Controls;

namespace BDSUtil.UI
{
    /// <summary>
    /// WinForms-compatible adapter wrapping Material3.WinForms <see cref="MaterialDropdownSelect"/>
    /// with standard Items, SelectedItem, SelectedIndex, Hint, and SelectedIndexChanged semantics.
    /// </summary>
    public class MaterialComboBox : Control, ISupportInitialize
    {
        private readonly MaterialDropdownSelect _dropdown;
        private readonly ComboItemCollection _items;
        private bool _initializing;
        private bool _suppressSelectionEvents;
        private string _hint = string.Empty;

        public event EventHandler SelectedIndexChanged;

        public MaterialComboBox()
        {
            Material3AppTheme.EnsureInitialized();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            _dropdown = new MaterialDropdownSelect
            {
                Dock = DockStyle.Fill,
                AutoSize = false
            };

            _items = new ComboItemCollection(this);

            _dropdown.SelectedIndexChanged += (s, e) =>
            {
                if (!_initializing && !_suppressSelectionEvents)
                {
                    SelectedIndexChanged?.Invoke(this, EventArgs.Empty);
                }
            };

            Controls.Add(_dropdown);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ComboItemCollection Items => _items;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get => _dropdown.SelectedIndex;
            set
            {
                if (value < -1 || value >= _items.Count)
                {
                    return;
                }

                _dropdown.SelectedIndex = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedItem
        {
            get
            {
                int idx = _dropdown.SelectedIndex;
                return (idx >= 0 && idx < _items.Count) ? _items[idx] : null;
            }
            set
            {
                if (value == null)
                {
                    _dropdown.SelectedIndex = -1;
                    return;
                }

                int idx = _items.IndexOf(value);
                if (idx >= 0)
                {
                    _dropdown.SelectedIndex = idx;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get => SelectedItem?.ToString() ?? string.Empty;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    return;
                }

                int idx = _items.IndexOf(value);
                if (idx >= 0)
                {
                    _dropdown.SelectedIndex = idx;
                }
            }
        }

        [DefaultValue("")]
        public string Hint
        {
            get => _hint;
            set => _hint = value ?? string.Empty;
        }

        [DefaultValue(ComboBoxStyle.DropDownList)]
        public ComboBoxStyle DropDownStyle { get; set; } = ComboBoxStyle.DropDownList;

        public void BeginInit() => _initializing = true;

        public void EndInit()
        {
            _initializing = false;
        }

        public void BeginUpdate() => _suppressSelectionEvents = true;

        public void EndUpdate()
        {
            _suppressSelectionEvents = false;
            RebuildDropdownItems(_dropdown.SelectedIndex);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (Height > 0)
            {
                _dropdown.HeaderHeight = Math.Max(28, Height);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _dropdown.Focus();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (_items.Count == 0 || !Enabled) return;
            int step = e.Delta > 0 ? -1 : 1;
            int newIndex = Math.Max(0, Math.Min(SelectedIndex + step, _items.Count - 1));
            if (newIndex != SelectedIndex)
            {
                SelectedIndex = newIndex;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (_items.Count == 0 || !Enabled) return;
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Right)
            {
                if (SelectedIndex < _items.Count - 1)
                {
                    SelectedIndex++;
                    e.Handled = true;
                }
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Left)
            {
                if (SelectedIndex > 0)
                {
                    SelectedIndex--;
                    e.Handled = true;
                }
            }
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _dropdown.Enabled = Enabled;
            Invalidate(true);
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            _dropdown.Font = Font;
        }

        private void RebuildDropdownItems(int preserveIndex)
        {
            bool previousSuppress = _suppressSelectionEvents;
            _suppressSelectionEvents = true;
            try
            {
                _dropdown.ClearItems();
                foreach (object item in _items.InternalList)
                {
                    string display = item?.ToString() ?? string.Empty;
                    _dropdown.AddItem(display, item);
                }

                if (preserveIndex >= 0 && preserveIndex < _items.Count)
                {
                    _dropdown.SelectedIndex = preserveIndex;
                }
                else if (_items.Count == 0)
                {
                    _dropdown.SelectedIndex = -1;
                }
            }
            finally
            {
                _suppressSelectionEvents = previousSuppress;
            }
        }

        public sealed class ComboItemCollection : IList, IEnumerable<object>
        {
            private readonly MaterialComboBox _owner;
            internal readonly List<object> InternalList = new List<object>();

            internal ComboItemCollection(MaterialComboBox owner)
            {
                _owner = owner;
            }

            public int Count => InternalList.Count;
            public bool IsFixedSize => false;
            public bool IsReadOnly => false;
            public bool IsSynchronized => false;
            public object SyncRoot => ((ICollection)InternalList).SyncRoot;

            public object this[int index]
            {
                get => InternalList[index];
                set
                {
                    InternalList[index] = value;
                    _owner.RebuildDropdownItems(_owner.SelectedIndex);
                }
            }

            public int Add(object item)
            {
                InternalList.Add(item);
                bool wasEmpty = InternalList.Count == 1;
                _owner._suppressSelectionEvents = true;
                try
                {
                    _owner._dropdown.AddItem(item?.ToString() ?? string.Empty, item);
                    if (wasEmpty && _owner._initializing)
                    {
                        _owner._dropdown.SelectedIndex = 0;
                    }
                }
                finally
                {
                    _owner._suppressSelectionEvents = false;
                }

                return InternalList.Count - 1;
            }

            public void AddRange(object[] items)
            {
                if (items == null || items.Length == 0)
                {
                    return;
                }

                _owner._suppressSelectionEvents = true;
                try
                {
                    foreach (object item in items)
                    {
                        InternalList.Add(item);
                        _owner._dropdown.AddItem(item?.ToString() ?? string.Empty, item);
                    }
                }
                finally
                {
                    _owner._suppressSelectionEvents = false;
                }
            }

            public void Clear()
            {
                InternalList.Clear();
                _owner._suppressSelectionEvents = true;
                try
                {
                    _owner._dropdown.ClearItems();
                }
                finally
                {
                    _owner._suppressSelectionEvents = false;
                }
            }

            public bool Contains(object value) => IndexOf(value) >= 0;

            public int IndexOf(object value)
            {
                if (value == null)
                {
                    return -1;
                }

                string strVal = value.ToString();
                for (int i = 0; i < InternalList.Count; i++)
                {
                    if (Equals(InternalList[i], value) ||
                        string.Equals(InternalList[i]?.ToString(), strVal, StringComparison.OrdinalIgnoreCase))
                    {
                        return i;
                    }
                }

                return -1;
            }

            public void Insert(int index, object value)
            {
                InternalList.Insert(index, value);
                _owner.RebuildDropdownItems(_owner.SelectedIndex);
            }

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
                int current = _owner.SelectedIndex;
                InternalList.RemoveAt(index);
                int next = current == index ? -1 : (current > index ? current - 1 : current);
                _owner.RebuildDropdownItems(next);
            }

            public void CopyTo(Array array, int index) => ((ICollection)InternalList).CopyTo(array, index);
            public IEnumerator<object> GetEnumerator() => InternalList.GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator() => InternalList.GetEnumerator();
        }
    }
}

