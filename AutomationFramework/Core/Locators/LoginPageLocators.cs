using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class LoginPageLocators
    {
        public const string UsernameKey = "Login.Username";
        public static readonly By UsernameLocator = By.XPath("//input[@id='username']");
        public static readonly By[] UsernameAlternatives = {
            By.CssSelector("input[name='user']"),
            By.XPath("//input[contains(@placeholder, 'User')]")
        };

        public const string PasswordKey = "Login.Password";
        public static readonly By PasswordLocator = By.XPath("//input[@id='password']");
        public static readonly By[] PasswordAlternatives = {
            By.CssSelector("input[type='password']"),
            By.XPath("//input[contains(@placeholder, 'Pass')]")
        };

        public const string LoginButtonKey = "Login.LoginButton";
        public static readonly By LoginButtonLocator = By.XPath("//button[@id='loginBtn']");
        public static readonly By[] LoginButtonAlternatives = {
            By.CssSelector("button[type='submit']"),
            By.XPath("//button[contains(text(),'Login')]")
        };
    }
}