using System;
using System.IO;
using Newtonsoft.Json;

namespace AutomationFramework.Core.Configuration
{
    public class Config
    {
        public string BaseUrl { get; set; }
        public string Browser { get; set; }
        public string EncryptedUsername { get; set; }
        public string EncryptedPassword { get; set; }
    }

    public static class ConfigManager
    {
        private static Config _config;
        public static Config Settings
        {
            get
            {
                if (_config == null)
                {
                    var json = File.ReadAllText("config.json");
                    _config = JsonConvert.DeserializeObject<Config>(json);
                }
                return _config;
            }
        }
    }
}