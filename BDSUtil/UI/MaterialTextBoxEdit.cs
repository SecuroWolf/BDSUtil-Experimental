using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Material3.WinForms.Controls;
using Material3.WinForms.Typography;

namespace BDSUtil.UI
{
    /// <summary>
    /// Outlined Material 3 text input adapter wrapping <see cref="MaterialTextField"/> with compact-height scaling
    /// and standard TextBox keyboard/selection/focus forwarding.
    /// </summary>
    public class MaterialTextBoxEdit : Control
    {
        private readonly MaterialTextField _field;
        private int _maxLength = 32767;

        public new event KeyPressEventHandler KeyPress;
        public new event KeyEventHandler KeyDown;
        public new event KeyEventHandler KeyUp;
        public new event EventHandler Leave;

        public MaterialTextBoxEdit()
        {
            Material3AppTheme.EnsureInitialized();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;

            _field = new MaterialTextField
            {
                Dock = DockStyle.Fill,
                AutoSize = false,
                Variant = MaterialTextFieldVariant.Outlined,
                CornerRadius = 6,
                HorizontalPadding = 12,
                Font = TypeScale.BodyMedium
            };

            _field.TextChanged += (s, e) => OnTextChanged(EventArgs.Empty);
            _field.Editor.KeyPress += (s, e) => KeyPress?.Invoke(this, e);
            _field.Editor.KeyDown += (s, e) => KeyDown?.Invoke(this, e);
            _field.Editor.KeyUp += (s, e) => KeyUp?.Invoke(this, e);
            _field.Editor.LostFocus += (s, e) => Leave?.Invoke(this, EventArgs.Empty);

            Controls.Add(_field);
            UpdateFieldMetrics();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TextBox Editor => _field.Editor;

        [DefaultValue("")]
        public string Hint
        {
            get => _field.LabelText;
            set => _field.LabelText = value ?? string.Empty;
        }

        [DefaultValue("")]
        public string SupportingText
        {
            get => _field.SupportingText;
            set
            {
                _field.SupportingText = value ?? string.Empty;
                UpdateFieldMetrics();
            }
        }

        [DefaultValue("")]
        public override string Text
        {
            get => _field.Text;
            set => _field.Text = value ?? string.Empty;
        }

        [DefaultValue(false)]
        public bool ReadOnly
        {
            get => _field.ReadOnly;
            set => _field.ReadOnly = value;
        }

        [DefaultValue(false)]
        public bool UseSystemPasswordChar
        {
            get => _field.UseSystemPasswordChar;
            set => _field.UseSystemPasswordChar = value;
        }

        [DefaultValue('\0')]
        public char PasswordChar
        {
            get => _field.Editor.PasswordChar;
            set => _field.Editor.PasswordChar = value;
        }

        [DefaultValue(32767)]
        public int MaxLength
        {
            get => _maxLength;
            set
            {
                _maxLength = value;
                _field.MaxLength = value;
            }
        }

        [DefaultValue(MaterialInputKind.Text)]
        public MaterialInputKind InputKind
        {
            get => _field.InputKind;
            set => _field.InputKind = value;
        }

        [DefaultValue("")]
        public string LeadingGlyph
        {
            get => _field.LeadingGlyph;
            set => _field.LeadingGlyph = value ?? string.Empty;
        }

        [DefaultValue("")]
        public string TrailingGlyph
        {
            get => _field.TrailingGlyph;
            set => _field.TrailingGlyph = value ?? string.Empty;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart
        {
            get => _field.Editor.SelectionStart;
            set => _field.Editor.SelectionStart = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionLength
        {
            get => _field.Editor.SelectionLength;
            set => _field.Editor.SelectionLength = value;
        }

        [Browsable(false)]
        public int TextLength => _field.Editor.TextLength;

        public new bool Focus() => _field.Focus() || _field.Editor.Focus();

        public void SelectAll() => _field.Editor.SelectAll();

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateFieldMetrics();
        }

        private void UpdateFieldMetrics()
        {
            if (Height <= 0)
            {
                return;
            }

            // MaterialTextField starts its outlined rectangle at y=9 and draws SupportingText 18px below it.
            int reservedBelow = string.IsNullOrEmpty(_field.SupportingText) && string.IsNullOrEmpty(_field.ErrorText) ? 2 : 18;
            int available = Height - 9 - reservedBelow;
            _field.FieldHeight = Math.Max(25, Math.Min(56, available));
        }
    }
}

