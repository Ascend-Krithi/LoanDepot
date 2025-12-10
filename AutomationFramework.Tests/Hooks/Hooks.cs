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
        public static SelfHealingWebDriver Driver;
        private static Dictionary<string, (string, List<string>)> _locators;

        [BeforeScenario]
        public void BeforeScenario()
        {
            var webDriver = DriverFactory.CreateDriver();
            // Load locators from JSON (assume loaded into _locators)
            Driver = new SelfHealingWebDriver(webDriver, _locators);

            // Load config and decrypt credentials
            var username = ConfigManager.Get("Username");
            var password = ConfigManager.Get("Password");

            // Load test data for current scenario (handled in StepDefinitions)
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Driver?.Dispose();
            // Flush reports if needed
        }
    }
}