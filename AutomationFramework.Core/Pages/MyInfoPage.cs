// AutomationFramework.Core/Pages/MyInfoPage.cs

using AutomationFramework.Core.Drivers;

namespace AutomationFramework.Core.Pages
{
    /// <summary>
    /// Page Template for the My Info Page.
    /// </summary>
    public class MyInfoPage : BasePage
    {
        public MyInfoPage(SelfHealingWebDriver driver) : base(driver)
        {
        }

        public bool IsOnMyInfoPage()
        {
            // Verification logic for the 'My Info' page.
            // For example, checking for a unique header or URL.
            // Assuming the URL contains '/myinfo'.
            return Wait.Until(d => d.Url.Contains("/myinfo"));
        }
    }
}