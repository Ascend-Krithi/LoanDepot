using System.Text.Json;

namespace AutomationFramework.Core
{
    public class ConfigManager
    {
        private static readonly Lazy<ConfigManager> _instance = new(() => new ConfigManager());
        public static ConfigManager Instance => _instance.Value;

        private readonly string _configPath;
        private readonly JsonDocument _doc;

        private ConfigManager()
        {
            _configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(_configPath))
                throw new FileNotFoundException("Missing appsettings.json", _configPath);

            using var fs = File.OpenRead(_configPath);
            _doc = JsonDocument.Parse(fs);
        }

        private JsonElement GetSection(string path)
        {
            var parts = path.Split(':', StringSplitOptions.RemoveEmptyEntries);
            JsonElement current = _doc.RootElement;
            foreach (var part in parts)
            {
                if (!current.TryGetProperty(part, out current))
                    throw new KeyNotFoundException($"Configuration key not found: {path}");
            }
            return current;
        }

        public string GetString(string path)
        {
            var section = GetSection(path);
            return section.GetString() ?? string.Empty;
        }

        public int GetInt(string path)
        {
            var section = GetSection(path);
            return section.GetInt32();
        }

        public bool GetBool(string path)
        {
            var section = GetSection(path);
            return section.GetBoolean();
        }

        public string BaseUrl => GetString("Environment:BaseUrl");
        public string DriverType => GetString("Driver:Type");
        public bool Headless => GetBool("Driver:Headless");
        public int ImplicitWaitSeconds => GetInt("Driver:ImplicitWaitSeconds");
        public string TestDataPath => GetString("Data:TestDataPath");
        public string ScreenFlowPath => GetString("Data:ScreenFlowPath");
        public bool CloseBrowserAfterTest => GetBool("Run:CloseBrowserAfterTest");
    }
}