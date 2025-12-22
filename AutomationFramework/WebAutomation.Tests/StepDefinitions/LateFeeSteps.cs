using NUnit.Framework;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using WebAutomation.Tests.Pages;
using WebAutomation.Core.Utilities;
using System.Collections.Generic;

namespace WebAutomation.Tests.StepDefinitions
{
    [Binding]
    public class LateFeeSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly string _excelPath = "TestData/LateFee.xlsx";
        private Dictionary<string, string> _testData;
        private IWebDriver _driver;
        private LoginPage _loginPage;
        private MfaPage _mfaPage;
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
            _loginPage = new LoginPage(_driver);
            _mfaPage = new MfaPage(_driver);
            _dashboardPage = new DashboardPage(_driver);
            _paymentPage = new PaymentPage(_driver);

            var baseUrl = WebAutomation.Core.Configuration.ConfigManager.Settings.BaseUrl;
            _driver.Navigate().GoToUrl(baseUrl);
        }

        [Given(@"logs in with valid credentials")]
        public void GivenLogsInWithValidCredentials()
        {
            _loginPage.LoginWithDefaultCredentials();
        }

        [Given(@"completes MFA verification")]
        public void GivenCompletesMFAVerification()
        {
            _mfaPage.CompleteMfa();
        }

        [Given(@"navigates to the dashboard")]
        public void GivenNavigatesToTheDashboard()
        {
            _dashboardPage.WaitForDashboardToLoad();
        }

        [Given(@"dismisses any pop-ups if present")]
        public void GivenDismissesAnyPopupsIfPresent()
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

        [When(@"continues past the scheduled payment popup if it appears")]
        public void WhenContinuesPastTheScheduledPaymentPopupIfItAppears()
        {
            _paymentPage.ContinueScheduledPaymentIfPresent();
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
            Assert.False(_paymentPage.IsLateFeeMessageDisplayed(), "Late fee message should not be displayed.");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Get TestCaseId from SpecFlow context
            var testCaseId = _scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString();
            _testData = ExcelReader.GetRow(_excelPath, "Sheet1", "TestCaseId", testCaseId);

            _scenarioContext["LoanNumber"] = _testData["LoanNumber"];
            _scenarioContext["PaymentDate"] = _testData["PaymentDate"];
        }
    }
}