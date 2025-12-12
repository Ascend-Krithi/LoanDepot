using System;
using System.IO;
using BoDi;
using TechTalk.SpecFlow;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;
using AutomationFramework.Tests.Reporting;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public class Hooks
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
            // Ensure configuration is loaded
            var _ = ConfigManager.Settings;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var driver = WebDriverFactory.CreateDriver();
            _container.RegisterInstanceAs<IWebDriver>(driver.InnerDriver);
            _container.RegisterInstanceAs(driver);

            var baseUrl = ConfigManager.Settings.BaseUrl;
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                driver.Navigate().GoToUrl(baseUrl);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var driver = _container.Resolve<SelfHealingWebDriver>();
            bool failed = _scenarioContext.TestError != null;
            string featureName = _scenarioContext.ScenarioInfo.FeatureTitle;
            string scenarioName = _scenarioContext.ScenarioInfo.Title;
            string errorMessage = failed ? _scenarioContext.TestError?.ToString() : null;

            if (failed)
            {
                try
                {
                    var takesScreenshot = driver.InnerDriver as ITakesScreenshot;
                    if (takesScreenshot != null)
                    {
                        var screenshot = takesScreenshot.GetScreenshot();
                        var screenshotsDir = Path.Combine(AppContext.BaseDirectory, "Screenshots");
                        Directory.CreateDirectory(screenshotsDir);
                        var fileName = $"{featureName}_{scenarioName}_{DateTime.Now:yyyyMMdd_HHmmss}.png".Replace(" ", "_");
                        var filePath = Path.Combine(screenshotsDir, fileName);
                        screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
                    }
                }
                catch { }
            }

            HtmlReportGenerator.GenerateReport(featureName, scenarioName, !failed, errorMessage);

            try { driver.Quit(); } catch { }
            try { driver.Dispose(); } catch { }
        }
    }
}