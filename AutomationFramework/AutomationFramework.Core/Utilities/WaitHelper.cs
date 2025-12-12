using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Engines;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AutomationFramework.Core.Utilities
{
    public static class WaitHelper
    {
        public static void WaitForElementVisible(BasePage page, string logicalKey, int timeoutSeconds = 10)
        {
            var driver = ((dynamic)page).Driver as SelfHealingWebDriver;
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(drv => page.FindElement(logicalKey).Displayed);
        }
    }
}