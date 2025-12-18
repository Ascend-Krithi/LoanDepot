using TechTalk.SpecFlow;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.Models;
using FluentAssertions;
using System;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class MakePayment_NoLateFeeSteps
    {
        private readonly LoginPage _loginPage;
        private readonly MFAPage _mfaPage;
        private readonly OtpPage _otpPage;
        private readonly DashboardPage _dashboardPage;
        private readonly MakePaymentPage _makePaymentPage;
        private readonly ScenarioContext _scenarioContext;

        public MakePayment_NoLateFeeSteps(
            LoginPage loginPage,
            MFAPage mfaPage,
            OtpPage otpPage,
            DashboardPage dashboardPage,
            MakePaymentPage makePaymentPage,
            ScenarioContext scenarioContext)
        {
            _loginPage = loginPage;
            _mfaPage = mfaPage;
            _otpPage = otpPage;
            _dashboardPage = dashboardPage;
            _makePaymentPage = makePaymentPage;
            _scenarioContext = scenarioContext;
        }

        [Given(@"I launch the customer servicing application")]
        public void GivenILaunchTheCustomerServicingApplication()
        {
            var url = ConfigManager.Settings.BaseUrl;
            _loginPage.Driver.Navigate().GoToUrl(url);
            _loginPage.WaitForPageReady();
        }

        [Given(@"I am on the Sign-In screen")]
        public void GivenIAmOnTheSignInScreen()
        {
            _loginPage.WaitForPageReady();
        }

        [Given(@"I log in with valid customer credentials")]
        public void GivenILogInWithValidCustomerCredentials()
        {
            var creds = ConfigManager.Settings.Credentials;
            _loginPage.EnterUsername(creds.Username);
            _loginPage.EnterPassword(creds.Password);
            _loginPage.ClickSubmit();
            _mfaPage.WaitForPageReady();
        }

        [Given(@"I complete MFA verification")]
        public void GivenICompleteMFAVerification()
        {
            // For demo, use email from config or test data, and static OTP from helper
            var creds = ConfigManager.Settings.Credentials;
            _mfaPage.EnterEmail(creds.Username);
            _mfaPage.ClickRequestEmailCode();
            _otpPage.WaitForPageReady();
            var otp = TestDataHelper.GetOtp();
            _otpPage.EnterCode(otp);
            _otpPage.ClickVerify();
            _dashboardPage.WaitForPageReady();
        }

        [Given(@"I am on the dashboard")]
        public void GivenIAmOnTheDashboard()
        {
            _dashboardPage.WaitForPageReady();
        }

        [Given(@"I dismiss any pop-ups")]
        public void GivenIDismissAnyPopUps()
        {
            PopupEngine.DismissPopups(_dashboardPage.Driver.InnerDriver);
        }

        [When(@"I select the applicable loan account from test data ""(.*)""")]
        public void WhenISelectTheApplicableLoanAccountFromTestData(string testCaseId)
        {
            var data = ExcelReader.GetTestData<PaymentScenario>(testCaseId);
            _scenarioContext["PaymentScenario"] = data;
            _dashboardPage.OpenLoanDropdown();
            _dashboardPage.SelectLoanAccount(data.LoanAccount);
        }

        [When(@"I click Make a Payment")]
        public void WhenIClickMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [When(@"I continue past the scheduled payment popup if it appears")]
        public void WhenIContinuePastTheScheduledPaymentPopupIfItAppears()
        {
            _makePaymentPage.WaitForPageReady();
            try
            {
                var modal = _makePaymentPage.FindElement("Payment.ScheduledPaymentModal", MakePaymentPage.ScheduledPaymentModal, 3);
                if (modal != null && modal.Displayed)
                {
                    _makePaymentPage.ClickContinue();
                }
            }
            catch { /* Modal not present, continue */ }
        }

        [When(@"I open the payment date picker")]
        public void WhenIOpenThePaymentDatePicker()
        {
            _makePaymentPage.OpenDatePicker();
        }

        [When(@"I select the payment date from test data ""(.*)""")]
        public void WhenISelectThePaymentDateFromTestData(string testCaseId)
        {
            var data = (PaymentScenario)_scenarioContext["PaymentScenario"];
            var date = DateTime.Parse(data.PaymentDate);
            _makePaymentPage.SelectCalendarYear(date.Year.ToString());
            _makePaymentPage.SelectCalendarMonth(date.ToString("MMMM"));
            _makePaymentPage.SelectCalendarDay(date.Day.ToString());
        }

        [Then(@"no late fee message should be displayed")]
        public void ThenNoLateFeeMessageShouldBeDisplayed()
        {
            _makePaymentPage.IsLateFeeMessageDisplayed().Should().BeFalse("No late fee message should be shown for this payment date.");
        }
    }
}