using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationFramework.Core.Utilities
{
    public static class WaitHelper
    {
        public static IWebElement? WaitForElement(IWebDriver driver, By locator, int timeoutSeconds = 10)
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
    }
}