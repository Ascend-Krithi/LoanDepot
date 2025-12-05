using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class CommonLocators
    {
        public static By HomeLinkByXpath => By.XPath("//a[contains(@href, '/dashboard')]");
        public static By HomeLinkByCss => By.CssSelector("nav .dashboard-link");

        public static By ReportsLinkByXpath => By.XPath("//a[contains(text(), 'Reports')]");
        public static By ReportsLinkByCss => By.CssSelector("#nav-reports");

        public static By SettingsGearByXpath => By.XPath("//i[contains(@class, 'fa-cog')]/..");
        public static By SettingsGearByCss => By.CssSelector(".settings-icon");

        public static By GlobalSearchById => By.Id("globalSearch");
        public static By GlobalSearchByXpath => By.XPath("//input[@placeholder='Search...']");
    }
}
