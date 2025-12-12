using System;
using System.IO;

namespace AutomationFramework.Tests.Reporting
{
    public static class HtmlReportGenerator
    {
        public static void GenerateReport(string featureName, string scenarioName, bool passed, string errorMessage)
        {
            var reportsDir = Path.Combine(AppContext.BaseDirectory, "HtmlReports");
            Directory.CreateDirectory(reportsDir);

            var fileName = $"{featureName}_{scenarioName}_{DateTime.Now:yyyyMMdd_HHmmss}.html";
            var filePath = Path.Combine(reportsDir, fileName);

            var html = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <title>Test Report</title>
    <style>
        body {{ font-family: Arial, sans-serif; }}
        .passed {{ color: green; }}
        .failed {{ color: red; }}
        .section {{ margin-bottom: 1em; }}
    </style>
</head>
<body>
    <h2>Test Scenario Report</h2>
    <div class='section'><strong>Feature:</strong> {featureName}</div>
    <div class='section'><strong>Scenario:</strong> {scenarioName}</div>
    <div class='section'><strong>Status:</strong> <span class='{(passed ? "passed" : "failed")}'>{(passed ? "Passed" : "Failed")}</span></div>
    <div class='section'><strong>Timestamp:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</div>
    {(passed ? "" : $"<div class='section'><strong>Error:</strong><pre>{System.Net.WebUtility.HtmlEncode(errorMessage)}</pre></div>")}
</body>
</html>
";
            File.WriteAllText(filePath, html);
        }
    }
}