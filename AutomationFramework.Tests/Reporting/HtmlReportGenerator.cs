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
                var reportsDir = Path.Combine(AppContext.BaseDirectory, "HtmlReports");
                Directory.CreateDirectory(reportsDir);

                var fileName = $"{featureName}_{scenarioName}_{DateTime.Now:yyyyMMdd_HHmmss}.html".Replace(" ", "_");
                var filePath = Path.Combine(reportsDir, fileName);

                using (var sw = new StreamWriter(filePath, false))
                {
                    sw.WriteLine("<html><head><title>Test Report</title></head><body>");
                    sw.WriteLine($"<h2>Feature: {featureName}</h2>");
                    sw.WriteLine($"<h3>Scenario: {scenarioName}</h3>");
                    sw.WriteLine($"<p>Status: <b style='color:{(passed ? "green" : "red")}'>{(passed ? "Passed" : "Failed")}</b></p>");
                    sw.WriteLine($"<p>Timestamp: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>");
                    if (!passed && !string.IsNullOrWhiteSpace(errorMessage))
                    {
                        sw.WriteLine("<h4>Error Details:</h4>");
                        sw.WriteLine($"<pre>{System.Net.WebUtility.HtmlEncode(errorMessage)}</pre>");
                    }
                    sw.WriteLine("</body></html>");
                }
            }
            catch
            {
                // Swallow all exceptions
            }
        }
    }
}