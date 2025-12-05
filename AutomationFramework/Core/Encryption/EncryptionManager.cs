using System;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Encryption
{
    public static class EncryptionManager
    {
        public static string Encrypt(string plainText, string key)
        {
            using var aes = Aes.Create();
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
            aes.IV = new byte[16];
            using var enc = aes.CreateEncryptor(aes.Key, aes.IV);
            var input = Encoding.UTF8.GetBytes(plainText);
            var cipher = enc.TransformFinalBlock(input, 0, input.Length);
            return Convert.ToBase64String(cipher);
        }

        public static string Decrypt(string cipherText, string key)
        {
            using var aes = Aes.Create();
            aes.Key = SHA256.HashData(Encoding.UTF8.GetBytes(key));
            aes.IV = new byte[16];
            using var dec = aes.CreateDecryptor(aes.Key, aes.IV);
            var input = Convert.FromBase64String(cipherText);
            var plain = dec.TransformFinalBlock(input, 0, input.Length);
            return Encoding.UTF8.GetString(plain);
        }
    }
}