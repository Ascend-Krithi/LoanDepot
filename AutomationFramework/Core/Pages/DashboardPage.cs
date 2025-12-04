using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Locators;
using OpenQA.Selenium;
using System.Linq;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPage
    {
        private readonly SelfHealingWebDriver _driver;

        public DashboardPage(SelfHealingWebDriver driver)
        {
            _driver = driver;
        }

        public void SelectLoanType(string loanType)
        {
            var dropdown = _driver.FindElementByKey(DashboardPageLocators.LoanDropdownKey);
            var select = new OpenQA.Selenium.Support.UI.SelectElement(dropdown);
            select.SelectByText(loanType);
        }

        public void SelectLoanFromList(string loanName)
        {
            var loans = _driver.FindElements(DashboardPageLocators.LoanList);
            var loan = loans.FirstOrDefault(l => l.Text.Contains(loanName));
            loan?.Click();
        }

        public void DismissPopup()
        {
            var closeBtn = _driver.FindElementByKey(DashboardPageLocators.PopupCloseKey);
            closeBtn.Click();
        }

        public void HandleDelayedChatPopup()
        {
            try
            {
                var chat = _driver.FindElementByKey(DashboardPageLocators.ChatPopupKey);
                if (chat.Displayed)
                {
                    _driver.SwitchTo().Frame(chat);
                    var closeBtn = _driver.FindElement(By.XPath("//button[contains(@aria-label,'Close')]") );
                    closeBtn.Click();
                    _driver.SwitchTo().DefaultContent();
                }
            }
            catch { /* Ignore if not present */ }
        }

        public void SelectDate(string date)
        {
            var dateInput = _driver.FindElementByKey(DashboardPageLocators.DatePickerInputKey);
            dateInput.Click();
            dateInput.Clear();
            dateInput.SendKeys(date);
            dateInput.SendKeys(Keys.Enter);
        }

        public string GetMessage()
        {
            var msg = _driver.FindElementByKey(DashboardPageLocators.MessageKey);
            return msg.Text;
        }
    }
}