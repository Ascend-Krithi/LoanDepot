using OpenQA.Selenium;
using System.Collections.ObjectModel;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver, IJavaScriptExecutor, IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly DomAnalyzer _domAnalyzer;

        public SelfHealingWebDriver(IWebDriver driver)
        {
            _driver = driver;
            _domAnalyzer = new DomAnalyzer(driver);
        }

        // Custom FindElement with self-healing logic
        public IWebElement FindElement(string logicalKey, By locator)
        {
            SelfHealingRepository.StoreLocator(logicalKey, locator);
            try
            {
                return _driver.FindElement(locator);
            }
            catch (NoSuchElementException)
            {
                Logger.Log($"Element '{logicalKey}' with locator '{locator}' not found. Attempting to heal.");
                var healedLocator = _domAnalyzer.AttemptToHeal(locator);

                if (healedLocator != null)
                {
                    Logger.Log($"Healing successful. New locator for '{logicalKey}' is '{healedLocator}'.");
                    SelfHealingRepository.UpdateLocator(logicalKey, healedLocator);
                    try
                    {
                        return _driver.FindElement(healedLocator);
                    }
                    catch (NoSuchElementException ex)
                    {
                        Logger.Log($"Finding element '{logicalKey}' failed even after healing.");
                        throw new NoSuchElementException($"Element with logical key '{logicalKey}' could not be found, even after attempting to heal.", ex);
                    }
                }

                Logger.Log($"Healing failed for element '{logicalKey}'.");
                throw; // Re-throw original exception if healing fails
            }
        }

        #region IWebDriver Implementation (delegating to wrapped driver)

        public IWebElement FindElement(By by)
        {
            // This standard method is discouraged. Use the logical key version for self-healing.
            // It's provided for compatibility with Selenium internals (e.g., WebDriverWait).
            return _driver.FindElement(by);
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by) => _driver.FindElements(by);
        public void Close() => _driver.Close();
        public void Quit() => _driver.Quit();
        public IOptions Manage() => _driver.Manage();
        public INavigation Navigate() => _driver.Navigate();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();
        public string Url { get => _driver.Url; set => _driver.Url = value; }
        public string Title => _driver.Title;
        public string PageSource => _driver.PageSource;
        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;
        public void Dispose()
        {
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IJavaScriptExecutor Implementation

        public object? ExecuteScript(string script, params object[] args) => ((IJavaScriptExecutor)_driver).ExecuteScript(script, args);
        public object? ExecuteAsyncScript(string script, params object[] args) => ((IJavaScriptExecutor)_driver).ExecuteAsyncScript(script, args);

        #endregion
    }
}