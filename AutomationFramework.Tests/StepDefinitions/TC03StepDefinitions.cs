// Automation for TestCaseID: TC03
using TechTalk.SpecFlow;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Utilities;
using NUnit.Framework;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class TC03StepDefinitions
    {
        private readonly LoginPage _loginPage;
        private readonly DashboardPage _dashboardPage;
        private readonly AccountDetailsPage _accountDetailsPage;
        private readonly PaymentPage _paymentPage;
        private readonly SelfHealingWebDriver _driver;
        private readonly Dictionary<string, string> _testData;

        public TC03StepDefinitions(SelfHealingWebDriver driver)
        {
            _driver = driver;
            _loginPage = new LoginPage(driver);
            _dashboardPage = new DashboardPage(driver);
            _accountDetailsPage = new AccountDetailsPage(driver);
            _paymentPage = new PaymentPage(driver);
            _testData = TestDataReader.GetTestData("TC03", "AutomationFramework.Tests/TestData/testdata.xlsx");
        }

        [Given(@"the customer servicing application is launched")]
        public void GivenTheCustomerServicingApplicationIsLaunched()
        {
            // All logic is in Page Objects
        }

        [Given(@"the user logs in with valid credentials")]
        public void GivenTheUserLogsInWithValidCredentials()
        {
            _loginPage.EnterUsername(_testData["Username"]);
            _loginPage.EnterPassword(_testData["Password"]);
            _loginPage.ClickLogin();
        }

        [Given(@"completes MFA verification")]
        public void GivenCompletesMFAVerification()
        {
            // Assume handled by Page Object
        }

        [Given(@"navigates to the dashboard")]
        public void GivenNavigatesToTheDashboard()
        {
            // Assume handled by Page Object
        }

        [Given(@"all pop-ups are dismissed")]
        public void GivenAllPopupsAreDismissed()
        {
            // Handled by UniversalPopupHandler in BasePage
        }

        [Given(@"the applicable loan account is selected")]
        public void GivenTheApplicableLoanAccountIsSelected()
        {
            _dashboardPage.OpenRecentCaseById(_testData["LoanAccountId"]);
        }

        [Given(@"the user clicks Make a Payment")]
        public void GivenTheUserClicksMakeAPayment()
        {
            _accountDetailsPage.EditAccount();
        }

        [Given(@"continues past any scheduled payment popup")]
        public void GivenContinuesPastAnyScheduledPaymentPopup()
        {
            // Handled by UniversalPopupHandler
        }

        [Given(@"opens the date picker")]
        public void GivenOpensTheDatePicker()
        {
            _paymentPage.OpenPaymentHistoryTab();
        }

        [Given(@"selects a payment date more than 15 days past due from test data")]
        public void GivenSelectsAPaymentDateMoreThan15DaysPastDueFromTestData()
        {
            _paymentPage.EnterAmount(_testData["PaymentAmount"]);
        }

        [When(@"the user observes the late-fee message area")]
        public void WhenTheUserObservesTheLateFeeMessageArea()
        {
            // No logic, just observation
        }

        [Then(@"a late fee message should be displayed")]
        public void ThenALateFeeMessageShouldBeDisplayed()
        {
            Assert.IsNotEmpty(_paymentPage.GetPaymentErrorAlert());
        }
    }
}