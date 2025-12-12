using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Utilities
{
    public static class PopupEngine
    {
        private static bool _chatWidgetsCleaned = false;

        public static void CleanPopups(IWebDriver driver)
        {
            try
            {
                // Generic overlays and modals
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
                            var closeBtn = TryFindCloseButton(el);
                            if (closeBtn != null)
                            {
                                closeBtn.Click();
                            }
                            else
                            {
                                RemoveElementViaJs(driver, el);
                            }
                        }
                        catch { }
                        Thread.Sleep(60);
                    }
                }

                // Chat widgets in iframes (only once per run)
                if (!_chatWidgetsCleaned)
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
                                RemoveElementViaJs(driver, chatEl);
                            }
                            driver.SwitchTo().DefaultContent();
                        }
                        catch { driver.SwitchTo().DefaultContent(); }
                        Thread.Sleep(60);
                    }
                    _chatWidgetsCleaned = true;
                }
            }
            catch { }
        }

        private static IWebElement TryFindCloseButton(IWebElement el)
        {
            try
            {
                var closeBtn = el.FindElements(By.CssSelector("button[aria-label*='close'],button.close,[class*='close'],[data-dismiss='modal']")).FirstOrDefault();
                return closeBtn;
            }
            catch
            {
                return null;
            }
        }

        private static void RemoveElementViaJs(IWebDriver driver, IWebElement el)
        {
            try
            {
                var js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", el);
            }
            catch { }
        }
    }
}