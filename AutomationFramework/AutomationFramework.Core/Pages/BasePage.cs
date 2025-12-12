using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public abstract class BasePage
    {
        protected readonly SelfHealingWebDriver Driver;
        private readonly PopupEngine _popupEngine;

        protected BasePage(SelfHealingWebDriver driver)
        {
            Driver = driver;
            _popupEngine = new PopupEngine(driver);
        }

        /// <summary>
        /// Finds an element using a logical key and locator, automatically handling popups first.
        /// </summary>
        /// <param name="logicalKey">A unique string identifying the element (e.g., "LoginPage.UsernameInput").</param>
        /// <param name="locator">The Selenium By locator for the element.</param>
        /// <returns>The found IWebElement.</returns>
        protected IWebElement FindElement(string logicalKey, By locator)
        {
            // Proactively handle any unexpected popups before interacting with the page.
            _popupEngine.HandlePopups();
            
            // Use the self-healing driver to find the element.
            return Driver.FindElement(logicalKey, locator);
        }
    }
}