using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BDSUtil.Helpers.RemoteApi
{
    public sealed class RemoteActionResult
    {
        public bool Success { get; set; }
        public int HttpStatusCode { get; set; } = 200;
        public string Message { get; set; } = string.Empty;
        public object Data { get; set; }

        public static RemoteActionResult Ok(string message, object data = null) => new RemoteActionResult
        {
            Success = true,
            HttpStatusCode = 200,
            Message = message ?? string.Empty,
            Data = data
        };

        public static RemoteActionResult Accepted(string message, object data = null) => new RemoteActionResult
        {
            Success = true,
            HttpStatusCode = 202,
            Message = message ?? string.Empty,
            Data = data
        };

        public static RemoteActionResult Fail(int httpStatusCode, string message, object data = null) => new RemoteActionResult
        {
            Success = false,
            HttpStatusCode = httpStatusCode,
            Message = message ?? string.Empty,
            Data = data
        };
    }

    public sealed class RemoteServerPortsInfo
    {
        public int PrimaryIpv4 { get; set; }
        public int LowestServerPort { get; set; }
        public int MaxPlayers { get; set; }
        public List<int> UdpNodes { get; set; } = new List<int>();
    }

    public sealed class RemoteApiNetworkingInfo
    {
        public bool ApiRunning { get; set; }
        public int ApiPort { get; set; }
        public bool UpnpEnabled { get; set; }
        public bool ApiUpnpForwarded { get; set; }
        public int LeasedUpnpPort { get; set; }
        public string ExternalIp { get; set; } = string.Empty;
        public string LocalIp { get; set; } = string.Empty;
        public string ExternalUrl { get; set; } = string.Empty;
        public string LanUrl { get; set; } = string.Empty;
        public string UpnpVerifyState { get; set; } = string.Empty;
        public string StatusMessage { get; set; } = string.Empty;
    }

    public sealed class RemoteServerStatusSnapshot
    {
        public string WrapperVersion { get; set; } = "1.0.0";
        public string ServerVersion { get; set; } = string.Empty;
        public bool ServerRunning { get; set; }
        public bool ServerInitialized { get; set; }
        public int? ProcessId { get; set; }
        public double? UptimeSeconds { get; set; }
        public double? MemoryMb { get; set; }
        public bool ServerPathValid { get; set; }
        public bool BackupInProgress { get; set; }
        public bool AutoModEnabled { get; set; }
        public int OnlinePlayerCount { get; set; }
        public List<ConnectedPlayerInfo> OnlinePlayers { get; set; } = new List<ConnectedPlayerInfo>();
        public RemoteServerPortsInfo Ports { get; set; } = new RemoteServerPortsInfo();
        public RemoteApiNetworkingInfo RemoteApi { get; set; } = new RemoteApiNetworkingInfo();
        public DateTimeOffset TimestampUtc { get; set; } = DateTimeOffset.UtcNow;
    }

    public sealed class ConsoleLogEntry
    {
        public long Sequence { get; set; }
        public DateTimeOffset TimestampUtc { get; set; }
        public string Line { get; set; } = string.Empty;
    }

    public sealed class ConsoleLogPage
    {
        public long LatestSequence { get; set; }
        public long ReturnedFromSequence { get; set; }
        public List<ConsoleLogEntry> Entries { get; set; } = new List<ConsoleLogEntry>();
    }

    public sealed class ConnectedPlayerInfo
    {
        public string Gamertag { get; set; } = string.Empty;
        public string Xuid { get; set; } = string.Empty;
        public DateTimeOffset ConnectedAtUtc { get; set; }
    }

    public sealed class KnownPlayerInfo
    {
        public string Gamertag { get; set; } = string.Empty;
        public string Xuid { get; set; } = string.Empty;
    }

    public sealed class RemotePlayersSnapshot
    {
        public List<ConnectedPlayerInfo> OnlinePlayers { get; set; } = new List<ConnectedPlayerInfo>();
        public List<KnownPlayerInfo> KnownPlayers { get; set; } = new List<KnownPlayerInfo>();
    }

    public sealed class WhitelistEntryDto
    {
        public string Name { get; set; } = string.Empty;
        public string Xuid { get; set; }
        public bool IgnoresPlayerLimit { get; set; }
    }

    public sealed class OperatorEntryDto
    {
        public string Xuid { get; set; } = string.Empty;
        public string Gamertag { get; set; } = string.Empty;
        public string Permission { get; set; } = "operator";
    }

    public sealed class BackupArchiveDto
    {
        public string FileName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }
    }

    public sealed class RemoteConfigSummaryDto
    {
        public int ConfigVersion { get; set; }
        public bool EnableUPnP { get; set; }
        public bool ConsolidateUdpPorts { get; set; }
        public bool HideTelemetryWarnings { get; set; }
        public bool WorldEditorEnabled { get; set; }
        public int CompressionMode { get; set; }
        public bool AutoModEnabled { get; set; }
        public bool AutoModChatEnforcement { get; set; }
        public bool AutoModAntiSpam { get; set; }
        public bool RemoteApiEnabled { get; set; }
    }

    public sealed class RemoteUpnpMappingSummaryDto
    {
        public int ExternalPort { get; set; }
        public int InternalPort { get; set; }
        public string Protocol { get; set; } = "UDP";
        public string InternalClient { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Enabled { get; set; }
    }

    public sealed class RemoteUpnpStatusDto
    {
        public bool UpnpEnabled { get; set; }
        public bool GatewayFound { get; set; }
        public string GatewayDescription { get; set; } = string.Empty;
        public string ExternalIp { get; set; } = string.Empty;
        public string LocalIp { get; set; } = string.Empty;
        public int ApiPort { get; set; }
        public PortForwardVerificationResult PrimaryPortVerification { get; set; }
        public PortForwardVerificationResult ApiPortVerification { get; set; }
        public List<RemoteUpnpMappingSummaryDto> ActiveGatewayMappings { get; set; } = new List<RemoteUpnpMappingSummaryDto>();
    }

    // --- Incoming JSON Request Payloads ---

    public sealed class ConsoleCommandRequest
    {
        public string Command { get; set; } = string.Empty;
    }

    public sealed class ServerStopRequest
    {
        public bool Force { get; set; } = false;
    }

    public sealed class WhitelistAddRequest
    {
        public string Name { get; set; } = string.Empty;
        public bool IgnoresPlayerLimit { get; set; } = false;
    }

    public sealed class PlayerActionRequest
    {
        public string Action { get; set; } = string.Empty;
        public string Player { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public sealed class BackupTriggerRequest
    {
        public string Type { get; set; } = "worlds";
        public bool StopServerFirst { get; set; } = false;
    }

    public sealed class ServerPropertiesPatchRequest
    {
        public Dictionary<string, string> Properties { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    public sealed class AutoModTogglePatch
    {
        public bool? Enabled { get; set; }
        public bool? EnableChatEnforcement { get; set; }
        public bool? AntiSpamEnabled { get; set; }
        public bool? HideTelemetryWarnings { get; set; }
    }

    /// <summary>
    /// Host boundary implemented by BDSUtil (and test doubles) so the Remote Management HTTP server
    /// can drive server lifecycle, console, player/whitelist state, backups, config, and UPnP diagnostics safely.
    /// </summary>
    public interface IRemoteManagementHost
    {
        RemoteServerStatusSnapshot GetStatusSnapshot();
        ConsoleLogPage GetConsoleLogs(long sinceSequence, int limit);
        Task<RemoteActionResult> StartServerAsync(CancellationToken ct);
        Task<RemoteActionResult> StopServerAsync(bool force, CancellationToken ct);
        Task<RemoteActionResult> RestartServerAsync(CancellationToken ct);
        Task<RemoteActionResult> SendConsoleCommandAsync(string command, CancellationToken ct);
        RemotePlayersSnapshot GetPlayersSnapshot();
        Task<RemoteActionResult> ExecutePlayerActionAsync(PlayerActionRequest request, CancellationToken ct);
        IReadOnlyList<WhitelistEntryDto> GetWhitelist();
        Task<RemoteActionResult> AddWhitelistPlayerAsync(string name, bool ignoresPlayerLimit, CancellationToken ct);
        Task<RemoteActionResult> RemoveWhitelistPlayerAsync(string name, CancellationToken ct);
        IReadOnlyList<OperatorEntryDto> GetOperators();
        IReadOnlyDictionary<string, string> GetServerProperties();
        Task<RemoteActionResult> UpdateServerPropertiesAsync(IReadOnlyDictionary<string, string> updates, CancellationToken ct);
        IReadOnlyList<BackupArchiveDto> ListBackups();
        Task<RemoteActionResult> StartBackupAsync(string backupType, bool stopServerFirst, CancellationToken ct);
        RemoteConfigSummaryDto GetConfigSummary();
        RemoteConfigSummaryDto UpdateConfigToggles(AutoModTogglePatch patch);
        Task<RemoteUpnpStatusDto> GetUpnpStatusAsync(bool runVerification, CancellationToken ct);
    }
}

