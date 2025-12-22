using OpenQA.Selenium;

namespace AutomationFramework.Pages
{
    public class MfaPage : BasePage
    {
        private readonly By MfaDialog = By.CssSelector("mat-dialog-container");
        private readonly By EmailMethodSelect = By.CssSelector("mat-select[formcontrolname='email']");
        private readonly By SendCodeButton = By.XPath("//button[.//span[normalize-space()='Receive Code Via Email']]");
        private readonly By OtpCodeInput = By.Id("otp");
        private readonly By VerifyButton = By.Id("VerifyCodeBtn");

        public bool IsMfaDialogDisplayed()
        {
            return IsElementVisible(MfaDialog);
        }

        public void SelectEmailMethod()
        {
            Click(EmailMethodSelect);
            // Select first option or as needed
        }

        public void SendCode()
        {
            Click(SendCodeButton);
        }

        public void EnterOtpCode(string code)
        {
            EnterText(OtpCodeInput, code);
        }

        public void VerifyCode()
        {
            Click(VerifyButton);
        }
    }
}