using AutomationFramework.Core.Base;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class PaymentPage : BasePage
    {
        public PaymentPage(SelfHealingWebDriver driver) : base(driver) { }

        public string GetPaymentHeader()
        {
            return Driver.GetText("PaymentHeader");
        }

        public void EnterAmount(string amount)
        {
            Driver.SendKeys("AmountInput", amount);
        }

        public void SelectPaymentMethod(string method)
        {
            Driver.SelectDropdownByText("PaymentMethodDropdown", method);
        }

        public void EnterCardDetails(string number, string expiry, string cvv)
        {
            Driver.SendKeys("CardNumberInput", number);
            Driver.SendKeys("CardExpiryInput", expiry);
            Driver.SendKeys("CardCvvInput", cvv);
        }

        public void EnterBankDetails(string accountNumber, string routingNumber)
        {
            Driver.SendKeys("BankAccountNumberInput", accountNumber);
            Driver.SendKeys("BankRoutingNumberInput", routingNumber);
        }

        public void SubmitPayment()
        {
            Driver.Click("SubmitPaymentButton");
        }

        public string GetPaymentSuccessAlert()
        {
            return Driver.GetText("PaymentSuccessAlert");
        }

        public string GetPaymentErrorAlert()
        {
            return Driver.GetText("PaymentErrorAlert");
        }

        public void OpenPaymentHistoryTab()
        {
            Driver.Click("PaymentHistoryTab");
        }

        public string GetPaymentHistoryTable()
        {
            return Driver.GetText("PaymentHistoryTable");
        }
    }
}