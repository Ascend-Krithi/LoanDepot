using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Dashboard Page (sources: Servicing _ loanDepot_Popup 2.html, Servicing _ loanDepot_Popup 1.html, Servicing _ loanDepot_Date Selection for Payment.html)
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
        }

        #region Locators
        // Source mapping: Servicing _ loanDepot_Popup 2.html, Servicing _ loanDepot_Popup 1.html, Servicing _ loanDepot_Date Selection for Payment.html
        public By pageReadyLocBy = By.CssSelector("div.loan-selection-main, div.dashboard-container"); // Dashboard.PageReady
        public By loanDropdownLocBy = By.CssSelector("button.loan-selector"); // Dashboard.LoanDropdown
        public By loanAccountRowLocBy(string loanName) => By.XPath($"//li[normalize-space(text())='{loanName}']"); // Dashboard.LoanAccountRow
        public By makePaymentLocBy = By.XPath("//button[contains(normalize-space(.),'Make a Payment')]"); // Dashboard.MakePayment
        #endregion

        #region Methods
        public void WaitForDashboard()
        {
            webElementExtensions.WaitForElement(_driver, pageReadyLocBy, ConfigSettings.WaitTime);
            test.Log(AventStack.ExtentReports.Status.Info, "Dashboard page loaded (loan-selection-main/dashboard-container present)");
        }

        public void SelectLoanAccount(string loanName)
        {
            webElementExtensions.Click(_driver, loanDropdownLocBy, ConfigSettings.SmallWaitTime);
            webElementExtensions.Click(_driver, loanAccountRowLocBy(loanName), ConfigSettings.SmallWaitTime);
            test.Log(AventStack.ExtentReports.Status.Info, $"Selected loan account: {loanName}");
        }

        public void ClickMakePayment()
        {
            webElementExtensions.Click(_driver, makePaymentLocBy, ConfigSettings.SmallWaitTime);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked Make a Payment button");
        }

        public bool IsDashboardDisplayed()
        {
            return webElementExtensions.IsElementDisplayed(_driver, pageReadyLocBy);
        }
        #endregion
    }
}
