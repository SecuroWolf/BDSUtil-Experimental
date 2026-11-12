using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BDSUtil.Configuration
{
    /// <summary>
    /// Root unified configuration document persisted to config.json.
    /// Add new configuration domains as properties here, or store dynamic feature modules in <see cref="Modules"/>.
    /// </summary>
    public sealed class AppConfig
    {
        public const int CurrentConfigVersion = 2;

        public int ConfigVersion { get; set; } = CurrentConfigVersion;
        public GeneralSettings General { get; set; } = new GeneralSettings();
        public NetworkingSettings Networking { get; set; } = new NetworkingSettings();
        public RemoteApiSettings RemoteApi { get; set; } = new RemoteApiSettings();
        public AutoModSettings AutoMod { get; set; } = new AutoModSettings();
        public UtilPackSettings UtilPack { get; set; } = new UtilPackSettings();

        /// <summary>
        /// Modular storage slot for future subsystems or plugin settings without altering the core schema.
        /// </summary>
        public Dictionary<string, JsonElement> Modules { get; set; } = new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Preserves any unknown root-level properties across serialization cycles.
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }

        /// <summary>
        /// Ensures all nested sections and collections are non-null and initialized with valid defaults.
        /// </summary>
        public void Normalize()
        {
            ConfigVersion = CurrentConfigVersion;

            General ??= new GeneralSettings();
            General.ServerExecutablePath ??= string.Empty;

            Networking ??= new NetworkingSettings();
            Networking.Normalize();

            RemoteApi ??= new RemoteApiSettings();
            RemoteApi.Normalize();

            AutoMod ??= new AutoModSettings();
            AutoMod.Normalize();

            UtilPack ??= new UtilPackSettings();
            UtilPack.Normalize();

            Modules = Modules != null
                ? new Dictionary<string, JsonElement>(Modules, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, JsonElement>(StringComparer.OrdinalIgnoreCase);
        }
    }
}

