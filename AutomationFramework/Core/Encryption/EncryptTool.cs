using System;

namespace AutomationFramework.Core.Encryption
{
    public static class EncryptTool
    {
        // Stub helper to read secrets from env and decrypt if needed
        public static string GetSecret(string envVar, string defaultValue = "")
        {
            var val = Environment.GetEnvironmentVariable(envVar);
            return string.IsNullOrWhiteSpace(val) ? defaultValue : val;
        }
    }
}
