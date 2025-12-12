// AutomationFramework.Core/Utilities/Logger.cs
using System;
using System.IO;

namespace AutomationFramework.Core.Utilities
{
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine(AppContext.BaseDirectory, "automation.log");
        private static readonly object _lock = new object();

        public static void Log(string message)
        {
            try
            {
                lock (_lock)
                {
                    string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}";
                    File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
                }
            }
            catch (IOException)
            {
                // Swallow IO exceptions to prevent logging from crashing tests.
            }
        }
    }
}