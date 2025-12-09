using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AutomationFramework.Core.Utilities
{
    public static class HtmlReportManager
    {
        private static List<string> _steps = new List<string>();
        private static List<string> _healingEvents = new List<string>();
        private static string _scenarioName;
        private static DateTime _startTime;
        private static string _browser;
        private static string _environment;
        private static string _screenshotPath;
        private static bool _testPassed = true;
        private static string _error;
        private static string _stackTrace;

        public static void StartScenario(string scenarioName, string browser, string environment)
        {
            _scenarioName = scenarioName;
            _browser = browser;
            _environment = environment;
            _startTime = DateTime.Now;
            _steps.Clear();
            _healingEvents.Clear();
            _testPassed = true;
            _error = null;
            _stackTrace = null;
            _screenshotPath = null;
        }

        public static void LogStep(string step, TimeSpan duration)
        {
            _steps.Add($"<tr><td>{step}</td><td>{duration.TotalSeconds:F2}s</td></tr>");
        }

        public static void LogHealingEvent(string locatorKey, string by, bool success, string error)
        {
            var color = success ? "green" : "red";
            _healingEvents.Add($"<tr><td>{locatorKey}</td><td>{by}</td><td style='color:{color}'>{(success ? "Success" : "Fail")}</td><td>{error}</td></tr>");
        }

        public static void MarkFailed(string error, string stackTrace, string screenshotPath)
        {
            _testPassed = false;
            _error = error;
            _stackTrace = stackTrace;
            _screenshotPath = screenshotPath;
        }

        public static void SaveReport()
        {
            var endTime = DateTime.Now;
            var duration = endTime - _startTime;
            var status = _testPassed ? "PASS" : "FAIL";
            var color = _testPassed ? "green" : "red";
            var reportDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tests", "Reporting");
            Directory.CreateDirectory(reportDir);
            var fileName = $"{_scenarioName}_{endTime:yyyyMMdd_HHmmss}.html";
            var filePath = Path.Combine(reportDir, fileName);

            var sb = new StringBuilder();
            sb.AppendLine("<html><head><title>Test Report</title></head><body>");
            sb.AppendLine($"<h2>Scenario: {_scenarioName}</h2>");
            sb.AppendLine($"<b>Status:</b> <span style='color:{color}'>{status}</span><br>");
            sb.AppendLine($"<b>Start:</b> {_startTime}<br>");
            sb.AppendLine($"<b>End:</b> {endTime}<br>");
            sb.AppendLine($"<b>Duration:</b> {duration.TotalSeconds:F2}s<br>");
            sb.AppendLine($"<b>Browser:</b> {_browser}<br>");
            sb.AppendLine($"<b>Environment:</b> {_environment}<br>");
            if (!_testPassed)
            {
                sb.AppendLine($"<b>Error:</b> {_error}<br>");
                sb.AppendLine($"<b>StackTrace:</b><pre>{_stackTrace}</pre><br>");
                if (!string.IsNullOrEmpty(_screenshotPath))
                {
                    var relPath = Path.GetFileName(_screenshotPath);
                    sb.AppendLine($"<b>Screenshot:</b><br><img src='{relPath}' width='600'/><br>");
                }
            }
            sb.AppendLine("<h3>Steps</h3>");
            sb.AppendLine("<table border='1'><tr><th>Step</th><th>Duration</th></tr>");
            foreach (var step in _steps) sb.AppendLine(step);
            sb.AppendLine("</table>");

            sb.AppendLine("<h3>Self-Healing Events</h3>");
            sb.AppendLine("<table border='1'><tr><th>Locator Key</th><th>By</th><th>Status</th><th>Error</th></tr>");
            foreach (var evt in _healingEvents) sb.AppendLine(evt);
            sb.AppendLine("</table>");

            sb.AppendLine("</body></html>");
            File.WriteAllText(filePath, sb.ToString());
        }
    }
}