using System;
using System.Text;
using System.IO;
using AventStack.ExtentReports;
using System.Security.Cryptography;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Utilities;
using log4net;
using iTextSharp.text.pdf.qrcode;

namespace LD_AutomationFramework.Base
{
    public class EncryptionManager
    {
        ExtentTest test { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string keyBase64 { get; set; }
        public static string vectorBase64 { get; set; }

        public EncryptionManager(ExtentTest test)
        {
            this.test = test;            
        }

        #region Base64Encryption

        /// <summary>
        /// Method to decode text from base64 encoding
        /// </summary>
        /// <param name="base64EncodedText"></param>
        /// <returns></returns>
        public string ToBase64Decode(string base64EncodedText)
        {
            byte[] base64EncodedBytes = null;
            try
            {
                if (!string.IsNullOrEmpty(base64EncodedText))
                {
                    base64EncodedBytes = Convert.FromBase64String(base64EncodedText);
                }
            }
            catch (Exception e)
            {
                log.Error("The text provided for decoding is empty." + e.Message);                
            }
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Method to decode text 
        /// </summary>
        /// <param name="encodedText">Text to be decoded</param>
        /// <returns></returns>
        public string DecodeText(string encodedText)
        {
            string result = string.Empty;
            try
            {
                UTF8Encoding encoder = new UTF8Encoding();
                Decoder utf8Decode = encoder.GetDecoder();
                byte[] toDecode_Byte = Convert.FromBase64String(encodedText);
                int charCount = utf8Decode.GetCharCount(toDecode_Byte, 0, toDecode_Byte.Length);
                char[] decoded_Char = new char[charCount];
                utf8Decode.GetChars(toDecode_Byte, 0, toDecode_Byte.Length, decoded_Char, 0);
                result = new string(decoded_Char);
            }
            catch (Exception e)
            {
                log.Error("Error in decoding the text - " + e.Message);
            }
            return result;
        }

        /// <summary>
        /// Method to encode text - https://www.base64encode.org/
        /// </summary>
        /// <param name="encodedText">Text to be encoded</param>
        /// <returns></returns>
        public string EncodeText(string text)
        {
            string result = string.Empty;
            try
            {
                byte[] toEncode_Byte = new byte[text.Length];
                toEncode_Byte = Encoding.UTF8.GetBytes(text);
                result = Convert.ToBase64String(toEncode_Byte);
            }
            catch (Exception e)
            {
                log.Error("Error in encoding the text - " + e.Message);
            }
            return result;
        }

        #endregion Base64Encryption

        #region AESEncryption

        /// <summary>
        /// Method to generate encryption key and initialization vector
        /// </summary>
        public void GenerateKeyAndVector()
        {
            try
            {
                using (Aes aesAlgorithm = Aes.Create())
                {
                    aesAlgorithm.KeySize = 256;
                    aesAlgorithm.GenerateKey();
                    keyBase64 = Convert.ToBase64String(aesAlgorithm.Key);
                    aesAlgorithm.GenerateIV();
                    vectorBase64 = Convert.ToBase64String(aesAlgorithm.IV);
                }
            }
            catch (Exception e)
            {
                log.Error("Error in generating key and IV - " + e.Message);
            }
        }

        /// <summary>
        /// Method to AES encrypt any text using Key and IV
        /// </summary>
        /// <param name="plainText">Any text</param>
        /// <returns>Encrypted text in base64 format</returns>
        public string EncryptDataWithAes(string plainText)
        {
            byte[] encryptedData = null;
            try
            {
                using (Aes aesAlgorithm = Aes.Create())
                {
                    aesAlgorithm.Key = Convert.FromBase64String(Constants.EncryptionKey.EncryptKey);
                    aesAlgorithm.IV = Convert.FromBase64String(Constants.EncryptionKey.InitializationVector);

                    // Create encryptor object
                    ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();                    

                    //Encryption
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(plainText);
                            }
                            encryptedData = ms.ToArray();
                        }
                    }                    
                }
            }
            catch (Exception e)
            {
                log.Error("Error in encrypting the text - " + e.Message);
            }
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Method to AES decrypt the encrypted text using same Key and IV
        /// </summary>
        /// <param name="cipherText">Encrypted text</param>
        /// <returns>Decrypted text</returns>
        public string DecryptDataWithAes(string cipherText)
        {
            string decryptedText = string.Empty;
            try
            {
                using (Aes aesAlgorithm = Aes.Create())
                {
                    aesAlgorithm.Key = Convert.FromBase64String(Constants.EncryptionKey.EncryptKey);
                    aesAlgorithm.IV = Convert.FromBase64String(Constants.EncryptionKey.InitializationVector);

                    // Create decryptor object
                    ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();
                    byte[] cipher = Convert.FromBase64String(cipherText);

                    //Decryption
                    using (MemoryStream ms = new MemoryStream(cipher))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                decryptedText = sr.ReadToEnd();
                            }
                        }
                    }
                }                
            }
            catch (Exception e)
            {
                log.Error("Error in encrypting the text - " + e.Message);
            }
            return decryptedText;
        }

        #endregion AESEncryption
    }
}
