using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Locators;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Pages;
using AutomationFramework.Tests.Reporting;
using TechTalk.SpecFlow;
using OpenQA.Selenium;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public class Hooks
    {
        private static IWebDriver _driver;
        private static SelfHealingWebDriver _shDriver;
        private static LocatorRepository _locatorRepo;
        private static LoginPage _loginPage;
        private static DashboardPage _dashboardPage;
        private static PaymentPage _paymentPage;

        [BeforeScenario]
        public void BeforeScenario()
        {
            var browser = ConfigManager.Get("Browser");
            var baseUrl = ConfigManager.Get("BaseUrl");
            _driver = WebDriverFactory.CreateDriver(browser);
            _locatorRepo = new LocatorRepository();
            _shDriver = new SelfHealingWebDriver(_driver, _locatorRepo);

            _loginPage = new LoginPage(_shDriver);
            _dashboardPage = new DashboardPage(_shDriver);
            _paymentPage = new PaymentPage(_shDriver);

            ScenarioContext.Current["LoginPage"] = _loginPage;
            ScenarioContext.Current["DashboardPage"] = _dashboardPage;
            ScenarioContext.Current["PaymentPage"] = _paymentPage;

            _driver.Navigate().GoToUrl(baseUrl);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var scenario = ScenarioContext.Current.ScenarioInfo.Title;
            var status = ScenarioContext.Current.TestError == null ? "Passed" : "Failed";
            string screenshotPath = null;
            if (status == "Failed")
            {
                var screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                screenshotPath = $"Screenshots/{scenario}_{DateTime.Now:yyyyMMddHHmmss}.png";
                screenshot.SaveAsFile(screenshotPath, ScreenshotImageFormat.Png);
            }
            HtmlReportManager.Instance.AddScenarioResult(scenario, status, screenshotPath, ScenarioContext.Current.TestError?.ToString());
            _driver.Quit();
        }
    }
}