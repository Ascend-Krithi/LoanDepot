// NuGet Packages: Selenium.WebDriver, Selenium.Support, DotNetSeleniumExtras.WaitHelpers
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Core.Utilities
{
    /// <summary>
    /// Provides helper methods for explicit waits in Selenium.
    /// </summary>
    public class WaitHelper
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public WaitHelper(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(ConfigManager.Settings.DefaultTimeoutInSeconds));
        }

        /// <summary>
        /// Waits for an element to be present on the DOM of a page.
        /// </summary>
        public IWebElement WaitForElement(By locator)
        {
            return _wait.Until(ExpectedConditions.ElementExists(locator));
        }

        /// <summary>
        /// Waits for an element to be visible.
        /// </summary>
        public IWebElement WaitForElementVisible(By locator)
        {
            return _wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        /// <summary>
        /// Waits for an element to be clickable.
        /// </summary>
        public IWebElement WaitForElementClickable(By locator)
        {
            return _wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        /// <summary>
        /// Waits for an element to be selected.
        /// </summary>
        public bool WaitForElementSelected(By locator)
        {
            return _wait.Until(ExpectedConditions.ElementToBeSelected(locator));
        }

        /// <summary>
        /// Waits for the page title to contain a specific text.
        /// </summary>
        public bool WaitForTitleContains(string title)
        {
            return _wait.Until(ExpectedConditions.TitleContains(title));
        }

        /// <summary>
        /// Waits for an alert to be present.
        /// </summary>
        public IAlert WaitForAlert()
        {
            return _wait.Until(ExpectedConditions.AlertIsPresent());
        }
    }
}