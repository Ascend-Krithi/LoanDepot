using AutomationFramework.Core.Models;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.PopupEngine;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class HELOC_LateFeeValidationSteps
    {
        private readonly LoginPageTemplate _loginPage;
        private readonly MFAPageTemplate _mfaPage;
        private readonly OTPVerificationPageTemplate _otpPage;
        private readonly DashboardPageTemplate _dashboardPage;
        private readonly LoanDetailsPageTemplate _loanDetailsPage;
        private readonly PaymentPageTemplate _paymentPage;
        private readonly ScenarioContext _scenarioContext;

        public HELOC_LateFeeValidationSteps(
            LoginPageTemplate loginPage,
            MFAPageTemplate mfaPage,
            OTPVerificationPageTemplate otpPage,
            DashboardPageTemplate dashboardPage,
            LoanDetailsPageTemplate loanDetailsPage,
            PaymentPageTemplate paymentPage,
            ScenarioContext scenarioContext)
        {
            _loginPage = loginPage;
            _mfaPage = mfaPage;
            _otpPage = otpPage;
            _dashboardPage = dashboardPage;
            _loanDetailsPage = loanDetailsPage;
            _paymentPage = paymentPage;
            _scenarioContext = scenarioContext;
        }

        [Given(@"I launch the customer servicing application")]
        public void GivenILaunchTheCustomerServicingApplication()
        {
            var appUrl = ConfigurationHelper.Get("AppUrl");
            _loginPage.NavigateTo(appUrl);
            _loginPage.WaitForPageToLoad();
        }

        [Given(@"I log in with valid customer credentials")]
        public void GivenILogInWithValidCustomerCredentials()
        {
            var credentials = TestDataHelper.GetCredentials();
            _loginPage.EnterEmail(credentials.Username);
            _loginPage.EnterPassword(credentials.Password);
            _loginPage.ClickSignIn();
            _mfaPage.WaitForPageToLoad();
        }

        [Given(@"I complete MFA verification")]
        public void GivenICompleteMFAVerification()
        {
            _mfaPage.SelectFirstEmailOption();
            _mfaPage.ClickReceiveCodeViaEmail();
            _otpPage.WaitForPageToLoad();
            var otp = TestDataHelper.GetOtp();
            _otpPage.EnterOtp(otp);
            _otpPage.ClickVerify();
            _dashboardPage.WaitForPageToLoad();
        }

        [Given(@"I am on the dashboard page")]
        public void GivenIAmOnTheDashboardPage()
        {
            _dashboardPage.CheckLoanAccountsVisible().Should().BeTrue("Loan accounts should be visible on dashboard");
        }

        [Given(@"I dismiss any pop-ups")]
        public void GivenIDismissAnyPopUps()
        {
            _dashboardPage.DismissPopupsIfPresent();
        }

        [Given(@"I select the applicable loan account from test data")]
        public void GivenISelectTheApplicableLoanAccountFromTestData()
        {
            var testData = ExcelReader.GetTestData<LateFeeValidationModel>("TC01");
            _scenarioContext["TestData"] = testData;
            _dashboardPage.SelectLoanAccount(testData.LoanNumber);
            _loanDetailsPage.WaitForPageToLoad();
        }

        [When(@"I click the Make a Payment button")]
        public void WhenIClickTheMakeAPaymentButton()
        {
            _loanDetailsPage.ClickMakeAPayment();
        }

        [When(@"I continue past the scheduled payment popup if it appears")]
        public void WhenIContinuePastTheScheduledPaymentPopupIfItAppears()
        {
            _loanDetailsPage.ContinueScheduledPaymentPopupIfPresent();
            _paymentPage.WaitForPageToLoad();
        }

        [When(@"I open the payment date picker")]
        public void WhenIOpenThePaymentDatePicker()
        {
            _paymentPage.OpenPaymentDatePicker();
        }

        [When(@"I select the payment date from test data")]
        public void WhenISelectThePaymentDateFromTestData()
        {
            var testData = (LateFeeValidationModel)_scenarioContext["TestData"];
            _paymentPage.SelectPaymentDate(testData.PaymentDate);
        }

        [Then(@"I should not see a late fee message")]
        public void ThenIShouldNotSeeALateFeeMessage()
        {
            _paymentPage.IsLateFeeMessageDisplayed().Should().BeFalse("No late fee message should be displayed for payments less than 15 days past due");
        }
    }
}