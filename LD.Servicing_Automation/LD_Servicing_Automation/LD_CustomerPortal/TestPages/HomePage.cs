using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using log4net;
using OpenQA.Selenium;
using System;

namespace LD_CustomerPortal.Pages
{
    public class HomePage
    {
        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }
        ReportLogger reportLogger { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        WebElementExtensionsPage webElementExtensions = null;
        public HomePage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            reportLogger = new ReportLogger(_driver);
        }

        #region Locators        

        public By myAccountBtnLocBy = By.Id("header_my_account");

        public By loanServicingLocBy = By.XPath("//span[text()='Loan Servicing']");

        public By loginButtonLocBy = By.XPath("//span[text()='Log In']/../parent::button[@aria-label='']");

        public By signInButtonLocBy = By.XPath("//*[text()='Sign in']");

        public By regLinkLocBy = By.Id("texas-register-link");

        public By firstNameLocBy = By.Id("extension_FirstName");

        public By signUpheaderLocBy = By.Id("sign-up-header");
       
        #endregion Locators

        #region Services

        #endregion
    }
}