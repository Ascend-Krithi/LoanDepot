namespace AutomationFramework.Core.Configuration
{
    public class TestSettings
    {
        public string Environment { get; set; }
        public string BaseUrl { get; set; }
        public string Browser { get; set; }
        public string TestDataFolder { get; set; }
        public Credentials Credentials { get; set; }
        public Encryption Encryption { get; set; }

        public class Credentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class Encryption
        {
            public string Key { get; set; }
        }
    }
}