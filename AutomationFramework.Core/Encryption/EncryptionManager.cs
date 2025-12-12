using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Encryption
{
    public static class EncryptionManager
    {
        public static string Encrypt(string plainText, string base64Key)
        {
            if (string.IsNullOrWhiteSpace(plainText))
                throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrWhiteSpace(base64Key))
                throw new ArgumentNullException(nameof(base64Key));

            var keyBytes = Convert.FromBase64String(base64Key);
            if (keyBytes.Length != 32)
                throw new ArgumentException("Key must be 32 bytes (256 bits) in Base64.", nameof(base64Key));

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var result = new byte[aes.IV.Length + cipherBytes.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
            Buffer.BlockCopy(cipherBytes, 0, result, aes.IV.Length, cipherBytes.Length);

            return Convert.ToBase64String(result);
        }

        public static string Decrypt(string cipherText, string base64Key)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
                return cipherText;
            if (string.IsNullOrWhiteSpace(base64Key))
                throw new ArgumentNullException(nameof(base64Key));

            byte[] keyBytes = Convert.FromBase64String(base64Key);
            if (keyBytes.Length != 32)
                throw new ArgumentException("Key must be 32 bytes (256 bits) in Base64.", nameof(base64Key));

            byte[] cipherBytes;
            try
            {
                cipherBytes = Convert.FromBase64String(cipherText);
            }
            catch
            {
                // Not base64, treat as plaintext
                return cipherText;
            }

            if (cipherBytes.Length < 16)
                throw new ArgumentException("Cipher text is too short to contain IV.", nameof(cipherText));

            byte[] iv = new byte[16];
            Buffer.BlockCopy(cipherBytes, 0, iv, 0, 16);
            byte[] actualCipher = new byte[cipherBytes.Length - 16];
            Buffer.BlockCopy(cipherBytes, 16, actualCipher, 0, actualCipher.Length);

            using var aes = Aes.Create();
            aes.Key = keyBytes;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(actualCipher, 0, actualCipher.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }
    }
}