using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AutomationFramework.Core
{
    public sealed class DriverManager
    {
        private static readonly Lazy<DriverManager> lazy = new Lazy<DriverManager>(() => new DriverManager());
        public static DriverManager Instance => lazy.Value;

        private IWebDriver _driver;
        public IWebDriver Driver
        {
            get
            {
                if (_driver == null)
                {
                    _driver = new ChromeDriver();
                }
                return _driver;
            }
        }

        private DriverManager()
        {
            // Private constructor for singleton
        }

        public void QuitDriver()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver = null;
            }
        }
    }
}