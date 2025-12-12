// NuGet Packages: Selenium.WebDriver
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationFramework.Core.Utilities
{
    /// <summary>
    /// A high-performance engine to detect and handle common popups, overlays, and chat widgets.
    /// </summary>
    public class PopupEngine
    {
        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _jsExecutor;

        // A list of common selectors for popups. This can be expanded.
        private readonly List<By> _popupSelectors = new List<By>
        {
            // Cookie Banners
            By.CssSelector("[id*='cookie'], [class*='cookie'], [aria-label*='cookie']"),
            // Modals & Overlays
            By.CssSelector(".modal-dialog, .modal-content, [role='dialog']"),
            // Chat Widgets
            By.CssSelector("iframe[title*='chat'], div[id*='chat-widget']"),
            // Subscription Popups
            By.CssSelector("[id*='subscribe'], [class*='subscribe-popup']")
        };

        public PopupEngine(IWebDriver driver)
        {
            _driver = driver;
            _jsExecutor = (IJavaScriptExecutor)driver;
        }

        /// <summary>
        /// Scans the page for any known popup types and attempts to close them.
        /// </summary>
        /// <returns>The number of popups that were closed.</returns>
        public int ClosePopups()
        {
            int closedCount = 0;
            var originalTimeout = _driver.Manage().Timeouts().ImplicitWait;
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1); // Use a short timeout to avoid long waits

            foreach (var selector in _popupSelectors)
            {
                try
                {
                    var elements = _driver.FindElements(selector).Where(e => e.Displayed).ToList();
                    foreach (var element in elements)
                    {
                        if (TryClosePopup(element))
                        {
                            closedCount++;
                            // Wait a moment for the UI to settle after closing
                            System.Threading.Thread.Sleep(500);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error while trying to close popup with selector '{selector}': {ex.Message}");
                }
            }

            _driver.Manage().Timeouts().ImplicitWait = originalTimeout; // Restore original timeout
            return closedCount;
        }

        private bool TryClosePopup(IWebElement popupElement)
        {
            // Strategy 1: Look for a close button (X, 'close', 'dismiss', etc.)
            var closeButtons = popupElement.FindElements(By.CssSelector("button[class*='close'], button[aria-label*='close'], button[aria-label*='Dismiss'], [class*='close-icon']"));
            if (closeButtons.Any(b => b.Displayed))
            {
                try
                {
                    closeButtons.First(b => b.Displayed).Click();
                    Logger.Log("Closed a popup using a close button.");
                    return true;
                }
                catch { /* Ignore click errors and try next method */ }
            }

            // Strategy 2: Look for a button with text like "Accept", "Agree", "No Thanks"
            var actionButtons = popupElement.FindElements(By.TagName("button"));
            var acceptButton = actionButtons.FirstOrDefault(b => 
                b.Text.ToLower().Contains("accept") || 
                b.Text.ToLower().Contains("agree") ||
                b.Text.ToLower().Contains("got it"));
            
            if (acceptButton != null && acceptButton.Displayed)
            {
                try
                {
                    acceptButton.Click();
                    Logger.Log("Closed a popup using an 'Accept' style button.");
                    return true;
                }
                catch { /* Ignore click errors */ }
            }

            // Strategy 3: If all else fails, hide the element with JavaScript
            try
            {
                _jsExecutor.ExecuteScript("arguments[0].style.display='none';", popupElement);
                Logger.Log("Hid a popup using JavaScript as a last resort.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Failed to hide popup with JS: {ex.Message}");
            }

            return false;
        }
    }
}