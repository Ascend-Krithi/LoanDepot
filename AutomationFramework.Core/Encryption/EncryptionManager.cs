// NuGet Packages: None (uses System.Security.Cryptography)
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Encryption
{
    /// <summary>
    /// Provides AES-256 encryption and decryption functionalities.
    /// The IV is prepended to the ciphertext for self-contained decryption.
    /// </summary>
    public static class EncryptionManager
    {
        private const int KeySize = 256;
        private const int BlockSize = 128;

        /// <summary>
        /// Encrypts a plain text string using a provided key.
        /// The format of the output is Base64(IV):Base64(CipherText).
        /// </summary>
        /// <param name="plainText">The text to encrypt.</param>
        /// <param name="key">The Base64 encoded encryption key (must be 256-bit).</param>
        /// <returns>An encrypted string containing both the IV and the ciphertext.</returns>
        public static string Encrypt(string plainText, string key)
        {
            if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException(nameof(plainText));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            byte[] keyBytes = Convert.FromBase64String(key);

            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Key = keyBytes;
                aes.GenerateIV(); // Generate a new IV for each encryption

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    byte[] encryptedBytes = msEncrypt.ToArray();
                    // Prepend IV to the ciphertext
                    return $"{Convert.ToBase64String(aes.IV)}:{Convert.ToBase64String(encryptedBytes)}";
                }
            }
        }

        /// <summary>
        /// Decrypts a string that was encrypted with the Encrypt method.
        /// It expects the format Base64(IV):Base64(CipherText).
        /// </summary>
        /// <param name="cipherText">The encrypted text.</param>
        /// <param name="key">The Base64 encoded encryption key (must be 256-bit).</param>
        /// <returns>The decrypted plain text string.</returns>
        public static string Decrypt(string cipherText, string key)
        {
            if (string.IsNullOrEmpty(cipherText)) throw new ArgumentNullException(nameof(cipherText));
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            string[] parts = cipherText.Split(':');
            if (parts.Length != 2) throw new FormatException("Cipher text is not in the expected format 'IV:CipherText'.");

            byte[] iv = Convert.FromBase64String(parts[0]);
            byte[] cipherBytes = Convert.FromBase64String(parts[1]);
            byte[] keyBytes = Convert.FromBase64String(key);

            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = BlockSize;
                aes.Key = keyBytes;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var msDecrypt = new MemoryStream(cipherBytes))
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

        /// <summary>
        /// Generates a new 256-bit AES key and returns it as a Base64 encoded string.
        /// </summary>
        /// <returns>A Base64 encoded key.</returns>
        public static string GenerateNewKey()
        {
            using (var aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.GenerateKey();
                return Convert.ToBase64String(aes.Key);
            }
        }
    }
}