using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Core.PopupEngine;

namespace AutomationFramework.Core.Pages
{
    public class LoginPageTemplate : BasePage
    {
        public const string EmailInputKey = "Login.EmailInput";
        public const string PasswordInputKey = "Login.PasswordInput";
        public const string SignInButtonKey = "Login.SignInButton";

        public LoginPageTemplate(SelfHealingWebDriver driver, WaitHelper waitHelper, PopupEngine popupEngine)
            : base(driver, waitHelper, popupEngine) { }

        public void EnterEmail(string email)
        {
            FindElement(EmailInputKey).SendKeys(email);
        }

        public void EnterPassword(string password)
        {
            FindElement(PasswordInputKey).SendKeys(password);
        }

        public void ClickSignIn()
        {
            FindElement(SignInButtonKey).Click();
        }

        public override void WaitForPageToLoad()
        {
            WaitHelper.WaitForElementVisible(EmailInputKey);
        }
    }
}