using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Locators;
using OpenQA.Selenium;
using System.Linq;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(SelfHealingWebDriver driver) : base(driver) { }

        public string GetWelcomeMessage()
        {
            var welcome = Driver.FindElement(DashboardPageLocators.WelcomeMessage);
            return welcome.Text;
        }

        public void SelectLoanFromDropdown(string loanName)
        {
            var dropdown = Driver.FindElement(DashboardPageLocators.LoanListDropdown);
            dropdown.Click();
            var options = Driver.FindElements(DashboardPageLocators.LoanListDropdown);
            var option = options.FirstOrDefault(o => o.Text.Contains(loanName));
            option?.Click();
        }

        public void DismissChatPopupIfPresent()
        {
            var popups = Driver.FindElements(DashboardPageLocators.ChatPopup);
            if (popups.Any())
            {
                var closeBtn = Driver.FindElement(DashboardPageLocators.ChatPopupClose);
                closeBtn.Click();
            }
        }
    }
}