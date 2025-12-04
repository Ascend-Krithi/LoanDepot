using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Locators;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Tests.Reporting;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public class Hooks
    {
        public static SelfHealingWebDriver Driver;
        public static Dictionary<string, (By, By[])> LocatorRepo;

        [BeforeScenario]
        public void BeforeScenario()
        {
            var browser = ConfigManager.Get("Browser") ?? "chrome";
            var baseUrl = ConfigManager.Get("BaseUrl");
            var webDriver = WebDriverFactory.CreateDriver(browser);

            LocatorRepo = new()
            {
                { LoginPageLocators.UsernameKey, (LoginPageLocators.UsernameLocator, LoginPageLocators.UsernameAlternatives) },
                { LoginPageLocators.PasswordKey, (LoginPageLocators.PasswordLocator, LoginPageLocators.PasswordAlternatives) },
                { LoginPageLocators.LoginButtonKey, (LoginPageLocators.LoginButtonLocator, LoginPageLocators.LoginButtonAlternatives) },
                { DashboardPageLocators.WelcomeMessageKey, (DashboardPageLocators.WelcomeMessageLocator, DashboardPageLocators.WelcomeMessageAlternatives) },
                { DashboardPageLocators.LoanListDropdownKey, (DashboardPageLocators.LoanListDropdownLocator, DashboardPageLocators.LoanListDropdownAlternatives) },
                { LoanListPageLocators.LoanRowKey, (LoanListPageLocators.LoanRowLocator, LoanListPageLocators.LoanRowAlternatives) },
                { LoanListPageLocators.PopupCloseKey, (LoanListPageLocators.PopupCloseLocator, LoanListPageLocators.PopupCloseAlternatives) }
            };

            Driver = new SelfHealingWebDriver(webDriver, LocatorRepo);
            Driver.Url = baseUrl;
        }

        [AfterScenario]
        public void AfterScenario(ScenarioContext scenarioContext)
        {
            var screenshotPath = string.Empty;
            if (scenarioContext.TestError != null)
            {
                screenshotPath = Core.Utilities.ScreenshotHelper.CaptureScreenshot(Driver, scenarioContext.ScenarioInfo.Title);
            }
            HtmlReportManager.GenerateReport(scenarioContext, screenshotPath, Driver);
            Driver.Quit();
        }
    }
}