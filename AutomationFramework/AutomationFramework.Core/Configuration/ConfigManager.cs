using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static IConfigurationRoot _configuration;
        private static TestSettings _settings;
        private static readonly object _lock = new object();

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
                            LoadConfiguration();
                        }
                    }
                }
                return _settings;
            }
        }

        private static void LoadConfiguration()
        {
            var basePath = AppContext.BaseDirectory;
            var env = Environment.GetEnvironmentVariable("TEST_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            if (!string.IsNullOrWhiteSpace(env))
            {
                builder.AddJsonFile($"appsettings.{env}.json", optional: true, reloadOnChange: true);
            }

            builder.AddEnvironmentVariables();

            _configuration = builder.Build();
            _settings = new TestSettings();
            _configuration.Bind(_settings);

            if (_settings.Credentials != null && _settings.Encryption != null &&
                !string.IsNullOrWhiteSpace(_settings.Encryption.Key))
            {
                var key = _settings.Encryption.Key;
                if (!string.IsNullOrWhiteSpace(_settings.Credentials.Username))
                {
                    _settings.Credentials.Username = Encryption.EncryptionManager.Decrypt(_settings.Credentials.Username, key);
                }
                if (!string.IsNullOrWhiteSpace(_settings.Credentials.Password))
                {
                    _settings.Credentials.Password = Encryption.EncryptionManager.Decrypt(_settings.Credentials.Password, key);
                }
            }
        }
    }
}