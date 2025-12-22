using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Tests.Pages
{
    public class OtpPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public OtpPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators/Locators.txt");
        }

        public void EnterOtpAndVerify()
        {
            Wait.UntilVisible(_locators.GetBy("Otp.Code.Input"));
            Driver.FindElement(_locators.GetBy("Otp.Code.Input")).SendKeys(ConfigManager.Settings.StaticOtp);
            Driver.FindElement(_locators.GetBy("Otp.Verify.Button")).Click();
        }
    }
}