using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Security;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver) { }

        private IWebElement UsernameField => Wait.UntilVisible(By.Id("username"));
        private IWebElement PasswordField => Wait.UntilVisible(By.Id("password"));
        private IWebElement LoginButton => Wait.UntilClickable(By.CssSelector("button[type='submit']"));

        public void EnterCredentials()
        {
            var (username, password) = CredentialProvider.GetDefaultCredentials();
            UsernameField.SendKeys(username);
            PasswordField.SendKeys(password);
        }

        public void ClickLogin()
        {
            LoginButton.Click();
        }

        public bool IsOnLoginPage()
        {
            return Wait.UntilPresent(By.Id("username"));
        }
    }
}