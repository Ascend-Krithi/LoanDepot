using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.PopupEngine;

namespace AutomationFramework.Core.Pages
{
    public class MFAPageTemplate : BasePage
    {
        public const string EmailDropdownKey = "MFA.EmailDropdown";
        public const string ReceiveCodeButtonKey = "MFA.ReceiveCodeButton";

        public MFAPageTemplate(SelfHealingWebDriver driver, WaitHelper waitHelper, PopupEngine popupEngine)
            : base(driver, waitHelper, popupEngine) { }

        public void SelectFirstEmailOption()
        {
            FindElement(EmailDropdownKey).Click();
            // Select first option logic (implementation depends on control)
        }

        public void ClickReceiveCodeViaEmail()
        {
            FindElement(ReceiveCodeButtonKey).Click();
        }

        public override void WaitForPageToLoad()
        {
            WaitHelper.WaitForElementVisible(EmailDropdownKey);
        }
    }
}