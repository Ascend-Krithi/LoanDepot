using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Customer Portal Login Page
    /// Source HTML: LoginLoanDepot.html (no interactive tags found, plausible locators used)
    /// </summary>
    public class LoginPage : BasePage
    {
        private IWebDriver _driver;
        private ExtentTest test;
        private WebElementExtensionsPage webElementExtensions;

        public LoginPage(IWebDriver driver, ExtentTest test) : base()
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
            PageFactory.InitElements(driver, this);
        }

        #region Locators
        // Username input (plausible XPath)
        public By usernameInput = By.XPath("//input[@type='text' or @type='email' or contains(@aria-label, 'Username') or contains(@placeholder, 'Username')]");
        // Password input (plausible XPath)
        public By passwordInput = By.XPath("//input[@type='password' or contains(@aria-label, 'Password') or contains(@placeholder, 'Password')]");
        // Login button (plausible XPath)
        public By loginButton = By.XPath("//button[contains(normalize-space(text()), 'Login') or contains(@aria-label, 'Login')]");
        #endregion

        #region Methods
        /// <summary>
        /// Logs in to the Customer Portal
        /// </summary>
        public void Login(string username, string password)
        {
            test.Log(AventStack.ExtentReports.Status.Info, "Entering username and password on LoginPage.");
            webElementExtensions.SendKeys(_driver, usernameInput, username, ConfigSettings.WaitTime);
            webElementExtensions.SendKeys(_driver, passwordInput, password, ConfigSettings.WaitTime);
            webElementExtensions.Click(_driver, loginButton, ConfigSettings.WaitTime);
        }
        #endregion
    }
}
