using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace AutomationFramework.Core.Utilities
{
    public class PopupEngine
    {
        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _js;

        // Common selectors for popups/modals/overlays
        private readonly List<string> _popupSelectors = new List<string>
        {
            "div[role='dialog']",
            "div.modal",
            "div.overlay",
            "div.popup-container",
            "div[id*='popup']",
            "div[class*='modal']",
            "div[aria-modal='true']"
        };

        // Common selectors for close buttons within popups
        private readonly List<string> _closeButtonSelectors = new List<string>
        {
            "button[aria-label*='close' i]",
            "button[aria-label*='dismiss' i]",
            "button.close",
            "button.btn-close",
            "span.close",
            "i.close-icon",
            "[class*='close']"
        };

        public PopupEngine(IWebDriver driver)
        {
            _driver = driver;
            _js = (IJavaScriptExecutor)driver;
        }

        public void DismissPopups()
        {
            // This script finds visible popups and their close buttons
            const string script = @"
                const popupSelectors = arguments[0];
                const closeButtonSelectors = arguments[1];
                let closedSomething = false;

                for (const pSelector of popupSelectors) {
                    const popups = document.querySelectorAll(pSelector);
                    for (const popup of popups) {
                        // Check if the popup is visible
                        const style = window.getComputedStyle(popup);
                        if (style.display !== 'none' && style.visibility !== 'hidden' && parseFloat(style.opacity) > 0) {
                            for (const cSelector of closeButtonSelectors) {
                                const closeButton = popup.querySelector(cSelector);
                                if (closeButton) {
                                    closeButton.click();
                                    closedSomething = true;
                                    // Wait a bit for the popup to disappear before continuing
                                    await new Promise(r => setTimeout(r, 500));
                                    break; // Assume one close button is enough
                                }
                            }
                        }
                    }
                }
                return closedSomething;
            ";

            try
            {
                // Temporarily reduce implicit wait to avoid long waits for non-existent elements
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                _js.ExecuteAsyncScript(script, _popupSelectors, _closeButtonSelectors);
            }
            catch (System.Exception ex)
            {
                Logger.Log($"PopupEngine encountered an error: {ex.Message}");
            }
            finally
            {
                // Restore implicit wait to its default (0)
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            }
        }
    }
}