using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Locators;

namespace AutomationFramework.Core.Pages
{
    public class LoanPage
    {
        private readonly IWebDriver _driver;
        private readonly SelfHealingWebDriver _sh;

        public LoanPage(IWebDriver driver)
        {
            _driver = driver;
            _sh = new SelfHealingWebDriver(driver);
        }

        public bool IsRecentActivityVisible()
        {
            return _sh.IsVisible(null, LoanPageLocators.RecentActivityTableByXpath, LoanPageLocators.RecentActivityTableByCss);
        }

        public void ClickLoansNav()
        {
            _sh.Click(null, LoanPageLocators.LoansLinkByXpath, LoanPageLocators.LoansLinkByCss);
        }

        public void ClickCreateNewLoan()
        {
            _sh.Click(null, LoanPageLocators.CreateNewLoanButtonByXpath, LoanPageLocators.CreateNewLoanButtonByCss);
        }

        public void OpenFirstLoanRow()
        {
            _sh.Click(null, LoanPageLocators.FirstLoanRowByXpath, null);
        }

        public void ClickMakePayment()
        {
            _sh.Click(null, LoanPageLocators.MakePaymentButtonByXpath, LoanPageLocators.MakePaymentButtonByCss);
        }

        public void OpenEscrowTab()
        {
            _sh.Click(null, LoanPageLocators.EscrowTabByXpath, LoanPageLocators.EscrowTabByCss);
        }
    }
}
