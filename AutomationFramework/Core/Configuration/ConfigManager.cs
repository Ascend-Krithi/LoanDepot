using System;
using System.Collections.Generic;

namespace AutomationFramework.Core.Configuration
{
    public static class ConfigManager
    {
        public static string Environment => EnvironmentVariableOrDefault("ENVIRONMENT", "QA1");
        public static string BaseUrl => EnvironmentVariableOrDefault("BASE_URL", "https://servicing-qa1.loandepotdev.works/dashboard");

        public static string Get(string key, string defaultValue = "")
        {
            var envValue = Environment.GetEnvironmentVariable(key);
            return string.IsNullOrWhiteSpace(envValue) ? defaultValue : envValue;
        }

        private static string EnvironmentVariableOrDefault(string key, string defaultValue)
        {
            var value = Environment.GetEnvironmentVariable(key);
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }
    }
}
