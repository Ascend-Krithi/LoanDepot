using TechTalk.SpecFlow;
using AutomationFramework.Pages;
using NUnit.Framework;

namespace AutomationFramework.StepDefinitions
{
    [Binding]
    public class MakePaymentSteps
    {
        private readonly LoginPage _loginPage;
        private readonly MfaPage _mfaPage;
        private readonly DashboardPage _dashboardPage;
        private readonly PaymentPage _paymentPage;

        public MakePaymentSteps()
        {
            _loginPage = new LoginPage();
            _mfaPage = new MfaPage();
            _dashboardPage = new DashboardPage();
            _paymentPage = new PaymentPage();
        }

        [Given(@"the customer servicing application is launched")]
        public void GivenTheApplicationIsLaunched()
        {
            _loginPage.LaunchApplication();
            Assert.IsTrue(_loginPage.IsPageReady(), "Sign-In screen did not load.");
        }

        [Given(@"I log in with valid customer credentials")]
        public void GivenILogInWithValidCredentials()
        {
            _loginPage.Login("testuser@example.com", "Password123"); // Replace with test data source
            Assert.IsTrue(_mfaPage.IsMfaDialogDisplayed(), "MFA verification screen not displayed.");
        }

        [Given(@"I complete MFA verification")]
        public void GivenICompleteMfaVerification()
        {
            _mfaPage.SelectEmailMethod();
            _mfaPage.SendCode();
            _mfaPage.EnterOtpCode("123456"); // Replace with test data source
            _mfaPage.VerifyCode();
            Assert.IsTrue(_dashboardPage.IsPageReady(), "Dashboard not loaded after MFA.");
        }

        [Given(@"I am on the dashboard")]
        public void GivenIAmOnTheDashboard()
        {
            Assert.IsTrue(_dashboardPage.IsPageReady(), "Dashboard page not loaded.");
        }

        [Given(@"I dismiss any pop-ups if they appear")]
        public void GivenIDismissAnyPopupsIfTheyAppear()
        {
            _dashboardPage.DismissPopups();
        }

        [When(@"I select the applicable loan account")]
        public void WhenISelectTheApplicableLoanAccount()
        {
            _dashboardPage.SelectLoanAccount("123456789"); // Replace with test data source
            Assert.IsTrue(_dashboardPage.IsLoanDetailsLoaded(), "Loan details did not load.");
        }

        [When(@"I click Make a Payment")]
        public void WhenIClickMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
            Assert.IsTrue(_paymentPage.IsScheduledPaymentPopupDisplayed() || _paymentPage.IsPageReady(), "Scheduled payment popup or Make a Payment screen not displayed.");
        }

        [When(@"I continue past any scheduled payment popup")]
        public void WhenIContinuePastAnyScheduledPaymentPopup()
        {
            if (_paymentPage.IsScheduledPaymentPopupDisplayed())
            {
                _paymentPage.ClickContinueOnScheduledPaymentPopup();
            }
            Assert.IsTrue(_paymentPage.IsPageReady(), "Make a Payment screen not loaded.");
        }

        [When(@"I open the payment date picker")]
        public void WhenIOpenThePaymentDatePicker()
        {
            _paymentPage.OpenDatePicker();
            Assert.IsTrue(_paymentPage.IsCalendarDisplayed(), "Calendar widget not displayed.");
        }

        [When(@"I select the payment date from test data")]
        public void WhenISelectThePaymentDateFromTestData()
        {
            _paymentPage.SelectPaymentDate("2024-06-10"); // Replace with test data source
            Assert.IsTrue(_paymentPage.IsDateSelected("2024-06-10"), "Payment date not selected.");
        }

        [Then(@"no late fee message is displayed")]
        public void ThenNoLateFeeMessageIsDisplayed()
        {
            Assert.IsFalse(_paymentPage.IsLateFeeMessageDisplayed(), "Late fee message was displayed.");
        }
    }
}