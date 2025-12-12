using System;
using System.IO;
using BoDi;
using TechTalk.SpecFlow;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Tests.Reporting;
using OpenQA.Selenium;

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
            var _ = ConfigManager.Settings;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var driver = WebDriverFactory.CreateDriver();
            _container.RegisterInstanceAs<IWebDriver>(driver.InnerDriver);
            _container.RegisterInstanceAs(driver);

            var baseUrl = ConfigManager.Settings.BaseUrl;
            if (!string.IsNullOrEmpty(baseUrl))
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
            string errorMsg = failed ? _scenarioContext.TestError?.Message : null;

            if (failed)
            {
                try
                {
                    var screenshot = ((ITakesScreenshot)driver.InnerDriver).GetScreenshot();
                    var screenshotsDir = Path.Combine(AppContext.BaseDirectory, "Screenshots");
                    Directory.CreateDirectory(screenshotsDir);
                    var filePath = Path.Combine(screenshotsDir, $"{scenarioName}_{DateTime.Now:yyyyMMddHHmmss}.png");
                    screenshot.SaveAsFile(filePath, ScreenshotImageFormat.Png);
                }
                catch { }
            }

            HtmlReportGenerator.GenerateReport(featureName, scenarioName, !failed, errorMsg);

            try { driver.Quit(); } catch { }
        }
    }
}