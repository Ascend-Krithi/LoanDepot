using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;
using System;
using AutomationFramework.Tests.Reporting;
using System.IO;

namespace AutomationFramework.Tests.TestPages
{
    [TestClass]
    public class BaseTest
    {
        protected SelfHealingWebDriver Driver;
        protected HtmlReportManager Report;

        [TestInitialize]
        public void TestInitialize()
        {
            var config = ConfigManager.Settings;
            var browser = config.Browser;
            var environment = config.BaseUrl;
            Report = new HtmlReportManager(TestContext.TestName, browser, environment);

            var webDriver = DriverFactory.CreateDriver(browser);
            Driver = new SelfHealingWebDriver(webDriver);
            Driver.Url = config.BaseUrl;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            bool passed = TestContext.CurrentTestOutcome == UnitTestOutcome.Passed;
            string error = null, stackTrace = null, screenshotPath = null;

            if (!passed)
            {
                try
                {
                    var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                    screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), $"{TestContext.TestName}_fail.png");
                    screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
                }
                catch { }
                error = TestContext.Properties.Contains("Error") ? TestContext.Properties["Error"].ToString() : "";
                stackTrace = TestContext.Properties.Contains("StackTrace") ? TestContext.Properties["StackTrace"].ToString() : "";
            }

            Report.MarkResult(passed, error, stackTrace, screenshotPath);
            Report.GenerateReport();

            Driver.Quit();
        }

        public TestContext TestContext { get; set; }
    }
}