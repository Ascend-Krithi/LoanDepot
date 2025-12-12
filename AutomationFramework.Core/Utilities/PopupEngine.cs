// AutomationFramework.Core/Utilities/PopupEngine.cs
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Threading;

namespace AutomationFramework.Core.Utilities
{
    public static class PopupEngine
    {
        private static readonly string[] PopupSelectors =
        {
            "[role='dialog']",
            ".cdk-overlay-backdrop",
            ".modal-backdrop",
            "[aria-modal='true']",
            "[id*='cookie']",
            "[class*='cookie']"
        };

        private static readonly string[] CloseButtonSelectors =
        {
            "[aria-label*='close']",
            "[aria-label*='dismiss']",
            "[class*='close']",
            "button:contains('Accept')",
            "button:contains('OK')"
        };

        private static readonly string[] ChatWidgetSelectors =
        {
            "[id*='chat']",
            "[class*='chat']",
            "[title*='chat']"
        };

        private static bool _chatWidgetCleaned = false;

        public static void CleanPopups(IWebDriver driver)
        {
            var jsExecutor = (IJavaScriptExecutor)driver;

            // Clean generic popups
            foreach (var selector in PopupSelectors)
            {
                var elements = driver.FindElements(B.CssSelector(selector));
                foreach (var element in elements.Where(e => e.Displayed))
                {
                    try
                    {
                        // Try to find and click a close button first
                        var closeButton = CloseButtonSelectors
                            .Select(sel => element.FindElements(By.CssSelector(sel)).FirstOrDefault())
                            .FirstOrDefault(btn => btn != null && btn.Displayed);

                        if (closeButton != null)
                        {
                            closeButton.Click();
                            Thread.Sleep(80); // Small delay for UI to update
                        }
                        else
                        {
                            // If no close button, remove via JS
                            jsExecutor.ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", element);
                        }
                    }
                    catch (Exception ex) when (ex is StaleElementReferenceException || ex is ElementNotInteractableException)
                    {
                        // Ignore if element is gone or not interactable
                    }
                }
            }

            // Clean chat widgets in iframes (once per run)
            if (!_chatWidgetCleaned)
            {
                CleanChatWidgetsInFrames(driver, jsExecutor);
                _chatWidgetCleaned = true;
            }
        }

        private static void CleanChatWidgetsInFrames(IWebDriver driver, IJavaScriptExecutor jsExecutor)
        {
            var iframes = driver.FindElements(By.TagName("iframe"));
            foreach (var frame in iframes)
            {
                try
                {
                    driver.SwitchTo().Frame(frame);
                    foreach (var selector in ChatWidgetSelectors)
                    {
                        var chatElements = driver.FindElements(By.CssSelector(selector));
                        foreach (var chatElement in chatElements.Where(e => e.Displayed))
                        {
                            jsExecutor.ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", chatElement);
                        }
                    }
                }
                catch (Exception)
                {
                    // Ignore exceptions (e.g., frame detached)
                }
                finally
                {
                    driver.SwitchTo().DefaultContent();
                }
            }
        }
    }
}