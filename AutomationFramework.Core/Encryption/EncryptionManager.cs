using System;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Encryption
{
    public static class EncryptionManager
    {
        public static string Decrypt(string input, string base64Key)
        {
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(base64Key))
                throw new ArgumentException("Input or key is null or empty.");

            // If not valid Base64, treat as plain text
            try
            {
                var cipherBytes = Convert.FromBase64String(input);
                var keyBytes = Convert.FromBase64String(base64Key);
                if (keyBytes.Length != 32)
                    throw new ArgumentException("Key must be 32 bytes for AES-256.");

                var iv = new byte[16];
                Array.Copy(cipherBytes, 0, iv, 0, 16);
                var cipherText = new byte[cipherBytes.Length - 16];
                Array.Copy(cipherBytes, 16, cipherText, 0, cipherText.Length);

                using (var aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var decryptor = aes.CreateDecryptor())
                    {
                        var plainBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                        return Encoding.UTF8.GetString(plainBytes);
                    }
                }
            }
            catch (FormatException)
            {
                // Not Base64, return as is
                return input;
            }
        }

        public static string Encrypt(string plainText, string base64Key)
        {
            if (string.IsNullOrWhiteSpace(plainText) || string.IsNullOrWhiteSpace(base64Key))
                throw new ArgumentException("PlainText or key is null or empty.");

            var keyBytes = Convert.FromBase64String(base64Key);
            if (keyBytes.Length != 32)
                throw new ArgumentException("Key must be 32 bytes for AES-256.");

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateIV();
                var iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor())
                {
                    var plainBytes = Encoding.UTF8.GetBytes(plainText);
                    var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    var result = new byte[iv.Length + cipherBytes.Length];
                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(cipherBytes, 0, result, iv.Length, cipherBytes.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }
    }
}