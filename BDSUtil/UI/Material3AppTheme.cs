using System;
using System.Drawing;
using System.Windows.Forms;
using Material3.Core;
using Material3.WinForms.Controls;
using Material3.WinForms.Theming;
using Material3.WinForms.Typography;

namespace BDSUtil.UI
{
    /// <summary>
    /// Central Material 3 theme bootstrapper and shared dark-surface helpers for BDSUtil.
    /// </summary>
    public static class Material3AppTheme
    {
        private static bool _initialized;
        private static readonly object _sync = new object();

        // BlueGrey 500 seed preserves BDSUtil's dark slate/blue-grey aesthetic in Material 3 tonal palettes.
        private static readonly Color DefaultSeedColor = Color.FromArgb(0x60, 0x7D, 0x8B);

        /// <summary>
        /// Applies the Material 3 dark color scheme idempotently before forms or controls initialize.
        /// </summary>
        public static void EnsureInitialized()
        {
            if (_initialized)
            {
                return;
            }

            lock (_sync)
            {
                if (_initialized)
                {
                    return;
                }

                MaterialTheme theme = MaterialTheme.FromSeed(DefaultSeedColor.ToCore(), SchemeVariant.TonalSpot);
                ThemeManager.Apply(theme, isDark: true);
                _initialized = true;
            }
        }

        /// <summary>
        /// Applies Material 3 surface colors to a control container so child controls never see light SystemColors backgrounds.
        /// </summary>
        public static void ApplySurfaceTheme(Control control, bool useContainerLow = false)
        {
            if (control == null)
            {
                return;
            }

            EnsureInitialized();

            void Apply()
            {
                if (control.IsDisposed)
                {
                    return;
                }

                control.BackColor = useContainerLow ? MaterialColors.SurfaceContainerLow : MaterialColors.Surface;
                control.ForeColor = MaterialColors.OnSurface;
            }

            Apply();
            ThemeHook.Attach(control, Apply);
        }

        /// <summary>
        /// Styles a standard label with Material 3 typography and foreground colors.
        /// </summary>
        public static void ConfigureLabel(Material3.WinForms.Controls.MaterialLabel label, Font font = null, bool subtle = false)
        {
            if (label == null)
            {
                return;
            }

            EnsureInitialized();
            if (font != null)
            {
                label.Font = font;
            }

            void Apply()
            {
                if (label.IsDisposed)
                {
                    return;
                }

                label.ForeColor = subtle ? MaterialColors.OnSurfaceVariant : MaterialColors.OnSurface;
                label.BackColor = Color.Transparent;
            }

            Apply();
            ThemeHook.Attach(label, Apply);
        }

        /// <summary>
        /// Configures a multi-line or bordered console/input TextBox with matching Material 3 dark surface tokens.
        /// </summary>
        public static void StyleNativeTextBox(TextBoxBase textBox, bool recessedContainer = true)
        {
            if (textBox == null)
            {
                return;
            }

            EnsureInitialized();

            void Apply()
            {
                if (textBox.IsDisposed)
                {
                    return;
                }

                textBox.BackColor = recessedContainer ? MaterialColors.SurfaceContainerLowest : MaterialColors.SurfaceContainer;
                textBox.ForeColor = MaterialColors.OnSurface;
            }

            Apply();
            ThemeHook.Attach(textBox, Apply);
        }
    }
}

