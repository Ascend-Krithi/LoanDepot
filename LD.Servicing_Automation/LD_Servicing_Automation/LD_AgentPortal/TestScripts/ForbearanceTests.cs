using AventStack.ExtentReports;
using LD_AgentPortal.Pages;
using LD_AutomationFramework;
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
    public class ForbearanceTests: BasePage
    {

        public static string loanDetailsQuery;

        string getLoanLevelDetailsForActiveForbearance = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForActiveForbearance));
        string getLoanLevelDetailsForDeletedForbearance = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForDeletedForbearance));
        string getLoanLevelDetailsForCurrentMonthDeletedForbearance = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForCurrentMonthDeletedForbearance));

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
        List<Hashtable> listOfLoanLevelData = new List<Hashtable>();
        Hashtable loanLevelData = null;
        DashboardPage dashboard = null;
        LD_AutomationFramework.Pages.PaymentsPage apPayments = null;
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
        string bankAccountNameAfterSelection = ""+Constants.BankAccountData.BankAccountName+" (Loan Depot Bank-"+ Constants.BankAccountData.BankAccountNumber.Substring(Constants.BankAccountData.BankAccountNumber.Length - 4) + ")";
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
            apPayments = new LD_AutomationFramework.Pages.PaymentsPage(_driver, test);
        }

        [TestMethod]
        [Description("<br> Test FUNC - Verify Active Forbearance Setup_Edit_Cancel <br> " +
            "TC-1375 [SCM-4519]-Test-Verify Make a Payment button is orange when IsEligible = Allowed with Override <br> " +
            "TC-1409 [SCM-4522]-Test-Verify OTP Setup screen opens when user clicks 'Confirm button' on Override Required to Continue pop-up <br> " +
            "TC-649 [SCM-4481]-Test FUNC - Make a Payment Screen - Active Repay/FB/Trial Plan - Hide Payment Breakdown <br> " +
            "TC-1329 [SCM-4694]-Test TEST - Remove Late Fee Message - For Active Payment Plans <br> " +
            "TC-825 [SCM-5063]-Verify edit payment screen for default payment with active forbearance plan <br> " +
            "TC-738 [SCM-5249] - TEST - Verify Payment Breakdown is not displayed in Edit OTP screen when loan has active forbearance plan")]
        [TestCategory("AP_Regression"), TestCategory("AP_Forbearance")]
        public void TC_1375_1409_649_1329_825_738_TC_VerifyTheActiveForbearanceSetup_Edit_Cancel()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForActiveForbearance;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);
           
            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForDefaultsPastMonths(Constants.DefaultsPlan.Forbearance))
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                {
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    Assert.Fail();
                }
            }
            loanLevelData = listOfLoanLevelData[retryCount];

            string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
               loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
               $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);
            webElementExtensions.VerifyElementColor(Constants.Colors.Orange, null, Constants.ButtonNames.MakeAPayment, "Verify Make a Payment button color is Orange");
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.IsActiveForbearancePlanAlertDisplayed();
            payments.ClickButtonUsingName(Constants.ButtonNames.Confirm, Constants.AlertPopupNames.ActiveForbearancePlanAlert);
            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
            payments.VerifyPaymentBreakdownSectionNotVisible();
            payments.VerifyOTPScreenDetails(loanLevelData,Constants.ForbearancePlans.Active);
            payments.VerifyBannerPastDueDateDetails(loanLevelData, Constants.ForbearancePlans.Active,Constants.BannerDetails.YourLoanCurrentlyOnAnActiveForbearancePlan);
            webElementExtensions.VerifyElementColor(Constants.Colors.Red, payments.pastDueBannerLocBy, null, "Verify Forbearence Account Standing Banner details is Red");
            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
            string accountFullName = firstName + " " + lastName;
            payments.ClickAmountMonthlyPaymentRadioButton();
            string amountMonthlyPayment = payments.GetAmountMonthlyPaymentValue();
            string[] amountMonthlyPaymentArray = amountMonthlyPayment.Replace(",", "").Replace("$", "").Split('.');
            double addMonthlyAmount = Convert.ToDouble(amountMonthlyPaymentArray[0]) * 100;
            addMonthlyAmount = addMonthlyAmount + Convert.ToInt32(amountMonthlyPaymentArray[1]);
            string totalPayment = payments.EnterAmountMonthlyPaymentValue(addMonthlyAmount);
            string workingDate = commonServices.GetWorkingDateAfter16thDate();
            commonServices.SelectPaymentDateInDateField(workingDate);
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
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
            payments.VerifyPaymentBreakdownSectionNotVisible(Constants.PageNames.EditPayment);
            payments.VerifyOTPScreenDetails(loanLevelData,Constants.ForbearancePlans.Active,Constants.PaymentFlowType.Edit);
            payments.VerifyBannerPastDueDateDetails(loanLevelData, Constants.PaymentFlowType.Edit,Constants.BannerDetails.YourLoanCurrentlyOnAnActiveForbearancePlan);
            payments.VerifyUpdatePaymentButtonIsDisabled(Constants.PageNames.EditPayment);
            webElementExtensions.VerifyElementColor(Constants.Colors.Red, payments.pastDueBannerLocBy, null, "Verify Edit Forbearence Account Standing Banner details is Red");
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
            payments.VerifyAmountMonthlyPayment(amountMonthlyPayment);
            addMonthlyAmount = addMonthlyAmount + 10;
            totalPayment = payments.EnterAmountMonthlyPaymentValue(addMonthlyAmount);
            payments.VerifyTotalAmountInPaymentPage(totalPayment,Constants.PageNames.EditPayment);
            payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
            payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PageNames.EditPayment);
             payments.VerifyTotalAmountPaymentReviewPage(totalPayment,Constants.PageNames.EditPayment);
            payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment, Constants.PageNames.EditPayment);
            payments.ClickConfirmButtonPaymentReviewPage();
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow,Constants.PageNames.EditPayment);
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
        [Description("<br>TC-1412 [SCM-4520]-Test-Verify display pop-up when IsEligible = Allowed with Override due to Error_Code 'Payment_Plan_Delinquent'")]
        [TestCategory("AP_Regression"), TestCategory("AP_Forbearance")]
        public void TC_1412_TC_VerifyDisplayPopUpWhenIsEligibleIsAllowedWithOverrideDueToErrorCodePaymentPlanDelinquent()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForDeletedForbearance;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);


            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForDefaultsPastMonths(Constants.DefaultsPlan.Forbearance))
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                {
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    Assert.Fail();
                }
            }
            loanLevelData = listOfLoanLevelData[retryCount];

            string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
               loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
               $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);
            webElementExtensions.VerifyElementColor(Constants.Colors.Orange, null, Constants.ButtonNames.MakeAPayment, "Verify Make a Payment button color is Orange");
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyOverrideRequiredToContinueBrokenOrDeletedPopupIsDisplayed();
            payments.ClickButtonUsingName(Constants.ButtonNames.Confirm, Constants.AlertPopupNames.DeletedForbearancePlanAlert);
            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
            payments.VerifyOTPScreenDetails(loanLevelData,Constants.ForbearancePlans.Deleted);
            payments.VerifyForbearancePlanBannerDetails(loanLevelData,"Delete");
            if (Convert.ToInt16(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 1) {
                webElementExtensions.VerifyElementColor(Constants.Colors.Yellow, payments.pastDueDivLocBy, null, "Verify Forbearance Account Standing Banner details is Yellow");
            } else if(Convert.ToInt16(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) > 1)
            {
                webElementExtensions.VerifyElementColor(Constants.Colors.Red, payments.delinquentDivLocBy, null, "Verify Forbearance Account Standing Banner details is Red");
            }

            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
            string accountFullName = firstName + " " + lastName;
            Dictionary<string, string> dictionary = payments.VerifyDeleteForbearencePaymentFields(loanLevelData,10.00,20.00);
            string workingDate = commonServices.GetWorkingDate();
            payments.SelectPaymentDateInDateField(workingDate);
            payments.VerifyLateFeesWarningMsgNotDisplayed();
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyReviewAndConfirmationPageIsDisplayed();
            string totalAmount = payments.VerifyTotalAmountPaymentReviewPage(dictionary["TotalPayment"]);
            payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, dictionary["TotalPayment"]);
            payments.ClickConfirmButtonPaymentReviewPage();
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
            string confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
            payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
            payments.CloseNotesTab();

            payments.EditNewlyAddedPayment(confirmationNumber);

            payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
            payments.VerifyOTPScreenDetails(loanLevelData, Constants.ForbearancePlans.Deleted,Constants.PaymentFlowType.Edit);
            payments.VerifyForbearancePlanBannerDetails(loanLevelData, "Delete");
            if (Convert.ToInt16(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 1)
            {
                webElementExtensions.VerifyElementColor(Constants.Colors.Yellow, payments.pastDueDivLocBy, null, "Verify Edit Forbearance Account Standing Banner details is Yellow");
            }
            else if (Convert.ToInt16(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) > 1)
            {
                webElementExtensions.VerifyElementColor(Constants.Colors.Red, payments.delinquentDivLocBy, null, "Verify Edit Forbearance Account Standing Banner details is Red");
            }
            Dictionary<string, string> editdictionary = payments.VerifyDeleteForbearencePaymentFields(loanLevelData,10.00, 20.00,15.00,Constants.PaymentFlowType.Edit);
            payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
            payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PageNames.EditPayment);
            payments.VerifyTotalAmountPaymentReviewPage(editdictionary["TotalPayment"], Constants.PageNames.EditPayment);
            payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, editdictionary["TotalPayment"], Constants.PageNames.EditPayment);
            payments.ClickConfirmButtonPaymentReviewPage();
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PageNames.EditPayment);
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
        [Description("<br> Verify Please enter payment amount. Inline Error message display in OTP screen for active forbearance plan <br> " +
            "TC-920 [SCM-4979] - TEST - Verify inline message display in OTP screen for active forbearance plan")]
        [TestCategory("AP_Regression"), TestCategory("AP_Forbearance")]
        public void TC_920_TC_VerifyInlineErrorMessageDisplayInOTPScreenForActiveForbearancePlan()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForActiveForbearance;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForDefaultsPastMonths(Constants.DefaultsPlan.Forbearance))
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                {
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    Assert.Fail();
                }
            }
            loanLevelData = listOfLoanLevelData[retryCount];

            string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
               loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
               $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);
            webElementExtensions.VerifyElementColor(Constants.Colors.Orange, null, Constants.ButtonNames.MakeAPayment, "Verify Make a Payment button color is Orange");
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.IsActiveForbearancePlanAlertDisplayed();
            payments.ClickButtonUsingName(Constants.ButtonNames.Confirm, Constants.AlertPopupNames.ActiveForbearancePlanAlert);
            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
            payments.VerifyPaymentBreakdownSectionNotVisible();
            payments.VerifyOTPScreenDetails(loanLevelData, Constants.ForbearancePlans.Active);
            payments.VerifyBannerPastDueDateDetails(loanLevelData, Constants.ForbearancePlans.Active,Constants.BannerDetails.YourLoanCurrentlyOnAnActiveForbearancePlan);
            webElementExtensions.VerifyElementColor(Constants.Colors.Red, payments.pastDueBannerLocBy, null, "Verify Forbearence Account Standing Banner details is Red");
            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
            string accountFullName = firstName + " " + lastName;
            payments.VerifyAmountMonthlyRadioButtonSelectedByDefault();
            payments.ClickAmountMonthlyPaymentRadioButton();
            payments.EnterAndClickOutsideAmountMonthlyPaymentField();
            payments.VerifyInlineErrorMessageIsDisplayed("Please enter payment amount.");
            string workingDate = commonServices.GetWorkingDate();
            payments.SelectPaymentDateInDateField(workingDate);
            payments.VerifyMakeAPaymentButtonIsDisabled();
        }

        [TestMethod]
        [Description("<br>TC-1291 [SCM-4550]-Test-Verify Make a Payment button is disabled and message displayed when Amount >= total reinstatement(Setup OTP-Active Forbearance Plan)")]
        [TestCategory("AP_Regression"), TestCategory("AP_Forbearance"), TestCategory("AP_Sanity")]
        public void TC_1291_1389_TC_VerifyMakeAPaymentButtonIsDisabledAndMessageDisplayedInOTPScreenForActiveForbearancePlan()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForActiveForbearance;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForDefaultsPastMonths(Constants.DefaultsPlan.Forbearance))
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                {
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    Assert.Fail();
                }
            }
            loanLevelData = listOfLoanLevelData[retryCount];

            string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
               loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
               $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);
            webElementExtensions.VerifyElementColor(Constants.Colors.Orange, null,Constants.ButtonNames.MakeAPayment, "Verify Make a Payment button color is Orange");
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.IsActiveForbearancePlanAlertDisplayed();
            payments.ClickButtonUsingName(Constants.ButtonNames.Confirm, Constants.AlertPopupNames.ActiveForbearancePlanAlert);
            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
            payments.VerifyPaymentBreakdownSectionNotVisible();
            payments.VerifyOTPScreenDetails(loanLevelData, Constants.ForbearancePlans.Active);
            payments.VerifyBannerPastDueDateDetails(loanLevelData, Constants.ForbearancePlans.Active, Constants.BannerDetails.YourLoanCurrentlyOnAnActiveForbearancePlan);
            webElementExtensions.VerifyElementColor(Constants.Colors.Red, payments.pastDueBannerLocBy, null, "Verify Forbearence Account Standing Banner details is Red");
            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
            string accountFullName = firstName + " " + lastName;
            payments.VerifyAmountMonthlyRadioButtonSelectedByDefault();
            string workingDate = payments.SelectEnabledPaymentDateInDateField();
            payments.VerifyMakeAPaymentButtonIsDisabled();
            payments.ClickAmountMonthlyPaymentRadioButton();
            payments.EnterAmountMonthlyPaymentGreaterThanUnpaidPrincipalBalance();
            //Inline message is not fully verified as an open defect TC-54 is present.
            payments.VerifyInlineErrorMessageIsDisplayed("Please enter an amount less than the total reinstatement amount");
            payments.VerifyMakeAPaymentButtonIsDisabled();
        }

        [TestMethod]
        [Description("<br>TC-1416 [SCM-5261]- TEST - Verify the user able to submit OTP successfully for Deleted FB when next payment due date is current month")]
        [TestCategory("AP_Regression"), TestCategory("AP_Forbearance")]
        public void TC_1416_TC_VerifyUserAbleToSubmitOTPSuccessfullyForDeletedFBWhenNextPaymentDueDateIsCurrentMonth()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForCurrentMonthDeletedForbearance;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery, true);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            if (listOfLoanLevelData.Count > 0)
            {
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
                {
                    if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                    {
                        if (dashboard.VerifyIfLoanNumberIsEligibleForCurrentMonth())
                            break;
                    }
                    retryCount++;
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        Assert.Fail();
                    }
                }
                loanLevelData = listOfLoanLevelData[retryCount];

                string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                   loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                   $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                payments.DeleteAllExistingPendingPayments(deleteReason);
                webElementExtensions.VerifyElementColor(Constants.Colors.Purple, null, Constants.ButtonNames.MakeAPayment, "Verify Make a Payment button color is Purple");
                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyOTPPageIsDisplayed();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                payments.VerifyOTPScreenDetails(loanLevelData, Constants.ForbearancePlans.Deleted);
                payments.VerifyBannerPastDueDateDetails(loanLevelData, Constants.ForbearancePlans.Deleted);
                webElementExtensions.VerifyElementColor(Constants.Colors.Orange, payments.pastDueCurrentMonthDeleteDivLocBy, null, "Verify Forbearence Account Standing Banner details is Orange");
                payments.SelectValueInAuthorizedByDropdown(borrower);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                string accountFullName = firstName + " " + lastName;
                string totalPayment = payments.VerifyCurrentMonthForbearenceDeletedPlanPaymentFields(10.00, 20.00);
                string workingDate = commonServices.GetWorkingDate();
                payments.SelectPaymentDateInDateField(workingDate);
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
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
                payments.VerifyOTPScreenDetails(loanLevelData, Constants.ForbearancePlans.Deleted, Constants.PaymentFlowType.Edit);
                payments.VerifyBannerPastDueDateDetails(loanLevelData, Constants.ForbearancePlans.Deleted, Constants.PaymentFlowType.Edit);
                webElementExtensions.VerifyElementColor(Constants.Colors.Orange, payments.pastDueCurrentMonthDeleteDivLocBy, null, "Verify Forbearence Account Standing Banner details is Orange");
                totalPayment = payments.VerifyCurrentMonthForbearenceDeletedPlanPaymentFields(10.00, 20.00, 15.00, Constants.PageNames.EditPayment);
                payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PageNames.EditPayment);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment, Constants.PageNames.EditPayment);
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment, Constants.PageNames.EditPayment);
                payments.ClickConfirmButtonPaymentReviewPage();
                payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PageNames.EditPayment);

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
