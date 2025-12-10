using System;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Utilities
{
    public static class EncryptionService
    {
        public static string Encrypt(string plainText, string hexKey, string hexIV)
        {
            var key = HexStringToBytes(hexKey);
            var iv = HexStringToBytes(hexIV);
            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using var encryptor = aes.CreateEncryptor(key, iv);
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
            return Convert.ToBase64String(cipherBytes);
        }

        public static string Decrypt(string base64Cipher, string hexKey, string hexIV)
        {
            var key = HexStringToBytes(hexKey);
            var iv = HexStringToBytes(hexIV);
            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using var decryptor = aes.CreateDecryptor(key, iv);
            var cipherBytes = Convert.FromBase64String(base64Cipher);
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }

        private static byte[] HexStringToBytes(string hex)
        {
            if (hex.Length % 2 != 0)
                throw new ArgumentException("Invalid hex key/IV length.");
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return bytes;
        }
    }
}