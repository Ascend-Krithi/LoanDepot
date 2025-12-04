using System;

namespace AutomationFramework.Core.Encryption
{
    public class EncryptTool
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: EncryptTool <plaintext>");
                return;
            }
            var plain = args[0];
            var encrypted = EncryptionManager.Encrypt(plain);
            Console.WriteLine("Encrypted: " + encrypted);
        }
    }
}