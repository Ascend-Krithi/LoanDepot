using BoDi;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using WebAutomation.Core.Drivers;
using WebAutomation.Core.Configuration;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Hooks
{
    [Binding]
    public sealed class TestHooks
    {
        private readonly IObjectContainer _container;

        public TestHooks(IObjectContainer container)
        {
            _container = container;
        }

        [BeforeScenario(Order = 0)]
        public void BeforeScenario()
        {
            // Create and register the WebDriver instance
            var driver = WebDriverFactory.Create();
            _container.RegisterInstanceAs<IWebDriver>(driver);

            // Navigate to base URL
            driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            driver.Manage().Window.Maximize();
        }

        [BeforeScenario(Order = 1)]
        public void RegisterDependencies()
        {
            // Register LocatorRepository for DI into Step Definition classes
            // Assuming a single locators file for simplicity. This can be expanded.
            var locatorRepo = new LocatorRepository("locators.json");
            _container.RegisterInstanceAs(locatorRepo);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var driver = _container.Resolve<IWebDriver>();

            // TODO: Add logic for taking screenshots on failure

            driver?.Quit();
            driver?.Dispose();
        }
    }
}