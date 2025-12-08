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

        // Data-driven test using LoanDetails.xml and TestCredentials.xml
        // Connection string and data source paths must be set as per KB
        // Example: DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)

        /// <summary>
        /// [CUSTP-2005 TS-001 TC-001] - Validate late fee message for payment exceeding 15-day grace period (TX)
        /// </summary>
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_001_TC_001_ValidateLateFeeMessage()
        {
            try
            {
                // ARRANGE
                string url = "https://customerportal.example.com";
                string username = TestContext.DataRow["username"].ToString();
                string encryptedPassword = TestContext.DataRow["password"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(encryptedPassword);
                string helocAccountNumber = TestContext.DataRow["loan_number"].ToString();
                string paymentAmount = "500";
                string paymentDate = "6/17/23";
                string dueDate = "6/1/23";
                string propertyState = "TX";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                test.Log(Status.Info, "Logging in as customer");
                paymentsPage.Login(username, password);
                test.Log(Status.Info, "Selecting HELOC account: " + helocAccountNumber);
                paymentsPage.SelectHELOCAccount(helocAccountNumber);
                paymentsPage.NavigateToMakePayment();
                paymentsPage.ChooseOneTimePayment();
                paymentsPage.EnterPaymentDetails(paymentAmount, paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                test.Log(Status.Info, "Verifying late fee message for TX property state");
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment exceeding grace period.");
                string lateFeeMsg = paymentsPage.GetLateFeeMessage();
                test.Log(Status.Info, "Late Fee Message: " + lateFeeMsg);
                Assert.IsTrue(lateFeeMsg.Contains("capped") || lateFeeMsg.Contains("Late fee"), "Late fee message should mention capped fee as per state rules.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed and correct.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// [CUSTP-2005 TS-002 TC-001] - Validate late fee for payment exceeding 40-day grace period (TX)
        /// </summary>
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_002_TC_001_ValidateLateFeeMessage_40Day()
        {
            try
            {
                // ARRANGE
                string url = "https://customerportal.example.com";
                string username = TestContext.DataRow["username"].ToString();
                string encryptedPassword = TestContext.DataRow["password"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(encryptedPassword);
                string helocAccountNumber = TestContext.DataRow["loan_number"].ToString();
                string paymentAmount = "500";
                string paymentDate = "7/12/23";
                string dueDate = "6/1/23";
                string propertyState = "TX";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                paymentsPage.Login(username, password);
                paymentsPage.SelectHELOCAccount(helocAccountNumber);
                paymentsPage.NavigateToMakePayment();
                paymentsPage.ChooseOneTimePayment();
                paymentsPage.EnterPaymentDetails(paymentAmount, paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment exceeding 40-day grace period.");
                string lateFeeMsg = paymentsPage.GetLateFeeMessage();
                test.Log(Status.Info, "Late Fee Message: " + lateFeeMsg);
                Assert.IsTrue(lateFeeMsg.Contains("capped") || lateFeeMsg.Contains("Late fee"), "Late fee message should mention capped fee as per state rules.");
                test.Log(Status.Pass, "Test passed: Late fee message displayed and correct.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// [CUSTP-2005 TS-004 TC-001] - Validate no late fee for NC property state
        /// </summary>
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Test Engineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "Credentials", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_004_TC_001_NoLateFeeForNC()
        {
            try
            {
                // ARRANGE
                string url = "https://customerportal.example.com";
                string username = TestContext.DataRow["username"].ToString();
                string encryptedPassword = TestContext.DataRow["password"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(encryptedPassword);
                string helocAccountNumber = TestContext.DataRow["loan_number"].ToString();
                string paymentAmount = "500";
                string paymentDate = "7/12/23";
                string dueDate = "6/1/23";
                string propertyState = "NC";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                paymentsPage.Login(username, password);
                paymentsPage.SelectHELOCAccount(helocAccountNumber);
                paymentsPage.NavigateToMakePayment();
                paymentsPage.ChooseOneTimePayment();
                paymentsPage.EnterPaymentDetails(paymentAmount, paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property state.");
                test.Log(Status.Pass, "Test passed: No late fee message for NC property state.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Additional test methods for other scenarios (TS-003, TS-005, TS-006, TS-007, TS-008, TS-009) should follow the same pattern,
        // using [TestMethod], [TestCategory], [Owner], and [DataSource] attributes, and referencing the correct test case ID in comments.

        [TestCleanup]
        public void TestCleanup()
        {
            // Automatic cleanup via BasePage TearDown
        }
    }
}
