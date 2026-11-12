using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BDSUtil.Helpers
{
    /// <summary>
    /// Built-in presets, custom URL templating, and response parsing for optional external port reachability checks.
    /// </summary>
    public static class ExternalPortChecker
    {
        public const string ProviderOff = "Off";
        public const string ProviderIfconfigCo = "ifconfig.co";
        public const string ProviderIpconfigIo = "ipconfig.io";
        public const string ProviderPortcheckerIo = "portchecker.io";
        public const string ProviderCustom = "Custom";

        public const string PresetUrlIfconfigCo = "https://ifconfig.co/port/{port}";
        public const string PresetUrlIpconfigIo = "https://ipconfig.io/port/{port}";
        public const string PresetSpecPortcheckerIo = "preset://portchecker.io";
        private const string PortcheckerIoApiEndpoint = "https://portchecker.io/api/v1/query";

        public sealed class ProviderOption
        {
            public string Key { get; }
            public string DisplayName { get; }
            public string ProbeSpec { get; }

            public ProviderOption(string key, string displayName, string probeSpec)
            {
                Key = key;
                DisplayName = displayName;
                ProbeSpec = probeSpec;
            }
        }

        public static IReadOnlyList<ProviderOption> Providers { get; } = new[]
        {
            new ProviderOption(ProviderOff, "Off (Local Probe Only — Private)", string.Empty),
            new ProviderOption(ProviderIfconfigCo, "ifconfig.co (TCP Port Check)", PresetUrlIfconfigCo),
            new ProviderOption(ProviderIpconfigIo, "ipconfig.io (TCP Port Check)", PresetUrlIpconfigIo),
            new ProviderOption(ProviderPortcheckerIo, "portchecker.io (TCP Port Check)", PresetSpecPortcheckerIo),
            new ProviderOption(ProviderCustom, "Custom API URL", string.Empty)
        };

        private static readonly string[] ArrayContainerKeys = { "check", "checks", "ports", "results" };
        private static readonly string[] WrapperObjectKeys = { "data", "result", "response", "port_check" };
        private static readonly string[] ExplicitReachabilityKeys =
        {
            "reachable", "accessible", "open", "isOpen", "is_open",
            "port_open", "portOpen", "port_status", "portStatus", "available"
        };

        private static readonly HttpClient _ipv4RelayHttpClient = CreateIpv4RelayHttpClient();

        private static HttpClient CreateIpv4RelayHttpClient()
        {
            var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
                ConnectTimeout = TimeSpan.FromSeconds(4),
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 3,
                ConnectCallback = async (context, ct) =>
                {
                    // Force IPv4 for DNS hostnames so IPv4 UPnP port checks are not sent over IPv6 on dual-stack hosts.
                    if (IPAddress.TryParse(context.DnsEndPoint.Host, out IPAddress literalIp))
                    {
                        var literalSocket = new Socket(literalIp.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
                        {
                            NoDelay = true
                        };
                        try
                        {
                            await literalSocket.ConnectAsync(literalIp, context.DnsEndPoint.Port, ct).ConfigureAwait(false);
                            return new NetworkStream(literalSocket, ownsSocket: true);
                        }
                        catch
                        {
                            literalSocket.Dispose();
                            throw;
                        }
                    }

                    IPAddress[] addresses = await Dns.GetHostAddressesAsync(
                        context.DnsEndPoint.Host,
                        AddressFamily.InterNetwork,
                        ct).ConfigureAwait(false);

                    if (addresses == null || addresses.Length == 0)
                    {
                        throw new SocketException((int)SocketError.HostNotFound);
                    }

                    var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                    {
                        NoDelay = true
                    };
                    try
                    {
                        await socket.ConnectAsync(addresses, context.DnsEndPoint.Port, ct).ConfigureAwait(false);
                        return new NetworkStream(socket, ownsSocket: true);
                    }
                    catch
                    {
                        socket.Dispose();
                        throw;
                    }
                }
            };

            return new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        public static string NormalizeProviderKey(string provider, string legacyOrCustomUrl = null)
        {
            if (!string.IsNullOrWhiteSpace(provider))
            {
                string trimmed = provider.Trim();
                foreach (ProviderOption opt in Providers)
                {
                    if (opt.Key.Equals(trimmed, StringComparison.OrdinalIgnoreCase))
                    {
                        return opt.Key;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(legacyOrCustomUrl))
            {
                string trimmedUrl = legacyOrCustomUrl.Trim();
                if (trimmedUrl.Equals(PresetUrlIfconfigCo, StringComparison.OrdinalIgnoreCase))
                    return ProviderIfconfigCo;
                if (trimmedUrl.Equals(PresetUrlIpconfigIo, StringComparison.OrdinalIgnoreCase))
                    return ProviderIpconfigIo;
                if (trimmedUrl.Equals(PresetSpecPortcheckerIo, StringComparison.OrdinalIgnoreCase) ||
                    trimmedUrl.Equals(PortcheckerIoApiEndpoint, StringComparison.OrdinalIgnoreCase))
                    return ProviderPortcheckerIo;

                return ProviderCustom;
            }

            return ProviderOff;
        }

        public static int GetProviderIndex(string providerKey)
        {
            string normalized = NormalizeProviderKey(providerKey);
            for (int i = 0; i < Providers.Count; i++)
            {
                if (Providers[i].Key.Equals(normalized, StringComparison.OrdinalIgnoreCase))
                {
                    return i;
                }
            }
            return 0;
        }

        public static string ResolveEffectiveProbeSpec(string provider, string customUrl)
        {
            string normalized = NormalizeProviderKey(provider, customUrl);
            if (normalized == ProviderOff)
                return string.Empty;

            if (normalized == ProviderCustom)
                return customUrl?.Trim() ?? string.Empty;

            foreach (ProviderOption opt in Providers)
            {
                if (opt.Key == normalized)
                    return opt.ProbeSpec;
            }

            return string.Empty;
        }

        public static bool SupportsProtocol(string effectiveSpecOrUrl, UpnpProtocol protocol)
        {
            if (string.IsNullOrWhiteSpace(effectiveSpecOrUrl))
                return false;

            if (protocol == UpnpProtocol.Tcp)
                return true;

            // Built-in public APIs are TCP-only. Only allow UDP external checks when a custom URL explicitly supports {protocol}.
            string trimmed = effectiveSpecOrUrl.Trim();
            if (IsBuiltInSpec(trimmed))
                return false;

            return trimmed.Contains("{protocol}", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsBuiltInSpec(string effectiveSpecOrUrl)
        {
            if (string.IsNullOrWhiteSpace(effectiveSpecOrUrl))
                return false;

            string trimmed = effectiveSpecOrUrl.Trim();
            return trimmed.Equals(PresetUrlIfconfigCo, StringComparison.OrdinalIgnoreCase) ||
                   trimmed.Equals(PresetUrlIpconfigIo, StringComparison.OrdinalIgnoreCase) ||
                   trimmed.Equals(PresetSpecPortcheckerIo, StringComparison.OrdinalIgnoreCase);
        }

        public static string GetDisplayNameForSpec(string effectiveSpecOrUrl)
        {
            if (string.IsNullOrWhiteSpace(effectiveSpecOrUrl))
                return ProviderOff;

            string trimmed = effectiveSpecOrUrl.Trim();
            if (trimmed.Equals(PresetUrlIfconfigCo, StringComparison.OrdinalIgnoreCase))
                return ProviderIfconfigCo;
            if (trimmed.Equals(PresetUrlIpconfigIo, StringComparison.OrdinalIgnoreCase))
                return ProviderIpconfigIo;
            if (trimmed.Equals(PresetSpecPortcheckerIo, StringComparison.OrdinalIgnoreCase))
                return ProviderPortcheckerIo;

            if (Uri.TryCreate(trimmed.Replace("{port}", "19132", StringComparison.OrdinalIgnoreCase)
                                     .Replace("{ip}", "1.1.1.1", StringComparison.OrdinalIgnoreCase)
                                     .Replace("{protocol}", "tcp", StringComparison.OrdinalIgnoreCase),
                              UriKind.Absolute,
                              out Uri parsed))
            {
                return parsed.Host;
            }

            return "custom API";
        }

        public static string GetCustomTemplateHint(string template)
        {
            string trimmed = template?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(trimmed))
            {
                return "Put {port} where the port goes (optional: {ip}, {protocol}). Example: https://ifconfig.co/port/{port}";
            }

            if (!trimmed.StartsWith("https://", StringComparison.OrdinalIgnoreCase) &&
                !trimmed.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                return "Tip: Start the URL with https:// or http:// and put {port} where the port goes. Example: https://ifconfig.co/port/{port}";
            }

            if (!trimmed.Contains("{port}", StringComparison.OrdinalIgnoreCase))
            {
                return "Tip: Put {port} wherever the port number goes (if omitted, ?port={port}&ip={ip}&protocol={protocol} will be appended).";
            }

            return "Ready: {port} will be substituted for each checked port (optional placeholders: {ip}, {protocol}).";
        }

        public static Uri BuildCustomRequestUri(string template, string externalIp, int externalPort, string protocol)
        {
            if (string.IsNullOrWhiteSpace(template))
                return null;

            string targetUrl = template.Trim();
            string safeIp = externalIp ?? string.Empty;
            string safeProto = string.IsNullOrWhiteSpace(protocol) ? "tcp" : protocol.Trim().ToLowerInvariant();
            string portStr = externalPort.ToString(CultureInfo.InvariantCulture);

            bool hasPortPlaceholder = targetUrl.Contains("{port}", StringComparison.OrdinalIgnoreCase);
            bool hasIpPlaceholder = targetUrl.Contains("{ip}", StringComparison.OrdinalIgnoreCase);
            bool hasProtoPlaceholder = targetUrl.Contains("{protocol}", StringComparison.OrdinalIgnoreCase);

            if (hasPortPlaceholder || hasIpPlaceholder || hasProtoPlaceholder)
            {
                targetUrl = targetUrl
                    .Replace("{ip}", Uri.EscapeDataString(safeIp), StringComparison.OrdinalIgnoreCase)
                    .Replace("{port}", portStr, StringComparison.OrdinalIgnoreCase)
                    .Replace("{protocol}", Uri.EscapeDataString(safeProto), StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                char sep = targetUrl.Contains('?') ? '&' : '?';
                targetUrl = $"{targetUrl}{sep}port={portStr}&ip={Uri.EscapeDataString(safeIp)}&protocol={Uri.EscapeDataString(safeProto)}";
            }

            if (Uri.TryCreate(targetUrl, UriKind.Absolute, out Uri uri) &&
                (uri.Scheme.Equals(Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) ||
                 uri.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase)))
            {
                return uri;
            }

            return null;
        }

        public static async Task<bool?> QueryAsync(
            string effectiveSpecOrUrl,
            string externalIp,
            int externalPort,
            string protocol,
            HttpClient customHttpClient = null,
            CancellationToken cancellationToken = default)
        {
            UpnpProtocol parsedProto = !string.IsNullOrWhiteSpace(protocol) &&
                                     protocol.Trim().Equals("TCP", StringComparison.OrdinalIgnoreCase)
                ? UpnpProtocol.Tcp
                : UpnpProtocol.Udp;

            if (!SupportsProtocol(effectiveSpecOrUrl, parsedProto))
                return null;

            HttpClient client = customHttpClient ?? _ipv4RelayHttpClient;
            using var relayTimeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            relayTimeoutCts.CancelAfter(TimeSpan.FromSeconds(8.5));

            try
            {
                string trimmedSpec = effectiveSpecOrUrl.Trim();
                using HttpRequestMessage request = trimmedSpec.Equals(PresetSpecPortcheckerIo, StringComparison.OrdinalIgnoreCase)
                    ? BuildPortcheckerIoRequest(externalIp, externalPort)
                    : BuildGetRequest(trimmedSpec, externalIp, externalPort, protocol);

                if (request == null)
                    return null;

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.UserAgent.ParseAdd("BDSUtil/1.0");

                using HttpResponseMessage response = await client.SendAsync(request, relayTimeoutCts.Token).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                    return null;

                string body = await response.Content.ReadAsStringAsync(relayTimeoutCts.Token).ConfigureAwait(false);
                if (TryParseProbeResponse(body, externalPort, out bool accessible))
                    return accessible;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[UPnP External Relay Error]: {ex.Message}");
            }

            return null;
        }

        private static HttpRequestMessage BuildPortcheckerIoRequest(string externalIp, int externalPort)
        {
            if (string.IsNullOrWhiteSpace(externalIp))
                return null;

            var payload = new
            {
                host = externalIp.Trim(),
                ports = new[] { externalPort.ToString(CultureInfo.InvariantCulture) }
            };

            string json = JsonSerializer.Serialize(payload);
            return new HttpRequestMessage(HttpMethod.Post, PortcheckerIoApiEndpoint)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        private static HttpRequestMessage BuildGetRequest(string template, string externalIp, int externalPort, string protocol)
        {
            Uri targetUri = BuildCustomRequestUri(template, externalIp, externalPort, protocol);
            return targetUri != null ? new HttpRequestMessage(HttpMethod.Get, targetUri) : null;
        }

        /// <summary>
        /// Parses common JSON and plain-text port-checker API responses, including nested check arrays
        /// and wrapper objects while avoiding false positives from API status envelopes like {"status":"ok"}.
        /// </summary>
        public static bool TryParseProbeResponse(string body, int testedPort, out bool accessible)
        {
            accessible = false;
            if (string.IsNullOrWhiteSpace(body))
                return false;

            string trimmed = body.Trim();

            try
            {
                using JsonDocument doc = JsonDocument.Parse(trimmed, JsonHelper.DocumentOptions);
                JsonElement root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.True)
                {
                    accessible = true;
                    return true;
                }

                if (root.ValueKind == JsonValueKind.False)
                {
                    accessible = false;
                    return true;
                }

                if (root.ValueKind == JsonValueKind.Object)
                {
                    // 1. Check explicit per-port result arrays (e.g., portchecker.io: {"check":[{"port":19132,"status":true}]})
                    if (TryExtractFromArrayContainers(root, testedPort, out accessible))
                        return true;

                    // 2. Check dedicated reachability properties on root object, then common wrapper objects.
                    if (TryExtractExplicitReachabilityKeys(root, out accessible))
                        return true;

                    foreach (string wrapperKey in WrapperObjectKeys)
                    {
                        if (JsonHelper.TryGetPropertyIgnoreCase(root, wrapperKey, out JsonElement nested))
                        {
                            if (nested.ValueKind == JsonValueKind.Object)
                            {
                                if (TryExtractFromArrayContainers(nested, testedPort, out accessible))
                                    return true;
                                if (TryExtractExplicitReachabilityKeys(nested, out accessible))
                                    return true;
                            }
                            else if (nested.ValueKind == JsonValueKind.Array &&
                                     TryFindMatchingPortResultInArray(nested, testedPort, out accessible))
                            {
                                return true;
                            }
                        }
                    }

                    // 3. Conservative fallback on "status" (ignoring generic API envelope values like "ok"/200) or "success"/"ok".
                    if (TryExtractConservativeFallback(root, out accessible))
                        return true;

                    foreach (string wrapperKey in WrapperObjectKeys)
                    {
                        if (JsonHelper.TryGetPropertyIgnoreCase(root, wrapperKey, out JsonElement nested) &&
                            nested.ValueKind == JsonValueKind.Object &&
                            TryExtractConservativeFallback(nested, out accessible))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }
            catch (JsonException)
            {
                // Fall back to concise plain-text inspection below.
            }

            if (trimmed.Length <= 64)
            {
                return TryParseReachabilityText(trimmed, strictEnvelopeAvoidance: false, out accessible);
            }

            return false;
        }

        private static bool TryExtractFromArrayContainers(JsonElement container, int testedPort, out bool accessible)
        {
            accessible = false;
            foreach (string arrayKey in ArrayContainerKeys)
            {
                if (JsonHelper.TryGetPropertyIgnoreCase(container, arrayKey, out JsonElement arr) &&
                    arr.ValueKind == JsonValueKind.Array &&
                    TryFindMatchingPortResultInArray(arr, testedPort, out accessible))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool TryFindMatchingPortResultInArray(JsonElement arrayElement, int testedPort, out bool accessible)
        {
            accessible = false;
            int count = arrayElement.GetArrayLength();
            if (count == 0)
                return false;

            foreach (JsonElement item in arrayElement.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Object)
                    continue;

                bool portMatches = count == 1;
                if (JsonHelper.TryGetPropertyIgnoreCase(item, "port", out JsonElement portEl))
                {
                    if (TryReadPortNumber(portEl, out int itemPort))
                    {
                        if (itemPort != testedPort)
                            continue;
                        portMatches = true;
                    }
                }

                if (!portMatches)
                    continue;

                if (TryExtractExplicitReachabilityKeys(item, out accessible))
                    return true;
                if (TryExtractConservativeFallback(item, out accessible))
                    return true;
            }

            return false;
        }

        private static bool TryExtractExplicitReachabilityKeys(JsonElement obj, out bool accessible)
        {
            accessible = false;
            foreach (string key in ExplicitReachabilityKeys)
            {
                if (JsonHelper.TryGetPropertyIgnoreCase(obj, key, out JsonElement val) &&
                    TryParseReachabilityJsonToken(val, strictEnvelopeAvoidance: false, out accessible))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool TryExtractConservativeFallback(JsonElement obj, out bool accessible)
        {
            accessible = false;

            if (JsonHelper.TryGetPropertyIgnoreCase(obj, "status", out JsonElement statusEl) &&
                TryParseReachabilityJsonToken(statusEl, strictEnvelopeAvoidance: true, out accessible))
            {
                return true;
            }

            bool hasErrorTrue = JsonHelper.TryGetPropertyIgnoreCase(obj, "error", out JsonElement errorEl) &&
                                errorEl.ValueKind == JsonValueKind.True;

            if (!hasErrorTrue &&
                JsonHelper.TryGetPropertyIgnoreCase(obj, "success", out JsonElement successEl) &&
                (successEl.ValueKind == JsonValueKind.True || successEl.ValueKind == JsonValueKind.False))
            {
                accessible = successEl.GetBoolean();
                return true;
            }

            return false;
        }

        private static bool TryReadPortNumber(JsonElement portEl, out int port)
        {
            port = 0;
            if (portEl.ValueKind == JsonValueKind.Number && portEl.TryGetInt32(out port))
                return true;

            if (portEl.ValueKind == JsonValueKind.String)
                return int.TryParse(portEl.GetString()?.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out port);

            return false;
        }

        private static bool TryParseReachabilityJsonToken(JsonElement token, bool strictEnvelopeAvoidance, out bool accessible)
        {
            accessible = false;
            switch (token.ValueKind)
            {
                case JsonValueKind.True:
                    accessible = true;
                    return true;

                case JsonValueKind.False:
                    accessible = false;
                    return true;

                case JsonValueKind.Number:
                    if (!strictEnvelopeAvoidance && token.TryGetInt32(out int num))
                    {
                        if (num == 1)
                        {
                            accessible = true;
                            return true;
                        }
                        if (num == 0)
                        {
                            accessible = false;
                            return true;
                        }
                    }
                    return false;

                case JsonValueKind.String:
                    return TryParseReachabilityText(token.GetString(), strictEnvelopeAvoidance, out accessible);

                default:
                    return false;
            }
        }

        private static bool TryParseReachabilityText(string text, bool strictEnvelopeAvoidance, out bool accessible)
        {
            accessible = false;
            if (string.IsNullOrWhiteSpace(text))
                return false;

            string token = text.Trim().ToLowerInvariant();
            switch (token)
            {
                case "true":
                case "open":
                case "opened":
                case "reachable":
                case "accessible":
                case "up":
                    accessible = true;
                    return true;

                case "yes":
                case "1":
                case "ok":
                case "pass":
                    if (strictEnvelopeAvoidance)
                        return false;
                    accessible = true;
                    return true;

                case "false":
                case "closed":
                case "unreachable":
                case "blocked":
                case "filtered":
                case "refused":
                case "timeout":
                case "timed_out":
                case "down":
                    accessible = false;
                    return true;

                case "no":
                case "0":
                case "fail":
                    if (strictEnvelopeAvoidance)
                        return false;
                    accessible = false;
                    return true;

                default:
                    return false;
            }
        }
    }
}

