using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_CustomerPortal.TestPages;

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

        [TestCleanup]
        public void TestCleanup()
        {
            TearDown();
        }

        // Test Case Mapping: CUSTP-2005 TS-001 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_001_TC_001_LateFeeAfterGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/17/23";
                string dueDate = "6/1/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHELOC(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after grace period.");
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
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_002_TC_001_LateFeeAfter40DayGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "7/12/23";
                string dueDate = "6/1/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHELOC(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
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
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_003_TC_001_LateFeeAfterJulyGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "8/11/23";
                string dueDate = "7/1/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHELOC(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after July grace period.");
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
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_004_TC_001_NoLateFeeForNC()
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

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHELOC(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                paymentsPage.SubmitPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property on 6/17/23.");
                paymentsPage.SelectPaymentDate(paymentDate2);
                paymentsPage.SubmitPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property on 7/12/23.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: No late fee message for NC property as expected.");
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
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_005_TC_001_NoLateFeeWithinGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/16/23";
                string dueDate = "6/1/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHELOC(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed within grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: No late fee message within grace period as expected.");
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
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_006_TC_001_GracePeriodBoundary()
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

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHELOC(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                paymentsPage.SubmitPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed on last day of grace period.");
                paymentsPage.SelectPaymentDate(paymentDate2);
                paymentsPage.SubmitPayment();
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed on first day after grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Grace period boundary handled as expected.");
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
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_007_TC_001_40DayGracePeriodBoundary()
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

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHELOC(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                paymentsPage.SubmitPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed on last day of 40-day grace period.");
                paymentsPage.SelectPaymentDate(paymentDate2);
                paymentsPage.SubmitPayment();
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed on first day after 40-day grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: 40-day grace period boundary handled as expected.");
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
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_008_TC_001_LateFeeLeapYear()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "3/12/24";
                string dueDate = "2/1/24";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                dashboardPage.NavigateToHELOC(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after leap year grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Late fee message displayed for leap year as expected.");
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
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_009_TC_001_MakePaymentButtonEnabled()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/17/23";
                string dueDate = "6/1/23";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl("https://Customerportal.example.com");

                // ACT
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");
                // Verify Make a Payment button is enabled (purple)
                Assert.IsTrue(webElementExtensions.IsElementEnabled(_driver, dashboardPage.makePaymentButton), "Make a Payment button should be enabled and clickable.");
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
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
