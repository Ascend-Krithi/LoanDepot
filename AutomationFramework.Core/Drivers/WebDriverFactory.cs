using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Drivers
{
    public static class WebDriverFactory
    {
        public static SelfHealingWebDriver CreateDriver()
        {
            IWebDriver driver;
            string browser = ConfigManager.Settings.Browser?.ToLowerInvariant() ?? "chrome";
            switch (browser)
            {
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                case "chrome":
                default:
                    driver = new ChromeDriver();
                    break;
            }
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2);
            return new SelfHealingWebDriver(driver);
        }
    }
}