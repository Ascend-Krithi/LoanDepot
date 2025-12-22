using TechTalk.SpecFlow;
using OpenQA.Selenium;
using WebAutomation.Tests.Pages;
using FluentAssertions;

namespace WebAutomation.Tests.Steps
{
    [Binding]
    public class LoginSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly LoginPage _loginPage;
        private readonly DashboardPage _dashboardPage;

        public LoginSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            var driver = _scenarioContext.Get<IWebDriver>("driver");
            _loginPage = new LoginPage(driver);
            _dashboardPage = new DashboardPage(driver);
        }

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            _loginPage.IsOnLoginPage().Should().BeTrue();
        }

        [When(@"I enter valid credentials")]
        public void WhenIEnterValidCredentials()
        {
            _loginPage.EnterCredentials();
        }

        [When(@"I click the login button")]
        public void WhenIClickTheLoginButton()
        {
            _loginPage.ClickLogin();
        }

        [Then(@"I should be redirected to the dashboard")]
        public void ThenIShouldBeRedirectedToTheDashboard()
        {
            _dashboardPage.IsOnDashboard().Should().BeTrue();
        }
    }
}