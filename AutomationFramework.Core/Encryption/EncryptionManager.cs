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
                throw new ArgumentException("Input or key is null/empty.");

            // If not valid Base64, treat as plaintext
            try
            {
                var cipherBytes = Convert.FromBase64String(input);
                var keyBytes = Convert.FromBase64String(base64Key);

                if (keyBytes.Length != 32)
                    throw new ArgumentException("Encryption key must be exactly 32 bytes (Base64 encoded).");

                if (cipherBytes.Length < 16)
                    throw new ArgumentException("Ciphertext too short to contain IV.");

                var iv = new byte[16];
                Buffer.BlockCopy(cipherBytes, 0, iv, 0, 16);

                var actualCipher = new byte[cipherBytes.Length - 16];
                Buffer.BlockCopy(cipherBytes, 16, actualCipher, 0, actualCipher.Length);

                using (var aes = Aes.Create())
                {
                    aes.Key = keyBytes;
                    aes.IV = iv;
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    using (var decryptor = aes.CreateDecryptor())
                    {
                        var plainBytes = decryptor.TransformFinalBlock(actualCipher, 0, actualCipher.Length);
                        return Encoding.UTF8.GetString(plainBytes);
                    }
                }
            }
            catch (FormatException)
            {
                // Not Base64, treat as plaintext
                return input;
            }
        }
    }
}