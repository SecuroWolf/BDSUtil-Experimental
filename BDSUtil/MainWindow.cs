using BDSUtil.Modals;
using ReaLTaiizor.Colors; // Core Material Color templates
using ReaLTaiizor.Forms; // Core ReaLTaiizor MaterialForm base class mapping
using ReaLTaiizor.Manager; // Unified skin and theme scheme manager hooks
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace BDSUtil
{
    public partial class MainWindow : MaterialForm
    {
        private readonly MaterialSkinManager _materialSkinManager;
        private Process _serverProcess;
        private string _currentLogPath;
        private int _previousActiveTabIndex = 0;

        // State flag to signal if a backup sequence is waiting for the active server exit thread event
        private bool _isBackupPendingAfterStop = false;

        // --- BACKGROUND WORKER PIPELINE FIELDS ---
        private System.ComponentModel.BackgroundWorker _backupWorker;
        private int _activeBackupType = 0; // 1 = Worlds, 2 = Packs, 3 = Full Server
        private string _backupDestinationRoot = string.Empty;
        private readonly string _winRarRegistryPath = @"C:\Program Files\WinRAR\WinRAR.exe";

                public MainWindow()
        {
            InitializeComponent();

            // Set up your universal ReaLTaiizor theme configuration layers cleanly
            _materialSkinManager = ReaLTaiizor.Manager.MaterialSkinManager.Instance;
            _materialSkinManager.EnforceBackcolorOnAllComponents = true;
            _materialSkinManager.AddFormToManage(this);
            _materialSkinManager.Theme = ReaLTaiizor.Manager.MaterialSkinManager.Themes.DARK;

            // THE DIRECT CORE ASSIGNMENT:
            // Uses the true, verified class names under the ReaLTaiizor.Colors folder hierarchy,
            // immediately satisfying the compiler with zero namespace errors!
            _materialSkinManager.ColorScheme = new ReaLTaiizor.Colors.MaterialColorScheme(
                ReaLTaiizor.Colors.MaterialPrimary.BlueGrey800,
                ReaLTaiizor.Colors.MaterialPrimary.BlueGrey900,
                ReaLTaiizor.Colors.MaterialPrimary.BlueGrey500,
                ReaLTaiizor.Colors.MaterialAccent.LightBlue200,
                ReaLTaiizor.Util.MaterialTextShade.WHITE
            );

            // Bind your main window frame to track navigation natively matching the demo
            this.DrawerTabControl = MaterialTabControl1;
            this.DrawerShowIconsWhenHidden = true;

            // Force our tracking cache variable to lock onto the console tab (Index 0) instantly on boot
            _previousActiveTabIndex = 0;
            MaterialTabControl1.SelectedIndex = 0;

            // Initialize background compilation tasks
            InitializeBackupEngine();
        }




        private void MainWindow_Load(object sender, EventArgs e)
        {
            //
        }

        private void InitializeBackupEngine()
        {
            _backupWorker = new System.ComponentModel.BackgroundWorker
            {
                WorkerReportsProgress = true, // MUST BE TRUE TO TRANSMIT PROGRESS VALUES
                WorkerSupportsCancellation = false
            };

            _backupWorker.DoWork += BackupWorker_DoWork;
            _backupWorker.ProgressChanged += BackupWorker_ProgressChanged; // NEW SELECTION
            _backupWorker.RunWorkerCompleted += BackupWorker_RunWorkerCompleted;
        }

        private void BackupWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            // Update the UI control bar elements safely straight out of the background worker thread reports
            BackupProgressBar.Value = e.ProgressPercentage;
        }


        private void StartServerBtn_Click(object sender, EventArgs e)
        {
            if (_serverProcess != null && !_serverProcess.HasExited) return;

            string serverDirectory = AppSettings.Instance.ServerExecutablePath;
            string fullBinaryPath = System.IO.Path.Combine(serverDirectory, "bedrock_server.exe");

            if (string.IsNullOrWhiteSpace(serverDirectory) || !System.IO.File.Exists(fullBinaryPath))
            {
                DialogResult promptResult = ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    "The Minecraft Bedrock Server path has not been configured, or 'bedrock_server.exe' could not be found.\n\n" +
                    "Would you like to open the application Settings panel now to link a valid server directory target path?",
                    "Server Routing Target Missing",
                    MessageBoxButtons.YesNo,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );

                if (promptResult == DialogResult.Yes)
                {
                    using (var settings = new SettingsForm())
                    {
                        if (settings.ShowDialog(this) == DialogResult.OK)
                        {
                            AppendTerminalLine("[MANAGER INFO] Configuration paths updated. Click 'Start Server' again to launch.");
                        }
                    }
                }
                return;
            }

            ConsoleOutputTextBox.Clear();
            ConsoleOutputTextBox.AppendText("Initializing Server Boot Pipeline...\r\n");

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmm");
            System.IO.Directory.CreateDirectory("logs");
            _currentLogPath = $@"logs\server_{timestamp}.log";

            _serverProcess = new Process();
            _serverProcess.StartInfo.FileName = fullBinaryPath;
            _serverProcess.StartInfo.WorkingDirectory = serverDirectory;
            _serverProcess.StartInfo.UseShellExecute = false;
            _serverProcess.StartInfo.CreateNoWindow = true;
            _serverProcess.StartInfo.RedirectStandardOutput = true;
            _serverProcess.StartInfo.RedirectStandardError = true;
            _serverProcess.StartInfo.RedirectStandardInput = true;

            _serverProcess.OutputDataReceived += (s, args) => AppendTerminalLine(args.Data);
            _serverProcess.ErrorDataReceived += (s, args) => AppendTerminalLine(args.Data);

            _serverProcess.EnableRaisingEvents = true;
            _serverProcess.Exited += ServerProcess_Exited;

            _serverProcess.Start();
            _serverProcess.BeginOutputReadLine();
            _serverProcess.BeginErrorReadLine();
        }

        private void StopServerBtn_Click(object sender, EventArgs e)
        {
            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                AppendTerminalLine("[MANAGER INFO] Injected administrative 'stop' command stream...");
                _serverProcess.StandardInput.WriteLine("stop");
            }
        }

        private void CommandInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string cmd = CommandInputTextBox.Text;

                if (string.IsNullOrWhiteSpace(cmd)) return;

                if (_serverProcess != null && !_serverProcess.HasExited)
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");
                    AppendTerminalLine($"[{timestamp} CONSOLE] {cmd}");
                    _serverProcess.StandardInput.WriteLine(cmd);
                    CommandInputTextBox.Clear();
                }

                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void CommandInputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
            }
        }

        private void AppendTerminalLine(string text)
        {
            if (string.IsNullOrEmpty(text)) return;

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<string>(AppendTerminalLine), text);
                return;
            }

            ConsoleOutputTextBox.AppendText(text + Environment.NewLine);
            ConsoleOutputTextBox.SelectionStart = ConsoleOutputTextBox.Text.Length;
            ConsoleOutputTextBox.ScrollToCaret();

            try
            {
                File.AppendAllText(_currentLogPath, text + Environment.NewLine);
            }
            catch { }
        }

        private void ServerProcess_Exited(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(ServerProcess_Exited), sender, e);
                return;
            }

            ConsoleOutputTextBox.AppendText(Environment.NewLine + "==================================================" + Environment.NewLine + "Server has shutdown successfully." + Environment.NewLine + "==================================================" + Environment.NewLine);

            ConsoleOutputTextBox.SelectionStart = ConsoleOutputTextBox.Text.Length;
            ConsoleOutputTextBox.ScrollToCaret();


            if (_isBackupPendingAfterStop)
            {
                _isBackupPendingAfterStop = false;
                ExecuteBackgroundBackupPipeline();
            }
        }

        private void MaterialTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Wipe out the visibility states immediately upon tab switching operations
            BKCompleteLBL.Visible = false;
            BackupProgressBar.Visible = false;

            int settingsTabIndex = MaterialTabControl1.TabCount - 1;

            if (MaterialTabControl1.SelectedIndex != settingsTabIndex)
            {
                _previousActiveTabIndex = MaterialTabControl1.SelectedIndex;
                return;
            }

            int snapshotIndex = _previousActiveTabIndex;
            this.Hide();

            using (var settings = new SettingsForm())
            {
                settings.ShowDialog(this);
            }

            this.Show();

            Timer restorationTimer = new Timer { Interval = 1 };
            restorationTimer.Tick += (s, args) =>
            {
                restorationTimer.Stop();
                MaterialTabControl1.SelectedIndex = snapshotIndex;
                MaterialTabControl1.Update();
                restorationTimer.Dispose();
            };
            restorationTimer.Start();
        }

        //#############################################################################
        //############################ MAIN BACKUP PIPELINE ###########################
        //#############################################################################

        private void BackupWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            var worker = sender as System.ComponentModel.BackgroundWorker;
            string serverDir = AppSettings.Instance.ServerExecutablePath;
            string timestamp = DateTime.Now.ToString("MM_dd_yyyy_HHmmss");
            int compressionType = AppSettings.Instance.CompressionMode;
            bool winRarExists = File.Exists(_winRarRegistryPath);

            if (!winRarExists && (compressionType == 0 || compressionType == 1))
            {
                compressionType = 2; // Auto-fallback lock if missing
            }

            string fileExtension = (compressionType == 0) ? ".rar" : ".zip";

            // Set naming prefix based on operation type
            string stagingFolderName = "Worlds_Backup_";
            if (_activeBackupType == 2) stagingFolderName = "Packs_Backup_";
            else if (_activeBackupType == 3) stagingFolderName = "Server_Backup_";

            string archiveName = stagingFolderName + timestamp + fileExtension;
            string finalArchiveDestination = Path.Combine(_backupDestinationRoot, archiveName);

            worker?.ReportProgress(5);
            System.Threading.Thread.Sleep(1500);

            // Create a dedicated local staging path for temporary assembly
            string tempBackupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "temp_bk_" + timestamp);

            try
            {
                // =========================================================================
                // BRANCH A: ADDON/PACKS EXTRACTION (TYPE 2)
                // =========================================================================
                if (_activeBackupType == 2)
                {
                    string[] targetPackFolders = new string[] { "behavior_packs", "resource_packs" };
                    bool foundAnyCustomPacks = false;

                    foreach (string packDirName in targetPackFolders)
                    {
                        string sourceRoot = Path.Combine(serverDir, packDirName);
                        if (!Directory.Exists(sourceRoot)) continue;

                        string[] customPackDirs = Directory.GetDirectories(sourceRoot, "custom_*", SearchOption.TopDirectoryOnly);

                        foreach (string packSourcePath in customPackDirs)
                        {
                            foundAnyCustomPacks = true;
                            string folderName = Path.GetFileName(packSourcePath);
                            string targetStagingRoot = Path.Combine(tempBackupPath, packDirName, folderName);

                            foreach (string filePath in Directory.GetFiles(packSourcePath, "*.*", SearchOption.AllDirectories))
                            {
                                string destFilePath = filePath.Replace(packSourcePath, targetStagingRoot);
                                string destFileDir = Path.GetDirectoryName(destFilePath);
                                if (!string.IsNullOrEmpty(destFileDir)) Directory.CreateDirectory(destFileDir);

                                using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    using (FileStream destStream = new FileStream(destFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                                    {
                                        sourceStream.CopyTo(destStream);
                                    }
                                }
                            }
                        }
                    }

                    if (!foundAnyCustomPacks)
                    {
                        throw new DirectoryNotFoundException("No active directory nodes starting with your prefix naming scheme 'custom_' were detected inside behavior or resource containers.");
                    }
                }
                // =========================================================================
                // BRANCH B: EXPLICIT FULL SERVER ARCHITECTURE SWEEP (TYPE 3)
                // =========================================================================
                else if (_activeBackupType == 3)
                {
                    // Explicit whitelist map extracted from server layout assets and requirements
                    string[] whitelistedItems = new string[]
                    {
                        // Standard base files from your archive capture
                        "allowlist.json",
                        "bedrock_server.exe",
                        "packetlimitconfig.json",
                        "permissions.json",
                        "profanity_filter.wlist",
                        "server.properties",
                        
                        // Core configuration and data folder blocks
                        "behavior_packs",
                        "config",
                        "data",
                        "definitions",
                        "resource_packs",
                        
                        // Requested development and template directory targets
                        "development_behavior_packs",
                        "development_resource_packs",
                        "development_skin_packs",
                        "world_templates",
                        "worlds"
                    };

                    foreach (string item in whitelistedItems)
                    {
                        string sourceFullPath = Path.Combine(serverDir, item);
                        string targetStagingRoot = Path.Combine(tempBackupPath, item);

                        if (File.Exists(sourceFullPath))
                        {
                            // Handle root configuration files cleanly
                            string destFileDir = Path.GetDirectoryName(targetStagingRoot);
                            if (!string.IsNullOrEmpty(destFileDir)) Directory.CreateDirectory(destFileDir);

                            using (FileStream sourceStream = new FileStream(sourceFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                using (FileStream destStream = new FileStream(targetStagingRoot, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    sourceStream.CopyTo(destStream);
                                }
                            }
                        }
                        else if (Directory.Exists(sourceFullPath))
                        {
                            // Sweep entire directory trees recursively using shared non-exclusive flags
                            foreach (string filePath in Directory.GetFiles(sourceFullPath, "*.*", SearchOption.AllDirectories))
                            {
                                string destFilePath = filePath.Replace(sourceFullPath, targetStagingRoot);
                                string destFileDir = Path.GetDirectoryName(destFilePath);
                                if (!string.IsNullOrEmpty(destFileDir)) Directory.CreateDirectory(destFileDir);

                                using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    using (FileStream destStream = new FileStream(destFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                                    {
                                        sourceStream.CopyTo(destStream);
                                    }
                                }
                            }
                        }
                    }
                }
                // =========================================================================
                // BRANCH C: STANDARD TOTAL WORLDS EXTRACTION (TYPE 1)
                // =========================================================================
                else
                {
                    string worldsSourceFolder = Path.Combine(serverDir, "worlds");
                    if (!Directory.Exists(worldsSourceFolder))
                    {
                        throw new DirectoryNotFoundException("The target 'worlds' directory could not be located inside the server root folder.");
                    }

                    foreach (string filePath in Directory.GetFiles(worldsSourceFolder, "*.*", SearchOption.AllDirectories))
                    {
                        string destFilePath = filePath.Replace(worldsSourceFolder, tempBackupPath);
                        string destFileDir = Path.GetDirectoryName(destFilePath);
                        if (!string.IsNullOrEmpty(destFileDir)) Directory.CreateDirectory(destFileDir);

                        using (FileStream sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (FileStream destStream = new FileStream(destFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                sourceStream.CopyTo(destStream);
                            }
                        }
                    }
                }

                worker?.ReportProgress(50);

                // =========================================================================
                // ARCHIVE COMPILATION PIPELINE CORES
                // =========================================================================
                if (compressionType == 0 || compressionType == 1)
                {
                    string archiveFlag = (compressionType == 1) ? " -afzip" : "";
                    string winRarArgs = "a -ep1 -r -ibck -dh" + archiveFlag + " \"" + finalArchiveDestination + "\" \"" + tempBackupPath + "\\*\"";

                    var rarInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = _winRarRegistryPath,
                        Arguments = winRarArgs,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (System.Diagnostics.Process rarProcess = System.Diagnostics.Process.Start(rarInfo))
                    {
                        if (rarProcess != null)
                        {
                            int fauxProgress = 55;
                            while (!rarProcess.HasExited)
                            {
                                System.Threading.Thread.Sleep(200);
                                fauxProgress += 5;
                                if (fauxProgress >= 95) fauxProgress = 55;
                                worker?.ReportProgress(fauxProgress);
                            }
                            rarProcess.WaitForExit();
                        }
                    }
                }
                else
                {
                    System.IO.Compression.ZipFile.CreateFromDirectory(tempBackupPath, finalArchiveDestination);
                }
            }
            finally
            {
                if (Directory.Exists(tempBackupPath))
                {
                    Directory.Delete(tempBackupPath, true);
                }
            }

            worker?.ReportProgress(100);
            e.Result = archiveName;
        }





        private void BackupWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            BackupWorldBTN.Enabled = true;
            BackupPacksBTN.Enabled = true;
            BackupServerBTN.Enabled = true;

            BackupProgressBar.Visible = false;

            if (e.Error != null)
            {
                AppendTerminalLine($"[MANAGER ERROR] Background compression failed: {e.Error.Message}");
            }
            else
            {
                string savedName = e.Result as string;
                AppendTerminalLine($"[CONSOLE] [SUCCESS] Backup package '{savedName}' successfully committed.");

                BKCompleteLBL.Visible = true;

                // --- PROMPT MODAL: CENTER LOCK TO PARENT CANVAS ---
                // By passing 'this' as the first parameter hook argument signature,
                // ReaLTaiizor anchors the dialog box bounding coordinates directly to the center 
                // of your main user window layout, preventing it from straying onto random monitor screens!
                DialogResult promptResult = ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    this,
                    $"Backup package successfully created:\n'{savedName}'\n\nWould you like to open the local Backups archive directory in File Explorer now?",
                    "Backup Operations Successful",
                    MessageBoxButtons.YesNo,
                    false, // Critical positioning flag to satisfy the method signature
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );

                if (promptResult == DialogResult.Yes)
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(_backupDestinationRoot) && Directory.Exists(_backupDestinationRoot))
                        {
                            Process.Start("explorer.exe", _backupDestinationRoot);
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendTerminalLine($"[MANAGER ERROR] Failed to automatically initialize File Explorer context loop: {ex.Message}");
                    }
                }
            }
        }

        private void ExecuteBackgroundBackupPipeline()
        {
            BackupWorldBTN.Enabled = false;
            BackupPacksBTN.Enabled = false; // NEW BLOCK LOCK
            BackupServerBTN.Enabled = false;

            BKCompleteLBL.Visible = false;
            BackupProgressBar.Value = 0;
            BackupProgressBar.Visible = true;

            _backupDestinationRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
            if (!Directory.Exists(_backupDestinationRoot))
            {
                Directory.CreateDirectory(_backupDestinationRoot);
            }

            AppendTerminalLine("[MANAGER INFO] Initializing background storage compilation loop...");
            _backupWorker.RunWorkerAsync();
        }



        //#############################################################################
        //############################## Restore Pipeline #############################
        //#############################################################################

        // A specialized data-holding structure to link clear visual tokens to raw files
        private class BackupRestoreItem
        {
            public string FileFullPath { get; set; }
            public string VisualTextString { get; set; }

            // Standard WinForms ListBox reads ToString() to paint the text lines flawlessly on screen
            public override string ToString()
            {
                return VisualTextString;
            }
        }

        public void PopulateRestoreSnapshotsList()
        {
            // 1. Reset state controls safely before sweeping disk directories
            BackupPointsListBox.Items.Clear();
            RunRestoreBTN.Visible = false;

            string backupRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
            if (!Directory.Exists(backupRoot)) return;

            // Target mapping evaluation: 0 = Worlds, 1 = Packs, 2 = Server
            int targetType = RestoreTypeComboBox.SelectedIndex;
            string matchPrefix = "Worlds_Backup_";
            if (targetType == 1) matchPrefix = "Packs_Backup_";
            else if (targetType == 2) matchPrefix = "Server_Backup_";

            try
            {
                string[] files = Directory.GetFiles(backupRoot, matchPrefix + "*.*", SearchOption.TopDirectoryOnly);

                foreach (string filePath in files)
                {
                    string filename = Path.GetFileNameWithoutExtension(filePath);
                    string rawTimestamp = filename.Replace(matchPrefix, "");
                    string[] timeTokens = rawTimestamp.Split('_');

                    if (timeTokens.Length >= 4)
                    {
                        string month = timeTokens[0];
                        string day = timeTokens[1];
                        string year = timeTokens[2];

                        string timeString = timeTokens[3];
                        string hourStr = timeString.Substring(0, 2);
                        string minStr = timeString.Substring(2, 2);
                        string secStr = timeString.Substring(4, 2);

                        string readableDate = month + "/" + day + "/" + year + " @ " + hourStr + ":" + minStr + ":" + secStr;
                        string extension = Path.GetExtension(filePath).ToUpper().Replace(".", "");

                        // Instantiate ReaLTaiizor's native list container item cleanly.
                        // We store the vital disk path securely right inside the SecondaryText property block!
                        var item = new ReaLTaiizor.Child.Material.MaterialListBoxItem
                        {
                            Text = "📅  " + readableDate,
                            SecondaryText = filePath // This acts as our hidden tag parameter container
                        };

                        BackupPointsListBox.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                AppendTerminalLine("[MANAGER ERROR] Failed to load restore points index map: " + ex.Message);
            }
        }


        private void RestoreTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateRestoreSnapshotsList();
        }

        // --- NEW SELECTION CHECK HOOK ---
        // Click event/SelectedIndexChanged wire-up for your MaterialListBox container canvas
        private void BackupPointsListBox_SelectedIndexChanged(object sender, ReaLTaiizor.Child.Material.MaterialListBoxItem selectedItem)
        {
            // If nothing is selected or the item is empty, ensure the button stays hidden
            if (selectedItem == null || string.IsNullOrEmpty(selectedItem.SecondaryText))
            {
                RunRestoreBTN.Visible = false;
                return;
            }

            // Reveal the button immediately when a valid snapshot row is highlighted!
            RunRestoreBTN.Visible = true;
        }


        //#############################################################################
        //########################### Snapshot Page BTNs ##############################
        //#############################################################################

        private void BackupWorldBTN_Click(object sender, EventArgs e)
        {
            if (_backupWorker.IsBusy) return;

            string serverDir = AppSettings.Instance.ServerExecutablePath;
            if (string.IsNullOrWhiteSpace(serverDir) || !System.IO.Directory.Exists(serverDir))
            {
                ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    "Please configure a valid server path in Settings before running backup operations.",
                    "Missing Configuration Parameters",
                    MessageBoxButtons.OK,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );
                return;
            }

            _activeBackupType = 1;

            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                DialogResult result = ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    "The Minecraft Bedrock server must be completely stopped before a backup can safely compile to prevent LevelDB file corruption.\n\n" +
                    "Would you like to issue a clean 'stop' command to the server now and automatically proceed with the backup once it fully exits?",
                    "Server Shutdown Required",
                    MessageBoxButtons.YesNo,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );

                if (result == DialogResult.Yes)
                {
                    _isBackupPendingAfterStop = true;
                    BackupWorldBTN.Enabled = false;
                    BackupServerBTN.Enabled = false;

                    AppendTerminalLine("[CONSOLE] [MANAGER INTERCEPT] Issuing server shutdown request for pending backup...");
                    _serverProcess.StandardInput.WriteLine("stop");
                }
                return;
            }

            ExecuteBackgroundBackupPipeline();
        }

        private void BackupPacksBTN_Click(object sender, EventArgs e)
        {
            if (_backupWorker.IsBusy) return;

            string serverDir = AppSettings.Instance.ServerExecutablePath;
            if (string.IsNullOrWhiteSpace(serverDir) || !System.IO.Directory.Exists(serverDir))
            {
                ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    "Please configure a valid server path in Settings before running backup operations.",
                    "Missing Configuration Parameters",
                    MessageBoxButtons.OK,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );
                return;
            }

            // TYPE 2 signals a specialized Packs backup sequence to the background worker thread
            _activeBackupType = 2;

            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                DialogResult result = ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    "The Minecraft Bedrock server must be completely stopped before a backup can safely compile to prevent file corruption.\n\n" +
                    "Would you like to issue a clean 'stop' command to the server now and automatically proceed with the backup once it fully exits?",
                    "Server Shutdown Required",
                    MessageBoxButtons.YesNo,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );

                if (result == DialogResult.Yes)
                {
                    _isBackupPendingAfterStop = true;
                    BackupWorldBTN.Enabled = false;
                    BackupPacksBTN.Enabled = false;
                    BackupServerBTN.Enabled = false;

                    AppendTerminalLine("[CONSOLE] [MANAGER INTERCEPT] Issuing server shutdown request for pending packs backup...");
                    _serverProcess.StandardInput.WriteLine("stop");
                }
                return;
            }

            ExecuteBackgroundBackupPipeline();
        }

        private void BackupServerBTN_Click(object sender, EventArgs e)
        {
            if (_backupWorker.IsBusy) return;

            string serverDir = AppSettings.Instance.ServerExecutablePath;
            if (string.IsNullOrWhiteSpace(serverDir) || !System.IO.Directory.Exists(serverDir))
            {
                ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    "Please configure a valid server path in Settings before running backup operations.",
                    "Missing Configuration Parameters",
                    MessageBoxButtons.OK,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );
                return;
            }

            // TYPE 3 signals a Full Server backup sequence to the background worker thread
            _activeBackupType = 3;

            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                DialogResult result = ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    "The Minecraft Bedrock server must be completely stopped before a backup can safely compile to prevent file corruption.\n\n" +
                    "Would you like to issue a clean 'stop' command to the server now and automatically proceed with the backup once it fully exits?",
                    "Server Shutdown Required",
                    MessageBoxButtons.YesNo,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );

                if (result == DialogResult.Yes)
                {
                    _isBackupPendingAfterStop = true;
                    BackupWorldBTN.Enabled = false;
                    BackupPacksBTN.Enabled = false;
                    BackupServerBTN.Enabled = false;

                    AppendTerminalLine("[CONSOLE] [MANAGER INTERCEPT] Issuing server shutdown request for full server backup...");
                    _serverProcess.StandardInput.WriteLine("stop");
                }
                return;
            }

            ExecuteBackgroundBackupPipeline();
        }

        private void RunRestoreBTN_Click(object sender, EventArgs e)
        {
            // Extract the hidden configuration path string directly out of the secondary metadata tracker property
            if (!(BackupPointsListBox.SelectedItem is ReaLTaiizor.Child.Material.MaterialListBoxItem selectedItem) || string.IsNullOrEmpty(selectedItem.SecondaryText))
            {
                RunRestoreBTN.Visible = false;
                return;
            }

            // 1. OPERATION STATE CEILING: Block execution passes if the server binary loop is active
            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    this,
                    "The Bedrock Server process is currently active. You must completely 'Stop Server' before overwriting server states to avoid catastrophic data corruption.",
                    "Active Server Process Blocked",
                    MessageBoxButtons.OK,
                    false,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );
                return;
            }

            // 2. DEFENSIVE CONFIRMATION DIALOG MODAL PROMPT
            DialogResult confirmResult = ReaLTaiizor.Controls.MaterialMessageBox.Show(
                this,
                "WARNING: Proceeding will completely OVERWRITE your existing data matching the snapshot type chosen.\n\nAre you absolutely sure you want to execute recovery?",
                "Confirm Snapshot Overwrite",
                MessageBoxButtons.YesNo,
                false,
                ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
            );

            if (confirmResult != DialogResult.Yes) return;

            string serverDir = AppSettings.Instance.ServerExecutablePath;
            int targetType = RestoreTypeComboBox.SelectedIndex; // 0 = Worlds, 1 = Packs, 2 = Server
            string archivePath = selectedItem.SecondaryText;
            string extension = Path.GetExtension(archivePath).ToLower();

            try
            {
                AppendTerminalLine("[RESTORE INITIALIZED] Preparing target environment directories...");

                // 3. DEFENSIVE DESTINATION TARGET CONTAINER SCRUBBING
                if (targetType == 0) // Worlds Only
                {
                    string targetFolder = Path.Combine(serverDir, "worlds");
                    if (Directory.Exists(targetFolder)) Directory.Delete(targetFolder, true);
                }
                else if (targetType == 1) // Custom Packs Only
                {
                    string[] parentContainers = new string[] { "behavior_packs", "resource_packs" };
                    foreach (string container in parentContainers)
                    {
                        string containerPath = Path.Combine(serverDir, container);
                        if (!Directory.Exists(containerPath)) continue;

                        string[] customDirs = Directory.GetDirectories(containerPath, "custom_*", SearchOption.TopDirectoryOnly);
                        foreach (string dir in customDirs) Directory.Delete(dir, true);
                    }
                }
                else if (targetType == 2) // Full Server Override - Scrub directories AND loose files
                {
                    string[] serverFolders = new string[] {
                        "behavior_packs", "config", "data", "definitions", "resource_packs",
                        "development_behavior_packs", "development_resource_packs", "development_skin_packs",
                        "world_templates", "worlds"
                    };
                    foreach (string folder in serverFolders)
                    {
                        string targetFolder = Path.Combine(serverDir, folder);
                        if (Directory.Exists(targetFolder)) Directory.Delete(targetFolder, true);
                    }

                    // Explicitly scrub loose root json/configuration files so extraction never conflicts
                    string[] rootConfigFileNames = new string[] {
                        "allowlist.json", "packetlimitconfig.json", "permissions.json",
                        "profanity_filter.wlist", "server.properties"
                    };
                    foreach (string fileName in rootConfigFileNames)
                    {
                        string targetFile = Path.Combine(serverDir, fileName);
                        if (File.Exists(targetFile)) File.Delete(targetFile);
                    }
                }

                AppendTerminalLine("[RESTORE EXTRACTION] Extracting archive file bytes down to disk blocks...");

                // 4. EXTRACTION PIPELINE DISPATCH
                if (extension == ".rar" || (extension == ".zip" && File.Exists(_winRarRegistryPath) && AppSettings.Instance.CompressionMode != 2))
                {
                    // WinRAR Extraction: "x" extracts with full paths, "-o+" forces overwrite confirmation bypasses
                    string winRarArgs = "x -o+ \"" + archivePath + "\" \"" + serverDir + "\\\"";

                    var rarInfo = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = _winRarRegistryPath,
                        Arguments = winRarArgs,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (System.Diagnostics.Process rarProcess = System.Diagnostics.Process.Start(rarInfo))
                    {
                        rarProcess?.WaitForExit();
                    }
                }
                else
                {
                    // Full .NET Framework compatible file streaming iteration loop
                    using (System.IO.Compression.ZipArchive archive = System.IO.Compression.ZipFile.OpenRead(archivePath))
                    {
                        foreach (System.IO.Compression.ZipArchiveEntry entry in archive.Entries)
                        {
                            string destinationPath = Path.GetFullPath(Path.Combine(serverDir, entry.FullName));

                            if (entry.FullName.EndsWith("/") || entry.FullName.EndsWith("\\"))
                            {
                                if (!Directory.Exists(destinationPath))
                                {
                                    Directory.CreateDirectory(destinationPath);
                                }
                            }
                            else
                            {
                                string directoryPath = Path.GetDirectoryName(destinationPath);
                                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                                {
                                    Directory.CreateDirectory(directoryPath);
                                }

                                // Overwrite existing loose files cleanly by passing true
                                entry.ExtractToFile(destinationPath, true);
                            }
                        }
                    }
                }

                AppendTerminalLine("[CONSOLE] [SUCCESS] Snapshot recovery operation successfully committed.");

                RunRestoreBTN.Visible = false;
                PopulateRestoreSnapshotsList(); // Refresh list layout natively

                ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    this,
                    "Snapshot recovery extraction loop finished successfully! Your active server environment has been rolled back.",
                    "Recovery Process Complete",
                    MessageBoxButtons.OK,
                    false,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );
            }
            catch (Exception ex)
            {
                AppendTerminalLine("[MANAGER ERROR] Recovery execution pipeline failed: " + ex.Message);
                ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    this,
                    "A critical exception error occurred during restoration loops:\n\n" + ex.Message,
                    "Recovery Fatal Failure",
                    MessageBoxButtons.OK,
                    false,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );
            }
        }

        // Add a private tracking flag at the top of your class fields block inside MainWindow.cs
        private bool _forceSystemExit = false;

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // If our private force flag is true, bypass all prompts and let the window exit cleanly!
            if (_forceSystemExit)
            {
                return;
            }

            // If the server process tracking variable is empty or already dead, exit normally
            if (_serverProcess == null || _serverProcess.HasExited)
            {
                return;
            }

            // INTERCEPT EXIT ATTEMPT: Launch your custom modal button form layout panel
            using (CloseConfirmForm confirmDialog = new CloseConfirmForm())
            {
                if (confirmDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string action = confirmDialog.SelectedExitAction;

                    if (action == "CLEAN_STOP") // USER CLICKED "YES (CLEAN STOP)"
                    {
                        e.Cancel = true; // Suspend application termination pass frames temporarily
                        this.Enabled = false; // Block parent window input to prevent double clicks

                        try
                        {
                            AppendTerminalLine("[EXIT CONSOLE INTERCEPT] Processing clean administrative shutdown sequence...");

                            // Unsubscribe from standard runtime exit event to stop a thread race collision
                            _serverProcess.Exited -= ServerProcess_Exited;

                            // Wire a specialized event loop to forcefully exit the entire process 
                            // environment the precise millisecond the server drops clear, bypassing recursion!
                            _serverProcess.Exited += (s, args) => {
                                _forceSystemExit = true;
                                Environment.Exit(0);
                            };

                            // Pipe the clean close stream command straight down the input register
                            _serverProcess.StandardInput.WriteLine("stop");
                        }
                        catch
                        {
                            // Hard safety fallback ceiling if input piping handles throw an exception
                            try { _serverProcess.Kill(); } catch { }
                            _forceSystemExit = true;
                            Environment.Exit(0);
                        }
                    }
                    else if (action == "FORCE_KILL") // USER CLICKED "FORCE STOP"
                    {
                        try
                        {
                            AppendTerminalLine("[EXIT CONSOLE INTERCEPT] Forcefully terminating active background server tree...");
                            _serverProcess.Kill();
                        }
                        catch { }

                        // Trip the force flag true to allow the window exit pass to drop without re-triggering the prompt modal
                        _forceSystemExit = true;
                        Application.Exit();
                    }
                    else // FALLBACK PROTECTION LOOP
                    {
                        e.Cancel = true;
                    }
                }
                else // USER CLICKED "NO" OR CLOSED THE MODAL WINDOW
                {
                    e.Cancel = true; // Discard the closing window frame context and return focus to console
                }
            }
        }

        private void StartServerBTN_New_Click(object sender, EventArgs e)
        {
            StartServerBTN_New.Hide();
            if (_serverProcess != null && !_serverProcess.HasExited) return;

            string serverDirectory = AppSettings.Instance.ServerExecutablePath;
            string fullBinaryPath = System.IO.Path.Combine(serverDirectory, "bedrock_server.exe");

            if (string.IsNullOrWhiteSpace(serverDirectory) || !System.IO.File.Exists(fullBinaryPath))
            {
                DialogResult promptResult = ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    "The Minecraft Bedrock Server path has not been configured, or 'bedrock_server.exe' could not be found.\n\n" +
                    "Would you like to open the application Settings panel now to link a valid server directory target path?",
                    "Server Routing Target Missing",
                    MessageBoxButtons.YesNo,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );

                if (promptResult == DialogResult.Yes)
                {
                    using (var settings = new SettingsForm())
                    {
                        if (settings.ShowDialog(this) == DialogResult.OK)
                        {
                            AppendTerminalLine("[MANAGER INFO] Configuration paths updated. Click 'Start Server' again to launch.");
                        }
                    }
                }
                return;
            }

            ConsoleOutputTextBox.Clear();
            ConsoleOutputTextBox.AppendText("Initializing Server Boot Pipeline...\r\n");


            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmm");
            System.IO.Directory.CreateDirectory("logs");
            _currentLogPath = $@"logs\server_{timestamp}.log";

            _serverProcess = new Process();
            _serverProcess.StartInfo.FileName = fullBinaryPath;
            _serverProcess.StartInfo.WorkingDirectory = serverDirectory;
            _serverProcess.StartInfo.UseShellExecute = false;
            _serverProcess.StartInfo.CreateNoWindow = true;
            _serverProcess.StartInfo.RedirectStandardOutput = true;
            _serverProcess.StartInfo.RedirectStandardError = true;
            _serverProcess.StartInfo.RedirectStandardInput = true;

            _serverProcess.OutputDataReceived += (s, args) => AppendTerminalLine(args.Data);
            _serverProcess.ErrorDataReceived += (s, args) => AppendTerminalLine(args.Data);

            _serverProcess.EnableRaisingEvents = true;
            _serverProcess.Exited += ServerProcess_Exited;

            _serverProcess.Start();
            _serverProcess.BeginOutputReadLine();
            _serverProcess.BeginErrorReadLine();
            StopServerBTN_New.Show();
        }

        private void StopServerBTN_New_Click(object sender, EventArgs e)
        {
            StopServerBTN_New.Hide();
            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                AppendTerminalLine("[MANAGER INFO] Injected administrative 'stop' command stream...");
                _serverProcess.StandardInput.WriteLine("stop");
                StartServerBTN_New.Show();
            }
        }

        private void SvConfigButton_Click(object sender, EventArgs e)
        {
            // 1. OPERATION STATE CEILING: Block input changes if the server binary loop is actively active
            if (_serverProcess != null && !_serverProcess.HasExited)
            {
                ReaLTaiizor.Controls.MaterialMessageBox.Show(
                    this,
                    "The Bedrock Server process is currently active. You must completely 'Stop Server' before editing core server configurations to prevent disk write collisions.",
                    "Active Server Process Blocked",
                    MessageBoxButtons.OK,
                    false,
                    ReaLTaiizor.Forms.MaterialFlexibleForm.ButtonsPosition.Center
                );
                return;
            }

            // 2. MODAL DISPATCH: Pass 'this' as the parent context handler to force centering over your console canvas
            using (var configForm = new ServerConfigModalForm(this))
            {
                configForm.ShowDialog(this);
            }
        }
    }
}
