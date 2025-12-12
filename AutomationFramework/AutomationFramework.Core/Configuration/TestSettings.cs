namespace AutomationFramework.Core.Configuration
{
    public class TestSettings
    {
        public string Browser { get; set; } = "Chrome";
        public string BaseUrl { get; set; } = "http://localhost";
        public int DefaultWaitTimeout { get; set; } = 30;
        public bool Headless { get; set; } = false;
        public string SelfHealingRepositoryPath { get; set; } = "selfHealingRepository.json";
        public bool EnableSelfHealing { get; set; } = true;
        public string EncryptionKey { get; set; } = "AAECAwQFBgcICQoLDA0ODw=="; // Replace with a secure key
        public string EncryptionIV { get; set; } = "EBAYGBodHiEjJA=="; // Replace with a secure IV
    }
}