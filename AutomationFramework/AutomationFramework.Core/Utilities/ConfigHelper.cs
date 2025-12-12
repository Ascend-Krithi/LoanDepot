using System.Configuration;

namespace AutomationFramework.Core.Utilities
{
    public static class ConfigHelper
    {
        public static string Get(string key)
        {
            // Implement configuration retrieval logic here
            // For demo, return a placeholder URL
            if (key == "AppUrl")
                return "https://your-app-url.com";
            return string.Empty;
        }
    }
}