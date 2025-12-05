using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System.IO;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Core.Drivers
{
    public static class WebDriverFactory
    {
        public static IWebDriver CreateWebDriver()
        {
            var browser = ConfigManager.Get("Browser", "Chrome");
            var headless = ConfigManager.Get("Headless", "false").Equals("true", StringComparison.OrdinalIgnoreCase);

            IWebDriver driver;
            switch (browser.ToLower())
            {
                case "edge":
                    var edgeOptions = new EdgeOptions();
                    if (headless) edgeOptions.AddArgument("--headless=new");
                    edgeOptions.AddArgument("--start-maximized");
                    driver = new EdgeDriver(edgeOptions);
                    break;
                case "firefox":
                    var ffOptions = new FirefoxOptions();
                    if (headless) ffOptions.AddArgument("-headless");
                    driver = new FirefoxDriver(ffOptions);
                    break;
                case "chrome":
                default:
                    var chromeOptions = new ChromeOptions();
                    if (headless) chromeOptions.AddArgument("--headless=new");
                    chromeOptions.AddArgument("--start-maximized");
                    chromeOptions.AddArgument("--disable-gpu");
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddArgument("--disable-dev-shm-usage");
                    driver = new ChromeDriver(chromeOptions);
                    break;
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            return driver;
        }
    }
}