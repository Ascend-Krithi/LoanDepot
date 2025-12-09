using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;

namespace AutomationFramework.Core.Configuration
{
    public class Config
    {
        public string BaseUrl { get; set; }
        public string Browser { get; set; }
        public string EncryptedUsername { get; set; }
        public string EncryptedPassword { get; set; }
        public string TestEnvironment { get; set; }
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
                    LoadConfig();
                }
                return _config;
            }
        }

        private static void LoadConfig()
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Core", "Configuration", "appsettings.json");
            var json = File.ReadAllText(configPath);
            _config = JsonSerializer.Deserialize<Config>(json);
        }
    }
}