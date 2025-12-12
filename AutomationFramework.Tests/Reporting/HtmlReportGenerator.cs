// AutomationFramework.Tests/Reporting/HtmlReportGenerator.cs
using System;
using System.IO;
using System.Text;

namespace AutomationFramework.Tests.Reporting
{
    public static class HtmlReportGenerator
    {
        public static void GenerateReport(string featureName, string scenarioName, bool passed, string? errorMessage, string? screenshotPath)
        {
            var reportsDir = Path.Combine(AppContext.BaseDirectory, "HtmlReports");
            Directory.CreateDirectory(reportsDir);

            var fileName = $"{featureName}_{scenarioName}_{DateTime.Now:yyyyMMddHHmmss}.html".Replace(" ", "_");
            var filePath = Path.Combine(reportsDir, fileName);

            var status = passed ? "<span style='color:green;'>Passed</span>" : "<span style='color:red;'>Failed</span>";
            var errorHtml = !passed && !string.IsNullOrEmpty(errorMessage)
                ? $"<h3>Error Details</h3><pre style='background-color:#f0f0f0; padding:10px; border:1px solid #ccc;'>{errorMessage}</pre>"
                : "";

            var screenshotHtml = !string.IsNullOrEmpty(screenshotPath)
                ? $"<h3>Screenshot</h3><img src='{screenshotPath}' style='max-width:100%; border:1px solid #ccc;'/>"
                : "";

            var html = new StringBuilder();
            html.AppendLine("<html><head><title>Test Report</title></head><body style='font-family:sans-serif;'>");
            html.AppendLine($"<h1>Test Execution Report</h1>");
            html.AppendLine($"<p><strong>Timestamp:</strong> {DateTime.Now:F}</p>");
            html.AppendLine($"<p><strong>Feature:</strong> {featureName}</p>");
            html.AppendLine($"<p><strong>Scenario:</strong> {scenarioName}</p>");
            html.AppendLine($"<p><strong>Status:</strong> {status}</p>");
            html.AppendLine(errorHtml);
            html.AppendLine(screenshotHtml);
            html.AppendLine("</body></html>");

            File.WriteAllText(filePath, html.ToString());
        }
    }
}