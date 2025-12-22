using TechTalk.SpecFlow;
using NUnit.Framework;
using OpenQA.Selenium;
using WebAutomation.Core.Utilities;
using WebAutomation.Tests.Pages;

namespace WebAutomation.Tests.StepDefinitions
{
    [Binding]
    public class LateFeeSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;
        private SmartWait _wait;
        private PopupHandler _popup;
        private LoginPage _loginPage;
        private MfaPage _mfaPage;
        private OtpPage _otpPage;
        private DashboardPage _dashboardPage;
        private PaymentPage _paymentPage;
        private Dictionary<string, string> _testData;

        public LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the user launches the customer servicing application")]
        public void GivenTheUserLaunchesTheCustomerServicingApplication()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _wait = _scenarioContext.Get<SmartWait>("wait");
            _popup = _scenarioContext.Get<PopupHandler>("popup");
            _loginPage = new LoginPage(_driver);
            _mfaPage = new MfaPage(_driver);
            _otpPage = new OtpPage(_driver);
            _dashboardPage = new DashboardPage(_driver);
            _paymentPage = new PaymentPage(_driver);
        }

        [Given(@"logs in using valid customer credentials")]
        public void GivenLogsInUsingValidCustomerCredentials()
        {
            _loginPage.LoginWithDefaultCredentials();
        }

        [Given(@"completes MFA verification")]
        public void GivenCompletesMFAVerification()
        {
            _mfaPage.SelectEmailMethodAndSendCode();
            _otpPage.EnterStaticOtpAndVerify();
        }

        [Given(@"navigates to the dashboard")]
        public void GivenNavigatesToTheDashboard()
        {
            _dashboardPage.WaitForDashboardToLoad();
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

        [Given(@"clicks Make a Payment")]
        public void GivenClicksMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [Given(@"continues past scheduled payment popup if present")]
        public void GivenContinuesPastScheduledPaymentPopupIfPresent()
        {
            _paymentPage.ContinueScheduledPaymentIfPresent();
        }

        [Given(@"opens the payment date picker")]
        public void GivenOpensThePaymentDatePicker()
        {
            _paymentPage.OpenDatePicker();
        }

        [Given(@"selects the payment date ""(.*)""")]
        public void GivenSelectsThePaymentDate(string paymentDate)
        {
            _paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"no late fee message is displayed")]
        public void ThenNoLateFeeMessageIsDisplayed()
        {
            Assert.False(_paymentPage.IsLateFeeMessageDisplayed(), "Late fee message should not be displayed.");
        }
    }
}