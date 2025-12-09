using System;
using System.IO;
using System.Text.Json;

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
        public static Config Settings => _config ??= LoadConfig();

        private static Config LoadConfig()
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
            var json = File.ReadAllText(configPath);
            return JsonSerializer.Deserialize<Config>(json);
        }
    }
}