using AutomationFramework.Core.Base;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.Widgets;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public class Hooks
    {
        private IWebDriver _driver;
        private SelfHealingWebDriver _selfHealingDriver;
        private TestDataReader _testDataReader;
        private UniversalPopupHandler _popupHandler;

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            _driver = DriverFactory.CreateDriver();
            _selfHealingDriver = new SelfHealingWebDriver(_driver);

            // Decrypt credentials
            var username = EncryptionService.Decrypt(ConfigManager.Get("Username"));
            var password = EncryptionService.Decrypt(ConfigManager.Get("Password"));
            var mfa = EncryptionService.Decrypt(ConfigManager.Get("Mfa"));

            // Load test data
            _testDataReader = new TestDataReader("TestData/testdata.xlsx");

            // Popup handler
            _popupHandler = new UniversalPopupHandler(_selfHealingDriver);

            scenarioContext["Driver"] = _selfHealingDriver;
            scenarioContext["TestDataReader"] = _testDataReader;
            scenarioContext["PopupHandler"] = _popupHandler;
            scenarioContext["Username"] = username;
            scenarioContext["Password"] = password;
            scenarioContext["Mfa"] = mfa;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _driver?.Quit();
        }
    }
}