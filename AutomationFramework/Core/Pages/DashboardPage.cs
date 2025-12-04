using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium.Support.UI;
using System;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPage
    {
        private readonly SelfHealingWebDriver _driver;
        public DashboardPage(SelfHealingWebDriver driver)
        {
            _driver = driver;
        }

        public string GetWelcomeMessage()
        {
            var element = _driver.FindElementByKey(Locators.DashboardPageLocators.WelcomeMessageKey);
            return element.Text;
        }

        public void SelectLoanFromDropdown(string loanName)
        {
            var dropdown = _driver.FindElementByKey(Locators.DashboardPageLocators.LoanListDropdownKey);
            var select = new SelectElement(dropdown);
            select.SelectByText(loanName);
        }
    }
}