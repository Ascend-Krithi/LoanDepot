// NuGet Packages: Selenium.WebDriver, Selenium.Support
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.SelfHealing
{
    /// <summary>
    /// A proxy for IWebDriver that intercepts element finding methods to provide self-healing capabilities.
    /// If an element is not found using its original locator, it attempts to find it using a previously
    /// stored snapshot and updates the locator if a new one is found.
    /// </summary>
    public class SelfHealingWebDriver : IWebDriver, IJavaScriptExecutor, ITakesScreenshot, IWrapsDriver
    {
        private readonly IWebDriver _driver;
        private readonly DomAnalyzer _domAnalyzer;

        public SelfHealingWebDriver(IWebDriver driver)
        {
            _driver = driver;
            _domAnalyzer = new DomAnalyzer(driver);
        }

        public IWebDriver WrappedDriver => _driver;

        public string Url { get => _driver.Url; set => _driver.Url = value; }
        public string Title => _driver.Title;
        public string PageSource => _driver.PageSource;
        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;

        public void Close() => _driver.Close();
        public void Quit() => _driver.Quit();
        public IOptions Manage() => _driver.Manage();
        public INavigation Navigate() => _driver.Navigate();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();
        public void Dispose() => _driver.Dispose();

        public IWebElement FindElement(By by)
        {
            try
            {
                var element = _driver.FindElement(by);
                // If found, store a snapshot for future healing
                var snapshot = new LocatorSnapshot(element, by);
                SelfHealingRepository.Store(snapshot);
                return element;
            }
            catch (NoSuchElementException)
            {
                Logger.Log($"Self-healing: Could not find element with locator: {by}. Attempting to heal.");
                var snapshot = SelfHealingRepository.Retrieve(by);
                if (snapshot == null)
                {
                    Logger.Log("Self-healing: No snapshot found in repository. Cannot heal.");
                    throw; // Re-throw original exception
                }

                var newLocator = _domAnalyzer.FindNewLocator(snapshot);
                if (newLocator != null)
                {
                    try
                    {
                        var healedElement = _driver.FindElement(newLocator);
                        Logger.Log($"Self-healing successful! Found element with new locator: {newLocator}");
                        // Update the repository with the new successful locator
                        var newSnapshot = new LocatorSnapshot(healedElement, newLocator);
                        SelfHealingRepository.Store(newSnapshot); 
                        return healedElement;
                    }
                    catch (NoSuchElementException ex)
                    {
                        Logger.Log($"Self-healing: Attempt failed. New locator {newLocator} also did not find the element.");
                        throw new NoSuchElementException($"Element not found with original locator '{by}' and healing attempt with new locator '{newLocator}' also failed.", ex);
                    }
                }
                
                Logger.Log("Self-healing: Could not determine a new locator.");
                throw; // Re-throw original exception if healing fails
            }
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            // Self-healing is typically more complex and less reliable for FindElements.
            // For now, we pass it through, but one could implement a similar logic.
            // The primary value is in healing specific, single elements.
            return _driver.FindElements(by);
        }

        // IJavaScriptExecutor implementation
        public object ExecuteScript(string script, params object[] args)
        {
            return ((IJavaScriptExecutor)_driver).ExecuteScript(script, args);
        }

        public object ExecuteAsyncScript(string script, params object[] args)
        {
            return ((IJavaScriptExecutor)_driver).ExecuteAsyncScript(script, args);
        }

        // ITakesScreenshot implementation
        public Screenshot GetScreenshot()
        {
            return ((ITakesScreenshot)_driver).GetScreenshot();
        }
    }
}