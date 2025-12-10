using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using System;

namespace LD_Servicing_Automation.LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Login Page (Source: Login - LoanDepot.html)
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

        #region Locators (Strict Source Mapping: Login - LoanDepot.html)
        [FindsBy(How = How.XPath, Using = "#logo-home-link")]
        public IWebElement LogoHomeLink;

        [FindsBy(How = How.XPath, Using = "#email")]
        public IWebElement EmailAddressInput;

        [FindsBy(How = How.XPath, Using = "#password")]
        public IWebElement PasswordInput;

        [FindsBy(How = How.XPath, Using = "[aria-label='Show Password']")]
        public IWebElement ShowPasswordButton;

        [FindsBy(How = How.XPath, Using = "#next")]
        public IWebElement NextButton;

        [FindsBy(How = How.XPath, Using = "#forgotEmailLink")]
        public IWebElement ForgotEmailLink;

        [FindsBy(How = How.XPath, Using = "#forgotPassword")]
        public IWebElement ForgotPasswordLink;

        [FindsBy(How = How.XPath, Using = "#createAccount")]
        public IWebElement CreateAccountLink;

        [FindsBy(How = How.XPath, Using = "a.f-small.fg-footer-link.no-underline.underline-hov")]
        public IWebElement FooterSmallLink;

        [FindsBy(How = How.XPath, Using = "a.fg-footer-link.no-underline")]
        public IWebElement FooterLink;
        #endregion

        #region Methods
        /// <summary>
        /// Login to the portal using username and password
        /// </summary>
        public void Login(string username, string password)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Entering email address");
                EmailAddressInput.Clear();
                EmailAddressInput.SendKeys(username);

                test.Log(AventStack.ExtentReports.Status.Info, "Entering password");
                PasswordInput.Clear();
                PasswordInput.SendKeys(password);

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Next button");
                NextButton.Click();
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
