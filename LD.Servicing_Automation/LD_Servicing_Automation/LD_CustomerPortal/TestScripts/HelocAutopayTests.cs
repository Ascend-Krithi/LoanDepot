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
using System.Text.RegularExpressions;

namespace LD_CustomerPortal.Tests
{
    [TestClass]
    public class HelocAutopayTests : BasePage
    {
        string loanDetailsQueryForEligibleHelocAutopayPastDue = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayPastDue));
        string loanDetailsQueryForEligibleHelocAutopayOntime = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayOntime));
        string loanDetailsQueryForEligibleHelocAutopayPrepaidOneMonth = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayPrepaidOneMonth));
        string loanDetailsQueryForDeleteHelocAutopayOntime = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayDeleteScenarioOntime));
        string loanDetailsQueryForDeleteHelocAutopayPastdue = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayDeleteScenarioPastDue));
        string loanDetailsQueryForDeleteHelocAutopayOneMonthPrepaid = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayDeleteScenarioPrepaid));

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
        [Description("<br><b>TPR-2485-E2E-Setup_Edit_HELOC Autopay Ontime</b>"+
            "\n TPR-1442 Verify HELOC loan is eligible for autopay when Investor ID is Cxx, L10, W10 and process_stop_code in (0, 1, 7, C, E, G, I, Q, S. T, Y)\n")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay"), TestCategory("CP_Sanity")]
        public void TPR_1442_2485_TPR_VerifyHelocAutoPaySetup_Edit_Functionality_For_Monthly_Ontime()
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, true))
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                Dictionary<string, string> feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                decimal totalFees = feeDic?.Values
                    .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                    .Sum() ?? 0;
                dashboard.VerifyPaymentSummaryDetailsForHelocLoanOnDashboardPage(loanLevelData[retryCount], helocLoanInfo, totalFees);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");

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
                string expTotalAmount = $"${(Convert.ToDouble(helocLoanInfo.NextPaymentDue) + Convert.ToDouble(totalFees)).ToString("N").Trim()}";
                string expMonthlyAutopayAmount = "$" + Convert.ToDouble(loanLevelData[retryCount][Constants.LoanLevelDataColumns.TotalMonthlyPayment]).ToString("N").Trim();
                webElementExtensions.VerifyElementText(_driver, payments.monthlyPaymentPlanAmountTextLocBy, (helocLoanInfo.IsBillGenerated) ? expMonthlyAutopayAmount : $"${0.00.ToString("N").Trim()}", "Monthly Autopay payment plan Amount", isReportRequired: true);
                ReportingMethods.LogAssertionContains(test, (helocLoanInfo.IsBillGenerated) ? expTotalAmount : $"${0.00.ToString("N").Trim()}", actualTotalAmountOnSetupAutoPayPage, "Total Monthly Autopay payment plan Amount");
                decimal principalInterest = Convert.ToDecimal(loanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount]);
                string interest = string.Format(new CultureInfo("en-US"), "{0:C}", principalInterest);
                decimal taxInDecimal = Convert.ToDecimal(loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount]);
                string tax = string.Format(new CultureInfo("en-US"), "{0:C}", taxInDecimal);
                if (!helocLoanInfo.IsBillGenerated)
                {
                    webElementExtensions.MoveToElement(_driver, payments.autopayMonthlyAmountToDebitInfoIconLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.tooltipPleaseRevisitAfter16thTextMsgOnSetupaAutopayLocBy);
                    webElementExtensions.VerifyElementText(_driver, payments.tooltipPleaseRevisitAfter16thTextMsgOnSetupaAutopayLocBy, Constants.CustomerPortalTextMessages.HelocAutopayPleaseRevisitAfter16thOfEveryMonthOnSetupAutopayPageText, "Tooltip : Please revisit after 16th of every month Text Msg");
                }

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
                // Edit Scenarios Starts from here
                test.Log(Status.Info, $"<b>*****************************************<u>Edit Scenarios Starts from here</u>*****************************************</b>");
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.editIconManageAutopayLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElement(_driver, payments.editIconManageAutopayLocBy, "Edit Button on Manage Autopay", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.editIconManageAutopayLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForStalenessOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy, 2);
                webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, isScrollIntoViewRequired: false);
                bool isUpdateAutopayButtonDisabled = webElementExtensions.IsElementDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Autopay Page");
                ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonDisabled, isUpdateAutopayButtonDisabled ? $"Successfully validated that on the Edit Autopay Page Update Autopay buttonis <b>Disabled</b>" : $"Failed - On Edit Autopay Page Update Autopay button is <b>Enabled</b>");
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
                ReportingMethods.LogAssertionTrue(test, paymentDate.Contains(paymentDate), "Verify Payment Start Date");
                ReportingMethods.LogAssertionTrue(test, bankAccountName.Contains(bankAccountName), "Verify Bank Account Name");
                ReportingMethods.LogAssertionTrue(test, accountNumberWhileEdit.Contains(accountNumberWhileEdit), "Verify Bank Account Number");
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal) && test.Model.FullName.ToString().Contains("HelocAutoPay"))
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(Constants.CustomerPortalTextMessages.HelocAutopaySetupReviewPageDisclosureText), "Verify Bank Account Name");
                else
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
                ReportingMethods.LogAssertionTrue(test, paymentDate.Contains(paymentDate), "Verify Payment Start Date");
                ReportingMethods.LogAssertionTrue(test, bankAccountName.Contains(bankAccountName), "Verify Bank account Name");
                ReportingMethods.LogAssertionTrue(test, accountNumberWhileEdit.Contains(accountNumberWhileEdit), "Verify Bank Account Number");
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal) && test.Model.FullName.ToString().Contains("HelocAutoPay"))
                    ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.HelocAutopaySetupConfirmationPageDisclosureText), "Verify Autopay Confirmation Page Disclosure Text");
                else
                    ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupConfirmationPageDisclosureText), "Verify Autopay Confirmation Page Disclosure Text");
                reportLogger.TakeScreenshot(test, "Autopay Setup Confirmaion Page");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmAutopayButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.editAutopayConfirmationPageLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToManageAutoPayButtonLocBy, "Back To Manage AutoPay Button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                webElementExtensions.WaitForElement(_driver, payments.manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                if (!helocLoanInfo.IsBillGenerated)
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.expandMoreDownArrowOnManageAutopayLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.additionalPrincipalBalanceOnManageAutopayTextLocBy);
                    webElementExtensions.VerifyElementText(_driver, payments.additionalPrincipalBalanceOnManageAutopayTextLocBy, amount, "Additional Principal Balance On Manage Autopay Page", true, true);
                }
                test.Log(Status.Info, $"<b><u>Manage Autopay Page Content Validation Started</u></b>");
                webElementExtensions.VerifyElementText(_driver, payments.planForAutopayPaymentManageAutopayTextLocBy, "Monthly", "Autopay Payment Plan", true);
                webElementExtensions.VerifyElementText(_driver, payments.nextDebitDateManageAutoPayTextLocBy, paymentDate, isReportRequired: true);
                webElementExtensions.VerifyElementText(_driver, payments.amountOnManageAutoPayTextLocBy, (helocLoanInfo.IsBillGenerated) ? amount : $"${0.00.ToString("N").Trim()}", isReportRequired: true);

                webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryLinkLocBy, "Back to Account Summary Link", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
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
        [Description("<br><b>TPR-2486-E2E-Setup_Edit_HELOC Autopay PastDue</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_2486_TPR_VerifyHelocAutoPaySetup_Edit_Functionality_For_Monthly_PastDue()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            List<string> exptDates = new List<string>();
            bool isEligible = false;
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
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
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
                    test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                    webElementExtensions.WaitForPageLoad(_driver);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                    APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                    Dictionary<string, string> feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                    decimal totalFees = feeDic?.Values
                        .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                        .Sum() ?? 0;
                    dashboard.VerifyPaymentSummaryDetailsForHelocLoanOnDashboardPage(loanLevelData[retryCount], helocLoanInfo, totalFees);
                    test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");

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
                    string expTotalAmount = "$" + (Convert.ToDouble(loanLevelData[retryCount][Constants.LoanLevelDataColumns.TotalMonthlyPayment]) + Convert.ToDouble(totalFees)).ToString("N").Trim();
                    string expMonthlyAutopayAmount = "$" + Convert.ToDouble(loanLevelData[retryCount][Constants.LoanLevelDataColumns.TotalMonthlyPayment]).ToString("N").Trim();
                    webElementExtensions.VerifyElementText(_driver, payments.monthlyPaymentPlanAmountTextLocBy, expMonthlyAutopayAmount, "Monthly Autopay payment plan Amount", isReportRequired: true);
                    ReportingMethods.LogAssertionContains(test, expTotalAmount, actualTotalAmountOnSetupAutoPayPage, "Total Monthly Autopay payment plan Amount");
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
                    // Edit Scenarios Starts from here
                    test.Log(Status.Info, $"<b>*****************************************<u>Edit Scenarios Starts from here</u>*****************************************</b>");
                    webElementExtensions.WaitForElementToBeEnabled(_driver, payments.editIconManageAutopayLocBy, isScrollIntoViewRequired: false);
                    webElementExtensions.ClickElement(_driver, payments.editIconManageAutopayLocBy, "Edit Button on Manage Autopay", isReportRequired: true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.editIconManageAutopayLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForStalenessOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy, 2);
                    webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, isScrollIntoViewRequired: false);
                    bool isUpdateAutopayButtonDisabled = webElementExtensions.IsElementDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Autopay Page");
                    ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonDisabled, isUpdateAutopayButtonDisabled ? $"Successfully validated that on the Edit Autopay Page Update Autopay buttonis <b>Disabled</b>" : $"Failed - On Edit Autopay Page Update Autopay button is <b>Enabled</b>");
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
                    ReportingMethods.LogAssertionTrue(test, paymentDate.Contains(paymentDate), "Verify Payment Start Date");
                    ReportingMethods.LogAssertionTrue(test, bankAccountName.Contains(bankAccountName), "Verify Bank Account Name");
                    ReportingMethods.LogAssertionTrue(test, accountNumberWhileEdit.Contains(accountNumberWhileEdit), "Verify Bank Account Number");
                    if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal) && test.Model.FullName.ToString().Contains("HelocAutoPay"))
                        ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(Constants.CustomerPortalTextMessages.HelocAutopaySetupReviewPageDisclosureText), "Verify Bank Account Name");
                    else
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
                    ReportingMethods.LogAssertionTrue(test, paymentDate.Contains(paymentDate), "Verify Payment Start Date");
                    ReportingMethods.LogAssertionTrue(test, bankAccountName.Contains(bankAccountName), "Verify Bank account Name");
                    ReportingMethods.LogAssertionTrue(test, accountNumberWhileEdit.Contains(accountNumberWhileEdit), "Verify Bank Account Number");
                    if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal) && test.Model.FullName.ToString().Contains("HelocAutoPay"))
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.HelocAutopaySetupConfirmationPageDisclosureText), "Verify Autopay Confirmation Page Disclosure Text");
                    else
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupConfirmationPageDisclosureText), "Verify Autopay Confirmation Page Disclosure Text");
                    reportLogger.TakeScreenshot(test, "Autopay Setup Confirmaion Page");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmAutopayButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.editAutopayConfirmationPageLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToManageAutoPayButtonLocBy, "Back To Manage AutoPay Button", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);
                    reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                    if (!(helocLoanInfo.IsBillGenerated))
                    {
                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.expandMoreDownArrowOnManageAutopayLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, payments.additionalPrincipalBalanceOnManageAutopayTextLocBy);
                        webElementExtensions.VerifyElementText(_driver, payments.additionalPrincipalBalanceOnManageAutopayTextLocBy, $"${1.00.ToString("N").Trim()}", "Additional Principal Balance On Manage Autopay Page", true, true);
                    }
                    test.Log(Status.Info, $"<b><u>Manage Autopay Page Content Validation Started</u></b>");
                    webElementExtensions.VerifyElementText(_driver, payments.planForAutopayPaymentManageAutopayTextLocBy, "Monthly", "Autopay Payment Plan", true);
                    webElementExtensions.VerifyElementText(_driver, payments.nextDebitDateManageAutoPayTextLocBy, paymentDate, isReportRequired: true);
                    webElementExtensions.VerifyElementText(_driver, payments.amountOnManageAutoPayTextLocBy, (!helocLoanInfo.IsBillGenerated) ? amount : $"${0.00.ToString("N").Trim()}", isReportRequired: true);

                    webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryLinkLocBy, "Back to Account Summary Link", isReportRequired: true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
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
                else
                    test.Log(Status.Warning, "<font color='Orange'><b>Skipped the Execution ahead as right now PastDue loans are not Eligible for Heloc Autopay Setup</b></font>");
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
        [Description("<br><b>TPR-2484-E2E-Setup_Edit_HELOC Autopay Prepaid One Month</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_2484_TPR_VerifyHelocAutoPaySetup_Edit_Functionality_For_Monthly_PrepaidOneMonth()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForEligibleHelocAutopayPrepaidOneMonth} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForEligibleHelocAutopayPrepaidOneMonth, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, true))
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                Dictionary<string, string> feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                decimal totalFees = feeDic?.Values
                    .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                    .Sum() ?? 0;
                dashboard.VerifyPaymentSummaryDetailsForHelocLoanOnDashboardPage(loanLevelData[retryCount], helocLoanInfo, totalFees);
                test.Log(Status.Info, "<b>********************************************<u>Ending Account Summary Page Validation</u>*******************************************</b>");

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
                string expTotalAmount = "$" + (Convert.ToDouble(loanLevelData[retryCount][Constants.LoanLevelDataColumns.TotalMonthlyPayment]) + Convert.ToDouble(loanLevelData[retryCount][Constants.LoanLevelDataColumns.AccruedLateChargeAmount])).ToString("N").Trim();
                string expMonthlyAutopayAmount = "$" + Convert.ToDouble(loanLevelData[retryCount][Constants.LoanLevelDataColumns.TotalMonthlyPayment]).ToString("N").Trim();
                webElementExtensions.VerifyElementText(_driver, payments.monthlyPaymentPlanAmountTextLocBy, (helocLoanInfo.IsBillGenerated) ? expMonthlyAutopayAmount : $"${0.00.ToString("N").Trim()}", "Monthly Autopay payment plan Amount", isReportRequired: true);
                ReportingMethods.LogAssertionContains(test, (helocLoanInfo.IsBillGenerated) ? expTotalAmount : $"${0.00.ToString("N").Trim()}", actualTotalAmountOnSetupAutoPayPage, "Total Monthly Autopay payment plan Amount");
                decimal principalInterest = Convert.ToDecimal(loanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount]);
                string interest = string.Format(new CultureInfo("en-US"), "{0:C}", principalInterest);
                decimal taxInDecimal = Convert.ToDecimal(loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount]);
                string tax = string.Format(new CultureInfo("en-US"), "{0:C}", taxInDecimal);

                webElementExtensions.MoveToElement(_driver, payments.autopayMonthlyAmountToDebitInfoIconLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.tooltipPleaseRevisitAfter16thTextMsgOnSetupaAutopayLocBy);
                webElementExtensions.VerifyElementText(_driver, payments.tooltipPleaseRevisitAfter16thTextMsgOnSetupaAutopayLocBy, Constants.CustomerPortalTextMessages.HelocAutopayPleaseRevisitAfter16thOfEveryMonthOnSetupAutopayPageText, "Tooltip : Please revisit after 16th of every month Text Msg");

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
                // Edit Scenarios Starts from here
                test.Log(Status.Info, $"<b>*****************************************<u>Edit Scenarios Starts from here</u>*****************************************</b>");
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.editIconManageAutopayLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElement(_driver, payments.editIconManageAutopayLocBy, "Edit Button on Manage Autopay", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.editIconManageAutopayLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForStalenessOfElement(_driver, payments.updateAutopayButtonEditAutopayLocBy, 2);
                webElementExtensions.WaitForElementToBeDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, isScrollIntoViewRequired: false);
                bool isUpdateAutopayButtonDisabled = webElementExtensions.IsElementDisabled(_driver, payments.updateAutopayButtonEditAutopayLocBy, "Update Autopay Button on Autopay Page");
                ReportingMethods.LogAssertionTrue(test, isUpdateAutopayButtonDisabled, isUpdateAutopayButtonDisabled ? $"Successfully validated that on the Edit Autopay Page Update Autopay buttonis <b>Disabled</b>" : $"Failed - On Edit Autopay Page Update Autopay button is <b>Enabled</b>");
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
                ReportingMethods.LogAssertionTrue(test, paymentDate.Contains(paymentDate), "Verify Payment Start Date");
                ReportingMethods.LogAssertionTrue(test, bankAccountName.Contains(bankAccountName), "Verify Bank Account Name");
                ReportingMethods.LogAssertionTrue(test, accountNumberWhileEdit.Contains(accountNumberWhileEdit), "Verify Bank Account Number");
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal) && test.Model.FullName.ToString().Contains("HelocAutoPay"))
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(Constants.CustomerPortalTextMessages.HelocAutopaySetupReviewPageDisclosureText), "Verify Bank Account Name");
                else
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
                ReportingMethods.LogAssertionTrue(test, "Monthly".Contains("Monthly"), "Verify Autopay Plan Type");
                ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains("*******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4)), "Verify Loan Number");
                ReportingMethods.LogAssertionTrue(test, paymentDate.Contains(paymentDate), "Verify Payment Start Date");
                ReportingMethods.LogAssertionTrue(test, bankAccountName.Contains(bankAccountName), "Verify Bank account Name");
                ReportingMethods.LogAssertionTrue(test, accountNumberWhileEdit.Contains(accountNumberWhileEdit), "Verify Bank Account Number");
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal) && test.Model.FullName.ToString().Contains("HelocAutoPay"))
                    ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.HelocAutopaySetupConfirmationPageDisclosureText), "Verify Autopay Confirmation Page Disclosure Text");
                else
                    ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupConfirmationPageDisclosureText), "Verify Autopay Confirmation Page Disclosure Text");
                reportLogger.TakeScreenshot(test, "Autopay Setup Confirmaion Page");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmAutopayButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.editAutopayConfirmationPageLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToManageAutoPayButtonLocBy, "Back To Manage AutoPay Button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.backToManageAutoPayButtonLocBy);
                webElementExtensions.WaitForElement(_driver, payments.manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                if (!helocLoanInfo.IsBillGenerated)
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.expandMoreDownArrowOnManageAutopayLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.additionalPrincipalBalanceOnManageAutopayTextLocBy);
                    webElementExtensions.VerifyElementText(_driver, payments.additionalPrincipalBalanceOnManageAutopayTextLocBy, amount, "Additional Principal Balance On Manage Autopay Page", true, true);
                }
                test.Log(Status.Info, $"<b><u>Manage Autopay Page Content Validation Started</u></b>");
                webElementExtensions.VerifyElementText(_driver, payments.planForAutopayPaymentManageAutopayTextLocBy, "Monthly", "Autopay Payment Plan", true);
                webElementExtensions.VerifyElementText(_driver, payments.nextDebitDateManageAutoPayTextLocBy, paymentDate, isReportRequired: true);
                webElementExtensions.VerifyElementText(_driver, payments.amountOnManageAutoPayTextLocBy, (helocLoanInfo.IsBillGenerated) ? amount : $"${0.00.ToString("N").Trim()}", isReportRequired: true);

                webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryLinkLocBy, "Back to Account Summary Link", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
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
        [Description("<br>TPR-2488 Delete_HELOC Autopay Ontime")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_2488_TPR_VerifyDeleteHelocAutopayFunctionality_Ontime()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><b>{loanDetailsQueryForDeleteHelocAutopayOntime}</b>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForDeleteHelocAutopayOntime, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                string title = " loanDepot Cancellation Notice of Recurring Automatic Payment Plan - SM256 ";
                string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM256, "Monthly");
                string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
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
        [Description("<br>TPR-2489 Delete_HELOC Autopay Past Due")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_2489_TPR_VerifyDeleteHelocAutopayFunctionality_Pastdue()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><b>{loanDetailsQueryForDeleteHelocAutopayPastdue}</b>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForDeleteHelocAutopayPastdue, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                string title = " loanDepot Cancellation Notice of Recurring Automatic Payment Plan - SM256 ";
                string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM256, "Monthly");
                string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
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
        [Description("<br>TPR-2487 Delete_HELOC Autopay Prepaid 1M")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_2487_TPR_VerifyDeleteHelocAutopayFunctionality_OneMonthPrepaid()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><b>{loanDetailsQueryForDeleteHelocAutopayOneMonthPrepaid}</b>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForDeleteHelocAutopayOneMonthPrepaid, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                reportLogger.TakeScreenshot(test, "Secure Message Center Page");
                string title = " loanDepot Cancellation Notice of Recurring Automatic Payment Plan - SM256 ";
                string messageTemplateContent = smc.GetSMCTemplateContent(Constants.SMCCode.SM256, "Monthly");
                string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                string encrypedLoanNumber = "******" + loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(title.Trim()), messageTemplateContent.Contains(title.Trim()) ? $"Successfully validated the Title of SMC Message Template - <b>{title.Trim()}</b>" : $"Failed while verifying the Title of SMC Message Template - <b>{title.Trim()}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(recipientName), messageTemplateContent.Contains(recipientName) ? $"Successfully validated the Recipient User Name of SMC Message Template - <b>{recipientName}</b>" : $"Failed while verifying the Recipient User Name In SMC Message Template - <b>{recipientName}</b>, please check the logs");
                ReportingMethods.LogAssertionTrue(test, messageTemplateContent.Contains(encrypedLoanNumber), messageTemplateContent.Contains(encrypedLoanNumber) ? $"Successfully validated the encrypted loan number in SMC Message Template - <b>{encrypedLoanNumber}</b>" : $"Failed while verifying the encrypted loan number In SMC Message Template - <b>{encrypedLoanNumber}</b>, please check the logs");
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
        [Description("<br>TPR-1033-Test Heloc Autopay Monthly - OTP Not Allowed When Enrolled In Autopay Pastdue")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocOTP")]
        public void TPR_1033_TPR_VerifyHelocOTPShouldNotBeAllowedForActiveAutopayScheduledForPastDue()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><b>{loanDetailsQueryForDeleteHelocAutopayPastdue}</b>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForDeleteHelocAutopayPastdue, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                webElementExtensions.MoveToElement(_driver, dashboard.loanDepotLogoLocBy);
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                if (loanLevelData[retryCount][Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                    payments.SelectDelinquencyReason("Curtailment of Income");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                string nextPaymentDueDate = DateTime.Parse(loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
                commonServices.ClickConfirmButtonPaymentReviewPage();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.unableToProcessPaymentWarningPopUpLocBy, ConfigSettings.WaitTime);
                webElementExtensions.VerifyElementText(_driver, payments.unableToProcessPaymentWarningPopUpForHelocLocBy, Constants.CustomerPortalErrorMsgs.UnableToProceedPaymentWarningForHelocMsg, "Unable to process payment Popup", true);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.unableToProcessPaymentWarningPopUpCloseButtonLocBy, "Close button on Unable to proceed payment Popup", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
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
        [Description("<br>TPR-1021-Test Heloc Autopay Monthly - OTP Not Allowed When Enrolled In Autopay Ontime")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocOTP")]
        public void TPR_1021_TPR_VerifyHelocOTPShouldNotBeAllowedForActiveAutopayScheduledForOntime()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><b>{loanDetailsQueryForDeleteHelocAutopayOntime}</b>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForDeleteHelocAutopayOntime, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                Dictionary<string, string> feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                decimal totalFees = feeDic?.Values
                    .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                    .Sum() ?? 0;
                test.Log(Status.Info, (helocLoanInfo.IsBillGenerated) ? "<b>Bill is Generated for the upcoming month</b>" : "<b>Bill is not Generated for the upcoming month</b>");
                webElementExtensions.MoveToElement(_driver, dashboard.loanDepotLogoLocBy);
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

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
                webElementExtensions.VerifyElementText(_driver, payments.unableToProcessPaymentWarningPopUpForHelocLocBy, Constants.CustomerPortalErrorMsgs.UnableToProceedPaymentWarningForHelocOntimeMsg, "Unable to process payment Popup", true);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.unableToProcessPaymentWarningPopUpCloseButtonLocBy, "Close button on Unable to proceed payment Popup", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
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
