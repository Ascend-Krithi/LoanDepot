using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;

namespace AutomationFramework.Core.Drivers
{
    public static class WebDriverFactory
    {
        public static SelfHealingWebDriver CreateDriver()
        {
            var browser = ConfigManager.Settings.Browser;
            IWebDriver driver;

            switch (browser.ToLowerInvariant())
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    // Selenium Manager handles driver download automatically
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
                    throw new NotSupportedException($"Browser '{browser}' is not supported.");
            }

            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            return new SelfHealingWebDriver(driver);
        }
    }
}