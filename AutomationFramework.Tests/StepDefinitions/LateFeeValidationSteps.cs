using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Models;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Utilities;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

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
        private PaymentScenario _scenarioData;

        public LateFeeValidationSteps()
        {
            _driver = (SelfHealingWebDriver)ScenarioContext.Current["Driver"];
            _loginPage = (LoginPage)ScenarioContext.Current["LoginPage"];
            _mfaPage = (MFAPage)ScenarioContext.Current["MFAPage"];
            _otpPage = (OtpPage)ScenarioContext.Current["OtpPage"];
            _dashboardPage = (DashboardPage)ScenarioContext.Current["DashboardPage"];
            _makePaymentPage = (MakePaymentPage)ScenarioContext.Current["MakePaymentPage"];
        }

        [Given(@"I launch the application")]
        public void GivenILaunchTheApplication()
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
        }

        [Given(@"I complete MFA verification")]
        public void GivenICompleteMFAVerification()
        {
            _mfaPage.WaitForPageReady();
            _mfaPage.EnterEmail(ConfigManager.Settings.Credentials.Username);
            _mfaPage.ClickRequestEmailCode();
            _otpPage.WaitForPageReady();
            var otp = Environment.GetEnvironmentVariable("OTP_CODE") ?? ConfigManager.Settings.OtpCode;
            _otpPage.EnterCode(otp);
            _otpPage.ClickVerify();
        }

        [Given(@"I am on the dashboard")]
        public void GivenIAmOnTheDashboard()
        {
            _dashboardPage.WaitForPageReady();
        }

        [Given(@"I dismiss any popups")]
        public void GivenIDismissAnyPopups()
        {
            PopupEngine.DismissPopups(_driver.InnerDriver);
        }

        [Given(@"I select the loan account ""(.*)""")]
        public void GivenISelectTheLoanAccount(string loanNumber)
        {
            _dashboardPage.OpenLoanDropdown();
            _dashboardPage.SelectLoanAccount(loanNumber);
        }

        [When(@"I click Make a Payment")]
        public void WhenIClickMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [When(@"I continue past any scheduled payment popup")]
        public void WhenIContinuePastAnyScheduledPaymentPopup()
        {
            // If scheduled payment popup appears, click Continue
            try
            {
                _makePaymentPage.WaitForPageReady();
            }
            catch
            {
                _makePaymentPage.ClickContinue();
                _makePaymentPage.WaitForPageReady();
            }
        }

        [When(@"I open the payment date picker")]
        public void WhenIOpenThePaymentDatePicker()
        {
            _makePaymentPage.OpenDatePicker();
        }

        [When(@"I select the payment date ""(.*)""")]
        public void WhenISelectThePaymentDate(string paymentDate)
        {
            var date = DateTime.Parse(paymentDate);
            _makePaymentPage.SelectCalendarYear(date.Year.ToString());
            _makePaymentPage.SelectCalendarMonth(date.ToString("MMM"));
            _makePaymentPage.SelectCalendarDay(date.Day.ToString());
        }

        [Then(@"the late fee message displayed should be (.*)")]
        public void ThenTheLateFeeMessageDisplayedShouldBe(bool expectedLateFee)
        {
            var actualLateFee = _makePaymentPage.IsLateFeeMessageDisplayed();
            Assert.AreEqual(expectedLateFee, actualLateFee, $"Expected late fee message: {expectedLateFee}, but was: {actualLateFee}");
        }
    }