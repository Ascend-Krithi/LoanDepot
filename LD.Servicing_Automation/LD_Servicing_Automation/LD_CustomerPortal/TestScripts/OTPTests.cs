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
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace LD_CustomerPortal.Tests
{
    [TestClass]
    public class OTPTests : BasePage
    {
        private string loanLevelDetailsForEligibleOTPPrepaid = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleOTPPrepaid));
        private string loanLevelDetailsForEligibleOTPPrepaidOptOut = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleOTPPrepaidOptOut));
        private string loanLevelDetailsForEligibleOTPOntime = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleOTPOntime));
        private string loanLevelDetailsForEligibleOTPOntimeOptOut = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleOTPOntimeOptOut));
        private string loanLevelDetailsForEligibleOTPPastDue = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleOTPPastDue));
        private string loanLevelDetailsForEligibleOTPPastDueOptOut = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleOTPPastDueOptOut));
        private string loanLevelDetailsForEligibleOTPDelinquent = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleOTPDelinquent));
        private string loanLevelDetailsForEligibleOTPDelinquentOptOut = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleOTPDelinquentOptOut));
        private string loanlevelDetailsForTimeSensitiveCheck = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPTimeSensitiveCheck));
        private string loanlevelDetailsForOTPWithSubsidy = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansForOTPWithSubsidy));

        public TestContext TestContext
        {
            set;
            get;
        }

        #region ObjectInitialization

        private DashboardPage dashboard = null;
        private WebElementExtensionsPage webElementExtensions = null;
        private CommonServicesPage commonServices = null;
        private DBconnect dBconnect = null;
        private Pages.PaymentsPage payments = null;
        private SMCPage smc = null;
        YopmailPage yopmailPage = null;
        private ReportLogger reportLogger { get; set; }

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
            yopmailPage = new YopmailPage(_driver, test);
            dBconnect = new DBconnect(test, Constants.DBNames.MelloServETL);
            reportLogger = new ReportLogger(_driver);
            //unlink loans from test account
            dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
            test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
        }

        [TestMethod]
        [Description("<br> TPR-1485 CP_E2E_OTP_PrePaid_Setup_payment_Otp_In")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP"), TestCategory("CP_Sanity")]
        public void TPR_1485_TPR_VerifyOTPE2EScenarioForLoanStatusPrepaid()
        {
            int retryCount = 0;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanLevelDetailsForEligibleOTPPrepaid}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForEligibleOTPPrepaid, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                dashboard.VerifyPaymentSummaryDetailsOnDashboardPage(loanLevelData[retryCount]);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                webElementExtensions.WaitForStalenessOfElement(_driver, payments.backToAccountSummaryBtnLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                if (ConfigSettings.PaymentsDataDeletionRequired)
                {
                    willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                    if (willScheduledPaymentInProgressPopAppear)
                        payments.AcceptScheduledPaymentIsProcessingPopUp();
                    dashboard.HandlePaperLessPage();
                    payments.VerifytOTPPageTitle();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                    commonServices.DeleteAllAddedBankAccounts();
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                    amountForSMC = totalPayment;

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.paymentDatePickerIconLocBy);
                    webElementExtensions.WaitForElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.SmallWaitTime);
                    commonServices.SelectPaymentDateInDateField(dateSelected, true);
                    webElementExtensions.VerifyElementText(_driver, payments.paymentAlreadyExistsErrorTextLocBy, Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay, "Payment already exists error", true);
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true, type: "edit", isReportRequired: false);

                    //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, 15.00, 20.00, "edit", false);
                    webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.setupAutopayReviewPageTextContentLocBy);
                    string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                    List<string> editOtpKeywords = new List<string>() { totalPayment, "One-Time Payment" };

                    reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ScrollIntoView(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.ClickElement(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryBtnLocBy);

                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryBtnLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.LongWaitTime);

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    string draftDate = DateTime.Parse(dateSelected).ToString("MM/dd/yyyy");
                    string title = " loanDepot One Time Payment Notice - SM258 ";
                    string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amountForSMC), messageTemplateContent.Contains(amountForSMC) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amountForSMC}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amountForSMC}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    payments.DeleteOtpPaymentSetup(payment_date);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                    webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                    reportLogger.TakeScreenshot(test, "Delete OTP");
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy);
                    reportLogger.TakeScreenshot(test, "After Deleting OTP");
                    var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                    var counter = 0;

                    while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                    {
                        pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        counter++;
                    }
                    reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                    ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Verify Pending Payment Activity should not have deleted OTP payment");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Delete OTP</u>********************************************</b>");
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    title = " loanDepot Cancellation Notice - SM260 ";
                    messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                    recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(username) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{username}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{username}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR-1531 CP_E2E_OTP_PrePaid_Setup_payment_Otp_OUT")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP"), TestCategory("CP_Sanity")]
        public void TPR_1531_TPR_VerifyOTPE2EScenarioForLoanStatusPrepaidOptOut()
        {
            int retryCount = 0;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanLevelDetailsForEligibleOTPPrepaidOptOut}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForEligibleOTPPrepaidOptOut, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                dashboard.VerifyPaymentSummaryDetailsOnDashboardPage(loanLevelData[retryCount]);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                webElementExtensions.WaitForStalenessOfElement(_driver, payments.backToAccountSummaryBtnLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                if (ConfigSettings.PaymentsDataDeletionRequired)
                {
                    willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                    if (willScheduledPaymentInProgressPopAppear)
                        payments.AcceptScheduledPaymentIsProcessingPopUp();
                    dashboard.HandlePaperLessPage();
                    payments.VerifytOTPPageTitle();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                    commonServices.DeleteAllAddedBankAccounts();
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                    amountForSMC = totalPayment;

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.paymentDatePickerIconLocBy);
                    webElementExtensions.WaitForElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.SmallWaitTime);
                    commonServices.SelectPaymentDateInDateField(dateSelected, true);
                    webElementExtensions.VerifyElementText(_driver, payments.paymentAlreadyExistsErrorTextLocBy, Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay, "Payment already exists error", true);
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true, type: "edit", isReportRequired: false);

                    //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, 15.00, 20.00, "edit", false);
                    webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.setupAutopayReviewPageTextContentLocBy);
                    string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                    List<string> editOtpKeywords = new List<string>() { totalPayment, "One-Time Payment" };

                    reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ScrollIntoView(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.ClickElement(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryBtnLocBy);

                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryBtnLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    string draftDate = DateTime.Parse(dateSelected).ToString("MM/dd/yyyy");
                    string title = " loanDepot One Time Payment Notice - SM258 ";
                    string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amountForSMC), messageTemplateContent.Contains(amountForSMC) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amountForSMC}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amountForSMC}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    payments.DeleteOtpPaymentSetup(payment_date);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                    webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                    reportLogger.TakeScreenshot(test, "Delete OTP");
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy);
                    reportLogger.TakeScreenshot(test, "After Deleting OTP");
                    var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                    var counter = 0;

                    while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                    {
                        pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        counter++;
                    }
                    reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                    ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Verify Pending Payment Activity should not have deleted OTP payment");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Delete OTP</u>********************************************</b>");
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    title = " loanDepot Cancellation Notice - SM260 ";
                    messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                    recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(username) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{username}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{username}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Delete</u>********************************************</b>");
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
        [Description("<br>TPR-1530-Verify OTP E2E Scenario for Loan Status Ontime <br>" +
                         "TPR-1061-Verify OTP eligibility for loans with different loan types- Ontime <br>" +
                         "TPR-678-Verify add new Bank  Account Validations <br>" +
                         "TPR-1593-Verify Pending Payment Section after OTP Setup <br>" +
                         "TPR-1592-Verify Pending Payment Section after OTP Edit<br>" +
                         "TPR-1547-Verify_Setting_OTP_is_not_allowed_on_same_day_When_other_payment_is_pending for Ontime")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP"), TestCategory("CP_Sanity")]
        public void TPR_1530_1061_678_1593_1592_1547_TPR_VerifyOTPE2EScenarioForLoanStatusOntime()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.Ontime;

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
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                dashboard.VerifyPaymentSummaryDetailsOnDashboardPage(loanLevelData[retryCount]);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                webElementExtensions.WaitForStalenessOfElement(_driver, payments.backToAccountSummaryBtnLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                if (ConfigSettings.PaymentsDataDeletionRequired)
                {
                    willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                    if (willScheduledPaymentInProgressPopAppear)
                        payments.AcceptScheduledPaymentIsProcessingPopUp();
                    dashboard.HandlePaperLessPage();
                    payments.VerifytOTPPageTitle();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                    commonServices.DeleteAllAddedBankAccounts();
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                    amountForSMC = totalPayment;

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.paymentDatePickerIconLocBy);
                    webElementExtensions.WaitForElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.SmallWaitTime);
                    commonServices.SelectPaymentDateInDateField(dateSelected, true);
                    webElementExtensions.VerifyElementText(_driver, payments.paymentAlreadyExistsErrorTextLocBy, Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay, "Payment already exists error", true);
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);

                    string emailNotification = yopmailPage.GetEmailContentFromYopmail(username, emailNotificationType: Constants.EmailNotificationFromYopmail.OTPSetup);
                    if (emailNotification.Contains(Constants.EmailContents.OTPSetupTitle))
                        ReportingMethods.Log(test, "");
                    if (emailNotification.Contains(Constants.EmailContents.OTPSetupEmailContentBody))
                        ReportingMethods.Log(test, "");
                    if (emailNotification.Contains(Constants.EmailContents.SenderEmailId))
                        ReportingMethods.Log(test, "");
                    ReportingMethods.LogAssertionTrue(test, emailNotification.Contains(Constants.EmailContents.OTPSetupTitle), "Verify OTP Setup Title from Email Notification");
                    ReportingMethods.LogAssertionTrue(test, emailNotification.Contains(Constants.EmailContents.OTPSetupEmailContentBody), "Verify OTP Setup Email Body from Email Notification");
                    ReportingMethods.LogAssertionTrue(test, emailNotification.Contains(Constants.EmailContents.SenderEmailId), "Verify OTP Setup Email Sender Email ID from Email Notification");

                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true, type: "edit", isReportRequired: false);

                    //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, 15.00, 20.00, "edit", false);
                    webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.setupAutopayReviewPageTextContentLocBy);
                    string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                    List<string> editOtpKeywords = new List<string>() { totalPayment, "One-Time Payment" };

                    reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ScrollIntoView(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.ClickElement(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryBtnLocBy);

                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryBtnLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    string draftDate = DateTime.Parse(dateSelected).ToString("MM/dd/yyyy");
                    string title = " loanDepot One Time Payment Notice - SM258 ";
                    string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amountForSMC), messageTemplateContent.Contains(amountForSMC) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amountForSMC}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amountForSMC}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    payments.DeleteOtpPaymentSetup(payment_date);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                    webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                    reportLogger.TakeScreenshot(test, "Delete OTP");
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy);
                    reportLogger.TakeScreenshot(test, "After Deleting OTP");
                    var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                    var counter = 0;

                    while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                    {
                        pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        counter++;
                    }
                    reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                    ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Verify Pending Payment Activity should not have deleted OTP payment");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Delete OTP</u>********************************************</b>");

                    emailNotification = yopmailPage.GetEmailContentFromYopmail(username, emailNotificationType: Constants.EmailNotificationFromYopmail.OTPDelete);
                    ReportingMethods.LogAssertionTrue(test, emailNotification.Contains(Constants.EmailContents.OTPDeleteTitle), "Verify OTP Delete Title from Email Notification");
                    ReportingMethods.LogAssertionContains(test, Constants.EmailContents.OTPDeleteEmailContentBody, emailNotification, "Verify OTP Delete Email Body from Email Notification");
                    ReportingMethods.LogAssertionTrue(test, emailNotification.Contains(Constants.EmailContents.OTPDeleteEmailContentBody), "Verify OTP Delete Email Body from Email Notification");
                    ReportingMethods.LogAssertionTrue(test, emailNotification.Contains(Constants.EmailContents.SenderEmailId), "Verify OTP Delete Email Sender Email ID from Email Notification");

                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    title = " loanDepot Cancellation Notice - SM260 ";
                    messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                    recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(username) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{username}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{username}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Delete</u>********************************************</b>");
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
        [Description("<br>TPR-1507	CP_E2E_OTP_Ontime_Setup_payment_Opt_Out")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP"), TestCategory("CP_Sanity")]
        public void TPR_1507_TPR_VerifyOTPE2EScenarioForLoanStatusOntime_Opt_Out()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.Ontime;

                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanLevelDetailsForEligibleOTPOntimeOptOut}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForEligibleOTPOntimeOptOut, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                dashboard.VerifyPaymentSummaryDetailsOnDashboardPage(loanLevelData[retryCount]);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                webElementExtensions.WaitForStalenessOfElement(_driver, payments.backToAccountSummaryBtnLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                if (ConfigSettings.PaymentsDataDeletionRequired)
                {
                    willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                    if (willScheduledPaymentInProgressPopAppear)
                        payments.AcceptScheduledPaymentIsProcessingPopUp();
                    dashboard.HandlePaperLessPage();
                    payments.VerifytOTPPageTitle();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                    commonServices.DeleteAllAddedBankAccounts();
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                    amountForSMC = totalPayment;

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.paymentDatePickerIconLocBy);
                    webElementExtensions.WaitForElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.SmallWaitTime);
                    commonServices.SelectPaymentDateInDateField(dateSelected, true);
                    webElementExtensions.VerifyElementText(_driver, payments.paymentAlreadyExistsErrorTextLocBy, Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay, "Payment already exists error", true);
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true, type: "edit", isReportRequired: false);

                    //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, 15.00, 20.00, "edit", false);
                    webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.setupAutopayReviewPageTextContentLocBy);
                    string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                    List<string> editOtpKeywords = new List<string>() { totalPayment, "One-Time Payment" };

                    reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ScrollIntoView(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.ClickElement(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryBtnLocBy);

                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryBtnLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    string draftDate = DateTime.Parse(dateSelected).ToString("MM/dd/yyyy");
                    string title = " loanDepot One Time Payment Notice - SM258 ";
                    string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amountForSMC), messageTemplateContent.Contains(amountForSMC) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amountForSMC}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amountForSMC}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    payments.DeleteOtpPaymentSetup(payment_date);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                    webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                    reportLogger.TakeScreenshot(test, "Delete OTP");
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy);
                    reportLogger.TakeScreenshot(test, "After Deleting OTP");
                    var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                    var counter = 0;

                    while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                    {
                        pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        counter++;
                    }
                    reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                    ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Verify Pending Payment Activity should not have deleted OTP payment");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Delete OTP</u>********************************************</b>");
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    title = " loanDepot Cancellation Notice - SM260 ";
                    messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                    recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(username) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{username}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{username}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR-1521 - Verify OTP E2E Scenario for Loan Status Past Due<br>" +
                          "TPR-1060- Verify OTP eligibility for loans with different loan types- PastDue <br>" +
                          "TPR-406-OTP/Autopay Manage Bank Accounts - Rearrange the data field columns to match in OTP/Autopay<br>" +
                          "TPR-1540 - Verify_Setting_OTP_is_not_allowed_on_same_day_When_other_payment_is_pending_for Pastdue")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP")]
        public void TPR_1521_1060_406_1540_TPR_VerifyOTPE2EScenarioForLoanStatusPastDue()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.PastDue;

                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanLevelDetailsForEligibleOTPPastDue}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForEligibleOTPPastDue, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                while (retryCount <= loanLevelData.Count && retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                dashboard.VerifyPaymentSummaryDetailsOnDashboardPage(loanLevelData[retryCount]);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                webElementExtensions.ScrollToTop(_driver);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.backToAccountSummaryBtnLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                if (ConfigSettings.PaymentsDataDeletionRequired)
                {
                    willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                    if (willScheduledPaymentInProgressPopAppear)
                        payments.AcceptScheduledPaymentIsProcessingPopUp();
                    dashboard.HandlePaperLessPage();
                    payments.VerifytOTPPageTitle();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                    commonServices.DeleteAllAddedBankAccounts();
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                    amountForSMC = totalPayment;

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.paymentDatePickerIconLocBy);
                    webElementExtensions.WaitForElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.SmallWaitTime);
                    commonServices.SelectPaymentDateInDateField(dateSelected, true);
                    webElementExtensions.VerifyElementText(_driver, payments.paymentAlreadyExistsErrorTextLocBy, Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay, "Payment already exists error", true);
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);

                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true, type: "edit", isReportRequired: false);

                    //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, 15.00, 20.00, "edit", false);
                    webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.setupAutopayReviewPageTextContentLocBy);
                    string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                    List<string> editOtpKeywords = new List<string>() { totalPayment, "One-Time Payment" };

                    reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ScrollIntoView(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.ClickElement(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryBtnLocBy);

                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryBtnLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    string draftDate = DateTime.Parse(dateSelected).ToString("MM/dd/yyyy");
                    string title = " loanDepot One Time Payment Notice - SM258 ";
                    string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amountForSMC), messageTemplateContent.Contains(amountForSMC) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amountForSMC}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amountForSMC}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    payments.DeleteOtpPaymentSetup(payment_date);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                    webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                    reportLogger.TakeScreenshot(test, "Delete OTP");
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    reportLogger.TakeScreenshot(test, "After Deleting OTP");
                    webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy);
                    var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                    var counter = 0;

                    while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                    {
                        pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        counter++;
                    }
                    reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                    ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Verify Pending Payment Activity should not have deleted OTP payment");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Delete OTP</u>********************************************</b>");

                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    title = " loanDepot Cancellation Notice - SM260 ";
                    messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                    recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(username) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{username}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{username}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR-1506	CP_E2E_OTP_PastDue_Setup_payment_Opt_Out")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP")]
        public void TPR_1506_TPR_VerifyOTPE2EScenarioForLoanStatusPastDue_Opt_Out()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.PastDue;

                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanLevelDetailsForEligibleOTPPastDueOptOut}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForEligibleOTPPastDueOptOut, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                while (retryCount <= loanLevelData.Count && retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                dashboard.VerifyPaymentSummaryDetailsOnDashboardPage(loanLevelData[retryCount]);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                webElementExtensions.ScrollToTop(_driver);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.backToAccountSummaryBtnLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                if (ConfigSettings.PaymentsDataDeletionRequired)
                {
                    willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                    if (willScheduledPaymentInProgressPopAppear)
                        payments.AcceptScheduledPaymentIsProcessingPopUp();
                    dashboard.HandlePaperLessPage();
                    payments.VerifytOTPPageTitle();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                    commonServices.DeleteAllAddedBankAccounts();
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                    amountForSMC = totalPayment;

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.paymentDatePickerIconLocBy);
                    webElementExtensions.WaitForElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.SmallWaitTime);
                    commonServices.SelectPaymentDateInDateField(dateSelected, true);
                    webElementExtensions.VerifyElementText(_driver, payments.paymentAlreadyExistsErrorTextLocBy, Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay, "Payment already exists error", true);
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);

                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true, type: "edit", isReportRequired: false);

                    //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, 15.00, 20.00, "edit", false);
                    webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.setupAutopayReviewPageTextContentLocBy);
                    string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                    List<string> editOtpKeywords = new List<string>() { totalPayment, "One-Time Payment" };

                    reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ScrollIntoView(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.ClickElement(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryBtnLocBy);

                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryBtnLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    string draftDate = DateTime.Parse(dateSelected).ToString("MM/dd/yyyy");
                    string title = " loanDepot One Time Payment Notice - SM258 ";
                    string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amountForSMC), messageTemplateContent.Contains(amountForSMC) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amountForSMC}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amountForSMC}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    payments.DeleteOtpPaymentSetup(payment_date);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                    webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                    reportLogger.TakeScreenshot(test, "Delete OTP");
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    reportLogger.TakeScreenshot(test, "After Deleting OTP");
                    webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy);
                    var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                    var counter = 0;

                    while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                    {
                        pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        counter++;
                    }
                    reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                    ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Verify Pending Payment Activity should not have deleted OTP payment");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Delete OTP</u>********************************************</b>");

                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    title = " loanDepot Cancellation Notice - SM260 ";
                    messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                    recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(username) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{username}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{username}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR-1519- Verify OTP E2E Scenario for Loan Status Delinquent<br>" +
                          "TPR-1057- Verify OTP eligibility for loans with different loan types- Delinquent<br>" +
                          "TPR-2940 - Verify_Setting_OTP_is_not_allowed_on_same_day_When_other_payment_is_pending for Delinquent")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP")]
        public void TPR_1519_1057_2940_TPR_VerifyOTPE2EScenarioForLoanStatusDelinquent()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.Delinquent;

                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanLevelDetailsForEligibleOTPDelinquent}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForEligibleOTPDelinquent, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                test.Log(Status.Info, "-> ********************************************Starting Account Summary Page Validation*******************************************");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                dashboard.VerifyPaymentSummaryDetailsOnDashboardPage(loanLevelData[retryCount]);
                test.Log(Status.Info, "-> ********************************************Ending Account Summary Page Validation*******************************************");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");

                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.backToAccountSummaryBtnLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                if (ConfigSettings.PaymentsDataDeletionRequired)
                {
                    willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                    if (willScheduledPaymentInProgressPopAppear)
                        payments.AcceptScheduledPaymentIsProcessingPopUp();
                    dashboard.HandlePaperLessPage();
                    payments.VerifytOTPPageTitle();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                    commonServices.DeleteAllAddedBankAccounts();
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                    amountForSMC = totalPayment;

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.paymentDatePickerIconLocBy);
                    webElementExtensions.WaitForElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.SmallWaitTime);
                    commonServices.SelectPaymentDateInDateField(dateSelected, true);
                    webElementExtensions.VerifyElementText(_driver, payments.paymentAlreadyExistsErrorTextLocBy, Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay, "Payment already exists error", true);
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);

                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment Button", isScrollIntoViewRequired: false);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true, type: "edit", isReportRequired: false);

                    //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, 15.00, 20.00, "edit", false);
                    webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy, "Update Payment Button", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.setupAutopayReviewPageTextContentLocBy);
                    string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                    List<string> editOtpKeywords = new List<string>() { totalPayment, "One-Time Payment" };

                    reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ScrollIntoView(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.ClickElement(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryBtnLocBy);

                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryBtnLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    string draftDate = DateTime.Parse(dateSelected).ToString("MM/dd/yyyy");
                    string title = " loanDepot One Time Payment Notice - SM258 ";
                    string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amountForSMC), messageTemplateContent.Contains(amountForSMC) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amountForSMC}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amountForSMC}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Starting Process to Delete OTP</u>********************************************</b>");

                    payments.DeleteOtpPaymentSetup(payment_date);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                    webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                    reportLogger.TakeScreenshot(test, "Delete OTP");
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    reportLogger.TakeScreenshot(test, "After Deleting OTP");
                    webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy);
                    var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                    var counter = 0;

                    while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                    {
                        pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        counter++;
                    }
                    reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                    ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Verify Pending Payment Activity should not have deleted OTP payment");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Delete OTP</u>********************************************</b>");
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    title = " loanDepot Cancellation Notice - SM260 ";
                    messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                    recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(username) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{username}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{username}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR-1495 CP_E2E_OTP_Delinquent_Setup_payment_Opt_Out")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP")]
        public void TPR_1495_TPR_VerifyOTPE2EScenarioForLoanStatusDelinquentOptOut()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.Delinquent;

                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanLevelDetailsForEligibleOTPDelinquentOptOut}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForEligibleOTPDelinquentOptOut, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                test.Log(Status.Info, "-> ********************************************Starting Account Summary Page Validation*******************************************");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                dashboard.VerifyPaymentSummaryDetailsOnDashboardPage(loanLevelData[retryCount]);
                test.Log(Status.Info, "-> ********************************************Ending Account Summary Page Validation*******************************************");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");

                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.backToAccountSummaryBtnLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                if (ConfigSettings.PaymentsDataDeletionRequired)
                {
                    willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                    if (willScheduledPaymentInProgressPopAppear)
                        payments.AcceptScheduledPaymentIsProcessingPopUp();
                    dashboard.HandlePaperLessPage();
                    payments.VerifytOTPPageTitle();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                    commonServices.DeleteAllAddedBankAccounts();
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                    amountForSMC = totalPayment;

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.paymentDatePickerIconLocBy);
                    webElementExtensions.WaitForElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.SmallWaitTime);
                    commonServices.SelectPaymentDateInDateField(dateSelected, true);
                    webElementExtensions.VerifyElementText(_driver, payments.paymentAlreadyExistsErrorTextLocBy, Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay, "Payment already exists error", true);
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);

                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment Button", isScrollIntoViewRequired: false);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true, type: "edit", isReportRequired: false);

                    //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow
                    totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, 15.00, 20.00, "edit", false);
                    webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy, "Update Payment Button", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.setupAutopayReviewPageTextContentLocBy);
                    string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                    List<string> editOtpKeywords = new List<string>() { totalPayment, "One-Time Payment" };

                    reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ScrollIntoView(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.ClickElement(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryBtnLocBy);

                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                    webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryBtnLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    string draftDate = DateTime.Parse(dateSelected).ToString("MM/dd/yyyy");
                    string title = " loanDepot One Time Payment Notice - SM258 ";
                    string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amountForSMC), messageTemplateContent.Contains(amountForSMC) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amountForSMC}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amountForSMC}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Starting Process to Delete OTP</u>********************************************</b>");

                    payments.DeleteOtpPaymentSetup(payment_date);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                    webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                    reportLogger.TakeScreenshot(test, "Delete OTP");
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    reportLogger.TakeScreenshot(test, "After Deleting OTP");
                    webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy);
                    var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                    var counter = 0;

                    while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                    {
                        pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        counter++;
                    }
                    reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                    ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Verify Pending Payment Activity should not have deleted OTP payment");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Delete OTP</u>********************************************</b>");
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    title = " loanDepot Cancellation Notice - SM260 ";
                    messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                    recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(username) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{username}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{username}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR-555 - Verify OTP E2E Scenario for Loan Status Delinquent<br>" +
                  "TPR-556 Verify OTP eligibility for loans with different loan types- Delinquent<br>" +
                  "TPR-564-Verify that the update Payment button is enabled on editing the OTP Pending payment details for Delinquent plus loan<br>" +
                  "TPR-661-Verify that the update Payment button is enabled on editing the OTP Pending payment details for Delinquent plus loan<br>" +
                  "TPR-666-Verify that on clicking the confirm payment button from Review screen getting success Payment screen for Delinquent plus loan<br>" +
                  "TPR-667-Verify that on clicking the Edit payment button from Review screen taking the user to Edit Payment screen for Delinquent plus loan")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP")]
        public void TPR_555_556_564_661_666_667_TPR_VerifyOTPE2EScenarioForLoanStatusDelinquentPlus()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.Delinquent;

                string delinqunetPlus = loanLevelDetailsForEligibleOTPDelinquent.Replace("delinquent_payment_count = 2", "delinquent_payment_count > 2");

                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {delinqunetPlus}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(delinqunetPlus, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                test.Log(Status.Info, "-> ********************************************Starting Account Summary Page Validation*******************************************");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                dashboard.VerifyPaymentSummaryDetailsOnDashboardPage(loanLevelData[retryCount]);
                test.Log(Status.Info, "-> ********************************************Ending Account Summary Page Validation*******************************************");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");

                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.backToAccountSummaryLinkLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy, "Back To Account Summary Link", true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                //Edit the OTP setup done in previous steps
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                payments.EditOtpPayment(payment_date);

                //verify update payment is disabled
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment Button", isScrollIntoViewRequired: false);
                webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isReportRequired: false, isSelectBankAccountInTheEndRequired: true, type: "edit");

                webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy, "Update Payment Button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.setupAutopayReviewPageTextContentLocBy);
                string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                List<string> editOtpKeywords = new List<string>() { totalPayment, "One-Time Payment" };

                reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                foreach (string keyword in editOtpKeywords)
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                webElementExtensions.ScrollIntoView(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                webElementExtensions.ClickElement(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryBtnLocBy);

                string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                foreach (string keyword in editOtpKeywords)
                    ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                //SMC Verification
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                string draftDate = DateTime.Parse(dateSelected).ToString("MM/dd/yyyy");
                string title = " loanDepot One Time Payment Notice - SM258 ";
                string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amountForSMC), messageTemplateContent.Contains(amountForSMC) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amountForSMC}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amountForSMC}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");

                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                if (!ConfigSettings.PaymentsDataDeletionRequired)
                {
                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Starting Process to Delete OTP</u>********************************************</b>");

                    payments.DeleteOtpPaymentSetup(payment_date);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                    webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                    reportLogger.TakeScreenshot(test, "Delete OTP");
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    reportLogger.TakeScreenshot(test, "After Deleting OTP");
                    webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy);
                    var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                    var counter = 0;

                    while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                    {
                        pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        counter++;
                    }
                    reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                    ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Verify Pending Payment Activity should not have deleted OTP payment");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Delete OTP</u>********************************************</b>");
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    title = " loanDepot Cancellation Notice - SM260 ";
                    messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                    recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(username) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{username}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{username}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR 2754 - Verify Edit and Delete options are hidden for todays draft date from pending payment after the cutoff (8PMCST) <br>")]
        [TestCategory("CP_OTP_BeforeCutoff_TimeSensitive"), TestCategory("CP_Regression")]
        public void TPR_2754_TPR_VerifyOTPSetupForLoanStatusOntimeBeforeCutoffTime()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.Ontime;

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
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                dashboard.VerifyPaymentSummaryDetailsOnDashboardPage(loanLevelData[retryCount]);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                webElementExtensions.WaitForStalenessOfElement(_driver, payments.backToAccountSummaryBtnLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");
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
        [Description("<br> TPR 2755 - Verify Edit and Delete options are hidden for todays draft date from pending payment after the cutoff (8PMCST) <br>")]
        [TestCategory("CP_OTP_AfterCutoff_TimeSensitive"), TestCategory("CP_Regression")]
        public void TPR_2755_TPR_VerifyEditeDeletingofExisitngOTPSetupForLoanStatusOntimeAfterCutoffTime()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.Ontime;

                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanlevelDetailsForTimeSensitiveCheck}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanlevelDetailsForTimeSensitiveCheck, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        break;
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
                string dateSelected = DateTime.Now.ToString("MMMM d, yyyy");
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", CultureInfo.InvariantCulture);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                IWebElement table = _driver.FindElement(payments.tblPendingPaymentsLocBy);
                var pendingPaymentsAfterSetup = table.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                var firstRow = table.FindElements(dashboard.pendingPaymentDatesTextLocBy).Where(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture) == payment_date).FirstOrDefault();
                webElementExtensions.MoveToElement(_driver, firstRow);

                var date_formatted = String.Format("{0:MMM d, yyyy}", payment_date);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), $"Pending Payment Activity should have latest set payment date : {date_formatted}");

                var currentCstTime = UtilAdditions.GetCSTTimeNow();
                By editOtpLocBy = By.XPath(string.Format(payments.editOtpXpath, date_formatted.ToString()));
                By deleteOtpLocBy = By.XPath(string.Format(payments.deleteOtpXpath, date_formatted.ToString()));
                bool editExists = webElementExtensions.IsElementDisplayed(_driver, editOtpLocBy, isScrollIntoViewRequired: false);
                bool deleteExists = webElementExtensions.IsElementDisplayed(_driver, deleteOtpLocBy, isScrollIntoViewRequired: false);
                reportLogger.TakeScreenshot(test, $"Edit/Delete OTP Page");
                ReportingMethods.Log(test, $"CST Time now : {currentCstTime}");
                if (currentCstTime.Hour >= 20)
                {
                    ReportingMethods.Log(test, $"Edit/Delete Buttons should not present because CST time now is : {currentCstTime}");
                    ReportingMethods.LogAssertionFalse(test, editExists, $"Edit option should not be available after cutoff time");
                    ReportingMethods.LogAssertionFalse(test, deleteExists, $"Delete option should not be available Option after cutoff time");
                }
                else
                {
                    ReportingMethods.Log(test, $"Edit/Delete Buttons should present because CST time now is : {currentCstTime}");
                    ReportingMethods.LogAssertionTrue(test, editExists, "Edit option should be available before cutoff time");
                    ReportingMethods.LogAssertionTrue(test, deleteExists, "Delete option should be available Option before cutoff time");
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
        [Description("<br>TPR-596 Verify Buy down / Subsidy calculation for delinquent plus loans when buy down / subsidy is same for all months\n+" +
            "TPR 595 Verify Buy down/Subsidy calculation for delinquent plus loans when buy down/subsidy is different across months")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP")]
        public void TPR_595_596_TPR_VerifyOTPE2EScenarioWithBuydownSubsidy()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.Ontime;

                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loanlevelDetailsForOTPWithSubsidy}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanlevelDetailsForOTPWithSubsidy, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                bool willScheduledPaymentInProgressPopAppear = false;
                int numberOfPendingPayments = 0;
                List<string> pendingPaymentDates = null;
                List<DateTime> formattedDates = null;
                DateTime today = new DateTime();
                string dateString = DateTime.Now.ToString("MMMM d, yyyy");

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


                        willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                        numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                        pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                        formattedDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        today = DateTime.ParseExact(dateString, "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        reportLogger.TakeScreenshot(test, "Loan Dashboard Page ");

                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, true, false) && !formattedDates.Contains(today))
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

                        willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                        numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                        pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                        formattedDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        today = DateTime.ParseExact(dateString, "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        reportLogger.TakeScreenshot(test, "Loan Dashboard Page ");

                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, true, false) && !formattedDates.Contains(today))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    test.Log(Status.Info, "Loan is not eligible for payment due to Make Payment is disabled or payment already exists for today's date");
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                }
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                dashboard.VerifyPaymentSummaryDetailsOnDashboardPage(loanLevelData[retryCount]);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");

                willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false);
                var subsidyAmount = webElementExtensions.GetElementText(_driver, dashboard.subsidyTextLocBy);
                var subsidyCalculated = dashboard.CalculateSubsidyAmount(loanLevelData[retryCount]);
                ReportingMethods.LogAssertionContains(test, $"-${subsidyCalculated}", subsidyAmount, "Subsidy Amount in Make Payment Page");
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToTop(_driver);
                webElementExtensions.WaitForStalenessOfElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy, "Back To Account Summary Button", true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                //Edit the OTP setup done in previous steps
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                payments.EditOtpPayment(payment_date);

                //verify update payment is disabled
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true, type: "edit", isReportRequired: false);

                //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow
                totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, 15.00, 20.00, "edit", false);
                webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.setupAutopayReviewPageTextContentLocBy);
                string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                List<string> editOtpKeywords = new List<string>() { totalPayment, "One-Time Payment" };

                reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                foreach (string keyword in editOtpKeywords)
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                webElementExtensions.ScrollIntoView(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                webElementExtensions.ClickElement(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryLinkLocBy);

                string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                foreach (string keyword in editOtpKeywords)
                    ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                //SMC Verification
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                string draftDate = DateTime.Parse(dateSelected).ToString("MM/dd/yyyy");
                string title = " loanDepot One Time Payment Notice - SM258 ";
                string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amountForSMC), messageTemplateContent.Contains(amountForSMC) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amountForSMC}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amountForSMC}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");

                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                if (!ConfigSettings.PaymentsDataDeletionRequired)
                {
                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    payments.DeleteOtpPaymentSetup(payment_date);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                    webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                    reportLogger.TakeScreenshot(test, "Delete OTP");
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy);
                    reportLogger.TakeScreenshot(test, "After Deleting OTP");
                    var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                    var counter = 0;

                    while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                    {
                        pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        counter++;
                    }
                    reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                    ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Verify Pending Payment Activity should not have deleted OTP payment");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Delete OTP</u>********************************************</b>");
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    title = " loanDepot Cancellation Notice - SM260 ";
                    messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                    recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(username) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{username}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{username}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Delete</u>********************************************</b>");
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
    }
}