using System;
using AutomationFramework.Core.Encryption;

namespace AutomationFramework.Core.Encryption
{
    public class EncryptTool
    {
        public static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage: EncryptTool <text-to-encrypt>");
                return;
            }
            string plainText = args[0];
            string encrypted = EncryptionManager.Encrypt(plainText);
            Console.WriteLine("Encrypted: " + encrypted);
        }
    }
}