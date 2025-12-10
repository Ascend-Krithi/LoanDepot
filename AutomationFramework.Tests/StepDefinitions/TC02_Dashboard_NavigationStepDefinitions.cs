// Automation for TestCaseID: TC02
using TechTalk.SpecFlow;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.SelfHealing;
using NUnit.Framework;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class TC02_Dashboard_NavigationStepDefinitions
    {
        private readonly DashboardPage _dashboardPage;

        public TC02_Dashboard_NavigationStepDefinitions(SelfHealingWebDriver driver)
        {
            _dashboardPage = new DashboardPage(driver);
        }

        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            // Login handled in Hooks
        }

        [When(@"I navigate to the Dashboard")]
        public void WhenINavigateToTheDashboard()
        {
            // Navigation handled in Hooks or via Page Object
        }

        [Then(@"the Dashboard should be visible")]
        public void ThenTheDashboardShouldBeVisible()
        {
            Assert.IsTrue(_dashboardPage.IsDashboardVisible());
        }
    }
}