using AutomationFramework.Core.Engines;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(SelfHealingWebDriver driver) : base(driver) { }

        public string GetTitle()
        {
            return FindElement("DashboardPage.Title").Text;
        }

        public void OpenMenu()
        {
            FindElement("DashboardPage.MenuButton").Click();
        }

        public void GoToUsers()
        {
            FindElement("DashboardPage.UsersLink").Click();
        }
    }
}