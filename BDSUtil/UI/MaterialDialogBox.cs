using System;
using System.Drawing;
using System.Windows.Forms;
using Material3.Core;
using Material3.WinForms.Controls;
using Material3.WinForms.Drawing;
using Material3.WinForms.Forms;
using Material3.WinForms.Theming;

namespace BDSUtil.UI
{
    /// <summary>
    /// Material 3 modal dialog helper replacing ReaLTaiizor MaterialMessageBox and MaterialFlexibleForm.
    /// </summary>
    public static class MaterialDialogBox
    {
        public static DialogResult Show(string text, string caption = "BDSUtil", MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            return Show(null, text, caption, buttons);
        }

        public static DialogResult Show(
            IWin32Window owner,
            string text,
            string caption = "BDSUtil",
            MessageBoxButtons buttons = MessageBoxButtons.OK)
        {
            Material3AppTheme.EnsureInitialized();

            IWin32Window effectiveOwner = owner ?? Form.ActiveForm;

            using (var dialog = new MaterialDialog())
            {
                (string glyph, Color glyphColor) = ResolveIcon(caption ?? string.Empty, buttons);
                dialog.Glyph = glyph;
                dialog.GlyphColor = glyphColor;
                dialog.TitleText = caption ?? "BDSUtil";
                dialog.BodyText = text ?? string.Empty;

                switch (buttons)
                {
                    case MessageBoxButtons.YesNo:
                        dialog.AddAction("No", DialogResult.No, MaterialButtonVariant.Tonal);
                        dialog.AddAction("Yes", DialogResult.Yes, MaterialButtonVariant.Filled);
                        break;

                    case MessageBoxButtons.YesNoCancel:
                        dialog.AddAction("Cancel", DialogResult.Cancel, MaterialButtonVariant.Text);
                        dialog.AddAction("No", DialogResult.No, MaterialButtonVariant.Tonal);
                        dialog.AddAction("Yes", DialogResult.Yes, MaterialButtonVariant.Filled);
                        break;

                    case MessageBoxButtons.OKCancel:
                        dialog.AddAction("Cancel", DialogResult.Cancel, MaterialButtonVariant.Tonal);
                        dialog.AddAction("OK", DialogResult.OK, MaterialButtonVariant.Filled);
                        break;

                    default:
                        dialog.AddAction("OK", DialogResult.OK, MaterialButtonVariant.Filled);
                        break;
                }

                return effectiveOwner != null ? dialog.ShowDialog(effectiveOwner) : dialog.ShowDialog();
            }
        }

        private static (string glyph, Color color) ResolveIcon(string caption, MessageBoxButtons buttons)
        {
            string lower = caption.ToLowerInvariant();

            if (lower.Contains("error") || lower.Contains("fail") || lower.Contains("violation") || lower.Contains("invalid"))
            {
                return (MaterialIcons.ErrorFilled, MaterialColors.Error);
            }

            if (lower.Contains("warn") || lower.Contains("risk") || lower.Contains("abort") || lower.Contains("security") || lower.Contains("disabled"))
            {
                return (MaterialIcons.WarningFilled, MaterialColors.Warning);
            }

            if (buttons == MessageBoxButtons.YesNo || buttons == MessageBoxButtons.YesNoCancel)
            {
                return (MaterialIcons.Help, MaterialColors.Primary);
            }

            return (MaterialIcons.InfoFilled, MaterialColors.Primary);
        }
    }
}

