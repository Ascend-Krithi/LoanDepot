using AutomationFramework.Core.Engines;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.Pages
{
    public class UsersPage : BasePage
    {
        public UsersPage(SelfHealingWebDriver driver) : base(driver) { }

        public string GetTitle()
        {
            return FindElement("UsersPage.Title").Text;
        }
    }
}