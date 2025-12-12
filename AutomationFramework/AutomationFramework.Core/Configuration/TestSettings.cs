namespace AutomationFramework.Core.Configuration
{
    public class TestSettings
    {
        public string Browser { get; set; }
        public string BaseUrl { get; set; }
        public int DefaultTimeout { get; set; }
        public string EncryptedPassword { get; set; }
        public string ExcelDataPath { get; set; }
        public string ReportPath { get; set; }
        public string Environment { get; set; }
        public bool Headless { get; set; }
        public string Language { get; set; }
    }
}