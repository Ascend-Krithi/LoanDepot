using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver
    {
        private readonly IWebDriver _driver;
        private readonly Dictionary<string, (By, By[])> _locatorRepo;
        public SelfHealingWebDriver(IWebDriver driver, Dictionary<string, (By, By[])> locatorRepo)
        {
            _driver = driver;
            _locatorRepo = locatorRepo;
        }

        public IWebElement FindElementByKey(string key)
        {
            if (!_locatorRepo.ContainsKey(key))
                throw new ArgumentException($"Locator key not found: {key}");

            var (primary, alternatives) = _locatorRepo[key];
            try
            {
                return _driver.FindElement(primary);
            }
            catch
            {
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

        public ReadOnlyCollection<IWebElement> FindElements(By by) => _driver.FindElements(by);

        private void LogHealingEvent(string key, By alt)
        {
            Console.WriteLine($"[Self-Healing] Healed locator for {key} using alternative: {alt}");
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
        public IOptions Manage() => _driver.Manage();
        public INavigation Navigate() => _driver.Navigate();
        public void Quit() => _driver.Quit();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();
    }
}