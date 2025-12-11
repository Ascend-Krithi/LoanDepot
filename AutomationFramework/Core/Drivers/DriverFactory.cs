using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;

namespace AutomationFramework.Core.Drivers
{
    public static class DriverFactory
    {
        public static IWebDriver CreateDriver(string browser)
        {
            switch (browser.ToLower())
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--start-maximized");
                    return new ChromeDriver(chromeOptions);
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    return new FirefoxDriver(firefoxOptions);
                default:
                    throw new ArgumentException($"Unsupported browser: {browser}");
            }
        }
    }
}