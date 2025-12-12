// AutomationFramework.Core/Encryption/EncryptionManager.cs
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Encryption
{
    public static class EncryptionManager
    {
        private const int AesKeySize = 32; // 256 bits
        private const int AesIvSize = 16;  // 128 bits

        public static string Encrypt(string plainText, string base64Key)
        {
            if (string.IsNullOrEmpty(plainText)) return plainText;

            var key = GetValidatedKey(base64Key);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateIV();
                var iv = aes.IV;

                using (var encryptor = aes.CreateEncryptor(aes.Key, iv))
                using (var ms = new MemoryStream())
                {
                    ms.Write(iv, 0, iv.Length); // Prepend IV
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        var plainBytes = Encoding.UTF8.GetBytes(plainText);
                        cs.Write(plainBytes, 0, plainBytes.Length);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText, string base64Key)
        {
            if (string.IsNullOrEmpty(cipherText)) return cipherText;

            // If not valid Base64, treat as plaintext
            try
            {
                Convert.FromBase64String(cipherText);
            }
            catch (FormatException)
            {
                return cipherText;
            }

            var key = GetValidatedKey(base64Key);
            var fullCipherBytes = Convert.FromBase64String(cipherText);

            if (fullCipherBytes.Length < AesIvSize)
            {
                throw new ArgumentException("Cipher text is too short to contain an IV.", nameof(cipherText));
            }

            var iv = new byte[AesIvSize];
            Array.Copy(fullCipherBytes, 0, iv, 0, iv.Length);

            var cipherBytes = new byte[fullCipherBytes.Length - AesIvSize];
            Array.Copy(fullCipherBytes, AesIvSize, cipherBytes, 0, cipherBytes.Length);

            using (var aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(cipherBytes))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private static byte[] GetValidatedKey(string base64Key)
        {
            if (string.IsNullOrEmpty(base64Key))
            {
                throw new ArgumentException("Encryption key cannot be null or empty.");
            }

            var key = Convert.FromBase64String(base64Key);
            if (key.Length != AesKeySize)
            {
                throw new ArgumentException($"Encryption key must be exactly {AesKeySize} bytes (256 bits) long, but was {key.Length} bytes.");
            }
            return key;
        }
    }
}