using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Drivers
{
    public static class WebDriverFactory
    {
        public static SelfHealingWebDriver CreateDriver()
        {
            string browser = ConfigManager.Settings.Browser?.ToLowerInvariant() ?? "chrome";
            IWebDriver driver;

            switch (browser)
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    driver = new ChromeDriver(chromeOptions);
                    break;
                case "edge":
                    var edgeOptions = new EdgeOptions();
                    driver = new EdgeDriver(edgeOptions);
                    break;
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    driver = new FirefoxDriver(firefoxOptions);
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