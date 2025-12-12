using OpenQA.Selenium;
using System;
using System.Linq;

namespace AutomationFramework.Core.Utilities
{
    public static class PopupEngine
    {
        public static void HandleAlert(IWebDriver driver, bool accept = true)
        {
            try
            {
                var alert = driver.SwitchTo().Alert();
                if (accept)
                    alert.Accept();
                else
                    alert.Dismiss();
            }
            catch (NoAlertPresentException) { }
        }

        public static void HandleMultipleWindows(IWebDriver driver, Action<IWebDriver> onPopup)
        {
            var mainWindow = driver.CurrentWindowHandle;
            var allWindows = driver.WindowHandles;
            foreach (var handle in allWindows.Where(h => h != mainWindow))
            {
                driver.SwitchTo().Window(handle);
                onPopup(driver);
                driver.Close();
            }
            driver.SwitchTo().Window(mainWindow);
        }

        public static void HandleFrame(IWebDriver driver, string frameNameOrId, Action<IWebDriver> onFrame)
        {
            driver.SwitchTo().Frame(frameNameOrId);
            onFrame(driver);
            driver.SwitchTo().DefaultContent();
        }
    }
}