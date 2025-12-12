// AutomationFramework.Tests/Hooks/Hooks.cs
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Tests.Reporting;
using BoDi;
using OpenQA.Selenium;
using System;
using System.IO;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        private SelfHealingWebDriver? _driver;
        private readonly ScenarioContext _scenarioContext;
        private readonly FeatureContext _featureContext;

        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Access settings once to ensure configuration is loaded and validated early.
            _ = ConfigManager.Settings;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _driver = WebDriverFactory.CreateDriver();
            _objectContainer.RegisterInstanceAs<IWebDriver>(_driver.InnerDriver);
            _objectContainer.RegisterInstanceAs(_driver);

            if (!string.IsNullOrEmpty(ConfigManager.Settings.BaseUrl))
            {
                _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            string? screenshotPath = null;
            if (_scenarioContext.TestError != null && _driver != null)
            {
                try
                {
                    var screenshotDriver = (ITakesScreenshot)_driver.InnerDriver;
                    var screenshot = screenshotDriver.GetScreenshot();
                    var screenshotsDir = Path.Combine(AppContext.BaseDirectory, "Screenshots");
                    Directory.CreateDirectory(screenshotsDir);
                    var screenshotFileName = $"{_featureContext.FeatureInfo.Title}_{_scenarioContext.ScenarioInfo.Title}_{DateTime.Now:yyyyMMddHHmmss}.png".Replace(" ", "_");
                    screenshotPath = Path.Combine(screenshotsDir, screenshotFileName);
                    screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to take screenshot: {ex.Message}");
                }
            }

            HtmlReportGenerator.GenerateReport(
                _featureContext.FeatureInfo.Title,
                _scenarioContext.ScenarioInfo.Title,
                _scenarioContext.TestError == null,
                _scenarioContext.TestError?.ToString(),
                screenshotPath
            );

            _driver?.Quit();
            _driver?.Dispose();
        }
    }
}