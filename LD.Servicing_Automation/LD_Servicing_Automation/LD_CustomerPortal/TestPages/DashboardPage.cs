using OpenQA.Selenium;
using AventStack.ExtentReports;
using SeleniumExtras.PageObjects;
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
            PageFactory.InitElements(driver, this);
        }

        #region Locators
        // HELOC loan account tile (plausible XPath, Servicing _ loanDepot.html)
        public By helocLoanTileLocBy = By.XPath("//div[contains(@class, 'loan-tile') and contains(., 'HELOC')]");
        // Make a Payment button (plausible XPath, Servicing _ loanDepot.html)
        public By makePaymentButtonLocBy = By.XPath("//button[contains(normalize-space(text()), 'Make a Payment')]");
        // Dashboard loaded indicator (plausible XPath)
        public By dashboardLoadedLocBy = By.XPath("//h1[contains(text(), 'Dashboard')]");
        #endregion

        #region Methods
        public void NavigateToHELOCLoan(string propertyState)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, $"Navigating to HELOC loan for property state: {propertyState}");
                webElementExtensions.WaitForElement(_driver, helocLoanTileLocBy, ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, helocLoanTileLocBy, ConfigSettings.WaitTime);
            }
            catch (NoSuchElementException ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "HELOC loan tile not found: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Navigation to HELOC loan failed: " + ex.Message);
                throw;
            }
        }

        public void ClickMakePayment()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment button on Dashboard");
                webElementExtensions.WaitForElementClickable(_driver, makePaymentButtonLocBy, ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, makePaymentButtonLocBy, ConfigSettings.WaitTime);
            }
            catch (NoSuchElementException ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Make a Payment button not found: " + ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Click Make a Payment failed: " + ex.Message);
                throw;
            }
        }

        public bool IsDashboardDisplayed()
        {
            try
            {
                return webElementExtensions.IsElementDisplayed(_driver, dashboardLoadedLocBy);
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Warning, "Dashboard display check failed: " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}
