// NuGet Packages: SpecFlow
using AutomationFramework.Core.Configuration;
using SpecFlow;
using System;
using System.IO;
using System.Text;

namespace AutomationFramework.Tests.Reporting
{
    /// <summary>
    /// Generates a simple, self-contained HTML report for a single scenario execution.
    /// </summary>
    public static class HtmlReportGenerator
    {
        public static void GenerateReport(ScenarioContext scenarioContext, string screenshotPath)
        {
            var reportDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigManager.Settings.ReportPath);
            Directory.CreateDirectory(reportDir);

            string sanitizedTitle = string.Join("_", scenarioContext.ScenarioInfo.Title.Split(Path.GetInvalidFileNameChars()));
            var reportPath = Path.Combine(reportDir, $"{sanitizedTitle}_{DateTime.Now:yyyyMMddHHmmss}.html");

            var sb = new StringBuilder();
            var status = scenarioContext.TestError == null ? "Passed" : "Failed";
            var statusColor = status == "Passed" ? "green" : "red";

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='en'>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset='UTF-8'>");
            sb.AppendLine("<title>Test Report</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            sb.AppendLine("h1 { color: #333; }");
            sb.AppendLine(".summary { border: 1px solid #ccc; padding: 15px; margin-bottom: 20px; background-color: #f9f9f9; }");
            sb.AppendLine(".status { font-weight: bold; }");
            sb.AppendLine($".status.passed {{ color: {statusColor}; }}");
            sb.AppendLine($".status.failed {{ color: {statusColor}; }}");
            sb.AppendLine(".details { margin-top: 10px; }");
            sb.AppendLine(".error { background-color: #fdd; border: 1px solid red; padding: 10px; white-space: pre-wrap; font-family: monospace; }");
            sb.AppendLine("img { max-width: 100%; border: 1px solid #ddd; margin-top: 10px; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<h1>Scenario Execution Report</h1>");
            sb.AppendLine("<div class='summary'>");
            sb.AppendLine($"<h2>{scenarioContext.ScenarioInfo.Title}</h2>");
            sb.AppendLine($"<div class='details'><strong>Status:</strong> <span class='status {status.ToLower()}'>{status}</span></div>");
            sb.AppendLine($"<div class='details'><strong>Executed At:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</div>");
            sb.AppendLine("</div>");

            if (scenarioContext.TestError != null)
            {
                sb.AppendLine("<h3>Error Details</h3>");
                sb.AppendLine($"<div class='error'>{System.Net.WebUtility.HtmlEncode(scenarioContext.TestError.ToString())}</div>");

                if (!string.IsNullOrEmpty(screenshotPath) && File.Exists(screenshotPath))
                {
                    var imageBytes = File.ReadAllBytes(screenshotPath);
                    var base64Image = Convert.ToBase64String(imageBytes);
                    sb.AppendLine("<h3>Failure Screenshot</h3>");
                    sb.AppendLine($"<img src='data:image/png;base64,{base64Image}' alt='Failure Screenshot' />");
                }
            }

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            File.WriteAllText(reportPath, sb.ToString());
        }
    }
}