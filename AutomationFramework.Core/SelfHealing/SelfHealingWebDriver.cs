using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver
    {
        private readonly IWebDriver _driver;

        public SelfHealingWebDriver(IWebDriver driver)
        {
            _driver = driver;
        }

        // Example usage: Try best_locator, then fallback_locators
        public IWebElement FindElementWithFallback(By bestLocator, List<By> fallbackLocators)
        {
            try
            {
                return _driver.FindElement(bestLocator);
            }
            catch
            {
                foreach (var locator in fallbackLocators)
                {
                    try
                    {
                        return _driver.FindElement(locator);
                    }
                    catch { }
                }
                throw;
            }
        }

        // Implement all IWebDriver members by delegating to _driver
        public string Url { get => _driver.Url; set => _driver.Url = value; }
        public string Title => _driver.Title;
        public string PageSource => _driver.PageSource;
        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;
        public void Close() => _driver.Close();
        public void Dispose() => _driver.Dispose();
        public IWebElement FindElement(By by) => _driver.FindElement(by);
        public ReadOnlyCollection<IWebElement> FindElements(By by) => _driver.FindElements(by);
        public IOptions Manage() => _driver.Manage();
        public INavigation Navigate() => _driver.Navigate();
        public void Quit() => _driver.Quit();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();
    }
}