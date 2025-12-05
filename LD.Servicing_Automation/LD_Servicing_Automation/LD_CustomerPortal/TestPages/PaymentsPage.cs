using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using LD_AutomationFramework.Pages;
using AventStack.ExtentReports;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// PaymentsPage POM for Make a Payment functionalities.
    /// No interactive tags found in Servicing _ loanDepot_payment.html, so plausible locators are used.
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
        // Plausible locators for payment elements
        public By oneTimePaymentButtonBy = By.XPath("//button[contains(normalize-space(text()), 'One-Time Payment')]"); // Source: Servicing _ loanDepot_payment.html
        public By paymentAmountInputBy = By.XPath("//input[@type='number' or contains(@aria-label, 'Payment Amount') or contains(@placeholder, 'Amount')]"); // Source: Servicing _ loanDepot_payment.html
        public By paymentDateInputBy = By.XPath("//input[@type='date' or contains(@aria-label, 'Payment Date') or contains(@placeholder, 'Date')]"); // Source: Servicing _ loanDepot_payment.html
        public By submitPaymentButtonBy = By.XPath("//button[contains(normalize-space(text()), 'Submit Payment')]"); // Source: Servicing _ loanDepot_payment.html
        public By lateFeeMessageBy = By.XPath("//div[contains(@class, 'late-fee-message')]"); // Source: Servicing _ loanDepot_payment.html
        #endregion

        #region Methods
        public void ClickOneTimePayment()
        {
            webElementExtensions.Click(_driver, oneTimePaymentButtonBy, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked One-Time Payment button.");
        }
        public void EnterPaymentAmount(string amount)
        {
            webElementExtensions.SendKeys(_driver, paymentAmountInputBy, amount, 20);
            test.Log(AventStack.ExtentReports.Status.Info, $"Entered payment amount: {amount}.");
        }
        public void EnterPaymentDate(string date)
        {
            webElementExtensions.SendKeys(_driver, paymentDateInputBy, date, 20);
            test.Log(AventStack.ExtentReports.Status.Info, $"Entered payment date: {date}.");
        }
        public void SubmitPayment()
        {
            webElementExtensions.Click(_driver, submitPaymentButtonBy, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked Submit Payment button.");
        }
        public bool IsLateFeeMessageDisplayed()
        {
            return webElementExtensions.IsElementDisplayed(_driver, lateFeeMessageBy);
        }
        public string GetLateFeeMessage()
        {
            return webElementExtensions.GetText(_driver, lateFeeMessageBy, 20);
        }
        #endregion
    }
}
