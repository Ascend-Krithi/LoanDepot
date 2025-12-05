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
using System.Data;
using System.Linq;
using System.Reflection;

namespace LD_CustomerPortal.Tests
{
    [TestClass]
    public class HelocDrawsTests : BasePage
    {
        string loanDetailsQueryForActiveHelocDraws = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoansForActiveHelocDraws));

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
        YopmailPage yopmail = null;
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
            yopmail = new YopmailPage(_driver, test);
            dBconnect = new DBconnect(test, Constants.DBNames.MelloServETL);
            reportLogger = new ReportLogger(_driver);
            //unlink loans from test account 
            dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
            test.Log(Status.Info, $"Cleared all the linked loans with User: {username}");
            string queryToUpdateTCPAFlagIsGlobalValue = UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.UpdateTCPAFlagIsGlobalValue).Replace("TCPA_FLAG_VALUE", "0");
            dBconnect.ExecuteQuery(queryToUpdateTCPAFlagIsGlobalValue).ToString();
        }

        [TestMethod]
        [Description("<br> TPR - 2234 CUSTP - 5012 - Test - HELOC DRAWS - Verify Registered Email ID and FAQ link is displayed in MFA pop - up | Request Funds from Advance from Line of Credit page<br>" +
                          "TPR - 309 Given a customer has a heloc loan eligible for draws When the loan already has a draw requested & processed within the same business day  Then disable Request Funds button and show the msg<br>" +
                          "TPR - 704 Given a customer wants to draw funds When clicked on Request Funds and the amount entered is greater than the available credit line<br>" +
                          "TPR - 705 Given a customer is eligible to draw funds with available credit line amount, line status is open and no previous draw is still pending  When clicked on Request Funds and amount entered is < $1000<br>" +
                          "TPR - 687 Test MFA to request funds using new bank account or using existing bank account<br>" +
                          "TPR-813	Test Advance Request from Line of Credit - Request Funds")]
        [TestCategory("CP_Regression"), TestCategory("CP_HelocDraws")]
        public void TPR_2234_309_704_705_687_813_TPR_VerifyHelocDrawsEmailAndFaqLinkInMfaPopupAndSuccessfulFundRequest()
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
                test.Log(Status.Info, $"Query Used:<br><font color='brown'><b> {loanDetailsQueryForActiveHelocDraws} </b></font>");
                var requiredColumns = typeof(Constants.LoanLevelDataColumns)
                     .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                     .Where(field => field.IsLiteral && !field.IsInitOnly)
                     .Select(field => field.GetValue(null).ToString()).ToList();
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
                    requiredColumns.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
                }
                loanLevelData = commonServices.GetLoanDataFromDatabase(loanDetailsQueryForActiveHelocDraws, null, requiredColumns, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
                if (loanLevelData == null || loanLevelData.Count == 0)
                {
                    test.Log(Status.Warning, $"There is no loan available for the above query");
                    return;
                }
                #endregion TestData

                string queryToGetUsersRegisteredViaAutomation = UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetUsersRegisteredViaAutomation);
                DBconnect dBconnect = new DBconnect(test, Constants.DBNames.MelloServ);
                List<string> emailIds = dBconnect.ExecuteQuery(queryToGetUsersRegisteredViaAutomation)
                  .Rows
                  .Cast<DataRow>()
                  .Where(row => row["Email"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["Email"].ToString()))
                  .Select(row => row["Email"].ToString())
                  .ToList();
                username = emailIds.Count > 0 ? emailIds[new Random().Next(emailIds.Count)] : string.Empty;
                commonServices.LoginToTheApplication(username, password);

                //Retrieve valid loan level data from the database
                while (retryCount < ConfigSettings.NumberOfLoanTestDataRequired && retryCount < loanLevelData.Count)
                {
                    if (retryCount >= 0)
                    {
                        helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData[retryCount]);
                        if (helocLoanInfo.IsDrawEligible == true && Convert.ToDouble(helocLoanInfo.AvailableHELOCAmount) > 1000)
                        {
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                            webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                            dashboard.LinkLoan(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.PropertyZip].ToString(), loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerSsn].ToString(), emailID: username);
                            dashboard.HandlePaperLessPage();
                            webElementExtensions.WaitUntilUrlContains("/dashboard");
                            dashboard.HandleServiceChatBot();
                            dashboard.ClosePopUpsAfterLinkingNewLoan();

                            feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                            totalFees = feeDic?.Values
                                .Select(v => decimal.TryParse(v, out var val) ? val : 0)
                                .Sum() ?? 0;
                            if (dashboard.VerifyIfLinkingOfLoanNumberIsSuccessful(loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString(), false, true, false, true, false))
                            {
                                if (webElementExtensions.IsElementEnabled(_driver, dashboard.requestFundsButtonLocBy, isScrollIntoViewRequired: false) && Convert.ToInt32(helocLoanInfo.AvailableHELOCAmount) >= 1000)
                                    break;
                                int availableAmount = Convert.ToInt32(helocLoanInfo.AvailableHELOCAmount);
                                ReportingMethods.Log(test, $"Available Credit Line : ${availableAmount}");
                                dBconnect.DeleteUserProfile(dBconnect.GetUserProfileId(username));
                                _driver.Navigate().Refresh();
                                webElementExtensions.WaitUntilUrlContains("/link-user-loan");
                                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                            }
                        }
                        else
                            test.Log(Status.Info, $"Is Heloc Draw Eligible : {helocLoanInfo.IsDrawEligible} | Loan Number : {helocLoanInfo.LoanNumber}");
                        retryCount++;


                    }
                    if (retryCount == ConfigSettings.NumberOfLoanTestDataRequired || retryCount == loanLevelData.Count - 1)
                    {
                        test.Log(Status.Warning, "Could not find the correct set of loan level test data even after " + retryCount + " retries.");
                        return;
                    }

                }
                firstName = "ADITI";
                lastName = "NAIK";
                string checkingOrSavingsAccountType = "Savings";
                accountNumber = "61231234380";
                By locator = null;
                string accountNickname = "ADITINAIK4380";
                bool saveBankAccount = true;
                bool makeTheAccountDefault = true;
                string routingNumber = "122199983";
                string accountFullName = firstName + " " + lastName;
                string fccRequired = "No";

                string queryToUpdateBorrowersFirstLastAndFullName = UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.UpdateBorrowersFirstLastAndFullName).Replace("LN_NUMBER", loanLevelData[retryCount][Constants.LoanLevelDataColumns.LoanNumber].ToString()).Replace("FIRST_NAME", firstName).Replace("LAST_NAME", lastName).Replace("FULL_NAME", accountFullName);
                dBconnect = new DBconnect(test, Constants.DBNames.MelloServETL);
                dBconnect.ExecuteQuery(queryToUpdateBorrowersFirstLastAndFullName).ToString();


                _driver.Navigate().Refresh();
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.loanDepotLogoLocBy, ConfigSettings.WaitTime);
                test.Log(Status.Info, "<b>********************************************<u>Refreshed the Screen After Updating the Borrower's First Name and the Last Name </u>*******************************************</b>");
                dashboard.HandlePaperLessPage();

                dashboard.HandleServiceChatBot();
                dashboard.ClosePopUpsAfterLinkingNewLoan();
                test.Log(Status.Info, "<b>********************************************<u>Starting Account Summary Page Validation</u>*******************************************</b>");
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.manageAutopayOnOffButtonLocBy);

                dashboard.VerifyAdvanceRequestFromLineOfCreditTextContent(true);

                webElementExtensions.ActionClick(_driver, dashboard.requestFundsButtonLocBy, "Request Funds Button", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.requestFundsButtonLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "Advance From Line Of Credit Page Initial State");

                test.Log(Status.Info, "<b>********************************************<u>Deletion of all the Mails and Bank Accounts Started</u>*******************************************</b>");
                commonServices.DeleteAllAddedBankAccounts();
                yopmail.GetEmailContentFromYopmail(username, true);
                test.Log(Status.Info, "<b>********************************************<u>Deletion of all the Mails and Bank Accounts Completed</u>*******************************************</b>");

                test.Log(Status.Info, "<b>********************************************<u>Started Adding the Bank Account</u>*******************************************</b>");
                // Click on the Add an Account Button
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.addAnAccountLinkOnAdvanceFromLineOfCreditPageLocBy, ConfigSettings.WaitTime);
                webElementExtensions.ActionClick(_driver, payments.addAnAccountLinkOnAdvanceFromLineOfCreditPageLocBy, "Add An Account Button", false, true);

                // Verify MFA Pop Up Email Verification
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.mfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy, ConfigSettings.WaitTime);
                string expMfaPopUpTextContent = webElementExtensions.GetElementText(_driver, payments.mfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy, true);

                ReportingMethods.LogAssertionTrue(test, expMfaPopUpTextContent.Equals(Constants.CustomerPortalTextMessages.MFAPopUpAddAnAccountOnAdvanceFromLineOfCreditTextContent.Replace("<EMAIL_ID>", username)), "MFA Pop Up for Add An Account on Advance From Line Of Credit");
                //Verify the FAQ link and Navigation
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.faqsLinkOnMFAPopUpLocBy);
                webElementExtensions.ActionClick(_driver, payments.faqsLinkOnMFAPopUpLocBy, "FAQ Link On MFA Popup", isReportRequired: true);
                webElementExtensions.WaitForNewTabWindow(_driver, 1, ConfigSettings.SmallWaitTime);
                webElementExtensions.SwitchTabs(_driver, 1);
                webElementExtensions.WaitForStalenessOfElement(_driver, dashboard.faqHeaderTextOnFaqPageLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitUntilUrlContains("/faqs");
                ReportingMethods.LogAssertionContains(test, "/faqs", _driver.Url.ToString(), $"Verify if the UR contains <b>/faqs<b>");
                reportLogger.TakeScreenshot(test, "FAQs Page");
                _driver.Close();
                webElementExtensions.SwitchTabs(_driver, 0);

                // Handle Email verification code
                string verificationCodeFromYopmail = string.Empty;
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.mfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy))
                    verificationCodeFromYopmail = yopmail.GetEmailContentFromYopmail(username);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.verificationCodeInputFieldOnMfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy, "Verification Code Input Field", isScrollIntoViewRequired: false);
                webElementExtensions.EnterText(_driver, payments.verificationCodeInputFieldOnMfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy, verificationCodeFromYopmail, false, "Verification Code Input Field", true, false, true);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.verifyButtonOnMfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy, "Verify Button On MFA Pop Up", isScrollIntoViewRequired: false);
                webElementExtensions.ActionClick(_driver, payments.verifyButtonOnMfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy, "Verify Button On MFA Pop Up");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.mfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.addBankAccountPopUpLocBy, ConfigSettings.WaitTime);


                // Add account
                if (!string.IsNullOrEmpty(accountNickname))
                    webElementExtensions.EnterText(_driver, commonServices.accountNicknameLocBy, accountNickname, true, null, false, false);
                if (fccRequired.ToLower().Contains("no"))
                    locator = commonServices.fccNotRequiredOptionLocBy;
                else if (fccRequired.ToLower().Contains("yes"))
                    locator = commonServices.fccRequiredOptionLocBy;
                webElementExtensions.MoveToElement(_driver, locator);
                webElementExtensions.ActionClick(_driver, locator);
                if (checkingOrSavingsAccountType.ToLower().Equals("checking"))
                    locator = commonServices.checkingRadioButtonLocBy;
                else if (checkingOrSavingsAccountType.ToLower().Equals("savings"))
                    locator = commonServices.savingsRadioButtonLocBy;
                webElementExtensions.MoveToElement(_driver, locator);
                webElementExtensions.ActionClick(_driver, locator);

                webElementExtensions.EnterText(_driver, commonServices.routingNumberTextboxLocBy, routingNumber, false, null, false);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.bankNameDisplayedBelowRoutingNumberFieldLocBy);
                webElementExtensions.EnterText(_driver, commonServices.accountNumberTextboxLocBy, accountNumber, false, null, false);
                webElementExtensions.EnterText(_driver, commonServices.confirmAccountNumberTextboxLocBy, accountNumber, false, null, false);
                if (!saveBankAccount)
                {
                    webElementExtensions.MoveToElement(_driver, commonServices.saveBankAccountCheckboxLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.saveBankAccountCheckboxLocBy);
                }
                if (makeTheAccountDefault)
                {
                    webElementExtensions.MoveToElement(_driver, commonServices.makeThisTheDefaultAccountCheckboxLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.makeThisTheDefaultAccountCheckboxLocBy);
                }
                reportLogger.TakeScreenshot(test, "After filling all the details on Add Bank Account Page");
                webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.addAccountButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.addAccountButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.addAnAccountLinkLocBy);
                test.Log(Status.Info, "<b>********************************************<u>Completed the Addition of Bank Account Process</u>*******************************************</b>");

                test.Log(Status.Info, "<b>********************************************<u>Started the Process to validate the Warning messages for Less than $1000, Greater than Available Credit Balance</u>*******************************************</b>");
                webElementExtensions.EnterText(_driver, payments.amountInputFiledOnAdvanceFromLineOfCredit, "999.00", false, "Amount Input Field", true, isClickRequired: true);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.pleaseEnterDollorgreaterOrEqualTo1000OrUptoAvailableCreditLineErrorMsgLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.VerifyElementText(_driver, payments.pleaseEnterDollorgreaterOrEqualTo1000OrUptoAvailableCreditLineErrorMsgLocBy, Constants.CustomerPortalErrorMsgs.PleaseEnterDollorGreaterOrEqualTo1000AndUptoAvailableCreditLineErrorMsg.Trim(), "Error Msg to enter Dollor greater than or Equal to 1000 or upto Available Credit Line", true);
                string amountMoreThanAvailableCreditBalance = Convert.ToString(Convert.ToDouble(helocLoanInfo.AvailableHELOCAmount + 100.00));
                webElementExtensions.EnterText(_driver, payments.amountInputFiledOnAdvanceFromLineOfCredit, amountMoreThanAvailableCreditBalance, false, "Amount Input Field", true, isClickRequired: true);
                webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);

                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.pleaseEnterAnAmountEqualToOrLessThanYourAvailableCreditLineErrorMsg, ConfigSettings.SmallWaitTime);
                string str = webElementExtensions.GetElementText(_driver, payments.pleaseEnterAnAmountEqualToOrLessThanYourAvailableCreditLineErrorMsg);
                webElementExtensions.VerifyElementText(_driver, payments.pleaseEnterAnAmountEqualToOrLessThanYourAvailableCreditLineErrorMsg, Constants.CustomerPortalErrorMsgs.PleaseEnterAnAmountEqualToOrLessThanYourAvailableCreditLineErrorMsg.Trim(), "Error Msg  Please enter an amount equal to or less than your available credit line.", true);
                test.Log(Status.Info, "<b>********************************************<u>Completed the Process to validate the Warning messages for Less than $1000, Greater than Available Credit Balance</u>*******************************************</b>");

                test.Log(Status.Info, "<b>********************************************<u>Started the Process to Validate and request fund</u>*******************************************</b>");
                webElementExtensions.EnterText(_driver, payments.amountInputFiledOnAdvanceFromLineOfCredit, "1000.00", false, "Amount Input Field", true, isClickRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.pleaseEnterDollorgreaterOrEqualTo1000OrUptoAvailableCreditLineErrorMsgLocBy, ConfigSettings.SmallWaitTime);
                ReportingMethods.LogAssertionFalse(test, webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.pleaseEnterDollorgreaterOrEqualTo1000OrUptoAvailableCreditLineErrorMsgLocBy), $"Verify that Message : {Constants.CustomerPortalErrorMsgs.PleaseEnterDollorGreaterOrEqualTo1000AndUptoAvailableCreditLineErrorMsg} is NO MORE DISPLAYED");
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.requestFundsButtonLocBy, ConfigSettings.WaitTime);
                webElementExtensions.ActionClick(_driver, payments.requestFundsButtonLocBy, "Request Funds Button", isReportRequired: true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.drawRequestProcessingTimePopUpLocBy, ConfigSettings.WaitTime);
                string actualDrawRequestProcessingTimePopUpTextContent = webElementExtensions.GetElementText(_driver, payments.drawRequestProcessingTimePopUpLocBy, true);
                ReportingMethods.LogAssertionTrue(test, actualDrawRequestProcessingTimePopUpTextContent.Equals(Constants.CustomerPortalTextMessages.DrawRequestProcessingTimePopUpTextContent), "Verify Draw Request Processing Time Pop Up Text Content");
                reportLogger.TakeScreenshot(test, "Draw Request Processing Time Pop Up Text Content");
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.cancelButtonOnDrawRequestProcessingTimePopUpLocBy, ConfigSettings.WaitTime);
                webElementExtensions.ActionClick(_driver, payments.cancelButtonOnDrawRequestProcessingTimePopUpLocBy, "Cancel Button", isReportRequired: true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.requestFundsButtonLocBy, ConfigSettings.WaitTime);
                webElementExtensions.ActionClick(_driver, payments.requestFundsButtonLocBy, "Request Funds Button", isReportRequired: true);
                webElementExtensions.WaitUntilElementIsClickable(_driver, payments.confirmButtonOnDrawRequestProcessingTimePopUpLocBy, ConfigSettings.WaitTime);
                webElementExtensions.ActionClick(_driver, payments.confirmButtonOnDrawRequestProcessingTimePopUpLocBy, "Confirm Button", isReportRequired: true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, payments.mfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.mfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy))
                    verificationCodeFromYopmail = yopmail.GetEmailContentFromYopmail(username);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.verificationCodeInputFieldOnMfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy, "Verification Code Input Field", isScrollIntoViewRequired: false);
                webElementExtensions.EnterText(_driver, payments.verificationCodeInputFieldOnMfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy, verificationCodeFromYopmail, false, "Verification Code Input Field", true, false, true);
                webElementExtensions.WaitForElementToBeEnabled(_driver, payments.verifyButtonOnMfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy, "Verify Button On MFA Pop Up", isScrollIntoViewRequired: false);
                webElementExtensions.ActionClick(_driver, payments.verifyButtonOnMfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy, "Verify Button On MFA Pop Up");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.mfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.addBankAccountPopUpLocBy, ConfigSettings.WaitTime);

                webElementExtensions.WaitUntilUrlContains("/dashboard");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.LongWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.drawFundRequestedSuccessAlertLocBy, ConfigSettings.WaitTime);
                test.Log(Status.Info, "<b>********************************************<u>Completed the Process to Validate and request fund</u>*******************************************</b>");
                test.Log(Status.Info, "<b>********************************************<u>Started the Process to Validate After Requesting Fund</u>*******************************************</b>");
                reportLogger.TakeScreenshot(test, "Before Logout");
                string actDrawFundsSuccessAlertMsg = webElementExtensions.GetElementText(_driver, dashboard.drawFundRequestedSuccessAlertLocBy, true);
                ReportingMethods.LogAssertionTrue(test, Constants.CustomerPortalTextMessages.DrawRequestSuccessRequestFundsTextContent.Equals(actDrawFundsSuccessAlertMsg), "Verify Draw Fund Success Alert Msg");
                string actRequestFundsAdvanceRequestFromLineOfCreditTextContent = webElementExtensions.GetElementText(_driver, dashboard.requestFundsAdvanceRequestFromLineOfCreditTextContentLocBy, true);
                ReportingMethods.LogAssertionTrue(test, Constants.CustomerPortalTextMessages.RequestFundsAdvanceRequestFromLineOfCreditTextContent.Replace("<CURRENT_DATE>", DateTime.Now.ToString("MMM dd, yyyy")).Equals(actRequestFundsAdvanceRequestFromLineOfCreditTextContent), "Verify Request Funds Advance Requests From Line of Credit Text Content");
                reportLogger.TakeScreenshot(test, "Before Logout After making Request");
                commonServices.LogoutOfTheApplication();
                commonServices.LoginToTheApplication(username, password);
                dashboard.HandlePaperLessPage();
                webElementExtensions.WaitUntilUrlContains("/dashboard");
                dashboard.HandleServiceChatBot();
                dashboard.ClosePopUpsAfterLinkingNewLoan();

                actRequestFundsAdvanceRequestFromLineOfCreditTextContent = webElementExtensions.GetElementText(_driver, dashboard.requestFundsAdvanceRequestFromLineOfCreditTextContentLocBy, true);
                ReportingMethods.LogAssertionTrue(test, Constants.CustomerPortalTextMessages.RequestFundsAdvanceRequestFromLineOfCreditTextContent.Replace("<CURRENT_DATE>", DateTime.Now.ToString("MMM dd, yyyy")).Equals(actRequestFundsAdvanceRequestFromLineOfCreditTextContent), "Verify Request Funds Advance Requests From Line of Credit Text Content"); reportLogger.TakeScreenshot(test, "After Logout");
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
