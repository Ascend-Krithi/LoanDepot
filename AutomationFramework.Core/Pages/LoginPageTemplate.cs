// NuGet Packages: Selenium.WebDriver
using OpenQA.Selenium;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Core.Pages
{
    /// <summary>
    /// A generic template for a Login Page Object.
    /// It uses logical keys for locators instead of hardcoding them.
    /// </summary>
    public class LoginPageTemplate : BasePage
    {
        public LoginPageTemplate(IWebDriver driver) : base(driver)
        {
            // In a real project, these locators would be defined for the specific application.
            // They are mapped from logical names to actual 'By' strategies.
            Locators["UsernameField"] = By.Id("username");
            Locators["PasswordField"] = By.Name("password");
            Locators["LoginButton"] = By.CssSelector("button[type='submit']");
            Locators["ErrorMessage"] = By.ClassName("error-message");
        }

        /// <summary>
        /// Navigates to the login page using the BaseUrl from configuration.
        /// </summary>
        public void GoTo()
        {
            GoToUrl(ConfigManager.Settings.BaseUrl + "/login"); // Assumes a /login route
        }

        /// <summary>
        /// Enters the username into the username field.
        /// </summary>
        /// <param name="username">The username to enter.</param>
        public void EnterUsername(string username)
        {
            TypeText("UsernameField", username);
        }

        /// <summary>
        /// Enters the password into the password field.
        /// </summary>
        /// <param name="password">The password to enter.</param>
        public void EnterPassword(string password)
        {
            TypeText("PasswordField", password);
        }

        /// <summary>
        /// Clicks the login button.
        /// </summary>
        public void ClickLogin()
        {
            ClickElement("LoginButton");
        }

        /// <summary>
        /// A generic login method combining the steps.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        public void Login(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLogin();
        }

        /// <summary>
        /// Gets the text of an error message displayed on the page.
        /// </summary>
        /// <returns>The error message text, or an empty string if not found.</returns>
        public string GetErrorMessage()
        {
            try
            {
                var errorElement = Wait.WaitForElementVisible(Locators["ErrorMessage"]);
                return errorElement.Text;
            }
            catch (WebDriverTimeoutException)
            {
                return string.Empty;
            }
        }
    }
}