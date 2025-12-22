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

        public void WaitForDashboardToLoad()
        {
            Wait.UntilVisible(_locators.GetBy("Dashboard.PageReady"));
        }

        public void DismissPopupsIfPresent()
        {
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactUpdateLater"));
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactContinue"));
        }

        public void SelectLoanAccount(string loanNumber)
        {
            Wait.UntilClickable(_locators.GetBy("Dashboard.LoanSelector.Button")).Click();
            Wait.UntilClickable(_locators.GetBy("Dashboard.LoanCard.ByAccount", loanNumber)).Click();
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(_locators.GetBy("Dashboard.MakePayment.Button")).Click();
        }
    }
}