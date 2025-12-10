using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Widgets
{
    public class UniversalPopupHandler
    {
        private readonly SelfHealingWebDriver _driver;

        public UniversalPopupHandler(SelfHealingWebDriver driver)
        {
            _driver = driver;
        }

        public void HandlePopups()
        {
            // Detect and close Chat Iframes
            if (_driver.ElementExists("ChatIframe"))
                _driver.FindElement("ChatIframeClose").Click();

            // Close "Update Contact" modals
            if (_driver.ElementExists("UpdateContactModal"))
                _driver.FindElement("UpdateContactClose").Click();

            // Close Scheduled Payment dialogs
            if (_driver.ElementExists("ScheduledPaymentDialog"))
                _driver.FindElement("ScheduledPaymentClose").Click();

            // Remove blocking overlays
            if (_driver.ElementExists("BlockingOverlay"))
                _driver.FindElement("BlockingOverlay").Click();
        }
    }
}