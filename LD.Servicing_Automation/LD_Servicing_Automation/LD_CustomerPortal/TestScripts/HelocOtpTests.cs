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
    public class HelocOtpTests : BasePage
    {
        string loanDetailsQueryForEligibleHelocAutopayOntime = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayOntime));
        string loanDetailsQueryForEligibleHelocAutopayPrepaid = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayPrepaidOneMonth));
        string loanDetailsQueryForEligibleHelocAutopayPastDue = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayPastDue));
        string loanDetailsQueryForEligibleHelocAutopayPastDueWithOutFee = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayPastDueWithoutFee));
        string loanDetailsQueryForEligibleHelocAutopayDelinquent = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayDelinquent));
        string eligibleHelocLoans = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForHELOCOTPEligible));
        string eligibleHelocLoansForTimeSensitive = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForHelocOTPTimeSensitiveCheck));

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
        string deleteReason = "Test Delete Reason";
        string firstName = "TESTFN";
        string lastName = "TESTLN";
        string personalOrBussiness = "Personal";
        string savings = "Savings";
        string accountNumber = Constants.BankAccountData.BankAccountNumber;
        string accountNumberWhileEdit = Constants.BankAccountData.BankAccountNumberWhileEdit;
        string routingNumber = "122199983";
        string bankAccountName = Constants.BankAccountData.BankAccountName;
        string accountFullName = "TESTFN TESTLN";
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
        [Description("<br> TPR-1182-E2E: HELOC Ontime Setup/Edit Before bill gen<br>" +
                     "TPR-988-E2E: HELOC Ontime Setup/Edit After bill gen")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocOtp"), TestCategory("CP_Sanity")]
        public void TPR_1182_988_TPR_VerifyHelocOTPSetup_Edit_Functionality_For_Ontime()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleHelocAutopayOntime} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleHelocAutopayOntime, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
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
                APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                Dictionary<string, string> feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                decimal totalFees = feeDic?.Values
                    .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                    .Sum() ?? 0;
                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated for the upcoming month</b>" : "<b>Bill is not Generated for the upcoming month</b>");
                dashboard.VerifyPaymentSummaryDetailsForHelocLoanOnDashboardPage(loanLevelData[retryCount], helocLoanInfo, totalFees);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.MoveToElement(_driver, dashboard.loanDepotLogoLocBy);
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

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
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");
                if (ConfigSettings.PaymentsDataDeletionRequired)
                {

                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ScrollToTop(_driver);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: false, type: "edit");

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


                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");
                    webElementExtensions.ScrollToBottom(_driver);

                    commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

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

                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR-2731-HELOC Prepaid 1M Ontime Setup<br>" +
                          "TPR-2732-HELOC Prepaid 2M Ontime Setup")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocOtp")]
        public void TPR_2731_2732_TPR_VerifyHelocOTPSetup_Edit_Functionality_For_Prepaid()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleHelocAutopayPrepaid} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleHelocAutopayPrepaid, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
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
                APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                Dictionary<string, string> feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                decimal totalFees = feeDic?.Values
                    .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                    .Sum() ?? 0;
                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated for the upcoming month</b>" : "<b>Bill is not Generated for the upcoming month</b>");
                dashboard.VerifyPaymentSummaryDetailsForHelocLoanOnDashboardPage(loanLevelData[retryCount], helocLoanInfo, totalFees);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.MoveToElement(_driver, dashboard.loanDepotLogoLocBy);
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

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
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");
                if (ConfigSettings.PaymentsDataDeletionRequired)
                {

                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ScrollToTop(_driver);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: false, type: "edit");

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


                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");
                    webElementExtensions.ScrollToBottom(_driver);

                    commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

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

                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR-915- Verify HELOC Landing Page - Verify Amount Due Calculation - Delinquent<br>" +
                          "TPR-984- Verify HELOC PastDue/Delinquent -  Add an account<br>" +
                          "TPR-1198- Verify Add Explanation of amount due when user hovers over the information icon next to Amount due for Heloc Delinquent loans<br>" +
                          "TPR-821- HELOC Delinquent- OTP review details page<br>" +
                          "TPR-841-  HELOC :Delin Verify Delete Pending Payment when user click delete icon button")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocOtp")]
        public void TPR_915_984_1198_821_841_TPR_VerifyHelocOTPDelinquentSetup_Edit_Functionality()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleHelocAutopayDelinquent} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleHelocAutopayDelinquent, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, true, false))
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, true, false))
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
                APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                Dictionary<string, string> feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                decimal totalFees = feeDic?.Values
                    .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                    .Sum() ?? 0;
                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated for the upcoming month</b>" : "<b>Bill is not Generated for the upcoming month</b>");
                dashboard.VerifyPaymentSummaryDetailsForHelocLoanOnDashboardPage(loanLevelData[retryCount], helocLoanInfo, totalFees);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.MoveToElement(_driver, dashboard.loanDepotLogoLocBy);
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

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
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");
                if (ConfigSettings.PaymentsDataDeletionRequired)
                {

                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ScrollToTop(_driver);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: false, type: "edit");

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


                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");
                    webElementExtensions.ScrollToBottom(_driver);

                    commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

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

                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR- 1121 Test HELOC OTP Past due - Before Bill Gen - Without fees")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocOtp")]
        public void TPR_1121_TPR_VerifyHelocOTPPastDueBeforeBillGenerationWithoutFees()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            APIConstants.HelocLoanInfo helocLoanInfo = new APIConstants.HelocLoanInfo();
            Dictionary<string, string> feeDic = new Dictionary<string, string>();
            decimal totalFees = new decimal(0);

            try
            {
                TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById(Constants.TimeZones.PacificStandardTime);
                DateTime pstNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone);

                if (pstNow.Day >= 17)
                {
                    test.Log(Status.Warning, $"Since Current Date in PST is {pstNow} . Bill is already Yet Generated!!!");
                    return;
                }
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleHelocAutopayPastDueWithOutFee} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleHelocAutopayPastDueWithOutFee, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                        if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                        {
                            test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                            return;
                        }

                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                        feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                        totalFees = feeDic?.Values
                            .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                            .Sum() ?? 0;
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
                        {
                            if (!commonServices.GetHelocLoanInfo(loanLevelData[retryCount]).IsBillGenerated && totalFees == 0)
                            {
                                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated for the upcoming month</b>" : "<b>Bill is not Generated for the upcoming month</b>");
                                test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                                break;
                            }
                            else
                            {
                                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                                test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                            }
                        }
                        else
                        {
                            test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                            test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                        }
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
                        helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                        feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                        totalFees = feeDic?.Values
                            .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                            .Sum() ?? 0;
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
                        {
                            if (!commonServices.GetHelocLoanInfo(loanLevelData[retryCount]).IsBillGenerated && totalFees == 0)
                            {
                                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                                test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                                break;
                            }
                            {
                                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                                test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                            }
                        }
                        else
                        {
                            test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                            test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                        }
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                dashboard.VerifyPaymentSummaryDetailsForHelocLoanOnDashboardPage(loanLevelData[retryCount], helocLoanInfo, totalFees);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.MoveToElement(_driver, dashboard.loanDepotLogoLocBy);
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true); ;
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false, helocLoanInfo: helocLoanInfo);
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
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");
                if (ConfigSettings.PaymentsDataDeletionRequired)
                {

                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ScrollToTop(_driver);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: false, type: "edit");

                    //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow                   
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


                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");
                    webElementExtensions.ScrollToBottom(_driver);

                    commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

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

                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR-1012 Test HELOC OTP Past due - Before Bill Gen - With fees")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocOtp")]
        public void TPR_1012_TPR_VerifyHelocOTPPastDueBeforeBillGenerationWithFees()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            APIConstants.HelocLoanInfo helocLoanInfo = new APIConstants.HelocLoanInfo();
            Dictionary<string, string> feeDic = new Dictionary<string, string>();
            decimal totalFees = new decimal(0);
            try
            {
                TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById(Constants.TimeZones.PacificStandardTime);
                DateTime pstNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone);

                if (pstNow.Day >= 17)
                {
                    test.Log(Status.Warning, $"Since Current Date in PST is {pstNow} . Bill is already Yet Generated for the Upcoming Month!!!");
                    return;
                }
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
                        helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                        feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                        totalFees = feeDic?.Values
                            .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                            .Sum() ?? 0;
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
                        {
                            if (!commonServices.GetHelocLoanInfo(loanLevelData[retryCount]).IsBillGenerated && totalFees > 0)
                            {
                                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                                test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                                break;
                            }
                        }
                        else
                        {
                            test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                            test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                        }

                        test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                        test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
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
                        helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                        feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                        totalFees = feeDic?.Values
                            .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                            .Sum() ?? 0;
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false) && !commonServices.GetHelocLoanInfo(loanLevelData[retryCount]).IsBillGenerated && totalFees > 0)
                        {
                            if (!commonServices.GetHelocLoanInfo(loanLevelData[retryCount]).IsBillGenerated && totalFees > 0)
                            {
                                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                                test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                                break;
                            }
                        }
                        else
                        {
                            test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                            test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                        }

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
                dashboard.VerifyPaymentSummaryDetailsForHelocLoanOnDashboardPage(loanLevelData[retryCount], helocLoanInfo, totalFees);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.MoveToElement(_driver, dashboard.loanDepotLogoLocBy);
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false, helocLoanInfo: helocLoanInfo);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (totalFees > 0 && helocLoanInfo.IsBillGenerated)
                    webElementExtensions.VerifyElementText(_driver, payments.lateFeeMessageTextLocBy, Constants.CustomerPortalErrorMsgs.LateFeeMessage, "Late Fee Message", true);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");
                if (ConfigSettings.PaymentsDataDeletionRequired)
                {

                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ScrollToTop(_driver);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: false, type: "edit");

                    //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow                    
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


                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");
                    webElementExtensions.ScrollToBottom(_driver);

                    commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

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

                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR- 1123 Test HELOC OTP Past due - After Bill Gen - Without fees")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocOtp")]
        public void TPR_1123_TPR_VerifyHelocOTPPastDueAfterBillGenerationWithoutFees()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            APIConstants.HelocLoanInfo helocLoanInfo = new APIConstants.HelocLoanInfo();
            Dictionary<string, string> feeDic = new Dictionary<string, string>();
            decimal totalFees = new decimal(0);
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleHelocAutopayPastDueWithOutFee} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleHelocAutopayPastDueWithOutFee, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                        feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                        totalFees = feeDic?.Values
                            .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                            .Sum() ?? 0;
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
                        {
                            if (commonServices.GetHelocLoanInfo(loanLevelData[retryCount]).IsBillGenerated && totalFees == 0)
                            {
                                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated for the upcoming month</b>" : "<b>Bill is not Generated for the upcoming month</b>");
                                test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                                break;
                            }
                        }
                        else
                        {
                            test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated for the upcoming month</b>" : "<b>Bill is not Generated for the upcoming month</b>");
                            test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                        }
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
                        helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                        feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                        totalFees = feeDic?.Values
                            .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                            .Sum() ?? 0;
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
                        {
                            if (commonServices.GetHelocLoanInfo(loanLevelData[retryCount]).IsBillGenerated && totalFees == 0)
                            {
                                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                                test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                                break;
                            }
                        }
                        else
                        {
                            test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                            test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                        }
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
                dashboard.VerifyPaymentSummaryDetailsForHelocLoanOnDashboardPage(loanLevelData[retryCount], helocLoanInfo, totalFees);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.MoveToElement(_driver, dashboard.loanDepotLogoLocBy);
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false, helocLoanInfo: helocLoanInfo);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (totalFees > 0 && helocLoanInfo.IsBillGenerated)
                    webElementExtensions.VerifyElementText(_driver, payments.lateFeeMessageTextLocBy, Constants.CustomerPortalErrorMsgs.LateFeeMessage, "Late Fee Message", true);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");
                if (ConfigSettings.PaymentsDataDeletionRequired)
                {

                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ScrollToTop(_driver);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: false, type: "edit");

                    //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow                    
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


                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");
                    webElementExtensions.ScrollToBottom(_driver);

                    commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

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

                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR-1013 Test HELOC OTP Past due - After Bill Gen - With fees")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocOtp")]
        public void TPR_1013_TPR_VerifyHelocOTPPastDueAfterBillGenerationWithFees()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            APIConstants.HelocLoanInfo helocLoanInfo = new APIConstants.HelocLoanInfo();
            Dictionary<string, string> feeDic = new Dictionary<string, string>();
            decimal totalFees = new decimal(0);
            try
            {
                TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById(Constants.TimeZones.PacificStandardTime);
                DateTime pstNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, pstZone);

                if (pstNow.Day < 17)
                {
                    test.Log(Status.Warning, $"Since Current Date in PST is {pstNow} which is Before 17th of the Month. Bill is not Yet Generated for Upcoming month!!!");
                    return;
                }
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
                        helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                        feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                        totalFees = feeDic?.Values
                            .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                            .Sum() ?? 0;
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
                        {
                            if (commonServices.GetHelocLoanInfo(loanLevelData[retryCount]).IsBillGenerated && totalFees > 0)
                            {
                                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated for the upcoming month</b>" : "<b>Bill is not Generated for the upcoming month</b>");
                                test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                                break;
                            }
                        }
                        else
                        {
                            test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated for the upcoming month</b>" : "<b>Bill is not Generated for the upcoming month</b>");
                            test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                        }
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
                        helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                        feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                        totalFees = feeDic?.Values
                            .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                            .Sum() ?? 0;
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
                        {
                            if (commonServices.GetHelocLoanInfo(loanLevelData[retryCount]).IsBillGenerated && totalFees > 0)
                            {
                                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                                test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                                break;
                            }
                        }
                        else
                        {
                            test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated</b>" : "<b>Bill is Not Generated</b>");
                            test.Log(Status.Info, (totalFees == 0) ? "<b>Without Fee</b>" : "<b>With Fee</b>");
                        }
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
                dashboard.VerifyPaymentSummaryDetailsForHelocLoanOnDashboardPage(loanLevelData[retryCount], helocLoanInfo, totalFees);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.MoveToElement(_driver, dashboard.loanDepotLogoLocBy);
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData[retryCount], 10.00, 15.00, isScrollIntoViewRequired: false, helocLoanInfo: helocLoanInfo);
                string amountForSMC = totalPayment;
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (totalFees > 0 && helocLoanInfo.IsBillGenerated)
                    webElementExtensions.VerifyElementText(_driver, payments.lateFeeMessageTextLocBy, Constants.CustomerPortalErrorMsgs.LateFeeMessage, "Late Fee Message", true);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");
                if (ConfigSettings.PaymentsDataDeletionRequired)
                {

                    //Edit the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to EDIT OTP</u>********************************************</b>");

                    payments.EditOtpPayment(payment_date);

                    //verify update payment is disabled
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy);
                    webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                    webElementExtensions.ScrollToTop(_driver);
                    ReportingMethods.Log(test, "We are Editiing the Payment By Updating the Bank Account");
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: false, type: "edit");

                    //Edit the amount 15.00 and 20.00 more than monin additional Principal and escrow                   
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


                    string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                    foreach (string keyword in editOtpKeywords)
                        ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");
                    webElementExtensions.ScrollToBottom(_driver);

                    commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    reportLogger.TakeScreenshot(test, "Dashboard Page");

                    //SMC Verification
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Setup</u>********************************************</b>");
                    //Delete the OTP setup done in previous steps
                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Delete OTP</u>********************************************</b>");

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

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

                    test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
                    test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for Heloc OTP Delete</u>********************************************</b>");
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
        [Description("<br> TPR 2754 - Verify Edit and Delete options are hidden for todays draft date from pending payment after the cutoff (8PMCST) <br>")]
        [TestCategory("CP_OTP_BeforeCutoff_TimeSensitive"), TestCategory("CP_Regression")]
        public void TPR_2754_TPR_VerifyOTPSetupForLoanStatusHELOCOntimeBeforeCutoffTime()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData

            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {eligibleHelocLoans}  </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(eligibleHelocLoans, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
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

                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, false, false, false))
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
                APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                Dictionary<string, string> feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                decimal totalFees = feeDic?.Values
                    .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                    .Sum() ?? 0;
                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated for the upcoming month</b>" : "<b>Bill is not Generated for the upcoming month</b>");
                dashboard.VerifyPaymentSummaryDetailsForHelocLoanOnDashboardPage(loanLevelData[retryCount], helocLoanInfo, totalFees);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.MoveToElement(_driver, dashboard.loanDepotLogoLocBy);
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

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
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                commonServices.ClickButtonUsingName(Constants.ButtonNames.BackToAccountSummary, "Back To Account Summary Button", true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                DateTime payment_date = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
                ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

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
        [Description("<br> TPR 2755 - Verify Edit and Delete options are hidden for todays draft date from pending payment after the cutoff (8PMCST) <br>")]
        [TestCategory("CP_OTP_AfterCutoff_TimeSensitive"), TestCategory("CP_Regression")]
        public void TPR_2755_TPR_VerifyEditeDeletingofExisitngOTPSetupForHELOCLoanStatusOntimeAfterCutoffTime()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();

            #region TestData

            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {eligibleHelocLoansForTimeSensitive}  </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(eligibleHelocLoansForTimeSensitive, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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




    }
}
