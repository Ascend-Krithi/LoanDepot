// AutomationFramework.Core/Pages/DashboardPage.cs

using AutomationFramework.Core.Drivers;

namespace AutomationFramework.Core.Pages
{
    /// <summary>
    /// Page Template for the Dashboard Page.
    /// </summary>
    public class DashboardPage : BasePage
    {
        // Logical key for the 'My Info' link
        private const string MyInfoLink = "DashboardPage.MyInfoLink";

        public DashboardPage(SelfHealingWebDriver driver) : base(driver)
        {
        }

        public void ClickMyInfoLink()
        {
            Wait.UntilElementToBeClickable(MyInfoLink);
            Driver.FindElement(MyInfoLink).Click();
        }

        public bool IsOnDashboardPage()
        {
            // A simple verification by checking the URL or a unique element.
            // Assuming the URL contains '/dashboard'.
            return Wait.Until(d => d.Url.Contains("/dashboard"));
        }
    }
}