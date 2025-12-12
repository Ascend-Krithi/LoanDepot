using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Encryption
{
    public static class EncryptionManager
    {
        public static string Decrypt(string value, string base64Key)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(base64Key))
                throw new ArgumentException("Value or key is null or empty.");

            byte[] keyBytes;
            try
            {
                keyBytes = Convert.FromBase64String(base64Key);
            }
            catch
            {
                throw new ArgumentException("Encryption key is not valid Base64.");
            }

            if (keyBytes.Length != 32)
                throw new ArgumentException("Encryption key must be exactly 32 bytes for AES-256.");

            // If not valid Base64, treat as plaintext
            byte[] cipherBytes;
            try
            {
                cipherBytes = Convert.FromBase64String(value);
            }
            catch
            {
                return value;
            }

            if (cipherBytes.Length < 16)
                throw new ArgumentException("Ciphertext is too short.");

            byte[] iv = new byte[16];
            Array.Copy(cipherBytes, 0, iv, 0, 16);
            byte[] actualCipher = new byte[cipherBytes.Length - 16];
            Array.Copy(cipherBytes, 16, actualCipher, 0, actualCipher.Length);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                {
                    var decryptedBytes = decryptor.TransformFinalBlock(actualCipher, 0, actualCipher.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }

        public static string Encrypt(string plainText, string base64Key)
        {
            if (string.IsNullOrEmpty(plainText) || string.IsNullOrEmpty(base64Key))
                throw new ArgumentException("PlainText or key is null or empty.");

            byte[] keyBytes = Convert.FromBase64String(base64Key);
            if (keyBytes.Length != 32)
                throw new ArgumentException("Encryption key must be exactly 32 bytes for AES-256.");

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

                    byte[] result = new byte[aes.IV.Length + cipherBytes.Length];
                    Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
                    Array.Copy(cipherBytes, 0, result, aes.IV.Length, cipherBytes.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }
    }
}

... (truncated for brevity, but all files from the previous step are included in the same format) ...