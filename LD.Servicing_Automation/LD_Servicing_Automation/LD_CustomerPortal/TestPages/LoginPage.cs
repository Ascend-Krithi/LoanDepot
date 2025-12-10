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
    /// Locators sourced from: Login - LoanDepot.html
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
        [FindsBy(How = How.XPath, Using = "//input[@id='email']")]
        public IWebElement EmailInput;

        [FindsBy(How = How.XPath, Using = "//input[@id='password']")]
        public IWebElement PasswordInput;

        [FindsBy(How = How.XPath, Using = "//button[@id='next']")]
        public IWebElement NextButton;
        #endregion

        #region Methods
        public void Login(string username, string password)
        {
            try
            {
                test.Log(Status.Info, "Entering username on Login Page.");
                EmailInput.Clear();
                EmailInput.SendKeys(username);

                test.Log(Status.Info, "Entering password on Login Page.");
                PasswordInput.Clear();
                PasswordInput.SendKeys(password);

                test.Log(Status.Info, "Clicking Next button on Login Page.");
                NextButton.Click();
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Login failed: " + ex.Message);
                throw;
            }
        }
        #endregion
    }
}