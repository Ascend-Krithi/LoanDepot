using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System.Collections.ObjectModel;
using System.Linq;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver, IWrapsDriver, IJavaScriptExecutor
    {
        private readonly IWebDriver _driver;
        private readonly SelfHealingRepository _repository;
        private readonly DomAnalyzer _analyzer;

        public SelfHealingWebDriver(IWebDriver driver, SelfHealingRepository repository)
        {
            _driver = driver;
            _repository = repository;
            _analyzer = new DomAnalyzer(driver); // The real driver is passed to the analyzer
        }

        public IWebDriver WrappedDriver => _driver;

        public string Url
        {
            get => _driver.Url;
            set => _driver.Url = value;
        }

        public string Title => _driver.Title;
        public string PageSource => _driver.PageSource;
        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;

        public void Close() => _driver.Close();
        public void Quit()
        {
            _driver.Quit();
            _repository.SaveRepository();
        }

        public IOptions Manage() => _driver.Manage();
        public INavigation Navigate() => _driver.Navigate();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();

        public IWebElement FindElement(By by)
        {
            try
            {
                return _driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                Logger.Log($"Locator '{by}' failed. Attempting self-healing.");
                _repository.RecordFailure(by);
                var snapshot = _repository.Get(by);

                // Try healed locator first if it exists
                if (!string.IsNullOrEmpty(snapshot?.HealedLocator))
                {
                    try
                    {
                        var healedBy = ConvertStringToBy(snapshot.HealedLocator);
                        Logger.Log($"Trying previously healed locator: '{healedBy}'");
                        return _driver.FindElement(healedBy);
                    }
                    catch (NoSuchElementException)
                    {
                        Logger.Log($"Healed locator '{snapshot.HealedLocator}' also failed.");
                    }
                }

                // If no healed locator or it failed, try to find a new one
                // This part is complex. For a robust solution, we'd need a reference to the 'intended' element.
                // Since we can't get the stale element, we'll search the DOM for candidates.
                // This is a simplified approach. A better one would involve capturing element snapshots before interaction.
                // For this implementation, we will log the failure and re-throw.
                // A more advanced implementation would be needed for true on-the-fly healing without a reference element.
                Logger.Log("Could not find a reference element to analyze for healing. Re-throwing original exception.");
                throw;
            }
            catch (StaleElementReferenceException)
            {
                Logger.Log($"Stale element with locator '{by}'. Attempting self-healing.");
                // In a real scenario, you might have the stale element reference here to pass to the analyzer.
                // Since we don't, we'll try to re-find it with the original locator.
                // This handles cases where the element just needs to be re-fetched.
                return _driver.FindElement(by);
            }
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            // Self-healing is typically more complex for FindElements and often not implemented.
            // We will just pass through to the original driver.
            return _driver.FindElements(by);
        }

        public void Dispose()
        {
            _driver.Dispose();
            _repository.SaveRepository();
        }

        public object ExecuteScript(string script, params object[] args)
        {
            return ((IJavaScriptExecutor)_driver).ExecuteScript(script, args);
        }

        public object ExecuteAsyncScript(string script, params object[] args)
        {
            return ((IJavaScriptExecutor)_driver).ExecuteAsyncScript(script, args);
        }

        private By ConvertStringToBy(string byString)
        {
            var parts = byString.Split(new[] { ": " }, 2, StringSplitOptions.None);
            var type = parts[0].Replace("By.", "");
            var value = parts[1];

            return type.ToLower() switch
            {
                "id" => By.Id(value),
                "name" => By.Name(value),
                "classname" => By.ClassName(value),
                "cssselector" => By.CssSelector(value),
                "linktext" => By.LinkText(value),
                "partiallinktext" => By.PartialLinkText(value),
                "tagname" => By.TagName(value),
                "xpath" => By.XPath(value),
                _ => throw new ArgumentException($"Unknown locator type: {type}")
            };
        }
    }
}