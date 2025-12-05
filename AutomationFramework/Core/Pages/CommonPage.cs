using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Locators;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class CommonPage
    {
        private readonly SelfHealingWebDriver _sh;
        public CommonPage(IWebDriver driver)
        {
            _sh = new SelfHealingWebDriver(driver);
        }

        public bool IsHomeLinkVisible() => _sh.IsVisible(null, CommonLocators.HomeLinkByXpath, CommonLocators.HomeLinkByCss);
        public bool IsGlobalSearchVisible() => _sh.IsVisible(CommonLocators.GlobalSearchById, CommonLocators.GlobalSearchByXpath, null);
        public void ClickReports() => _sh.Click(null, CommonLocators.ReportsLinkByXpath, CommonLocators.ReportsLinkByCss);
        public void ClickSettings() => _sh.Click(null, CommonLocators.SettingsGearByXpath, CommonLocators.SettingsGearByCss);
    }
}
