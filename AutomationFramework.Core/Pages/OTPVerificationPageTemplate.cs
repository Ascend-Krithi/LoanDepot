using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class OTPVerificationPageTemplate : BasePage
    {
        private readonly By otpInput = By.Id("verificationCode");
        private readonly By verifyButton = By.CssSelector("button[type='submit']");

        public void EnterOTP(string otp)
        {
            var input = FindElement("OTPPage.OTPInput", otpInput);
            input.Clear();
            input.SendKeys(otp);
        }

        public void ClickVerify()
        {
            var button = FindElement("OTPPage.VerifyButton", verifyButton);
            button.Click();
        }
    }
}