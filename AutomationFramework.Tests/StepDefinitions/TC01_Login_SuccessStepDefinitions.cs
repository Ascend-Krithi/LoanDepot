// Automation for TestCaseID: TC01
using TechTalk.SpecFlow;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using NUnit.Framework;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class TC01_Login_SuccessStepDefinitions
    {
        private readonly LoginPage _loginPage;
        private readonly DashboardPage _dashboardPage;

        public TC01_Login_SuccessStepDefinitions(SelfHealingWebDriver driver)
        {
            _loginPage = new LoginPage(driver);
            _dashboardPage = new DashboardPage(driver);
        }

        [Given(@"I am on the Login Page")]
        public void GivenIAmOnTheLoginPage()
        {
            // Navigation handled in Hooks
        }

        [When(@"I enter valid credentials")]
        public void WhenIEnterValidCredentials()
        {
            var data = TestDataReader.GetTestData("TC01");
            _loginPage.EnterUsername(data["Username"].ToString());
            _loginPage.EnterPassword(data["Password"].ToString());
        }

        [When(@"I click the Login button")]
        public void WhenIClickTheLoginButton()
        {
            _loginPage.ClickLogin();
        }

        [Then(@"I should see the Dashboard")]
        public void ThenIShouldSeeTheDashboard()
        {
            Assert.IsTrue(_dashboardPage.IsDashboardVisible());
        }
    }
}