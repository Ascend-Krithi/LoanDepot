using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Core.Utilities
{
    public class WaitHelper
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public WaitHelper(IWebDriver driver)
        {
            _driver = driver;
            var settings = ConfigManager.GetTestSettings();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(settings.DefaultWaitTimeout));
        }

        public IWebElement WaitForElementToBeVisible(By locator)
        {
            return _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        public IWebElement WaitForElementToBeClickable(By locator)
        {
            return _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }

        public bool WaitForElementToDisappear(By locator, int timeoutInSeconds)
        {
            var customWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            return customWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locator));
        }

        public void WaitForPageToLoad(int timeoutInSeconds = 30)
        {
            var customWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            customWait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public void WaitUntil(Func<IWebDriver, bool> condition, int timeoutInSeconds = 30)
        {
            var customWait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
            customWait.Until(condition);
        }
    }
}