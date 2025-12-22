using OpenQA.Selenium;

namespace AutomationFramework.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly By PageReady = By.CssSelector("header");
        private readonly By LoanSelectorButton = By.CssSelector("button[logaction='Open Loan Selection Window']");
        private readonly string LoanCardByAccountXpath = "//p[contains(normalize-space(.),'Account - {0}')]";
        private readonly By MakePaymentButton = By.CssSelector("p.make-payment");
        private readonly By ChatbotIframe = By.Id("servisbot-messenger-iframe-roundel");
        private readonly By ContactPopup = By.CssSelector("mat-dialog-container");
        private readonly By ContactUpdateLater = By.XPath("//button[normalize-space()='Update Later']");

        public bool IsPageReady()
        {
            return IsElementVisible(PageReady);
        }

        public void DismissPopups()
        {
            if (IsElementVisible(ContactPopup))
            {
                Click(ContactUpdateLater);
            }
            if (IsElementVisible(ChatbotIframe))
            {
                DismissIframe(ChatbotIframe);
            }
            // Add other modal dismissals as needed
        }

        public void SelectLoanAccount(string accountNumber)
        {
            Click(LoanSelectorButton);
            var loanCard = By.XPath(string.Format(LoanCardByAccountXpath, accountNumber));
            Click(loanCard);
        }

        public bool IsLoanDetailsLoaded()
        {
            // Implement check for loan details loaded, e.g., action buttons visible
            return IsElementVisible(MakePaymentButton);
        }

        public void ClickMakePayment()
        {
            Click(MakePaymentButton);
        }
    }
}