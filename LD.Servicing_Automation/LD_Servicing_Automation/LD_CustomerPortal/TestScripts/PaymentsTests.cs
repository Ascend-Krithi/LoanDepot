using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_CustomerPortal.TestPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
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

        // Test Case: CUSTP-2005 TS-001 TC-001
        // Grace period late fee for HELOC loan (property state other than NC, payment 16 days after due date)
        [TestMethod]
        [TestCategory("HELOC")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_001_TC_001_GracePeriodLateFee_HELOC_OtherThanNC_16DaysAfterDueDate()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/17/23";
                string dueDate = "6/1/23";
                test.Log(Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                test.Log(Status.Info, "Logging in as Customer");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHelocLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment past 15-day grace period.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case: CUSTP-2005 TS-002 TC-001
        // Grace period late fee for HELOC loan (property state other than NC, payment 41 days after due date)
        [TestMethod]
        [TestCategory("HELOC")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_002_TC_001_GracePeriodLateFee_HELOC_OtherThanNC_41DaysAfterDueDate()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "7/12/23";
                string dueDate = "6/1/23";
                test.Log(Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                test.Log(Status.Info, "Logging in as Customer");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHelocLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment past 40-day grace period.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case: CUSTP-2005 TS-003 TC-001
        // Grace period late fee for HELOC loan (property state other than NC, payment 41 days after due date in July)
        [TestMethod]
        [TestCategory("HELOC")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_003_TC_001_GracePeriodLateFee_HELOC_OtherThanNC_41DaysAfterDueDate_July()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "8/11/23";
                string dueDate = "7/1/23";
                test.Log(Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                test.Log(Status.Info, "Logging in as Customer");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHelocLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment past 40-day grace period.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case: CUSTP-2005 TS-004 TC-001
        // No late fee for HELOC loan (property state NC, payment after due date)
        [TestMethod]
        [TestCategory("HELOC")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_004_TC_001_NoLateFee_HELOC_NC_AfterDueDate()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "NC";
                string paymentDate1 = "6/17/23";
                string paymentDate2 = "7/12/23";
                string dueDate = "6/1/23";
                test.Log(Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                test.Log(Status.Info, "Logging in as Customer");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHelocLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                paymentsPage.SubmitPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property state.");
                paymentsPage.SelectPaymentDate(paymentDate2);
                paymentsPage.SubmitPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property state.");
                test.Log(Status.Pass, "Test passed: No late fee message displayed for NC property state as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case: CUSTP-2005 TS-005 TC-001
        // No late fee for HELOC loan (property state other than NC, payment within 15-day grace period)
        [TestMethod]
        [TestCategory("HELOC")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_005_TC_001_NoLateFee_HELOC_OtherThanNC_WithinGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/16/23";
                string dueDate = "6/1/23";
                test.Log(Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                test.Log(Status.Info, "Logging in as Customer");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHelocLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed within 15-day grace period.");
                test.Log(Status.Pass, "Test passed: No late fee message displayed within grace period as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case: CUSTP-2005 TS-006 TC-001
        // Grace period boundary for HELOC loan (property state other than NC, last day of grace period and first day after)
        [TestMethod]
        [TestCategory("HELOC")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_006_TC_001_GracePeriodBoundary_HELOC_OtherThanNC_LastDayAndFirstDayAfter()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate1 = "6/16/23";
                string paymentDate2 = "6/17/23";
                string dueDate = "6/1/23";
                test.Log(Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                test.Log(Status.Info, "Logging in as Customer");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHelocLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                paymentsPage.SubmitPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed on last day of grace period.");
                paymentsPage.SelectPaymentDate(paymentDate2);
                paymentsPage.SubmitPayment();
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed on first day after grace period.");
                test.Log(Status.Pass, "Test passed: Grace period boundary validated as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case: CUSTP-2005 TS-007 TC-001
        // Grace period boundary for HELOC loan (property state other than NC, last day of 40-day grace period and first day after)
        [TestMethod]
        [TestCategory("HELOC")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_007_TC_001_GracePeriodBoundary_HELOC_OtherThanNC_40DayLastDayAndFirstDayAfter()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate1 = "7/11/23";
                string paymentDate2 = "7/12/23";
                string dueDate = "6/1/23";
                test.Log(Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                test.Log(Status.Info, "Logging in as Customer");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHelocLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                paymentsPage.SubmitPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed on last day of 40-day grace period.");
                paymentsPage.SelectPaymentDate(paymentDate2);
                paymentsPage.SubmitPayment();
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed on first day after 40-day grace period.");
                test.Log(Status.Pass, "Test passed: 40-day grace period boundary validated as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case: CUSTP-2005 TS-008 TC-001
        // Grace period late fee for HELOC loan (property state other than NC, leap year, payment 40 days after due date)
        [TestMethod]
        [TestCategory("HELOC")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_008_TC_001_GracePeriodLateFee_HELOC_OtherThanNC_LeapYear_40DaysAfterDueDate()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "3/12/24";
                string dueDate = "2/1/24";
                test.Log(Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                test.Log(Status.Info, "Logging in as Customer");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHelocLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment past 40-day grace period in leap year.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed for leap year as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case: CUSTP-2005 TS-009 TC-001
        // Make a Payment button enabled and late fee message after grace period
        [TestMethod]
        [TestCategory("HELOC")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_009_TC_001_MakePaymentButtonEnabled_LateFeeAfterGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/17/23";
                string dueDate = "6/1/23";
                test.Log(Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                test.Log(Status.Info, "Logging in as Customer");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                Assert.IsTrue(webElementExtensions.IsElementEnabled(_driver, dashboardPage.makePaymentButtonLocBy), "Make a Payment button should be enabled (purple)");
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after grace period.");
                test.Log(Status.Pass, "Test passed: Make a Payment button enabled and late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Automatic cleanup via BasePage TearDown
        }
    }
}
