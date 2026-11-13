using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Material3.WinForms;
using Material3.WinForms.Drawing;
using Material3.WinForms.Theming;

namespace BDSUtil.UI
{
    /// <summary>
    /// Material 3 linear progress indicator supporting both determinate <see cref="Value"/>
    /// and indeterminate <see cref="ProgressBarStyle.Marquee"/> modes.
    /// </summary>
    public class MaterialMarqueeProgressBar : Control
    {
        private readonly Timer _marqueeTimer;
        private ProgressBarStyle _style = ProgressBarStyle.Continuous;
        private int _value;
        private float _marqueePhase;

        public MaterialMarqueeProgressBar()
        {
            Material3AppTheme.EnsureInitialized();
            SetStyle(
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.SupportsTransparentBackColor,
                true);
            BackColor = Color.Transparent;
            Height = 8;

            _marqueeTimer = new Timer { Interval = 16 };
            _marqueeTimer.Tick += (s, e) =>
            {
                _marqueePhase = (_marqueePhase + 0.022f) % 1.4f;
                Invalidate();
            };

            ThemeHook.Attach(this, Invalidate);
        }

        [DefaultValue(ProgressBarStyle.Continuous)]
        public ProgressBarStyle Style
        {
            get => _style;
            set
            {
                if (_style == value)
                {
                    return;
                }

                _style = value;
                SyncTimer();
                Invalidate();
            }
        }

        [DefaultValue(0)]
        public int Value
        {
            get => _value;
            set
            {
                int clamped = Math.Max(0, Math.Min(100, value));
                if (_value == clamped)
                {
                    return;
                }

                _value = clamped;
                Invalidate();
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            SyncTimer();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _marqueeTimer.Stop();
                _marqueeTimer.Dispose();
            }

            base.Dispose(disposing);
        }

        private void SyncTimer()
        {
            if (!DesignMode && _style == ProgressBarStyle.Marquee && Visible)
            {
                if (!_marqueeTimer.Enabled)
                {
                    _marqueeTimer.Start();
                }
            }
            else if (_marqueeTimer.Enabled)
            {
                _marqueeTimer.Stop();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int thickness = Math.Min(Height, Dpi.Scale(this, 6));
            int y = (Height - thickness) / 2;
            int radius = Math.Max(1, thickness / 2);
            int width = Math.Max(1, Width - 1);

            var trackRect = new Rectangle(0, y, width, thickness);
            using (GraphicsPath trackPath = RoundedControlRenderer.GetFigurePath(trackRect, radius))
            using (var trackBrush = new SolidBrush(MaterialColors.SecondaryContainer))
            {
                g.FillPath(trackBrush, trackPath);
            }

            if (_style == ProgressBarStyle.Marquee)
            {
                int pillWidth = Math.Max(24, width / 3);
                int travel = width + pillWidth;
                int x = (int)(_marqueePhase / 1.4f * travel) - pillWidth;
                int clampedLeft = Math.Max(0, x);
                int clampedRight = Math.Min(width, x + pillWidth);
                if (clampedRight > clampedLeft)
                {
                    var pillRect = new Rectangle(clampedLeft, y, clampedRight - clampedLeft, thickness);
                    using (GraphicsPath pillPath = RoundedControlRenderer.GetFigurePath(pillRect, radius))
                    using (var fillBrush = new SolidBrush(MaterialColors.Primary))
                    {
                        g.FillPath(fillBrush, pillPath);
                    }
                }
            }
            else if (_value > 0)
            {
                int fillWidth = (int)Math.Round(width * (_value / 100f));
                if (fillWidth > 0)
                {
                    var fillRect = new Rectangle(0, y, Math.Min(width, fillWidth), thickness);
                    using (GraphicsPath fillPath = RoundedControlRenderer.GetFigurePath(fillRect, radius))
                    using (var fillBrush = new SolidBrush(MaterialColors.Primary))
                    {
                        g.FillPath(fillBrush, fillPath);
                    }
                }
            }
        }
    }
}

