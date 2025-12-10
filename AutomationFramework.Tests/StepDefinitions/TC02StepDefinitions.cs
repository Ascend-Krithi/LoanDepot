// TestCaseID: TC02 – HELOC Late Fee Validation
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.Widgets;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class TC02StepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly TestDataReader _testDataReader;
        private readonly UniversalPopupHandler _popupHandler;

        public TC02StepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _testDataReader = (TestDataReader)_scenarioContext["TestDataReader"];
            _popupHandler = (UniversalPopupHandler)_scenarioContext["PopupHandler"];
        }

        [Given(@"the application is launched")]
        public void GivenTheApplicationIsLaunched()
        {
            // Launch logic here
        }

        [When(@"the user logs in using valid credentials")]
        public void WhenTheUserLogsInUsingValidCredentials()
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

        [When(@"closes or dismisses any pop-ups")]
        public void WhenClosesOrDismissesAnyPopups()
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

        [When(@"if scheduled payment popup appears, clicks Continue")]
        public void WhenIfScheduledPaymentPopupAppearsClicksContinue()
        {
            _popupHandler.HandleAllPopups();
        }

        [When(@"opens the payment date picker")]
        public void WhenOpensThePaymentDatePicker()
        {
            // Open date picker logic here
        }

        [When(@"selects the payment date from test data \(exactly 15 days past due\)")]
        public void WhenSelectsThePaymentDateFromTestData()
        {
            var data = _testDataReader.GetDataByTestCaseId("TC02");
            // Select date logic using data
        }

        [Then(@"no late fee message should be displayed")]
        public void ThenNoLateFeeMessageShouldBeDisplayed()
        {
            // Assert no late fee message
        }
    }
}