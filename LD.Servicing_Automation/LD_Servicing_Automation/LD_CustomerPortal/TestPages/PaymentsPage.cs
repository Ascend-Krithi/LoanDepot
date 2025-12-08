using OpenQA.Selenium;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object Model for 'Make a Payment' functionality.
    /// Locators mapped from Servicing _ loanDepot_payment.html (element_count: 0, plausible XPath used).
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
        // Login page locators (from LoginLoanDepot.html, element_count: 0, plausible XPath)
        public By usernameInputLocBy = By.XPath("//input[@name='username' or @id='username']"); // LoginLoanDepot.html
        public By passwordInputLocBy = By.XPath("//input[@name='password' or @id='password']"); // LoginLoanDepot.html
        public By loginButtonLocBy = By.XPath("//button[contains(text(),'Login') or @id='loginBtn']"); // LoginLoanDepot.html

        // Dashboard/Account selection (from Servicing _ loanDepot.html, element_count: 0)
        public By helocAccountLocBy = By.XPath("//div[contains(@class,'account-card') and contains(text(),'HELOC')]"); // Servicing _ loanDepot.html
        public By makePaymentButtonLocBy = By.XPath("//button[contains(text(),'Make a Payment')]"); // Servicing _ loanDepot.html

        // Payment screen (from Servicing _ loanDepot_payment.html, element_count: 0)
        public By oneTimePaymentRadioLocBy = By.XPath("//input[@type='radio' and @value='One-Time Payment']"); // Servicing _ loanDepot_payment.html
        public By paymentAmountInputLocBy = By.XPath("//input[@name='paymentAmount' or @id='paymentAmount']"); // Servicing _ loanDepot_payment.html
        public By paymentDateInputLocBy = By.XPath("//input[@name='paymentDate' or @id='paymentDate']"); // Servicing _ loanDepot_payment.html
        public By submitPaymentButtonLocBy = By.XPath("//button[contains(text(),'Submit Payment')]"); // Servicing _ loanDepot_payment.html
        public By lateFeeMessageLocBy = By.XPath("//div[contains(@class,'late-fee-message') or contains(text(),'Late fee')] "); // Servicing _ loanDepot_payment.html
        #endregion

        #region Methods
        public void Login(string username, string password)
        {
            webElementExtensions.SendKeys(_driver, usernameInputLocBy, username, ConfigSettings.LoginWait);
            webElementExtensions.SendKeys(_driver, passwordInputLocBy, password, ConfigSettings.LoginWait);
            webElementExtensions.Click(_driver, loginButtonLocBy, ConfigSettings.LoginWait);
        }

        public void SelectHELOCAccount(string accountNumber)
        {
            webElementExtensions.WaitForElement(_driver, helocAccountLocBy, ConfigSettings.WaitTime);
            webElementExtensions.Click(_driver, helocAccountLocBy, ConfigSettings.WaitTime);
        }

        public void NavigateToMakePayment()
        {
            webElementExtensions.WaitForElement(_driver, makePaymentButtonLocBy, ConfigSettings.WaitTime);
            webElementExtensions.Click(_driver, makePaymentButtonLocBy, ConfigSettings.WaitTime);
        }

        public void ChooseOneTimePayment()
        {
            webElementExtensions.WaitForElement(_driver, oneTimePaymentRadioLocBy, ConfigSettings.WaitTime);
            webElementExtensions.Click(_driver, oneTimePaymentRadioLocBy, ConfigSettings.WaitTime);
        }

        public void EnterPaymentDetails(string amount, string paymentDate)
        {
            webElementExtensions.SendKeys(_driver, paymentAmountInputLocBy, amount, ConfigSettings.WaitTime);
            webElementExtensions.SendKeys(_driver, paymentDateInputLocBy, paymentDate, ConfigSettings.WaitTime);
        }

        public void SubmitPayment()
        {
            webElementExtensions.Click(_driver, submitPaymentButtonLocBy, ConfigSettings.WaitTime);
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return webElementExtensions.IsElementDisplayed(_driver, lateFeeMessageLocBy);
        }

        public string GetLateFeeMessage()
        {
            return webElementExtensions.GetText(_driver, lateFeeMessageLocBy, ConfigSettings.WaitTime);
        }
        #endregion
    }
}
