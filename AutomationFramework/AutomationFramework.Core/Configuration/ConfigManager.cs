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

            // Decrypt credentials if necessary
            if (settings.Credentials != null && settings.Encryption != null &&
                !string.IsNullOrWhiteSpace(settings.Credentials.Username) &&
                !string.IsNullOrWhiteSpace(settings.Credentials.Password) &&
                !string.IsNullOrWhiteSpace(settings.Encryption.Key))
            {
                settings.Credentials.Username = Encryption.EncryptionManager.Decrypt(settings.Credentials.Username, settings.Encryption.Key);
                settings.Credentials.Password = Encryption.EncryptionManager.Decrypt(settings.Credentials.Password, settings.Encryption.Key);
            }

            return settings;
        }
    }
}