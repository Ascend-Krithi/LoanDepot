using AutomationFramework.Core.Base;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    // Automation for Page: PaymentPage
    public class PaymentPage : BasePage
    {
        public PaymentPage(SelfHealingWebDriver driver) : base(driver) { }

        public void EnterAmount(string amount)
        {
            BeforeCriticalAction();
            Driver.FindElement("AmountInput").SendKeys(amount);
        }

        public void ClickSubmit()
        {
            BeforeCriticalAction();
            Driver.FindElement("SubmitButton").Click();
        }

        public bool IsPaymentSuccess()
        {
            BeforeCriticalAction();
            return Driver.FindElement("SuccessMessage").Displayed;
        }
    }
}