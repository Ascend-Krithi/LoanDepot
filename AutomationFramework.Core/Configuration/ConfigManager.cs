using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using AutomationFramework.Core.Encryption;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static readonly object _lock = new object();
        private static TestSettings _settings;

        public static TestSettings Settings
        {
            get
            {
                if (_settings == null)
                {
                    lock (_lock)
                    {
                        if (_settings == null)
                        {
                            LoadSettings();
                        }
                    }
                }
                return _settings;
            }
        }

        private static void LoadSettings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

            var env = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT");
            if (!string.IsNullOrWhiteSpace(env))
            {
                builder.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: false);
            }
            builder.AddEnvironmentVariables();

            var configuration = builder.Build();
            var settings = new TestSettings();
            configuration.Bind(settings);

            if (settings.Credentials != null && !string.IsNullOrWhiteSpace(settings.Credentials.Username) && !string.IsNullOrWhiteSpace(settings.Encryption?.Key))
            {
                settings.Credentials.Username = EncryptionManager.Decrypt(settings.Credentials.Username, settings.Encryption.Key);
            }
            if (settings.Credentials != null && !string.IsNullOrWhiteSpace(settings.Credentials.Password) && !string.IsNullOrWhiteSpace(settings.Encryption?.Key))
            {
                settings.Credentials.Password = EncryptionManager.Decrypt(settings.Credentials.Password, settings.Encryption.Key);
            }

            _settings = settings;
        }
    }
}