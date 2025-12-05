using AventStack.ExtentReports;
using LD_AgentPortal.Pages;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static LD_AutomationFramework.Constants;

namespace LD_AgentPortal.Tests
{
    [TestClass]
    public class OTPTests : BasePage
    {
        public static string loanDetailsQuery;

        string getLoanLevelDetailsForSuspenseWithProcessStopCode = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPSuspenseWithProcessStopCode));
        string getLoanLevelDetailsForSuspenseWithProcessStopCodeNot = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPSuspenseWithProcessStopCodeNot));
        string getLoanLevelDetailsForOTPSuspenseWithProcessStopCode1B9 = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPSuspenseWithProcessStopCode1B9));
        string getLoanLevelDetailsForOTPSuspenseWithProcessStopCodeNot1B9 = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPSuspenseWithProcessStopCodeNot1B9));
        string getLoanLevelDetailsForOTPSuspenseGreaterThanZero = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPSuspenseGreaterThanZero));
        string getLoanLevelDetailsForOTPSuspenseIsZero = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPSuspenseIsZero));
        string getLoanLevelDetailsForOTPSuspenseWithDelinquentPaymentCountAndProcessStopCode = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPSuspenseWithDelinquentPaymentCountAndProcessStopCode));
        string getLoanLevelDetailsForOTPSuspenseIsZeroAndProcessStopCodeELN = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPSuspenseIsZeroAndProcessStopCodeELN));
        string getLoanLevelDetailsForOTPWithProcessStopCode = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPWithProcessStopCode));
        string getLoanLevelDetailsForOTPConditionWithProcessStopCode = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPConditionWithProcessStopCode));
        string getLoanLevelDetailsForOTPWithBadCheckStopCode = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPWithBadCheckStopCode));
        string getLoanLevelDetailsForOTPNextPaymentDueDate = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPNextPaymentDueDate));
        string getLoanLevelDetailsForOTPMiscellaneousGreaterThanZero = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPMiscellaneousGreaterThanZero));
        string getLoanLevelDetailsForOTPTaxAndInsuranceGreaterThanZero = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPTaxAndInsuranceGreaterThanZero));
        string getLoanLevelDetailsForOTPWhenProcessStopCodeIsB = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPWhenProcessStopCodeIsB));
        string getLoanLevelDetailsForOTPForPaymentAdded = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPForPaymentAdded));
        string getLoanLovelDetailsForOTPWithFees = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLovelDetailsForOTPWithFees));
        string getLoanLevelDetailsForOTPCutOff = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOTPCutOff));
        string getE2EOTPOnTimeSetupEditPayment = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetE2EOTPOnTimeSetupEditPayment));
        string getE2EOTPPrepaidSetupEditPayment = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetE2EOTPPrepaidSetupEditPayment));
        string getE2EOTPPastdueSetupEditPayment = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetE2EOTPPastdueSetupEditPayment));
        string getE2EOTPDelinquentSetupEditPayment = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetE2EOTPDelinquentSetupEditPayment));
        string getE2EOTPPrimaryBorrowerIsOptedInForPaperless = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetE2EOTPPrimaryBorrowerIsOptedInForPaperless));
        string getE2EOTPPrimaryBorrowerOptedOutForPaperless = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetE2EOTPPrimaryBorrowerOptedOutForPaperless));
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
        string bankAccountNameAfterSelection = "AutoAccount (Loan Depot Bank-" + Constants.BankAccountData.BankAccountNumber.Substring(Constants.BankAccountData.BankAccountNumber.Length - 4) + ")";
        string email = "servicingtestautomation_msp@yopmail.com"; //Providing test account email for now until new id is created

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
        [Description("<br> TC-1841 [SCM-9366] - Test-FUNC - OTP Screen - Suspense Payment Checkbox When Process Stop Code = 1, B, 9")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_1841_TC_VerifySuspensePaymentNotDisplayedInOTPScreenWhenProcessStopCodeIs_1_B_9()
        {
            #region TestData

            int retryCount;
            string[] processStopCodeTypes = { "1", "B", "9" };

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (string processStopCodeType in processStopCodeTypes)
            {
                retryCount = 0;

                ReportingMethods.Log(test, "<b>****** Verify Suspense Payment Checkbox When Process Stop Code : " + processStopCodeType + " ******</b>");
                loanDetailsQuery = getLoanLevelDetailsForSuspenseWithProcessStopCode.Replace("PROCESSSTOPCODE", processStopCodeType);
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
                            test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    }
                    loanLevelData = listOfLoanLevelData[retryCount];

                    string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                       loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                       $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);


                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    bool flag = webElementExtensions.IsElementDisplayed(_driver, payments.suspensePaymentCheckboxLocBy);
                    _driver.ReportResult(test, flag == false,
                        "Successfully verified that 'Suspense Payment' checkbox is not displayed when Process Stop Code is " + processStopCodeType,
                        "Failure - 'Suspense Payment' checkbox is displayed when Process Stop Code is " + processStopCodeType);

                    flag = webElementExtensions.IsElementDisplayed(_driver, payments.addToSuspenseBalanceTextboxLocBy);
                    _driver.ReportResult(test, flag == false,
                        "Successfully verified that 'Add to Suspense Balance' Textbox is not displayed when Process Stop Code is " + processStopCodeType,
                        "Failure - 'Add to Suspense Balance' Textbox is displayed when Process Stop Code is " + processStopCodeType);
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-657 [SCM-6519]-Test-FUNC - OTP Screen - Suspense Payment Checkbox When Process Stop Code = 1, B, 9 - Prepaid<br>" +
                     "TC-2193 [SCM-8210]-Test-FUNC - OTP Screen - Suspense Payment Checkbox When Process Stop Code = 1, B, 9 - Ontime <br>" +
                     "TC-2190 [SCM-8511]-Test-FUNC - OTP Screen - Suspense Payment Checkbox When Process Stop Code = 1, B, 9 - Past Due <br>" +
                     "TC-2235 [SCM-8512]-Test-FUNC - OTP Screen - Suspense Payment Checkbox When Process Stop Code = 1, B, 9 - Delinquent <br>" +
                     "TC-2236 [SCM-8513]-Test-FUNC - OTP Screen - Suspense Payment Checkbox When Process Stop Code = 1, B, 9 - DelinquentPlus ")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_657_2193_2190_2235_2236_TC_VerifySuspensePaymentNotDisplayedInOTPScreenWhenProcessStopCodeIs_1_B_9()
        {
            #region TestData

            int retryCount;

            #endregion TestData


            commonServices.LoginToTheApplication(username, password);

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify Suspense Payment Checkbox When Process Stop Code = 1, B, 9 for loan : " + loanType.Value + " ******</b>");

                retryCount = 0;
                if (loanType.Key == "0")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseWithProcessStopCode1B9.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else if (loanType.Key == "Prepaid")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseWithProcessStopCode1B9.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                }
                else if (loanType.Key == "3")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseWithProcessStopCode1B9.Replace("DELINQUENTCOUNT", ">2");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }
                else
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseWithProcessStopCode1B9.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }

                listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery, true);

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
                        if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                            test.Log(Status.Info, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
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

                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    bool flag = webElementExtensions.IsElementDisplayed(_driver, payments.suspensePaymentCheckboxLocBy);
                    _driver.ReportResult(test, flag == false,
                        "Successfully verified that 'Suspense Payment' checkbox is not displayed when Process Stop Code = 1, B, 9 for loan : " + loanType.Value,
                        "Failure - 'Suspense Payment' checkbox is displayed when Process Stop Code = 1, B, 9 for loan : " + loanType.Value);

                    flag = webElementExtensions.IsElementDisplayed(_driver, payments.addToSuspenseBalanceTextboxLocBy);
                    _driver.ReportResult(test, flag == false,
                        "Successfully verified that 'Add to Suspense Balance' Textbox is not displayed when Process Stop Code = 1, B, 9 for loan : " + loanType.Value,
                        "Failure - 'Add to Suspense Balance' Textbox is displayed when Process Stop Code = 1, B, 9 for loan : " + loanType.Value);
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }

            }
        }

        [TestMethod]
        [Description("<br>TC-1835 [SCM-9367] - Test-FUNC - OTP Screen - Suspense Payment Checkbox When Process Stop Code is not 1, B, 9")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_1835_TC_VerifySuspensePaymentDisplayedInOTPScreenWhenProcessStopCodeIsNot_1_B_9()
        {
            #region TestData

            int retryCount;
            string[] processStopCodeTypes = { "E", "Y", "G", "3", "H", "Q", "C", "7", "0", "D", "O", "I", "T" };

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (string processStopCodeType in processStopCodeTypes)
            {
                retryCount = 0;
                ReportingMethods.Log(test, "<b>****** Verify Suspense Payment Checkbox When Process Stop Code : " + processStopCodeType + " ******</b>");
                loanDetailsQuery = getLoanLevelDetailsForSuspenseWithProcessStopCode.Replace("PROCESSSTOPCODE", processStopCodeType);

                listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery, true);

                if (listOfLoanLevelData.Count > 0)
                {
                    ReportingMethods.Log(test, "Query - " + loanDetailsQuery);
                    while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
                    {
                        if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                        {
                            webElementExtensions.WaitForElement(_driver, By.XPath(dashboard.apNavigationTabsLocBy.Replace("<TABNAME>", Constants.AgentPortalTabNames.PaymentsTab)));
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

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);

                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    bool flag = webElementExtensions.IsElementDisplayed(_driver, payments.suspensePaymentCheckboxLocBy);
                    _driver.ReportResult(test, flag == true,
                        "Successfully verified that 'Suspense Payment' checkbox button is displayed when Process Stop Code is " + processStopCodeType,
                        "Failure - 'Suspense Payment' checkbox button is not displayed when Process Stop Code is " + processStopCodeType);

                    webElementExtensions.ActionClick(_driver, payments.suspensePaymentCheckboxLocBy);

                    flag = webElementExtensions.IsElementDisplayed(_driver, payments.addToSuspenseBalanceTextboxLocBy);
                    if (flag)
                    {
                        _driver.ReportResult(test, flag == true,
                            "Successfully verified that 'Add to Suspense Balance' Textbox is displayed when Process Stop Code is " + processStopCodeType,
                            "Failure - 'Add to Suspense Balance' Textbox is not displayed when Process Stop Code is " + processStopCodeType);
                    }

                    flag = webElementExtensions.IsElementDisplayed(_driver, payments.useSuspenseBalanceCheckboxLocBy);
                    if (flag)
                    {
                        _driver.ReportResult(test, flag == true,
                            "Successfully verified that 'Use Suspense Balance' Checkbox is displayed when Process Stop Code is " + processStopCodeType,
                            "Failure - 'Use Suspense Balance' Checkbox is not displayed when Process Stop Code is " + processStopCodeType);
                    }
                }
                else
                {
                    test.Log(Status.Warning, "Could not find the Loan level test data for Process Status code - " + processStopCodeType);
                }
            }
        }


        [TestMethod]
        [Description("<br>TC-658 [SCM-6520] - Test-FUNC - OTP Screen - Suspense Payment Checkbox When Process Stop Code is not 1, B, 9 - Prepaid<br>" +
                     "TC-2230 [SCM-8514]-Test-FUNC - OTP Screen - Suspense Payment Checkbox When Process Stop Code is not 1, B, 9 - Ontime <br>" +
                     "TC-2233 [SCM-8515]-Test-FUNC - OTP Screen - Suspense Payment Checkbox When Process Stop Code is not 1, B, 9 - Past Due <br>" +
                     "TC-2227 [SCM-8516]-Test-FUNC - OTP Screen - Suspense Payment Checkbox When Process Stop Code is not 1, B, 9 - Delinquent <br>" +
                     "TC-2228 [SCM-8517]-Test-FUNC - OTP Screen - Suspense Payment Checkbox When Process Stop Code is not 1, B, 9 - DelinquentPlus ")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_658_2230_2233_2227_2228_TC_VerifySuspensePaymentDisplayedInOTPScreenWhenProcessStopCodeIsNot_1_B_9()
        {
            #region TestData

            int retryCount;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify Suspense Payment Checkbox When Process Stop Code is not 1, B, 9 for loan : " + loanType.Value + " ******</b>");

                retryCount = 0;
                if (loanType.Key == "0")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseWithProcessStopCodeNot1B9.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else if (loanType.Key == "Prepaid")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseWithProcessStopCodeNot1B9.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                }
                else if (loanType.Key == "3")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseWithProcessStopCodeNot1B9.Replace("DELINQUENTCOUNT", ">2");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }
                else
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseWithProcessStopCodeNot1B9.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }

                ReportingMethods.Log(test, "Query - " + loanDetailsQuery);
                listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery, true);

                if (listOfLoanLevelData.Count > 0)
                {
                    while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
                    {
                        if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                        {
                            webElementExtensions.WaitForElement(_driver, By.XPath(dashboard.apNavigationTabsLocBy.Replace("<TABNAME>", Constants.AgentPortalTabNames.PaymentsTab)));
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

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);

                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    bool flag = webElementExtensions.IsElementDisplayed(_driver, payments.suspensePaymentCheckboxLocBy);
                    _driver.ReportResult(test, flag == true,
                        "Successfully verified that 'Suspense Payment' checkbox button is displayed when  Process Stop Code is not 1, B, 9 for loan : " + loanType.Value,
                        "Failure - 'Suspense Payment' checkbox button is not displayed when Process Stop Code is not 1, B, 9 for loan : " + loanType.Value);

                    webElementExtensions.ActionClick(_driver, payments.suspensePaymentCheckboxLocBy);

                    flag = webElementExtensions.IsElementDisplayed(_driver, payments.addToSuspenseBalanceTextboxLocBy);
                    if (flag)
                    {
                        _driver.ReportResult(test, flag == true,
                            "Successfully verified that 'Add to Suspense Balance' Textbox is displayed when  Process Stop Code is not 1, B, 9 for loan : " + loanType.Value,
                            "Failure - 'Add to Suspense Balance' Textbox is not displayed when  Process Stop Code is not 1, B, 9 for loan : " + loanType.Value);
                    }

                    flag = webElementExtensions.IsElementDisplayed(_driver, payments.useSuspenseBalanceCheckboxLocBy);
                    if (flag)
                    {
                        _driver.ReportResult(test, flag == true,
                            "Successfully verified that 'Use Suspense Balance' Checkbox is displayed when  Process Stop Code is not 1, B, 9 for loan : " + loanType.Value,
                            "Failure - 'Use Suspense Balance' Checkbox is not displayed when  Process Stop Code is not 1, B, 9 for loan : " + loanType.Value);
                    }
                }
                else
                {
                    test.Log(Status.Warning, "Could not find the Loan level test data for  Process Stop Code is not 1, B, 9 for loan : " + loanType.Value);
                }
            }
        }


        [TestMethod]
        [Description("<br>TC-669 [SCM-6507] -Test-FUNC - OTP Screen - Use Suspense Balance Field when Suspense Balance is 0 - Prepaid <br>" +
                     "TC-2258 [SCM-8522]-Test-FUNC - OTP Screen - Use Suspense Balance Field when Suspense Balance is 0 - Ontime <br>" +
                     "TC-2253 [SCM-8523]-Test-FUNC - OTP Screen - Use Suspense Balance Field when Suspense Balance is 0 - Past Due <br>" +
                     "TC-2254 [SCM-8524]-Test-FUNC - OTP Screen - Use Suspense Balance Field when Suspense Balance is 0 - Delinquent <br>" +
                     "TC-2248 [SCM-8525]-Test-FUNC - OTP Screen - Use Suspense Balance Field when Suspense Balance is 0 - DelinquentPlus <br>" +
                     "TC-605  [SCM-6882] - Test - Verify Past Due field is not selected by default for Delinquent loans when suspense balance = 0 <br>" +
                     "TC-595  [SCM-6894] - Test - Verify total payment when amount is entered in add to suspense balance field | Prepaid <br>" +
                     "TC-1711 [SCM-8739] - Test - Verify total payment when amount is entered in add to suspense balance field | Ontime <br>" +
                     "TC-1701 [SCM-8740] - Test - Verify total payment when amount is entered in add to suspense balance field | Past Due <br>" +
                     "TC-1697 [SCM-8741] - Test - Verify total payment when amount is entered in add to suspense balance field | Delinquent <br>" +
                     "TC-1694 [SCM-8742] - Test - Verify total payment when amount is entered in add to suspense balance field | DelinquentPlus <br>" +
                     "TC-597  [SCM-6895] - Test - Verify OTP submitted successfully when add to suspense balance field is entered | Prepaid <br>" +
                     "TC-2405 [SCM-8743] - Test - Verify OTP submitted successfully when add to suspense balance field is entered | Ontime <br>" +
                     "TC-2402 [SCM-8744] - Test - Verify OTP submitted successfully when add to suspense balance field is entered | Past Due <br>" +
                     "TC-2396 [SCM-8745] - Test - Verify OTP submitted successfully when add to suspense balance field is entered | Delinquent <br>" +
                     "TC-2395 [SCM-8746] - Test - Verify OTP submitted successfully when add to suspense balance field is entered | DelinquentPlus <br>" +
                     "TC-600  [SCM-6892] - Test - Verify Add to Suspense balance field is displayed when Suspense balance = 0 | Prepaid <br>" +
                     "TC-1675 [SCM-8731] - Test - Verify Add to Suspense balance field is displayed when Suspense balance = 0 | Ontime <br>" +
                     "TC-1671 [SCM-8732] - Test - Verify Add to Suspense balance field is displayed when Suspense balance = 0 | Past Due <br>" +
                     "TC-1667 [SCM-8733] - Test - Verify Add to Suspense balance field is displayed when Suspense balance = 0 | Delinquent <br>" +
                     "TC-1665 [SCM-8734] - Test - Verify Add to Suspense balance field is displayed when Suspense balance = 0 | DelinquentPlus")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_669_2258_2253_2254_2248_605_595_1711_1701_1697_1694_597_2405_2402_2396_2395_600_1675_1671_1667_1665_TC_VerifySuspensePaymentCheckboxAndAddToSuspenseBalanceFieldInOTPSubmitWhenSuspenseBalanceIs0()
        {

            #region TestData

            int retryCount;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify Suspense Payment checkbox and Add to Suspense Balance Field in OTP Submit when Suspense Balance is 0 for loan : " + loanType.Value + " ******</b>");
                try
                {
                    retryCount = 0;
                    if (loanType.Key == "0")
                    {
                        loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseIsZero.Replace("DELINQUENTCOUNT", "=0");
                        loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                    }
                    else if (loanType.Key == "Prepaid")
                    {
                        loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseIsZero.Replace("DELINQUENTCOUNT", "=0");
                        loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                    }
                    else if (loanType.Key == "3")
                    {
                        loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseIsZero.Replace("DELINQUENTCOUNT", ">2");
                        loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                    }
                    else
                    {
                        loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseIsZero.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                        loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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

                        payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                        payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                        payments.DeleteAllExistingPendingPayments(deleteReason);
                        payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                        By locBy = By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm));
                        if (webElementExtensions.IsElementDisplayed(_driver, locBy))
                            webElementExtensions.ScrollIntoView(_driver, locBy);
                            payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                        payments.VerifyOTPPageIsDisplayed();
                        payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                        payments.SelectValueInAuthorizedByDropdown(borrower);
                        commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                        string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00);
                        totalPayment = payments.VerifySuspenseAmountCalculation(loanLevelData, totalPayment, 10.00);
                        string workingDate = commonServices.GetWorkingDate();
                        payments.SelectPaymentDateInDateField(workingDate);
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
                catch (Exception ex)
                {
                    ReportingMethods.Log(test, "Exception: " + ex.Message);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-694 [SCM-6521] - Test-FUNC - OTP Screen - Suspense Payment Checkbox When Suspense balance > 0 - Prepaid <br>" +
                     "TC-2222 [SCM-8518] - Test-FUNC - OTP Screen - Suspense Payment Checkbox When Suspense balance > 0 - Ontime <br>" +
                     "TC-2225 [SCM-8519] - Test-FUNC - OTP Screen - Suspense Payment Checkbox When Suspense balance > 0 - Past Due <br>" +
                     "TC-2220 [SCM-8520] - Test-FUNC - OTP Screen - Suspense Payment Checkbox When Suspense balance > 0 - Delinquent <br>" +
                     "TC-2221 [SCM-8521] - Test-FUNC - OTP Screen - Suspense Payment Checkbox When Suspense balance > 0 - DelinquentPlus <br>" +
                     "TC-662 [SCM-6508] - Test-FUNC - OTP Screen -verify Use Suspense Balance Field when Suspense Balance is >0 - Prepaid <br>" +
                     "TC-2250 [SCM-8527] - Test-FUNC - OTP Screen -verify Use Suspense Balance Field when Suspense Balance is >0 - Ontime <br>" +
                     "TC-2245 [SCM-8528] - Test-FUNC - OTP Screen -verify Use Suspense Balance Field when Suspense Balance is >0 - Past Due <br>" +
                     "TC-2246 [SCM-8529] - Test-FUNC - OTP Screen -verify Use Suspense Balance Field when Suspense Balance is >0 - Delinquent <br>" +
                     "TC-2242 [SCM-8530] - Test-FUNC - OTP Screen -verify Use Suspense Balance Field when Suspense Balance is >0 - DelinquentPlus <br>" +
                     "TC-661 [SCM-6511] - Test-FUNC - OTP Screen - Use Suspense Balance Field when OTP is successfully submitted - Prepaid <br>" +
                     "TC-2132 [SCM-8646] - Test-FUNC - OTP Screen - Use Suspense Balance Field when OTP is successfully submitted - Ontime <br>" +
                     "TC-2126 [SCM-8647] - Test-FUNC - OTP Screen - Use Suspense Balance Field when OTP is successfully submitted - Past Due <br>" +
                     "TC-2160 [SCM-8648] - Test-FUNC - OTP Screen - Use Suspense Balance Field when OTP is successfully submitted - Delinquent <br>" +
                     "TC-2157 [SCM-8649] - Test-FUNC - OTP Screen - Use Suspense Balance Field when OTP is successfully submitted - DelinquentPlus <br>" +
                     "TC-602 [SCM-6893] - Test - Verify Add to Suspense balance field is not displayed when Suspense balance > 0 | Prepaid <br>" +
                     "TC-1688 [SCM-8735] - Test - Verify Add to Suspense balance field is not displayed when Suspense balance > 0 | Ontime <br>" +
                     "TC-1687 [SCM-8736] - Test - Verify Add to Suspense balance field is not displayed when Suspense balance > 0 | Past Due <br>" +
                     "TC-1680 [SCM-8737] - Test - Verify Add to Suspense balance field is not displayed when Suspense balance > 0 | Delinquent <br>" +
                     "TC-1678 [SCM-8738] - Test - Verify Add to Suspense balance field is not displayed when Suspense balance > 0 | DelinquentPlus <br>")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_694_2222_2225_2220_2221_662_2250_2245_2246_2242_661_2132_2126_2160_2157_602_1688_1687_1680_1678_TC_VerifySuspensePaymentCheckboxAndUseSuspenseBalanceFieldInOTPSubmitWhenSuspenseBalanceGreaterThan0()
        {
            #region TestData

            int retryCount;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify Suspense Payment checkbox and Use Suspense Balance Checkbox OTP submit when Suspense Balance > 0 for loan : " + loanType.Value + " ******</b>");
                retryCount = 0;
                if (loanType.Key == "0")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseGreaterThanZero.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else if (loanType.Key == "Prepaid")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseGreaterThanZero.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                }
                else if (loanType.Key == "3")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseGreaterThanZero.Replace("DELINQUENTCOUNT", ">2");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }
                else
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseGreaterThanZero.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    payments.SelectValueInAuthorizedByDropdown(borrower);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                    string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00);
                    totalPayment = payments.VerifySuspenseAmountCalculation(loanLevelData, totalPayment);
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
        [Description("<br> TC-664 [SCM-6509] -Test-FUNC - OTP Screen - Use Suspense Balance Field when User selects checkbox for 'Use Suspense Balance' Prepaid <br>" +
                     "TC-2243 [SCM-8531] -Test-FUNC - OTP Screen - Use Suspense Balance Field when User selects checkbox for 'Use Suspense Balance' Ontime <br>" +
                     "TC-2240 [SCM-8532] -Test-FUNC - OTP Screen - Use Suspense Balance Field when User selects checkbox for 'Use Suspense Balance' Past Due <br>" +
                     "TC-2284 [SCM-8533] -Test-FUNC - OTP Screen - Use Suspense Balance Field when User selects checkbox for 'Use Suspense Balance' Delinquent <br>" +
                     "TC-2287 [SCM-8534] -Test-FUNC - OTP Screen - Use Suspense Balance Field when User selects checkbox for 'Use Suspense Balance' DelinquentPlus <br>")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_664_2243_2240_2284_2287_TC_Test_FUNC_OTPScreen_UseSuspenseBalanceFieldWhenUserSelectsCheckboxForUseSuspenseBalance()
        {
            #region TestData

            int retryCount;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "****** Verify Use Suspense Balance Field when User selects checkbox for 'Use Suspense Balance' for loan : " + loanType.Value + " ******");
                retryCount = 0;
                if (loanType.Key == "Prepaid")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseGreaterThanZero.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0)");
                }
                else if (loanType.Key == "0")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseGreaterThanZero.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date >= DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+0,0) and next_payment_due_date <= DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else if (loanType.Key == "3")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseGreaterThanZero.Replace("DELINQUENTCOUNT", ">2");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }
                else
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseGreaterThanZero.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                    if (webElementExtensions.IsElementDisplayed(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    payments.VerifySuspenseAmountCalculation(loanLevelData);
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-660 [SCM-6510]-Test-FUNC - OTP Screen - Use Suspense Balance Field when Loan is in active loss mitigation (process stop code L) - Prepaid <br>" +
                     "TC-2087 [SCM-8642]-Test-FUNC - OTP Screen - Use Suspense Balance Field when Loan is in active loss mitigation (process stop code L) - Ontime <br>" +
                     "TC-2090 [SCM-8643]-Test-FUNC - OTP Screen - Use Suspense Balance Field when Loan is in active loss mitigation (process stop code L) - Past Due <br>" +
                     "TC-2082 [SCM-8644]-Test-FUNC - OTP Screen - Use Suspense Balance Field when Loan is in active loss mitigation (process stop code L) - Delinquent <br>" +
                     "TC-2135 [SCM-8645]-Test-FUNC - OTP Screen - Use Suspense Balance Field when Loan is in active loss mitigation (process stop code L) - DelinquentPlus")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_660_2087_2090_2082_2135_TC_Test_FUNC_OTPScreen_UseSuspenseBalanceFieldWhenLoanIsInActiveLossMitigationWithProcessStopCodeL()
        {
            #region TestData

            int retryCount;


            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify Use Suspense Balance Field when User selects checkbox for 'Use Suspense Balance' for loan : " + loanType.Value + " ******</b>");
                retryCount = 0;
                loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseWithDelinquentPaymentCountAndProcessStopCode.Replace("PROCESSSTOPCODE", "L");
                if (loanType.Key == "Prepaid")
                {
                    loanDetailsQuery = loanDetailsQuery.Replace("DELINQUENTCOUNT", "0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0)");
                }
                else if (loanType.Key == "0")
                {
                    loanDetailsQuery = loanDetailsQuery.Replace("DELINQUENTCOUNT", "0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date >= DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+0,0) and next_payment_due_date <= DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else
                {
                    loanDetailsQuery = loanDetailsQuery.Replace("DELINQUENTCOUNT", loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    webElementExtensions.ScrollIntoView(_driver, payments.suspensePaymentCheckboxLocBy);
                    webElementExtensions.ActionClick(_driver, payments.suspensePaymentCheckboxLocBy, "Suspense Payment checkbox");
                    webElementExtensions.IsElementDisabled(_driver, payments.useSuspenseBalanceCheckboxLocBy, "Use Suspense Balance checkbox");
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-2299 [SCM-8747] - SCM-6265 - Test - Verify Add to suspense balance field is disabled when loan has Process Stop E or L or N | Ontime <br>" +
                     "TC-2296 [SCM-8748] - SCM-6265 - Test - Verify Add to suspense balance field is disabled when loan has Process Stop E or L or N | Past Due <br>" +
                     "TC-2291 [SCM-8749] - SCM-6265 - Test - Verify Add to suspense balance field is disabled when loan has Process Stop E or L or N | Delinquent <br>" +
                     "TC-2290 [SCM-8750] - SCM-6265 - Test - Verify Add to suspense balance field is disabled when loan has Process Stop E or L or N | DelinquentPlus <br>")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_2299_2296_2291_2290_TC_VerifyAddToSuspenseBalanceFieldIsDisabledWhenLoanHasProcessStopCode_E_L_N()
        {
            #region TestData

            int retryCount;

            Dictionary<string, string> loanTypes = new Dictionary<string, string>
            {
            { "0", "On-time" },
            { "1", "Past Due" },
            { "2", "Delinquent" },
            { "3", "Delinquent+" },
            };

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify Add to suspense balance field is disabled when loan has Process Stop E or L or N for loan : " + loanType.Value + " ******</b>");

                retryCount = 0;
                if (loanType.Key == "0")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseIsZeroAndProcessStopCodeELN.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else if (loanType.Key == "3")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseIsZeroAndProcessStopCodeELN.Replace("DELINQUENTCOUNT", ">2");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }
                else
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseIsZeroAndProcessStopCodeELN.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    payments.SelectValueInAuthorizedByDropdown(borrower);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                    bool flag = _driver.FindElements(payments.suspensePaymentCheckboxLocBy).Count > 0 ? true : false;
                    _driver.ReportResult(test, flag, "Successfully verified that 'Suspense Payment' checkbox is displayed ", "Failure - 'Suspense Payment' checkbox is not displayed");
                    webElementExtensions.ScrollIntoView(_driver, payments.suspensePaymentCheckboxLocBy);
                    webElementExtensions.ClickElement(_driver, payments.suspensePaymentCheckboxLocBy, "Suspense Payment checkbox", true);
                    flag = _driver.FindElements(payments.addToSuspenseBalanceDisabledTextboxLocBy).Count > 0 ? true : false;
                    _driver.ReportResult(test, flag, "Successfully verified that 'Suspense Balance' Textbox is disabled ", "Failure - 'Suspense Balance' Textbox is not disabled");
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }


        [TestMethod]
        [Description("<br> TC-2311 [SCM-8751] - SCM-6265 - Test - Validate Add to suspense balance field when loan has Process Stop other than E or L or N | Ontime <br>" +
                     "TC-2308 [SCM-8752] - SCM-6265 - Test - Validate Add to suspense balance field when loan has Process Stop other than E or L or N | Past Due <br>" +
                     "TC-2305 [SCM-8753] - SCM-6265 - Test - Validate Add to suspense balance field when loan has Process Stop other than E or L or N | Delinquent <br>")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_2311_2308_2305_TC_VerifyAddToSuspenseBalanceFieldWhenLoanHasProcessStopCodeOtherThan_E_L_N()
        {
            #region TestData

            int retryCount;

            Dictionary<string, string> loanTypes = new Dictionary<string, string>
            {
            { "0", "On-time" },
            { "1", "Past Due" },
            { "2", "Delinquent" }
            };

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify Add to suspense balance field when loan has Process Stop other than E or L or N for loan : " + loanType.Value + " ******</b>");

                retryCount = 0;
                if (loanType.Key == "0")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseIsZero.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseIsZero.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    payments.SelectValueInAuthorizedByDropdown(borrower);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                    bool flag = _driver.FindElements(payments.suspensePaymentCheckboxLocBy).Count > 0 ? true : false;
                    _driver.ReportResult(test, flag, "Successfully verified that 'Suspense Payment' checkbox is displayed ", "Failure - 'Suspense Payment' checkbox is not displayed");
                    webElementExtensions.ScrollIntoView(_driver, payments.suspensePaymentCheckboxLocBy);
                    webElementExtensions.ClickElement(_driver, payments.suspensePaymentCheckboxLocBy, "Suspense Payment checkbox", true);
                    flag = _driver.FindElements(payments.addToSuspenseBalanceDisabledTextboxLocBy).Count > 0 ? false : true;
                    _driver.ReportResult(test, flag, "Successfully verified that 'Suspense Balance' Textbox is enabled ", "Failure - 'Suspense Balance' Textbox is not enabled");
                    double maxSuspensePayment = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment]);
                    maxSuspensePayment = Math.Round(maxSuspensePayment / 2);
                    webElementExtensions.EnterText(_driver, payments.addToSuspenseBalanceTextboxLocBy, (maxSuspensePayment * 100).ToString());
                    ReportingMethods.Log(test, "Entered Add to suspense balance : $" + maxSuspensePayment);
                    flag = _driver.FindElements(payments.addToSuspenceBalanceErrorMsgLocBy).Count == 0 ? true : false;
                    _driver.ReportResult(test, flag, "Successfully verified that 'Maximum suspense payment allowed Error Message' is not displayed ", "Failure - 'Maximum suspense payment allowed Error Message' is displayed");
                    webElementExtensions.EnterText(_driver, payments.addToSuspenseBalanceTextboxLocBy, ((maxSuspensePayment * 100) + 1).ToString());
                    ReportingMethods.Log(test, "Entered Add to suspense balance : $" + (maxSuspensePayment + 1));
                    flag = _driver.FindElements(payments.addToSuspenceBalanceErrorMsgLocBy).Count > 0 ? true : false;
                    _driver.ReportResult(test, flag, "Successfully verified that 'Maximum suspense payment allowed Error Message' is displayed ", "Failure - 'Maximum suspense payment allowed Error Message' is not displayed");
                    string maxSuspensePaymentErrorMessage = webElementExtensions.GetElementText(_driver, payments.addToSuspenceBalanceErrorMsgLocBy).Trim();
                    ReportingMethods.LogAssertionEqual(test, "Maximum suspense payment allowed is $" + maxSuspensePayment.ToString("N"), maxSuspensePaymentErrorMessage, "Verify Maximum suspense payment allowed Error Message");
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-2071 [SCM-7551] -  Test - Verify use suspense balance is disabled for Prepaid/Ontime/Pastdue loans when suspense balance > 0" +
                 " and user unselects the monthly payment amount <br>")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_2071_TC_VerifyUseSuspenseBalanceDisabledWhenSuspenseBalanceGreaterThan0AndUserUnselectsMonthlyPaymentAmount()
        {
            #region TestData

            int retryCount;

            Dictionary<string, string> loanTypes = new Dictionary<string, string>{
                 { "Prepaid", "Prepaid" },
                 { "0", "On-time" },
                 { "1", "Past Due" }};

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify Use Suspense Balance Checkbox disabled when Suspense Balance > 0 and user unselects the monthly payment amount  for loan : " + loanType.Value + " ******</b>");
                retryCount = 0;

                if (loanType.Key == "Prepaid")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseGreaterThanZero.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                }
                else if (loanType.Key == "0")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseGreaterThanZero.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPSuspenseGreaterThanZero.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                    if (webElementExtensions.IsElementDisplayed(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    bool flag;
                    if (loanType.Key == "Prepaid")
                    {
                        webElementExtensions.ScrollIntoView(_driver, payments.prePaymentCheckBoxLocBy);
                        flag = Convert.ToBoolean(webElementExtensions.GetElementAttribute(_driver, payments.prePaymentCheckBoxLocBy, Constants.ElementAttributes.AriaChecked));
                        if (flag)
                        {
                            ReportingMethods.Log(test, "Intially Prepay Checkbox is checked");
                            webElementExtensions.ClickElementUsingJavascript(_driver, payments.prePaymentCheckBoxLocBy, "Prepay Checkbox", true);
                            ReportingMethods.Log(test, "Prepay Checkbox is unchecked");
                        }
                        else
                        {
                            ReportingMethods.Log(test, "Initially Prepay Checkbox is not checked");
                        }
                    }
                    else if (loanType.Key == "0")
                    {
                        webElementExtensions.ScrollIntoView(_driver, payments.currentPaymentCheckboxLocBy);
                        flag = Convert.ToBoolean(webElementExtensions.GetElementAttribute(_driver, payments.currentPaymentCheckboxLocBy, Constants.ElementAttributes.AriaChecked));
                        if (flag)
                        {
                            ReportingMethods.Log(test, "Intially Current Payment Checkbox is checked");
                            webElementExtensions.ClickElementUsingJavascript(_driver, payments.currentPaymentCheckboxLocBy, "Current Payment Checkbox", true);
                            ReportingMethods.Log(test, "Current Payment Checkbox is unchecked");
                        }
                        else
                        {
                            ReportingMethods.Log(test, "Intially Current Payment is not checked");
                        }
                    }
                    else
                    {
                        webElementExtensions.ScrollIntoView(_driver, payments.monthlyPaymentCheckboxLocBy);
                        flag = Convert.ToBoolean(webElementExtensions.GetElementAttribute(_driver, payments.monthlyPaymentCheckboxLocBy, Constants.ElementAttributes.AriaChecked));
                        if (flag)
                        {
                            ReportingMethods.Log(test, "Intially Monthly Payment (Past Due) Checkbox is checked");
                            webElementExtensions.ClickElementUsingJavascript(_driver, payments.monthlyPaymentCheckboxLocBy, "Monthly Payment (Past Due) Checkbox", true);
                            ReportingMethods.Log(test, "Monthly Payment (Past Due) Checkbox is unchecked");
                        }
                        else
                        {
                            ReportingMethods.Log(test, "Intially Monthly Payment (Past Due) is not checked");
                        }
                    }

                    webElementExtensions.ScrollIntoView(_driver, payments.suspensePaymentCheckboxLocBy);
                    _driver.ReportResult(test, _driver.FindElements(payments.suspensePaymentCheckboxLocBy).Count > 0 ? true : false,
                        "Successfully verified that 'Suspense Payment' checkbox is displayed ",
                        "Failure - 'Suspense Payment' checkbox is not displayed");
                    webElementExtensions.ClickElement(_driver, payments.suspensePaymentCheckboxLocBy, "Suspense Payment checkbox", true);
                    _driver.ReportResult(test, _driver.FindElements(payments.useSuspenseBalanceCheckboxLocBy).Count > 0 ? true : false,
                        "Successfully verified that 'Use Suspense Balance' Checkbox is displayed",
                        "Failure - 'Use Suspense Balance' Checkbox is not displayed");
                    payments.VerifyUseSuspenseBalanceIsNegative();
                    _driver.ReportResult(test, _driver.FindElements(payments.useSuspenseBalanceDisabledCheckboxLocBy).Count > 0 ? true : false,
                       "Successfully verified that 'Use Suspense Balance' checkbox is disabled",
                       "Failure - 'Use Suspense Balance' checkbox is not disabled", true);
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-1146 [SCM-3981]- Test Verify- Loan Indicator - Soft Process Stops")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_1146_TC_VerifyLoanIndicatorForSoftProcessStops()
        {
            #region TestData

            int retryCount;

            JObject agentPaymentRulesData = JObject.Parse(commonServices.GetSettingValueDetailsFromSettingsTable("Agent_Payment_Rules").FirstOrDefault()[Constants.SettingDataColumns.SettingValue].ToString());
            // Extract the "softProcessStopCodeTypes" array and select the "Code" values containing override error message as a string array
            string[] softProcessStopCodeTypes = agentPaymentRulesData["ProcessStopCodes"].Where(x => ((string)x["ErrorMessage"]).Contains("override")).Select(x => (string)x["Code"]).ToArray();

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (string processStopCodeType in softProcessStopCodeTypes)
            {
                retryCount = 0;

                ReportingMethods.Log(test, "<b>****** Verify Loan Indicator for Soft Process Stop Code : " + processStopCodeType + " ******</b>");
                loanDetailsQuery = getLoanLevelDetailsForOTPWithProcessStopCode.Replace("PROCESSSTOPCODE", processStopCodeType);
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
                            test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    }
                    loanLevelData = listOfLoanLevelData[retryCount];

                    string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                       loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                       $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";
                    payments.VerifyIndicatorMessage(loanLevelData, Constants.StopCodeTypes.Soft);
                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);

                    webElementExtensions.VerifyElementColor(Constants.Colors.Yellow, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)), null, "Verify Make A Payment Button color is Yellow");
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                    reportLogger.TakeScreenshot(test, "Soft Process Stop Code Alert");
                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);

                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-1141 [SCM-3982]-Test verify- Loan Indicator - Bad Check Stops")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_1141_TC_VerifyLoanIndicatorBadCheckStops()
        {
            #region TestData

            int retryCount;

            JObject agentPaymentRulesData = JObject.Parse(commonServices.GetSettingValueDetailsFromSettingsTable("Agent_Payment_Rules").FirstOrDefault()[Constants.SettingDataColumns.SettingValue].ToString());
            // Extract the "BadCheckStopCodes" array and select the "Code" values as a string array
            string[] badCheckStopCodeTypes = agentPaymentRulesData["BadCheckStopCodes"].Select(x => (string)x["Code"]).ToArray();

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (string processStopCodeType in badCheckStopCodeTypes)
            {
                retryCount = 0;

                ReportingMethods.Log(test, "<b>****** Verify Loan Indicator for Bad Check Stop : " + processStopCodeType + " ******</b>");
                loanDetailsQuery = getLoanLevelDetailsForOTPWithBadCheckStopCode.Replace("BADCHECKSTOPCODE", processStopCodeType);
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
                            if (!webElementExtensions.IsElementEnabled(_driver, payments.makeAPaymentButtonLocBy))
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
                    payments.VerifyIndicatorMessage(loanLevelData, Constants.StopCodeTypes.BadCheck);
                    webElementExtensions.VerifyElementColor(Constants.Colors.LightRed, payments.badCheckStopsIndicatorMsgLocBy, null, "Verify Bad Check Stops Banner Indicator is Red");
                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);
                    webElementExtensions.IsElementDisabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)), Constants.ButtonNames.MakeAPayment);
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }


        [TestMethod]
        [Description("<br> TC-3227 [SCM-3995]-Verify Make a payment button disabled for hard stop process stop codes")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_3227_TC_VerifyMakeAPaymentButtonDisabledForHardStopProcessStopCodes()
        {
            #region TestData

            int retryCount;

            JObject agentPaymentRulesData = JObject.Parse(commonServices.GetSettingValueDetailsFromSettingsTable("Agent_Payment_Rules").FirstOrDefault()[Constants.SettingDataColumns.SettingValue].ToString());
            // Extract the "hardProcessStopCodeTypes" array and select the "Code" values not containing override error message as a string array
            string[] hardProcessStopCodeTypes = agentPaymentRulesData["ProcessStopCodes"].Where(x => !((string)x["ErrorMessage"]).Contains("override")).Select(x => (string)x["Code"]).ToArray();


            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (string processStopCodeType in hardProcessStopCodeTypes)
            {
                retryCount = 0;

                ReportingMethods.Log(test, "<b>****** Verify Loan Indicator for Hard Process Stop Code : " + processStopCodeType + " ******</b>");
                loanDetailsQuery = getLoanLevelDetailsForOTPConditionWithProcessStopCode.Replace("PROCESSSTOPCODE", processStopCodeType);
                if (processStopCodeType == "P")
                {
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", " and pif_stop_code = 1 and is_active = '0' ");
                }
                else if (processStopCodeType == "9")
                {
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and is_active = '1' and repay_plan_status_code is null and delinquent_payment_count <= 2");
                }
                else
                {
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and is_active = '1' ");
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
                            if (!webElementExtensions.IsElementEnabled(_driver, payments.makeAPaymentButtonLocBy))
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
                    payments.VerifyIndicatorMessage(loanLevelData, Constants.StopCodeTypes.Hard);
                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);

                    webElementExtensions.IsElementDisabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)), Constants.ButtonNames.MakeAPayment);
                    reportLogger.TakeScreenshot(test, Constants.ButtonNames.MakeAPayment);
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-3230 [SCM-3996]-Verify Make a payment button enabled for no stop process stop codes")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_3230_TC_VerifyMakeAPaymentButtonEnabledForNoStopProcessStopCodes()
        {
            #region TestData

            int retryCount;
            string[] noStopProcessStopCodeTypes = { "E", "Y", "G", "Q", "C", "1", "7", "0", "O", "S", "I", "T" };

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (string processStopCodeType in noStopProcessStopCodeTypes)
            {
                retryCount = 0;

                ReportingMethods.Log(test, "<b>****** Verify Make a payment button enabled for No stop Process Stop Code : " + processStopCodeType + " ******</b>");
                loanDetailsQuery = getLoanLevelDetailsForOTPWithProcessStopCode.Replace("PROCESSSTOPCODE", processStopCodeType);
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
                            test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    }
                    if (listOfLoanLevelData.Count < retryCount + 1)
                    {
                        test.Log(Status.Warning, "Loan Level Data count : " + listOfLoanLevelData.Count + " is less than the accessing Retry Count :" + retryCount);
                        break;
                    }
                    loanLevelData = listOfLoanLevelData[retryCount];

                    string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                       loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                       $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";
                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);

                    webElementExtensions.VerifyElementColor(Constants.Colors.Purple, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)), null, "Verify Make A Payment Button color is Purple");
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);

                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-1788 [SCM-7622]-Test- Verify DelinquentPaymentBalance calculation if first day of the month falls on Weekend/Holiday/Monday <br>")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_1788_TC_VerifyDelinquentPaymentBalanceCalculationIfFirstDayOfTheMonthFallsOnWeekendHolidayMonday()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForOTPNextPaymentDueDate;
            loanDetailsQuery = getLoanLevelDetailsForOTPNextPaymentDueDate.Replace("NEXTPAYMENTDUEDATE", "DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())-1,0)");
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

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
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                }
                loanLevelData = listOfLoanLevelData[retryCount];

                string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                   loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                   $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                payments.DeleteAllExistingPendingPayments(deleteReason);

                bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
                _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected OTP loan.",
                                                 "Failure - 'Make a Payment' button is not Purple in color for the selected OTP loan.");
                reportLogger.TakeScreenshot(test, "Make a Payment' button");

                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyOTPPageIsDisplayed();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);

                payments.VerifyOTPScreenDetails(loanLevelData, Constants.DefaultsPlan.OTP);
                payments.VerifyBannerDetails(loanLevelData);
                payments.SelectValueInAuthorizedByDropdown(borrower);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                payments.VerifyDelinquentPaymentBalanceCalculationBasedOnStartOfMonthIsWeekendHoliday(loanLevelData);

            }
            else
            {
                test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
            }
        }

        [TestMethod]
        [Description("<br> TC-1780 [SCM-7623]-Test- Verify DelinquentPlus PaymentBalance calculation if first day of the month falls on Weekend/Holiday/Mon <br>")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_1780_TC_VerifyDelinquentPlusPaymentBalanceCalculationIfFirstDayOfTheMonthFallsOnWeekendHolidayMonday()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getLoanLevelDetailsForOTPNextPaymentDueDate;
            loanDetailsQuery = getLoanLevelDetailsForOTPNextPaymentDueDate.Replace("NEXTPAYMENTDUEDATE", "DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())-2,0)");
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

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
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                }
                loanLevelData = listOfLoanLevelData[retryCount];

                string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                   loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                   $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                payments.DeleteAllExistingPendingPayments(deleteReason);

                bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
                _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected OTP loan.",
                                                 "Failure - 'Make a Payment' button is not Purple in color for the selected OTP loan.");
                reportLogger.TakeScreenshot(test, "Make a Payment' button");

                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyOTPPageIsDisplayed();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);

                payments.VerifyOTPScreenDetails(loanLevelData, Constants.DefaultsPlan.OTP);
                payments.VerifyBannerDetails(loanLevelData);
                payments.SelectValueInAuthorizedByDropdown(borrower);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                payments.VerifyDelinquentPaymentBalanceCalculationBasedOnStartOfMonthIsWeekendHoliday(loanLevelData);
            }
            else
            {
                test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
            }
        }

        [TestMethod]
        [Description("<br> TC-2261 [SCM-7177]-Test-Verify Setup OTP - Amount displayed includes NJ Firefighter Fee when [miscellaneous_amount] > 0 <br>")]
        [TestCategory("AP_OTP")]
        public void TC_2261_TC_VerifySetupOTPAmountDisplayedIncludesNJFirefighterFeeWhenMiscellaneousAmountGreaterThan0()
        {
            #region TestData

            int retryCount;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify Setup OTP - Amount displayed includes NJ Firefighter Fee when [miscellaneous_amount] > 0  for loan : " + loanType.Value + " ******</b>");
                retryCount = 0;
                if (loanType.Key == "0")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPMiscellaneousGreaterThanZero.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else if (loanType.Key == "Prepaid")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPMiscellaneousGreaterThanZero.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                }
                else if (loanType.Key == "3")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPMiscellaneousGreaterThanZero.Replace("DELINQUENTCOUNT", ">2");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }
                else
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPMiscellaneousGreaterThanZero.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }
                loanDetailsQuery = loanDetailsQuery.Replace("GREATER_THEN", ">");
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

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    payments.SelectValueInAuthorizedByDropdown(borrower);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                    string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00);
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
        [Description("<br> TC-1804 [SCM-7776]-Verify Agents are able to schedule additional escrow only payments when the loan is past due <br>" +
                     "TC-1544 [SCM-7141] - Test - Verify Additional payment checkbox is enabled when the Monthly Payment (Past Due) checkbox is unchecked on the OTP setup/edit screen")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_1804_1544_TC_VerifyAgentsAbleToScheduleAdditionalEscrowOnlyPayments()
        {
            #region TestData

            int retryCount;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            Dictionary<string, string> loanTypes = new Dictionary<string, string> { { "Prepaid", "Prepaid" }, { "0", "On-time" }, { "1", "Past Due" } };
            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify Agents are able to schedule additional escrow only payments when the loan is : " + loanType.Value + " ******</b>");
                retryCount = 0;
                if (loanType.Key == "0")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPTaxAndInsuranceGreaterThanZero.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else if (loanType.Key == "Prepaid")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPTaxAndInsuranceGreaterThanZero.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                }
                else
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPTaxAndInsuranceGreaterThanZero.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }
                loanDetailsQuery = loanDetailsQuery.Replace("GREATER_THEN", ">");
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

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    payments.SelectValueInAuthorizedByDropdown(borrower);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                    string totalPayment = payments.MakeAdditionalEscrowPaymentOnly(loanType.Value, 10.00);
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

                    payments.EditNewlyAddedPayment(confirmationNumber);
                    payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
                    payments.VerifyOTPScreenDetails(loanLevelData, Constants.Plans.Completed, Constants.PaymentFlowType.Edit);
                    payments.VerifyBannerDetails(loanLevelData, Constants.Plans.Completed, Constants.PageNames.EditPayment);
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
                    payments.VerifyAuthorizedByIsDisabled();
                    totalPayment = payments.MakeAdditionalEscrowPaymentOnly(loanType.Value, 10.00, 15.00, Constants.PaymentFlowType.Edit);
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

        [TestMethod]
        [Description("<br> TC-1447 [SCM-5994] - Verify the indicator message on payments tab when the loan is in active bankruptcy <br>" +
                     "TC-1444 [SCM-5995] - Verify Make a payment button is enabled and displayed in orange color when the loan is in active bankruptcy <br>" +
                     "TC-1445 [SCM-5996] - Verify OTP screen displayed on clicking confirm in popup when loan is in active bankruptcy <br>" +
                     "TC-1152 [SCM-6018]-Test-Active Bankruptcy - OTP - Verify display message and Monthly Payment field validation <br>" +
                     "TC-1027 [SCM-6019]-Test-Active Bankruptcy - Submit OTP <br>")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_1447_1444_1445_1152_1027_TC_Test_ActiveBankruptcy_VerifyDisplayMessageAndMonthlyPaymentFieldValidationOTPSubmit()
        {
            #region TestData

            int retryCount;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify display message and Monthly Payment field validation when Process Stop Code is B for loan : " + loanType.Value + " ******</b>");

                retryCount = 0;
                if (loanType.Key == "0")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPWhenProcessStopCodeIsB.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else if (loanType.Key == "Prepaid")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPWhenProcessStopCodeIsB.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                }
                else if (loanType.Key == "3")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPWhenProcessStopCodeIsB.Replace("DELINQUENTCOUNT", ">2");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }
                else
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPWhenProcessStopCodeIsB.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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
                    payments.VerifyIndicatorMessage(loanLevelData, Constants.StopCodeTypes.Soft);
                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);

                    webElementExtensions.VerifyElementColor(Constants.Colors.Orange, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)), null, "Verify Make A Payment Button color is Orange");
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm)));
                    reportLogger.TakeScreenshot(test, "Active Bankruptcy Alert");
                    webElementExtensions.WaitForElement(_driver, payments.overridePopupMessageLocBy);
                    string actualProcessStopCodeBMessage = _driver.FindElement(payments.overridePopupMessageLocBy).Text;
                    ReportingMethods.LogAssertionEqual(test, Constants.Messages.ProcessStopCodeBOverrideMessage, actualProcessStopCodeBMessage.Trim().Replace("\r\n", " "), "Verify Process Stop Code 'B' popup Override message");
                    payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    payments.SelectValueInAuthorizedByDropdown(borrower);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.amountRadioButtonLocBy);
                    bool radiobuttonStatus = payments.IsRadiobuttonSelected(payments.amountRadioButtonLocBy);
                    if (radiobuttonStatus != true)
                    {
                        webElementExtensions.ScrollIntoView(_driver, payments.amountRadioButtonLocBy);
                        payments.ClickAmountMonthlyPaymentRadioButton();
                    }
                    double unPaidPrincipalBalanceAmount = (Convert.ToDouble(webElementExtensions.GetElementText(_driver, payments.unpaidPrincipalBalanceAmountLocBy).Replace("$", "")) + 1) * 100;
                    payments.EnterAmountMonthlyPaymentValue(unPaidPrincipalBalanceAmount);
                    By locBy = By.XPath(payments.spanContainsByText.Replace("<TEXT>", Constants.ErrorMessages.TotalPaymentAmountGreaterThanUnpaidPrincipalBalance));
                    webElementExtensions.WaitForElement(_driver, locBy);
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, locBy), "Verify 'Total payment amount is greater than unpaid principal balance. Please update or submit payoff request.' message is displayed");
                    unPaidPrincipalBalanceAmount = Convert.ToDouble(payments.GetAmountMonthlyPaymentValue().Replace("$", "")) * 100;
                    payments.EnterAmountMonthlyPaymentValue(unPaidPrincipalBalanceAmount);
                    string totalPayment = "$" + Convert.ToDouble(payments.GetAmountText(payments.divTotalAmountLocBy).Replace("$", "")).ToString("N");
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
                    if (ConfigSettings.PaymentsDataDeletionRequired)
                    {
                        if (loanType.Key != "0")
                        {
                            payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                            payments.VerifyPaymentDeletionMessageIsDisplayed();
                            payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
                            payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
                            payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
                        }
                    }

                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-1457 [SCM-5997] - Verify user is still on payments tab on clicking cancel in popup when loan is in active bankruptcy")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_1457_TC_Test_ActiveBankruptcy_OTP_VerifyUserStillOnPaymentsTabOnClickingCancelInPopupWhenLoanIsInActiveBankruptcy()
        {
            #region TestData

            int retryCount = 0;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            loanDetailsQuery = getLoanLevelDetailsForOTPWhenProcessStopCodeIsB.Replace("DELINQUENTCOUNT", "=0");
            loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
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
                payments.VerifyIndicatorMessage(loanLevelData, Constants.StopCodeTypes.Soft);
                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                payments.DeleteAllExistingPendingPayments(deleteReason);

                webElementExtensions.VerifyElementColor(Constants.Colors.Orange, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)), null, "Verify Make A Payment Button color is Orange");
                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy);
                reportLogger.TakeScreenshot(test, "Active Bankruptcy Alert");
                webElementExtensions.ClickElement(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy, "Cancel Button");
                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            }
            else
            {
                test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
            }
        }

        [TestMethod]
        [Description("<br> TC-1022 [SCM-6020]-Test-Active Bankruptcy - Verify user can Edit/Delete OTP before 8PM CST on the payment date")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_1022_TC_Test_VerifyUserCanEditAndDeleteOTPBefore8PMCSTOnPaymentDate()
        {
            #region TestData

            int retryCount;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify display message and Monthly Payment field validation when Process Stop Code is B for loan : " + loanType.Value + " ******</b>");

                retryCount = 0;
                if (loanType.Key == "0")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPWhenProcessStopCodeIsB.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                }
                else if (loanType.Key == "Prepaid")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPWhenProcessStopCodeIsB.Replace("DELINQUENTCOUNT", "=0");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                }
                else if (loanType.Key == "3")
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPWhenProcessStopCodeIsB.Replace("DELINQUENTCOUNT", ">2");
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                }
                else
                {
                    loanDetailsQuery = getLoanLevelDetailsForOTPWhenProcessStopCodeIsB.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                    loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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
                    payments.VerifyIndicatorMessage(loanLevelData, Constants.StopCodeTypes.Soft);
                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);

                    webElementExtensions.VerifyElementColor(Constants.Colors.Orange, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)), null, "Verify Make A Payment Button color is Orange");
                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                    reportLogger.TakeScreenshot(test, "Active Bankruptcy Alert");
                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    payments.SelectValueInAuthorizedByDropdown(borrower);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, payments.amountRadioButtonLocBy);
                    bool radiobuttonStatus = payments.IsRadiobuttonSelected(payments.amountRadioButtonLocBy);
                    if (radiobuttonStatus != true)
                    {
                        webElementExtensions.ScrollIntoView(_driver, payments.amountRadioButtonLocBy);
                        payments.ClickAmountMonthlyPaymentRadioButton();
                    }
                    double unPaidPrincipalBalanceAmount = (Convert.ToDouble(webElementExtensions.GetElementText(_driver, payments.unpaidPrincipalBalanceAmountLocBy).Replace("$", "")) + 1) * 100;
                    payments.EnterAmountMonthlyPaymentValue(unPaidPrincipalBalanceAmount);
                    By locBy = By.XPath(payments.spanContainsByText.Replace("<TEXT>", Constants.ErrorMessages.TotalPaymentAmountGreaterThanUnpaidPrincipalBalance));
                    webElementExtensions.WaitForElement(_driver, locBy);
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, locBy), "Verify 'Total payment amount is greater than unpaid principal balance. Please update or submit payoff request.' message is displayed");
                    unPaidPrincipalBalanceAmount = Convert.ToDouble(payments.GetAmountMonthlyPaymentValue().Replace("$", "")) * 100;
                    payments.EnterAmountMonthlyPaymentValue(unPaidPrincipalBalanceAmount);
                    string totalPayment = "$" + Convert.ToDouble(payments.GetAmountText(payments.divTotalAmountLocBy).Replace("$", "")).ToString("N");
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
                    payments.CloseNotesTab();
                    payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
                    bool flag = payments.VerifyUserCanEditAndDeleteOTPBasedOn8PMCSTCutOffTime(workingDate);
                    DateTime date = Convert.ToDateTime(workingDate);
                    DateTime currentTime = DateTime.UtcNow;
                    DateTime currentCSTTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZones.CentralStandardTime));
                    DateTime cutOffCSTTime = new DateTime(date.Year, date.Month, date.Day, 20, 00, 00);
                    // Check if the current time (in CST) is before 8:00 PM on the payment date
                    if (currentCSTTime < cutOffCSTTime)
                    {
                        ReportingMethods.LogAssertionTrue(test, flag, "Verify Edit and Delete Payment Buttons are Enabled because the current time is before 8PM CST of Payment Date " + workingDate);
                    }

                    if (flag)
                    {
                        payments.EditNewlyAddedPayment(confirmationNumber);
                        payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
                        payments.VerifyAuthorizedByIsDisabled();
                        totalPayment = payments.EnterAmountMonthlyPaymentValue(unPaidPrincipalBalanceAmount + 10);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
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
                            if (loanType.Key != "0")
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
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }
            }
        }
        [TestMethod]
        [Description("<br> TC-1023 [SCM-6021]-Test-Active Bankruptcy - Verify Edit/Delete OTP is disabled after 8PM CST on the payment date")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP"), TestCategory("AP_CutoffTime")]
        public void TC_1023_TC_Test_VerifyEditAndDeleteOTPDisabledAfter8PMCSTOnPaymentDate()
        {
            #region TestData

            int retryCount = 0;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            DateTime currentDate = DateTime.Now;
            List<DateTime> bankHoliDays = commonServices.GetBankHolidays(Convert.ToString(currentDate.Year));
            string workingDate = commonServices.GetWorkingDay(currentDate, bankHoliDays).ToString("dd MMM yyyy");
            loanDetailsQuery = getLoanLevelDetailsForOTPForPaymentAdded.Replace("CONDITION", " (CONVERT(VARCHAR(11),PaymentDate,113) = '" + workingDate + "' ");
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

                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                bool flag = payments.VerifyUserCanEditAndDeleteOTPBasedOn8PMCSTCutOffTime(workingDate);
                DateTime date = Convert.ToDateTime(workingDate);
                DateTime currentTime = DateTime.UtcNow;
                DateTime currentCSTTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZones.CentralStandardTime));
                DateTime cutOffCSTTime = new DateTime(date.Year, date.Month, date.Day, 20, 00, 00);
                // Check if the current time (in CST) is before 8:00 PM on the payment date
                if (currentCSTTime < cutOffCSTTime)
                {
                    ReportingMethods.LogAssertionTrue(test, flag, "Verify Edit and Delete Payment Buttons are Enabled because the current time is before 8PM CST of Payment Date " + workingDate);
                }
                else
                {
                    ReportingMethods.LogAssertionFalse(test, flag, "Verify Edit and Delete Payment Buttons are Disabled because the current time is after 8PM CST of Payment Date " + workingDate);
                }
            }
            else
            {
                test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
            }
        }

        [TestMethod]
        [Description("<br> TC-688 [SCM-6527]-Verify fee validations for Ontime loan[Setup OTP]" +
                     "TC-2202 [SCM-8502]-Verify fee validations for Ontime loan[Edit OTP]")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_688_2202_TC_Test_VerifyFeeValidationsForOntimeLoanSetupEditOTP()
        {
            #region TestData
            int retryCount;
            #endregion TestData
            commonServices.LoginToTheApplication(username, password);
            Dictionary<string, string> feeTypes = new Dictionary<string, string> { { "0", "Other Fee" }, { "1", "Late Fee" }, { "2", "NSF Fee" } };

            foreach (var fee in feeTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify fee validations for Ontime loan[Setup OTP] when " + fee.Value + " > '0' ******</b>");

                retryCount = 0;
                loanDetailsQuery = getLoanLovelDetailsForOTPWithFees.Replace("FEE", fee.Key);
                loanDetailsQuery = loanDetailsQuery.Replace("DELINQUENTCOUNT", "= 0");
                loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
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
                            test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    }
                    loanLevelData = listOfLoanLevelData[retryCount];

                    string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
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
                    payments.SelectValueInAuthorizedByDropdown(borrower);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                    // Verification 3 - Verify the Payment dates in the one time payment screen when the user is having OTP plan and due is current month and current date is before cut off time
                    string workingDate = commonServices.GetWorkingDate();
                    payments.SelectPaymentDateInDateField(workingDate);
                    string totalPayment = payments.VerifyOTPPaymentFieldsAndFeeValidations(loanLevelData, 10.00, 15.00);
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
                    workingDate = commonServices.GetWorkingDate(Constants.PaymentFlowType.Edit);
                    payments.SelectPaymentDateInDateField(workingDate);
                    totalPayment = payments.VerifyOTPPaymentFieldsAndFeeValidations(loanLevelData, 10.00, 15.00, 20.00, 30.00, Constants.PaymentFlowType.Edit);
                    payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
                    payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PaymentFlowType.Edit);
                    payments.VerifyTotalAmountPaymentReviewPage(totalPayment, Constants.PaymentFlowType.Edit);
                    payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment, Constants.PaymentFlowType.Edit);
                    payments.ClickConfirmButtonPaymentReviewPage();
                    payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PaymentFlowType.Edit);
                    confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
                    payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                    payments.VerifyPaymentDeletionMessageIsDisplayed();
                    payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
                    payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
                    payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }

            }
        }

        [TestMethod]
        [Description("<br> TC-691 [SCM-6528]-Verify fee validations for Past Due loan[Setup OTP]" +
                    "TC-2195 [SCM-8503]-Verify fee validations for Past Due loan[Edit OTP]")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_691_2195_TC_Test_VerifyFeeValidationsForPastDueLoanSetupEditOTP()
        {
            #region TestData
            int retryCount;
            #endregion TestData
            commonServices.LoginToTheApplication(username, password);
            Dictionary<string, string> feeTypes = new Dictionary<string, string> { { "0", "Other Fee" }, { "1", "Late Fee" }, { "2", "NSF Fee" } };

            foreach (var fee in feeTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify fee validations for Past Due loan[Setup OTP] when " + fee.Value + " > '0' ******</b>");

                retryCount = 0;
                loanDetailsQuery = getLoanLovelDetailsForOTPWithFees.Replace("FEE", fee.Key);
                loanDetailsQuery = loanDetailsQuery.Replace("DELINQUENTCOUNT", "= 1");
                loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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
                            test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    }
                    loanLevelData = listOfLoanLevelData[retryCount];

                    string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
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
                    payments.SelectValueInAuthorizedByDropdown(borrower);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                    // Verification 3 - Verify the Payment dates in the one time payment screen when the user is having OTP plan and due is current month and current date is before cut off time
                    string workingDate = commonServices.GetWorkingDate();
                    payments.SelectPaymentDateInDateField(workingDate);
                    string totalPayment = payments.VerifyOTPPaymentFieldsAndFeeValidations(loanLevelData, 10.00, 15.00);
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
                    workingDate = commonServices.GetWorkingDate(Constants.PaymentFlowType.Edit);
                    payments.SelectPaymentDateInDateField(workingDate);
                    totalPayment = payments.VerifyOTPPaymentFieldsAndFeeValidations(loanLevelData, 10.00, 15.00, 20.00, 30.00, Constants.PaymentFlowType.Edit);
                    payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
                    payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PaymentFlowType.Edit);
                    payments.VerifyTotalAmountPaymentReviewPage(totalPayment, Constants.PaymentFlowType.Edit);
                    payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment, Constants.PaymentFlowType.Edit);
                    payments.ClickConfirmButtonPaymentReviewPage();
                    payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PaymentFlowType.Edit);
                    confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
                    payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                    payments.VerifyPaymentDeletionMessageIsDisplayed();
                    payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
                    payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
                    payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }

            }
        }

        [TestMethod]
        [Description("<br> TC-684 [SCM-6529]-Verify fee validations for Delinquent loan[Setup OTP]" +
                     " TC-2198 [SCM-8504]-Verify fee validations for Delinquent loan[Edit OTP]")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_684_2198_TC_Test_VerifyFeeValidationsForDelinquentLoanSetupEditOTP()
        {
            #region TestData
            int retryCount;
            #endregion TestData
            commonServices.LoginToTheApplication(username, password);
            Dictionary<string, string> feeTypes = new Dictionary<string, string> { { "0", "Other Fee" }, { "1", "Late Fee" }, { "2", "NSF Fee" } };

            foreach (var fee in feeTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify fee validations for Delinquent loan[Setup OTP] when " + fee.Value + " > '0' ******</b>");

                retryCount = 0;
                loanDetailsQuery = getLoanLovelDetailsForOTPWithFees.Replace("FEE", fee.Key);
                loanDetailsQuery = loanDetailsQuery.Replace("DELINQUENTCOUNT", "= 2");
                loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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
                            test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    }
                    loanLevelData = listOfLoanLevelData[retryCount];

                    string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                       loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                       $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);

                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    payments.VerifyOTPScreenDetails(loanLevelData, Constants.DefaultsPlan.OTP);
                    payments.VerifyBannerDetails(loanLevelData);
                    payments.SelectValueInAuthorizedByDropdown(borrower);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                    // Verification 3 - Verify the Payment dates in the one time payment screen when the user is having OTP plan and due is current month and current date is before cut off time
                    string workingDate = commonServices.GetWorkingDate();
                    payments.SelectPaymentDateInDateField(workingDate);
                    string totalPayment = payments.VerifyOTPPaymentFieldsAndFeeValidations(loanLevelData, 10.00, 15.00);
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
                    workingDate = commonServices.GetWorkingDate(Constants.PaymentFlowType.Edit);
                    payments.SelectPaymentDateInDateField(workingDate);
                    totalPayment = payments.VerifyOTPPaymentFieldsAndFeeValidations(loanLevelData, 10.00, 15.00, 20.00, 30.00, Constants.PaymentFlowType.Edit);
                    payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
                    payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PaymentFlowType.Edit);
                    payments.VerifyTotalAmountPaymentReviewPage(totalPayment, Constants.PaymentFlowType.Edit);
                    payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment, Constants.PaymentFlowType.Edit);
                    payments.ClickConfirmButtonPaymentReviewPage();
                    payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PaymentFlowType.Edit);
                    confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
                    payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                    payments.VerifyPaymentDeletionMessageIsDisplayed();
                    payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
                    payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
                    payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }

            }
        }

        [TestMethod]
        [Description("<br> TC-687 [SCM-6530]-Verify fee validations for DelinquentPlus loan[Setup OTP] <br>" +
                     "TC-2192 [SCM-8505]-Verify fee validations for DelinquentPlus loan[Edit OTP]")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_687_2192_TC_Test_VerifyFeeValidationsForDelinquentPlusLoanSetupEditOTP()
        {
            #region TestData
            int retryCount;
            #endregion TestData
            commonServices.LoginToTheApplication(username, password);
            Dictionary<string, string> feeTypes = new Dictionary<string, string> { { "0", "Other Fee" }, { "1", "Late Fee" }, { "2", "NSF Fee" } };

            foreach (var fee in feeTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify fee validations for Delinquent Plus loan[Setup OTP] when " + fee.Value + " > '0' ******</b>");

                retryCount = 0;
                loanDetailsQuery = getLoanLovelDetailsForOTPWithFees.Replace("FEE", fee.Key);
                loanDetailsQuery = loanDetailsQuery.Replace("DELINQUENTCOUNT", "> 2");
                loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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
                            test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                    }
                    loanLevelData = listOfLoanLevelData[retryCount];

                    string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                       loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                       $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                    payments.DeleteAllExistingPendingPayments(deleteReason);

                    payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                    if (webElementExtensions.IsElementEnabled(_driver, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Confirm))))
                        payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);

                    payments.VerifyOTPPageIsDisplayed();
                    payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                    payments.VerifyOTPScreenDetails(loanLevelData, Constants.DefaultsPlan.OTP);
                    payments.VerifyBannerDetails(loanLevelData);
                    payments.SelectValueInAuthorizedByDropdown(borrower);
                    commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                    // Verification 3 - Verify the Payment dates in the one time payment screen when the user is having OTP plan and due is current month and current date is before cut off time
                    string workingDate = commonServices.GetWorkingDate();
                    payments.SelectPaymentDateInDateField(workingDate);
                    string totalPayment = payments.VerifyOTPPaymentFieldsAndFeeValidations(loanLevelData, 10.00, 15.00);
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
                    totalPayment = payments.VerifyOTPPaymentFieldsAndFeeValidations(loanLevelData, 10.00, 15.00, 20.00, 30.00, Constants.PaymentFlowType.Edit);
                    payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
                    payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PaymentFlowType.Edit);
                    payments.VerifyTotalAmountPaymentReviewPage(totalPayment, Constants.PaymentFlowType.Edit);
                    payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment, Constants.PaymentFlowType.Edit);
                    payments.ClickConfirmButtonPaymentReviewPage();
                    payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PaymentFlowType.Edit);
                    confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
                    payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
                    payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                    payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                    payments.VerifyPaymentDeletionMessageIsDisplayed();
                    payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
                    payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
                    payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
                }
                else
                {
                    test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
                }

            }
        }

        [TestMethod]
        [Description("<br> TC-5402 Verify the user is able to Edit existing OTP before cutoff time on Draft Date <br> " +
                     "TC-5404 Verify the user is able to Delete existing OTP before cutoff time on Draft Date")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP"), TestCategory("AP_CutoffTime")]
        public void TC_5402_5404_TC_Test_VerifyTheUserIsAbleToEditAndDeleteOTPBeforeCutoffTimeOnDraftDate()
        {
            #region TestData

            int retryCount;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            foreach (var loanType in loanTypes)
            {
                ReportingMethods.Log(test, "<b>****** Verify the user is able to Edit and Delete existing OTP before cutoff time on Draft Date : " + loanType.Value + " ******</b>");
                try
                {
                    retryCount = 0;
                    if (loanType.Key == "0")
                    {
                        loanDetailsQuery = getLoanLevelDetailsForOTPCutOff.Replace("DELINQUENTCOUNT", "=0");
                        loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
                    }
                    else if (loanType.Key == "Prepaid")
                    {
                        loanDetailsQuery = getLoanLevelDetailsForOTPCutOff.Replace("DELINQUENTCOUNT", "=0");
                        loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+2,0) ");
                    }
                    else if (loanType.Key == "3")
                    {
                        loanDetailsQuery = getLoanLevelDetailsForOTPCutOff.Replace("DELINQUENTCOUNT", ">2");
                        loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
                    }
                    else
                    {
                        loanDetailsQuery = getLoanLevelDetailsForOTPCutOff.Replace("DELINQUENTCOUNT", "=" + loanType.Key);
                        loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "");
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
                        payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                        payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                        payments.DeleteAllExistingPendingPayments(deleteReason);

                        webElementExtensions.VerifyElementColor(Constants.Colors.Purple, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)), null, "Verify Make A Payment Button color is Orange");
                        payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                        payments.VerifyOTPPageIsDisplayed();
                        payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                        payments.SelectValueInAuthorizedByDropdown(borrower);
                        commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                        string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00);
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
                        payments.CloseNotesTab();

                        payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
                        bool flag = payments.VerifyUserCanEditAndDeleteOTPBasedOn8PMCSTCutOffTime(workingDate);
                        DateTime date = Convert.ToDateTime(workingDate);
                        DateTime currentTime = DateTime.UtcNow;
                        DateTime currentCSTTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZones.CentralStandardTime));
                        DateTime cutOffCSTTime = new DateTime(date.Year, date.Month, date.Day, 20, 00, 00);
                        // Check if the current time (in CST) is before 8:00 PM on the payment date
                        if (currentCSTTime < cutOffCSTTime)
                        {
                            ReportingMethods.LogAssertionTrue(test, flag, "<b>Verify Edit and Delete Payment Buttons are Enabled because the current time is before 8PM CST of Payment Date " + workingDate + "<b>");
                        }

                        if (flag)
                        {
                            payments.EditNewlyAddedPayment(confirmationNumber);
                            payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
                            payments.VerifyAuthorizedByIsDisabled();
                            totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00, 15.00, 20.00, Constants.PaymentFlowType.Edit);
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
                catch (Exception ex)
                {
                    ReportingMethods.Log(test, "Exception: " + ex.Message);
                }
            }
        }

        [TestMethod]
        [Description("<br> TC-5403 Verify the user is not able to Edit existing OTP after cutoff time on Draft Date <br>" +
                     "TC-5405 Verify the user is not able to Delete existing OTP after cutoff time on Draft Date")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP"), TestCategory("AP_CutoffTime")]
        public void TC_5403_5405_TC_Test_VerifyTheUserIsNotAbleToEditAndDeleteExistingOTPAfterCutoffTimeOnDraftDate()
        {
            #region TestData

            int retryCount = 0;

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            DateTime currentDate = DateTime.Now;
            List<DateTime> bankHoliDays = commonServices.GetBankHolidays(Convert.ToString(currentDate.Year));
            string workingDate = commonServices.GetWorkingDay(currentDate, bankHoliDays).ToString("dd MMM yyyy");
            loanDetailsQuery = getLoanLevelDetailsForOTPForPaymentAdded.Replace("CONDITION", " (CONVERT(VARCHAR(11),PaymentDate,113) = '" + workingDate + "' ");
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
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                }
                loanLevelData = listOfLoanLevelData[retryCount];

                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                bool flag = payments.VerifyUserCanEditAndDeleteOTPBasedOn8PMCSTCutOffTime(workingDate);
                DateTime date = Convert.ToDateTime(workingDate);
                DateTime currentTime = DateTime.UtcNow;
                DateTime currentCSTTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZones.CentralStandardTime));
                DateTime cutOffCSTTime = new DateTime(date.Year, date.Month, date.Day, 20, 00, 00);
                // Check if the current time (in CST) is before 8:00 PM on the payment date
                if (currentCSTTime < cutOffCSTTime)
                {
                    ReportingMethods.LogAssertionTrue(test, flag, "Verify Edit and Delete Payment Buttons are Enabled because the current time is before 8PM CST of Payment Date " + workingDate);
                }
                else
                {
                    ReportingMethods.LogAssertionFalse(test, flag, "Verify Edit and Delete Payment Buttons are Disabled because the current time is after 8PM CST of Payment Date " + workingDate);
                }
                reportLogger.TakeScreenshot(test, "Edit and Delete buttons status");
            }
            else
            {
                test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
            }
        }

        [TestMethod]
        [Description("<br> TC-5410 Verify the user is able to setup payment for the same date before cut offtime 8 PM CST <br>" +
                     "TC-5411 Verify the user is not able to setup payment for the same date After cut offtime 8 PM CST")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP"), TestCategory("AP_CutoffTime")]
        public void TC_5410_5411_TC_Test_VerifySetupPaymentForSameDateBeforeAndAfterCutOfftime8PMCST()
        {
            commonServices.LoginToTheApplication(username, password);
            #region TestData

            int retryCount = 0;
            loanDetailsQuery = getLoanLevelDetailsForOTPCutOff.Replace("DELINQUENTCOUNT", "=0");
            loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery, true);

            #endregion TestData
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
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                }
                loanLevelData = listOfLoanLevelData[retryCount];

                string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                   loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                   $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";
                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                payments.DeleteAllExistingPendingPayments(deleteReason);

                webElementExtensions.VerifyElementColor(Constants.Colors.Purple, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)), null, "Verify Make A Payment Button color is Orange");
                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyOTPPageIsDisplayed();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                payments.SelectValueInAuthorizedByDropdown(borrower);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00);
                DateTime currentDate = DateTime.Now;
                List<DateTime> bankHoliDays = commonServices.GetBankHolidays(Convert.ToString(currentDate.Year));
                DateTime holidayCheckDate = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 00, 00, 00);
                if (currentDate.DayOfWeek.ToString().ToLower() == "saturday")
                {
                    ReportingMethods.Log(test, "Current day is 'Saturday'");
                }
                else if (currentDate.DayOfWeek.ToString().ToLower() == "sunday")
                {
                    ReportingMethods.Log(test, "Current day is 'Sunday'");
                }
                else if (bankHoliDays.Contains(holidayCheckDate))
                {
                    ReportingMethods.Log(test, "Current day is 'Bank Holiday'");
                }
                else
                {
                    DateTime currentTime = DateTime.UtcNow;
                    DateTime currentCSTTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZones.CentralStandardTime));
                    DateTime cutOffCSTTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 20, 00, 00);
                    webElementExtensions.ScrollIntoView(_driver, commonServices.paymentDatePickerIconLocBy);
                    if (_driver.FindElements(commonServices.monthYearSpanLocBy).Count == 0) { webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.paymentDatePickerIconLocBy); }
                    By paymentDateToBeSelectedLocBy = By.CssSelector(commonServices.paymentDateTobeSelectedIsEnabledOrDisabledLocBy.Replace("<DATETOBESELECTED>", currentDate.ToString("MMM d, yyyy")));
                    bool flag = Convert.ToBoolean(webElementExtensions.GetElementAttribute(_driver, paymentDateToBeSelectedLocBy, Constants.ElementAttributes.AriaDisabled));
                    reportLogger.TakeScreenshot(test, "Date picker");
                    // Check if the current time (in CST) is before 8:00 PM on the Current date
                    if (currentCSTTime < cutOffCSTTime)
                    {
                        ReportingMethods.LogAssertionFalse(test, flag, "Verify Same day Payment Date : " + currentDate + " is Enabled in Date picker before 8PM CST");
                        commonServices.SelectPaymentDateInDateField(currentDate.ToString("MMM d, yyyy"));
                        payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                        payments.VerifyReviewAndConfirmationPageIsDisplayed();
                        payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                        payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, currentDate.ToString("MMM d, yyyy"), totalPayment);
                        payments.ClickConfirmButtonPaymentReviewPage();
                        payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
                        string confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
                        payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
                        payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                        payments.CloseNotesTab();
                        payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                        payments.VerifyPaymentDeletionMessageIsDisplayed();
                        payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
                        payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
                        payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
                    }
                    else
                    {
                        ReportingMethods.LogAssertionTrue(test, flag, "Verify Same day Payment Date : " + currentDate + " is Disabled in Date picker after 8PM CST");
                        string workingDate = commonServices.GetWorkingDate();
                        ReportingMethods.Log(test, "The next avaliable working date : " + workingDate);
                    }
                }
            }
            else
            {
                test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
            }
        }

        [TestMethod]
        [Description("<br> TC-5406 Verify the user is able to Edit existing OTP setup at cutoff time 8 PM CST for future draft date <br>" +
         "TC-5407 Verify the user is able to Delete existing OTP setup at cutoff time 8 PM CST for future draft date <br>" +
         "TC-5408 Verify the user is able to Edit existing OTP setup at cutoff time 8 PM CST for future draft date <br>" +
         "TC-5409 Verify the user is able to Delete existing OTP setup at cutoff time 8 PM CST for future draft date")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP"), TestCategory("AP_CutoffTime")]
        public void TC_5406_5407_5408_5409_TC_Test_VerifyUserAbleToEditAndDeleteExistingOTPSetupBeforeAtAfterCutoffTime8PMCSTForFutureDraftDate()
        {
            commonServices.LoginToTheApplication(username, password);
            #region TestData

            int retryCount = 0;
            loanDetailsQuery = getLoanLevelDetailsForOTPCutOff.Replace("DELINQUENTCOUNT", "=0");
            loanDetailsQuery = loanDetailsQuery.Replace("CONDITION", "and next_payment_due_date = DATEADD(MONTH,DATEDIFF(MONTH,0,GETDATE())+1,0)");
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery, true);

            #endregion TestData
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
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                }
                loanLevelData = listOfLoanLevelData[retryCount];

                string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                   loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                   $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";
                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                payments.DeleteAllExistingPendingPayments(deleteReason);

                webElementExtensions.VerifyElementColor(Constants.Colors.Purple, By.XPath(payments.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)), null, "Verify Make A Payment Button color is Orange");
                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyOTPPageIsDisplayed();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                payments.SelectValueInAuthorizedByDropdown(borrower);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00);
                string workingDate = commonServices.GetWorkingDate(Constants.PaymentFlowType.SetUp, 1);
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
                payments.CloseNotesTab();
                bool flag = payments.VerifyUserCanEditAndDeleteOTPBasedOn8PMCSTCutOffTime(workingDate);
                DateTime date = Convert.ToDateTime(workingDate);
                DateTime currentTime = DateTime.UtcNow;
                DateTime currentCSTTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZones.CentralStandardTime));
                DateTime cutOffCSTTime = new DateTime(currentCSTTime.Year, currentCSTTime.Month, currentCSTTime.Day, 20, 00, 00);
                // Check if the current time (in CST) is before 8:00 PM on the payment date
                if (currentCSTTime < cutOffCSTTime)
                {
                    ReportingMethods.LogAssertionTrue(test, flag, "<b> Verify Edit and Delete Payment Buttons are Enabled for current time is before 8PM CST of Current Date " + currentCSTTime.ToString("MMMM d, yyyy") + " and Future Payment Date is " + workingDate + "</b>");
                }
                else if (currentCSTTime == cutOffCSTTime)
                {
                    ReportingMethods.LogAssertionTrue(test, flag, "<b> Verify Edit and Delete Payment Buttons are Enabled for current time at 8PM CST of Current Date " + currentCSTTime.ToString("MMMM d, yyyy") + " and Future Payment Date is " + workingDate + "</b>");
                }
                else
                {
                    ReportingMethods.LogAssertionTrue(test, flag, "<b> Verify Edit and Delete Payment Buttons are Enabled for current time is after 8PM CST of Current Date " + currentCSTTime.ToString("MMMM d, yyyy") + " and Future Payment Date is " + workingDate + "</b>");
                }
                reportLogger.TakeScreenshot(test, "Edit and Delete buttons status");

                payments.EditNewlyAddedPayment(confirmationNumber);
                payments.VerifyOTPPageIsDisplayed(Constants.PaymentFlowType.Edit);
                payments.VerifyAuthorizedByIsDisabled();
                workingDate = commonServices.GetWorkingDate(Constants.PaymentFlowType.Edit, 1);
                commonServices.SelectPaymentDateInDateField(workingDate);
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
                payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                payments.VerifyPaymentDeletionMessageIsDisplayed();
                payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
                payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
                payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
            }
            else
            {
                test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
            }
        }


        [TestMethod]
        [Description("<br> TC-773 AP_E2E_OTP_Ontime_Setup_payment <br>" +
                     "TC-473 Verify account standing indicator is is green for on time <br>" +
                     "TC-1057 AP_OTP_Ontime_ManualCancel_Payment <br>" +
                     "TC-1048 Verify Deleted reason displayed when payment deleted from agent portal"+
                     "AP_E2E_OTP_Ontime_Edit_Delete_payment")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_773_473_1057_1048_TC_AP_E2E_OTP_OnTimeSetupEditDeletePayment()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getE2EOTPOnTimeSetupEditPayment;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForOTP())
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                    test.Log(Status.Fail, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
            }
            loanLevelData = listOfLoanLevelData[retryCount];

            string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
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
            payments.SelectValueInAuthorizedByDropdown(borrower);
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
            payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
            payments.VerifyPaymentDeletionMessageIsDisplayed();
            payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
            payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
            payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
        }

        [TestMethod]
        [Description("<br> TC-794 AP_E2E_OTP_Prepaid_Setup_payment<br>" +
                     "TC-5717 Verify account standing indicator is is green for Prepaid<br>" +
                     "TC-859 AP_OTP_PrePaid_AutoCancel_payment<br>" +
                     "AP_E2E_OTP_Prepaid_Edit_Delete_payment")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_794_5717_859_TC_AP_E2E_OTP_PrepaidSetupEditDeletePayment()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getE2EOTPPrepaidSetupEditPayment;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForOTP())
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                    test.Log(Status.Fail, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
            }
            loanLevelData = listOfLoanLevelData[retryCount];

            string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
               loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
               $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);

            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected OTP loan.",
                                             "Failure - 'Make a Payment' button is not Purple in color for the selected OTP loan.");
            reportLogger.TakeScreenshot(test, "Make a Payment' button");

            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);

            payments.VerifyOTPScreenDetails(loanLevelData, Constants.DefaultsPlan.OTP);
            payments.VerifyBannerDetails(loanLevelData);
            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
            string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00);

            string workingDate = commonServices.GetWorkingDate();
            payments.SelectPaymentDateInDateField(workingDate);
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
            totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00, 15.00, 20.00, Constants.PaymentFlowType.Edit);
            workingDate = commonServices.GetWorkingDate(Constants.PaymentFlowType.Edit);
            commonServices.SelectPaymentDateInDateField(workingDate);
            payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
            payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PaymentFlowType.Edit);
            payments.VerifyTotalAmountPaymentReviewPage(totalPayment, Constants.PaymentFlowType.Edit);
            payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment, Constants.PaymentFlowType.Edit);
            payments.ClickConfirmButtonPaymentReviewPage();
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PaymentFlowType.Edit);
            confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
            payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
            payments.VerifyPaymentDeletionMessageIsDisplayed();
            payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
            payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
            payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
        }

        [TestMethod]
        [Description("<br> TC-613 AP_E2E_OTP_Pastdue_Setup_payment <br>" +
                     "TC-471 Verify account standing indicator is is yellow for past due time <br>" +
                     "TC-822 AP_OTP_PastDue_ManualCancel_Payment <br>" +
                     "AP_E2E_OTP_Pastdue_Edit_Delete_payment")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_613_471_822_TC_AP_E2E_OTP_PastdueSetupEditDeletePayment()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getE2EOTPPastdueSetupEditPayment;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForOTP())
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                    test.Log(Status.Fail, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
            }
            loanLevelData = listOfLoanLevelData[retryCount];

            string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
               loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
               $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);

            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected OTP loan.",
                                             "Failure - 'Make a Payment' button is not Purple in color for the selected OTP loan.");
            reportLogger.TakeScreenshot(test, "Make a Payment' button");

            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);

            payments.VerifyOTPScreenDetails(loanLevelData, Constants.DefaultsPlan.OTP);
            payments.VerifyBannerPastDueDateDetails(loanLevelData);
            flag = webElementExtensions.VerifyElementColor(Constants.Colors.Orange, payments.pastDueDivLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Past Due' banner is 'Orange' in color for the selected OTP loan.",
                                             "Failure - 'Past Due' Banner is not Orange in color for the selected  OTP loan.");
            reportLogger.TakeScreenshot(test, "Past Due");
            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);

            string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00);

            commonServices.VerifyEnabledDisabledPaymentDatesForMonth();
            commonServices.VerifyMinimumPayDateOfMonthNotBankingHoliday();
            commonServices.VerifyMaximumPayDateIsLastDayOfMonthNotBankingHoliday();
            string workingDate = commonServices.GetWorkingDateAfter16thDate();

            payments.SelectPaymentDateInDateField(workingDate);
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
            payments.VerifyPaymentBreakdownSectionNotVisible(Constants.PaymentFlowType.Edit);
            payments.VerifyOTPScreenDetails(loanLevelData, Constants.Plans.Active, Constants.PaymentFlowType.Edit);
            payments.VerifyBannerPastDueDateDetails(loanLevelData, Constants.Plans.Empty, Constants.Plans.Empty, Constants.PaymentFlowType.Edit);
            flag = webElementExtensions.VerifyElementColor(Constants.Colors.Orange, payments.pastDueDivLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that Edit 'Past Due' banner is 'Orange' in color for the selected OTP loan.",
                                             "Failure - 'Past Due' Edit Banner is not Orange in color for the selected  OTP loan.");
            reportLogger.TakeScreenshot(test, "Past Due Banner");
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
            payments.VerifyUpdatePaymentButtonIsDisabled(Constants.PaymentFlowType.Edit);

            totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00, 15.00, 20.00, Constants.PaymentFlowType.Edit);
            workingDate = commonServices.GetWorkingDateAfter16thDate(Constants.PaymentFlowType.Edit);
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
        }

        [TestMethod]
        [Description("<br> TC-609 AP_E2E_OTP_Delinquent_Setup_payment <br>" +
                     "TC-462 Verify account standing indicator is is red for delinquent <br>" +
                     "TC-823 AP_OTP_Delinquent_ManualCancel_Payment <br>" +
                     "AP_E2E_OTP_Delinquent_Edit_Delete_payment")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_609_462_823_TC_AP_E2E_OTP_DelinquentSetupEditDeletePayment()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getE2EOTPDelinquentSetupEditPayment;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForOTP())
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                    test.Log(Status.Fail, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
            }
            loanLevelData = listOfLoanLevelData[retryCount];

            string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
               loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
               $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);

            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected OTP loan.",
                                             "Failure - 'Make a Payment' button is not Purple in color for the selected OTP loan.");
            reportLogger.TakeScreenshot(test, "Make a Payment' button");

            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);

            payments.VerifyOTPScreenDetails(loanLevelData, Constants.DefaultsPlan.OTP);
            payments.VerifyBannerDetails(loanLevelData);
            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
            string totalPayment = payments.VerifyOTPDelinquentPaymentFields(10.00, 15.00);
            string workingDate = commonServices.GetWorkingDate();
            payments.SelectPaymentDateInDateField(workingDate);
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
            payments.VerifyPaymentBreakdownSectionNotVisible(Constants.PaymentFlowType.Edit);
            payments.VerifyOTPScreenDetails(loanLevelData, Constants.Plans.Active, Constants.PaymentFlowType.Edit);
            payments.VerifyBannerDetails(loanLevelData, Constants.Plans.Empty, Constants.PaymentFlowType.Edit);
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);
            totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00, 15.00, 20.00, Constants.PaymentFlowType.Edit);
            workingDate = commonServices.GetWorkingDate(Constants.PaymentFlowType.Edit);
            commonServices.SelectPaymentDateInDateField(workingDate);
            payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
            payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PaymentFlowType.Edit);
            payments.VerifyTotalAmountPaymentReviewPage(totalPayment, Constants.PaymentFlowType.Edit);
            payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, workingDate, totalPayment, Constants.PaymentFlowType.Edit);
            payments.ClickConfirmButtonPaymentReviewPage();
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PaymentFlowType.Edit);
            confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
            payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
            payments.VerifyPaymentDeletionMessageIsDisplayed();
            payments.VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(confirmationNumber);
            payments.VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(confirmationNumber);
            payments.VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(confirmationNumber, deleteReason);
        }

        [TestMethod]
        [Description("<br> TC-535 Verify display message when Customer has opted in for paperless notifications <br>")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_535_TC_VerifyDisplayMessageWhenCustomerHasOptedInForPaperlessNotifications()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getE2EOTPPrimaryBorrowerIsOptedInForPaperless;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);

            while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
            {
                if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                {
                    if (dashboard.VerifyIfLoanNumberIsEligibleForOTP())
                        break;
                }
                retryCount++;
                if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                    test.Log(Status.Fail, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
            }

            loanLevelData = listOfLoanLevelData[retryCount];

            string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
               loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
               $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
            payments.DeleteAllExistingPendingPayments(deleteReason);

            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected OTP loan.",
                                             "Failure - 'Make a Payment' button is not Purple in color for the selected OTP loan.");
            reportLogger.TakeScreenshot(test, "Make a Payment' button");

            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyOTPPageIsDisplayed();
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
            payments.SelectValueInAuthorizedByDropdown(borrower);
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
            payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00);

            commonServices.VerifyEnabledDisabledPaymentDatesForMonth();
            commonServices.VerifyMinimumPayDateOfMonthNotBankingHoliday();
            commonServices.VerifyMaximumPayDateIsLastDayOfMonthNotBankingHoliday();
            string workingDate = commonServices.GetWorkingDate();
            payments.SelectPaymentDateInDateField(workingDate);
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.VerifyReviewAndConfirmationPageIsDisplayed();
            flag = webElementExtensions.IsElementDisplayed(_driver, By.XPath(payments.paraByText.Replace("<TEXT>", Constants.Messages.OptedForPaperLess)));
            _driver.ReportResult(test, flag, "Successfully verified that " + payments.paraByText.Replace("<TEXT>", Constants.Messages.OptedForPaperLess),
                                             "Failure - " + payments.paraByText.Replace("<TEXT>", Constants.Messages.OptedForPaperLess));
            reportLogger.TakeScreenshot(test, "Opted for Paperless");
            payments.ClickEditAndEnterMail(email);
        }

        [TestMethod]
        [Description("<br> TC-529 Verify display message when Customer has opted out of paperless notifications <br>")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_529_TC_VerifyDisplayMessageWhenCustomerHasOptedOutOfPaperlessNotifications()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getE2EOTPPrimaryBorrowerOptedOutForPaperless;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery, true);

            #endregion TestData

            if (listOfLoanLevelData.Count > 0)
            {
                commonServices.LoginToTheApplication(username, password);

                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
                {
                    if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                    {
                        if (dashboard.VerifyIfLoanNumberIsEligibleForOTP())
                            break;
                    }
                    retryCount++;
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                        test.Log(Status.Fail, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                }

                loanLevelData = listOfLoanLevelData[retryCount];

                string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                   loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                   $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                payments.DeleteAllExistingPendingPayments(deleteReason);

                bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
                _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected OTP loan.",
                                                 "Failure - 'Make a Payment' button is not Purple in color for the selected OTP loan.");
                reportLogger.TakeScreenshot(test, "Make a Payment' button");

                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyOTPPageIsDisplayed();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);
                payments.SelectValueInAuthorizedByDropdown(borrower);
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);
                string totalPayment = payments.VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00);


                commonServices.VerifyEnabledDisabledPaymentDatesForMonth();
                commonServices.VerifyMinimumPayDateOfMonthNotBankingHoliday();
                commonServices.VerifyMaximumPayDateIsLastDayOfMonthNotBankingHoliday();
                string workingDate = commonServices.GetWorkingDate();
                payments.SelectPaymentDateInDateField(workingDate);
                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed();
                flag = webElementExtensions.IsElementDisplayed(_driver, By.XPath(payments.paraByText.Replace("<TEXT>", Constants.Messages.NotOptedForPaperLess)));
                _driver.ReportResult(test, flag, "Successfully verified that " + payments.paraByText.Replace("<TEXT>", Constants.Messages.NotOptedForPaperLess),
                                                 "Failure - " + payments.paraByText.Replace("<TEXT>", Constants.Messages.NotOptedForPaperLess));
                reportLogger.TakeScreenshot(test, "Opted Out for Paperless");
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
                workingDate = commonServices.GetWorkingDate(Constants.PaymentFlowType.Edit);
                payments.SelectPaymentDateInDateField(workingDate);
                payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(Constants.PaymentFlowType.Edit);
                payments.ClickConfirmButtonPaymentReviewPage();
                payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, Constants.PaymentFlowType.Edit);
                confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
                payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.DeleteNewlyAddedPayment(confirmationNumber, deleteReason);
                payments.VerifyPaymentDeletionMessageIsDisplayed();
            }
            else
            {
                test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
            }
        }


        [TestMethod]
        [Description("<br> TC-1189 SCM-3456-Test-Manage Accounts - OTP setup screen")]
        [TestCategory("AP_Regression"), TestCategory("AP_OTP")]
        public void TC_1189_TC_Test_ManageAccounts_OTPSetupScreen()
        {
            int retryCount = 0;

            #region TestData

            loanDetailsQuery = getE2EOTPPrepaidSetupEditPayment;
            listOfLoanLevelData = commonServices.GetListOfLoanLevelData(loanDetailsQuery);

            #endregion TestData

            if (listOfLoanLevelData.Count > 0)
            {
                commonServices.LoginToTheApplication(username, password);

                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired)
                {
                    if (commonServices.LaunchUrlWithLoanNumber(listOfLoanLevelData, retryCount, true))
                    {
                        if (dashboard.VerifyIfLoanNumberIsEligibleForOTP())
                            break;
                    }
                    retryCount++;
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired)
                        test.Log(Status.Fail, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                }
                loanLevelData = listOfLoanLevelData[retryCount];

                string borrower = loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName] == null ? "" :
                   loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToLower() == "null" ? "" :
                   $"{loanLevelData[Constants.LoanLevelDataColumns.BorrowerFirstName].ToString().ToUpper()} {loanLevelData[Constants.LoanLevelDataColumns.BorrowerLastName].ToString().ToUpper()}";

                payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
                payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData);
                payments.DeleteAllExistingPendingPayments(deleteReason);

                bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Purple, payments.makeAPaymentButtonLocBy);
                _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Purple' in color for the selected OTP loan.",
                                                 "Failure - 'Make a Payment' button is not Purple in color for the selected OTP loan.");
                reportLogger.TakeScreenshot(test, "Make a Payment' button");

                payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyOTPPageIsDisplayed();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment);

                payments.VerifyOTPScreenDetails(loanLevelData, Constants.DefaultsPlan.OTP);
                payments.VerifyBannerDetails(loanLevelData);
                payments.SelectValueInAuthorizedByDropdown(borrower);
                string paymentList = payments.GetMethodDropDownValues();
                payments.ClickButtonUsingName(Constants.ButtonNames.ManageAccounts);
                payments.VerifyBankAccounts(paymentList, "Account no.");
            }
            else
            {
                test.Log(Status.Warning, "No Data Found for Query : " + loanDetailsQuery);
            }
        }
    }
}