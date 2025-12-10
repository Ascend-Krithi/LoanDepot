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
    /// Locators sourced from Servicing _ loanDepot_Popup1.json
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
            PageFactory.InitElements(driver, this);
        }

        #region Locators
        public By paymentDateInputLocBy = By.XPath("//input[@id='start-date' and @type='date']"); // Servicing _ loanDepot_Popup1.json
        public By saveButtonLocBy = By.XPath("//button[@id='save-loan' and contains(@class,'primary-btn')]"); // Servicing _ loanDepot_Popup1.json
        public By lateFeeMessageLocBy = By.XPath("//div[contains(@class,'late-fee-message')]"); // Assumed based on late fee message context
        #endregion

        #region Methods
        /// <summary>
        /// Selects the payment date
        /// </summary>
        public void SelectPaymentDate(string paymentDate)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, $"Selecting payment date: {paymentDate}");
                webElementExtensions.WaitForElement(_driver, paymentDateInputLocBy, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.SendKeys(_driver, paymentDateInputLocBy, paymentDate, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                test.Log(AventStack.ExtentReports.Status.Pass, "Payment date selected");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Failed to select payment date: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Clicks the Save button to submit payment
        /// </summary>
        public void SubmitPayment()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Submitting payment");
                webElementExtensions.WaitForElementClickable(_driver, saveButtonLocBy, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, saveButtonLocBy, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                test.Log(AventStack.ExtentReports.Status.Pass, "Payment submitted");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Failed to submit payment: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Checks if late fee message is displayed
        /// </summary>
        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Checking for late fee message");
                bool isDisplayed = webElementExtensions.IsElementDisplayed(_driver, lateFeeMessageLocBy);
                test.Log(AventStack.ExtentReports.Status.Info, $"Late fee message displayed: {isDisplayed}");
                return isDisplayed;
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Warning, "Error checking late fee message: " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}
