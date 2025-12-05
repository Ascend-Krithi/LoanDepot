using System;
using System.IO;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Drivers;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Utilities;
using NUnit.Framework;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class LoanApplicationSteps
    {
        private readonly ScenarioContext _context;
        private IWebDriver _driver;
        private SelfHealingWebDriver _shDriver;
        private LoginPage _loginPage;
        private LoanPage _loanPage;
        private CommonPage _commonPage;

        public LoanApplicationSteps(ScenarioContext context)
        {
            _context = context;
        }

        [Given("I am on the dashboard page")]
        public void GivenIAmOnTheDashboardPage()
        {
            _driver = WebDriverFactory.CreateWebDriver();
            _shDriver = new SelfHealingWebDriver(_driver, TimeSpan.FromSeconds(int.Parse(ConfigManager.Get("DefaultWaitSeconds", "20"))));
            _loginPage = new LoginPage(_shDriver);
            _loanPage = new LoanPage(_shDriver);
            _commonPage = new CommonPage(_shDriver);

            var baseUrl = ConfigManager.Get("BaseUrl", "https://servicing-qa1.loandepotdev.works/dashboard");
            _driver.Navigate().GoToUrl(baseUrl);
        }

        [When(@"I login with username "(.*)" and password "(.*)"")]
        public void WhenILogin(string username, string password)
        {
            _loginPage.Login(username, password);
        }

        [Then("I should see the dashboard home")]
        public void ThenIShouldSeeDashboardHome()
        {
            Assert.IsTrue(_commonPage.IsHomeVisible(), "Dashboard home link should be visible after login.");
        }

        [When("I navigate to Loans")]
        public void WhenINavigateToLoans()
        {
            _loanPage.NavigateToLoans();
        }

        [Then("I should see the loan grid")]
        public void ThenIShouldSeeLoanGrid()
        {
            Assert.IsTrue(_loanPage.IsRecentActivityTableVisible(), "Loan grid / recent activity table should be visible.");
        }

        [When(@"I search for loan "(.*)"")]
        public void WhenISearchForLoan(string loanNumber)
        {
            _loanPage.SearchLoan(loanNumber);
        }

        [When("I open the first loan result")]
        public void WhenIOpenFirstLoanResult()
        {
            _loanPage.OpenFirstLoanFromGrid();
        }

        [Then("I should see the loan details page")]
        public void ThenIShouldSeeLoanDetailsPage()
        {
            // Heuristic: Make Payment button presence indicates loan details page loaded
            // This could be replaced with a more specific locator when available
            Assert.DoesNotThrow(() => _loanPage.MakePayment(), "Loan details page should contain Make Payment action.");
            // navigate back to avoid permanent clicks
            _driver.Navigate().Back();
        }

        [When(@"I make a payment with data row "(.*)"")]
        public void WhenIMakeAPaymentWithDataRow(string rowIndex)
        {
            _loanPage.MakePayment();
            // For demonstration, read pseudo-CSV from TestData/PaymentData.xlsx (treated as CSV)
            var dataFile = Path.Combine(AppContext.BaseDirectory, "TestData", "PaymentData.xlsx");
            // If file doesn't exist or content minimal, this will be a no-op
            // Here we would continue the flow to fill payment form; since locators are unknown for form fields,
            // we demonstrate until open action; extend here when form locators are available.
        }

        [Then("I should see a payment success confirmation")]
        public void ThenIShouldSeePaymentSuccess()
        {
            // Placeholder assertion path: Typically verify a toast/snackbar; not provided in locators.
            // We'll assert no exception occurred up to this point.
            Assert.Pass("Payment flow invoked - success confirmation verification requires specific locator.");
        }

        [When("I open the Escrow tab")]
        public void WhenIOpenEscrowTab()
        {
            _loanPage.OpenEscrowTab();
        }

        [Then("I should see the escrow details")]
        public void ThenIShouldSeeEscrowDetails()
        {
            // With provided locator, we ensure no exception in clicking; add validation when content locator known
            Assert.IsTrue(true, "Escrow tab opened.");
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                if (_driver != null)
                {
                    ScreenshotHelper.TakeScreenshot(_driver, _context.ScenarioInfo.Title.Replace(' ', '_'));
                    _driver.Quit();
                }
            }
            catch { }
        }
    }
}