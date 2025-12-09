```csharp
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
            var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(dropdown);
            selectElement.SelectByText(loanType);
        }

        public void SelectLoanFromList(string loanName)
        {
            var loans = _driver.FindElements(DashboardPageLocators.LoanListKey);
            var loan = loans.FirstOrDefault(l => l.Text.Contains(loanName));
            loan?.Click();
        }

        public void DismissChatPopup()
        {
            var popup = _driver.FindElementByKey(DashboardPageLocators.ChatPopupKey);
            if (popup.Displayed)
            {
                var closeBtn = popup.FindElement(By.XPath(".//button[contains(@class,'close')]"));
                closeBtn.Click();
            }
        }

        public void SelectDate(string date)
        {
            var datePicker = _driver.FindElementByKey(DashboardPageLocators.DatePickerKey);
            datePicker.Click();
            // Assume Angular Material date picker, select date via input
            datePicker.SendKeys(date);
            datePicker.SendKeys(Keys.Enter);
        }
    }