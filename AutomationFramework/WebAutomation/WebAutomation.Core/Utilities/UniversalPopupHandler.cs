using OpenQA.Selenium;

namespace WebAutomation.Core.Utilities
{
    /// <summary>
    /// A handler for popups that can appear unexpectedly and block test execution.
    /// This should be called strategically, e.g., after navigation or major actions.
    /// </summary>
    public static class UniversalPopupHandler
    {
        public static void HandleAll(IWebDriver driver)
        {
            // The order can be important. Removing overlays might be necessary
            // before trying to click buttons on popups underneath.
            RemoveAngularOverlays(driver);
            CloseChatbot(driver);
            CloseContactUpdate(driver);
            CloseScheduledPayment(driver);
        }

        private static void CloseChatbot(IWebDriver driver)
        {
            try
            {
                if (driver is not IJavaScriptExecutor js) return;

                js.ExecuteScript(@"
                    const elementsToRemove = document.querySelectorAll(
                        '#servisbot-messenger-iframe-roundel, #servisbot-messenger-iframe, iframe[src*=servisbot]'
                    );
                    elementsToRemove.forEach(e => e.remove());
                ");
            }
            catch { /* Ignored */ }
        }

        private static void CloseContactUpdate(IWebDriver driver)
        {
            HandleGenericPopup(driver, "//button[normalize-space()='Update Later' or normalize-space()='Continue']");
        }

        private static void CloseScheduledPayment(IWebDriver driver)
        {
            HandleGenericPopup(driver, "//button[normalize-space()='Continue']");
        }

        private static void RemoveAngularOverlays(IWebDriver driver)
        {
            try
            {
                if (driver is not IJavaScriptExecutor js) return;

                js.ExecuteScript(
                    "document.querySelectorAll('.cdk-overlay-backdrop.cdk-overlay-dark-backdrop.cdk-overlay-backdrop-showing').forEach(e => e.remove());"
                );
            }
            catch { /* Ignored */ }
        }

        private static void HandleGenericPopup(IWebDriver driver, string buttonXPath)
        {
            try
            {
                // Use a very short, implicit wait to avoid slowing down tests
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
                var buttons = driver.FindElements(By.XPath(buttonXPath));

                foreach (var button in buttons)
                {
                    try
                    {
                        if (button.Displayed && button.Enabled)
                        {
                            button.Click();
                            // Once one is clicked, assume the popup is gone and exit
                            break; 
                        }
                    }
                    catch (StaleElementReferenceException) { /* Element disappeared, which is fine */ }
                }
            }
            catch { /* Ignored */ }
            finally
            {
                // Reset implicit wait to zero to not interfere with explicit waits
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(0);
            }
        }
    }
}