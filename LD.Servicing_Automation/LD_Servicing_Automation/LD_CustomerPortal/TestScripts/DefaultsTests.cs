using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_CustomerPortal.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace LD_CustomerPortal.Tests
{
    [TestClass]
    public class DefaultsTests : BasePage
    {
        string loanDetailsForActiveForbearance = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForActiveForbearance));
        string loanDetailsForRepay = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForActiveRepayment));
        string loanDetailsForActiveModificationTrail = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForModificationTrail));
        string modTrialBrokenOrDeletedLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData("LoanDetails.xml", "xml/Query_GetLoanLevelDetailsForBrokenOrDeletedModificationTrial"));


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
        [Description("<br>    TPR-940-Verify that on clicking the Edit payment button from Review screen taking the user to Edit Payment screen for the active Repayment/ Forbearance/ Modification Trial/ Delinquent Loan" +
                             "TPR-1011-Verify that on clicking cancel button from the Delete Confirmation popup taking the user to Pending payment page for the Forbearance loan " +
                             "TPR-968-Verify the Prepay Option is hidden in the one time payment screen when the user is having active Forbearance plan " +
                             "TPR-957-Verify inline message when user tries to pay more than reinstatement amount " +
                             "TPR-1014-Verify that the user is able to delete the OTP Pending payment details for the Forbearance loan " +
                             "TPR-1006-Verify that the user is able to edit the OTP Pending payment details for the Forbearance loan " +
                             "TPR-978-Verify the user is not having option to pay additional Principal, Additional Escrow and fees  in the one time payment screen when the user is having active Forbearance plan " +
                             "TPR-1008-Verify the user is able to make payment without any authorization for Active Forbearance " +
                             "TPR-987-Verify the Payment dates in the one time payment screen when the user is having active Forbearance plan and due is current month and current date is before cut off time. " +
                             "TPR-990-Verify the Plan status when the user have not done the payment when the loan is in active Forbearance plan and current date is before cut off time. " +
                             "TPR-961-Verify the user is not having option to pay additional Principal, Additional Escrow and fees  in the one time payment screen when the user is having active Forbearance plan " +
                             "TPR-964-Verify user is allowed to make OTP for amount less than total reinstated amount " +
                             "TPR-965-Verify user is allowed to pay the total reinstatement amount")]
        [TestCategory("CP_Regression"), TestCategory("CP_Defaults")]
        public void TPR_940_1011_968_957_1014_1006_978_1008_987_990_961_964_965_TPR_VerifyOTPSetupEditAndDeleteForActiveForbearance()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData

            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<b>{loanDetailsForActiveForbearance}</b>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForActiveForbearance, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                bool retrying = false;

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                    {
                        test.Log(Status.Fail, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        Assert.Fail("Could not find the correct set of loan level test data even after " + retryCount + " retries.");

                    }
                    var loan_number = loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.SmallWaitTime);
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        if (_driver.Url.Contains("link-user-loan"))
                        {
                            dashboard.LinkLoan(loan_number, loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        }
                        else
                        {
                            webElementExtensions.WaitUntilUrlContains("/dashboard");
                            dashboard.HandleServiceChatBot();
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
                            dashboard.LinkLoan(loan_number, loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        }
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                    }
                    else
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);

                    }
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.statusOfActiveForbearanceLocBy);
                    string loanStatus = webElementExtensions.GetElementText(_driver, dashboard.accountStandingStatusLocBy);
                    bool isactiveForbearance = webElementExtensions.IsElementDisplayed(_driver, dashboard.statusOfActiveForbearanceLocBy, isScrollIntoViewRequired: false);
                    //make payment button is enabled for couple of seconds for ineligible loans then 
                    webElementExtensions.WaitForPageLoad(_driver);
                    bool isEligble = webElementExtensions.IsElementEnabled(_driver, dashboard.btnMakePaymentLocBy, isScrollIntoViewRequired: false);
                    if (isactiveForbearance && isEligble)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                        List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                        reportLogger.ReportWriter(true, test, $"There are {numberOfPendingPayments} pending payments, {string.Join(Environment.NewLine, pendingPaymentDates)}", false);
                        bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);

                        //Values from Db
                        string emailId = username.ToString();
                        string principalAndInterestDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount].ToString();
                        string taxAndInsuranceDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString();
                        string feesDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.OtherFees].ToString();
                        string delinquentBalanceDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.DelinquentPaymentBalance].ToString();
                        string lateChargeAmountDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.AccruedLateChargeAmount].ToString();
                        string optout_code = loanLevelData[retryCount][Constants.LoanLevelDataColumns.OptOutStopCode].ToString();
                        string amountDueCalculated = (Convert.ToDouble(delinquentBalanceDb) + Convert.ToDouble(lateChargeAmountDb)).ToString("0.00");
                        string monthlyAmountCalculated = (Convert.ToDouble(principalAndInterestDb) + Convert.ToDouble(taxAndInsuranceDb)).ToString("0.00");

                        dashboard.VerifyPaymentSummary(loanLevelData[retryCount]);
                        webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);

                        if (willScheduledPaymentInProgressPopAppear)
                            payments.AcceptScheduledPaymentIsProcessingPopUp();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                        //Setup OTP 
                        var payment_date = payments.SetupOTP(pendingPaymentDates, monthly_payment: monthlyAmountCalculated, amount_due: amountDueCalculated, true, true, emailId, optout_code);
                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy, "Back To Account Summary Button", true);
                        webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                        var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                        //Edit the OTP setup done in previous steps
                        test.Log(Status.Info, $"<b><u>Started Process to EDIT OTP</u></b>");
                        payments.EditOtpPayment(payment_date);

                        //verify update payment is disabled
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy, isScrollIntoViewRequired: false);
                        webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", isScrollIntoViewRequired: false);
                        reportLogger.TakeScreenshot(test, "Edit OTP");
                        //Edit the amount 100 more than monthly amount
                        var amount_to_edit = (Convert.ToDouble(monthlyAmountCalculated) + 100).ToString("0.00");
                        webElementExtensions.EnterText(_driver, payments.reinstatementInputLocBy, amount_to_edit, isClickRequired: false, isScrollIntoViewRequired: false);
                        webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", isScrollIntoViewRequired: false);
                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                        List<string> editOtpKeywords = new List<string>() {
                            double.Parse(amount_to_edit).ToString("N"),
                            "One-Time Payment",
                        };

                        reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                        foreach (string keyword in editOtpKeywords)
                            ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                        webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryLinkLocBy);

                        string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                        foreach (string keyword in editOtpKeywords)
                            ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                        //SMC Verification
                        test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                        webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                        reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                        string draftDate = DateTime.Parse(payment_date.ToString()).ToString("MM/dd/yyyy");
                        string title = " loanDepot One Time Payment Notice - SM258 ";
                        string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                        string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                        string monthly_payment_formatted = double.Parse(monthlyAmountCalculated).ToString("N");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(monthly_payment_formatted), messageTemplateContent.Contains(monthlyAmountCalculated) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{monthly_payment_formatted}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{monthly_payment_formatted}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                        test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");

                        webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        if (ConfigSettings.PaymentsDataDeletionRequired)
                        {
                            //Delete the OTP setup done in previous steps
                            test.Log(Status.Info, $"<b><u>Started Process to Delete OTP</u></b>");

                            payments.DeleteOtpPaymentSetup(payment_date);

                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                            webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                            webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                            reportLogger.TakeScreenshot(test, "Delete OTP");
                            webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                            webElementExtensions.WaitForPageLoad(_driver);
                            webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                            var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                            var counter = 0;

                            while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                            {
                                pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                                counter++;
                            }
                            reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                            ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Pending Payment Activity should not have deleted OTP payment");
                            test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                            webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                            reportLogger.TakeScreenshot(test, "Dashboard Page");
                            webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                            webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                            reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                            title = " loanDepot Cancellation Notice - SM260 ";
                            messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                            string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                            ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                            ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                            test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Delete</u>********************************************</b>");
                            break;
                        }
                        break;
                    }
                    else
                    {
                        retryCount++;
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
                throw;
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
        [Description("<br> TPR-859-Verify that on clicking the Edit payment button from Review screen taking the user to Edit Payment screen for the active Repayment/ Forbearance/ Modification Trial/ Delinquent Loan " +
                          "TPR-924-Verify that on clicking cancel button from the Delete Confirmation popup taking the user to Pending payment page for the Forbearance loan " +
                          "TPR-930-Verify the Prepay Option is hidden in the one time payment screen when the user is having active Forbearance plan")]
        [TestCategory("CP_Regression"), TestCategory("CP_Defaults")]
        public void TPR_859_924_930_TPR_VerifyOTPSetupEditAndDeleteForRepay()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<b>{loanDetailsForRepay}</b>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForRepay, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                bool retrying = false;

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                    {
                        test.Log(Status.Fail, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        Assert.Fail("Could not find the correct set of loan level test data even after " + retryCount + " retries.");

                    }
                    var loan_number = loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.SmallWaitTime);
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        if (_driver.Url.Contains("link-user-loan"))
                        {
                            dashboard.LinkLoan(loan_number, loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        }
                        else
                        {
                            dashboard.HandlePaperLessPage();
                            webElementExtensions.WaitUntilUrlContains("/dashboard");
                            dashboard.HandleServiceChatBot();
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
                            dashboard.LinkLoan(loan_number, loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        }
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                    }
                    else
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                    }
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.statusOfActiveRepayLocBy);
                    string loanStatus = webElementExtensions.GetElementText(_driver, dashboard.accountStandingStatusLocBy);
                    bool isRepay = webElementExtensions.IsElementDisplayed(_driver, dashboard.statusOfActiveRepayLocBy, isScrollIntoViewRequired: false);
                    //make payment button is enabled for couple of seconds for ineligible loans then 
                    webElementExtensions.WaitForPageLoad(_driver);
                    bool isEligble = webElementExtensions.IsElementEnabled(_driver, dashboard.btnMakePaymentLocBy, isScrollIntoViewRequired: false);
                    if (isRepay && isEligble)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                        List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                        reportLogger.ReportWriter(true, test, $"There are {numberOfPendingPayments} pending payments, {string.Join(Environment.NewLine, pendingPaymentDates)}", false);
                        bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);

                        //Values from Db
                        string emailId = username.ToString();
                        string principalAndInterestDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount].ToString();
                        string taxAndInsuranceDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString();
                        string feesDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.OtherFees].ToString();
                        string delinquentBalanceDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.DelinquentPaymentBalance].ToString();
                        string lateChargeAmountDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.AccruedLateChargeAmount].ToString();
                        string optout_code = loanLevelData[retryCount][Constants.LoanLevelDataColumns.OptOutStopCode].ToString();
                        string amountDueCalculated = (Convert.ToDouble(delinquentBalanceDb) + Convert.ToDouble(lateChargeAmountDb)).ToString("0.00");
                        string monthlyAmountCalculated = (Convert.ToDouble(loanLevelData[retryCount][Constants.LoanLevelDataColumns.DueAmount]).ToString("0.00"));

                        dashboard.VerifyPaymentSummary(loanLevelData[retryCount]);
                        webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);

                        if (willScheduledPaymentInProgressPopAppear)
                            payments.AcceptScheduledPaymentIsProcessingPopUp();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                        //Setup OTP 
                        var payment_date = payments.SetupOTP(pendingPaymentDates, monthly_payment: monthlyAmountCalculated, amount_due: amountDueCalculated, true, true, emailId, optout_code, true, payments.repayAmountLocBy);
                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy, "Back To Account Summary Button", true);
                        webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                        var pendingPayments = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                        var pendingPaymentsAfterSetup = pendingPayments.Select(e => DateTime.Parse(e, CultureInfo.InvariantCulture)).ToList();
                        ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                        //Edit the OTP setup done in previous steps
                        test.Log(Status.Info, $"<b><u>Started Process to EDIT OTP</u></b>");
                        payments.EditOtpPayment(payment_date);

                        //verify update payment is disabled
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy, isScrollIntoViewRequired: false);
                        webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", isScrollIntoViewRequired: false);

                        //Change the Date and Edit the payment
                        reportLogger.TakeScreenshot(test, "Edit OTP");
                        var newPaymentDate = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPayments);
                        webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", isScrollIntoViewRequired: false);
                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                        List<string> editOtpKeywords = new List<string>() {
                            newPaymentDate,
                            "One-Time Payment",
                        };

                        reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                        foreach (string keyword in editOtpKeywords)
                            ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                        webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryLinkLocBy);

                        string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                        foreach (string keyword in editOtpKeywords)
                            ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        //SMC Verification
                        test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                        webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                        reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                        string draftDate = DateTime.Parse(payment_date.ToString()).ToString("MM/dd/yyyy");
                        string title = " loanDepot One Time Payment Notice - SM258 ";
                        string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                        string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                        string monthly_payment_formatted = double.Parse(monthlyAmountCalculated).ToString("N");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(monthly_payment_formatted), messageTemplateContent.Contains(monthlyAmountCalculated) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{monthly_payment_formatted}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{monthly_payment_formatted}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                        test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");
                        webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        if (ConfigSettings.PaymentsDataDeletionRequired)
                        {
                            //Delete the OTP setup done in previous steps
                            test.Log(Status.Info, $"<b><u>Started Process to Delete OTP</u></b>");

                            payments.DeleteOtpPaymentSetup(DateTime.Parse(newPaymentDate, CultureInfo.InvariantCulture));

                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                            webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                            webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                            reportLogger.TakeScreenshot(test, "Delete OTP");
                            webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                            webElementExtensions.WaitForPageLoad(_driver);
                            webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                            var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                            var counter = 0;

                            while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                            {
                                pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                                counter++;
                            }
                            reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                            ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Pending Payment Activity should not have deleted OTP payment");
                            test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                            webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                            reportLogger.TakeScreenshot(test, "Dashboard Page");
                            webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                            webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                            reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                            title = " loanDepot Cancellation Notice - SM260 ";
                            messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                            string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                            ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                            ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                            test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Delete</u>********************************************</b>");
                            break;
                        }
                        break;
                    }
                    else
                    {
                        retryCount++;
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
                throw;
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
        [Description("<br>TPR-651-Verify that on clicking the Edit payment button from Review screen taking the user to Edit Payment screen for the active Repayment/ Forbearance/ Modification Trial/ Delinquent Loan " +
                         "TPR-610-Verify that on clicking cancel button from the Delete Confirmation popup taking the user to Pending payment page for the Forbearance loan ")]
        [TestCategory("CP_Regression"), TestCategory("CP_Defaults")]
        public void TPR_651_610_TPR_VerifyOTPSetupEditAndDeleteForActiveModificationTrails()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<b>{loanDetailsForActiveModificationTrail}</b>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForActiveModificationTrail, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                bool retrying = false;

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                    {
                        test.Log(Status.Fail, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        Assert.Fail("Could not find the correct set of loan level test data even after " + retryCount + " retries.");

                    }
                    var loan_number = loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.SmallWaitTime);
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        if (_driver.Url.Contains("link-user-loan"))
                        {
                            dashboard.LinkLoan(loan_number, loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        }
                        else
                        {
                            webElementExtensions.WaitUntilUrlContains("/dashboard");
                            dashboard.HandleServiceChatBot();
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
                            dashboard.LinkLoan(loan_number, loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        }
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        if (dashboard.WillPaperLessPageAppear(loanLevelData[retryCount]))
                            dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                    }
                    else
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);

                    }
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.statusOfModificationTrail);
                    string loanStatus = webElementExtensions.GetElementText(_driver, dashboard.accountStandingStatusLocBy);
                    bool isactiveModificationTrail = webElementExtensions.IsElementDisplayed(_driver, dashboard.statusOfModificationTrail, isScrollIntoViewRequired: false);
                    //make payment button is enabled for couple of seconds for ineligible loans then 
                    webElementExtensions.WaitForPageLoad(_driver);
                    bool isEligble = webElementExtensions.IsElementEnabled(_driver, dashboard.btnMakePaymentLocBy, isScrollIntoViewRequired: false);
                    if (isactiveModificationTrail && isEligble)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                        List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                        reportLogger.ReportWriter(true, test, $"There are {numberOfPendingPayments} pending payments, {string.Join(Environment.NewLine, pendingPaymentDates)}", false);
                        bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);

                        //Values from Db
                        string emailId = username.ToString();
                        string principalAndInterestDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount].ToString();
                        string taxAndInsuranceDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString();
                        string feesDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.OtherFees].ToString();
                        string delinquentBalanceDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.DelinquentPaymentBalance].ToString();
                        string lateChargeAmountDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.AccruedLateChargeAmount].ToString();
                        string optout_code = loanLevelData[retryCount][Constants.LoanLevelDataColumns.OptOutStopCode].ToString();
                        string amountDueCalculated = (Convert.ToDouble(delinquentBalanceDb) + Convert.ToDouble(lateChargeAmountDb)).ToString("0.00");
                        string monthlyAmountCalculated = (Convert.ToDouble(principalAndInterestDb) + Convert.ToDouble(taxAndInsuranceDb)).ToString("0.00");

                        dashboard.VerifyPaymentSummary(loanLevelData[retryCount]);
                        webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);

                        if (willScheduledPaymentInProgressPopAppear)
                            payments.AcceptScheduledPaymentIsProcessingPopUp();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                        //Setup OTP 
                        var payment_date = payments.SetupOTP(pendingPaymentDates, monthly_payment: monthlyAmountCalculated, amount_due: amountDueCalculated, true, true, emailId, optout_code, isDefaultLoan: true, payments.modificationTrailAmountLocBy);
                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy, "Back To Account Summary Button", true);
                        webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                        var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                        //Edit the OTP setup done in previous steps
                        test.Log(Status.Info, $"<b><u>Started Process to EDIT OTP</u></b>");
                        payments.EditOtpPayment(payment_date);

                        //verify update payment is disabled
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy, isScrollIntoViewRequired: false);
                        webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", isScrollIntoViewRequired: false);
                        reportLogger.TakeScreenshot(test, "Edit OTP");
                        //Edit the amount 100 more than monthly amount

                        string accountFullName = "TESTFN1 TESTLN1";
                        string accountNumberWhileEdit = Constants.BankAccountData.BankAccountNumberWhileEdit;
                        string bankAccountName = Constants.BankAccountData.BankAccountName;
                        string firstName = "TESTFN1";
                        string lastName = "TESTLN1";
                        string personalOrBussiness = "Personal";
                        string routingNumber = "122199983";
                        string accountType = "Savings";
                        commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, accountType, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isReportRequired: false, isSelectBankAccountInTheEndRequired: true);
                        webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", isScrollIntoViewRequired: false);
                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                        List<string> editOtpKeywords = new List<string>() {
                            double.Parse(monthlyAmountCalculated).ToString("N"),
                            "One-Time Payment",
                        };

                        reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                        foreach (string keyword in editOtpKeywords)
                            ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                        webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryLinkLocBy);

                        string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);

                        foreach (string keyword in editOtpKeywords)
                            ReportingMethods.LogAssertionTrue(test, confirmEditOtpContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        //SMC Verification
                        test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                        webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                        reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                        string draftDate = DateTime.Parse(payment_date.ToString()).ToString("MM/dd/yyyy");
                        string title = " loanDepot One Time Payment Notice - SM258 ";
                        string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                        string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                        string monthly_payment_formatted = double.Parse(monthlyAmountCalculated).ToString("N");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(monthly_payment_formatted), messageTemplateContent.Contains(monthlyAmountCalculated) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{monthly_payment_formatted}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{monthly_payment_formatted}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                        test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");
                        webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        if (ConfigSettings.PaymentsDataDeletionRequired)
                        {
                            //Delete the OTP setup done in previous steps
                            test.Log(Status.Info, $"<b><u>Started Process to Delete OTP</u></b>");

                            payments.DeleteOtpPaymentSetup(payment_date);

                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                            webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                            webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                            reportLogger.TakeScreenshot(test, "Delete OTP");
                            webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                            webElementExtensions.WaitForPageLoad(_driver);
                            webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                            var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                            var counter = 0;

                            while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                            {
                                pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                                counter++;
                            }
                            reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                            ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Pending Payment Activity should not have deleted OTP payment");
                            test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                            webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                            reportLogger.TakeScreenshot(test, "Dashboard Page");
                            webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                            webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                            reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                            title = " loanDepot Cancellation Notice - SM260 ";
                            messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                            string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                            ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                            ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                            test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Delete</u>********************************************</b>");
                            break;
                        }
                        break;
                    }
                    else
                    {
                        retryCount++;
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
                throw;
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
        [Description("<br>TPR-624-Verify that user should not eb allowed to setup OTP for Modification Trial loan if due date < current date ")]
        [TestCategory("CP_Regression"), TestCategory("CP_Defaults")]
        public void TPR_624_TPR_VerifyOTPIneligbleMessageForBrokenOrDeletedModificationTrialLoans()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData

            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<b>{modTrialBrokenOrDeletedLoanDetailsQuery}</b>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(modTrialBrokenOrDeletedLoanDetailsQuery, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                bool retrying = false;

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                    {
                        test.Log(Status.Fail, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        Assert.Fail("Could not find the correct set of loan level test data even after " + retryCount + " retries.");

                    }
                    var loan_number = loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.SmallWaitTime);
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        if (_driver.Url.Contains("link-user-loan"))
                        {
                            dashboard.LinkLoan(loan_number, loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        }
                        else
                        {
                            webElementExtensions.WaitUntilUrlContains("/dashboard");
                            dashboard.HandleServiceChatBot();
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
                            dashboard.LinkLoan(loan_number, loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        }
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        if (dashboard.WillPaperLessPageAppear(loanLevelData[retryCount]))
                            dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                    }
                    else
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);

                    }
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                    webElementExtensions.ScrollToTop(_driver);
                    reportLogger.TakeScreenshot(test, "OTP Ineligible Msg");

                    bool isDisabled = webElementExtensions.IsElementDisabled(_driver, dashboard.makeAPaymentButtonLocBy);
                    ReportingMethods.LogAssertionTrue(test, isDisabled, "Make payment should be disabled");

                    string otpMsg = webElementExtensions.GetElementText(_driver, dashboard.loanNotEligibleMsgLocBy);
                    ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.ModificationTrailOTPErrorMsg, otpMsg, $"Messge should be {Constants.CustomerPortalErrorMsgs.ModificationTrailOTPErrorMsg}");
                    break;
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
                throw;
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
        [Description("<br>TPR-625-Verify Setup/Edit/Delete OTP for Broken/DeletedModificationTrails")]
        [TestCategory("CP_Regression"), TestCategory("CP_Defaults")]
        public void TPR_625_TPR_VerifyOTPSetupEditAndDeleteForBrokenOrDeletedModificationTrails()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            #region TestData
            string query = modTrialBrokenOrDeletedLoanDetailsQuery.Replace("<", ">=")
                .Replace("getdate()", "getdate() and ll.next_payment_due_date < DATEADD(month,1,getdate()) ");

            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<b>{query}</b>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(query, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                bool retrying = false;

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                    {
                        test.Log(Status.Fail, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        Assert.Fail("Could not find the correct set of loan level test data even after " + retryCount + " retries.");

                    }
                    var loan_number = loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.SmallWaitTime);
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        if (_driver.Url.Contains("link-user-loan"))
                        {
                            dashboard.LinkLoan(loan_number, loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        }
                        else
                        {
                            webElementExtensions.WaitUntilUrlContains("/dashboard");
                            dashboard.HandleServiceChatBot();
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
                            dashboard.LinkLoan(loan_number, loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        }
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        if (dashboard.WillPaperLessPageAppear(loanLevelData[retryCount]))
                            dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                    }
                    else
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);

                    }
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.statusOfModificationTrail);
                    //make payment button is enabled for couple of seconds for ineligible loans then 
                    webElementExtensions.WaitForPageLoad(_driver);
                    bool isEligble = webElementExtensions.IsElementEnabled(_driver, dashboard.btnMakePaymentLocBy, isScrollIntoViewRequired: false);
                    if (isEligble)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                        List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                        reportLogger.ReportWriter(true, test, $"There are {numberOfPendingPayments} pending payments, {string.Join(Environment.NewLine, pendingPaymentDates)}", false);
                        bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);

                        //Values from Db
                        string emailId = username.ToString();
                        string principalAndInterestDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount].ToString();
                        string taxAndInsuranceDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString();
                        string feesDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.OtherFees].ToString();
                        string delinquentBalanceDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.DelinquentPaymentBalance].ToString();
                        string lateChargeAmountDb = loanLevelData[retryCount][Constants.LoanLevelDataColumns.AccruedLateChargeAmount].ToString();
                        string optout_code = loanLevelData[retryCount][Constants.LoanLevelDataColumns.OptOutStopCode].ToString();
                        string amountDueCalculated = (Convert.ToDouble(delinquentBalanceDb) + Convert.ToDouble(lateChargeAmountDb)).ToString("0.00");
                        string monthlyAmountCalculated = (Convert.ToDouble(principalAndInterestDb) + Convert.ToDouble(taxAndInsuranceDb)).ToString("0.00");

                        dashboard.VerifyPaymentSummary(loanLevelData[retryCount]);
                        webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);

                        if (willScheduledPaymentInProgressPopAppear)
                            payments.AcceptScheduledPaymentIsProcessingPopUp();
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                        //Setup OTP 
                        var payment_date = payments.SetupOTP(pendingPaymentDates, monthly_payment: monthlyAmountCalculated, amount_due: amountDueCalculated, true, true, emailId, optout_code, isDefaultLoan: true, payments.modificationTrailAmountBrokenLocBy);
                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);
                        webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                        var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                        ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(payment_date), "Pending Payment Activity should have latest set payment");

                        //Edit the OTP setup done in previous steps
                        test.Log(Status.Info, $"<b><u>Started Process to EDIT OTP</u></b>");
                        payments.EditOtpPayment(payment_date);

                        //verify update payment is disabled
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updatePaymentBtnLocBy, isScrollIntoViewRequired: false);
                        webElementExtensions.IsElementDisabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", isScrollIntoViewRequired: false);
                        reportLogger.TakeScreenshot(test, "Edit OTP");
                        //Edit the amount 100 more than monthly amount

                        string accountFullName = "TESTFN1 TESTLN1";
                        string accountNumberWhileEdit = Constants.BankAccountData.BankAccountNumberWhileEdit;
                        string bankAccountName = Constants.BankAccountData.BankAccountName;
                        string firstName = "TESTFN1";
                        string lastName = "TESTLN1";
                        string personalOrBussiness = "Personal";
                        string routingNumber = "122199983";
                        string accountType = "Savings";

                        var accountNickNameOrAccountType = string.IsNullOrEmpty(bankAccountName) ? accountType : bankAccountName;
                        commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, accountType, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isReportRequired: false, isSelectBankAccountInTheEndRequired: true, type: "edit", makeTheAccountDefault: false);
                        commonServices.SelectValueInMethodDropdown(accountNickNameOrAccountType, Constants.BankAccountData.BankAccountNumber);
                        commonServices.SelectValueInMethodDropdown(accountNickNameOrAccountType, Constants.BankAccountData.BankAccountNumberWhileEdit);
                        webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", isScrollIntoViewRequired: false);
                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.updatePaymentBtnLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);
                        List<string> editOtpKeywords = new List<string>() {
                            double.Parse(monthlyAmountCalculated).ToString("N"),
                            "One-Time Payment",
                        };

                        reportLogger.TakeScreenshot(test, "Edit OTP Setup Review Page");

                        foreach (string keyword in editOtpKeywords)
                            ReportingMethods.LogAssertionContains(test, keyword, reviewPageContent, $"Validate that Review page text content contains the {keyword}</b>");

                        webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm OTP button.", true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                        string confirmEditOtpContent = webElementExtensions.GetElementText(_driver, payments.otpConfirmedPaymentMsgLocBy);
                        foreach (string keyword in editOtpKeywords)
                            ReportingMethods.LogAssertionContains(test, keyword, confirmEditOtpContent, $"Validate that Review page text content contains the {keyword}</b>");


                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmOtpPaymentBtnLocBy);
                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);

                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        //SMC Verification
                        test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Setup</u>********************************************</b>");
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                        webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                        reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                        string draftDate = DateTime.Parse(payment_date.ToString()).ToString("MM/dd/yyyy");
                        string title = " loanDepot One Time Payment Notice - SM258 ";
                        string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                        string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM258, "Monthly");
                        string monthly_payment_formatted = double.Parse(monthlyAmountCalculated).ToString("N");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(monthly_payment_formatted), messageTemplateContent.Contains(monthlyAmountCalculated) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{monthly_payment_formatted}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{monthly_payment_formatted}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                        test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Setup</u>********************************************</b>");
                        webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Logo");
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        if (ConfigSettings.PaymentsDataDeletionRequired)
                        {
                            //Delete the OTP setup done in previous steps
                            test.Log(Status.Info, $"<b><u>Started Process to Delete OTP</u></b>");

                            payments.DeleteOtpPaymentSetup(payment_date);

                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                            webElementExtensions.WaitForElement(_driver, payments.otpDeleteReasonLocBy);
                            webElementExtensions.ActionClick(_driver, payments.otpDeleteReasonLocBy);
                            reportLogger.TakeScreenshot(test, "Delete OTP");
                            webElementExtensions.ActionClick(_driver, payments.autopayDeleteConfirmLocBy);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.autopayDeleteConfirmLocBy);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                            webElementExtensions.WaitForPageLoad(_driver);
                            webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
                            var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                            var counter = 0;

                            while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                            {
                                pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                                counter++;
                            }
                            reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                            ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Pending Payment Activity should not have deleted OTP payment");
                            test.Log(Status.Info, $"<b>********************************************<u>Started Process to Validate SMC for OTP Delete</u>********************************************</b>");
                            webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                            reportLogger.TakeScreenshot(test, "Dashboard Page");
                            webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                            webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                            reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                            title = " loanDepot Cancellation Notice - SM260 ";
                            messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM260, "Monthly");
                            string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                            ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                            ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
                            test.Log(Status.Info, $"<b>********************************************<u>Ending Process to Validate SMC for OTP Delete</u>********************************************</b>");
                            break;
                        }
                        break;
                    }
                    else
                    {
                        retryCount++;
                    }
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
                throw;
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
