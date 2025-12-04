using System;
using System.IO;
using System.Text;

namespace Tests.Reporting
{
    public static class HtmlReportManager
    {
        public static void GenerateReport(string scenario, string steps, bool passed, string error, string screenshotPath, string environment, string browser, TimeSpan duration)
        {
            var html = new StringBuilder();
            html.AppendLine("<html><head><title>Test Report</title></head><body>");
            html.AppendLine($"<h2>Scenario: {scenario}</h2>");
            html.AppendLine($"<b>Status:</b> {(passed ? "<span style='color:green'>PASS</span>" : "<span style='color:red'>FAIL</span>")}<br>");
            html.AppendLine($"<b>Duration:</b> {duration.TotalSeconds} seconds<br>");
            html.AppendLine($"<b>Environment:</b> {environment}<br>");
            html.AppendLine($"<b>Browser:</b> {browser}<br>");
            html.AppendLine("<h3>Steps</h3>");
            html.AppendLine($"<pre>{steps}</pre>");
            if (!passed)
            {
                html.AppendLine("<h3>Error</h3>");
                html.AppendLine($"<pre>{error}</pre>");
            }
            if (!string.IsNullOrEmpty(screenshotPath) && File.Exists(screenshotPath))
            {
                var base64 = Convert.ToBase64String(File.ReadAllBytes(screenshotPath));
                html.AppendLine("<h3>Screenshot</h3>");
                html.AppendLine($"<img src='data:image/png;base64,{base64}' width='600'/>");
            }
            html.AppendLine("</body></html>");
            var reportDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            Directory.CreateDirectory(reportDir);
            var reportPath = Path.Combine(reportDir, $"{scenario}_{DateTime.Now:yyyyMMdd_HHmmss}.html");
            File.WriteAllText(reportPath, html.ToString());
        }
    }
}