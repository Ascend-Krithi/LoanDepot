using System;

namespace AutomationFramework.Utilities
{
    public static class Logger
    {
        public static void Info(string message)
        {
            Console.WriteLine($"[INFO] {DateTime.Now}: {message}");
        }

        public static void Error(string message, Exception ex = null)
        {
            Console.WriteLine($"[ERROR] {DateTime.Now}: {message}");
            if (ex != null)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}