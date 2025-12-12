using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPageTemplate : BasePage
    {
        private readonly By loanAccountSelector = By.CssSelector("div.loan-account[data-loan-number]");
        private readonly By makePaymentButton = By.Id("makePaymentBtn");

        public void WaitForDashboardToLoad()
        {
            WaitHelper.WaitForElementVisible(loanAccountSelector);
        }

        public void SelectLoanAccount(string loanNumber)
        {
            var account = FindElement("DashboardPage.LoanAccount", By.CssSelector($"div.loan-account[data-loan-number='{loanNumber}']"));
            account.Click();
        }

        public void ClickMakePayment()
        {
            var button = FindElement("DashboardPage.MakePaymentButton", makePaymentButton);
            button.Click();
        }
    }
}