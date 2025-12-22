using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using WebAutomation.Tests.Pages;
using WebAutomation.Core.Utilities;

namespace WebAutomation.Tests.StepDefinitions
{
    [Binding]
    public class LateFeeSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;
        private Dictionary<string, string> _testData;
        private LoginPage _loginPage;
        private MfaPage _mfaPage;
        private DashboardPage _dashboardPage;
        private PaymentPage _paymentPage;

        public LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the customer servicing application is launched")]
        public void GivenTheCustomerServicingApplicationIsLaunched()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _loginPage = new LoginPage(_driver);
            _mfaPage = new MfaPage(_driver);
            _dashboardPage = new DashboardPage(_driver);
            _paymentPage = new PaymentPage(_driver);

            // Read test data for this scenario
            var featureTitle = _scenarioContext.ScenarioInfo.Title;
            var testCaseId = _scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString();
            _testData = ExcelReader.GetRow(
                filePath: "TestData/LateFee/LateFee.xlsx",
                sheetName: "Sheet1",
                keyColumn: "TestCaseId",
                keyValue: testCaseId
            );

            _loginPage.NavigateToLogin();
            Assert.True(_loginPage.IsPageReady(), "Login page did not load successfully.");
        }

        [Given(@"the user logs in with valid credentials")]
        public void GivenTheUserLogsInWithValidCredentials()
        {
            _loginPage.Login();
            Assert.True(_mfaPage.IsPageReady(), "MFA page did not load after login.");
        }

        [Given(@"the user completes MFA verification")]
        public void GivenTheUserCompletesMFAVerification()
        {
            _mfaPage.SelectEmailAndSendCode();
            _mfaPage.EnterOtpAndVerify();
            Assert.True(_dashboardPage.IsPageReady(), "Dashboard did not load after MFA.");
        }

        [Given(@"the dashboard is loaded")]
        public void GivenTheDashboardIsLoaded()
        {
            Assert.True(_dashboardPage.IsPageReady(), "Dashboard page is not ready.");
        }

        [Given(@"all pop-ups are dismissed")]
        public void GivenAllPopupsAreDismissed()
        {
            _dashboardPage.DismissContactUpdatePopup();
            _dashboardPage.DismissChatbotIframe();
        }

        [Given(@"the user selects the applicable loan account")]
        public void GivenTheUserSelectsTheApplicableLoanAccount()
        {
            var loanNumber = _testData["LoanNumber"];
            _dashboardPage.SelectLoanAccount(loanNumber);
            Assert.True(_dashboardPage.IsLoanDetailsLoaded(loanNumber), "Loan details did not load.");
        }

        [When(@"the user clicks Make a Payment")]
        public void WhenTheUserClicksMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [When(@"the user continues past any scheduled payment popup")]
        public void WhenTheUserContinuesPastAnyScheduledPaymentPopup()
        {
            _dashboardPage.ContinueScheduledPaymentPopupIfPresent();
            Assert.True(_paymentPage.IsPageReady(), "Payment page did not load.");
        }

        [When(@"the user opens the payment date picker")]
        public void WhenTheUserOpensThePaymentDatePicker()
        {
            _paymentPage.OpenDatePicker();
            Assert.True(_paymentPage.IsDatePickerOpen(), "Date picker did not open.");
        }

        [When(@"the user selects the payment date from test data")]
        public void WhenTheUserSelectsThePaymentDateFromTestData()
        {
            var paymentDate = _testData["PaymentDate"];
            _paymentPage.SelectPaymentDate(paymentDate);
            Assert.AreEqual(paymentDate, _paymentPage.GetSelectedPaymentDate(), "Selected payment date does not match.");
        }

        [Then(@"no late fee message is displayed")]
        public void ThenNoLateFeeMessageIsDisplayed()
        {
            Assert.False(_paymentPage.IsLateFeeMessageDisplayed(), "Late fee message was displayed but should not be.");
        }
    }
}