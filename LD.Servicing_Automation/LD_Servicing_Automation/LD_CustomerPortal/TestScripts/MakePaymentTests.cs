using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using LD_Servicing_Automation.LD_CustomerPortal.TestPages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using System;

namespace LD_Servicing_Automation.LD_CustomerPortal.TestScripts
{
    [TestClass]
    public class MakePaymentTests : BasePage
    {
        public TestContext TestContext { get; set; }
        private LoginPage loginPage;
        private DashboardPage dashboardPage;
        private MakePaymentPage makePaymentPage;
        private WebElementExtensionsPage webElementExtensions;
        private DBconnect dbConnect;

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeFramework(TestContext);
            loginPage = new LoginPage(_driver, test);
            dashboardPage = new DashboardPage(_driver, test);
            makePaymentPage = new MakePaymentPage(_driver, test);
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            dbConnect = new DBconnect(test, Constants.DBNames.MelloServETL);
        }

        // Test Case Mapping: CUSTP-2005 TS-001 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_001_TC_001_VerifyLateFeeAfterGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/17/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                test.Log(AventStack.ExtentReports.Status.Info, "Logging in");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan account");
                dashboardPage.NavigateToLoan(propertyState);

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make a Payment screen must load.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date after grace period");
                makePaymentPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after grace period.");
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
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_002_TC_001_VerifyLateFeeAfter40DayGracePeriod()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "7/12/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan account");
                dashboardPage.NavigateToLoan(propertyState);

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make a Payment screen must load.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date after 40-day grace period");
                makePaymentPage.SelectPaymentDate(paymentDate);

                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 40-day grace period.");
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
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_003_TC_001_VerifyLateFeeAfterJulyGracePeriod()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "8/11/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan account");
                dashboardPage.NavigateToLoan(propertyState);

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make a Payment screen must load.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date after July grace period");
                makePaymentPage.SelectPaymentDate(paymentDate);

                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after July grace period.");
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
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_004_TC_001_VerifyNoLateFeeForNCState()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "NC";
                string paymentDate1 = "6/17/23";
                string paymentDate2 = "7/12/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan account for NC");
                dashboardPage.NavigateToLoan(propertyState);

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make a Payment screen must load.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date 6/17/23");
                makePaymentPage.SelectPaymentDate(paymentDate1);
                Assert.IsFalse(makePaymentPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC state on 6/17/23.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date 7/12/23");
                makePaymentPage.SelectPaymentDate(paymentDate2);
                Assert.IsFalse(makePaymentPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC state on 7/12/23.");

                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: No late fee message for NC state payments.");
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
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_005_TC_001_VerifyNoLateFeeWithinGracePeriod()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/16/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan account");
                dashboardPage.NavigateToLoan(propertyState);

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make a Payment screen must load.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date within grace period");
                makePaymentPage.SelectPaymentDate(paymentDate);
                Assert.IsFalse(makePaymentPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed within grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: No late fee message within grace period.");
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
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_006_TC_001_VerifyLateFeeBoundary()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate1 = "6/16/23";
                string paymentDate2 = "6/17/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan account");
                dashboardPage.NavigateToLoan(propertyState);

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make a Payment screen must load.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date last day of grace period");
                makePaymentPage.SelectPaymentDate(paymentDate1);
                Assert.IsFalse(makePaymentPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed on last day of grace period.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date first day after grace period");
                makePaymentPage.SelectPaymentDate(paymentDate2);
                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed on first day after grace period.");

                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Late fee boundary verified.");
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
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_007_TC_001_VerifyLateFeeBoundaryJuly()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate1 = "7/11/23";
                string paymentDate2 = "7/12/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan account");
                dashboardPage.NavigateToLoan(propertyState);

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make a Payment screen must load.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date last day of 40-day grace period");
                makePaymentPage.SelectPaymentDate(paymentDate1);
                Assert.IsFalse(makePaymentPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed on last day of 40-day grace period.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date first day after 40-day grace period");
                makePaymentPage.SelectPaymentDate(paymentDate2);
                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed on first day after 40-day grace period.");

                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Late fee boundary for July verified.");
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
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_008_TC_001_VerifyLateFeeLeapYear()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "3/12/24";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Navigating to HELOC loan account");
                dashboardPage.NavigateToLoan(propertyState);

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make a Payment screen must load.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date after leap year grace period");
                makePaymentPage.SelectPaymentDate(paymentDate);
                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after leap year grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Late fee message displayed for leap year grace period.");
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
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_009_TC_001_VerifyMakePaymentButtonEnabled()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/17/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                test.Log(AventStack.ExtentReports.Status.Info, "Logging in");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Verifying Make a Payment button is enabled");
                Assert.IsTrue(dashboardPage.PaymentLink.Enabled, "Make a Payment button should be enabled (purple)");

                test.Log(AventStack.ExtentReports.Status.Info, "Clicking Make a Payment");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make a Payment screen must load.");

                test.Log(AventStack.ExtentReports.Status.Info, "Selecting payment date after grace period");
                makePaymentPage.SelectPaymentDate(paymentDate);
                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Make a Payment button enabled and late fee message displayed.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Automatic cleanup via base class
        }
    }
}
