using System;
using System.IO;
using System.Text.Json;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static readonly string configFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testsettings.json");
        private static TestSettings _settings;

        public static TestSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    LoadSettings();
                }
                return _settings;
            }
        }

        public static void LoadSettings()
        {
            if (!File.Exists(configFile))
                throw new FileNotFoundException($"Configuration file not found: {configFile}");

            var json = File.ReadAllText(configFile);
            _settings = JsonSerializer.Deserialize<TestSettings>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}