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

        [Given(@"the application is launched")]
        public void GivenTheApplicationIsLaunched()
        {
            // Launch application logic
        }

        [When(@"the user logs in using valid customer credentials")]
        public void WhenTheUserLogsIn()
        {
            // Login logic
        }

        [When(@"completes MFA verification")]
        public void WhenCompletesMfaVerification()
        {
            // MFA logic
        }

        [When(@"navigates to the dashboard")]
        public void WhenNavigatesToDashboard()
        {
            // Dashboard navigation logic
        }

        [When(@"closes or dismisses any pop-ups if they appear")]
        public void WhenClosesOrDismissesPopups()
        {
            _popupHandler.HandleAllPopups();
        }

        [When(@"selects the applicable loan account")]
        public void WhenSelectsLoanAccount()
        {
            // Select loan account logic
        }

        [When(@"clicks Make a Payment")]
        public void WhenClicksMakeAPayment()
        {
            // Click Make a Payment logic
        }

        [When(@"if a scheduled payment popup appears, clicks Continue")]
        public void WhenScheduledPaymentPopupAppearsClicksContinue()
        {
            _popupHandler.HandleAllPopups();
        }

        [When(@"opens the payment date picker")]
        public void WhenOpensPaymentDatePicker()
        {
            // Open date picker logic
        }

        [When(@"selects the payment date from test data \(less than 15 days past due\)")]
        public void WhenSelectsPaymentDateFromTestData()
        {
            var data = _testDataReader.GetDataByTestCaseId("TC01");
            // Use data["PaymentDate"]
        }

        [Then(@"no late fee message is displayed")]
        public void ThenNoLateFeeMessageIsDisplayed()
        {
            // Assert no late fee message
        }
    }
}