using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class LoanPageLocators
    {
        public static By LoansLinkByXpath => By.XPath("//a[contains(text(), 'Loans') or contains(@href, '/loan-search')]");
        public static By LoansLinkByCss => By.CssSelector("#nav-loans");

        public static By RecentActivityTableByXpath => By.XPath("//table[contains(@class, 'activity-table')]");
        public static By RecentActivityTableByCss => By.CssSelector("#recent-loans-grid");

        public static By FirstLoanRowByXpath => By.XPath("(//table//tr)[2]");

        public static By CreateNewLoanButtonByXpath => By.XPath("//button[contains(text(), 'Create Loan') or contains(text(), 'New Application')]");
        public static By CreateNewLoanButtonByCss => By.CssSelector(".btn-create-loan");

        public static By MakePaymentButtonByXpath => By.XPath("//button[contains(text(), 'Make Payment')]");
        public static By MakePaymentButtonByCss => By.CssSelector(".payment-action");

        public static By EscrowTabByXpath => By.XPath("//div[@role='tab' and contains(text(), 'Escrow')]");
        public static By EscrowTabByCss => By.CssSelector("#tab-escrow");
    }
}
