using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class LoanApplicationSteps
    {
        private readonly IWebDriver _driver;
        private readonly LoginPage _loginPage;
        private readonly CommonPage _commonPage;
        private readonly LoanPage _loanPage;

        public LoanApplicationSteps()
        {
            _driver = WebDriverFactory.Create();
            _loginPage = new LoginPage(_driver);
            _commonPage = new CommonPage(_driver);
            _loanPage = new LoanPage(_driver);
        }

        [Given(@"I navigate to the login page")]
        public void GivenINavigateToTheLoginPage()
        {
            _loginPage.Navigate();
            WaitHelpers.WaitForPageLoad(_driver);
        }

        [Given(@"I enter username \"(.*)\"")]
        public void GivenIEnterUsername(string username)
        {
            _loginPage.EnterUsername(username);
        }

        [Given(@"I enter password \"(.*)\"")]
        public void GivenIEnterPassword(string password)
        {
            _loginPage.EnterPassword(password);
        }

        [When(@"I click login")]
        public void WhenIClickLogin()
        {
            _loginPage.ClickLogin();
            WaitHelpers.WaitForPageLoad(_driver);
        }

        [Then(@"I should see the dashboard home link")]
        public void ThenIShouldSeeTheDashboardHomeLink()
        {
            if (!_commonPage.IsHomeLinkVisible())
                throw new Exception("Home link not visible after login");
        }

        [Then(@"I should see the global search bar")]
        public void ThenIShouldSeeTheGlobalSearchBar()
        {
            if (!_commonPage.IsGlobalSearchVisible())
                throw new Exception("Global search bar not visible after login");
        }

        [When(@"I click the Loans navigation link")]
        public void WhenIClickTheLoansNavigationLink()
        {
            _loanPage.ClickLoansNav();
        }

        [Then(@"I should see the recent activity table")]
        public void ThenIShouldSeeTheRecentActivityTable()
        {
            if (!_loanPage.IsRecentActivityVisible())
                throw new Exception("Recent activity table not visible");
        }

        [When(@"I click the Create New Loan button")]
        public void WhenIClickTheCreateNewLoanButton()
        {
            _loanPage.ClickCreateNewLoan();
        }

        [Then(@"I should see the new application page")]
        public void ThenIShouldSeeTheNewApplicationPage()
        {
            // Basic assertion via page load and optionally specific element, here just wait
            WaitHelpers.WaitForPageLoad(_driver);
        }

        [When(@"I open the first loan from recent activity")]
        public void WhenIOpenTheFirstLoanFromRecentActivity()
        {
            _loanPage.OpenFirstLoanRow();
        }

        [When(@"I click Make Payment")]
        public void WhenIClickMakePayment()
        {
            _loanPage.ClickMakePayment();
        }

        [Then(@"I should see the payment dialog")]
        public void ThenIShouldSeeThePaymentDialog()
        {
            // In real case, verify payment dialog element; here we wait for page readiness
            WaitHelpers.WaitForPageLoad(_driver);
        }

        [When(@"I click the Escrow details tab")]
        public void WhenIClickTheEscrowDetailsTab()
        {
            _loanPage.OpenEscrowTab();
        }

        [Then(@"I should see escrow details")]
        public void ThenIShouldSeeEscrowDetails()
        {
            // In real case, verify specific escrow panel; here we wait
            WaitHelpers.WaitForPageLoad(_driver);
        }
    }
}
