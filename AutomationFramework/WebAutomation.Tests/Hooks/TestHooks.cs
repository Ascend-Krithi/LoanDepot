using TechTalk.SpecFlow;
using OpenQA.Selenium;
using WebAutomation.Core.Drivers;
using WebAutomation.Core.Utilities;

namespace WebAutomation.Tests.Hooks
{
    [Binding]
    public class TestHooks
    {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver? _driver;

        public TestHooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _driver = WebDriverFactory.Create();

            var wait = new SmartWait(_driver);
            var popup = new PopupHandler(_driver, wait);

            _scenarioContext.Set<IWebDriver>(_driver, "driver");
            _scenarioContext.Set(wait, "wait");
            _scenarioContext.Set(popup, "popup");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _driver?.Quit();
        }
    }
}