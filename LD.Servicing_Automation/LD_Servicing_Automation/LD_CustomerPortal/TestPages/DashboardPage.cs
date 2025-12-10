using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using AventStack.ExtentReports;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Customer Portal Dashboard Page
    /// Locators sourced from: Servicing _ loanDepot_Loan Details.html
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

        #region Locators
        // My Loans Dropdown Link (Servicing _ loanDepot_Loan Details.html)
        public By myLoansDropdownLocBy = By.XPath("//*[@id='navbarDropdownMenuLinkMyLoans']");
        // View Loan Button (Servicing _ loanDepot_Loan Details.html)
        public By viewLoanButtonLocBy = By.XPath("//*[@id='btnViewLoan']");
        // Make a Payment Button (Servicing _ loanDepot_Loan Details.html)
        public By makePaymentButtonLocBy = By.XPath("//*[@id='navbarPaymentLink']");
        // Loan Focus Link (Servicing _ loanDepot_Loan Details.html)
        public By loanFocusLinkLocBy = By.XPath("//a[contains(@class,'loan-focus')]");
        // Manage Autopay Button (Servicing _ loanDepot_Loan Details.html)
        public By manageAutopayButtonLocBy = By.XPath("//*[@id='linkManageAutopay']");
        #endregion

        #region Methods
        public void NavigateToLoan(string propertyState)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, $"Navigating to HELOC loan for property state: {propertyState}");
                webElementExtensions.Click(_driver, myLoansDropdownLocBy, ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, loanFocusLinkLocBy, ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, viewLoanButtonLocBy, ConfigSettings.WaitTime);
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Navigation to loan failed: " + ex.Message);
                throw;
            }
        }

        public void ClickMakePayment()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment button on Dashboard Page");
                webElementExtensions.Click(_driver, makePaymentButtonLocBy, ConfigSettings.WaitTime);
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Click Make Payment failed: " + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
