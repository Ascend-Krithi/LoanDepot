using AventStack.ExtentReports;
using LD_AgentPortal.Pages;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
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
    public class ModificationTrialTests : BasePage
    {
        public static string modificationTrialLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetActiveModificationTrialLoanLevelDetails));
        public static string modTrialChangesInProgressLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetSystemChangesInProgressTrialLoanLevelDetails));
        public static string modTrialBrokenOrDeletedLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetBrokenOrDeletedTrialLoanLevelDetails));
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

        DashboardPage dashboard = null;
        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;
        DBconnect dBconnect = null;
        Pages.PaymentsPage payments = null;
        ReportLogger reportLogger = null;
        List<Hashtable> loanLevelData = null;
        JiraManager jiraManager = null;

        #endregion ObjectInitialization

        #region CommonTestData

        string deleteReason = "Test Delete Reason";

        #endregion CommonTestData

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
        [Description("<br> TC-819[SCM_5692] - Verify Make a Payment button is displayed in orange color and confirmation popup is displayed on clicking the Make a Payment button <br> " +
                     "TC-555[SCM_5708] - Verify Prepay is not displayed in OTP Setup screen for Active Trail plan <br>" +
                     "TC-847[SCM_5696] - Verify late Fee message is not displayed when loan has Active Trail Plan <br>" +
                     "TC-1325[SCM-5645] - Verify Additional Prinicipal,Additional Escrow,Fees are not displayed in the Make a Payment screen for Active Trail Loan <br>" +
                     "TC-558[SCM-5709] - Verify Prepay option is not displayed in OTP Edit screen for Active Trail plan")]
        [TestCategory("AP_Regression"), TestCategory("AP_ModificationTrial")]
        public void TC_819_555_847_1325_558_TC_VerifyMakeAPaymentButtonForModificationTrialLoans()
        {
            #region TestData

            string borrowerName = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> columnDataRequired = typeof(Constants.LoanLevelDataColumns)
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(field => field.FieldType == typeof(string))
                    .Select(field => (string)field.GetValue(null)).ToList();
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
            loanLevelData = commonServices.GetLoanDataFromDatabase(modificationTrialLoanDetailsQuery, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            borrowerName = loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerFirstName].ToString() + " " + loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerLastName].ToString();
            string monthlyPaymentAmount = loanLevelData[0][Constants.LoanLevelDataColumns.TotalMonthlyPayment].ToString();

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            commonServices.LaunchUrlWithLoanNumber(loanLevelData, 0, true);
            dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
            webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
            webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData[0]);

            //SCM_5692: Verification 1 - Make a payment button is Orange in color            
            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Orange, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Orange' in color for the selected Modification Trial loan.",
                                             "Failure - 'Make a Payment' button is not Orange in color for the selected  Modification Trial loan.");
            reportLogger.TakeScreenshot(test, "'Make a Payment' button");

            //SCM_5692: Verification 2 - 'Make a Payment' confirmation pop up is displayed on clicking Make a Payment button
            webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentButtonLocBy, "Make a Payment button");
            webElementExtensions.WaitForElement(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy);
            flag = webElementExtensions.IsElementDisplayed(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy) &&
                webElementExtensions.IsElementDisplayed(_driver, payments.makeAPaymentConfirmationPopupConfirmButtonLocBy);
            _driver.ReportResult(test, true, "Successfully verified that 'Make a Payment' confirmation pop up is displayed.",
                                             "Failure - 'Make a Payment' confirmation pop up is not displayed.");
            reportLogger.TakeScreenshot(test, "'Make a Payment' confirmation pop up");

            //SCM_5692: Verification 3 - User is navigated back to Payments tab after closing 'Make a Payment' confirmation pop up.
            webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy);
            
            if (webElementExtensions.IsElementDisplayed(_driver, payments.manageAutopayButtonLocBy))
                _driver.ReportResult(test, true, "Successfully verified that user is on Payments tab after closing 'Make a Payment' confirmation pop up.", "");
            else
                _driver.ReportResult(test, false, "", "Failure - User is not routed back to Payments tab after closing 'Make a Payment' confirmation pop up.");
            reportLogger.TakeScreenshot(test, "Payments page");

            //SCM_5692: Verification 4 - OTP setup screen opens after clicking on Confirm button in 'Make a Payment' confirmation pop up.
            webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentButtonLocBy);
            webElementExtensions.WaitForElement(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentConfirmationPopupConfirmButtonLocBy);
            webElementExtensions.WaitForElement(_driver, payments.paymentDatePickerIconLocBy);
            if (webElementExtensions.IsElementDisplayed(_driver, payments.authorizedByDropdownLocBy))
                _driver.ReportResult(test, true, "Successfully verified that user is on OTP page after clicking on Confirm button in 'Make a Payment' confirmation pop up.", "");
            else
                _driver.ReportResult(test, false, "", "Failure - User is not navigated to OTP page after clicking on Confirm button in 'Make a Payment' confirmation pop up.");
            reportLogger.TakeScreenshot(test, "OTP page");

            //SCM_5708: Verification 1 - Total Reinstatement Amount and Prepay option is not present in OTP setup screen
            By amountSectionInOtpSetupScreenLocBy = By.XPath(commonServices.amountSectionInOtpSetupScreen.Replace("<AMOUNTSECTIONFIELDNAME>", "Total Reinstatement Amount"));
            flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, amountSectionInOtpSetupScreenLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that Total Reinstatement Amount in OTP setup screen amount section is not displayed.", "Total Reinstatement Amount in OTP setup screen amount section is displayed.");

            amountSectionInOtpSetupScreenLocBy = By.XPath(commonServices.amountSectionInOtpSetupScreen.Replace("<AMOUNTSECTIONFIELDNAME>", "Prepay"));
            flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, amountSectionInOtpSetupScreenLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that Prepay option in OTP setup screen amount section is not displayed.", "Prepay option in OTP setup screen amount section is displayed.");

            //SCM_5696: Verification 1 - Verify late Fee message is not displayed when loan has Active Trail Plan
            string workingDate = commonServices.GetWorkingDateAfter16thDate();
            commonServices.SelectPaymentDateInDateField(workingDate);
            flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.lateFeeMessageInOtpScreenLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that the message - 'A late fee may be assessed for payments' is not displayed.", "The message - 'A late fee may be assessed for payments' is displayed.");

            //SCM-5645: Verification 1 - Verify Additional Prinicipal,Additional Escrow,Fees are not displayed in the Make a Payment screen for Active Trail Loan
            amountSectionInOtpSetupScreenLocBy = By.XPath(commonServices.amountSectionInOtpSetupScreen.Replace("<AMOUNTSECTIONFIELDNAME>", "Additional Principal"));
            flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, amountSectionInOtpSetupScreenLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that Additional Principal field in OTP setup screen amount section is not displayed.", "Additional Principal field in OTP setup screen amount section is displayed.");

            amountSectionInOtpSetupScreenLocBy = By.XPath(commonServices.amountSectionInOtpSetupScreen.Replace("<AMOUNTSECTIONFIELDNAME>", "Additional Escrow"));
            flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, amountSectionInOtpSetupScreenLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that Additional Escrow field in OTP setup screen amount section is not displayed.", "Additional Escrow field in OTP setup screen amount section is displayed.");

            flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.feeSubSectionInAmountSectionOfOtpScreenLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that Fee sub section in OTP setup screen amount section is not displayed.", "Fee sub section in OTP setup screen amount section is displayed.");

            amountSectionInOtpSetupScreenLocBy = By.XPath(commonServices.amountSectionInOtpSetupScreen.Replace("<AMOUNTSECTIONFIELDNAME>", "Late Fee"));
            flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, amountSectionInOtpSetupScreenLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Late Fee' field of Fee sub section in OTP setup screen amount section is not displayed.", "'Late Fee' field of Fee sub section in OTP setup screen amount section is displayed.");

            //SCM-5709: Verification 1 - Verify Prepay option is not displayed in OTP Edit screen for Active Trail plan
            payments.SelectValueInAuthorizedByDropdown(borrowerName);
            string bankAccountName = Constants.BankAccountData.BankAccountName;
            commonServices.AddAnAccount(bankAccountName, "TestFN", "TestLN", "Personal", "Savings", "122199983");
            string randomNumber = webElementExtensions.RandomNumberGenerator(_driver, 1, Convert.ToInt32(monthlyPaymentAmount.Split('.')[0])) + ".00";
            webElementExtensions.ClickElement(_driver, commonServices.amountRadioButtonLocBy, "Amount radio button");
            string amountMonthlyPaymentFromUI = payments.GetAmountMonthlyPaymentValue();
            if (amountMonthlyPaymentFromUI == "$" + Convert.ToDouble(monthlyPaymentAmount).ToString("#,##0.00"))
                webElementExtensions.EnterText(_driver, commonServices.amountInputFieldLocBy, randomNumber, true, "Monthly Payment Amount", true);

            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
            string confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
            payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
            payments.CloseNotesTab();
            payments.EditNewlyAddedPayment(confirmationNumber);
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);

            amountSectionInOtpSetupScreenLocBy = By.XPath(commonServices.amountSectionInOtpSetupScreen.Replace("<AMOUNTSECTIONFIELDNAME>", "Prepay"));
            flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, amountSectionInOtpSetupScreenLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that Prepay option in OTP setup screen amount section is not displayed.", "Prepay option in OTP setup screen amount section is displayed.");

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
        [Description("<br> TC-845[SCM_5700] - Verify Update button is disabled when there is no update made and on clicking go back to Payments user is navigated to Payments angular tab <br> " +
                     "TC-839[SCM_5702] - Verify the edit payment page when loan has System changes in progress <br>" +
                     "TC-842[SCM_5698] [DEFECT#SCM-5754] - Verify late Fee message is not displayed when loan has System changes in progress <br>" +
                     "TC-547[SCM-5737] [DEFECT#SCM-5492] - Verify user able to make payment successfully when amount<total monthly payment is scheduled for System changes in progress - Modification trail <br>" +
                     "TC-952[SCM-5663] - Verify Total Reinstatement amount is displayed correctly in OTP/Edit screen for loan with system changes in progress when user enters amount>= total reinstatement amount <br>" +
                     "TC-958[SCM-5666] [DEFECT#SCM-5754] - Verify Total Reinstatement field is not displayed in Edit screen for System changes in progress")]
        [TestCategory("AP_Regression"), TestCategory("AP_ModificationTrial")]
        public void TC_845_839_842_547_952_958_TC_VerifySystemChangesInProgressForModificationTrialLoans()
        {
            #region TestData

            int retryCount = 0;
            string borrowerName = string.Empty, monthlyPaymentAmount = string.Empty, principalAndInterest = string.Empty, taxAndInsurance = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> columnDataRequired = typeof(Constants.LoanLevelDataColumns)
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(field => field.FieldType == typeof(string))
                    .Select(field => (string)field.GetValue(null)).ToList();
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
            loanLevelData = commonServices.GetLoanDataFromDatabase(modTrialChangesInProgressLoanDetailsQuery, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            borrowerName = loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerFirstName].ToString() + " " + loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerLastName].ToString();
            monthlyPaymentAmount = loanLevelData[0][Constants.LoanLevelDataColumns.TotalMonthlyPayment].ToString();
            principalAndInterest = loanLevelData[0][Constants.LoanLevelDataColumns.PrincipalInterestAmount].ToString();
            taxAndInsurance = loanLevelData[0][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString();

            #endregion TestData
            
            commonServices.LoginToTheApplication(username, password);            
            while (retryCount < loanLevelData.Count)
            {
                commonServices.LaunchUrlWithLoanNumber(loanLevelData, retryCount, true);
                dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
                webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
                webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                
                if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.makeAPaymentButtonDisabledLocBy))
                    break;
                else if (retryCount == loanLevelData.Count - 1)
                {
                    Assert.Fail("Make a payment button is disabled for all the selected loans.");
                    test.Log(Status.Fail, "Make a payment button is disabled for all the selected loans.");
                }
                else
                    retryCount++;
            }
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData[0]);
            payments.DeleteAllExistingPendingPayments(deleteReason);
            webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentButtonLocBy);
            webElementExtensions.WaitForElement(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentConfirmationPopupConfirmButtonLocBy);
            webElementExtensions.WaitForElement(_driver, payments.paymentDatePickerIconLocBy);
            payments.SelectValueInAuthorizedByDropdown(borrowerName);
            string bankAccountName = Constants.BankAccountData.BankAccountName;
            string bankAccountNumber = Constants.BankAccountData.BankAccountNumber;
            commonServices.AddAnAccount(bankAccountName, "TestFN", "TestLN", "Personal", "Savings", "122199983");

            //SCM_5698: Verification 1 - Verify late Fee message is not displayed when loan has System changes in progress
            string workingDate = commonServices.GetWorkingDate();
            commonServices.SelectPaymentDateInDateField(workingDate);
            bool flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.lateFeeMessageInOtpScreenLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that the message - 'A late fee may be assessed for payments' is not displayed.", "[DEFECT#SCM-5754] The message - 'A late fee may be assessed for payments' is displayed.");

            flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.feeSubSectionInAmountSectionOfOtpScreenLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that Fee sub section in OTP setup screen amount section is not displayed.", "Fee sub section in OTP setup screen amount section is displayed.");

            By amountSectionInOtpSetupScreenLocBy = By.XPath(commonServices.amountSectionInOtpSetupScreen.Replace("<AMOUNTSECTIONFIELDNAME>", "Late Fee"));
            flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, amountSectionInOtpSetupScreenLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Late Fee' field of Fee sub section in OTP setup screen amount section is not displayed.", "'Late Fee' field of Fee sub section in OTP setup screen amount section is displayed.");
            
            string randomNumber = webElementExtensions.RandomNumberGenerator(_driver, 1, Convert.ToInt32(monthlyPaymentAmount.Split('.')[0])) + ".00";
            webElementExtensions.ClickElement(_driver, commonServices.amountRadioButtonLocBy, "Amount radio button");
            string amountMonthlyPaymentFromUI = payments.GetAmountMonthlyPaymentValue();
            if (amountMonthlyPaymentFromUI == "$" + Convert.ToDouble(monthlyPaymentAmount).ToString("#,##0.00"))
                webElementExtensions.EnterText(_driver, commonServices.amountInputFieldLocBy, randomNumber, true, "Monthly Payment Amount", true);
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
            string confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
            payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
            payments.CloseNotesTab();
            payments.EditNewlyAddedPayment(confirmationNumber);
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);

            //SCM_5702 - Verify the edit payment page when loan has System changes in progress
            flag = webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.authorizedByDropdownMainLocatorLocBy, Constants.ElementAttributes.AriaDisabled, "true");
            _driver.ReportResult(test, flag, "Successfully verified that Authorized By dropdown is disabled.", "Authorized By dropdown is not disabled.");

            payments.VerifyValueSelectedInAuthorizedByDropdown(borrowerName);
            commonServices.VerifyValueInMethodDropdown(bankAccountName, bankAccountNumber, true);
            randomNumber = "$ " + Convert.ToDouble(randomNumber).ToString("#,##0.00");
            commonServices.VerifyAmountEnteredInMonthlyPaymentInputField(randomNumber);

            flag = webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.updatePaymentButtonLocBy, Constants.ElementAttributes.Disabled, "true");
            _driver.ReportResult(test, flag, "Successfully verified that 'Update Payment' button is disabled if there are no updates made on the Edit Payment page.", "'Update Payment' button is enabled even if updates are not made on the Edit Payment page.");

            principalAndInterest = Convert.ToDouble(principalAndInterest).ToString("#,##0.00");
            webElementExtensions.VerifyElementText(_driver, commonServices.principalAndInterestLocBy, "$" + $"{principalAndInterest:n}", "Principal & Interest", true);
            taxAndInsurance = Convert.ToDouble(taxAndInsurance).ToString("#,##0.00");
            webElementExtensions.VerifyElementText(_driver, commonServices.taxesAndOrInsuranceLocBy, "$" + $"{taxAndInsurance:n}", "Taxes &/or Insurance", true);

            //SCM-5666: Verify Total Reinstatement field is not displayed in Edit screen for System changes in progress
            //To be scripted after resolution of defect: SCM-5492

            webElementExtensions.EnterText(_driver, commonServices.amountInputFieldLocBy, webElementExtensions.RandomNumberGenerator(_driver, Convert.ToInt32(monthlyPaymentAmount.Split('.')[0]), Convert.ToInt32(monthlyPaymentAmount.Split('.')[0]) + 100));
            payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
            payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
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
        [Description("<br> TC-944[SCM_5688] - Verify Make a payment button is displayed in orange color when loan has Broken/Deleted Trail Plan and  IsEligible = Allowed with Override due to Error_Code 'PaymentPlanDelinquentDueToPlanBroken' and NextPaymentDue date is in past months <br> " +
                     "TC-931[SCM_5681] - Verify that the user is able to setup OTP payment when the loan is Broken from Active Trail plan and next payment due is previous month or past months <br> " +
                     "TC-830[SCM_5703] [DEFECT#SCM-6492] - Verify the edit payment page when loan has Broken/Deleted Trail Plan <br>" +
                     "TC-956[SCM_5669] - Verify Payment Break down is displayed for Broken/Deleted Trail Plan in Setup/Edit screen")]
        [TestCategory("AP_Regression"), TestCategory("AP_ModificationTrial")]
        public void TC_944_931_830_956_TC_VerifyTheFunctionalityOfBrokenOrDeletedModificationTrialLoans()
        {
            #region TestData

            int retryCount = 0;
            string borrowerName = string.Empty, principalAndInterest = string.Empty, taxAndInsurance = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            List<string> columnDataRequired = typeof(Constants.LoanLevelDataColumns)
                    .GetFields(BindingFlags.Public | BindingFlags.Static)
                    .Where(field => field.FieldType == typeof(string))
                    .Select(field => (string)field.GetValue(null)).ToList();
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
            loanLevelData = commonServices.GetLoanDataFromDatabase(modTrialBrokenOrDeletedLoanDetailsQuery, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            
            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            while (retryCount < loanLevelData.Count)
            {
                commonServices.LaunchUrlWithLoanNumber(loanLevelData, retryCount, true);
                dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
                webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
                webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.makeAPaymentButtonDisabledLocBy) &&
                    !payments.VerifyIfPendingPaymentAlreadyExists("Payment Date", DateTime.UtcNow.ToString().Split(' ')[0]))
                    break;
                else if (retryCount == loanLevelData.Count - 1)
                {
                    Assert.Fail("Make a payment button is disabled for all the selected loans.");
                    test.Log(Status.Fail, "Make a payment button is disabled for all the selected loans.");
                }
                else
                    retryCount++;
            }
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData[retryCount]);
            string pastDuePayments = webElementExtensions.GetElementText(_driver, payments.pastDuePaymentsLocBy).Trim(' ');
            borrowerName = loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerFirstName].ToString() + " " + loanLevelData[retryCount][Constants.LoanLevelDataColumns.BorrowerLastName].ToString();
            principalAndInterest = loanLevelData[retryCount][Constants.LoanLevelDataColumns.PrincipalInterestAmount].ToString();
            taxAndInsurance = loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString();
            //SCM_5688: Verification - Make a payment button is Orange in color            
            bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.Orange, payments.makeAPaymentButtonLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that 'Make a Payment' button is 'Orange' in color for the selected Modification Trial loan.",
                                             "Failure - 'Make a Payment' button is not Orange in color for the selected  Modification Trial loan.");
            reportLogger.TakeScreenshot(test, "'Make a Payment' button");

            webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentButtonLocBy);
            webElementExtensions.WaitForElement(_driver, payments.makeAPaymentConfirmationPopupCancelButtonLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, payments.makeAPaymentConfirmationPopupConfirmButtonLocBy);
            webElementExtensions.WaitForElement(_driver, payments.paymentDatePickerIconLocBy);

            //SCM_5681: Verification - User able to set up payment
            payments.SelectValueInAuthorizedByDropdown(borrowerName);
            string bankAccountName = Constants.BankAccountData.BankAccountName;
            string accountNumber = Constants.BankAccountData.BankAccountNumber;
            commonServices.AddAnAccount(bankAccountName, "TestFN", "TestLN", "Personal", "Savings", "122199983");

            _driver.ReportResult(test, pastDuePayments.Equals(webElementExtensions.GetElementText(_driver, commonServices.pastDueAmountLocBy).Trim(' ')), 
                    "Successfully verified that 'Contractual Payment Amount - "+pastDuePayments+"' is displayed in OTP page for the selected Broken or Deleted Modification Trial loan.",
                    "Failed while verifying Contractual Payment Amount for the selected Broken or Deleted Modification Trial loan.");
            reportLogger.TakeScreenshot(test, "Contractual Payment Amount");

            webElementExtensions.ClickElementUsingJavascript(_driver, payments.pastDueRadioButtonLocBy, "Past Due Amount radio button");
            string workingDate = webElementExtensions.DateTimeConverter(_driver, DateTime.UtcNow.ToString(), "m/d/yyyy to fullMonthName d, yyyy");
            commonServices.SelectPaymentDateInDateField(workingDate);
            payments.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);
            payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
            string confirmationNumber = payments.GetConfirmationNumberFromPaymentsConfirmationPage();
            payments.ClickOnGoBackToPaymentsLinkOnPaymentReviewPage();
            payments.VerifyAngularPaymentDetailsPageDisplayed(Constants.SectionNames.PaymentSummary);
            payments.VerifyNewPaymentAdded("Confirmation Number", confirmationNumber);
            payments.CloseNotesTab();
            payments.EditNewlyAddedPayment(confirmationNumber);
            payments.VerifyOTPPageTitleIsDisplayed(Constants.PageNames.EditPayment);            

            //SCM_5703 - Verify the edit payment page when loan has Broken/Deleted Trail Plan
            flag = webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.authorizedByDropdownMainLocatorLocBy, Constants.ElementAttributes.AriaDisabled, "true");
            _driver.ReportResult(test, flag, "Successfully verified that Authorized By dropdown is disabled.", "Authorized By dropdown is not disabled.");
            payments.VerifyValueSelectedInAuthorizedByDropdown(borrowerName);
            commonServices.VerifyValueInMethodDropdown(bankAccountName, accountNumber, true);
            //Defect associated: SCM-6492
            flag = webElementExtensions.VerifyElementAttributeValue(_driver, commonServices.updatePaymentButtonLocBy, Constants.ElementAttributes.Disabled, "true");
            _driver.ReportResult(test, flag, "Successfully verified that 'Update Payment' button is disabled if there are no updates made on the Edit Payment page.", "[DEFECT#SCM-6492] 'Update Payment' button is enabled even if updates are not made on the Edit Payment page.");
            _driver.ReportResult(test, pastDuePayments.Equals(webElementExtensions.GetElementText(_driver, commonServices.pastDueAmountLocBy).Trim(' ')),
                    "Successfully verified that 'Contractual Payment Amount - " + pastDuePayments + "' is displayed in OTP page for the selected Broken or Deleted Modification Trial loan.",
                    "Failed while verifying Contractual Payment Amount for the selected Broken or Deleted Modification Trial loan.");
            reportLogger.TakeScreenshot(test, "Contractual Payment Amount");

            //SCM_5669:Verify Payment Break down is displayed for Broken/Deleted Trail Plan in Setup/Edit screen
            principalAndInterest = Convert.ToDouble(principalAndInterest).ToString("#,##0.00");
            webElementExtensions.VerifyElementText(_driver, commonServices.principalAndInterestLocBy, "$" + $"{principalAndInterest:n}", "Principal & Interest", true);
            taxAndInsurance = Convert.ToDouble(taxAndInsurance).ToString("#,##0.00");
            webElementExtensions.VerifyElementText(_driver, commonServices.taxesAndOrInsuranceLocBy, "$" + $"{taxAndInsurance:n}", "Taxes &/or Insurance", true);

            string randomNumber = webElementExtensions.RandomNumberGenerator(_driver, 1, 100) + ".00";
            webElementExtensions.WaitForVisibilityOfElement(_driver, payments.lateFeeAmountCheckboxLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, payments.lateFeeAmountCheckboxLocBy);
            webElementExtensions.WaitForVisibilityOfElement(_driver, payments.lateFeeAmountCheckboxCheckedLocBy);
            webElementExtensions.EnterText(_driver, payments.lateFeeTextboxLocBy, randomNumber, true, "Late Fee Amount", true);
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);

            string totalPayment = "$" + (Convert.ToDouble(pastDuePayments.Trim('$').Trim(',')) + Convert.ToDouble(randomNumber)).ToString("#,##0.00");

            payments.VerifyTotalAmountInPaymentPage(totalPayment, "edit");
            payments.ClickButtonUsingName(Constants.ButtonNames.UpdatePayment);
            payments.ClickButtonUsingName(Constants.ButtonNames.Confirm);
            payments.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow);
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
    }
}
