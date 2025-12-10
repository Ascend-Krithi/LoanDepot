using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver
    {
        private readonly IWebDriver _driver;
        private readonly Dictionary<string, string> _locators;
        private readonly Dictionary<string, List<string>> _fallbackLocators;

        public SelfHealingWebDriver(IWebDriver driver, Dictionary<string, string> locators, Dictionary<string, List<string>> fallbackLocators = null)
        {
            _driver = driver;
            _locators = locators;
            _fallbackLocators = fallbackLocators ?? new Dictionary<string, List<string>>();
        }

        public void Click(string alias)
        {
            FindElement(alias).Click();
        }

        public void ClickDynamic(string alias, string dynamicValue)
        {
            var locator = _locators[alias].Replace("${CaseId}", dynamicValue).Replace("${CaseNumber}", dynamicValue);
            _driver.FindElement(By.CssSelector(locator)).Click();
        }

        public void SendKeys(string alias, string value)
        {
            var element = FindElement(alias);
            element.Clear();
            element.SendKeys(value);
        }

        public void SelectDropdownByText(string alias, string text)
        {
            var element = FindElement(alias);
            var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(element);
            selectElement.SelectByText(text);
        }

        public string GetText(string alias)
        {
            return FindElement(alias).Text;
        }

        public void WaitForElementToDisappear(string alias)
        {
            // Implement wait logic as needed
        }

        public bool ElementExists(string locator)
        {
            try
            {
                _driver.FindElement(By.CssSelector(locator));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SwitchToFrame(string locator)
        {
            _driver.SwitchTo().Frame(_driver.FindElement(By.CssSelector(locator)));
        }

        public void SwitchToDefaultContent()
        {
            _driver.SwitchTo().DefaultContent();
        }

        private IWebElement FindElement(string alias)
        {
            try
            {
                return _driver.FindElement(By.CssSelector(_locators[alias]));
            }
            catch
            {
                if (_fallbackLocators.ContainsKey(alias))
                {
                    foreach (var fallback in _fallbackLocators[alias])
                    {
                        try
                        {
                            return _driver.FindElement(By.CssSelector(fallback));
                        }
                        catch { }
                    }
                }
                throw;
            }
        }
    }
}