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
            // Eagerly load configuration to ensure it's available and valid
            _ = ConfigManager.Settings;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var driver = WebDriverFactory.CreateDriver();
            _container.RegisterInstanceAs<IWebDriver>(driver.InnerDriver);
            _container.RegisterInstanceAs(driver);

            if (!string.IsNullOrEmpty(ConfigManager.Settings.BaseUrl))
            {
                driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var driver = _container.Resolve<SelfHealingWebDriver>();
            var featureName = _scenarioContext.ScenarioContainer.Resolve<FeatureContext>().FeatureInfo.Title;
            var scenarioName = _scenarioContext.ScenarioInfo.Title;
            var passed = _scenarioContext.TestError == null;
            string? errorMessage = null;

            if (!passed)
            {
                errorMessage = _scenarioContext.TestError.Message;
                try
                {
                    var screenshotDriver = (ITakesScreenshot)driver.InnerDriver;
                    var screenshot = screenshotDriver.GetScreenshot();
                    var screenshotDir = Path.Combine(AppContext.BaseDirectory, "Screenshots");
                    Directory.CreateDirectory(screenshotDir);
                    var screenshotPath = Path.Combine(screenshotDir, $"{scenarioName}_{DateTime.Now:yyyyMMddHHmmss}.png");
                    screenshot.SaveAsFile(screenshotPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to take screenshot: {ex.Message}");
                }
            }

            HtmlReportGenerator.GenerateReport(featureName, scenarioName, passed, errorMessage);

            driver?.Dispose();
        }
    }
}