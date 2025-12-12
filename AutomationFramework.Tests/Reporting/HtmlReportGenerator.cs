using System;
using System.IO;

namespace AutomationFramework.Tests.Reporting
{
    public static class HtmlReportGenerator
    {
        public static void GenerateReport(string featureName, string scenarioName, bool passed, string errorMessage)
        {
            try
            {
                var dir = Path.Combine(AppContext.BaseDirectory, "HtmlReports");
                Directory.CreateDirectory(dir);
                var file = Path.Combine(dir, $"{featureName}_{scenarioName}_{DateTime.Now:yyyyMMddHHmmssfff}.html");

                var html = $@"
<html>
<head>
    <title>Test Report - {featureName} - {scenarioName}</title>
    <style>
        body {{ font-family: Arial, sans-serif; }}
        .passed {{ color: green; }}
        .failed {{ color: red; }}
        .section {{ margin-bottom: 1em; }}
    </style>
</head>
<body>
    <h2>Feature: {featureName}</h2>
    <h3>Scenario: {scenarioName}</h3>
    <div class='section'>
        <strong>Status:</strong> <span class='{(passed ? "passed" : "failed")}'>{(passed ? "Passed" : "Failed")}</span>
    </div>
    {(passed ? "" : $"<div class='section'><strong>Error:</strong><pre>{System.Net.WebUtility.HtmlEncode(errorMessage)}</pre></div>")}
    <div class='section'>
        <strong>Timestamp:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}
    </div>
</body>
</html>
";
                File.WriteAllText(file, html);
            }
            catch
            {
                // Swallow all exceptions
            }
        }
    }
}