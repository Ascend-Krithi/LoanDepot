using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using LD_CustomerPortal.TestPages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using AventStack.ExtentReports;
using System;

namespace LD_CustomerPortal.TestScripts
{
    [TestClass]
    public class AutomationTests : BasePage
    {
        public TestContext TestContext { get; set; }
        private IWebDriver driver;
        private HomePage homePage;
        private DashboardPage dashboardPage;
        private PaymentsPage paymentsPage;
        private WebElementExtensionsPage webElementExtensions;
        private DBconnect dbConnect;

        [TestInitialize]
        public void Setup()
        {
            InitializeFramework(TestContext);
            driver = _driver;
            homePage = new HomePage(driver, test);
            dashboardPage = new DashboardPage(driver, test);
            paymentsPage = new PaymentsPage(driver, test);
            webElementExtensions = new WebElementExtensionsPage(driver, test);
            dbConnect = new DBconnect(test, Constants.DBNames.MelloServETL);
        }

        [TestMethod]
        [TestCategory("LoginFlow")]
        [Owner("AutomationTestEngineer")]
        public void CUSTP_2005_LoginAndDashboard()
        {
            try
            {
                // ARRANGE
                string url = "https://customerportal.example.com";
                string username = "testuser";
                string password = "Pass@123";
                string helocAccountNumber = "123456789";
                driver.Navigate().GoToUrl(url);
                test.Log(AventStack.ExtentReports.Status.Info, "Navigated to Customer Portal.");

                // ACT
                homePage.EnterUsername(username);
                homePage.EnterPassword(password);
                homePage.ClickLoginButton();
                test.Log(AventStack.ExtentReports.Status.Info, "Performed login.");

                // ASSERT
                Assert.IsTrue(dashboardPage.IsDashboardDisplayed(), "Dashboard should be displayed.");
                Assert.IsTrue(dashboardPage.IsHELOCAccountVisible(helocAccountNumber), "HELOC account should be visible on dashboard.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Login and dashboard verification passed.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"Test failed: {ex.Message}");
                throw;
            }
        }

        [TestMethod]
        [TestCategory("PaymentsFlow")]
        [Owner("AutomationTestEngineer")]
        public void CUSTP_2005_MakePayment_LateFeeValidation_TX()
        {
            try
            {
                // ARRANGE
                string url = "https://customerportal.example.com";
                string username = "testuser";
                string password = "Pass@123";
                string helocAccountNumber = "123456789";
                string paymentAmount = "500";
                string paymentDate = "6/17/23";
                string dueDate = "6/1/23";
                string propertyState = "TX";
                driver.Navigate().GoToUrl(url);
                homePage.EnterUsername(username);
                homePage.EnterPassword(password);
                homePage.ClickLoginButton();
                dashboardPage.SelectHELOCAccount(helocAccountNumber);
                dashboardPage.ClickMakePayment();
                paymentsPage.ClickOneTimePayment();
                paymentsPage.EnterPaymentAmount(paymentAmount);
                paymentsPage.EnterPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for TX property state exceeding grace period.");
                string lateFeeMsg = paymentsPage.GetLateFeeMessage();
                Assert.IsTrue(lateFeeMsg.Contains("late fee"), "Late fee message should mention late fee.");
                test.Log(AventStack.ExtentReports.Status.Pass, "Late fee validation for TX passed.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"Test failed: {ex.Message}");
                throw;
            }
        }

        [TestMethod]
        [TestCategory("PaymentsFlow")]
        [Owner("AutomationTestEngineer")]
        public void CUSTP_2005_MakePayment_NoLateFee_NC()
        {
            try
            {
                // ARRANGE
                string url = "https://customerportal.example.com";
                string username = "testuser";
                string password = "Pass@123";
                string helocAccountNumber = "123456789";
                string paymentAmount = "500";
                string paymentDate = "6/17/23";
                string propertyState = "NC";
                driver.Navigate().GoToUrl(url);
                homePage.EnterUsername(username);
                homePage.EnterPassword(password);
                homePage.ClickLoginButton();
                dashboardPage.SelectHELOCAccount(helocAccountNumber);
                dashboardPage.ClickMakePayment();
                paymentsPage.ClickOneTimePayment();
                paymentsPage.EnterPaymentAmount(paymentAmount);
                paymentsPage.EnterPaymentDate(paymentDate);
                paymentsPage.SubmitPayment();

                // ASSERT
                Assert.IsFalse(paymentsPage.IsLateFeeMessageDisplayed(), "No late fee message should be displayed for NC property state.");
                test.Log(AventStack.ExtentReports.Status.Pass, "No late fee validation for NC passed.");
            }
            catch (Exception ex)
            {
                test.Log(AventStack.ExtentReports.Status.Fail, $"Test failed: {ex.Message}");
                throw;
            }
        }

        [TestCleanup]
        public void Teardown()
        {
            TearDown();
        }
    }
}
