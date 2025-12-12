// AutomationFramework.Tests/StepDefinitions/LoginStepDefinitions.cs

using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Pages;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class LoginStepDefinitions
    {
        private readonly LoginPage _loginPage;
        private readonly DashboardPage _dashboardPage;
        private readonly ConfigReader _config;

        public LoginStepDefinitions(LoginPage loginPage, DashboardPage dashboardPage, ConfigReader config)
        {
            _loginPage = loginPage;
            _dashboardPage = dashboardPage;
            _config = config;
        }

        [Given(@"the user is on the login page")]
        public void GivenTheUserIsOnTheLoginPage()
        {
            // The driver navigation is handled in the Hooks file.
            // This step is for readability.
        }

        [When(@"the user enters valid credentials")]
        public void WhenTheUserEntersValidCredentials()
        {
            // Assuming test data is loaded from a source like Excel or config
            // For this example, we'll use hardcoded values representing the data
            _loginPage.EnterUsername("valid_user");
            _loginPage.EnterPassword("valid_password");
        }

        [When(@"the user enters invalid credentials")]
        public void WhenTheUserEntersInvalidCredentials()
        {
            _loginPage.EnterUsername("invalid_user");
            _loginPage.EnterPassword("invalid_password");
        }

        [When(@"the user clicks the login button")]
        public void WhenTheUserClicksTheLoginButton()
        {
            _loginPage.ClickLoginButton();
        }

        [Then(@"the user should be redirected to the dashboard page")]
        public void ThenTheUserShouldBeRedirectedToTheDashboardPage()
        {
            _dashboardPage.IsOnDashboardPage().Should().BeTrue("the user should be on the dashboard after a successful login.");
        }

        [Then(@"an error message '([^']*)' should be displayed")]
        public void ThenAnErrorMessageShouldBeDisplayed(string expectedMessage)
        {
            _loginPage.IsErrorMessageDisplayed().Should().BeTrue("an error message should be visible for invalid credentials.");
            _loginPage.GetErrorMessageText().Should().Be(expectedMessage, $"the error message should match '{expectedMessage}'.");
        }
    }
}