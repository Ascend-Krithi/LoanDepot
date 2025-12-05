using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Tests.Reporting;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public class TestHooks
    {
        private static IWebDriver? _driver;
        private static HtmlReportManager? _report;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            _report = new HtmlReportManager();
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenario)
        {
            _driver ??= WebDriverFactory.Create();
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenario)
        {
            var screenshotPath = ScreenshotHelper.Capture(_driver!, scenario.ScenarioInfo.Title);
            _report!.AddResult(scenario.ScenarioInfo.Title, scenario.TestError == null, screenshotPath);
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _report?.Flush();
            _driver?.Quit();
            _driver = null;
        }
    }
}
