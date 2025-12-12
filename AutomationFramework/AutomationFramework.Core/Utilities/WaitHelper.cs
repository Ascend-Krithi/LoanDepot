using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AutomationFramework.Core.Utilities
{
    public class WaitHelper
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public WaitHelper(IWebDriver driver, TimeSpan timeout)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, timeout);
        }

        public IWebElement? WaitForElement(By locator)
        {
            try
            {
                return _wait.Until(ExpectedConditions.ElementExists(locator));
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Log($"Timeout waiting for element to exist: {locator}");
                return null;
            }
        }

        public IWebElement? WaitForElementVisible(By locator)
        {
            try
            {
                return _wait.Until(ExpectedConditions.ElementIsVisible(locator));
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Log($"Timeout waiting for element to be visible: {locator}");
                return null;
            }
        }
    }
}