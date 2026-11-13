using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Material3.WinForms;
using Material3.WinForms.Controls;
using Material3.WinForms.Forms;
using Material3.WinForms.Theming;

namespace BDSUtil.UI
{
    /// <summary>
    /// Shared Material 3 borderless window chrome base class with integrated title bar and DWM theming.
    /// </summary>
    public class Material3Form : BorderlessForm
    {
        private readonly MaterialTitleBar _titleBar;
        private bool _showTitleBar = true;
        private bool _sizable = true;

        public Material3Form()
        {
            Material3AppTheme.EnsureInitialized();

            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(96F, 96F);
            DoubleBuffered = true;

            _titleBar = new MaterialTitleBar
            {
                Name = "_m3TitleBar",
                Dock = DockStyle.Top,
                ShowMinimize = true,
                ShowMaximize = true,
                ShowClose = true,
                TabStop = false
            };

            Controls.Add(_titleBar);

            ApplySurfacePalette();
            ThemeHook.Attach(this, ApplySurfacePalette);
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MaterialTitleBar TitleBar => _titleBar;

        /// <summary>
        /// Gets or sets whether the Material 3 title bar is visible. Fixed headerless dialogs set this to false.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowTitleBar
        {
            get => _showTitleBar;
            set
            {
                if (_showTitleBar == value)
                {
                    return;
                }

                _showTitleBar = value;
                _titleBar.Visible = value;
                if (!value)
                {
                    EnableDragAnywhere = true;
                }

                PerformLayout();
                Invalidate();
            }
        }

        /// <summary>
        /// Gets or sets whether window edge resizing and the maximize caption button are enabled.
        /// </summary>
        [DefaultValue(true)]
        public bool Sizable
        {
            get => _sizable;
            set
            {
                _sizable = value;
                EnableEdgeResize = value;
                _titleBar.ShowMaximize = value && MaximizeBox;
            }
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (_titleBar != null && !ReferenceEquals(e.Control, _titleBar) && Controls.Contains(_titleBar))
            {
                _titleBar.SendToBack();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SyncTitleBarMetadata();
            DwmWindow.ThemeCaption(this);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            _titleBar.TitleText = Text ?? string.Empty;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (WindowState == FormWindowState.Normal && ClientSize.Width > 1 && ClientSize.Height > 1)
            {
                using (var pen = new Pen(MaterialColors.OutlineVariant, 1f))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
                }
            }
        }

        protected void SyncTitleBarMetadata()
        {
            _titleBar.TitleText = Text ?? string.Empty;
            _titleBar.ShowMinimize = MinimizeBox;
            _titleBar.ShowMaximize = _sizable && MaximizeBox;
            _titleBar.ShowClose = ControlBox;
            if (ShowIcon && Icon != null && _titleBar.IconImage == null)
            {
                try
                {
                    _titleBar.IconImage = Icon.ToBitmap();
                }
                catch
                {
                    // Ignore malformed or non-convertible icon handles cleanly.
                }
            }
        }

        private void ApplySurfacePalette()
        {
            if (IsDisposed)
            {
                return;
            }

            BackColor = MaterialColors.Surface;
            ForeColor = MaterialColors.OnSurface;
            if (IsHandleCreated)
            {
                DwmWindow.ThemeCaption(this);
            }

            Invalidate();
        }
    }

    /// <summary>
    /// Base UserControl configured with Material 3 dark surface colors and DPI scaling.
    /// </summary>
    public class Material3UserControl : UserControl
    {
        public Material3UserControl()
        {
            Material3AppTheme.EnsureInitialized();
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoScaleDimensions = new SizeF(96F, 96F);
            DoubleBuffered = true;
            Material3AppTheme.ApplySurfaceTheme(this);
        }
    }
}
