using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Encryption;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using TechTalk.SpecFlow;
using Tests.Reporting;

namespace Tests.Hooks
{
    [Binding]
    public class Hooks
    {
        private static SelfHealingWebDriver _driver;
        private static Stopwatch _scenarioTimer;
        private static string _stepsLog;
        private static string _screenshotPath;
        private static string _error;
        private static bool _passed = true;

        [BeforeScenario]
        public void BeforeScenario()
        {
            _scenarioTimer = Stopwatch.StartNew();
            _stepsLog = "";
            _error = "";
            _passed = true;

            var config = ConfigManager.GetConfig();
            var browser = config.Browser;
            var driver = WebDriverFactory.CreateDriver(browser);
            _driver = new SelfHealingWebDriver(driver);
            ScenarioContext.Current["DRIVER"] = _driver;

            var baseUrl = config.BaseUrl;
            _driver.Navigate().GoToUrl(baseUrl);
        }

        [AfterStep]
        public void AfterStep()
        {
            var stepInfo = ScenarioStepContext.Current.StepInfo;
            _stepsLog += $"{stepInfo.StepDefinitionType} {stepInfo.Text}\n";
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _scenarioTimer.Stop();
            var scenario = ScenarioContext.Current.ScenarioInfo.Title;
            var config = ConfigManager.GetConfig();
            var browser = config.Browser;
            var environment = config.BaseUrl;

            if (ScenarioContext.Current.TestError != null)
            {
                _passed = false;
                _error = ScenarioContext.Current.TestError.ToString();
                _screenshotPath = ScreenshotHelper.CaptureScreenshot(_driver, scenario);
            }

            HtmlReportManager.GenerateReport(
                scenario,
                _stepsLog,
                _passed,
                _error,
                _screenshotPath,
                environment,
                browser,
                _scenarioTimer.Elapsed
            );

            _driver.Quit();
        }
    }
}