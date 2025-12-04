using System;

namespace AutomationFramework.Core.Encryption
{
    public class EncryptTool
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter text to encrypt:");
            var plainText = Console.ReadLine();
            var key = Environment.GetEnvironmentVariable("ENCRYPTION_KEY");
            if (string.IsNullOrEmpty(key) || key.Length != 32)
            {
                Console.WriteLine("Set ENCRYPTION_KEY environment variable to a 32-char string.");
                return;
            }
            var encrypted = EncryptionManager.Encrypt(plainText, key);
            Console.WriteLine($"Encrypted: {encrypted}");
        }
    }
}