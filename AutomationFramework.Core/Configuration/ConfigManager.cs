// AutomationFramework.Core/Configuration/ConfigManager.cs
using AutomationFramework.Core.Encryption;
using Microsoft.Extensions.Configuration;
using System;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static readonly Lazy<TestSettings> _settings = new Lazy<TestSettings>(() =>
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("TEST_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var settings = new TestSettings();
            config.Bind(settings);

            DecryptCredentials(settings);

            return settings;
        });

        public static TestSettings Settings => _settings.Value;

        private static void DecryptCredentials(TestSettings settings)
        {
            if (settings.Credentials != null && !string.IsNullOrEmpty(settings.Encryption?.Key))
            {
                if (!string.IsNullOrEmpty(settings.Credentials.Username))
                {
                    settings.Credentials.Username = EncryptionManager.Decrypt(settings.Credentials.Username, settings.Encryption.Key);
                }

                if (!string.IsNullOrEmpty(settings.Credentials.Password))
                {
                    settings.Credentials.Password = EncryptionManager.Decrypt(settings.Credentials.Password, settings.Encryption.Key);
                }
            }
        }
    }
}