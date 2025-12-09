using AutomationFramework.Core.Drivers;
using OpenQA.Selenium;
using System;

namespace AutomationFramework.Core.Pages
{
    public abstract class BasePage
    {
        protected readonly SelfHealingWebDriver Driver;

        protected BasePage(SelfHealingWebDriver driver)
        {
            Driver = driver;
        }

        protected void SwitchToIFrame(string locatorKey)
        {
            var iframe = Driver.FindElement(locatorKey);
            Driver.SwitchTo().Frame(iframe);
        }

        protected void SwitchToDefaultContent()
        {
            Driver.SwitchTo().DefaultContent();
        }
    }
}