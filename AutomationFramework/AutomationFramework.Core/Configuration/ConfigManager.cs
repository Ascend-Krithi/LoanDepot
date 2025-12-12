using AutomationFramework.Core.Encryption;
using Microsoft.Extensions.Configuration;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static readonly Lazy<IConfiguration> s_configuration = new(BuildConfiguration);
        private static readonly Lazy<TestSettings> s_testSettings = new(BindTestSettings);

        public static IConfiguration Configuration => s_configuration.Value;
        public static TestSettings TestSettings => s_testSettings.Value;

        private static IConfiguration BuildConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            return config;
        }

        private static TestSettings BindTestSettings()
        {
            var settings = new TestSettings();
            Configuration.GetSection("TestSettings").Bind(settings);

            // Initialize EncryptionManager with the key from settings
            if (!string.IsNullOrEmpty(settings.Encryption?.Key))
            {
                EncryptionManager.Initialize(settings.Encryption.Key);
            }

            // Decrypt credentials
            foreach (var user in settings.Credentials.Values)
            {
                user.Password = EncryptionManager.Decrypt(user.Password);
            }

            return settings;
        }
    }
}