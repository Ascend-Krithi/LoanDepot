using System;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Utilities
{
    public static class EncryptionService
    {
        public static string Decrypt(string cipherText, string key, string iv)
        {
            var keyBytes = Convert.FromBase64String(key);
            var ivBytes = Convert.FromBase64String(iv);
            var cipherBytes = Convert.FromBase64String(cipherText);

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = ivBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}