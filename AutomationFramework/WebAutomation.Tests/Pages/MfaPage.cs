using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public MfaPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators/Locators.txt");
        }

        public void SelectEmailMethodAndSendCode()
        {
            Wait.UntilVisible(_locators.GetBy("Mfa.Dialog"));
            Wait.UntilClickable(_locators.GetBy("Mfa.EmailMethod.Select")).Click();
            Wait.UntilClickable(_locators.GetBy("Mfa.SendCode.Button")).Click();
        }
    }
}