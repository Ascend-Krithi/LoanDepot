using OpenQA.Selenium;
using System;
using System.Linq;

namespace AutomationFramework.Core.Widgets
{
    public class UniversalPopupHandler
    {
        private readonly IWebDriver _driver;

        public UniversalPopupHandler(IWebDriver driver)
        {
            _driver = driver;
        }

        public void HandleAllPopups()
        {
            // Chatbot iframe
            try
            {
                var chatIframe = _driver.FindElements(By.CssSelector("iframe[src*='chat']"));
                if (chatIframe.Any())
                {
                    _driver.SwitchTo().Frame(chatIframe.First());
                    var closeBtn = _driver.FindElements(By.CssSelector(".close, .close-btn, .close-button"));
                    if (closeBtn.Any())
                        closeBtn.First().Click();
                    _driver.SwitchTo().DefaultContent();
                }
            }
            catch { }

            // Contact Update Popup
            try
            {
                var updateLaterBtn = _driver.FindElements(By.XPath("//button[contains(text(),'Update Later')]"));
                if (updateLaterBtn.Any())
                    updateLaterBtn.First().Click();
            }
            catch { }

            // Scheduled Payment Popup
            try
            {
                var continueBtn = _driver.FindElements(By.XPath("//button[contains(text(),'Continue')]"));
                if (continueBtn.Any())
                    continueBtn.First().Click();
            }
            catch { }

            // Banners/Modals
            try
            {
                var banners = _driver.FindElements(By.CssSelector(".modal, .banner, .popup"));
                foreach (var banner in banners)
                {
                    var close = banner.FindElements(By.CssSelector(".close, .close-btn, .close-button"));
                    if (close.Any())
                        close.First().Click();
                }
            }
            catch { }
        }
    }
}