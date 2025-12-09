using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Pages;

namespace AutomationFramework.Tests.TestPages
{
    public class LoginTestPage
    {
        public LoginPage LoginPage { get; }
        public DashboardPage DashboardPage { get; }
        public LoanListPage LoanListPage { get; }

        public LoginTestPage(SelfHealingWebDriver driver)
        {
            LoginPage = new LoginPage(driver);
            DashboardPage = new DashboardPage(driver);
            LoanListPage = new LoanListPage(driver);
        }
    }
}