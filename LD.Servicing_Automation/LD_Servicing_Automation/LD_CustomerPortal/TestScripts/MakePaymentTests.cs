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
    public class MakePaymentTests : BasePage
    {
        public TestContext TestContext { get; set; }
        private LoginPage loginPage;
        private DashboardPage dashboardPage;
        private MakePaymentPage paymentPage;
        private WebElementExtensionsPage webElementExtensions;
        private DBconnect dbConnect;

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeFramework(TestContext);
            loginPage = new LoginPage(_driver, test);
            dashboardPage = new DashboardPage(_driver, test);
            paymentPage = new MakePaymentPage(_driver, test);
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            dbConnect = new DBconnect(test, Constants.DBNames.MelloServETL);
        }

        // Maps to testCaseId: TS-001
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\TestCredentials.xml", "Credential", DataAccessMethod.Sequential)]
        public void TS_001_VerifyOneTimePayment_EndToEnd()
        {
            try
            {
                // ARRANGE
                test.Log(AventStack.ExtentReports.Status.Info, "Arranging test data for TS-001");
                string username = TestContext.DataRow["username"].ToString();
                string encryptedPassword = TestContext.DataRow["password"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(encryptedPassword);
                string loanName = TestContext.DataRow["loan_name"].ToString();
                string paymentYear = TestContext.DataRow["payment_year"].ToString();
                string paymentMonth = TestContext.DataRow["payment_month"].ToString();
                string paymentDay = TestContext.DataRow["payment_day"].ToString();

                // ACT
                test.Log(AventStack.ExtentReports.Status.Info, "Step 1: Login");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after successful login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Step 2: Select Loan Account");
                dashboardPage.SelectLoanAccount(loanName);

                test.Log(AventStack.ExtentReports.Status.Info, "Step 3: Click Make Payment");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(paymentPage.IsPaymentScreenDisplayed(), "Make a Payment screen must load.");

                test.Log(AventStack.ExtentReports.Status.Info, "Step 4: Select Payment Date");
                paymentPage.SelectPaymentDate(paymentYear, paymentMonth, paymentDay);

                test.Log(AventStack.ExtentReports.Status.Info, "Step 5: Continue Payment");
                paymentPage.ClickContinue();

                // ASSERT
                test.Log(AventStack.ExtentReports.Status.Info, "Verifying payment modal and late fee message");
                Assert.IsTrue(webElementExtensions.IsElementDisplayed(_driver, paymentPage.scheduledPaymentModalLocBy), "Scheduled Payment Modal should be displayed.");
                Assert.IsFalse(webElementExtensions.IsElementDisplayed(_driver, paymentPage.lateFeeMessageLocBy), "Late Fee message should not be displayed for on-time payment.");

                test.Log(AventStack.ExtentReports.Status.Pass, "TS-001: One-time payment flow completed successfully.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"TS-001 failed: {ex.Message}");
                throw;
            }
        }

        // Maps to testCaseId: TS-002
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\TestCredentials.xml", "Credential", DataAccessMethod.Sequential)]
        public void TS_002_VerifyLateFeeMessageForPastDuePayment()
        {
            try
            {
                // ARRANGE
                test.Log(AventStack.ExtentReports.Status.Info, "Arranging test data for TS-002");
                string username = TestContext.DataRow["username"].ToString();
                string encryptedPassword = TestContext.DataRow["password"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(encryptedPassword);
                string loanName = TestContext.DataRow["loan_name"].ToString();
                string paymentYear = TestContext.DataRow["payment_year"].ToString();
                string paymentMonth = TestContext.DataRow["payment_month"].ToString();
                string paymentDay = TestContext.DataRow["payment_day"].ToString();

                // ACT
                test.Log(AventStack.ExtentReports.Status.Info, "Step 1: Login");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after successful login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Step 2: Select Loan Account");
                dashboardPage.SelectLoanAccount(loanName);

                test.Log(AventStack.ExtentReports.Status.Info, "Step 3: Click Make Payment");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(paymentPage.IsPaymentScreenDisplayed(), "Make a Payment screen must load.");

                test.Log(AventStack.ExtentReports.Status.Info, "Step 4: Select Payment Date");
                paymentPage.SelectPaymentDate(paymentYear, paymentMonth, paymentDay);

                test.Log(AventStack.ExtentReports.Status.Info, "Step 5: Continue Payment");
                paymentPage.ClickContinue();

                // ASSERT
                test.Log(AventStack.ExtentReports.Status.Info, "Verifying payment modal and late fee message");
                Assert.IsTrue(webElementExtensions.IsElementDisplayed(_driver, paymentPage.scheduledPaymentModalLocBy), "Scheduled Payment Modal should be displayed.");
                Assert.IsTrue(webElementExtensions.IsElementDisplayed(_driver, paymentPage.lateFeeMessageLocBy), "Late Fee message should be displayed for past-due payment.");

                test.Log(AventStack.ExtentReports.Status.Pass, "TS-002: Late fee message verified for past-due payment.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"TS-002 failed: {ex.Message}");
                throw;
            }
        }

        // Maps to testCaseId: TS-003
        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("AutomationTestEngineer")]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\LoanDetails.xml", "Loan", DataAccessMethod.Sequential)]
        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\TestData\TestCredentials.xml", "Credential", DataAccessMethod.Sequential)]
        public void TS_003_VerifyPaymentDateSelection()
        {
            try
            {
                // ARRANGE
                test.Log(AventStack.ExtentReports.Status.Info, "Arranging test data for TS-003");
                string username = TestContext.DataRow["username"].ToString();
                string encryptedPassword = TestContext.DataRow["password"].ToString();
                string password = new EncryptionManager(test).DecryptDataWithAes(encryptedPassword);
                string loanName = TestContext.DataRow["loan_name"].ToString();
                string paymentYear = TestContext.DataRow["payment_year"].ToString();
                string paymentMonth = TestContext.DataRow["payment_month"].ToString();
                string paymentDay = TestContext.DataRow["payment_day"].ToString();

                // ACT
                test.Log(AventStack.ExtentReports.Status.Info, "Step 1: Login");
                loginPage.Login(username, password);
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard must be displayed after successful login.");

                test.Log(AventStack.ExtentReports.Status.Info, "Step 2: Select Loan Account");
                dashboardPage.SelectLoanAccount(loanName);

                test.Log(AventStack.ExtentReports.Status.Info, "Step 3: Click Make Payment");
                dashboardPage.ClickMakePayment();
                Assert.IsTrue(paymentPage.IsPaymentScreenDisplayed(), "Make a Payment screen must load.");

                test.Log(AventStack.ExtentReports.Status.Info, "Step 4: Select Payment Date");
                paymentPage.SelectPaymentDate(paymentYear, paymentMonth, paymentDay);

                // ASSERT
                test.Log(AventStack.ExtentReports.Status.Info, "Verifying payment date selection");
                Assert.IsTrue(webElementExtensions.IsElementDisplayed(_driver, paymentPage.calendarOverlayLocBy), "Calendar overlay should be displayed after date picker toggle.");
                test.Log(AventStack.ExtentReports.Status.Pass, "TS-003: Payment date selection verified.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"TS-003 failed: {ex.Message}");
                throw;
            }
        }

        // ... Repeat for TS-004 through TS-009 with full method bodies, AAA pattern, logging, assertions, and data-driven attributes as above ...
    }
}
