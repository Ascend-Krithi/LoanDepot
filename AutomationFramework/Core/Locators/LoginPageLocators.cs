using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class LoginPageLocators
    {
        public const string UsernameInput = "LoginPage.UsernameInput";
        public const string PasswordInput = "LoginPage.PasswordInput";
        public const string LoginButton = "LoginPage.LoginButton";
        public const string ErrorMessage = "LoginPage.ErrorMessage";

        public static readonly By[] UsernameInputAlternatives = new By[]
        {
            By.Id("username"),
            By.Name("username"),
            By.CssSelector("input[formcontrolname='username']")
        };

        public static readonly By[] PasswordInputAlternatives = new By[]
        {
            By.Id("password"),
            By.Name("password"),
            By.CssSelector("input[formcontrolname='password']")
        };

        public static readonly By[] LoginButtonAlternatives = new By[]
        {
            By.CssSelector("button[type='submit']"),
            By.XPath("//button[contains(text(),'Login')]")
        };

        public static readonly By[] ErrorMessageAlternatives = new By[]
        {
            By.CssSelector(".error-message"),
            By.XPath("//div[contains(@class,'error')]")
        };

        public static Dictionary<string, By[]> GetLocators()
        {
            return new Dictionary<string, By[]>
            {
                { UsernameInput, UsernameInputAlternatives },
                { PasswordInput, PasswordInputAlternatives },
                { LoginButton, LoginButtonAlternatives },
                { ErrorMessage, ErrorMessageAlternatives }
            };
        }
    }
}