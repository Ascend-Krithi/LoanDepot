using OpenQA.Selenium;
using AventStack.ExtentReports;
using SeleniumExtras.PageObjects;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Customer Portal Payments Page
    /// Locators derived from Servicing _ loanDepot_payment.html (element_count: 0, plausible XPath used)
    /// </summary>
    public class PaymentsPage : BasePage
    {
        public IWebDriver _driver { get; set; }
        public ExtentTest test { get; set; }
        public WebElementExtensionsPage webElementExtensions;

        public PaymentsPage(IWebDriver driver, ExtentTest test) : base()
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
        }

        #region Locators
        // Payment date input (plausible XPath)
        public By paymentDateInputLocBy = By.XPath("//input[@type='date' or contains(@aria-label, 'Payment Date') or contains(@placeholder, 'Payment Date')]"); // Source: Servicing _ loanDepot_payment.html
        // Submit payment button (plausible XPath)
        public By submitPaymentButtonLocBy = By.XPath("//button[contains(normalize-space(text()), 'Submit Payment')]"); // Source: Servicing _ loanDepot_payment.html
        // Late fee message (plausible XPath)
        public By lateFeeMessageLocBy = By.XPath("//div[contains(@class, 'late-fee-message') or contains(text(), 'late fee')]"); // Source: Servicing _ loanDepot_payment.html
        #endregion

        #region Methods
        public void SelectPaymentDate(string paymentDate)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, $"Selecting payment date: {paymentDate}");
                webElementExtensions.SendKeys(_driver, paymentDateInputLocBy, paymentDate, ConfigSettings.WaitTime);
            }
            catch (NoSuchElementException ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Payment date input not found: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Selecting payment date failed: " + ex.Message);
                throw;
            }
        }

        public void SubmitPayment()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Submitting payment");
                webElementExtensions.Click(_driver, submitPaymentButtonLocBy, ConfigSettings.WaitTime);
            }
            catch (NoSuchElementException ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Submit Payment button not found: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Submitting payment failed: " + ex.Message);
                throw;
            }
        }

        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Checking for late fee message");
                return webElementExtensions.IsElementDisplayed(_driver, lateFeeMessageLocBy);
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Late fee message verification failed: " + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
