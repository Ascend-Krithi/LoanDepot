using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.PopupEngine;
using System;

namespace AutomationFramework.Core.Pages
{
    public class PaymentPageTemplate : BasePage
    {
        public const string PaymentDatePickerKey = "Payment.PaymentDatePicker";
        public const string PaymentDateCellKey = "Payment.PaymentDateCell";
        public const string LateFeeMessageAreaKey = "Payment.LateFeeMessageArea";

        public PaymentPageTemplate(SelfHealingWebDriver driver, WaitHelper waitHelper, PopupEngine popupEngine)
            : base(driver, waitHelper, popupEngine) { }

        public void OpenPaymentDatePicker()
        {
            FindElement(PaymentDatePickerKey).Click();
        }

        public void SelectPaymentDate(DateTime paymentDate)
        {
            var dynamicKey = $"{PaymentDateCellKey}:{paymentDate:yyyy-MM-dd}";
            FindElement(dynamicKey).Click();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return FindElement(LateFeeMessageAreaKey).Displayed && !string.IsNullOrWhiteSpace(FindElement(LateFeeMessageAreaKey).Text);
        }

        public override void WaitForPageToLoad()
        {
            WaitHelper.WaitForElementVisible(PaymentDatePickerKey);
        }
    }
}