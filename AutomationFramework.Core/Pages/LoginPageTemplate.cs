using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class LoginPageTemplate : BasePage
    {
        public const string UsernameInputKey = "LoginPage.UsernameInput";
        public const string PasswordInputKey = "LoginPage.PasswordInput";
        public const string SubmitButtonKey = "LoginPage.SubmitButton";

        private readonly By _usernameInput = By.CssSelector("input[type='text'],input[type='email']");
        private readonly By _passwordInput = By.CssSelector("input[type='password']");
        private readonly By _submitButton = By.CssSelector("button[type='submit'],input[type='submit']");

        public LoginPageTemplate(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement UsernameInput => FindElement(UsernameInputKey, _usernameInput);
        public IWebElement PasswordInput => FindElement(PasswordInputKey, _passwordInput);
        public IWebElement SubmitButton => FindElement(SubmitButtonKey, _submitButton);

        public void Login(string username, string password)
        {
            UsernameInput.Clear();
            UsernameInput.SendKeys(username);
            PasswordInput.Clear();
            PasswordInput.SendKeys(password);
            JsClick(SubmitButton);
        }
    }
}