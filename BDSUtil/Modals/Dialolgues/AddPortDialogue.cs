using System;
using System.Drawing;
using System.Windows.Forms;
using BDSUtil.UI;
using Material3.WinForms.Controls;
using Material3.WinForms.Typography;
using M3MaterialButton = Material3.WinForms.Controls.MaterialButton;
using M3MaterialLabel = Material3.WinForms.Controls.MaterialLabel;
using M3MaterialSwitch = Material3.WinForms.Controls.MaterialSwitch;

namespace BDSUtil.Modals
{
    public partial class AddPortDialogue : Material3Form
    {
        public int ExternalPort { get; private set; } = 0;
        public int InternalPort { get; private set; } = 0;
        public bool IsConfirmed { get; private set; } = false;

        private readonly MaterialTextBoxEdit extPortBox;
        private readonly MaterialTextBoxEdit intPortBox;
        private readonly M3MaterialSwitch splitPortSwitch;
        private readonly M3MaterialLabel intPortLabel;
        private readonly M3MaterialButton btnSave;
        private readonly M3MaterialButton btnCancel;

        public AddPortDialogue()
        {
            this.Width = 380;
            this.Height = 230;
            this.Text = "Add Port Mapping Node";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;
            SyncTitleBarMetadata();

            extPortBox = new MaterialTextBoxEdit
            {
                Left = 20,
                Top = 58,
                Width = 340,
                Height = 46,
                InputKind = MaterialInputKind.Integer,
                Hint = "External Port (e.g. 19133)"
            };
            extPortBox.KeyPress += OnlyAllowNumericInputs;

            splitPortSwitch = new M3MaterialSwitch
            {
                Text = "Map to a different internal port",
                Left = 20,
                Top = 114,
                AutoSize = false,
                Width = 340,
                Height = 32
            };
            splitPortSwitch.CheckedChanged += SplitPortSwitch_CheckedChanged;

            intPortLabel = new M3MaterialLabel
            {
                Text = "Specify Destination Internal Port Node:",
                Left = 20,
                Top = 154,
                AutoSize = true,
                Visible = false
            };
            Material3AppTheme.ConfigureLabel(intPortLabel, TypeScale.BodySmall, subtle: true);

            intPortBox = new MaterialTextBoxEdit
            {
                Left = 20,
                Top = 176,
                Width = 340,
                Height = 46,
                InputKind = MaterialInputKind.Integer,
                Hint = "Internal Port (e.g. 19132)"
            };
            intPortBox.KeyPress += OnlyAllowNumericInputs;

            btnSave = new M3MaterialButton
            {
                Text = "Confirm Node",
                Left = 20,
                Top = 165,
                Width = 150,
                Height = 40,
                Variant = MaterialButtonVariant.Filled
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new M3MaterialButton
            {
                Text = "Cancel",
                Left = 210,
                Top = 165,
                Width = 150,
                Height = 40,
                Variant = MaterialButtonVariant.Outlined
            };
            btnCancel.Click += (s, e) => { this.Close(); };

            this.Controls.Add(extPortBox);
            this.Controls.Add(splitPortSwitch);
            this.Controls.Add(intPortLabel);
            this.Controls.Add(intPortBox);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
            this.AcceptButton = btnSave;

            UpdateWindowLayoutBounds(false);
        }

        private void OnlyAllowNumericInputs(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void SplitPortSwitch_CheckedChanged(object sender, EventArgs e)
        {
            UpdateWindowLayoutBounds(splitPortSwitch.Checked);
        }

        private void UpdateWindowLayoutBounds(bool isExpanded)
        {
            if (isExpanded)
            {
                this.Height = 305;
                intPortLabel.Visible = true;
                intPortBox.Visible = true;

                btnSave.Top = 240;
                btnCancel.Top = 240;
            }
            else
            {
                this.Height = 225;
                intPortLabel.Visible = false;
                intPortBox.Visible = false;
                intPortBox.Text = string.Empty;

                btnSave.Top = 162;
                btnCancel.Top = 162;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(extPortBox.Text, out int extParsed) || extParsed < 1 || extParsed > 65535)
            {
                MaterialDialogBox.Show(this, "Please provide a valid External Port integer between 1 and 65535.", "Input Boundary Violation");
                return;
            }

            int intParsed = extParsed;
            if (splitPortSwitch.Checked)
            {
                if (!int.TryParse(intPortBox.Text, out intParsed) || intParsed < 1 || intParsed > 65535)
                {
                    MaterialDialogBox.Show(this, "Please provide a valid Internal Port integer between 1 and 65535.", "Input Boundary Violation");
                    return;
                }
            }

            this.ExternalPort = extParsed;
            this.InternalPort = intParsed;
            this.IsConfirmed = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
