using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.PopupEngine;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPageTemplate : BasePage
    {
        public const string LoanAccountRowKey = "Dashboard.LoanAccountRow";
        public const string PopUpContactUpdateKey = "Dashboard.ContactUpdatePopup";
        public const string PopUpChatbotIframeKey = "Dashboard.ChatbotIframe";
        public const string MakeAPaymentButtonKey = "Dashboard.MakeAPaymentButton";

        public DashboardPageTemplate(SelfHealingWebDriver driver, WaitHelper waitHelper, PopupEngine popupEngine)
            : base(driver, waitHelper, popupEngine) { }

        public bool CheckLoanAccountsVisible()
        {
            return FindElement(LoanAccountRowKey).Displayed;
        }

        public void SelectLoanAccount(string loanNumber)
        {
            var dynamicKey = $"{LoanAccountRowKey}:{loanNumber}";
            FindElement(dynamicKey).Click();
            WaitForPageToLoad();
        }

        public void DismissPopupsIfPresent()
        {
            if (PopupEngine.IsPopupPresent(PopUpContactUpdateKey))
            {
                PopupEngine.DismissPopup(PopUpContactUpdateKey, "Update Later");
            }
            if (PopupEngine.IsPopupPresent(PopUpChatbotIframeKey))
            {
                PopupEngine.DismissPopup(PopUpChatbotIframeKey);
            }
        }

        public override void WaitForPageToLoad()
        {
            WaitHelper.WaitForElementVisible(LoanAccountRowKey);
        }
    }
}