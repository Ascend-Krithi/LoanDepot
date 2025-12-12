using System;
using System.IO;

namespace AutomationFramework.Tests.Reporting
{
    public static class HtmlReportGenerator
    {
        public static void GenerateReport(string featureName, string scenarioName, bool passed, string? errorMessage)
        {
            var reportDir = Path.Combine(AppContext.BaseDirectory, "HtmlReports");
            Directory.CreateDirectory(reportDir);

            var fileName = $"{scenarioName.Replace(" ", "_")}_{DateTime.Now:yyyyMMddHHmmss}.html";
            var filePath = Path.Combine(reportDir, fileName);

            var status = passed ? "Passed" : "Failed";
            var color = passed ? "green" : "red";

            var htmlContent = $@"
            <!DOCTYPE html>
            <html>
            <head>
                <title>Test Report: {scenarioName}</title>
                <style>
                    body {{ font-family: Arial, sans-serif; }}
                    .container {{ width: 80%; margin: auto; padding: 20px; border: 1px solid #ccc; border-radius: 5px; }}
                    .header {{ font-size: 24px; font-weight: bold; }}
                    .status {{ font-size: 20px; font-weight: bold; color: {color}; }}
                    .details {{ margin-top: 20px; }}
                    .error {{ background-color: #fdd; border: 1px solid red; padding: 10px; white-space: pre-wrap; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>Test Execution Report</div>
                    <hr>
                    <div class='details'>
                        <p><strong>Feature:</strong> {featureName}</p>
                        <p><strong>Scenario:</strong> {scenarioName}</p>
                        <p><strong>Timestamp:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>
                        <p><strong>Status:</strong> <span class='status'>{status}</span></p>
                    </div>
                    {(passed ? "" : $"<div class='error'><strong>Error:</strong><br>{System.Security.SecurityElement.Escape(errorMessage)}</div>")}
                </div>
            </body>
            </html>";

            File.WriteAllText(filePath, htmlContent);
        }
    }
}