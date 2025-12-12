using System.Text;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.Reporting
{
    public static class HtmlReportGenerator
    {
        public static void GenerateReport(ScenarioInfo scenarioInfo, Exception? error)
        {
            var reportDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
            Directory.CreateDirectory(reportDir);
            var fileName = $"Report_{scenarioInfo.Title.Replace(" ", "_")}_{DateTime.Now:yyyyMMddHHmmss}.html";
            var filePath = Path.Combine(reportDir, fileName);

            var status = error == null ? "Passed" : "Failed";
            var statusColor = error == null ? "green" : "red";

            var sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<title>Test Report</title>");
            sb.AppendLine("<style>");
            sb.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            sb.AppendLine("h1 { color: #333; }");
            sb.AppendLine(".report-card { border: 1px solid #ccc; border-radius: 5px; padding: 20px; max-width: 800px; margin: auto; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }");
            sb.AppendLine(".status { font-size: 24px; font-weight: bold; }");
            sb.AppendLine($".status.passed {{ color: {statusColor}; }}");
            sb.AppendLine($".status.failed {{ color: {statusColor}; }}");
            sb.AppendLine(".details { margin-top: 20px; }");
            sb.AppendLine(".details p { margin: 5px 0; }");
            sb.AppendLine(".error { background-color: #fdd; border: 1px solid red; padding: 10px; margin-top: 15px; white-space: pre-wrap; word-wrap: break-word; }");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<div class='report-card'>");
            sb.AppendLine("<h1>Scenario Test Report</h1>");
            sb.AppendLine("<hr>");
            sb.AppendLine("<div class='details'>");
            sb.AppendLine($"<p><b>Feature:</b> {scenarioInfo.FolderPath}</p>");
            sb.AppendLine($"<p><b>Scenario:</b> {scenarioInfo.Title}</p>");
            sb.AppendLine($"<p><b>Timestamp:</b> {DateTime.Now}</p>");
            sb.AppendLine($"<p><b>Status:</b> <span class='status {status.ToLower()}'>{status}</span></p>");
            sb.AppendLine("</div>");

            if (error != null)
            {
                sb.AppendLine("<div class='error'>");
                sb.AppendLine("<b>Error Details:</b><br>");
                sb.AppendLine(System.Net.WebUtility.HtmlEncode(error.ToString()));
                sb.AppendLine("</div>");
            }

            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}