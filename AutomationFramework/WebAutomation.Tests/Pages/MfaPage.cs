using OpenQA.Selenium;
using WebAutomation.Core.Pages;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage : BasePage
    {
        public MfaPage(IWebDriver driver) : base(driver) { }

        public void SelectEmailAndSendCode()
        {
            Wait.UntilVisible(By.CssSelector("mat-select[formcontrolname='email']")).Click();
            Wait.UntilClickable(By.CssSelector("mat-option")).Click();
            Wait.UntilClickable(By.XPath("//button[.//span[normalize-space()='Receive Code Via Email']]")).Click();
        }
    }
}