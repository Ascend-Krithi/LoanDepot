using System;
using AutomationFramework.Core.Encryption;

namespace AutomationFramework.Core.Encryption
{
    public static class EncryptTool
    {
        // Utility for local generation of encrypted secrets in CI if needed
        public static void MainEncrypt(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: EncryptTool <secret> <key>");
                return;
            }
            var enc = EncryptionManager.Encrypt(args[0], args[1]);
            Console.WriteLine(enc);
        }
    }
}