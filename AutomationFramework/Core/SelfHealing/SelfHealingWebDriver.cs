using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public SelfHealingWebDriver(IWebDriver driver, TimeSpan timeout)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, timeout);
        }

        public IWebElement FindElementWithFallback(IEnumerable<By> strategies)
        {
            foreach (var by in strategies)
            {
                try
                {
                    return _wait.Until(ExpectedConditions.ElementExists(by));
                }
                catch { }
            }
            throw new NoSuchElementException("Element not found with provided strategies.");
        }

        public IReadOnlyCollection<IWebElement> FindElementsWithFallback(IEnumerable<By> strategies)
        {
            foreach (var by in strategies)
            {
                var elements = _driver.FindElements(by);
                if (elements != null && elements.Count > 0) return elements;
            }
            return Array.Empty<IWebElement>();
        }

        public IWebDriver RawDriver => _driver;
    }
}