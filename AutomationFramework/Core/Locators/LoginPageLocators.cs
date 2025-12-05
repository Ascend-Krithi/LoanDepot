using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class LoginPageLocators
    {
        public static By UsernameById => By.Id("username");
        public static By UsernameByXpath => By.XPath("//input[@name='username' or @placeholder='Username']");
        public static By UsernameByCss => By.CssSelector("input[type='text'].login-field");

        public static By PasswordById => By.Id("password");
        public static By PasswordByXpath => By.XPath("//input[@name='password']");
        public static By PasswordByCss => By.CssSelector("input[type='password']");

        public static By LoginButtonByXpath => By.XPath("//button[normalize-space()='Log In' or @type='submit']");
        public static By LoginButtonByCss => By.CssSelector(".btn-primary.login-btn");
    }
}
