using TechTalk.SpecFlow;
using System.IO;
using System.Text;
using System;

namespace AutomationFramework.Tests.Reporting
{
    public class HtmlReportGenerator
    {
        public void GenerateReport(ScenarioContext context)
        {
            var reportDir = Path.Combine(Directory.GetCurrentDirectory(), "TestReports");
            Directory.CreateDirectory(reportDir);

            var scenarioTitle = context.ScenarioInfo.Title;
            var fileName = $"{scenarioTitle.Replace(" ", "_")}_{DateTime.Now:yyyyMMddHHmmss}.html";
            var filePath = Path.Combine(reportDir, fileName);

            var status = context.TestError == null ? "Passed" : "Failed";
            var statusColor = status == "Passed" ? "green" : "red";

            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<title>Test Report</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            sb.AppendLine("h1 { color: #333; }");
            sb.AppendLine(".summary { border: 1px solid #ccc; padding: 15px; margin-bottom: 20px; background-color: #f9f9f9; }");
            sb.AppendLine(".status { font-weight: bold; }");
            sb.AppendLine(".passed { color: green; }");
            sb.AppendLine(".failed { color: red; }");
            sb.AppendLine(".error { background-color: #ffebeb; border: 1px solid red; padding: 10px; white-space: pre-wrap; font-family: monospace; }");
            sb.AppendLine("img { max-width: 100%; border: 1px solid #ddd; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine($"<h1>Test Report: {scenarioTitle}</h1>");
            sb.AppendLine("<div class='summary'>");
            sb.AppendLine($"<p><strong>Scenario:</strong> {scenarioTitle}</p>");
            sb.AppendLine($"<p><strong>Feature:</strong> {context.FeatureContext.FeatureInfo.Title}</p>");
            sb.AppendLine($"<p><strong>Execution Time:</strong> {DateTime.Now:F}</p>");
            sb.AppendLine($"<p><strong>Status:</strong> <span class='status {status.ToLower()}'>{status}</span></p>");
            sb.AppendLine("</div>");

            if (context.TestError != null)
            {
                sb.AppendLine("<h2>Error Details</h2>");
                sb.AppendLine($"<div class='error'><strong>Message:</strong> {context.TestError.Message}<br/><br/><strong>Stack Trace:</strong><br/>{context.TestError.StackTrace}</div>");

                if (context.ContainsKey("ScreenshotPath"))
                {
                    var screenshotPath = context["ScreenshotPath"].ToString();
                    var screenshotFileName = Path.GetFileName(screenshotPath);
                    // Copy screenshot to report directory and use relative path
                    var destScreenshotPath = Path.Combine(reportDir, screenshotFileName);
                    File.Copy(screenshotPath, destScreenshotPath, true);

                    sb.AppendLine("<h2>Screenshot</h2>");
                    sb.AppendLine($"<a href='{screenshotFileName}' target='_blank'><img src='{screenshotFileName}' alt='Failure Screenshot'/></a>");
                }
            }

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            File.WriteAllText(filePath, sb.ToString());
            Console.WriteLine($"HTML report generated at: {filePath}");
        }
    }
}