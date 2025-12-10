using Microsoft.Extensions.Configuration;
using System.IO;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static IConfigurationRoot configuration;

        static ConfigManager()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
        }

        public static string Get(string key)
        {
            return configuration[key];
        }
    }
}