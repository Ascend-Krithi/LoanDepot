using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Make Payment Page
    /// Locators sourced from: Servicing _ loanDepot_Payment Page.html and Servicing _ loanDepot_Date Selection for Payment.html
    /// </summary>
    public class MakePaymentPage : BasePage
    {
        private IWebDriver _driver;
        private ExtentTest test;
        private WebElementExtensionsPage webElementExtensions;

        public MakePaymentPage(IWebDriver driver, ExtentTest test) : base()
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
            PageFactory.InitElements(driver, this);
        }

        #region Locators
        [FindsBy(How = How.XPath, Using = "//input[@id='paymtDateId']")]
        public IWebElement PaymentDateInput;

        [FindsBy(How = How.XPath, Using = "//button[@aria-label='Open calendar']")]
        public IWebElement OpenCalendarButton;

        [FindsBy(How = How.XPath, Using = "//div[contains(@class,'late-fee-message')]" )]
        public IWebElement LateFeeMessage;

        [FindsBy(How = How.XPath, Using = "//button[@id='btnIdCancel']")]
        public IWebElement CancelButton;
        #endregion

        #region Methods
        public void SelectPaymentDate(string paymentDate)
        {
            try
            {
                test.Log(Status.Info, $"Selecting payment date: {paymentDate}");
                OpenCalendarButton.Click();
                PaymentDateInput.Clear();
                PaymentDateInput.SendKeys(paymentDate);
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Selecting payment date failed: " + ex.Message);
                throw;
            }
        }

        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                test.Log(Status.Info, "Checking for late fee message.");
                return LateFeeMessage.Displayed;
            }
            catch (Exception ex)
            {
                test.Log(Status.Info, "Late fee message not displayed: " + ex.Message);
                return false;
            }
        }

        public bool IsPaymentScreenDisplayed()
        {
            try
            {
                test.Log(Status.Info, "Verifying Make Payment screen is displayed.");
                return PaymentDateInput.Displayed;
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Payment screen display verification failed: " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}