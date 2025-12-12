using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Encryption
{
    public static class EncryptionManager
    {
        public static string Decrypt(string input, string base64Key)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // If not valid Base64, treat as plaintext
            try
            {
                var cipherBytes = Convert.FromBase64String(input);
                var keyBytes = Convert.FromBase64String(base64Key);

                if (keyBytes.Length != 32)
                    throw new ArgumentException("Encryption key must be 32 bytes (Base64-encoded).");

                if (cipherBytes.Length < 17)
                    return input; // Not enough bytes for IV + ciphertext

                var iv = new byte[16];
                Array.Copy(cipherBytes, 0, iv, 0, 16);

                var actualCipher = new byte[cipherBytes.Length - 16];
                Array.Copy(cipherBytes, 16, actualCipher, 0, actualCipher.Length);

                using (var aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var decryptor = aes.CreateDecryptor())
                    using (var ms = new MemoryStream(actualCipher))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs, Encoding.UTF8))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch
            {
                // Not valid Base64 or decryption failed, treat as plaintext
                return input;
            }
        }

        public static string Encrypt(string plainText, string base64Key)
        {
            if (string.IsNullOrWhiteSpace(plainText))
                return plainText;

            var keyBytes = Convert.FromBase64String(base64Key);
            if (keyBytes.Length != 32)
                throw new ArgumentException("Encryption key must be 32 bytes (Base64-encoded).");

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length); // Prepend IV
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs, Encoding.UTF8))
                    {
                        sw.Write(plainText);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }
    }
}