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
        private readonly string _excelFilePath = "LateFee/LateFee.xlsx";
        private readonly string _sheetName = "Sheet1";
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

        [Given(@"the user launches the customer servicing application")]
        public void GivenTheUserLaunchesTheCustomerServicingApplication()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _driver.Navigate().GoToUrl(WebAutomation.Core.Configuration.ConfigManager.Settings.BaseUrl);
            _loginPage = new LoginPage(_driver);
        }

        [Given(@"logs in using valid customer credentials")]
        public void GivenLogsInUsingValidCustomerCredentials()
        {
            _loginPage.LoginWithDefaultCredentials();
            _mfaPage = new MfaPage(_driver);
        }

        [Given(@"completes MFA verification")]
        public void GivenCompletesMFAVerification()
        {
            _mfaPage.CompleteMfa();
            _otpPage = new OtpPage(_driver);
        }

        [Given(@"navigates to the dashboard")]
        public void GivenNavigatesToTheDashboard()
        {
            _otpPage.EnterOtpAndVerify();
            _dashboardPage = new DashboardPage(_driver);
        }

        [Given(@"dismisses any pop-ups if present")]
        public void GivenDismissesAnyPopUpsIfPresent()
        {
            _dashboardPage.DismissPopupsIfPresent();
        }

        [Given(@"selects the applicable loan account ""(.*)""")]
        public void GivenSelectsTheApplicableLoanAccount(string loanNumber)
        {
            _dashboardPage.SelectLoanAccount(loanNumber);
        }

        [When(@"the user clicks Make a Payment")]
        public void WhenTheUserClicksMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [When(@"continues past any scheduled payment popup if present")]
        public void WhenContinuesPastAnyScheduledPaymentPopupIfPresent()
        {
            _dashboardPage.ContinueScheduledPaymentIfPresent();
            _paymentPage = new PaymentPage(_driver);
        }

        [When(@"opens the payment date picker")]
        public void WhenOpensThePaymentDatePicker()
        {
            _paymentPage.OpenDatePicker();
        }

        [When(@"selects the payment date ""(.*)""")]
        public void WhenSelectsThePaymentDate(string paymentDate)
        {
            _paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"no late fee message is displayed")]
        public void ThenNoLateFeeMessageIsDisplayed()
        {
            bool isLateFeeMessageDisplayed = _paymentPage.IsLateFeeMessageDisplayed();
            isLateFeeMessageDisplayed.Should().BeFalse("No late fee message should be displayed for payment date less than 15 days past due.");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var testCaseId = _scenarioContext.ScenarioInfo.Title;
            _testData = ExcelReader.GetRow(_excelFilePath, _sheetName, "TestCaseId", testCaseId);
        }
    }
}