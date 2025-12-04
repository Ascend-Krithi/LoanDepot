using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Locators;
using OpenQA.Selenium;
using System.Threading;

namespace AutomationFramework.Core.Pages
{
    public class PaymentPage
    {
        private readonly SelfHealingWebDriver _driver;

        public PaymentPage(SelfHealingWebDriver driver)
        {
            _driver = driver;
        }

        public void SelectPaymentType(string type)
        {
            var dropdown = _driver.FindElementByKey(nameof(PaymentPageLocators.PaymentDropdownLocator));
            dropdown.Click();
            var option = dropdown.FindElements(By.TagName("mat-option"))
                .FirstOrDefault(o => o.Text.Contains(type));
            if (option != null)
                option.Click();
            else
                throw new Exception($"Payment type '{type}' not found.");
        }

        public void SelectDate(string date)
        {
            var dateInput = _driver.FindElementByKey(nameof(PaymentPageLocators.DatePickerInputLocator));
            dateInput.Click();
            dateInput.Clear();
            dateInput.SendKeys(date);
            dateInput.SendKeys(Keys.Enter);
        }

        public void SubmitPayment()
        {
            var btn = _driver.FindElementByKey(nameof(PaymentPageLocators.SubmitButtonLocator));
            btn.Click();
        }
    }
}