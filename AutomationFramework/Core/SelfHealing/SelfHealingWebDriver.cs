using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using AutomationFramework.Core.Locators;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver
    {
        private readonly IWebDriver _driver;
        private readonly ILocatorRepository _locatorRepo;

        public SelfHealingWebDriver(IWebDriver driver, ILocatorRepository locatorRepo)
        {
            _driver = driver;
            _locatorRepo = locatorRepo;
        }

        public IWebElement FindElementByKey(string key)
        {
            var locator = _locatorRepo.GetLocator(key);
            try
            {
                return _driver.FindElement(locator);
            }
            catch (NoSuchElementException)
            {
                var alternatives = _locatorRepo.GetAlternatives(key);
                foreach (var alt in alternatives)
                {
                    try
                    {
                        var element = _driver.FindElement(alt);
                        LogHealingEvent(key, alt);
                        return element;
                    }
                    catch { }
                }
                throw;
            }
        }

        private void LogHealingEvent(string key, By alt)
        {
            Console.WriteLine($"[Self-Healing] Locator healed for '{key}' using alternative: {alt}");
            // Optionally log to file or report
        }

        // IWebDriver implementation (delegated)
        public string Url { get => _driver.Url; set => _driver.Url = value; }
        public string Title => _driver.Title;
        public string PageSource => _driver.PageSource;
        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;
        public void Close() => _driver.Close();
        public void Quit() => _driver.Quit();
        public IOptions Manage() => _driver.Manage();
        public INavigation Navigate() => _driver.Navigate();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();
        public IWebElement FindElement(By by) => _driver.FindElement(by);
        public ReadOnlyCollection<IWebElement> FindElements(By by) => _driver.FindElements(by);
        public void Dispose() => _driver.Dispose();
    }
}