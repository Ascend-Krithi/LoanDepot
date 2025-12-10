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

        // Implement all IWebDriver members, wrapping FindElement with self-healing logic
        public IWebElement FindElement(By by)
        {
            try
            {
                return _driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                // Fallback logic can be implemented here
                throw;
            }
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by) => _driver.FindElements(by);
        public void Close() => _driver.Close();
        public void Dispose() => _driver.Dispose();
        public object ExecuteScript(string script, params object[] args) => ((IJavaScriptExecutor)_driver).ExecuteScript(script, args);
        public object ExecuteAsyncScript(string script, params object[] args) => ((IJavaScriptExecutor)_driver).ExecuteAsyncScript(script, args);
        public IOptions Manage() => _driver.Manage();
        public INavigation Navigate() => _driver.Navigate();
        public void Quit() => _driver.Quit();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();
        public string Url { get => _driver.Url; set => _driver.Url = value; }
        public string Title => _driver.Title;
        public string PageSource => _driver.PageSource;
        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;
    }
}