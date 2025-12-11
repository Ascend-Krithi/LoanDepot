using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Utilities;
using System;
using System.Collections.Generic;

namespace AutomationFramework.Tests.TestPages
{
    [TestClass]
    public class LoanFlowTests : BaseTest
    {
        [TestMethod]
        public void LoanSelectionAndPaymentTest()
        {
            var loginPage = new LoginPage(Driver);
            var dashboardPage = new DashboardPage(Driver);
            var paymentPage = new PaymentPage(Driver);

            // Test data from Excel
            var excel = new ExcelReader("TestData/LoanData.xlsx");
            Dictionary<string, string> data = excel.GetRowByTestCaseId("Sheet1", "TC_001");

            var stepStart = DateTime.Now;
            loginPage.EnterUsername(data["Username"]);
            Report.AddStep("Enter Username", DateTime.Now - stepStart);

            stepStart = DateTime.Now;
            loginPage.EnterPassword(data["Password"]);
            Report.AddStep("Enter Password", DateTime.Now - stepStart);

            stepStart = DateTime.Now;
            loginPage.ClickLogin();
            Report.AddStep("Click Login", DateTime.Now - stepStart);

            stepStart = DateTime.Now;
            dashboardPage.SelectLoanType(data["LoanType"]);
            Report.AddStep("Select Loan Type", DateTime.Now - stepStart);

            stepStart = DateTime.Now;
            dashboardPage.SelectLoanFromList(data["LoanName"]);
            Report.AddStep("Select Loan From List", DateTime.Now - stepStart);

            stepStart = DateTime.Now;
            dashboardPage.DismissPopup();
            Report.AddStep("Dismiss Popup", DateTime.Now - stepStart);

            stepStart = DateTime.Now;
            dashboardPage.HandleChatPopupIfPresent();
            Report.AddStep("Handle Chat Popup", DateTime.Now - stepStart);

            stepStart = DateTime.Now;
            dashboardPage.SelectDate(data["PaymentDate"]);
            Report.AddStep("Select Date", DateTime.Now - stepStart);

            stepStart = DateTime.Now;
            paymentPage.EnterAmount(data["Amount"]);
            Report.AddStep("Enter Amount", DateTime.Now - stepStart);

            stepStart = DateTime.Now;
            paymentPage.ClickPay();
            Report.AddStep("Click Pay", DateTime.Now - stepStart);

            stepStart = DateTime.Now;
            var message = dashboardPage.GetMessageBanner();
            Report.AddStep("Validate Message", DateTime.Now - stepStart);

            Assert.AreEqual(data["ExpectedMessage"], message, "Message validation failed.");
        }
    }
}