using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Locators;
using OpenQA.Selenium;
using System.Linq;

namespace AutomationFramework.Core.Pages
{
    public class LoanListPage : BasePage
    {
        public LoanListPage(SelfHealingWebDriver driver) : base(driver) { }

        public void SelectLoanByName(string loanName)
        {
            var rows = Driver.FindElements(LoanListPageLocators.LoanRows);
            var row = rows.FirstOrDefault(r => r.Text.Contains(loanName));
            row?.Click();
        }

        public void SelectDate(string day)
        {
            var dateInput = Driver.FindElement(LoanListPageLocators.DatePickerInput);
            dateInput.Click();
            var dayAlternatives = LoanListPageLocators.GetDatePickerDayAlternatives(day);
            foreach (var by in dayAlternatives)
            {
                var days = Driver.FindElements(by);
                var dayElem = days.FirstOrDefault();
                if (dayElem != null)
                {
                    dayElem.Click();
                    break;
                }
            }
        }

        public void DismissPopupIfPresent()
        {
            var popups = Driver.FindElements(LoanListPageLocators.PopupDialog);
            if (popups.Any())
            {
                var closeBtn = Driver.FindElement(LoanListPageLocators.PopupClose);
                closeBtn.Click();
            }
        }
    }
}