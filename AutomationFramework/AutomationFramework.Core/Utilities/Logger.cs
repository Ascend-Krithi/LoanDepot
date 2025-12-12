using System;
using System.IO;

namespace AutomationFramework.Core.Utilities
{
    public static class Logger
    {
        private static readonly string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "automation.log");
        private static readonly object _lock = new();

        public static void Log(string message)
        {
            lock (_lock)
            {
                File.AppendAllText(logFile, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} {message}{Environment.NewLine}");
            }
        }

        public static void LogError(string message, Exception ex)
        {
            Log($"ERROR: {message} - {ex}");
        }
    }
}