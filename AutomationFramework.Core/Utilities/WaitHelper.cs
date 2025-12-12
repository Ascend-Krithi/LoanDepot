// AutomationFramework.Core/Utilities/WaitHelper.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AutomationFramework.Core.Utilities
{
    public static class WaitHelper
    {
        public static IWebElement? WaitForElement(IWebDriver driver, By locator, int timeoutSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(d => d.FindElement(locator));
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public static IWebElement? WaitForElementVisible(IWebDriver driver, By locator, int timeoutSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(d =>
                {
                    var element = d.FindElement(locator);
                    return (element != null && element.Displayed) ? element : null;
                });
            }
            catch (WebDriverTimeoutException)
            {
                return null;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }
    }
}