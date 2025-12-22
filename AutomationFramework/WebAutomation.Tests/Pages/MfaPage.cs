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

        public void CompleteMfa()
        {
            Wait.UntilVisible(_locators.GetBy("Mfa.Dialog"));
            Driver.FindElement(_locators.GetBy("Mfa.EmailMethod.Select")).Click();
            Driver.FindElement(_locators.GetBy("Mfa.SendCode.Button")).Click();
        }
    }
}