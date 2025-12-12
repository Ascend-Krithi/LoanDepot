using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Utilities
{
    public static class PopupEngine
    {
        private static bool _chatWidgetsCleaned = false;
        private static readonly object _lock = new();

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
                            var closeButtons = overlay.FindElements(By.CssSelector("button, [role='button'], .close, [aria-label*='close']"));
                            if (closeButtons.Any())
                            {
                                foreach (var btn in closeButtons)
                                {
                                    try
                                    {
                                        if (btn.Displayed && btn.Enabled)
                                        {
                                            btn.Click();
                                            Thread.Sleep(60);
                                            break;
                                        }
                                    }
                                    catch { }
                                }
                            }
                            else
                            {
                                var js = (IJavaScriptExecutor)driver;
                                js.ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", overlay);
                                Thread.Sleep(60);
                            }
                        }
                        catch { }
                    }
                }

                CleanChatWidgets(driver);
            }
            catch { }
        }

        private static void CleanChatWidgets(IWebDriver driver)
        {
            lock (_lock)
            {
                if (_chatWidgetsCleaned)
                    return;
                _chatWidgetsCleaned = true;
            }

            try
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
                                try
                                {
                                    var js = (IJavaScriptExecutor)driver;
                                    js.ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", chat);
                                    Thread.Sleep(60);
                                }
                                catch { }
                            }
                        }
                        driver.SwitchTo().DefaultContent();
                    }
                    catch
                    {
                        driver.SwitchTo().DefaultContent();
                    }
                }
            }
            catch { }
        }
    }
}