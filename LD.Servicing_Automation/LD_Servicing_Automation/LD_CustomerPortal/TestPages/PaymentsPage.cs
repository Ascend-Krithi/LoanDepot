using OpenQA.Selenium;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using AventStack.ExtentReports;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object Model for 'Make a Payment' functionality.
    /// Locators mapped from Servicing _ loanDepot_payment.html (element_count: 0, plausible XPaths used).
    /// </summary>
    public class PaymentsPage
    {
        private IWebDriver _driver;
        private ExtentTest test;
        private WebElementExtensionsPage webElementExtensions;

        public PaymentsPage(IWebDriver driver, ExtentTest test)
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
        }

        #region Locators
        // Login Page (LoginLoanDepot.html)
        public By usernameInputLocBy = By.XPath("//input[@type='text' or @type='email' or contains(@aria-label, 'Username') or contains(@placeholder, 'Username')]"); // plausible
        public By passwordInputLocBy = By.XPath("//input[@type='password' or contains(@aria-label, 'Password') or contains(@placeholder, 'Password')]"); // plausible
        public By loginButtonLocBy = By.XPath("//button[contains(normalize-space(text()), 'Login')]"); // plausible
        // Dashboard/Account Selection (Servicing _ loanDepot.html)
        public By helocAccountLocBy = By.XPath("//div[contains(@class, 'account-card') and contains(., 'HELOC')]"); // plausible
        public By makePaymentButtonLocBy = By.XPath("//button[contains(normalize-space(text()), 'Make a Payment')]"); // plausible
        // One-Time Payment (Servicing _ loanDepot_payment.html)
        public By oneTimePaymentOptionLocBy = By.XPath("//button[contains(normalize-space(text()), 'One-Time Payment')]"); // plausible
        public By paymentAmountInputLocBy = By.XPath("//input[@type='number' or contains(@aria-label, 'Amount') or contains(@placeholder, 'Amount')]"); // plausible
        public By paymentDateInputLocBy = By.XPath("//input[@type='date' or contains(@aria-label, 'Payment Date') or contains(@placeholder, 'Payment Date')]"); // plausible
        public By submitPaymentButtonLocBy = By.XPath("//button[contains(normalize-space(text()), 'Submit Payment')]"); // plausible
        public By lateFeeMessageLocBy = By.XPath("//div[contains(@class, 'late-fee-message') or contains(text(), 'Late fee')]"); // plausible
        #endregion

        #region Methods
        /// <summary>
        /// Login to the Customer Portal.
        /// </summary>
        public void Login(string username, string password)
        {
            webElementExtensions.SendKeys(_driver, usernameInputLocBy, username, ConfigSettings.LoginWait);
            webElementExtensions.SendKeys(_driver, passwordInputLocBy, password, ConfigSettings.LoginWait);
            webElementExtensions.Click(_driver, loginButtonLocBy, ConfigSettings.LoginWait);
        }

        /// <summary>
        /// Select HELOC account from dashboard.
        /// </summary>
        public void SelectHelocAccount(string accountNumber)
        {
            // Assumes account card contains account number
            By accountCardLocBy = By.XPath($"//div[contains(@class, 'account-card') and contains(., '{accountNumber}')]");
            webElementExtensions.Click(_driver, accountCardLocBy, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Navigate to Make a Payment section.
        /// </summary>
        public void NavigateToMakePayment()
        {
            webElementExtensions.Click(_driver, makePaymentButtonLocBy, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Choose One-Time Payment option.
        /// </summary>
        public void ChooseOneTimePayment()
        {
            webElementExtensions.Click(_driver, oneTimePaymentOptionLocBy, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Enter payment details and submit.
        /// </summary>
        public void EnterPaymentDetailsAndSubmit(string amount, string paymentDate)
        {
            webElementExtensions.SendKeys(_driver, paymentAmountInputLocBy, amount, ConfigSettings.WaitTime);
            webElementExtensions.SendKeys(_driver, paymentDateInputLocBy, paymentDate, ConfigSettings.WaitTime);
            webElementExtensions.Click(_driver, submitPaymentButtonLocBy, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Validate late fee message is displayed and correct.
        /// </summary>
        public bool IsLateFeeMessageDisplayed()
        {
            return webElementExtensions.IsElementDisplayed(_driver, lateFeeMessageLocBy);
        }

        /// <summary>
        /// Get late fee message text.
        /// </summary>
        public string GetLateFeeMessageText()
        {
            return webElementExtensions.GetText(_driver, lateFeeMessageLocBy, ConfigSettings.WaitTime);
        }
        #endregion
    }
}
