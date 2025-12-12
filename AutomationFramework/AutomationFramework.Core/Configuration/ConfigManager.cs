using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static IConfigurationRoot configuration;
        private static TestSettings settings;
        public static TestSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    LoadConfiguration();
                }
                return settings;
            }
        }

        private static void LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var env = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT");
            if (!string.IsNullOrEmpty(env))
            {
                builder.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
            }

            builder.AddEnvironmentVariables();
            configuration = builder.Build();

            settings = new TestSettings();
            configuration.Bind(settings);

            if (settings.Credentials != null && settings.Encryption != null && 
                !string.IsNullOrEmpty(settings.Credentials.Username) && 
                !string.IsNullOrEmpty(settings.Encryption.Key))
            {
                settings.Credentials.Username = Encryption.EncryptionManager.Decrypt(settings.Credentials.Username, settings.Encryption.Key);
                settings.Credentials.Password = Encryption.EncryptionManager.Decrypt(settings.Credentials.Password, settings.Encryption.Key);
            }
        }
    }
}