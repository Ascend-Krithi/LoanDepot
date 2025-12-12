using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPageTemplate : BasePage
    {
        public DashboardPageTemplate(IWebDriver driver) : base(driver) { }

        public By WelcomeBanner => By.CssSelector(".dashboard-welcome");

        public override bool IsLoaded()
        {
            return Driver.FindElement(WelcomeBanner).Displayed;
        }
    }
}