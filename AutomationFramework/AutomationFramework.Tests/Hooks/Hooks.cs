using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public class Hooks
    {
        private static IWebDriver _driver;
        private static SelfHealingRepository _repository;
        private static SelfHealingWebDriver _selfHealingDriver;

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ConfigManager.LoadSettings();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _repository = new SelfHealingRepository();
            _driver = WebDriverFactory.CreateWebDriver();
            _selfHealingDriver = new SelfHealingWebDriver(_driver, _repository);
            ScenarioContext.Current["WebDriver"] = _selfHealingDriver;
            ScenarioContext.Current["SelfHealingRepository"] = _repository;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                _selfHealingDriver?.Quit();
            }
            catch (Exception ex)
            {
                Logger.LogError("Error quitting driver", ex);
            }
        }
    }
}