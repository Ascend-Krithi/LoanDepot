using OpenQA.Selenium;
using WebAutomation.Core.Utilities;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage
    {
        private readonly IWebDriver _driver;
        private readonly SmartWait _wait;

        public MfaPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new SmartWait(driver);
        }

        public bool IsPageReady()
        {
            return _wait.UntilPresent(By.CssSelector("mat-dialog-container"), 10);
        }

        public void SelectEmailAndSendCode()
        {
            _wait.UntilClickable(By.CssSelector("mat-select[formcontrolname='email']")).Click();
            _wait.UntilClickable(By.XPath("//mat-option[1]")).Click();
            _wait.UntilClickable(By.XPath("//button[.//span[normalize-space()='Receive Code Via Email']]")).Click();
        }

        public void EnterOtpAndVerify()
        {
            var otp = WebAutomation.Core.Configuration.ConfigManager.Settings.StaticOtp;
            _wait.UntilVisible(By.Id("otp")).SendKeys(otp);
            _wait.UntilClickable(By.Id("VerifyCodeBtn")).Click();
        }
    }
}