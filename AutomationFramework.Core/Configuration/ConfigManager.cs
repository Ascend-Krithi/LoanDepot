// NuGet Packages: Microsoft.Extensions.Configuration, Microsoft.Extensions.Configuration.Json, Microsoft.Extensions.Configuration.Binder
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AutomationFramework.Core.Configuration
{
    /// <summary>
    /// Manages the application's configuration settings.
    /// It reads settings from 'appsettings.json' and provides them via a strongly-typed TestSettings object.
    /// </summary>
    public static class ConfigManager
    {
        private static readonly TestSettings _testSettings;

        static ConfigManager()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _testSettings = new TestSettings();
            configuration.Bind(_testSettings);
        }

        /// <summary>
        /// Gets the singleton instance of the TestSettings.
        /// </summary>
        public static TestSettings Settings => _testSettings;
    }
}