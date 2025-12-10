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
        // Locator for HELOC loan account selection (assumed XPath based on context)
        public By helocLoanAccountLocBy = By.XPath("//div[contains(@class,'loan-account') and .//span[contains(text(),'HELOC')]]");
        // Locator for Make a Payment button (assumed XPath based on context)
        public By makePaymentButtonLocBy = By.XPath("//button[contains(text(),'Make a Payment')]");
        #endregion

        #region Methods
        /// <summary>
        /// Navigates to the HELOC loan account for the specified property state
        /// </summary>
        public void NavigateToHelocLoanAccount(string propertyState)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, $"Navigating to HELOC loan account for property state: {propertyState}");
                webElementExtensions.WaitForElement(_driver, helocLoanAccountLocBy, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, helocLoanAccountLocBy, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                test.Log(AventStack.ExtentReports.Status.Pass, "HELOC loan account selected");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Failed to navigate to HELOC loan account: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Clicks the 'Make a Payment' button
        /// </summary>
        public void ClickMakePayment()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Clicking 'Make a Payment' button");
                webElementExtensions.WaitForElementClickable(_driver, makePaymentButtonLocBy, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, makePaymentButtonLocBy, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                test.Log(AventStack.ExtentReports.Status.Pass, "'Make a Payment' button clicked");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Failed to click 'Make a Payment' button: " + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
