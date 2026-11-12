using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BDSUtil.Configuration
{
    /// <summary>
    /// Core application and server wrapper preferences.
    /// </summary>
    public sealed class GeneralSettings
    {
        public string ServerExecutablePath { get; set; } = string.Empty;
        public int CompressionMode { get; set; } = 0;
        public bool WorldEditorEnabled { get; set; } = false;
        public bool ExperimentsWarned { get; set; } = false;
        public bool HideTelemetryWarnings { get; set; } = false;

        /// <summary>
        /// Preserves unrecognized properties inside the General section for forward/backward compatibility.
        /// </summary>
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtensionData { get; set; }
    }
}

