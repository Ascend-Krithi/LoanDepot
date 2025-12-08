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
    /// Locators derived from Servicing _ loanDepot_payment.html (element_count: 0, plausible XPath used)
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
        // Payment date input (plausible XPath, Servicing _ loanDepot_payment.html)
        public By paymentDateInput = By.XPath("//input[@type='date' or contains(@aria-label, 'Payment Date') or contains(@placeholder, 'Payment Date')]");
        // Submit Payment button (plausible XPath, Servicing _ loanDepot_payment.html)
        public By submitPaymentButton = By.XPath("//button[contains(normalize-space(text()), 'Submit Payment')]");
        // Late fee message (plausible XPath, Servicing _ loanDepot_payment.html)
        public By lateFeeMessage = By.XPath("//div[contains(@class, 'late-fee-message') or contains(text(), 'late fee')]");
        #endregion

        #region Methods
        public void SelectPaymentDate(string paymentDate)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, $"Selecting payment date: {paymentDate}.");
                webElementExtensions.WaitForElement(_driver, paymentDateInput, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.SendKeys(_driver, paymentDateInput, paymentDate, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
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
                test.Log(AventStack.ExtentReports.Status.Info, "Submitting payment.");
                webElementExtensions.WaitForElementClickable(_driver, submitPaymentButton, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, submitPaymentButton, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
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
                test.Log(AventStack.ExtentReports.Status.Info, "Checking for late fee message.");
                return webElementExtensions.IsElementDisplayed(_driver, lateFeeMessage);
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Late fee message check failed: " + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
