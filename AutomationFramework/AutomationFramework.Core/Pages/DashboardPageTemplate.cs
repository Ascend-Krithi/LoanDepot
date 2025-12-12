using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPageTemplate : BasePage
    {
        public DashboardPageTemplate(SelfHealingWebDriver driver) : base(driver) { }

        private IWebElement HeaderTitle => FindElement("DashboardPage.HeaderTitle", By.CssSelector(".header-title"));
        private IWebElement UserProfileMenu => FindElement("DashboardPage.UserProfileMenu", By.Id("user-profile-menu"));
        private IWebElement LogoutButton => FindElement("DashboardPage.LogoutButton", By.LinkText("Logout"));

        public string GetHeaderTitle()
        {
            return HeaderTitle.Text;
        }

        public void Logout()
        {
            UserProfileMenu.Click();
            LogoutButton.Click();
        }
    }
}