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
            PageFactory.InitElements(driver, this);
        }

        #region Locators
        // Username input (plausible XPath, LoginLoanDepot.html)
        public By usernameInputLocBy = By.XPath("//input[@type='text' or @type='email' or contains(@aria-label, 'Username') or contains(@placeholder, 'Username')]");
        // Password input (plausible XPath, LoginLoanDepot.html)
        public By passwordInputLocBy = By.XPath("//input[@type='password' or contains(@aria-label, 'Password') or contains(@placeholder, 'Password')]");
        // Login button (plausible XPath, LoginLoanDepot.html)
        public By loginButtonLocBy = By.XPath("//button[contains(normalize-space(text()), 'Login') or contains(@aria-label, 'Login')]");
        #endregion

        #region Methods
        public void Login(string username, string password)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Entering username on Login Page");
                webElementExtensions.WaitForElement(_driver, usernameInputLocBy, ConfigSettings.WaitTime);
                webElementExtensions.SendKeys(_driver, usernameInputLocBy, username, ConfigSettings.WaitTime);

                test.Log(AventStack.ExtentReports.Status.Info, "Entering password on Login Page");
                webElementExtensions.WaitForElement(_driver, passwordInputLocBy, ConfigSettings.WaitTime);
                webElementExtensions.SendKeys(_driver, passwordInputLocBy, password, ConfigSettings.WaitTime);

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Login button");
                webElementExtensions.WaitForElementClickable(_driver, loginButtonLocBy, ConfigSettings.WaitTime);
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
