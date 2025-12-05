using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_CustomerPortal.Pages;
using LD_CustomerPortal.TestPages;
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
    [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
    public class NonPaymentsTests : BasePage
    {
        #region ObjectInitialization

        string loansDataForPaperlessEligible = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsforPaperless));
        string helocLoansDataForPaperlessEligible = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetHelocLoanLevelDetailsforPaperless));
        string activeLoansForNonPayments = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetActiveLoanForNonPayments));
        string loanLevelDetailsForPayoffquote = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansLevelDetailsForPayoffquote));
        string loansLevelDetailsForSMCVerification = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansLevelDetailsForSMCVerification));
        string loanLevelDetailsForOntimePastdue = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOntimePastdue));
        string loanLevelDetailsForOntimePastdueHeloc = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForOntimePastdueHeloc));
        string loanLevelDetailsForAutopayPrompt = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansAutopayPromptFM));
        string loanDetailsQueryForEligibleHelocAutopayOntime = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleHelocAutopayOntime));
        string loanDetailsQueryForEligibleAutopayOntime = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEligibleAutoPayOntime));
        string loanDetailsQueryforExisitngAutopay = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.FMPendingAutopayLoans));
        string loanDetailsQueryforExisitngOTP = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.FMPendingOTPLoans));
        string loanDetailsQueryforExisitngAutopayHeloc = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.HelocPendingAutopayLoans));
        string loanDetailsQueryforExisitngOTPHeloc = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.HelocPendingOTPLoans));
        string loanDetailsQueryforExsitingOtpHelocBankValidation = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.HelocExsitingOTPForBankAccountValidation));
        string loanDetailsQueryforExsitingOtpFMBankValidation = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.FMExistingOTPForBankAccountValidation));


        DashboardPage dashboard = null;
        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;
        Pages.PaymentsPage payments = null;
        StatementsPage statements = null;
        PayoffQuotePage payoffQuote = null;
        PaperlessSettingsPage paperless = null;
        SMCPage smc = null;
        DBconnect dBconnect = null;
        FAQsPage FAQs = null;
        YopmailPage yopmail = null;
        ReportLogger reportLogger { get; set; }


        #endregion ObjectInitialization

        public TestContext TestContext
        {
            set;
            get;
        }

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
            payoffQuote = new PayoffQuotePage(_driver, test);
            FAQs = new FAQsPage(_driver, test);
            statements = new StatementsPage(_driver, test);
            paperless = new PaperlessSettingsPage(_driver, test);
            yopmail = new YopmailPage(_driver, test);
            //unlink loans from test account
            if (!test.Model.FullName.ToString().Contains("TPR_2244_VerifyRegistrationFeature"))
            {
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
                string queryToUpdateTCPAFlagIsGlobalValue = UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.UpdateTCPAFlagIsGlobalValue).Replace("TCPA_FLAG_VALUE", "0");
                dBconnect.ExecuteQuery(queryToUpdateTCPAFlagIsGlobalValue).ToString();
            }
        }

        [TestMethod]
        [Description("<br><b>TPR-2600 Verify Help and Support FAQ</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_2600_TPR_VerifyHelpAndSupport()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            List<string> usedLoanTestData = new List<string>();

            try
            {
                #region TestData

                test.Log(Status.Info, $"Query Used: {activeLoansForNonPayments} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(activeLoansForNonPayments, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.helpAndSupportLocBy))
                            break;
                        retryCount++;
                    }
                    if (retryCount >= ConfigSettings.NumberOfLoanTestDataRequired && retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.helpAndSupportLocBy);
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
                //Navigate to FAQs
                test.Log(Status.Info, "<b>********************************************<u>Help and Support Hyperlink</u>*******************************************</b>");
                webElementExtensions.ScrollToTop(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.helpSupportLinkLocBy);
                reportLogger.TakeScreenshot(test, "Help And Support Link");
                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.helpSupportLinkLocBy);
                webElementExtensions.WaitUntilUrlContains("faqs");
                webElementExtensions.WaitForPageLoad(_driver);
                FAQs.SearchByTopic("payment");
                webElementExtensions.ClickElement(_driver, dashboard.loanDepotLogoLocBy);
                webElementExtensions.WaitUntilUrlContains("dashboard");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                test.Log(Status.Info, "<b>********************************************<u>FAQ in 'Help and Support' dropdown </u>*******************************************</b>");
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.helpAndSupportLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.helpAndSupportLocBy);
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.faqAnchorLinkLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.faqAnchorLinkLocBy);
                webElementExtensions.WaitUntilUrlContains("faqs");
                webElementExtensions.WaitForPageLoad(_driver);
                FAQs.SearchByTopic("payment");
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
        [Description("<br><b>TPR-743 Verify Request Payoff Quote</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_743_TPR_VerifyRequestPayoffQuote()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            List<string> usedLoanTestData = new List<string>();
            try
            {
                #region TestData

                test.Log(Status.Info, $"Query Used: {loanLevelDetailsForPayoffquote} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForPayoffquote, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
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
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.payoffQuoteLocBy);
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Payoff Quote</u>*******************************************</b>");

                //Navigate to  PayoffQuote Page
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.payoffQuoteLocBy);
                dashboard.RequestPayoffQuote();
                webElementExtensions.WaitUntilUrlContains("payoff-quote");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForElement(_driver, payoffQuote.pageTitleLocBy);
                string title = webElementExtensions.GetElementText(_driver, payoffQuote.pageTitleLocBy);
                ReportingMethods.LogAssertionContains(test, "Request Payoff Quote", title, $"Expected Title : Request Payoff Quote, Atual Title : {title}");
                string payoffDescriptionActual = webElementExtensions.GetElementText(_driver, payoffQuote.payoffDescriptionLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.PayoffDescription, payoffDescriptionActual, "Payoff Description not matching");

                foreach (var method in Constants.PayoffQuoteChannels)
                {
                    List<string> dates = null;
                    dates = payoffQuote.RequestPayoffQuote(method);
                    if (method != "US Postal Mail")
                    {
                        _driver.ReportResult(test, (dates.Count == 30 || dates.Count == 31), $"{method} : Successfully validated number of calander days enabled", $"Expected number of eligible days=30/31 but found {dates.Count}");
                        payoffQuote.SelecDateFromCalanderForPayoffQuote(dates[0]);
                    }
                    else
                    {
                        var isEnabled = webElementExtensions.IsElementEnabled(_driver, commonServices.paymentDatePickerIconLocBy);
                        ReportingMethods.LogAssertionFalse(test, isEnabled, "Expected Calander button tobe Disabled");
                    }
                }
                webElementExtensions.ScrollToTop(_driver);
                webElementExtensions.ClickElementUsingJavascript(_driver, payoffQuote.btnIdRequestPayoffLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payoffQuote.dialogHeaderLocBy);
                var msgStatus = webElementExtensions.GetElementText(_driver, payoffQuote.dialogHeaderLocBy);

                if (msgStatus.Contains(" Contact Us"))
                {
                    ReportingMethods.LogAssertionContains(test, "Contact Us", msgStatus, $"Expected Contact Us dialog ! but found {msgStatus}");
                    var msg = webElementExtensions.GetElementText(_driver, payoffQuote.payoffQuoteContentLocBy);
                    reportLogger.TakeScreenshot(test, "Payoff Quote Request Status Dialog");
                    ReportingMethods.LogAssertionContains(test, "we cannot process your payoff quote at this time due to some technical challenges", msg, $"Expected '{msg}' should have 'we cannot process your payoff quote at this time due to some technical challenges'");
                    webElementExtensions.ClickElementUsingJavascript(_driver, payoffQuote.btnCloseDialogLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    test.Log(Status.Warning, "<b>This loan has failed to process Payoff quote</b>");
                }
                else
                {
                    ReportingMethods.LogAssertionContains(test, "Success", msgStatus, $"Expected Success! but found {msgStatus}");
                    var msg = webElementExtensions.GetElementText(_driver, payoffQuote.payoffQuoteContentLocBy);
                    reportLogger.TakeScreenshot(test, "Payoff Quote Request Status Dialog");
                    ReportingMethods.LogAssertionContains(test, "Your payoff quote is being processed", msg, $"Expected '{msg}' should have 'Your payoff quote is being processed'");
                    webElementExtensions.ClickElement(_driver, payoffQuote.backToHomeLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForPageLoad(_driver);
                    webElementExtensions.WaitUntilUrlContains("dashboard");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.payoffQuoteLocBy);
                    var statusInDashboard = webElementExtensions.GetElementText(_driver, dashboard.payoffQuoteLocBy);
                    webElementExtensions.ScrollToTop(_driver);
                    reportLogger.TakeScreenshot(test, "Payoff Request Status Dashboard");
                    ReportingMethods.LogAssertionEqualIgnoreCase(test, "Payoff Quote Request In Progress", statusInDashboard, $"Actual {statusInDashboard} , Expected 'Payoff Quote Request In Progres'");
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
        [Description("<br>TPR-793-Verify Statements and Documents")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_793_TPR_VerifyStatementsAndDocuments()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();

            try
            {
                #region TestData

                test.Log(Status.Info, $"Query Used: {activeLoansForNonPayments} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(activeLoansForNonPayments, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
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
                        dashboard.HandleServiceChatBot();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.payoffQuoteLocBy);
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Statements and Documents</u>*******************************************</b>");

                //Navigate to Statement and Documents
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.statementsLinkLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.statementsLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitUntilUrlContains("documents");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitUntilElementIsClickable(_driver, statements.searchBtnLocBy);
                var start = webElementExtensions.GetElementAttribute(_driver, statements.startDateLocBy, "min");
                var end = webElementExtensions.GetElementAttribute(_driver, statements.endDateLocBy, "max");

                var startDate = DateTime.Parse(start).Date;
                var endDate = DateTime.Parse(end).Date;
                var today = DateTime.Today.Date;

                ReportingMethods.LogAssertionEqual(test, end, today.ToString("yyyy-MM-dd"), $"End Date should be {today.ToString("yyyy-MM-dd")}");
                var expectedStart = today.AddMonths(-13);
                ReportingMethods.LogAssertionEqual(test, start, expectedStart.ToString("yyyy-MM-dd"), $"Start Date should be '{expectedStart.ToString("yyyy-MM-dd")}'");

                //Get listed Documents
                var documents = statements.GetDocumentsList();

                bool viewedDoc = false;
                bool isdocExists = false;
                foreach (string category in Constants.Statements)
                {
                    isdocExists = statements.SearchForDocumentCategory(category);
                    //Check viw Document
                    if (isdocExists && !viewedDoc)
                    {
                        statements.CheckViewDocument();
                        viewedDoc = true;
                    }
                }
                if (isdocExists)
                    ReportingMethods.LogAssertionTrue(test, viewedDoc, "View Document Verification Successful");
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
        [Description("<br><b>TPR-396 Verify Paperless Settings For Mortgage Loans</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_396_TPR_VerifyPaperlessSettingsForMortgageLoans()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                test.Log(Status.Info, $"Query Used: {loansDataForPaperlessEligible} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loansDataForPaperlessEligible, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
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
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.payoffQuoteLocBy);
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Paperless Settings</u>*******************************************</b>");

                //Navigate to Settings->Paperless
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.settingsBtnLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.settingsBtnLocBy);
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.managePaperlessLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.managePaperlessLocBy);
                webElementExtensions.WaitUntilUrlContains("manage-paperless");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForElement(_driver, paperless.btnSaveChangesLocBy);
                reportLogger.TakeScreenshot(test, $"Manage Paperless");

                var isDisabled = webElementExtensions.IsElementDisabled(_driver, paperless.btnSaveChangesLocBy);
                ReportingMethods.LogAssertionTrue(test, isDisabled, "Save Chnages Button Should be disabled if no changes");

                string paperlessDescription = webElementExtensions.GetElementText(_driver, paperless.paperlessDescriptionLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.PaperlessDescription, paperlessDescription, "Paperless Description Comparison");

                string toggleDescription = webElementExtensions.GetElementText(_driver, paperless.toggleDescriptionLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.PaperlessToggleDescription, toggleDescription, "Toggle Description Comparison");

                paperless.VerifySelectAllButtonFunctionality();
                bool isSaveDone = paperless.VerifySaveChanges();
                ReportingMethods.LogAssertionTrue(test, isSaveDone, "Save Should be successful");
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
        [Description("<br><b>TPR-793 Verify Paperless Settings For Heloc Loans</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_793_TPR_VerifyPaperlessSettingsForHelocLoans()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                test.Log(Status.Info, $"Query Used: {helocLoansDataForPaperlessEligible} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(helocLoansDataForPaperlessEligible, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Paperless Settings</u>*******************************************</b>");

                //Navigate to Settings->Paperless
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.settingsBtnLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.settingsBtnLocBy);
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.managePaperlessLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.managePaperlessLocBy);
                webElementExtensions.WaitUntilUrlContains("manage-paperless");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForElement(_driver, paperless.btnSaveChangesLocBy);
                reportLogger.TakeScreenshot(test, $"Manage Paperless");

                var isDisabled = webElementExtensions.IsElementDisabled(_driver, paperless.btnSaveChangesLocBy);
                ReportingMethods.LogAssertionTrue(test, isDisabled, "Save Chnages Button Should be disabled if no changes");

                string paperlessDescription = webElementExtensions.GetElementText(_driver, paperless.paperlessDescriptionLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.PaperlessDescription, paperlessDescription, "Paperless Description Comparison");

                string toggleDescription = webElementExtensions.GetElementText(_driver, paperless.toggleDescriptionLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.PaperlessToggleDescription, toggleDescription, "Toggle Description Comparison");

                paperless.VerifySelectAllButtonFunctionality();
                bool isSaveDone = paperless.VerifySaveChanges();
                ReportingMethods.LogAssertionTrue(test, isSaveDone, "Save Should be successful");
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
        [Description("<br><b>TPR-1377_1375_735 Verify Dashboard Products and Services</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_1377_1375_735_TPR_VerifyDashboardProductsAndServices()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();

            try
            {
                #region TestData

                test.Log(Status.Info, $"Query Used: {activeLoansForNonPayments} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(activeLoansForNonPayments, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
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
                        dashboard.HandleServiceChatBot();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.payoffQuoteLocBy);
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Products and Services</u>*******************************************</b>");

                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.statementsLinkLocBy);
                //check products and services
                foreach (string product in Constants.Products)
                {
                    dashboard.CheckProduct(product);
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
        [Description("<br><b>TPR-542 Verify Mobile Video Tutorial</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_542_TPR_VerifyMobileVideoTutorial()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                test.Log(Status.Info, $"Query Used: {activeLoansForNonPayments} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(activeLoansForNonPayments, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
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
                        dashboard.HandleServiceChatBot();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.payoffQuoteLocBy);
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
                test.Log(Status.Info, "<b>********************************************<u>Starting Help and Support</u>*******************************************</b>");

                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.helpAndSupportLocBy);
                dashboard.VerifyMobileTutorial();
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
        [Description("<br><b>TPR-2596 Verify National American Indian Housing Council (NAIHC) link from the Dashboard Link</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_2596_TPR_VerifyNAIHCLink()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                test.Log(Status.Info, $"Query Used: {activeLoansForNonPayments} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(activeLoansForNonPayments, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
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
                        dashboard.HandleServiceChatBot();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.payoffQuoteLocBy);
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
                test.Log(Status.Info, "<b>********************************************<u>Starting NAIHC</u>*******************************************</b>");

                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.helpAndSupportLocBy);
                dashboard.VerifyNAIHC_Assistance();
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
        [Description("<br><b>TPR-2599 Verify Enable Paperless link from the Dashboard Link</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_2599_TPR_VerifyPaperlessSettingsFromDashboardLink()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                test.Log(Status.Info, $"Query Used: {loansDataForPaperlessEligible} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loansDataForPaperlessEligible, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                loanLevelData.RemoveAll(loan => loan.ContainsKey("loanNumber") && loan["loanNumber"].ToString().StartsWith("9"));


                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        retryCount++;
                    }
                    if (retryCount >= ConfigSettings.NumberOfLoanTestDataRequired && retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.payoffQuoteLocBy);
                        retryCount++;
                    }
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount >= ConfigSettings.NumberOfLoanTestDataRequired && retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                }
                test.Log(Status.Info, "<b>********************************************<u>Starting Paperless Settings</u>*******************************************</b>");

                //Navigate to Paperless
                dashboard.ClickPaperlessSettings();
                webElementExtensions.WaitUntilUrlContains("manage-paperless");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForElement(_driver, paperless.btnSaveChangesLocBy);
                reportLogger.TakeScreenshot(test, $"Manage Paperless");

                var isDisabled = webElementExtensions.IsElementDisabled(_driver, paperless.btnSaveChangesLocBy);
                ReportingMethods.LogAssertionTrue(test, isDisabled, "Save Chnages Button Should be disabled if no changes");

                string paperlessDescription = webElementExtensions.GetElementText(_driver, paperless.paperlessDescriptionLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.PaperlessDescription, paperlessDescription, "Paperless Description Comparison");

                string toggleDescription = webElementExtensions.GetElementText(_driver, paperless.toggleDescriptionLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.PaperlessToggleDescription, toggleDescription, "Toggle Description Comparison");

                paperless.VerifySelectAllButtonFunctionality();
                bool isSaveDone = paperless.VerifySaveChanges();
                ReportingMethods.LogAssertionTrue(test, isSaveDone, "Save Should be successful");
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
        [Description("<br><b>TPR-2598 Verify Licensing Information and privacy policy link from the Dashboard Link</b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_2598_TPR_VerifyLicensingAndPrivacyPolicyLinks()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                test.Log(Status.Info, $"Query Used: {activeLoansForNonPayments} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(activeLoansForNonPayments, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
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
                        dashboard.HandleServiceChatBot();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.payoffQuoteLocBy);
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

                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.licensingLinkLocBy);
                test.Log(Status.Info, "<b>********************************************<u>Comehome Licensing Info </u>*******************************************</b>");
                dashboard.VerifyLink(dashboard.licensingLinkLocBy, "Licensing", "https://www.housecanary.com/products/brokerage-services");
                test.Log(Status.Info, "<b>********************************************<u>Comehome Privacy Policy </u>*******************************************</b>");
                dashboard.VerifyLink(dashboard.widgetPrivacyLinkLocBy, "Privacy Policy", "https://loandepot.demo.ch.housecanary.net/privacy-policy");
                test.Log(Status.Info, "<b>********************************************<u>Starting Privacy Policy at Footer</u>*******************************************</b>");
                dashboard.VerifyLink(dashboard.footerPrivacyLinkLocBy, "Privacy Policy", "https://www.loandepot.com/privacypolicy");
                test.Log(Status.Info, "<b>********************************************<u>Terms of Use</u>*******************************************</b>");
                dashboard.VerifyLink(dashboard.termsLinkLocBy, "Terms of Use", "https://www.loandepot.com/termsofuse");
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
        [Description("<br>TPR-2544-Verify MFA functionality linking 1st loan - Borrower's Email<br>" +
                         "TPR-2545-Verify MFA functionality while editing profile - Borrower's Email<br>" +
                         "TPR-2639-Verify MFA on Profile Page Updation" +
                         "TPR-2207-Test Mailing Address - City Field Validations - Borrower")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_2544_2545_2639_2207_TPR_VerifyMFAFunctionalityWhileLinkingFMLoanWithEmailVerification()
        {
            int loansFound = 0;
            int retryCount = 0;
            bool retry = true;
            List<Hashtable> loanLevelData = new List<Hashtable>();

            try
            {
                #region TestData
                // Execute the query and retrieve the loan data
                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loansLevelDetailsForSMCVerification}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loansLevelDetailsForSMCVerification, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);


                if (retryCount == 0)
                {
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    test.Log(Status.Info, $"<b>***********************************<u>Started the process to Link 1st loan and validate MFA</u>***********************************</b>");
                    dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username, isVerifyMFARequired: true);
                    dashboard.HandlePaperLessPage();
                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                    dashboard.HandleServiceChatBot();
                    dashboard.ClosePopUpsAfterLinkingNewLoan();
                    test.Log(Status.Info, $"<b>***********************************<u>Ending the process to Link 1st loan and validate MFA</u>***********************************</b>");
                }
                retryCount++;

                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                test.Log(Status.Info, $"<b>***********************************<u>Started the process to Profile Update and validate MFA</u>***********************************</b>");
                webElementExtensions.ActionClick(_driver, dashboard.profileAvatarIconLocBy, "Profile Avatar Icon");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.editButtonOnContactInformationLocBy, "Edit Button on contact info page");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.homePhoneInputFieldOnContactInfoPageLocBy);
                webElementExtensions.EnterText(_driver, dashboard.homePhoneInputFieldOnContactInfoPageLocBy, "6789123456", false, "Home Phone Number field", true);
                test.Log(Status.Info, $"<b>***********************************<u>Profile Update and City Validation</u>***********************************</b>");
                var cityName = "12@#$%^&*";
                webElementExtensions.EnterText(_driver, dashboard.cityInputFieldOnCantactInfoPageLocBy, cityName, false, "City Name");
                test.Log(Status.Info, $"Entered City Name as {cityName}");
                var enteredCity = webElementExtensions.GetElementText(_driver, dashboard.cityInputFieldOnCantactInfoPageLocBy);
                ReportingMethods.LogAssertionTrue(test, enteredCity.Length == 0, "City name should not allow numerics and special characters,only Aplhabets are allowed");
                reportLogger.TakeScreenshot(test, $"City name : {cityName}");
                cityName = "Miami";
                webElementExtensions.EnterText(_driver, dashboard.cityInputFieldOnCantactInfoPageLocBy, "Miami", false, "City Name");
                test.Log(Status.Info, $"Entered City Name as {cityName}");
                reportLogger.TakeScreenshot(test, $"City name : {cityName}");
                webElementExtensions.WaitForElementToBeEnabled(_driver, dashboard.saveChangesButtonOnContactInfoPageLocBy, "Save Changes Button on contact info page", isScrollIntoViewRequired: false);
                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.saveChangesButtonOnContactInfoPageLocBy, "Save Changes Button on contact info page", isReportRequired: true);

                webElementExtensions.WaitForElement(_driver, dashboard.saveBtnLocBy);
                reportLogger.TakeScreenshot(test, "Save Profile Update");
                webElementExtensions.ActionClick(_driver, dashboard.saveBtnLocBy, "Save Button");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.saveBtnLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);

                dashboard.ByPassMFAAuthentication(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString());
                reportLogger.TakeScreenshot(test, "Profile Updated");
                test.Log(Status.Info, $"<b>***********************************<u>Ending the process to Profile Update and validate MFA</u>***********************************</b>");
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
        [Description("<br>TPR-2029-UI validations - Secure Message Center")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_2029_TPR_VerifySecureMessageCenter()
        {
            int loansFound = 0;
            int retryCount = 0;
            bool retry = true;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                // Execute the query and retrieve the loan data
                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loansLevelDetailsForSMCVerification}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loansLevelDetailsForSMCVerification, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");

                        webElementExtensions.WaitForElementToBeEnabled(_driver, By.XPath(string.Format(dashboard.serviceBotMessangerLocString, "visible")), timeoutInSeconds: ConfigSettings.SmallWaitTime);
                        if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(string.Format(dashboard.serviceBotMessangerLocString, "visible"))))
                        {
                            dashboard.HandleServiceChatBot();
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
                            break;
                        }
                        else
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
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
                        webElementExtensions.WaitForElementToBeEnabled(_driver, By.XPath(string.Format(dashboard.serviceBotMessangerLocString, "visible")), timeoutInSeconds: ConfigSettings.SmallWaitTime);
                        if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(string.Format(dashboard.serviceBotMessangerLocString, "visible"))))
                        {
                            dashboard.HandleServiceChatBot();
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
                            break;
                        }
                        else
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
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

                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy, ConfigSettings.WaitTime);
                test.Log(Status.Info, $"<b>***********************************<u>Started the process to Validate SMC </u>***********************************</b>");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.ScrollToBottom(_driver);
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.sendUsASceureMessageLinkOnContactUsCardLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.sendUsASceureMessageLinkOnContactUsCardLocBy, "Send us a secure message Link on Countact Us Card", isReportRequired: true);
                webElementExtensions.WaitUntilUrlContains("/smc", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, smc.newMessageButtonOnSMCPageLocBy);
                reportLogger.TakeScreenshot(test, "SMC via Send us a secure message Link on Countact Us Card");
                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "LoanDepot Icon", isReportRequired: true);
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.ActionClick(_driver, dashboard.messagesLinkLocBy, "SMC - Message Link", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitUntilUrlContains("/smc", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, smc.newMessageButtonOnSMCPageLocBy);
                reportLogger.TakeScreenshot(test, "SMC via Message Link");

                webElementExtensions.ActionClick(_driver, smc.newMessageButtonOnSMCPageLocBy, "New Message button on SMC page", isReportRequired: true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, smc.needAnEvenQuickerResponsePopupLocBy, ConfigSettings.SmallWaitTime);
                string a = webElementExtensions.GetElementText(_driver, smc.needAnEvenQuickerResponsePopupLocBy);
                webElementExtensions.VerifyElementText(_driver, smc.needAnEvenQuickerResponsePopupLocBy, (TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time")).TimeOfDay > new TimeSpan(17, 0, 0)) ? Constants.CustomerPortalTextMessages.NeedAnEvenQuickerResponsePopupTextContentOnSMCPageAfter5PmPst : Constants.CustomerPortalTextMessages.NeedAnEvenQuickerResponsePopupTextContentOnSMCPage, "Need An Even Quicker Response Popup Text Content On SMC Page", true);
                webElementExtensions.ActionClick(_driver, smc.letsChatButtonOnNeedAnEvenQuickerResponsePopupLocBy, "Let's Chat Button On Need An Even Quicker Response Popup", false, true);
                webElementExtensions.WaitForElementToBeEnabled(_driver, By.XPath(string.Format(dashboard.serviceBotMessangerLocString, "visible")), "Service bot", ConfigSettings.WaitTime);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(string.Format(dashboard.serviceBotMessangerLocString, "visible"))))
                    dashboard.HandleServiceChatBot();
                webElementExtensions.ActionClick(_driver, smc.newMessageButtonOnSMCPageLocBy, "New Message button on SMC page", isReportRequired: true);
                webElementExtensions.WaitForStalenessOfElement(_driver, smc.needAnEvenQuickerResponsePopupLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.ActionClick(_driver, smc.continueWithSecureMessageCenterLinkOnNeedAnEvenQuickerResponsePopupLocBy, "Let's Chat Button On Need An Even Quicker Response Popup", false, true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, smc.sendUpASecureMessagePopupLocBy);
                webElementExtensions.WaitForElementToBeEnabled(_driver, smc.whatCanWeHelpYouWithOnSendUsASecureMessagePopupLocBy, "What can we help you with? dropdown", ConfigSettings.WaitTime);
                webElementExtensions.ActionClick(_driver, smc.whatCanWeHelpYouWithOnSendUsASecureMessagePopupLocBy, "What can we help you with? dropdown", false, true);
                webElementExtensions.WaitForElementToBeEnabled(_driver, smc.optionsOnSendUsASecureMessagePopupLocBy, "Options on send us a secure message Popup", ConfigSettings.WaitTime);

                reportLogger.TakeScreenshot(test, "Before Selecting General Questions");
                var optionsLocator = _driver.FindElements(smc.optionsOnSendUsASecureMessagePopupLocBy);
                _driver.FindElements(By.CssSelector("mat-option"))
                    .First(o => o.GetAttribute("innerText").Trim() == "General Question").Click();
                reportLogger.TakeScreenshot(test, "After Selecting General Questions");
                webElementExtensions.WaitForVisibilityOfElement(_driver, smc.messageTextInputBoxLocBy);
                webElementExtensions.EnterText(_driver, smc.messageTextInputBoxLocBy, "Automation Test");
                webElementExtensions.ActionClick(_driver, smc.submitButtonOnSendUsASecureMessageLocBy, "Submit button on Send us a secure message Popup", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, smc.submitButtonOnSendUsASecureMessageLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, smc.messageSentPopupLocBy, ConfigSettings.WaitTime);
                webElementExtensions.ActionClick(_driver, smc.closeIconOnMessageSentPopUpLocBy, "Close Icon on the Message Sent Popup", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, smc.closeIconOnMessageSentPopUpLocBy, ConfigSettings.WaitTime);
                webElementExtensions.ActionClick(_driver, smc.sentMessagesTabLocBy, "Sent Message Tab", isReportRequired: true);
                webElementExtensions.ActionClick(_driver, smc.refreshButtonLocBy, "Refresh Button", isReportRequired: true);

                string recipientName = char.ToUpper(username[0]) + username.Substring(1, 3) + " " + char.ToUpper(username[4]) + username.Substring(5, 4) + "(you)";
                string lastfourDigitOfLinkedLoanNumber = loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);
                By firstNotificationLocBy = By.XPath(string.Format(commonServices.divLocatorWithSpecificText, $" General Question - Loan #XXXXXX{lastfourDigitOfLinkedLoanNumber} "));
                webElementExtensions.WaitForElementToBeEnabled(_driver, firstNotificationLocBy, "First General message", ConfigSettings.SmallWaitTime, false);
                webElementExtensions.ActionClick(_driver, firstNotificationLocBy, $"First SMC Sent Message with {$" General Question - Loan #XXXXXX{lastfourDigitOfLinkedLoanNumber} "}", false, true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, smc.smcMessageTemplateContentTestLocBy);
                string messageTextContent = webElementExtensions.GetElementText(_driver, smc.smcMessageTemplateContentTestLocBy, true);
                reportLogger.TakeScreenshot(test, "Sent Message Template Content");
                string expectedTextContent = $"Secure Message Center\r\nGeneral Question - Loan #XXXXXX{lastfourDigitOfLinkedLoanNumber}\r\nAT\r\n{recipientName}\r\nTo: Support Team\r\nToday ";
                ReportingMethods.LogAssertionTrue(test, messageTextContent.Contains(expectedTextContent.Trim()), "Verify the SMC Sent Message Template Content");
                ReportingMethods.LogAssertionTrue(test, messageTextContent.Contains("Automation Test"), "Verify the Entered Message Text");
                test.Log(Status.Info, $"<b>***********************************<u>Ending the process to Validate SMC</u>***********************************</b>");
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
        [Description("<br>TPR-2244-Verify the Registration Feature of Customer Portal Application")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_2244_TPR_VerifyRegistrationFeature()
        {
            string emailToUseForRegistration = string.Empty; string verificationCodeFromYopmail = string.Empty;
            try
            {
                //Get Email ID from Yopmail.com to use - Unique Random Email ID
                string prefix = "cp_testaccount";
                string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss"); // YearMonthDayHourMinuteSecond
                emailToUseForRegistration = $"{prefix}{timestamp}@yopmail.com";

                test.Log(Status.Info, $"<b>***********************************<u>Starting the process to Validate Registration Feature</u>***********************************</b>");
                //Method to Do the registration 

                webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.cpSignUpNowLinkLocBy, "Sign Up Now Link", ConfigSettings.WaitTime, isScrollIntoViewRequired: false);
                ReportingMethods.LogAssertionTrue(test, _driver.Url.ToString().Contains("b2c_1_unified_signup_signin"), "Verify if the url contains: b2c_1_unified_signup_signin");
                webElementExtensions.ActionClick(_driver, commonServices.cpSignUpNowLinkLocBy, "Sign Up Now Link", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.cpSignUpNowLinkLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.cpSendVerificationCodeButtonLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "Sign Up Page Before entering the details");
                ReportingMethods.LogAssertionTrue(test, _driver.Url.ToString().Contains("B2C_1_UNIFIED_SIGNUP"), "Verify if the url contains: B2C_1_UNIFIED_SIGNUP");
                webElementExtensions.VerifyElementColor(Constants.Colors.NeutralGrey, commonServices.cpSendVerificationCodeButtonLocBy, "Send Verification Code on CP Sign UP Page", "Verify Send Verification Code button color on CP Sign UP Page", isScrollIntoViewRequired: false);

                webElementExtensions.EnterText(_driver, commonServices.cpFirstNameTextInputLocBy, $"AutomatedFirstName{timestamp}", false, "First Name On CP Sign Up Page", true);
                webElementExtensions.EnterText(_driver, commonServices.cpLastNameTextInputLocBy, "AutomatedLastName", false, "Last Name On CP Sign Up Page", true);
                webElementExtensions.EnterText(_driver, commonServices.cpUsernameLocBy, emailToUseForRegistration, false, "Email Id On CP Sign Up Page", true);
                webElementExtensions.IsElementEnabled(_driver, commonServices.cpSendVerificationCodeButtonLocBy, "Send Verification Code on CP Sign UP Page");
                webElementExtensions.VerifyElementColor(Constants.Colors.NeonViolet, commonServices.cpSendVerificationCodeButtonLocBy, "Send Verification Code on CP Sign UP Page", "Verify Send Verification Code button color on CP Sign UP Page", isScrollIntoViewRequired: false);
                webElementExtensions.ActionClick(_driver, commonServices.cpSendVerificationCodeButtonLocBy, "Send Varification Code Button", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.cpSendVerificationCodeButtonLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.cpVerificationCodeInputLocBy, ConfigSettings.WaitTime);

                //Method to fetch the code from Yopmail.com
                verificationCodeFromYopmail = yopmail.GetEmailContentFromYopmail(emailToUseForRegistration);
                webElementExtensions.VerifyElementColor(Constants.Colors.NeutralGrey, commonServices.cpVerifYCodeButtonLocBy, "Verify Code Button on CP Sign UP Page", "Verify Verify Code Button color on CP Sign UP Page", isScrollIntoViewRequired: false);
                webElementExtensions.IsElementDisabled(_driver, commonServices.cpVerifYCodeButtonLocBy, "Verify Code Button", false);
                webElementExtensions.EnterText(_driver, commonServices.cpVerificationCodeInputLocBy, verificationCodeFromYopmail, false, "Verification Code On CP Sign Up Page", true);
                webElementExtensions.VerifyElementColor(Constants.Colors.NeonViolet, commonServices.cpVerifYCodeButtonLocBy, "Verify Code Button on CP Sign UP Page", "Verify Verify Code Button color on CP Sign UP Page", isScrollIntoViewRequired: false);
                webElementExtensions.IsElementEnabled(_driver, commonServices.cpVerifYCodeButtonLocBy, "Verify Code Button", false);
                reportLogger.TakeScreenshot(test, "Verify Code button on Sign Up Page after entering the verification Code");
                webElementExtensions.ActionClick(_driver, commonServices.cpVerifYCodeButtonLocBy, "Verify Code Button", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.cpVerifYCodeButtonLocBy, ConfigSettings.SmallWaitTime);

                webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.cpNewPasswordInputFieldLocBy, 2);
                webElementExtensions.EnterText(_driver, commonServices.cpNewPasswordInputFieldLocBy, "NewPassWord@12345", false, "New Password On CP Sign Up Page", isClickRequired: true);
                webElementExtensions.VerifyElementColor(Constants.Colors.NeutralGrey, commonServices.cpCreateAndSignInButtonLocBy, "Create and Sign In Button on CP Sign UP Page", "Verify Create and Sign In Button color on CP Sign UP Page", isScrollIntoViewRequired: false);
                webElementExtensions.IsElementDisabled(_driver, commonServices.cpCreateAndSignInButtonLocBy, "Create and Sign In Button", false);
                webElementExtensions.EnterText(_driver, commonServices.cpReEnterNewPasswordInputFieldLocBy, "NewPassWord@12345", false, "Confirm New Password On CP Sign Up Page");
                webElementExtensions.VerifyElementColor(Constants.Colors.NeonViolet, commonServices.cpCreateAndSignInButtonLocBy, "Create and Sign In Button", "Verify Create and Sign In Button color on CP Sign UP Page", isScrollIntoViewRequired: false);
                webElementExtensions.IsElementEnabled(_driver, commonServices.cpCreateAndSignInButtonLocBy, "Create and Sign In Button", false);
                reportLogger.TakeScreenshot(test, "Sign Up Page after entering the details");

                webElementExtensions.ActionClick(_driver, commonServices.cpCreateAndSignInButtonLocBy, "Create and Sign In button on CP Sign Up Page", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.cpCreateAndSignInButtonLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitUntilUrlContains("/link-user-loan");
                ReportingMethods.LogAssertionTrue(test, _driver.Url.ToString().Contains("/link-user-loan"), "Verify if the url contains: /link-user-loan");
                reportLogger.TakeScreenshot(test, "Link A Loan Page after successful Registration and SignIn");
                test.Log(Status.Info, $"<b>***********************************<u>Ending the process to Validate Registration Feature</u>***********************************</b>");

            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR 1437 Validate loan status for an Ontime loan after 12am PST on 2nd of the month")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity"), TestCategory("CP_CutoffTime")]
        public void TPR_1437_2003_2265_2147_TPR_ValidateLoanStatusOntimeLoanAfter12amPSTOn2ndOfTheMonth()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {

                #region TestData

                test.Log(Status.Info, $"Query Used: {loanLevelDetailsForOntimePastdue} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForOntimePastdue, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
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
                        dashboard.HandleServiceChatBot();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        retryCount++;
                    }
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    test.Log(Status.Info, "<b>********************************************<u>Starting Account Standing Status Checks</u>*******************************************</b>");
                }
                var currentCstTime = UtilAdditions.GetCSTTimeNow();

                bool statusExists = webElementExtensions.IsElementDisplayed(_driver, dashboard.accountStandingValueTextLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ScrollToTop(_driver);
                test.Log(Status.Info, $"<b>********************************************<u>Check Account Status</u>********************************************</b>");
                reportLogger.TakeScreenshot(test, $"Dashboard Page");
                ReportingMethods.Log(test, $"CST Time now :{currentCstTime}");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                var status = webElementExtensions.GetElementText(_driver, payments.loanStandingStatusLocBy);
                reportLogger.TakeScreenshot(test, $"Make Payment Page");

                if (currentCstTime.Day >= 2 && currentCstTime.Day <= 16)
                {
                    ReportingMethods.Log(test, $"Account Standing should not present because CST time now is : {currentCstTime}");
                    ReportingMethods.LogAssertionFalse(test, statusExists, $"Dashboard : Account Standing is not be available");
                    ReportingMethods.LogAssertionFalse(test, status.Length > 0, $"Make Payment : Account Standing Status is not be available");
                }
                else
                {
                    ReportingMethods.Log(test, $"Account Standing should present because CST time now is : {currentCstTime}");
                    ReportingMethods.LogAssertionTrue(test, statusExists, "Dashboard : Account Standing should be available");
                    ReportingMethods.LogAssertionTrue(test, status.Length > 0, "Make Payment : Account Standing should be available");
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
        [Description("<br>TPR 2136 Verify loan status changes after 11:59PM PST on every first day of the month for First Mortgage from Past Due to Del")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity"), TestCategory("CP_CutoffTime")]
        public void TPR_2136_2125_1436_TPR_ValidateLoanStatusAfter1159PMPSTOnFirstOfMonthFromPastDueToDel()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                //Replace date with required date value
                var pFrom = loanLevelDetailsForOntimePastdue.IndexOf("next_payment_due_date =") + "next_payment_due_date =".Length;
                var pTo = loanLevelDetailsForOntimePastdue.LastIndexOf("and", StringComparison.OrdinalIgnoreCase);

                var dateComponent = loanLevelDetailsForOntimePastdue.Substring(pFrom, pTo - pFrom);

                var lastMonthDate = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).AddMonths(-1).ToString("yyyy-MM-dd");

                var loanLevelDetailsPastdueDel = loanLevelDetailsForOntimePastdue.Replace(dateComponent, $"'{lastMonthDate}' ");

                test.Log(Status.Info, $"Query Used: {loanLevelDetailsPastdueDel} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsPastdueDel, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {

                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
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
                        dashboard.HandleServiceChatBot();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        retryCount++;
                    }
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    test.Log(Status.Info, "<b>********************************************<u>Starting Account Standing Status Checks</u>*******************************************</b>");
                }
                var currentCstTime = UtilAdditions.GetCSTTimeNow();

                bool statusExists = webElementExtensions.IsElementDisplayed(_driver, dashboard.accountStandingValueTextLocBy, isScrollIntoViewRequired: false);
                string status = webElementExtensions.GetElementText(_driver, dashboard.accountStandingValueTextLocBy);
                webElementExtensions.ScrollToTop(_driver);
                test.Log(Status.Info, $"<b>********************************************<u>Check Account Status</u>********************************************</b>");
                reportLogger.TakeScreenshot(test, $"Dashboard Page");
                ReportingMethods.Log(test, $"CST Time now :{currentCstTime}");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                var standingStatus = webElementExtensions.GetElementText(_driver, payments.loanStandingStatusLocBy);
                var banner = webElementExtensions.GetElementText(_driver, payments.statusBannerLocBy);
                reportLogger.TakeScreenshot(test, $"Make Payment Page");

                if (currentCstTime.Day >= 2)
                {
                    ReportingMethods.Log(test, $"Account Standing As Delinquent should show because CST time now is : {currentCstTime}");
                    ReportingMethods.LogAssertionTrue(test, statusExists, $"Dashboard : Account Standing should be available");
                    ReportingMethods.LogAssertionEqual(test, "Delinquent", status, $"Account Standing Status is Delinquent");
                    ReportingMethods.LogAssertionContains(test, "Past Due", standingStatus, $"Account Standing Status in Make Payment screen is Past Due");
                    ReportingMethods.LogAssertionTrue(test, banner.Length > 0, $"Make Payment : Banner is showing {standingStatus}");
                }
                else
                {
                    ReportingMethods.Log(test, $"Account Standing should be Pastdue because CST time now is : {currentCstTime}");
                    ReportingMethods.LogAssertionTrue(test, statusExists, "Dashboard : Account Standing should be available");
                    ReportingMethods.LogAssertionEqual(test, "Past Due", status, $"Account Standing Status is Past Due");
                    ReportingMethods.LogAssertionContains(test, "Past Due", standingStatus, $"Account Standing Status in Make Payment Screen is Past Due");
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
        [Description("<br>TPR 2133 Verify loan status changes after 11:59PM PST on every first day of the month for First Mortgage from Del to Del+")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity"), TestCategory("CP_CutoffTime")]
        public void TPR_2133_2141_TPR_ValidateLoanStatusAfter1159PMPSTOnFirstOfMonthFromDelToDelPlus()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData
                //Replace date with required date value
                var pFrom = loanLevelDetailsForOntimePastdue.IndexOf("next_payment_due_date =") + "next_payment_due_date =".Length;
                var pTo = loanLevelDetailsForOntimePastdue.LastIndexOf("and", StringComparison.OrdinalIgnoreCase);

                var dateComponent = loanLevelDetailsForOntimePastdue.Substring(pFrom, pTo - pFrom);

                var lastMonthDate = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).AddMonths(-2).ToString("yyyy-MM-dd");

                var loanLevelDetailsDelDelPlus = loanLevelDetailsForOntimePastdue.Replace(dateComponent, $"'{lastMonthDate}' ");

                test.Log(Status.Info, $"Query Used: {loanLevelDetailsDelDelPlus} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsDelDelPlus, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {

                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
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
                        dashboard.HandleServiceChatBot();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.payoffQuoteLocBy))
                            break;
                        retryCount++;
                    }

                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                    test.Log(Status.Info, "<b>********************************************<u>Starting Account Standing Status Checks</u>*******************************************</b>");
                }
                var currentCstTime = UtilAdditions.GetCSTTimeNow();

                bool statusExists = webElementExtensions.IsElementDisplayed(_driver, dashboard.accountStandingValueTextLocBy, isScrollIntoViewRequired: false);
                string status = webElementExtensions.GetElementText(_driver, dashboard.accountStandingValueTextLocBy);
                webElementExtensions.ScrollToTop(_driver);
                test.Log(Status.Info, $"<b>********************************************<u>Check Account Status</u>********************************************</b>");
                reportLogger.TakeScreenshot(test, $"Dashboard Page");
                ReportingMethods.Log(test, $"CST Time now :{currentCstTime}");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                var standingStatus = webElementExtensions.GetElementText(_driver, payments.loanStandingStatusLocBy);
                var banner = webElementExtensions.GetElementText(_driver, payments.statusBannerLocBy);
                reportLogger.TakeScreenshot(test, $"Make Payment Page");

                if (currentCstTime.Day >= 2)
                {
                    ReportingMethods.Log(test, $"Account Standing As Delinquent should show because CST time now is : {currentCstTime}");
                    ReportingMethods.LogAssertionTrue(test, statusExists, $"Dashboard : Account Standing should be available");
                    ReportingMethods.LogAssertionEqual(test, "Delinquent", status, $"Account Standing Status is Delinquent");
                    ReportingMethods.LogAssertionContains(test, "Delinquent", standingStatus, $"Account Standing Status in Make Payment screen is Delinquent");
                    ReportingMethods.LogAssertionTrue(test, banner.Length > 0, $"Make Payment : Banner is showing {standingStatus}");
                }
                else
                {
                    ReportingMethods.Log(test, $"Account Standing should be Pastdue because CST time now is : {currentCstTime}");
                    ReportingMethods.LogAssertionTrue(test, statusExists, "Dashboard : Account Standing should be available");
                    ReportingMethods.LogAssertionEqual(test, "Delinquent", status, $"Account Standing Status is Delinquent");
                    ReportingMethods.LogAssertionContains(test, "Past Due", standingStatus, $"Account Standing Status in Make Payment Screen is Past Due");
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
        [Description("<br>TPR 2928 Validate loan status for an Ontime loan after 12am PST on 2nd of the month")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity"), TestCategory("CP_CutoffTime")]
        public void TPR_2928_2929_2930_2931_TPR_ValidateHelocLoanStatusOntimeLoanAfter12amPSTOn2ndOfTheMonth()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                test.Log(Status.Info, $"Query Used: {loanLevelDetailsForOntimePastdueHeloc} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForOntimePastdueHeloc, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {

                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
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
                        dashboard.HandleServiceChatBot();
                        break;
                        retryCount++;
                    }
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                    test.Log(Status.Info, "<b>********************************************<u>Starting Account Standing Status Checks</u>*******************************************</b>");
                }
                var currentCstTime = UtilAdditions.GetCSTTimeNow();

                bool statusExists = webElementExtensions.IsElementDisplayed(_driver, dashboard.helocLoanStandingLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ScrollToTop(_driver);
                test.Log(Status.Info, $"<b>********************************************<u>Check Account Status</u>********************************************</b>");
                reportLogger.TakeScreenshot(test, $"Dashboard Page");
                ReportingMethods.Log(test, $"CST Time now :{currentCstTime}");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.helocStandingStatusLocBy);
                var status = webElementExtensions.GetElementText(_driver, payments.helocStandingStatusLocBy);
                reportLogger.TakeScreenshot(test, $"Make Payment Page");

                if (currentCstTime.Day >= 2 && currentCstTime.Day <= 16)
                {
                    ReportingMethods.Log(test, $"Account Standing should not present because CST time now is : {currentCstTime}");
                    ReportingMethods.LogAssertionFalse(test, statusExists, $"Dashboard : Account Standing should not available");
                    ReportingMethods.LogAssertionFalse(test, status.Length > 0, $"Make Payment : Account Standing Status should not available");
                }
                else
                {
                    ReportingMethods.Log(test, $"Account Standing should present because CST time now is : {currentCstTime}");
                    ReportingMethods.LogAssertionTrue(test, statusExists, "Dashboard : Account Standing should be available");
                    ReportingMethods.LogAssertionTrue(test, status.Length > 0, "Make Payment : Account Standing should be available");
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
        [Description("<br>TPR 2209 Verify Make a payment shouldnot be enabled during cutoff time of 12am PST (HELOC Delinquent Plus Loan)")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity"), TestCategory("CP_CutoffTime")]
        public void TPR_2209_TPR_ValidateHelocLoanStatusDelinquentLoanAfter12amPSTOn2ndOfTheMonth()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();

            try
            {
                #region TestData

                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();

                //Replace date with required date value
                var pFrom = loanLevelDetailsForOntimePastdueHeloc.IndexOf("next_payment_due_date =") + "next_payment_due_date =".Length;
                var pTo = loanLevelDetailsForOntimePastdueHeloc.LastIndexOf("and", StringComparison.OrdinalIgnoreCase);

                var dateComponent = loanLevelDetailsForOntimePastdueHeloc.Substring(pFrom, pTo - pFrom);

                var lastMonthDate = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).AddMonths(-2).ToString("yyyy-MM-dd");

                var loanLevelDetailsDelplus = loanLevelDetailsForOntimePastdueHeloc.Replace(dateComponent, $"'{lastMonthDate}' ");

                test.Log(Status.Info, $"Query Used: {loanLevelDetailsDelplus} ");

                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsDelplus, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {

                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
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
                        dashboard.HandleServiceChatBot();
                        break;
                        retryCount++;
                    }
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                    test.Log(Status.Info, "<b>********************************************<u>Starting Account Standing Status Checks</u>*******************************************</b>");
                }
                var currentCstTime = UtilAdditions.GetCSTTimeNow();

                bool statusExists = webElementExtensions.IsElementDisplayed(_driver, dashboard.helocLoanStandingLocBy, isScrollIntoViewRequired: false);
                var status = webElementExtensions.GetElementText(_driver, dashboard.helocLoanStandingLocBy);
                var ineligibleMsg = webElementExtensions.GetElementText(_driver, dashboard.helocNotEligibleMsgLocBy);
                webElementExtensions.ScrollToTop(_driver);
                test.Log(Status.Info, $"<b>********************************************<u>Check Account Status</u>********************************************</b>");
                reportLogger.TakeScreenshot(test, $"Dashboard Page");
                ReportingMethods.Log(test, $"CST Time now :{currentCstTime}");


                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                var isMakePaymentDisabled = webElementExtensions.IsElementDisabled(_driver, dashboard.btnHelocMakeApaymentLocBy);

                if (currentCstTime.Day >= 2)
                {
                    ReportingMethods.Log(test, $"Account Standing should present");
                    ReportingMethods.LogAssertionTrue(test, statusExists, $"Dashboard : Account Standing available");
                    ReportingMethods.LogAssertionContains(test, "Delinquent", status, "Loan Status should be Delinquent");
                    ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.HelocIneligibleForPayment, ineligibleMsg, "InEligible msg should present because loan became Delinquent+");
                    ReportingMethods.LogAssertionTrue(test, isMakePaymentDisabled, $"Make a Payment should be disabled");
                }
                else
                {

                    bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                    int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                    List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();

                    commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    if (willScheduledPaymentInProgressPopAppear)
                        payments.AcceptScheduledPaymentIsProcessingPopUp();
                    var statusAtPayment = webElementExtensions.GetElementText(_driver, payments.helocStandingStatusLocBy);
                    reportLogger.TakeScreenshot(test, $"Make Payment Page");

                    ReportingMethods.Log(test, $"Account Standing should present because CST time now is : {currentCstTime}");
                    ReportingMethods.LogAssertionTrue(test, statusExists, $"Dashboard : Account Standing available");
                    ReportingMethods.LogAssertionContains(test, "Delinquent", status, "Loan Status should be Delinquent");
                    ReportingMethods.LogAssertionFalse(test, isMakePaymentDisabled, $"Make a Payment should be enabled");
                    ReportingMethods.LogAssertionContains(test, "Delinquent", statusAtPayment, $"Delinquent Staus should show in Make a Payment screen");

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
        [Description("<br>TPR 1444 Verify Make a payment shouldnot be enabled during cutoff time of 12am PST (HELOC Delinquent Plus Loan)")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity"), TestCategory("CP_CutoffTime")]
        public void TPR_1444_TPR_ValidateHelocLoanStatusPastdueToDelinquentLoanAfter12amPSTOn2ndOfTheMonth()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();

            try
            {
                #region TestData

                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();

                //Replace date with required date value
                var pFrom = loanLevelDetailsForOntimePastdueHeloc.IndexOf("next_payment_due_date =") + "next_payment_due_date =".Length;
                var pTo = loanLevelDetailsForOntimePastdueHeloc.LastIndexOf("and", StringComparison.OrdinalIgnoreCase);

                var dateComponent = loanLevelDetailsForOntimePastdueHeloc.Substring(pFrom, pTo - pFrom);

                var lastMonthDate = DateTime.Today.AddDays(-(DateTime.Today.Day - 1)).AddMonths(-1).ToString("yyyy-MM-dd");

                var loanLevelDetailsPastdueDel = loanLevelDetailsForOntimePastdueHeloc.Replace(dateComponent, $"'{lastMonthDate}' ");

                test.Log(Status.Info, $"Query Used: {loanLevelDetailsPastdueDel} ");

                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsPastdueDel, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {

                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
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
                        dashboard.HandleServiceChatBot();
                        break;
                        retryCount++;
                    }
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    test.Log(Status.Info, "<b>********************************************<u>Starting Account Standing Status Checks</u>*******************************************</b>");
                }
                var currentCstTime = UtilAdditions.GetCSTTimeNow();

                bool statusExists = webElementExtensions.IsElementDisplayed(_driver, dashboard.helocLoanStandingLocBy, isScrollIntoViewRequired: false);
                var status = webElementExtensions.GetElementText(_driver, dashboard.helocLoanStandingLocBy);
                webElementExtensions.ScrollToTop(_driver);
                test.Log(Status.Info, $"<b>********************************************<u>Check Account Status</u>********************************************</b>");
                reportLogger.TakeScreenshot(test, $"Dashboard Page");
                ReportingMethods.Log(test, $"CST Time now :{currentCstTime}");


                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                var isMakePaymentEnabled = webElementExtensions.IsElementEnabled(_driver, dashboard.btnHelocMakeApaymentLocBy);
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.loanStandingStatusLocBy);
                var statusAtPayment = webElementExtensions.GetElementText(_driver, payments.loanStandingStatusLocBy);
                webElementExtensions.ScrollToTop(_driver);
                reportLogger.TakeScreenshot(test, $"Make Payment Page");

                if (currentCstTime.Day >= 2)
                {

                    ReportingMethods.Log(test, $"Account Standing should present");
                    ReportingMethods.LogAssertionTrue(test, statusExists, $"Dashboard : Account Standing available");
                    ReportingMethods.LogAssertionContains(test, "Delinquent", status, "Loan Status should be Delinquent");
                    ReportingMethods.LogAssertionTrue(test, isMakePaymentEnabled, $"Make a Payment should be enabled");
                    ReportingMethods.LogAssertionContains(test, "Delinquent", statusAtPayment, $"Delinquent Staus should show in Make a Payment screen");


                }
                else
                {

                    ReportingMethods.Log(test, $"Account Standing should present");
                    ReportingMethods.LogAssertionTrue(test, statusExists, $"Dashboard : Account Standing should be available");
                    ReportingMethods.LogAssertionContains(test, "Past Due", status, "Loan Status should be Past Due");
                    ReportingMethods.LogAssertionTrue(test, isMakePaymentEnabled, $"Make a Payment should be enabled");
                    ReportingMethods.LogAssertionContains(test, "Past Due", statusAtPayment, $"Past Due Staus should show in Make a Payment screen");

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
        [Description("<br>TPR-2169-Verify forgot email for LD loans with borrower ssn")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_2169_TPR_VerifyForgotEmailFeature()
        {
            int loansFound = 0;
            int retryCount = 0;
            bool retry = true;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            string emailToUseForRegistration = string.Empty; string verificationCodeFromYopmail = string.Empty;

            #region TestData
            // Execute the query and retrieve the loan data
            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loansLevelDetailsForSMCVerification}  </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loansLevelDetailsForSMCVerification, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
            try
            {
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

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
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
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
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                commonServices.LogoutOfTheApplication();
                webElementExtensions.WaitUntilUrlContains("b2c_1_unified_signup_signin");
                webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.cpForgotYourEmailLinkLocBy, "Forgot Email Link", ConfigSettings.WaitTime, isScrollIntoViewRequired: false);
                ReportingMethods.LogAssertionTrue(test, _driver.Url.ToString().Contains("b2c_1_unified_signup_signin"), "Verify if the url contains: b2c_1_unified_signup_signin");
                webElementExtensions.ActionClick(_driver, commonServices.cpForgotYourEmailLinkLocBy, "Forgot Email Link", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.cpForgotYourEmailLinkLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.cpVerifyButtonOnForgotYourEmailPageLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "Forgot Email Page Before entering the details");
                ReportingMethods.LogAssertionTrue(test, _driver.Url.ToString().Contains("forgot-email"), "Verify if the url contains: forgot-email");
                webElementExtensions.IsElementDisabled(_driver, commonServices.cpVerifyButtonOnForgotYourEmailPageLocBy, "Verify Button is Disabled on CP Forgot Email Page");
                webElementExtensions.VerifyElementColor(Constants.Colors.LightGrey, commonServices.cpVerifyButtonOnForgotYourEmailPageLocBy, "Verify Button on CP Forgot Email Page", "Verify Verify Button color on CP Forgot Email Page", isScrollIntoViewRequired: false);
                webElementExtensions.EnterText(_driver, commonServices.cpLinkYourExistingLoanPage_LoanNumberFieldLocBy, loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), false, "Loan Lumber Field on Forgot Email");
                webElementExtensions.EnterText(_driver, commonServices.cpLinkYourExistingLoanPage_ZipcodeFieldLocBy, loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), false, "Zip Field on Forgot Email");
                webElementExtensions.EnterText(_driver, commonServices.cpLinkYourExistingLoanPage_SsnNumberLocBy, loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString().Length - 4), false, "Ssn Field on Forgot Email");
                webElementExtensions.IsElementEnabled(_driver, commonServices.cpVerifyButtonOnForgotYourEmailPageLocBy, "Verify Button is Enabled on CP Forgot Email Page");
                webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.cpVerifyButtonOnForgotYourEmailPageLocBy, 2);
                reportLogger.TakeScreenshot(test, "Verify Button Color");
                webElementExtensions.ActionClick(_driver, commonServices.cpVerifyButtonOnForgotYourEmailPageLocBy, "Verify Button on CP Forgot Email Page", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.cpVerifyButtonOnForgotYourEmailPageLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.cpForgotEmailVerifiedPageLocBy, ConfigSettings.WaitTime);
                string actualVerifiedPageTextContent = webElementExtensions.GetElementText(_driver, commonServices.cpForgotEmailVerifiedPageLocBy, true);
                string masked = Regex.Replace(username, @"(^.{2})[^@]*(?=@)|(?<=@.{2})[^.]+", m => m.Groups[1].Success ? m.Groups[1].Value + "****" : "****");
                string expectedTextContent = $"FORGOT EMAIL\r\nVerified\r\nYour identity has been verified.\r\nThe email address associated with your account is:\r\n{masked}\r\nThis email address has been masked for added security.\r\nSIGN IN";
                ReportingMethods.LogAssertionEqual(test, expectedTextContent, actualVerifiedPageTextContent, "Verify the Forgot Email Verified Text content");
                webElementExtensions.ActionClick(_driver, commonServices.cpSignInOnForgotEmailVerifiedPageButtonLocBy, "SignIn Button on Forgot Email Verified Page", isReportRequired: true);
                webElementExtensions.WaitUntilUrlContains("b2c_1_unified_signup_signin");
                commonServices.LoginToTheApplication(username, password);
                test.Log(Status.Info, $"<b>***********************************<u>Ending the process to Validate Forgot Email Feature</u>***********************************</b>");
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
        [Description("<br>TPR-973-Test Forgot Password- [B2C] - Changes in Azure B2C email communications")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_973_TPR_VerifyForgotPasswordFeature()
        {
            List<Hashtable> loanLevelData = new List<Hashtable>();
            string emailToUseForRegistration = string.Empty; string verificationCodeFromYopmail = string.Empty;
            try
            {
                webElementExtensions.WaitUntilUrlContains("b2c_1_unified_signup_signin");
                webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.cpForgotYourPasswordLinkLocBy, "Forgot Your Password Link", ConfigSettings.WaitTime, isScrollIntoViewRequired: false);
                ReportingMethods.LogAssertionTrue(test, _driver.Url.ToString().Contains("b2c_1_unified_signup_signin"), "Verify if the url contains: b2c_1_unified_signup_signin");
                webElementExtensions.ActionClick(_driver, commonServices.cpForgotYourPasswordLinkLocBy, "Forgot Your Password Link", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.cpForgotYourPasswordLinkLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.cpForgotYourPasswordLinkLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "PASSWORD RESET: 1 of 3");
                ReportingMethods.LogAssertionTrue(test, _driver.Url.ToString().Contains("B2C_1_UNIFIED_RESET_PASSWORD"), "Verify if the url contains: forgot-email");
                webElementExtensions.IsElementDisabled(_driver, commonServices.cpSendVerificationCodeButtonLocBy, "Send verification Code on Password Reset");
                webElementExtensions.EnterText(_driver, commonServices.cpEmailOnPasswordResetPageLocBy, username, false, "Email Address Input Field", true);
                webElementExtensions.IsElementEnabled(_driver, commonServices.cpSendVerificationCodeButtonLocBy, "Send verification Code on Password Reset");
                webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.cpSendVerificationCodeButtonLocBy, 2);
                webElementExtensions.VerifyElementColor(Constants.Colors.DeepPurple, commonServices.cpSendVerificationCodeButtonLocBy, "Send verification Code on Password Reset", isScrollIntoViewRequired: false);
                reportLogger.TakeScreenshot(test, "Send Verification Code Button");
                webElementExtensions.ActionClick(_driver, commonServices.cpSendVerificationCodeButtonLocBy, "Send verification Code on Password Reset", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.cpSendVerificationCodeButtonLocBy, ConfigSettings.SmallWaitTime);

                verificationCodeFromYopmail = yopmail.GetEmailContentFromYopmail(username);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.cpVerifYCodeButtonLocBy, ConfigSettings.WaitTime);
                webElementExtensions.IsElementDisabled(_driver, commonServices.cpVerifYCodeButtonLocBy, "Verify Code Button", false);
                webElementExtensions.EnterText(_driver, commonServices.cpVerificationCodeInputLocBy, verificationCodeFromYopmail, false, "Verification Code Input Field", true);
                webElementExtensions.IsElementEnabled(_driver, commonServices.cpVerifYCodeButtonLocBy, "Verify Code Button", false);
                webElementExtensions.ActionClick(_driver, commonServices.cpVerifYCodeButtonLocBy, "Verify Code Button", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.cpVerifYCodeButtonLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.cpCreateAndSignInButtonLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.cpCreateAndSignInButtonLocBy, "Continue Button", ConfigSettings.WaitTime);
                webElementExtensions.ActionClick(_driver, commonServices.cpCreateAndSignInButtonLocBy, "Continue Button", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.cpCreateAndSignInButtonLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.cpCreateYourPasswordContentLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.cpCreateYourPasswordContentLocBy, 5);

                string actualCreateYourPasswordPageTextContent = webElementExtensions.GetElementText(_driver, commonServices.cpCreateYourPasswordContentLocBy, true);
                ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.CreateYourPasswordTextContent, actualCreateYourPasswordPageTextContent, "Create Your Password Text content");
                reportLogger.TakeScreenshot(test, "Create Your Password Text Content");

                _driver.Navigate().GoToUrl(ConfigSettings.AppUrl);
                test.Log(Status.Info, $"Navigated to the URL: '{ConfigSettings.AppUrl}'.");
                commonServices.LoginToTheApplication(username, password);
                test.Log(Status.Info, $"<b>***********************************<u>Ending the process to Validate Forgot Email Feature</u>***********************************</b>");
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error : <b>{e.Message}</b>");
            }
            finally
            {
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-472-Verify API utterance response in Service Chatbot")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("CP_Sanity")]
        public void TPR_472_TPR_VerifyChatBotFeature()
        {
            int loansFound = 0;
            int retryCount = 0;
            bool retry = true;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            string emailToUseForRegistration = string.Empty; string verificationCodeFromYopmail = string.Empty;
            try
            {
                #region TestData
                // Execute the query and retrieve the loan data
                List<string> usedLoanTestData = new List<string>();
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {loansLevelDetailsForSMCVerification}  </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loansLevelDetailsForSMCVerification, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);

                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");

                        webElementExtensions.WaitForElementToBeEnabled(_driver, By.XPath(string.Format(dashboard.serviceBotMessangerLocString, "visible")), timeoutInSeconds: ConfigSettings.SmallWaitTime);
                        if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(string.Format(dashboard.serviceBotMessangerLocString, "visible"))))
                        {
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
                            break;
                        }
                        else
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
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
                        webElementExtensions.WaitForElementToBeEnabled(_driver, By.XPath(string.Format(dashboard.serviceBotMessangerLocString, "visible")), timeoutInSeconds: ConfigSettings.SmallWaitTime);
                        if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(string.Format(dashboard.serviceBotMessangerLocString, "visible"))))
                        {
                            dashboard.ClosePopUpsAfterLinkingNewLoan();
                            break;
                        }
                        else
                            dashboard.ClosePopUpsAfterLinkingNewLoan();

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
                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.messagesLinkLocBy, ConfigSettings.WaitTime);
                test.Log(Status.Info, $"<b>***********************************<u>Started the Validatation of Chat Bot Response for the API Utterences</u>***********************************</b>");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                dashboard.VerifyAPIUtterencesInServiceChatBot(loanLevelData[retryCount]);

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
        [Description("<br>TPR 2448 TPR-90-Test FM- Prompt to Enroll in Autopay when Scheduling OTP - Loan is Prepaid 2M, 1M, Ontime, Past Due")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments")]
        public void TPR_2448_TPR_ValidateFirstMortgageEnrollAutopayPrompt()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();

            #region TestData

            test.Log(Status.Info, $"Query Used: {loanLevelDetailsForAutopayPrompt} ");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanLevelDetailsForAutopayPrompt, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
            try
            {

                #endregion TestData

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {

                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
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
                        dashboard.HandleServiceChatBot();
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString()))
                            break;
                        retryCount++;
                    }
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                    test.Log(Status.Info, "<b>********************************************<u>Starting Account Standing Status Checks</u>*******************************************</b>");
                }

                webElementExtensions.ScrollToTop(_driver);
                reportLogger.TakeScreenshot(test, $"Dashboard Page");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();

                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                var promptMsg = webElementExtensions.GetElementText(_driver, payments.autopayPromptLocBy);
                reportLogger.TakeScreenshot(test, $"Make Payment Page");

                ReportingMethods.LogAssertionTrue(test, promptMsg.Length > 0, $"Payments Page : Autopay Prompt should be available");
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.AutopayPromptMsg, promptMsg, $"Prompt Message should have '{Constants.CustomerPortalTextMessages.AutopayPromptMsg}'");

                webElementExtensions.ClickElement(_driver, payments.autopayLinkInPrompt, "Setup Autopay Link");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                reportLogger.TakeScreenshot(test, $"Click Autopay Prompt Link");
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.continueBtnLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.continueBtnLocBy);
                webElementExtensions.WaitUntilUrlContains("autopay");
                ReportingMethods.LogAssertionTrue(test, _driver.Url.Contains("autopay"), "Should be navigated to Setup Autopay Page");
                reportLogger.TakeScreenshot(test, $"Setup Autopay Page");
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
        [Description("<br>TPR 2449 TPR-90-Test FM- Prompt to Enroll in Autopay when Scheduling OTP -Heloc Loan is Prepaid 2M, 1M, Ontime, Past Due")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments")]
        public void TPR_2449_TPR_ValidateHelocEnrollAutopayPrompt()
        {
            int retryCount = 0;
            List<string> usedLoanTestData = new List<string>();
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                test.Log(Status.Info, $"Query Used: {loanDetailsQueryForEligibleHelocAutopayOntime} ");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                loanDetailsQueryForEligibleHelocAutopayOntime = loanDetailsQueryForEligibleHelocAutopayOntime.Replace("<", "<=");
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

                // Login to app and link loans
                commonServices.LoginToTheApplication(username, password);
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {

                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitUntilUrlContains("link-user-loan");
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
                        dashboard.HandleServiceChatBot();
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                        if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true, true, true))
                            break;
                        retryCount++;
                    }
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }
                    test.Log(Status.Info, "<b>********************************************<u>Starting Account Standing Status Checks</u>*******************************************</b>");
                }

                webElementExtensions.ScrollToTop(_driver);
                reportLogger.TakeScreenshot(test, $"Dashboard Page");

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();

                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                var promptMsg = webElementExtensions.GetElementText(_driver, payments.autopayPromptLocBy);
                reportLogger.TakeScreenshot(test, $"Make Payment Page");

                ReportingMethods.LogAssertionTrue(test, promptMsg.Length > 0, $"Payments Page : Autopay Prompt should be available");
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.AutopayPromptMsg, promptMsg, $"Prompt Message should have '{Constants.CustomerPortalTextMessages.AutopayPromptMsg}'");

                webElementExtensions.ClickElement(_driver, payments.autopayLinkInPrompt, "Setup Autopay Link");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                reportLogger.TakeScreenshot(test, $"Click Autopay Prompt Link");
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.continueBtnLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.continueBtnLocBy);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.setupAutopaybuttonLocBy);

                ReportingMethods.LogAssertionTrue(test, _driver.Url.Contains("dashboard"), "Should be navigated to Setup Autopay Page");
                reportLogger.TakeScreenshot(test, $"Setup Autopay Page");

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
        [Description("<br>TPR 910 :Test Additional Principal Payment - Setup Autopay Monthly/Bi-weekly Payments for On-time/Prepaid One Month</br>")]
        [TestCategory("CP_Regression"), TestCategory("CP_AutoPay")]
        public void TPR_910_TPR_VerifyAddnlPrincipalValidation_FirstMortgage()
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
                webElementExtensions.ActionClick(_driver, commonServices.additionalMonthlyPrincipalCheckBoxInputLocBy);
                webElementExtensions.ClickElement(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy);
                webElementExtensions.EnterText(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, (1000000).ToString(), isScrollIntoViewRequired: true, isClickRequired: true, isReportRequired: true);
                var error = webElementExtensions.GetElementText(_driver, commonServices.additionalMonthlyPrincipalMaximumErrorMessageLocBy);
                reportLogger.TakeScreenshot(test, "Addnl Principal Error");
                ReportingMethods.LogAssertionEqual(test, Constants.Messages.MaximumAdditionalPrincipalPayment, error, $"Expected {Constants.Messages.MaximumAdditionalPrincipalPayment}. Actual {error}");

                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo");
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button");
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.ActionClick(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);

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
        [Description("<br><b>TPR-1441-Addnl Princiapl should not allow morethan 10000 for HELOC </b>")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocAutoPay")]
        public void TPR_1441_TPR_VerifyAddnlPrincipalValidation_HELOC()
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

                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);

                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.backToManageAutoPayButtonLocBy, isScrollIntoViewRequired: false);

                bool isAdditionalMonthlyPrincipalCheckBoxEnabled = webElementExtensions.IsElementEnabled(_driver, commonServices.additionalMonthlyPrincipalCheckBoxInputLocBy, isScrollIntoViewRequired: false);
                if (isAdditionalMonthlyPrincipalCheckBoxEnabled)
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.additionalMonthlyPrincipalCheckBoxLabelLocBy);
                    webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, "Additional Monthly Principal Input Text Box", isScrollIntoViewRequired: false);
                    webElementExtensions.EnterText(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, "1000000", isReportRequired: true, isClickRequired: true);
                }

                var error = webElementExtensions.GetElementText(_driver, commonServices.additionalMonthlyPrincipalMaximumErrorMessageLocBy);
                reportLogger.TakeScreenshot(test, "Addnl Principal Error");
                ReportingMethods.LogAssertionEqual(test, Constants.Messages.MaximumAdditionalPrincipalPayment, error, $"Expected {Constants.Messages.MaximumAdditionalPrincipalPayment}. Actual {error}");

                webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button");
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.ActionClick(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);

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
        [Description("<br>TPR 1163 : FM Monthly/Biweekly -Verify OTP with the same payment date when loan has a pending OTP FM Monthly/Biweekly -Verify OTP  with the same payment date when loan has a pending Autopay")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments")]
        public void TPR_1163_TPR_VerifySettingOTPWhenAutopayExistsOnSameDate()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                string formattedPaymentDate = string.Empty;
                DateTime paymentDate = new DateTime();
                List<string> columnsRequired = new List<string> { "loan_number", "borrower_ssn", "property_zip", "PaymentDate" };
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryforExisitngAutopay} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryforExisitngAutopay, null, columnDataRequired: columnsRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                    var paymentDateFromDb = loanLevelData[retryCount][Constants.PaymentSetupDataColumns.PaymentDate].ToString();
                    paymentDate = DateTime.ParseExact(paymentDateFromDb, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    var holidays = commonServices.GetBankHolidays(paymentDate.Year.ToString());

                    if (holidays.Contains(paymentDate))
                    {
                        test.Log(Status.Info, $"{loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString()} has autopay setup on Holiday {paymentDate}, skipping this loan");
                        retryCount++;
                        if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                        {
                            test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                            return;
                        }
                        continue;
                    }

                    formattedPaymentDate = paymentDate.ToString("MMMM d, yyyy");
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();

                        if (webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", "On Autopay"))
                            break;
                        retryCount++;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", "On Autopay"))
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
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"Exisiting Autopay Payment Date {formattedPaymentDate}");
                test.Log(Status.Info, $"<b>************************************<u>Started Process to Setup OTP</u>************************************</b>");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                List<string> availableDates = commonServices.GetAvailablePaymentDateInDateField();
                test.Log(Status.Info, $"Autopay is set on {paymentDate} and it is not Bank Holiday, hence expecting date selection for OTP should be allowed with error message");
                commonServices.SelectPaymentDateInDateField(formattedPaymentDate, selectNextMonth: true);
                reportLogger.TakeScreenshot(test, "Payment Date");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.paymentAlreadyExistErrorLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay, errorMsg, $"Error msg should be {Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay}");

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
        [Description("<br>TPR 1064 : Verify Autopay  with the same payment date when loan has a pending OTP")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments")]
        public void TPR_1064_TPR_VerifySettingAutopayWhenOTPExistsOnSameDate()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                string formattedPaymentDate = string.Empty;
                DateTime paymentDate = new DateTime();
                List<string> columnsRequired = new List<string> { "loan_number", "borrower_ssn", "property_zip", "PaymentDate" };
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryforExisitngOTP} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryforExisitngOTP, null, columnDataRequired: columnsRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                    var paymentDateFromDb = loanLevelData[retryCount][Constants.PaymentSetupDataColumns.PaymentDate].ToString();
                    paymentDate = DateTime.ParseExact(paymentDateFromDb, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                    formattedPaymentDate = paymentDate.ToString("MMMM d, yyyy");
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();

                        if (!dashboard.IsLoanHasAutopayToggleON())
                            break;
                        retryCount++;
                        continue;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (!dashboard.IsLoanHasAutopayToggleON())
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                        continue;
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
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                test.Log(Status.Info, $"Exisiting OTP Payment Date {formattedPaymentDate}");
                test.Log(Status.Info, $"<b>************************************<u>Started Process to Setup Autopay</u>************************************</b>");
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                test.Log(Status.Info, $"<b><u>Started Process to Setup Autopay Monthly plan</u></b>");
                payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);
                List<string> availableDates = commonServices.GetAvailablePaymentDateInDateField();
                commonServices.SelectPaymentDateInDateField(formattedPaymentDate, selectNextMonth: true);
                reportLogger.TakeScreenshot(test, "Payment Date");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.paymentAlreadyExistErrorLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay, errorMsg, $"Error msg should be {Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay}");

                webElementExtensions.ActionClick(_driver, dashboard.loanDepotLogoLocBy, "Loan Depot Logo");
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button");
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.ActionClick(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.leaveButtonOnLeavingPagePopupLocBy);


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
        [Description("<br>TPR 1483 : Heloc Verify OTP with the same payment date when loan has a pending OTP FM Monthly/Biweekly -Verify OTP  with the same payment date when loan has a pending Autopay")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments")]
        public void TPR_1483_TPR_VerifyHelocSettingOTPWhenAutopayExistsOnSameDate()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                string formattedPaymentDate = string.Empty;
                DateTime paymentDate = new DateTime();
                List<string> columnsRequired = new List<string> { "loan_number", "borrower_ssn", "property_zip", "PaymentDate" };
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryforExisitngAutopayHeloc} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryforExisitngAutopayHeloc, null, columnDataRequired: columnsRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                    var paymentDateFromDb = loanLevelData[retryCount][Constants.PaymentSetupDataColumns.PaymentDate].ToString();
                    paymentDate = DateTime.ParseExact(paymentDateFromDb, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                    var holidays = commonServices.GetBankHolidays(paymentDate.Year.ToString());

                    if (holidays.Contains(paymentDate))
                    {
                        test.Log(Status.Info, $"{loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString()} has autopay setup on Holiday {paymentDate}, skipping this loan");
                        retryCount++;
                        if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount >= loanLevelData.Count)
                        {
                            test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                            return;
                        }
                        continue;
                    }

                    formattedPaymentDate = paymentDate.ToString("MMMM d, yyyy");
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();

                        if (webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", "On Autopay"))
                            break;
                        retryCount++;
                        continue;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", "On Autopay"))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                        continue;
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
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"Exisiting Autopay Payment Date {formattedPaymentDate}");
                test.Log(Status.Info, $"<b>************************************<u>Started Process to Setup OTP</u>************************************</b>");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                test.Log(Status.Info, $"Autopay is set on {paymentDate} and it is not Bank Holiday, hence expecting date selection for OTP should be allowed with error message");
                commonServices.SelectPaymentDateInDateField(formattedPaymentDate, selectNextMonth: true);
                reportLogger.TakeScreenshot(test, "Payment Date");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.paymentAlreadyExistErrorLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay, errorMsg, $"Error msg should be {Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay}");

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
        [Description("<br>TPR 236 : Heloc Verify Autopay  with the same payment date when loan has a pending OTP")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments")]
        public void TPR_236_TPR_VerifyHelocSettingAutopayWhenOTPExistsOnSameDate()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                string formattedPaymentDate = string.Empty;
                DateTime paymentDate = new DateTime();
                List<string> columnsRequired = new List<string> { "loan_number", "borrower_ssn", "property_zip", "PaymentDate" };
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryforExisitngOTPHeloc} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryforExisitngOTPHeloc, null, columnDataRequired: columnsRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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
                    var paymentDateFromDb = loanLevelData[retryCount][Constants.PaymentSetupDataColumns.PaymentDate].ToString();
                    paymentDate = DateTime.ParseExact(paymentDateFromDb, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                    formattedPaymentDate = paymentDate.ToString("MMMM d, yyyy");
                    if (retryCount == 0)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.HandleServiceChatBot();
                        dashboard.ClosePopUpsAfterLinkingNewLoan();

                        if (!dashboard.IsLoanHasAutopayToggleON())
                            break;
                        retryCount++;
                        continue;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (!dashboard.IsLoanHasAutopayToggleON())
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                        continue;
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
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                test.Log(Status.Info, $"Exisiting OTP Payment Date {formattedPaymentDate}");
                test.Log(Status.Info, $"<b>************************************<u>Started Process to Setup Autopay</u>************************************</b>");
                webElementExtensions.ActionClick(_driver, dashboard.manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, payments.backToAccountSummaryLinkLocBy);
                test.Log(Status.Info, $"<b><u>Started Process to Setup Autopay Monthly plan</u></b>");
                payments.NavigateToSetupAutopayPage(willScheduledPaymentInProgressPopAppear);
                List<string> availableDates = commonServices.GetAvailablePaymentDateInDateField();
                commonServices.SelectPaymentDateInDateField(formattedPaymentDate, selectNextMonth: true);
                reportLogger.TakeScreenshot(test, "Payment Date");
                var errorMsg = webElementExtensions.GetElementText(_driver, payments.paymentAlreadyExistErrorLocBy);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay, errorMsg, $"Error msg should be {Constants.CustomerPortalErrorMsgs.PaymentAlreadyExistsOnSameDay}");

                webElementExtensions.ActionClick(_driver, payments.backToAccountSummaryLinkLocBy);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.leaveButtonOnLeavingPagePopupLocBy, "Leave Button");
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
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR 238 : HELOC Autopay Monthly - Verify user is not able to delete the Bank account when it has a pending payment")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("Bank_Delete")]
        public void TPR_238_TPR_VerifyHELOCBankAccountDeleteValidationWhenPendingAutopayPaymentExists()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                string formattedPaymentDate = string.Empty;
                DateTime paymentDate = new DateTime();
                List<string> columnsRequired = new List<string> { "loan_number", "borrower_ssn", "property_zip", "PaymentDate", "MaskedBankAccountNumber" };
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryforExisitngAutopayHeloc} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryforExisitngAutopayHeloc, null, columnDataRequired: columnsRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                        if (webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", "On Autopay"))
                            break;
                        retryCount++;
                        continue;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", "On Autopay"))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                        continue;
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
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                string maskedBankAccountNumber = loanLevelData[retryCount][Constants.PaymentSetupDataColumns.MaskedBankAccountNumber].ToString();
                test.Log(Status.Info, $"Exsiting Autopay Setup is using Bank Account Number : {maskedBankAccountNumber}");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();

                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);

                webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.manageBankAccountsLinkEnabledLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.manageBankAccountsLinkEnabledLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, commonServices.backToPaymentsButtonLocBy);

                string xpath = commonServices.deleteButtonBasedOnAccountNumber.Replace("<ACCOUNT_NUMBER>", maskedBankAccountNumber.Substring(maskedBankAccountNumber.Length - 4));
                reportLogger.TakeScreenshot(test, "Bank Accounts");
                var deleteIcon = _driver.FindElement(By.XPath(xpath));
                bool isValidationExists = false;
                if (deleteIcon != null)
                {
                    webElementExtensions.WaitForElementToBeEnabled(_driver, deleteIcon);
                    webElementExtensions.ClickElementUsingJavascript(_driver, deleteIcon);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);

                    webElementExtensions.WaitForElementToBeEnabled(
                        _driver,
                        commonServices.deleteButtonOnDeleteBankAccountPopupLocBy,
                        timeoutInSeconds: ConfigSettings.WaitTime
                    );

                    webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.deleteButtonOnDeleteBankAccountPopupLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);

                    if (webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.closeButtonOnBankAccountCannotBeDeletedPopupLocBy, timeoutInSeconds: 2))
                    {

                        var msg = webElementExtensions.GetElementText(_driver, dashboard.dialogLocBy);
                        isValidationExists = true;
                        reportLogger.TakeScreenshot(test, "Unable to Delete Bank");
                        ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.BankAccountDeleteErrorMsgHeloc, msg, $"Bank Account Delegte should check if payment is pending");
                        webElementExtensions.ClickElementUsingJavascript(
                            _driver,
                            commonServices.closeButtonOnBankAccountCannotBeDeletedPopupLocBy
                        );
                    }
                }

                ReportingMethods.LogAssertionTrue(test, isValidationExists, "Unable to Delete Bank should show if payment is pending from bank account");

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
        [Description("<br>TPR 3358 : FM Autopay Monthly - Verify user is not able to delete the Bank account when it has a pending payment")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("Bank_Delete")]
        public void TPR_3358_TPR_VerifyFMBankAccountDeleteValidationWhenPendingAutopayPaymentExists()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                string formattedPaymentDate = string.Empty;
                DateTime paymentDate = new DateTime();
                List<string> columnsRequired = new List<string> { "loan_number", "borrower_ssn", "property_zip", "PaymentDate", "MaskedBankAccountNumber" };
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryforExisitngAutopay} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryforExisitngAutopay, null, columnDataRequired: columnsRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                        if (webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", "On Autopay"))
                            break;
                        retryCount++;
                        continue;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.VerifyElementAttributeValue(_driver, dashboard.manageAutopayOnOffButtonLocBy, "alt", "On Autopay"))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                        continue;
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
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                string maskedBankAccountNumber = loanLevelData[retryCount][Constants.PaymentSetupDataColumns.MaskedBankAccountNumber].ToString();
                test.Log(Status.Info, $"Exsiting Autopay Setup is using Bank Account Number : {maskedBankAccountNumber}");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();

                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);

                webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.manageBankAccountsLinkEnabledLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.manageBankAccountsLinkEnabledLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, commonServices.backToPaymentsButtonLocBy);

                string xpath = commonServices.deleteButtonBasedOnAccountNumber.Replace("<ACCOUNT_NUMBER>", maskedBankAccountNumber.Substring(maskedBankAccountNumber.Length - 4));
                reportLogger.TakeScreenshot(test, "Bank Accounts");
                var deleteIcon = _driver.FindElement(By.XPath(xpath));
                bool isValidationExists = false;
                if (deleteIcon != null)
                {
                    webElementExtensions.WaitForElementToBeEnabled(_driver, deleteIcon);
                    webElementExtensions.ClickElementUsingJavascript(_driver, deleteIcon);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);

                    webElementExtensions.WaitForElementToBeEnabled(
                        _driver,
                        commonServices.deleteButtonOnDeleteBankAccountPopupLocBy,
                        timeoutInSeconds: ConfigSettings.WaitTime
                    );

                    webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.deleteButtonOnDeleteBankAccountPopupLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);

                    if (webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.closeButtonOnBankAccountCannotBeDeletedPopupLocBy, timeoutInSeconds: 2))
                    {

                        var msg = webElementExtensions.GetElementText(_driver, dashboard.dialogLocBy);
                        isValidationExists = true;
                        reportLogger.TakeScreenshot(test, "Unable to Delete Bank");
                        ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.BankAccountDeleteErrorMsgFM, msg, $"Bank Account Delegte should check if payment is pending");
                        webElementExtensions.ClickElementUsingJavascript(
                            _driver,
                            commonServices.closeButtonOnBankAccountCannotBeDeletedPopupLocBy
                        );
                    }
                }

                ReportingMethods.LogAssertionTrue(test, isValidationExists, "Unable to Delete Bank should show if payment is pending from bank account");

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
        [Description("<br>TPR 2015 : HELOC:Verify user is not able to delete the Bank account when it has a pending OTP payment")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("Bank_Delete")]
        public void TPR_2015_TPR_VerifyHelocBankAccountDeleteValidationWhenOTPPendingPaymentExists()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                string formattedPaymentDate = string.Empty;
                DateTime paymentDate = new DateTime();
                List<string> columnsRequired = new List<string> { "loan_number", "borrower_ssn", "property_zip", "PaymentDate", "MaskedBankAccountNumber" };
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryforExsitingOtpHelocBankValidation} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryforExsitingOtpHelocBankValidation, null, columnDataRequired: columnsRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.btnHelocMakeApaymentLocBy))
                            break;
                        retryCount++;
                        continue;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.btnHelocMakeApaymentLocBy))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                        continue;
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
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                string maskedBankAccountNumber = loanLevelData[retryCount][Constants.PaymentSetupDataColumns.MaskedBankAccountNumber].ToString();
                test.Log(Status.Info, $"Exsiting OTP Setup is using Bank Account Number : {maskedBankAccountNumber}");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();

                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);

                webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.manageBankAccountsLinkEnabledLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.manageBankAccountsLinkEnabledLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, commonServices.backToPaymentsButtonLocBy);

                string xpath = commonServices.deleteButtonBasedOnAccountNumber.Replace("<ACCOUNT_NUMBER>", maskedBankAccountNumber.Substring(maskedBankAccountNumber.Length - 4));
                reportLogger.TakeScreenshot(test, "Bank Accounts");
                var deleteIcon = _driver.FindElement(By.XPath(xpath));
                bool isValidationExists = false;
                if (deleteIcon != null)
                {
                    webElementExtensions.WaitForElementToBeEnabled(_driver, deleteIcon);
                    webElementExtensions.ClickElementUsingJavascript(_driver, deleteIcon);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);

                    webElementExtensions.WaitForElementToBeEnabled(
                        _driver,
                        commonServices.deleteButtonOnDeleteBankAccountPopupLocBy,
                        timeoutInSeconds: ConfigSettings.WaitTime
                    );

                    webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.deleteButtonOnDeleteBankAccountPopupLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);

                    if (webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.closeButtonOnBankAccountCannotBeDeletedPopupLocBy, timeoutInSeconds: 2))
                    {

                        var msg = webElementExtensions.GetElementText(_driver, dashboard.dialogLocBy);
                        isValidationExists = true;
                        reportLogger.TakeScreenshot(test, "Unable to Delete Bank");
                        ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.BankAccountDeleteErrorMsgHeloc, msg, $"Bank Account Delegte should check if payment is pending");
                        webElementExtensions.ClickElementUsingJavascript(
                            _driver,
                            commonServices.closeButtonOnBankAccountCannotBeDeletedPopupLocBy
                        );
                    }
                }

                ReportingMethods.LogAssertionTrue(test, isValidationExists, "Unable to Delete Bank should show if payment is pending from bank account");

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
        [Description("<br>TPR 3357 : FM:Verify user is not able to delete the Bank account when it has a pending OTP payment")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments"), TestCategory("Bank_Delete")]
        public void TPR_3357_TPR_VerifyFMBankAccountDeleteValidationWhenOTPPendingPaymentExists()
        {
            int retryCount = 0;
            string maskedBankAccountNumber = string.Empty;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            try
            {
                #region TestData

                List<string> usedLoanTestData = new List<string>();
                string formattedPaymentDate = string.Empty;
                DateTime paymentDate = new DateTime();
                List<string> columnsRequired = new List<string> { "loan_number", "borrower_ssn", "property_zip", "PaymentDate", "MaskedBankAccountNumber" };
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryforExsitingOtpFMBankValidation} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryforExsitingOtpFMBankValidation, null, columnDataRequired: columnsRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.makeAPaymentButtonLocBy))
                            break;
                        retryCount++;
                        continue;
                    }

                    if (retryCount > 0)
                    {
                        dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilUrlContains("/dashboard");
                        dashboard.ClosePopUpsAfterLinkingNewLoan();
                        if (webElementExtensions.IsElementEnabled(_driver, dashboard.makeAPaymentButtonLocBy))
                            break;
                        dashboard.HandlePaperLessPage();
                        webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.manageAutopayButtonLocBy);
                        retryCount++;
                        continue;
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
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                maskedBankAccountNumber = loanLevelData[retryCount][Constants.PaymentSetupDataColumns.MaskedBankAccountNumber].ToString();
                test.Log(Status.Info, $"Exsiting OTP Setup is using Bank Account Number : {maskedBankAccountNumber}");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();

                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);

                webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.manageBankAccountsLinkEnabledLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.manageBankAccountsLinkEnabledLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, commonServices.backToPaymentsButtonLocBy);

                string xpath = commonServices.deleteButtonBasedOnAccountNumber.Replace("<ACCOUNT_NUMBER>", maskedBankAccountNumber.Substring(maskedBankAccountNumber.Length - 4));
                reportLogger.TakeScreenshot(test, "Bank Accounts");
                var deleteIcon = _driver.FindElement(By.XPath(xpath));
                bool isValidationExists = false;
                if (deleteIcon != null)
                {
                    webElementExtensions.WaitForElementToBeEnabled(_driver, deleteIcon);
                    webElementExtensions.ClickElementUsingJavascript(_driver, deleteIcon);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);

                    webElementExtensions.WaitForElementToBeEnabled(
                        _driver,
                        commonServices.deleteButtonOnDeleteBankAccountPopupLocBy,
                        timeoutInSeconds: ConfigSettings.WaitTime
                    );

                    webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.deleteButtonOnDeleteBankAccountPopupLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);

                    if (webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.closeButtonOnBankAccountCannotBeDeletedPopupLocBy, timeoutInSeconds: 2))
                    {

                        var msg = webElementExtensions.GetElementText(_driver, dashboard.dialogLocBy);
                        isValidationExists = true;
                        reportLogger.TakeScreenshot(test, "Unable to Delete Bank");
                        ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.BankAccountDeleteErrorMsgFM, msg, $"Bank Account Delegte should check if payment is pending");
                        webElementExtensions.ClickElementUsingJavascript(
                            _driver,
                            commonServices.closeButtonOnBankAccountCannotBeDeletedPopupLocBy
                        );
                    }
                }

                ReportingMethods.LogAssertionTrue(test, isValidationExists, "Unable to Delete Bank should show if payment is pending from bank account");

            }
            catch (NoSuchElementException e)
            {
                test.Log(Status.Error, $"No Bank Account Account in Manage Accounts Page with Account Number {maskedBankAccountNumber}. It is known data issue in lower environments ");
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
        [Description("<br>TPR-3310 Skip MFA when \"Remember this device\" is checked<br>" +
                         "TPR-3312 One-Time MFA Bypass when MFAStatus = 3 ('LinkaLoan')<br>" +
                         "TPR-3349 Test MFA Login - No Phone Number - Single or Multiple loans linked<br>" +
                         "TPR-3305 Test MFA Feature Login - Verify Email Template For MFA Code<br>" +
                         "TPR-3347 Test MFA Login - Include Both Phone Number & Email Options - Phone Number[C1/H1/W1/C2/H2/W2] - Single loan linked<br>" +
                         "TPR-3311 MFA Skipped for 24 Hours when MFAStatus = 2<br>" +
                         "TPR-3348 Test MFA Login - Include Both Phone Number & Email Options - Phone Number[C1/H1/W1/C2/H2/W2] - Multiple loans linked")]
        [TestCategory("CP_Regression"), TestCategory("CP_NonPayments")]
        public void TPR_3310_3312_3349_3305_3347_3311_3348_TPR_VerifyMfaLoginFeature()
        {
            int retryCount = 0;
            List<Hashtable> loanLevelData = new List<Hashtable>();
            Dictionary<string, string> existingPhoneNumberDetails = new Dictionary<string, string>();
            #region TestData

            List<string> usedLoanTestData = new List<string>();
            test.Log(Status.Info, $"Query Used:<br><font color='brown'><b>  {activeLoansForNonPayments}  </b></font>");
            var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                 .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                 .Where(field => field.IsLiteral && !field.IsInitOnly)
                 .Select(field => field.GetValue(null).ToString()).ToList();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
            {
                requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            }
            loanLevelData = commonServices.GetLoanDataFromDatabase(activeLoansForNonPayments, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, true);
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

                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                ReportingMethods.Log(test, "********************************************Linking first Loan - Verify MFA Status 3********************************************");
                //TPR - 3312    One - Time MFA Bypass when MFAStatus = 3('LinkaLoan')
                existingPhoneNumberDetails = dBconnect.GetBorrowerPhoneNumber(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString());
                dBconnect.UpdateBorrowerPhoneNumber(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), "8355071111", "C1", existingPhoneNumberDetails["TelelphoneID"].ToString());

                dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                dashboard.HandleServiceChatBot();
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                commonServices.LogoutOfTheApplication();
                ReportingMethods.Log(test, "********************************************Login after first Loan linked - Verify MFA Status 0 with Single Loan********************************************");
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForElement(_driver, dashboard.loginMFAPopUpLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "Login MFA for Single Loan Linked with Email Id And Phone Number");
                webElementExtensions.ActionClick(_driver, dashboard.cancelBtnLocBy, "Cancel button on Login MFA", isReportRequired: true);
                webElementExtensions.WaitForStalenessOfElement(_driver, commonServices.cpUsernameLocBy, ConfigSettings.SmallWaitTime);
                commonServices.LoginToTheApplication(username, password);

                string emailTemplate = yopmail.GetEmailContentFromYopmail(username, emailNotificationType: Constants.EmailNotificationFromYopmail.EmailTemplateYourAuthenticationCodeTextContent);
                string expectedTitleAndSenderText = "Your Authentication Code\r\n\r\nDoNotReply@loanDepot.com <DoNotReply@dispatch-loanDepot.com>";
                string expectedLastFourDigitsOfLoanNumber = loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Substring(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString().Length - 4);

                dashboard.HandleLoginMFA(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString());
                ReportingMethods.LogAssertionContains(test, expectedTitleAndSenderText, emailTemplate, "Verify Title and Sender Details for the Email Template");
                ReportingMethods.LogAssertionContains(test, expectedLastFourDigitsOfLoanNumber, emailTemplate, "Verify Encrypted loan Number in the Email Template");
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.LoginMFAVerificationCodeEmailTemplateText.ToString(), emailTemplate, "Verify Email Body for the Email Template");

                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                dashboard.HandleServiceChatBot();
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                ReportingMethods.Log(test, "********************************************TPR-3311 MFA Skipped for 24 Hours when MFAStatus = 2********************************************");
                //TPR-3311	MFA Skipped for 24 Hours when MFAStatus = 2                
                string flagID = ConfigSettings.Environment == "QA" ? "35" : ConfigSettings.Environment == "SG" ? "26" : "0";
                dBconnect.InsertLoanIntoLoanFlagTable(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), flagID, "Auto Test 2");
                commonServices.LogoutOfTheApplication();
                commonServices.LoginToTheApplication(username, password);
                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                dashboard.HandleServiceChatBot();
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                dBconnect.DeleteLoanFromLoanFlagTable(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), flagID);

                ReportingMethods.Log(test, "********************************************TPR - 3349 Test MFA Login - No Phone Number -Single or Multiple loans linked********************************************");
                //TPR - 3349    Test MFA Login - No Phone Number -Single or Multiple loans linked
                commonServices.LogoutOfTheApplication();
                commonServices.LoginToTheApplication(username, password);
                dashboard.HandleLoginMFA(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString());
                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                dashboard.HandleServiceChatBot();
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                retryCount++;

                ReportingMethods.Log(test, "********************************************TPR - 3349 Test MFA Login - No Phone Number -Single or Multiple loans linked********************************************");
                //TPR - 3349    Test MFA Login - No Phone Number -Single or Multiple loans linked
                ReportingMethods.Log(test, "********************************************Linking Second Loan - Verify MFA Status 0 with Multiple Loans********************************************");
                dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                dashboard.HandleServiceChatBot();
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                commonServices.LogoutOfTheApplication();
                commonServices.LoginToTheApplication(username, password);

                //TPR - 3310    Skip MFA when "Remember this device" is checked
                ReportingMethods.Log(test, "********************************************Remember My Device checkbox Click - Verify MFA Status 1********************************************");
                dashboard.HandleLoginMFA(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), true);
                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                dashboard.HandleServiceChatBot();
                dashboard.ClosePopUpsAfterLinkingNewLoan();

                commonServices.LogoutOfTheApplication();
                commonServices.LoginToTheApplication(username, password);
                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                dashboard.HandleServiceChatBot();
                dashboard.ClosePopUpsAfterLinkingNewLoan();
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, "Test Failed at this screen");
            }
            finally
            {
                dBconnect.UpdateBorrowerPhoneNumber(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), existingPhoneNumberDetails["phone_number"].ToString(), existingPhoneNumberDetails["phone_code"].ToString(), existingPhoneNumberDetails["TelelphoneID"].ToString());
                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }
    }
}