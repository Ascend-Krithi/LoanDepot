using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class LoginPageTemplate : BasePage
    {
        public LoginPageTemplate(SelfHealingWebDriver driver) : base(driver) { }

        // Locators using logical keys
        private IWebElement UsernameInput => FindElement("LoginPage.UsernameInput", By.Id("username"));
        private IWebElement PasswordInput => FindElement("LoginPage.PasswordInput", By.Id("password"));
        private IWebElement LoginButton => FindElement("LoginPage.LoginButton", By.CssSelector("button[type='submit']"));
        private IWebElement ErrorMessage => FindElement("LoginPage.ErrorMessage", By.ClassName("error-message"));

        public void EnterUsername(string username)
        {
            UsernameInput.Clear();
            UsernameInput.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            PasswordInput.Clear();
            PasswordInput.SendKeys(password);
        }

        public void ClickLogin()
        {
            LoginButton.Click();
        }

        public string GetErrorMessage()
        {
            return ErrorMessage.Text;
        }

        public void LoginAs(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLogin();
        }
    }
}