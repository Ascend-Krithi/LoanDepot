using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class LoginPageTemplate : BasePage
    {
        public const string UsernameInputKey = "LoginPage.UsernameInput";
        public const string PasswordInputKey = "LoginPage.PasswordInput";
        public const string SubmitButtonKey = "LoginPage.SubmitButton";

        private readonly By usernameInput = By.CssSelector("input[type='text'], input[name*='user']");
        private readonly By passwordInput = By.CssSelector("input[type='password']");
        private readonly By submitButton = By.CssSelector("button[type='submit'], input[type='submit']");

        public LoginPageTemplate(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement UsernameInput => FindElement(UsernameInputKey, usernameInput);
        public IWebElement PasswordInput => FindElement(PasswordInputKey, passwordInput);
        public IWebElement SubmitButton => FindElement(SubmitButtonKey, submitButton);
    }
}