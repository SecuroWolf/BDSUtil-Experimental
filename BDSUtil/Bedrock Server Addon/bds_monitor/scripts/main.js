import { world } from "@minecraft/server";

console.warn("[BDSUTIL_BRIDGE] Telemetry engine initializing...");

// ==========================================
// 💬 CHAT LOGGING ENGINE
// ==========================================
// chatSend is still a beta API in @minecraft/server (2.11.0-beta for 26.50/26.52).
// It needs "@minecraft/server": "2.11.0-beta" in the manifest AND the Beta APIs
// toggle enabled on the world. If either is missing, the property is undefined,
// so this guard logs a clear warning instead of crashing the script.
try {
    if (world.afterEvents.chatSend) {
        world.afterEvents.chatSend.subscribe((event) => {
            if (!event || !event.sender) return;

            // JSON.stringify escapes quotes/newlines so the log line stays parseable
            console.warn(
                `[BDSUTIL_BRIDGE][CHAT] player="${event.sender.name}" message=${JSON.stringify(event.message)}`
            );
        });
        console.warn("[BDSUTIL_BRIDGE][INIT] Chat logging engine online.");
    } else {
        console.warn(
            "[BDSUTIL_BRIDGE][INIT_ERR] world.afterEvents.chatSend unavailable. " +
            "Use @minecraft/server 2.11.0-beta and enable Beta APIs on the world."
        );
    }
} catch (e) {
    console.warn(`[BDSUTIL_BRIDGE][INIT_ERR] Chat channel failed: ${e.message}`);
}

// ==========================================
// 💀 DEATH MONITORING ENGINE
// ==========================================
try {
    world.afterEvents.entityDie.subscribe((event) => {
        if (!event || !event.deadEntity || event.deadEntity.typeId !== "minecraft:player") return;

        const player = event.deadEntity;
        const damageSource = event.damageSource;

        let cause = "unknown";
        let killerName = "Environment";

        if (damageSource) {
            cause = damageSource.cause || "unknown";

            if (damageSource.damagingEntity) {
                if (damageSource.damagingEntity.typeId === "minecraft:player") {
                    killerName = damageSource.damagingEntity.name;
                    console.warn(`[BDSUTIL_BRIDGE][DEATH][PVP] victim="${player.name}" killer="${killerName}" cause="${cause}"`);
                    return;
                } else {
                    killerName = damageSource.damagingEntity.typeId.replace("minecraft:", "");
                }
            }
        }

        console.warn(`[BDSUTIL_BRIDGE][DEATH][ENV] victim="${player.name}" source="${killerName}" cause="${cause}"`);
    });
    console.warn("[BDSUTIL_BRIDGE][INIT] Death telemetry engine online.");
} catch (e) {
    console.warn(`[BDSUTIL_BRIDGE][INIT_ERR] Death channel failed: ${e.message}`);
}