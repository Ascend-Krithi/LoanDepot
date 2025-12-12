using AutomationFramework.Core.Encryption;
using Microsoft.Extensions.Configuration;
using System;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static readonly Lazy<TestSettings> _settings = new(LoadSettings);
        public static TestSettings Settings => _settings.Value;

        private static TestSettings LoadSettings()
        {
            var environment = System.Environment.GetEnvironmentVariable("TEST_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var settings = new TestSettings();
            configuration.Bind(settings);

            DecryptCredentials(settings);

            return settings;
        }

        private static void DecryptCredentials(TestSettings settings)
        {
            if (settings.Credentials == null || string.IsNullOrEmpty(settings.Encryption?.Key))
            {
                return;
            }

            var encryptionKey = settings.Encryption.Key;

            if (!string.IsNullOrEmpty(settings.Credentials.Username))
            {
                settings.Credentials.Username = EncryptionManager.Decrypt(settings.Credentials.Username, encryptionKey);
            }

            if (!string.IsNullOrEmpty(settings.Credentials.Password))
            {
                settings.Credentials.Password = EncryptionManager.Decrypt(settings.Credentials.Password, encryptionKey);
            }
        }
    }
}