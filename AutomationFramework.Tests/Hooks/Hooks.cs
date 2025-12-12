using System;
using System.IO;
using BoDi;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Tests.Reporting;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _container;
        private readonly ScenarioContext _scenarioContext;
        private SelfHealingWebDriver _driver;

        public Hooks(IObjectContainer container, ScenarioContext scenarioContext)
        {
            _container = container;
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Ensure config is loaded
            var _ = ConfigManager.Settings;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _driver = WebDriverFactory.CreateDriver();
            _container.RegisterInstanceAs<IWebDriver>(_driver.InnerDriver);
            _container.RegisterInstanceAs<SelfHealingWebDriver>(_driver);

            var baseUrl = ConfigManager.Settings.BaseUrl;
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                _driver.Url = baseUrl;
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            bool passed = _scenarioContext.TestError == null;
            string featureName = _scenarioContext.ScenarioInfo?.FeatureTitle ?? "UnknownFeature";
            string scenarioName = _scenarioContext.ScenarioInfo?.Title ?? "UnknownScenario";
            string errorMessage = _scenarioContext.TestError?.ToString();

            // Screenshot on failure
            if (!passed)
            {
                try
                {
                    var screenshotDriver = _driver.InnerDriver as ITakesScreenshot;
                    if (screenshotDriver != null)
                    {
                        var screenshot = screenshotDriver.GetScreenshot();
                        var dir = Path.Combine(AppContext.BaseDirectory, "Screenshots");
                        Directory.CreateDirectory(dir);
                        var file = Path.Combine(dir, $"{featureName}_{scenarioName}_{DateTime.Now:yyyyMMddHHmmssfff}.png");
                        screenshot.SaveAsFile(file, ScreenshotImageFormat.Png);
                    }
                }
                catch { }
            }

            // HTML report
            HtmlReportGenerator.GenerateReport(featureName, scenarioName, passed, errorMessage);

            // Dispose driver
            try { _driver?.Quit(); } catch { }
        }
    }
}