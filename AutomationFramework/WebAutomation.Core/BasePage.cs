using OpenQA.Selenium;
using WebAutomation.Core.Utilities;

namespace WebAutomation.Core.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly SmartWait Wait;
        protected readonly PopupHandler Popup;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
            Wait = new SmartWait(driver);
            Popup = new PopupHandler(driver, Wait);
        }
    }
}