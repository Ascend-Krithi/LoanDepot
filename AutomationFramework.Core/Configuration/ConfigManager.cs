using System;
using System.IO;
using System.Text.Json;
using AutomationFramework.Core.Encryption;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        public static TestSettings Settings { get; private set; }

        static ConfigManager()
        {
            string baseDir = AppContext.BaseDirectory;
            string env = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT");
            string baseConfig = Path.Combine(baseDir, "appsettings.json");
            string envConfig = env != null ? Path.Combine(baseDir, $"appsettings.{env}.json") : null;

            if (!File.Exists(baseConfig))
                throw new FileNotFoundException($"Base configuration file not found: {baseConfig}");

            string json = File.ReadAllText(baseConfig);
            if (envConfig != null && File.Exists(envConfig))
            {
                string envJson = File.ReadAllText(envConfig);
                using var doc1 = JsonDocument.Parse(json);
                using var doc2 = JsonDocument.Parse(envJson);
                json = MergeJson(doc1, doc2);
            }

            Settings = JsonSerializer.Deserialize<TestSettings>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Decrypt credentials
            if (Settings?.Credentials != null && Settings.Encryption != null)
            {
                var key = Settings.Encryption.Key;
                Settings.Credentials.Username = EncryptionManager.Decrypt(Settings.Credentials.Username, key);
                Settings.Credentials.Password = EncryptionManager.Decrypt(Settings.Credentials.Password, key);
            }
        }

        // Simple JSON merge: env overrides base
        private static string MergeJson(JsonDocument baseDoc, JsonDocument envDoc)
        {
            var baseObj = baseDoc.RootElement.Clone();
            var envObj = envDoc.RootElement.Clone();
            using var ms = new MemoryStream();
            using var writer = new Utf8JsonWriter(ms);

            baseObj.WriteTo(writer);
            writer.Flush();
            ms.Position = 0;
            var merged = JsonDocument.Parse(ms).RootElement.Clone();

            foreach (var prop in envObj.EnumerateObject())
            {
                // Overwrite or add
                if (merged.TryGetProperty(prop.Name, out _))
                {
                    // Not deep merge, just override
                }
            }
            // For simplicity, env completely overrides base for matching properties
            var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(baseObj.GetRawText());
            var envDict = JsonSerializer.Deserialize<Dictionary<string, object>>(envObj.GetRawText());
            foreach (var kv in envDict)
                dict[kv.Key] = kv.Value;
            return JsonSerializer.Serialize(dict);
        }
    }
}