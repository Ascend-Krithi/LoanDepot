using System;
using TechTalk.SpecFlow;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Utilities;
using NUnit.Framework;
using System.Collections.Generic;
using System.Globalization;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class LateFeeValidationSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private static SelfHealingWebDriver _driver;
        private static LoginPage _loginPage;
        private static MFAPage _mfaPage;
        private static OtpPage _otpPage;
        private static DashboardPage _dashboardPage;
        private static MakePaymentPage _makePaymentPage;

        // Test data
        private string _loanNumber;
        private string _paymentDate;
        private string _state;
        private bool _expectedLateFee;

        public LateFeeValidationSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _driver = WebDriverFactory.CreateDriver();
            _loginPage = new LoginPage(_driver);
            _mfaPage = new MFAPage(_driver);
            _otpPage = new OtpPage(_driver);
            _dashboardPage = new DashboardPage(_driver);
            _makePaymentPage = new MakePaymentPage(_driver);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                if (_driver != null)
                {
                    _driver.Quit();
                }
            }
            catch { }
        }

        [Given(@"I launch the customer servicing application")]
        public void GivenILaunchTheCustomerServicingApplication()
        {
            var url = ConfigManager.Settings.BaseUrl;
            _driver.Navigate().GoToUrl(url);
            _loginPage.WaitForPageReady();
        }

        [When(@"I log in with valid customer credentials")]
        public void WhenILogInWithValidCustomerCredentials()
        {
            var username = ConfigManager.Settings.Credentials.Username;
            var password = ConfigManager.Settings.Credentials.Password;
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);
            _loginPage.ClickSubmit();
        }

        [When(@"I complete MFA verification")]
        public void WhenICompleteMFAVerification()
        {
            _mfaPage.WaitForPageReady();
            var mfaEmail = ConfigManager.Settings.Credentials.Username;
            _mfaPage.EnterEmail(mfaEmail);
            _mfaPage.ClickRequestEmailCode();

            _otpPage.WaitForPageReady();
            var otpCode = Environment.GetEnvironmentVariable("OTP_CODE") ?? "000000"; // fallback stub
            _otpPage.EnterCode(otpCode);
            _otpPage.ClickVerify();
        }

        [When(@"I navigate to the dashboard")]
        public void WhenINavigateToTheDashboard()
        {
            _dashboardPage.WaitForPageReady();
        }

        [When(@"I dismiss any pop-ups if present")]
        public void WhenIDismissAnyPopUpsIfPresent()
        {
            // Popups are handled by PopupEngine in BasePage automatically
        }

        [When(@"I select the loan account ""(.*)"")]
        public void WhenISelectTheLoanAccount(string loanNumber)
        {
            _dashboardPage.OpenLoanDropdown();
            _dashboardPage.SelectLoanAccount(loanNumber);
        }

        [When(@"I click Make a Payment")]
        public void WhenIClickMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [When(@"I continue past any scheduled payment popup if present")]
        public void WhenIContinuePastAnyScheduledPaymentPopupIfPresent()
        {
            // If scheduled payment modal appears, click Continue
            try
            {
                _makePaymentPage.ClickContinue();
            }
            catch
            {
                // Modal not present, continue
            }
        }

        [When(@"I open the payment date picker")]
        public void WhenIOpenThePaymentDatePicker()
        {
            _makePaymentPage.WaitForPageReady();
            _makePaymentPage.OpenDatePicker();
        }

        [When(@"I select the payment date ""(.*)"")]
        public void WhenISelectThePaymentDate(string paymentDate)
        {
            // Parse date for calendar selection
            DateTime dt = DateTime.ParseExact(paymentDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            _makePaymentPage.SelectCalendarYear(dt.Year.ToString());
            _makePaymentPage.SelectCalendarMonth(dt.ToString("MMM", CultureInfo.InvariantCulture));
            _makePaymentPage.SelectCalendarDay(dt.Day.ToString());
        }

        [Then(@"no late fee message should be displayed")]
        public void ThenNoLateFeeMessageShouldBeDisplayed()
        {
            bool isLateFeeDisplayed = _makePaymentPage.IsLateFeeMessageDisplayed();
            Assert.IsFalse(isLateFeeDisplayed, "Late fee message should NOT be displayed for payment less than 15 days past due.");
        }

        [Scope(Tag = "TestCaseId=TC01")]
        [BeforeScenario]
        public void LoadTestData()
        {
            // Read test data from Excel
            var data = ExcelReader.ReadSheet("TestData.xlsx", "Sheet1");
            var headers = data[0];
            foreach (var row in data)
            {
                if (row.Count > 0 && row[0] == "TC01")
                {
                    _loanNumber = row[headers.IndexOf("LoanNumber")];
                    _paymentDate = row[headers.IndexOf("PaymentDate")];
                    _state = row[headers.IndexOf("State")];
                    _expectedLateFee = bool.Parse(row[headers.IndexOf("ExpectedLateFee")]);
                    break;
                }
            }
        }
    }
}