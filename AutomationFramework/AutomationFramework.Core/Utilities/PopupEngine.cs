using System.Linq;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Utilities
{
    public static class PopupEngine
    {
        private static bool chatCleaned = false;

        public static void CleanPopups(IWebDriver driver)
        {
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
                        var closeBtn = el.FindElements(By.CssSelector("[data-dismiss], .close, [aria-label='Close']")).FirstOrDefault();
                        if (closeBtn != null)
                            closeBtn.Click();
                        else
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", el);
                    }
                    catch { }
                }
            }

            if (!chatCleaned)
            {
                var iframes = driver.FindElements(By.TagName("iframe"));
                foreach (var iframe in iframes)
                {
                    try
                    {
                        driver.SwitchTo().Frame(iframe);
                        var chatEls = driver.FindElements(By.CssSelector("[id*='chat'], [class*='chat']"));
                        foreach (var chatEl in chatEls)
                        {
                            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].parentNode.removeChild(arguments[0]);", chatEl);
                        }
                        driver.SwitchTo().DefaultContent();
                    }
                    catch { }
                }
                chatCleaned = true;
            }
        }
    }
}