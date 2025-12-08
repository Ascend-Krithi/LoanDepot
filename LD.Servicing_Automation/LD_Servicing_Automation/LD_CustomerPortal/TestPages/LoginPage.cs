using OpenQA.Selenium;
using AventStack.ExtentReports;
using SeleniumExtras.PageObjects;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Customer Portal Login Page
    /// Locators derived from LoginLoanDepot.html (element_count: 0, plausible XPath used)
    /// </summary>
    public class LoginPage : BasePage
    {
        public IWebDriver _driver { get; set; }
        public ExtentTest test { get; set; }
        public WebElementExtensionsPage webElementExtensions;

        public LoginPage(IWebDriver driver, ExtentTest test) : base()
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
        }

        #region Locators
        // Username input (plausible XPath)
        public By usernameInputLocBy = By.XPath("//input[@type='text' or @type='email' or contains(@aria-label, 'Username') or contains(@placeholder, 'Username')]"); // Source: LoginLoanDepot.html
        // Password input (plausible XPath)
        public By passwordInputLocBy = By.XPath("//input[@type='password' or contains(@aria-label, 'Password') or contains(@placeholder, 'Password')]"); // Source: LoginLoanDepot.html
        // Login button (plausible XPath)
        public By loginButtonLocBy = By.XPath("//button[contains(normalize-space(text()), 'Login') or contains(@aria-label, 'Login')]"); // Source: LoginLoanDepot.html
        #endregion

        #region Methods
        public void Login(string username, string password)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Entering username on Login Page");
                webElementExtensions.SendKeys(_driver, usernameInputLocBy, username, ConfigSettings.WaitTime);
                test.Log(AventStack.ExtentReports.Status.Info, "Entering password on Login Page");
                webElementExtensions.SendKeys(_driver, passwordInputLocBy, password, ConfigSettings.WaitTime);
                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Login button");
                webElementExtensions.Click(_driver, loginButtonLocBy, ConfigSettings.WaitTime);
            }
            catch (NoSuchElementException ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Login element not found: " + ex.Message);
                throw;
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
