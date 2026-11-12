using System;
using System.IO;
using System.Text;
using System.Text.Json;

namespace BDSUtil.Helpers
{
    public static class BDSPackInstaller
    {
        // Migrated from PackManager
        public const string MONITOR_PACK_UUID = "4f9d2a6b-c7e1-432a-9e12-b5c6d7e8f9a0";
        public const string MONITOR_PACK_VERSION = "0, 1, 1";
        public const string MONITOR_MAIN_SCRIPT_VERSION = "0, 1, 1";

        // Using the required bdsutil_ prefix
        public const string MONITOR_PACK_FOLDER = "bdsutil_monitor";

        /// <summary>
        /// Natively builds and writes the production bds_monitor Bedrock Script API behavior pack files.
        /// </summary>
        public static bool InstallMonitorPackFiles(string serverRoot)
        {
            if (string.IsNullOrWhiteSpace(serverRoot) || !Directory.Exists(serverRoot)) return false;

            try
            {
                string packRoot = Path.Combine(serverRoot, "behavior_packs", MONITOR_PACK_FOLDER);
                string scriptsFolder = Path.Combine(packRoot, "scripts");

                // Establish clean file system structural coordinates
                Directory.CreateDirectory(packRoot);
                Directory.CreateDirectory(scriptsFolder);

                // Write the production JSON manifest configuration
                string manifestJson = GetManifestJsonContent();
                File.WriteAllText(Path.Combine(packRoot, "manifest.json"), manifestJson, Encoding.UTF8);

                // Write the explicit Bedrock event middleware listener script entrypoint
                string mainScript = GetMainScriptContent();
                File.WriteAllText(Path.Combine(scriptsFolder, "main.js"), mainScript, Encoding.UTF8);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[BDSPackInstaller Write Exception]: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Delegates to PackManager to register the BDSUtil Monitor pack into the target world.
        /// </summary>
        public static bool EnableMonitorPack(string serverRoot, string worldName)
        {
            return PackManager.EnablePackInWorld(serverRoot, worldName, MONITOR_PACK_UUID, MONITOR_PACK_VERSION, isBehaviorPack: true);
        }

        private static string GetManifestJsonContent()
        {
            var manifest = new
            {
                format_version = 2,
                header = new
                {
                    description = "BDSUtil Live Chat Bridge Log Monitor Middleware Pipeline",
                    name = MONITOR_PACK_FOLDER,
                    uuid = MONITOR_PACK_UUID,
                    version = ParseVersionTuple(MONITOR_PACK_VERSION),
                    min_engine_version = new[] { 1, 26, 50 }
                },
                modules = new[]
                {
                    new
                    {
                        description = "Script API Entrypoint",
                        type = "script",
                        language = "javascript",
                        uuid = "1b2c3d4e-5f6a-7b8c-9d0e-1f2a3b4c5d6e",
                        version = ParseVersionTuple(MONITOR_MAIN_SCRIPT_VERSION),
                        entry = "scripts/main.js"
                    }
                },
                dependencies = new[]
                {
                    new
                    {
                        module_name = "@minecraft/server",
                        version = "2.12.0-beta"
                    }
                }
            };

            return JsonSerializer.Serialize(manifest, JsonHelper.IndentedOptions);
        }

        private static int[] ParseVersionTuple(string version)
        {
            string[] parts = version.Split(new[] { ',', '.' }, StringSplitOptions.RemoveEmptyEntries);
            int[] result = new int[parts.Length];
            for (int i = 0; i < parts.Length; i++)
            {
                result[i] = int.TryParse(parts[i].Trim(), out int num) ? num : 0;
            }
            return result.Length > 0 ? result : new[] { 1, 0, 0 };
        }

        private static string GetMainScriptContent()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("import { world } from \"@minecraft/server\";");
            sb.AppendLine();
            sb.AppendLine("console.warn(\"[BDSUTIL_BRIDGE] Telemetry engine initializing...\");");
            sb.AppendLine();
            sb.AppendLine("// ==========================================");
            sb.AppendLine("// CHAT LOGGING ENGINE");
            sb.AppendLine("// ==========================================");
            sb.AppendLine("try {");
            sb.AppendLine("    if (world.afterEvents.chatSend) {");
            sb.AppendLine("        world.afterEvents.chatSend.subscribe((event) => {");
            sb.AppendLine("            if (!event || !event.sender) return;");
            sb.AppendLine("            console.warn(");
            sb.AppendLine("                `[BDSUTIL_BRIDGE][CHAT] player=\"${event.sender.name}\" message=${JSON.stringify(event.message)}`");
            sb.AppendLine("            );");
            sb.AppendLine("        });");
            sb.AppendLine("        console.warn(\"[BDSUTIL_BRIDGE][INIT] Chat logging engine online.\");");
            sb.AppendLine("    } else {");
            sb.AppendLine("        console.warn(");
            sb.AppendLine("            \"[BDSUTIL_BRIDGE][INIT_ERR] world.afterEvents.chatSend unavailable. \" +");
            sb.AppendLine("            \"Use @minecraft/server 2.11.0-beta and enable Beta APIs on the world.\"");
            sb.AppendLine("        );");
            sb.AppendLine("    }");
            sb.AppendLine("} catch (e) {");
            sb.AppendLine("    console.warn(`[BDSUTIL_BRIDGE][INIT_ERR] Chat channel failed: ${e.message}`);");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("// ==========================================");
            sb.AppendLine("// DEATH MONITORING ENGINE");
            sb.AppendLine("// ==========================================");
            sb.AppendLine("try {");
            sb.AppendLine("    world.afterEvents.entityDie.subscribe((event) => {");
            sb.AppendLine("        if (!event || !event.deadEntity || event.deadEntity.typeId !== \"minecraft:player\") return;");
            sb.AppendLine("        const player = event.deadEntity;");
            sb.AppendLine("        const damageSource = event.damageSource;");
            sb.AppendLine("        let cause = \"unknown\";");
            sb.AppendLine("        let killerName = \"Environment\";");
            sb.AppendLine("        if (damageSource) {");
            sb.AppendLine("            cause = damageSource.cause || \"unknown\";");
            sb.AppendLine("            if (damageSource.damagingEntity) {");
            sb.AppendLine("                if (damageSource.damagingEntity.typeId === \"minecraft:player\") {");
            sb.AppendLine("                    killerName = damageSource.damagingEntity.name;");
            sb.AppendLine("                    console.warn(`[BDSUTIL_BRIDGE][DEATH][PVP] victim=\"${player.name}\" killer=\"${killerName}\" cause=\"${cause}\"`);");
            sb.AppendLine("                    return;");
            sb.AppendLine("                } else {");
            sb.AppendLine("                    killerName = damageSource.damagingEntity.typeId.replace(\"minecraft:\", \"\");");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("        console.warn(`[BDSUTIL_BRIDGE][DEATH][ENV] victim=\"${player.name}\" source=\"${killerName}\" cause=\"${cause}\"`);");
            sb.AppendLine("    });");
            sb.AppendLine("    console.warn(\"[BDSUTIL_BRIDGE][INIT] Death telemetry engine online.\");");
            sb.AppendLine("} catch (e) {");
            sb.AppendLine("    console.warn(`[BDSUTIL_BRIDGE][INIT_ERR] Death channel failed: ${e.message}`);");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}