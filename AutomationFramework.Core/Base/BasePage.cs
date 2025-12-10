using OpenQA.Selenium;

namespace AutomationFramework.Core.Base
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        // Common navigation, wait, and popup handling methods would go here.
    }
}