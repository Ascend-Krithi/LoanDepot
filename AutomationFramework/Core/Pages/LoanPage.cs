using System;
using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Locators;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.Pages
{
    public class LoanPage
    {
        private readonly SelfHealingWebDriver _driver;

        public LoanPage(SelfHealingWebDriver driver)
        {
            _driver = driver;
        }

        public void NavigateToLoans()
        {
            _driver.FindElementWithFallback(LoanPageLocators.LoansLink).Click();
        }

        public void SearchLoan(string loanNumber)
        {
            var search = _driver.FindElementWithFallback(LoanPageLocators.GlobalSearchBar);
            search.Clear();
            search.SendKeys(loanNumber + Keys.Enter);
        }

        public void OpenFirstLoanFromGrid()
        {
            _driver.FindElementWithFallback(LoanPageLocators.FirstLoanRow).Click();
        }

        public void MakePayment()
        {
            _driver.FindElementWithFallback(LoanPageLocators.MakePaymentButton).Click();
        }

        public void OpenEscrowTab()
        {
            _driver.FindElementWithFallback(LoanPageLocators.EscrowDetailsTab).Click();
        }

        public bool IsRecentActivityTableVisible()
        {
            try
            {
                var table = _driver.FindElementWithFallback(LoanPageLocators.RecentActivityTable);
                return table.Displayed;
            }
            catch { return false; }
        }
    }
}