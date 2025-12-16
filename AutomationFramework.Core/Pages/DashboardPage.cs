using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement PageReady() => FindElement("Dashboard.PageReady", By.CssSelector("div.loan-selection-main, div.dashboard-container"));
        public IWebElement LoanDropdown() => FindElement("Dashboard.LoanDropdown", By.CssSelector("button.loan-selector"));
        public IWebElement LoanAccountRow(string account) => FindElement("Dashboard.LoanAccountRow", By.XPath($"//li[normalize-space(text())='{account}']"));
        public IWebElement MakePayment() => FindElement("Dashboard.MakePayment", By.XPath("//button[contains(normalize-space(.),'Make a Payment')]"));
    }
}