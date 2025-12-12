using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework.Core.Utilities
{
    public static class PopupEngine
    {
        private static bool _chatWidgetsCleaned = false;

        private static readonly string[] OverlaySelectors = new[]
        {
            "[role='dialog']",
            ".cdk-overlay-backdrop",
            ".modal-backdrop",
            "[aria-modal='true']",
            "[id*='cookie']",
            "[class*='cookie']"
        };

        private static readonly string[] ChatSelectors = new[]
        {
            "[id*='chat']",
            "[class*='chat']"
        };

        public static void CleanPopups(IWebDriver driver)
        {
            try
            {
                foreach (var selector in OverlaySelectors)
                {
                    var overlays = driver.FindElements(By.CssSelector(selector));
                    foreach (var overlay in overlays)
                    {
                        try
                        {
                            var closeButton = overlay.FindElements(By.CssSelector("button, [role='button'], .close, .btn-close")).FirstOrDefault();
                            if (closeButton != null && closeButton.Displayed && closeButton.Enabled)
                            {
                                closeButton.Click();
                                Thread.Sleep(60);
                                continue;
                            }
                            // Remove via JS
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", overlay);
                            Thread.Sleep(60);
                        }
                        catch { }
                    }
                }

                if (!_chatWidgetsCleaned)
                {
                    var iframes = driver.FindElements(By.TagName("iframe"));
                    foreach (var iframe in iframes)
                    {
                        try
                        {
                            driver.SwitchTo().Frame(iframe);
                            foreach (var selector in ChatSelectors)
                            {
                                var chats = driver.FindElements(By.CssSelector(selector));
                                foreach (var chat in chats)
                                {
                                    ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", chat);
                                    Thread.Sleep(60);
                                }
                            }
                            driver.SwitchTo().DefaultContent();
                        }
                        catch
                        {
                            try { driver.SwitchTo().DefaultContent(); } catch { }
                        }
                    }
                    _chatWidgetsCleaned = true;
                }
            }
            catch { }
        }
    }
}