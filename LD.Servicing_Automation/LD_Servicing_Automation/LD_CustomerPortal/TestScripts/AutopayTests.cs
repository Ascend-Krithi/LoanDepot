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
using System.Text.RegularExpressions;

namespace LD_CustomerPortal.Tests
{
    [TestClass]
    public class AutopayTests : BasePage
    {

        string loanDetailsForDeleteAutopay = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetEligibleAutopayDeleteLoans));
        string loanDetailsForDeleteAutopayForPastDue = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetEligibleAutopayDeleteLoansForPastDue));
        string loanDetailsForDeleteAutopayForOntime = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetEligibleAutopayDeleteLoansForOntime));
        string loanDetailsForDeleteAutopayForPrepaidOneMonth = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetEligibleAutopayDeleteLoansForPrepaidOneMonth));
        string loanDetailsForBiWeekly = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForBiWeeklyAutoPay));
        string loanDetailsForBiWeeklyOntime = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForBiWeeklyAutoPayOntime));
        string loanDetailsForBiWeeklyOneMonthPrepaid = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForBiWeeklyAutoPayOneMonthPrepaid));
        string loanDetailsForBiWeeklyTwoMonthPrepaid = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForBiWeeklyAutoPayTwoMonthPrepaid));
        string loanDetailsQueryForEligibleAutopayPastDue = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleAutoPayPastDue), Constants.LoanStatus.PastDue);
        string loanDetailsQueryForEligibleAutopayOntime = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleAutoPayOntime), Constants.LoanStatus.Ontime);
        string loanDetailsQueryForEligibleAutopayOntimeW01 = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleAutoPayOntimeW01));
        string loanDetailsQueryForEligibleAutopayPrepaid = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleAutoPayPrepaid), Constants.LoanStatus.PrepaidOneMonth);
        string loanDetailsForDeleteBiWeeklyAutopay = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetEligibleAutopayBiWeeklyDeleteLoans));
        string loanDetailsForExisitngAutopayFM = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.FMExistingAutopayOnTodayDraftDateLoans));
        string loanDetailsForExisitngAutopayHeloc = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.HelocExistingAutopayOnTodayDraftDateLoans));


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
        YopmailPage yopmailPage = null;
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
            yopmailPage = new YopmailPage(_driver, test);
            dBconnect = new DBconnect(test, Constants.DBNames.MelloServETL);
            reportLogger = new ReportLogger(_driver);
            //unlink loans from test account 
            dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
            test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            string queryToUpdateTCPAFlagIsGlobalValue = UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.UpdateTCPAFlagIsGlobalValue).Replace("TCPA_FLAG_VALUE", "0");
            dBconnect.ExecuteQuery(queryToUpdateTCPAFlagIsGlobalValue).ToString();
        }

        [TestMethod]
        [Description("<br>TPR-1166-E2E_Setup_Edit_Autopay_Monthly_PastDue <br>" +
                         "TPR-481-Verify Autopay eligibility for loans with different loan types PastDue")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_1166_481_TRP_VerifyTheAutoPaySetup_Edit_FunctionalityFor_MonthlyPastDue()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            List<string> exptDates = new List<string>();
            bool isEligible = false;
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>{loanDetailsQueryForEligibleAutopayPastDue}</b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleAutopayPastDue, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                exptDates = commonServices.GetExpectedAvailableDates(Constants.LoanStatus.PastDue);
                isEligible = (exptDates == null) ? false : true;
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: isEligible))
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), checkManageAutopayPage: isEligible))
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
                if (isEligible)
                {
                    bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                    int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                    List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                    test.Log(Status.Info, $"<b><u>Started Process to Setup Autopay Monthly plan</u></b>");
                    webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                    test.Log(Status.Info, $"<b><u>Started Process to Setup Autopay Monthly plan</u></b>");
                    payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);
                    string actualTotalAmountOnSetupAutoPayPage = webElementExtensions.GetElementText(_driver, payments.totalAutoPayAmountSetUpAutoPayLocBy);
                    string amount = Regex.Match(actualTotalAmountOnSetupAutoPayPage, @"\$\d{1,3}(,\d{3})*(\.\d{2})?").Value;
                    string actTotalAmount = webElementExtensions.GetElementText(_driver, payments.monthlyPaymentPlanAmountTextLocBy);
                    webElementExtensions.VerifyElementText(_driver, payments.monthlyPaymentPlanAmountTextLocBy, amount, "Monthly Autopay payment plan Amount", isReportRequired: true);
                    decimal principalInterest = Convert.ToDecimal(loanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount]);
                    string interest = string.Format(new CultureInfo("en-US"), "{0:C}", principalInterest);
                    decimal taxInDecimal = Convert.ToDecimal(loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount]);
                    string tax = string.Format(new CultureInfo("en-US"), "{0:C}", taxInDecimal);
                    string paymentDate = payments.SetupAutopay(pendingPaymentDates, loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString(), amount, interest, tax, true, true);
                    pendingPaymentDates.Add(paymentDate);
                    string paymentDateForSMC = paymentDate;
                    webElementExtensions.ScrollIntoView(_driver, payments.backToManageAutoPayButtonLocBy);
                    webElementExtensions.ActionClick(_driver, payments.backToManageAutoPayButtonLocBy, "Back To Manage AutoPay Button", true);
                    webElementExtensions.ScrollToTop(_driver);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);
                    reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                    test.Log(Status.Info, $"<b>**********************************<u>Manage Autopay Page Content Validation Started</u>**********************************</b>");
                    webElementExtensions.VerifyElementText(_driver, payments.planForAutopayPaymentManageAutopayTextLocBy, "Monthly", "Autopay Payment Plan", true);
                    webElementExtensions.VerifyElementText(_driver, payments.nextDebitDateManageAutoPayTextLocBy, paymentDate, isReportRequired: true);
                    webElementExtensions.VerifyElementText(_driver, payments.amountOnManageAutoPayTextLocBy, amount, isReportRequired: true);

                    // Edit Process Started
                    test.Log(Status.Info, $"<b>**********************************<u>Edit Process Started</u>**********************************</b>");
                    webElementExtensions.WaitForElementToBeEnabled(_driver, payments.editIconManageAutopayLocBy, isScrollIntoViewRequired: false);
                    webElementExtensions.ClickElement(_driver, payments.editIconManageAutopayLocBy, "Edit Button on Manage Autopay", isReportRequired: true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.editIconManageAutopayLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.WaitForElement(_driver, payments.updateAutopayButtonEditAutopayLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, isScrollIntoViewRequired: false, timeoutInSeconds: ConfigSettings.SmallWaitTime);
                    bool isUpdateAutopayButtonDisabled = webElementExtensions.IsElementDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Autopay Page", isScrollIntoViewRequired: false);
                    ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonDisabled, isUpdateAutopayButtonDisabled ? $"Successfully validated that on the Edit Autopay Page Update Autopay buttonis <b>Disabled</b>" : $"Failed - On Edit Autopay Page Update Autopay button is <b>Enabled</b>");
                    reportLogger.TakeScreenshot(test, "Update Button Default View on Edit Autopay Page", true);
                    webElementExtensions.ScrollToTop(_driver);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true, isReportRequired: false, type: "edit");
                    accountNumberWhileEdit = accountNumberWhileEdit.Substring(accountNumber.Length - 4);
                    webElementExtensions.WaitForElementToBeEnabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, isScrollIntoViewRequired: false);
                    bool isUpdateAutopayButtonEnabled = webElementExtensions.IsElementEnabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Autopay Page");
                    ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonEnabled, isUpdateAutopayButtonEnabled ? $"Successfully validated that on the Edit Autopay Page Update Autopay buttonis <b>Enabled</b>" : $"Failed - On Edit Autopay Page Update Autopay button is <b>Disabled</b>");
                    reportLogger.TakeScreenshot(test, "Update Button View on Edit Autopay Page after adding Bank Account");

                    string amountForSMC = amount;
                    webElementExtensions.ScrollToTop(_driver);
                    bool isAdditionalMonthlyPrincipalCheckBoxEnabled = webElementExtensions.IsElementEnabled(_driver, commonServices.additionalMonthlyPrincipalCheckBoxInputLocBy, isScrollIntoViewRequired: false);
                    if (isAdditionalMonthlyPrincipalCheckBoxEnabled)
                    {
                        webElementExtensions.ScrollToTop(_driver);
                        webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.additionalMonthlyPrincipalCheckBoxLabelLocBy);
                        bool isAdditionalMonthlyPaymentInputFieldIsEnabled = webElementExtensions.IsElementEnabled(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, "Additional Monthly Principal Input filed on Autopay Page");
                        ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonEnabled, isUpdateAutopayButtonEnabled ? $"Successfully validated that on the Edit Autopay Page Additional monthly payment input field is <b>Enabled</b>" : $"Failed - On Edit Autopay Page Additional monthly payment input field is <b>Disabled</b>");
                        webElementExtensions.EnterText(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, "100", isReportRequired: true, isClickRequired: true);
                        amount = (double.Parse(decimal.Parse(amount.Replace(",", "").TrimStart('$')).ToString()) + 1).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                    }

                    paymentDate = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, "edit", isScrollIntoViewRequired: false);
                    webElementExtensions.WaitForElementToBeEnabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Edit Autopay", isScrollIntoViewRequired: true);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Edit Autopay", isReportRequired: true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.editAutopayReviewPageTextContentLocBy, ConfigSettings.SmallWaitTime);
                    test.Log(Status.Info, $"<b><u>Review Page Content Validation Started</u></b>");
                    webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.confirmAutopayButtonLocBy, isScrollIntoViewRequired: false);
                    reportLogger.TakeScreenshot(test, "Autopay Setup Review Page", true);
                    webElementExtensions.ScrollToTop(_driver);
                    string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.editAutopayReviewPageTextContentLocBy, true);
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(amount), reviewPageContent.Contains(amount) ? $"Successfully validated that Review page text content contains the expected Total Amount - <b>{amount}</b>" : $"Failed while verifying Total Amount - <b>{amount}</b>, please check the Review Page Text content in logs");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains("Monthly"), reviewPageContent.Contains("Monthly") ? $"Successfully validated that Review page text content contains the expected Autopay plan Type - <b>Monthly</b>" : $"Failed while verifying Autopay Plan Type - <b>Monthly</b>, please check the Review Page Text content in logs");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains("*******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)), reviewPageContent.Contains("*******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)) ? $"Successfully validated that Review page text content contains the expected Loan Number - <b>*******{loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)}</b>" : $"Failed while verifying Loan Number - <b>*******{loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)}</b>, please check the Review Page Text content in logs");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(paymentDate), reviewPageContent.Contains(paymentDate) ? $"Successfully validated that Review page text content contains the expected Payment Start Date - <b>{paymentDate}</b>" : $"Failed while verifying Payment Start Date - <b>{paymentDate}</b>, please check the Review Page Text content in logs");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(bankAccountName), reviewPageContent.Contains(bankAccountName) ? $"Successfully validated that Review page text content contains the expected Bank Account Name - <b>{bankAccountName}</b>" : $"Failed while verifying Bank Account Name - <b>{bankAccountName}</b>, please check the Review Page Text content in logs");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(accountNumberWhileEdit), reviewPageContent.Contains(accountNumberWhileEdit) ? $"Successfully validated that Review page text content contains the expected Bank Account Number - <b>{accountNumberWhileEdit}</b>" : $"Failed while verifying Bank Account Number - </b>{accountNumberWhileEdit}</b>, please check the Review Page Text content in logs");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupReviewPageDisclosureText), reviewPageContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupReviewPageDisclosureText) ? $"Successfully validated that Review page text content contains the expected Disclosure Text - <b>{Constants.CustomerPortalTextMessages.AutopaySetupReviewPageDisclosureText}</b>" : $"Failed while verifying Bank Account Name - <b>{Constants.CustomerPortalTextMessages.AutopaySetupReviewPageDisclosureText}</b>, please check the Review Page Text content in logs");
                    webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.confirmAutopayButtonLocBy, "Confirm Autopay button.", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmAutopayButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                    test.Log(Status.Info, $"<b><u>Confirmation Page Content Validation Started</u></b>");
                    webElementExtensions.WaitForElementToBeEnabled(_driver, payments.backToManageAutoPayButtonLocBy, isScrollIntoViewRequired: false);
                    reportLogger.TakeScreenshot(test, "Autopay Setup Confirmation Page", true);
                    webElementExtensions.ScrollToTop(_driver);
                    string confirmationPageTextContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayConfirmationPageTextContentLocBy, true);
                    ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(amount), confirmationPageTextContent.Equals(amount) ? $"Successfully validated that Autopay Setup Confirmation Page contains Total Amount - <b>{amount}</b>" : $"Failed while verifying Total Amount on Autopay Setup Confirmation Page - <b>{amount}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains("Monthly"), confirmationPageTextContent.Contains("Monthly") ? $"Successfully validated that Review page text content contains the expected Autopay plan Type - <b>Monthly</b>" : $"Successfully validated that Review page text content contains the expected Autopay plan Type - <b>Monthly</b>");
                    ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains("*******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)), confirmationPageTextContent.Contains("*******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)) ? $"Successfully validated that Autopay Setup Confirmation Page text content contains the expected Loan Number - <b>*******{loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)}</b>" : $"Failed while verifying Loan Number - <b>*******{loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)}</b>, please check the Autopay Setup Confirmation Page Text content in logs");
                    ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(paymentDate), confirmationPageTextContent.Contains(paymentDate) ? $"Successfully validated that Autopay Setup Autopay Confirmation page text content contains the expected Payment Start Date - <b>{paymentDate}</b>" : $"Failed while verifying Payment Start Date - <b>{paymentDate}</b>, please check the Autopay Setup Confirmation Page Text content in logs");
                    ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(bankAccountName), confirmationPageTextContent.Contains(bankAccountName) ? $"Successfully validated that Setup Autopay Confirmation  page text content contains the expected Bank Account Name - <b>{bankAccountName}</b>" : $"Failed while verifying Bank Account Name - <b>{bankAccountName}</b>, please check the Autopay Setup Autopay Confirmation Page Text content in logs");
                    ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(accountNumberWhileEdit), confirmationPageTextContent.Contains(accountNumberWhileEdit) ? $"Successfully validated that Autopay Setup Autopay Confirmation Page Text content contains the expected Bank Account Number - <b>{accountNumberWhileEdit}</b>" : $"Failed while verifying Bank Account Number - <b>{bankAccountName.Substring(bankAccountName.Length - 4)}</b>, please check the Autopay Setup Autopay Confirmation Page Text content in logs");
                    ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupConfirmationPageDisclosureText), confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupConfirmationPageDisclosureText) ? $"Successfully validated that Autopay Setup Autopay Confirmation Page Text content contains the expected Disclosure Text - <b>{Constants.CustomerPortalTextMessages.AutopaySetupConfirmationPageDisclosureText}</b>" : $"Failed while verifying Bank Account Name - <b>{Constants.CustomerPortalTextMessages.AutopaySetupReviewPageDisclosureText}</b>, please check the Autopay Setup Autopay Confirmation Page Text content in logs");
                    reportLogger.TakeScreenshot(test, "Autopay Setup Confirmaion Page");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmAutopayButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.editAutopayConfirmationPageLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToManageAutoPayButtonLocBy, "Back To Manage AutoPay Button", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);
                    reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                    test.Log(Status.Info, $"<b><u>Manage Autopay Page Content Validation Started</u></b>");
                    webElementExtensions.VerifyElementText(_driver, payments.planForAutopayPaymentManageAutopayTextLocBy, "Monthly", "Autopay Payment Plan", true);
                    webElementExtensions.VerifyElementText(_driver, payments.nextDebitDateManageAutoPayTextLocBy, paymentDate, isReportRequired: true);
                    webElementExtensions.VerifyElementText(_driver, payments.amountOnManageAutoPayTextLocBy, amount, isReportRequired: true);

                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo", isReportRequired: true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToAccountSummaryLinkLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    bool flag = webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", "On Autopay");
                    ReportingMethods.LogAssertionTrue(test, flag, flag ? $"Successfully validated that on the Dashboard Page Manage Autopay toggle is <b>turnned OFF</b>" : $"Failed - Manage Autopay toggle is <b>turnned ON</b>");
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    string draftDate = DateTime.Parse(paymentDateForSMC).ToString("MM/dd/yyyy");
                    string title = " loanDepot Authorized Recurring Automatic Payment - SM203 ";
                    string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                    string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM203, "Monthly");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains("Monthly"), messageTemplateContent.Contains("Monthly") ? $"Successfully validated the Payment Frequency in SMC Message Template - <b>{"Monthly"}</b>" : $"Failed while verifying the payment Frequency In SMC Message Template - <b>{"Monthly"}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amountForSMC), messageTemplateContent.Contains(amountForSMC) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amountForSMC}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amountForSMC}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
                }
                else
                    test.Log(Status.Warning, "<font color='Orange'><b>Skipped the Execution ahead as right now PastDue loans are not Eligible for Autopay Setup</b></font>");
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
        [Description("<br>TPR-1165-E2E_Setup_Edit_Autopay_Monthly_Ontime <br>" +
                         "TPR-497-Verify Autopay eligibility for loans with different loan types- Ontime\n"+
            "TPR-1965-Test [FM Autopay] Frequency Disclosure is displayed on Edit Autopay Page")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay"), TestCategory("CP_Sanity")]
        public void TPR_1165_497_1965_TPR_VerifyTheAutoPaySetup_Edit_FunctionalityFor_MonthlyOntime()
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
                test.Log(Status.Info, $"<b>************************************<u>Started Process to Setup Autopay Monthly plan</u>************************************</b>");
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);
                string actualTotalAmountOnSetupAutoPayPage = webElementExtensions.GetElementText(_driver, payments.totalAutoPayAmountSetUpAutoPayLocBy);
                string amount = Regex.Match(actualTotalAmountOnSetupAutoPayPage, @"\$\d{1,3}(,\d{3})*(\.\d{2})?").Value;
                string actTotalAmount = webElementExtensions.GetElementText(_driver, payments.monthlyPaymentPlanAmountTextLocBy);
                webElementExtensions.VerifyElementText(_driver, payments.monthlyPaymentPlanAmountTextLocBy, amount, "Monthly Autopay payment plan Amount", isReportRequired: true);
                decimal principalInterest = Convert.ToDecimal(loanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount]);
                string interest = string.Format(new CultureInfo("en-US"), "{0:C}", principalInterest);
                decimal taxInDecimal = Convert.ToDecimal(loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount]);
                string tax = string.Format(new CultureInfo("en-US"), "{0:C}", taxInDecimal);
                string paymentDate = payments.SetupAutopay(pendingPaymentDates, loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString(), amount, interest, tax, true, true, isScroolIntoViewRequired: false);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.backToManageAutoPayButtonLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToManageAutoPayButtonLocBy, "Back To Manage AutoPay Button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                webElementExtensions.WaitForElement(_driver, payments.manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                test.Log(Status.Info, $"<b>*****************************************<u>Manage Autopay Page Content Validation Started</u>*****************************************</b>");
                webElementExtensions.VerifyElementText(_driver, payments.planForAutopayPaymentManageAutopayTextLocBy, "Monthly", "Autopay Payment Plan", true);
                webElementExtensions.VerifyElementText(_driver, payments.nextDebitDateManageAutoPayTextLocBy, paymentDate, isReportRequired: true);
                webElementExtensions.VerifyElementText(_driver, payments.amountOnManageAutoPayTextLocBy, amount, isReportRequired: true);

                string emailContent = yopmailPage.GetEmailContentFromYopmail(username, emailNotificationType: Constants.EmailNotificationFromYopmail.AutopayDelete);
                ReportingMethods.LogAssertionTrue(test, emailContent.Contains(Constants.EmailContents.AutopaySetupTitle), "Verify Autopay Setup Email Notification Title");
                ReportingMethods.LogAssertionTrue(test, emailContent.Contains(Constants.EmailContents.AutopaySetupEmailContentBody), "Verify Autopay Setup Email Notification Body Content");
                ReportingMethods.LogAssertionTrue(test, emailContent.Contains(Constants.EmailContents.SenderEmailId), "Verify Autopay Setup Email Notification Sender Email ID");

                // Edit Scenarios Starts from here
                test.Log(Status.Info, $"<b>*****************************************<u>Edit Scenarios Starts from here</u>*****************************************</b>");
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.editIconManageAutopayLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElement(_driver, payments.editIconManageAutopayLocBy, "Edit Button on Manage Autopay", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.editIconManageAutopayLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, isScrollIntoViewRequired: false, timeoutInSeconds: ConfigSettings.SmallWaitTime);
                bool isUpdateAutopayButtonDisabled = webElementExtensions.IsElementDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Autopay Page");
                string editAutopayMsg = webElementExtensions.GetElementText(_driver, payments.editAutopayTextLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.EditAutopayMsg, editAutopayMsg,$"Edit Autopay should display '{Constants.CustomerPortalTextMessages.EditAutopayMsg}'");
                ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonDisabled, isUpdateAutopayButtonDisabled ? $"Successfully validated that on the Edit Autopay Page Update Autopay button is <b>Disabled</b>" : $"Failed - On Edit Autopay Page Update Autopay button is <b>Enabled</b>");
                reportLogger.TakeScreenshot(test, "Update Button Default View on Edit Autopay Page");
                webElementExtensions.ScrollToTop(_driver);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isReportRequired: false, isScrollIntoViewRequired: false, type: "edit");
                accountNumberWhileEdit = accountNumberWhileEdit.Substring(accountNumber.Length - 4);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.updateAutopayButtonEditAutopayLocBy);
                bool isUpdateAutopayButtonEnabled = webElementExtensions.IsElementEnabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Autopay Page");
                ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonEnabled, isUpdateAutopayButtonEnabled ? $"Successfully validated that on the Edit Autopay Page Update Autopay buttonis <b>Enabled</b>" : $"Failed - On Edit Autopay Page Update Autopay button is <b>Disabled</b>");
                reportLogger.TakeScreenshot(test, "Update Button View on Edit Autopay Page after adding Bank Account");
                webElementExtensions.ScrollToTop(_driver);
                string amountForSMC = amount;
                bool isAdditionalMonthlyPrincipalCheckBoxEnabled = webElementExtensions.IsElementEnabled(_driver, commonServices.additionalMonthlyPrincipalCheckBoxInputLocBy, isScrollIntoViewRequired: false);
                if (isAdditionalMonthlyPrincipalCheckBoxEnabled)
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.additionalMonthlyPrincipalCheckBoxLabelLocBy);
                    bool isAdditionalMonthlyPaymentInputFieldIsEnabled = webElementExtensions.IsElementEnabled(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, "Additional Monthly Principal Input filed on Autopay Page", isScrollIntoViewRequired: false);
                    ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonEnabled, isUpdateAutopayButtonEnabled ? $"Successfully validated that on the Edit Autopay Page Additional monthly payment input field is <b>Enabled</b>" : $"Failed - On Edit Autopay Page Additional monthly payment input field is <b>Disabled</b>");
                    webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, "Additional Monthly Principal Input Text Box", isScrollIntoViewRequired: false);
                    webElementExtensions.EnterText(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, "100", isReportRequired: true, isClickRequired: true);
                    amount = (double.Parse(decimal.Parse(amount.Replace(",", "").TrimStart('$')).ToString()) + 1).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                }
                paymentDate = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, "edit", isScrollIntoViewRequired: false);
                webElementExtensions.ScrollToTop(_driver);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Edit Autopay", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.editAutopayReviewPageTextContentLocBy);
                test.Log(Status.Info, $"<b><u>Review Page Content Validation Started</u></b>");
                webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.confirmAutopayButtonLocBy, isScrollIntoViewRequired: false);
                reportLogger.TakeScreenshot(test, "Autopay Setup Review Page", true);
                webElementExtensions.ScrollToTop(_driver);
                string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.editAutopayReviewPageTextContentLocBy, true);
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(amount), "Verify Total Amount");
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains("Monthly"), "Verify Autopay Plan Type");
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains("*******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)), "Verify Loan Number");
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(paymentDate), "Verify Payment Start Date");
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(bankAccountName), "Verify Bank Account Name");
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(accountNumberWhileEdit), "Verify Bank Account Number");
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupReviewPageDisclosureText), "Verify Bank Account Name");
                webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.confirmAutopayButtonLocBy, "Confirm Autopay button.", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmAutopayButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.backToManageAutoPayButtonLocBy, isScrollIntoViewRequired: false);
                test.Log(Status.Info, $"<b><u>Confirmation Page Content Validation Started</u></b>");
                reportLogger.TakeScreenshot(test, "Autopay Setup Confirmation Page");
                string confirmationPageTextContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayConfirmationPageTextContentLocBy, true);
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(amount), "Verify Total Amount");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains("Monthly"), "Verify Autopay Plan Type");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains("*******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)), "Verify Loan Number");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(paymentDate), "Verify Payment Start Date");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(bankAccountName), "Verify Bank account Name");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(accountNumberWhileEdit), "Verify Bank Account Number");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupConfirmationPageDisclosureText), "Verify Autopay Confirmation Page Disclosure Text");
                reportLogger.TakeScreenshot(test, "Autopay Setup Confirmaion Page");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmAutopayButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.editAutopayConfirmationPageLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToManageAutoPayButtonLocBy, "Back To Manage AutoPay Button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                webElementExtensions.WaitForElement(_driver, payments.manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                test.Log(Status.Info, $"<b><u>Manage Autopay Page Content Validation Started</u></b>");
                webElementExtensions.VerifyElementText(_driver, payments.planForAutopayPaymentManageAutopayTextLocBy, "Monthly", "Autopay Payment Plan", true);
                webElementExtensions.VerifyElementText(_driver, payments.nextDebitDateManageAutoPayTextLocBy, paymentDate, isReportRequired: true);
                webElementExtensions.VerifyElementText(_driver, payments.amountOnManageAutoPayTextLocBy, amount, isReportRequired: true);

                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                bool flag = webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", "On Autopay");
                ReportingMethods.LogAssertionTrue(test, flag, flag ? $"Successfully validated that on the Dashboard Page Manage Autopay toggle is <b>turnned OFF</b>" : $"Failed - Manage Autopay toggle is <b>turnned ON</b>");
                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo");
                reportLogger.TakeScreenshot(test, "Dashboard Page");
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                string draftDate = DateTime.Parse(paymentDate).ToString("MM/dd/yyyy");
                string title = " loanDepot Authorized Recurring Automatic Payment - SM203 ";
                string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM203, "Monthly");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), "Verify the Title of SMC Message Template");
                ReportingMethods.LogAssertionTrue(test, recipientName.Contains(recipientName), "Verify the Recipient User Name of SMC Message Template");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains("Monthly"), "Verify the Payment Frequency in SMC Message Template");
                ReportingMethods.LogAssertionTrue(test, encryptedLoanNumber.Contains(encryptedLoanNumber), "Verify the encrypted loan number in SMC Message Template");
                ReportingMethods.LogAssertionTrue(test, amountForSMC.Contains(amountForSMC), "Verify the Total Amount in SMC Message Template");
                ReportingMethods.LogAssertionTrue(test, draftDate.Contains(draftDate), "Verify the Payment date in SMC Message Template");
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
        [Description("<br> TPR-294-Verify Pending Payment Section after Autopay Setup <br>" +
                          "TPR-1175-E2E_Setup_Edit_Autopay_Monthly_Prepaid <br>" +
                          "TPR-1112-Verify EM203/SM203 template content - Autopay Monthly Setup <br>" +
                          "TPR-1839-Verify user should see  scheduled payment is processing when autopay payment is pending <br>" +
                          "TPR-525-Verify Test Autopay Monthly/Bi-weekly - Do not add/delete payments on the same date & viceversa <br>" +
                          "TPR-589-Test CSS- View current AutoPay settings <br>" +
                          "TPR-546-Verify ON/OFF badge is displayed for loans enrolled for Monthly Autopay <br>" +
                          "TPR-588-Test CSS- Review and Pay Screen <br>" +
                          "TPR-585-Test CSS- Review and Pay Screen- Confirmation <br>" +
                          "TPR-591-Test CSS- Review and Pay Screen- Edit Payment <br>" +
                          "TPR-395-Verify ON/OFF badge is displayed for loans for which Autopay/Biweekly setup is processed <br>" +
                          "TPR-388-Verify customer should navigate to payment page when clicks on edit <br>" +
                          "TPR-474-Verify Autopay eligibility for loans with different loan types- Prepay")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_294_1175_1112_1839_525_589_546_588_585_591_395_388_474_TPR_VerifyTheAutoPaySetup_Edit_FunctionalityFor_MonthlyPrepaid()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {

                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleAutopayPrepaid} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleAutopayPrepaid, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b><u>Started Process to Setup Autopay Monthly plan</u></b>");
                webElementExtensions.WaitForElementToBeEnabled(_driver, dashboard.manageAutopayButtonLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                test.Log(Status.Info, $"<b><u>Started Process to Setup Autopay Monthly plan</u></b>");
                payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);
                string actualTotalAmountOnSetupAutoPayPage = webElementExtensions.GetElementText(_driver, payments.totalAutoPayAmountSetUpAutoPayLocBy);
                string amount = Regex.Match(actualTotalAmountOnSetupAutoPayPage, @"\$\d{1,3}(,\d{3})*(\.\d{2})?").Value;
                string actTotalAmount = webElementExtensions.GetElementText(_driver, payments.monthlyPaymentPlanAmountTextLocBy);
                webElementExtensions.VerifyElementText(_driver, payments.monthlyPaymentPlanAmountTextLocBy, amount, "Monthly Autopay payment plan Amount", isReportRequired: true);
                decimal principalInterest = Convert.ToDecimal(loanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount]);
                string interest = string.Format(new CultureInfo("en-US"), "{0:C}", principalInterest);
                decimal taxInDecimal = Convert.ToDecimal(loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount]);
                string tax = string.Format(new CultureInfo("en-US"), "{0:C}", taxInDecimal);
                string paymentDate = payments.SetupAutopay(pendingPaymentDates, loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerEmail].ToString(), amount, interest, tax, true, true);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToManageAutoPayButtonLocBy, "Back To Manage AutoPay Button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                webElementExtensions.WaitForElement(_driver, payments.manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                test.Log(Status.Info, $"<b><u>Manage Autopay Page Content Validation Started</u></b>");
                webElementExtensions.VerifyElementText(_driver, payments.planForAutopayPaymentManageAutopayTextLocBy, "Monthly", "Autopay Payment Plan", true);
                webElementExtensions.VerifyElementText(_driver, payments.nextDebitDateManageAutoPayTextLocBy, paymentDate, isReportRequired: true);
                webElementExtensions.VerifyElementText(_driver, payments.amountOnManageAutoPayTextLocBy, amount, isReportRequired: true);
                //Edit scenario starts from here
                test.Log(Status.Info, $"<b>***********************************<u>Edit scenario starts from here</u>***********************************</b>");
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.editIconManageAutopayLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.editIconManageAutopayLocBy, "Edit Button on Manage Autopay", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.editIconManageAutopayLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForStalenessOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, isScrollIntoViewRequired: false, timeoutInSeconds: ConfigSettings.WaitTime);
                bool isUpdateAutopayButtonDisabled = webElementExtensions.IsElementDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Autopay Page", isScrollIntoViewRequired: false);
                ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonDisabled, isUpdateAutopayButtonDisabled ? $"Successfully validated that on the Edit Autopay Page Update Autopay buttonis <b>Disabled</b>" : $"Failed - On Edit Autopay Page Update Autopay button is <b>Enabled</b>");
                reportLogger.TakeScreenshot(test, "Update Button Default View on Edit Autopay Page");
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true, type: "edit", isReportRequired: false);
                accountNumberWhileEdit = accountNumberWhileEdit.Substring(accountNumber.Length - 4);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, isScrollIntoViewRequired: true);
                bool isUpdateAutopayButtonEnabled = webElementExtensions.IsElementEnabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Autopay Page", isScrollIntoViewRequired: false);
                ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonEnabled, isUpdateAutopayButtonEnabled ? $"Successfully validated that on the Edit Autopay Page Update Autopay buttonis <b>Enabled</b>" : $"Failed - On Edit Autopay Page Update Autopay button is <b>Disabled</b>");
                reportLogger.TakeScreenshot(test, "Update Button View on Edit Autopay Page after adding Bank Account");
                webElementExtensions.ScrollToTop(_driver);
                string amountForSMC = amount;
                webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.additionalMonthlyPrincipalCheckBoxInputLocBy, timeoutInSeconds: ConfigSettings.SmallWaitTime, isScrollIntoViewRequired: false);
                bool isAdditionalMonthlyPrincipalCheckBoxEnabled = webElementExtensions.IsElementEnabled(_driver, commonServices.additionalMonthlyPrincipalCheckBoxInputLocBy, isScrollIntoViewRequired: false);
                if (isAdditionalMonthlyPrincipalCheckBoxEnabled)
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.additionalMonthlyPrincipalCheckBoxLabelLocBy);
                    bool isAdditionalMonthlyPaymentInputFieldIsEnabled = webElementExtensions.IsElementEnabled(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, "Additional Monthly Principal Input filed on Autopay Page");
                    ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonEnabled, isUpdateAutopayButtonEnabled ? $"Successfully validated that on the Edit Autopay Page Additional monthly payment input field is <b>Enabled</b>" : $"Failed - On Edit Autopay Page Additional monthly payment input field is <b>Disabled</b>");
                    webElementExtensions.EnterText(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, "100", isReportRequired: true, isClickRequired: true);
                    amount = (double.Parse(decimal.Parse(amount.Replace(",", "").TrimStart('$')).ToString()) + 1).ToString("C", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                }
                paymentDate = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, "edit", isScrollIntoViewRequired: false);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Edit Autopay", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.editAutopayReviewPageTextContentLocBy);
                webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.confirmAutopayButtonLocBy, isScrollIntoViewRequired: false);
                test.Log(Status.Info, $"<b><u>Review Page Content Validation Started</u></b>");
                reportLogger.TakeScreenshot(test, "Autopay Setup Review Page");
                string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.editAutopayReviewPageTextContentLocBy, true);
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(amount), reviewPageContent.Contains(amount) ? $"Successfully validated that Review page text content contains the expected Total Amount - <b>{amount}</b>" : $"Failed while verifying Total Amount - <b>{amount}</b>, please check the Review Page Text content in logs");
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains("Monthly"), reviewPageContent.Contains("Monthly") ? $"Successfully validated that Review page text content contains the expected Autopay plan Type - <b>Monthly</b>" : $"Failed while verifying Autopay Plan Type - <b>Monthly</b>, please check the Review Page Text content in logs");
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains("*******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)), reviewPageContent.Contains("*******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)) ? $"Successfully validated that Review page text content contains the expected Loan Number - <b>*******{loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)}</b>" : $"Failed while verifying Loan Number - <b>*******{loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)}</b>, please check the Review Page Text content in logs");
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(paymentDate), reviewPageContent.Contains(paymentDate) ? $"Successfully validated that Review page text content contains the expected Payment Start Date - <b>{paymentDate}</b>" : $"Failed while verifying Payment Start Date - <b>{paymentDate}</b>, please check the Review Page Text content in logs");
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(bankAccountName), reviewPageContent.Contains(bankAccountName) ? $"Successfully validated that Review page text content contains the expected Bank Account Name - <b>{bankAccountName}</b>" : $"Failed while verifying Bank Account Name - <b>{bankAccountName}</b>, please check the Review Page Text content in logs");
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(accountNumberWhileEdit), reviewPageContent.Contains(accountNumberWhileEdit) ? $"Successfully validated that Review page text content contains the expected Bank Account Number - <b>{accountNumberWhileEdit}</b>" : $"Failed while verifying Bank Account Number - </b>{accountNumberWhileEdit}</b>, please check the Review Page Text content in logs");
                ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupReviewPageDisclosureText), reviewPageContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupReviewPageDisclosureText) ? $"Successfully validated that Review page text content contains the expected Disclosure Text - <b>{Constants.CustomerPortalTextMessages.AutopaySetupReviewPageDisclosureText}</b>" : $"Failed while verifying Bank Account Name - <b>{Constants.CustomerPortalTextMessages.AutopaySetupReviewPageDisclosureText}</b>, please check the Review Page Text content in logs");
                webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.confirmAutopayButtonLocBy, "Confirm Autopay button.", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmAutopayButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.backToManageAutoPayButtonLocBy, isScrollIntoViewRequired: false);
                test.Log(Status.Info, $"<b><u>Confirmation Page Content Validation Started</u></b>");
                reportLogger.TakeScreenshot(test, "Autopay Setup Confirmation Page");
                string confirmationPageTextContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayConfirmationPageTextContentLocBy, true);
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(amount), confirmationPageTextContent.Equals(amount) ? $"Successfully validated that Autopay Setup Confirmation Page contains Total Amount - <b>{amount}</b>" : $"Failed while verifying Total Amount on Autopay Setup Confirmation Page - <b>{amount}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains("Monthly"), confirmationPageTextContent.Contains("Monthly") ? $"Successfully validated that Review page text content contains the expected Autopay plan Type - <b>Monthly</b>" : $"Successfully validated that Review page text content contains the expected Autopay plan Type - <b>Monthly</b>");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains("*******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)), confirmationPageTextContent.Contains("*******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)) ? $"Successfully validated that Autopay Setup Confirmation Page text content contains the expected Loan Number - <b>*******{loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)}</b>" : $"Failed while verifying Loan Number - <b>*******{loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)}</b>, please check the Autopay Setup Confirmation Page Text content in logs");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(paymentDate), confirmationPageTextContent.Contains(paymentDate) ? $"Successfully validated that Autopay Setup Autopay Confirmation page text content contains the expected Payment Start Date - <b>{paymentDate}</b>" : $"Failed while verifying Payment Start Date - <b>{paymentDate}</b>, please check the Autopay Setup Confirmation Page Text content in logs");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(bankAccountName), confirmationPageTextContent.Contains(bankAccountName) ? $"Successfully validated that Setup Autopay Confirmation  page text content contains the expected Bank Account Name - <b>{bankAccountName}</b>" : $"Failed while verifying Bank Account Name - <b>{bankAccountName}</b>, please check the Autopay Setup Autopay Confirmation Page Text content in logs");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(accountNumberWhileEdit), confirmationPageTextContent.Contains(accountNumberWhileEdit) ? $"Successfully validated that Autopay Setup Autopay Confirmation Page Text content contains the expected Bank Account Number - <b>{accountNumberWhileEdit}</b>" : $"Failed while verifying Bank Account Number - <b>{accountNumberWhileEdit}</b>, please check the Autopay Setup Autopay Confirmation Page Text content in logs");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupConfirmationPageDisclosureText), confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupConfirmationPageDisclosureText) ? $"Successfully validated that Autopay Setup Autopay Confirmation Page Text content contains the expected Disclosure Text - <b>{Constants.CustomerPortalTextMessages.AutopaySetupConfirmationPageDisclosureText}</b>" : $"Failed while verifying Bank Account Name - <b>{Constants.CustomerPortalTextMessages.AutopaySetupReviewPageDisclosureText}</b>, please check the Autopay Setup Autopay Confirmation Page Text content in logs");
                reportLogger.TakeScreenshot(test, "Autopay Setup Confirmaion Page");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmAutopayButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.editAutopayConfirmationPageLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToManageAutoPayButtonLocBy, "Back To Manage AutoPay Button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                webElementExtensions.WaitForElement(_driver, payments.manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                test.Log(Status.Info, $"<b><u>Manage Autopay Page Content Validation Started</u></b>");
                webElementExtensions.VerifyElementText(_driver, payments.planForAutopayPaymentManageAutopayTextLocBy, "Monthly", "Autopay Payment Plan", true);
                webElementExtensions.VerifyElementText(_driver, payments.nextDebitDateManageAutoPayTextLocBy, paymentDate, isReportRequired: true);
                webElementExtensions.VerifyElementText(_driver, payments.amountOnManageAutoPayTextLocBy, amount, isReportRequired: true);

                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                bool flag = webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", "On Autopay");
                ReportingMethods.LogAssertionTrue(test, flag, flag ? $"Successfully validated that on the Dashboard Page Manage Autopay toggle is <b>turnned OFF</b>" : $"Failed - Manage Autopay toggle is <b>turnned ON</b>");


                reportLogger.TakeScreenshot(test, "Dashboard Page");
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                string draftDate = DateTime.Parse(paymentDate).ToString("MM/dd/yyyy");
                string title = " loanDepot Authorized Recurring Automatic Payment - SM203 ";
                string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM203, "Monthly");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains("Monthly"), messageTemplateContent.Contains("Monthly") ? $"Successfully validated the Payment Frequency in SMC Message Template - <b>{"Monthly"}</b>" : $"Failed while verifying the payment Frequency In SMC Message Template - <b>{"Monthly"}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amountForSMC), messageTemplateContent.Contains(amountForSMC) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amountForSMC}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amountForSMC}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
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
        [Description("<br>TPR-526-Biweekly - Verify if user is able to Setup Autopay and try Deleting Autopay the very next day<br>" +
                         "TPR-2218-Verify EM256/SM256 template content - Autopay Biweekly Cancel")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_526_2218_TPR_VerifyTheAutoPayDeleteBiWeeklyFunctionality()
        {
            string setupType = "BiWeekly";
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                loanDetailsForDeleteBiWeeklyAutopay = loanDetailsForDeleteBiWeeklyAutopay.Replace("SETUP_TYPE", setupType);

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForDeleteBiWeeklyAutopay} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForDeleteBiWeeklyAutopay, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        if (dashboard.IsLoanHasAutopayToggleON())
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
                        if (dashboard.IsLoanHasAutopayToggleON())
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
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b><u>Started Process to Delete Autopay plan</u></b>");
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.ScrollIntoView(_driver, payments.backToManageAutoPayButtonLocBy);
                if (webElementExtensions.IsElementEnabled(_driver, payments.autopayDeleteLocBy))
                {
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteLocBy);
                    reportLogger.TakeScreenshot(test, "Confirm Autopay Delete");
                    string actPopUpMsg = webElementExtensions.GetElementText(_driver, payments.autopaySetupOneBusinessdayMsgTextLocBy);
                    string expectedMsg = "The autopay plan deletion may take up to 1 business day to process and cannot be reactivated on the same day the plan was deleted. You may return on the following business day to reactivate your payment plan. Click the Confirm button if you wish to continue deleting this plan.";

                    ReportingMethods.LogAssertionEqual(test, expectedMsg, actPopUpMsg, "Verify Businessdays Message!");
                    webElementExtensions.ClickElement(_driver, commonServices.continueButtonInConfirmAutopayScreenLocBy, "Continue Button on Delete Autopay number of business days pop up", isReportRequired: true);
                    webElementExtensions.ClickElement(_driver, payments.autopayDeleteReasonLocBy);
                    webElementExtensions.ClickElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitUntilElementIsClickable(_driver, payments.backToManageAutopayButtonFromDeleteLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.autopayDeletedMsgLocBy);
                    reportLogger.TakeScreenshot(test, "Deleted Autopay");
                    webElementExtensions.ClickElement(_driver, payments.backToManageAutopayButtonFromDeleteLocBy);

                    webElementExtensions.VerifyElementText(_driver, payments.autopayNoTallowedMsgLocBy, "Autopay not allowed at this time.", isReportRequired: true);
                    webElementExtensions.ClickElement(_driver, payments.backToAccountSummaryLinkLocBy);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");

                    bool autopayStatus = dashboard.IsLoanHasAutopayToggleON();
                    ReportingMethods.LogAssertionFalse(test, autopayStatus, "Autopay Flag should be OFF after delete");

                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                    string title = " loanDepot Cancellation Notice of Recurring Automatic Payment Plan - SM256 ";
                    string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM256, "Monthly");
                    string userName = "autotesting autotesting(you)";
                    string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(userName), messageTemplateContent.Contains(username) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{username}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{username}</b>, please check the logs");
                    ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");

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
        [Description("<br>TPR-1141-Verify delete exisitng Autopay Setup for Monthly <br>" +
                         "TPR-1879-Verify EM256/SM256 template content - Autopay Monthly Cancel <br>" +
                         "TPR-292-Verify Payment Activity Section after Autopay has been Deleted <br>" +
                         "TPR-308-Verify Payment Activity Section after Autopay has Cancelled")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_1141_1879_292_308_TPR_VerifyTheAutoPayDeleteMonthlyFunctionality()
        {
            string setupType = "Monthly";
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                loanDetailsForDeleteAutopay = loanDetailsForDeleteAutopay.Replace("SETUP_TYPE", setupType);

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForDeleteAutopay} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForDeleteAutopay, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                DateTime next_debit_date;
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
                        if (dashboard.IsLoanHasAutopayToggleON())
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
                        if (dashboard.IsLoanHasAutopayToggleON())
                        {
                            break;
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

                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.ScrollIntoView(_driver, payments.backToManageAutoPayButtonLocBy);
                next_debit_date = DateTime.Parse(webElementExtensions.GetElementText(_driver, commonServices.nextDebitDateManageAutoPayTextLocBy),
                CultureInfo.InvariantCulture);
                if ((next_debit_date - DateTime.Now).Days >= 2)
                {
                    webElementExtensions.ActionClick(_driver, payments.autopayDeleteLocBy);
                    test.Log(Status.Info, $"<b><u>Started Process to Delete Autopay plan</u></b>");
                    reportLogger.TakeScreenshot(test, "Confirm Autopay Delete");
                    string actPopUpMsg = webElementExtensions.GetElementText(_driver, payments.autopaySetupOneBusinessdayMsgTextLocBy);
                    if (commonServices.GetBankHolidays().Contains(DateTime.Today) || DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                        ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.AutopayDelete2BusinessdayPopupMsg, actPopUpMsg, "Verify Businessdays Message!");
                    else
                        ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.AutopayDelete1BusinessdayPopupMsg, actPopUpMsg, "Verify Businessdays Message!");
                    webElementExtensions.ClickElement(_driver, commonServices.continueButtonInConfirmAutopayScreenLocBy, "Continue Button on Delete Autopay number of business days pop up", isReportRequired: true);
                    webElementExtensions.ClickElement(_driver, payments.autopayDeleteReasonLocBy);
                    webElementExtensions.ClickElement(_driver, payments.autopayDeleteConfirmLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitUntilElementIsClickable(_driver, payments.backToManageAutopayButtonFromDeleteLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.autopayDeletedMsgLocBy);
                    reportLogger.TakeScreenshot(test, "Deleted Autopay");
                    webElementExtensions.ClickElement(_driver, payments.backToManageAutopayButtonFromDeleteLocBy);
                    webElementExtensions.VerifyElementText(_driver, payments.autopayNoTallowedMsgLocBy, Constants.CustomerPortalTextMessages.AutopayAfterDeleteAllowUs1BusinessDayForProcessingMsg, isReportRequired: true);
                    webElementExtensions.ClickElement(_driver, payments.backToAccountSummaryLinkLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    bool autopayStatus = dashboard.IsLoanHasAutopayToggleON();
                    ReportingMethods.LogAssertionFalse(test, autopayStatus, "Autopay Flag should be OFF after delete");
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                }
                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy);
                reportLogger.TakeScreenshot(test, "Dashboard Page");

                string emailContent = yopmailPage.GetEmailContentFromYopmail(username, emailNotificationType: Constants.EmailNotificationFromYopmail.AutopayDelete);
                ReportingMethods.LogAssertionContains(test, Constants.EmailContents.AutopayDeleteTitle, emailContent, "Verify Autopay Delete Email Notification Title");
                ReportingMethods.LogAssertionContains(test, Constants.EmailContents.AutopayDeleteEmailContentBody, emailContent, "Verify Autopay Delete Email Notification Body Content");
                ReportingMethods.LogAssertionContains(test, Constants.EmailContents.SenderEmailId, emailContent, "Verify Autopay Delete Email Notification Recipient");


                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                string title = " loanDepot Cancellation Notice of Recurring Automatic Payment Plan - SM256 ";
                string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM256, "Monthly");
                string userName = "autotesting autotesting(you)";
                string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(userName), messageTemplateContent.Contains(username) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{username}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{username}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
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
        [Description("<br>TPR-1128-Verify Editing existing Bi-Weekly Autopay Setup <br> " +
                         "TPR-392-Verify ON/OFF badge is displayed for loans for which Autopay/Biweekly setup is processed <br>" +
                         "TPR-2257-[BUG#CUSTP-5425] - Update Autopay button is not enabled on edit with Bank account <br>" +
                         "TPR-436-Verify display Pending Autopay in Pending Payments When Status is 'Processing'  for bi-weekly payments <br>" +
                         "TPR-529-Test Autopay Monthly/Bi-weekly - Do not add/delete payments on the same date & viceversa <br>" +
                         "TPR-1108-Verify EM257/SM257 template content - Autopay Biweekly Setup")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_1128_392_2257_436_529_1108_TPR_VerifySetupAndEditBiWeeklyAutoPaySetup()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForBiWeekly} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForBiWeekly, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        int cout = loanLevelData.Count;
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
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);

                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);


                payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);

                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.biWeeklyRadioLocBy);
                webElementExtensions.ClickElement(_driver, payments.biWeeklyRadioLocBy, "Bi-Weekly Radio Button", isReportRequired: true);

                string amount_payble = webElementExtensions.GetElementText(_driver, payments.autopayAmountBiWeeklyAmountAtTop);
                webElementExtensions.VerifyElementText(_driver, payments.totalAutoPayAmountSetUpAutoPayLocBy, amount_payble, isReportRequired: true, checkContains: true);

                string interest = Regex.Match(loanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount].ToString(), @"\$\d{1,3}(,\d{3})*(\.\d{2})?").Value;
                string tax = Regex.Match(loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString(), @"\$\d{1,3}(,\d{3})*(\.\d{2})?").Value;
                string selected_loan = "******" + dashboard.GetLastFourDigitsFromLoanSelected();

                string paymentDate =
                    payments.SetupAutopay(pendingPaymentDates, selected_loan, username, amount_payble, interest, tax, true, true, autopayType: "Bi-Weekly", opt_out_code: loanLevelData[retryCount][Constants.LoanLevelDataColumns.OptOutStopCode].ToString());

                webElementExtensions.ScrollIntoView(_driver, payments.backToManageAutoPayButtonLocBy);
                webElementExtensions.ActionClick(_driver, payments.backToManageAutoPayButtonLocBy, "Back To Manage AutoPay Button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                webElementExtensions.WaitForElement(_driver, payments.manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);

                webElementExtensions.VerifyElementText(_driver, payments.nextDebitDateManageAutoPayTextLocBy, paymentDate, isReportRequired: true);
                webElementExtensions.VerifyElementText(_driver, payments.amountOnManageAutoPayTextLocBy, amount_payble, isReportRequired: true);

                //Edit the Autopay Biweekly setup done in previous steps
                webElementExtensions.ActionClick(_driver, payments.autopayEditLocBy, webElementName: "Edit Autopay", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToManageAutopayLinkLocBy);

                //verify payment and princiapl payment fields are disabled
                webElementExtensions.IsElementDisabled(_driver, payments.paymentDateLocBy, "Payment Date");
                webElementExtensions.IsElementDisabled(_driver, payments.addnlPrincipalAmountLocBy, "Addnl Principal Amount");
                webElementExtensions.IsElementDisabled(_driver, payments.updateAutopayLocBy, "Update Autopay");
                //Add new bank account
                string bankAccountNameAfterSelection = string.Empty, paymentDateToBeSelected = string.Empty;
                webElementExtensions.ScrollToTop(_driver);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber: routingNumber, accountNumberWhileEdit, isReportRequired: false, isScrollIntoViewRequired: false, type: "edit");
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.updateAutopayLocBy);
                webElementExtensions.ActionClick(_driver, payments.updateAutopayLocBy);

                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                string reviewPageContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayReviewPageTextContentLocBy, true);

                List<string> editAutopayKeywords = new List<string>() {
                    "Bi-Weekly",
                    amount_payble,
                    selected_loan,
                    paymentDate,
                    bankAccountName,
                    bankAccountName.Substring(bankAccountName.Length - 4)

                };

                reportLogger.TakeScreenshot(test, "Edit Autopay Setup Review Page");

                foreach (string keyword in editAutopayKeywords)
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Review page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");

                webElementExtensions.ScrollIntoView(_driver, commonServices.confirmAutopayButtonLocBy);
                webElementExtensions.ClickElement(_driver, commonServices.confirmAutopayButtonLocBy, "Confirm Autopay button.", true);
                webElementExtensions.WaitForElement(_driver, commonServices.continueButtonInConfirmAutopayScreenLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmAutopayButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                string confirmEditAutopayContent = webElementExtensions.GetElementText(_driver, payments.setupAutopayConfirmationPageTextContentLocBy);

                foreach (string keyword in editAutopayKeywords)
                    ReportingMethods.LogAssertionTrue(test, confirmEditAutopayContent.Contains(keyword), reviewPageContent.Contains(keyword) ? $"Successfully validated that Confirmation page text content contains the {keyword}</b>" : $"Failed while verifying {keyword} check the Review Page Text content in logs");


                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy);
                reportLogger.TakeScreenshot(test, "Dashboard Page");
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                string draftDate = DateTime.Parse(paymentDate).ToString("MM/dd/yyyy");
                string title = " loanDepot Authorized Automatic Recurring Payment - SM257 ";
                string recipientName = "autotesting autotesting(you)";
                string encryptedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM257, "bi-weekly");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains("biweekly"), messageTemplateContent.Contains("biweekly") ? $"Successfully validated the Payment Frequency in SMC Message Template - <b>{"biweekly"}</b>" : $"Failed while verifying the payment Frequency In SMC Message Template - <b>{"biweekly"}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encryptedLoanNumber), messageTemplateContent.Contains(encryptedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encryptedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encryptedLoanNumber}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(amount_payble), messageTemplateContent.Contains(amount_payble) ? $"Successfully valdated the Total Amount in SMC Message Template - <b>{amount_payble}</b>" : $"Failed while verifying the Total Amount In SMC Message Template - <b>{amount_payble}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(draftDate), messageTemplateContent.Contains(draftDate) ? $"Successfully validated the Payment date in SMC Message Template - <b>{draftDate}</b>" : $"Failed while verifying the payment date In SMC Message Template - <b>{draftDate}</b>, please check the logs");
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
        [Description("<br>TPR - 1173 - Verify Dates for Setting Autopay Monthly Payment for Past Due")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_1173_TPR_VerifyDatesWhileSettingAutoPayMonthlyPaymentForPastDue()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {

                #region TestData                               

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleAutopayPastDue} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleAutopayPastDue, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                string loanStatus = Constants.LoanStatus.PastDue;
                exptDates = commonServices.GetExpectedAvailableDates(loanStatus, Constants.AutopayPaymentFrequency.Monthly);
                isEligible = (exptDates == null) ? false : true;
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, isEligible))
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, isEligible))
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
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                if (!isEligible)
                {
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg, webElementExtensions.GetElementText(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy), "Verification of Payment Not Allowed Message on Manage Autopay Page");
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, payments.setupAutopaybuttonLocBy, isScrollIntoViewRequired: false), "Verification for Setup Autopay Button to be disabled");
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementEnabled(_driver, payments.sendASecureMessageButtonLocBy, isScrollIntoViewRequired: false), "Verification for Send a secure Message Button to be Enabled");
                    reportLogger.TakeScreenshot(test, "Autopay Not Allowed Error Message");
                }
                else
                {
                    payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);
                    var actPaymentDatesToEnabled = commonServices.GetAvailablePaymentDateInDateField();
                    ReportingMethods.LogAssertionTrue(test, exptDates.SequenceEqual(actPaymentDatesToEnabled), $"Verification of Actual: <b>{string.Join(", ", actPaymentDatesToEnabled)}</b> and expected Enabled Dates: <b>{string.Join(", ", exptDates)}</b> For PastDue Loan Status");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo", isReportRequired: true);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.WaitUntilElementIsClickable(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.ActionClick(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button", isReportRequired: true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);
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
        [Description("<brtpr-1171-Verify Dates for Setting Autopay Monthly Payment for Ontime")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TRP_1171_TRP_VerifyDatesWhileSettingAutoPayMonthlyPaymentForOntime()
        {
            int retryCount = 0;
            bool isEligible = false;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.Ontime;
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
                exptDates = commonServices.GetExpectedAvailableDates(loanStatus, Constants.AutopayPaymentFrequency.Monthly);
                isEligible = (exptDates == null) ? false : true;
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, isEligible))
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, isEligible))
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
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                if (!isEligible)
                {
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg, webElementExtensions.GetElementText(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy), "Verification of Payment Not Allowed Message on Manage Autopay Page");
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, payments.setupAutopaybuttonLocBy, isScrollIntoViewRequired: false), "Verification for Setup Autopay Button to be disabled");
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementEnabled(_driver, payments.sendASecureMessageButtonLocBy, isScrollIntoViewRequired: false), "Verification for Send a secure Message Button to be Enabled");
                    reportLogger.TakeScreenshot(test, "Autopay Not Allowed Error Message");
                }
                else
                {
                    payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);
                    var actPaymentDatesToEnabled = commonServices.GetAvailablePaymentDateInDateField();
                    ReportingMethods.LogAssertionTrue(test, exptDates.SequenceEqual(actPaymentDatesToEnabled), $"Verification of Actual: <b>{string.Join(", ", actPaymentDatesToEnabled)}</b> and expected Enabled Dates: <b>{string.Join(", ", exptDates)}</b> For Ontime Loan Status");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo", isReportRequired: true);
                    webElementExtensions.WaitForElementToBeEnabled(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button");
                    webElementExtensions.WaitUntilElementIsClickable(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.ActionClick(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button", isReportRequired: true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);
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
        [Description("<br>TPR-1170 - Verify Dates for Setting Autopay Monthly Payment for Prepaid")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TRP_1170_TPR_VerifyDatesWhileSettingAutoPayMonthlyPaymentForPrepaid()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.PrepaidOneMonth;
                #region TestData                                
                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleAutopayPrepaid} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleAutopayPrepaid, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                exptDates = commonServices.GetExpectedAvailableDates(loanStatus);
                isEligible = (exptDates == null) ? false : true;
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, isEligible))
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, isEligible))
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
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                if (!isEligible)
                {
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg, webElementExtensions.GetElementText(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy), "Verification of Payment Not Allowed Message on Manage Autopay Page");
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, payments.setupAutopaybuttonLocBy, isScrollIntoViewRequired: false), "Verification for Setup Autopay Button to be disabled");
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementEnabled(_driver, payments.sendASecureMessageButtonLocBy, isScrollIntoViewRequired: false), "Verification for Send a secure Message Button to be Enabled");
                    reportLogger.TakeScreenshot(test, "Autopay Not Allowed Error Message");
                }
                else
                {
                    payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);
                    var actPaymentDatesToEnabled = commonServices.GetAvailablePaymentDateInDateField();
                    ReportingMethods.LogAssertionTrue(test, exptDates.SequenceEqual(actPaymentDatesToEnabled), $"Verification of Actual: <b>{string.Join(", ", actPaymentDatesToEnabled)}</b> and expected Enabled Dates: <b>{string.Join(", ", exptDates)}</b> For Prepaid Loan Status");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo");
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo", isReportRequired: true);
                    webElementExtensions.WaitUntilElementIsClickable(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.ScrollToTop(_driver);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button", isReportRequired: true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);
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
        [Description("<br>TPR-1072-Verify Dates for Setting Autopay Bi-Weekly Payment for On Time")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_1072_TPR_VerifyDatesForSettingAutopayBiWeeklyPaymentForOnTime()
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
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForBiWeeklyOntime} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForBiWeeklyOntime, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                exptDates = commonServices.GetExpectedAvailableDates(loanStatus, Constants.AutopayPaymentFrequency.Biweekly);
                isEligible = (exptDates == null) ? false : true;
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, isEligible))
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, isEligible))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);
                if (!isEligible)
                {
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, payments.biWeeklyRadioLocBy, isScrollIntoViewRequired: false), "Verification for Biweekly Radio Button to be disabled");
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.AutopaySetupBiweeklyNotAllowedWarningForOntimeText.Trim(), webElementExtensions.GetElementText(_driver, By.XPath(string.Format(commonServices.divLocatorWithSpecificText, Constants.CustomerPortalTextMessages.AutopaySetupBiweeklyNotAllowedWarningForOntimeText))), "Verification of Payment Not Allowed Message on Manage Autopay Page");
                    reportLogger.TakeScreenshot(test, "Autopay Biweekly Not Allowed Warning Message");
                }
                else
                {
                    webElementExtensions.WaitUntilElementIsClickable(_driver, payments.biWeeklyRadioLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.ClickElement(_driver, payments.biWeeklyRadioLocBy, "Biweekly Radio Button", true);
                    var actPaymentDatesToEnabled = commonServices.GetAvailablePaymentDateInDateField();
                    webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo");
                    ReportingMethods.LogAssertionTrue(test, exptDates.SequenceEqual(actPaymentDatesToEnabled), $"Verification of Actual: <b>{string.Join(", ", actPaymentDatesToEnabled)}</b> and expected Enabled Dates: <b>{string.Join(", ", exptDates)}</b> For PastDue Loan Status");
                }
                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.ActionClick(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);
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
        [Description("<br>TPR-1071-Verify Dates for Setting Autopay Biweekly Payment for One Month Prepaid")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_1071_TPR_VerifyDatesForSettingAutopayBiweeklyPaymentForOneMonthPrepaid()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.PrepaidOneMonth;
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForBiWeeklyOneMonthPrepaid} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForBiWeeklyOneMonthPrepaid, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                exptDates = commonServices.GetExpectedAvailableDates(loanStatus, Constants.AutopayPaymentFrequency.Biweekly);
                isEligible = (exptDates == null) ? false : true;
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, isEligible))
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, isEligible))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                    }
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);
                if (!isEligible)
                {
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, payments.biWeeklyRadioLocBy, isScrollIntoViewRequired: false), "Verification for Biweekly Radio Button to be disabled");
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.AutopaySetupBiweeklyNotAllowedWarningForOntimeText.Trim(), webElementExtensions.GetElementText(_driver, By.XPath(string.Format(commonServices.divLocatorWithSpecificText, Constants.CustomerPortalTextMessages.AutopaySetupBiweeklyNotAllowedWarningForOntimeText))), "Verification of Payment Not Allowed Message on Manage Autopay Page");
                    reportLogger.TakeScreenshot(test, "Autopay Biweekly Not Allowed Warning Message");
                }
                else
                {
                    webElementExtensions.WaitUntilElementIsClickable(_driver, payments.biWeeklyRadioLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.ClickElement(_driver, payments.biWeeklyRadioLocBy, "Biweekly Radio Button", true);
                    var actPaymentDatesToEnabled = commonServices.GetAvailablePaymentDateInDateField(selectNextMonth: true);// Due to a known issue we have to click on the next month right now
                    ReportingMethods.LogAssertionTrue(test, exptDates.SequenceEqual(actPaymentDatesToEnabled), $"Verification of Actual: <b>{string.Join(", ", actPaymentDatesToEnabled)}</b> and expected Enabled Dates: <b>{string.Join(", ", exptDates)}</b> For PastDue Loan Status");
                }
                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo");
                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.ActionClick(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);

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
        [Description("<br>TPR-1068-Verify Dates for Setting Autopay Biweekly Payment for Two Months Prepaid")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_1068_TPR_VerifyDatesForSettingAutopayBiweeklyPaymentForTwoMonthsPrepaid()
        {
            int retryCount = 0;
            bool isEligible = true;
            List<string> exptDates = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                string loanStatus = Constants.LoanStatus.PrepaidTwoMonth;
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForBiWeeklyTwoMonthPrepaid} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForBiWeeklyTwoMonthPrepaid, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                exptDates = commonServices.GetExpectedAvailableDates(loanStatus, Constants.AutopayPaymentFrequency.Biweekly);
                isEligible = (exptDates == null) ? false : true;
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, isEligible))
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, isEligible))
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
                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo");
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalErrorMsgs.AutopayNotAllowedOnAutopayPageErrorMsg, webElementExtensions.GetElementText(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy), "Verification of Payment Not Allowed Message on Manage Autopay Page");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, payments.setupAutopaybuttonLocBy, isScrollIntoViewRequired: false), "Verification for Setup Autopay Button to be disabled");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementEnabled(_driver, payments.sendASecureMessageButtonLocBy, isScrollIntoViewRequired: false), "Verification for Send a secure Message Button to be Enabled");
                reportLogger.TakeScreenshot(test, "Autopay Biweekly Not Allowed Warning Message");
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
        [Description("<br>CUSTP-2537-Verify customers to make autopay for loans that have InvesterId W01")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_1967_TPR_VerifyMonthlyAutopayInEligibilityForLoansWithInvesterIdW01()
        {
            int loansFound = 0;
            int retryCount = 0;

            List<Hashtable> loanLevelData = new List<Hashtable>();

            try
            {

                #region TestData
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleAutopayOntimeW01} </b></font>");
                List<string> usedLoanTestData = new List<string>();
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(field => field.IsLiteral && !field.IsInitOnly)
                .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleAutopayOntimeW01, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Info($"No Loans found with given SQL query!");
                    // continue;
                }
                else
                {
                    loanLevelData.RemoveAll(loan => loan[Constants.LoanLevelDataColumns.LoanNumber].ToString().StartsWith("9"));
                    if (loanLevelData == null || loanLevelData.Count == 0)
                    {
                        test.Info($"No First Mortgage Loans Found!");
                        //    continue;
                    }
                    else
                    {
                        loansFound = loanLevelData.Count;
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        string expectedAutopayStatus = null;//draftingIndicators[iteration].Equals("Y") ? "On Autopay" : "Off Autopay";
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
                            if (!_driver.Url.Contains("/link-user-loan"))
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
                    }
                }
                reportLogger.TakeScreenshot(test, "Manage Autopay");
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitUntilUrlContains("autopay");
                webElementExtensions.WaitForElement(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy);
                reportLogger.TakeScreenshot(test, "Autopay Not Allowed");
                var msg = webElementExtensions.GetElementText(_driver, payments.paymentsNotAllowedAutopayPageMsgTextLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.AutopayNotEligibleForW01, msg, $"{msg} should contain '{Constants.CustomerPortalErrorMsgs.AutopayNotEligibleForW01}'");


            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Error while executing {testContext.TestName} test method. Error: {e.Message}.");
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
        [Description("<br>TPR-616-Verify OTP should not be allowed for if Autopay is scheduled : Pastdue <br>")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP")]
        public void TPR_616_TPR_VerifyOTPShouldNotBeAllowedForActiveAutopayScheduledForPastDue()
        {
            string setupType = "Monthly";
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                loanDetailsForDeleteAutopay = loanDetailsForDeleteAutopayForPastDue.Replace("SETUP_TYPE", setupType);

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForDeleteAutopay} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForDeleteAutopay, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        if (dashboard.IsLoanHasAutopayToggleON())
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
                        if (dashboard.IsLoanHasAutopayToggleON())
                        {
                            break;
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

                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                webElementExtensions.ScrollToTop(_driver);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                commonServices.ClickConfirmButtonPaymentReviewPage();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.unableToProcessPaymentWarningPopUpLocBy, ConfigSettings.WaitTime);
                webElementExtensions.VerifyElementText(_driver, payments.unableToProcessPaymentWarningPopUpLocBy, Constants.CustomerPortalErrorMsgs.UnableToProceedPaymentWarningMsg, "Unable to process payment Popup", true);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.unableToProcessPaymentWarningPopUpCloseButtonLocBy, "Close button on Unable to proceed payment Popup", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
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
        [Description("<br>TPR-1089-Verify OTP should not be allowed for if Autopay is scheduled : Ontime <br>")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP")]
        public void TPR_1089_TPR_VerifyOTPShouldNotBeAllowedForActiveAutopayScheduledForOntime()
        {
            string setupType = "Monthly";
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                loanDetailsForDeleteAutopay = loanDetailsForDeleteAutopayForOntime.Replace("SETUP_TYPE", setupType);

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForDeleteAutopay} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForDeleteAutopay, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        if (dashboard.IsLoanHasAutopayToggleON())
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
                        if (dashboard.IsLoanHasAutopayToggleON())
                        {
                            break;
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

                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                payments.AcceptScheduledPaymentIsProcessingPopUp();
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
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                commonServices.ClickConfirmButtonPaymentReviewPage();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.unableToProcessPaymentWarningPopUpLocBy, ConfigSettings.WaitTime);
                webElementExtensions.VerifyElementText(_driver, payments.unableToProcessPaymentWarningPopUpLocBy, Constants.CustomerPortalErrorMsgs.UnableToProceedPaymentWarningForFMOntimeMsg, "Unable to process payment Popup", true);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.unableToProcessPaymentWarningPopUpCloseButtonLocBy, "Close button on Unable to proceed payment Popup", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
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
        [Description("<br>TPR-609-Verify OTP should not be allowed for if Autopay is scheduled : Prepaid One Month <br>")]
        [TestCategory("CP_Regression"), TestCategory("CP_OTP")]
        public void TPR_609_TPR_VerifyOTPShouldNotBeAllowedForActiveAutopayScheduledForPrepaidOneMonth()
        {
            string setupType = "Monthly";
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                loanDetailsForDeleteAutopay = loanDetailsForDeleteAutopayForPrepaidOneMonth.Replace("SETUP_TYPE", setupType);

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForDeleteAutopay} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForDeleteAutopay, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        if (dashboard.IsLoanHasAutopayToggleON())
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
                        if (dashboard.IsLoanHasAutopayToggleON())
                        {
                            break;
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

                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
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
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                commonServices.ClickConfirmButtonPaymentReviewPage();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.unableToProcessPaymentWarningPopUpLocBy, ConfigSettings.WaitTime);
                webElementExtensions.VerifyElementText(_driver, payments.unableToProcessPaymentWarningPopUpLocBy, Constants.CustomerPortalErrorMsgs.UnableToProceedPaymentWarningForFMOntimeMsg, "Unable to process payment Popup", true);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.unableToProcessPaymentWarningPopUpCloseButtonLocBy, "Close button on Unable to proceed payment Popup", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
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
        [Description("TPR-2690-Verify Edits/Deletes to autopay not allowed | Draft date is 1st thru 15th and its a weekend or holiday | Todayâ€™s date = draft date | Before Cutoff Time \n"+
            "TPR_2688 Test-FM Autopay-Verify Edit and Delete buttons are enabled | Draft date is 1st thru 13th (13th is not a Fri) | Today is a business day And Todayâ€™s date = draft date or Todayâ€™s date != draft date | Before Cutoff Time \n")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_2690_2688_TPR_VerifyTheFMAutoPaySetup_EditDelete_If_DraftDateIsToday()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForExisitngAutopayFM} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                requiredColumns.Add("PaymentDate");
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForExisitngAutopayFM, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        if (dashboard.IsLoanHasAutopayToggleON())
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
                        if (dashboard.IsLoanHasAutopayToggleON())
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
                var paymentDateFromDb = loanLevelData[retryCount][Constants.PaymentSetupDataColumns.PaymentDate].ToString();
                var paymentDate = DateTime.ParseExact(paymentDateFromDb, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                var holidays = commonServices.GetBankHolidays(paymentDate.Year.ToString());
                var currentCstTime = UtilAdditions.GetCSTTimeNow();
                if (holidays.Contains(paymentDate) 
                    || paymentDate.DayOfWeek == DayOfWeek.Saturday 
                    || paymentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    test.Log(Status.Warning, $"{loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString()} has autopay setup on Weekend/Holiday : {paymentDate.Date}, Edit/Delete is not allowed for this loan today");
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.manageAutopayButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                    reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                    return;
                }

                if (currentCstTime.Hour >= 20)
                {
                    test.Log(Status.Info, $"Current CST time is {currentCstTime}, which is after cutoff time 8 PM CST, hence Edit/Delete Autopay setup not allowed");
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.manageAutopayButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                    reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                    return;
                }
                else
                {
                    test.Log(Status.Info, $"Current CST time is {currentCstTime}, which is before cutoff time 8 PM CST, Expecting Edit/Delete should be allowed");
                    var formattedPaymentDate = paymentDate.ToString("MMM d, yyyy");
                    bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                    int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                    List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                    ReportingMethods.LogAssertionTrue(test, pendingPaymentDates.Contains(formattedPaymentDate), $"Check Pending Payment with Payment Date {formattedPaymentDate}");
                    reportLogger.TakeScreenshot(test, "Dashboard Page");


                    test.Log(Status.Info, $"<b>************************************<u>Started Process to Edit/Delete");
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.manageAutopayButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                    reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                    // Edit Scenarios Starts from here
                    test.Log(Status.Info, $"<b>*****************************************<u>Edit Scenarios Starts from here</u>*****************************************</b>");
                    webElementExtensions.WaitForElementToBeEnabled(_driver, payments.editIconManageAutopayLocBy, isScrollIntoViewRequired: true);
                    webElementExtensions.ClickElement(_driver, payments.editIconManageAutopayLocBy, "Edit Button on Manage Autopay", isReportRequired: true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.editIconManageAutopayLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, isScrollIntoViewRequired: false, timeoutInSeconds: ConfigSettings.SmallWaitTime);
                    bool isUpdateAutopayButtonDisabled = webElementExtensions.IsElementDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Autopay Page");
                    ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonDisabled, "Edit should be allowed");
                    reportLogger.TakeScreenshot(test, " Edit Autopay Page");
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToManageAutopayLinkLocBy);
                    webElementExtensions.WaitForElementToBeEnabled(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);
                    webElementExtensions.ClickElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    webElementExtensions.WaitForElementToBeEnabled(_driver, payments.deleteIconManageAutopayLocBy, isScrollIntoViewRequired: true);
                    webElementExtensions.ClickElement(_driver, payments.deleteIconManageAutopayLocBy, "Delete Button on Manage Autopay", isReportRequired: true);
                    webElementExtensions.WaitForElementToBeEnabled(_driver, dashboard.dialogLocBy);
                    bool isDeleteAllowing = webElementExtensions.IsElementEnabled(_driver, dashboard.dialogLocBy);
                    ReportingMethods.LogAssertionTrue(test, isDeleteAllowing, "Delete should be allowed");
                    reportLogger.TakeScreenshot(test, " Delete Autopay Page");
                    webElementExtensions.ClickElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    test.Log(Status.Info, $"<b>************************************<u>Started Process to Edit/Delete Autopay from Pending Payments Section</u>************************************</b>");
                    // Edit Scenarios Starts from here
                    test.Log(Status.Info, $"<b>*****************************************<u>Edit Scenarios Starts from here</u>*****************************************</b>");
                    payments.EditAutopayPayment(paymentDate);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);

                    bool isManageAutopayShowing = webElementExtensions.IsElementDisplayed(_driver, payments.backToAccountSummaryLinkLocBy);
                    ReportingMethods.LogAssertionTrue(test, isManageAutopayShowing, "Manage Autopay should Show for edit");
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    payments.DeleteAutopayPaymentSetup(paymentDate);
                    isManageAutopayShowing = webElementExtensions.IsElementDisplayed(_driver, payments.backToAccountSummaryLinkLocBy);
                    ReportingMethods.LogAssertionTrue(test, isManageAutopayShowing, "Manage Autopay should Show for Delete");

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                }


            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("TPR-2709-Test-HELOC Autopay-Verify Edits/Deletes to autopay not allowed | Draft date is 1st thru 15th and its a weekend or holiday | Todayâ€™s date = draft date | Before Cutoff Time \n"+
            "TPR-2712 Test-HELOC Autopay-Verify Edit and Delete buttons are disabled | Draft date is 1st thru 15th, it is a business day | Todayâ€™s date = draft date and it is >= 8PM CST | After Cutoff Time \n" +
            "TPR-2711 Test-HELOC Autopay-Verify Edit and Delete buttons are enabled. User can edit or delete autopay as normal | Draft date is > 13th (14th/15th) | Today is a business day | Todayâ€™s date != draft date | Before Cutoff Time \n")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_2709_2712_2711_TPR_VerifyTheHelocAutoPaySetup_EditDelete_If_DraftDateIsToday()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsForExisitngAutopayHeloc} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                requiredColumns.Add("PaymentDate");
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsForExisitngAutopayHeloc, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        if (dashboard.IsLoanHasAutopayToggleON())
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
                        if (dashboard.IsLoanHasAutopayToggleON())
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
                var paymentDateFromDb = loanLevelData[retryCount][Constants.PaymentSetupDataColumns.PaymentDate].ToString();
                var paymentDate = DateTime.ParseExact(paymentDateFromDb, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                var holidays = commonServices.GetBankHolidays(paymentDate.Year.ToString());
                var currentCstTime = UtilAdditions.GetCSTTimeNow();
                if (holidays.Contains(paymentDate)
                    || paymentDate.DayOfWeek == DayOfWeek.Saturday
                    || paymentDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    test.Log(Status.Warning, $"{loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString()} has autopay setup on Weekend/Holiday : {paymentDate.Date}, Edit/Delete is not allowed for this loan today");
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.manageAutopayButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                    reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                    return;
                }

                if (currentCstTime.Hour >= 20)
                {
                    test.Log(Status.Info, $"Current CST time is {currentCstTime}, which is after cutoff time 8 PM CST, hence Edit/Delete Autopay setup not allowed");
                    reportLogger.TakeScreenshot(test, "Dashboard Page");
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.manageAutopayButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                    reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                    return;
                }
                else
                {
                    test.Log(Status.Info, $"Current CST time is {currentCstTime}, which is before cutoff time 8 PM CST, Expecting Edit/Delete should be allowed");
                    var formattedPaymentDate = paymentDate.ToString("MMM d, yyyy");
                    bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                    int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                    List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                    ReportingMethods.LogAssertionTrue(test, pendingPaymentDates.Contains(formattedPaymentDate), $"Check Pending Payment with Payment Date {formattedPaymentDate}");
                    reportLogger.TakeScreenshot(test, "Dashboard Page");

                    test.Log(Status.Info, $"<b>************************************<u>Started Process to Edit/Delete");
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.manageAutopayButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                    reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                    // Edit Scenarios Starts from here
                    webElementExtensions.WaitForElementToBeEnabled(_driver, payments.editIconManageAutopayLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.editIconManageAutopayLocBy, "Edit Button on Manage Autopay", isReportRequired: true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.editIconManageAutopayLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, isScrollIntoViewRequired: false, timeoutInSeconds: ConfigSettings.SmallWaitTime);
                    bool isUpdateAutopayButtonDisabled = webElementExtensions.IsElementDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Autopay Page");
                    ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonDisabled, "Edit should be allowed");
                    reportLogger.TakeScreenshot(test, " Edit Autopay Page");
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy);

                    webElementExtensions.WaitForElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);
                    if (webElementExtensions.IsElementDisplayed(_driver, payments.leaveButtonOnLeavingPagePopupLocBy))
                    {
                        webElementExtensions.WaitForElementToBeEnabled(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);
                        webElementExtensions.ClickElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);
                    }

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForPageLoad(_driver);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy);
                    webElementExtensions.ScrollToTop(_driver);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayButtonLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.manageAutopayButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);

                    webElementExtensions.WaitForElementToBeEnabled(_driver, payments.deleteIconManageAutopayLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.deleteIconManageAutopayLocBy, "Delete Button on Manage Autopay", isReportRequired: true);
                    webElementExtensions.WaitForElementToBeEnabled(_driver, dashboard.dialogLocBy);
                    bool isDeleteAllowing = webElementExtensions.IsElementEnabled(_driver, dashboard.dialogLocBy);
                    ReportingMethods.LogAssertionTrue(test, isDeleteAllowing, "Delete should be allowed");
                    reportLogger.TakeScreenshot(test, " Delete Autopay Page");
                    webElementExtensions.ClickElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    test.Log(Status.Info, $"<b>************************************<u>Started Process to Edit/Delete Autopay from Pending Payments Section</u>************************************</b>");
                    webElementExtensions.ScrollToTop(_driver);
                    // Edit Scenarios Starts from here
                    test.Log(Status.Info, $"<b>*****************************************<u>Edit Scenarios Starts from here</u>*****************************************</b>");
                    reportLogger.TakeScreenshot(test, " Dashboard Page");
                    payments.EditAutopayPayment(paymentDate);

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                    webElementExtensions.ScrollToTop(_driver);
                    bool isManageAutopayShowing = webElementExtensions.IsElementDisplayed(_driver, payments.backToAccountSummaryLinkLocBy);
                    ReportingMethods.LogAssertionTrue(test, isManageAutopayShowing, "Manage Autopay should Show for edit");
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.ScrollToTop(_driver);
                    payments.DeleteAutopayPaymentSetup(paymentDate);
                    isManageAutopayShowing = webElementExtensions.IsElementDisplayed(_driver, payments.backToAccountSummaryLinkLocBy);
                    ReportingMethods.LogAssertionTrue(test, isManageAutopayShowing, "Manage Autopay should Show for Delete");
                    webElementExtensions.ScrollToTop(_driver);
                    reportLogger.TakeScreenshot(test, " Manage Autopay Page");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                }


            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }


    }
}
