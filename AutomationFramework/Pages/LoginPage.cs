using OpenQA.Selenium;

namespace AutomationFramework.Pages
{
    public class LoginPage : BasePage
    {
        private readonly By PageReady = By.CssSelector("form");
        private readonly By Username = By.Id("email");
        private readonly By Password = By.Id("password");
        private readonly By SubmitButton = By.CssSelector("button[type='submit']");

        public void LaunchApplication()
        {
            Driver.Navigate().GoToUrl(Config.ApplicationUrl);
        }

        public bool IsPageReady()
        {
            return IsElementVisible(PageReady);
        }

        public void Login(string username, string password)
        {
            EnterText(Username, username);
            EnterText(Password, password);
            Click(SubmitButton);
        }
    }
}