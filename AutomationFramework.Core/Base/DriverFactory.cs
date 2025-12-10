using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;

namespace AutomationFramework.Core.Base
{
    public static class DriverFactory
    {
        [ThreadStatic]
        private static IWebDriver _driver;

        public static IWebDriver GetDriver()
        {
            if (_driver == null)
            {
                var browser = Configuration.ConfigManager.GetBrowser();
                switch (browser.ToLower())
                {
                    case "chrome":
                        _driver = new ChromeDriver();
                        break;
                    case "edge":
                        _driver = new EdgeDriver();
                        break;
                    default:
                        throw new ArgumentException($"Unsupported browser: {browser}");
                }
                _driver.Manage().Window.Maximize();
            }
            return _driver;
        }

        public static void QuitDriver()
        {
            _driver?.Quit();
            _driver = null;
        }
    }
}