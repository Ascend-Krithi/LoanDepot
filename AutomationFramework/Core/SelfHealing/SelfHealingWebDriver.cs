using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public SelfHealingWebDriver(IWebDriver driver, TimeSpan? timeout = null)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, timeout ?? TimeSpan.FromSeconds(20));
        }

        public IWebElement Find(By? idSelector = null, By? xpathSelector = null, By? cssSelector = null)
        {
            IWebElement? element = null;
            Exception? lastEx = null;
            foreach (var by in new By?[] { idSelector, xpathSelector, cssSelector })
            {
                if (by == null) continue;
                try
                {
                    element = _wait.Until(ExpectedConditions.ElementExists(by));
                    if (element != null) return element;
                }
                catch (Exception ex)
                {
                    lastEx = ex;
                }
            }
            throw new NoSuchElementException($"Element not found by any selector. Last error: {lastEx?.Message}");
        }

        public void Click(By? idSelector = null, By? xpathSelector = null, By? cssSelector = null)
        {
            var el = Find(idSelector, xpathSelector, cssSelector);
            _wait.Until(ExpectedConditions.ElementToBeClickable(el));
            el.Click();
        }

        public void Type(string text, By? idSelector = null, By? xpathSelector = null, By? cssSelector = null)
        {
            var el = Find(idSelector, xpathSelector, cssSelector);
            el.Clear();
            el.SendKeys(text);
        }

        public bool IsVisible(By? idSelector = null, By? xpathSelector = null, By? cssSelector = null)
        {
            try
            {
                var el = Find(idSelector, xpathSelector, cssSelector);
                return el.Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}
