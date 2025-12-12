using System.Security.Cryptography;
using System.Text;

namespace AutomationFramework.Core.Encryption
{
    public static class EncryptionManager
    {
        private static byte[]? _key;
        private const int IvSize = 16; // 128 bits for AES
        private const int KeySize = 32; // 256 bits for AES
        private const string EncryptedPrefix = "ENCRYPTED:";

        public static void Initialize(string base64Key)
        {
            if (string.IsNullOrWhiteSpace(base64Key))
            {
                throw new ArgumentException("Encryption key cannot be null or empty.", nameof(base64Key));
            }
            _key = Convert.FromBase64String(base64Key);
            if (_key.Length != KeySize)
            {
                throw new ArgumentException($"Encryption key must be {KeySize} bytes long.", nameof(base64Key));
            }
        }

        public static string Encrypt(string plainText)
        {
            if (_key == null) throw new InvalidOperationException("EncryptionManager is not initialized.");

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();
            var iv = aes.IV;

            using var encryptor = aes.CreateEncryptor(aes.Key, iv);
            using var ms = new MemoryStream();
            
            // Prepend IV to the stream
            ms.Write(iv, 0, iv.Length);
            
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using var sw = new StreamWriter(cs);
                sw.Write(plainText);
            }
            
            var encryptedBytes = ms.ToArray();
            return EncryptedPrefix + Convert.ToBase64String(encryptedBytes);
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrWhiteSpace(cipherText) || !cipherText.StartsWith(EncryptedPrefix))
            {
                // Return as-is if not encrypted or empty
                return cipherText;
            }
            
            if (_key == null) throw new InvalidOperationException("EncryptionManager is not initialized. Cannot decrypt value.");

            var fullCipher = Convert.FromBase64String(cipherText.Substring(EncryptedPrefix.Length));

            using var aes = Aes.Create();
            aes.Key = _key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            // Extract IV from the beginning of the cipher text
            var iv = new byte[IvSize];
            Array.Copy(fullCipher, 0, iv, 0, iv.Length);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            
            // Write the actual ciphertext (without IV) to the memory stream
            ms.Write(fullCipher, IvSize, fullCipher.Length - IvSize);
            ms.Position = 0; // Reset stream position for reading

            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            
            return sr.ReadToEnd();
        }
    }
}