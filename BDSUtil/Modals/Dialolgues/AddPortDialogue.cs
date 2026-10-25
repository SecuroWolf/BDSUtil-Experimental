using System;
using System.Drawing;
using System.Windows.Forms;
using ReaLTaiizor.Forms;
using ReaLTaiizor.Controls;
using ReaLTaiizor.Manager; // Added to resolve the MaterialSkinManager namespace wrapper

namespace BDSUtil.Modals
{
    public partial class AddPortDialogue : MaterialForm
    {
        public int ExternalPort { get; private set; } = 0;
        public int InternalPort { get; private set; } = 0;
        public bool IsConfirmed { get; private set; } = false;

        private readonly MaterialTextBoxEdit extPortBox;
        private readonly MaterialTextBoxEdit intPortBox;
        private readonly MaterialSwitch splitPortSwitch;
        private readonly MaterialLabel intPortLabel;
        private readonly MaterialButton btnSave;

        public AddPortDialogue()
        {
            // Initial dimensions for the compact view layer state
            this.Width = 380;
            this.Height = 240;
            this.Text = "Add Port Mapping Node";
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.Sizable = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterParent;

            // FIX: Explicitly inherit your main application's theme colors so text turns dark/light correctly
            var skinManager = ReaLTaiizor.Manager.MaterialSkinManager.Instance;
            skinManager.AddFormToManage(this);

            // External Port Text Box Layout Configuration
            extPortBox = new MaterialTextBoxEdit()
            {
                Left = 20,
                Top = 85,
                Width = 340,
                Hint = "External Port (e.g. 19134)"
            };
            extPortBox.KeyPress += OnlyAllowNumericInputs;

            // Split Port Mapping Toggle Switch Control
            splitPortSwitch = new MaterialSwitch()
            {
                Text = "Map to a different internal port",
                Left = 20,
                Top = 145,
                Width = 340,
                Height = 30,
                UseAccentColor = false
            };
            splitPortSwitch.CheckedChanged += SplitPortSwitch_CheckedChanged;

            // Hidden Destination Internal Port Controls
            intPortLabel = new MaterialLabel()
            {
                Text = "Specify Destination Internal Port Node:",
                Left = 20,
                Top = 190,
                AutoSize = true,
                Visible = false,
                FontType = ReaLTaiizor.Manager.MaterialSkinManager.FontType.Body2
            };

            intPortBox = new MaterialTextBoxEdit()
            {
                Left = 20,
                Top = 215,
                Width = 340,
                Hint = "Internal Port (e.g. 19132)"
            };
            intPortBox.KeyPress += OnlyAllowNumericInputs;

            // Window Action Controls
            btnSave = new MaterialButton()
            {
                Text = "Confirm Node",
                Left = 20,
                Top = 190,
                Width = 150,
                Type = MaterialButton.MaterialButtonType.Contained
            };
            btnSave.Click += BtnSave_Click;

            MaterialButton btnCancel = new MaterialButton()
            {
                Text = "Cancel",
                Left = 210,
                Top = 190,
                Width = 150,
                Type = MaterialButton.MaterialButtonType.Outlined
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
                this.Height = 330;
                intPortLabel.Visible = true;
                intPortBox.Visible = true;

                btnSave.Top = 275;
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is MaterialButton btn && btn.Text == "Cancel") btn.Top = 275;
                }
            }
            else
            {
                this.Height = 250;
                intPortLabel.Visible = false;
                intPortBox.Visible = false;
                intPortBox.Text = string.Empty;

                btnSave.Top = 195;
                foreach (Control ctrl in this.Controls)
                {
                    if (ctrl is MaterialButton btn && btn.Text == "Cancel") btn.Top = 195;
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(extPortBox.Text, out int extParsed) || extParsed < 1 || extParsed > 65535)
            {
                MaterialMessageBox.Show(this, "Please provide a valid External Port integer between 1 and 65535.", "Input Boundary Violation");
                return;
            }

            int intParsed = extParsed;
            if (splitPortSwitch.Checked)
            {
                if (!int.TryParse(intPortBox.Text, out intParsed) || intParsed < 1 || intParsed > 65535)
                {
                    MaterialMessageBox.Show(this, "Please provide a valid Internal Port integer between 1 and 65535.", "Input Boundary Violation");
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
