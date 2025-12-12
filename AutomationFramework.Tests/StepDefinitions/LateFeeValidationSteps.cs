using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Models;
using AutomationFramework.Core.Utilities;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class LateFeeValidationSteps
    {
        private readonly LoginPageTemplate loginPage;
        private readonly MFAVerificationPageTemplate mfaPage;
        private readonly OTPVerificationPageTemplate otpPage;
        private readonly DashboardPageTemplate dashboardPage;
        private readonly PaymentPageTemplate paymentPage;
        private readonly PopupEngine popupEngine;
        private readonly ScenarioContext scenarioContext;

        public LateFeeValidationSteps(
            LoginPageTemplate loginPage,
            MFAVerificationPageTemplate mfaPage,
            OTPVerificationPageTemplate otpPage,
            DashboardPageTemplate dashboardPage,
            PaymentPageTemplate paymentPage,
            PopupEngine popupEngine,
            ScenarioContext scenarioContext)
        {
            this.loginPage = loginPage;
            this.mfaPage = mfaPage;
            this.otpPage = otpPage;
            this.dashboardPage = dashboardPage;
            this.paymentPage = paymentPage;
            this.popupEngine = popupEngine;
            this.scenarioContext = scenarioContext;
        }

        [Given(@"I launch the customer servicing application")]
        public void GivenILaunchTheCustomerServicingApplication()
        {
            loginPage.OpenApplication();
        }

        [Given(@"I log in using valid credentials")]
        public void GivenILogInUsingValidCredentials()
        {
            var testData = scenarioContext.Get<PaymentTestDataModel>("TestData");
            loginPage.EnterEmail(testData.Email);
            loginPage.EnterPassword(testData.Password);
            loginPage.ClickSignIn();
        }

        [Given(@"I complete MFA verification")]
        public void GivenICompleteMFAVerification()
        {
            mfaPage.SelectEmailOption();
            mfaPage.ClickReceiveCodeViaEmail();
            otpPage.EnterOTP(scenarioContext.Get<PaymentTestDataModel>("TestData").OTP);
            otpPage.ClickVerify();
        }

        [Given(@"I am on the dashboard page")]
        public void GivenIAmOnTheDashboardPage()
        {
            dashboardPage.WaitForDashboardToLoad();
        }

        [Given(@"I close any pop-ups if present")]
        public void GivenICloseAnyPopUpsIfPresent()
        {
            popupEngine.CloseContactInfoPopupIfPresent();
            popupEngine.CloseChatbotIframeIfPresent();
            popupEngine.CloseOtherModalsIfPresent();
        }

        [Given(@"I select the loan account ""(.*)""")]
        public void GivenISelectTheLoanAccount(string loanNumber)
        {
            dashboardPage.SelectLoanAccount(loanNumber);
        }

        [Given(@"I click Make a Payment")]
        public void GivenIClickMakeAPayment()
        {
            dashboardPage.ClickMakePayment();
        }

        [Given(@"I continue past any scheduled payment popup")]
        public void GivenIContinuePastAnyScheduledPaymentPopup()
        {
            popupEngine.ContinueScheduledPaymentPopupIfPresent();
        }

        [Given(@"I open the payment date picker")]
        public void GivenIOpenThePaymentDatePicker()
        {
            paymentPage.OpenPaymentDatePicker();
        }

        [When(@"I select the payment date ""(.*)""")]
        public void WhenISelectThePaymentDate(string paymentDate)
        {
            paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"the late fee message area should display ""(.*)""")]
        public void ThenTheLateFeeMessageAreaShouldDisplay(string expectedLateFee)
        {
            bool actualLateFeeDisplayed = paymentPage.IsLateFeeMessageDisplayed();
            actualLateFeeDisplayed.Should().Be(bool.Parse(expectedLateFee));
        }
    }
}