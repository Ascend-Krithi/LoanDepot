using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using AutomationFramework.Core.Locators;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.Drivers
{
    public class SelfHealingWebDriver : IWebDriver
    {
        private readonly IWebDriver _driver;
        private readonly Dictionary<string, By[]> _locatorRepository;

        public SelfHealingWebDriver(IWebDriver driver, Dictionary<string, By[]> locatorRepository)
        {
            _driver = driver;
            _locatorRepository = locatorRepository;
        }

        public IWebElement FindElement(string locatorKey)
        {
            if (!_locatorRepository.ContainsKey(locatorKey))
                throw new NoSuchElementException($"Locator key '{locatorKey}' not found in repository.");

            var alternatives = _locatorRepository[locatorKey];
            Exception lastException = null;
            foreach (var by in alternatives)
            {
                try
                {
                    var element = _driver.FindElement(by);
                    HtmlReportManager.LogHealingEvent(locatorKey, by.ToString(), true, null);
                    return element;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    HtmlReportManager.LogHealingEvent(locatorKey, by.ToString(), false, ex.Message);
                }
            }
            throw new NoSuchElementException($"Element not found for locator key '{locatorKey}'.", lastException);
        }

        public IReadOnlyCollection<IWebElement> FindElements(string locatorKey)
        {
            if (!_locatorRepository.ContainsKey(locatorKey))
                throw new NoSuchElementException($"Locator key '{locatorKey}' not found in repository.");

            var alternatives = _locatorRepository[locatorKey];
            foreach (var by in alternatives)
            {
                try
                {
                    var elements = _driver.FindElements(by);
                    if (elements.Any())
                    {
                        HtmlReportManager.LogHealingEvent(locatorKey, by.ToString(), true, null);
                        return elements;
                    }
                }
                catch (Exception ex)
                {
                    HtmlReportManager.LogHealingEvent(locatorKey, by.ToString(), false, ex.Message);
                }
            }
            return new List<IWebElement>();
        }

        // IWebDriver implementation
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