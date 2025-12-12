using AutomationFramework.Core.Engines;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public abstract class BasePage
    {
        protected readonly SelfHealingWebDriver Driver;

        protected BasePage(SelfHealingWebDriver driver)
        {
            Driver = driver;
        }

        protected IWebElement FindElement(string logicalKey)
        {
            return Driver.FindElement(logicalKey);
        }
    }
}