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
    /// Locators sourced from Login - LoanDepot.json
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
        // Login form
        public By loginFormLocBy = By.XPath("//form[@action='/api/login/passwordless/start' and translate(@method,'post','POST')='POST']"); // Login - LoanDepot.json
        public By emailInputLocBy = By.XPath("//input[@id='email' and @type='email']"); // Login - LoanDepot.json
        public By passwordInputLocBy = By.XPath("//input[@id='password' and @type='password']"); // Login - LoanDepot.json
        public By submitButtonLocBy = By.XPath("//form[@class='css-1ofckhl']//button[@type='submit' and contains(@class,'css-hpbufo')]"); // Login - LoanDepot.json
        #endregion

        #region Methods
        /// <summary>
        /// Performs login using provided credentials
        /// </summary>
        public void Login(string username, string password)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Attempting login with username: " + username);
                webElementExtensions.WaitForElement(_driver, loginFormLocBy, LD_AutomationFramework.Config.ConfigSettings.LoginWait);
                webElementExtensions.SendKeys(_driver, emailInputLocBy, username, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.SendKeys(_driver, passwordInputLocBy, password, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, submitButtonLocBy, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                test.Log(AventStack.ExtentReports.Status.Pass, "Login submitted successfully");
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
