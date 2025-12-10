using System.IO;
using Microsoft.Extensions.Configuration;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static IConfigurationRoot _config;

        static ConfigManager()
        {
            _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static string Get(string key) => _config[key];
    }
}