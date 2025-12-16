using System;
using OpenQA.Selenium;
using System.Linq;
using System.Threading;

namespace AutomationFramework.Core.Utilities
{
    public static class PopupEngine
    {
        public static void CleanPopups(IWebDriver driver)
        {
            // Generic modal overlays
            try
            {
                var overlays = driver.FindElements(By.CssSelector("[role='dialog'], .modal, .overlay, .cdk-overlay-backdrop"));
                foreach (var overlay in overlays)
                {
                    try
                    {
                        if (overlay.Displayed)
                        {
                            overlay.Click();
                            Thread.Sleep(100); // micro-sleep
                        }
                    }
                    catch { }
                }
            }
            catch { }

            // Generic chat iframes
            try
            {
                var iframes = driver.FindElements(By.TagName("iframe"));
                foreach (var iframe in iframes)
                {
                    try
                    {
                        driver.SwitchTo().Frame(iframe);
                        var closeBtns = driver.FindElements(By.CssSelector("button, .close, .close-btn"));
                        foreach (var btn in closeBtns)
                        {
                            if (btn.Displayed)
                            {
                                btn.Click();
                                Thread.Sleep(100);
                            }
                        }
                        driver.SwitchTo().DefaultContent();
                    }
                    catch { driver.SwitchTo().DefaultContent(); }
                }
            }
            catch { }
        }
    }
}