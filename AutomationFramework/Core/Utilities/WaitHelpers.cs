using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Core.Utilities
{
    public static class WaitHelpers
    {
        public static WebDriverWait GetWait(IWebDriver driver)
        {
            var seconds = int.TryParse(ConfigManager.Get("DefaultWaitSeconds", "20"), out var s) ? s : 20;
            return new WebDriverWait(driver, TimeSpan.FromSeconds(seconds));
        }

        public static IWebElement WaitForVisible(IWebDriver driver, By by)
        {
            return GetWait(driver).Until(ExpectedConditions.ElementIsVisible(by));
        }
    }
}