using TechTalk.SpecFlow;
using WebAutomation.Tests.Pages;
using WebAutomation.Core.Utilities;
using OpenQA.Selenium;

namespace WebAutomation.Tests.StepDefinitions
{
    [Binding]
    public class LoginSteps
    {
        private readonly ScenarioContext _context;
        private readonly IWebDriver _driver;
        private readonly SmartWait _wait;
        private readonly LoginPage _loginPage;
        private readonly DashboardPage _dashboardPage;

        public LoginSteps(ScenarioContext context)
        {
            _context = context;
            _driver = context.Get<IWebDriver>("driver");
            _wait = context.Get<SmartWait>("wait");
            _loginPage = new LoginPage(_driver);
            _dashboardPage = new DashboardPage(_driver);
        }

        [Given(@"the user is on the login page")]
        public void GivenTheUserIsOnTheLoginPage()
        {
            _driver.Navigate().GoToUrl(WebAutomation.Core.Configuration.ConfigManager.Settings.BaseUrl);
            _loginPage.WaitForPageReady();
        }

        [When(@"the user logs in with TestCaseId ""(.*)""")]
        public void WhenTheUserLogsInWithTestCaseId(string testCaseId)
        {
            var testData = ExcelReader.GetRow(
                WebAutomation.Core.Configuration.ConfigManager.Settings.TestDataPath + "/LoginData.xlsx",
                "Login",
                "TestCaseId",
                testCaseId
            );

            _loginPage.Login(testData["Username"], testData["Password"]);
        }

        [Then(@"the dashboard is displayed")]
        public void ThenTheDashboardIsDisplayed()
        {
            bool isReady = _dashboardPage.IsPageReady();
            NUnit.Framework.Assert.IsTrue(isReady, "Dashboard page was not displayed.");
        }
    }
}