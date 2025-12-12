using OpenQA.Selenium;
using AutomationFramework.Core.Utilities;
using System.Collections.Generic;

namespace AutomationFramework.Core.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly WaitHelper Wait;
        private readonly PopupEngine _popupEngine;

        // Using logical names for locators to decouple from implementation
        protected Dictionary<string, By> Locators = new Dictionary<string, By>();

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new WaitHelper(driver);
            _popupEngine = new PopupEngine(driver);

            // Automatically handle popups on page initialization
            _popupEngine.DismissPopups();
        }

        public virtual void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
            Wait.WaitForPageToLoad();
            _popupEngine.DismissPopups(); // Handle popups that may appear on load
        }

        protected IWebElement GetElement(string logicalName)
        {
            if (!Locators.ContainsKey(logicalName))
            {
                throw new KeyNotFoundException($"Locator with logical name '{logicalName}' not found in the page object.");
            }
            return Wait.WaitForElementToBeVisible(Locators[logicalName]);
        }

        protected IWebElement GetClickableElement(string logicalName)
        {
            if (!Locators.ContainsKey(logicalName))
            {
                throw new KeyNotFoundException($"Locator with logical name '{logicalName}' not found in the page object.");
            }
            return Wait.WaitForElementToBeClickable(Locators[logicalName]);
        }

        protected void Click(string logicalName)
        {
            GetClickableElement(logicalName).Click();
        }

        protected void SendKeys(string logicalName, string text)
        {
            var element = GetElement(logicalName);
            element.Clear();
            element.SendKeys(text);
        }

        protected string GetText(string logicalName)
        {
            return GetElement(logicalName).Text;
        }

        // Use JavaScript click as a fallback for stubborn elements
        protected void JsClick(string logicalName)
        {
            if (!Locators.ContainsKey(logicalName))
            {
                throw new KeyNotFoundException($"Locator with logical name '{logicalName}' not found in the page object.");
            }
            var element = Wait.WaitForElementToBeVisible(Locators[logicalName]);
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }
    }
}