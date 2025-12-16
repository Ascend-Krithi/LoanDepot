using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.Models;
using TechTalk.SpecFlow;
using Xunit;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class LateFeeValidationSteps
    {
        private readonly SelfHealingWebDriver _driver;
        private readonly LoginPage _loginPage;
        private readonly MFAPage _mfaPage;
        private readonly OtpPage _otpPage;
        private readonly DashboardPage _dashboardPage;
        private readonly MakePaymentPage _makePaymentPage;
        private PaymentScenario _scenario;

        public LateFeeValidationSteps(SelfHealingWebDriver driver,
                                      LoginPage loginPage,
                                      MFAPage mfaPage,
                                      OtpPage otpPage,
                                      DashboardPage dashboardPage,
                                      MakePaymentPage makePaymentPage)
        {
            _driver = driver;
            _loginPage = loginPage;
            _mfaPage = mfaPage;
            _otpPage = otpPage;
            _dashboardPage = dashboardPage;
            _makePaymentPage = makePaymentPage;
        }

        [Given(@"I launch the customer servicing application")]
        public void GivenILaunchTheCustomerServicingApplication()
        {
            _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            _loginPage.WaitForPageReady();
        }

        [Given(@"I log in with valid credentials")]
        public void GivenILogInWithValidCredentials()
        {
            _loginPage.EnterUsername(ConfigManager.Settings.Credentials.Username);
            _loginPage.EnterPassword(ConfigManager.Settings.Credentials.Password);
            _loginPage.ClickSubmit();
            _mfaPage.WaitForPageReady();
        }

        [Given(@"I complete MFA verification")]
        public void GivenICompleteMFAVerification()
        {
            _mfaPage.EnterEmail(ConfigManager.Settings.Credentials.Username);
            _mfaPage.ClickRequestEmailCode();
            _otpPage.WaitForPageReady();
            var otp = Environment.GetEnvironmentVariable("OTP_CODE") ?? ConfigManager.Settings.Encryption?.Key ?? "123456";
            _otpPage.EnterCode(otp);
            _otpPage.ClickVerify();
        }

        [Given(@"I navigate to the dashboard")]
        public void GivenINavigateToTheDashboard()
        {
            _dashboardPage.WaitForPageReady();
        }

        [Given(@"I close any pop-ups if present")]
        public void GivenICloseAnyPopUpsIfPresent()
        {
            AutomationFramework.Core.Utilities.PopupEngine.DismissPopups(_driver.InnerDriver);
        }

        [Given(@"I select the loan account ""(.*)""")]
        public void GivenISelectTheLoanAccount(string loanNumber)
        {
            _dashboardPage.OpenLoanDropdown();
            _dashboardPage.SelectLoanAccount(loanNumber);
        }

        [Given(@"I click Make a Payment")]
        public void GivenIClickMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [Given(@"I handle scheduled payment popup if present")]
        public void GivenIHandleScheduledPaymentPopupIfPresent()
        {
            // If scheduled payment modal appears, click continue
            try
            {
                _makePaymentPage.WaitForPageReady();
                _makePaymentPage.ClickContinue();
            }
            catch { /* Modal not present, continue */ }
        }

        [Given(@"I open the payment date picker")]
        public void GivenIOpenThePaymentDatePicker()
        {
            _makePaymentPage.OpenDatePicker();
        }

        [Given(@"I select the payment date ""(.*)""")]
        public void GivenISelectThePaymentDate(string paymentDate)
        {
            // Assume paymentDate is in yyyy-MM-dd format
            var date = DateTime.Parse(paymentDate);
            _makePaymentPage.SelectCalendarYear(date.Year.ToString());
            _makePaymentPage.SelectCalendarMonth(date.ToString("MMMM"));
            _makePaymentPage.SelectCalendarDay(date.Day.ToString());
        }

        [Then(@"the late fee message should be (.*)")]
        public void ThenTheLateFeeMessageShouldBe(bool expectedLateFee)
        {
            var actualLateFee = _makePaymentPage.IsLateFeeMessageDisplayed();
            Assert.Equal(expectedLateFee, actualLateFee);
        }
    }
}