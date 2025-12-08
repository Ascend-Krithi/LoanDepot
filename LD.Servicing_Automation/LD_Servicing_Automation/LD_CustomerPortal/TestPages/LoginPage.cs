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
    /// Locators derived from LoginLoanDepot.html (element_count: 0, plausible XPath used)
    /// </summary>
    public class LoginPage
    {
        private IWebDriver _driver;
        private ExtentTest test;
        private WebElementExtensionsPage webElementExtensions;

        public LoginPage(IWebDriver driver, ExtentTest test)
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
        }

        #region Locators
        // Username input (plausible XPath, LoginLoanDepot.html)
        public By usernameInput = By.XPath("//input[@type='text' or @type='email' or contains(@aria-label, 'Username') or contains(@placeholder, 'Username')]");
        // Password input (plausible XPath, LoginLoanDepot.html)
        public By passwordInput = By.XPath("//input[@type='password' or contains(@aria-label, 'Password') or contains(@placeholder, 'Password')]");
        // Login button (plausible XPath, LoginLoanDepot.html)
        public By loginButton = By.XPath("//button[contains(normalize-space(text()), 'Login') or contains(@aria-label, 'Login')]");
        #endregion

        #region Methods
        public void Login(string username, string password)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Entering username on Login page.");
                webElementExtensions.WaitForElement(_driver, usernameInput, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.SendKeys(_driver, usernameInput, username, LD_AutomationFramework.Config.ConfigSettings.WaitTime);

                test.Log(AventStack.ExtentReports.Status.Info, "Entering password on Login page.");
                webElementExtensions.WaitForElement(_driver, passwordInput, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.SendKeys(_driver, passwordInput, password, LD_AutomationFramework.Config.ConfigSettings.WaitTime);

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Login button.");
                webElementExtensions.WaitForElementClickable(_driver, loginButton, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, loginButton, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Login failed: " + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
