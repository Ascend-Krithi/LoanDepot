using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static readonly Lazy<TestSettings> _settings = new Lazy<TestSettings>(LoadSettings);
        public static TestSettings Settings => _settings.Value;

        private static TestSettings LoadSettings()
        {
            var basePath = AppContext.BaseDirectory;
            var env = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

            if (!string.IsNullOrWhiteSpace(env))
            {
                builder.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false);
            }

            builder.AddEnvironmentVariables();

            var config = builder.Build();
            var settings = new TestSettings();
            config.Bind(settings);

            // Decrypt credentials if needed
            if (settings.Credentials != null && settings.Encryption != null && !string.IsNullOrWhiteSpace(settings.Encryption.Key))
            {
                var key = settings.Encryption.Key;
                if (!string.IsNullOrWhiteSpace(settings.Credentials.Username))
                {
                    settings.Credentials.Username = Encryption.EncryptionManager.Decrypt(settings.Credentials.Username, key);
                }
                if (!string.IsNullOrWhiteSpace(settings.Credentials.Password))
                {
                    settings.Credentials.Password = Encryption.EncryptionManager.Decrypt(settings.Credentials.Password, key);
                }
            }

            return settings;
        }
    }
}