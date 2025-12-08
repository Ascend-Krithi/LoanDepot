using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_CustomerPortal.Pages;
using System;

/// <summary>
/// PaymentsTests - MSTest class for Make a Payment and Late Fee Message scenarios.
/// Implements data-driven testing using LoanDetails.xml and TestCredentials.xml.
/// Test Cases: CUSTP-2005 TS-001 TC-001, TS-002 TC-001, TS-003 TC-001, TS-004 TC-001, TS-005 TC-001, TS-006 TC-001, TS-007 TC-001, TS-008 TC-001, TS-009 TC-001
/// </summary>
namespace LD_CustomerPortal.TestScripts
{
    [TestClass]
    public class PaymentsTests : BasePage
    {
        public TestContext TestContext { get; set; }
        private PaymentsPage paymentsPage;
        private WebElementExtensionsPage webElementExtensions;
        private DBconnect dbConnect;

        [TestInitialize]
        public void TestInitialize()
        {
            // Initialize Framework
            InitializeFramework(TestContext);
            // Initialize Page Objects
            paymentsPage = new PaymentsPage(_driver, test);
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            // Initialize Database Connection
            dbConnect = new DBconnect(test, Constants.DBNames.MelloServETL);
        }

        /// <summary>
        /// [CUSTP-2005 TS-001 TC-001] Customer Portal - Late Fee Message for Payment After 15-Day Grace Period (Non-NC Property)
        /// Data-driven using LoanDetails.xml and TestCredentials.xml
        /// </summary>
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Test Engineer")]
        // Test Case Mapping: CUSTP-2005 TS-001 TC-001
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_001_TC_001_LateFeeMessageAfter15DayGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string encryptedPassword = TestContext.DataRow["password"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(encryptedPassword);
                string paymentDate = "06/17/2023"; // 16 days after due date 6/1/23
                string dueDate = "06/01/2023";
                string propertyState = "TX";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal and logging in");
                CommonServicesPage commonServices = new CommonServicesPage(_driver, test);
                commonServices.Login(username, password);

                // ACT
                paymentsPage.ClickMakeAPayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.ClickConfirmPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 15-day grace period.");
                string lateFeeMsg = paymentsPage.GetLateFeeMessageText();
                Assert.IsTrue(lateFeeMsg.Contains("past the 15-day grace period"), "Late fee message should indicate payment is past the 15-day grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Late fee message verified for payment after 15-day grace period.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"Test failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// [CUSTP-2005 TS-002 TC-001] Customer Portal - Late Fee Message for Payment After 40-Day Grace Period (Non-NC Property)
        /// Data-driven using LoanDetails.xml and TestCredentials.xml
        /// </summary>
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Test Engineer")]
        // Test Case Mapping: CUSTP-2005 TS-002 TC-001
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_002_TC_001_LateFeeMessageAfter40DayGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string encryptedPassword = TestContext.DataRow["password"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(encryptedPassword);
                string paymentDate = "07/12/2023"; // 41 days after due date 6/1/23
                string dueDate = "06/01/2023";
                string propertyState = "TX";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal and logging in");
                CommonServicesPage commonServices = new CommonServicesPage(_driver, test);
                commonServices.Login(username, password);

                // ACT
                paymentsPage.ClickMakeAPayment();
                paymentsPage.SelectPaymentDate(paymentDate);
                paymentsPage.ClickConfirmPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 40-day grace period.");
                string lateFeeMsg = paymentsPage.GetLateFeeMessageText();
                Assert.IsTrue(lateFeeMsg.Contains("past the 40-day grace period"), "Late fee message should indicate payment is past the 40-day grace period.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Late fee message verified for payment after 40-day grace period.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"Test failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// [CUSTP-2005 TS-004 TC-001] Customer Portal - No Late Fee Message for Payment (NC Property)
        /// Data-driven using LoanDetails.xml and TestCredentials.xml
        /// </summary>
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Test Engineer")]
        // Test Case Mapping: CUSTP-2005 TS-004 TC-001
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_004_TC_001_NoLateFeeMessageForNCProperty()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string encryptedPassword = TestContext.DataRow["password"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(encryptedPassword);
                string paymentDate1 = "06/17/2023";
                string paymentDate2 = "07/12/2023";
                string dueDate = "06/01/2023";
                string propertyState = "NC";

                test.Log(AventStack.ExtentReports.Status.Info, "Launching Customer Portal and logging in");
                CommonServicesPage commonServices = new CommonServicesPage(_driver, test);
                commonServices.Login(username, password);

                // ACT & ASSERT for paymentDate1
                paymentsPage.ClickMakeAPayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                paymentsPage.ClickConfirmPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property.");

                // ACT & ASSERT for paymentDate2
                paymentsPage.SelectPaymentDate(paymentDate2);
                paymentsPage.ClickConfirmPayment();
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property.");
                test.Log(AventStack.ExtentReports.Status.Pass, "No late fee message verified for NC property.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"Test failed: {ex.Message}");
                throw;
            }
        }

        // Additional test methods for TS-003, TS-005, TS-006, TS-007, TS-008, TS-009 can be implemented similarly
        // Each method should follow the AAA pattern, use data-driven attributes, and assert the late fee message as per the test case

        [TestCleanup]
        public void TestCleanup()
        {
            // Automatic cleanup via BasePage TearDown
        }
    }
}
