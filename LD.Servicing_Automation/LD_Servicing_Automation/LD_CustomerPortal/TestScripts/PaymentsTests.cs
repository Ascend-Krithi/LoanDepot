using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using LD_CustomerPortal.TestPages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using AventStack.ExtentReports;
using System;

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
            InitializeFramework(TestContext);
            paymentsPage = new PaymentsPage(_driver, test);
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            dbConnect = new DBconnect(test, Constants.DBNames.MelloServETL);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Automatic cleanup via BasePage TearDown
        }

        // CUSTP-2005 TS-001 TC-001
        // Test Case Mapping: Customer Portal - Late Fee Message for Payment After 15-Day Grace Period (Non-NC Property)
        [TestMethod]
        [TestCategory("Payments")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_001_TC_001_LateFeeMessageAfter15DayGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string paymentDate = "6/17/23";
                string dueDate = "6/1/23";
                string propertyState = "TX";
                test.Log(Status.Info, $"Test Data - Username: {username}, Payment Date: {paymentDate}, Property State: {propertyState}");

                // ACT
                paymentsPage.ClickMakeAPayment();
                paymentsPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 15-day grace period.");
                string lateFeeMsg = paymentsPage.GetLateFeeMessageText();
                Assert.IsTrue(lateFeeMsg.Contains("past the 15-day grace period"), "Late fee message should indicate payment is past the 15-day grace period.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-002 TC-001
        // Test Case Mapping: Customer Portal - Late Fee Message for Payment After 40-Day Grace Period (Non-NC Property)
        [TestMethod]
        [TestCategory("Payments")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_002_TC_001_LateFeeMessageAfter40DayGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string paymentDate = "7/12/23";
                string dueDate = "6/1/23";
                string propertyState = "TX";
                test.Log(Status.Info, $"Test Data - Username: {username}, Payment Date: {paymentDate}, Property State: {propertyState}");

                // ACT
                paymentsPage.ClickMakeAPayment();
                paymentsPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 40-day grace period.");
                string lateFeeMsg = paymentsPage.GetLateFeeMessageText();
                Assert.IsTrue(lateFeeMsg.Contains("past the 40-day grace period"), "Late fee message should indicate payment is past the 40-day grace period.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-003 TC-001
        // Test Case Mapping: Customer Portal - Late Fee Message for Payment After 40-Day Grace Period (Non-NC Property, July)
        [TestMethod]
        [TestCategory("Payments")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_003_TC_001_LateFeeMessageAfter40DayGracePeriodJuly()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string paymentDate = "8/11/23";
                string dueDate = "7/1/23";
                string propertyState = "TX";
                test.Log(Status.Info, $"Test Data - Username: {username}, Payment Date: {paymentDate}, Property State: {propertyState}");

                // ACT
                paymentsPage.ClickMakeAPayment();
                paymentsPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 40-day grace period (July).");
                string lateFeeMsg = paymentsPage.GetLateFeeMessageText();
                Assert.IsTrue(lateFeeMsg.Contains("past the 40-day grace period"), "Late fee message should indicate payment is past the 40-day grace period.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-004 TC-001
        // Test Case Mapping: Customer Portal - No Late Fee Message for Payment (NC Property)
        [TestMethod]
        [TestCategory("Payments")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_004_TC_001_NoLateFeeMessageNCProperty()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string paymentDate1 = "6/17/23";
                string paymentDate2 = "7/12/23";
                string dueDate = "6/1/23";
                string propertyState = "NC";
                test.Log(Status.Info, $"Test Data - Username: {username}, Payment Dates: {paymentDate1}, {paymentDate2}, Property State: {propertyState}");

                // ACT & ASSERT for paymentDate1
                paymentsPage.ClickMakeAPayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property (paymentDate1).");
                test.Log(Status.Pass, "Test passed: No late fee message for NC property (paymentDate1).");

                // ACT & ASSERT for paymentDate2
                paymentsPage.SelectPaymentDate(paymentDate2);
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property (paymentDate2).");
                test.Log(Status.Pass, "Test passed: No late fee message for NC property (paymentDate2).");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-005 TC-001
        // Test Case Mapping: Customer Portal - No Late Fee Message for Payment Within 15-Day Grace Period (Non-NC Property)
        [TestMethod]
        [TestCategory("Payments")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_005_TC_001_NoLateFeeMessageWithin15DayGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string paymentDate = "6/16/23";
                string dueDate = "6/1/23";
                string propertyState = "TX";
                test.Log(Status.Info, $"Test Data - Username: {username}, Payment Date: {paymentDate}, Property State: {propertyState}");

                // ACT
                paymentsPage.ClickMakeAPayment();
                paymentsPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for payment within 15-day grace period.");
                test.Log(Status.Pass, "Test passed: No late fee message for payment within 15-day grace period.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-006 TC-001
        // Test Case Mapping: Customer Portal - Late Fee Message for Payment After Grace Period (Non-NC Property)
        [TestMethod]
        [TestCategory("Payments")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_006_TC_001_LateFeeMessageAfterGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string paymentDate1 = "6/16/23";
                string paymentDate2 = "6/17/23";
                string dueDate = "6/1/23";
                string propertyState = "TX";
                test.Log(Status.Info, $"Test Data - Username: {username}, Payment Dates: {paymentDate1}, {paymentDate2}, Property State: {propertyState}");

                // ACT & ASSERT for paymentDate1
                paymentsPage.ClickMakeAPayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for last day of grace period.");
                test.Log(Status.Pass, "Test passed: No late fee message for last day of grace period.");

                // ACT & ASSERT for paymentDate2
                paymentsPage.SelectPaymentDate(paymentDate2);
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for first day after grace period.");
                string lateFeeMsg = paymentsPage.GetLateFeeMessageText();
                Assert.IsTrue(lateFeeMsg.Contains("past the grace period"), "Late fee message should indicate payment is past the grace period.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed for first day after grace period.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-007 TC-001
        // Test Case Mapping: Customer Portal - Late Fee Message for Payment After 40-Day Grace Period (Non-NC Property, July)
        [TestMethod]
        [TestCategory("Payments")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_007_TC_001_LateFeeMessageAfter40DayGracePeriodJuly()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string paymentDate1 = "7/11/23";
                string paymentDate2 = "7/12/23";
                string dueDate = "6/1/23";
                string propertyState = "TX";
                test.Log(Status.Info, $"Test Data - Username: {username}, Payment Dates: {paymentDate1}, {paymentDate2}, Property State: {propertyState}");

                // ACT & ASSERT for paymentDate1
                paymentsPage.ClickMakeAPayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for last day of 40-day grace period.");
                test.Log(Status.Pass, "Test passed: No late fee message for last day of 40-day grace period.");

                // ACT & ASSERT for paymentDate2
                paymentsPage.SelectPaymentDate(paymentDate2);
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for first day after 40-day grace period.");
                string lateFeeMsg = paymentsPage.GetLateFeeMessageText();
                Assert.IsTrue(lateFeeMsg.Contains("past the 40-day grace period"), "Late fee message should indicate payment is past the 40-day grace period.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed for first day after 40-day grace period.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-008 TC-001
        // Test Case Mapping: Customer Portal - Late Fee Message for Payment After 40-Day Grace Period (Leap Year)
        [TestMethod]
        [TestCategory("Payments")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_008_TC_001_LateFeeMessageAfter40DayGracePeriodLeapYear()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string paymentDate = "3/12/24";
                string dueDate = "2/1/24";
                string propertyState = "TX";
                test.Log(Status.Info, $"Test Data - Username: {username}, Payment Date: {paymentDate}, Property State: {propertyState}");

                // ACT
                paymentsPage.ClickMakeAPayment();
                paymentsPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after 40-day grace period (leap year).");
                string lateFeeMsg = paymentsPage.GetLateFeeMessageText();
                Assert.IsTrue(lateFeeMsg.Contains("past the 40-day grace period"), "Late fee message should indicate payment is past the 40-day grace period.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed as expected (leap year).");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-009 TC-001
        // Test Case Mapping: Customer Portal - Late Fee Message for Payment After Grace Period (Button Enabled)
        [TestMethod]
        [TestCategory("Payments")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_009_TC_001_LateFeeMessageAfterGracePeriodButtonEnabled()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string paymentDate = "6/17/23";
                string dueDate = "6/1/23";
                test.Log(Status.Info, $"Test Data - Username: {username}, Payment Date: {paymentDate}");

                // ACT
                // Verify 'Make a Payment' button is enabled (purple)
                bool isEnabled = webElementExtensions.IsElementEnabled(_driver, paymentsPage.makeAPaymentBtnBy);
                Assert.IsTrue(isEnabled, "'Make a Payment' button should be enabled (purple).");
                test.Log(Status.Pass, "'Make a Payment' button is enabled (purple).");

                paymentsPage.ClickMakeAPayment();
                paymentsPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after grace period.");
                string lateFeeMsg = paymentsPage.GetLateFeeMessageText();
                Assert.IsTrue(lateFeeMsg.Contains("past the grace period"), "Late fee message should indicate payment is past the grace period.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed for payment after grace period.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }
    }
}
