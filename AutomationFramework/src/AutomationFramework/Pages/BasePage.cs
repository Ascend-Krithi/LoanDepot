using AutomationFramework.Core;
using OpenQA.Selenium;

namespace AutomationFramework.Pages
{
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver;
        }

        protected void LogStep(string message)
        {
            Logger.Info($"[Page Step] {GetType().Name}: {message}");
        }

        // Placeholder Wait/Navigation utilities
        public void NavigateTo(string url)
        {
            LogStep($"Navigate to URL: {url}");
            Driver.Navigate().GoToUrl(url);
        }
    }
}