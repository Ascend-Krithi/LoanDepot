using Microsoft.Extensions.Configuration;
using System.IO;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static readonly IConfigurationRoot _configuration;

        static ConfigManager()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public static TestSettings GetTestSettings()
        {
            var testSettings = new TestSettings();
            _configuration.GetSection("TestSettings").Bind(testSettings);
            return testSettings;
        }

        public static string GetConnectionString(string name)
        {
            return _configuration.GetConnectionString(name);
        }
    }
}