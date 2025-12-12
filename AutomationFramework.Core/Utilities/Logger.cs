using System;
using System.IO;

namespace AutomationFramework.Core.Utilities
{
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine(AppContext.BaseDirectory, "automation.log");
        private static readonly object _lock = new();

        public static void Log(string message)
        {
            try
            {
                lock (_lock)
                {
                    var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}{System.Environment.NewLine}";
                    File.AppendAllText(LogFilePath, logMessage);
                }
            }
            catch (IOException)
            {
                // Swallow IO exceptions to prevent logging from crashing tests
            }
        }
    }
}