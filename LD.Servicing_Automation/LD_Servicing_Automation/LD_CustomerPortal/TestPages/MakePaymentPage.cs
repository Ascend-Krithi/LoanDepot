using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using System;

namespace LD_Servicing_Automation.LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Make Payment Page (Source: Servicing _ loanDepot_Date Selection for Payment.html)
    /// </summary>
    public class MakePaymentPage
    {
        private IWebDriver _driver;
        private ExtentTest test;
        private WebElementExtensionsPage webElementExtensions;

        public MakePaymentPage(IWebDriver driver, ExtentTest test)
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
            PageFactory.InitElements(driver, this);
        }

        #region Locators (Strict Source Mapping: Servicing _ loanDepot_Date Selection for Payment.html)
        [FindsBy(How = How.XPath, Using = "#paymtDateId")]
        public IWebElement PaymentDateInput;

        [FindsBy(How = How.XPath, Using = "[aria-label='Open calendar']")]
        public IWebElement OpenCalendarButton;

        [FindsBy(How = How.XPath, Using = "#no-thanks-link")]
        public IWebElement NoThanksButton;

        [FindsBy(How = How.XPath, Using = "#btnIdCancel")]
        public IWebElement CancelButton;

        [FindsBy(How = How.XPath, Using = "#footer-link")]
        public IWebElement FooterLink;

        [FindsBy(How = How.XPath, Using = "button.border-0.mx-1.py-2.bg-indigo.btn-continue")]
        public IWebElement ContinueButton;

        [FindsBy(How = How.XPath, Using = "textarea")]
        public IWebElement GenericInput;
        #endregion

        #region Methods
        /// <summary>
        /// Select payment date
        /// </summary>
        public void SelectPaymentDate(string paymentDate)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, $"Selecting payment date: {paymentDate}");
                PaymentDateInput.Clear();
                PaymentDateInput.SendKeys(paymentDate);
                ContinueButton.Click();
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Select payment date failed: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Verify if late fee message is displayed
        /// </summary>
        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                // Assume late fee message is displayed in GenericInput or another element
                return GenericInput.Displayed && GenericInput.Text.Contains("late fee");
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verify if Make a Payment screen is displayed
        /// </summary>
        public bool IsPaymentScreenDisplayed()
        {
            try
            {
                return PaymentDateInput.Displayed && ContinueButton.Displayed;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
