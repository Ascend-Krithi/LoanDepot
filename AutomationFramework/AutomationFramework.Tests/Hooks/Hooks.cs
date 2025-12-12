using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Tests.Reporting;
using BoDi;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private readonly IObjectContainer _container;
        private readonly ScenarioContext _scenarioContext;

        public Hooks(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _container = container;
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Initialize logger and load configuration
            Logger.Initialize();
            Logger.Log("Framework initialization started.");
            var baseUrl = ConfigManager.TestSettings.BaseUrl;
            Logger.Log($"Configuration loaded. Base URL: {baseUrl}");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Logger.Log($"--- Starting Scenario: {_scenarioContext.ScenarioInfo.Title} ---");
            var webDriverFactory = new WebDriverFactory();
            var driver = webDriverFactory.Create();
            
            // Register instances for dependency injection
            _container.RegisterInstanceAs<SelfHealingWebDriver>(driver);
            _container.RegisterInstanceAs<IWebDriver>(driver); // Register as IWebDriver for compatibility
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var driver = _container.Resolve<SelfHealingWebDriver>();
            var scenarioInfo = _scenarioContext.ScenarioInfo;
            var testError = _scenarioContext.TestError;

            if (testError != null)
            {
                Logger.Log($"!!! Scenario Failed: {scenarioInfo.Title}");
                Logger.Log($"Error: {testError.Message}");
                
                // Take screenshot on failure
                try
                {
                    var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                    string screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", $"{scenarioInfo.Title.Replace(" ", "_")}_failure.png");
                    Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)!);
                    screenshot.SaveAsFile(screenshotPath);
                    Logger.Log($"Screenshot saved to: {screenshotPath}");
                }
                catch (Exception ex)
                {
                    Logger.Log($"Failed to take screenshot: {ex.Message}");
                }
            }
            else
            {
                Logger.Log($"--- Scenario Passed: {scenarioInfo.Title} ---");
            }

            // Generate HTML Report
            HtmlReportGenerator.GenerateReport(scenarioInfo, testError);

            // Clean up driver
            driver?.Quit();
            driver?.Dispose();
        }
    }
}