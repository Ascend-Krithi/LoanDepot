using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Widgets
{
    public static class UniversalPopupHandler
    {
        public static void HandlePopups(SelfHealingWebDriver driver)
        {
            // Chatbot iframe
            if (driver.ElementExists("iframe[src*='chatbot']"))
            {
                driver.SwitchToFrame("iframe[src*='chatbot']");
                if (driver.ElementExists("button.close"))
                    driver.Click("button.close");
                driver.SwitchToDefaultContent();
            }

            // Contact Update modal
            if (driver.ElementExists("div.modal-dialog"))
            {
                if (driver.ElementExists("button#update-later"))
                    driver.Click("button#update-later");
                else if (driver.ElementExists("button.close"))
                    driver.Click("button.close");
            }

            // Scheduled Payment dialog
            if (driver.ElementExists("div#scheduled-payment-modal"))
            {
                if (driver.ElementExists("button#continue"))
                    driver.Click("button#continue");
            }

            // Blocking overlays
            if (driver.ElementExists("div.blocking-overlay"))
            {
                driver.Click("div.blocking-overlay");
            }
        }
    }
}