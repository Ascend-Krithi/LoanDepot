// AutomationFramework.Tests/Hooks/Hooks.cs

using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Pages;
using BoDi;
using TechTalk.SpecFlow;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public sealed class Hooks
    {
        private readonly IObjectContainer _container;

        public Hooks(IObjectContainer container)
        {
            _container = container;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Load configuration
            var config = new ConfigReader();
            _container.RegisterInstanceAs(config);

            // Initialize and register the self-healing driver
            var driver = new SelfHealingWebDriver(config);
            _container.RegisterInstanceAs(driver);

            // Register Page Objects for Dependency Injection
            _container.RegisterTypeAs<LoginPage, LoginPage>();
            _container.RegisterTypeAs<DashboardPage, DashboardPage>();
            _container.RegisterTypeAs<MyInfoPage, MyInfoPage>();

            // Navigate to the base URL
            driver.Navigate().GoToUrl(config.Get("BaseUrl"));
        }

        [BeforeScenario("@LoggedIn")]
        public void BeforeLoggedInScenario()
        {
            // This hook runs for scenarios tagged with @LoggedIn
            // It performs a login action to satisfy the precondition
            var driver = _container.Resolve<SelfHealingWebDriver>();
            var loginPage = new LoginPage(driver);
            
            // Using credentials from a secure source or config
            loginPage.EnterUsername("valid_user");
            loginPage.EnterPassword("valid_password");
            loginPage.ClickLoginButton();

            // Wait for dashboard to ensure login is complete
            var dashboardPage = new DashboardPage(driver);
            dashboardPage.IsOnDashboardPage();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var driver = _container.Resolve<SelfHealingWebDriver>();
            driver?.Quit();
        }
    }
}