// AutomationFramework.Tests/StepDefinitions/DashboardStepDefinitions.cs

using AutomationFramework.Core.Pages;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class DashboardStepDefinitions
    {
        private readonly DashboardPage _dashboardPage;
        private readonly MyInfoPage _myInfoPage;

        public DashboardStepDefinitions(DashboardPage dashboardPage, MyInfoPage myInfoPage)
        {
            _dashboardPage = dashboardPage;
            _myInfoPage = myInfoPage;
        }

        [Given(@"the user is on the dashboard page")]
        public void GivenTheUserIsOnTheDashboardPage()
        {
            // This precondition is met by the @LoggedIn hook.
            // This step confirms the state and enhances readability.
            _dashboardPage.IsOnDashboardPage().Should().BeTrue("the user must be on the dashboard to start this scenario.");
        }

        [When(@"the user clicks on the 'My Info' link in the navigation menu")]
        public void WhenTheUserClicksOnTheMyInfoLinkInTheNavigationMenu()
        {
            _dashboardPage.ClickMyInfoLink();
        }

        [Then(@"the user should be redirected to the 'My Info' page")]
        public void ThenTheUserShouldBeRedirectedToTheMyInfoPage()
        {
            _myInfoPage.IsOnMyInfoPage().Should().BeTrue("the user should be navigated to the 'My Info' page.");
        }
    }
}