using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Utilities
{
    public static class EncryptionService
    {
        private static readonly string Key = "ReplaceWithYourKey123"; // Should be stored securely

        public static string Decrypt(string encryptedText)
        {
            // Dummy implementation for illustration. Replace with actual AES decryption logic.
            // Assume encryptedText is base64-encoded for this example.
            try
            {
                byte[] cipherBytes = Convert.FromBase64String(encryptedText);
                return Encoding.UTF8.GetString(cipherBytes);
            }
            catch
            {
                return encryptedText; // Return as-is if not encrypted
            }
        }
    }
}