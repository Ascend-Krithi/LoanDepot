using TechTalk.SpecFlow;
using WebAutomation.Core.Utilities;
using WebAutomation.Tests.Pages;
using OpenQA.Selenium;
using System.Collections.Generic;
using FluentAssertions;

namespace WebAutomation.Tests.StepDefinitions
{
    [Binding]
    public class LateFeeSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private Dictionary<string, string> _testData;
        private IWebDriver _driver;
        private LoginPage _loginPage;
        private MfaPage _mfaPage;
        private OtpPage _otpPage;
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
            _driver.Navigate().GoToUrl("https://servicing-qa1.loandepotdev.works");
            _loginPage = new LoginPage(_driver);
        }

        [Given(@"I log in as a valid customer")]
        public void GivenILogInAsAValidCustomer()
        {
            _loginPage.LoginWithDefaultCredentials();
            _mfaPage = new MfaPage(_driver);
        }

        [Given(@"I complete MFA verification")]
        public void GivenICompleteMFAVerification()
        {
            _mfaPage.SelectEmailAndSendCode();
            _otpPage = new OtpPage(_driver);
            _otpPage.EnterOtpAndVerify();
        }

        [Given(@"I am on the dashboard")]
        public void GivenIAmOnTheDashboard()
        {
            _dashboardPage = new DashboardPage(_driver);
            _dashboardPage.WaitForDashboardReady();
        }

        [Given(@"I dismiss any pop-ups if present")]
        public void GivenIDismissAnyPopUpsIfPresent()
        {
            _dashboardPage.DismissPopupsIfPresent();
        }

        [Given(@"I select the applicable loan account")]
        public void GivenISelectTheApplicableLoanAccount()
        {
            // Read test data for this scenario
            _testData = ExcelReader.GetRow(
                filePath: "LateFee/LateFee.xlsx",
                sheetName: "Sheet1",
                keyColumn: "TestCaseId",
                keyValue: "Test Case HAP-700 TS-001 TC-001"
            );
            _dashboardPage.SelectLoanAccount(_testData["LoanNumber"]);
        }

        [Given(@"I click Make a Payment")]
        public void GivenIClickMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [Given(@"I handle scheduled payment popup if present")]
        public void GivenIHandleScheduledPaymentPopupIfPresent()
        {
            _dashboardPage.HandleScheduledPaymentPopupIfPresent();
            _paymentPage = new PaymentPage(_driver);
            _paymentPage.WaitForPaymentPageReady();
        }

        [Given(@"I open the payment date picker")]
        public void GivenIOpenThePaymentDatePicker()
        {
            _paymentPage.OpenDatePicker();
        }

        [Given(@"I select the payment date ""(.*)""")]
        public void GivenISelectThePaymentDate(string paymentDate)
        {
            _paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"no late fee message is displayed")]
        public void ThenNoLateFeeMessageIsDisplayed()
        {
            _paymentPage.IsLateFeeMessageDisplayed().Should().BeFalse();
        }
    }
}