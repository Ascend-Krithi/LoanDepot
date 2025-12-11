using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Encryption
{
    public static class EncryptionManager
    {
        private static readonly string SecretKey = Environment.GetEnvironmentVariable("AUTOMATION_SECRET_KEY");

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(SecretKey))
                throw new Exception("Secret key not set in environment variable AUTOMATION_SECRET_KEY.");

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(SecretKey);
                aesAlg.GenerateIV();
                var iv = aesAlg.IV;

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv))
                using (var msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(iv, 0, iv.Length);
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(SecretKey))
                throw new Exception("Secret key not set in environment variable AUTOMATION_SECRET_KEY.");

            var fullCipher = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(SecretKey);
                var iv = new byte[16];
                Array.Copy(fullCipher, 0, iv, 0, iv.Length);
                aesAlg.IV = iv;

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                using (var msDecrypt = new MemoryStream(fullCipher, 16, fullCipher.Length - 16))
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (var srDecrypt = new StreamReader(csDecrypt))
                {
                    return srDecrypt.ReadToEnd();
                }
            }
        }
    }
}