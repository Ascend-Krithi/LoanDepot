using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using AventStack.ExtentReports;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Customer Portal Payments Page
    /// Locators sourced from: Servicing _ loanDepot_Payment Page.html, Servicing _ loanDepot_Date Selection for Payment.html
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
        // Payment Date Input (Servicing _ loanDepot_Date Selection for Payment.html)
        public By paymentDateInputLocBy = By.XPath("//*[@id='paymtDateId']");
        // Open Calendar Button (Servicing _ loanDepot_Date Selection for Payment.html)
        public By openCalendarButtonLocBy = By.XPath("//*[@aria-label='Open calendar']");
        // Continue Button (Servicing _ loanDepot_Payment Page.html)
        public By continueButtonLocBy = By.XPath("//button[contains(@class,'btn-continue')]");
        // Late Fee Message (Assumed XPath for message display)
        public By lateFeeMessageLocBy = By.XPath("//div[contains(@class,'late-fee-message')]");
        // No Late Fee Message (Assumed XPath for message display)
        public By noLateFeeMessageLocBy = By.XPath("//div[contains(@class,'no-late-fee-message')]");
        #endregion

        #region Methods
        public void SelectPaymentDate(string paymentDate)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, $"Selecting payment date: {paymentDate}");
                webElementExtensions.Click(_driver, openCalendarButtonLocBy, ConfigSettings.WaitTime);
                webElementExtensions.SendKeys(_driver, paymentDateInputLocBy, paymentDate, ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, continueButtonLocBy, ConfigSettings.WaitTime);
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Selecting payment date failed: " + ex.Message);
                throw;
            }
        }

        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                return webElementExtensions.IsElementDisplayed(_driver, lateFeeMessageLocBy);
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Warning, "Late fee message not found: " + ex.Message);
                return false;
            }
        }

        public bool IsNoLateFeeMessageDisplayed()
        {
            try
            {
                return webElementExtensions.IsElementDisplayed(_driver, noLateFeeMessageLocBy);
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Warning, "No late fee message not found: " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}
