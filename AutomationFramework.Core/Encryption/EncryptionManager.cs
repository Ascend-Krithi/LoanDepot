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
                throw new ArgumentException("Key must be 32 bytes for AES-256.", nameof(base64Key));

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs, Encoding.UTF8))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText, string base64Key)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
                return cipherText;
            if (string.IsNullOrWhiteSpace(base64Key))
                throw new ArgumentNullException(nameof(base64Key));

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

            var keyBytes = Convert.FromBase64String(base64Key);
            if (keyBytes.Length != 32)
                throw new ArgumentException("Key must be 32 bytes for AES-256.", nameof(base64Key));

            if (cipherBytes.Length < 16)
                throw new ArgumentException("Cipher text too short to contain IV.", nameof(cipherText));

            var iv = new byte[16];
            Array.Copy(cipherBytes, 0, iv, 0, 16);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream(cipherBytes, 16, cipherBytes.Length - 16))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}