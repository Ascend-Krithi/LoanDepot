using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Encryption
{
    public class EncryptionManager
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptionManager(string base64Key, string base64Iv)
        {
            if (string.IsNullOrEmpty(base64Key) || string.IsNullOrEmpty(base64Iv))
            {
                throw new ArgumentException("Key and IV must be provided.");
            }
            _key = Convert.FromBase64String(base64Key);
            _iv = Convert.FromBase64String(base64Iv);

            if (_key.Length != 16 && _key.Length != 24 && _key.Length != 32)
            {
                throw new ArgumentException("Invalid key size. Key must be 128, 192, or 256 bits.");
            }
            if (_iv.Length != 16)
            {
                throw new ArgumentException("Invalid IV size. IV must be 128 bits.");
            }
        }

        public string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        byte[] encrypted = msEncrypt.ToArray();
                        // Prepend IV to the ciphertext for use in decryption
                        byte[] result = new byte[_iv.Length + encrypted.Length];
                        Buffer.BlockCopy(_iv, 0, result, 0, _iv.Length);
                        Buffer.BlockCopy(encrypted, 0, result, _iv.Length, encrypted.Length);
                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;

            byte[] fullCipher = Convert.FromBase64String(cipherText);

            byte[] iv = new byte[16];
            byte[] cipher = new byte[fullCipher.Length - 16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);

            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var msDecrypt = new MemoryStream(cipher))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}