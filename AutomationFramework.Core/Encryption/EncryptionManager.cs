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

        public static string Decrypt(string base64CipherText, string base64Key)
        {
            if (string.IsNullOrEmpty(base64CipherText)) return string.Empty;

            byte[] cipherBytes;
            try
            {
                cipherBytes = Convert.FromBase64String(base64CipherText);
            }
            catch (FormatException)
            {
                // If not valid Base64, treat as plaintext
                return base64CipherText;
            }

            if (string.IsNullOrEmpty(base64Key))
            {
                throw new ArgumentException("Encryption key cannot be null or empty for decryption.", nameof(base64Key));
            }

            var key = Convert.FromBase64String(base64Key);
            if (key.Length != AesKeySize)
            {
                throw new ArgumentException($"Encryption key must be {AesKeySize} bytes (256 bits) long.", nameof(base64Key));
            }

            if (cipherBytes.Length < AesIvSize)
            {
                throw new CryptographicException("Invalid ciphertext length. It must be at least the size of the IV.");
            }

            var iv = new byte[AesIvSize];
            var cipherTextBytes = new byte[cipherBytes.Length - AesIvSize];

            Buffer.BlockCopy(cipherBytes, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(cipherBytes, iv.Length, cipherTextBytes, 0, cipherTextBytes.Length);

            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var msDecrypt = new MemoryStream(cipherTextBytes);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);

            return srDecrypt.ReadToEnd();
        }

        public static string Encrypt(string plainText, string base64Key)
        {
            if (string.IsNullOrEmpty(plainText)) return string.Empty;
            if (string.IsNullOrEmpty(base64Key))
            {
                throw new ArgumentException("Encryption key cannot be null or empty for encryption.", nameof(base64Key));
            }

            var key = Convert.FromBase64String(base64Key);
            if (key.Length != AesKeySize)
            {
                throw new ArgumentException($"Encryption key must be {AesKeySize} bytes (256 bits) long.", nameof(base64Key));
            }

            using var aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV(); // Generate a new IV for each encryption
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var iv = aes.IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var msEncrypt = new MemoryStream();
            using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }

            var encryptedContent = msEncrypt.ToArray();
            var result = new byte[iv.Length + encryptedContent.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(encryptedContent, 0, result, iv.Length, encryptedContent.Length);

            return Convert.ToBase64String(result);
        }
    }
}