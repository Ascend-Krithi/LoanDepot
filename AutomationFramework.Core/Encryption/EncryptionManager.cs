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
                return input;

            // If not base64, treat as plaintext
            try
            {
                var cipherBytes = Convert.FromBase64String(input);
                var key = Convert.FromBase64String(base64Key);

                using var ms = new MemoryStream(cipherBytes);
                byte[] iv = new byte[16];
                ms.Read(iv, 0, 16);
                using var aes = Aes.Create();
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
                using var sr = new StreamReader(cs, Encoding.UTF8);
                return sr.ReadToEnd();
            }
            catch
            {
                // Not base64 or decrypt failed, treat as plaintext
                return input;
            }
        }
    }
}