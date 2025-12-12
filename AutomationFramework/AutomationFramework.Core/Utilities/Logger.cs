using System;
using System.IO;

namespace AutomationFramework.Core.Utilities
{
    public static class Logger
    {
        private static readonly string logPath = Path.Combine(AppContext.BaseDirectory, "automation.log");

        public static void Log(string message)
        {
            try
            {
                var logMsg = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}{Environment.NewLine}";
                File.AppendAllText(logPath, logMsg);
            }
            catch { }
        }
    }
}