using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPageTemplate : BasePage
    {
        // Logical Keys
        private const string HeaderKey = "DashboardPage.Header";
        private const string MainTableKey = "DashboardPage.MainTable";
        private const string FilterDropdownKey = "DashboardPage.FilterDropdown";

        // Locators
        private readonly By _header = By.CssSelector("h1, h2, .page-title, [role='heading']");
        private readonly By _mainTable = By.TagName("table");
        private readonly By _filterDropdown = By.CssSelector("select[name*='filter'], select[id*='filter']");

        public DashboardPageTemplate(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement Header => FindElement(HeaderKey, _header);
        public IWebElement MainTable => FindElement(MainTableKey, _mainTable);
        public IWebElement FilterDropdown => FindElement(FilterDropdownKey, _filterDropdown);
    }
}