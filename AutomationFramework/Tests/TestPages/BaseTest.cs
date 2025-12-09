using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Encryption;
using AutomationFramework.Core.Locators;
using AutomationFramework.Core.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;

namespace AutomationFramework.Tests.TestPages
{
    [TestClass]
    public abstract class BaseTest
    {
        protected SelfHealingWebDriver Driver;
        protected Dictionary<string, By[]> LocatorRepository;
        protected string TestName;

        [TestInitialize]
        public void TestInitialize()
        {
            var config = ConfigManager.Settings;
            TestName = TestContext.TestName;
            // Merge all locator repositories
            LocatorRepository = new Dictionary<string, By[]>();
            foreach (var kv in LoginPageLocators.GetLocators())
                LocatorRepository[kv.Key] = kv.Value;
            foreach (var kv in DashboardPageLocators.GetLocators())
                LocatorRepository[kv.Key] = kv.Value;
            foreach (var kv in LoanListPageLocators.GetLocators())
                LocatorRepository[kv.Key] = kv.Value;

            var driver = DriverFactory.CreateDriver(config.Browser);
            Driver = new SelfHealingWebDriver(driver, LocatorRepository);

            HtmlReportManager.StartScenario(TestName, config.Browser, config.TestEnvironment);

            Driver.Navigate().GoToUrl(config.BaseUrl);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            try
            {
                if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
                {
                    var screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                    var reportDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tests", "Reporting");
                    Directory.CreateDirectory(reportDir);
                    var screenshotPath = Path.Combine(reportDir, $"{TestName}_{DateTime.Now:yyyyMMdd_HHmmss}.png");
                    screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
                    HtmlReportManager.MarkFailed(TestContext.TestResultsDirectory, TestContext.CurrentTestOutcome.ToString(), screenshotPath);
                }
            }
            catch { }
            finally
            {
                HtmlReportManager.SaveReport();
                Driver.Quit();
            }
        }

        public TestContext TestContext { get; set; }
    }
}