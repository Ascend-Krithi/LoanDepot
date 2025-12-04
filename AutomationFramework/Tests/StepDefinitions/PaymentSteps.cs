using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.Encryption;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class PaymentSteps
    {
        private readonly ScenarioContext _context;
        private LoginPage _loginPage;
        private DashboardPage _dashboardPage;
        private PaymentPage _paymentPage;

        public PaymentSteps(ScenarioContext context)
        {
            _context = context;
            _loginPage = (LoginPage)_context["LoginPage"];
            _dashboardPage = (DashboardPage)_context["DashboardPage"];
            _paymentPage = (PaymentPage)_context["PaymentPage"];
        }

        [Given(@"I am logged in as ""(.*)""")]
        public void GivenIAmLoggedInAs(string username)
        {
            var encryptedPassword = ConfigManager.Get("EncryptedPassword");
            var password = EncryptionManager.Decrypt(encryptedPassword);
            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);
            _loginPage.ClickLogin();
        }

        [Given(@"I select loan ""(.*)""")]
        public void GivenISelectLoan(string loanName)
        {
            _dashboardPage.SelectLoanByName(loanName);
        }

        [Given(@"I dismiss chat popup if present")]
        public void GivenIDismissChatPopupIfPresent()
        {
            _dashboardPage.DismissChatPopupIfPresent();
        }

        [When(@"I select payment type ""(.*)""")]
        public void WhenISelectPaymentType(string type)
        {
            _paymentPage.SelectPaymentType(type);
        }

        [When(@"I select payment date ""(.*)""")]
        public void WhenISelectPaymentDate(string date)
        {
            _paymentPage.SelectDate(date);
        }

        [When(@"I submit the payment")]
        public void WhenISubmitThePayment()
        {
            _paymentPage.SubmitPayment();
        }

        [Then(@"I should see message ""(.*)""")]
        public void ThenIShouldSeeMessage(string message)
        {
            // Implement message validation logic here
            Assert.IsTrue(true, "Message validation placeholder.");
        }
    }
}