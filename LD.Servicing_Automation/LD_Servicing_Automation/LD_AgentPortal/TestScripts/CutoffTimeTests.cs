using System;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LD_AgentPortal.Pages;
using LD_AutomationFramework.Pages;
using System.Linq;
using System.Collections.Generic;
using LD_AutomationFramework.Utilities;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using System.Collections;
using LD_AutomationFramework.Config;
using AventStack.ExtentReports;
using System.Reflection;
using LD_CustomerPortal;

namespace LD_AgentPortal.Tests
{
    [TestClass]
    public class CutoffTimeTests : BasePage
    {
        public static string cutOffTime_GetPaymentsSubmittedByServiceAccount = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.CutOffTime_GetPaymentsSubmittedByServiceAccount));
        
        public TestContext TestContext
        {
            set;
            get;
        }

        #region ObjectInitialization

        DashboardPage dashboard = null;
        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;
        DBconnect dBconnect = null;
        Pages.PaymentsPage payments = null;
        ReportLogger reportLogger = null;
        List<Hashtable> loanLevelData = null;
        JiraManager jiraManager = null;
        LD_CustomerPortal.Pages.PaymentsPage cpPayments = null;
        LD_CustomerPortal.Pages.DashboardPage cpDashboard = null;

        #endregion ObjectInitialization

        #region CommonTestData

        string deleteReason = "Test Delete Reason";
        static List<string> columnDataRequired = typeof(Constants.PaymentSetupDataColumns)
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(field => field.FieldType == typeof(string))
                    .Select(field => (string)field.GetValue(null)).ToList();

        #endregion CommonTestData

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeFramework(TestContext);
            dashboard = new DashboardPage(_driver, test);
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            dBconnect = new DBconnect(test, "MelloServETL");
            payments = new Pages.PaymentsPage(_driver, test);
            reportLogger = new ReportLogger(_driver);
            loanLevelData = new List<Hashtable>();
            jiraManager = new JiraManager(test);
            cpPayments = new LD_CustomerPortal.Pages.PaymentsPage(_driver, test);
            cpDashboard = new LD_CustomerPortal.Pages.DashboardPage(_driver, test);
        }

        [TestMethod]
        [Description("<br> Verify One Time Payment status change after Cut Off time <br>")]
        [TestCategory("AP_CutoffTime")]
        public void SCM_1_SCM_VerifyOTPStatusChangeAfterCutOffTime()
        {
            #region TestData

            string loanNumber = string.Empty, confirmationNumber = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            loanLevelData = commonServices.GetLoanDataFromDatabase(cutOffTime_GetPaymentsSubmittedByServiceAccount, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            
            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            for (int row = 0; row < ConfigSettings.NumberOfLoanTestDataRequired; row++)
            {
                loanNumber = loanLevelData[row][Constants.PaymentSetupDataColumns.LoanNumber].ToString();
                confirmationNumber = loanLevelData[row][Constants.PaymentSetupDataColumns.ConfirmationNumber].ToString();
                if (commonServices.LaunchUrlWithLoanNumber(loanLevelData, row, true))
                {
                    payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
                }
            }           
        }

        [TestMethod]
        [Description("<br> Verify autopay Additional Monthly Principal amount after 13th of every month <br>")]
        [TestCategory("AP_CutoffTime")]
        public void SCM_2_SCM_VerifyAutopayAdditionalMonthlyPrincipalAmountAfter13thOfEveryMonth()
        {
            #region TestData

            string borrowerName = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
            
            borrowerName = loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerFirstName].ToString() + " " + loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerLastName].ToString();

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            commonServices.LaunchUrlWithLoanNumber(loanLevelData, 0, true);
            dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
            webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
            webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData[0]);

            //Code to be added            
        }
        
    }
}
