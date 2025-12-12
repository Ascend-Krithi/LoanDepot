// AutomationFramework.Core/Pages/LoginPage.cs

using AutomationFramework.Core.Drivers;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    /// <summary>
    /// Page Template for the Login Page.
    /// Contains atomic methods for interacting with login page elements.
    /// </summary>
    public class LoginPage : BasePage
    {
        // Logical keys for elements, matching Locators.json
        private const string UsernameField = "LoginPage.Username";
        private const string PasswordField = "LoginPage.Password";
        private const string LoginButton = "LoginPage.LoginButton";
        private const string ErrorMessage = "LoginPage.ErrorMessage";

        public LoginPage(SelfHealingWebDriver driver) : base(driver)
        {
        }

        public void EnterUsername(string username)
        {
            var usernameElement = Driver.FindElement(UsernameField);
            usernameElement.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            var passwordElement = Driver.FindElement(PasswordField);
            passwordElement.SendKeys(password);
        }

        public void ClickLoginButton()
        {
            Driver.FindElement(LoginButton).Click();
        }

        public bool IsErrorMessageDisplayed()
        {
            // Wait for the element to be visible before checking
            return Wait.UntilElementIsVisible(ErrorMessage);
        }

        public string GetErrorMessageText()
        {
            if (IsErrorMessageDisplayed())
            {
                return Driver.FindElement(ErrorMessage).Text;
            }
            return string.Empty;
        }
    }
}