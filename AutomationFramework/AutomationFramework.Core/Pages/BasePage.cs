using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.Pages
{
    public class BasePage
    {
        protected readonly SelfHealingWebDriver Driver;

        public BasePage(SelfHealingWebDriver driver)
        {
            Driver = driver;
        }

        protected IWebElement FindElement(string logicalKey, By locator, int timeoutSeconds = 10)
        {
            PopupEngine.CleanPopups(Driver.InnerDriver);
            return Driver.FindElement(logicalKey, locator, timeoutSeconds);
        }

        protected void JsClick(IWebElement element)
        {
            ((IJavaScriptExecutor)Driver.InnerDriver).ExecuteScript("arguments[0].click();", element);
        }
    }
}