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

        // CUSTP-2005 TS-001 TC-001
        // Customer makes a payment 16 days after due date for property state other than NC
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Query_GetLoanLevelDetailsForEligibleOTPOntime", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_001_TC_001_MakePayment_16DaysAfterDueDate_OtherThanNC()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/17/23";
                string dueDate = "6/1/23";
                test.Log(Status.Info, "Test Data: Username=" + username + ", PropertyState=" + propertyState + ", PaymentDate=" + paymentDate);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after grace period.");
                test.Log(Status.Pass, "Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-002 TC-001
        // Customer makes a payment 41 days after due date for property state other than NC (June)
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Query_GetLoanLevelDetailsForEligibleOTPOntime", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_002_TC_001_MakePayment_41DaysAfterDueDate_OtherThanNC_June()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "7/12/23";
                string dueDate = "6/1/23";
                test.Log(Status.Info, "Test Data: Username=" + username + ", PropertyState=" + propertyState + ", PaymentDate=" + paymentDate);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment past 40-day grace period.");
                test.Log(Status.Pass, "Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-003 TC-001
        // Customer makes a payment 41 days after due date for property state other than NC (July)
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Query_GetLoanLevelDetailsForEligibleOTPOntime", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_003_TC_001_MakePayment_41DaysAfterDueDate_OtherThanNC_July()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "8/11/23";
                string dueDate = "7/1/23";
                test.Log(Status.Info, "Test Data: Username=" + username + ", PropertyState=" + propertyState + ", PaymentDate=" + paymentDate);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment past 40-day grace period.");
                test.Log(Status.Pass, "Late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-004 TC-001
        // Customer makes payments for property state NC, no late fee message
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Query_GetLoanLevelDetailsForEligibleOTPOntime", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_004_TC_001_MakePayments_NC_NoLateFee()
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
                test.Log(Status.Info, "Test Data: Username=" + username + ", PropertyState=" + propertyState + ", PaymentDate1=" + paymentDate1 + ", PaymentDate2=" + paymentDate2);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                Assert.IsTrue(paymentsPage.IsNoLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property state (paymentDate1).");
                paymentsPage.SelectPaymentDate(paymentDate2);
                Assert.IsTrue(paymentsPage.IsNoLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property state (paymentDate2).");
                test.Log(Status.Pass, "No late fee message displayed for both payment dates as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-005 TC-001
        // Customer makes a payment within 15-day grace period for property state other than NC
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Query_GetLoanLevelDetailsForEligibleOTPOntime", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_005_TC_001_MakePayment_WithinGracePeriod_OtherThanNC()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/16/23";
                string dueDate = "6/1/23";
                test.Log(Status.Info, "Test Data: Username=" + username + ", PropertyState=" + propertyState + ", PaymentDate=" + paymentDate);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsTrue(paymentsPage.IsNoLateFeeMessageDisplayed(), "No late fee message should be displayed for payment within grace period.");
                test.Log(Status.Pass, "No late fee message displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-006 TC-001
        // Customer makes payments on last day of grace period and first day after grace period for property state other than NC
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Query_GetLoanLevelDetailsForEligibleOTPOntime", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_006_TC_001_MakePayments_LastDayAndFirstDayAfterGracePeriod_OtherThanNC()
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
                test.Log(Status.Info, "Test Data: Username=" + username + ", PropertyState=" + propertyState + ", PaymentDate1=" + paymentDate1 + ", PaymentDate2=" + paymentDate2);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                Assert.IsTrue(paymentsPage.IsNoLateFeeMessageDisplayed(), "No late fee message should be displayed for last day of grace period.");
                paymentsPage.SelectPaymentDate(paymentDate2);
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for first day after grace period.");
                test.Log(Status.Pass, "Grace period boundary messages displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-007 TC-001
        // Customer makes payments on last day of 40-day grace period and first day after grace period for property state other than NC
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Query_GetLoanLevelDetailsForEligibleOTPOntime", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_007_TC_001_MakePayments_LastDayAndFirstDayAfter40DayGracePeriod_OtherThanNC()
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
                test.Log(Status.Info, "Test Data: Username=" + username + ", PropertyState=" + propertyState + ", PaymentDate1=" + paymentDate1 + ", PaymentDate2=" + paymentDate2);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate1);
                Assert.IsTrue(paymentsPage.IsNoLateFeeMessageDisplayed(), "No late fee message should be displayed for last day of 40-day grace period.");
                paymentsPage.SelectPaymentDate(paymentDate2);
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for first day after 40-day grace period.");
                test.Log(Status.Pass, "40-day grace period boundary messages displayed as expected.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-008 TC-001
        // Customer makes a payment 40 days after due date for property state other than NC (leap year)
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Query_GetLoanLevelDetailsForEligibleOTPOntime", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_008_TC_001_MakePayment_40DaysAfterDueDate_OtherThanNC_LeapYear()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "3/12/24";
                string dueDate = "2/1/24";
                test.Log(Status.Info, "Test Data: Username=" + username + ", PropertyState=" + propertyState + ", PaymentDate=" + paymentDate);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToLoan(propertyState);
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment past 40-day grace period (leap year).");
                test.Log(Status.Pass, "Late fee message displayed as expected for leap year.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // CUSTP-2005 TS-009 TC-001
        // Customer verifies 'Make a Payment' button is enabled and makes a payment after grace period
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\TestCredentials.xml", "TestMethodName", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\TestData\\LoanDetails.xml", "Query_GetLoanLevelDetailsForEligibleOTPOntime", DataAccessMethod.Sequential)]
        public void CUSTP_2005_TS_009_TC_001_VerifyMakePaymentButtonEnabled_And_MakePayment_AfterGracePeriod()
        {
            try
            {
                // ARRANGE
                string username = TestContext.DataRow["username"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(TestContext.DataRow["password"].ToString());
                string propertyState = "TX";
                string paymentDate = "6/17/23";
                string dueDate = "6/1/23";
                test.Log(Status.Info, "Test Data: Username=" + username + ", PropertyState=" + propertyState + ", PaymentDate=" + paymentDate);

                // ACT
                loginPage.Login(username, password);
                dashboardPage.NavigateToLoan(propertyState);
                // Verify Make a Payment button is enabled (purple)
                Assert.IsTrue(webElementExtensions.IsElementEnabled(_driver, dashboardPage.makePaymentButtonLocBy), "Make a Payment button should be enabled (purple).");
                dashboardPage.ClickMakePayment();
                paymentsPage.SelectPaymentDate(paymentDate);

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for payment after grace period.");
                test.Log(Status.Pass, "Make a Payment button enabled and late fee message displayed as expected.");
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
