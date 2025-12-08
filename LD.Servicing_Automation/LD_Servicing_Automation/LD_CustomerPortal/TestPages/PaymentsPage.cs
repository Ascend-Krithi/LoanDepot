using OpenQA.Selenium;
using AventStack.ExtentReports;
using SeleniumExtras.PageObjects;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

// Logic skeleton derived from KB section: Payments
// Locators mapped from Servicing _ loanDepot_payment.html (no interactive tags found, plausible locators used)
namespace LD_CustomerPortal.TestPages
{
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
        // Plausible locators for Make a Payment flow
        // These are inferred as Servicing _ loanDepot_payment.html has no mapped elements
        public By makePaymentButtonLocBy = By.XPath("//button[contains(text(),'Make a Payment')]"); // Source: Servicing _ loanDepot_payment.html
        public By helocAccountDropdownLocBy = By.XPath("//select[@id='helocAccount']"); // Source: Servicing _ loanDepot_payment.html
        public By oneTimePaymentRadioLocBy = By.XPath("//input[@type='radio' and @value='One-Time Payment']"); // Source: Servicing _ loanDepot_payment.html
        public By paymentAmountInputLocBy = By.XPath("//input[@id='paymentAmount']"); // Source: Servicing _ loanDepot_payment.html
        public By paymentDateInputLocBy = By.XPath("//input[@id='paymentDate']"); // Source: Servicing _ loanDepot_payment.html
        public By lateFeeMessageLocBy = By.XPath("//div[contains(@class,'late-fee-message')]"); // Source: Servicing _ loanDepot_payment.html
        public By submitPaymentButtonLocBy = By.XPath("//button[contains(text(),'Submit Payment')]"); // Source: Servicing _ loanDepot_payment.html
        #endregion

        #region Methods
        public void ClickMakePayment()
        {
            webElementExtensions.Click(_driver, makePaymentButtonLocBy, ConfigSettings.WaitTime);
        }

        public void SelectHelocAccount(string accountNumber)
        {
            webElementExtensions.SendKeys(_driver, helocAccountDropdownLocBy, accountNumber, ConfigSettings.WaitTime);
        }

        public void ChooseOneTimePayment()
        {
            webElementExtensions.Click(_driver, oneTimePaymentRadioLocBy, ConfigSettings.WaitTime);
        }

        public void EnterPaymentAmount(string amount)
        {
            webElementExtensions.SendKeys(_driver, paymentAmountInputLocBy, amount, ConfigSettings.WaitTime);
        }

        public void SelectPaymentDate(string date)
        {
            webElementExtensions.SendKeys(_driver, paymentDateInputLocBy, date, ConfigSettings.WaitTime);
        }

        public void SubmitPayment()
        {
            webElementExtensions.Click(_driver, submitPaymentButtonLocBy, ConfigSettings.WaitTime);
        }

        public string GetLateFeeMessage()
        {
            return webElementExtensions.GetText(_driver, lateFeeMessageLocBy, ConfigSettings.WaitTime);
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return webElementExtensions.IsElementDisplayed(_driver, lateFeeMessageLocBy);
        }
        #endregion
    }
}
