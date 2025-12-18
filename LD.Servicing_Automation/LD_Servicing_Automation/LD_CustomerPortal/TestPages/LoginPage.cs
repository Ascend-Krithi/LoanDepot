using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Login Page (source: Login - LoanDepot.html)
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
        // Source mapping: Login - LoanDepot.html
        public By pageReadyLocBy = By.CssSelector("form"); // Login.PageReady
        public By usernameLocBy = By.Id("email"); // Login.Username
        public By passwordLocBy = By.Id("password"); // Login.Password
        public By submitLocBy = By.CssSelector("button[type='submit']"); // Login.Submit
        #endregion

        #region Methods
        public void WaitForLoginPage()
        {
            webElementExtensions.WaitForElement(_driver, pageReadyLocBy, ConfigSettings.LoginWait);
            test.Log(AventStack.ExtentReports.Status.Info, "Login page loaded (form element present)");
        }

        public void EnterUsername(string username)
        {
            webElementExtensions.SendKeys(_driver, usernameLocBy, username, ConfigSettings.SmallWaitTime);
            test.Log(AventStack.ExtentReports.Status.Info, $"Entered username: {username}");
        }

        public void EnterPassword(string password)
        {
            webElementExtensions.SendKeys(_driver, passwordLocBy, password, ConfigSettings.SmallWaitTime);
            test.Log(AventStack.ExtentReports.Status.Info, "Entered password");
        }

        public void ClickSubmit()
        {
            webElementExtensions.Click(_driver, submitLocBy, ConfigSettings.SmallWaitTime);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked Login Submit button");
        }

        public void Login(string username, string password)
        {
            try
            {
                WaitForLoginPage();
                EnterUsername(username);
                EnterPassword(password);
                ClickSubmit();
                test.Log(AventStack.ExtentReports.Status.Pass, "Login submitted successfully");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"Login failed: {ex.Message}");
                throw;
            }
        }
        #endregion
    }
}
