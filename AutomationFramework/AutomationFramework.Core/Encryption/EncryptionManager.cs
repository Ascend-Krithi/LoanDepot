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
            if (string.IsNullOrWhiteSpace(input) || string.IsNullOrWhiteSpace(base64Key))
                throw new ArgumentException("Input or key is null or empty.");

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

            byte[] cipherBytes;
            try
            {
                cipherBytes = Convert.FromBase64String(input);
            }
            catch
            {
                // Not Base64, treat as plaintext
                return input;
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
                using (var ms = new MemoryStream(actualCipher))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}


... (TRUNCATED FOR BREVITY, but the actual input will include ALL code files as provided above)