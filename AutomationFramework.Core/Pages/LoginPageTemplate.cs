using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class LoginPageTemplate : BasePage
    {
        // Logical Keys
        private const string UsernameInputKey = "LoginPage.UsernameInput";
        private const string PasswordInputKey = "LoginPage.PasswordInput";
        private const string SubmitButtonKey = "LoginPage.SubmitButton";

        // Locators
        private readonly By _usernameInput = By.CssSelector("input[type='text'], input[type='email'], input[name*='user']");
        private readonly By _passwordInput = By.CssSelector("input[type='password'], input[name*='pass']");
        private readonly By _submitButton = By.CssSelector("button[type='submit'], input[type='submit']");

        public LoginPageTemplate(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement UsernameInput => FindElement(UsernameInputKey, _usernameInput);
        public IWebElement PasswordInput => FindElement(PasswordInputKey, _passwordInput);
        public IWebElement SubmitButton => FindElement(SubmitButtonKey, _submitButton);

        public void Login(string username, string password)
        {
            UsernameInput.SendKeys(username);
            PasswordInput.SendKeys(password);
            SubmitButton.Click();
        }
    }
}