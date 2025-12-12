using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class PopupEngine : BasePage
    {
        private readonly By contactInfoPopup = By.Id("contactInfoPopup");
        private readonly By updateLaterButton = By.Id("updateLaterBtn");
        private readonly By chatbotIframe = By.Id("servisbot-messenger-iframe");
        private readonly By scheduledPaymentPopup = By.Id("scheduledPaymentPopup");
        private readonly By continueButton = By.Id("continueBtn");

        public void CloseContactInfoPopupIfPresent()
        {
            if (IsElementVisible(contactInfoPopup))
            {
                var button = FindElement("PopupEngine.UpdateLaterButton", updateLaterButton);
                button.Click();
            }
        }

        public void CloseChatbotIframeIfPresent()
        {
            if (IsElementVisible(chatbotIframe))
            {
                Driver.SwitchTo().Frame(FindElement("PopupEngine.ChatbotIframe", chatbotIframe));
                Driver.SwitchTo().DefaultContent();
            }
        }

        public void CloseOtherModalsIfPresent()
        {
            // Implement logic for other modals if needed
        }

        public void ContinueScheduledPaymentPopupIfPresent()
        {
            if (IsElementVisible(scheduledPaymentPopup))
            {
                var button = FindElement("PopupEngine.ContinueButton", continueButton);
                button.Click();
            }
        }
    }
}