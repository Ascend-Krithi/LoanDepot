using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework.Core.Locators
{
    public static class CommonLocators
    {
        public static IEnumerable<By> HomeLink => new List<By>
        {
            By.XPath("//a[contains(@href, '/dashboard')"]),
            By.CssSelector("nav .dashboard-link")
        };

        public static IEnumerable<By> ReportsLink => new List<By>
        {
            By.XPath("//a[contains(text(), 'Reports')]"),
            By.CssSelector("#nav-reports")
        };

        public static IEnumerable<By> SettingsGear => new List<By>
        {
            By.XPath("//i[contains(@class, 'fa-cog')]/.."),
            By.CssSelector(".settings-icon")
        };
    }
}