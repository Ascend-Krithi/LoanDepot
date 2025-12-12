using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Models;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.Helpers;
using AutomationFramework.Core.Engines;
using TechTalk.SpecFlow;
using FluentAssertions;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class LoginAndUsersSteps
    {
        private readonly LoginPage _loginPage;
        private readonly DashboardPage _dashboardPage;
        private readonly UsersPage _usersPage;
        private readonly TestDataModel _testData;
        private readonly ScenarioContext _scenarioContext;

        public LoginAndUsersSteps(
            LoginPage loginPage,
            DashboardPage dashboardPage,
            UsersPage usersPage,
            TestDataModel testData,
            ScenarioContext scenarioContext)
        {
            _loginPage = loginPage;
            _dashboardPage = dashboardPage;
            _usersPage = usersPage;
            _testData = testData;
            _scenarioContext = scenarioContext;
        }

        [Given(@"I am on the Login Page")]
        public void GivenIAmOnTheLoginPage()
        {
            _loginPage.NavigateTo();
            PopupEngine.HandleAllPopups();
        }

        [When(@"I login with valid credentials")]
        public void WhenILoginWithValidCredentials()
        {
            var data = _testData.GetTestData("TC01");
            _loginPage.Login(data.Username, data.Password);
        }

        [When(@"I login with invalid credentials")]
        public void WhenILoginWithInvalidCredentials()
        {
            var data = _testData.GetTestData("TC02");
            _loginPage.Login(data.Username, data.Password);
        }

        [Then(@"I should be redirected to the Dashboard")]
        public void ThenIShouldBeRedirectedToTheDashboard()
        {
            WaitHelper.WaitForElementVisible(_dashboardPage, "DashboardPage.Title");
            _dashboardPage.GetTitle().Should().Be("PRODUCTS");
        }

        [Then(@"I should see an error message")]
        public void ThenIShouldSeeAnErrorMessage()
        {
            var data = _testData.GetTestData("TC02");
            WaitHelper.WaitForElementVisible(_loginPage, "LoginPage.ErrorMessage");
            _loginPage.GetErrorMessage().Should().Be(data.ErrorMessage);
        }

        [Given(@"I am logged in with valid credentials")]
        public void GivenIAmLoggedInWithValidCredentials()
        {
            _loginPage.NavigateTo();
            PopupEngine.HandleAllPopups();
            var data = _testData.GetTestData("TC03");
            _loginPage.Login(data.Username, data.Password);
            WaitHelper.WaitForElementVisible(_dashboardPage, "DashboardPage.Title");
        }

        [When(@"I navigate to the Users section")]
        public void WhenINavigateToTheUsersSection()
        {
            _dashboardPage.OpenMenu();
            _dashboardPage.GoToUsers();
        }

        [Then(@"the Users page title should be correct")]
        public void ThenTheUsersPageTitleShouldBeCorrect()
        {
            var data = _testData.GetTestData("TC03");
            WaitHelper.WaitForElementVisible(_usersPage, "UsersPage.Title");
            _usersPage.GetTitle().Should().Be(data.PageTitle);
        }
    }
}