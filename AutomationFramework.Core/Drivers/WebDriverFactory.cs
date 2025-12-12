using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Drivers
{
    public static class WebDriverFactory
    {
        public static SelfHealingWebDriver CreateDriver()
        {
            var browser = ConfigManager.Settings.Browser?.ToLowerInvariant() ?? "chrome";
            IWebDriver driver;
            switch (browser)
            {
                case "chrome":
                    driver = new ChromeDriver();
                    break;
                case "edge":
                    driver = new EdgeDriver();
                    break;
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                default:
                    throw new ArgumentException($"Unsupported browser: {browser}");
            }

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            return new SelfHealingWebDriver(driver);
        }
    }
}