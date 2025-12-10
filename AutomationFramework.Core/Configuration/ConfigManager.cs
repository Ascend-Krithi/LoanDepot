using Newtonsoft.Json.Linq;
using System.IO;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static JObject _config;

        static ConfigManager()
        {
            var encryptedJson = File.ReadAllText("AutomationFramework.Core/Configuration/appsettings.json");
            var decryptedJson = EncryptionService.Decrypt(encryptedJson);
            _config = JObject.Parse(decryptedJson);
        }

        public static string Get(string key)
        {
            return _config[key]?.ToString();
        }
    }
}