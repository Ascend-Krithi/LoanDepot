using System;
using System.IO;
using System.Text;

namespace AutomationFramework.Tests.Reporting
{
    public static class HtmlReportGenerator
    {
        private static readonly string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestReport.html");
        private static readonly object _lock = new();

        public static void InitializeReport()
        {
            lock (_lock)
            {
                var sb = new StringBuilder();
                sb.AppendLine("<html><head><title>Automation Test Report</title></head><body>");
                sb.AppendLine("<h1>Automation Test Report</h1>");
                sb.AppendLine("<table border='1'><tr><th>Scenario</th><th>Status</th><th>Message</th></tr>");
                File.WriteAllText(reportPath, sb.ToString());
            }
        }

        public static void AddScenarioResult(string scenario, string status, string message)
        {
            lock (_lock)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"<tr><td>{scenario}</td><td>{status}</td><td>{message}</td></tr>");
                File.AppendAllText(reportPath, sb.ToString());
            }
        }

        public static void FinalizeReport()
        {
            lock (_lock)
            {
                File.AppendAllText(reportPath, "</table></body></html>");
            }
        }
    }
}