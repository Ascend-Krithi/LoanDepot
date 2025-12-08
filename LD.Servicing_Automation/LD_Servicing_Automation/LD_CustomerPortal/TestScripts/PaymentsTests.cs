using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using LD_CustomerPortal.TestPages;
using LD_AutomationFramework.Base;
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

        // Data-driven test using MSTest DataSource (TestData/LoanDetails.xml)
        // Example for CUSTP-2005 TS-001 TC-001
        // Test Data XML should have relevant nodes for each test case
        [TestMethod]
        // [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "TestCase", DataAccessMethod.Sequential)]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        // Test Case Mapping: CUSTP-2005 TS-001 TC-001
        public void CUSTP_2005_TS_001_TC_001_ValidateLateFeeForHELOC_TX()
        {
            try
            {
                // ARRANGE
                string url = "https://customerportal.example.com";
                string username = "testuser";
                string password = "Pass@123";
                string helocAccountNumber = "123456789";
                string paymentAmount = "500";
                string dueDate = "6/1/23";
                string paymentDate = "6/17/23";
                string propertyState = "TX";

                test.Log(Status.Info, "Launching Customer Portal: " + url);
                _driver.Navigate().GoToUrl(url);

                // ACT
                test.Log(Status.Info, "Logging in as customer");
                paymentsPage.Login(username, password);
                test.Log(Status.Info, "Selecting HELOC account: " + helocAccountNumber);
                paymentsPage.SelectHelocAccount(helocAccountNumber);
                test.Log(Status.Info, "Navigating to Make a Payment section");
                paymentsPage.NavigateToMakePayment();
                test.Log(Status.Info, "Choosing One-Time Payment option");
                paymentsPage.ChooseOneTimePayment();
                test.Log(Status.Info, $"Entering payment amount: {paymentAmount}, payment date: {paymentDate}");
                paymentsPage.EnterPaymentDetailsAndSubmit(paymentAmount, paymentDate);

                // ASSERT
                test.Log(Status.Info, "Validating late fee message is displayed for TX property state");
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment date exceeding grace period.");
                string lateFeeMsg = paymentsPage.GetLateFeeMessageText();
                test.Log(Status.Info, "Late fee message: " + lateFeeMsg);
                Assert.IsTrue(lateFeeMsg.Contains("Late fee"), "Late fee message should mention 'Late fee'.");
                // Additional assertion for capped fee logic can be added here
                test.Log(Status.Pass, "Test passed: Late fee message displayed and validated.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Additional test methods for other test cases (TS-002 to TS-009) would follow similar structure
        // Each method should be mapped to its testCaseId and use relevant test data from XML
        // Example:
        // [TestMethod]
        // [TestCategory("OneTimePayment")]
        // [Owner("AutomationTestEngineer")]
        // // Test Case Mapping: CUSTP-2005 TS-002 TC-001
        // public void CUSTP_2005_TS_002_TC_001_ValidateLateFeeForHELOC_TX_40DayGrace()
        // {
        //     ...
        // }

        [TestCleanup]
        public void TestCleanup()
        {
            // Automatic cleanup via BasePage TearDown
        }
    }
}
