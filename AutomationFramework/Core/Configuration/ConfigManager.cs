using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static readonly Dictionary<string, string> _config = new();
        private static bool _initialized = false;

        public static void Initialize()
        {
            if (_initialized) return;
            // Defaults
            _config["BaseUrl"] = "https://servicing-qa1.loandepotdev.works/dashboard";
            _config["Environment"] = "QA1";
            _config["Browser"] = "Chrome";
            _config["Headless"] = "false";
            _config["DefaultWaitSeconds"] = "20";

            // Load from appsettings.json if present
            try
            {
                var file = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                if (File.Exists(file))
                {
                    var json = File.ReadAllText(file);
                    var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                    if (dict != null)
                    {
                        foreach (var kv in dict) _config[kv.Key] = kv.Value;
                    }
                }
            }
            catch { /* Ignore config load errors */ }
            _initialized = true;
        }

        public static string Get(string key, string defaultValue = "")
        {
            Initialize();
            if (_config.TryGetValue(key, out var val)) return val;
            return defaultValue;
        }

        public static void Set(string key, string value)
        {
            Initialize();
            _config[key] = value;
        }
    }
}