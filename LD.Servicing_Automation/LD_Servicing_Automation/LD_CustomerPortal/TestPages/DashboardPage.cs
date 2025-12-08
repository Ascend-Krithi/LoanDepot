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
    public class DashboardPage : BasePage
    {
        public IWebDriver _driver { get; set; }
        public ExtentTest test { get; set; }
        public WebElementExtensionsPage webElementExtensions;

        public DashboardPage(IWebDriver driver, ExtentTest test) : base()
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
        }

        #region Locators
        // HELOC loan account tile (plausible XPath)
        public By helocLoanTileLocBy = By.XPath("//div[contains(@class, 'loan-tile') and contains(., 'HELOC')]"); // Source: Servicing _ loanDepot.html
        // Property state label (plausible XPath)
        public By propertyStateLabelLocBy = By.XPath("//span[contains(@class, 'property-state')]"); // Source: Servicing _ loanDepot.html
        // Make a Payment button (plausible XPath)
        public By makePaymentButtonLocBy = By.XPath("//button[contains(normalize-space(text()), 'Make a Payment')]"); // Source: Servicing _ loanDepot.html
        #endregion

        #region Methods
        public void NavigateToHelocLoan(string propertyState)
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, $"Navigating to HELOC loan for property state: {propertyState}");
                webElementExtensions.WaitForElement(_driver, helocLoanTileLocBy, ConfigSettings.WaitTime);
                webElementExtensions.Click(_driver, helocLoanTileLocBy, ConfigSettings.WaitTime);
                string state = webElementExtensions.GetText(_driver, propertyStateLabelLocBy, ConfigSettings.WaitTime);
                Assert.AreEqual(propertyState, state, $"Property state should be {propertyState}");
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
                test.Log(AventStack.ExtentReports.Status.Info, "Clicking 'Make a Payment' button on Dashboard");
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
                test.Log(AventStack.ExtentReports.Status.Fail, "Clicking Make a Payment failed: " + ex.Message);
                throw;
            }
        }

        public bool IsDashboardDisplayed()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Verifying dashboard is displayed");
                return webElementExtensions.IsElementDisplayed(_driver, helocLoanTileLocBy);
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
