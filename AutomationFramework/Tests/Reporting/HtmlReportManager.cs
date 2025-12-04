using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutomationFramework.Tests.Reporting
{
    public class HtmlReportManager
    {
        private static HtmlReportManager _instance;
        private readonly List<string> _scenarioResults = new();

        public static HtmlReportManager Instance => _instance ??= new HtmlReportManager();

        public void AddScenarioResult(string scenario, string status, string screenshotPath, string error)
        {
            var sb = new StringBuilder();
            sb.Append($"<tr><td>{scenario}</td><td>{status}</td>");
            if (!string.IsNullOrEmpty(screenshotPath))
                sb.Append($"<td><img src='{screenshotPath}' width='200'/></td>");
            else
                sb.Append("<td>N/A</td>");
            sb.Append($"<td>{error}</td></tr>");
            _scenarioResults.Add(sb.ToString());
        }

        public void GenerateReport()
        {
            var html = new StringBuilder();
            html.Append("<html><head><title>Test Report</title></head><body>");
            html.Append("<h1>Test Execution Report</h1>");
            html.Append("<table border='1'><tr><th>Scenario</th><th>Status</th><th>Screenshot</th><th>Error</th></tr>");
            foreach (var row in _scenarioResults)
                html.Append(row);
            html.Append("</table>");
            html.Append($"<p>Environment: {Environment.MachineName}</p>");
            html.Append($"<p>Browser: {AutomationFramework.Core.Configuration.ConfigManager.Get("Browser")}</p>");
            html.Append("</body></html>");
            File.WriteAllText("TestReport.html", html.ToString());
        }
    }
}