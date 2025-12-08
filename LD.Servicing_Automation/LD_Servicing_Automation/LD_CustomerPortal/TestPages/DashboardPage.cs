using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Customer Portal Dashboard Page
    /// Source HTML: Servicing _ loanDepot.html (no interactive tags found, plausible locators used)
    /// </summary>
    public class DashboardPage : BasePage
    {
        private IWebDriver _driver;
        private ExtentTest test;
        private WebElementExtensionsPage webElementExtensions;

        public DashboardPage(IWebDriver driver, ExtentTest test) : base()
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
            PageFactory.InitElements(driver, this);
        }

        #region Locators
        // HELOC loan account tile (plausible XPath)
        public By helocLoanTile = By.XPath("//div[contains(@class, 'loan-tile') and contains(., 'HELOC')]");
        // Make a Payment button (plausible XPath)
        public By makePaymentButton = By.XPath("//button[contains(normalize-space(text()), 'Make a Payment')]");
        // Make a Payment button enabled state (purple)
        public By makePaymentButtonEnabled = By.XPath("//button[contains(normalize-space(text()), 'Make a Payment') and contains(@class, 'enabled')]");
        #endregion

        #region Methods
        /// <summary>
        /// Navigates to the HELOC loan account for a given property state
        /// </summary>
        public void NavigateToHelocLoan(string propertyState)
        {
            test.Log(AventStack.ExtentReports.Status.Info, $"Navigating to HELOC loan for property state: {propertyState}");
            webElementExtensions.WaitForElement(_driver, helocLoanTile, ConfigSettings.WaitTime);
            webElementExtensions.Click(_driver, helocLoanTile, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Clicks the 'Make a Payment' button
        /// </summary>
        public void ClickMakePayment()
        {
            test.Log(AventStack.ExtentReports.Status.Info, "Clicking 'Make a Payment' button on DashboardPage.");
            webElementExtensions.WaitForElementClickable(_driver, makePaymentButton, ConfigSettings.WaitTime);
            webElementExtensions.Click(_driver, makePaymentButton, ConfigSettings.WaitTime);
        }

        /// <summary>
        /// Verifies if 'Make a Payment' button is enabled (purple)
        /// </summary>
        public bool IsMakePaymentButtonEnabled()
        {
            return webElementExtensions.IsElementEnabled(_driver, makePaymentButtonEnabled);
        }
        #endregion
    }
}
