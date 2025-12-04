using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;

namespace AutomationFramework.Core.Drivers
{
    public static class WebDriverFactory
    {
        public static IWebDriver CreateDriver(string browser)
        {
            switch (browser.ToLower())
            {
                case "chrome":
                    return new ChromeDriver();
                case "firefox":
                    return new FirefoxDriver();
                default:
                    throw new ArgumentException($"Unsupported browser: {browser}");
            }
        }
    }
}