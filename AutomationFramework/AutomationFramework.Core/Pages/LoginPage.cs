using AutomationFramework.Core.Engines;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(SelfHealingWebDriver driver) : base(driver) { }

        public void NavigateTo()
        {
            Driver.Navigate().GoToUrl(ConfigHelper.Get("AppUrl"));
        }

        public void EnterUsername(string username)
        {
            FindElement("LoginPage.Username").SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            FindElement("LoginPage.Password").SendKeys(password);
        }

        public void ClickLogin()
        {
            FindElement("LoginPage.LoginButton").Click();
        }

        public void Login(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLogin();
        }

        public string GetErrorMessage()
        {
            return FindElement("LoginPage.ErrorMessage").Text;
        }
    }
}