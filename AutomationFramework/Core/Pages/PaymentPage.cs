using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Locators;

namespace AutomationFramework.Core.Pages
{
    public class PaymentPage
    {
        private readonly SelfHealingWebDriver _driver;

        public PaymentPage(SelfHealingWebDriver driver)
        {
            _driver = driver;
        }

        public void EnterAmount(string amount)
        {
            var input = _driver.FindElementByKey(PaymentPageLocators.AmountInputKey);
            input.Clear();
            input.SendKeys(amount);
        }

        public void ClickPay()
        {
            _driver.FindElementByKey(PaymentPageLocators.PayButtonKey).Click();
        }
    }
}