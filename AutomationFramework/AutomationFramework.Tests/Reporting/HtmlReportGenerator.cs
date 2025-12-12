using System;
using System.IO;
using System.Text;

namespace AutomationFramework.Tests.Reporting
{
    public static class HtmlReportGenerator
    {
        public static void GenerateReport(string featureName, string scenarioName, bool passed, string errorMessage)
        {
            var reportsDir = Path.Combine(AppContext.BaseDirectory, "HtmlReports");
            Directory.CreateDirectory(reportsDir);

            var fileName = $"{featureName}_{scenarioName}_{DateTime.Now:yyyyMMdd_HHmmss}.html".Replace(" ", "_");
            var filePath = Path.Combine(reportsDir, fileName);

            var sb = new StringBuilder();
            sb.AppendLine("<html><head><title>Test Report</title></head><body>");
            sb.AppendLine($"<h2>Feature: {featureName}</h2>");
            sb.AppendLine($"<h3>Scenario: {scenarioName}</h3>");
            sb.AppendLine($"<p>Status: <b style='color:{(passed ? "green" : "red")}'>{(passed ? "Passed" : "Failed")}</b></p>");
            sb.AppendLine($"<p>Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>");
            if (!passed && !string.IsNullOrWhiteSpace(errorMessage))
            {
                sb.AppendLine("<pre style='color:red;'>" + System.Net.WebUtility.HtmlEncode(errorMessage) + "</pre>");
            }
            sb.AppendLine("</body></html>");

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}