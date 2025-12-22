using OpenQA.Selenium;
using WebAutomation.Core.Pages;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(IWebDriver driver) : base(driver) { }

        public bool IsOnDashboard()
        {
            return Wait.UntilPresent(By.CssSelector("h1.dashboard-title"));
        }
    }
}