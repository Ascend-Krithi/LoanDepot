using System;
using System.IO;

namespace AutomationFramework.Tests.Reporting
{
    public static class HtmlReportGenerator
    {
        public static void GenerateReport(string featureName, string scenarioName, bool passed, string errorMessage)
        {
            string reportsDir = Path.Combine(AppContext.BaseDirectory, "HtmlReports");
            Directory.CreateDirectory(reportsDir);
            string fileName = $"{scenarioName}_{DateTime.Now:yyyyMMddHHmmss}.html";
            string filePath = Path.Combine(reportsDir, fileName);

            string html = $@"
<html>
<head><title>Test Report</title></head>
<body>
    <h2>Feature: {featureName}</h2>
    <h3>Scenario: {scenarioName}</h3>
    <p>Status: {(passed ? "Passed" : "Failed")}</p>
    {(passed ? "" : $"<pre>Error: {errorMessage}</pre>")}
    <p>Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
</body>
</html>";

            File.WriteAllText(filePath, html);
        }
    }
}