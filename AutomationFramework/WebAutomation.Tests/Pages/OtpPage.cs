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

        public void EnterStaticOtpAndVerify()
        {
            var staticOtp = ConfigManager.Settings.StaticOtp;
            Wait.UntilVisible(_locators.GetBy("Otp.Code.Input")).SendKeys(staticOtp);
            Wait.UntilClickable(_locators.GetBy("Otp.Verify.Button")).Click();
        }
    }
}