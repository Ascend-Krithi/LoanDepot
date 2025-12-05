using AventStack.ExtentReports;
using LD_AgentPortal.Pages;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;


namespace LD_AgentPortal.Tests
{
    [TestClass]
    public class RepaymentTests : BasePage
    {
        public static string loanDetailsQuery;

        string getLoanLevelDetailsForActiveRepayment = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForActiveRepayment));
        string getLoanLevelDetailsForBrokenRepayment = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForBrokenRepayment));
        string getLoanLevelDetailsForDeletedRepayment = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForDeletedRepayment));
        string getLoanLevelDetailsForCompletedActiveRepayment = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForCompletedActiveRepayment));
        string getLoanLevelDetailsByNextOrAfterMonthActiveRepayment = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsByNextOrAfterMonthActiveRepayment));
        string getLoanLevelDetailsByCurrentMonthAfterCutOffTimeActiveRepayment = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsByCurrentMonthAfterCutOffTimeActiveRepayment));

        public TestContext TestContext
        {
            set;
            get;
        }

        [ClassInitialize]
        public static void TestClassInitialize(TestContext TestContext)
        {

        }

        #region ObjectInitialization

        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;
        DBconnect dBconnect = null;
        Pages.PaymentsPage payments = null;
        LD_AutomationFramework.Base.ReportLogger reportLogger = null;
        List<Hashtable> listOfLoanLevelData = null;
        Hashtable loanLevelData = null;
        DashboardPage dashboard = null;
        JiraManager jiraManager = null;
        #endregion ObjectInitialization

        #region CommonTestData
        string deleteReason = "Test Delete Reason";
        string firstName = "TESTFN";
        string lastName = "TESTLN";
        string personalOrBussiness = "personal";
        string savings = "Savings";
        string accountNumber = Constants.BankAccountData.BankAccountNumber;
        string routingNumber = "122199983";
        string bankAccountName = Constants.BankAccountData.BankAccountName;
        string accountFullName = "TESTFN TESTLN";
        string bankAccountNameAfterSelection = "AutoAccount (Loan Depot Bank-"+ Constants.BankAccountData.BankAccountNumber.Substring(Constants.BankAccountData.BankAccountNumber.Length - 4) + ")";

        #endregion CommonTestData

        [TestInitialize]
        public void TestInitialize()
        {
            InitializeFramework(TestContext);
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            dBconnect = new DBconnect(test, "MelloServETL");
            payments = new Pages.PaymentsPage(_driver, test);
            reportLogger = new LD_AutomationFramework.Base.ReportLogger(_driver);
            listOfLoanLevelData = new List<Hashtable>();
            dashboard = new DashboardPage(_driver, test);
        }

        [TestMethod]
        [Description("<br>TC-1384 [SCM_5431] - Verify Make a Payment button is displayed in orange color and confirmation popup is displayed on clicking the Make a Payment button <br>" +
                    "TC-1430 [SCM_5388] - Verify Payment Break down is not displayed for Active Repayment Plan in Setup/Edit screen <br>" +
                    "TC-1361 [SCM_5401] - Verify Total Reinstatement field is not displayed in OTP Setup screen for Active Repayment plan <br>" +
                    "TC-1359 [SCM_5404] - Verify Prepay is not displayed in OTP Setup screen for Active Repayment plan <br>" +
                    "TC-1357 [SCM_5407] - Verify the Payment dates in the one time payment screen when the user is having active Repayment plan and due is current month and current date is before cut off time <br>" +
                    "TC-461 [SCM_5869] - Validate Additional Payments field displayed when loan has an active repayment plan in OTP setup/edit Payment screens <br>" +
                    "TC-1378 [SCM_5438] - Verify the edit payment page when loan has Active Repayment Plan <br>" +
                    "TC-845 [SCM_5437] - Verify Update button is disabled when there is no update made and on clicking go back to Payments user is navigated to Payments angular tab <br>" +
                    "TC-1426 [SCM_5395] - Verify payment is successfully submitted when Additional amount is entered along with Repayment Plan amount")]
        [TestCategory("AP_Regression"), TestCategory("AP_Repayment")]
        public void TC_1384_1430_1361_1359_1357_461_1378_845_1426_TC_VerifyActiveRepaymentSetupEditCancel()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForActiveRepayment;

            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForDefaultsPastMonths(Constants.DefaultsPlan.Repayment))
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
            }
            loanLevelData = listOfLoanLevelData[retryCount];

            string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
               loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
               $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);

            //SCM_5431: Verification 1 - Make a payment button is Orange in color            
            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Orange, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Orange' in color for the selected Repayment loan.",
                                             "Failure - 'Make a Payment' button is not Orange in color for the selected Repayment loan.");
            reportLogger.TakeScreenshot(test, "Make a Payment' button");

            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            webElementExtensions.WaitForVisibilityOfElement(_driver, payments.activeRepaymentPlanAlertTextLocBy);
            webElementExtensions.WaitForElement(_driver, payments.activeRepaymentPlanAlertTextLocBy);
            flag = webElementExtensions.IsElementDisplayed(_driver, payments.activeRepaymentPlanAlertTextLocBy, "Active Repayment plan Alert");
            _driver.ReportResult(test, flag, "Successfully verified that Active Repayment plan Alert displayed.",
                                            "Failure - Active Repayment plan Alert not displayed.");
            reportLogger.TakeScreenshot(test, "Active Repayment plan Alert");

            payments.ClickButtonUsingName(Constants.ButtonNames.Confirm, Constants.AlertPopupNames.ActiveRepaymentPlanAlert);
            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);

            //SCM_5388: Verification 2 - Verify Payment Break down is not displayed for Active Repayment Plan in Setup / Edit screen
            payments.VerifyPaymentBreakdownSectionNotVisible();
            payments.VerifyOTPScreenDetails(loanLevelData, Constants.Plans.Active);
            Hashtable hashData = payments.GetPromiseDetails(loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString());
            string bannerMesssage = Constants.BannerDetails.YourLoanCurrentlyOnAnActiveRepaymentPlan + Convert.ToInt32(hashData[Constants.PromiseDataColumns.PromiseNumber]) + " of " + Convert.ToInt32(hashData[Constants.PromiseDataColumns.TotalPromiseNumber]) + " is due";
            payments.VerifyBannerPastDueDateDetails(loanLevelData, Constants.Plans.Active, bannerMesssage);
            flag = webElementExtensions.VerifyElementColor(Constants.Colors.Red, payments.pastDueBannerLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Past Due' banner is 'Red' in color for the selected Repayment loan.",
                                             "Failure - 'Past Due' Banner is not Red in color for the selected  Repayment loan.");
            reportLogger.TakeScreenshot(test, "Past Due");

            //SSCM_5401: Verification 3 - Verify Total Reinstatement field is not displayed in OTP Setup screen for Active Repayment plan           
            flag = webElementExtensions.IsElementDisplayed(_driver, payments.partialReInstatementRadioButtonLocBy);
            _driver.ReportResult(test, !flag, "Successfully verified that  Total Reinstatement field is not displayed.",
                                             "Failure -  Total Reinstatement field is not displayed.");
            reportLogger.TakeScreenshot(test, "Total Reinstatement");

            //SCM_5404: Verification 4 - Verify Prepay is not displayed in OTP Setup screen for Active Repayment plan           
            flag = webElementExtensions.IsElementDisplayed(_driver, payments.prepayCheckboxLocBy);
            _driver.ReportResult(test, !flag, "Successfully verified that  Prepay Checkbox field is not displayed.",
                                             "Failure -  Prepay Checkbox field is displayed.");
            reportLogger.TakeScreenshot(test, "Prepay Checkbox");

            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);

            //SCM_5869: Verification 5 - Validate Additional Payments field displayed when loan has an active repayment plan in OTP setup/edit Payment screens
            //SCM_5395: Verification 6 - Verify payment is successfully submitted when Additional amount is entered along with Repayment Plan amount
            string totalPayment = payments.VerifyRepaymentPaymentFields(10.00);

            //SCM_5407: Verification 7 - Verify the Payment dates in the one time payment screen when the user is having active Repayment plan and due is current month and current date is before cut off time
            webElementExtensions.ScrollIntoView(_driver, payments.paymentDatePickerIconLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, payments.paymentDatePickerIconLocBy);
            string workingDate;
            if (hashData[Constants.PromiseDataColumns.AllDatesEnabledStatus].ToString().ToUpper() == "ALL")//means all dates enabled
            {
                commonServices.VerifyEnabledDisabledPaymentDatesForMonth();
                commonServices.VerifyMinimumPayDateOfMonthNotBankingHoliday();
                commonServices.VerifyMaximumPayDateIsLastDayOfMonthNotBankingHoliday();
                workingDate = commonServices.GetWorkingDateAfter16thDate();
            }
            else//means only 1 date enabled
            {
                workingDate = commonServices.GetWorkingDate();
            }
            payments.SelectPaymentDateInDateField(workingDate);
            payments.VerifyLateFeesWarningMsgNotDisplayed();
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyReviewAndConfirmationPageIsDisplayed();
            payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
            payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment);
            payments.ClickConfirmButtonPaymentReviewPage();
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
            string confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
            payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
            payments.CloseNotesTab();

            //SCM_5438: Verification 8 - Verify the edit payment page when loan has Active Repayment Plan
            payments.EditNewlyAddedPayment(confirmationNumber);
            payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
            payments.VerifyPaymentBreakdownSectionNotVisible(Constants.PaymentFlowType.Edit);
            payments.VerifyOTPScreenDetails(loanLevelData, Constants.Plans.Active, Constants.PaymentFlowType.Edit);
            payments.VerifyBannerPastDueDateDetails(loanLevelData, Constants.PaymentFlowType.Edit, bannerMesssage);
            flag = webElementExtensions.VerifyElementColor(Constants.Colors.Red, payments.pastDueBannerLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that Edit 'Past Due' banner is 'Red' in color for the selected Repayment loan.",
                                             "Failure - 'Past Due' Edit Banner is not Red in color for the selected  Repayment loan.");
            reportLogger.TakeScreenshot(test, "Past Due Banner");
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
            payments.VerifyAuthorizedByIsDisabled();
            webElementExtensions.IsElementEnabled(_driver, commonServices.bankAccountDropdownValueSelectedLocBy, "Bank Account Method");
            webElementExtensions.IsElementEnabled(_driver, payments.paymentDatePickerIconLocBy, "Date");
            webElementExtensions.IsElementEnabled(_driver, payments.additionalPaymentTextLocBy, "Additional Payment");
            flag = webElementExtensions.IsElementDisplayed(_driver, payments.lateFeeTextboxLocBy);
            _driver.ReportResult(test, !flag, "Successfully verified that  Late Fee field is not displayed.",
            "Failure -  Late Fee field is displayed.");
            reportLogger.TakeScreenshot(test, "Late Fee");

            //SCM_5437: Verification 9 - Verify Update button is disabled when there is no update made and on clicking go back to Payments user is navigated to Payments angular tab
            payments.VerifyUpdatePaymentButtonIsDisabled(Constants.PaymentFlowType.Edit);
            if (hashData[Constants.PromiseDataColumns.AllDatesEnabledStatus].ToString().ToUpper() == "ALL")//means all dates enabled
            {
                workingDate = commonServices.GetWorkingDateAfter16thDate(Constants.PaymentFlowType.Edit);
                payments.SelectPaymentDateInDateField(workingDate);
                payments.VerifyUpdatePaymentButtonIsEnabled(Constants.PaymentFlowType.Edit);
            }
            else
            {
                totalPayment = payments.VerifyRepaymentPaymentFields(10.00, 20.00, Constants.PaymentFlowType.Edit);
            }
            payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
            payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PaymentFlowType.Edit);
            payments.VerifyTotalAmountPaymentReviewPage(totalPayment, Constants.PaymentFlowType.Edit);
            payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment, Constants.PaymentFlowType.Edit);
            payments.ClickConfirmButtonPaymentReviewPage();
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PaymentFlowType.Edit);
            confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
            payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
            if (ConfigSettings.PaymentsDataDeletionRequired)
            {
                payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                payments.VerifyPaymentDeletionMessageIsDisplayed();
                payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
                payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
                payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
            }
        }

        [TestMethod]
        [Description("<br>TC-1371 [SCM_5412] - Verify that the user is able to setup OTP payment when the loan is Deleted from Active Repayment plan and next payment due is previous month or past months <br>" +
            "TC-1369 [SCM_5414] - Verify that the user is able to setup OTP payment when the loan is Broken from Active Repayment plan and next payment due is previous month or past months<br>" +
            "TC-1366 [SCM_5424] - Verify Make a payment button is displayed in orange color when loan has Broken/Deleted Repayment Plan and  IsEligible = Allowed with Override due to Error_Code 'PaymentPlanDelinquentDueToPlanBroken' and NextPaymentDue date is in past months<br>" +
            "TC-1383 [SCM_5435] - Verify late Fee message is displayed when loan has Broken/Deleted or Completed Repayment Plan<br>" +
            "TC-1413 [SCM_5439] - Verify the edit payment page when loan has Broken/Deleted Repayment Plan")]
        [TestCategory("AP_Regression"), TestCategory("AP_Repayment")]
        public void TC_1371_1369_1366_1383_1413_TC_VerifyUserAbleToSetUpOTPPaymentWhenLoanIsBrokenOrDeletedFromActiveRepaymentPlanAndNextPaymentDueIsPastMonths()
        {
            string[] planTypes = { Constants.Plans.Broken, Constants.Plans.Deleted };

            commonServices.LoginToTheApplication(username, password);

            foreach (string planType in planTypes)
            {
                int retryCount = 0;

                #region TestData
                if (planType == Constants.Plans.Broken)
                {
                    loanDetailsQuery = getLoanLevelDetailsForBrokenRepayment;
                }
                else
                {
                    loanDetailsQuery = getLoanLevelDetailsForDeletedRepayment;
                }
                listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery, true);
                #endregion TestData
                if (listOfLoanLevelData.Count > 0)
                {
                    while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
                    {
                        if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                        {
                            if (dashboard.VerifyIfLoanNumberIsEligibleForDefaultsPastMonths(Constants.DefaultsPlan.Repayment))
                                break;
                        }
                        retryCount++;
                        if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                            test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    }
                    loanLevelData = listOfLoanLevelData[retryCount];

                    ReportingMethods.Log(test, "*****Verify that the user is able to setup OTP payment when the loan is " + planType + " from Active Repayment plan and next payment due is previous month or past months*****");

                    string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                                      loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                                      $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);

                    bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Orange, payments.makeAPaymentButtonLocBy);
                    _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Orange' in color for the selected Repayment loan.",
                                                     "Failure - 'Make a Payment' button is not Orange in color for the selected Repayment loan.");
                    reportLogger.TakeScreenshot(test, "Make a Payment' button");
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                    webElementExtensions.WaitForElement(_driver, payments.planBrokenOrDeletedAlertTextBy);
                    flag = webElementExtensions.IsElementDisplayed(_driver, payments.planBrokenOrDeletedAlertTextBy, planType + " Repayment plan Alert");
                    _driver.ReportResult(test, flag, "Successfully verified that " + planType + " Repayment plan Alert displayed.",
                                                    "Failure - " + planType + " Repayment plan Alert not displayed.");
                    reportLogger.TakeScreenshot(test, planType + " Repayment plan Alert");

                    payments.ClickButtonUsingName(Constants.ButtonNames.Confirm, Constants.AlertPopupNames.BrokenRepaymentPlanAlert);
                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    payments.VerifyOTPScreenDetails(loanLevelData, planType);
                    Hashtable hashData = payments.GetPromiseDetails(loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString());
                    payments.VerifyBannerDetails(loanLevelData, planType);
                    payments.SelectValueInAuthorizedByDropdown(borrower);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                    string totalPayment = payments.VerifyRepaymentBrokenDeletePaymentFields(10.00, 20.00);
                    webElementExtensions.ScrollIntoView(_driver, payments.paymentDatePickerIconLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.paymentDatePickerIconLocBy);
                    string currentWorkingDate;
                    if (hashData[Constants.PromiseDataColumns.AllDatesEnabledStatus].ToString().ToUpper() == "ALL")//means all dates enabled
                    {
                        commonServices.VerifyEnabledDisabledPaymentDatesForMonth();
                        commonServices.VerifyMinimumPayDateOfMonthNotBankingHoliday();
                        commonServices.VerifyMaximumPayDateIsLastDayOfMonthNotBankingHoliday();
                        currentWorkingDate = commonServices.GetWorkingDateAfter16thDate();
                        payments.SelectPaymentDateInDateField(currentWorkingDate);
                        flag = webElementExtensions.IsElementDisplayed(_driver, payments.lateFeeDateMsgDivLocBy);
                        _driver.ReportResult(test, flag, "Successfully verified that  Late Fee Date Message field is displayed.",
                                                         "Failure -  Late Fee Date Message field is not displayed.");
                        reportLogger.TakeScreenshot(test, "Late Fee Date Message");
                    }
                    else//means only 1 date enabled
                    {
                        currentWorkingDate = commonServices.GetWorkingDate();
                        payments.SelectPaymentDateInDateField(currentWorkingDate);
                    }
                    flag = webElementExtensions.IsElementDisplayed(_driver, payments.lateFeeDateMsgDivLocBy);
                    _driver.ReportResult(test, flag, "Successfully verified that  Late Fee Date Message field is displayed.",
                                                     "Failure -  Late Fee Date Message field is not displayed.");
                    reportLogger.TakeScreenshot(test, "Late Fee Date Message");
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                    payments.VerifyReviewAndConfirmationPageIsDisplayed();
                    payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                    payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, currentWorkingDate, totalPayment);
                    payments.ClickConfirmButtonPaymentReviewPage();
                    payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
                    string confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
                    payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
                    payments.CloseNotesTab();

                    payments.EditNewlyAddedPayment(confirmationNumber);
                    payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
                    payments.VerifyPaymentBreakdownSectionNotVisible(Constants.PaymentFlowType.Edit);
                    payments.VerifyOTPScreenDetails(loanLevelData, planType, Constants.PaymentFlowType.Edit);
                    payments.VerifyBannerDetails(loanLevelData, planType, Constants.PaymentFlowType.Edit);
                    flag = webElementExtensions.VerifyElementColor(Constants.Colors.Red, payments.delinquentDivLocBy);
                    _driver.ReportResult(test, flag, "Successfully verified edit that 'Delinquent' banner is 'Red' in color for the selected Repayment loan.",
                                                     "Failure -Edit 'Delinquent' Banner is not Red in color for the selected  Repayment loan.");
                    reportLogger.TakeScreenshot(test, "Delinquent");
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
                    payments.VerifyAuthorizedByIsDisabled();
                    webElementExtensions.IsElementEnabled(_driver, commonServices.bankAccountDropdownValueSelectedLocBy, "Bank Account Method");
                    webElementExtensions.IsElementEnabled(_driver, payments.paymentDatePickerIconLocBy, "Date");

                    //Bug - SCM-6492
                    //payments.VerifyUpdatePaymentButtonIsDisabled(Constants.PaymentFlowType.Edit);

                    totalPayment = payments.VerifyRepaymentBrokenDeletePaymentFields(10.00, 20.00, 15.00, 25.00, Constants.PaymentFlowType.Edit);
                    payments.VerifyUpdatePaymentButtonIsEnabled(Constants.PaymentFlowType.Edit);
                    payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
                    payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PaymentFlowType.Edit);
                    payments.VerifyTotalAmountPaymentReviewPage(totalPayment, Constants.PaymentFlowType.Edit);
                    payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, currentWorkingDate, totalPayment, Constants.PaymentFlowType.Edit);
                    payments.ClickConfirmButtonPaymentReviewPage();
                    payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PaymentFlowType.Edit);
                    confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
                    payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
                    if (ConfigSettings.PaymentsDataDeletionRequired)
                    {
                        payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                        payments.VerifyPaymentDeletionMessageIsDisplayed();
                        payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
                        payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
                        payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
                    }
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-1370 [SCM-5415] - Test FUNC - Verify Completed Active Repayment plan Setup_Edit_Cancel<br>" +
                    "TC-1383 [SCM-5435] - Verify late Fee message is displayed when loan has Broken/Deleted or Completed Repayment Plan<br>" +
                    "TC-1410 [SCM-5440] - Verify the edit payment page when loan has Completed Repayment Plan")]
        [TestCategory("AP_Regression"), TestCategory("AP_Repayment")]
        public void TC_1370_1383_1410_TC_VerifyCompletedActiveRepaymentPlanSetupEditCancel()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForCompletedActiveRepayment;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForCurrentMonth())
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
            }
            loanLevelData = listOfLoanLevelData[retryCount];

            string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
               loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
               $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);

            //SCM_5431: Verification 1 - Make a payment button is Purple in color
            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected Repayment loan.",
                                             "Failure - 'Make a Payment' button is not Purple in color for the selected Repayment loan.");
            reportLogger.TakeScreenshot(test, "Make a Payment' button");
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
            payments.VerifyOTPScreenDetails(loanLevelData, Constants.Plans.Completed);
            Hashtable hashData = payments.GetPromiseDetails(loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString());
            payments.VerifyBannerDetails(loanLevelData, Constants.Plans.Completed);
            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
            string accountFullName = firstName + " " + lastName;
            string totalPayment = payments.VerifyCompletedActiveRepaymentPaymentFields(loanLevelData, 10.00, 15.00);
            webElementExtensions.ScrollIntoView(_driver, payments.paymentDatePickerIconLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, payments.paymentDatePickerIconLocBy);
            string workingDate;
            if (hashData[Constants.PromiseDataColumns.AllDatesEnabledStatus].ToString().ToUpper() == "ALL")//means all dates enabled
            {
                commonServices.VerifyEnabledDisabledPaymentDatesForMonth();
                commonServices.VerifyMinimumPayDateOfMonthNotBankingHoliday();
                commonServices.VerifyMaximumPayDateIsLastDayOfMonthNotBankingHoliday();
                workingDate = commonServices.GetWorkingDateAfter16thDate();
                payments.SelectPaymentDateInDateField(workingDate);
                flag = webElementExtensions.IsElementDisplayed(_driver, payments.lateFeeDateMsgDivLocBy);
                _driver.ReportResult(test, flag, "Successfully verified that  Late Fee Date Message field is displayed.",
                                                 "Failure -  Late Fee Date Message field is not displayed.");
                reportLogger.TakeScreenshot(test, "Late Fee Date Message");
            }
            else//means only 1 date enabled
            {
                workingDate = commonServices.GetWorkingDate();
                payments.SelectPaymentDateInDateField(workingDate);
            }
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyReviewAndConfirmationPageIsDisplayed();
            payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
            payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment);
            payments.ClickConfirmButtonPaymentReviewPage();
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
            string confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
            payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
            payments.CloseNotesTab();

            payments.EditNewlyAddedPayment(confirmationNumber);
            payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
            payments.VerifyOTPScreenDetails(loanLevelData, Constants.Plans.Completed, Constants.PaymentFlowType.Edit);
            payments.VerifyBannerDetails(loanLevelData, Constants.Plans.Completed, Constants.PageNames.EditPayment);
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
            payments.VerifyAuthorizedByIsDisabled();
            webElementExtensions.IsElementEnabled(_driver, commonServices.bankAccountDropdownValueSelectedLocBy, "Bank Account Method");
            webElementExtensions.IsElementEnabled(_driver, payments.paymentDatePickerIconLocBy, "Date");
            totalPayment = payments.VerifyCompletedActiveRepaymentPaymentFields(loanLevelData, 10.00, 15.00, 20.00, 25.00, Constants.PaymentFlowType.Edit);
            payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
            payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PaymentFlowType.Edit);
            payments.VerifyTotalAmountPaymentReviewPage(totalPayment, Constants.PaymentFlowType.Edit);
            payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment, Constants.PaymentFlowType.Edit);
            payments.ClickConfirmButtonPaymentReviewPage();
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PaymentFlowType.Edit);
            confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
            payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
            if (ConfigSettings.PaymentsDataDeletionRequired)
            {
                payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                payments.VerifyPaymentDeletionMessageIsDisplayed();
                payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
                payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
                payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
            }
        }

        [TestMethod]
        [Description("<br>TC-1354 [SCM-5408] - Verify the Payment dates in the one time payment screen when the user is having active Repayment plan and due is current month and current date is after cut off time<br>" +
                     "TC-1355 [SCM-5409] - Verify the Payment dates in the one time payment screen when the user is having active Repayment plan and due is Next month or after. <br>" +
                     "TC-947 [SCM-5654] - Verify the installment message and amount displayed when the status_code is blank when loan has active repayment plan")]
        [TestCategory("AP_Regression"), TestCategory("AP_Repayment")]
        public void TC_1354_1355_947_TC_VerifyUserAbleToSubmitOTPSuccessfullyForActiveRepaymentWhenCurrentAndNextMonthDateAfterCutOffTime()
        {

            #region TestData
            int retryCount = 0;

            string[] loanDetailsQueries = { getLoanLevelDetailsByCurrentMonthAfterCutOffTimeActiveRepayment, getLoanLevelDetailsByNextOrAfterMonthActiveRepayment };

            #endregion TestData
            commonServices.LoginToTheApplication(username, password);

            foreach (string query in loanDetailsQueries)
            {
                loanDetailsQuery = query;
                listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

                string queryMonth = query.Contains("Current") ? "Current" : "Next";
                ReportingMethods.Log(test, "*****Verify the user able to submit OTP successfully for Active when next payment due date is " + queryMonth + " month*****");

                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
                {
                    if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                    {
                        if (dashboard.VerifyIfLoanNumberIsEligibleForDefaultsPastMonths(Constants.DefaultsPlan.Repayment))
                            break;
                    }
                    retryCount++;
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                }
                loanLevelData = listOfLoanLevelData[retryCount];

                string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                   loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                   $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                payments.DeleteAllExistingPendingPayments(deleteReason);

                //Verification 1 - Make a payment button is Orange in color
                bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Orange, payments.makeAPaymentButtonLocBy);
                _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Orange' in color for the selected Repayment loan.",
                                                 "Failure - 'Make a Payment' button is not Orange in color for the selected Repayment loan.");
                reportLogger.TakeScreenshot(test, "Make a Payment' button");

                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                webElementExtensions.WaitForElement(_driver, payments.activeRepaymentPlanAlertTextLocBy);
                flag = webElementExtensions.IsElementDisplayed(_driver, payments.activeRepaymentPlanAlertTextLocBy, "Active Repayment plan Alert");
                _driver.ReportResult(test, flag, "Successfully verified that Active Repayment plan Alert displayed.",
                                                "Failure - Active Repayment plan Alert not displayed.");
                reportLogger.TakeScreenshot(test, "Active Repayment plan Alert");

                payments.ClickButtonUsingName(Constants.ButtonNames.Confirm, Constants.AlertPopupNames.ActiveRepaymentPlanAlert);
                payments.VerifyOTPPageIsDisplayed();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                payments.VerifyPaymentBreakdownSectionNotVisible();
                payments.VerifyOTPScreenDetails(loanLevelData, Constants.Plans.Active);
                Hashtable hashData = payments.GetPromiseDetails(loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString());
                string bannerMesssage = Constants.BannerDetails.YourLoanCurrentlyOnAnActiveRepaymentPlan + Convert.ToInt32(hashData[Constants.PromiseDataColumns.PromiseNumber]) + " of " + Convert.ToInt32(hashData[Constants.PromiseDataColumns.TotalPromiseNumber]) + " is due";
                payments.VerifyBannerPastDueDateDetails(loanLevelData, Constants.Plans.Active, bannerMesssage);
                flag = webElementExtensions.VerifyElementColor(Constants.Colors.Red, payments.pastDueBannerLocBy);
                _driver.ReportResult(test, flag, "Successfully verified that 'Repayment Account Standing Banner details is Red' in color for the selected Repayment loan.",
                                                 "Failure - 'Repayment Account Standing Banner details' is not Red in color for the selected Repayment loan.");
                reportLogger.TakeScreenshot(test, "Repayment Account Standing Banner' button");
                flag = webElementExtensions.IsElementDisplayed(_driver, payments.partialReInstatementRadioButtonLocBy);
                _driver.ReportResult(test, !flag, "Successfully verified that Total Reinstatement field is not displayed.",
                                                "Failure - Total Reinstatement field is displayed.");
                flag = webElementExtensions.IsElementDisplayed(_driver, payments.prepayCheckboxLocBy);
                _driver.ReportResult(test, !flag, "Successfully verified that Prepay Checkbox field is not displayed.",
                                                "Failure - Prepay Checkbox field is displayed.");
                payments.SelectValueInAuthorizedByDropdown(borrower);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                string accountFullName = firstName + " " + lastName;
                string totalPayment = payments.VerifyRepaymentPaymentFields(10.00);
                webElementExtensions.ScrollIntoView(_driver, payments.paymentDatePickerIconLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.paymentDatePickerIconLocBy);
                string workingDate;
                if (hashData[Constants.PromiseDataColumns.AllDatesEnabledStatus].ToString().ToUpper() == "ALL")//means all dates enabled
                {
                    commonServices.VerifyEnabledDisabledPaymentDatesForMonth();
                    commonServices.VerifyMinimumPayDateOfMonthNotBankingHoliday();
                    commonServices.VerifyMaximumPayDateIsLastDayOfMonthNotBankingHoliday();
                    workingDate = commonServices.GetWorkingDateAfter16thDate(Constants.PaymentFlowType.SetUp);
                    payments.SelectPaymentDateInDateField(workingDate);
                }
                else//means only 1 date enabled
                {
                    workingDate = commonServices.GetWorkingDate(Constants.PaymentFlowType.SetUp);
                    payments.SelectPaymentDateInDateField(workingDate);
                }
                payments.VerifyLateFeesWarningMsgNotDisplayed();
                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed();
                string totalAmount = payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment);
                payments.ClickConfirmButtonPaymentReviewPage();
                payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
                string confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
                payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
                payments.CloseNotesTab();

                payments.EditNewlyAddedPayment(confirmationNumber);
                payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
                payments.VerifyPaymentBreakdownSectionNotVisible(Constants.PaymentFlowType.Edit);
                payments.VerifyOTPScreenDetails(loanLevelData, Constants.Plans.Active, Constants.PaymentFlowType.Edit);
                payments.VerifyBannerPastDueDateDetails(loanLevelData, Constants.PaymentFlowType.Edit);
                flag = webElementExtensions.VerifyElementColor(Constants.Colors.Red, payments.pastDueBannerLocBy);
                _driver.ReportResult(test, flag, "Successfully verified edit that 'Repayment Account Standing Banner details is Red' in color for the selected Repayment loan.",
                                                 "Failure - 'Edit Repayment Account Standing Banner details' is not Red in color for the selected Repayment loan.");
                reportLogger.TakeScreenshot(test, "Edit Repayment Account Standing Banner' button");
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
                payments.VerifyAuthorizedByIsDisabled();
                webElementExtensions.IsElementEnabled(_driver, commonServices.bankAccountDropdownValueSelectedLocBy, "Bank Account Method");
                webElementExtensions.IsElementEnabled(_driver, payments.paymentDatePickerIconLocBy, "Date");
                webElementExtensions.IsElementEnabled(_driver, payments.additionalPaymentTextLocBy, "Additional Payment");
                flag = webElementExtensions.IsElementDisplayed(_driver, payments.lateFeeTextboxLocBy);
                _driver.ReportResult(test, !flag, "Successfully verified thatLate Fee field is not displayed.",
                                                "Failure -Late Fee field is displayed.");
                //Bug - SCM-6492
                //payments.VerifyUpdatePaymentButtonIsDisabled(Constants.PaymentFlowType.Edit);

                if (hashData[Constants.PromiseDataColumns.AllDatesEnabledStatus].ToString().ToUpper() == "ALL")//means all dates enabled
                {
                    workingDate = commonServices.GetWorkingDateAfter16thDate(Constants.PaymentFlowType.Edit);
                    payments.SelectPaymentDateInDateField(workingDate);
                }
                else//means only 1 date enabled
                {
                    workingDate = commonServices.GetWorkingDate(Constants.PaymentFlowType.Edit);
                    payments.SelectPaymentDateInDateField(workingDate);
                }
                payments.VerifyUpdatePaymentButtonIsEnabled(Constants.PaymentFlowType.Edit);
                payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PaymentFlowType.Edit);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment, Constants.PaymentFlowType.Edit);
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment, Constants.PaymentFlowType.Edit);
                payments.ClickConfirmButtonPaymentReviewPage();
                payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PaymentFlowType.Edit);
                confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
                payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
                if (ConfigSettings.PaymentsDataDeletionRequired)
                {
                    payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                    payments.VerifyPaymentDeletionMessageIsDisplayed();
                    payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
                    payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
                    payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
                }
            }

        }

    }
}
