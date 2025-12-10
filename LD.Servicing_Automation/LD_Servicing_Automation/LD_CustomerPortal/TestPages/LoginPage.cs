using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using AventStack.ExtentReports;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Customer Portal Login Page
    /// Locators sourced from: Login - LoanDepot.html
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
        // Email Address Input (Login - LoanDepot.html)
        public By emailInputLocBy = By.XPath("//*[@id='email']");
        // Password Input (Login - LoanDepot.html)
        public By passwordInputLocBy = By.XPath("//*[@id='password']");
        // Next Button (Login - LoanDepot.html)
        public By nextButtonLocBy = By.XPath("//*[@id='next']");
        // Login Logo Home Link (Login - LoanDepot.html)
        public By logoHomeLinkLocBy = By.XPath("//*[@id='logo-home-link']");
        // Forgot Password Link (Login - LoanDepot.html)
        public By forgotPasswordLocBy = By.XPath("//*[@id='forgotPassword']");
        // Create Account Link (Login - LoanDepot.html)
        public By createAccountLocBy = By.XPath("//*[@id='createAccount']");
        #endregion

        #region Methods
        public void Login(string username, string password)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Entering username on Login Page");
                webElementExtensions.SendKeys(_driver, emailInputLocBy, username, ConfigSettings.WaitTime);
                test.Log(AventStack.ExtentReports.Status.Info, "Entering password on Login Page");
                webElementExtensions.SendKeys(_driver, passwordInputLocBy, password, ConfigSettings.WaitTime);
                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Next button on Login Page");
                webElementExtensions.Click(_driver, nextButtonLocBy, ConfigSettings.WaitTime);
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
