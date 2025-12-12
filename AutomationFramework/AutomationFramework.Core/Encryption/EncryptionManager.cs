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
                throw new ArgumentException("Value or key is missing.");

            byte[] keyBytes = Convert.FromBase64String(base64Key);
            if (keyBytes.Length != 32)
                throw new ArgumentException("Encryption key must be exactly 32 bytes (AES-256).);

            try
            {
                byte[] cipherBytes = Convert.FromBase64String(value);
                if (cipherBytes.Length < 16)
                    return value; // Not valid, treat as plaintext

                byte[] iv = new byte[16];
                Array.Copy(cipherBytes, iv, 16);
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
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
            catch
            {
                return value; // Not valid Base64, treat as plaintext
            }
        }
    }
}