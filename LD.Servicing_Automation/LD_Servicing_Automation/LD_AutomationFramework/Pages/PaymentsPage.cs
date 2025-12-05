using System;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;
using AventStack.ExtentReports;
using LD_AutomationFramework.Utilities;
using LD_AutomationFramework;
using LD_AutomationFramework.Pages;
using iTextSharp.text;
using System.Collections.Generic;
using System.Linq;

namespace LD_AutomationFramework.Pages
{
    public class PaymentsPage
    {
        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        WebElementExtensionsPage webElementExtensions = null;
        public PaymentsPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
        }

        #region Locators

        public By manageAutopayButtonLocBy = By.Id("btnManageAutopay");

        public By disabledManageAutopayButtonLocBy = By.XPath("//button[@id='btnManageAutopay' and @disabled='true']");

        public By goBackToPaymentsLinkLocBy = By.Id("cancel-close-link");

        public By manageAutopayGridLocBy = By.CssSelector("mat-card[id]");

        public By accountDoesNotHaveAutopayMessageLocBy = By.XPath("//div[text()='Account does not have an Autopay setup']");

        public By setupAutopaybuttonLocBy = By.XPath("//span[contains(text(),'Setup Autopay')]//parent::button");

        public By authorizedByDropdownLocBy = By.XPath("//div[text()='Authorized By']//following-sibling::div//div[contains(@id,'mat-select')]");

        public string authorizedByDropdownValue = "//span[@class='mat-option-text']//span[contains(text(),'<BORROWERNAME>')]";

        public By authorizedByDropdownValueSelectedLocBy = By.CssSelector("mat-select[id='borrowerSelect'] span[class*='mat-select-min-line']");

        public By bankAccountDropdownLocBy = By.XPath("//mat-select[@id='bankAccountSelect']//div[contains(@id,'mat-select')]");

        public string bankAccountDropdownValue = "//span[@class='mat-option-text']//span[contains(text(),'<BANKACCOUNT>')]";

        public By allValuesInbankAccountDropdownLocBy = By.CssSelector("span[class='mat-option-text'] span");

        public By bankAccountDropdownValueSelectedLocBy = By.CssSelector("mat-select[id='bankAccountSelect'] span[class*='mat-select-min-line']");

        public By addAnAccountLinkLocBy = By.Id("add-account-link");

        public By accountNicknameLocBy = By.Id("accountNickNameInput");

        public By savingsRadioButtonLocBy = By.Id("SavingsButton");

        public By firstNameOnAccountTextboxLocBy = By.Id("firstNameInput");

        public By lastNameOnAccountTextboxLocBy = By.Id("lastNameInput");

        public By routingNumberTextboxLocBy = By.Id("routingNumberInput");

        public By accountNumberTextboxLocBy = By.Id("accountNumber");

        public By confirmAccountNumberTextboxLocBy = By.Id("accountNumberConfirm");

        public By addAccountButtonLocBy = By.Id("addAccountFormGroupBtn");

        public By paymentDatePickerIconLocBy = By.CssSelector("button[aria-label='Open calendar']");

        public By paymentDateTextboxWithDateSelectedLocBy = By.Id("paymtDateId");

        public string paymentDateTobeSelected = "td[aria-label='<DATETOBESELECTED>'] div[class*='mat-focus']";

        public By setupAutopayButtonInSetupAutopayScreenLocBy = By.XPath("//button//span[contains(text(),'Setup Autopay')]");

        public By confirmAutopayButtonLocBy = By.XPath("//span[contains(text(),'Confirm Autopay')]");

        public By continueButtonInConfirmAutopayScreenLocBy = By.Id("okBtn");

        #endregion Locators

        #region Services

