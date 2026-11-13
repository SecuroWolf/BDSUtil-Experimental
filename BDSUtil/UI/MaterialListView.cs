using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Material3.WinForms.Theming;
using Material3.WinForms.Typography;

namespace BDSUtil.UI
{
    /// <summary>
    /// Material 3 dark owner-drawn <see cref="ListView"/> matching M3 surface and tonal selection tokens.
    /// </summary>
    public class MaterialListView : ListView
    {
        public MaterialListView()
        {
            Material3AppTheme.EnsureInitialized();
            OwnerDraw = true;
            DoubleBuffered = true;
            BorderStyle = BorderStyle.None;
            FullRowSelect = true;
            HideSelection = false;
            Font = TypeScale.BodyMedium;

            ApplyPalette();
            ThemeHook.Attach(this, ApplyPalette);
        }

        [DefaultValue(false)]
        public bool AutoSizeTable { get; set; }

        [DefaultValue(5)]
        public int MouseLocation { get; set; } = 5;

        private void ApplyPalette()
        {
            if (IsDisposed)
            {
                return;
            }

            BackColor = MaterialColors.SurfaceContainerLow;
            ForeColor = MaterialColors.OnSurface;
            Invalidate();
        }

        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            using (var bg = new SolidBrush(MaterialColors.SurfaceContainerHigh))
            using (var border = new Pen(MaterialColors.OutlineVariant, 1f))
            {
                e.Graphics.FillRectangle(bg, e.Bounds);
                e.Graphics.DrawLine(border, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            }

            Rectangle textRect = new Rectangle(e.Bounds.X + 8, e.Bounds.Y, Math.Max(1, e.Bounds.Width - 16), e.Bounds.Height);
            TextRenderer.DrawText(
                e.Graphics,
                e.Header?.Text ?? string.Empty,
                TypeScale.TitleSmall,
                textRect,
                MaterialColors.OnSurface,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            // SubItem drawing paints the full Details view cleanly; handle non-Details fallback gracefully.
            if (View != View.Details)
            {
                Color rowBg = e.Item.Selected ? MaterialColors.SecondaryContainer : BackColor;
                Color rowFg = e.Item.Selected ? MaterialColors.OnSecondaryContainer : MaterialColors.OnSurface;
                using (var brush = new SolidBrush(rowBg))
                {
                    e.Graphics.FillRectangle(brush, e.Bounds);
                }

                TextRenderer.DrawText(
                    e.Graphics,
                    e.Item.Text,
                    Font,
                    e.Bounds,
                    rowFg,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
        }

        protected override void OnDrawSubItem(DrawListViewSubItemEventArgs e)
        {
            bool selected = e.Item.Selected;
            Color bgColor = selected
                ? MaterialColors.SecondaryContainer
                : (e.ItemIndex % 2 == 1 ? MaterialColors.SurfaceContainer : MaterialColors.SurfaceContainerLow);
            Color fgColor = selected ? MaterialColors.OnSecondaryContainer : MaterialColors.OnSurface;

            using (var brush = new SolidBrush(bgColor))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }

            using (var divider = new Pen(MaterialColors.OutlineVariant, 1f))
            {
                e.Graphics.DrawLine(divider, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            }

            if (!string.IsNullOrEmpty(e.SubItem?.Text))
            {
                Rectangle textRect = new Rectangle(e.Bounds.X + 8, e.Bounds.Y, Math.Max(1, e.Bounds.Width - 14), e.Bounds.Height);
                TextRenderer.DrawText(
                    e.Graphics,
                    e.SubItem.Text,
                    Font,
                    textRect,
                    fgColor,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }

            base.OnDrawSubItem(e);
        }
    }
}

