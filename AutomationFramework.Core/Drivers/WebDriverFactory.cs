// AutomationFramework.Core/Drivers/WebDriverFactory.cs
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
            IWebDriver driver;
            string browser = ConfigManager.Settings.Browser.ToLowerInvariant();

            switch (browser)
            {
                case "chrome":
                    driver = new ChromeDriver();
                    break;
                case "firefox":
                    driver = new FirefoxDriver();
                    break;
                case "edge":
                    driver = new EdgeDriver();
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