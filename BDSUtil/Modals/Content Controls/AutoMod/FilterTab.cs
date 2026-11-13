using BDSUtil.Helpers;
using BDSUtil.UI;
using Material3.WinForms.Controls;
using Material3.WinForms.Theming;
using Material3.WinForms.Typography;
using M3MaterialCheckBox = Material3.WinForms.Controls.MaterialCheckBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace BDSUtil.Modals.Content_Controls.AutoMod
{
    public partial class FilterTab : Material3UserControl
    {
        private readonly List<string> _bannedExpressionsCache = new List<string>();
        private string _deferredServerRootPath = string.Empty;

        private readonly Timer _batchTimer;
        private int _currentBatchIndex = 0;
        private const int BatchSize = 300;

        public FilterTab()
        {
            InitializeComponent();

            if (AddBTN != null) AddBTN.Click += AddBTN_Click;
            if (RemBTN != null) RemBTN.Click += RemBTN_Click;

            _batchTimer = new Timer { Interval = 1 };
            _batchTimer.Tick += BatchTimer_Tick;

            this.HandleCreated += FilterTab_HandleCreated;
        }

        public void InitializeControl(string serverRootPath)
        {
            if (string.IsNullOrEmpty(serverRootPath) || FilterCheckListBox == null) return;

            _deferredServerRootPath = serverRootPath;
            _batchTimer.Stop();

            FilterCheckListBox.Items.Clear();
            _bannedExpressionsCache.Clear();
            _currentBatchIndex = 0;

            string cacheFilePath = Path.Combine(serverRootPath, "profanity_filter.cache");
            string wlistPath = Path.Combine(serverRootPath, "profanity_filter.wlist");
            System.Threading.Tasks.Task.Run(() =>
            {
                try
                {
                    List<string> tempCacheList = new List<string>();

                    if (!File.Exists(cacheFilePath) && File.Exists(wlistPath))
                    {
                        StringBuilder sbBase64 = new StringBuilder();
                        using (StreamReader sr = new StreamReader(wlistPath, Encoding.UTF8, true))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                sbBase64.Append(line.Trim());
                            }
                        }

                        string cleanedBase64 = sbBase64.ToString()
                            .Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\t", "");

                        if (!string.IsNullOrEmpty(cleanedBase64))
                        {
                            int remainder = cleanedBase64.Length % 4;
                            if (remainder > 0) cleanedBase64 += new string('=', 4 - remainder);

                            byte[] decodedBytes = Convert.FromBase64String(cleanedBase64);
                            string translatedText = Encoding.UTF8.GetString(decodedBytes);

                            File.WriteAllText(cacheFilePath, translatedText, Encoding.UTF8);
                        }
                    }

                    if (File.Exists(cacheFilePath))
                    {
                        using (StreamReader sr = new StreamReader(cacheFilePath, Encoding.UTF8, true))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                string cleaned = line.Trim();
                                if (!string.IsNullOrWhiteSpace(cleaned) && !tempCacheList.Contains(cleaned))
                                {
                                    tempCacheList.Add(cleaned);
                                }
                            }
                        }
                    }

                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        _bannedExpressionsCache.AddRange(tempCacheList);
                        if (this.IsHandleCreated) PopulateListBoxItems();
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"[AutoMod Async Load Failure]: {ex}");
                }
            });
        }

        private void FilterTab_HandleCreated(object sender, EventArgs e)
        {
            PopulateListBoxItems();
        }
        private void PopulateListBoxItems()
        {
            if (FilterCheckListBox == null || _bannedExpressionsCache.Count == 0) return;

            _batchTimer.Stop();
            FilterCheckListBox.Items.Clear();

            _currentBatchIndex = _bannedExpressionsCache.Count - 1;
            _batchTimer.Start();
        }

        private void BatchTimer_Tick(object sender, EventArgs e)
        {
            if (FilterCheckListBox == null || _bannedExpressionsCache.Count == 0 || _currentBatchIndex < 0)
            {
                _batchTimer.Stop();
                return;
            }

            this.SuspendLayout();
            FilterCheckListBox.SuspendLayout();

            try
            {
                int itemsAddedInThisTick = 0;
                while (_currentBatchIndex >= 0 && itemsAddedInThisTick < BatchSize)
                {
                    string expression = _bannedExpressionsCache[_currentBatchIndex];
                    FilterCheckListBox.Items.Add(expression, false);

                    _currentBatchIndex--;
                    itemsAddedInThisTick++;
                }
            }
            finally
            {
                FilterCheckListBox.ResumeLayout(true);
                this.ResumeLayout(true);
            }

            if (_currentBatchIndex < 0) _batchTimer.Stop();
        }

        public List<string> GetBannedExpressions()
        {
            return _bannedExpressionsCache;
        }
        private void AddBTN_Click(object sender, EventArgs e)
        {
            using (Material3Form inputForm = new Material3Form())
            {
                inputForm.Text = "Add Banned Expression";
                inputForm.ClientSize = new System.Drawing.Size(420, 196);
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;
                inputForm.ShowInTaskbar = false;
                inputForm.Sizable = false;

                MaterialLabel promptLabel = new MaterialLabel
                {
                    Left = 20,
                    Top = 58,
                    Width = 380,
                    Height = 22,
                    BackColor = System.Drawing.Color.Transparent,
                    ForeColor = MaterialColors.OnSurfaceVariant,
                    Font = TypeScale.BodyMedium,
                    Text = "Enter keyword, slang phrase, or regex pattern:"
                };
                MaterialTextBoxEdit inputTextBox = new MaterialTextBoxEdit
                {
                    Left = 20,
                    Top = 86,
                    Width = 380,
                    Height = 46,
                    Hint = "Banned phrase or regex"
                };
                MaterialButton confirmButton = new MaterialButton
                {
                    Text = "OK",
                    Left = 228,
                    Width = 82,
                    Height = 36,
                    Top = 144,
                    AutoSize = false,
                    Variant = MaterialButtonVariant.Filled,
                    Tone = MaterialTone.Accent
                };
                MaterialButton cancelButton = new MaterialButton
                {
                    Text = "Cancel",
                    Left = 318,
                    Width = 82,
                    Height = 36,
                    Top = 144,
                    AutoSize = false,
                    Variant = MaterialButtonVariant.Outlined,
                    Tone = MaterialTone.Neutral
                };

                confirmButton.Click += (s, ev) => { inputForm.DialogResult = DialogResult.OK; inputForm.Close(); };
                cancelButton.Click += (s, ev) => { inputForm.DialogResult = DialogResult.Cancel; inputForm.Close(); };
                inputTextBox.KeyDown += (s, ke) =>
                {
                    if (ke.KeyCode == Keys.Enter)
                    {
                        ke.SuppressKeyPress = true;
                        inputForm.DialogResult = DialogResult.OK;
                        inputForm.Close();
                    }
                    else if (ke.KeyCode == Keys.Escape)
                    {
                        ke.SuppressKeyPress = true;
                        inputForm.DialogResult = DialogResult.Cancel;
                        inputForm.Close();
                    }
                };

                inputForm.Controls.Add(promptLabel);
                inputForm.Controls.Add(inputTextBox);
                inputForm.Controls.Add(confirmButton);
                inputForm.Controls.Add(cancelButton);
                inputForm.Shown += (s, ev) => inputTextBox.Focus();

                if (inputForm.ShowDialog(this) == DialogResult.OK)
                {
                    string inputPhrase = inputTextBox.Text.Trim();
                    if (string.IsNullOrEmpty(inputPhrase)) return;

                    if (!_bannedExpressionsCache.Contains(inputPhrase))
                    {
                        _bannedExpressionsCache.Add(inputPhrase);
                        FilterCheckListBox.Items.Add(inputPhrase, false);
                        SaveConfig(); // ✅ AUTOSAVE SYNC: Flush to disk right away
                    }
                    else
                    {
                        MaterialDialogBox.Show(this.ParentForm, "The entry you provided already exists.", "Duplicate Expression", MessageBoxButtons.OK);
                    }
                }
            }
        }

        private void RemBTN_Click(object sender, EventArgs e)
        {
            if (FilterCheckListBox == null || FilterCheckListBox.Items.Count == 0) return;

            List<M3MaterialCheckBox> entriesToRemove = new List<M3MaterialCheckBox>();
            List<string> phrasesToPurge = new List<string>();

            foreach (object item in FilterCheckListBox.Items)
            {
                if (item is M3MaterialCheckBox checkbox && checkbox.Checked)
                {
                    entriesToRemove.Add(checkbox);
                    phrasesToPurge.Add(checkbox.Text.Trim());
                }
            }

            if (entriesToRemove.Count == 0)
            {
                MaterialDialogBox.Show(this.ParentForm, "No expression listings are currently checked for removal.", "Selection Empty", MessageBoxButtons.OK);
                return;
            }

            _bannedExpressionsCache.RemoveAll(phrase => phrasesToPurge.Contains(phrase, StringComparer.OrdinalIgnoreCase));

            // ✅ AUTOSAVE SYNC: Rebuild disk data arrays, clear deep child layout controls, and reload safely from cache
            SaveConfig();
            PopulateListBoxItems();
        }

        /// <summary>
        /// ✅ REAL-TIME SERIALIZER: Compiles data structures and instantly flushes clean assets down to disk streams.
        /// </summary>
        public void SaveConfig()
        {
            if (string.IsNullOrEmpty(_deferredServerRootPath) || !Directory.Exists(_deferredServerRootPath)) return;

            try
            {
                string wlistPath = Path.Combine(_deferredServerRootPath, "profanity_filter.wlist");
                string autoModConfigFilePath = Path.Combine(_deferredServerRootPath, "automod.json");
                string cacheFilePath = Path.Combine(_deferredServerRootPath, "profanity_filter.cache");

                StringBuilder sb = new StringBuilder();
                foreach (string expression in _bannedExpressionsCache)
                {
                    sb.AppendLine(expression);
                }
                string plainTextData = sb.ToString();

                // 1. Output the clean Base64 encrypted game script addon package file
                byte[] bytesToEncode = Encoding.UTF8.GetBytes(plainTextData);
                string base64Payload = Convert.ToBase64String(bytesToEncode);
                File.WriteAllText(wlistPath, base64Payload, Encoding.UTF8);

                // 2. Output the server data mapping rules array configuration file as valid JSON
                string autoModJson = JsonSerializer.Serialize(_bannedExpressionsCache, JsonHelper.IndentedOptions);
                File.WriteAllText(autoModConfigFilePath, autoModJson, Encoding.UTF8);

                // 3. Keep the decrypted cache tracking file synchronized up-to-date
                File.WriteAllText(cacheFilePath, plainTextData, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[AutoMod Real-Time Save Violation Failure]: {ex.Message}");
            }
        }

        private void SABTN_Click(object sender, EventArgs e)
        {
            if (FilterCheckListBox == null || FilterCheckListBox.Items.Count == 0) return;
            this.SuspendLayout(); FilterCheckListBox.SuspendLayout();
            try { foreach (object item in FilterCheckListBox.Items) { if (item is M3MaterialCheckBox cb) cb.Checked = true; } }
            finally { FilterCheckListBox.ResumeLayout(true); this.ResumeLayout(true); }
        }

        private void DSABTN_Click(object sender, EventArgs e)
        {
            if (FilterCheckListBox == null || FilterCheckListBox.Items.Count == 0) return;
            this.SuspendLayout(); FilterCheckListBox.SuspendLayout();
            try { foreach (object item in FilterCheckListBox.Items) { if (item is M3MaterialCheckBox cb) cb.Checked = false; } }
            finally { FilterCheckListBox.ResumeLayout(true); this.ResumeLayout(true); }
        }
    }
}
