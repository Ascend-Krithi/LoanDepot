using System;

namespace AutomationFramework.Core.Encryption
{
    public class EncryptTool
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter plain text to encrypt:");
            var plainText = Console.ReadLine();
            var encrypted = EncryptionManager.Encrypt(plainText);
            Console.WriteLine("Encrypted text:");
            Console.WriteLine(encrypted);
        }
    }
}