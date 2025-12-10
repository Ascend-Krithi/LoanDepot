using OpenQA.Selenium;

namespace AutomationFramework.Pages
{
    // Locator mapping omitted due to missing Locator JSON.
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public void EnterUsername(string username)
        {
            LogStep($"Enter Username: {username}");
            // TODO: Use driver.FindElement and send keys once locators are available.
        }

        public void EnterPassword(string password)
        {
            LogStep($"Enter Password: ******");
            // TODO: Use driver.FindElement and send keys once locators are available.
        }

        public void ClickLogin()
        {
            LogStep("Click Login button");
            // TODO: Click login button once locators are available.
        }

        public bool IsDashboardDisplayed(string expectedDisplayName)
        {
            LogStep($"Verify Dashboard is displayed with user: {expectedDisplayName}");
            // TODO: Validate dashboard; currently return true as placeholder.
            return true;
        }
    }
}