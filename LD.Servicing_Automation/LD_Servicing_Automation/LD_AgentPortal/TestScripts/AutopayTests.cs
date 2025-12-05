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
    public class AutopayTests : BasePage
    {
        public static string noActiveAutopayLoanDetailsQuery = UtilAdditions.GetExtractedXmlData("LoanDetails.xml", "xml/Query_NoActiveAutopayViaRepayOrInhouse");
        public static string autopayEligibleLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForAutoPay));
        public static string autopayEligibleNonEscrowedLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetNonEscrowedLoanLevelDetailsForAutoPay));
        public static string autopayEligibleEscrowedLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetEscrowedLoanLevelDetailsForAutoPay));
        public static string activeAutopayLoanDetailsQuery = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsWithActiveAutoPay));
        public static string autopayEligibleEscrowedAutoPayWithoutLateFees = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEscrowedAutoPayPastDueWithoutLateFees));
        public static string escrowedAutopayWhenDueDateIsNextMonthOrTwoMonthsFromCurrentMonth = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLoanLevelDetailsForEscrowedAutopayWhenDueDateIsNextMonthOrTwoMonthsFromCurrentMonth));
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
        ReportLogger reportLogger = null;
        List<Hashtable> loanLevelData = null;
        JiraManager jiraManager = null;
        LD_CustomerPortal.Pages.PaymentsPage cpPayments = null;
        LD_CustomerPortal.Pages.DashboardPage cpDashboard = null;

        #endregion ObjectInitialization

        #region CommonTestData

        string deleteReason = "Test Delete Reason";
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
            cpPayments = new LD_CustomerPortal.Pages.PaymentsPage(_driver, test);
            cpDashboard = new LD_CustomerPortal.Pages.DashboardPage(_driver, test);
        }

        [TestMethod]
        [Description("<br> TC-1241[SCM_3878] - Test-Verify Status = Off <br> ")]
        [TestCategory("AP_Regression"), TestCategory("AP_Autopay")]
        public void TC_1241_TC_VerifyAutopayStatus()
        {
            #region TestData

            string loanNumber = string.Empty, borrowerName = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
            loanLevelData = commonServices.GetLoanDataFromDatabase(noActiveAutopayLoanDetailsQuery, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);
            loanNumber = loanLevelData[0][Constants.LoanLevelDataColumns.LoanNumber].ToString();
            borrowerName = loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerFirstName].ToString() + " " + loanLevelData[0][Constants.LoanLevelDataColumns.BorrowerLastName].ToString();

            #endregion TestData

            commonServices.LoginToTheApplication(username, password);
            commonServices.LaunchUrlWithLoanNumber(loanLevelData, 0, true);
            dashboard.NavigateToTab(Constants.AgentPortalTabNames.PaymentsTab);
            webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
            webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
            payments.VerifyPaymentSummaryDetailsOnPaymentPage(loanLevelData[0]); 

            //SCM_3878: Verification 1 - Verify autopay status is off
            webElementExtensions.WaitForElement(_driver, payments.autopayStatusLocBy);
            bool flag = webElementExtensions.VerifyElementText(_driver, payments.autopayStatusLocBy, "Off");
            _driver.ReportResult(test, flag, "Successfully verified that Autopay Status is 'OFF' for the selected loan.",
                                             "Failure - Autopay Status is not 'OFF' for the selected loan.");
            webElementExtensions.ScrollIntoView(_driver, payments.autopayStatusLocBy);
            reportLogger.TakeScreenshot(test, "Autopay Status");
        }


        [TestMethod]
        [Description("<br> TC-1148[SCM_3975] - Verify new page opens to allow user to 'Setup Autopay Now' when user clicks 'Manage Autopay' and There is no active autopay plan <br> " +
                     "TC-696[SCM_3500] - Route User to Manage Autopay Screen To Setup New Autopay - Setup Autopay Now <br> " +
                     "TC-811[SCM_4071] - Verify when user clicks 'Manage Autopay' button (Setup/Manage Autopay) <br> " +
                     "TC-626[SCM_4402] - Verify Manage Autopay takes User to Inhouse Manager Autopay when there is Autopay plan active at Inhouse <br> " +
                     "TC-531[SCM-3529] - Test FUNC - View Active Autopay - Inhouse")]
        [TestCategory("AP_Regression"), TestCategory("AP_Autopay")]
        public void TC_1148_696_811_626_531_TC_SetupNewAutopay()
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

            //Below lines of code are commented due to a defect
            //webElementExtensions.WaitForElement(_driver, payments.goBackToPaymentsLinkLocBy);
            //webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
            //webElementExtensions.WaitForElement(_driver, commonServices.setupAutopaybuttonLocBy);
            //webElementExtensions.ActionClick(_driver, commonServices.setupAutopaybuttonLocBy, "Setup Autopay button.");
            //webElementExtensions.WaitForElement(_driver, payments.authorizedByDropdownLocBy);
            //payments.SelectValueInAuthorizedByDropdown(borrowerName);
            //webElementExtensions.WaitForElement(_driver, payments.goBackToPaymentsLinkLocBy);
            ////webElementExtensions.ClickElementUsingJavascript(_driver, payments.goBackToPaymentsLinkLocBy);
            //webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
            //webElementExtensions.WaitForElement(_driver, payments.manageAutopayButtonLocBy);
            //webElementExtensions.ActionClick(_driver, payments.manageAutopayButtonLocBy, "Manage Autopay button.");
            commonServices.SetupAutopay(null, pendingPaymentDates, borrowerName);
        }

        [TestMethod]
        [Description("<br> TC-1320[SCM_3694] - Additional Monthly Principal Payment Field Placement (Angular) <br> " +
                     "TC-1382[SCM_4512] - Verify Next Debit Date pulled Pending Autopay Payment Date from Payment Details (Angular/D365) - active inhouse autopay <br> " +
                     "TC-1308[SCM_3706] - Verify tax/insurance field not displayed for non escrowed loan - Setup autopay")]
        [TestCategory("AP_Regression"), TestCategory("AP_Autopay")]
        public void TC_1320_1382_1308_TC_SetupNewAutopayAndVerifyNextDebitDateTaxInsuranceFields()
        {
            #region TestData

            int retryCount = 0;
            string borrowerName = string.Empty, nextPaymentDueDate = string.Empty, taxAndInsurance = string.Empty, paymentDateToBeSelected = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
            loanLevelData = commonServices.GetLoanDataFromDatabase(autopayEligibleNonEscrowedLoanDetailsQuery, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

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
            taxAndInsurance = loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString();
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
            webElementExtensions.WaitForElement(_driver, payments.goBackToPaymentsLinkLocBy);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, commonServices.setupAutopaybuttonLocBy);
            webElementExtensions.ActionClick(_driver, commonServices.setupAutopaybuttonLocBy, "Setup Autopay button.");
            webElementExtensions.WaitForElement(_driver, payments.goBackToPaymentsLinkLocBy);

            //SCM_3694 - Additional Monthly Principal Payment Field Placement (Angular)
            flag = false;
            flag = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.addlMonthlyPrincipalPaymentPlacedAfterPlanLocBy);
            _driver.ReportResult(test, flag, "Successfully verified that Additional Principal Payment field is placed below the plan amount options.",
                                         "Failure - Additional Principal Payment field is not placed below the plan amount options.");
            
            //SCM_3706 - Verify tax/insurance field not displayed for non escrowed loan - Setup autopay
            flag = false;
            flag = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.taxesAndOrInsuranceLocBy);
            _driver.ReportResult(test, !flag, "Successfully verified that tax or insurance field is not displayed for non escrowed loan while autopay setup.",
                                         "Failure - Tax or insurance field is displayed for non escrowed loan while autopay setup.");
            
            webElementExtensions.ClickElementUsingJavascript(_driver, payments.goBackToPaymentsLinkLocBy);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, payments.manageAutopayButtonLocBy);
            webElementExtensions.ActionClick(_driver, payments.manageAutopayButtonLocBy, "Manage Autopay button.");
            flag = false;
            flag = commonServices.SetupAutopay(null, pendingPaymentDates, borrowerName);            
        }

        [TestMethod]
        [Description("<br> TC-775[SCM-3316]-Test- Validate Autopay Monthly Amount To Debit Field (Angular)-Monthly <br> " +
                     "TC-1018[SCM-3132] - Verify 'Other' Not Present In 'Authorized By' Field <br> " +
                     "TC-1067[SCM-3169] - Test-Validate Additional Principal Payment amount with Next Payment Due Date <br> " +
                     "TC-1066[SCM-3170] - Test-Validate display message when Additional Principal Payment is more than $9999.99")]
        [TestCategory("AP_Regression"), TestCategory("AP_Autopay")]
        public void TC_775_1018_1067_1066_TC_SetupAutopayAndValidateAdditionalPrincipalAuthorizedByFields()
        {
            #region TestData

            int retryCount = 0;
            string borrowerName = string.Empty, coBorrowerName = string.Empty, nextPaymentDueDate = string.Empty, totalMonthlyPayment = string.Empty, taxAndInsurance = string.Empty, paymentDateToBeSelected = string.Empty;
            List<string> usedLoanTestData = new List<string>();
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.SuspenseAmount);
            columnDataRequired.Remove(Constants.LoanLevelDataColumns.PaymentDueDate);
            loanLevelData = commonServices.GetLoanDataFromDatabase(escrowedAutopayWhenDueDateIsNextMonthOrTwoMonthsFromCurrentMonth, null, columnDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired);

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
            coBorrowerName = loanLevelData[retryCount][Constants.LoanLevelDataColumns.CoBorrowerFirstName].ToString() + " " + loanLevelData[retryCount][Constants.LoanLevelDataColumns.CoBorrowerLastName].ToString();
            nextPaymentDueDate = loanLevelData[retryCount][Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString();
            totalMonthlyPayment = loanLevelData[retryCount][Constants.LoanLevelDataColumns.TotalMonthlyPayment].ToString();
            taxAndInsurance = loanLevelData[retryCount][Constants.LoanLevelDataColumns.TaxAndInsuranceAmount].ToString();
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
            webElementExtensions.WaitForElement(_driver, payments.goBackToPaymentsLinkLocBy);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, commonServices.setupAutopaybuttonLocBy);
            webElementExtensions.ActionClick(_driver, commonServices.setupAutopaybuttonLocBy, "Setup Autopay button.");
            webElementExtensions.WaitForElement(_driver, payments.goBackToPaymentsLinkLocBy);

            //SCM_2972 - Verify 'Other' is not present in 'Authorized By' dropdown
            flag = false;
            webElementExtensions.WaitForElement(_driver, commonServices.authorizedByDropdownLocBy);
            webElementExtensions.ActionClick(_driver, commonServices.authorizedByDropdownLocBy);
            if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(commonServices.authorizedByDropdownValue.Replace("<BORROWERNAME>", "Other"))))
                flag = true;
            _driver.ReportResult(test, flag, "Successfully verified that the value 'Other' is not present in Authorized By dropdown.",
                                         "Failure - The value 'Other' is present in Authorized By dropdown.");

            //SCM_2972 - Verify all borrower/coborrower names are present in Authorized By dropdown
            bool flag1 = false;
            if (!string.IsNullOrEmpty(coBorrowerName))
            {
                flag1 = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(commonServices.authorizedByDropdownValue.Replace("<BORROWERNAME>", borrowerName)));
                _driver.ReportResult(test, flag1, "Successfully verified that Borrower name - " + borrowerName + " is present in Authorized By dropdown.",
                                             "Failure: Borrower name - " + borrowerName + " is not present in Authorized By dropdown.");
            }
            else
            {
                flag1 = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(commonServices.authorizedByDropdownValue.Replace("<BORROWERNAME>", borrowerName))) &&
                webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(commonServices.authorizedByDropdownValue.Replace("<BORROWERNAME>", coBorrowerName)));
                _driver.ReportResult(test, flag1, "Successfully verified that Borrower - " + borrowerName + " and Co-Borrower - " + coBorrowerName + " names are present in Authorized By dropdown.",
                                             "Failure: Borrower - " + borrowerName + " and Co-Borrower - " + coBorrowerName + " names are not present in Authorized By dropdown.");
            }
            webElementExtensions.ActionClick(_driver, commonServices.authorizedByDropdownLocBy);

            flag = false;
            //SCM_3316 - Validate Autopay Monthly Amount To Debit Field (Angular)-Monthly
            double totalMonthlyPaymentFormatted = Convert.ToDouble(totalMonthlyPayment);
            flag = "$" + totalMonthlyPaymentFormatted.ToString("N") == webElementExtensions.GetElementText(_driver, payments.divTotalAmountLocBy).Split(' ')[0];
            _driver.ReportResult(test, flag, "Successfully verified that Autopay monthly amount to debit - " + totalMonthlyPaymentFormatted + " is equal to the Total payment amount.",
                                         "Failure - Autopay monthly amount to debit - " + totalMonthlyPaymentFormatted + " is not equal to the Total payment amount.");

            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber);

            //SCM-3177 - Test-Validate display message when Additional Principal Payment is more than $9999.99
            webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.additionalMonthlyPrincipalCheckBoxInputLocBy);
            webElementExtensions.EnterText(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, "10000.00", true, "Additional Principal", true);
            webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.additionalMonthlyPrincipalMaximumErrorMessageLocBy);
            _driver.ReportResult(test, webElementExtensions.VerifyElementText(_driver, commonServices.additionalMonthlyPrincipalMaximumErrorMessageLocBy, Constants.Messages.MaximumAdditionalPrincipalPayment), "Successfully verified that the error message - " + Constants.Messages.MaximumAdditionalPrincipalPayment + " is displayed.", "The error message - " + Constants.Messages.MaximumAdditionalPrincipalPayment + " is not displayed.");

            webElementExtensions.EnterText(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, "9999.99", true, "Additional Principal", true);
            _driver.ReportResult(test, !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.additionalMonthlyPrincipalMaximumErrorMessageLocBy), "Successfully verified that the error message - " + Constants.Messages.MaximumAdditionalPrincipalPayment + " is not displayed.", "The error message - " + Constants.Messages.MaximumAdditionalPrincipalPayment + " is displayed.");

            webElementExtensions.EnterText(_driver, commonServices.additionallyMonthlyPrincipalInputLocBy, webElementExtensions.RandomNumberGenerator(_driver, 9999.99, 99999.99), true, "Additional Principal", true);
            _driver.ReportResult(test, webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.additionalMonthlyPrincipalMaximumErrorMessageLocBy), "Successfully verified that the error message - " + Constants.Messages.MaximumAdditionalPrincipalPayment + " is displayed.", "The error message - " + Constants.Messages.MaximumAdditionalPrincipalPayment + " is not displayed.");

            webElementExtensions.ClickElementUsingJavascript(_driver, payments.goBackToPaymentsLinkLocBy);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
            webElementExtensions.WaitForVisibilityOfElement(_driver, commonServices.paymentSummarySectionLocBy);
            if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.paymentSummarySectionLocBy))
                _driver.Navigate().Refresh();
            webElementExtensions.WaitForElement(_driver, payments.manageAutopayButtonLocBy);
            webElementExtensions.ActionClick(_driver, payments.manageAutopayButtonLocBy, "Manage Autopay button.");

            //SCM-3169[TC-1067] - Test-Validate Additional Principal Payment amount with Next Payment Due Date
            flag = false;
            flag = commonServices.SetupAutopay(null, pendingPaymentDates, borrowerName, false, webElementExtensions.RandomNumberGenerator(_driver, 1.00, 9999.99));
        }
    }
}
