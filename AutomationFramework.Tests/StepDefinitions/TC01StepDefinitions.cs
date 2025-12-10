// TestCaseID: TC01 – HELOC Late Fee Validation
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

        [Given(@"the customer servicing application is launched")]
        public void GivenTheCustomerServicingApplicationIsLaunched()
        {
            // Launch logic here
        }

        [When(@"the user logs in using valid customer credentials")]
        public void WhenTheUserLogsInUsingValidCustomerCredentials()
        {
            // Login logic here
        }

        [When(@"completes MFA verification")]
        public void WhenCompletesMFAVerification()
        {
            // MFA logic here
        }

        [When(@"navigates to the dashboard")]
        public void WhenNavigatesToTheDashboard()
        {
            // Navigation logic here
        }

        [When(@"closes or dismisses any pop-ups if they appear")]
        public void WhenClosesOrDismissesAnyPopupsIfTheyAppear()
        {
            _popupHandler.HandleAllPopups();
        }

        [When(@"selects the applicable loan account")]
        public void WhenSelectsTheApplicableLoanAccount()
        {
            // Select loan logic here
        }

        [When(@"clicks Make a Payment")]
        public void WhenClicksMakeAPayment()
        {
            // Click Make a Payment logic here
        }

        [When(@"if a scheduled payment popup appears, clicks Continue")]
        public void WhenIfAScheduledPaymentPopupAppearsClicksContinue()
        {
            _popupHandler.HandleAllPopups();
        }

        [When(@"opens the payment date picker")]
        public void WhenOpensThePaymentDatePicker()
        {
            // Open date picker logic here
        }

        [When(@"selects the payment date from test data \(less than 15 days past due\)")]
        public void WhenSelectsThePaymentDateFromTestData()
        {
            var data = _testDataReader.GetDataByTestCaseId("TC01");
            // Select date logic using data
        }

        [Then(@"no late fee message is displayed")]
        public void ThenNoLateFeeMessageIsDisplayed()
        {
            // Assert no late fee message
        }
    }
}