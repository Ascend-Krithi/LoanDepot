using TechTalk.SpecFlow;
using AutomationFramework.Core.Base;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Utilities;
using OpenQA.Selenium;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public class Hooks
    {
        public static IWebDriver WebDriver;
        public static SelfHealingWebDriver Driver;

        [BeforeScenario]
        public void BeforeScenario()
        {
            var browser = EncryptionService.Decrypt(ConfigManager.Get("Browser"));
            WebDriver = DriverFactory.CreateDriver(browser);
            // Load locators for all pages (pseudo-code, should be loaded from JSONs)
            var locators = new Dictionary<string, string>(); // Populate from locator JSONs
            Driver = new SelfHealingWebDriver(WebDriver, locators);
            // Decrypt credentials and load test data as needed
        }

        [AfterScenario]
        public void AfterScenario()
        {
            WebDriver.Quit();
            // Flush reports if needed
        }
    }
}