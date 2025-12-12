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
            {
                return element;
            }

            // Initial attempt failed, begin self-healing
            Logger.Log($"Self-healing: Initial attempt failed for '{logicalKey}' with locator '{locator}'.");

            var healedLocator = _analyzer.Heal(locator);

            if (!healedLocator.Equals(locator))
            {
                Logger.Log($"Self-healing: DOM Analyzer suggested a new locator: '{healedLocator}'.");
                element = WaitHelper.WaitForElement(_innerDriver, healedLocator, timeoutSeconds);

                if (element != null)
                {
                    // Update repository with the successful healed locator
                    _repository.AddOrUpdate(logicalKey, new LocatorSnapshot(logicalKey, healedLocator));
                    Logger.Log($"Self-healing: Successfully found element '{logicalKey}' with the new locator.");
                    return element;
                }
            }

            // If healing fails or provides no new locator, throw
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

        public IWebElement FindElement(By by)
        {
            // This is the standard IWebDriver method. We delegate but could add healing here too.
            // For this framework, we enforce using the explicit healing method.
            return _innerDriver.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return _innerDriver.FindElements(by);
        }

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