using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace AutomationFramework.Core.Utilities
{
    public static class WaitHelper
    {
        public static IWebElement WaitForElement(IWebDriver driver, By locator, int timeoutSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(drv => drv.FindElement(locator));
            }
            catch
            {
                return null;
            }
        }

        public static IWebElement WaitForElementVisible(IWebDriver driver, By locator, int timeoutSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(ExpectedConditions.ElementIsVisible(locator));
            }
            catch
            {
                return null;
            }
        }
    }
}