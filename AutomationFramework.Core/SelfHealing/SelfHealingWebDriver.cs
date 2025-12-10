using OpenQA.Selenium;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver
    {
        private readonly IWebDriver _driver;

        public SelfHealingWebDriver(IWebDriver driver)
        {
            _driver = driver;
        }

        // Implement IWebDriver members, using best_locator and fallback_locators if available.
        // Omitted here due to missing locator JSON.
        // All methods delegate to _driver.
        public string Url { get => _driver.Url; set => _driver.Url = value; }
        public string Title => _driver.Title;
        public string PageSource => _driver.PageSource;
        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;

        public void Close() => _driver.Close();
        public void Dispose() => _driver.Dispose();
        public IWebElement FindElement(By by) => _driver.FindElement(by);
        public ReadOnlyCollection<IWebElement> FindElements(By by) => _driver.FindElements(by);
        public IOptions Manage() => _driver.Manage();
        public INavigation Navigate() => _driver.Navigate();
        public void Quit() => _driver.Quit();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();
    }
}