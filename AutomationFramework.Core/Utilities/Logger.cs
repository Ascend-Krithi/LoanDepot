// NuGet Packages: None
using System;

namespace AutomationFramework.Core.Utilities
{
    /// <summary>
    /// A simple static logger that writes messages to the console with a timestamp.
    /// In a real enterprise framework, this would be replaced by a more robust logging library like Serilog or NLog.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Logs an informational message to the console.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public static void Log(string message)
        {
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] INFO: {message}");
        }

        /// <summary>
        /// Logs an error message to the console.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="exception">Optional exception to include in the log.</param>
        public static void Error(string message, Exception exception = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ERROR: {message}");
            if (exception != null)
            {
                Console.WriteLine($"Exception: {exception.Message}");
                Console.WriteLine($"StackTrace: {exception.StackTrace}");
            }
            Console.ResetColor();
        }
    }
}