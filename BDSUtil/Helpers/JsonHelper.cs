using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BDSUtil.Helpers
{
    /// <summary>
    /// Centralized System.Text.Json options, converters, and helper methods used across BDSUtil.
    /// </summary>
    public static class JsonHelper
    {
        public static readonly JsonSerializerOptions IndentedOptions = CreateOptions(indented: true);
        public static readonly JsonSerializerOptions CompactOptions = CreateOptions(indented: false);

        public static readonly JsonDocumentOptions DocumentOptions = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        private static JsonSerializerOptions CreateOptions(bool indented)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = indented,
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            options.Converters.Add(new NullableDateTimeJsonConverter());
            options.Converters.Add(new FlexibleNullableIntJsonConverter());
            return options;
        }

        private static void EnsureParentDirectory(string targetPath)
        {
            string directory = Path.GetDirectoryName(targetPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static string CreateStagingPath(string targetPath)
        {
            return $"{targetPath}.{Guid.NewGuid():N}.tmp";
        }

        /// <summary>
        /// Writes text content to disk atomically using a temporary staging file.
        /// </summary>
        public static void WriteTextAtomically(string targetPath, string content)
        {
            EnsureParentDirectory(targetPath);
            string tempPath = CreateStagingPath(targetPath);
            try
            {
                File.WriteAllText(tempPath, content ?? string.Empty, Encoding.UTF8);
                File.Move(tempPath, targetPath, overwrite: true);
            }
            catch
            {
                try { if (File.Exists(tempPath)) File.Delete(tempPath); } catch { }
                throw;
            }
        }

        /// <summary>
        /// Streams UTF-8 JSON from disk into a strongly typed value, returning whether deserialization succeeded.
        /// </summary>
        public static bool TryDeserializeFile<T>(string path, out T value, JsonSerializerOptions options = null)
        {
            value = default;
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                return false;
            }

            try
            {
                using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                if (stream.Length == 0)
                {
                    return false;
                }

                value = JsonSerializer.Deserialize<T>(stream, options ?? IndentedOptions);
                return value != null;
            }
            catch (Exception ex) when (ex is JsonException || ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
            {
                System.Diagnostics.Debug.WriteLine($"JSON read failed for '{path}': {ex.Message}");
                value = default;
                return false;
            }
        }

        /// <summary>
        /// Streams UTF-8 JSON from disk into a strongly typed value, returning <paramref name="fallback"/> if missing or invalid.
        /// </summary>
        public static T DeserializeFileOrDefault<T>(string path, T fallback = default, JsonSerializerOptions options = null)
        {
            return TryDeserializeFile(path, out T value, options) ? value : fallback;
        }

        /// <summary>
        /// Serializes <paramref name="value"/> directly as UTF-8 JSON to a staging file and replaces <paramref name="targetPath"/> atomically.
        /// </summary>
        public static void WriteJsonFileAtomically<T>(string targetPath, T value, JsonSerializerOptions options = null)
        {
            EnsureParentDirectory(targetPath);
            string tempPath = CreateStagingPath(targetPath);
            try
            {
                using (FileStream stream = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    JsonSerializer.Serialize(stream, value, options ?? IndentedOptions);
                    stream.Flush();
                }

                File.Move(tempPath, targetPath, overwrite: true);
            }
            catch
            {
                try { if (File.Exists(tempPath)) File.Delete(tempPath); } catch { }
                throw;
            }
        }

        /// <summary>
        /// Performs a case-insensitive property lookup on a JsonElement object.
        /// </summary>
        public static bool TryGetPropertyIgnoreCase(JsonElement element, string propertyName, out JsonElement value)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                if (element.TryGetProperty(propertyName, out value))
                {
                    return true;
                }

                foreach (JsonProperty prop in element.EnumerateObject())
                {
                    if (prop.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase))
                    {
                        value = prop.Value;
                        return true;
                    }
                }
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Reads a property string value from a JSON object element using a case-insensitive property lookup.
        /// </summary>
        public static string GetPropertyString(JsonElement element, string propertyName, string fallback = "")
        {
            return TryGetPropertyIgnoreCase(element, propertyName, out JsonElement propValue)
                ? GetStringValue(propValue, fallback)
                : fallback;
        }

        /// <summary>
        /// Reads a string value from a JsonElement safely regardless of whether the underlying token is string, number, or boolean.
        /// </summary>
        public static string GetStringValue(JsonElement element, string fallback = "")
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.String:
                    return element.GetString() ?? fallback;
                case JsonValueKind.True:
                    return "true";
                case JsonValueKind.False:
                    return "false";
                case JsonValueKind.Number:
                    return element.GetRawText();
                default:
                    return fallback;
            }
        }

        /// <summary>
        /// Reads a boolean value from a JsonElement safely, supporting both native JSON booleans and stringified "true"/"false".
        /// </summary>
        public static bool GetBoolValue(JsonElement element, bool fallback = false)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.True:
                    return true;
                case JsonValueKind.False:
                    return false;
                case JsonValueKind.String:
                    string str = element.GetString();
                    if (bool.TryParse(str, out bool parsed)) return parsed;
                    if (int.TryParse(str, NumberStyles.Integer, CultureInfo.InvariantCulture, out int num)) return num != 0;
                    return fallback;
                case JsonValueKind.Number:
                    if (element.TryGetInt32(out int intVal)) return intVal != 0;
                    return fallback;
                default:
                    return fallback;
            }
        }

        /// <summary>
        /// Reads an integer value from a JsonElement safely, supporting both JSON numbers and stringified integers.
        /// </summary>
        public static int GetIntValue(JsonElement element, int fallback = 0)
        {
            if (element.ValueKind == JsonValueKind.Number && element.TryGetInt32(out int num))
            {
                return num;
            }

            if (element.ValueKind == JsonValueKind.String)
            {
                string raw = element.GetString();
                if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed))
                {
                    return parsed;
                }
            }

            return fallback;
        }

        /// <summary>
        /// Reads a nullable integer value from a JsonElement safely, treating empty strings and nulls as null.
        /// </summary>
        public static int? GetNullableIntValue(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Null || element.ValueKind == JsonValueKind.Undefined)
            {
                return null;
            }

            if (element.ValueKind == JsonValueKind.Number && element.TryGetInt32(out int num))
            {
                return num;
            }

            if (element.ValueKind == JsonValueKind.String)
            {
                string raw = element.GetString();
                if (string.IsNullOrWhiteSpace(raw)) return null;
                if (int.TryParse(raw.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed))
                {
                    return parsed;
                }
            }

            return null;
        }

        /// <summary>
        /// Extracts a quoted or JSON.stringify-encoded bridge token value (e.g. player="..." or message="...") from a log line.
        /// </summary>
        public static string ExtractBridgeTokenValue(string line, string tokenName)
        {
            if (string.IsNullOrEmpty(line) || string.IsNullOrEmpty(tokenName))
                return string.Empty;

            int idx = line.IndexOf(tokenName, StringComparison.Ordinal);
            if (idx == -1) return string.Empty;

            int valueStart = idx + tokenName.Length;
            if (valueStart >= line.Length) return string.Empty;

            if (line[valueStart] == '"')
            {
                try
                {
                    byte[] utf8 = Encoding.UTF8.GetBytes(line.Substring(valueStart));
                    var reader = new Utf8JsonReader(utf8);
                    if (reader.Read() && reader.TokenType == JsonTokenType.String)
                    {
                        return reader.GetString() ?? string.Empty;
                    }
                }
                catch
                {
                    // Fallback below if the token was emitted without full JSON string escaping.
                }

                int fallbackStart = valueStart + 1;
                int fallbackEnd = line.IndexOf('"', fallbackStart);
                if (fallbackEnd > fallbackStart)
                {
                    return line.Substring(fallbackStart, fallbackEnd - fallbackStart);
                }
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Handles DateTime? deserialization gracefully when legacy JSON files represent null as "".
    /// </summary>
    public sealed class NullableDateTimeJsonConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string raw = reader.GetString();
                if (string.IsNullOrWhiteSpace(raw)) return null;

                if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out DateTime roundtrip))
                    return roundtrip;

                if (DateTime.TryParse(raw, out DateTime parsed))
                    return parsed;

                return null;
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString("O", CultureInfo.InvariantCulture));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }

    /// <summary>
    /// Handles int? deserialization gracefully when JSON represents empty values as "" or numbers as strings.
    /// </summary>
    public sealed class FlexibleNullableIntJsonConverter : JsonConverter<int?>
    {
        public override int? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonTokenType.Number && reader.TryGetInt32(out int num))
            {
                return num;
            }

            if (reader.TokenType == JsonTokenType.String)
            {
                string raw = reader.GetString();
                if (string.IsNullOrWhiteSpace(raw)) return null;
                if (int.TryParse(raw.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsed))
                    return parsed;
            }

            return null;
        }

        public override void Write(Utf8JsonWriter writer, int? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteNumberValue(value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
