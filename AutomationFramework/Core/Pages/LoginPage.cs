using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.Locators;
using AutomationFramework.Core.Encryption;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(SelfHealingWebDriver driver) : base(driver) { }

        public void EnterUsername(string username)
        {
            var usernameInput = Driver.FindElement(LoginPageLocators.UsernameInput);
            usernameInput.Clear();
            usernameInput.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            var passwordInput = Driver.FindElement(LoginPageLocators.PasswordInput);
            passwordInput.Clear();
            passwordInput.SendKeys(password);
        }

        public void ClickLogin()
        {
            var loginButton = Driver.FindElement(LoginPageLocators.LoginButton);
            loginButton.Click();
        }

        public string GetErrorMessage()
        {
            var error = Driver.FindElement(LoginPageLocators.ErrorMessage);
            return error.Text;
        }
    }
}