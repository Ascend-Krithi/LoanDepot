using System;
using System.IO;

namespace AutomationFramework.Tests.Reporting
{
    public static class HtmlReportManager
    {
        public static void WriteSummary(string content)
        {
            var folder = Path.Combine(AppContext.BaseDirectory, "Reports");
            Directory.CreateDirectory(folder);
            var file = Path.Combine(folder, $"Report_{DateTime.UtcNow:yyyyMMdd_HHmmss}.html");
            File.WriteAllText(file, content);
        }
    }
}