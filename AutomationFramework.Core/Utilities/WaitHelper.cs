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
                Logger.Log($"Timeout: Element with locator '{locator}' was not found within {timeoutSeconds} seconds.");
                return null;
            }
            catch (NoSuchElementException)
            {
                Logger.Log($"Not Found: Element with locator '{locator}' could not be found.");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Log($"An unexpected error occurred in WaitForElement for locator '{locator}': {ex.Message}");
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
                    return element.Displayed ? element : null;
                });
            }
            catch (WebDriverTimeoutException)
            {
                Logger.Log($"Timeout: Element with locator '{locator}' was not visible within {timeoutSeconds} seconds.");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Log($"An unexpected error occurred in WaitForElementVisible for locator '{locator}': {ex.Message}");
                return null;
            }
        }
    }
}