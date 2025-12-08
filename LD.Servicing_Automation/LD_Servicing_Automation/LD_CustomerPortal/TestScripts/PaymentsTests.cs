using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using AventStack.ExtentReports;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_CustomerPortal.TestPages;
using System;

/// <summary>
/// PaymentsTests - MSTest class for 'Make a Payment' scenarios.
/// Strictly follows KB skeleton and implements data-driven testing using LoanDetails.xml and TestCredentials.xml.
/// </summary>
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
        // Initialize framework and page objects
        InitializeFramework(TestContext);
        paymentsPage = new PaymentsPage(_driver, test);
        webElementExtensions = new WebElementExtensionsPage(_driver, test);
        dbConnect = new DBconnect(test, Constants.DBNames.MelloServETL);
    }

    /// <summary>
    /// Test Case Mapping: CUSTP-2005 TS-001 TC-001
    /// Data-driven test using LoanDetails.xml and TestCredentials.xml
    /// </summary>
    [TestMethod]
    [TestCategory("OneTimePayment")]
    [Owner("AutomationTestEngineer")]
    // Test Case ID: CUSTP-2005 TS-001 TC-001
    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestCredentials", DataAccessMethod.Sequential)]
    public void CUSTP_2005_TS_001_TC_001_ValidateLateFeeForTXState()
    {
        try
        {
            // ARRANGE
            string url = "https://customerportal.example.com";
            string username = TestContext.DataRow["username"].ToString();
            string encryptedPassword = TestContext.DataRow["password"].ToString();
            string password = new EncryptionManager(test).DecryptDataWithAes(encryptedPassword);
            string loanNumber = TestContext.DataRow["loan_number"].ToString();
            string paymentAmount = "500";
            string paymentDate = "6/17/23";
            string propertyState = "TX";

            test.Log(AventStack.ExtentReports.Status.Info, $"Launching Customer Portal: {url}");
            _driver.Navigate().GoToUrl(url);

            // ACT
            CommonServicesPage commonServices = new CommonServicesPage(_driver, test);
            commonServices.Login(username, password);
            test.Log(AventStack.ExtentReports.Status.Info, "Logged in as customer.");

            paymentsPage.ClickMakeAPayment();
            paymentsPage.EnterPaymentAmount(paymentAmount);
            paymentsPage.SelectPaymentDate(paymentDate);
            paymentsPage.ClickConfirmPayment();

            // ASSERT
            Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for TX state exceeding grace period.");
            string lateFeeMsg = paymentsPage.GetLateFeeMessageText();
            test.Log(AventStack.ExtentReports.Status.Info, $"Late fee message: {lateFeeMsg}");
            Assert.IsTrue(lateFeeMsg.Contains("late fee"), "Late fee message should mention late fee.");
            test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: Late fee message displayed and validated.");
        }
        catch (Exception ex)
        {
            test.Log(AventStack.ExtentReports.Status.Fail, $"Test failed: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Test Case Mapping: CUSTP-2005 TS-004 TC-001
    /// Data-driven test for NC state (no late fee)
    /// </summary>
    [TestMethod]
    [TestCategory("OneTimePayment")]
    [Owner("AutomationTestEngineer")]
    // Test Case ID: CUSTP-2005 TS-004 TC-001
    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\LoanDetails.xml", "LoanDetails", DataAccessMethod.Sequential)]
    [DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML", "..\\..\\TestData\\TestCredentials.xml", "TestCredentials", DataAccessMethod.Sequential)]
    public void CUSTP_2005_TS_004_TC_001_ValidateNoLateFeeForNCState()
    {
        try
        {
            // ARRANGE
            string url = "https://customerportal.example.com";
            string username = TestContext.DataRow["username"].ToString();
            string encryptedPassword = TestContext.DataRow["password"].ToString();
            string password = new EncryptionManager(test).DecryptDataWithAes(encryptedPassword);
            string loanNumber = TestContext.DataRow["loan_number"].ToString();
            string paymentAmount = "500";
            string paymentDate = "6/17/23";
            string propertyState = "NC";

            test.Log(AventStack.ExtentReports.Status.Info, $"Launching Customer Portal: {url}");
            _driver.Navigate().GoToUrl(url);

            // ACT
            CommonServicesPage commonServices = new CommonServicesPage(_driver, test);
            commonServices.Login(username, password);
            test.Log(AventStack.ExtentReports.Status.Info, "Logged in as customer.");

            paymentsPage.ClickMakeAPayment();
            paymentsPage.EnterPaymentAmount(paymentAmount);
            paymentsPage.SelectPaymentDate(paymentDate);
            paymentsPage.ClickConfirmPayment();

            // ASSERT
            Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should NOT be displayed for NC state.");
            test.Log(AventStack.ExtentReports.Status.Pass, "Test passed: No late fee message displayed for NC state.");
        }
        catch (Exception ex)
        {
            test.Log(AventStack.ExtentReports.Status.Fail, $"Test failed: {ex.Message}");
            throw;
        }
    }

    [TestCleanup]
    public void TestCleanup()
    {
        // Automatic cleanup via base class
    }
}
