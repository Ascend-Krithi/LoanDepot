// NuGet Packages: Selenium.WebDriver
using OpenQA.Selenium;
using System.Collections.Generic;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.Pages
{
    /// <summary>
    /// The base class for all Page Objects. It provides shared functionality
    /// like the WebDriver instance, WaitHelper, PopupEngine, and common actions.
    /// </summary>
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly WaitHelper Wait;
        protected readonly PopupEngine PopupHandler;
        private readonly IJavaScriptExecutor _jsExecutor;

        // Using a dictionary to map logical names to locators promotes maintainability.
        protected Dictionary<string, By> Locators = new Dictionary<string, By>();

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WaitHelper(driver);
            PopupHandler = new PopupEngine(driver);
            _jsExecutor = (IJavaScriptExecutor)driver;
        }

        /// <summary>
        /// Navigates to the specified URL and automatically cleans up any initial popups.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        public virtual void GoToUrl(string url)
        {
            Driver.Navigate().GoToUrl(url);
            CleanupPopups();
        }

        /// <summary>
        /// Runs the PopupEngine to find and close common popups like cookie banners or chat widgets.
        /// </summary>
        public void CleanupPopups()
        {
            Logger.Log("Scanning for and cleaning up popups...");
            int closedCount = PopupHandler.ClosePopups();
            if (closedCount > 0)
            {
                Logger.Log($"Closed {closedCount} popup(s).");
            }
        }

        /// <summary>
        /// A robust click method that uses JavaScript. This can bypass issues where
        /// an element is obscured by another element or not fully in view.
        /// </summary>
        /// <param name="locator">The locator of the element to click.</param>
        protected void ClickWithJs(By locator)
        {
            var element = Wait.WaitForElement(locator);
            _jsExecutor.ExecuteScript("arguments[0].click();", element);
        }

        /// <summary>
        /// Finds an element using the logical name from the page's Locators dictionary.
        /// </summary>
        /// <param name="locatorKey">The logical name of the locator.</param>
        /// <returns>The found IWebElement.</returns>
        protected IWebElement FindElement(string locatorKey)
        {
            return Driver.FindElement(Locators[locatorKey]);
        }

        /// <summary>
        /// Types text into an element identified by its logical name.
        /// </summary>
        /// <param name="locatorKey">The logical name of the locator.</param>
        /// <param name="text">The text to type.</param>
        protected void TypeText(string locatorKey, string text)
        {
            var element = Wait.WaitForElementVisible(Locators[locatorKey]);
            element.Clear();
            element.SendKeys(text);
        }

        /// <summary>
        /// Clicks an element identified by its logical name.
        /// </summary>
        /// <param name="locatorKey">The logical name of the locator.</param>
        protected void ClickElement(string locatorKey)
        {
            var element = Wait.WaitForElementClickable(Locators[locatorKey]);
            element.Click();
        }
    }
}