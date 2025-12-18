using TechTalk.SpecFlow;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.Models;
using FluentAssertions;
using System;
using System.Globalization;

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
            var url = Core.Configuration.ConfigManager.Settings.BaseUrl;
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
            var creds = Core.Configuration.ConfigManager.Settings.Credentials;
            _loginPage.EnterUsername(creds.Username);
            _loginPage.EnterPassword(creds.Password);
            _loginPage.ClickSubmit();
        }

        [Given(@"I complete MFA verification")]
        public void GivenICompleteMFAVerification()
        {
            _mfaPage.WaitForPageReady();
            var creds = Core.Configuration.ConfigManager.Settings.Credentials;
            _mfaPage.EnterEmail(creds.Username);
            _mfaPage.ClickRequestEmailCode();

            _otpPage.WaitForPageReady();
            var otp = TestDataHelper.GetOtp();
            _otpPage.EnterCode(otp);
            _otpPage.ClickVerify();
        }

        [Given(@"I am redirected to the dashboard")]
        public void GivenIAmRedirectedToTheDashboard()
        {
            _dashboardPage.WaitForPageReady();
        }

        [Given(@"I dismiss any pop-ups")]
        public void GivenIDismissAnyPopUps()
        {
            PopupEngine.DismissPopups(_dashboardPage.Driver.InnerDriver);
        }

        [Given(@"I select the applicable loan account")]
        public void GivenISelectTheApplicableLoanAccount()
        {
            var testData = ExcelReader.GetTestData<PaymentScenario>("TC01");
            _scenarioContext["PaymentScenario"] = testData;
            _dashboardPage.OpenLoanDropdown();
            _dashboardPage.SelectLoanAccount(testData.LoanAccount);
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

        [When(@"I select the payment date ""(.*)""")]
        public void WhenISelectThePaymentDate(string paymentDate)
        {
            DateTime date = DateTime.Parse(paymentDate, CultureInfo.InvariantCulture);
            _makePaymentPage.SelectCalendarYear(date.Year.ToString());
            _makePaymentPage.SelectCalendarMonth(date.ToString("MMM", CultureInfo.InvariantCulture));
            _makePaymentPage.SelectCalendarDay(date.Day.ToString());
        }

        [Then(@"no late fee message should be displayed")]
        public void ThenNoLateFeeMessageShouldBeDisplayed()
        {
            _makePaymentPage.IsLateFeeMessageDisplayed().Should().BeFalse("No late fee message should be displayed for payment date less than 15 days past due.");
        }
    }
}