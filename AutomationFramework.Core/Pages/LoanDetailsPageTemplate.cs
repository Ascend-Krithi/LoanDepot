using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.PopupEngine;

namespace AutomationFramework.Core.Pages
{
    public class LoanDetailsPageTemplate : BasePage
    {
        public const string MakeAPaymentButtonKey = "LoanDetails.MakeAPaymentButton";
        public const string ScheduledPaymentPopupKey = "LoanDetails.ScheduledPaymentPopup";
        public const string ScheduledPaymentContinueButtonKey = "LoanDetails.ScheduledPaymentContinueButton";

        public LoanDetailsPageTemplate(SelfHealingWebDriver driver, WaitHelper waitHelper, PopupEngine popupEngine)
            : base(driver, waitHelper, popupEngine) { }

        public void ClickMakeAPayment()
        {
            FindElement(MakeAPaymentButtonKey).Click();
        }

        public void ContinueScheduledPaymentPopupIfPresent()
        {
            if (PopupEngine.IsPopupPresent(ScheduledPaymentPopupKey))
            {
                PopupEngine.DismissPopup(ScheduledPaymentPopupKey, ScheduledPaymentContinueButtonKey);
            }
        }

        public override void WaitForPageToLoad()
        {
            WaitHelper.WaitForElementVisible(MakeAPaymentButtonKey);
        }
    }
}