using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class LoanListPageLocators
    {
        public const string LoanRowKey = "LoanList.LoanRow";
        public static readonly By LoanRowLocator = By.XPath("//tr[contains(@class,'loan-row')]");
        public static readonly By[] LoanRowAlternatives = {
            By.CssSelector("tr.loan-row"),
            By.XPath("//tr[td[contains(text(),'Loan')]]")
        };

        public const string PopupCloseKey = "LoanList.PopupClose";
        public static readonly By PopupCloseLocator = By.XPath("//button[@class='close-popup']");
        public static readonly By[] PopupCloseAlternatives = {
            By.CssSelector("button[aria-label='Close']"),
            By.XPath("//button[contains(text(),'Dismiss')]")
        };
    }
}