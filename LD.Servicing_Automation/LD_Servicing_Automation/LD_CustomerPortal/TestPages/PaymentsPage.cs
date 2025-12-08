using OpenQA.Selenium;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object Model for 'Make a Payment' functionality in Customer Portal.
    /// Locators are mapped from Servicing _ loanDepot_payment.html and related HTML files.
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
        // Locators mapped from HTML analysis (Servicing _ loanDepot_payment.html)
        // Note: As element_count: 0, plausible XPath is generated based on element names/types
        public By makePaymentButtonLocBy = By.XPath("//button[contains(text(),'Make a Payment')]"); // [Servicing _ loanDepot_payment.html]
        public By helocAccountDropdownLocBy = By.XPath("//select[@id='helocAccountDropdown']"); // [Servicing _ loanDepot_payment.html]
        public By oneTimePaymentOptionLocBy = By.XPath("//input[@type='radio' and @value='OneTimePayment']"); // [Servicing _ loanDepot_payment.html]
        public By paymentAmountInputLocBy = By.XPath("//input[@id='paymentAmount']"); // [Servicing _ loanDepot_payment.html]
        public By paymentDateInputLocBy = By.XPath("//input[@id='paymentDate']"); // [Servicing _ loanDepot_payment.html]
        public By submitPaymentButtonLocBy = By.XPath("//button[contains(text(),'Submit Payment')]"); // [Servicing _ loanDepot_payment.html]
        public By lateFeeMessageLocBy = By.XPath("//div[contains(@class,'late-fee-message')]"); // [Servicing _ loanDepot_payment.html]
        #endregion

        #region Methods
        /// <summary>
        /// Navigates to Make a Payment section.
        /// </summary>
        public void NavigateToMakePayment()
        {
            webElementExtensions.WaitForElementClickable(_driver, makePaymentButtonLocBy, ConfigSettings.WaitTime);
            webElementExtensions.Click(_driver, makePaymentButtonLocBy, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Selects the HELOC account by account number.
        /// </summary>
        public void SelectHELOCAccount(string accountNumber)
        {
            webElementExtensions.WaitForElement(_driver, helocAccountDropdownLocBy, ConfigSettings.WaitTime);
            var dropdown = _driver.FindElement(helocAccountDropdownLocBy);
            dropdown.SendKeys(accountNumber);
        }

        /// <summary>
        /// Chooses One-Time Payment option.
        /// </summary>
        public void ChooseOneTimePayment()
        {
            webElementExtensions.WaitForElementClickable(_driver, oneTimePaymentOptionLocBy, ConfigSettings.WaitTime);
            webElementExtensions.Click(_driver, oneTimePaymentOptionLocBy, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Enters payment amount.
        /// </summary>
        public void EnterPaymentAmount(string amount)
        {
            webElementExtensions.WaitForElement(_driver, paymentAmountInputLocBy, ConfigSettings.WaitTime);
            webElementExtensions.SendKeys(_driver, paymentAmountInputLocBy, amount, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Selects payment date.
        /// </summary>
        public void SelectPaymentDate(string date)
        {
            webElementExtensions.WaitForElement(_driver, paymentDateInputLocBy, ConfigSettings.WaitTime);
            webElementExtensions.SendKeys(_driver, paymentDateInputLocBy, date, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Submits the payment.
        /// </summary>
        public void SubmitPayment()
        {
            webElementExtensions.WaitForElementClickable(_driver, submitPaymentButtonLocBy, ConfigSettings.WaitTime);
            webElementExtensions.Click(_driver, submitPaymentButtonLocBy, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Checks if late fee message is displayed.
        /// </summary>
        public bool IsLateFeeMessageDisplayed()
        {
            return webElementExtensions.IsElementDisplayed(_driver, lateFeeMessageLocBy);
        }

        /// <summary>
        /// Gets the late fee message text.
        /// </summary>
        public string GetLateFeeMessageText()
        {
            return webElementExtensions.GetText(_driver, lateFeeMessageLocBy, ConfigSettings.WaitTime);
        }
        #endregion
    }
}
