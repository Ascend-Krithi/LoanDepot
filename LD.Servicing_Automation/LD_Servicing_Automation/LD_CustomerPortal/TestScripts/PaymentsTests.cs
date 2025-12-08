using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_CustomerPortal.TestPages;
using System;

namespace LD_CustomerPortal.TestScripts
{
    [TestClass]
    public class PaymentsTests : BasePage
    {
        public TestContext TestContext { get; set; }
        private IWebDriver driver;
        private LoginPage loginPage;
        private DashboardPage dashboardPage;
        private PaymentsPage paymentsPage;
        private WebElementExtensionsPage webElementExtensions;
        private DBconnect dbConnect;

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeFramework(TestContext);
            loginPage = new LoginPage(_driver, test);
            dashboardPage = new DashboardPage(_driver, test);
            paymentsPage = new PaymentsPage(_driver, test);
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            dbConnect = new DBconnect(test, Constants.DBNames.MelloServETL);
        }

        // Test Case Mapping: CUSTP-2005 TS-001 TC-001
        [TestMethod]
        [TestCategory("Payments")]
        [Owner("Automation Test Engineer")]
        public void CUSTP_2005_TS_001_TC_001_LateFeeMessageAfter15DayGracePeriod_NonNC()
        {
            try
            {
                // ARRANGE
                string username = "Customer1";
                string password = "Pass@123";
                string propertyState = "TX";
                string paymentDate = "6/17/23";

                // ACT
                test.Log(AventStack.ExtentReports.Status.Info, "Step 1: Launch Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");
                test.Log(AventStack.ExtentReports.Status.Info, "Step 2: Login");
                loginPage.Login(username, password);
                test.Log(AventStack.ExtentReports.Status.Info, "Step 3: Navigate to HELOC loan account");
                dashboardPage.NavigateToHelocLoan(propertyState);
                test.Log(AventStack.ExtentReports.Status.Info, "Step 4: Click 'Make a Payment'");
                dashboardPage.ClickMakePayment();
                test.Log(AventStack.ExtentReports.Status.Info, "Step 5: Select payment date after grace period");
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 15-day grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"Test failed: {ex.Message}");
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-002 TC-001
        [TestMethod]
        [TestCategory("Payments")]
        [Owner("Automation Test Engineer")]
        public void CUSTP_2005_TS_002_TC_001_LateFeeMessageAfter40DayGracePeriod_NonNC()
        {
            try
            {
                string username = "Customer1";
                string password = "Pass@123";
                string propertyState = "TX";
                string paymentDate = "7/12/23";

                _driver.Navigate().GoToUrl("https://Customerportal.example.com");
                loginPage.Login(username, password);
                dashboardPage.NavigateToHelocLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 40-day grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"Test failed: {ex.Message}");
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-004 TC-001
        [TestMethod]
        [TestCategory("Payments")]
        [Owner("Automation Test Engineer")]
        public void CUSTP_2005_TS_004_TC_001_NoLateFeeMessageForNCProperty()
        {
            try
            {
                string username = "Customer1";
                string password = "Pass@123";
                string propertyState = "NC";
                string paymentDate1 = "6/17/23";
                string paymentDate2 = "7/12/23";

                _driver.Navigate().GoToUrl("https://Customerportal.example.com");
                loginPage.Login(username, password);
                dashboardPage.NavigateToHelocLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                paymentsPage.SubmitPayment();
                Assert.IsTrue(paymentsPage.IsNoLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property.");
                paymentsPage.SelectPaymentDate(paymentDate2);
                paymentsPage.SubmitPayment();
                Assert.IsTrue(paymentsPage.IsNoLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property.");
                test.Log(AventStack.ExtentReports.Status.Pass, "No late fee message displayed for NC property as expected.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"Test failed: {ex.Message}");
                throw;
            }
        }

        // Additional test methods for other scenarios (TS-003, TS-005, TS-006, TS-007, TS-008, TS-009) would follow the same pattern,
        // instantiating page objects, navigating, performing actions, and asserting expected results.

        [TestCleanup]
        public void TestCleanup()
        {
            // Automatic cleanup via BasePage TearDown
        }
    }
}