        /// <summary>
        /// Method to select borrower name in Authorized By dropdown
        /// </summary>
        /// <param name="borrowerName">Borrower name/ Co-Borrower name/ Other</param>
        /// <param name="isReportRequired">True/False</param>
        public void SelectValueInAuthorizedByDropdown(string borrowerName, bool isReportRequired = true)
        {
            bool flag = false;
            By authorizedByDropdownValueLocBy = null;
            try
            {
                webElementExtensions.ActionClick(_driver, authorizedByDropdownLocBy);                
                authorizedByDropdownValueLocBy = By.XPath(authorizedByDropdownValue.Replace("<BORROWERNAME>", borrowerName.ToUpper()));
                webElementExtensions.ActionClick(_driver, authorizedByDropdownValueLocBy);
                webElementExtensions.WaitForElement(_driver, authorizedByDropdownValueSelectedLocBy);
                if (_driver.FindElement(authorizedByDropdownValueSelectedLocBy).Text == borrowerName.ToUpper())
                    flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while selecting borrower name in Authorized By dropdown: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully selected borrower name - "+ borrowerName +" in Authorized By dropdown.", "Failed while selecting borrower name - "+ borrowerName +" in Authorized By dropdown.");
        }

        /// <summary>
        /// Method to select bank account in Method dropdown
        /// </summary>
        /// <param name="bankAccountName">Bank account details for payment</param>
        /// <param name="isReportRequired">True/False</param>
        public bool SelectValueInMethodDropdown(string bankAccountName, bool isReportRequired = true)
        {
            bool flag = false;
            By bankAccountDropdownValueLocBy = null;
            List<IWebElement> allWebElementsInMethodDropdown = null;
            try
            {
                webElementExtensions.ActionClick(_driver, bankAccountDropdownLocBy);
                bankAccountDropdownValueLocBy = By.XPath(bankAccountDropdownValue.Replace("<BANKACCOUNT>", bankAccountName));
                allWebElementsInMethodDropdown = _driver.FindElements(allValuesInbankAccountDropdownLocBy).ToList();
                foreach (IWebElement element in allWebElementsInMethodDropdown)
                {                    
                    if (element.Text.Equals(bankAccountName))
                    {
                        webElementExtensions.ActionClick(_driver, bankAccountDropdownValueLocBy);
                        webElementExtensions.WaitForElement(_driver, bankAccountDropdownValueSelectedLocBy);
                        if (_driver.FindElement(bankAccountDropdownValueSelectedLocBy).Text == bankAccountName)
                            flag = true;
                        break;
                    }                     
                }
                if(!flag)
                    webElementExtensions.ActionClick(_driver, bankAccountDropdownLocBy);

            }
            catch (Exception ex)
            {
                log.Error("Failed while selecting bank account in Method dropdown: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully selected bank account - " + bankAccountName + " in Method dropdown.", "Failed while selecting bank account - " + bankAccountName + " in Method dropdown.");
            return flag;
        }        

        /// <summary>
        /// Method to select the payment date
        /// </summary>
        /// <param name="date">Date value in the following format - June 15, 2024</param>
        /// <param name="isReportRequired">true/false</param>
        public void SelectPaymentDateInDateField(string date, bool isReportRequired = true)
        {
            bool flag = false;
            By paymentDateToBeSelectedLocBy = null;
            try
            {
                webElementExtensions.ActionClick(_driver, paymentDatePickerIconLocBy);
                paymentDateToBeSelectedLocBy = By.CssSelector(paymentDateTobeSelected.Replace("<DATETOBESELECTED>", date));
                webElementExtensions.ClickElement(_driver, paymentDateToBeSelectedLocBy);
                webElementExtensions.WaitForElement(_driver, paymentDatePickerIconLocBy);
                if (!_driver.FindElement(paymentDateTextboxWithDateSelectedLocBy).GetAttribute("class").Contains("untouched"))
                    flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while selecting payment date in Date field: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully selected payment date - " + date + " in Date field.", "Failed while selecting payment date - " + date + " in Date field.");
        }

        #endregion Services
    }
}
