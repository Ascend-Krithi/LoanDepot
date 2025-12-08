using AventStack.ExtentReports;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_CustomerPortal.TestPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
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

        [TestMethod]
        [TestCategory("OneTimePayment")]
        [Owner("Automation Test Engineer")]
        public void CUSTP_2005_OneTimePayment_LateFeeValidation_TX()
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

                test.Log(Status.Info, "Launching Customer Portal application");
                _driver.Navigate().GoToUrl(url);

                // ACT
                test.Log(Status.Info, "Logging in as customer");
                CommonServicesPage commonServices = new CommonServicesPage(_driver, test);
                commonServices.Login(username, password);

                test.Log(Status.Info, "Navigating to 'Make a Payment' section");
                paymentsPage.NavigateToMakePayment();

                test.Log(Status.Info, "Selecting HELOC account and choosing 'One-Time Payment'");
                paymentsPage.SelectHELOCAccount(helocAccountNumber);
                paymentsPage.ChooseOneTimePayment();

                test.Log(Status.Info, "Entering payment amount and selecting payment date");
                paymentsPage.EnterPaymentAmount(paymentAmount);
                paymentsPage.SelectPaymentDate(paymentDate);

                test.Log(Status.Info, "Submitting payment");
                paymentsPage.SubmitPayment();

                // ASSERT
                test.Log(Status.Info, "Validating late fee message display and fee amount");
                Assert.IsTrue(paymentsPage.IsLateFeeMessageDisplayed(), "Late fee message should be displayed for TX property state exceeding grace period.");
                string lateFeeMsg = paymentsPage.GetLateFeeMessageText();
                Assert.IsTrue(lateFeeMsg.Contains("late fee"), "Late fee message should mention late fee.");
                // Additional assertion for capped fee amount can be added based on business rules
                test.Log(Status.Pass, "Late fee message validated successfully.");
            }
            catch (Exception ex)
            {
                test.Log(Status.Fail, "Test failed: " + ex.Message);
                throw;
            }
        }

        // Additional test methods for other scenarios (NC state, grace period boundary, etc.)
        // Each test should follow AAA pattern and use PaymentsPage methods

        [TestCleanup]
        public void TestCleanup()
        {
            // Automatic cleanup via BasePage TearDown
        }
    }
}
