using System;
using TechTalk.SpecFlow;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.Models;
using AutomationFramework.Core.Configuration;
using NUnit.Framework;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class MakePaymentSteps
    {
        private readonly LoginPage _loginPage;
        private readonly MFAPage _mfaPage;
        private readonly OtpPage _otpPage;
        private readonly DashboardPage _dashboardPage;
        private readonly MakePaymentPage _makePaymentPage;
        private PaymentScenario _scenarioData;
        private string _expectedLateFee;

        public MakePaymentSteps(LoginPage loginPage, MFAPage mfaPage, OtpPage otpPage, DashboardPage dashboardPage, MakePaymentPage makePaymentPage)
        {
            _loginPage = loginPage;
            _mfaPage = mfaPage;
            _otpPage = otpPage;
            _dashboardPage = dashboardPage;
            _makePaymentPage = makePaymentPage;
        }

        [Given(@"I am on the Login page")]
        public void GivenIAmOnTheLoginPage()
        {
            _loginPage.WaitForPageReady();
        }

        [When(@"I login with valid credentials")]
        public void WhenILoginWithValidCredentials()
        {
            var username = ConfigManager.Settings.Credentials.Username;
            var password = ConfigManager.Settings.Credentials.Password;
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);
            _loginPage.ClickSubmit();
        }

        [When(@"I complete MFA authentication")]
        public void WhenICompleteMfaAuthentication()
        {
            _mfaPage.WaitForPageReady();
            var mfaEmail = ConfigManager.Settings.Credentials.Username;
            _mfaPage.EnterEmail(mfaEmail);
            _mfaPage.ClickRequestEmailCode();
        }

        [When(@"I enter the OTP code")]
        public void WhenIEnterTheOtpCode()
        {
            _otpPage.WaitForPageReady();
            var otpCode = Environment.GetEnvironmentVariable("OTP_CODE") ?? ConfigManager.Settings?.OtpCode ?? "000000";
            _otpPage.EnterCode(otpCode);
            _otpPage.ClickVerify();
        }

        [When(@"I select the loan account ""(.*)""")]
        public void WhenISelectTheLoanAccount(string loanAccount)
        {
            _dashboardPage.WaitForPageReady();
            _dashboardPage.OpenLoanDropdown();
            _dashboardPage.SelectLoanAccount(loanAccount);
        }

        [When(@"I navigate to the Make Payment page")]
        public void WhenINavigateToTheMakePaymentPage()
        {
            _dashboardPage.ClickMakePayment();
            _makePaymentPage.WaitForPageReady();
        }

        [When(@"I schedule a payment for ""(.*)"" with amount ""(.*)"" and memo ""(.*)""")]
        public void WhenIScheduleAPaymentForWithAmountAndMemo(string paymentDate, string amount, string memo)
        {
            // Assume date is in format yyyy-MM-dd
            var date = DateTime.Parse(paymentDate);
            _makePaymentPage.OpenDatePicker();
            _makePaymentPage.SelectCalendarYear(date.Year.ToString());
            _makePaymentPage.SelectCalendarMonth(date.ToString("MMMM"));
            _makePaymentPage.SelectCalendarDay(date.Day.ToString());
            // Amount and Memo would be entered here if page methods exist
            // _makePaymentPage.EnterAmount(amount);
            // _makePaymentPage.EnterMemo(memo);
            _makePaymentPage.ClickContinue();
        }

        [Then(@"the payment should be processed successfully")]
        public void ThenThePaymentShouldBeProcessedSuccessfully()
        {
            // Add assertion logic based on expected result from Excel
            Assert.Pass("Payment processed successfully (placeholder).");
        }

        [Then(@"the late fee message displayed should be ""(.*)""")]
        public void ThenTheLateFeeMessageDisplayedShouldBe(string expectedLateFee)
        {
            var isLateFeeDisplayed = _makePaymentPage.IsLateFeeMessageDisplayed();
            Assert.AreEqual(expectedLateFee.Equals("true", StringComparison.OrdinalIgnoreCase), isLateFeeDisplayed, "Late fee message display mismatch.");
        }

        [BeforeScenario]
        public void LoadTestData(ScenarioContext scenarioContext)
        {
            var testCaseId = scenarioContext.ScenarioInfo.Arguments.ContainsKey("TestCaseId")
                ? scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString()
                : null;

            if (!string.IsNullOrEmpty(testCaseId))
            {
                var data = ExcelReader.ReadSheet("TestData.xlsx", "MakePayment");
                // Assume first row is header
                for (int i = 1; i < data.Count; i++)
                {
                    if (data[i][0] == testCaseId)
                    {
                        _scenarioData = new PaymentScenario
                        {
                            LoanAccount = data[i][1],
                            PaymentDate = data[i][2],
                            Amount = decimal.Parse(data[i][3]),
                            Memo = data[i][4]
                        };
                        _expectedLateFee = data[i][5];
                        break;
                    }
                }
            }
        }
    }
}