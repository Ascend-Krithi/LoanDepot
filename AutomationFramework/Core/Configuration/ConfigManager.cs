using System.IO;
using Microsoft.Extensions.Configuration;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        private static IConfigurationRoot configuration;

        static ConfigManager()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static string Get(string key) => configuration[key];
    }
}