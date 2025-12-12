using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPageTemplate : BasePage
    {
        public DashboardPageTemplate(IWebDriver driver) : base(driver)
        {
            Locators["HeaderTitle"] = By.CssSelector("h1.dashboard-title");
            Locators["UserProfileMenu"] = By.Id("user-profile-menu");
            Locators["LogoutButton"] = By.CssSelector("a[href='/logout']");
        }

        public string GetHeaderTitle()
        {
            return GetText("HeaderTitle");
        }

        public bool IsUserProfileMenuVisible()
        {
            return GetElement("UserProfileMenu").Displayed;
        }

        public void Logout()
        {
            Click("UserProfileMenu");
            Click("LogoutButton");
        }
    }
}