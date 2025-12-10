using OpenQA.Selenium;

namespace AutomationFramework.Core.Base
{
    public abstract class BasePage
    {
        protected IWebDriver Driver;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        // Common page methods can be added here
    }
}