using OpenQA.Selenium;

namespace AutomationFramework.Core.Utilities
{
    public class PopupEngine
    {
        private readonly IWebDriver _driver;
        private readonly IJavaScriptExecutor _jsExecutor;

        // Universal selectors for common popups/modals/overlays
        private static readonly By[] PopupSelectors =
        {
            By.CssSelector("[role='dialog']"),
            By.CssSelector(".modal, .Modal, .popup, .Popup"),
            By.CssSelector("[id*='modal'], [id*='popup']"),
            By.CssSelector("div[class*='overlay']"),
        };

        private static readonly By[] CloseButtonSelectors =
        {
            By.CssSelector("[aria-label*='close'], [aria-label*='Close']"),
            By.CssSelector("button[class*='close'], button[class*='Close']"),
            By.CssSelector("span[class*='close'], span[class*='Close']"),
            By.CssSelector("i[class*='close'], i[class*='Close']"),
        };

        // Selectors for chat widgets often found in iframes
        private static readonly By[] ChatFrameSelectors =
        {
            By.CssSelector("iframe[id*='chat'], iframe[title*='chat']"),
        };

        public PopupEngine(IWebDriver driver)
        {
            _driver = driver;
            _jsExecutor = (IJavaScriptExecutor)driver;
        }

        public void HandlePopups()
        {
            HandleChatWidgets();
            HandleOverlaysAndModals();
        }

        private void HandleOverlaysAndModals()
        {
            foreach (var selector in PopupSelectors)
            {
                var popups = _driver.FindElements(selector);
                foreach (var popup in popups.Where(p => p.Displayed))
                {
                    TryClosePopup(popup);
                }
            }
        }

        private void TryClosePopup(IWebElement popup)
        {
            foreach (var closeSelector in CloseButtonSelectors)
            {
                try
                {
                    var closeButton = popup.FindElement(closeSelector);
                    if (closeButton.Displayed && closeButton.Enabled)
                    {
                        Logger.Log($"Closing popup with button: {closeSelector}");
                        _jsExecutor.ExecuteScript("arguments[0].click();", closeButton);
                        // Wait a moment for the popup to disappear
                        Thread.Sleep(500); 
                        return; // Assume one close action is enough
                    }
                }
                catch (NoSuchElementException)
                {
                    // Ignore if close button is not found
                }
                catch (Exception ex)
                {
                    Logger.Log($"Error trying to close popup: {ex.Message}");
                }
            }
        }

        private void HandleChatWidgets()
        {
            foreach (var frameSelector in ChatFrameSelectors)
            {
                var frames = _driver.FindElements(frameSelector);
                if (frames.Any(f => f.Displayed))
                {
                    var frame = frames.First(f => f.Displayed);
                    Logger.Log($"Hiding chat widget in iframe: {frameSelector}");
                    _jsExecutor.ExecuteScript("arguments[0].style.display='none';", frame);
                }
            }
        }
    }
}