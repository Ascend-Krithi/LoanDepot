using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.PopupEngine;

namespace AutomationFramework.Core.Pages
{
    public class OTPVerificationPageTemplate : BasePage
    {
        public const string OtpInputKey = "OTPVerification.OtpInput";
        public const string VerifyButtonKey = "OTPVerification.VerifyButton";

        public OTPVerificationPageTemplate(SelfHealingWebDriver driver, WaitHelper waitHelper, PopupEngine popupEngine)
            : base(driver, waitHelper, popupEngine) { }

        public void EnterOtp(string otp)
        {
            FindElement(OtpInputKey).SendKeys(otp);
        }

        public void ClickVerify()
        {
            FindElement(VerifyButtonKey).Click();
        }

        public override void WaitForPageToLoad()
        {
            WaitHelper.WaitForElementVisible(OtpInputKey);
        }
    }
}