using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Widgets;

namespace AutomationFramework.Core.Base
{
    public abstract class BasePage
    {
        protected readonly SelfHealingWebDriver Driver;

        protected BasePage(SelfHealingWebDriver driver)
        {
            Driver = driver;
        }

        protected void HandleUniversalPopups()
        {
            UniversalPopupHandler.HandlePopups(Driver);
        }
    }
}