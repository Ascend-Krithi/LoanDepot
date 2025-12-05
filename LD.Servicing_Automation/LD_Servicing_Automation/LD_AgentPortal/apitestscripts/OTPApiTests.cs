using System;
using OpenQA.Selenium;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LD_AgentPortal.Pages;
using LD_AutomationFramework.Pages;
using System.Threading;
using System.Linq;
using OpenQA.Selenium.Interactions;
using AutoIt;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Blobs;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Text;
using System.Net;
using System.Collections.Generic;
using LD_AutomationFramework.Utilities;
using System.Data;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using System.Collections;
using LD_AutomationFramework.Config;
using AventStack.ExtentReports;
using iTextSharp.text.pdf.security;
using log4net;
using LD_PaymentEngine.APIServices;

namespace LD_AgentPortal.Tests
{
    [TestClass]
    public class OTPApiTests : BasePage
    {
        public static string loanDetailsQuery = UtilAdditions.GetExtractedXmlData("LoanDetails.xml", "xml/Query_GetLoanLevelDetailsForAutoPay");
        public static string loanDetailsQuery1 = UtilAdditions.GetExtractedXmlData("LoanDetails.xml", "xml/Query_GetLoanDataFor0CDTesting").Replace("greaterthan", ">");

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

        string deleteReason = "Test Delete Reason";

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
        }


        [TestMethod]
        [Description("<br> Verify the functionality of performing One Time Payment from agent portal UI and verify with GetPayments API  <br>")]
        [TestCategory("AP_API")]
        public void OneTimePayment_APIIntegration_GetPayments()
        {
            int retryCount = 0;
            string loanNumber = string.Empty, borrowerName = string.Empty, result = string.Empty, dateToBeSelected = string.Empty;

            #region TestData

            List<string> loansWithSuccessfullPaymentSetup = new List<string>();
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = new List<string>() { Constants.LoanLevelDataColumns.LoanNumber,
                                                                 Constants.LoanLevelDataColumns.BorrowerFirstName,
                                                                 Constants.LoanLevelDataColumns.BorrowerLastName};
            loanDetailsQuery1 = loanDetailsQuery1.Replace("GREATER_THEN", ">");
            loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQuery1, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

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
                    webElementExtensions.WaitForElement(_driver, dashboard.viewAccountButtonLocBy);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);

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
                        string bankAccountName = "TestAutoAccount";
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
                                //API Integration
                                LoanPaymentIntegrationAPIServices loanPaymentIntegrationAPIServices = new LoanPaymentIntegrationAPIServices(_driver, test);
                                string paymentId = loanPaymentIntegrationAPIServices.GetPayments(loanNumber);
                                Assert.IsNotNull(paymentId, "Unable to get the payment Id from Get Payments API  method");
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
    }
}
