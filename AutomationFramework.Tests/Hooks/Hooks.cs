using TechTalk.SpecFlow;
using OpenQA.Selenium;
using AutomationFramework.Core.Base;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public class Hooks
    {
        public static IWebDriver Driver { get; private set; }

        [BeforeScenario]
        public void BeforeScenario()
        {
            Driver = DriverFactory.CreateDriver(ConfigManager.Get("headless") == "true");
            Driver.Navigate().GoToUrl(ConfigManager.Get("QA:baseUrl"));
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Driver?.Quit();
        }
    }
}