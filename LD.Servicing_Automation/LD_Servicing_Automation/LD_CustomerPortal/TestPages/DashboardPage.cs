using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using System;

namespace LD_Servicing_Automation.LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Dashboard Page (Source: Servicing _ loanDepot_Popup 2.html, Servicing _ loanDepot_Popup 1.html, Servicing _ loanDepot_Date Selection for Payment.html)
    /// </summary>
    public class DashboardPage
    {
        private IWebDriver _driver;
        private ExtentTest test;
        private WebElementExtensionsPage webElementExtensions;

        public DashboardPage(IWebDriver driver, ExtentTest test)
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
            PageFactory.InitElements(driver, this);
        }

        #region Locators (Strict Source Mapping)
        // From Servicing _ loanDepot_Popup 2.html
        [FindsBy(How = How.XPath, Using = "#navbarDropdownMenuLinkMyLoans")]
        public IWebElement MyLoansDropdownLink;

        [FindsBy(How = How.XPath, Using = "#btnViewLoan")]
        public IWebElement ViewLoanButton;

        [FindsBy(How = How.XPath, Using = "#linkManageAutopay")]
        public IWebElement ManageAutopayButton;

        [FindsBy(How = How.XPath, Using = "#navbarPaymentLink")]
        public IWebElement PaymentLink;

        [FindsBy(How = How.XPath, Using = "#navbarDropdownMenuLink")]
        public IWebElement DropdownMenuLink;

        [FindsBy(How = How.XPath, Using = "#pendingAction")]
        public IWebElement PendingActionLink;

        [FindsBy(How = How.XPath, Using = "#pendingdelete")]
        public IWebElement PendingDeleteLink;

        [FindsBy(How = How.XPath, Using = "#footer-link")]
        public IWebElement FooterLink;

        // From Servicing _ loanDepot_Date Selection for Payment.html
        [FindsBy(How = How.XPath, Using = "#mat-select-0")]
        public IWebElement ItemsPerPageDropdown;

        [FindsBy(How = How.XPath, Using = "[aria-label='Previous page']")]
        public IWebElement PreviousPageButton;

        [FindsBy(How = How.XPath, Using = "[aria-label='Next page']")]
        public IWebElement NextPageButton;
        #endregion

        #region Methods
        /// <summary>
        /// Navigate to HELOC loan account for a given property state
        /// </summary>
        public void NavigateToLoan(string propertyState)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, $"Navigating to HELOC loan for property state: {propertyState}");
                MyLoansDropdownLink.Click();
                // Assume logic to select loan by property state (would require additional locator/method if not present)
                ViewLoanButton.Click();
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Navigation to loan failed: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Click 'Make a Payment' button
        /// </summary>
        public void ClickMakePayment()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment button");
                PaymentLink.Click();
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Click Make a Payment failed: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Verify Dashboard is displayed
        /// </summary>
        public bool IsDashboardDisplayed()
        {
            try
            {
                return MyLoansDropdownLink.Displayed && ViewLoanButton.Displayed;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
