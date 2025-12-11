using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutomationFramework.Tests.Reporting
{
    public class HtmlReportManager
    {
        private readonly List<string> _steps = new List<string>();
        private readonly string _scenarioName;
        private readonly string _browser;
        private readonly string _environment;
        private DateTime _startTime;
        private DateTime _endTime;
        private bool _passed;
        private string _error;
        private string _stackTrace;
        private string _screenshotPath;

        public HtmlReportManager(string scenarioName, string browser, string environment)
        {
            _scenarioName = scenarioName;
            _browser = browser;
            _environment = environment;
            _startTime = DateTime.Now;
        }

        public void AddStep(string step, TimeSpan duration)
        {
            _steps.Add($"<tr><td>{step}</td><td>{duration.TotalSeconds:F2}s</td></tr>");
        }

        public void MarkResult(bool passed, string error = null, string stackTrace = null, string screenshotPath = null)
        {
            _passed = passed;
            _error = error;
            _stackTrace = stackTrace;
            _screenshotPath = screenshotPath;
            _endTime = DateTime.Now;
        }

        public void GenerateReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html><head><title>Test Report</title></head><body>");
            sb.AppendLine($"<h2>Scenario: {_scenarioName}</h2>");
            sb.AppendLine($"<b>Browser:</b> {_browser} <br/>");
            sb.AppendLine($"<b>Environment:</b> {_environment} <br/>");
            sb.AppendLine($"<b>Start:</b> {_startTime} <br/>");
            sb.AppendLine($"<b>End:</b> {_endTime} <br/>");
            sb.AppendLine($"<b>Duration:</b> {(_endTime - _startTime).TotalSeconds:F2}s <br/>");
            sb.AppendLine($"<b>Status:</b> <span style='color:{(_passed ? "green" : "red")}'>{(_passed ? "PASS" : "FAIL")}</span><br/>");

            if (!_passed)
            {
                sb.AppendLine($"<b>Error:</b> {_error}<br/>");
                sb.AppendLine($"<b>StackTrace:</b> <pre>{_stackTrace}</pre><br/>");
            }

            if (!string.IsNullOrEmpty(_screenshotPath))
            {
                var base64 = Convert.ToBase64String(File.ReadAllBytes(_screenshotPath));
                sb.AppendLine($"<b>Screenshot:</b><br/><img src='data:image/png;base64,{base64}' width='600'/><br/>");
            }

            sb.AppendLine("<h3>Steps</h3>");
            sb.AppendLine("<table border='1'><tr><th>Step</th><th>Duration</th></tr>");
            foreach (var step in _steps)
                sb.AppendLine(step);
            sb.AppendLine("</table>");
            sb.AppendLine("</body></html>");

            var fileName = $"TestReport_{_scenarioName}_{DateTime.Now:yyyyMMdd_HHmmss}.html";
            File.WriteAllText(fileName, sb.ToString());
        }
    }
}