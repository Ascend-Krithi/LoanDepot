using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public DashboardPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators/Locators.txt");
        }

        public void DismissPopupsIfPresent()
        {
            // Contact Update popup
            if (Popup.IsPresent(_locators.GetBy("Dashboard.ContactPopup")))
            {
                Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactUpdateLater"));
                Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactContinue"));
            }
            // Chatbot iframe handled by framework utilities
        }

        public void SelectLoanAccount(string loanNumber)
        {
            Wait.UntilVisible(_locators.GetBy("Dashboard.PageReady"));
            Driver.FindElement(_locators.GetBy("Dashboard.LoanSelector.Button")).Click();
            Wait.UntilVisible(_locators.GetBy("Dashboard.LoanCard.ByAccount", loanNumber)).Click();
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(_locators.GetBy("Dashboard.MakePayment.Button")).Click();
        }

        public void ContinueScheduledPaymentIfPresent()
        {
            if (Popup.IsPresent(_locators.GetBy("Dashboard.ContactPopup")))
            {
                Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactContinue"));
            }
        }
    }
}