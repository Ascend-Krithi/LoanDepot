using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using AventStack.ExtentReports;
using System;

// Page Object for Payments functionality
// Locators mapped from Servicing _ loanDepot_Payment Page.html and Servicing _ loanDepot_Date Selection for Payment.html
// All XPath values are sourced from the provided LOCATORS JSON
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
        // Make a Payment button (Servicing _ loanDepot_Payment Page.html)
        public By makeAPaymentBtnBy = By.XPath("//button[contains(text(),'Make a Payment')]"); // Source: Servicing _ loanDepot_Payment Page.html
        // Manage Autopay button (Servicing _ loanDepot_Payment Page.html)
        public By manageAutopayBtnBy = By.XPath("//button[contains(text(),'Manage Autopay')]"); // Source: Servicing _ loanDepot_Payment Page.html
        // Add an Account button (Servicing _ loanDepot_Date Selection for Payment.html)
        public By addAnAccountBtnBy = By.XPath("//button[contains(text(),'Add an Account')]"); // Source: Servicing _ loanDepot_Date Selection for Payment.html
        // Manage Accounts button (Servicing _ loanDepot_Date Selection for Payment.html)
        public By manageAccountsBtnBy = By.XPath("//button[contains(text(),'Manage Accounts')]"); // Source: Servicing _ loanDepot_Date Selection for Payment.html
        // Edit Payment button (Servicing _ loanDepot_Date Selection for Payment.html)
        public By editPaymentBtnBy = By.XPath("//button[contains(text(),'Edit Payment')]"); // Source: Servicing _ loanDepot_Date Selection for Payment.html
        // Confirm Payment button (Servicing _ loanDepot_Date Selection for Payment.html)
        public By confirmPaymentBtnBy = By.XPath("//button[contains(text(),'Confirm Payment')]"); // Source: Servicing _ loanDepot_Date Selection for Payment.html
        // Late Fee Message (Assumed XPath, as not present in elements)
        public By lateFeeMessageBy = By.XPath("//div[contains(@class,'late-fee-message')]"); // Assumed based on element name
        // Payment Date Picker (Assumed XPath)
        public By paymentDatePickerBy = By.XPath("//input[@name='paymentDate']"); // Assumed based on element name
        #endregion

        #region Methods
        public void ClickMakeAPayment()
        {
            webElementExtensions.WaitForElementClickable(_driver, makeAPaymentBtnBy, ConfigSettings.WaitTime);
            webElementExtensions.Click(_driver, makeAPaymentBtnBy, ConfigSettings.WaitTime);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked 'Make a Payment' button.");
        }

        public void SelectPaymentDate(string paymentDate)
        {
            webElementExtensions.WaitForElement(_driver, paymentDatePickerBy, ConfigSettings.WaitTime);
            webElementExtensions.SendKeys(_driver, paymentDatePickerBy, paymentDate, ConfigSettings.WaitTime);
            test.Log(AventStack.ExtentReports.Status.Info, $"Selected payment date: {paymentDate}.");
        }

        public bool IsLateFeeMessageDisplayed()
        {
            bool isDisplayed = webElementExtensions.IsElementDisplayed(_driver, lateFeeMessageBy);
            test.Log(AventStack.ExtentReports.Status.Info, $"Late fee message displayed: {isDisplayed}");
            return isDisplayed;
        }

        public string GetLateFeeMessageText()
        {
            string message = webElementExtensions.GetText(_driver, lateFeeMessageBy, ConfigSettings.WaitTime);
            test.Log(AventStack.ExtentReports.Status.Info, $"Late fee message text: {message}");
            return message;
        }
        #endregion
    }
}
