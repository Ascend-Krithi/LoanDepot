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
                    var options = new ChromeOptions();
                    options.AddArgument("--start-maximized");
                    return new ChromeDriver(options);
                case "firefox":
                    var ffOptions = new FirefoxOptions();
                    ffOptions.AddArgument("--start-maximized");
                    return new FirefoxDriver(ffOptions);
                default:
                    throw new ArgumentException($"Unsupported browser: {browser}");
            }
        }
    }
}