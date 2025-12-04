using AutomationFramework.Core.Encryption;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Utilities;
using TechTalk.SpecFlow;
using NUnit.Framework;

namespace Tests.StepDefinitions
{
    [Binding]
    public class LoginSteps
    {
        private readonly SelfHealingWebDriver _driver;
        private readonly LoginPage _loginPage;
        private readonly DashboardPage _dashboardPage;

        public LoginSteps()
        {
            _driver = (SelfHealingWebDriver)ScenarioContext.Current["DRIVER"];
            _loginPage = new LoginPage(_driver);
            _dashboardPage = new DashboardPage(_driver);
        }

        [Given(@"I login with valid credentials")]
        public void GivenILoginWithValidCredentials()
        {
            var config = ConfigManager.GetConfig();
            var username = EncryptionManager.Decrypt(config.EncryptedUsername);
            var password = EncryptionManager.Decrypt(config.EncryptedPassword);

            _loginPage.EnterUsername(username);
            _loginPage.EnterPassword(password);
            _loginPage.ClickLogin();
        }

        [When(@"I select ""(.*)"" from the loan dropdown")]
        public void WhenISelectFromTheLoanDropdown(string loanType)
        {
            _dashboardPage.SelectLoanType(loanType);
        }

        [When(@"I select ""(.*)"" from the loan list")]
        public void WhenISelectFromTheLoanList(string loanName)
        {
            _dashboardPage.SelectLoanFromList(loanName);
        }

        [When(@"I dismiss any popup")]
        public void WhenIDismissAnyPopup()
        {
            _dashboardPage.DismissPopup();
        }

        [When(@"I handle delayed chat popup")]
        public void WhenIHandleDelayedChatPopup()
        {
            _dashboardPage.HandleDelayedChatPopup();
        }

        [When(@"I select ""(.*)"" in the date picker")]
        public void WhenISelectInTheDatePicker(string date)
        {
            _dashboardPage.SelectDate(date);
        }

        [Then(@"I should see the message ""(.*)""")]
        public void ThenIShouldSeeTheMessage(string expected)
        {
            var actual = _dashboardPage.GetMessage();
            Assert.AreEqual(expected, actual);
        }
    }
}