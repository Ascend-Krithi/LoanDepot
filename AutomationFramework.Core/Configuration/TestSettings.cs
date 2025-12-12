// AutomationFramework.Core/Configuration/TestSettings.cs
namespace AutomationFramework.Core.Configuration
{
    public class TestSettings
    {
        public string Environment { get; set; } = "dev";
        public string BaseUrl { get; set; } = string.Empty;
        public string Browser { get; set; } = "Chrome";
        public string TestDataFolder { get; set; } = "TestData";
        public Credentials? Credentials { get; set; }
        public Encryption? Encryption { get; set; }
    }

    public class Credentials
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class Encryption
    {
        public string Key { get; set; } = string.Empty;
    }
}