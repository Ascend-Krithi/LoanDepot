using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework.Core.Locators
{
    public static class LoanPageLocators
    {
        public static IEnumerable<By> LoansLink => new List<By>
        {
            By.XPath("//a[contains(text(), 'Loans') or contains(@href, '/loan-search')"]),
            By.CssSelector("#nav-loans")
        };

        public static IEnumerable<By> GlobalSearchBar => new List<By>
        {
            By.Id("globalSearch"),
            By.XPath("//input[@placeholder='Search...']")
        };

        public static IEnumerable<By> CreateNewLoanButton => new List<By>
        {
            By.XPath("//button[contains(text(), 'Create Loan') or contains(text(), 'New Application')]"),
            By.CssSelector(".btn-create-loan")
        };

        public static IEnumerable<By> RecentActivityTable => new List<By>
        {
            By.XPath("//table[contains(@class, 'activity-table')"]),
            By.CssSelector("#recent-loans-grid")
        };

        public static IEnumerable<By> FirstLoanRow => new List<By>
        {
            By.XPath("(//table//tr)[2]")
        };

        public static IEnumerable<By> MakePaymentButton => new List<By>
        {
            By.XPath("//button[contains(text(), 'Make Payment')"]),
            By.CssSelector(".payment-action")
        };

        public static IEnumerable<By> EscrowDetailsTab => new List<By>
        {
            By.XPath("//div[@role='tab' and contains(text(), 'Escrow')"]),
            By.CssSelector("#tab-escrow")
        };
    }
}