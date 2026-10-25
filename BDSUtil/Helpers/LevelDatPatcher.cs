using System;
using System.IO;
using System.Text;

namespace BDSUtil.Helpers
{
    public static class LevelDatPatcher
    {
        private const int HEADER_SIZE_BYTES = 8;

        public static bool EnableBetaApis(string levelDatPath)
        {
            return SetExperimentState(levelDatPath, "gametest", true);
        }

        /// <summary>
        /// Scans for specific experimental token tags inside uncompressed binary streams.
        /// </summary>
        public static bool IsExperimentEnabled(string levelDatPath, string experimentToken)
        {
            if (!File.Exists(levelDatPath)) return false;

            try
            {
                byte[] fileBytes = File.ReadAllBytes(levelDatPath);
                if (fileBytes.Length < HEADER_SIZE_BYTES) return false;

                byte[] tokenSignature = Encoding.UTF8.GetBytes(experimentToken);
                int matchIndex = FindBytePatternMatch(fileBytes, tokenSignature, HEADER_SIZE_BYTES);

                if (matchIndex != -1)
                {
                    int valueOffset = matchIndex + tokenSignature.Length;
                    if (valueOffset < fileBytes.Length)
                    {
                        return fileBytes[valueOffset] == 1;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LevelDatPatcher Read Exception]: {ex.Message}");
            }

            return false;
        }

        private static int FindBytePatternMatch(byte[] sourceBuffer, byte[] matchPattern, int startSearchIndex)
        {
            for (int i = startSearchIndex; i <= sourceBuffer.Length - matchPattern.Length; i++)
            {
                bool matchFound = true;
                for (int j = 0; j < matchPattern.Length; j++)
                {
                    if (sourceBuffer[i + j] != matchPattern[j])
                    {
                        matchFound = false;
                        break;
                    }
                }
                if (matchFound) return i;
            }
            return -1;
        }
        /// <summary>
        /// Slices new tags inside the compound and flips master activation track status bits safely.
        /// </summary>
        public static bool SetExperimentState(string levelDatPath, string experimentToken, bool enable)
        {
            if (!File.Exists(levelDatPath)) return false;

            try
            {
                string directory = Path.GetDirectoryName(levelDatPath);
                string bakPath = Path.Combine(directory, $"level.dat.bak_{DateTime.Now:yyyyMMdd_HHmmss}");
                File.Copy(levelDatPath, bakPath, true);

                byte[] fileBytes = File.ReadAllBytes(levelDatPath);
                if (fileBytes.Length < HEADER_SIZE_BYTES) return false;

                // 1. Ensure master tracking parameters are forced active across structural paths
                FlipMasterActivationFlags(fileBytes);

                byte[] tokenSignature = Encoding.UTF8.GetBytes(experimentToken);
                int matchIndex = FindBytePatternMatch(fileBytes, tokenSignature, HEADER_SIZE_BYTES);

                if (matchIndex != -1)
                {
                    int valueOffset = matchIndex + tokenSignature.Length;
                    if (valueOffset < fileBytes.Length)
                    {
                        fileBytes[valueOffset] = (byte)(enable ? 1 : 0);
                    }
                }
                else if (enable)
                {
                    byte[] compoundSignature = Encoding.UTF8.GetBytes("experiments");
                    int compoundIndex = FindBytePatternMatch(fileBytes, compoundSignature, HEADER_SIZE_BYTES);

                    if (compoundIndex != -1)
                    {
                        // Splice the new sub-tag token payload directly into the compound block structure
                        int insertionPoint = compoundIndex + compoundSignature.Length;
                        fileBytes = InjectTagIntoCompound(fileBytes, insertionPoint, experimentToken);
                    }
                }

                // 2. Align Little-Endian uncompressed data payload length boundaries
                int newPayloadLength = fileBytes.Length - HEADER_SIZE_BYTES;
                byte[] lengthHeaderBytes = BitConverter.GetBytes(newPayloadLength);
                Array.Copy(lengthHeaderBytes, 0, fileBytes, 4, 4);

                File.WriteAllBytes(levelDatPath, fileBytes);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LevelDatPatcher Write Exception]: {ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// Locates master verification keys and flips their value states from 0 to 1 automatically.
        /// </summary>
        private static void FlipMasterActivationFlags(byte[] buffer)
        {
            string[] trackingKeys = { "experiments_ever_used", "saved_with_toggled_experiments" };
            foreach (string key in trackingKeys)
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                int matchIdx = FindBytePatternMatch(buffer, keyBytes, HEADER_SIZE_BYTES);
                if (matchIdx != -1)
                {
                    int valOffset = matchIdx + keyBytes.Length;
                    if (valOffset < buffer.Length)
                    {
                        buffer[valOffset] = 1; // Flip state status cleanly to active
                    }
                }
            }
        }

        private static byte[] InjectTagIntoCompound(byte[] originalBuffer, int offset, string token)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Write preceding data sections
                ms.Write(originalBuffer, 0, offset);

                // NBT element structure format rules: TAG_Byte (0x01)
                ms.WriteByte(1);

                // Write string name length metadata (Int16 Little-Endian representation bounds)
                byte[] nameLenBytes = BitConverter.GetBytes((short)token.Length);
                ms.Write(nameLenBytes, 0, 2);

                // Append uncompressed string name payload bytes
                byte[] nameBytes = Encoding.UTF8.GetBytes(token);
                ms.Write(nameBytes, 0, nameBytes.Length);

                // Force toggle state value parameter cleanly active (1 = True)
                ms.WriteByte(1);

                // Append remaining file tracking elements securely
                ms.Write(originalBuffer, offset, originalBuffer.Length - offset);
                return ms.ToArray();
            }
        }
    }
}
