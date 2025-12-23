using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Pages;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public DashboardPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators.json");
        }

        public bool IsPageReady()
        {
            try
            {
                return Wait.UntilVisible(_locators.GetBy("Dashboard.PageReady"), 10) != null;
            }
            catch
            {
                return false;
            }
        }
    }
}