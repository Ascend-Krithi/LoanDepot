using System.IO;
using Newtonsoft.Json.Linq;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static JObject _config;

        static ConfigManager()
        {
            var configText = File.ReadAllText("appsettings.json");
            _config = JObject.Parse(configText);
        }

        public static string Get(string key)
        {
            return _config[key]?.ToString();
        }
    }
}