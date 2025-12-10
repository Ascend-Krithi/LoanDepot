using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Widgets;

namespace AutomationFramework.Core.Base
{
    public abstract class BasePage
    {
        protected SelfHealingWebDriver Driver { get; private set; }
        protected UniversalPopupHandler PopupHandler { get; private set; }

        protected BasePage(SelfHealingWebDriver driver)
        {
            Driver = driver;
            PopupHandler = new UniversalPopupHandler(driver);
        }

        protected void BeforeCriticalAction()
        {
            PopupHandler.HandlePopups();
        }
    }
}