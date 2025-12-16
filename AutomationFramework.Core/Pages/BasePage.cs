using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
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

        protected IWebElement FindElement(string logicalKey, By locator, int timeoutSeconds = 10)
        {
            PopupEngine.CleanPopups(Driver);
            return Driver.FindElement(logicalKey, locator, timeoutSeconds);
        }
    }
}