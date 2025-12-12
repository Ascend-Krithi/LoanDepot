using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPageTemplate : BasePage
    {
        public const string HeaderKey = "DashboardPage.Header";
        public const string TableKey = "DashboardPage.Table";
        public const string DropdownKey = "DashboardPage.Dropdown";

        private readonly By header = By.CssSelector("header, h1, h2");
        private readonly By table = By.CssSelector("table");
        private readonly By dropdown = By.CssSelector("select, .dropdown");

        public DashboardPageTemplate(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement Header => FindElement(HeaderKey, header);
        public IWebElement Table => FindElement(TableKey, table);
        public IWebElement Dropdown => FindElement(DropdownKey, dropdown);
    }
}