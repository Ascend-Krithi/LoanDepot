using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AutomationFramework.Core.Utilities
{
    public static class WaitHelpers
    {
        public static IWebElement WaitForElementVisible(IWebDriver driver, By by, int timeoutSeconds = 10)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public static IWebElement WaitForElementClickable(IWebDriver driver, By by, int timeoutSeconds = 10)
        {
            return new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds))
                .Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(by));
        }
    }
}