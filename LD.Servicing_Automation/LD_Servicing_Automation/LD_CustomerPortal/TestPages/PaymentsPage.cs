using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Customer Portal Payments Page
    /// Source HTML: Servicing _ loanDepot_payment.html (no interactive tags found, plausible locators used)
    /// </summary>
    public class PaymentsPage : BasePage
    {
        private IWebDriver _driver;
        private ExtentTest test;
        private WebElementExtensionsPage webElementExtensions;

        public PaymentsPage(IWebDriver driver, ExtentTest test) : base()
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
            PageFactory.InitElements(driver, this);
        }

        #region Locators
        // Payment date picker (plausible XPath)
        public By paymentDatePicker = By.XPath("//input[@type='date' or contains(@aria-label, 'Payment Date') or contains(@placeholder, 'Payment Date')]");
        // Submit payment button (plausible XPath)
        public By submitPaymentButton = By.XPath("//button[contains(normalize-space(text()), 'Submit Payment')]");
        // Late fee message (plausible XPath)
        public By lateFeeMessage = By.XPath("//div[contains(@class, 'late-fee-message') or contains(text(), 'late fee')]");
        // No late fee message (plausible XPath)
        public By noLateFeeMessage = By.XPath("//div[contains(@class, 'no-late-fee-message') or contains(text(), 'No late fee')]");
        #endregion

        #region Methods
        /// <summary>
        /// Selects the payment date
        /// </summary>
        public void SelectPaymentDate(string paymentDate)
        {
            test.Log(AventStack.ExtentReports.Status.Info, $"Selecting payment date: {paymentDate}");
            webElementExtensions.SendKeys(_driver, paymentDatePicker, paymentDate, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Submits the payment
        /// </summary>
        public void SubmitPayment()
        {
            test.Log(AventStack.ExtentReports.Status.Info, "Submitting payment on PaymentsPage.");
            webElementExtensions.Click(_driver, submitPaymentButton, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Verifies if late fee message is displayed
        /// </summary>
        public bool IsLateFeeMessageDisplayed()
        {
            return webElementExtensions.IsElementDisplayed(_driver, lateFeeMessage);
        }

        /// <summary>
        /// Verifies if no late fee message is displayed
        /// </summary>
        public bool IsNoLateFeeMessageDisplayed()
        {
            return webElementExtensions.IsElementDisplayed(_driver, noLateFeeMessage);
        }
        #endregion
    }
}
