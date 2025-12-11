using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class LoginPageLocators
    {
        public const string UsernameInputKey = "LoginPage.UsernameInput";
        public static readonly By UsernameInput = By.XPath("//input[@id='username']");
        public static readonly By[] UsernameInputAlternatives = {
            By.CssSelector("input[name='user']"),
            By.XPath("//input[contains(@placeholder, 'User')]")
        };

        public const string PasswordInputKey = "LoginPage.PasswordInput";
        public static readonly By PasswordInput = By.XPath("//input[@id='password']");
        public static readonly By[] PasswordInputAlternatives = {
            By.CssSelector("input[type='password']"),
            By.XPath("//input[contains(@placeholder, 'Pass')]")
        };

        public const string LoginButtonKey = "LoginPage.LoginButton";
        public static readonly By LoginButton = By.XPath("//button[@id='loginBtn']");
        public static readonly By[] LoginButtonAlternatives = {
            By.CssSelector("button[type='submit']"),
            By.XPath("//button[contains(text(),'Login')]")
        };
    }
}