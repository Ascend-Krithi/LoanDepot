using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using LD_CustomerPortal.TestPages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestScripts
{
    [TestClass]
    public class MakePaymentTests : BasePage
    {
        public TestContext TestContext { get; set; }
        private IWebDriver driver;
        private ExtentTest test;
        private LoginPage loginPage;
        private DashboardPage dashboardPage;
        private MakePaymentPage makePaymentPage;
        private WebElementExtensionsPage webElementExtensions;

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeFramework(TestContext);
            loginPage = new LoginPage(_driver, test);
            dashboardPage = new DashboardPage(_driver, test);
            makePaymentPage = new MakePaymentPage(_driver, test);
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
        }

        // Test Case Mapping: CUSTP-2005 TS-001 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_001_TC_001_LateFeeAfter15Days()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string url = "https://Customerportal.example.com";
                string propertyState = "TX";
                string paymentDate = "6/17/23";

                test.Log(Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl(url);
                Assert.IsTrue(loginPage.EmailInput.Displayed, "Login page should be displayed.");

                // ACT
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make Payment screen should be displayed.");

                makePaymentPage.SelectPaymentDate(paymentDate);
                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 15-day grace period.");

                test.Log(Status.Pass, "Test CUSTP-2005 TS-001 TC-001 passed.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test CUSTP-2005 TS-001 TC-001 failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-002 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_002_TC_001_LateFeeAfter40Days()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string url = "https://Customerportal.example.com";
                string propertyState = "TX";
                string paymentDate = "7/12/23";

                test.Log(Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl(url);
                Assert.IsTrue(loginPage.EmailInput.Displayed, "Login page should be displayed.");

                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make Payment screen should be displayed.");

                makePaymentPage.SelectPaymentDate(paymentDate);
                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 40-day grace period.");

                test.Log(Status.Pass, "Test CUSTP-2005 TS-002 TC-001 passed.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test CUSTP-2005 TS-002 TC-001 failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-003 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_003_TC_001_LateFeeAfter40DaysJuly()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string url = "https://Customerportal.example.com";
                string propertyState = "TX";
                string paymentDate = "8/11/23";

                test.Log(Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl(url);
                Assert.IsTrue(loginPage.EmailInput.Displayed, "Login page should be displayed.");

                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make Payment screen should be displayed.");

                makePaymentPage.SelectPaymentDate(paymentDate);
                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 40-day grace period.");

                test.Log(Status.Pass, "Test CUSTP-2005 TS-003 TC-001 passed.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test CUSTP-2005 TS-003 TC-001 failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-004 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_004_TC_001_NoLateFeeNC()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string url = "https://Customerportal.example.com";
                string propertyState = "NC";
                string paymentDate1 = "6/17/23";
                string paymentDate2 = "7/12/23";

                test.Log(Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl(url);
                Assert.IsTrue(loginPage.EmailInput.Displayed, "Login page should be displayed.");

                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make Payment screen should be displayed.");

                makePaymentPage.SelectPaymentDate(paymentDate1);
                Assert.IsFalse(makePaymentPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property state (payment date 6/17/23).");

                makePaymentPage.SelectPaymentDate(paymentDate2);
                Assert.IsFalse(makePaymentPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property state (payment date 7/12/23).");

                test.Log(Status.Pass, "Test CUSTP-2005 TS-004 TC-001 passed.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test CUSTP-2005 TS-004 TC-001 failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-005 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_005_TC_001_NoLateFeeWithinGracePeriod()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string url = "https://Customerportal.example.com";
                string propertyState = "TX";
                string paymentDate = "6/16/23";

                test.Log(Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl(url);
                Assert.IsTrue(loginPage.EmailInput.Displayed, "Login page should be displayed.");

                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make Payment screen should be displayed.");

                makePaymentPage.SelectPaymentDate(paymentDate);
                Assert.IsFalse(makePaymentPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed within 15-day grace period.");

                test.Log(Status.Pass, "Test CUSTP-2005 TS-005 TC-001 passed.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test CUSTP-2005 TS-005 TC-001 failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-006 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_006_TC_001_GracePeriodBoundary()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string url = "https://Customerportal.example.com";
                string propertyState = "TX";
                string paymentDate1 = "6/16/23";
                string paymentDate2 = "6/17/23";

                test.Log(Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl(url);
                Assert.IsTrue(loginPage.EmailInput.Displayed, "Login page should be displayed.");

                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make Payment screen should be displayed.");

                makePaymentPage.SelectPaymentDate(paymentDate1);
                Assert.IsFalse(makePaymentPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for last day of grace period.");

                makePaymentPage.SelectPaymentDate(paymentDate2);
                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for first day after grace period.");

                test.Log(Status.Pass, "Test CUSTP-2005 TS-006 TC-001 passed.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test CUSTP-2005 TS-006 TC-001 failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-007 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_007_TC_001_GracePeriodBoundary40Days()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string url = "https://Customerportal.example.com";
                string propertyState = "TX";
                string paymentDate1 = "7/11/23";
                string paymentDate2 = "7/12/23";

                test.Log(Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl(url);
                Assert.IsTrue(loginPage.EmailInput.Displayed, "Login page should be displayed.");

                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make Payment screen should be displayed.");

                makePaymentPage.SelectPaymentDate(paymentDate1);
                Assert.IsFalse(makePaymentPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for last day of 40-day grace period.");

                makePaymentPage.SelectPaymentDate(paymentDate2);
                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for first day after 40-day grace period.");

                test.Log(Status.Pass, "Test CUSTP-2005 TS-007 TC-001 passed.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test CUSTP-2005 TS-007 TC-001 failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-008 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_008_TC_001_LateFeeLeapYear()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string url = "https://Customerportal.example.com";
                string propertyState = "TX";
                string paymentDate = "3/12/24";

                test.Log(Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl(url);
                Assert.IsTrue(loginPage.EmailInput.Displayed, "Login page should be displayed.");

                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make Payment screen should be displayed.");

                makePaymentPage.SelectPaymentDate(paymentDate);
                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 40-day grace period in leap year.");

                test.Log(Status.Pass, "Test CUSTP-2005 TS-008 TC-001 passed.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test CUSTP-2005 TS-008 TC-001 failed: " + ex.Message);
                throw;
            }
        }

        // Test Case Mapping: CUSTP-2005 TS-009 TC-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Team")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_009_TC_001_MakePaymentButtonEnabled()
        {
            try
            {
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string url = "https://Customerportal.example.com";
                string propertyState = "TX";
                string paymentDate = "6/17/23";

                test.Log(Status.Info, "Launching Customer Portal.");
                _driver.Navigate().GoToUrl(url);
                Assert.IsTrue(loginPage.EmailInput.Displayed, "Login page should be displayed.");

                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after login.");

                Assert.IsTrue(dashboardPage.MakePaymentButton.Enabled, "Make a Payment button should be enabled (purple).");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(makePaymentPage.IsPaymentScreenDisplayed(), "Make Payment screen should be displayed.");

                makePaymentPage.SelectPaymentDate(paymentDate);
                Assert.IsTrue(makePaymentPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after grace period.");

                test.Log(Status.Pass, "Test CUSTP-2005 TS-009 TC-001 passed.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test CUSTP-2005 TS-009 TC-001 failed: " + ex.Message);
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
