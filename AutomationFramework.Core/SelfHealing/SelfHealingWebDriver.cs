using System;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver
    {
        private readonly IWebDriver _innerDriver;
        private readonly SelfHealingRepository _repository;
        private readonly DomAnalyzer _analyzer;

        public IWebDriver InnerDriver => _innerDriver;

        public SelfHealingWebDriver(IWebDriver driver)
        {
            _innerDriver = driver ?? throw new ArgumentNullException(nameof(driver));
            _repository = new SelfHealingRepository();
            _analyzer = new DomAnalyzer();
        }

        public IWebElement FindElement(string logicalKey, By locator, int timeoutSeconds = 10)
        {
            _repository.AddOrUpdate(logicalKey, new LocatorSnapshot(logicalKey, locator));
            var element = WaitHelper.WaitForElement(_innerDriver, locator, timeoutSeconds);
            if (element != null)
                return element;

            Utilities.Logger.Log($"Element not found: {logicalKey} ({locator}) - attempting self-heal.");
            var healedLocator = _analyzer.Heal(locator);
            if (!healedLocator.Equals(locator))
            {
                element = WaitHelper.WaitForElement(_innerDriver, healedLocator, timeoutSeconds);
                if (element != null)
                    return element;
            }

            throw new NoSuchElementException($"Element not found for logical key '{logicalKey}' using locator '{locator}'.");
        }

        #region IWebDriver Implementation (delegation)

        public string Url { get => _innerDriver.Url; set => _innerDriver.Url = value; }
        public string Title => _innerDriver.Title;
        public string PageSource => _innerDriver.PageSource;
        public string CurrentWindowHandle => _innerDriver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _innerDriver.WindowHandles;

        public void Close() => _innerDriver.Close();
        public void Dispose()
        {
            try { _innerDriver.Quit(); }
            catch { /* swallow */ }
        }
        public IWebElement FindElement(By by) => _innerDriver.FindElement(by);
        public ReadOnlyCollection<IWebElement> FindElements(By by) => _innerDriver.FindElements(by);
        public IOptions Manage() => _innerDriver.Manage();
        public INavigation Navigate() => _innerDriver.Navigate();
        public void Quit()
        {
            try { _innerDriver.Quit(); }
            catch { /* swallow */ }
        }
        public ITargetLocator SwitchTo() => _innerDriver.SwitchTo();

        #endregion
    }
}