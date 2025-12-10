using TechTalk.SpecFlow;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.Widgets;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    // TestCaseID: TC03 – HELOC Late Fee Validation
    public class TC03StepDefinitions
    {
        [Given(@"I launch the application")]
        public void GivenILaunchTheApplication()
        {
            // Handled by Hooks
        }

        [Given(@"I login with encrypted credentials")]
        public void GivenILoginWithEncryptedCredentials()
        {
            // Use EncryptionService to decrypt credentials and perform login
        }

        [Given(@"I complete MFA")]
        public void GivenICompleteMFA()
        {
            // Complete MFA step
        }

        [Given(@"I am on the dashboard")]
        public void GivenIAmOnTheDashboard()
        {
            // Verify dashboard loaded
        }

        [Given(@"I dismiss any popups")]
        public void GivenIDismissAnyPopups()
        {
            UniversalPopupHandler.DismissAllPopups(Hooks.Hooks.Driver);
        }

        [Given(@"I select the applicable loan account")]
        public void GivenISelectTheApplicableLoanAccount()
        {
            // Select loan account
        }

        [When(@"I navigate to Make a Payment")]
        public void WhenINavigateToMakeAPayment()
        {
            // Navigate to Make Payment
        }

        [When(@"I handle any scheduled payment popup")]
        public void WhenIHandleAnyScheduledPaymentPopup()
        {
            UniversalPopupHandler.DismissAllPopups(Hooks.Hooks.Driver);
        }

        [When(@"I open the payment date picker")]
        public void WhenIOpenThePaymentDatePicker()
        {
            // Open date picker
        }

        [When(@"I select the payment date more than 15 days past due")]
        public void WhenISelectThePaymentDateMoreThan15DaysPastDue()
        {
            // Select date from test data
        }

        [Then(@"I should see the late fee message")]
        public void ThenIShouldSeeTheLateFeeMessage()
        {
            // Assert late fee message present
        }
    }
}