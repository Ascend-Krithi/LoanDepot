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
            try
            {
                // Example: Chatbot iframe
                var iframes = _driver.FindElements(By.TagName("iframe"));
                foreach (var iframe in iframes)
                {
                    if (iframe.GetAttribute("title")?.ToLower().Contains("chat") == true)
                    {
                        _driver.SwitchTo().Frame(iframe);
                        var closeBtn = _driver.FindElements(By.CssSelector("[aria-label='Close'], .close, .close-btn")).FirstOrDefault();
                        closeBtn?.Click();
                        _driver.SwitchTo().DefaultContent();
                    }
                }

                // Example: Contact Update popup
                var updateLaterBtn = _driver.FindElements(By.XPath("//*[text()='Update Later' or text()='Skip']")).FirstOrDefault();
                updateLaterBtn?.Click();

                // Example: Scheduled payment popup
                var continueBtn = _driver.FindElements(By.XPath("//*[text()='Continue']")).FirstOrDefault();
                continueBtn?.Click();

                // Example: Generic modal close
                var modalClose = _driver.FindElements(By.CssSelector(".modal [aria-label='Close'], .modal .close")).FirstOrDefault();
                modalClose?.Click();
            }
            catch (Exception)
            {
                // Log and ignore popup handler errors
            }
        }
    }
}