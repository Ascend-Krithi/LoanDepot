using OpenQA.Selenium;

namespace AutomationFramework.Pages
{
    // Locator mapping omitted due to missing Locator JSON.
    public class DashboardPage : BasePage
    {
        public DashboardPage(IWebDriver driver) : base(driver)
        {
        }

        public void NavigateToServicingModule()
        {
            LogStep("Navigate to Servicing module from Dashboard");
            // TODO: Implement navigation via menu click once locators are available.
        }
    }
}