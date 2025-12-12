// NuGet Packages: Selenium.WebDriver
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    /// <summary>
    /// A generic template for a Dashboard Page Object.
    /// </summary>
    public class DashboardPageTemplate : BasePage
    {
        public DashboardPageTemplate(IWebDriver driver) : base(driver)
        {
            // Locators for a generic dashboard.
            Locators["PageHeader"] = By.TagName("h1");
            Locators["UserProfileMenu"] = By.Id("user-profile-menu");
            Locators["LogoutButton"] = By.LinkText("Logout");
        }

        /// <summary>
        /// Checks if the main dashboard header is visible.
        /// </summary>
        /// <param name="expectedHeaderText">The expected text of the header.</param>
        /// <returns>True if the header is visible and contains the expected text.</returns>
        public bool IsDashboardHeaderVisible(string expectedHeaderText)
        {
            try
            {
                var header = Wait.WaitForElementVisible(Locators["PageHeader"]);
                return header.Text.Contains(expectedHeaderText);
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Performs a logout action by clicking the user profile menu and then the logout button.
        /// </summary>
        public void Logout()
        {
            ClickElement("UserProfileMenu");
            ClickElement("LogoutButton");
        }
    }
}