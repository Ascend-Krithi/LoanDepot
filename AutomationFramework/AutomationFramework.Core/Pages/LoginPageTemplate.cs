using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class LoginPageTemplate : BasePage
    {
        public LoginPageTemplate(IWebDriver driver) : base(driver) { }

        public By UsernameField => By.Id("username");
        public By PasswordField => By.Id("password");
        public By LoginButton => By.Id("login");

        public void Login(string username, string password)
        {
            EnterText(UsernameField, username);
            EnterText(PasswordField, password);
            Click(LoginButton);
        }

        public override bool IsLoaded()
        {
            return Driver.FindElement(UsernameField).Displayed;
        }
    }
}