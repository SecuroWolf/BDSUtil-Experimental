using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace BDSUtil.Helpers.RemoteApi
{
    /// <summary>
    /// Lightweight self-hosted HTTP/1.1 + JSON Remote Management API server over TcpListener.
    /// Runs without administrator privileges or http.sys URL ACL registration on Windows.
    /// </summary>
    public sealed class RemoteManagementHttpServer : IDisposable
    {
        private const int MaxHeaderBytes = 16 * 1024;
        private const int MaxBodyBytes = 256 * 1024;
        private const int RequestReadTimeoutMs = 5000;
        private const int MaxConcurrentConnections = 32;
        private const int MaxFailedAuthAttemptsPerMinute = 10;

        public static readonly JsonSerializerOptions ApiJsonOptions = CreateApiJsonOptions();

        private sealed class AuthFailureBucket
        {
            public int FailureCount;
            public DateTimeOffset WindowStartUtc;
            public DateTimeOffset BlockedUntilUtc;
        }

        private readonly object _lifecycleLock = new object();
        private readonly ConcurrentDictionary<string, AuthFailureBucket> _authFailures = new ConcurrentDictionary<string, AuthFailureBucket>(StringComparer.OrdinalIgnoreCase);
        private SemaphoreSlim _connectionSlots;
        private TcpListener _listener;
        private CancellationTokenSource _cts;
        private Task _acceptLoopTask;
        private Func<string> _apiKeyProvider;
        private IRemoteManagementHost _host;
        private Action<string> _logCallback;

        public bool IsRunning
        {
            get
            {
                lock (_lifecycleLock)
                {
                    return _listener != null && _cts != null && !_cts.IsCancellationRequested;
                }
            }
        }

        public int BoundPort { get; private set; }
        public IPAddress BoundAddress { get; private set; } = IPAddress.Any;
        public string LastError { get; private set; } = string.Empty;

        public event Action<bool, int, string> StateChanged;

        public Task<bool> StartAsync(
            int port,
            Func<string> apiKeyProvider,
            IRemoteManagementHost host,
            IPAddress bindAddress = null,
            Action<string> logCallback = null)
        {
            if (port <= 0 || port > 65535)
                throw new ArgumentOutOfRangeException(nameof(port), "Port must be between 1 and 65535.");
            if (apiKeyProvider == null)
                throw new ArgumentNullException(nameof(apiKeyProvider));
            if (host == null)
                throw new ArgumentNullException(nameof(host));

            IPAddress effectiveBind = bindAddress ?? IPAddress.Any;

            lock (_lifecycleLock)
            {
                StopInternalLocked();

                try
                {
                    var listener = new TcpListener(effectiveBind, port);
                    listener.Server.ExclusiveAddressUse = true;
                    listener.Start();

                    _listener = listener;
                    _cts = new CancellationTokenSource();
                    _connectionSlots = new SemaphoreSlim(MaxConcurrentConnections, MaxConcurrentConnections);
                    _apiKeyProvider = apiKeyProvider;
                    _host = host;
                    _logCallback = logCallback;

                    BoundPort = ((IPEndPoint)listener.LocalEndpoint).Port;
                    BoundAddress = effectiveBind;
                    LastError = string.Empty;

                    _acceptLoopTask = Task.Run(() => AcceptLoopAsync(_listener, _cts.Token));
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    BoundPort = 0;
                    _listener = null;
                    _cts?.Dispose();
                    _cts = null;
                    StateChanged?.Invoke(false, 0, ex.Message);
                    return Task.FromResult(false);
                }
            }

            StateChanged?.Invoke(true, BoundPort, string.Empty);
            return Task.FromResult(true);
        }

        public async Task StopAsync()
        {
            Task acceptTask;
            lock (_lifecycleLock)
            {
                acceptTask = _acceptLoopTask;
                StopInternalLocked();
            }

            if (acceptTask != null)
            {
                try
                {
                    await Task.WhenAny(acceptTask, Task.Delay(1000)).ConfigureAwait(false);
                }
                catch { }
            }

            StateChanged?.Invoke(false, 0, string.Empty);
        }

        public void Dispose()
        {
            lock (_lifecycleLock)
            {
                StopInternalLocked();
            }
        }

        private void StopInternalLocked()
        {
            try { _cts?.Cancel(); } catch { }
            try { _listener?.Stop(); } catch { }
            _listener = null;
            try { _cts?.Dispose(); } catch { }
            _cts = null;
            _acceptLoopTask = null;
            BoundPort = 0;
        }

        private async Task AcceptLoopAsync(TcpListener listener, CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                TcpClient client = null;
                try
                {
                    client = await listener.AcceptTcpClientAsync(ct).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
                catch (SocketException) when (ct.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[RemoteApi Accept Error]: {ex.Message}");
                    continue;
                }

                if (_connectionSlots == null || !await _connectionSlots.WaitAsync(0, ct).ConfigureAwait(false))
                {
                    try
                    {
                        await WriteJsonErrorAsync(client.GetStream(), 503, "Service Busy", "Too many concurrent API connections.", CancellationToken.None).ConfigureAwait(false);
                    }
                    catch { }
                    finally
                    {
                        client.Dispose();
                    }
                    continue;
                }

                _ = HandleClientAsync(client, ct);
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken serverCt)
        {
            using (client)
            {
                try
                {
                    client.NoDelay = true;
                    string remoteIp = (client.Client.RemoteEndPoint as IPEndPoint)?.Address?.ToString() ?? "unknown";

                    using NetworkStream stream = client.GetStream();
                    using var readCts = CancellationTokenSource.CreateLinkedTokenSource(serverCt);
                    readCts.CancelAfter(RequestReadTimeoutMs);

                    ParsedHttpRequest req = await ReadHttpRequestAsync(stream, readCts.Token).ConfigureAwait(false);
                    if (req == null)
                    {
                        return;
                    }

                    if (req.ErrorStatusCode > 0)
                    {
                        await WriteJsonErrorAsync(stream, req.ErrorStatusCode, req.ErrorReason, req.ErrorMessage, serverCt).ConfigureAwait(false);
                        await ShutdownAndDrainAsync(client, stream, serverCt).ConfigureAwait(false);
                        return;
                    }

                    if (req.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
                    {
                        await WriteCorsPreflightAsync(stream, serverCt).ConfigureAwait(false);
                        TryShutdownSend(client);
                        return;
                    }

                    string path = req.Path;
                    bool isHealthRoute = req.Method.Equals("GET", StringComparison.OrdinalIgnoreCase) &&
                                         (path.Equals("/api/v1/ping", StringComparison.OrdinalIgnoreCase) ||
                                          path.Equals("/health", StringComparison.OrdinalIgnoreCase));

                    string suppliedToken = ExtractAuthToken(req.Headers);
                    string expectedToken = _apiKeyProvider?.Invoke() ?? string.Empty;
                    bool isAuthenticated = IsTokenValid(suppliedToken, expectedToken);

                    if (isHealthRoute)
                    {
                        await WriteJsonResponseAsync(stream, 200, "OK", new
                        {
                            ok = true,
                            service = "BDSUtil.RemoteApi",
                            apiVersion = "1.0",
                            port = BoundPort,
                            authenticated = isAuthenticated
                        }, serverCt).ConfigureAwait(false);
                        TryShutdownSend(client);
                        return;
                    }

                    if (IsIpRateLimited(remoteIp))
                    {
                        await WriteJsonErrorAsync(stream, 429, "Too Many Requests", "Too many failed authentication attempts. Try again shortly.", serverCt, retryAfterSeconds: 30).ConfigureAwait(false);
                        TryShutdownSend(client);
                        return;
                    }

                    if (!isAuthenticated)
                    {
                        RecordAuthFailure(remoteIp);
                        _logCallback?.Invoke($"[REMOTE API AUTH DENIED] Rejected unauthorized {req.Method} {req.Path} request from {remoteIp}.");
                        await WriteJsonErrorAsync(stream, 401, "Unauthorized", "Missing or invalid API token. Provide 'Authorization: Bearer <key>' or 'X-API-Key: <key>'.", serverCt).ConfigureAwait(false);
                        TryShutdownSend(client);
                        return;
                    }

                    ClearAuthFailure(remoteIp);

                    await DispatchAuthenticatedRouteAsync(stream, req, serverCt).ConfigureAwait(false);
                    TryShutdownSend(client);
                }
                catch (OperationCanceledException)
                {
                    // Timeout or shutdown; close connection cleanly.
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[RemoteApi Client Error]: {ex.Message}");
                }
                finally
                {
                    try { _connectionSlots?.Release(); } catch { }
                }
            }
        }

        private async Task DispatchAuthenticatedRouteAsync(NetworkStream stream, ParsedHttpRequest req, CancellationToken ct)
        {
            string method = req.Method;
            string path = req.Path;
            IRemoteManagementHost host = _host;

            try
            {
                if (method == "GET" && path.Equals("/api/v1/status", StringComparison.OrdinalIgnoreCase))
                {
                    await WriteJsonResponseAsync(stream, 200, "OK", host.GetStatusSnapshot(), ct).ConfigureAwait(false);
                    return;
                }

                if (method == "GET" && path.Equals("/api/v1/console", StringComparison.OrdinalIgnoreCase))
                {
                    long since = ParseLongQuery(req.Query, "since", 0);
                    int limit = (int)Math.Clamp(ParseLongQuery(req.Query, "limit", 200), 1, 1000);
                    await WriteJsonResponseAsync(stream, 200, "OK", host.GetConsoleLogs(since, limit), ct).ConfigureAwait(false);
                    return;
                }

                if (method == "POST" && path.Equals("/api/v1/server/start", StringComparison.OrdinalIgnoreCase))
                {
                    RemoteActionResult res = await host.StartServerAsync(ct).ConfigureAwait(false);
                    await WriteActionResultAsync(stream, res, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "POST" && path.Equals("/api/v1/server/stop", StringComparison.OrdinalIgnoreCase))
                {
                    ServerStopRequest body = DeserializeOptionalBody<ServerStopRequest>(req.BodyBytes) ?? new ServerStopRequest();
                    RemoteActionResult res = await host.StopServerAsync(body.Force, ct).ConfigureAwait(false);
                    await WriteActionResultAsync(stream, res, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "POST" && path.Equals("/api/v1/server/restart", StringComparison.OrdinalIgnoreCase))
                {
                    RemoteActionResult res = await host.RestartServerAsync(ct).ConfigureAwait(false);
                    await WriteActionResultAsync(stream, res, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "POST" && path.Equals("/api/v1/server/command", StringComparison.OrdinalIgnoreCase))
                {
                    if (!TryDeserializeBody(req.BodyBytes, out ConsoleCommandRequest body, out string err))
                    {
                        await WriteJsonErrorAsync(stream, 400, "Bad Request", err, ct).ConfigureAwait(false);
                        return;
                    }

                    RemoteActionResult res = await host.SendConsoleCommandAsync(body?.Command, ct).ConfigureAwait(false);
                    await WriteActionResultAsync(stream, res, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "GET" && path.Equals("/api/v1/players", StringComparison.OrdinalIgnoreCase))
                {
                    await WriteJsonResponseAsync(stream, 200, "OK", host.GetPlayersSnapshot(), ct).ConfigureAwait(false);
                    return;
                }

                if (method == "POST" && path.Equals("/api/v1/players/action", StringComparison.OrdinalIgnoreCase))
                {
                    if (!TryDeserializeBody(req.BodyBytes, out PlayerActionRequest body, out string err))
                    {
                        await WriteJsonErrorAsync(stream, 400, "Bad Request", err, ct).ConfigureAwait(false);
                        return;
                    }

                    RemoteActionResult res = await host.ExecutePlayerActionAsync(body, ct).ConfigureAwait(false);
                    await WriteActionResultAsync(stream, res, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "GET" && path.Equals("/api/v1/whitelist", StringComparison.OrdinalIgnoreCase))
                {
                    await WriteJsonResponseAsync(stream, 200, "OK", new { entries = host.GetWhitelist() }, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "POST" && path.Equals("/api/v1/whitelist", StringComparison.OrdinalIgnoreCase))
                {
                    if (!TryDeserializeBody(req.BodyBytes, out WhitelistAddRequest body, out string err))
                    {
                        await WriteJsonErrorAsync(stream, 400, "Bad Request", err, ct).ConfigureAwait(false);
                        return;
                    }

                    RemoteActionResult res = await host.AddWhitelistPlayerAsync(body?.Name, body != null && body.IgnoresPlayerLimit, ct).ConfigureAwait(false);
                    await WriteActionResultAsync(stream, res, ct).ConfigureAwait(false);
                    return;
                }

                const string whitelistPrefix = "/api/v1/whitelist/";
                if (method == "DELETE" && path.StartsWith(whitelistPrefix, StringComparison.OrdinalIgnoreCase) && path.Length > whitelistPrefix.Length)
                {
                    string encodedName = path.Substring(whitelistPrefix.Length);
                    string decodedName = WebUtility.UrlDecode(encodedName);
                    RemoteActionResult res = await host.RemoveWhitelistPlayerAsync(decodedName, ct).ConfigureAwait(false);
                    await WriteActionResultAsync(stream, res, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "GET" && path.Equals("/api/v1/operators", StringComparison.OrdinalIgnoreCase))
                {
                    await WriteJsonResponseAsync(stream, 200, "OK", new { operators = host.GetOperators() }, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "GET" && path.Equals("/api/v1/server/properties", StringComparison.OrdinalIgnoreCase))
                {
                    await WriteJsonResponseAsync(stream, 200, "OK", new { properties = host.GetServerProperties() }, ct).ConfigureAwait(false);
                    return;
                }

                if ((method == "PUT" || method == "PATCH") && path.Equals("/api/v1/server/properties", StringComparison.OrdinalIgnoreCase))
                {
                    if (!TryDeserializeBody(req.BodyBytes, out ServerPropertiesPatchRequest body, out string err))
                    {
                        await WriteJsonErrorAsync(stream, 400, "Bad Request", err, ct).ConfigureAwait(false);
                        return;
                    }

                    RemoteActionResult res = await host.UpdateServerPropertiesAsync(body?.Properties, ct).ConfigureAwait(false);
                    await WriteActionResultAsync(stream, res, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "GET" && path.Equals("/api/v1/backups", StringComparison.OrdinalIgnoreCase))
                {
                    await WriteJsonResponseAsync(stream, 200, "OK", new { backups = host.ListBackups() }, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "POST" && path.Equals("/api/v1/backups", StringComparison.OrdinalIgnoreCase))
                {
                    BackupTriggerRequest body = DeserializeOptionalBody<BackupTriggerRequest>(req.BodyBytes) ?? new BackupTriggerRequest();
                    RemoteActionResult res = await host.StartBackupAsync(body.Type, body.StopServerFirst, ct).ConfigureAwait(false);
                    await WriteActionResultAsync(stream, res, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "GET" && path.Equals("/api/v1/config", StringComparison.OrdinalIgnoreCase))
                {
                    await WriteJsonResponseAsync(stream, 200, "OK", host.GetConfigSummary(), ct).ConfigureAwait(false);
                    return;
                }

                if ((method == "PATCH" || method == "POST") && path.Equals("/api/v1/automod", StringComparison.OrdinalIgnoreCase))
                {
                    if (!TryDeserializeBody(req.BodyBytes, out AutoModTogglePatch body, out string err))
                    {
                        await WriteJsonErrorAsync(stream, 400, "Bad Request", err, ct).ConfigureAwait(false);
                        return;
                    }

                    RemoteConfigSummaryDto updated = host.UpdateConfigToggles(body ?? new AutoModTogglePatch());
                    await WriteJsonResponseAsync(stream, 200, "OK", updated, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "GET" && path.Equals("/api/v1/network/upnp", StringComparison.OrdinalIgnoreCase))
                {
                    RemoteUpnpStatusDto upnp = await host.GetUpnpStatusAsync(runVerification: false, ct).ConfigureAwait(false);
                    await WriteJsonResponseAsync(stream, 200, "OK", upnp, ct).ConfigureAwait(false);
                    return;
                }

                if (method == "POST" && path.Equals("/api/v1/network/upnp/verify", StringComparison.OrdinalIgnoreCase))
                {
                    RemoteUpnpStatusDto upnp = await host.GetUpnpStatusAsync(runVerification: true, ct).ConfigureAwait(false);
                    await WriteJsonResponseAsync(stream, 200, "OK", upnp, ct).ConfigureAwait(false);
                    return;
                }

                await WriteJsonErrorAsync(stream, 404, "Not Found", $"Unknown endpoint '{method} {path}'.", ct).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await WriteJsonErrorAsync(stream, 500, "Internal Server Error", ex.Message, ct).ConfigureAwait(false);
            }
        }

        private sealed class ParsedHttpRequest
        {
            public string Method { get; set; } = string.Empty;
            public string Path { get; set; } = "/";
            public Dictionary<string, string> Query { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            public byte[] BodyBytes { get; set; } = Array.Empty<byte>();
            public int ErrorStatusCode { get; set; }
            public string ErrorReason { get; set; } = string.Empty;
            public string ErrorMessage { get; set; } = string.Empty;
        }

        private static async Task<ParsedHttpRequest> ReadHttpRequestAsync(NetworkStream stream, CancellationToken ct)
        {
            byte[] headerBuf = new byte[MaxHeaderBytes];
            int totalRead = 0;
            int headerEndIndex = -1;

            while (totalRead < headerBuf.Length)
            {
                int bytesRead = await stream.ReadAsync(headerBuf, totalRead, headerBuf.Length - totalRead, ct).ConfigureAwait(false);
                if (bytesRead == 0)
                {
                    return totalRead == 0 ? null : MakeError(400, "Bad Request", "Incomplete HTTP request.");
                }

                totalRead += bytesRead;
                headerEndIndex = FindHeaderTerminator(headerBuf, totalRead);
                if (headerEndIndex >= 0) break;
            }

            if (headerEndIndex < 0)
            {
                return MakeError(431, "Request Header Fields Too Large", "HTTP headers exceed 16 KB limit.");
            }

            string headerText = Encoding.ASCII.GetString(headerBuf, 0, headerEndIndex);
            string[] lines = headerText.Split(new[] { "\r\n" }, StringSplitOptions.None);
            if (lines.Length == 0)
            {
                return MakeError(400, "Bad Request", "Empty HTTP request line.");
            }

            string[] requestLineParts = lines[0].Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);
            if (requestLineParts.Length < 2)
            {
                return MakeError(400, "Bad Request", "Malformed HTTP request line.");
            }

            var req = new ParsedHttpRequest
            {
                Method = requestLineParts[0].Trim().ToUpperInvariant()
            };

            string rawTarget = requestLineParts[1].Trim();
            if (Uri.TryCreate(rawTarget, UriKind.Absolute, out Uri absoluteUri))
            {
                rawTarget = absoluteUri.PathAndQuery;
            }

            int qIndex = rawTarget.IndexOf('?');
            string pathPart = qIndex >= 0 ? rawTarget.Substring(0, qIndex) : rawTarget;
            if (string.IsNullOrEmpty(pathPart)) pathPart = "/";
            if (pathPart.Length > 1 && pathPart.EndsWith("/"))
            {
                pathPart = pathPart.TrimEnd('/');
            }
            req.Path = pathPart;

            if (qIndex >= 0 && qIndex + 1 < rawTarget.Length)
            {
                string queryString = rawTarget.Substring(qIndex + 1);
                foreach (string pair in queryString.Split('&', StringSplitOptions.RemoveEmptyEntries))
                {
                    int eqIdx = pair.IndexOf('=');
                    string k = WebUtility.UrlDecode(eqIdx >= 0 ? pair.Substring(0, eqIdx) : pair);
                    string v = eqIdx >= 0 ? WebUtility.UrlDecode(pair.Substring(eqIdx + 1)) : string.Empty;
                    if (!string.IsNullOrEmpty(k))
                    {
                        req.Query[k] = v;
                    }
                }
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrEmpty(line)) continue;
                int colon = line.IndexOf(':');
                if (colon <= 0) continue;
                string name = line.Substring(0, colon).Trim();
                string value = line.Substring(colon + 1).Trim();
                req.Headers[name] = value;
            }

            if (req.Headers.TryGetValue("Transfer-Encoding", out string te) &&
                te.IndexOf("chunked", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return MakeError(501, "Not Implemented", "Chunked request transfer encoding is not supported.");
            }

            int contentLength = 0;
            if (req.Headers.TryGetValue("Content-Length", out string clValue))
            {
                if (!int.TryParse(clValue, out contentLength) || contentLength < 0)
                {
                    return MakeError(400, "Bad Request", "Invalid Content-Length header.");
                }

                if (contentLength > MaxBodyBytes)
                {
                    return MakeError(413, "Payload Too Large", "Request body exceeds 256 KB limit.");
                }
            }

            if (contentLength > 0)
            {
                byte[] body = new byte[contentLength];
                int bodyStart = headerEndIndex + 4;
                int alreadyReadBody = Math.Max(0, totalRead - bodyStart);
                int copied = Math.Min(alreadyReadBody, contentLength);

                if (copied > 0)
                {
                    Buffer.BlockCopy(headerBuf, bodyStart, body, 0, copied);
                }

                while (copied < contentLength)
                {
                    int read = await stream.ReadAsync(body, copied, contentLength - copied, ct).ConfigureAwait(false);
                    if (read == 0)
                    {
                        return MakeError(400, "Bad Request", "Unexpected end of HTTP request body.");
                    }
                    copied += read;
                }

                req.BodyBytes = body;
            }

            return req;
        }

        private static int FindHeaderTerminator(byte[] buffer, int length)
        {
            for (int i = 0; i <= length - 4; i++)
            {
                if (buffer[i] == '\r' && buffer[i + 1] == '\n' && buffer[i + 2] == '\r' && buffer[i + 3] == '\n')
                {
                    return i;
                }
            }
            return -1;
        }

        private static ParsedHttpRequest MakeError(int code, string reason, string message) => new ParsedHttpRequest
        {
            ErrorStatusCode = code,
            ErrorReason = reason,
            ErrorMessage = message
        };

        private static string ExtractAuthToken(IReadOnlyDictionary<string, string> headers)
        {
            if (headers.TryGetValue("Authorization", out string auth) && !string.IsNullOrWhiteSpace(auth))
            {
                const string bearerPrefix = "Bearer ";
                if (auth.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    return auth.Substring(bearerPrefix.Length).Trim();
                }
            }

            if (headers.TryGetValue("X-API-Key", out string apiKeyHeader) && !string.IsNullOrWhiteSpace(apiKeyHeader))
            {
                return apiKeyHeader.Trim();
            }

            return string.Empty;
        }

        private static bool IsTokenValid(string supplied, string expected)
        {
            if (string.IsNullOrWhiteSpace(expected) || string.IsNullOrWhiteSpace(supplied))
                return false;

            byte[] suppliedHash = SHA256.HashData(Encoding.UTF8.GetBytes(supplied));
            byte[] expectedHash = SHA256.HashData(Encoding.UTF8.GetBytes(expected));
            return CryptographicOperations.FixedTimeEquals(suppliedHash, expectedHash);
        }

        private bool IsIpRateLimited(string ip)
        {
            if (_authFailures.TryGetValue(ip, out AuthFailureBucket bucket))
            {
                lock (bucket)
                {
                    return bucket.BlockedUntilUtc > DateTimeOffset.UtcNow;
                }
            }
            return false;
        }

        private void RecordAuthFailure(string ip)
        {
            var bucket = _authFailures.GetOrAdd(ip, _ => new AuthFailureBucket { WindowStartUtc = DateTimeOffset.UtcNow });
            DateTimeOffset now = DateTimeOffset.UtcNow;
            lock (bucket)
            {
                if (now - bucket.WindowStartUtc > TimeSpan.FromMinutes(1))
                {
                    bucket.WindowStartUtc = now;
                    bucket.FailureCount = 0;
                }

                bucket.FailureCount++;
                if (bucket.FailureCount >= MaxFailedAuthAttemptsPerMinute)
                {
                    bucket.BlockedUntilUtc = now.AddSeconds(30);
                }
            }
        }

        private void ClearAuthFailure(string ip)
        {
            _authFailures.TryRemove(ip, out _);
        }

        private static bool TryDeserializeBody<T>(byte[] bodyBytes, out T result, out string error) where T : class
        {
            result = null;
            error = null;

            if (bodyBytes == null || bodyBytes.Length == 0)
            {
                error = "Request JSON body is required.";
                return false;
            }

            try
            {
                result = JsonSerializer.Deserialize<T>(bodyBytes, ApiJsonOptions);
                if (result == null)
                {
                    error = "JSON body deserialized to null.";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                error = $"Invalid JSON request body: {ex.Message}";
                return false;
            }
        }

        private static T DeserializeOptionalBody<T>(byte[] bodyBytes) where T : class
        {
            if (bodyBytes == null || bodyBytes.Length == 0) return null;
            try
            {
                return JsonSerializer.Deserialize<T>(bodyBytes, ApiJsonOptions);
            }
            catch
            {
                return null;
            }
        }

        private static long ParseLongQuery(IReadOnlyDictionary<string, string> query, string key, long fallback)
        {
            return query.TryGetValue(key, out string raw) && long.TryParse(raw, out long parsed)
                ? parsed
                : fallback;
        }

        private static Task WriteActionResultAsync(NetworkStream stream, RemoteActionResult res, CancellationToken ct)
        {
            int status = res?.HttpStatusCode > 0 ? res.HttpStatusCode : (res != null && res.Success ? 200 : 400);
            string reason = status switch
            {
                200 => "OK",
                201 => "Created",
                202 => "Accepted",
                400 => "Bad Request",
                404 => "Not Found",
                409 => "Conflict",
                423 => "Locked",
                _ => "OK"
            };

            return WriteJsonResponseAsync(stream, status, reason, new
            {
                success = res?.Success ?? false,
                message = res?.Message ?? string.Empty,
                data = res?.Data
            }, ct);
        }

        private static Task WriteJsonErrorAsync(NetworkStream stream, int code, string reason, string message, CancellationToken ct, int? retryAfterSeconds = null)
        {
            return WriteJsonResponseAsync(stream, code, reason, new
            {
                success = false,
                error = reason,
                message = message ?? string.Empty
            }, ct, retryAfterSeconds);
        }

        private static Task WriteCorsPreflightAsync(NetworkStream stream, CancellationToken ct)
        {
            string headers =
                "HTTP/1.1 204 No Content\r\n" +
                "Access-Control-Allow-Origin: *\r\n" +
                "Access-Control-Allow-Methods: GET, POST, PUT, PATCH, DELETE, OPTIONS\r\n" +
                "Access-Control-Allow-Headers: Authorization, X-API-Key, Content-Type\r\n" +
                "Access-Control-Max-Age: 86400\r\n" +
                "Content-Length: 0\r\n" +
                "Connection: close\r\n\r\n";

            byte[] bytes = Encoding.ASCII.GetBytes(headers);
            return stream.WriteAsync(bytes, 0, bytes.Length, ct);
        }

        private static async Task WriteJsonResponseAsync(NetworkStream stream, int statusCode, string reasonPhrase, object payload, CancellationToken ct, int? retryAfterSeconds = null)
        {
            byte[] jsonBytes = JsonSerializer.SerializeToUtf8Bytes(payload, ApiJsonOptions);
            var sb = new StringBuilder();
            sb.Append($"HTTP/1.1 {statusCode} {reasonPhrase}\r\n");
            sb.Append("Content-Type: application/json; charset=utf-8\r\n");
            sb.Append($"Content-Length: {jsonBytes.Length}\r\n");
            sb.Append("Cache-Control: no-store\r\n");
            sb.Append("X-Content-Type-Options: nosniff\r\n");
            sb.Append("Access-Control-Allow-Origin: *\r\n");
            sb.Append("Access-Control-Allow-Methods: GET, POST, PUT, PATCH, DELETE, OPTIONS\r\n");
            sb.Append("Access-Control-Allow-Headers: Authorization, X-API-Key, Content-Type\r\n");
            if (retryAfterSeconds.HasValue)
            {
                sb.Append($"Retry-After: {retryAfterSeconds.Value}\r\n");
            }
            sb.Append("Connection: close\r\n\r\n");

            byte[] headerBytes = Encoding.ASCII.GetBytes(sb.ToString());
            await stream.WriteAsync(headerBytes, 0, headerBytes.Length, ct).ConfigureAwait(false);
            await stream.WriteAsync(jsonBytes, 0, jsonBytes.Length, ct).ConfigureAwait(false);
            await stream.FlushAsync(ct).ConfigureAwait(false);
        }

        private static void TryShutdownSend(TcpClient client)
        {
            try
            {
                client?.Client?.Shutdown(SocketShutdown.Send);
            }
            catch
            {
            }
        }

        private static async Task ShutdownAndDrainAsync(TcpClient client, NetworkStream stream, CancellationToken serverCt)
        {
            TryShutdownSend(client);

            // Drain any unread request bytes briefly so Winsock does not emit a TCP RST
            // (10054) before the client finishes reading the error response (e.g., 413/431).
            using var drainCts = CancellationTokenSource.CreateLinkedTokenSource(serverCt);
            drainCts.CancelAfter(250);
            byte[] drainBuf = new byte[8192];
            int totalDrained = 0;
            const int maxDrainBytes = 1024 * 1024;

            try
            {
                while (totalDrained < maxDrainBytes && !drainCts.IsCancellationRequested)
                {
                    int read = await stream.ReadAsync(drainBuf, 0, drainBuf.Length, drainCts.Token).ConfigureAwait(false);
                    if (read <= 0)
                    {
                        break;
                    }
                    totalDrained += read;
                }
            }
            catch
            {
            }
        }

        private static JsonSerializerOptions CreateApiJsonOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }
    }
}
