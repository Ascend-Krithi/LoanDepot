using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework.Core.Widgets
{
    public static class UniversalPopupHandler
    {
        public static void DismissAllPopups(IWebDriver driver)
        {
            try
            {
                DismissJavascriptAlert(driver);
                DismissCommonModalButtons(driver);
                CloseOverlayByClasses(driver);
            }
            catch { }
        }

        private static void DismissJavascriptAlert(IWebDriver driver)
        {
            try
            {
                var alert = driver.SwitchTo().Alert();
                alert.Dismiss();
            }
            catch { }
        }

        private static void DismissCommonModalButtons(IWebDriver driver)
        {
            var candidates = new List<string> { "Close", "Dismiss", "No Thanks", "Okay", "OK", "Cancel", "Got it" };
            foreach (var text in candidates)
            {
                var buttons = driver.FindElements(By.XPath($"//button[normalize-space()='{text}']|//a[normalize-space()='{text}']"));
                foreach (var btn in buttons)
                {
                    if (btn.Displayed && btn.Enabled)
                        btn.Click();
                }
            }
        }

        private static void CloseOverlayByClasses(IWebDriver driver)
        {
            var closeIcons = driver.FindElements(By.CssSelector(".modal .close, .modal .btn-close, .x-close, .popup-close, .overlay-close"));
            foreach (var el in closeIcons)
            {
                if (el.Displayed && el.Enabled)
                    el.Click();
            }
        }
    }
}