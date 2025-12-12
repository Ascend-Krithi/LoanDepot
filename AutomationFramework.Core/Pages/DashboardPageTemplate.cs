using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPageTemplate : BasePage
    {
        public const string HeaderKey = "DashboardPage.Header";
        public const string TableKey = "DashboardPage.Table";
        public const string DropdownKey = "DashboardPage.Dropdown";

        private readonly By _header = By.CssSelector("header, h1, h2");
        private readonly By _table = By.CssSelector("table");
        private readonly By _dropdown = By.CssSelector("select, [role='listbox']");

        public DashboardPageTemplate(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement Header => FindElement(HeaderKey, _header);
        public IWebElement Table => FindElement(TableKey, _table);
        public IWebElement Dropdown => FindElement(DropdownKey, _dropdown);
    }
}