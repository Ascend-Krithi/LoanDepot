using System;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Encryption
{
    public static class EncryptionManager
    {
        public static string Encrypt(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText)) return string.Empty;
            using var aes = Aes.Create();
            aes.Key = DeriveKey(key);
            aes.IV = new byte[16];
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            var bytes = Encoding.UTF8.GetBytes(plainText);
            var encrypted = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string cipherText, string key)
        {
            if (string.IsNullOrEmpty(cipherText)) return string.Empty;
            using var aes = Aes.Create();
            aes.Key = DeriveKey(key);
            aes.IV = new byte[16];
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            var bytes = Convert.FromBase64String(cipherText);
            var decrypted = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(decrypted);
        }

        private static byte[] DeriveKey(string key)
        {
            using var sha = SHA256.Create();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(key));
        }
    }
}
