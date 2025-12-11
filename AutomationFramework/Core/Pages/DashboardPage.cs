using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Locators;
using OpenQA.Selenium;
using System.Linq;
using System.Threading;

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
            var items = _driver.FindElements(DashboardPageLocators.LoanList);
            var item = items.FirstOrDefault(i => i.Text.Contains(loanName));
            if (item != null)
                item.Click();
            else
                throw new NoSuchElementException($"Loan '{loanName}' not found in list.");
        }

        public void DismissPopup()
        {
            var closeBtn = _driver.FindElementByKey(DashboardPageLocators.PopupCloseKey);
            closeBtn.Click();
        }

        public void HandleChatPopupIfPresent()
        {
            try
            {
                var chatPopup = _driver.FindElementByKey(DashboardPageLocators.ChatPopupKey, 3);
                if (chatPopup.Displayed)
                {
                    chatPopup.FindElement(By.CssSelector("button.close")).Click();
                }
            }
            catch (NoSuchElementException) { }
        }

        public void SelectDate(string date)
        {
            var dateInput = _driver.FindElementByKey(DashboardPageLocators.DatePickerInputKey);
            dateInput.Click();
            dateInput.Clear();
            dateInput.SendKeys(date);
            dateInput.SendKeys(Keys.Enter);
        }

        public string GetMessageBanner()
        {
            return _driver.FindElementByKey(DashboardPageLocators.MessageBannerKey).Text;
        }
    }
}