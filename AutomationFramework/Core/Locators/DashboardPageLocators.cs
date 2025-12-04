using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class DashboardPageLocators
    {
        public const string WelcomeMessageKey = "Dashboard.WelcomeMessage";
        public static readonly By WelcomeMessageLocator = By.XPath("//div[@id='welcome']");
        public static readonly By[] WelcomeMessageAlternatives = {
            By.CssSelector("div.welcome"),
            By.XPath("//span[contains(text(),'Welcome')]")
        };

        public const string LoanListDropdownKey = "Dashboard.LoanListDropdown";
        public static readonly By LoanListDropdownLocator = By.XPath("//select[@id='loanList']");
        public static readonly By[] LoanListDropdownAlternatives = {
            By.CssSelector("select.loan-list"),
            By.XPath("//select[contains(@name,'loan')]")
        };
    }
}