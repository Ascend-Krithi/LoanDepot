// TestCaseID: TC01 – HELOC Late Fee Validation
using AutomationFramework.Core.Base;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.Widgets;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class TC01StepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly TestDataReader _testDataReader;
        private readonly UniversalPopupHandler _popupHandler;

        public TC01StepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _testDataReader = (TestDataReader)_scenarioContext["TestDataReader"];
            _popupHandler = (UniversalPopupHandler)_scenarioContext["PopupHandler"];
        }

        [Given(@"the user has valid credentials and a HELOC loan available")]
        public void GivenTheUserHasValidCredentials()
        {
            // Credentials and loan data are loaded in Hooks
        }

        [When(@"the user launches the customer servicing application")]
        public void WhenUserLaunchesApplication()
        {
            // Driver is initialized in Hooks
        }

        [When(@"logs in using valid customer credentials")]
        public void WhenLogsIn()
        {
            _popupHandler.HandleAllPopups();
            // LoginPage.Login(...)
        }

        [When(@"completes MFA verification")]
        public void WhenCompletesMfa()
        {
            _popupHandler.HandleAllPopups();
            // MfaPage.CompleteMfa(...)
        }

        [When(@"navigates to the dashboard")]
        public void WhenNavigatesToDashboard()
        {
            _popupHandler.HandleAllPopups();
            // DashboardPage.NavigateTo()
        }

        [When(@"closes or dismisses any pop-ups if they appear")]
        public void WhenClosesPopups()
        {
            _popupHandler.HandleAllPopups();
        }

        [When(@"selects the applicable loan account")]
        public void WhenSelectsLoanAccount()
        {
            // DashboardPage.SelectLoanAccount(...)
        }

        [When(@"clicks Make a Payment")]
        public void WhenClicksMakePayment()
        {
            _popupHandler.HandleAllPopups();
            // DashboardPage.ClickMakePayment()
        }

        [When(@"if a scheduled payment popup appears, clicks Continue")]
        public void WhenScheduledPaymentPopupContinue()
        {
            _popupHandler.HandleAllPopups();
        }

        [When(@"opens the payment date picker")]
        public void WhenOpensDatePicker()
        {
            // PaymentPage.OpenDatePicker()
        }

        [When(@"selects the payment date from test data less than 15 days past due")]
        public void WhenSelectsPaymentDate()
        {
            var data = _testDataReader.GetDataByTestCaseId("TC01");
            var paymentDate = data["PaymentDate"];
            // PaymentPage.SelectPaymentDate(paymentDate)
        }

        [Then(@"no late fee message is displayed")]
        public void ThenNoLateFeeMessage()
        {
            // Assert.IsFalse(PaymentPage.IsLateFeeMessageDisplayed())
        }
    }
}