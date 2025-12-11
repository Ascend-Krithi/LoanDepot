using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AutomationFramework.Core.Locators;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver
    {
        private readonly IWebDriver _driver;
        private readonly List<string> _healingLog = new List<string>();

        public SelfHealingWebDriver(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement FindElementByKey(string key, int timeoutSeconds = 10)
        {
            var (primary, alternatives) = LocatorRepository.GetLocators(key);
            try
            {
                return new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds))
                    .Until(drv => drv.FindElement(primary));
            }
            catch (NoSuchElementException)
            {
                foreach (var alt in alternatives)
                {
                    try
                    {
                        var element = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds))
                            .Until(drv => drv.FindElement(alt));
                        LogHealingEvent(key, alt);
                        return element;
                    }
                    catch { }
                }
                throw;
            }
        }

        private void LogHealingEvent(string key, By usedAlternative)
        {
            var msg = $"[Self-Healing] Locator healed for key '{key}' using alternative: {usedAlternative}";
            _healingLog.Add(msg);
            Console.WriteLine(msg);
        }

        public IReadOnlyList<string> GetHealingLog() => _healingLog.AsReadOnly();

        // IWebDriver implementation (delegated)
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