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

            var fileName = $"{featureName}_{scenarioName}_{DateTime.Now:yyyyMMddHHmmss}.html";
            var filePath = Path.Combine(reportsDir, fileName);

            var sb = new StringBuilder();
            sb.AppendLine("<html><head><title>Test Report</title></head><body>");
            sb.AppendLine($"<h2>Feature: {featureName}</h2>");
            sb.AppendLine($"<h3>Scenario: {scenarioName}</h3>");
            sb.AppendLine($"<p>Status: <b style='color:{(passed ? "green" : "red")}'>{(passed ? "Passed" : "Failed")}</b></p>");
            sb.AppendLine($"<p>Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>");
            if (!passed && !string.IsNullOrEmpty(errorMessage))
            {
                sb.AppendLine("<h4>Error Details:</h4>");
                sb.AppendLine($"<pre>{System.Net.WebUtility.HtmlEncode(errorMessage)}</pre>");
            }
            sb.AppendLine("</body></html>");

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}