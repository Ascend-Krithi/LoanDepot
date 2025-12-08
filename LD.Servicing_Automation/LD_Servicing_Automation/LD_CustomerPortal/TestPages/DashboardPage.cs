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
    /// Locators derived from Servicing _ loanDepot.html (element_count: 0, plausible XPath used)
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
        // HELOC loan account tile (plausible XPath, Servicing _ loanDepot.html)
        public By helocLoanTile = By.XPath("//div[contains(@class, 'loan-tile') and contains(., 'HELOC')]");
        // Make a Payment button (plausible XPath, Servicing _ loanDepot.html)
        public By makePaymentButton = By.XPath("//button[contains(normalize-space(text()), 'Make a Payment')]");
        #endregion

        #region Methods
        public void NavigateToHELOC(string propertyState)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, $"Navigating to HELOC loan account for property state: {propertyState}.");
                webElementExtensions.WaitForElement(_driver, helocLoanTile, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, helocLoanTile, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Navigation to HELOC failed: " + ex.Message);
                throw;
            }
        }

        public void ClickMakePayment()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Clicking 'Make a Payment' button on Dashboard.");
                webElementExtensions.WaitForElementClickable(_driver, makePaymentButton, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, makePaymentButton, LD_AutomationFramework.Config.ConfigSettings.WaitTime);
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Clicking 'Make a Payment' failed: " + ex.Message);
                throw;
            }
        }

        public bool IsDashboardDisplayed()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Verifying Dashboard is displayed.");
                return webElementExtensions.IsElementDisplayed(_driver, helocLoanTile);
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Dashboard display verification failed: " + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
