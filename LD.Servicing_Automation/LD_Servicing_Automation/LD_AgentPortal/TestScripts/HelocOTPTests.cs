using AventStack.ExtentReports;
using LD_AgentPortal.Pages;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;


namespace LD_AgentPortal.Tests
{
    [TestClass]
    public class HelocOTPTests : BasePage
    {
        public static string loanDetailsQuery;

        string getLoanLevelDetailsForOnTimeHelocOTP = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOnTimeHelocOTP));
        string getLoanLevelDetailsForPastDueHelocOTP = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForPastDueHelocOTP));
        string getLoanLevelDetailsForPastDueHelocOTPWithSoftProcessStopCode = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForPastDueHelocOTPWithSoftProcessStopCode));
        string getLoanLevelDetailsForPastDueOrDelinquentHelocOTP = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForPastDueOrDelinquentHelocOTP));
        string getLoanLevelDetailsForOnTimeOrPrepaidHelocOTP = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOnTimeOrPrepaidHelocOTP));
        string getLoanLevelDetailsForOnTimeWithUPBHelocOTP = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOnTimeWithUPBHelocOTP));

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
        string bankAccountNameAfterSelection = "" + Constants.BankAccountData.BankAccountName + " (Loan Depot Bank-" + Constants.BankAccountData.BankAccountNumber.Substring(Constants.BankAccountData.BankAccountNumber.Length - 4) + ")";
        Dictionary<string, string> loanTypes = new Dictionary<string, string>
            {
            { "0", "On-time" },
            { "1", "Past Due" },
            { "2", "Delinquent" },
            { "3", "Delinquent+" },
            { "Prepaid", "Prepaid" }
            };

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
        [Description("<br> TC-1006 SCM-5809-Test-Verify functionality for 'Next Payment' in header through 16th of the month[If 15th falls on Holiday] <br>" +
                     "TC-1072 [SCM-6179] - Test-Verify functionality for 'Next Payment' in header through 16th of the month [If 15th falls on Mon thru Thurs (non-holiday/non-weekend)]<br>" +
                     "TC-976 [SCM-6182] - Test-Verify functionality for 'Next Payment' in header through 16th of the month[If 15th falls on Friday (non-holiday)]<br>" +
                     "TC-1009 [SCM-6386] - Test-Verify functionality for 'Next Payment' in header through 16th of the month[If 15th falls on Saturday or Sunday]<br>" +
                     "TC-1002 SCM-6034-Test HELOC - Delta - verify Show Message In Payment Breakdown Through 16th of the Month - Ontime[If 15th falls on Holiday]<br>" +
                     "TC-1004 SCM-6034-Test HELOC - Delta - verify Show Message In Payment Breakdown Through 16th of the Month - Ontime[If 15th falls on Friday (non-holiday)]<br>" +
                     "TC-1005 SCM-6034-Test HELOC - Delta - verify Show Message In Payment Breakdown Through 16th of the Month - Ontime[If 15th falls on Saturday or Sunday]<br>" +
                     "TC-1071 SCM-6034-Test HELOC - Delta - verify Show Message In Payment Breakdown Through 16th of the Month - Ontime [If 15th falls on Mon thru Thurs (non-holiday/non-weekend)]<br>" +
                     "TC-1074 SCM-5810-Test-Verify 'Next Due' Amount is displayed as actual amount when current date is >= 17th of the month<br>" +
            "TC-1075 SCM-5810-Test-Verify OTP screen when current date is >= 17th of the month")]
        [TestCategory("AP_Regression"), TestCategory("AP_HELOC_OTP")]
        public void TC_1006_1072_976_1009_1002_1004_1005_1071_1074_1075_TC_VerifyNextPaymentInHeaderAndPaymentBreakdownThrough16thOfMonth()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForOnTimeHelocOTP;

            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    webElementExtensions.WaitForVisibilityOfElement(_driver, By.XPath(dashboard.apNavigationTabsLocBy.Replace("<TABNAME>", Constants.AgentPortalTabNames.PaymentsTab)));
                    dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
                    webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                    if (webElementExtensions.IsElementEnabled(_driver, payments.makeAPaymentButtonLocBy))
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
            APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData);
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPageForHeloc(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);

            //Verification 1 - Make a payment button is Purple in color
            webElementExtensions.MoveToElement(_driver, payments.searchPaymentActivityButtonLocBy);
            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected Heloc loan.",
                                                 "Failure - 'Make a Payment' button is not Purple in color for the selected Heloc loan.");
            reportLogger.TakeScreenshot(test, "Make a Payment' button");
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
            payments.VerifyOTPScreenDetailsForHeloc(loanLevelData);
            payments.VerifyBannerDetailsForHeloc(loanLevelData);
            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
            string accountFullName = firstName + " " + lastName;
            string totalPayment = payments.VerifyHelocPaymentFields(loanLevelData, helocLoanInfo, 10.00);
            webElementExtensions.ActionClick(_driver, payments.paymentDatePickerIconLocBy);
            commonServices.VerifyEnabledDisabledPaymentDatesForMonth();
            commonServices.VerifyMinimumPayDateOfMonthNotBankingHoliday();
            commonServices.VerifyMaximumPayDateIsLastDayOfMonthNotBankingHoliday();
            string workingDate = commonServices.GetWorkingDateAfter16thDate();
            commonServices.SelectPaymentDateInDateField(workingDate);
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

            //Verify the edit payment page when loan has Heloc Plan
            payments.EditNewlyAddedPayment(confirmationNumber);
            payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
            payments.VerifyOTPScreenDetailsForHeloc(loanLevelData, Constants.PaymentFlowType.Edit);
            payments.VerifyBannerDetailsForHeloc(loanLevelData, Constants.PaymentFlowType.Edit);
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
            totalPayment = payments.VerifyHelocPaymentFields(loanLevelData, helocLoanInfo, 10.00, 15.00, Constants.PaymentFlowType.Edit);
            workingDate = commonServices.GetWorkingDateAfter16thDate(Constants.PaymentFlowType.Edit);
            payments.SelectPaymentDateInDateField(workingDate);
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

        [TestMethod]
        [Description("<br>TC-645 [SCM-6478] - Test-verify HELOC - Delta - Make a Payment Screen UI Validations Amount field- Past Due<br>" +
                     "TC-635 [SCM-6479] - Test-verify HELOC - Delta - Make a Payment Screen UI Validations Additional payment field - Past Due<br>" +
                     "TC-637 [SCM-6480] - TestSet-HELOC - Delta - Make a Payment Screen UI Validations Fees field - Past Due<br>" +
                     "TC-632 [SCM-6481] - Test-verify HELOC - Delta - Make a Payment Screen UI Validations Date field - Past Due"
                     )]
        [TestCategory("AP_Regression"), TestCategory("AP_HELOC_OTP")]
        public void TC_645_635_637_632_TC_VerifyMakeAPaymentScreenUIValidationsForAmountFeesDateAdditionalPaymentFieldsOfPastDueLoan()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForPastDueHelocOTP;

            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    webElementExtensions.WaitForVisibilityOfElement(_driver, By.XPath(dashboard.apNavigationTabsLocBy.Replace("<TABNAME>", Constants.AgentPortalTabNames.PaymentsTab)), ConfigSettings.LongWaitTime);
                    dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
                    webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                    if (webElementExtensions.IsElementEnabled(_driver, payments.makeAPaymentButtonLocBy))
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
            payments.VerifyPaymentSummaryDetailsOnPaymentPageForHeloc(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);

            //Verification 1 - Make a payment button is Purple in color

            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected Heloc loan.",
                                                 "Failure - 'Make a Payment' button is not Purple in color for the selected Heloc loan.");
            reportLogger.TakeScreenshot(test, "Make a Payment' button");
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
            payments.VerifyOTPScreenDetailsForHeloc(loanLevelData);
            payments.VerifyBannerDetailsForHeloc(loanLevelData);
            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
            string accountFullName = firstName + " " + lastName;
            flag = webElementExtensions.IsElementDisplayed(_driver, payments.monthlyPaymentPastDueCheckedAndDisabledLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that Heloc plan Monthly Paymnet PastDue Check Box is Checked intially and  Disabled.",
                                            "Failure - Heloc plan Alert not Monthly Paymnet PastDue Check Box is Checked intially and  Disabled..");
            reportLogger.TakeScreenshot(test, "Heloc plan Monthly Paymnet PastDue Check Box");
            string totalPayment = payments.VerifyUpcomingPayment(loanLevelData, 10.00);
            string currentWorkingDate;
            webElementExtensions.ScrollIntoView(_driver, payments.paymentDatePickerIconLocBy);
            webElementExtensions.ActionClick(_driver, payments.paymentDatePickerIconLocBy);
            commonServices.VerifyEnabledDisabledPaymentDatesForMonth();
            commonServices.VerifyMinimumPayDateOfMonthNotBankingHoliday();
            commonServices.VerifyMaximumPayDateIsLastDayOfMonthNotBankingHoliday();
            currentWorkingDate = commonServices.GetWorkingDateAfter16thDate();
            payments.SelectPaymentDateInDateField(currentWorkingDate);
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

            //SCM_5438: Verification 8 - Verify the edit payment page when loan has Heloc Plan
            payments.EditNewlyAddedPayment(confirmationNumber);
            payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
            payments.VerifyOTPScreenDetailsForHeloc(loanLevelData, Constants.PaymentFlowType.Edit);
            payments.VerifyBannerDetailsForHeloc(loanLevelData, Constants.PaymentFlowType.Edit);
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
            totalPayment = payments.VerifyUpcomingPayment(loanLevelData, 10.00, 15.00, Constants.PaymentFlowType.Edit);
            currentWorkingDate = commonServices.GetWorkingDateAfter16thDate(Constants.PaymentFlowType.Edit);
            commonServices.SelectPaymentDateInDateField(currentWorkingDate);
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

        [TestMethod]
        [Description("<br>TC-631 [SCM-6485] - Test-verify HELOC - Delta - Make a Payment Screen UI Validations Additional payment field - Past Due when loan has soft stop<br>" +
                     "TC-670 [SCM-6487] - Test-verify HELOC - Delta - Make a Payment Screen UI Validations Amount, fees and date fields - Past Due when loan has soft stop"
                     )]
        [TestCategory("AP_Regression"), TestCategory("AP_HELOC_OTP")]
        public void TC_631_670_TC_VerifyMakeAPaymentScreenUIValidationsForAmountFeesDateAdditionalPaymentFieldsOfPastDueLoanWithSoftStopCodes()
        {
            int retryCount = 0;
            commonServices.LoginToTheApplication(username, password);
            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForPastDueHelocOTPWithSoftProcessStopCode;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery, true);

            #endregion TestData
            if (listOfLoanLevelData.Count > 0)
            {

                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
                {
                    if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                    {
                        webElementExtensions.WaitForVisibilityOfElement(_driver, By.XPath(dashboard.apNavigationTabsLocBy.Replace("<TABNAME>", Constants.AgentPortalTabNames.PaymentsTab)));
                        dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
                        webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                        if (webElementExtensions.IsElementEnabled(_driver, payments.makeAPaymentButtonLocBy))
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
                payments.VerifyPaymentSummaryDetailsOnPaymentPageForHeloc(loanLevelData);
                payments.DeleteAllExistingPendingPayments(deleteReason);

                //Verification 1 - Make a payment button is Orange in color
                bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Orange, payments.makeAPaymentButtonLocBy);
                _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Orange' in color for the selected Heloc loan.",
                                                    "Failure - 'Make a Payment' button is not Orange in color for the selected Heloc loan.");
                reportLogger.TakeScreenshot(test, "Make a Payment' button");
                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment, null, true);

                payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                payments.VerifyOTPPageIsDisplayed();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                payments.VerifyOTPScreenDetailsForHeloc(loanLevelData);
                payments.VerifyBannerDetailsForHeloc(loanLevelData);
                payments.SelectValueInAuthorizedByDropdown(borrower);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                string accountFullName = firstName + " " + lastName;
                flag = webElementExtensions.IsElementDisplayed(_driver, payments.monthlyPaymentPastDueCheckedAndDisabledLocBy);
                _driver.ReportResult(test, flag, "Successfully verified that Heloc plan Monthly Paymnet PastDue Check Box is Checked intially and  Disabled.",
                                                "Failure - Heloc plan Alert not Monthly Paymnet PastDue Check Box is Checked intially and  Disabled.");
                reportLogger.TakeScreenshot(test, "Heloc plan Monthly Paymnet PastDue Check Box");
                string totalPayment = payments.VerifyUpcomingPayment(loanLevelData, 10.00);
                commonServices.VerifyEnabledDisabledPaymentDatesForMonth();
                commonServices.VerifyMinimumPayDateOfMonthNotBankingHoliday();
                commonServices.VerifyMaximumPayDateIsLastDayOfMonthNotBankingHoliday();
                string workingDate = commonServices.GetWorkingDateAfter16thDate();
                commonServices.SelectPaymentDateInDateField(workingDate);
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

                //SCM_5438: Verification 8 - Verify the edit payment page when loan has Heloc Plan
                payments.EditNewlyAddedPayment(confirmationNumber);
                payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
                payments.VerifyOTPScreenDetailsForHeloc(loanLevelData, Constants.PaymentFlowType.Edit);
                payments.VerifyBannerDetailsForHeloc(loanLevelData, Constants.PaymentFlowType.Edit);
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
                webElementExtensions.ScrollIntoView(_driver, commonServices.paymentDatePickerIconLocBy);
                workingDate = commonServices.GetWorkingDateAfter16thDate(Constants.PaymentFlowType.Edit);
                commonServices.SelectPaymentDateInDateField(workingDate);
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
            else
            {
                test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
            }
        }

        [TestMethod]
        [Description("<br>TC-1483 [SCM-6958] - Test Verify Payment Breakdown of heloc loan when Loan is past due or delinquent [deliquent_payment_count > 1]<br>")]
        [TestCategory("AP_Regression"), TestCategory("AP_HELOC_OTP")]
        public void TC_1483_TC_VerifyPaymentBreakdownOfHelocLoanWhenLoanIsPastDueOrDelinquent()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForPastDueOrDelinquentHelocOTP;

            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    webElementExtensions.WaitForVisibilityOfElement(_driver, By.XPath(dashboard.apNavigationTabsLocBy.Replace("<TABNAME>", Constants.AgentPortalTabNames.PaymentsTab)));
                    dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
                    webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                    if (webElementExtensions.IsElementEnabled(_driver, payments.makeAPaymentButtonLocBy))
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
            APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData);
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPageForHeloc(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);

            //Verification 1 - Make a payment button is Purple in color
            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected Heloc loan.",
                                                 "Failure - 'Make a Payment' button is not Purple in color for the selected Heloc loan.");
            reportLogger.TakeScreenshot(test, "Make a Payment' button");
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
            payments.VerifyOTPScreenDetailsForHeloc(loanLevelData);
            payments.VerifyBannerDetailsForHeloc(loanLevelData);
            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
            string accountFullName = firstName + " " + lastName;
            string totalPayment = payments.VerifyHelocPaymentFields(loanLevelData, helocLoanInfo, 10.00);
            string currentWorkingDate = commonServices.GetWorkingDate();
            payments.SelectPaymentDateInDateField(currentWorkingDate);
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
            payments.VerifyOTPScreenDetailsForHeloc(loanLevelData, Constants.PaymentFlowType.Edit);
            payments.VerifyBannerDetailsForHeloc(loanLevelData, Constants.PaymentFlowType.Edit);
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
            totalPayment = payments.VerifyHelocPaymentFields(loanLevelData, helocLoanInfo, 10.00, 15.00, Constants.PaymentFlowType.Edit);
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

        [TestMethod]
        [Description("<br> TC-1478 1SCM-6827-Test-Verify Payment Breakdown for Heloc loan when loan is prepaid or current [deliquent_payment_count = 0]")]
        [TestCategory("AP_Regression"), TestCategory("AP_HELOC_OTP")]
        public void TC_1478_TC_Test_VerifyPaymentBreakdownForHelocLoanWhenLoanIsPrepaidOrCurrent()
        {
            #region TestData

            int retryCount;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            Dictionary<string, string> loanTypes = new Dictionary<string, string> { { "0", "On-time" }, { "Prepaid", "Prepaid" } };
            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify the user is able to Edit and Delete existing OTP before cutoff time on Draft Date : " + loanType.Value + " ******</b>");
                try
                {
                    retryCount = 0;
                    if (loanType.Key == "0")
                    {
                        loanDetailsQuery = getLoanLevelDetailsForOnTimeOrPrepaidHelocOTP.Replace("DELINQUENTCOUNT", "=0");
                        loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                    }
                    else if (loanType.Key == "Prepaid")
                    {
                        loanDetailsQuery = getLoanLevelDetailsForOnTimeOrPrepaidHelocOTP.Replace("DELINQUENTCOUNT", "=0");
                        loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                    }

                    listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery, true);

                    if (listOfLoanLevelData.Count > 0)
                    {
                        ReportingMethods.Log(test, "Query - " + loanDetailsQuery);
                        while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
                        {
                            if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                            {
                                webElementExtensions.WaitForVisibilityOfElement(_driver, By.XPath(dashboard.apNavigationTabsLocBy.Replace("<TABNAME>", Constants.AgentPortalTabNames.PaymentsTab)));
                                dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
                                webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                                if (webElementExtensions.IsElementEnabled(_driver, payments.makeAPaymentButtonLocBy))
                                    break;
                            }
                            retryCount++;
                            if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                                test.Log(Status.Info, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        }
                        loanLevelData = listOfLoanLevelData[retryCount];

                        string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                        loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                        $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";
                        APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData);
                        payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                        payments.VerifyPaymentBreakDownDetailsOnPaymentPageForHeloc(loanLevelData);
                    }
                    else
                    {
                        test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                    }
                }
                catch (Exception ex)
                {
                    ReportingMethods.Log(test, "Exception: " + ex.Message);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-1058 SCM-5807-Test Verify HELOC - Delta - Make a Payment Screen - UI Validations - On Time")]
        [TestCategory("AP_Regression"), TestCategory("AP_HELOC_OTP")]
        public void TC_1058_TC_VerifyHELOC_DeltaMakeAPaymentScreen_UIValidations_OnTime()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForOnTimeWithUPBHelocOTP;

            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery, false);

            #endregion TestData

            if (listOfLoanLevelData.Count > 0)
            {
                commonServices.LoginToTheApplication(username, password);

                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
                {
                    if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                    {
                        webElementExtensions.WaitForVisibilityOfElement(_driver, By.XPath(dashboard.apNavigationTabsLocBy.Replace("<TABNAME>", Constants.AgentPortalTabNames.PaymentsTab)));
                        dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
                        webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                        if (webElementExtensions.IsElementEnabled(_driver, payments.makeAPaymentButtonLocBy))
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
                APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData);
                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyPaymentSummaryDetailsOnPaymentPageForHeloc(loanLevelData);
                payments.DeleteAllExistingPendingPayments(deleteReason);

                //Verification 1 - Make a payment button is Purple in color
                webElementExtensions.MoveToElement(_driver, payments.searchPaymentActivityButtonLocBy);
                bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
                _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected Heloc loan.",
                                                     "Failure - 'Make a Payment' button is not Purple in color for the selected Heloc loan.");
                reportLogger.TakeScreenshot(test, "Make a Payment' button");
                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                payments.VerifyOTPPageIsDisplayed();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                payments.VerifyOTPScreenDetailsForHeloc(loanLevelData);
                payments.VerifyBannerDetailsForHeloc(loanLevelData);
                payments.SelectValueInAuthorizedByDropdown(borrower);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                string accountFullName = firstName + " " + lastName;
                string totalPayment = payments.VerifyHelocUIValidations(loanLevelData, helocLoanInfo, Constants.AmountValues.AdditionalPrincipalPaymentParamValue, Constants.AmountValues.AdditionalPrincipalPaymentEditParamValue);
                string workingDate = commonServices.GetWorkingDate();
                commonServices.SelectPaymentDateInDateField(workingDate);
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

                //Verify the edit payment page when loan has Heloc Plan
                payments.EditNewlyAddedPayment(confirmationNumber);
                payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
                payments.VerifyOTPScreenDetailsForHeloc(loanLevelData, Constants.PaymentFlowType.Edit);
                payments.VerifyBannerDetailsForHeloc(loanLevelData, Constants.PaymentFlowType.Edit);
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
                totalPayment = payments.VerifyHelocUIValidations(loanLevelData, helocLoanInfo, Constants.AmountValues.AdditionalPrincipalPaymentParamValue, Constants.AmountValues.AdditionalPrincipalPaymentEditParamValue, Constants.PaymentFlowType.Edit);
                workingDate = commonServices.GetWorkingDate(Constants.PaymentFlowType.Edit);
                payments.SelectPaymentDateInDateField(workingDate);
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
            else
            {
                test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
            }
        }
    }
}
