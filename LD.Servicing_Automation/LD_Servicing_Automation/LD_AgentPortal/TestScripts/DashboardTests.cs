using AventStack.ExtentReports;
using LD_AgentPortal.Pages;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LD_AgentPortal.Tests
{
    [TestClass]
    public class DashboardTests : BasePage
    {
        public static string loanDetailsQueryforAutopay = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForAutoPay));
        public static string loanDetailsQueryForOtp = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.Get100LoanLevelDetails));
        public static string loanLevelDetailsForDifferentLoanTypes = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForDifferentLoanTypes));
        public static string autopayEligibleLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForAutoPay));
        public static string activeAutopayLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsWithActiveAutoPay));
        public static string modificationTrialLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetActiveModificationTrialLoanLevelDetails));
        public static string modTrialChangesInProgressLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetSystemChangesInProgressTrialLoanLevelDetails));
        public static string getLoanLevelDetailsForActiveForbearance = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForActiveForbearance));
        public static string getLoanLevelDetailsForActiveRepayment = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForActiveRepayment));

        public TestContext TestContext
        {
            set;
            get;
        }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [ClassInitialize]
        public static void TestClassInitialize(TestContext TestContext)
        {

        }

        #region CommonTestData

        public static string loanDetailsQuery = string.Empty;
        string deleteReason = "Test Delete Reason";
        List<Hashtable> listOfLoanLevelData = null;
        static List<string> columnDataRequired = typeof(Constants.LoanLevelDataColumns)
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(field => field.FieldType == typeof(string))
                    .Select(field => (string)field.GetValue(null)).ToList();
        string firstName = "TESTFN";
        string lastName = "TESTLN";
        string personalOrBussiness = "Personal";
        string savings = "Savings";
        string accountNumber = Constants.BankAccountData.BankAccountNumber;
        string accountNumberWhileEdit = "92300361000";
        string routingNumber = "122199983";
        string bankAccountName = Constants.BankAccountData.BankAccountName;
        string accountFullName = "TESTFN TESTLN";
        Dictionary<string, string> loanTypes = new Dictionary<string, string>
        {
            { "0", "On-time" },
            { "1", "Past Due" },
            { "2", "Delinquent" },
            { "3", "Delinquent+" },
            { "Prepaid", "Prepaid" }
        };

        #endregion CommonTestData

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

        #region TestScriptsWithoutTestcaseID
        //Test cases included in this region are miscellaneous tests like OCD file testing, bulk otp setup, etc.

        [TestMethod]
        [Description("Verify the functionality of making a One Time Payment for any loan type - On time, Past due, Trial, Delinquent, etc.")]
        [TestCategory("AP_Dashboard")]
        public void SCM_XXXX_VerifyOneTimePaymentFunctionalityForAnyLoanType()
        {
            int retryCount = 0;
            string loanNumber = string.Empty, borrowerName = string.Empty, result = string.Empty, dateToBeSelected = string.Empty;

            #region TestData

            List<string> loansWithSuccessfullPaymentSetup = new List<string>();
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber,
                                                                 Constants.LoanLevelDataColumns.BorrowerFirstName,
                                                                 Constants.LoanLevelDataColumns.BorrowerLastName};

            loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForOtp, null, loanDataRequired, usedLoanTestData);

            #endregion TestData

            try
            {
                commonServices.LoginToTheApplication(username, password);
                retryCount = 0;
                dateToBeSelected = webElementExtensions.DateTimeConverter(_driver, DateTime.Now.ToString(), "m/d/yyyy to fullMonthName d, yyyy");
                while (retryCount < loanLevelData.Count)
                {
                    if (commonServices.LaunchUrlWithLoanNumber(loanLevelData, retryCount, true))
                    {
                        loanNumber = loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                        borrowerName = loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerFirstName].ToString() + " " + loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerLastName].ToString();
                        dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
                        webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
                        webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                        if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.makeAPaymentButtonDisabledLocBy))
                        {
                            webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentButtonLocBy);
                            if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy))
                                webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentConfirmationPopupConfirmButtonLocBy);
                            webElementExtensions.WaitForElement(_driver, payments.paymentDatePickerIconLocBy);
                            payments.SelectValueInAuthorizedByDropdown(borrowerName);
                            string bankAccountName = Constants.BankAccountData.BankAccountName;
                            commonServices.AddAnAccount(bankAccountName, "TestFN", "TestLN", "Personal", "Savings", "122199983");

                            if (commonServices.VerifyPaymentDateToBeSelectedInDateFieldIsEnabled(dateToBeSelected, false))
                            {
                                commonServices.SelectPaymentDateInDateField(dateToBeSelected);
                                if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.paymentAlreadyExistsBannerLocBy))
                                {
                                    if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.monthlyPaymentPrepaidLocBy))
                                        if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.overDueAddPrincipalDisabledTextboxLocBy))
                                            webElementExtensions.EnterText(_driver, payments.additionalPrincipalTextboxLocBy, webElementExtensions.RandomNumberGenerator(_driver, 1, 10));
                                        else
                                            webElementExtensions.EnterText(_driver, payments.additionalEscrowTextboxLocBy, webElementExtensions.RandomNumberGenerator(_driver, 1, 10));
                                    else if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.prepayCheckboxLocBy))
                                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.prepayCheckboxLocBy);
                                    else if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.pastDueRadioButtonLocBy))
                                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.pastDueRadioButtonLocBy);
                                    //On time condition to be added here.
                                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                                    payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);
                                    if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.paymentAlreadyExistsAlertPopUpCloseIconLocBy))
                                    {
                                        webElementExtensions.ClickElement(_driver, payments.paymentAlreadyExistsAlertPopUpCloseIconLocBy);
                                        webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
                                        webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentButtonLocBy);
                                        if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy))
                                            webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentConfirmationPopupConfirmButtonLocBy);
                                        webElementExtensions.WaitForElement(_driver, payments.paymentDatePickerIconLocBy);
                                        payments.SelectValueInAuthorizedByDropdown(borrowerName);
                                        commonServices.AddAnAccount(bankAccountName, "TestFN", "TestLN", "Personal", "Savings", "122199983");
                                        commonServices.SelectPaymentDateInDateField(dateToBeSelected);
                                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.currentPaymentCheckboxLocBy);
                                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.additionalPaymentCheckboxLocBy);
                                        if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.overDueAddPrincipalDisabledTextboxLocBy))
                                            webElementExtensions.EnterText(_driver, payments.additionalPrincipalTextboxLocBy, webElementExtensions.RandomNumberGenerator(_driver, 1, 10));
                                        else
                                            webElementExtensions.EnterText(_driver, payments.additionalEscrowTextboxLocBy, webElementExtensions.RandomNumberGenerator(_driver, 1, 10));
                                        payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);
                                    }
                                    payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
                                    string confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
                                    payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
                                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                                    payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
                                    loansWithSuccessfullPaymentSetup.Add(loanNumber);
                                }
                            }
                        }
                    }
                    retryCount++;
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying Make a payment functionality: " + ex.Message);
            }
            finally
            {
                foreach (string loan in loansWithSuccessfullPaymentSetup)
                {
                    if (!string.IsNullOrEmpty(result))
                        result += ", ";
                    result += loan;
                }
                int count = loansWithSuccessfullPaymentSetup.Count;
                test.Log(Status.Pass, "PAYMENT SETUP SUCCESSFULL for THE LOAN NUMBERS - " + result + ". Count = " + count + ". Retry - " + retryCount + ".");
            }
        }

        [TestMethod]
        [Description("<br> Verify the sanity functionality of performing loan search, verifying page navigation and making a One Time Payment for any loan type - On time, Past due, Trial, Delinquent, etc. <br>")]
        [TestCategory("AP_Dashboard")]
        public void TC_XXXX_TC_AP_PerformLoanSearch_Navigation_OneTimePayment()
        {
            int retryCount = 0;
            string loanNumber = string.Empty, borrowerName = string.Empty, result = string.Empty, dateToBeSelected = string.Empty;

            #region TestData

            List<string> loansWithSuccessfullPaymentSetup = new List<string>();
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber,
                                                                 Constants.LoanLevelDataColumns.BorrowerFirstName,
                                                                 Constants.LoanLevelDataColumns.BorrowerLastName};

            loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForOtp, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

            #endregion TestData

            try
            {
                commonServices.LoginToTheApplication(username, password);
                retryCount = 0;
                dateToBeSelected = webElementExtensions.DateTimeConverter(_driver, DateTime.Now.ToString(), "m/d/yyyy to fullMonthName d, yyyy");
                while (retryCount < loanLevelData.Count)
                {
                    loanNumber = loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                    webElementExtensions.EnterText(_driver, dashboard.enterLoanNumberInputFieldLocBy, loanNumber, true, "Loan Number search", true);
                    webElementExtensions.ScrollIntoView(_driver, dashboard.verifyCallerButtonLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.verifyCallerButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, dashboard.viewAccountButtonLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.verifyCallerButtonLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);

                    //Close the Verify caller pop up
                    webElementExtensions.ClickElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy, "Verify Caller pop up Close icon", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);
                    webElementExtensions.WaitForElement(_driver, dashboard.backToSearchButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, dashboard.loanSummaryHeaderLabelLocBy);
                    borrowerName = loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerFirstName].ToString() + " " + loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerLastName].ToString();
                    dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
                    webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.makeAPaymentButtonDisabledLocBy))
                    {
                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentButtonLocBy, "Make a Payment button", true);
                        if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy))
                            webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentConfirmationPopupConfirmButtonLocBy);
                        webElementExtensions.WaitForElement(_driver, payments.paymentDatePickerIconLocBy);
                        payments.SelectValueInAuthorizedByDropdown(borrowerName);
                        string bankAccountName = Constants.BankAccountData.BankAccountName;
                        commonServices.AddAnAccount(bankAccountName, "TestFN", "TestLN", "Personal", "Savings", "122199983");

                        if (commonServices.VerifyPaymentDateToBeSelectedInDateFieldIsEnabled(dateToBeSelected, false))
                        {
                            commonServices.SelectPaymentDateInDateField(dateToBeSelected);
                            if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.paymentAlreadyExistsBannerLocBy))
                            {
                                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.monthlyPaymentPrepaidLocBy))
                                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.overDueAddPrincipalDisabledTextboxLocBy))
                                        webElementExtensions.EnterText(_driver, payments.additionalPrincipalTextboxLocBy, webElementExtensions.RandomNumberGenerator(_driver, 1, 10));
                                    else
                                        webElementExtensions.EnterText(_driver, payments.additionalEscrowTextboxLocBy, webElementExtensions.RandomNumberGenerator(_driver, 1, 10));
                                else if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.prepayCheckboxLocBy))
                                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.prepayCheckboxLocBy);
                                else if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.pastDueRadioButtonLocBy))
                                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.pastDueRadioButtonLocBy);
                                //On time condition to be added here.
                                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                                payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);
                                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.paymentAlreadyExistsAlertPopUpCloseIconLocBy))
                                {
                                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.paymentAlreadyExistsAlertPopUpCloseIconLocBy);
                                    webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
                                    webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentButtonLocBy);
                                    if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy))
                                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentConfirmationPopupConfirmButtonLocBy);
                                    webElementExtensions.WaitForElement(_driver, payments.paymentDatePickerIconLocBy);
                                    payments.SelectValueInAuthorizedByDropdown(borrowerName);
                                    commonServices.AddAnAccount(bankAccountName, "TestFN", "TestLN", "Personal", "Savings", "122199983");
                                    commonServices.SelectPaymentDateInDateField(dateToBeSelected);
                                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.currentPaymentCheckboxLocBy);
                                    if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.additionalPaymentCheckboxLocBy))
                                    {
                                        webElementExtensions.ClickElementUsingJavascript(_driver, payments.additionalPaymentCheckboxLocBy);
                                        if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.overDueAddPrincipalDisabledTextboxLocBy))
                                        {
                                            if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.lateFeeAmountCheckboxLocBy))
                                                webElementExtensions.ClickElementUsingJavascript(_driver, payments.lateFeeAmountCheckboxLocBy);
                                            webElementExtensions.EnterText(_driver, payments.additionalPrincipalTextboxLocBy, webElementExtensions.RandomNumberGenerator(_driver, 1, 10));
                                        }
                                        else
                                            webElementExtensions.EnterText(_driver, payments.additionalPrincipalTextboxLocBy, webElementExtensions.RandomNumberGenerator(_driver, 1, 10));
                                    }
                                    else
                                        webElementExtensions.EnterText(_driver, payments.additionalEscrowTextboxLocBy, webElementExtensions.RandomNumberGenerator(_driver, 1, 10));
                                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                                    payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);
                                }
                                payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
                                string confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
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
                                loansWithSuccessfullPaymentSetup.Add(loanNumber);
                            }
                        }
                    }
                    if (loansWithSuccessfullPaymentSetup.Count > 0)
                        break;
                    else
                        retryCount++;
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying Make a payment functionality: " + ex.Message);
            }
            finally
            {
                foreach (string loan in loansWithSuccessfullPaymentSetup)
                {
                    if (!string.IsNullOrEmpty(result))
                        result += ", ";
                    result += loan;
                }
                int count = loansWithSuccessfullPaymentSetup.Count;
                test.Log(Status.Pass, "PAYMENT SETUP SUCCESSFULL for THE LOAN NUMBERS - " + result + ". Count = " + count + ". Retry - " + retryCount + ".");
            }
        }

        [TestMethod]
        [Description("<br> TC_XXX_TC_Perform Make a Payment functionality for different loan types<br>")]
        [TestCategory("AP_Dashboard")]
        public void TC_XXXX_TC_PerformMakeAPaymentFunctionalityForDifferentLoanTypes()
        {
            commonServices.LoginToTheApplication(username, password);

            #region TestData

            int retryCount;
            Hashtable loanLevelData = null;

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Perform 'Make a payment' for loan type : " + loanType.Value + " ******</b>");

                retryCount = 0;
                if (loanType.Key == "0")
                {
                    loanDetailsQuery = loanLevelDetailsForDifferentLoanTypes.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else if (loanType.Key == "Prepaid")
                {
                    loanDetailsQuery = loanLevelDetailsForDifferentLoanTypes.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                }
                else if (loanType.Key == "3")
                {
                    loanDetailsQuery = loanLevelDetailsForDifferentLoanTypes.Replace("DELINQUENTCOUNT", ">2");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }
                else
                {
                    loanDetailsQuery = loanLevelDetailsForDifferentLoanTypes.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }

                listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery, true);

                #endregion TestData                

                if (listOfLoanLevelData.Count > 0)
                {
                    ReportingMethods.Log(test, "Query - " + loanDetailsQuery);
                    while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && listOfLoanLevelData.Count > retryCount)
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
                        if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= listOfLoanLevelData.Count)
                        {
                            test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                            Assert.Fail();
                        }
                    }
                    if (listOfLoanLevelData.Count < retryCount + 1)
                        break;
                    loanLevelData = listOfLoanLevelData[retryCount];

                    string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                       loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                       $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);

                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                }
            }
        }

        [TestMethod]
        [Description("<br> Verify the sanity functionality of performing loan search, verifying page navigation and setting up an Autopay for any loan type - On time, Past due, Trial, Delinquent, etc. <br>")]
        [TestCategory("AP_Dashboard")]
        public void TC_XXXX_TC_PerformLoanSearch_Navigation_SettingUpAutopay()
        {
            #region TestData

            int retryCount = 0;
            string borrowerName = string.Empty, nextPaymentDueDate = string.Empty, paymentDateToBeSelected = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
            loanLevelData = commonServices.GetLoanDataFromDatabase(autopayEligibleLoanDetailsQuery, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(loanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForAutopay())
                        break;
                    else
                        test.Log(Status.Info, "The loan number is not eligible for setting up autopay");
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                {
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    Assert.Fail();
                }
            }
            borrowerName = loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerFirstName].ToString() + " " + loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerLastName].ToString();
            nextPaymentDueDate = loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString();
            List<string> pendingPaymentDates = _driver.FindElements(cpDashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();

            dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
            webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
            webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData[retryCount]);

            webElementExtensions.WaitForElement(_driver, payments.autopayStatusLocBy);
            bool flag = webElementExtensions.VerifyElementText(_driver, payments.autopayStatusLocBy, "Off");
            _driver.ReportResult(test, flag, "Successfully verified that Autopay Status is 'OFF' for the selected loan.",
                                             "Failure - Autopay Status is not 'OFF' for the selected loan.");
            webElementExtensions.ScrollIntoView(_driver, payments.autopayStatusLocBy);
            reportLogger.TakeScreenshot(test, "Autopay Status");
            webElementExtensions.WaitForElement(_driver, payments.manageAutopayButtonLocBy);
            webElementExtensions.ActionClick(_driver, payments.manageAutopayButtonLocBy, "Manage Autopay button.");
            commonServices.SetupAutopay(null, pendingPaymentDates, borrowerName);
        }

        [TestMethod]
        [Description("<br> TC-XXX - Autopay deletion <br> ")]
        [TestCategory("AP_Dashboard")]
        public void TC_XXXX_TC_DeleteAutopayScheduledForAnyLoan()
        {
            #region TestData

            int retryCount = 0;
            string loanNumber = string.Empty, result = string.Empty;
            List<string> loansHavingSuccessfullAutopayDeletions = new List<string>();
            List<string> usedLoanTestData = new List<string>();
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
            loanLevelData = commonServices.GetLoanDataFromDatabase(activeAutopayLoanDetailsQuery, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

            #endregion TestData

            try
            {
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
                {
                    loanNumber = loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                    usedLoanTestData.Add(loanNumber);
                    if (commonServices.DeleteAutopayFromAgentPortal(loanNumber))
                        loansHavingSuccessfullAutopayDeletions.Add(loanNumber);
                    retryCount++;
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while performing autopay deletion functionality: " + ex.Message);
            }
            finally
            {
                foreach (string loan in usedLoanTestData)
                {
                    if (!string.IsNullOrEmpty(result))
                        result += ", ";
                    result += loan;
                }
                int count = usedLoanTestData.Count;
                test.Log(Status.Info, "LOAN NUMBERS USED - " + result + ". Count = " + count + ".");

                result = string.Empty;
                foreach (string loan in loansHavingSuccessfullAutopayDeletions)
                {
                    if (!string.IsNullOrEmpty(result))
                        result += ", ";
                    result += loan;
                }
                count = loansHavingSuccessfullAutopayDeletions.Count;
                test.Log(Status.Info, "AUTOPAY DELETION SUCCESSFULL FOR THE LOAN NUMBERS - " + result + ". Count = " + count + ". Retry - " + retryCount + ".");
            }
        }

        #endregion TestScriptsWithoutTestcaseID

        #region SanityTestScripts

        [TestMethod]
        [Description("<br> TC-3062 - Setup/Edit/Delete Autopay with the test loan <br>" +
            "TC-1315 - Test-Verify user is taken to Inhouse to setup a new autopay plan when there is no Autopay plan <br>" +
            "TC-1311 - Test-Verify user is taken to Inhouse Manage Autopay given Autopay plan active at Inhouse <br>")]
        [TestCategory("AP_Sanity")]
        public void TC_3062_1315_1311_TC_SetupEditDeleteAutopayForALoan()
        {
            #region TestData

            int retryCount = 0;
            string loanNumber = string.Empty, borrowerName = string.Empty, nextPaymentDueDate = string.Empty, paymentDateToBeSelected = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
            loanLevelData = commonServices.GetLoanDataFromDatabase(autopayEligibleLoanDetailsQuery, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(loanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForAutopay())
                        break;
                    else
                        test.Log(Status.Info, "The loan number is not eligible for setting up autopay");
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                {
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    Assert.Fail();
                }
            }
            loanNumber = loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
            borrowerName = loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerFirstName].ToString() + " " + loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerLastName].ToString();
            nextPaymentDueDate = loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString();
            List<string> pendingPaymentDates = _driver.FindElements(cpDashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();

            dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
            webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
            webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData[retryCount]);

            webElementExtensions.WaitForElement(_driver, payments.autopayStatusLocBy);
            bool flag = webElementExtensions.VerifyElementText(_driver, payments.autopayStatusLocBy, "Off");
            _driver.ReportResult(test, flag, "Successfully verified that Autopay Status is 'OFF' for the selected loan.",
                                             "Failure - Autopay Status is not 'OFF' for the selected loan.");
            webElementExtensions.ScrollIntoView(_driver, payments.autopayStatusLocBy);
            reportLogger.TakeScreenshot(test, "Autopay Status");
            webElementExtensions.WaitForElement(_driver, payments.manageAutopayButtonLocBy);
            webElementExtensions.ActionClick(_driver, payments.manageAutopayButtonLocBy, "Manage Autopay button.");

            commonServices.SetupAutopay(null, pendingPaymentDates, borrowerName);
            webElementExtensions.WaitForElement(_driver, payments.goBackToPaymentsLinkLocBy);
            webElementExtensions.ActionClick(_driver, payments.goBackToPaymentsLinkLocBy, "Back to Payments page link", false, true);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, payments.manageAutopayButtonLocBy);
            webElementExtensions.ActionClick(_driver, payments.manageAutopayButtonLocBy, "Manage Autopay button", false, true);
            commonServices.EditAutopay(null, null, borrowerName, true, "1");
            commonServices.DeleteAutopayFromAgentPortal(loanNumber, true);
        }
                
        [TestMethod]
        [Description("<br> TC-3075 - Verify header details displayed on the landing page when Loan is opened<br>")]
        [TestCategory("AP_Sanity")]
        public void TC_3075_TC_VerifyHeaderDetailsForDifferentLoanTypes()
        {
            commonServices.LoginToTheApplication(username, password);

            #region TestData

            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = typeof(Constants.LoanLevelDataColumns)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string)field.GetValue(null))
            .ToList();
            int retryCount;
            string loanNumber = string.Empty, borrowerName = string.Empty;

            #endregion TestData

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Perform 'Make a payment' for loan type : " + loanType.Value + " ******</b>");

                retryCount = 0;
                if (loanType.Key == "0")
                {
                    loanDetailsQuery = loanLevelDetailsForDifferentLoanTypes.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else if (loanType.Key == "Prepaid")
                {
                    loanDetailsQuery = loanLevelDetailsForDifferentLoanTypes.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                }
                else if (loanType.Key == "3")
                {
                    loanDetailsQuery = loanLevelDetailsForDifferentLoanTypes.Replace("DELINQUENTCOUNT", ">2");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }
                else
                {
                    loanDetailsQuery = loanLevelDetailsForDifferentLoanTypes.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }

                listOfLoanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQuery, null, columnDataRequired, usedLoanTestData, 1, true);
                ReportingMethods.Log(test, "Query - " + loanDetailsQuery);
                if (listOfLoanLevelData.Count > 0)
                {
                    loanNumber = listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                    borrowerName = listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerFirstName].ToString() + " " + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerLastName].ToString();
                    webElementExtensions.EnterText(_driver, dashboard.enterLoanNumberInputFieldLocBy, loanNumber, true, "Loan Number search", true);
                    webElementExtensions.ScrollIntoView(_driver, dashboard.verifyCallerButtonLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.verifyCallerButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, dashboard.viewAccountButtonLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.verifyCallerButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);

                    //Close the Verify caller pop up
                    webElementExtensions.ClickElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy, "Verify Caller pop up Close icon", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);
                    webElementExtensions.WaitForElement(_driver, dashboard.backToSearchButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, dashboard.loanSummaryHeaderLabelLocBy);

                    payments.VerifyPageOrSectionDisplayed(Constants.SectionNames.LoanSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(listOfLoanLevelData[retryCount], true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    webElementExtensions.ActionClick(_driver, dashboard.backToSearchButtonLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.enterLoanNumberInputFieldLocBy);
                    _driver.Navigate().Refresh();
                }
                else
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data for the given query.");
            }
        }

        [TestMethod]
        [Description("<br> TC-3061 - Setup/Edit/Delete OTP with the test loan <br> " +
                     "TC-3073 - Verify Payment Summary tab <br>")]
        [TestCategory("AP_Sanity")]
        public void TC_3061_3073_TC_SetupEditDeleteOtpForAPastDueLoan()
        {
            #region TestData

            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = typeof(Constants.LoanLevelDataColumns)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string)field.GetValue(null))
            .ToList();
            int retryCount;
            Hashtable loanLevelData = new Hashtable();
            string loanNumber = string.Empty, borrowerName = string.Empty;

            #endregion TestData

            ReportingMethods.Log(test, "<b>****** Perform 'One time payment' functionality ******</b>");

            retryCount = 0;
            loanDetailsQuery = loanLevelDetailsForDifferentLoanTypes.Replace("DELINQUENTCOUNT", "=1");
            loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");

            listOfLoanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQuery, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);

            commonServices.LoginToTheApplication(username, password);
            ReportingMethods.Log(test, "Query - " + loanDetailsQuery);
            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && listOfLoanLevelData.Count > retryCount)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForOTP())
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= listOfLoanLevelData.Count)
                {
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    Assert.Fail();
                }
            }
            loanLevelData = listOfLoanLevelData[retryCount];

            borrowerName = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
               loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
               $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);

            // Verification 1 - Make a payment button is Purple in color
            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected OTP loan.",
                                             "Failure - 'Make a Payment' button is not Purple in color for the selected OTP loan.");
            reportLogger.TakeScreenshot(test, "Make a Payment' button");

            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
            payments.VerifyOTPScreenDetails(loanLevelData, Constants.DefaultsPlan.OTP);
            payments.VerifyBannerDetails(loanLevelData);
            payments.SelectValueInAuthorizedByDropdown(borrowerName);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
            string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00);

            // Verification 3 - Verify the Payment dates in the one time payment screen when the user is having OTP plan and due is current month and current date is before cut off time
            string workingDate = commonServices.GetWorkingDate();
            payments.SelectPaymentDateInDateField(workingDate);
            payments.VerifyPaymentCutoffTimeMessage();
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

            // Verification 8 - Verify the edit payment page when loan has OTP Plan
            payments.EditNewlyAddedPayment(confirmationNumber);
            payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
            payments.VerifyPaymentBreakdownSectionNotVisible(Constants.PaymentFlowType.Edit);
            payments.VerifyOTPScreenDetails(loanLevelData, Constants.Plans.Active, Constants.PaymentFlowType.Edit);
            payments.VerifyBannerDetails(loanLevelData, Constants.Plans.Empty, Constants.PaymentFlowType.Edit);
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
            // Verification 9 - Verify Update button is disabled when there is no update made and on clicking go back to Payments user is navigated to Payments angular tab
            payments.VerifyUpdatePaymentButtonIsDisabled(Constants.PaymentFlowType.Edit);
            totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00, 15.00, 20.00, Constants.PaymentFlowType.Edit);
            workingDate = commonServices.GetWorkingDate(Constants.PaymentFlowType.Edit);
            payments.SelectPaymentDateInDateField(workingDate);
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
        [Description("<br> TC-3060 - Loan Summary verification <br> ")]
        [TestCategory("AP_Sanity")]
        public void TC_3060_TC_LoanSummaryVerification()
        {
            #region TestData

            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = typeof(Constants.LoanLevelDataColumns)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string)field.GetValue(null))
            .ToList();
            int retryCount;
            Hashtable loanLevelData = new Hashtable();
            string loanNumber = string.Empty, borrowerName = string.Empty;

            #endregion TestData

            ReportingMethods.Log(test, "<b>****** Loan Summary verification ******</b>");

            retryCount = 0;
            listOfLoanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForOtp, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);

            commonServices.LoginToTheApplication(username, password);
            ReportingMethods.Log(test, "Query - " + loanDetailsQuery);
            if (listOfLoanLevelData.Count > 0)
            {
                loanNumber = listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                borrowerName = listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerFirstName].ToString() + " " + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerLastName].ToString();
                webElementExtensions.EnterText(_driver, dashboard.enterLoanNumberInputFieldLocBy, loanNumber, true, "Loan Number search", true);
                webElementExtensions.ScrollIntoView(_driver, dashboard.verifyCallerButtonLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.verifyCallerButtonLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.viewAccountButtonLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.verifyCallerButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);

                //Close the Verify caller pop up
                webElementExtensions.ClickElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy, "Verify Caller pop up Close icon", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.backToSearchButtonLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.loanSummaryHeaderLabelLocBy);

                payments.VerifyPageOrSectionDisplayed(Constants.SectionNames.LoanSummary);
                string propertyFullAddress = listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyAddress].ToString() + ", "
                                + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyCity].ToString() + ", "
                                + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyState].ToString() + " "
                                + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString();
                List<string> loanSummaryDetails = new List<string>() { "Loan Number|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString() + "",
                "Type of Mortgage|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.ProductType].ToString() + "",
                "Property Address|" + propertyFullAddress + "",
                "Interest Rate|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.InterestRates].ToString() + "",
                "Investor (INV) Code|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.InvestorId].ToString() + "",
                "Principal & Interest|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount].ToString() + "",
                "Taxes & Insurance|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString() + "",
                "Total Monthly PITI|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.TotalMonthlyPayment].ToString() + "",
                "Escrow Balance|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.EscrowBalance].ToString() + "",
                "Suspense Balance|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.SuspenseBalance].ToString() + "",
                "Res Escrow Balance|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.RestrictedEscrowBalance].ToString() + "",
                "Corp Adv Balance|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.RecoverableCorpAdvBalance].ToString() + ""};
                dashboard.VerifyLoanSummaryDetails(loanSummaryDetails);
            }
        }

        [TestMethod]
        [Description("<br> TC-2846 - Verify Tools & Resources links <br> ")]
        [TestCategory("AP_Sanity")]
        public void TC_2846_TC_VerifyToolsAndResourcesLinks()
        {
            #region TestData

            int count = 0;
            string menuName = string.Empty;
            By locator = null;
            List<string> menuAndHyperlink = new List<string>() { "MSP|" + Constants.ToolsAndResourcesLinks.MSP,
            "Mello Assist|" + Constants.ToolsAndResourcesLinks.MelloAssist,
            "Workday|" + Constants.ToolsAndResourcesLinks.Workday,
            "Proctor|" + Constants.ToolsAndResourcesLinks.Proctor,
            "AIQ|" + Constants.ToolsAndResourcesLinks.AIQ};

            #endregion TestData

            ReportingMethods.Log(test, "<b>****** Verify Tools & Resources links ******</b>");

            commonServices.LoginToTheApplication(username, password);
            webElementExtensions.WaitForElement(_driver, dashboard.enterLoanNumberInputFieldLocBy);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
            webElementExtensions.ScrollToTop(_driver);
            webElementExtensions.IsElementDisplayedBasedOnCount(_driver, dashboard.toolsAndResourcesHeaderLocBy, "Tools & Resources header");
            List<IWebElement> toolsAndResourcesMenu = _driver.FindElements(dashboard.toolsAndResourcesMenuListLocBy).ToList();
            foreach (IWebElement element in toolsAndResourcesMenu)
            {
                menuName = webElementExtensions.GetElementText(_driver, By.XPath(dashboard.toolsAndResourcesMenuItemLocBy.Replace("<MENUNUMBER>", (count + 2).ToString())));
                ReportingMethods.LogAssertionEqual(test, menuAndHyperlink[count].Split('|')[0], menuName, "Menu");
                ReportingMethods.LogAssertionEqual(test, menuAndHyperlink[count].Split('|')[1], webElementExtensions.GetElementAttribute(_driver, By.XPath(dashboard.toolsAndResourcesMenuListHyperlinkLocBy.Replace("<MENUNAME>", menuName)), Constants.ElementAttributes.Href), "Hyperlink");
                webElementExtensions.ClickElement(_driver, By.XPath(dashboard.toolsAndResourcesMenuListHyperlinkLocBy.Replace("<MENUNAME>", menuName)));
                switch (menuName)
                {
                    case "MSP":
                        locator = commonServices.mspUsernameLocBy;
                        break;
                    case "Mello Assist":
                    case "Workday":
                        locator = dashboard.melloLogoInWorkdayPageLocBy;
                        break;
                    case "Proctor":
                        locator = dashboard.menuBarInProctorPageLocBy;
                        break;
                    case "AIQ":
                        locator = dashboard.userNameFieldInAiqPageLocBy;
                        break;
                    default:
                        locator = null;
                        break;
                }
                webElementExtensions.SwitchTabs(_driver, 1);
                webElementExtensions.WaitForElement(_driver, locator);
                _driver.ReportResult(test, webElementExtensions.IsElementDisplayedBasedOnCount(_driver, locator), "Successfully verified a field locator in the " + menuName + " page.", "Failed verifying a field locator in the " + menuName + " page.");
                _driver.Close();
                webElementExtensions.SwitchToFirstTab(_driver);
                count++;
            }
        }

        [TestMethod]
        [Description("<br> TC-806 - Verify user is navigated to Make a Payment screen when clicks on confirm in confirmation popup for Active Trail loan <br>" +
            "TC-1381 - Verify user is navigated to Make a Payment screen when clicks on confirm in confirmation popup for Active Repayment loan <br>")]
        [TestCategory("AP_Sanity")]
        public void TC_806_1381_TC_VerifyMakeAPaymentAlertPopupForTrialForbearanceRepaymentLoans()
        {
            commonServices.LoginToTheApplication(username, password);

            #region TestData

            List<string> queryList = new List<string>() { modificationTrialLoanDetailsQuery + "|Active Trial Plan",
                modTrialChangesInProgressLoanDetailsQuery + "|Modification System Changes in Progress",
                getLoanLevelDetailsForActiveForbearance + "|Active Forbearance Plan",
                getLoanLevelDetailsForActiveRepayment + "|Active Repayment Plan"};
            List<string> usedLoanTestData = new List<string>();
            List<string> columnDataRequired = typeof(Constants.LoanLevelDataColumns)
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(field => field.FieldType == typeof(string))
                    .Select(field => (string)field.GetValue(null)).ToList();
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);

            #endregion TestData

            foreach (string query in queryList)
            {
                ReportingMethods.Log(test, "<b>****** Query : " + query + " ******</b>");
                loanLevelData = commonServices.GetLoanDataFromDatabase(query.Split('|')[0], null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (loanLevelData.Count > 0)
                {
                    commonServices.LaunchUrlWithLoanNumber(loanLevelData, 0, true);
                    dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
                    webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData[0]);

                    //Make a payment button is Orange in color            
                    bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Orange, payments.makeAPaymentButtonLocBy);
                    _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Orange' in color for the selected loan.",
                                                     "Failure - 'Make a Payment' button is not Orange in color for the selected loan.");
                    reportLogger.TakeScreenshot(test, "'Make a Payment' button");

                    //Make a Payment confirmation pop up is displayed on clicking Make a Payment button
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentButtonLocBy, "Make a Payment button");
                    webElementExtensions.WaitForElement(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy);
                    flag = webElementExtensions.IsElementDisplayed(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy) &&
                        webElementExtensions.IsElementDisplayed(_driver, payments.makeAPaymentConfirmationPopupConfirmButtonLocBy);
                    _driver.ReportResult(test, true, "Successfully verified that 'Make a Payment' confirmation pop up is displayed.",
                                                     "Failure - 'Make a Payment' confirmation pop up is not displayed.");
                    reportLogger.TakeScreenshot(test, "'Make a Payment' confirmation pop up");

                    ReportingMethods.LogAssertionEqual(test, query.Split('|')[1] + ". Click 'Confirm' to continue.", webElementExtensions.GetElementText(_driver, payments.makeAPaymentConfirmationPopupTextLocBy), "Verify Make A Payment Button Alert pop up text");

                    //User is navigated back to Payments tab after closing 'Make a Payment' confirmation pop up.
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy);

                    if (webElementExtensions.IsElementDisplayed(_driver, payments.manageAutopayButtonLocBy))
                        _driver.ReportResult(test, true, "Successfully verified that user is on Payments tab after closing 'Make a Payment' confirmation pop up.", "");
                    else
                        _driver.ReportResult(test, false, "", "Failure - User is not routed back to Payments tab after closing 'Make a Payment' confirmation pop up.");
                    reportLogger.TakeScreenshot(test, "Payments page");

                    //OTP setup screen opens after clicking on Confirm button in 'Make a Payment' confirmation pop up.
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentConfirmationPopupConfirmButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.paymentDatePickerIconLocBy);
                    if (webElementExtensions.IsElementDisplayed(_driver, payments.authorizedByDropdownLocBy))
                        _driver.ReportResult(test, true, "Successfully verified that user is on OTP page after clicking on Confirm button in 'Make a Payment' confirmation pop up.", "");
                    else
                        _driver.ReportResult(test, false, "", "Failure - User is not navigated to OTP page after clicking on Confirm button in 'Make a Payment' confirmation pop up.");
                    reportLogger.TakeScreenshot(test, "OTP page");
                }
                else
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data for the given query.");
            }
        }

        [TestMethod]
        [Description("<br> TC-959 - Verify Total Reinstatement field is not displayed in OTP Setup screen for System changes in progress <br>")]
        [TestCategory("AP_Sanity")]
        public void TC_959_TC_VerifyTotalReinstatementFieldNotDisplayedForSystemChangesInProgressLoan()
        {
            commonServices.LoginToTheApplication(username, password);

            #region TestData

            List<string> queryList = new List<string>() { modTrialChangesInProgressLoanDetailsQuery + "|Modification System Changes in Progress" };
            List<string> usedLoanTestData = new List<string>();
            List<string> columnDataRequired = typeof(Constants.LoanLevelDataColumns)
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(field => field.FieldType == typeof(string))
                    .Select(field => (string)field.GetValue(null)).ToList();
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);

            #endregion TestData

            foreach (string query in queryList)
            {
                ReportingMethods.Log(test, "<b>****** Query : " + query + " ******</b>");
                loanLevelData = commonServices.GetLoanDataFromDatabase(query.Split('|')[0], null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
                if (loanLevelData.Count > 0)
                {
                    commonServices.LaunchUrlWithLoanNumber(loanLevelData, 0, true);
                    dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
                    webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData[0]);

                    //Make a payment button is Orange in color            
                    bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Orange, payments.makeAPaymentButtonLocBy);
                    _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Orange' in color for the selected loan.",
                                                     "Failure - 'Make a Payment' button is not Orange in color for the selected loan.");
                    reportLogger.TakeScreenshot(test, "'Make a Payment' button");

                    //Make a Payment confirmation pop up is displayed on clicking Make a Payment button
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentButtonLocBy, "Make a Payment button");
                    webElementExtensions.WaitForElement(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy);
                    flag = webElementExtensions.IsElementDisplayed(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy) &&
                        webElementExtensions.IsElementDisplayed(_driver, payments.makeAPaymentConfirmationPopupConfirmButtonLocBy);
                    _driver.ReportResult(test, true, "Successfully verified that 'Make a Payment' confirmation pop up is displayed.",
                                                     "Failure - 'Make a Payment' confirmation pop up is not displayed.");
                    reportLogger.TakeScreenshot(test, "'Make a Payment' confirmation pop up");

                    //OTP setup screen opens after clicking on Confirm button in 'Make a Payment' confirmation pop up.
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentConfirmationPopupConfirmButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, payments.paymentDatePickerIconLocBy);

                    //Total Reinstatement field is not displayed in OTP setup screen
                    By amountSectionInOtpSetupScreenLocBy = By.XPath(commonServices.amountSectionInOtpSetupScreen.Replace("<AMOUNTSECTIONFIELDNAME>", "Total Reinstatement Amount"));
                    flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, amountSectionInOtpSetupScreenLocBy);
                    _driver.ReportResult(test, flag, "Successfully verified that Total Reinstatement field is not displayed in OTP setup screen amount section.", "Total Reinstatement field is displayed in OTP setup screen amount section.");

                    reportLogger.TakeScreenshot(test, "OTP page");
                }
                else
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data for the given query.");
            }
        }

        [TestMethod]
        [Description("<br> TC-863[Payment processed verification not covered as part of sanity] - AP_OTP_PrePaid_Setup_payment <br>" +
            "TC-865 - AP_OTP_PrePaid_ManualCancel_payment <br>")]
        [TestCategory("AP_Sanity")]
        public void TC_863_865_TC_SetupAndDeleteOtpForOneMonthPrepaidLoan()
        {
            #region TestData

            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = typeof(Constants.LoanLevelDataColumns)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string)field.GetValue(null))
            .ToList();
            int retryCount;
            Hashtable loanLevelData = new Hashtable();
            string loanNumber = string.Empty, borrowerName = string.Empty, prepaidAmount = string.Empty, totalPayment = string.Empty, paperlessMessage = string.Empty;

            #endregion TestData

            ReportingMethods.Log(test, "<b>****** Perform 'One time payment' functionality ******</b>");

            retryCount = 0;
            loanDetailsQuery = loanLevelDetailsForDifferentLoanTypes.Replace("DELINQUENTCOUNT", "=0");
            loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date >= DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) and next_payment_due_date < DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+3,0)");

            listOfLoanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQuery, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);

            commonServices.LoginToTheApplication(username, password);
            ReportingMethods.Log(test, "Query - " + loanDetailsQuery);
            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && listOfLoanLevelData.Count > retryCount)
            {
                if (dashboard.QuickSearchForLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForOTP())
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= listOfLoanLevelData.Count)
                {
                    test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    Assert.Fail();
                }
                webElementExtensions.ActionClick(_driver, dashboard.backToSearchButtonLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.enterLoanNumberInputFieldLocBy);
            }
            loanLevelData = listOfLoanLevelData[retryCount];

            borrowerName = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
               loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
               $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);

            // Verification 1 - Make a payment button is Purple in color
            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected loan.",
                                             "Failure - 'Make a Payment' button is not Purple in color for the selected loan.");
            reportLogger.TakeScreenshot(test, "Make a Payment' button");

            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
            payments.VerifyOTPScreenDetails(loanLevelData, Constants.DefaultsPlan.OTP);
            payments.VerifyBannerDetails(loanLevelData);
            payments.SelectValueInAuthorizedByDropdown(borrowerName);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);

            if (_driver.FindElements(payments.prepayCheckboxLocBy).Count > 0)//Prepaid
            {
                flag = Convert.ToBoolean(webElementExtensions.GetElementAttribute(_driver, payments.prepayCheckboxLocBy, Constants.ElementAttributes.AriaChecked));
                if (flag == false)
                {
                    ReportingMethods.LogAssertionFalse(test, flag, "Verify initially Prepay Check Box is not checked");
                    webElementExtensions.ScrollIntoView(_driver, payments.prepayCheckboxLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, payments.prepayCheckboxLocBy, "Prepay Check Box");
                    prepaidAmount = webElementExtensions.GetElementText(_driver, payments.prePaymentCheckBoxAmountLocBy);
                }
            }
            totalPayment = payments.GetAmountText(payments.divTotalAmountLocBy);
            ReportingMethods.LogAssertionTrue(test, prepaidAmount == totalPayment, "Verify Prepay amount is equal to Total payment amount: Prepay amount - " + prepaidAmount + ", Total amount - " + totalPayment + ".");

            string workingDate = commonServices.GetWorkingDate();
            payments.SelectPaymentDateInDateField(workingDate);
            payments.VerifyPaymentCutoffTimeMessage();
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyReviewAndConfirmationPageIsDisplayed();
            payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
            payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment);
            flag = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(payments.paraByText.Replace("<TEXT>", Constants.Messages.OptedForPaperLess)));
            if (flag)
                paperlessMessage = webElementExtensions.GetElementText(_driver, By.XPath(payments.paraByText.Replace("<TEXT>", Constants.Messages.OptedForPaperLess)), true);
            _driver.ReportResult(test, flag, "Successfully verified that paperless email opt in message with email id is displayed.",
                                             "Failure - Paperless email opt in message with email id is not displayed");
            payments.ClickEditAndEnterMail(Constants.DeleteAutopayCredentials.Email);//Generic email to be used for email testing
            reportLogger.TakeScreenshot(test, "Opted for Paperless");
            payments.ClickConfirmButtonPaymentReviewPage();
            DateTime dateTimeUtc = DateTime.UtcNow;
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
            string confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
            payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
            dashboard.VerifyMessagesInNotesSectionOfDashboardPage(dateTimeUtc, "add", confirmationNumber, workingDate, totalPayment, accountNumber, username);

            if (ConfigSettings.PaymentsDataDeletionRequired)
            {
                payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                payments.VerifyPaymentDeletionMessageIsDisplayed();
                payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
                payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
                payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
                dashboard.VerifyMessagesInNotesSectionOfDashboardPage(dateTimeUtc, "delete", confirmationNumber, null, totalPayment, null, username, borrowerName, deleteReason);
            }
        }

        [TestMethod]
        [Description("<br> TC-2848 - Verify Quick Search and Advanced Search <br> ")]
        [TestCategory("AP_Sanity")]
        public void TC_2848_TC_VerifyQuickSearchAndAdvancedSearch()
        {
            #region TestData

            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = typeof(Constants.LoanLevelDataColumns)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string)field.GetValue(null))
            .ToList();
            int retryCount;
            Hashtable loanLevelData = new Hashtable();
            string loanNumber = string.Empty, borrowerName = string.Empty;

            #endregion TestData

            ReportingMethods.Log(test, "<b>****** Quick Search verification ******</b>");

            retryCount = 0;
            listOfLoanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForOtp, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);

            commonServices.LoginToTheApplication(username, password);
            ReportingMethods.Log(test, "Query - " + loanDetailsQuery);
            if (listOfLoanLevelData.Count > 0)
            {
                loanNumber = listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                borrowerName = listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerFirstName].ToString() + " " + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerLastName].ToString();
                
                //Quick Search
                webElementExtensions.EnterText(_driver, dashboard.enterLoanNumberInputFieldLocBy, loanNumber, true, "Loan Number search", true);
                webElementExtensions.ScrollIntoView(_driver, dashboard.verifyCallerButtonLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.verifyCallerButtonLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.viewAccountButtonLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.verifyCallerButtonLocBy, "Verify Caller button", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy, "Verify Caller pop up Close icon", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.backToSearchButtonLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.loanSummaryHeaderLabelLocBy);
                payments.VerifyPageOrSectionDisplayed(Constants.SectionNames.LoanSummary);
                webElementExtensions.ActionClick(_driver, dashboard.backToSearchButtonLocBy, "Back to loan search button", false, true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.enterLoanNumberInputFieldLocBy);

                ReportingMethods.Log(test, "<b>****** Advanced Search verification ******</b>");
                //Advanced Search
                _driver.ReportResult(test, webElementExtensions.IsElementDisplayedBasedOnCount(_driver, dashboard.searchForCustomerButtonDisabledLocBy), 
                    "Successfully verified that Search for Customer button is disabled if data is not entered in any of the fields",
                    "Failure - Search for Customer button is enabled even if data is not entered in any of the fields");

                webElementExtensions.EnterText(_driver, dashboard.streetAddress_PropertyAddInAdvSearchLocBy, listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyAddress].ToString(), true, "Street address field of Advanced search property address section ", true);
                _driver.ReportResult(test, webElementExtensions.IsElementDisplayedBasedOnCount(_driver, dashboard.searchForCustomerButtonLocBy),
                    "Successfully verified that Search for Customer button is enabled after entering data in any of the fields",
                    "Failure - Search for Customer button is disabled even after entering data in any of the fields");

                webElementExtensions.ActionClick(_driver, dashboard.searchForCustomerButtonLocBy, "Search for Customer button", false, true);
                webElementExtensions.WaitForElement(_driver, dashboard.viewAccountButtonInSearchResultsPopupLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanNumberInSearchResultsPopupLocBy);
                string loanNumberInSearchResultsPopup = webElementExtensions.GetElementText(_driver, dashboard.loanNumberInSearchResultsPopupLocBy).Split(' ')[1];
                ReportingMethods.LogAssertionEqual(test, loanNumber, loanNumberInSearchResultsPopup, "Verify that loan number from database for the searched property address and the loan number shown in the advanced search results pop up is same");

                webElementExtensions.ActionClick(_driver, dashboard.viewAccountButtonInSearchResultsPopupLocBy, "View Account button in Search results pop up", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.backToSearchButtonLocBy);
                webElementExtensions.WaitForElement(_driver, dashboard.loanSummaryHeaderLabelLocBy);
                payments.VerifyPageOrSectionDisplayed(Constants.SectionNames.LoanSummary);

                string propertyFullAddress = listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyAddress].ToString() + ", "
                                + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyCity].ToString() + ", "
                                + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyState].ToString() + " "
                                + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString();
                List<string> loanSummaryDetails = new List<string>() { "Loan Number|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString() + "",
                "Type of Mortgage|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.ProductType].ToString() + "",
                "Property Address|" + propertyFullAddress + "",
                "Interest Rate|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.InterestRates].ToString() + "",
                "Investor (INV) Code|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.InvestorId].ToString() + "",
                "Principal & Interest|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount].ToString() + "",
                "Taxes & Insurance|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString() + "",
                "Total Monthly PITI|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.TotalMonthlyPayment].ToString() + "",
                "Escrow Balance|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.EscrowBalance].ToString() + "",
                "Suspense Balance|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.SuspenseBalance].ToString() + "",
                "Res Escrow Balance|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.RestrictedEscrowBalance].ToString() + "",
                "Corp Adv Balance|" + listOfLoanLevelData[retryCount][Constants.LoanLevelDataColumns.RecoverableCorpAdvBalance].ToString() + ""};
                dashboard.VerifyLoanSummaryDetails(loanSummaryDetails);
            }
        }

        #endregion SanityTestScripts
    }
}
