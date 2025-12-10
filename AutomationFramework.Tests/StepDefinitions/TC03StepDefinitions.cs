// TestCaseID: TC03 – HELOC Late Fee Validation
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.Widgets;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class TC03StepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly TestDataReader _testDataReader;
        private readonly UniversalPopupHandler _popupHandler;

        public TC03StepDefinitions(ScenarioContext scenarioContext)
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

        [When(@"opens the date picker")]
        public void WhenOpensTheDatePicker()
        {
            // Open date picker logic here
        }

        [When(@"selects the payment date from test data \(more than 15 days late\)")]
        public void WhenSelectsThePaymentDateFromTestData()
        {
            var data = _testDataReader.GetDataByTestCaseId("TC03");
            // Select date logic using data
        }

        [Then(@"late fee message appears")]
        public void ThenLateFeeMessageAppears()
        {
            // Assert late fee message
        }
    }
}