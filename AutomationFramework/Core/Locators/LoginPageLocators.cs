using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class LoginPageLocators
    {
        public const string UsernameInputKey = "Login.UsernameInput";
        public static readonly By UsernameInput = By.XPath("//input[@id='username']");
        public static readonly By[] UsernameInputAlternatives = {
            By.XPath("//input[@name='user']"),
            By.CssSelector("input.login-username")
        };

        public const string PasswordInputKey = "Login.PasswordInput";
        public static readonly By PasswordInput = By.XPath("//input[@id='password']");
        public static readonly By[] PasswordInputAlternatives = {
            By.XPath("//input[@name='pass']"),
            By.CssSelector("input.login-password")
        };

        public const string LoginButtonKey = "Login.LoginButton";
        public static readonly By LoginButton = By.XPath("//button[@id='loginBtn']");
        public static readonly By[] LoginButtonAlternatives = {
            By.XPath("//button[contains(text(),'Sign In')]"),
            By.CssSelector("button.login-submit")
        };
    }
}