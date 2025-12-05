using System;
using System.Collections.Generic;
using System.IO;

namespace AutomationFramework.Tests.Reporting
{
    public class HtmlReportManager
    {
        private readonly List<(string scenario, bool passed, string screenshot)> _results = new();

        public void AddResult(string scenarioName, bool passed, string screenshotPath)
        {
            _results.Add((scenarioName, passed, screenshotPath));
        }

        public void Flush()
        {
            var dir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
            Directory.CreateDirectory(dir);
            var path = Path.Combine(dir, $"Report_{DateTime.UtcNow:yyyyMMddHHmmss}.html");
            using var sw = new StreamWriter(path);
            sw.WriteLine("<html><head><title>Test Report</title></head><body>");
            sw.WriteLine("<h1>SpecFlow Test Report</h1>");
            sw.WriteLine("<table border='1' cellpadding='5' cellspacing='0'>");
            sw.WriteLine("<tr><th>Scenario</th><th>Status</th><th>Screenshot</th></tr>");
            foreach (var r in _results)
            {
                var status = r.passed ? "Passed" : "Failed";
                sw.WriteLine($"<tr><td>{r.scenario}</td><td>{status}</td><td>{r.screenshot}</td></tr>");
            }
            sw.WriteLine("</table></body></html>");
        }
    }
}
