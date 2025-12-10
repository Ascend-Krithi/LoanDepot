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
            dbConnect = new DBconnect(test, LD_AutomationFramework.Base.Constants.DBNames.MelloServETL);
        }

        // Test Case Mapping: CUSTP-2005 TS-001 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation QA")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_001_TC_001_GracePeriodLateFee()
        {
            try
            {
                // ARRANGE
                string url = "https://Customerportal.example.com";
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/17/23";
                string dueDate = "6/1/23";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToHelocLoanAccount(propertyState);
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

        // Test Case Mapping: CUSTP-2005 TS-002 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation QA")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_002_TC_001_GracePeriod40DaysLateFee()
        {
            try
            {
                // ARRANGE
                string url = "https://Customerportal.example.com";
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "7/12/23";
                string dueDate = "6/1/23";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToHelocLoanAccount(propertyState);
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

        // Test Case Mapping: CUSTP-2005 TS-003 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation QA")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_003_TC_001_GracePeriodJulyLateFee()
        {
            try
            {
                // ARRANGE
                string url = "https://Customerportal.example.com";
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "8/11/23";
                string dueDate = "7/1/23";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToHelocLoanAccount(propertyState);
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

        // Test Case Mapping: CUSTP-2005 TS-004 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation QA")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_004_TC_001_NCNoLateFee()
        {
            try
            {
                // ARRANGE
                string url = "https://Customerportal.example.com";
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "NC";
                string paymentDate1 = "6/17/23";
                string paymentDate2 = "7/12/23";
                string dueDate = "6/1/23";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToHelocLoanAccount(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                paymentsPage.SubmitPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property state on 6/17/23.");
                paymentsPage.SelectPaymentDate(paymentDate2);
                paymentsPage.SubmitPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property state on 7/12/23.");
                test.Log(Status.Pass, "Test passed: No late fee message displayed for NC property state.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-005 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation QA")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_005_TC_001_WithinGracePeriodNoLateFee()
        {
            try
            {
                // ARRANGE
                string url = "https://Customerportal.example.com";
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/16/23";
                string dueDate = "6/1/23";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToHelocLoanAccount(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed within 15-day grace period.");
                test.Log(Status.Pass, "Test passed: No late fee message displayed within grace period.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-006 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation QA")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_006_TC_001_GracePeriodBoundaryLateFee()
        {
            try
            {
                // ARRANGE
                string url = "https://Customerportal.example.com";
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate1 = "6/16/23";
                string paymentDate2 = "6/17/23";
                string dueDate = "6/1/23";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToHelocLoanAccount(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                paymentsPage.SubmitPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed on last day of grace period (6/16/23).");
                paymentsPage.SelectPaymentDate(paymentDate2);
                paymentsPage.SubmitPayment();
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed on first day after grace period (6/17/23).");
                test.Log(Status.Pass, "Test passed: Grace period boundary late fee logic verified.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-007 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation QA")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_007_TC_001_40DayGracePeriodBoundaryLateFee()
        {
            try
            {
                // ARRANGE
                string url = "https://Customerportal.example.com";
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate1 = "7/11/23";
                string paymentDate2 = "7/12/23";
                string dueDate = "6/1/23";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToHelocLoanAccount(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                paymentsPage.SubmitPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed on last day of 40-day grace period (7/11/23).");
                paymentsPage.SelectPaymentDate(paymentDate2);
                paymentsPage.SubmitPayment();
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed on first day after 40-day grace period (7/12/23).");
                test.Log(Status.Pass, "Test passed: 40-day grace period boundary late fee logic verified.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-008 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation QA")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_008_TC_001_LeapYearLateFee()
        {
            try
            {
                // ARRANGE
                string url = "https://Customerportal.example.com";
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "3/12/24";
                string dueDate = "2/1/24";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToHelocLoanAccount(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment past 40-day grace period in leap year.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed for leap year scenario.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-009 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation QA")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_009_TC_001_MakePaymentButtonEnabledLateFee()
        {
            try
            {
                // ARRANGE
                string url = "https://Customerportal.example.com";
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/17/23";
                string dueDate = "6/1/23";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToHelocLoanAccount(propertyState);
                // Verify 'Make a Payment' button is enabled (purple)
                Assert.IsTrue(webElementExtensions.IsElementEnabled(_driver, dashboardPage.makePaymentButtonLocBy), "'Make a Payment' button should be enabled and clickable.");
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment past grace period.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed as expected.");
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
            // Automatic cleanup via base class
        }
    }
}
