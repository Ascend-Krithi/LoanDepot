// AutomationFramework.Core/SelfHealing/SelfHealingWebDriver.cs
using AutomationFramework.Core.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver
    {
        private readonly IWebDriver _innerDriver;
        private readonly SelfHealingRepository _repository;
        private readonly DomAnalyzer _analyzer;

        public SelfHealingWebDriver(IWebDriver innerDriver)
        {
            _innerDriver = innerDriver ?? throw new ArgumentNullException(nameof(innerDriver));
            _repository = new SelfHealingRepository();
            _analyzer = new DomAnalyzer();
        }

        public IWebDriver InnerDriver => _innerDriver;

        // Core self-healing method
        public IWebElement FindElement(string logicalKey, By locator, int timeoutSeconds = 10)
        {
            _repository.AddOrUpdate(logicalKey, new LocatorSnapshot(logicalKey, locator));

            var element = WaitHelper.WaitForElement(_innerDriver, locator, timeoutSeconds);
            if (element != null)
            {
                return element;
            }

            // Initial find failed, attempt to heal
            Logger.Log($"Self-healing: Element '{logicalKey}' with locator '{locator}' not found. Attempting to heal.");
            var healedLocator = _analyzer.Heal(locator);

            if (!healedLocator.Equals(locator))
            {
                Logger.Log($"Self-healing: DOM Analyzer suggested a new locator: '{healedLocator}'. Retrying.");
                element = WaitHelper.WaitForElement(_innerDriver, healedLocator, timeoutSeconds);
                if (element != null)
                {
                    _repository.AddOrUpdate(logicalKey, new LocatorSnapshot(logicalKey, healedLocator)); // Update repository with healed locator
                    return element;
                }
            }

            throw new NoSuchElementException($"Element with logical key '{logicalKey}' could not be found using locator '{locator}' or any healed alternatives.");
        }

        #region IWebDriver Implementation (Delegation)

        public string Url
        {
            get => _innerDriver.Url;
            set => _innerDriver.Url = value;
        }

        public string Title => _innerDriver.Title;
        public string PageSource => _innerDriver.PageSource;
        public string CurrentWindowHandle => _innerDriver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _innerDriver.WindowHandles;

        public void Close() => _innerDriver.Close();
        public void Quit() => _innerDriver.Quit();
        public IOptions Manage() => _innerDriver.Manage();
        public INavigation Navigate() => _innerDriver.Navigate();
        public ITargetLocator SwitchTo() => _innerDriver.SwitchTo();

        // These FindElement methods delegate to the inner driver directly,
        // as they lack the 'logicalKey' needed for self-healing.
        // The framework mandates using the custom FindElement method.
        public IWebElement FindElement(By by) => _innerDriver.FindElement(by);
        public ReadOnlyCollection<IWebElement> FindElements(By by) => _innerDriver.FindElements(by);

        public void Dispose()
        {
            try
            {
                _innerDriver.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Log($"Error during driver disposal: {ex.Message}");
            }
        }

        #endregion
    }
}