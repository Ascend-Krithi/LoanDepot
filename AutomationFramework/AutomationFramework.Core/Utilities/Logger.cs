namespace AutomationFramework.Core.Utilities
{
    public static class Logger
    {
        private static string? _logFilePath;
        private static readonly object s_lock = new();

        public static void Initialize(string logDirectory = "Logs")
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            _logFilePath = Path.Combine(logDirectory, $"log_{DateTime.Now:yyyyMMdd_HHmmss}.txt");
        }

        public static void Log(string message)
        {
            if (_logFilePath == null)
            {
                Initialize();
            }

            lock (s_lock)
            {
                File.AppendAllText(_logFilePath!, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}{Environment.NewLine}");
            }
        }
    }
}