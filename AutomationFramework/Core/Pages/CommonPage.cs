using System;
using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Locators;

namespace AutomationFramework.Core.Pages
{
    public class CommonPage
    {
        private readonly SelfHealingWebDriver _driver;

        public CommonPage(SelfHealingWebDriver driver)
        {
            _driver = driver;
        }

        public bool IsHomeVisible()
        {
            try
            {
                return _driver.FindElementWithFallback(CommonLocators.HomeLink).Displayed;
            }
            catch { return false; }
        }

        public bool IsReportsVisible()
        {
            try
            {
                return _driver.FindElementWithFallback(CommonLocators.ReportsLink).Displayed;
            }
            catch { return false; }
        }

        public void OpenSettings()
        {
            _driver.FindElementWithFallback(CommonLocators.SettingsGear).Click();
        }
    }
}