using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class MFAPage : BasePage
    {
        public MFAPage(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement PageReady() => FindElement("Mfa.PageReady", By.CssSelector("mat-select[id='email'], mat-select[formcontrolname*='email']"));
        public IWebElement EmailInput() => FindElement("Mfa.EmailInput", By.Id("emailInput"));
        public IWebElement RequestEmailCode() => FindElement("Mfa.RequestEmailCode", By.Id("emailCodeBtn"));
    }
}