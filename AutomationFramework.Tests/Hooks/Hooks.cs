// NuGet Packages: SpecFlow, BoDi, Selenium.WebDriver
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Tests.Reporting;
using BoDi;
using OpenQA.Selenium;
using SpecFlow;
using System;
using System.IO;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        private IWebDriver _driver;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Create a new WebDriver instance for each scenario
            var factory = new WebDriverFactory();
            _driver = factory.CreateDriver();

            // Register the WebDriver instance with SpecFlow's dependency injection container
            _objectContainer.RegisterInstanceAs<IWebDriver>(_driver);
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            string screenshotPath = null;
            if (scenarioContext.TestError != null)
            {
                // Take a screenshot on failure
                try
                {
                    var takesScreenshot = _driver as ITakesScreenshot;
                    if (takesScreenshot != null)
                    {
                        var screenshot = takesScreenshot.GetScreenshot();
                        string reportDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigManager.Settings.ReportPath);
                        Directory.CreateDirectory(reportDir);
                        // Sanitize scenario title for use as a filename
                        string sanitizedTitle = string.Join("_", scenarioContext.ScenarioInfo.Title.Split(Path.GetInvalidFileNameChars()));
                        screenshotPath = Path.Combine(reportDir, $"{sanitizedTitle}_{DateTime.Now:yyyyMMddHHmmss}_failure.png");
                        screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to take screenshot: {ex.Message}");
                }
            }

            // Generate HTML report
            HtmlReportGenerator.GenerateReport(scenarioContext, screenshotPath);

            // Clean up and close the browser
            _driver?.Quit();
            _driver?.Dispose();

            // Clear self-healing repository for the next test
            SelfHealingRepository.Clear();
        }
    }
}