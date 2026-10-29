using System;

namespace BDSUtil.Helpers.AutoMod
{
    public static class MessageProvider
    {
        private const string Prefix = "§8[§cAutoMod§8]§r";
        private const string WarningPrefix = "§8[§eAutoMod Warning§8]§r";

        /// <summary>
        /// Generates a private warning message sent directly to the violating player.
        /// </summary>
        public static string GetWarningCommand(string player, string reason)
        {
            string message = $"{WarningPrefix} {reason}";
            return FormatTellraw(player, message);
        }

        /// <summary>
        /// Generates the command to kick a player with a formatted disconnection screen message.
        /// </summary>
        public static string GetKickCommand(string player, string reason)
        {
            // Bedrock kick command format: kick "PlayerName" Reason
            return $"kick \"{player}\" {reason}";
        }

        /// <summary>
        /// Generates the commands to mute a player. 
        /// (Assumes your 'bdsutil_monitor' behavior pack restricts chat for players with the 'automod_mute' tag).
        /// </summary>
        public static string GetMuteCommand(string player, string durationText)
        {
            string tagCommand = $"tag \"{player}\" add automod_mute";
            string notifyCommand = FormatTellraw(player, $"{Prefix} §cYou have been muted for {durationText}.§r");

            // Return multiple commands separated by a newline so the console stream can process both
            return $"{tagCommand}\n{notifyCommand}";
        }

        /// <summary>
        /// Generates the command to lift a mute.
        /// </summary>
        public static string GetUnmuteCommand(string player)
        {
            string tagCommand = $"tag \"{player}\" remove automod_mute";
            string notifyCommand = FormatTellraw(player, $"{Prefix} §aYour mute has expired. You can speak again.§r");

            return $"{tagCommand}\n{notifyCommand}";
        }

        /// <summary>
        /// Generates a ban kick message. (The actual connection blocking is handled by the C# engine checking the .ep profile on join).
        /// </summary>
        public static string GetBanCommand(string player, string durationText, string reason)
        {
            string banMessage = $"BANNED: {durationText} | {reason}";
            return $"kick \"{player}\" {banMessage}";
        }

        /// <summary>
        /// Generates a global broadcast to all players on the server.
        /// </summary>
        public static string GetGlobalBroadcastCommand(string message)
        {
            return FormatTellraw("@a", $"{Prefix} {message}");
        }

        /// <summary>
        /// Helper method to safely format Bedrock JSON text injection.
        /// </summary>
        private static string FormatTellraw(string target, string text)
        {
            // Escape quotes inside the text to prevent JSON syntax crashes in the Bedrock console
            string safeText = text.Replace("\"", "\\\"");
            return $"tellraw \"{target}\" {{\"rawtext\":[{{\"text\":\"{safeText}\"}}]}}";
        }
    }
}