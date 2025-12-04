using AutomationFramework.Core.Pages;
using AutomationFramework.Core.SelfHealing;
using TechTalk.SpecFlow;
using Xunit;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class LoginSteps
    {
        private readonly SelfHealingWebDriver _driver;
        private readonly LoginPage _loginPage;
        private readonly DashboardPage _dashboardPage;

        public LoginSteps(SelfHealingWebDriver driver)
        {
            _driver = driver;
            _loginPage = new LoginPage(_driver);
            _dashboardPage = new DashboardPage(_driver);
        }

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            _driver.Url = "https://example.com/login";
        }

        [When(@"I enter valid credentials")]
        public void WhenIEnterValidCredentials()
        {
            _loginPage.EnterUsername("testuser");
            _loginPage.EnterPassword("password");
        }

        [When(@"I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            _loginPage.ClickLogin();
        }

        [Then(@"I should see the dashboard welcome message")]
        public void ThenIShouldSeeTheDashboardWelcomeMessage()
        {
            var message = _dashboardPage.GetWelcomeMessage();
            Assert.Contains("Welcome", message);
        }
    }
}