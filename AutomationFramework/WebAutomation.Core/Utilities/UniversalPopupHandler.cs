using OpenQA.Selenium;

namespace WebAutomation.Core.Utilities
{
    public static class UniversalPopupHandler
    {
        public static void HandleAll(IWebDriver driver)
        {
            CloseChatbot(driver);
            CloseContactUpdate(driver);
            CloseScheduledPayment(driver);
            RemoveAngularOverlays(driver);
        }

        private static void CloseChatbot(IWebDriver driver)
        {
            try
            {
                if (driver is not IJavaScriptExecutor js) return;

                js.ExecuteScript(@"
                    document
                      .querySelectorAll(
                        '#servisbot-messenger-iframe-roundel, 
                         #servisbot-messenger-iframe,
                         iframe[src*=servisbot]'
                      )
                      .forEach(e => e.remove());
                ");
            }
            catch { }
        }

        private static void CloseContactUpdate(IWebDriver driver)
        {
            try
            {
                var btns = driver.FindElements(
                    By.XPath("//button[normalize-space()='Update Later' or normalize-space()='Continue']")
                );

                if (btns.Count > 0)
                    btns[0].Click();
            }
            catch { }
        }

        private static void CloseScheduledPayment(IWebDriver driver)
        {
            try
            {
                var btns = driver.FindElements(
                    By.XPath("//button[normalize-space()='Continue']")
                );

                if (btns.Count > 0)
                    btns[0].Click();
            }
            catch { }
        }

        private static void RemoveAngularOverlays(IWebDriver driver)
        {
            try
            {
                if (driver is not IJavaScriptExecutor js) return;

                js.ExecuteScript(
                    "document.querySelectorAll('.cdk-overlay-backdrop').forEach(e => e.remove());"
                );
            }
            catch { }
        }
    }
}