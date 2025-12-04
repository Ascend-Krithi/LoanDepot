using OpenQA.Selenium;
using System;
using System.IO;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.Reporting
{
    public static class HtmlReportManager
    {
        public static void GenerateReport(ScenarioContext context, string screenshotPath, IWebDriver driver)
        {
            var reportDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
            Directory.CreateDirectory(reportDir);
            var fileName = $"{context.ScenarioInfo.Title}_{DateTime.Now:yyyyMMddHHmmss}.html";
            var filePath = Path.Combine(reportDir, fileName);

            using (var sw = new StreamWriter(filePath))
            {
                sw.WriteLine("<html><head><title>Test Report</title></head><body>");
                sw.WriteLine($"<h2>Scenario: {context.ScenarioInfo.Title}</h2>");
                sw.WriteLine("<ul>");
                // Steps and durations would be added here if available
                sw.WriteLine("</ul>");
                sw.WriteLine($"<b>Status:</b> {(context.TestError == null ? "Passed" : "Failed")}");
                if (context.TestError != null)
                {
                    sw.WriteLine($"<pre>{context.TestError.Message}<br>{context.TestError.StackTrace}</pre>");
                }
                if (!string.IsNullOrEmpty(screenshotPath))
                {
                    var imgBase64 = Convert.ToBase64String(File.ReadAllBytes(screenshotPath));
                    sw.WriteLine($"<img src='data:image/png;base64,{imgBase64}' width='600'/>");
                }
                sw.WriteLine($"<b>Browser:</b> {driver.GetType().Name}<br>");
                sw.WriteLine($"<b>Environment:</b> {AutomationFramework.Core.Configuration.ConfigManager.Get("Environment")}");
                sw.WriteLine("</body></html>");
            }
        }
    }
}