using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
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

        private static readonly string[] ChatIframeSelectors =
        {
            "iframe[id*='chat']",
            "iframe[title*='chat']"
        };

        private static bool _chatWidgetHandled = false;

        public static void CleanPopups(IWebDriver driver)
        {
            HandleOverlays(driver);
            if (!_chatWidgetHandled)
            {
                HandleChatWidgets(driver);
                _chatWidgetHandled = true;
            }
        }

        private static void HandleOverlays(IWebDriver driver)
        {
            var jsExecutor = (IJavaScriptExecutor)driver;
            foreach (var selector in PopupSelectors)
            {
                try
                {
                    var elements = driver.FindElements(By.CssSelector(selector));
                    foreach (var element in elements)
                    {
                        if (!element.Displayed) continue;

                        Logger.Log($"PopupEngine: Found potential overlay with selector '{selector}'.");
                        bool closed = TryClickCloseButton(element);
                        if (!closed)
                        {
                            Logger.Log($"PopupEngine: Could not find a close button. Removing element via JS.");
                            jsExecutor.ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", element);
                            Thread.Sleep(80); // Small delay to allow DOM to settle
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"PopupEngine: Error handling selector '{selector}': {ex.Message}");
                }
            }
        }

        private static bool TryClickCloseButton(IWebElement popupElement)
        {
            foreach (var selector in CloseButtonSelectors)
            {
                try
                {
                    var closeButton = popupElement.FindElement(By.CssSelector(selector));
                    if (closeButton.Displayed && closeButton.Enabled)
                    {
                        closeButton.Click();
                        Logger.Log($"PopupEngine: Clicked close button with selector '{selector}'.");
                        Thread.Sleep(80); // Small delay
                        return true;
                    }
                }
                catch (NoSuchElementException)
                {
                    // Ignore, button not found
                }
            }
            return false;
        }

        private static void HandleChatWidgets(IWebDriver driver)
        {
            var jsExecutor = (IJavaScriptExecutor)driver;
            ReadOnlyCollection<IWebElement> iframes;
            try
            {
                iframes = driver.FindElements(By.TagName("iframe"));
            }
            catch { return; }


            foreach (var iframe in iframes)
            {
                try
                {
                    driver.SwitchTo().Frame(iframe);
                    foreach (var selector in ChatIframeSelectors)
                    {
                        var chatElements = driver.FindElements(By.CssSelector(selector));
                        if (chatElements.Any())
                        {
                            Logger.Log($"PopupEngine: Found and removed chat widget in iframe.");
                            jsExecutor.ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", iframe);
                            driver.SwitchTo().DefaultContent();
                            return; // Assume one chat widget
                        }
                    }
                    driver.SwitchTo().DefaultContent();
                }
                catch (Exception ex)
                {
                    Logger.Log($"PopupEngine: Error handling iframe for chat widget: {ex.Message}");
                    driver.SwitchTo().DefaultContent(); // Ensure we switch back
                }
            }
        }
    }
}