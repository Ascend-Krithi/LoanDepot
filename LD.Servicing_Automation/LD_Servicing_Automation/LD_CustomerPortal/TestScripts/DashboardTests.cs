using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_CustomerPortal.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace LD_CustomerPortal.Tests
{


    [TestClass]
    public class DashboardTests : BasePage
    {
        //Autopay Ineligible 
        string loanDetailsForFmAutopayInEligibleProcessStopCode = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForFmAutopayInEligibleProcessStopCodeAutopay));
        string loanDetailsForFmAutopayInEligibleANDPifStopCodeAutopay = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForFmAutopayInEligibleANDPifStopCodeAutopay));
        string loanDetailsForFmAutopayInEligiblebadCheckStopCodeAutopay = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForFmAutopayInEligibleBadCheckStopCodeAutopay));
        string loanDetailsForFmAutopayInEligibleForeclosureStopCodeAutopay = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForFmAutopayInEligibleForeclosureStopCodeAutopay));
        string loanDetailsQueryForEligibleHelocAutopayPastDue = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayPastDue));
        string loanDetailsQueryForIneligibleForeclosureStopcodes = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.HelocIneligibleAutopayForeclosureStopCodes));
        string loanDetailsQueryForIneligibleProcessStopcodes = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.HelocIneligibleAutopayProcessStopCodes));
        string loanDetailsQueryForIneligibleBadCheckStopcodes = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.HelocIneligibleAutopayBadCheckStopCodes));
        string loanDetailsQueryForIneligiblePIFStopcodes = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.HelocIneligibleAutopayPIFStopCodes));
        string loanDetailsQueryForIneligibleInvCodes = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.HelocIneligibleAutopayInvCodes));
        string loanDetailsQueryForIneligiblePrepay2Months = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.HelocIneligibleAutopayPrepay2Months));
        string loanDetailsQueryForIneligibleDelinquent = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.HelocIneligibleAutopayDelinquentCountMorethanTwo));
        string loanDetailsQueryForEligibleAutopayOntime = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleAutoPayOntime), Constants.LoanStatus.Ontime);
        string loanDetailsQueryForEligibleAutopayPastdue = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleAutoPayPastDue), Constants.LoanStatus.PastDue);

        //OTP Ineligible
        string loanLevelDetailsForFmOTPInEligibleProcessStopCodeOTP = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForFmOTPInEligibleProcessStopCodeOTP));
        string loanLevelDetailsForFmOTPInEligibleANDPifStopCodeOTP = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForFmOTPInEligibleANDPifStopCodeOTP));
        string loanLevelDetailsForFmOTPInEligibleBadCheckStopCodeOTP = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForFmOTPInEligibleBadCheckStopCodeOTP));
        string loanLevelDetailsForFmOTPInEligibleForeclosureStopCodeOTP = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForFmOTPInEligibleForeclosureStopCodeOTP));

        string loanLevelDetailsForEligibleOTPOntime = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleOTPOntime));
        string loanDetailsForPendingPrepaidAutopay = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForPendingPrepaidAutopay));
        string loanDetailsForIneligiblePrepaidAutopay = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForIneligiblePrepaidAutopay));

        //in eligible payoff quote loans with different conditions
        string loanDetailsPayoffQuoteIneligibleDPA = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansPayoffIneligibleDPA));
        string loanDetailsPayoffQuoteIneligibleInactive = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansPayoffIneligibleInactive));
        string loanDetailsPayoffQuoteIneligibleFHA_RHS = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansPayoffIneligibleFHA_RHS));
        string loanDetailsPayoffQuoteIneligible_Foreclosure = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansPayoffIneligibleForeclosure));
        string loanDetailsPayoffQuoteIneligible_Delinquent = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansPayoffIneligibleDelinquent));
        string loanDetailsPayoffQuoteIneligibleVA = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansPayoffIneligibleVA));
        string loanDetailsPayoffQuoteIneligiblePIF = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansPayoffIneligiblePIF));
        string loanDetailsPayoffQuoteIneligibleTransferred = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansPayoffIneligibleTransferred));

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
        SMCPage smc = null;
        ReportLogger reportLogger { get; set; }
        #endregion ObjectInitialization

        #region CommonTestData
        private string deleteReason = "Test Delete Reason";
        private string firstName = "TESTFN";
        private string lastName = "TESTLN";
        private string personalOrBussiness = "Personal";
        private string savings = "Savings";
        private string accountNumber = Constants.BankAccountData.BankAccountNumber;
        private string accountNumberWhileEdit = Constants.BankAccountData.BankAccountNumberWhileEdit;
        private string routingNumber = "122199983";
        private string bankAccountName = Constants.BankAccountData.BankAccountName;
        private string accountFullName = "TESTFN TESTLN";
        #endregion CommonTestData

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeFramework(TestContext);
            dashboard = new DashboardPage(_driver, test);
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            payments = new Pages.PaymentsPage(_driver, test);
            smc = new SMCPage(_driver, test);
            dBconnect = new DBconnect(test, Constants.DBNames.MelloServETL);
            reportLogger = new ReportLogger(_driver);
            //unlink loans from test account 
            dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
            test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            string queryToUpdateTCPAFlagIsGlobalValue = UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.UpdateTCPAFlagIsGlobalValue).Replace("TCPA_FLAG_VALUE", "0");
            dBconnect.ExecuteQuery(queryToUpdateTCPAFlagIsGlobalValue).ToString();
        }

        [TestMethod]
        [Description("<br>TPR-1135-Verify Autopay Not Allowed for Ineligible Process Stop Code <br> +" +
                         "TPR-870-Verify ineligible loans are not allowed to setup autopay <br>" +
                         "TPR-496-Verify Autopay eligibility for loans with different process_stop_codes")]
        [TestCategory("CP_Regression"), TestCategory("CP_Dashboard")]
        public void TPR_1135_870_496_TPR_VerifyAutopayInEligibilityForLoansWithIneligibleProcessStopCode()
        {
            int loansFound = 0;
            int retryCount = 0;
            bool retry = true;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData
            // Execute the query and retrieve the loan data
            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForFmAutopayInEligibleProcessStopCode} </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForFmAutopayInEligibleProcessStopCode, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
            if (!test.Model.FullName.Contains("Heloc"))
                loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
            if (loanLevelData == null || loanLevelData.Count == 0)
            {
                test.Log(Status.Warning, $"There is no loan available for the above query");
                return;
            }
            loansFound = loanLevelData.Count;
            if (loansFound == 0)
            {
                test.Fail($"{loansFound} loans found for the specified criteria.");
                return;
            }
            else
                test.Info($"Found <b>{loansFound}</b> Loans");
            #endregion TestData

            try
            {
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                while ((retry && retryCount <= loansFound - 1))
                {
                    // Assuming the loanLevelData contains at least one loan
                    var loan = loanLevelData[retryCount];
                    dashboard.LinkLoan(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString(),
                                       loan[Constants.LoanLevelDataColumns.PropertyZip].ToString(),
                                       loan[Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), isReportRequired: true, emailID: username);
                    dashboard.HandlePaperLessPage();
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, dashboard.accountNotFoundMessageLocBy))
                    {
                        test.Info($"Successfully Linked Loan Number: {loan[Constants.LoanLevelDataColumns.LoanNumber].ToString()}");
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.makeAPaymentButtonLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        dashboard.HandlePaperLessPage();

                        dashboard.CloseHelocEligiblePopup(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString());
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();

                        // Validate the ineligibility message
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        reportLogger.TakeScreenshot(test, "My Dashboard Page");
                        webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay Button", isReportRequired: true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.manageAutopayButtonLocBy);
                        bool flag = webElementExtensions.VerifyElementText(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy, Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg, isReportRequired: true);
                        _driver.ReportResult(test, flag, "Successfully validated the Autopay Not allowed error msg", "Failed to validate the Autopay Not allowed error msg");
                        reportLogger.TakeScreenshot(test, "Autopay Not Allowed Error Message");
                        retry = false;
                    }
                    else
                    {
                        reportLogger.TakeScreenshot(test, "Account Not Found");
                        webElementExtensions.ScrollIntoView(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        webElementExtensions.ClickElement(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        retry = true;
                        retryCount++;
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-1133- Verify Autopay Not Allowed for Ineligible PIF Stop Code")]
        [TestCategory("CP_Regression"), TestCategory("CP_Dashboard")]
        public void TPR_1133_TPR_VerifyAutopayInEligibilityForLoansWithIneligiblePifStopCode()
        {
            int loansFound = 0;
            int retryCount = 0;
            bool retry = true;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData
            // Execute the query and retrieve the loan data
            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForFmAutopayInEligibleANDPifStopCodeAutopay} </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForFmAutopayInEligibleANDPifStopCodeAutopay, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
            if (!test.Model.FullName.Contains("Heloc"))
                loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
            if (loanLevelData == null || loanLevelData.Count == 0)
            {
                test.Log(Status.Warning, $"There is no loan available for the above query");
                return;
            }
            loansFound = loanLevelData.Count;
            if (loansFound == 0)
            {
                test.Fail($"{loansFound} loans found for the specified criteria.");
                return;
            }
            else
                test.Info($"Found <b>{loansFound}</b> Loans");
            #endregion TestData
            try
            {
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                while ((retry && retryCount <= loansFound - 1))
                {
                    // Assuming the loanLevelData contains at least one loan
                    var loan = loanLevelData[retryCount];
                    dashboard.LinkLoan(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString(),
                                       loan[Constants.LoanLevelDataColumns.PropertyZip].ToString(),
                                       loan[Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), isReportRequired: true, emailID: username);
                    dashboard.HandlePaperLessPage();
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, dashboard.accountNotFoundMessageLocBy))
                    {
                        test.Info($"Successfully Linked Loan Number: {loan[Constants.LoanLevelDataColumns.LoanNumber].ToString()}");
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.makeAPaymentButtonLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        dashboard.HandlePaperLessPage();
                        dashboard.CloseHelocEligiblePopup(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString());
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();

                        // Validate the ineligibility message
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        reportLogger.TakeScreenshot(test, "My Dashboard Page");
                        webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay Button", isReportRequired: true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.manageAutopayButtonLocBy);
                        string warningMsg = webElementExtensions.GetElementText(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy);
                        bool flag = webElementExtensions.VerifyElementText(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy, Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg, isReportRequired: true);
                        _driver.ReportResult(test, flag, "Successfully validated the Autopay Not allowed error msg", "Failed to validate the Autopay Not allowed error msg");
                        reportLogger.TakeScreenshot(test, "Autopay Not Allowed Error Message");
                        retry = false;
                    }
                    else
                    {
                        reportLogger.TakeScreenshot(test, "Account Not Found");
                        webElementExtensions.ScrollIntoView(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        webElementExtensions.ClickElement(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        retry = true;
                        retryCount++;
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-1131-Verify Autopay Not Allowed for Ineligible Bad Stop Code")]
        [TestCategory("CP_Regression"), TestCategory("CP_Dashboard")]
        public void TPR_1131_TPR_VerifyAutopayInEligibilityForLoansWithIneligibleBadCheckStopCode()
        {
            int loansFound = 0;
            int retryCount = 0;
            bool retry = true;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData
            // Execute the query and retrieve the loan data
            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanDetailsForFmAutopayInEligiblebadCheckStopCodeAutopay}  </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForFmAutopayInEligiblebadCheckStopCodeAutopay, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
            if (!test.Model.FullName.Contains("Heloc"))
                loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
            if (loanLevelData == null || loanLevelData.Count == 0)
            {
                test.Log(Status.Warning, $"There is no loan available for the above query");
                return;
            }
            loansFound = loanLevelData.Count;
            if (loansFound == 0)
            {
                test.Fail($"{loansFound} loans found for the specified criteria.");
                return;
            }
            else
                test.Info($"Found <b>{loansFound}</b> Loans");
            #endregion TestData

            try
            {
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                while ((retry && retryCount <= loansFound - 1))
                {
                    // Assuming the loanLevelData contains at least one loan
                    var loan = loanLevelData[retryCount];
                    dashboard.LinkLoan(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString(),
                                       loan[Constants.LoanLevelDataColumns.PropertyZip].ToString(),
                                       loan[Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), isReportRequired: true, emailID: username);
                    dashboard.HandlePaperLessPage();
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, dashboard.accountNotFoundMessageLocBy))
                    {
                        test.Info($"Successfully Linked Loan Number: {loan[Constants.LoanLevelDataColumns.LoanNumber].ToString()}");
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.makeAPaymentButtonLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        dashboard.HandlePaperLessPage();
                        dashboard.CloseHelocEligiblePopup(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString());
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();

                        // Validate the ineligibility message
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        reportLogger.TakeScreenshot(test, "My Dashboard Page");
                        webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay Button", isReportRequired: true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.manageAutopayButtonLocBy);
                        string warningMsg = webElementExtensions.GetElementText(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy);
                        bool flag = webElementExtensions.VerifyElementText(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy, Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg, isReportRequired: true);
                        _driver.ReportResult(test, flag, "Successfully validated the Autopay Not allowed error msg", "Failed to validate the Autopay Not allowed error msg");
                        reportLogger.TakeScreenshot(test, "Autopay Not Allowed Error Message");
                        retry = false;
                    }
                    else
                    {
                        reportLogger.TakeScreenshot(test, "Account Not Found");
                        webElementExtensions.ScrollIntoView(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        webElementExtensions.ClickElement(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        retry = true;
                        retryCount++;
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-1129-Verify Autopay Not Allowed for Ineligible Foreclosure Stop Code")]
        [TestCategory("CP_Regression"), TestCategory("CP_Dashboard")]
        public void TPR_1129_TPR_VerifyAutopayInEligibilityForLoansWithIneligibleForeclosureStopCode()
        {
            int loansFound = 0;
            int retryCount = 0;
            bool retry = true;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                // Execute the query and retrieve the loan data
                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanDetailsForFmAutopayInEligibleForeclosureStopCodeAutopay}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForFmAutopayInEligibleForeclosureStopCodeAutopay, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (!test.Model.FullName.Contains("Heloc"))
                    loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                loansFound = loanLevelData.Count;
                if (loansFound == 0)
                {
                    test.Fail($"{loansFound} loans found for the specified criteria.");
                    return;
                }
                else
                    test.Info($"Found <b>{loansFound}</b> Loans");
                #endregion TestData
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                while ((retry && retryCount <= loansFound - 1))
                {
                    // Assuming the loanLevelData contains at least one loan
                    var loan = loanLevelData[retryCount];
                    dashboard.LinkLoan(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString(),
                                       loan[Constants.LoanLevelDataColumns.PropertyZip].ToString(),
                                       loan[Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), isReportRequired: true, emailID: username);
                    dashboard.HandlePaperLessPage();
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, dashboard.accountNotFoundMessageLocBy))
                    {
                        test.Info($"Successfully Linked Loan Number: {loan[Constants.LoanLevelDataColumns.LoanNumber].ToString()}");
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.makeAPaymentButtonLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        dashboard.HandlePaperLessPage();
                        dashboard.CloseHelocEligiblePopup(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString());
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();

                        // Validate the ineligibility message
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        reportLogger.TakeScreenshot(test, "My Dashboard Page");
                        webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay Button", isReportRequired: true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.manageAutopayButtonLocBy);
                        string warningMsg = webElementExtensions.GetElementText(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy);
                        bool flag = webElementExtensions.VerifyElementText(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy, Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg, isReportRequired: true);
                        _driver.ReportResult(test, flag, "Successfully validated the Autopay Not allowed error msg", "Failed to validate the Autopay Not allowed error msg");
                        reportLogger.TakeScreenshot(test, "Autopay Not Allowed Error Message");
                        retry = false;
                    }
                    else
                    {
                        reportLogger.TakeScreenshot(test, "Account Not Found");
                        webElementExtensions.ScrollIntoView(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        webElementExtensions.ClickElement(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        retry = true;
                        retryCount++;
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-424-Verify customers can make autopay for loans that have Monthly active autopay with prepaid 2 or more months")]
        [TestCategory("CP_Regression"), TestCategory("CP_Dashboard")]
        public void TPR_424_TPR_VerifyEligibleAutopayForLoansWithActivePrepaid()
        {
            try
            {
                VerifyAutopayStatusForLoan(
                draftingIndicator: "= 'Y'",
                query: loanDetailsForPendingPrepaidAutopay.Replace("ZDRAFTING_INDICATOR", "= 'Y'"),
                expectedAutopayStatus: "On Autopay",
                isActivePlanExpected: true
            );
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Error while executing {testContext.TestName} test method. Error: {e.Message}.");
            }
            finally
            {
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>CUSTP-2538 - Verify ineligible message when loan has no Active plan with prepaid 2 or more months")]
        [TestCategory("CP_Regression"), TestCategory("CP_Dashboard")]
        public void TPR_425_TPR_VerifyIneligibleAutopayForLoansWithoutActivePlan()
        {
            try
            {
                VerifyAutopayStatusForLoan(
                draftingIndicator: "!= 'Y'",
                query: loanDetailsForIneligiblePrepaidAutopay.Replace("ZDRAFTING_INDICATOR", "!= 'Y'"),
                expectedAutopayStatus: "Off Autopay",
                isActivePlanExpected: false
            );
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Error while executing {testContext.TestName} test method. Error: {e.Message}.");
            }
            finally
            {
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        // This logic has been added to support 2 test cases, need to revisit this in future [TPR-2537-2538]
        public void VerifyAutopayStatusForLoan(string draftingIndicator, string query, string expectedAutopayStatus, bool isActivePlanExpected)
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            List<Hashtable> usedLoanTestData = new List<Hashtable>();
            try
            {
                #region TestData
                test.Info($"Query used for Prepaid loan with next_payment_due_date > 2 months and drafting_indicator <b>{draftingIndicator} : {query}</b>");

                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                    .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                    .Where(field => field.IsLiteral && !field.IsInitOnly)
                    .Select(field => field.GetValue(null).ToString()).ToList();

                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }

                loanLevelData = commonServices.GetLoanDataFromDatabase(query, null, requiredColumns, new List<string>(), ConfigSettings.NumberOfLoanTestDataRequired, true);

                if (!test.Model.FullName.Contains("Heloc"))
                    loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                var loansFound = loanLevelData.Count;
                if (loansFound == 0)
                {
                    test.Fail($"{loansFound} loans found for the specified criteria.");
                    return;
                }
                else
                    test.Info($"Found <b>{loansFound}</b> Loans");
                #endregion TestData

                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (_driver.Url.Contains("/link-user-loan"))
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        dashboard.CloseHelocEligiblePopup(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString());
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), false) && webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", expectedAutopayStatus))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    else
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        dashboard.CloseHelocEligiblePopup(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString());
                        if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(string.Format(dashboard.serviceBotMessangerLocString, "visible"))))
                            dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), false) && webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", expectedAutopayStatus))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForElement(_driver, payments.manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.ScrollIntoView(_driver, payments.manageAutoPayHeaderTextLocBy);

                if (isActivePlanExpected)
                {
                    payments.VerifyAutopayDetailsForActivePlan();
                    dashboard.ValidateAutopayBadge(expectedAutopayStatus);
                }
                else
                {
                    payments.VerifyAutopayDetailsForInactivePlan();
                    dashboard.ValidateAutopayBadge(expectedAutopayStatus);
                }

                reportLogger.TakeScreenshot(test, "Dashboard Page");

                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while Verifying Autopay Status. Error : <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
            }
        }

        [TestMethod]
        [Description("<br>TPR-2655:Verify IVR Request Payoff Quote for ineligible  loans -Penalty Header DPA<br>" +
            "<br>TPR-2105:Verify- Request payoff quote for Ineligible Loans in portal(Loans of type All RHS/USDA, FHA loans)</br>" +
            "<br>TPR-2148:Verify- Request payoff quote for Ineligible Loans Default or delinquent for 30+ days</br>" +
            "<br>TPR-1835:Verify- Request payoff quote for Ineligible Loans in portal(Loans of types Service Transfer_PIF_Inactive)</br>" +
            "<br>TPR-2146:Verify button Request payoff quote for ineligible loans-Loan has active foreclosure status code A or S or default Status</br>" +
            "<br>TPR-2152 :Verify button Request Payoff Quote button for ineligible  loans -VA loans with Loss Mitigation code P on MSP screen LMT1 under L/M IND</br>")]
        [TestCategory("CP_Regression"), TestCategory("CP_IneligiblePayoff_Request"), TestCategory("CP_Regression")]
        public void TPR_1835_2105_2148_2146_2152_TPR_VerifyRequestPayoffQuoteForInEligibleLoans()
        {


            List<Hashtable> usedLoanTestData = new List<Hashtable>();
            List<Hashtable> loanLevelData = null;
            List<Tuple<string, bool, string>> testStatus = new List<Tuple<string, bool, string>>();
            var ineligiblePayoffQueries = new[]
            {
               new {Name= nameof(loanDetailsPayoffQuoteIneligibleDPA),Query=loanDetailsPayoffQuoteIneligibleDPA },
               new {Name= nameof(loanDetailsPayoffQuoteIneligibleFHA_RHS),Query=loanDetailsPayoffQuoteIneligibleFHA_RHS },
               new {Name= nameof(loanDetailsPayoffQuoteIneligibleInactive),Query=loanDetailsPayoffQuoteIneligibleInactive },
               new {Name= nameof(loanDetailsPayoffQuoteIneligiblePIF), Query= loanDetailsPayoffQuoteIneligiblePIF },
               new {Name= nameof(loanDetailsPayoffQuoteIneligibleTransferred), Query= loanDetailsPayoffQuoteIneligibleTransferred },
               new {Name= nameof(loanDetailsPayoffQuoteIneligibleVA), Query= loanDetailsPayoffQuoteIneligibleVA },
               new {Name= nameof(loanDetailsPayoffQuoteIneligible_Delinquent), Query= loanDetailsPayoffQuoteIneligible_Delinquent },
               new {Name= nameof(loanDetailsPayoffQuoteIneligible_Foreclosure), Query = loanDetailsPayoffQuoteIneligible_Foreclosure }

             };
            try
            {

                commonServices.LoginToTheApplication(username, password);

                foreach (var payoffQuery in ineligiblePayoffQueries)
                {
                    #region TestData
                    test.Log(Status.Info, $"---------------------<b>{payoffQuery.Name}</b>------------------------");

                    test.Log(Status.Info, $"Query Used: {payoffQuery.Query} ");
                    var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                         .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                         .Where(field => field.IsLiteral && !field.IsInitOnly)
                         .Select(field => field.GetValue(null).ToString()).ToList();
                    if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                    {
                        requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                        requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                    }

                    try
                    {
                        loanLevelData = commonServices.GetLoanDataFromDatabase(payoffQuery.Query, null, requiredColumns, new List<string>(), ConfigSettings.NumberOfLoanTestDataRequired, true);
                    }
                    catch (AssertFailedException e)
                    {
                        testStatus.Add(new Tuple<string, bool, string>(payoffQuery.Name, false, "No Loans"));
                        continue;
                    }

                    #endregion TestData

                    // Login to app and link loans
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                    if (loanLevelData.Count > 0)
                    {
                        if (_driver.Url.Contains("link-user-loan") && loanLevelData.Count > 0)
                        {
                            webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                            dashboard.LinkLoan(loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[0][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                            usedLoanTestData.Add(loanLevelData[0]);
                            dashboard.HandlePaperLessPage();
                            webElementExtensions.WaitUntilUrlContains("/dashboard");
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
                        }

                        else
                        {
                            dashboard.LinkLoan(loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[0][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                            usedLoanTestData.Add(loanLevelData[0]);
                            dashboard.HandlePaperLessPage();
                            webElementExtensions.WaitUntilUrlContains("/dashboard");
                            webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.payoffQuoteLocBy);
                        }
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                        test.Log(Status.Info, "<b>********************************************<u>Starting Payoff Quote</u>*******************************************</b>");

                        //Navigate to PayoffQuote Page
                        webElementExtensions.ScrollToTop(_driver);
                        reportLogger.TakeScreenshot(test, "Dashboard Page");
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.payoffQuoteLocBy);
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy, isScrollIntoViewRequired: false))
                        {
                            dashboard.RequestPayoffQuote();
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                            webElementExtensions.WaitForPageLoad(_driver);
                            webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.dialogLocBy);
                            reportLogger.TakeScreenshot(test, "Payoff Quote Ineligible");
                            var content = webElementExtensions.GetElementText(_driver, dashboard.dialogLocBy);
                            ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.IneligiblePayoffQuoteErrorMsg, content, $"Payoff Quote Ineligibe Message should be appear");
                            webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.dialogCloseBtnLocBy);
                            testStatus.Add(new Tuple<string, bool, string>(payoffQuery.Name, true, ""));
                        }
                    }
                    else
                    {
                        test.Log(Status.Warning, $"No Loans Found with query {payoffQuery}");

                    }

                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Error while executing {testContext.TestName} test method. Error: {e.Message}.");
            }
            finally
            {
                test.Log(Status.Info, "---------------------------All Tests Status-----------------------------------");
                test.Log(Status.Info, $"Total : {testStatus.Count} , Passed : {testStatus.Where(t => t.Item2 == true).Count()} , Failed : {testStatus.Where(t => t.Item2 == false).Count()}");
                foreach (var ts in testStatus)
                    test.Log(Status.Info, $"Query : {ts.Item1} , IsSuccessful : {ts.Item2} , Reason : {ts.Item3}");

                foreach (var loanDetail in usedLoanTestData)
                {
                    if (loanLevelData.Count != 0)
                        dBconnect.UpdateBorrowerEmailID(loanDetail[Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanDetail[Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                }

                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-1548 - Verify Payment ineligible process_stop_code")]
        [TestCategory("CP_Regression"), TestCategory("CP_Dashboard")]
        public void TPR_1548_TPR_VerifyFmOTPInEligibilityForLoansWithIneligibleProcessStopCode()
        {
            int loansFound = 0;
            int retryCount = 0;
            bool retry = true;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData
            // Execute the query and retrieve the loan data
            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanLevelDetailsForFmOTPInEligibleProcessStopCodeOTP} </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForFmOTPInEligibleProcessStopCodeOTP, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
            if (loanLevelData == null || loanLevelData.Count == 0)
            {
                test.Log(Status.Warning, $"There is no loan available for the above query");
                return;
            }
            loansFound = loanLevelData.Count;
            if (loansFound == 0)
            {
                test.Warning("No loans found for the specified criteria.");
                return;
            }
            else
                test.Info($"Found <b>{loansFound}</b> Loans");
            #endregion TestData
            try
            {
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                while ((retry && retryCount <= loansFound - 1))
                {
                    // Assuming the loanLevelData contains at least one loan
                    var loan = loanLevelData[retryCount];
                    dashboard.LinkLoan(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString(),
                                       loan[Constants.LoanLevelDataColumns.PropertyZip].ToString(),
                                       loan[Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), isReportRequired: true, emailID: username);
                    dashboard.HandlePaperLessPage();
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, dashboard.accountNotFoundMessageLocBy))
                    {
                        test.Info($"Successfully Linked Loan Number: {loan[Constants.LoanLevelDataColumns.LoanNumber].ToString()}");
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.makeAPaymentButtonLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        dashboard.HandlePaperLessPage();

                        dashboard.CloseHelocEligiblePopup(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString());
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();

                        // Validate the ineligibility message
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        reportLogger.TakeScreenshot(test, "My Dashboard Page");
                        ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, dashboard.btnMakePaymentLocBy, "Make a payment button", false), "Verify Make a Payment button Disabled");
                        ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalErrorMsgs.IneligibleLoanForPaymentSetupErrorMsg, webElementExtensions.GetElementText(_driver, dashboard.paymentIneligibleMessageLocBy, true), "Verify Payment Ineligible Message");
                        retry = false;
                    }
                    else
                    {
                        reportLogger.TakeScreenshot(test, "Account Not Found");
                        webElementExtensions.ScrollIntoView(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        webElementExtensions.ClickElement(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        retry = true;
                        retryCount++;
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TRP-1523 - Verify Payment ineligible pif_check_stop_code")]
        [TestCategory("CP_Regression"), TestCategory("CP_Dashboard")]
        public void TPR_1523_TPR_VerifyFmOTPInEligibilityForLoansWithIneligiblePifStopCode()
        {
            int loansFound = 0;
            int retryCount = 0;
            bool retry = true;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData
            // Execute the query and retrieve the loan data
            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanLevelDetailsForFmOTPInEligibleANDPifStopCodeOTP} </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForFmOTPInEligibleANDPifStopCodeOTP, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
            if (loanLevelData == null || loanLevelData.Count == 0)
            {
                test.Log(Status.Warning, $"There is no loan available for the above query");
                return;
            }
            loansFound = loanLevelData.Count;
            if (loansFound == 0)
            {
                test.Warning("No loans found for the specified criteria.");
                return;
            }
            else
                test.Info($"Found <b>{loansFound}</b> Loans");
            #endregion TestData
            try
            {

                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                while ((retry && retryCount <= loansFound - 1))
                {
                    // Assuming the loanLevelData contains at least one loan
                    var loan = loanLevelData[retryCount];
                    dashboard.LinkLoan(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString(),
                                       loan[Constants.LoanLevelDataColumns.PropertyZip].ToString(),
                                       loan[Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), isReportRequired: true, emailID: username);
                    dashboard.HandlePaperLessPage();
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, dashboard.accountNotFoundMessageLocBy))
                    {
                        test.Info($"Successfully Linked Loan Number: {loan[Constants.LoanLevelDataColumns.LoanNumber].ToString()}");
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.makeAPaymentButtonLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        dashboard.HandlePaperLessPage();
                        dashboard.CloseHelocEligiblePopup(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString());
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();

                        // Validate the ineligibility message
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        reportLogger.TakeScreenshot(test, "My Dashboard Page");
                        ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, dashboard.btnMakePaymentLocBy, "Make a payment button", false), "Verify Make a Payment button Disabled");
                        ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalErrorMsgs.IneligibleLoanForPaymentSetupErrorMsg, webElementExtensions.GetElementText(_driver, dashboard.paymentIneligibleMessageLocBy, true), "Verify Payment Ineligible Message");
                        retry = false;
                    }
                    else
                    {
                        reportLogger.TakeScreenshot(test, "Account Not Found");
                        webElementExtensions.ScrollIntoView(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        webElementExtensions.ClickElement(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        retry = true;
                        retryCount++;
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-1524 - Verify Payment ineligible bad_check_stop_code")]
        [TestCategory("CP_Regression"), TestCategory("CP_Dashboard")]
        public void TPR_1524_TPR_VerifyFmOtpInEligibilityForLoansWithIneligibleBadCheckStopCode()
        {
            int loansFound = 0;
            int retryCount = 0;
            bool retry = true;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData
            // Execute the query and retrieve the loan data
            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanLevelDetailsForFmOTPInEligibleBadCheckStopCodeOTP}  </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForFmOTPInEligibleBadCheckStopCodeOTP, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
            if (loanLevelData == null || loanLevelData.Count == 0)
            {
                test.Log(Status.Warning, $"There is no loan available for the above query");
                return;
            }
            loansFound = loanLevelData.Count;
            if (loansFound == 0)
            {
                test.Warning("No loans found for the specified criteria.");
                return;
            }
            else
                test.Info($"Found <b>{loansFound}</b> Loans");
            #endregion TestData
            try
            {

                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                while ((retry && retryCount <= loansFound - 1))
                {
                    // Assuming the loanLevelData contains at least one loan
                    var loan = loanLevelData[retryCount];
                    dashboard.LinkLoan(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString(),
                                       loan[Constants.LoanLevelDataColumns.PropertyZip].ToString(),
                                       loan[Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), isReportRequired: true, emailID: username);
                    dashboard.HandlePaperLessPage();
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, dashboard.accountNotFoundMessageLocBy))
                    {
                        test.Info($"Successfully Linked Loan Number: {loan[Constants.LoanLevelDataColumns.LoanNumber].ToString()}");
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.makeAPaymentButtonLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        dashboard.HandlePaperLessPage();
                        dashboard.CloseHelocEligiblePopup(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString());
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();

                        // Validate the ineligibility message
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        reportLogger.TakeScreenshot(test, "My Dashboard Page");
                        ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, dashboard.btnMakePaymentLocBy, "Make a payment button", false), "Verify Make a Payment button Disabled");
                        ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalErrorMsgs.IneligibleLoanForPaymentSetupErrorMsg, webElementExtensions.GetElementText(_driver, dashboard.paymentIneligibleMessageLocBy, true), "Verify Payment Ineligible Message");
                        retry = false;
                    }
                    else
                    {
                        reportLogger.TakeScreenshot(test, "Account Not Found");
                        webElementExtensions.ScrollIntoView(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        webElementExtensions.ClickElement(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        retry = true;
                        retryCount++;
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-1549 - Verify Payment ineligible foreclosure_stop_code")]
        [TestCategory("CP_Regression"), TestCategory("CP_Dashboard")]
        public void TPR_1549_TPR_VerifyFmOTPInEligibilityForLoansWithIneligibleForeclosureStopCode()
        {
            int loansFound = 0;
            int retryCount = 0;
            bool retry = true;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData
            // Execute the query and retrieve the loan data
            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanLevelDetailsForFmOTPInEligibleForeclosureStopCodeOTP}  </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForFmOTPInEligibleForeclosureStopCodeOTP, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
            if (loanLevelData == null || loanLevelData.Count == 0)
            {
                test.Log(Status.Warning, $"There is no loan available for the above query");
                return;
            }
            loansFound = loanLevelData.Count;
            if (loansFound == 0)
            {
                test.Warning("No loans found for the specified criteria.");
                return;
            }
            else
                test.Info($"Found <b>{loansFound}</b> Loans");
            #endregion TestData
            try
            {
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                while ((retry && retryCount <= loansFound - 1))
                {
                    // Assuming the loanLevelData contains at least one loan
                    var loan = loanLevelData[retryCount];
                    dashboard.LinkLoan(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString(),
                                       loan[Constants.LoanLevelDataColumns.PropertyZip].ToString(),
                                       loan[Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), isReportRequired: true, emailID: username);
                    dashboard.HandlePaperLessPage();
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, dashboard.accountNotFoundMessageLocBy))
                    {
                        test.Info($"Successfully Linked Loan Number: {loan[Constants.LoanLevelDataColumns.LoanNumber].ToString()}");
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.makeAPaymentButtonLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        dashboard.HandlePaperLessPage();
                        dashboard.CloseHelocEligiblePopup(loan[Constants.LoanLevelDataColumns.LoanNumber].ToString());
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();

                        // Validate the ineligibility message
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        reportLogger.TakeScreenshot(test, "My Dashboard Page");
                        ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, dashboard.btnMakePaymentLocBy, "Make a payment button", false), "Verify Make a Payment button Disabled");
                        ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalErrorMsgs.IneligibleLoanForPaymentSetupErrorMsg, webElementExtensions.GetElementText(_driver, dashboard.paymentIneligibleMessageLocBy, true), "Verify Payment Ineligible Message");
                        retry = false;
                    }
                    else
                    {
                        reportLogger.TakeScreenshot(test, "Account Not Found");
                        webElementExtensions.ScrollIntoView(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        webElementExtensions.ClickElement(_driver, dashboard.cancelButtonLinkMyLoanPopupLocBy);
                        retry = true;
                        retryCount++;
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-1544-Verify Unable To Select Payment Date On Weekends")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP")]
        public void TPR_1544_TPR_VerifyUnableToSelectPaymentDateOnWeekends()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData

            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanLevelDetailsForEligibleOTPOntime}  </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForEligibleOTPOntime, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
            if (!test.Model.FullName.Contains("Heloc"))
                loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
            if (loanLevelData == null || loanLevelData.Count == 0)
            {
                test.Log(Status.Warning, $"There is no loan available for the above query");
                return;
            }
            var loansFound = loanLevelData.Count;
            if (loansFound == 0)
            {
                test.Fail($"{loansFound} loans found for the specified criteria.");
                return;
            }
            else
                test.Info($"Found <b>{loansFound}</b> Loans");
            #endregion TestData

            try
            {
                string loanStatus = Constants.LoanStatus.Ontime;

                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, true, true))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, true, true))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willSchedulePaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willSchedulePaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();

                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.SmallWaitTime);
                payments.VerifyWeekendDatesAreNotEnabledForSelection();
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, "Test Failed at this screen");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-1545 - Verify CP_Unable_To_SetUp_More_Than_Three_OTP for Ontime")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP")]
        public void TPR_1545_TPR_CP_Unable_To_SetUp_More_Than_Three_OTP()
        {
            int retryCount = 0;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData

            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanLevelDetailsForEligibleOTPOntime}  </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForEligibleOTPOntime, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
            if (!test.Model.FullName.Contains("Heloc"))
                loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
            if (loanLevelData == null || loanLevelData.Count == 0)
            {
                test.Log(Status.Warning, $"There is no loan available for the above query");
                return;
            }
            var loansFound = loanLevelData.Count;
            if (loansFound == 0)
            {
                test.Fail($"{loansFound} loans found for the specified criteria.");
                return;
            }
            else
                test.Info($"Found <b>{loansFound}</b> Loans");
            #endregion TestData
            try
            {
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, true, true))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, true, true))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willSchedulePaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<DateTime> paymentSetupDates = new List<DateTime>();
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                if (numberOfPendingPayments == 3)
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, dashboard.btnMakePaymentLocBy, "Make Payment Button", false), "Verify Make Payment Button to be disabled");
                else
                {
                    // Setup up to 2 more payments (limit to a max of 3)
                    int paymentsToMake = 3 - numberOfPendingPayments;
                    for (int i = 0; i < paymentsToMake; i++)
                    {
                        DateTime payment_date = DateTime.ParseExact(payments.MakeOTPPayment(loanLevelData[retryCount], bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, accountFullName).ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        paymentSetupDates.Add(payment_date);
                    }
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, dashboard.btnMakePaymentLocBy, "Make Payment Button", false), "Verify Make Payment Button to be disabled");
                }

                foreach (var date in paymentSetupDates)
                {
                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");
                    payments.DeleteOtpPaymentSetup(date);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                    webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                    reportLogger.TakeScreenshot(test, "Delete OTP");
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy);
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, "Test Failed at this screen");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-1117 - Verify add new Bank Account Validations<br>" +
                         "TPR-1119 - Verify Bank Account Number Validations<br>" +
                         "TPR-1130 - Verify add Bank account")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP")]
        public void TPR_1117_1119_1130_TPR_ValidateAddBankAccountFunctionality()
        {
            int retryCount = 0;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData

            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanLevelDetailsForEligibleOTPOntime}  </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForEligibleOTPOntime, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
            if (!test.Model.FullName.Contains("Heloc"))
                loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
            if (loanLevelData == null || loanLevelData.Count == 0)
            {
                test.Log(Status.Warning, $"There is no loan available for the above query");
                return;
            }
            var loansFound = loanLevelData.Count;
            if (loansFound == 0)
            {
                test.Fail($"{loansFound} loans found for the specified criteria.");
                return;
            }
            else
                test.Info($"Found <b>{loansFound}</b> Loans");
            #endregion TestData
            try
            {
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, true, true))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, true, true))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willSchedulePaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);

                if (willSchedulePaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();

                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);

                commonServices.DeleteAllAddedBankAccounts();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, commonServices.addAnAccountLinkLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.addAnAccountLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.addBankAccountPopUpLocBy);

                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.addBankAccountPopUpLocBy, "Add Bank Account Popup"), "Verify Add Bank Account Popup is Displayed");
                By locator = null;
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.accountNicknameLocBy, "maxlength", "100"), "Verify the Max Length allowed for AccountNickName Input Field");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.accountNicknameLocBy, "aria-required", "false"), "Verify AccountNickName Input Field is not a Required field");

                webElementExtensions.EnterText(_driver, commonServices.accountNicknameLocBy, accountFullName, true, null, false, false);
                if (personalOrBussiness.ToLower().Equals("personal"))
                    locator = commonServices.personalRadioButtonLocBy;
                else if (personalOrBussiness.ToLower().Equals("business"))
                    locator = commonServices.businessRadioButtonLocBy;
                webElementExtensions.MoveToElement(_driver, locator);
                webElementExtensions.ActionClick(_driver, locator);
                if (savings.ToLower().Equals("checking"))
                    locator = commonServices.checkingRadioButtonLocBy;
                else if (savings.ToLower().Equals("savings"))
                    locator = commonServices.savingsRadioButtonLocBy;
                webElementExtensions.MoveToElement(_driver, locator);
                webElementExtensions.ActionClick(_driver, locator);


                // First Name
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.firstNameOnAccountTextboxLocBy, "First name Input Field"), "Verify First name Input Field is Displayed");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.firstNameOnAccountTextboxLocBy, "maxlength", "100"), "Verify Max Length of First Name");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.firstNameOnAccountTextboxLocBy, "aria-required", "true"), "Verify First Name is Required");

                // Last Name
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.lastNameOnAccountTextboxLocBy, "Last name Input Field"), "Verify Last name Input Field is Displayed");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.lastNameOnAccountTextboxLocBy, "maxlength", "100"), "Verify Max Length of Last Name");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.lastNameOnAccountTextboxLocBy, "aria-required", "true"), "Verify Last Name is Required");

                // Routing Number
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.routingNumberTextboxLocBy, "Routing Number Input Field"), "Verify Routing Number Field is Displayed");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.routingNumberTextboxLocBy, "maxlength", "9"), "Verify Max Length of Routing Number");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.routingNumberTextboxLocBy, "aria-required", "true"), "Verify Routing Number is Required");

                // Account Number
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.accountNumberTextboxLocBy, "Account Number Input Field"), "Verify Account Number Field is Displayed");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.accountNumberTextboxLocBy, "minlength", "4"), "Verify Min Length of Account Number");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.accountNumberTextboxLocBy, "maxlength", "17"), "Verify Max Length of Account Number");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.accountNumberTextboxLocBy, "aria-required", "true"), "Verify Account Number is Required");

                // Confirm Account Number
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.confirmAccountNumberTextboxLocBy, "Confirm Account Number Input Field"), "Verify Confirm Account Number Field is Displayed");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.confirmAccountNumberTextboxLocBy, "minlength", "4"), "Verify Min Length of Confirm Account Number");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.confirmAccountNumberTextboxLocBy, "maxlength", "17"), "Verify Max Length of Confirm Account Number");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.confirmAccountNumberTextboxLocBy, "aria-required", "false"), "Verify Confirm Account Number is NOT Required");

                // First Name - Invalid Input
                webElementExtensions.EnterText(_driver, commonServices.firstNameOnAccountTextboxLocBy, "@!", false);
                webElementExtensions.EnterText(_driver, commonServices.firstNameOnAccountTextboxLocBy, Keys.Tab, false);
                webElementExtensions.WaitForVisibilityOfElement(_driver, By.XPath(commonServices.addAnAccountPopupErrorMsgs.Replace("<ERROR>", Constants.CustomerPortalErrorMsgs.AccountFirstNameErrorMsg)));
                ReportingMethods.LogAssertionTrue(test,
                    webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(commonServices.addAnAccountPopupErrorMsgs.Replace("<ERROR>", Constants.CustomerPortalErrorMsgs.AccountFirstNameErrorMsg)), "First Name"),
                    "Verify First Name Error Msg is Displayed");

                // Last Name - Invalid Input
                webElementExtensions.EnterText(_driver, commonServices.lastNameOnAccountTextboxLocBy, "@!", false);
                webElementExtensions.EnterText(_driver, commonServices.lastNameOnAccountTextboxLocBy, Keys.Tab, false);
                webElementExtensions.WaitForVisibilityOfElement(_driver, By.XPath(commonServices.addAnAccountPopupErrorMsgs.Replace("<ERROR>", Constants.CustomerPortalErrorMsgs.AccountLastNameErrorMsg)));
                ReportingMethods.LogAssertionTrue(test,
                    webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(commonServices.addAnAccountPopupErrorMsgs.Replace("<ERROR>", Constants.CustomerPortalErrorMsgs.AccountLastNameErrorMsg)), "Last Name"),
                    "Verify Last Name Error Msg is Displayed");

                // Routing Number - Invalid Input
                webElementExtensions.EnterText(_driver, commonServices.routingNumberTextboxLocBy, "@!", false);
                webElementExtensions.EnterText(_driver, commonServices.routingNumberTextboxLocBy, Keys.Tab, false);
                webElementExtensions.WaitForStalenessOfElement(_driver, By.XPath(commonServices.addAnAccountPopupErrorMsgs.Replace("<ERROR>", Constants.CustomerPortalErrorMsgs.RoutingNumberErrorMsg)), 2);
                ReportingMethods.LogAssertionTrue(test,
                    webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(commonServices.addAnAccountPopupErrorMsgs.Replace("<ERROR>", Constants.CustomerPortalErrorMsgs.RoutingNumberErrorMsg))),
                    "Verify Routing Number Error Msg is Displayed");

                // Account Number - Invalid Input
                webElementExtensions.EnterText(_driver, commonServices.accountNumberTextboxLocBy, "@!", false);
                webElementExtensions.EnterText(_driver, commonServices.accountNumberTextboxLocBy, Keys.Tab, false);
                webElementExtensions.WaitForVisibilityOfElement(_driver, By.XPath(commonServices.addAnAccountPopupErrorMsgs.Replace("<ERROR>", Constants.CustomerPortalErrorMsgs.AccountNicknameErrorMsg)));
                ReportingMethods.LogAssertionTrue(test,
                    webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(commonServices.addAnAccountPopupErrorMsgs.Replace("<ERROR>", Constants.CustomerPortalErrorMsgs.AccountNumberErrorMsg)), "Account Number"),
                    "Verify Account Number Error Msg is Displayed");

                // Confirm Account Number - Invalid Input
                webElementExtensions.EnterText(_driver, commonServices.confirmAccountNumberTextboxLocBy, "@", false);
                webElementExtensions.EnterText(_driver, commonServices.confirmAccountNumberTextboxLocBy, Keys.Tab, false);
                webElementExtensions.WaitForVisibilityOfElement(_driver, By.XPath(commonServices.addAnAccountPopupErrorMsgs.Replace("<ERROR>", Constants.CustomerPortalErrorMsgs.ConfirmAccountNumberErrorMsg)));
                ReportingMethods.LogAssertionTrue(test,
                    webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(commonServices.addAnAccountPopupErrorMsgs.Replace("<ERROR>", Constants.CustomerPortalErrorMsgs.ConfirmAccountNumberErrorMsg)), "Confirm Account Number"),
                    "Verify Confirm Account Number Error Msg is Displayed");
                reportLogger.TakeScreenshot(test, "Add An Account Pop up Error Msgs");


                webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.cancelButtonAddAnAccountPopupLocBy, "Cancel Button on Add An Account Popup");

            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, "Test Failed at this screen");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR 3226 : Bi-Weekly should not be allowed for ontime loans after 13th Month <br>")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_3226_TPR_VerifyInEligibleMessageForBiWeeklyAutoPaySetupForOntimeLoanAfter13th()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleAutopayOntime} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleAutopayOntime, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (!test.Model.FullName.Contains("Heloc"))
                    loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                var loansFound = loanLevelData.Count;
                if (loansFound == 0)
                {
                    test.Fail($"{loansFound} loans found for the specified criteria.");
                    return;
                }
                else
                    test.Info($"Found <b>{loansFound}</b> Loans");
                #endregion TestData

                commonServices.LoginToTheApplication(username, password);

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString()))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString()))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>************************************<u>Verify Setup Autopay Page for Biweekly option</u>************************************</b>");
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);
                reportLogger.TakeScreenshot(test, "Autopay Setup Page");
                var biweeklyMsg = webElementExtensions.GetElementText(_driver, payments.biweeklyMessageTextLocBy);

                var cstDateNow = UtilAdditions.GetCSTTimeNow();
                ReportingMethods.Log(test, $"CST Time Now : {cstDateNow}");
                webElementExtensions.ClickElement(_driver, commonServices.backtoAutopaySetupBtnLocBy);
                webElementExtensions.WaitForElement(_driver, commonServices.leaveButtonLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.leaveButtonLocBy, "Leave Button");

                if (cstDateNow.Day >= 13)
                {
                    ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.BiweeklyNotAllowedMessage, biweeklyMsg, $"Expected {Constants.CustomerPortalErrorMsgs.BiweeklyNotAllowedMessage}, Actual {biweeklyMsg}");
                }
                else
                {
                    ReportingMethods.LogAssertionFalse(test, biweeklyMsg.Contains(Constants.CustomerPortalErrorMsgs.BiweeklyNotAllowedMessage), $"Expected Not to show {Constants.CustomerPortalErrorMsgs.BiweeklyNotAllowedMessage}, Actual {biweeklyMsg}");
                }

            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-481 : Verify Autopay eligibility for loans with different loan types- PastDue <br>")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_481_TPR_VerifyInEligibleMessageForAutopaySetupPastdueLoans()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleAutopayPastdue} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleAutopayPastdue, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (!test.Model.FullName.Contains("Heloc"))
                    loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                var loansFound = loanLevelData.Count;
                if (loansFound == 0)
                {
                    test.Fail($"{loansFound} loans found for the specified criteria.");
                    return;
                }
                else
                    test.Info($"Found <b>{loansFound}</b> Loans");
                #endregion TestData

                commonServices.LoginToTheApplication(username, password);

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkMakeAOneTimePaymentPage: false))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>************************************<u>Verify Setup Autopay Page for Pastdue Loans</u>************************************</b>");
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                reportLogger.TakeScreenshot(test, "Autopay Setup Page");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.autopayNoTallowedMsgLocBy);

                var cstDateNow = UtilAdditions.GetCSTTimeNow();
                ReportingMethods.Log(test, $"CST Time Now : {cstDateNow}");

                if (cstDateNow.Day == 13 && cstDateNow.Hour >= 20)
                {
                    ReportingMethods.Log(test, $"Expecting not to allow Autopay setup as CST Time now is {cstDateNow} which is greater than cutoff time '8 PM CST 13th of Month'");
                    ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg, errorMsg, $"Expected {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");
                }
                if (cstDateNow.Day > 13)
                {
                    ReportingMethods.Log(test, $"Expecting not to allow Autopay setup as CST Time now is {cstDateNow} which is greater than cutoff time '8 PM CST 13th of Month'");
                    ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg, errorMsg, $"Expected {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");
                }
                else
                {
                    ReportingMethods.Log(test, $"Expecting allow Autopay setup as CST Time now is {cstDateNow} which is less than cutoff time '8 PM CST 13th of Month'");
                    ReportingMethods.LogAssertionFalse(test, errorMsg.Contains(Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg), $"Expected Not to show any error like {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");
                }

            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br><b>TPR-3242-Verify InEligible Message For Autopay Setup Pastdue Loans_HELOC </b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_3242_TPR_VerifyInEligibleMessageForAutopaySetupPastdueLoans_HELOC()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleHelocAutopayPastDue} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleHelocAutopayPastDue, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (!test.Model.FullName.Contains("Heloc"))
                    loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                var loansFound = loanLevelData.Count;
                if (loansFound == 0)
                {
                    test.Fail($"{loansFound} loans found for the specified criteria.");
                    return;
                }
                else
                    test.Info($"Found <b>{loansFound}</b> Loans");
                #endregion TestData

                commonServices.LoginToTheApplication(username, password);

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                reportLogger.TakeScreenshot(test, "Autopay Setup Page");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.autopayNoTallowedMsgLocBy);

                var cstDateNow = UtilAdditions.GetCSTTimeNow();
                ReportingMethods.Log(test, $"CST Time Now : {cstDateNow}");

                if (cstDateNow.Day == 13 && cstDateNow.Hour >= 20)
                {
                    ReportingMethods.Log(test, $"Expecting not to allow Autopay setup as CST Time now is {cstDateNow} which is greater than cutoff time '8 PM CST 13th of Month'");
                    ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.HelocAutopayNotAllowed, errorMsg, $"Expected {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");
                }
                if (cstDateNow.Day > 13)
                {
                    ReportingMethods.Log(test, $"Expecting not to allow Autopay setup as CST Time now is {cstDateNow} which is greater than cutoff time '8 PM CST 13th of Month'");
                    ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.HelocAutopayNotAllowed, errorMsg, $"Expected {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");
                }
                else
                {
                    ReportingMethods.Log(test, $"Expecting allow Autopay setup as CST Time now is {cstDateNow} which is less than cutoff time '8 PM CST 13th of Month'");
                    ReportingMethods.LogAssertionFalse(test, errorMsg.Contains(Constants.CustomerPortalErrorMsgs.HelocAutopayNotAllowed), $"Expected Not to show any error like {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br><b>TPR-1452- Verify HELOC loan is ineligible for autopay when foreclosure_stop_code in (1,2,3,4,5,6,7,8,9)</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_1452_TPR_VerifyHelocInEligibleMessageForAutopaySetupWithForeclosureStopCode()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForIneligibleForeclosureStopcodes} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForIneligibleForeclosureStopcodes, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (!test.Model.FullName.Contains("Heloc"))
                    loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                var loansFound = loanLevelData.Count;
                if (loansFound == 0)
                {
                    test.Fail($"{loansFound} loans found for the specified criteria.");
                    return;
                }
                else
                    test.Info($"Found <b>{loansFound}</b> Loans");
                #endregion TestData

                commonServices.LoginToTheApplication(username, password);

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                reportLogger.TakeScreenshot(test, "Autopay Setup Page");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.autopayNoTallowedMsgLocBy);

                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.HelocAutopayNotAllowed, errorMsg, $"Expected {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");

            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }




        [TestMethod]
        [Description("<br><b>TPR-1451- Verify HELOC loan is ineligible for autopay when badcheck stop code in  (1,4,6) (</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_1451_TPR_VerifyHelocInEligibleMessageForAutopaySetupWithBadCheckStopCode()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForIneligibleBadCheckStopcodes} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForIneligibleForeclosureStopcodes, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (!test.Model.FullName.Contains("Heloc"))
                    loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                var loansFound = loanLevelData.Count;
                if (loansFound == 0)
                {
                    test.Fail($"{loansFound} loans found for the specified criteria.");
                    return;
                }
                else
                    test.Info($"Found <b>{loansFound}</b> Loans");
                #endregion TestData

                commonServices.LoginToTheApplication(username, password);

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                reportLogger.TakeScreenshot(test, "Autopay Setup Page");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.autopayNoTallowedMsgLocBy);

                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.HelocAutopayNotAllowed, errorMsg, $"Expected {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");

            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }


        }
        [TestMethod]
        [Description("<br><b>TPR-1456- Verify HELOC loan is ineligible for autopay when  pif_stop_code in (1,2,3,4,5,6)</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_1456_TPR_VerifyHelocInEligibleMessageForAutopaySetupWithPIFStopCode()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForIneligiblePIFStopcodes} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForIneligiblePIFStopcodes, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (!test.Model.FullName.Contains("Heloc"))
                    loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                var loansFound = loanLevelData.Count;
                if (loansFound == 0)
                {
                    test.Fail($"{loansFound} loans found for the specified criteria.");
                    return;
                }
                else
                    test.Info($"Found <b>{loansFound}</b> Loans");
                #endregion TestData

                commonServices.LoginToTheApplication(username, password);

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                reportLogger.TakeScreenshot(test, "Autopay Setup Page");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.autopayNoTallowedMsgLocBy);

                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.HelocAutopayNotAllowed, errorMsg, $"Expected {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");

            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }
        [TestMethod]
        [Description("<br><b>TPR-1455- Verify HELOC loan is ineligible for autopay when Process_stop_code in ('2','3','5','8','9','A','B','D','F','H','L','M','N','P','R','U','W','!') </b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_1455_TPR_VerifyHelocInEligibleMessageForAutopaySetupWithProcessStopCode()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForIneligibleProcessStopcodes} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForIneligibleProcessStopcodes, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (!test.Model.FullName.Contains("Heloc"))
                    loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                var loansFound = loanLevelData.Count;
                if (loansFound == 0)
                {
                    test.Fail($"{loansFound} loans found for the specified criteria.");
                    return;
                }
                else
                    test.Info($"Found <b>{loansFound}</b> Loans");
                #endregion TestData

                commonServices.LoginToTheApplication(username, password);

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                reportLogger.TakeScreenshot(test, "Autopay Setup Page");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.autopayNoTallowedMsgLocBy);

                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.HelocAutopayNotAllowed, errorMsg, $"Expected {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");

            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br><b>TPR-514- Verify HELOC loan is ineligible for autopay when INV ID not in (Cxx, L10 or W10) </b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_514_TPR_VerifyHELOCIsIneligibleForAutopayWhenINVIDNotIn_Cxx_L10_W10()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForIneligibleInvCodes} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForIneligibleInvCodes, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (!test.Model.FullName.Contains("Heloc"))
                    loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                var loansFound = loanLevelData.Count;
                if (loansFound == 0)
                {
                    test.Fail($"{loansFound} loans found for the specified criteria.");
                    return;
                }
                else
                    test.Info($"Found <b>{loansFound}</b> Loans");
                #endregion TestData

                commonServices.LoginToTheApplication(username, password);

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                reportLogger.TakeScreenshot(test, "Autopay Setup Page");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.autopayNoTallowedMsgLocBy);

                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.HelocAutopayNotAllowed, errorMsg, $"Expected {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");

            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br><b>TPR-512- Verify HELOC loan is ineligible for autopay when Loan is  prepaid 2 or more months (Next Payment Due Date >= 2 months) </b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_512_TPR_VerifyHELOCIsIneligibleForAutopayWhenPrepay2Months()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForIneligiblePrepay2Months} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForIneligiblePrepay2Months, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (!test.Model.FullName.Contains("Heloc"))
                    loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                var loansFound = loanLevelData.Count;
                if (loansFound == 0)
                {
                    test.Fail($"{loansFound} loans found for the specified criteria.");
                    return;
                }
                else
                    test.Info($"Found <b>{loansFound}</b> Loans");
                #endregion TestData

                commonServices.LoginToTheApplication(username, password);

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                reportLogger.TakeScreenshot(test, "Autopay Setup Page");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.autopayNoTallowedMsgLocBy);

                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.HelocAutopayNotAllowed, errorMsg, $"Expected {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");

            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br><b>TPR-925-Verify HELOC loan is ineligible for autopay when delinquent_payment_count >=2 </b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_925_TPR_VerifyHELOCIsIneligibleForAutopayWhenDelinquentCountMorethanTwo()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForIneligibleDelinquent} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForIneligibleDelinquent, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (!test.Model.FullName.Contains("Heloc"))
                    loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                var loansFound = loanLevelData.Count;
                if (loansFound == 0)
                {
                    test.Fail($"{loansFound} loans found for the specified criteria.");
                    return;
                }
                else
                    test.Info($"Found <b>{loansFound}</b> Loans");
                #endregion TestData

                commonServices.LoginToTheApplication(username, password);

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: false))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                reportLogger.TakeScreenshot(test, "Autopay Setup Page");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.autopayNoTallowedMsgLocBy);

                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.HelocAutopayNotAllowed, errorMsg, $"Expected {Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg}, Actual {errorMsg}");

            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                if (loanLevelData.Count != 0)
                    dBconnect.UpdateBorrowerEmailID(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }
    }
}