using OpenQA.Selenium;

namespace AutomationFramework.Core.Widgets
{
    public class UniversalPopupHandler
    {
        private readonly IWebDriver driver;

        public UniversalPopupHandler(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void HandleAllPopups()
        {
            // Example: Dismiss contact update modal
            try
            {
                var updateLater = driver.FindElements(By.XPath("//button[contains(text(),'Update Later')]"));
                if (updateLater.Count > 0)
                    updateLater[0].Click();
            }
            catch { }

            // Example: Dismiss chatbot iframe
            try
            {
                driver.SwitchTo().Frame("chatbot-iframe");
                var closeBtn = driver.FindElements(By.CssSelector(".close-chat"));
                if (closeBtn.Count > 0)
                    closeBtn[0].Click();
                driver.SwitchTo().DefaultContent();
            }
            catch { }

            // Example: Dismiss scheduled payment popup
            try
            {
                var continueBtn = driver.FindElements(By.XPath("//button[contains(text(),'Continue')]"));
                if (continueBtn.Count > 0)
                    continueBtn[0].Click();
            }
            catch { }

            // Example: Dismiss banners/modals
            try
            {
                var banners = driver.FindElements(By.CssSelector(".banner-close, .modal-close"));
                if (banners.Count > 0)
                    banners[0].Click();
            }
            catch { }
        }
    }
}