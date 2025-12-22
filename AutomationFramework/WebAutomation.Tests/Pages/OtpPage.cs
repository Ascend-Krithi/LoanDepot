using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Tests.Pages
{
    public class OtpPage : BasePage
    {
        public OtpPage(IWebDriver driver) : base(driver) { }

        public void EnterOtpAndVerify()
        {
            Wait.UntilVisible(By.Id("otp")).SendKeys(ConfigManager.Settings.StaticOtp);
            Wait.UntilClickable(By.Id("VerifyCodeBtn")).Click();
        }
    }
}