using Newtonsoft.Json;
using System.IO;

namespace AutomationFramework.Core.Configuration
{
    public class ConfigManager
    {
        private static dynamic _config;
        static ConfigManager()
        {
            var configText = File.ReadAllText("AutomationFramework.Core/Configuration/appsettings.json");
            _config = JsonConvert.DeserializeObject(configText);
        }

        public static string GetBrowser() => _config["browser"];
        public static string GetEncryptedUsername() => _config["usernameEnc"];
        public static string GetEncryptedPassword() => _config["passwordEnc"];
        public static string GetBaseUrl() => _config["baseUrl"];
        public static string GetEncryptionKey() => _config["encryptionKey"];
        public static string GetEncryptionIV() => _config["encryptionIV"];
    }
}