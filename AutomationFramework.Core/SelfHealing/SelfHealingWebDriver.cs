using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver
    {
        private readonly IWebDriver _driver;
        private readonly SelfHealingRepository _repository = new();

        public SelfHealingWebDriver(IWebDriver driver)
        {
            _driver = driver;
        }

        public IWebElement FindElement(string logicalKey, By locator, int timeoutSeconds = 10)
        {
            _repository.AddOrUpdate(logicalKey, new LocatorSnapshot(logicalKey, locator));
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(drv => drv.FindElement(locator));
            }
            catch (NoSuchElementException)
            {
                var healed = DomAnalyzer.Heal(logicalKey, locator);
                try
                {
                    var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
                    return wait.Until(drv => drv.FindElement(healed));
                }
                catch (Exception ex)
                {
                    throw new NoSuchElementException($"Element not found for logicalKey: {logicalKey}", ex);
                }
            }
        }

        // Delegate all IWebDriver members
        public string Url { get => _driver.Url; set => _driver.Url = value; }
        public string Title => _driver.Title;
        public string PageSource => _driver.PageSource;
        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;

        public void Close() => _driver.Close();
        public void Dispose() => _driver.Dispose();
        public IWebElement FindElement(By by) => _driver.FindElement(by);
        public ReadOnlyCollection<IWebElement> FindElements(By by) => _driver.FindElements(by);
        public void Navigate() => _driver.Navigate();
        public void Quit() => _driver.Quit();
        public IOptions Manage() => _driver.Manage();
        public INavigation NavigateTo() => _driver.Navigate();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();
    }
}