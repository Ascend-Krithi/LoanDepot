using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver
    {
        private readonly IWebDriver driver;

        public SelfHealingWebDriver(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement FindElement(By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                // Fallback logic can be implemented here
                throw;
            }
        }

        // Implement other IWebDriver members by delegating to 'driver'
        public string Url { get => driver.Url; set => driver.Url = value; }
        public string Title => driver.Title;
        public string PageSource => driver.PageSource;
        public string CurrentWindowHandle => driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => driver.WindowHandles;

        public void Close() => driver.Close();
        public void Dispose() => driver.Dispose();
        public IWebElement FindElement(By by, int timeoutInSeconds)
        {
            // Optionally implement wait logic here
            return FindElement(by);
        }
        public ReadOnlyCollection<IWebElement> FindElements(By by) => driver.FindElements(by);
        public IOptions Manage() => driver.Manage();
        public INavigation Navigate() => driver.Navigate();
        public void Quit() => driver.Quit();
        public ITargetLocator SwitchTo() => driver.SwitchTo();
    }
}