using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace BDSUtil.Helpers
{
    public class BDSConsoleManager
    {
        private Process _serverProcess;
        private string _currentLogPath;

        // EVENT BRIDGE INTERFACES
        private readonly Action<string> _uiLogCallback;
        private readonly Action<string, string> _chatDetectedCallback;
        private readonly Action<string, string, string> _pvpKillCallback;
        private readonly Action _serverExitedCallback;
        private readonly Action _serverStartedCallback;

        public bool IsServerRunning => _serverProcess != null && !_serverProcess.HasExited;

        /// <summary>
        /// Centralizes process lifecycle management, stripping terminal logic out of the main view.
        /// </summary>
        public BDSConsoleManager(
            Action<string> uiLogCallback,
            Action<string, string> chatCallback,
            Action<string, string, string> pvpCallback,
            Action serverStartedCallback,
            Action serverExitedCallback)
        {
            _uiLogCallback = uiLogCallback;
            _chatDetectedCallback = chatCallback;
            _pvpKillCallback = pvpCallback;
            _serverStartedCallback = serverStartedCallback;
            _serverExitedCallback = serverExitedCallback;
        }

        /// <summary>
        /// Orchestrates server execution tracking using thread-safe non-blocking standard stream listeners.
        /// </summary>
        public bool StartServer(string fullBinaryPath, string serverDirectory)
        {
            if (IsServerRunning) return false;

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmm");
            Directory.CreateDirectory("logs");
            _currentLogPath = $@"logs\server_{timestamp}.log";

            _serverProcess = new Process();
            _serverProcess.StartInfo.FileName = fullBinaryPath;
            _serverProcess.StartInfo.WorkingDirectory = serverDirectory;
            _serverProcess.StartInfo.UseShellExecute = false;
            _serverProcess.StartInfo.CreateNoWindow = true;
            _serverProcess.StartInfo.RedirectStandardOutput = true;
            _serverProcess.StartInfo.RedirectStandardError = true;
            _serverProcess.StartInfo.RedirectStandardInput = true;
            _serverProcess.StartInfo.StandardOutputEncoding = Encoding.UTF8;

            _serverProcess.OutputDataReceived += (s, args) => ParseIncomingServerLine(args.Data);
            _serverProcess.ErrorDataReceived += (s, args) => ParseIncomingServerLine(args.Data);

            _serverProcess.EnableRaisingEvents = true;
            _serverProcess.Exited += (s, args) => _serverExitedCallback?.Invoke();

            try
            {
                _serverProcess.Start();
                _serverProcess.BeginOutputReadLine();
                _serverProcess.BeginErrorReadLine();
                return true;
            }
            catch (Exception ex)
            {
                _uiLogCallback?.Invoke($"[BDSUtil Severe Fault] Process engine execution error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Pumps commands straight down the standard input channel of the live server loop.
        /// </summary>
        public void SendCommand(string command)
        {
            if (IsServerRunning && !string.IsNullOrWhiteSpace(command))
            {
                _serverProcess.StandardInput.WriteLine(command);
            }
        }

        /// <summary>
        /// Explicit hook to cleanly unregister standard listeners and stop process collision loops on app crash.
        /// </summary>
        public void SafeInterceptionExit()
        {
            if (_serverProcess == null) return;
            try
            {
                _serverProcess.Exited -= (s, args) => _serverExitedCallback?.Invoke();
                _serverProcess.Exited += (s, args) => Environment.Exit(0);
                _serverProcess.StandardInput.WriteLine("stop");
            }
            catch
            {
                try { _serverProcess.Kill(); } catch { }
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Drops the process instantly via native hardware commands.
        /// </summary>
        public void ForceKillServer()
        {
            if (!IsServerRunning) return;
            try
            {
                _serverProcess.Kill();
            }
            catch { }
        }

        /// <summary>
        /// Background line worker parsing module. Filters massive logs down instantly.
        /// </summary>
        private void ParseIncomingServerLine(string text)
        {
            if (text == null) return;

            // Route raw terminal out instantly back up to the UI engine display lists
            _uiLogCallback?.Invoke(text);

            // Commit record straight to local physical logs
            try
            {
                File.AppendAllText(_currentLogPath, text + Environment.NewLine);
            }
            catch { }

            // 1. VANILLA COMPONENT INTERCEPTS: Core initialization tracking
            if (text.IndexOf("Server Started.", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                _serverStartedCallback?.Invoke();
                return;
            }

            // 2. VANILLA COMPONENT INTERCEPTS: Live player identity metrics logging
            if (text.IndexOf("Player connected:", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                try
                {
                    int labelLen = "Player connected:".Length;
                    int nameStartIndex = text.IndexOf("Player connected:") + labelLen;
                    int commaIndex = text.IndexOf(",", nameStartIndex);
                    int xuidLabelIndex = text.IndexOf("xuid:", commaIndex);
                    int xuidStartIndex = xuidLabelIndex + "xuid:".Length;

                    if (commaIndex > nameStartIndex && xuidLabelIndex > 0)
                    {
                        string gamertag = text.Substring(nameStartIndex, commaIndex - nameStartIndex).Trim();
                        string xuidValue = text.Substring(xuidStartIndex).Trim();

                        if (!string.IsNullOrEmpty(xuidValue) && !string.IsNullOrEmpty(gamertag))
                        {
                            BDSUtil.Modals.Content_Controls.CfgHelper.PlayerIdentityStore.SavePlayerNode(xuidValue, gamertag);
                        }
                    }
                }
                catch { }
                return;
            }

            // 3. BEHAVIOR PACK BRIDGE INTERCEPTS: Dump rows immediately if missing bridge tokens
            if (!text.Contains("[BDSUTIL_BRIDGE]")) return;

            try
            {
                // CHAT INTERCEPT ROUTINE
                if (text.Contains("[CHAT]"))
                {
                    int tokenIndex = text.IndexOf("[CHAT] ");
                    if (tokenIndex == -1) return;

                    string chatPayload = text.Substring(tokenIndex + 7).Trim(); // Slices cleanly to "<Player>: Message"
                    int endBracket = chatPayload.IndexOf('>');

                    if (chatPayload.StartsWith("<") && endBracket > 0)
                    {
                        string player = chatPayload.Substring(1, endBracket - 1);
                        string message = chatPayload.Substring(endBracket + 2).Trim();

                        _chatDetectedCallback?.Invoke(player, message);
                    }
                }
                // PVP DEATH INTERCEPT ROUTINE
                else if (text.Contains("[DEATH][PVP]"))
                {
                    string victim = ExtractTokenValue(text, "victim=");
                    string killer = ExtractTokenValue(text, "killer=");
                    string cause = ExtractTokenValue(text, "cause=");

                    _pvpKillCallback?.Invoke(victim, killer, cause);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Bridge Parsing Exception]: {ex.Message}");
            }
        }

        private string ExtractTokenValue(string line, string tokenName)
        {
            int start = line.IndexOf(tokenName);
            if (start == -1) return string.Empty;

            start += tokenName.Length + 1; // Advance past token name and opening quote marker
            int end = line.IndexOf('"', start);
            if (end == -1) return string.Empty;

            return line.Substring(start, end - start);
        }
    }
}
