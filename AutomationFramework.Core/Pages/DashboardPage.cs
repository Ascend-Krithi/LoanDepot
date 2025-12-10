using AutomationFramework.Core.Base;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    // Automation for Page: DashboardPage
    public class DashboardPage : BasePage
    {
        public DashboardPage(SelfHealingWebDriver driver) : base(driver) { }

        public bool IsDashboardVisible()
        {
            BeforeCriticalAction();
            return Driver.FindElement("DashboardHeader").Displayed;
        }
    }
}