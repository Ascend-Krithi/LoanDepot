using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class LoginPageTemplate : BasePage
    {
        public LoginPageTemplate(IWebDriver driver) : base(driver)
        {
            // In a real project, these might be loaded from a JSON/XML file
            Locators["UsernameField"] = By.Id("username");
            Locators["PasswordField"] = By.Id("password");
            Locators["LoginButton"] = By.CssSelector("button[type='submit']");
            Locators["ErrorMessage"] = By.Id("error-message");
        }

        public void EnterUsername(string username)
        {
            SendKeys("UsernameField", username);
        }

        public void EnterPassword(string password)
        {
            SendKeys("PasswordField", password);
        }

        public void ClickLogin()
        {
            Click("LoginButton");
        }

        public string GetErrorMessage()
        {
            return GetText("ErrorMessage");
        }

        public void Login(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLogin();
        }
    }
}