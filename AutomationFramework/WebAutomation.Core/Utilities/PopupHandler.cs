using OpenQA.Selenium;

namespace WebAutomation.Core.Utilities
{
    public sealed class PopupHandler
    {
        private readonly IWebDriver _driver;
        private readonly SmartWait _wait;

        public PopupHandler(IWebDriver driver, SmartWait wait)
        {
            _driver = driver;
            _wait = wait;
        }

        /// <summary>
        /// Safely clicks a popup action if present.
        /// Does nothing if popup is not found.
        /// </summary>
        public void HandleIfPresent(By popupActionLocator, int timeoutSeconds = 5)
        {
            try
            {
                if (_wait.UntilPresent(popupActionLocator, timeoutSeconds))
                {
                    _wait.UntilClickable(popupActionLocator, timeoutSeconds)
                         .Click();
                }
            }
            catch (StaleElementReferenceException)
            {
                // Popup disappeared between detection & click – safe to ignore
            }
            catch (ElementClickInterceptedException)
            {
                // Overlay animation / Angular transition – safe to ignore
            }
        }

        /// <summary>
        /// Checks whether popup exists (no interaction).
        /// </summary>
        public bool IsPresent(By popupLocator, int timeoutSeconds = 3)
        {
            return _wait.UntilPresent(popupLocator, timeoutSeconds);
        }
    }
}