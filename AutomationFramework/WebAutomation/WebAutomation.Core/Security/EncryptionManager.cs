using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WebAutomation.Core.Security;

/// <summary>
/// Provides AES-256 encryption and decryption functionality.
/// Assumes the encrypted string is a Base64 representation of:
/// [16-byte IV] + [Ciphertext]
/// </summary>
public static class EncryptionManager
{
    // IMPORTANT: This key MUST be kept secret and secure.
    // It should be loaded from a secure source like Azure Key Vault, not hardcoded.
    // For this generic framework, a placeholder is used.
    // It MUST be 32 bytes (256 bits) for AES-256.
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("p2s5v8y/B?E(H+MbQeThWmZq4t7w9z$C");

    /// <summary>
    /// Encrypts a plaintext string using AES-256.
    /// The resulting string is Base64 encoded and contains the IV prepended to the ciphertext.
    /// </summary>
    /// <param name="plainText">The string to encrypt.</param>
    /// <returns>A Base64 encoded encrypted string.</returns>
    public static string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
            return string.Empty;

        using var aes = Aes.Create();
        aes.Key = Key;
        aes.GenerateIV(); // Generate a new IV for each encryption
        var iv = aes.IV;

        using var encryptor = aes.CreateEncryptor(aes.Key, iv);
        using var memoryStream = new MemoryStream();
        
        // Prepend the IV to the stream
        memoryStream.Write(iv, 0, iv.Length);

        using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
        {
            using var streamWriter = new StreamWriter(cryptoStream);
            streamWriter.Write(plainText);
        }

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    /// <summary>
    /// Decrypts a Base64 encoded string that was encrypted with the Encrypt method.
    /// </summary>
    /// <param name="cipherText">The Base64 encoded string (IV + ciphertext).</param>
    /// <returns>The decrypted plaintext string.</returns>
    public static string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText))
            return string.Empty;

        // This is a placeholder check. If the value is not Base64, it's likely not encrypted.
        // A more robust solution might check for a specific prefix or format.
        if (!IsBase64String(cipherText))
        {
            return cipherText;
        }

        byte[] fullCipher = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = Key;

        // Extract the IV from the beginning of the cipher bytes
        var iv = new byte[aes.BlockSize / 8];
        Array.Copy(fullCipher, 0, iv, 0, iv.Length);
        aes.IV = iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream(fullCipher, iv.Length, fullCipher.Length - iv.Length);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        
        return streamReader.ReadToEnd();
    }

    private static bool IsBase64String(string s)
    {
        if (string.IsNullOrWhiteSpace(s) || s.Length % 4 != 0 || s.Contains(" ") || s.Contains("\t") || s.Contains("\r") || s.Contains("\n"))
            return false;
        try
        {
            Convert.FromBase64String(s);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}