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

        [TestCleanup]
        public void TestCleanup()
        {
            // Automatic cleanup via BasePage TearDown
        }

        // Test Case Mapping: CUSTP-2005 TS-001 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        public void CUSTP_2005_TS_001_TC_001_PaymentAfter15DayGracePeriod_NonNC()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in as Customer1");
                loginPage.Login("Customer1", "Pass@123");
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan for TX (not NC)");
                dashboardPage.NavigateToHELOCLoan("TX");

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date: 6/17/23 (16 days after due date 6/1/23)");
                paymentsPage.SelectPaymentDate("6/17/23");
                paymentsPage.SubmitPayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Verifying late fee message");
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 15-day grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-002 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        public void CUSTP_2005_TS_002_TC_001_PaymentAfter40DayGracePeriod_NonNC_June()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in as Customer1");
                loginPage.Login("Customer1", "Pass@123");
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan for TX (not NC)");
                dashboardPage.NavigateToHELOCLoan("TX");

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date: 7/12/23 (41 days after due date 6/1/23)");
                paymentsPage.SelectPaymentDate("7/12/23");
                paymentsPage.SubmitPayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Verifying late fee message");
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 40-day grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-003 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        public void CUSTP_2005_TS_003_TC_001_PaymentAfter40DayGracePeriod_NonNC_July()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in as Customer1");
                loginPage.Login("Customer1", "Pass@123");
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan for TX (not NC)");
                dashboardPage.NavigateToHELOCLoan("TX");

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date: 8/11/23 (41 days after due date 7/1/23)");
                paymentsPage.SelectPaymentDate("8/11/23");
                paymentsPage.SubmitPayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Verifying late fee message");
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 40-day grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-004 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        public void CUSTP_2005_TS_004_TC_001_PaymentForNCProperty_NoLateFee()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in as Customer1");
                loginPage.Login("Customer1", "Pass@123");
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan for NC");
                dashboardPage.NavigateToHELOCLoan("NC");

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date: 6/17/23");
                paymentsPage.SelectPaymentDate("6/17/23");
                paymentsPage.SubmitPayment();
                test.Log(AventStack.ExtentReports.Status.Info, "Verifying no late fee message for 6/17/23");
                Assert.IsTrue(paymentsPage.IsNoLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date: 7/12/23");
                paymentsPage.SelectPaymentDate("7/12/23");
                paymentsPage.SubmitPayment();
                test.Log(AventStack.ExtentReports.Status.Info, "Verifying no late fee message for 7/12/23");
                Assert.IsTrue(paymentsPage.IsNoLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property.");

                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: No late fee message displayed for NC property payments.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-005 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        public void CUSTP_2005_TS_005_TC_001_PaymentWithin15DayGracePeriod_NonNC()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in as Customer1");
                loginPage.Login("Customer1", "Pass@123");
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan for TX (not NC)");
                dashboardPage.NavigateToHELOCLoan("TX");

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date: 6/16/23 (within 15-day grace period)");
                paymentsPage.SelectPaymentDate("6/16/23");
                paymentsPage.SubmitPayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Verifying no late fee message");
                Assert.IsTrue(paymentsPage.IsNoLateFeeMessageDisplayed(), "No late fee message should be displayed for payment within 15-day grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: No late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-006 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        public void CUSTP_2005_TS_006_TC_001_PaymentOnLastDayAndFirstDayAfterGracePeriod_NonNC()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in as Customer1");
                loginPage.Login("Customer1", "Pass@123");
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan for TX (not NC)");
                dashboardPage.NavigateToHELOCLoan("TX");

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date: 6/16/23 (last day of grace period)");
                paymentsPage.SelectPaymentDate("6/16/23");
                paymentsPage.SubmitPayment();
                test.Log(AventStack.ExtentReports.Status.Info, "Verifying no late fee message for 6/16/23");
                Assert.IsTrue(paymentsPage.IsNoLateFeeMessageDisplayed(), "No late fee message should be displayed for last day of grace period.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date: 6/17/23 (first day after grace period)");
                paymentsPage.SelectPaymentDate("6/17/23");
                paymentsPage.SubmitPayment();
                test.Log(AventStack.ExtentReports.Status.Info, "Verifying late fee message for 6/17/23");
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for first day after grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Grace period boundary verified.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-007 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        public void CUSTP_2005_TS_007_TC_001_PaymentOnLastDayAndFirstDayAfter40DayGracePeriod_NonNC()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in as Customer1");
                loginPage.Login("Customer1", "Pass@123");
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan for TX (not NC)");
                dashboardPage.NavigateToHELOCLoan("TX");

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date: 7/11/23 (last day of 40-day grace period)");
                paymentsPage.SelectPaymentDate("7/11/23");
                paymentsPage.SubmitPayment();
                test.Log(AventStack.ExtentReports.Status.Info, "Verifying no late fee message for 7/11/23");
                Assert.IsTrue(paymentsPage.IsNoLateFeeMessageDisplayed(), "No late fee message should be displayed for last day of 40-day grace period.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date: 7/12/23 (first day after grace period)");
                paymentsPage.SelectPaymentDate("7/12/23");
                paymentsPage.SubmitPayment();
                test.Log(AventStack.ExtentReports.Status.Info, "Verifying late fee message for 7/12/23");
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for first day after 40-day grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: 40-day grace period boundary verified.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-008 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        public void CUSTP_2005_TS_008_TC_001_PaymentAfter40DayGracePeriod_LeapYear()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in as Customer1");
                loginPage.Login("Customer1", "Pass@123");
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan for TX (not NC)");
                dashboardPage.NavigateToHELOCLoan("TX");

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date: 3/12/24 (40 days after due date 2/1/24, leap year)");
                paymentsPage.SelectPaymentDate("3/12/24");
                paymentsPage.SubmitPayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Verifying late fee message");
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 40-day grace period in leap year.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Late fee message displayed as expected for leap year scenario.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-009 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        public void CUSTP_2005_TS_009_TC_001_MakePaymentButtonEnabled_PaymentAfterGracePeriod()
        {
            try
            {
                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in as Customer1");
                loginPage.Login("Customer1", "Pass@123");
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Verifying Make a Payment button is enabled (purple)");
                Assert.IsTrue(webElementExtensions.IsElementEnabled(_driver, dashboardPage.makePaymentButtonLocBy), "Make a Payment button should be enabled and clickable.");

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date: 6/17/23 (after grace period)");
                paymentsPage.SelectPaymentDate("6/17/23");
                paymentsPage.SubmitPayment();

                test.Log(AventStack.ExtentReports.Status.Info, "Verifying late fee message");
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Make a Payment button enabled and late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }
    }
}
