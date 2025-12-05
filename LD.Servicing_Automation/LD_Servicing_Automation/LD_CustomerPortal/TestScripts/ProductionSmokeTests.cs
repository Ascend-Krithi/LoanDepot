using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_CustomerPortal.Pages;
using LD_CustomerPortal.TestPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LD_CustomerPortal.Tests
{
    [TestClass]
    [TestCategory("CP_Sanity_Prod")]
    public class ProductionSmokeTests : BasePage
    {

        public TestContext TestContext
        {
            set;
            get;
        }


        #region ObjectInitialization

        DashboardPage dashboard = null;
        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;
        Pages.PaymentsPage payments = null;
        StatementsPage statements = null;
        PayoffQuotePage payoffQuote = null;
        PaperlessSettingsPage paperless = null;
        FAQsPage faqs = null;
        HomePage homePage = null;
        ReportLogger reportLogger { get; set; }
        string loanNumberToUse = "0020";
        #endregion ObjectInitialization

        [TestInitialize]
        public void TestInitialize()
        {

            // In prod we should not query/update the database. Hence removbed db related stuff 
            InitializeFramework(TestContext);
            dashboard = new DashboardPage(_driver, test);
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            payments = new Pages.PaymentsPage(_driver, test);
            statements = new StatementsPage(_driver, test);
            faqs = new FAQsPage(_driver, test);
            paperless = new PaperlessSettingsPage(_driver, test);
            payoffQuote = new PayoffQuotePage(_driver, test);
            reportLogger = new ReportLogger(_driver);
            homePage = new HomePage(_driver, test);

        }


        [TestMethod]
        [Description("<br>TPR-793-Verify Statements and Documents")]
        [TestCategory("CP_Sanity_Prod")]
        public void TPR_793_TPR_VerifyStatementsAndDocuments()
        {
            try
            {
                if (ConfigSettings.Environment != "PROD")
                {
                    test.Log(Status.Warning, $"Tests are running in {ConfigSettings.Environment}, can't run Production tests!");
                    return;
                }
                // Login to App with prelinked loan
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitUntilUrlContains("dashboard");
                reportLogger.TakeScreenshot(test, "My Dashboard Page");
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                dashboard.SwitchToLinkedLoan(loanNumberToUse);
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

                ReportingMethods.LogAssertionEqual(test, end, DateTime.Now.ToString("yyyy-MM-dd"), $"End Date should be {DateTime.Now.ToString("yyyy-MM-dd")}");
                var expectedStart = DateTime.Now.AddMonths(-13);
                ReportingMethods.LogAssertionEqual(test, start, expectedStart.ToString("yyyy-MM-dd"), $"Start Date should be '{expectedStart.ToString("yyyy-MM-dd")}'");

                //Get listed Documents
                var documents = statements.GetDocumentsList();
                ReportingMethods.LogAssertionTrue(test, documents.Count > 1, "Default Documents Count should be > 0");
                //Select Category                                   

                bool viewedDoc = false;

                foreach (string category in Constants.Statements)
                {
                    var isdocExists = statements.SearchForDocumentCategory(category);
                    //Check viw Document
                    if (isdocExists && !viewedDoc)
                    {
                        statements.CheckViewDocument();
                        viewedDoc = true;
                    }
                }
                ReportingMethods.LogAssertionTrue(test, viewedDoc, "View Document Verification Successful");
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, "Test Failed at this screen");
            }
        }



        [TestMethod]
        [Description("<br>TPR-743 Verify Request Payoff Quote")]
        [TestCategory("CP_Sanity_Prod"),]
        public void TPR_743_TPR_VerifyRequestPayoffQuote()
        {
            try
            {
                if (ConfigSettings.Environment != "PROD")
                {
                    test.Log(Status.Warning, $"Tests are running in {ConfigSettings.Environment}, can't run Production tests!");
                    return;
                }
                // Login to App with prelinked loan
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitUntilUrlContains("dashboard");
                reportLogger.TakeScreenshot(test, "My Dashboard Page");
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                dashboard.SwitchToLinkedLoan(loanNumberToUse);
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
                    //Gets all elgible dates to select in m/d/yyyy format
                    dates = payoffQuote.RequestPayoffQuote(method);
                    if (method != "US Postal Mail")
                    {
                        _driver.ReportResult(test, (dates.Count == 30 || dates.Count == 31), $"{method} : Successfully validated number of calander days enabled", $"Expected number of eligible days=31/30 but found {dates.Count}");
                        payoffQuote.SelecDateFromCalanderForPayoffQuote(dates[0]);
                    }
                    else
                    {
                        var isEnabled = webElementExtensions.IsElementEnabled(_driver, commonServices.paymentDatePickerIconLocBy);
                        ReportingMethods.LogAssertionFalse(test, isEnabled, "Expected Calander button tobe Disabled");
                    }
                }
                webElementExtensions.ClickElement(_driver, payoffQuote.btnIdRequestPayoffLocBy);
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
                    test.Log(Status.Warning, "<b>This loan doesn't exists in MSP</b>");
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
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, "Test Failed at this screen");
            }

        }

        [TestMethod]
        [Description("<br>TPR-396 Verify Paperless Settings")]
        [TestCategory("CP_Sanity_Prod"),]
        public void TPR_396_TPR_VerifyPaperlessSettings()
        {
            try
            {
                if (ConfigSettings.Environment != "PROD")
                {
                    test.Log(Status.Warning, $"Tests are running in {ConfigSettings.Environment}, can't run Production tests!");
                    return;
                }
                // Login to App with prelinked loan
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitUntilUrlContains("dashboard");
                reportLogger.TakeScreenshot(test, "My Dashboard Page");
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                dashboard.SwitchToLinkedLoan(loanNumberToUse);
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
                paperless.VerifySaveChanges();
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, "Test Failed at this screen");
            }

        }

        [TestMethod]
        [Description("<br>TPR-1530-Verify OTP E2E Scenario for Loan Status Ontime")]
        [TestCategory("CP_Sanity_Prod"),]
        public void TPR_1530_TPR_VerifyOTPE2EScenarioForLoanStatusOntime()
        {
            int retryCount = 0;
            bool isEligible = true;
            try
            {
                if (ConfigSettings.Environment != "PROD")
                {
                    test.Log(Status.Warning, $"Tests are running in {ConfigSettings.Environment}, can't run Production tests!");
                    return;
                }

                string loanStatus = Constants.LoanStatus.Ontime;

                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                dashboard.SwitchToLinkedLoan(loanNumberToUse);

                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);
                reportLogger.TakeScreenshot(test, $"Account Summary");
                bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                int numberOfPendingPayments = _driver.FindElements(dashboard.confirmationNumbersInPendinPaymentSectionLocBy).Count;
                List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();
                test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");
                string totalPayment = null;
                string accountNumber = null;
                string accountFullName = null;
                string nextPaymentDueDate = null;
                webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);
                if (willScheduledPaymentInProgressPopAppear)
                    payments.AcceptScheduledPaymentIsProcessingPopUp();
                dashboard.HandlePaperLessPage();
                payments.VerifytOTPPageTitle();
                payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);
                var selectedAccount = webElementExtensions.GetElementText(_driver, commonServices.bankAccountDropdownValueSelectedLocBy);
                var bankaccount = payments.GetDefaultBankAccountDetails();
                totalPayment = payments.VerifyOTPPaymentFields(10.00, 15.00, 30.00, 25.00, "setup", false);
                var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                webElementExtensions.ScrollToBottom(_driver);
                reportLogger.TakeScreenshot(test, $"Make Payment Setup");
                commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
                payments.VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
                payments.VerifyTotalAmountPaymentReviewPage(totalPayment);
                payments.VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(bankaccount.AcntNumber, bankaccount.Name, dateSelected, totalPayment, "setup", nextPaymentDueDate);
                commonServices.ClickConfirmButtonPaymentReviewPage();
                commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);
                string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
                webElementExtensions.ScrollToBottom(_driver);
                webElementExtensions.WaitForStalenessOfElement(_driver, payments.backToAccountSummaryBtnLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.makePaymentBtnLocBy);
                webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);
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
                webElementExtensions.ScrollToTop(_driver);
                //Edit the amount 10.00 and 10.00 more than monin additional Principal and escrow
                totalPayment = payments.VerifyOTPPaymentFields(10.00, 15.00, 30.00, 25.00, "edit", false);
                webElementExtensions.IsElementEnabled(_driver, payments.updatePaymentBtnLocBy, "Update Payment", false);
                webElementExtensions.ScrollToBottom(_driver);
                reportLogger.TakeScreenshot(test, $"Make Payment Edit");
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
                var pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                var counter = 0;

                while (pendingPaymentsAfterDelete.Count == pendingPaymentsAfterSetup.Count && counter <= ConfigSettings.WaitTime)
                {
                    pendingPaymentsAfterDelete = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture)).ToList();
                    counter++;
                }
                webElementExtensions.ScrollIntoView(_driver, dashboard.pendingPaymentDatesTextLocBy);
                reportLogger.TakeScreenshot(test, "Pending Payment Activity Section After Deleting OTP");
                ReportingMethods.LogAssertionFalse(test, pendingPaymentsAfterDelete.Contains(payment_date), "Verify Pending Payment Activity should not have deleted OTP payment");
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, "Test Failed at this screen");
            }
            finally
            {
                test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            }
        }

        [TestMethod]
        [Description("<br>TPR-1377_1375_735 Verify Dashboard Products and Services")]
        [TestCategory("CP_Sanity_Prod"),]
        public void TPR_1377_1375_735_TPR_VerifyDashboardProductsAndServices()
        {
            try
            {
                // Login to App with prelinked loan
                if (ConfigSettings.Environment != "PROD")
                {
                    test.Log(Status.Warning, $"Tests are running in {ConfigSettings.Environment}, can't run Production tests!");
                    return;
                }
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitUntilUrlContains("dashboard");
                reportLogger.TakeScreenshot(test, "My Dashboard Page");
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                dashboard.SwitchToLinkedLoan(loanNumberToUse);
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.statementsLinkLocBy);

                //check products and services
                foreach (string product in Constants.Products)
                {
                    dashboard.CheckProduct(product);
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, "Test Failed at this screen");
            }

        }

        [TestMethod]
        [Description("<br>TPR-837_970 Verify FAQs")]
        [TestCategory("CP_Sanity_Prod"),]
        public void TPR_837_970_TPR_VerifyFAQs()
        {
            try
            {
                if (ConfigSettings.Environment != "PROD")
                {
                    test.Log(Status.Warning, $"Tests are running in {ConfigSettings.Environment}, can't run Production tests!");
                    return;
                }
                // Login to App with prelinked loan
                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitUntilUrlContains("dashboard");
                reportLogger.TakeScreenshot(test, "My Dashboard Page");
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                dashboard.SwitchToLinkedLoan(loanNumberToUse);
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.helpAndSupportLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.helpAndSupportLocBy);
                webElementExtensions.WaitUntilElementIsClickable(_driver, dashboard.faqAnchorLinkLocBy);
                webElementExtensions.ClickElement(_driver, dashboard.faqAnchorLinkLocBy);
                webElementExtensions.WaitUntilUrlContains("faqs");
                faqs.SearchByTopic("payment");
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, "Test Failed at this screen");
            }
        }

        [TestMethod]
        [Description("<br>TPR-3111 Verify LoanDepot Home page SSO Register/Signin Page with Style/Font/Format & SSO Integrations")]
        [TestCategory("CP_Sanity_Prod"),]
        public void TPR_3111_TPR_VerifyHomepage()
        {
            try
            {
                if (ConfigSettings.Environment != "PROD")
                {
                    test.Log(Status.Warning, $"Tests are running in {ConfigSettings.Environment}, can't run Production tests!");
                    return;
                }

                _driver.Navigate().GoToUrl(Constants.Urls.LoanDepotUrl);
                test.Log(Status.Info, $"Navigate to {Constants.Urls.LoanDepotUrl}");
                webElementExtensions.WaitForElement(_driver, homePage.myAccountBtnLocBy);
                reportLogger.TakeScreenshot(test, "Loandepot Home Page");


                webElementExtensions.ClickElement(_driver, homePage.myAccountBtnLocBy, "My Account", true);
                webElementExtensions.WaitUntilUrlContains("/login");
                webElementExtensions.WaitForVisibilityOfElement(_driver, homePage.loanServicingLocBy);
                reportLogger.TakeScreenshot(test, "Loandepot Account Page");


                webElementExtensions.ClickElement(_driver, homePage.loanServicingLocBy, "Loan Servicing", true);
                webElementExtensions.WaitUntilUrlContains("about/loan-servicing");
                webElementExtensions.WaitForVisibilityOfElement(_driver, homePage.loginButtonLocBy);
                reportLogger.TakeScreenshot(test, "Servicing Page");

                webElementExtensions.ClickElement(_driver, homePage.loginButtonLocBy, "Login", true);
                webElementExtensions.WaitForNewTabWindow(_driver, 2, waitTime: 10);
                webElementExtensions.SwitchTabs(_driver, 1);
                webElementExtensions.WaitForVisibilityOfElement(_driver, homePage.signInButtonLocBy);
                webElementExtensions.WaitForElementToBeEnabled(_driver, homePage.signInButtonLocBy);

                reportLogger.TakeScreenshot(test, "Loandepot SignIn Page");
                var signinUrl = _driver.Url;
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.ProdClientLogin, signinUrl, $"Sign In Url should have {Constants.CustomerPortalTextMessages.ProdClientLogin}");

                commonServices.LoginToTheApplication(username, password);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                webElementExtensions.WaitUntilUrlContains("dashboard");
                reportLogger.TakeScreenshot(test, "My Dashboard Page");

                _driver.Close();
                ReportingMethods.Log(test, "Close Dashboard Page");
                webElementExtensions.SwitchToFirstTab(_driver);
                ReportingMethods.Log(test, "Switch to Servicing Page");
                reportLogger.TakeScreenshot(test, "Servicing Page");
                webElementExtensions.ClickElement(_driver, homePage.regLinkLocBy, "'Register Now' Link", true);
                webElementExtensions.WaitForNewTabWindow(_driver, 2, waitTime: 10);
                webElementExtensions.SwitchTabs(_driver, 1);
                webElementExtensions.WaitUntilUrlContains(Constants.CustomerPortalTextMessages.ProdRegSignup);

                webElementExtensions.WaitForVisibilityOfElement(_driver, homePage.signUpheaderLocBy);
                webElementExtensions.WaitUntilElementIsClickable(_driver, homePage.firstNameLocBy);
                webElementExtensions.EnterText(_driver, homePage.firstNameLocBy, "Automation Test");
                webElementExtensions.EnterText(_driver, homePage.firstNameLocBy, "");
                webElementExtensions.ScrollToTop(_driver);

                reportLogger.TakeScreenshot(test, "Loandepot Signup Page");

                var signupUrl = _driver.Url;
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.ProdRegSignup, signupUrl, $"Sign Up Url should have {Constants.CustomerPortalTextMessages.ProdRegSignup}");



            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while executing <b>{testContext.TestName}</b> test method. Error: <b>{e.Message}</b>");
                reportLogger.TakeScreenshot(test, "Test Failed at this screen");
            }
        }
    }
}
