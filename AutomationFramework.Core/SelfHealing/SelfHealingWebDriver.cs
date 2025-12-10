using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver
    {
        private readonly IWebDriver _driver;
        private readonly Dictionary<string, (string bestLocator, List<string> fallbackLocators)> _locators;

        public SelfHealingWebDriver(IWebDriver driver, Dictionary<string, (string, List<string>)> locators)
        {
            _driver = driver;
            _locators = locators;
        }

        public IWebElement FindElement(string alias)
        {
            var locator = _locators[alias].bestLocator;
            try
            {
                return _driver.FindElement(By.XPath(locator));
            }
            catch
            {
                foreach (var fallback in _locators[alias].fallbackLocators)
                {
                    try
                    {
                        return _driver.FindElement(By.XPath(fallback));
                    }
                    catch { }
                }
                throw;
            }
        }

        public bool ElementExists(string alias)
        {
            try
            {
                FindElement(alias);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}