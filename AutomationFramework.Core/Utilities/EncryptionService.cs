using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Utilities
{
    public static class EncryptionService
    {
        private static readonly string Key = "YourEncryptionKey123"; // Should be stored securely

        public static string Decrypt(string encryptedText)
        {
            // AES decryption logic
            // For demonstration, returns the input (replace with real decryption)
            return encryptedText;
        }
    }
}