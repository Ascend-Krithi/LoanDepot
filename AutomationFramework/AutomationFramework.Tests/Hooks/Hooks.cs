using TechTalk.SpecFlow;
using BoDi;
using OpenQA.Selenium;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Tests.Reporting;
using System.IO;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private readonly IObjectContainer _container;
        private IWebDriver _driver;
        private readonly TestSettings _testSettings;

        public Hooks(IObjectContainer container)
        {
            _container = container;
            _testSettings = ConfigManager.GetTestSettings();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var factory = new WebDriverFactory();
            _driver = factory.Create(_testSettings);
            _container.RegisterInstanceAs<IWebDriver>(_driver);
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            if (_driver != null)
            {
                if (scenarioContext.TestError != null)
                {
                    // Take screenshot on failure
                    try
                    {
                        var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                        string screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), $"{scenarioContext.ScenarioInfo.Title.Replace(" ", "_")}_failure.png");
                        screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
                        // Attach screenshot path to context for reporting
                        scenarioContext["ScreenshotPath"] = screenshotPath;
                    }
                    catch (System.Exception ex)
                    {
                        Core.Utilities.Logger.LogError("Failed to take screenshot.", ex);
                    }
                }

                // Generate HTML Report
                var reportGenerator = new HtmlReportGenerator();
                reportGenerator.GenerateReport(scenarioContext);

                _driver.Quit();
                _driver.Dispose();
            }
        }
    }
}