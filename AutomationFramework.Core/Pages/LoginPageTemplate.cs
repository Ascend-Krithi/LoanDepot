using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class LoginPageTemplate : BasePage
    {
        private readonly By emailInput = By.Id("email");
        private readonly By passwordInput = By.Id("password");
        private readonly By signInButton = By.CssSelector("button[type='submit']");

        public void OpenApplication()
        {
            Driver.Navigate().GoToUrl(AppConfig.ApplicationUrl);
            WaitHelper.WaitForElementVisible(emailInput);
        }

        public void EnterEmail(string email)
        {
            var input = FindElement("LoginPage.EmailInput", emailInput);
            input.Clear();
            input.SendKeys(email);
        }

        public void EnterPassword(string password)
        {
            var input = FindElement("LoginPage.PasswordInput", passwordInput);
            input.Clear();
            input.SendKeys(password);
        }

        public void ClickSignIn()
        {
            var button = FindElement("LoginPage.SignInButton", signInButton);
            button.Click();
        }
    }
}