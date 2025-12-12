// NuGet Packages: None
namespace AutomationFramework.Core.Configuration
{
    /// <summary>
    /// Represents the test settings that can be configured in a settings file (e.g., appsettings.json).
    /// This class is a Plain Old C# Object (POCO) used for deserializing configuration data.
    /// </summary>
    public class TestSettings
    {
        /// <summary>
        /// Gets or sets the target browser for test execution (e.g., "Chrome", "Firefox", "Edge").
        /// </summary>
        public string Browser { get; set; } = "Chrome";

        /// <summary>
        /// Gets or sets the base URL of the application under test.
        /// </summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the default timeout in seconds for explicit waits.
        /// </summary>
        public int DefaultTimeoutInSeconds { get; set; } = 30;

        /// <summary>
        /// Gets or sets a value indicating whether to run the browser in headless mode.
        /// </summary>
        public bool Headless { get; set; } = false;

        /// <summary>
        /// Gets or sets the path to the directory where reports and logs will be saved.
        /// </summary>
        public string ReportPath { get; set; } = "Reports";

        /// <summary>
        /// Gets or sets a value indicating whether the self-healing mechanism is enabled.
        /// </summary>
        public bool IsSelfHealingEnabled { get; set; } = true;
    }
}