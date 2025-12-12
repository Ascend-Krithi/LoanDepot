using OpenQA.Selenium;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly int DefaultTimeout;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            DefaultTimeout = 10;
        }

        protected IWebElement Find(By by)
        {
            return WaitHelper.WaitForElementVisible(Driver, by, DefaultTimeout);
        }

        protected void Click(By by)
        {
            WaitHelper.WaitForElementClickable(Driver, by, DefaultTimeout).Click();
        }

        protected void EnterText(By by, string text)
        {
            var element = Find(by);
            element.Clear();
            element.SendKeys(text);
        }

        public virtual bool IsLoaded()
        {
            return true;
        }
    }
}