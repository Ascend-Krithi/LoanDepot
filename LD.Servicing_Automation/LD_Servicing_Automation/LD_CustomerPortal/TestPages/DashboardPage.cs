using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using LD_AutomationFramework.Pages;
using AventStack.ExtentReports;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// DashboardPage POM for Customer Portal dashboard functionalities.
    /// No interactive tags found in Servicing _ loanDepot.html, so plausible locators are used.
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
        // Plausible locators for dashboard elements
        public By helocAccountCardBy = By.XPath("//div[contains(@class, 'account-card') and contains(text(), 'HELOC')]"); // Source: Servicing _ loanDepot.html
        public By makePaymentButtonBy = By.XPath("//button[contains(normalize-space(text()), 'Make a Payment')]"); // Source: Servicing _ loanDepot.html
        public By dashboardTitleBy = By.XPath("//h1[contains(text(), 'Dashboard')]"); // Source: Servicing _ loanDepot.html
        #endregion

        #region Methods
        public void SelectHELOCAccount(string accountNumber)
        {
            // Select the HELOC account card by account number
            By accountCardBy = By.XPath($"//div[contains(@class, 'account-card') and contains(text(), '{accountNumber}')]");
            webElementExtensions.Click(_driver, accountCardBy, 20);
            test.Log(AventStack.ExtentReports.Status.Info, $"Selected HELOC account {accountNumber}.");
        }
        public void ClickMakePayment()
        {
            webElementExtensions.Click(_driver, makePaymentButtonBy, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked Make a Payment button.");
        }
        public bool IsDashboardDisplayed()
        {
            return webElementExtensions.IsElementDisplayed(_driver, dashboardTitleBy);
        }
        public bool IsHELOCAccountVisible(string accountNumber)
        {
            By accountCardBy = By.XPath($"//div[contains(@class, 'account-card') and contains(text(), '{accountNumber}')]");
            return webElementExtensions.IsElementDisplayed(_driver, accountCardBy);
        }
        #endregion
    }
}
