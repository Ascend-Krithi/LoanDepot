namespace AutomationFramework.Core.Configuration
{
    public class TestSettings
    {
        public string Browser { get; set; } = "Chrome";
        public string BaseUrl { get; set; } = string.Empty;
        public int DefaultWaitTimeout { get; set; } = 10;
        public string TestDataFolder { get; set; } = "TestData";
        public Dictionary<string, Credentials> Credentials { get; set; } = new();
        public EncryptionSettings Encryption { get; set; } = new();
    }

    public class Credentials
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class EncryptionSettings
    {
        public string Key { get; set; } = string.Empty;
    }
}