using AutomationFramework.Core.Base;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(SelfHealingWebDriver driver) : base(driver) { }

        public void EnterUsername(string username)
        {
            HandleUniversalPopups();
            Driver.SendKeys("UsernameInput", username);
        }

        public void EnterPassword(string password)
        {
            Driver.SendKeys("PasswordInput", password);
        }

        public void ClickLogin()
        {
            Driver.Click("LoginButton");
        }

        public void ClickRememberMe()
        {
            Driver.Click("RememberMeCheckbox");
        }

        public void ClickForgotPassword()
        {
            Driver.Click("ForgotPasswordLink");
        }

        public string GetLoginErrorMessage()
        {
            return Driver.GetText("LoginErrorMessage");
        }

        public void ClickSignInWithSSO()
        {
            Driver.Click("SignInWithSSOButton");
        }

        public void SelectTenant(string tenant)
        {
            Driver.SelectDropdownByText("TenantDropdown", tenant);
        }
    }
}