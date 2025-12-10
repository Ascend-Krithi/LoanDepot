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
    /// Locators sourced from: Servicing _ loanDepot_Loan Details.html
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
        [FindsBy(How = How.XPath, Using = "//a[@id='navbarDropdownMenuLinkMyLoans']")]
        public IWebElement MyLoansDropdown;

        [FindsBy(How = How.XPath, Using = "//button[@id='btnViewLoan']")]
        public IWebElement ViewLoanButton;

        [FindsBy(How = How.XPath, Using = "//button[@id='linkManageAutopay']")]
        public IWebElement MakePaymentButton;
        #endregion

        #region Methods
        public void NavigateToLoan(string propertyState)
        {
            try
            {
                test.Log(Status.Info, $"Navigating to HELOC loan for property state: {propertyState}");
                MyLoansDropdown.Click();
                ViewLoanButton.Click();
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Navigation to loan failed: " + ex.Message);
                throw;
            }
        }

        public void ClickMakePayment()
        {
            try
            {
                test.Log(Status.Info, "Clicking 'Make a Payment' button on Dashboard Page.");
                MakePaymentButton.Click();
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Clicking Make Payment failed: " + ex.Message);
                throw;
            }
        }

        public bool IsDashboardDisplayed()
        {
            try
            {
                test.Log(Status.Info, "Verifying Dashboard is displayed.");
                return MyLoansDropdown.Displayed;
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Dashboard display verification failed: " + ex.Message);
                return false;
            }
        }
        #endregion
    }
}