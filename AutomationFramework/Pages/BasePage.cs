using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AutomationFramework.Pages
{
    public abstract class BasePage
    {
        protected IWebDriver Driver => DriverManager.Instance.Driver;

        protected bool IsElementVisible(By by, int timeoutSeconds = 10)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                return wait.Until(drv => drv.FindElement(by).Displayed);
            }
            catch
            {
                return false;
            }
        }

        protected void Click(By by)
        {
            Driver.FindElement(by).Click();
        }

        protected void EnterText(By by, string text)
        {
            var element = Driver.FindElement(by);
            element.Clear();
            element.SendKeys(text);
        }

        protected void DismissIframe(By iframeBy)
        {
            // Switch to iframe and close/dismiss if possible
            Driver.SwitchTo().Frame(Driver.FindElement(iframeBy));
            // Implement dismissal logic
            Driver.SwitchTo().DefaultContent();
        }
    }
}