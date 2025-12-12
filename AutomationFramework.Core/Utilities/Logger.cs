using System;
using System.IO;

namespace AutomationFramework.Core.Utilities
{
    public static class Logger
    {
        private static readonly string LogFile = Path.Combine(AppContext.BaseDirectory, "automation.log");

        public static void Log(string message)
        {
            try
            {
                var logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} | {message}";
                File.AppendAllText(LogFile, logLine + Environment.NewLine);
            }
            catch
            {
                // Swallow all IO exceptions
            }
        }
    }
}