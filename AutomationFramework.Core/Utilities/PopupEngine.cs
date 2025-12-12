using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Utilities
{
    public static class PopupEngine
    {
        private static bool _chatWidgetHandled = false;

        public static void CleanPopups(IWebDriver driver)
        {
            // Universal selectors for overlays, modals, cookie banners
            string[] selectors = new[]
            {
                "[role='dialog']",
                ".cdk-overlay-backdrop",
                ".modal-backdrop",
                "[aria-modal='true']",
                "[id*='cookie']",
                "[class*='cookie']"
            };

            foreach (var selector in selectors)
            {
                var elements = driver.FindElements(By.CssSelector(selector));
                foreach (var el in elements)
                {
                    try
                    {
                        var closeBtn = el.FindElements(By.CssSelector("button, [role='button'], .close, .dismiss")).FirstOrDefault();
                        if (closeBtn != null && closeBtn.Displayed && closeBtn.Enabled)
                        {
                            closeBtn.Click();
                        }
                        else
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", el);
                        }
                    }
                    catch { }
                    Thread.Sleep(60); // micro-sleep
                }
            }

            if (!_chatWidgetHandled)
            {
                var iframes = driver.FindElements(By.TagName("iframe"));
                foreach (var iframe in iframes)
                {
                    try
                    {
                        driver.SwitchTo().Frame(iframe);
                        var chatEls = driver.FindElements(By.CssSelector("[id*='chat'],[class*='chat']"));
                        foreach (var chatEl in chatEls)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", chatEl);
                        }
                        driver.SwitchTo().DefaultContent();
                    }
                    catch { driver.SwitchTo().DefaultContent(); }
                }
                _chatWidgetHandled = true;
            }
        }
    }
}