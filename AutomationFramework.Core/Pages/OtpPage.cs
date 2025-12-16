using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class OtpPage : BasePage
    {
        public OtpPage(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement PageReady() => FindElement("Otp.PageReady", By.Id("verificationCode"));
        public IWebElement CodeInput() => FindElement("Otp.CodeInput", By.Id("verificationCode"));
        public IWebElement Verify() => FindElement("Otp.Verify", By.CssSelector("button[type='submit']"));
    }
}