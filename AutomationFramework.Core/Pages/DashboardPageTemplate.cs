// AutomationFramework.Core/Pages/DashboardPageTemplate.cs
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
        private readonly By _header = By.CssSelector("header, .main-header, #header");
        private readonly By _mainTable = By.CssSelector("table, .data-grid, [role='grid']");
        private readonly By _filterDropdown = By.CssSelector("select[name*='filter'], select[id*='filter']");

        public DashboardPageTemplate(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement Header => FindElement(HeaderKey, _header);
        public IWebElement MainTable => FindElement(MainTableKey, _mainTable);
        public IWebElement FilterDropdown => FindElement(FilterDropdownKey, _filterDropdown);
    }
}