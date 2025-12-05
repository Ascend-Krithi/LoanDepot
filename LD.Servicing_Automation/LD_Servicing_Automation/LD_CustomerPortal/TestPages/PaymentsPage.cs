using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using log4net;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace LD_CustomerPortal.Pages
{
    public class PaymentsPage
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CommonServicesPage commonServices = null;
        private WebElementExtensionsPage webElementExtensions = null;
        public PaymentsPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            reportLogger = new ReportLogger(_driver);
        }

        private IWebDriver _driver { get; set; }
        private ReportLogger reportLogger { get; set; }
        private ExtentTest test { get; set; }
        #region CommonTestData

        private string accountFullName = "TESTFN TESTLN";
        private string accountNumber = Constants.BankAccountData.BankAccountNumber;
        private string accountNumberWhileEdit = Constants.BankAccountData.BankAccountNumberWhileEdit;
        private string bankAccountName = Constants.BankAccountData.BankAccountName;
        private string deleteReason = "Test Delete Reason";
        private string firstName = "TESTFN";
        private string lastName = "TESTLN";
        private string personalOrBussiness = "Personal";
        private string routingNumber = "122199983";
        private string savings = "Savings";
        #endregion CommonTestData

        #region Locators

        public By additionalEscrowTextboxLocBy = By.XPath("//input[@id='additionalEscrowInput']");
        public By additionalEscrowTextInputLocBy = By.Id("additionalEscrowInput");
        public By additionalPaymentCheckBoxLocBy = By.CssSelector("[id='additionalPaymentCheckbox-input']");
        public By additionalPrincipalTextboxLocBy = By.XPath("//input[@id='additionalPrincipalInput']");
        public By additionalPrincipalTextInputLocBy = By.Id("additionalPrincipalInput");
        public By addnlPrincipalAmountLocBy = By.XPath("//*[@id='biWeeklyPaymentAddnPrincipalElement']//mat-form-field");
        public By addPaymentCheckBoxLocBy = By.XPath("//mat-checkbox[contains(@id,'dditionalPaymentCheckbox')]/label/span/input");
        public By amountOnManageAutoPayTextLocBy = By.Id("spIdmonthlyPaymentAmount_");
        public By autopayAmountBiWeeklyAmountAtTop = By.XPath("//*[@id='biWeeklyPaymentRadioDiv']/span");
        public By autopayAmountBiWeeklyScheduledConfirmationPageTextLocBy = By.XPath("//p[text()=' Total Amount Bi-Weekly ']/..//p[contains(text(),'$')]");
        public By autoPayAmountScheduledConfirmationPageTextLocBy = By.XPath("//p[text()=' Total Amount Per Month ']/..//p[contains(text(),'$')]");
        public By autopayDeleteConfirmLocBy = By.Id("deleteBtn");
        public By autopayDeletedMsgLocBy = By.XPath("//*[contains(text(),'Your Autopay has been Deleted')]");
        public By autopayDeleteLocBy = By.Id("linkIdActionDelete_");
        public By autopayDeleteReasonLocBy = By.Id("mat-radio-5");
        public By autopayEditLocBy = By.Id("linkIdActionEdit_");
        public By autopayNoTallowedMsgLocBy = By.XPath("//*[@id='PaymentNotAllowed']");
        public By autopaySetupOneBusinessdayMsgTextLocBy = By.CssSelector("div[id='alertHeading'] div");
        public By backToAccountSummaryBtnLocBy = By.CssSelector("[id='btnIdSearchSubmit']");
        public By backToAccountSummaryLinkLocBy = By.CssSelector("[id='backtoAccountSummaryId'],[class='back-button']");

        public By backToManageAutopayButtonFromDeleteLocBy = By.Id("agreeBtn");
        public By backToManageAutoPayButtonLocBy = By.Id("btnIdSearchSubmit");

        public By backToManageAutopayLinkLocBy = By.Id("backToAutopayLabelId");
        public By bankAccountDropdownLocBy = By.XPath("//mat-select[@id='bankAccountSelect']//div[contains(@id,'mat-select')]");
        public By biWeeklyRadioLocBy = By.Id("biWeeklyPaymentRadioBtn");
        public By confirmationNumberTextLocBy = By.XPath("//div[text()='Confirmation Number']/following-sibling::div");
        public By confirmPaymentButtonLocBy = By.XPath("//span[text()='Confirm Payment']");
        public By continueButtonLocBy = By.CssSelector("button[id='btnClose']");
        public By datesAvailableToSelectOnCalndrLocBy = By.XPath("//td[contains(@class,'mat-calendar') and not(@aria-disabled)]/div[contains(@class,'content')]");
        public By deleteIconManageAutopayLocBy = By.Id("linkIdActionDelete_");
        public string deleteOtpXpath = "//*[contains(text(),'{0}')]//../..//img[@id='pendingdelete']";
        public string deleteAutopayXpath = "//*[contains(text(),'{0}')]//../..//img[contains(@id,'delete')]";
        public By delinquencyReasonDropdownLocBy = By.CssSelector("mat-select[formcontrolname='delinquencyReason']");
        public By delinquencyReasonDropDownLocBy = By.XPath("//mat-select[@formcontrolname='delinquencyReason']");
        public string delinquencyReasonText = "//span[@class='mat-option-text']/span[text()='<REASON>']";
        public By disclosureLabelReviewPageTextLocBy = By.CssSelector("[id='cardIdPaymentReview']");
        public By divTotalAmountLocBy = By.XPath("//div[text()='Total Payment']/following-sibling::div");
        public By editAutopayConfirmationPageLocBy = By.Id("cardIdPaymentPrint");
        public By editAutopayPageTextContentLocBy = By.Id("cardIdPaymentActivity");
        public By editAutopayReviewPageTextContentLocBy = By.Id("cardIdPaymentReview");
        public By editIconManageAutopayLocBy = By.Id("linkIdActionEdit_");
        public string editOtpXpath = "//*[contains(text(),'{0}')]//../..//*[@id='pendingEdit']";
        public string editAutopayXPath= "//*[contains(text(),'{0}')]//../..//img[contains(@id,'Edit')]";
        public By expandArrowLocBy = By.CssSelector("[aria-label='Expand']");
        public By lateFeeAmountCheckboxLocBy = By.XPath("//mat-label[text()='Late Fee']/ancestor::mat-form-field/preceding-sibling::mat-checkbox//input");
        public By lateFeeCheckBoxLocBy = By.CssSelector("[formcontrolname='feeCheckbox'] label span");
        public By lateFeeTextboxLocBy = By.XPath("//mat-label[text()='Late Fee']/ancestor::mat-form-field//input");
        public By annualMaintainenceFeeTextboxLocBy = By.XPath("//mat-label[text()='Annual Maintenance Fee']/ancestor::mat-form-field//input");
        public By leaveButtonOnLeavingPagePopupLocBy = By.XPath("//span[text()='Leave']/..");
        public By makeAPaymentButtonLocBy = By.CssSelector("p[class$='make-payment']");
        public By makePaymentBtnLocBy = By.XPath("//*[contains(text(),'Make a Payment')]");
        public By manageAutoPayHeaderTextLocBy = By.Id("spidPaymentActivity");
        public By expandMoreDownArrowOnManageAutopayLocBy = By.CssSelector("mat-icon[aria-label='Expand']");
        public By additionalPrincipalBalanceOnManageAutopayTextLocBy = By.CssSelector("span[id^='additionalPrincipal_']");
        public By monhlyPaymentlabelLocBy = By.XPath("//*[contains(text(),'Monthly Payment:')]");
        public By monthlyAmountValidationMsgLocBy = By.XPath("//*[contains(@id,'mat-hint')]//div");
        public By monthlyPaymentPlanAmountTextLocBy = By.CssSelector("[id='monthlyPaymentElmt'] + span");
        public By nextDebitDateManageAutoPayTextLocBy = By.Id("sppaymentDate_");
        public By nextMonthButtonCalanderLocBy = By.XPath("//button[@aria-label='Next month']");
        public By nSFFeeAmountCheckBoxLocBy = By.XPath("//mat-label[text()='NSF Fee']/ancestor::mat-form-field/preceding-sibling::mat-checkbox//input");
        public By nSFFeeTextLocBy = By.XPath("//mat-label[text()='NSF Fee']/ancestor::mat-form-field//input");
        public By numPaymentOnManageAutopayLocBy = By.CssSelector("[id='tblIdPaymentActivity'] tbody");
        public By otherFeeAmountCheckboxLocBy = By.XPath("//mat-label[text()='Other Fees']/ancestor::mat-form-field/preceding-sibling::mat-checkbox//input");
        public By otherFeeAmountTextboxLocBy = By.XPath("//div[@id='OtherFees']//input[@id='feeInput']");
        public By otpConfirmationMessageTextLocBy = By.XPath("//span[text()='You’ve successfully scheduled the payment below']");
        public By otpConfirmedPaymentMsgLocBy = By.Id("cardIdPaymentPrint");
        public By otpDeleteReasonLocBy = By.XPath("//*[contains(text(),'ll be paying another way ')]");
        public string paraByText = "//p[contains(text(),'<TEXT>')]";
        public By pastDueAmountRadioButtonLocBy = By.CssSelector("[id='pastDueAmountRadioBtn']");
        public By pastDueRadioButtonAmountTextLocBy = By.XPath("//div[@id='amountCheckDiv']//span[contains(@class,'semi-bold')] | //div[@id='pastdueAmountElmt']//span[contains(@class,'semi-bold')] | //div[@id='pastdueAmountElmt']/following-sibling::span[contains(@class,'semi-bold')]");
        public By pastDueRadioButtonLocBy = By.CssSelector("div[id='pastdueAmountElmt'] span[class*='mat-radio-outer-circle']");
        public By payingFromTextLocBy = By.XPath("//div[text()='Paying From']/following-sibling::div");
        public By paymentAlreadyExistErrorLocBy = By.XPath("//*[contains(@id,'paymentAlreadyExistsError')]");
        public By paymentDateLocBy = By.XPath("//*[@id='paymentDateElement']//mat-form-field");
        public By paymentDateTextLocBy = By.XPath("//div[contains(text(),'Payment Date')]/following-sibling::div");
        public By paymentFormLocBy = By.Id("paymentForm");
        public By paymentsNotAllowedAutopayPageMsgTextLocBy = By.Id("PaymentNotAllowed");
        public By planForAutopayPaymentManageAutopayTextLocBy = By.Id("spIdFrequency_");
        public By prepayCheckboxLocBy = By.XPath("//input[@id='prePaymentCheckBox-input']");
        public By prePaymentCheckBoxAmountLocBy = By.XPath("//mat-checkbox[@id='prePaymentCheckBox']/parent::div/following-sibling::div/span");
        public By prePaymentCheckBoxLocBy = By.XPath("//mat-checkbox[@id='prePaymentCheckBox']//input");
        public By prepayPaymentCheckBoxLocBy = By.CssSelector("[for='prePaymentCheckBox-input']");
        public By reinstatementInputLocBy = By.Id("partialReinstatementInput");
        public By reinstatementRadioBtnLocby = By.Id("partialReinstatementRadioBtn");
        public By scheduledPaymentIsProcessingPopupHeaderLocBy = By.XPath("//div[text()='Scheduled Payment is Processing']");
        public string sectionTitleText = "//*[contains(local-name(),'mello-serve-ui')]//span[text()='<TITLE>']";
        public By sendASecureMessageButtonLocBy = By.XPath("//span[contains(text(),' Send a Secure Message ')]//parent::button");
        public By setupAutopaybuttonLocBy = By.XPath("//span[contains(text(),'Setup Autopay')]//parent::button");
        public By setupAutopayConfirmationPageTextContentLocBy = By.Id("auto-pay-wrapper");
        public By setupAutopayHeaderLocBy = By.CssSelector("[id='paymentForm'] span[id='spidPaymentActivity']");
        public By setupAutopayReviewPageTextContentLocBy = By.Id("cardIdPaymentReview");
        public By startingDateConfirmationPageTextLocBy = By.XPath("//div[contains(text(),'Start Date')]/following-sibling::div");
        public By startingDateReviewPageTextLocBy = By.XPath("//div[@class='col-md-7']/p[@class='p16']");
        public By totalAutoPayAmountSetUpAutoPayLocBy = By.XPath("//div[text()='Total Payment']/following-sibling::div");
        public By totalPaymentAmountTextLocBy = By.XPath("//mat-card[@id='cardIdPaymentReview']//p[@class='h4']");
        public By updateAutopayButtonEditAutopayLocBy = By.XPath("//span[text()=' Update Autopay ']/..");
        public By updateAutopayLocBy = By.XPath("//*[contains(text(),'Update Autopay')]/parent::button");
        public By updatePaymentBtnLocBy = By.XPath("//*[contains(text(),'Update Payment')]//..//parent::button");
        public By warningIconOnManageAutopayLocBy = By.Id("imgwarningIcon");
        public By autopayMonthlyAmountToDebitInfoIconLocBy = By.CssSelector("[id='pIdPayAutopayBreakdown'] > img");
        public By tooltipPleaseRevisitAfter16thTextMsgOnSetupaAutopayLocBy = By.CssSelector("div[class^='mat-tooltip']");
        public By modificationTrailAmountLocBy = By.CssSelector("[id='trialPlanRadioBtn'] + span");
        public By modificationTrailAmountBrokenLocBy = By.XPath(" //*[@id='amountCheckDiv']//span[contains(.,'$')]");
        public By repayAmountLocBy = By.CssSelector("[id='repaymentPlanRadioBtn'] + span");
        public By tblPendingPaymentsLocBy = By.Id("tblIdPaymentPending");
        public By loanStandingStatusLocBy = By.Id("pastDueDelinquent");
        public By helocStandingStatusLocBy = By.Id("pastDue");
        public By statusBannerLocBy = By.XPath("//span[contains(text(),'If you are experiencing difficulty')]");
        public By autopayPromptLocBy = By.Id("cardIdAutopayPrompt");
        public By autopayLinkInPrompt = By.XPath("//*[@id='cardIdAutopayPrompt']//a");

        public By currentPaymentCheckboxLocBy = By.Id("currentPaymentCheckbox-input");
        public By paymentAlreadyExistsErrorTextLocBy = By.CssSelector("div[id^='paymentAlreadyExistsError']");
        public By upcomingPaymentCheckboxLocBy = By.Id("upcomingPaymentCheckbox-input");
        public By upcomingPaymentAmountLocBy = By.XPath("//mat-checkbox[@id='upcomingPaymentCheckbox']/parent::div/following-sibling::span");
        public By lateFeeMessageTextLocBy = By.CssSelector("[id='latefeeInfoMsg1']");
        public By biweeklyMessageTextLocBy = By.XPath("//*[contains(text(),'Bi-weekly Payment Plan is not allowed')]");
        public By amountInputFiledOnAdvanceFromLineOfCredit = By.Id("amount");
        public By pleaseEnterDollorgreaterOrEqualTo1000OrUptoAvailableCreditLineErrorMsgLocBy = By.XPath($"//mat-error[text()='{Constants.CustomerPortalErrorMsgs.PleaseEnterDollorGreaterOrEqualTo1000AndUptoAvailableCreditLineErrorMsg}']");
        public By pleaseEnterAnAmountEqualToOrLessThanYourAvailableCreditLineErrorMsg = By.XPath($"//span[text()='{Constants.CustomerPortalErrorMsgs.PleaseEnterAnAmountEqualToOrLessThanYourAvailableCreditLineErrorMsg}']");
        public By addAnAccountLinkOnAdvanceFromLineOfCreditPageLocBy = By.XPath("//span[text()='Add an account']/parent::button");
        public By mfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy = By.CssSelector("ldsm-mfa-popup");
        public By verificationCodeInputFieldOnMfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy = By.CssSelector("input[name='verificationCode']");
        public By faqsLinkOnMFAPopUpLocBy = By.CssSelector("ldsm-mfa-popup a[href='/faqs']");
        public By verifyButtonOnMfaPopUpForAddAnAccountForAdvanceFromLineOfCreditPageLocBy = By.CssSelector("ldsm-mfa-popup button[type='submit']");
        public By requestFundsButtonLocBy = By.XPath("//button[text()=' Request Funds ']");
        public By drawRequestProcessingTimePopUpLocBy = By.CssSelector("mhp-confirm-draw");
        public By confirmButtonOnDrawRequestProcessingTimePopUpLocBy = By.XPath("//button[text()=' Confirm ']");
        public By cancelButtonOnDrawRequestProcessingTimePopUpLocBy = By.XPath("//button[text()=' Cancel ']");
        public By unableToProcessPaymentWarningPopUpLocBy = By.CssSelector("custom-error-msg");
        public By unableToProcessPaymentWarningPopUpForHelocLocBy = By.Id("cust-err");
        public By unableToProcessPaymentWarningPopUpCloseButtonLocBy = By.Id("btnClose");
        public By editAutopayTextLocBy = By.XPath("//*[contains(text(),'existing autopay schedule')]");

        #endregion Locators

        #region Services

        /// <summary>
        /// Method to Accept the Scheduled Payment is processing popup
        /// </summary>
        /// <param name="isReportRequired">true/false</param>
        public void AcceptScheduledPaymentIsProcessingPopUp(bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                webElementExtensions.WaitForVisibilityOfElement(_driver, scheduledPaymentIsProcessingPopupHeaderLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForElementToBeEnabled(_driver, scheduledPaymentIsProcessingPopupHeaderLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ClickElementUsingJavascript(_driver, continueButtonLocBy, "Scheduled Payment Is Processing Header", true);
            }
            catch (Exception ex)
            {
                test.Log(Status.Error, $"Something happened while Accepting the Scheduled Payment is processing popup Error : <b>{ex.Message}</b>");
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully Accepted the Scheduled Payment is processing popup", "Failed while Accepting the Scheduled Payment is processing popup");
        }

        /// <summary>
        /// Delete the OTP payment set on given date
        /// </summary>
        /// <param name="date_of_exisiting_otp"></param>
        public void DeleteOtpPaymentSetup(DateTime date_of_exisiting_otp)

        {
            var date_formatted = String.Format("{0:MMM d, yyyy}", date_of_exisiting_otp);
            By deleteOtpLocBy = By.XPath(string.Format(deleteOtpXpath, date_formatted.ToString()));
            webElementExtensions.ClickElementUsingJavascript(_driver, deleteOtpLocBy, webElementName: "Delete Payment", isReportRequired: true);
        }


        /// <summary>
        /// Delete the Autopay payment set on given date
        /// </summary>
        /// <param name="date_of_exisiting_autopay"></param>
        public void DeleteAutopayPaymentSetup(DateTime date_of_exisiting_autopay)

        {
            var date_formatted = String.Format("{0:MMM d, yyyy}", date_of_exisiting_autopay);
            By deleteAutopayLocBy = By.XPath(string.Format(deleteAutopayXpath, date_formatted.ToString()));
            webElementExtensions.ClickElementUsingJavascript(_driver, deleteAutopayLocBy, webElementName: "Delete Payment", isReportRequired: true);
        }
        /// <summary>
        /// Edit the OTP payment set on given date
        /// </summary>
        /// <param name="date_of_exisiting_otp"></param>
        public void EditOtpPayment(DateTime date_of_exisiting_otp)

        {
            var date_formatted = String.Format("{0:MMM d, yyyy}", date_of_exisiting_otp);
            By editOtpLocBy = By.XPath(string.Format(editOtpXpath, date_formatted.ToString()));
            webElementExtensions.WaitForElementToBeEnabled(_driver, editOtpLocBy, isScrollIntoViewRequired: false);
            webElementExtensions.ClickElementUsingJavascript(_driver, editOtpLocBy, webElementName: "Edit Payment", isReportRequired: true);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
            webElementExtensions.WaitForElement(_driver, updatePaymentBtnLocBy);
            reportLogger.TakeScreenshot(test, "Edit OTP Page");
        }

        /// <summary>
        /// Edit the Autopay payment set on given date
        /// </summary>
        /// <param name="date_of_exisiting_autopay"></param>
        public void EditAutopayPayment(DateTime date_of_exisiting_autopay)

        {
            var date_formatted = String.Format("{0:MMM d, yyyy}", date_of_exisiting_autopay);
            By editAutopayLocBy = By.XPath(string.Format(editAutopayXPath, date_formatted.ToString()));
            webElementExtensions.ScrollToTop(_driver);
            webElementExtensions.WaitForElementToBeEnabled(_driver, editAutopayLocBy, isScrollIntoViewRequired: false);
            webElementExtensions.ClickElementUsingJavascript(_driver, editAutopayLocBy, webElementName: "Edit Payment", isReportRequired: true);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
            reportLogger.TakeScreenshot(test, "After Clicked on Edit Payment");
        }

        /// <summary>
        /// Method to navigate to Setup Autopay Page
        /// </summary>
        /// <param name="isReportRequired">true/false</param>
        public void NavigateToSetupAutopayPage(bool needToHandleScheduledPaymentInProgressPopup = false, bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                webElementExtensions.WaitUntilElementIsClickable(_driver, setupAutopaybuttonLocBy, ConfigSettings.WaitTime);
                webElementExtensions.ActionClick(_driver, setupAutopaybuttonLocBy, "Setup Autopay button.", isReportRequired: true);
                DashboardPage dashboard = new DashboardPage(_driver, test);
                dashboard.HandlePaperLessPage();
                if (needToHandleScheduledPaymentInProgressPopup)
                    AcceptScheduledPaymentIsProcessingPopUp();
                webElementExtensions.WaitForElement(_driver, bankAccountDropdownLocBy);
                flag = true;
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Unable to navigate to Setup Autopay Page: {e.Message}");
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Succesfully navigated to Setup Autopay Page", "Unable to navigate to Setup Autopay Page");
        }

        /// <summary>
        /// Method to select the Delinquency Reason
        /// </summary>
        /// <param name="reason">string "Curtailment of Income"</param>
        public void SelectDelinquencyReason(string reason)
        {
            webElementExtensions.ClickElementUsingJavascript(_driver, delinquencyReasonDropDownLocBy);
            _driver.FindElement(By.XPath(delinquencyReasonText.Replace("<REASON>", reason))).Click();
        }

        /// <summary>
        /// Method to set up Autopay functionality for both Agent Portal and Customer Portal
        /// </summary>
        /// <param name="loanNumber">Loan Number</param>
        /// <param name="email">email ID</param>
        /// <param name="principalInterestAmount">Principal Interest Amount</param>
        /// <param name="taxAndInsuranceAmount">Tax and Insurance Amount</param>
        /// <param name="isReportRequired">true / false</param>
        /// <returns></returns>
        public string SetupAutopay(List<string> pendingPaymentDates, string loanNumber = "", string email = "", string amount = "", string interest = "", string tax = "", bool isValidationRequired = false, bool isReportRequired = false, string autopayType = "Monthly", string opt_out_code = "0", bool isScroolIntoViewRequired = false)
        {
            List<bool> flag = new List<bool>();
            bool selecNextMonthNeeded = false;
            string paymentDate = string.Empty;
            try
            {
                string bankAccountNameAfterSelection = string.Empty, paymentDateToBeSelected = string.Empty;
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);

                if (autopayType == "Bi-Weekly")
                {
                    selecNextMonthNeeded = true;
                }

                paymentDate = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, selectNextMonth: selecNextMonthNeeded, isScrollIntoViewRequired: false);
                if (isScroolIntoViewRequired)
                    webElementExtensions.ScrollIntoView(_driver, totalAutoPayAmountSetUpAutoPayLocBy);
                webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.setupAutopayButtonInSetupAutopayScreenLocBy, isScrollIntoViewRequired: false);
                webElementExtensions.ActionClick(_driver, commonServices.setupAutopayButtonInSetupAutopayScreenLocBy, "Setup Autopay button.", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.setupAutopayButtonInSetupAutopayScreenLocBy);
                webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.confirmAutopayButtonLocBy, isScrollIntoViewRequired: false);

                if (isValidationRequired)
                {
                    test.Log(Status.Info, $"<b><u>Review Page Content Validation Started</u></b>");
                    string reviewPageContent = webElementExtensions.GetElementText(_driver, setupAutopayReviewPageTextContentLocBy, true);

                    reportLogger.TakeScreenshot(test, "Autopay Setup Review Page");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(amount), "Verify Total Amount on Review Page");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(autopayType), "Verify Autopay Plan Type on Review Page");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains("*******" + loanNumber.Substring(loanNumber.Length - 4)), "Verify Loan Number");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(paymentDate), "Verify Payment start date");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(bankAccountName), "Verify Bank Account Nickname");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(accountNumber.Substring(accountNumber.Length - 4)), "Verify Bank Account Number");
                    if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal) && test.Model.FullName.ToString().Contains("HelocAutoPay"))
                        ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(Constants.CustomerPortalTextMessages.HelocAutopaySetupReviewPageDisclosureText), "Verify Review Page Content");
                    else
                        ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupReviewPageDisclosureText), "Verify Review Page Content");
                    if (isScroolIntoViewRequired)
                        webElementExtensions.ScrollIntoView(_driver, commonServices.confirmAutopayButtonLocBy);
                    webElementExtensions.ClickElement(_driver, commonServices.confirmAutopayButtonLocBy, "Confirm Autopay button.", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                    webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.continueButtonInConfirmAutopayScreenLocBy, isScrollIntoViewRequired: false);


                    if (isValidationRequired)
                    {
                        reportLogger.TakeScreenshot(test, "Setup Autopay Number of Business days popup");
                        string actPopUpMsg = webElementExtensions.GetElementText(_driver, autopaySetupOneBusinessdayMsgTextLocBy);
                        if (commonServices.GetBankHolidays().Contains(DateTime.Today) || DateTime.Today.DayOfWeek == DayOfWeek.Saturday || DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                            ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.AutopaySetup2BusinessDayPopupMsg, actPopUpMsg, "Verify Pop up to Wait for no. of Business days to Setup Autopay");
                        else
                            ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.AutopaySetup1BusinessDayPopupMsg, actPopUpMsg, "Verify Pop up to Wait for no. of Business days to Setup Autopay");
                    }

                    webElementExtensions.ClickElement(_driver, commonServices.continueButtonInConfirmAutopayScreenLocBy, "Continue Button on Setup Autopay number of business days pop up", isReportRequired: true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.confirmAutopayButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    if (isValidationRequired)
                    {
                        test.Log(Status.Info, $"<b><u>Confirmation Page Content Validation Started</u></b>");
                        string confirmationPageTextContent = webElementExtensions.GetElementText(_driver, setupAutopayConfirmationPageTextContentLocBy, true);
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(amount), "verifying Total Amount on Autopay Setup Confirmation Page");
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(autopayType), "Verify Autopay Plan Type");
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains("*******" + loanNumber.Substring(loanNumber.Length - 4)), "Verify Loan Number");
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(paymentDate), "Verify Payment Start Date");
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(bankAccountName), "Verify Bank Account Name");
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(accountNumber.Substring(accountNumber.Length - 4)), "Verify Bank Account Number");

                        if (string.Equals(opt_out_code, "J", StringComparison.OrdinalIgnoreCase))
                        {
                            bool isMainling = confirmationPageTextContent.Contains("A notification will be sent to the mailing address on file through the US postal Service");
                            ReportingMethods.LogAssertionTrue(test, isMainling, "Mailing Address Check");
                        }
                        if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal) && test.Model.FullName.ToString().Contains("HelocAutoPay"))
                            ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.HelocAutopaySetupConfirmationPageDisclosureText), "Verify Autopay Confirmation Page Disclosure Text");
                        else
                            ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(Constants.CustomerPortalTextMessages.AutopaySetupConfirmationPageDisclosureText), "Verify Autopay Confirmation Page Disclosure Text");
                        reportLogger.TakeScreenshot(test, "Autopay Setup Confirmaion Page");
                    }
                }
            }
            catch (Exception ex)
            {
                test.Log(Status.Error, $"Something happened while Setting Up Autopay Error: {ex.Message}");
            }
            if (isReportRequired)
            {
                _driver.ReportResult(test, flag.All(f => f), $"Succesfully setup Autopay for Amount: {amount} and Payment Date: {paymentDate}", "Failed while setting up Autopay");
            }
            return (paymentDate);
        }
        /// <summary>
        /// Setup the OTP on eligible date
        /// </summary>
        /// <param name="pendingPaymentDates"></param>
        /// <param name="monthly_payment"></param>
        /// <param name="amount_due"></param>
        /// <param name="isValidationRequired"></param>
        /// <param name="isReportRequired"></param>
        /// <param name="emailId"></param>
        /// <param name="opt_out_code"></param>
        /// <returns>OTP setup date</returns>
        public DateTime SetupOTP(List<string> pendingPaymentDates, string monthly_payment = "", string amount_due = "", bool isValidationRequired = false, bool isReportRequired = false, string emailId = "", string opt_out_code = "0", bool isDefaultLoan = false, By monthlyAmountLocator = null)
        {
            List<bool> flag = new List<bool>();
            bool selecNextMonthNeeded = false;
            string monthly_payment_formatted = double.Parse(monthly_payment).ToString("N");
            string paymentDate = string.Empty;
            try
            {
                string bankAccountNameAfterSelection = string.Empty, paymentDateToBeSelected = string.Empty;
                string accountType = "Savings";
                commonServices.DeleteAllAddedBankAccounts();
                commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, accountType, routingNumber, accountNumber, isScrollIntoViewRequired: false, isReportRequired: false, isSelectBankAccountInTheEndRequired: true);

                webElementExtensions.ScrollToTop(_driver);
                if (!isDefaultLoan)
                    webElementExtensions.ClickElement(_driver, reinstatementRadioBtnLocby);
                string monthly_amount_actual;
                //Get Monthly calculated amount from default locator if not passed in parameter
                if (monthlyAmountLocator is null)
                    monthly_amount_actual = webElementExtensions.GetElementText(_driver, monhlyPaymentlabelLocBy);
                else
                    monthly_amount_actual = webElementExtensions.GetElementText(_driver, monthlyAmountLocator);
                monthly_amount_actual = monthly_amount_actual.Replace("$", "");
                ReportingMethods.LogAssertionContains(test, monthly_payment_formatted, monthly_amount_actual, "Compare Monthly Amount");
                if (!isDefaultLoan)
                    webElementExtensions.EnterText(_driver, reinstatementInputLocBy, monthly_payment, isScrollIntoViewRequired: false, isClickRequired: true);

                paymentDate = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);
                string total_amount = webElementExtensions.GetElementText(_driver, totalAutoPayAmountSetUpAutoPayLocBy);

                webElementExtensions.ClickElementUsingJavascript(_driver, makePaymentBtnLocBy, "Make a Payment", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, makePaymentBtnLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForElement(_driver, commonServices.confirmOtpPaymentBtnLocBy, ConfigSettings.WaitTime);

                if (isValidationRequired)
                {
                    test.Log(Status.Info, $"<b><u>Review Page Content Validation Started</u></b>");
                    string reviewPageContent = webElementExtensions.GetElementText(_driver, setupAutopayReviewPageTextContentLocBy, true);
                    reportLogger.TakeScreenshot(test, "OTP Setup Review Page");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(monthly_payment_formatted), reviewPageContent.Contains(monthly_payment_formatted) ? $"Successfully validated that Review page text content contains the expected Total Amount - <b>{monthly_payment}</b>" : $"Failed while verifying Total Amount - <b>{monthly_payment}</b>, please check the Review Page Text content in logs");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(paymentDate), reviewPageContent.Contains(paymentDate) ? $"Successfully validated that Review page text content contains the expected Payment Start Date - <b>{paymentDate}</b>" : $"Failed while verifying Payment Start Date - <b>{paymentDate}</b>, please check the Review Page Text content in logs");
                    ReportingMethods.LogAssertionTrue(test, reviewPageContent.Contains(bankAccountName.Substring(bankAccountName.Length - 4)), reviewPageContent.Contains(bankAccountName.Substring(bankAccountName.Length - 4)) ? $"Successfully validated that Review page text content contains the expected Bank Account Number - <b>{bankAccountName.Substring(bankAccountName.Length - 4)}</b>" : $"Failed while verifying Bank Account Number - </b>{bankAccountName.Substring(bankAccountName.Length - 4)}</b>, please check the Review Page Text content in logs");

                    webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.confirmOtpPaymentBtnLocBy, "Confirm Payment button.", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);

                    if (isValidationRequired)
                    {
                        test.Log(Status.Info, $"<b><u>Confirmation Page Content Validation Started</u></b>");
                        string confirmationPageTextContent = webElementExtensions.GetElementText(_driver, otpConfirmedPaymentMsgLocBy, true);
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(monthly_payment_formatted), confirmationPageTextContent.Equals(monthly_payment_formatted) ? $"Successfully validated that Autopay Setup Confirmation Page contains Total Amount - <b>{monthly_payment}</b>" : $"Verifying Total Amount on OTP Setup Confirmation Page - <b>{monthly_payment}</b>, please check the logs");
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains("Pending"), confirmationPageTextContent.Contains("Pending") ? $"Successfully validated Status in Confirmation Page text content contains the expected Status - <b>Pending</b>" : $"Verifying OTP Status - <b</b>, please check the Autopay Setup Confirmation Page Text content in logs");
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(paymentDate), confirmationPageTextContent.Contains(paymentDate) ? $"Successfully validated that Autopay Setup Autopay Confirmation page text content contains the expected Payment Start Date - <b>{paymentDate}</b>" : $"Verifying Payment Start Date - <b>{paymentDate}</b>, please check the OTP Setup Confirmation Page Text content in logs");
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(bankAccountName), confirmationPageTextContent.Contains(bankAccountName) ? $"Successfully validated that Setup Autopay Confirmation  page text content contains the expected Bank Account Name - <b>{bankAccountName}</b>" : $"Verifying Bank Account Name - <b>{bankAccountName}</b>, please check the Autopay Setup Autopay Confirmation Page Text content in logs");
                        ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(bankAccountName.Substring(bankAccountName.Length - 4)), confirmationPageTextContent.Contains(bankAccountName.Substring(bankAccountName.Length - 4)) ? $"Successfully validated that OTP Setup Confirmation Page Text content contains the expected Bank Account Number - <b>{bankAccountName.Substring(bankAccountName.Length - 4)}</b>" : $"Failed while verifying Bank Account Number - <b>{bankAccountName.Substring(bankAccountName.Length - 4)}</b>, please check the OTP Confirmation Page Text content in logs");

                        if (string.Equals(opt_out_code, "J", StringComparison.OrdinalIgnoreCase))
                        {
                            bool isMainling = confirmationPageTextContent.Contains("A notification will be sent to the mailing address on file through the US postal Service");
                            ReportingMethods.LogAssertionTrue(test, isMainling, "Mailing Address Check");
                        }
                        else
                        {
                            ReportingMethods.LogAssertionTrue(test, confirmationPageTextContent.Contains(UtilAdditions.MaskEmail(emailId)), confirmationPageTextContent.Contains(UtilAdditions.MaskEmail(emailId)) ? $"Successfully validated that Review page text content contains the expected Email - <b>{UtilAdditions.MaskEmail(emailId)}</b>" : $"Failed while verifying EmailId - <b>{UtilAdditions.MaskEmail(emailId)}</b>, please check the Review Page Text content in logs");
                        }

                        reportLogger.TakeScreenshot(test, "OTP Setup Confirmaion Page");
                    }
                }
            }
            catch (Exception ex)
            {
                test.Log(Status.Error, $"Something happened while Setting Up OTP Error: {ex.Message}");
            }
            if (isReportRequired)
            {
                _driver.ReportResult(test, flag.All(f => f), $"Succesfully setup OTP for Amount: {monthly_payment} and Payment Date: {paymentDate}", "Failed while setting up OTP");
            }
            return (DateTime.Parse(paymentDate, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Get the default selected bank account details
        /// </summary>
        public BankAccount GetDefaultBankAccountDetails()
        {
            BankAccount account = null;
            try
            {
                By manageAccountsLinkLocBy = By.XPath(commonServices.buttonByText.Replace("<BUTTONNAME>", "Manage Accounts"));
                webElementExtensions.WaitForElement(_driver, manageAccountsLinkLocBy);
                webElementExtensions.ScrollIntoView(_driver, manageAccountsLinkLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, manageAccountsLinkLocBy);
                account = commonServices.GetAllExisitingBankAccounts()
                    .Where(a => a.Default).First();
                webElementExtensions.WaitForElementToBeEnabled(_driver, commonServices.backToPaymentsButtonLocBy, "Back to Make Payments", isScrollIntoViewRequired: true);
                webElementExtensions.ClickElementUsingJavascript(_driver, commonServices.backToPaymentsButtonLocBy, "Back to Make Payments");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                test.Log(Status.Info, $"Deafult Account Found is  {account}");
            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, "Error while getting bank accounts");
                test.Log(Status.Error, $"Error while finding default bank account : {e.Message}");
            }
            return account;

        }
        /// <summary>
        /// Verify Autopay details for active plan
        /// </summary>
        ///<param name = "isReportRequired" > true / false </ param >
        public void VerifyAutopayDetailsForActivePlan(bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                string paymentDetail = webElementExtensions.GetElementText(_driver, numPaymentOnManageAutopayLocBy, true);
                if (webElementExtensions.IsElementDisplayed(_driver, numPaymentOnManageAutopayLocBy, $"Payment Detail Present: <b>{paymentDetail}</b>"))
                {
                    flag = true;
                    test.Pass($"Payment Details: <b>{paymentDetail}</b> is Displayed on Manage Autopay Page");
                    reportLogger.TakeScreenshot(test, "Payment Details on Manage Autopay Page");
                }
                else
                {
                    flag = false;
                    test.Fail("Payment details on Manage Autopay Page are not displayed");
                    reportLogger.TakeScreenshot(test, "Payment Details did not show up on Manage Autopay Page");
                }
            }
            catch (Exception ex)
            {
                test.Log(Status.Error, $"Failed to verify details for Active Autopay Plan Error: {ex.Message}");
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Succesfully verified details for Active Autopay Plan", "Failed to verify details for Active Autopay Plan");
        }

        /// <summary>
        /// Verify Autopay details for inactive plan
        /// </summary>
        /// <param name="isReportRequired">true/false</param>
        public void VerifyAutopayDetailsForInactivePlan(bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                if (webElementExtensions.IsElementDisplayed(_driver, warningIconOnManageAutopayLocBy, "Warning message on Manage Autopay Page"))
                {
                    flag = true;
                    test.Pass("Warning message on Manage Autopay Page is Displayed");
                }
                else
                {
                    flag = false;
                    test.Fail("Warning message on Manage Autopay Page is Not Displayed");
                }
                reportLogger.TakeScreenshot(test, "Manage Autopay Page");
            }
            catch (Exception ex)
            {
                test.Log(Status.Error, $"Failed to verify details for inactive Autopay Plan Error: {ex.Message}");
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Succesfully verified details for Inactive Autopay Plan", "Failed to verify details for Inactive Autopay Plan");
        }
        /// <summary>
        /// Method to check OTP Page Title Is Displayed
        /// </summary>
        /// <param name="title">OTP Title</param>
        public void VerifyOTPPageTitleIsDisplayed(string title, bool isReportRequired = false, bool isScrollIntoViewRequired = true)
        {
            bool flag = false;
            try
            {
                By elementLocBy = By.XPath(sectionTitleText.Replace("<TITLE>", title));
                webElementExtensions.WaitForElement(_driver, elementLocBy);
                if (isScrollIntoViewRequired)
                    webElementExtensions.ScrollIntoView(_driver, elementLocBy);
                webElementExtensions.IsElementDisplayed(_driver, elementLocBy, "page title is displayed as " + title, isScrollIntoViewRequired);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying the Element " + title + " is Displayed " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got verified the Element " + title + " is Displayed.", "Failed while getting verifying the Element " + title + " is Displayed.");
        }

        /// <summary>
        /// Method to verify Payment fields
        /// </summary>
        /// <param name="loanLevelData">Hashtable LoanDetails</param>
        /// <param name="additionalPrincipalParam">doulbe 10.00</param>
        /// <param name="additionalEscrowAmountParam">doulbe 15.00</param>
        /// <param name="additionalPrincipalEditParam">doulbe 15.00</param>
        /// <param name="additionalEscrowEditAmountParam">doulbe 20.00</param>
        /// <param name="type">"setup" / "edit"</param>
        /// <param name="isScrollIntoViewRequired">true/false</param>
        /// <returns>string total payment</returns>
        public string VerifyOTPPaymentFields(Hashtable loanLevelData, double additionalPrincipalParam, double additionalEscrowAmountParam, double additionalPrincipalEditParam = 0.0, double additionalEscrowEditAmountParam = 0.0, string type = "setup", bool isScrollIntoViewRequired = true, APIConstants.HelocLoanInfo helocLoanInfo = null)
        {
            double totalPayment = 0.00;
            string expTotalPaymentAfterFee = string.Empty;
            try
            {
                bool radiobuttonStatus, flag;
                double additionalPrincipalAmount = 0.00;
                double pastDueAmount = 0.00, prepaidAmount = 0.00;
                double intialTotalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.Log(test, "Intial Total Payment is" + " " + "$" + intialTotalPayment);

                if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) > 1)
                {
                    pastDueAmount = Convert.ToDouble(webElementExtensions.GetElementText(_driver, pastDueRadioButtonAmountTextLocBy).Replace("$", ""));
                    radiobuttonStatus = commonServices.IsRadiobuttonSelected(pastDueAmountRadioButtonLocBy); //pastDueRadioButtonLocBy
                    if (radiobuttonStatus == false)
                    {
                        ReportingMethods.LogAssertionFalse(test, radiobuttonStatus, "Verify Past Due Amount field Radio Button is not selected Initially");
                        webElementExtensions.ClickElementUsingJavascript(_driver, pastDueRadioButtonLocBy, "Past Due radio button");
                    }
                    else { ReportingMethods.LogAssertionTrue(test, radiobuttonStatus, "Verify Past Due Amount field Radio Button is selected Initially"); }

                    if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) < 3)
                    {
                        if (type.ToLower() == "edit")
                            webElementExtensions.WaitForElementToBeEnabled(_driver, additionalPrincipalTextboxLocBy, isScrollIntoViewRequired: false);
                        else
                            webElementExtensions.WaitForElementToBeDisabled(_driver, additionalPrincipalTextboxLocBy, isScrollIntoViewRequired: false);
                        webElementExtensions.ScrollToTop(_driver);
                    }
                }

                if (_driver.FindElements(prepayCheckboxLocBy).Count > 0)//Prepaid
                {
                    flag = Convert.ToBoolean(webElementExtensions.GetElementAttribute(_driver, prepayCheckboxLocBy, Constants.ElementAttributes.AriaChecked));
                    if (flag == false)
                    {
                        ReportingMethods.LogAssertionFalse(test, flag, "Verify initially Prepay Check Box is not checked");
                    }
                    if (type.ToLower() == "setup")
                        webElementExtensions.ClickElementUsingJavascript(_driver, prepayCheckboxLocBy, "Prepay Check Box");
                    prepaidAmount = Convert.ToDouble(webElementExtensions.GetElementText(_driver, prePaymentCheckBoxAmountLocBy).Replace("$", ""));
                    if (type.ToLower() == "edit") { prepaidAmount = 0.00; }
                }

                if (test.Model.FullName.Contains("HelocOTPPastDue") && helocLoanInfo.IsBillGenerated)
                {
                    var feeDictionary = helocLoanInfo.FeeBreakdown
                        .SelectMany(fb => fb.Fees)
                        .GroupBy(fee => fee.Description)
                        .ToDictionary(
                            g => g.Key,
                            g => Convert.ToDouble(g.First().Amount)
                        );

                    foreach (var fee in feeDictionary)
                    {
                        if (fee.Key == "LATE CHARGE FEE")
                        {
                            double actLateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                            ReportingMethods.LogAssertionTrue(test, fee.Value == actLateFee, $"Verify the Late Fee Amount should be : {fee.Value}");
                        }
                        if (fee.Key == "ANNUAL MAINT FEE")
                        {
                            double actAnnualMaintenanceFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, annualMaintainenceFeeTextboxLocBy).Replace("$", ""));
                            ReportingMethods.LogAssertionTrue(test, fee.Value == actAnnualMaintenanceFee, $"Verify the Annual Maintenance Fee Amount should be : {fee.Value}");
                        }
                    }

                    double actUpcommingPaymentAmount = Convert.ToDouble(webElementExtensions.GetElementText(_driver, upcomingPaymentAmountLocBy).Replace("$", ""));
                    ReportingMethods.Log(test, $"Upcoming Payment Amount: {actUpcommingPaymentAmount}");

                    bool isCurrentPaymentCheckBoxChecked = webElementExtensions.GetElementAttribute(_driver, currentPaymentCheckboxLocBy, Constants.ElementAttributes.AriaChecked) == "true";
                    ReportingMethods.LogAssertionTrue(test, isCurrentPaymentCheckBoxChecked, "Verify if the current payment checkbox is checked");
                    bool isUpcomingPaymentCheckBoxChecked = webElementExtensions.GetElementAttribute(_driver, upcomingPaymentCheckboxLocBy, Constants.ElementAttributes.AriaChecked) == "false";
                    ReportingMethods.LogAssertionTrue(test, isUpcomingPaymentCheckBoxChecked, "Verify if the Upcoming payment checkbox is not checked");


                    webElementExtensions.ClickElementUsingJavascript(_driver, upcomingPaymentCheckboxLocBy, "Upcoming Payment checkbox", true);
                    reportLogger.TakeScreenshot(test, "Verify Upcoming Payment checkbox after selecting it");
                    isUpcomingPaymentCheckBoxChecked = webElementExtensions.GetElementAttribute(_driver, upcomingPaymentCheckboxLocBy, Constants.ElementAttributes.AriaChecked) == "true";
                    ReportingMethods.LogAssertionTrue(test, isUpcomingPaymentCheckBoxChecked, "Verify if the Upcoming payment checkbox is checked");
                    bool isAdditionalPaymentCheckBoxChecked = webElementExtensions.GetElementAttribute(_driver, additionalPaymentCheckBoxLocBy, Constants.ElementAttributes.AriaChecked) == "false";
                    ReportingMethods.LogAssertionTrue(test, isAdditionalPaymentCheckBoxChecked, "Verify if the Additional payment checkbox is not checked");

                    webElementExtensions.ClickElementUsingJavascript(_driver, additionalPaymentCheckBoxLocBy, "Additional Payment checkbox", true);
                    isAdditionalPaymentCheckBoxChecked = webElementExtensions.GetElementAttribute(_driver, additionalPaymentCheckBoxLocBy, Constants.ElementAttributes.AriaChecked) == "true";
                    ReportingMethods.LogAssertionTrue(test, isAdditionalPaymentCheckBoxChecked, "Verify if the Additional payment checkbox is checked");
                    reportLogger.TakeScreenshot(test, "Verify Additional Payment checkbox after selecting it");
                    webElementExtensions.ClickElementUsingJavascript(_driver, upcomingPaymentCheckboxLocBy, "Upcoming Payment checkbox", true);
                    reportLogger.TakeScreenshot(test, "Verify Upcoming Payment checkbox after Unchecking it");
                }

                totalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                double lateFee = 0.00;
                double nSFFee = 0.00;
                double otherFee = 0.00;
                if (_driver.FindElements(lateFeeAmountCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
                {
                    if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal.ToString()) && !test.Model.FullName.ToString().Contains("Heloc"))
                    {
                        webElementExtensions.ClickElementUsingJavascript(_driver, lateFeeAmountCheckboxLocBy, "Late Fee Checkbox", true);
                        lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                        expTotalPaymentAfterFee = "$" + (intialTotalPayment +/** pastDueAmount**/ +lateFee + prepaidAmount).ToString("N");
                    }
                    else
                        expTotalPaymentAfterFee = "$" + (intialTotalPayment).ToString("N");
                    lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                    totalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), expTotalPaymentAfterFee, $"Verify Total Payment with LateFee calculation");
                }
                if (_driver.FindElements(nSFFeeAmountCheckBoxLocBy).Count > 0 && type.ToLower() == "setup" && !loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance].ToString().Equals("0.00"))
                {
                    if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal.ToString()) && !test.Model.FullName.ToString().Contains("Heloc"))
                    {
                        webElementExtensions.ClickElementUsingJavascript(_driver, nSFFeeAmountCheckBoxLocBy, "NSF Fee Checkbox", true);
                        nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextLocBy).Replace("$", ""));
                        expTotalPaymentAfterFee = "$" + (intialTotalPayment +/** pastDueAmount**/ +nSFFee + prepaidAmount).ToString("N");
                    }
                    else
                        expTotalPaymentAfterFee = "$" + (intialTotalPayment).ToString("N");
                    nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextLocBy).Replace("$", ""));
                    totalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), expTotalPaymentAfterFee, $"Verify Total Payment with NSF Fee calculation");
                }
                if (_driver.FindElements(otherFeeAmountCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
                {
                    if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal.ToString()) && !test.Model.FullName.ToString().Contains("Heloc"))
                    {
                        webElementExtensions.ClickElementUsingJavascript(_driver, otherFeeAmountCheckboxLocBy, "Other Fee Checkbox", isReportRequired: true);
                        otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                        expTotalPaymentAfterFee = "$" + (intialTotalPayment + lateFee + otherFee + prepaidAmount).ToString("N");
                    }
                    else
                        expTotalPaymentAfterFee = "$" + (intialTotalPayment).ToString("N");

                    otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                    totalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), expTotalPaymentAfterFee, $"Verify Total Payment with OtherFee calculation");
                }

                if (webElementExtensions.GetElementAttribute(_driver, addPaymentCheckBoxLocBy, Constants.ElementAttributes.AriaChecked) == "false")
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, addPaymentCheckBoxLocBy, "Additional Payment (Principal Only, Escrow, etc.) CheckBox");
                }
                if (!test.Model.FullName.Contains("HelocOTPPastDue"))
                    webElementExtensions.WaitForElementToBeEnabled(_driver, additionalPrincipalTextboxLocBy, "Additional Principal TextBox", isScrollIntoViewRequired: isScrollIntoViewRequired);

                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, lateFeeTextboxLocBy))
                    lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, nSFFeeTextLocBy))
                    nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextLocBy).Replace("$", ""));
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, otherFeeAmountTextboxLocBy))
                    otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                if (!test.Model.FullName.Contains("HelocOTPPastDue"))
                {
                    if (_driver.FindElements(additionalPrincipalTextboxLocBy).Count > 0)
                    {
                        if (_driver.FindElement(additionalPrincipalTextboxLocBy).Enabled)
                        {
                            ReportingMethods.Log(test, "Additional Principal Text Box is Enabled !!!");
                            if (type == "setup")
                            {
                                webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalParam * 100).ToString(), isScrollIntoViewRequired: isScrollIntoViewRequired, isClickRequired: true, isReportRequired: true);
                                ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalParam));
                            }
                            else
                            {
                                webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalEditParam * 100).ToString(), isScrollIntoViewRequired: isScrollIntoViewRequired, isClickRequired: true, isReportRequired: true);
                                ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalEditParam));
                            }
                            var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
                            totalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                            additionalPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
                            ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
                            if (type.ToLower() == "edit")
                            {
                                additionalPrincipalAmount = additionalPrincipalEditParam - additionalPrincipalParam;
                            }
                            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal.ToString()) && !test.Model.FullName.ToString().Contains("Heloc") && type == "setup")
                                expTotalPaymentAfterFee = "$" + (intialTotalPayment + lateFee + nSFFee + otherFee + prepaidAmount + additionalPrincipalAmount).ToString("N");
                            else
                                expTotalPaymentAfterFee = "$" + (intialTotalPayment + additionalPrincipalAmount).ToString("N");
                            ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), expTotalPaymentAfterFee, $"Verify Total Payment with Additional Principal calculation");
                        }
                    }
                    else
                        ReportingMethods.Log(test, "Additional Principal Text Box is Disabled !!!");
                    double escrowAmount = 0.00;
                    if (_driver.FindElements(additionalEscrowTextboxLocBy).Count > 0)
                    {
                        if (_driver.FindElement(additionalEscrowTextboxLocBy).Enabled)
                        {
                            ReportingMethods.Log(test, "Additional Escrow Text Box is Enabled !!!");
                            if (type == "setup")
                            {
                                webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowAmountParam * 100).ToString(), isScrollIntoViewRequired: isScrollIntoViewRequired, isClickRequired: true, isReportRequired: true);
                                ReportingMethods.Log(test, "Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowAmountParam));
                            }
                            else
                            {
                                webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowEditAmountParam * 100).ToString(), isScrollIntoViewRequired, isClickRequired: true);
                                ReportingMethods.Log(test, "Edit Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowEditAmountParam));
                            }
                            totalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                            ReportingMethods.Log(test, "Total Payment After Additional Escrow Amount Entered: " + "$" + totalPayment.ToString());
                            var addEscrowAmount = commonServices.GetValueUsingJS(additionalEscrowTextboxLocBy);
                            escrowAmount = Convert.ToDouble(addEscrowAmount.ToString().Trim().Replace("$", ""));
                            if (type.ToLower() == "edit")
                            {
                                escrowAmount = additionalEscrowEditAmountParam - additionalEscrowAmountParam;
                            }
                            if (type == "setup")
                                expTotalPaymentAfterFee = "$" + (intialTotalPayment + lateFee + nSFFee + otherFee + prepaidAmount + additionalPrincipalAmount + escrowAmount).ToString("N");
                            else
                                expTotalPaymentAfterFee = "$" + (intialTotalPayment + additionalPrincipalAmount + escrowAmount).ToString("N");
                            ReportingMethods.LogAssertionEqual(test, expTotalPaymentAfterFee, "$" + totalPayment.ToString("N"), $"Verify Total Payment with Additional Principal and Escrow calculation");
                        }
                    }
                    else
                        ReportingMethods.Log(test, "Additional Escrow Text Box is Disabled !!!");
                }

            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying the OTP Payments Fields" + ex.Message);
            }
            return "$" + totalPayment.ToString("N");
        }

        /// <summary>
        /// Method to verify Payment fields with connecting to Db 
        /// </summary>
        /// <param name="additionalPrincipalParam">doulbe 10.00</param>
        /// <param name="additionalEscrowAmountParam">doulbe 15.00</param>
        /// <param name="additionalPrincipalEditParam">doulbe 15.00</param>
        /// <param name="additionalEscrowEditAmountParam">doulbe 20.00</param>
        /// <param name="type">"setup" / "edit"</param>
        /// <param name="isScrollIntoViewRequired">true/false</param>
        /// <returns>string total payment</returns>
        public string VerifyOTPPaymentFields(double additionalPrincipalParam, double additionalEscrowAmountParam, double additionalPrincipalEditParam = 0.0, double additionalEscrowEditAmountParam = 0.0, string type = "setup", bool isScrollIntoViewRequired = true)
        {
            double totalPayment = 0.00;
            try
            {
                bool radiobuttonStatus, flag;
                double additionalPrincipalAmount = 0.00;
                double pastDueAmount = 0.00, prepaidAmount = 0.00;
                double intialTotalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.Log(test, "Intial Total Payment is" + " " + "$" + intialTotalPayment);

                totalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                double lateFee = 0.00;
                double nSFFee = 0.00;
                double otherFee = 0.00;

                if (_driver.FindElements(lateFeeAmountCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, lateFeeAmountCheckboxLocBy, "Late Fee Checkbox", true);
                    lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                    totalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment +/** pastDueAmount**/ +lateFee + prepaidAmount).ToString("N"), $"Verify Total Payment with LateFee calculation");
                }
                if (_driver.FindElements(nSFFeeAmountCheckBoxLocBy).Count > 0 && type.ToLower() == "setup")
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, nSFFeeAmountCheckBoxLocBy, "NSF Fee Checkbox", true);
                    nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextLocBy).Replace("$", ""));
                    totalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment +/** pastDueAmount**/ +nSFFee + prepaidAmount).ToString("N"), $"Verify Total Payment with NSF Fee calculation");
                }
                if (_driver.FindElements(otherFeeAmountCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, otherFeeAmountCheckboxLocBy, "Other Fee Checkbox", isReportRequired: true);
                    otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                    totalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + pastDueAmount + lateFee + otherFee + prepaidAmount).ToString("N"), $"Verify Total Payment with OtherFee calculation");
                }

                if (webElementExtensions.GetElementAttribute(_driver, addPaymentCheckBoxLocBy, Constants.ElementAttributes.AriaChecked) == "false")
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, addPaymentCheckBoxLocBy, "Additional Payment (Principal Only, Escrow, etc.) CheckBox");
                }

                webElementExtensions.WaitForElementToBeEnabled(_driver, additionalPrincipalTextboxLocBy, "Additional Principal TextBox", isScrollIntoViewRequired: isScrollIntoViewRequired);
                if (_driver.FindElements(additionalPrincipalTextboxLocBy).Count > 0)
                {
                    if (_driver.FindElement(additionalPrincipalTextboxLocBy).Enabled)
                    {
                        ReportingMethods.Log(test, "Additional Principal Text Box is Enabled !!!");
                        if (type == "setup")
                        {
                            webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalParam * 100).ToString(), isScrollIntoViewRequired: isScrollIntoViewRequired, isClickRequired: true, isReportRequired: true);
                            ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalParam));
                        }
                        else
                        {
                            webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalEditParam * 100).ToString(), isScrollIntoViewRequired: isScrollIntoViewRequired, isClickRequired: true, isReportRequired: true);
                            ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalEditParam));
                        }
                        var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                        totalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                        additionalPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
                        ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
                        if (type.ToLower() == "edit")
                        {
                            additionalPrincipalAmount = additionalPrincipalEditParam - additionalPrincipalParam;
                        }
                        ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + /**pastDueAmount +**/ lateFee + nSFFee + otherFee + prepaidAmount + additionalPrincipalAmount).ToString("N"), $"Verify Total Payment with Additional Principal calculation");
                    }
                }
                else
                    ReportingMethods.Log(test, "Additional Principal Text Box is Disabled !!!");
                double escrowAmount = 0.00;
                if (_driver.FindElements(additionalEscrowTextboxLocBy).Count > 0)
                {
                    if (_driver.FindElement(additionalEscrowTextboxLocBy).Enabled)
                    {
                        ReportingMethods.Log(test, "Additional Escrow Text Box is Enabled !!!");
                        if (type == "setup")
                        {
                            webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowAmountParam * 100).ToString(), isScrollIntoViewRequired: isScrollIntoViewRequired, isClickRequired: true, isReportRequired: true);
                            ReportingMethods.Log(test, "Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowAmountParam));
                        }
                        else
                        {
                            webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowEditAmountParam * 100).ToString(), isScrollIntoViewRequired, isClickRequired: true);
                            ReportingMethods.Log(test, "Edit Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowEditAmountParam));
                        }
                        totalPayment = Convert.ToDouble(commonServices.GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                        ReportingMethods.Log(test, "Total Payment After Additional Escrow Amount Entered: " + "$" + totalPayment.ToString());
                        var addEscrowAmount = commonServices.GetValueUsingJS(additionalEscrowTextboxLocBy);
                        escrowAmount = Convert.ToDouble(addEscrowAmount.ToString().Trim().Replace("$", ""));
                        if (type.ToLower() == "edit")
                        {
                            escrowAmount = additionalEscrowEditAmountParam - additionalEscrowAmountParam;
                        }
                        ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + /**pastDueAmount +**/ lateFee + nSFFee + otherFee + prepaidAmount + additionalPrincipalAmount + escrowAmount).ToString("N"), $"Verify Total Payment with Additional Principal and Escrow calculation");
                    }
                }
                else
                    ReportingMethods.Log(test, "Additional Escrow Text Box is Disabled !!!");
                reportLogger.TakeScreenshot(test, $"Make Payment : {type}");
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying the OTP Payments Fields" + ex.Message);
            }
            return "$" + totalPayment.ToString("N");
        }

        /// <summary>
        /// Method to Verify Review And Confirmation Page Is Displayed
        /// </summary>
        /// <param name="type">Payment flow - setup/Edit</param>
        public void VerifyReviewAndConfirmationPageIsDisplayed(string type = "setup", bool isScrollIntoViewRequired = true)
        {
            By locBy = By.XPath(paraByText.Replace("<TEXT>", "Does everything look okay?"));
            webElementExtensions.WaitForElement(_driver, locBy);
            ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, locBy, isScrollIntoViewRequired: isScrollIntoViewRequired), type == "setup" ? "Verify payment review page loaded" : "Verify " + type + " payment review page loaded");
            reportLogger.TakeScreenshot(test, "Review And Confirmation Page");
        }

        /// <summary>
        /// Method to Verify Total Amount Payment Review Page
        /// </summary>
        /// <param name="totalPayment">Total payment Amount</param>
        /// <param name="type">Payment flow - Setup/Edit</param>
        /// <returns>Total Amount</returns>
        public string VerifyTotalAmountPaymentReviewPage(string totalPayment, string type = "setup")
        {
            webElementExtensions.WaitForElement(_driver, totalPaymentAmountTextLocBy);
            string amount = _driver.FindElement(totalPaymentAmountTextLocBy).Text;
            ReportingMethods.LogAssertionEqual(test, totalPayment, amount, type == "setup" ? "Verify Total Amount value" : "Verify " + type + " Total Amount value");
            return amount;
        }

        /// <summary>
        /// Method to Verify OTP Page Is Displayed
        /// </summary>
        /// <param name="type">Payment flow - setup/Edit</param>
        public void VerifytOTPPageTitle(string type = "setup")
        {
            webElementExtensions.WaitForElement(_driver, paymentFormLocBy);
            webElementExtensions.IsElementDisplayed(_driver, paymentFormLocBy, "Main payment page", false, true);
            reportLogger.TakeScreenshot(test, "Make a One-Time Payment Page");
        }
        /// <summary>
        /// Method to Verify Updated Disclosure Statement In Review And Confirm Screen
        /// </summary>
        /// <param name="accountNumber">Account number details</param>
        /// <param name="accountFullName">Account Full name details</param>
        /// <param name="selectedDate">Selected Date</param>
        /// <param name="totalPayment">Total Payment Amount</param>
        /// <param name="Type">Payment flow - setup/Edit</param>
        public void VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(string accountNumber, string accountFullName, string selectedDate, string totalPayment, string type = "setup", string nextPaymentDueDate = "January 1, 2025")
        {
            try
            {
                string lastfourAccountNumber = accountNumber.Substring(accountNumber.Length - 4);
                string date = selectedDate.ToString();
                string totalAmount = totalPayment;
                string amountInWords = webElementExtensions.NumberToCurrencyText(Convert.ToDecimal(totalAmount.Replace("$", "")));
                string actualDisclosure = _driver.FindElement(disclosureLabelReviewPageTextLocBy /**: disclosureLabelLocBy**/).Text;
                ReportingMethods.LogAssertionTrue(test, actualDisclosure.Contains(lastfourAccountNumber), "Verify Last Four Digits of Account Number on Review And Confirmation Page");
                ReportingMethods.LogAssertionTrue(test, actualDisclosure.Contains(accountFullName), "Verify Account Full Name on Review And Confirmation Page");
                ReportingMethods.LogAssertionTrue(test, actualDisclosure.Contains(totalAmount), "Verify Total Amount on Review And Confirmation Page");
                ReportingMethods.LogAssertionTrue(test, actualDisclosure.Contains(nextPaymentDueDate), "Verify Next Payment Due Date on Review And Confirmation Page");
                ReportingMethods.LogAssertionTrue(test, actualDisclosure.Contains(date), "Verify Selected Date on Review And Confirmation Page");
            }
            catch (Exception e)
            {
            }
        }

        /// <summary>
        /// Verify Unable To Select Payment Date On Weekends
        /// </summary>
        public void VerifyWeekendDatesAreNotEnabledForSelection()
        {
            var actPaymentDatesToEnabled = commonServices.GetAvailablePaymentDateInDateField(selectNextMonth: true);
            bool flag = true;
            foreach (var dateStr in actPaymentDatesToEnabled)
            {
                DateTime date = DateTime.Parse(dateStr);
                bool isWeekday = date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday;
                if (isWeekday == false)
                {
                    flag = false;
                    ReportingMethods.LogAssertionTrue(test, isWeekday, $"{dateStr} falls on a Weekend but enabled for selection on Calendar");
                }
            }
            reportLogger.TakeScreenshot(test, "Dates in Calendar");
            ReportingMethods.LogAssertionTrue(test, flag, $"Verify that there are no dates enabled on the Calendar for selection on WeekEnds");
        }

        /// <summary>
        /// To make an FM OTP Payment
        /// </summary>
        /// <param name="loanLevelData">Hashtable 1041177351</param>
        /// <param name="bankAccountName">string AutomationTeamBankAccount</param>
        /// <param name="firstName">string firstName</param>
        /// <param name="lastName">string lastName</param>
        /// <param name="personalOrBussiness">string personal/Bussiness</param>
        /// <param name="savings">string savings</param>
        /// <param name="routingNumber">string 122199983</param>
        /// <param name="accountNumber">string 1000</param>
        /// <param name="accountFullName">string accountFullName</param>
        /// <returns>string paymentDate</returns>
        public string MakeOTPPayment(Hashtable loanLevelData, string bankAccountName, string firstName, string lastName, string personalOrBussiness, string savings, string routingNumber, string accountNumber, string accountFullName)
        {
            DashboardPage dashboard = new DashboardPage(_driver, test);
            bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, dashboard.confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
            List<string> pendingPaymentDates = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy).Select(e => e.Text).ToList();

            test.Log(Status.Info, $"<b>********************************************<u>Started Process to Setup OTP Payment</u>********************************************</b>");

            webElementExtensions.ClickElementUsingJavascript(_driver, dashboard.btnMakePaymentLocBy);

            if (willScheduledPaymentInProgressPopAppear)
                AcceptScheduledPaymentIsProcessingPopUp();

            dashboard.HandlePaperLessPage();
            VerifytOTPPageTitle();
            VerifyOTPPageTitleIsDisplayed(Constants.PageNames.MakeAOneTimePayment, isScrollIntoViewRequired: false);

            commonServices.DeleteAllAddedBankAccounts();
            commonServices.AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, isReportRequired: false, isScrollIntoViewRequired: false, isSelectBankAccountInTheEndRequired: true);

            string totalPayment = VerifyOTPPaymentFields(loanLevelData, 10.00, 15.00, isScrollIntoViewRequired: false);
            string amountForSMC = totalPayment;

            var dateSelected = commonServices.SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: false);

            if (loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.Delinquent))
                SelectDelinquencyReason("Curtailment of Income");

            commonServices.ClickButtonUsingName(Constants.ButtonNames.MakeAPayment);

            VerifyReviewAndConfirmationPageIsDisplayed(isScrollIntoViewRequired: false);
            VerifyTotalAmountPaymentReviewPage(totalPayment);

            string nextPaymentDueDate = DateTime.Parse(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMMM d, yyyy");
            VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(accountNumber, accountFullName, dateSelected, totalPayment, "setup", nextPaymentDueDate);

            commonServices.ClickConfirmButtonPaymentReviewPage();
            commonServices.VerifyPaymentConfirmationSuccessMessageIsDisplayed(Constants.Messages.YouHaveSuccessfullyScheduledPaymentBelow, isScrollIntoViewRequired: false);

            string confirmationNumber = commonServices.GetConfirmationNumberFromPaymentsConfirmationPage();
            ReportingMethods.Log(test, $"Successfully made a one time payment with the following confirmation number - {confirmationNumber}");
            webElementExtensions.ScrollToBottom(_driver);
            webElementExtensions.WaitForStalenessOfElement(_driver, backToAccountSummaryBtnLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, backToAccountSummaryBtnLocBy, "Back To Account Summary Button", true);

            webElementExtensions.WaitUntilElementIsClickable(_driver, makePaymentBtnLocBy);
            webElementExtensions.WaitForVisibilityOfElement(_driver, dashboard.pendingPaymentDatesTextLocBy, ConfigSettings.SmallWaitTime);

            var pendingPaymentsAfterSetup = _driver.FindElements(dashboard.pendingPaymentDatesTextLocBy)
                .Select(e => DateTime.Parse(e.Text, CultureInfo.InvariantCulture))
                .ToList();

            DateTime paymentDate = DateTime.ParseExact(dateSelected.ToString(), "MMMM d, yyyy", System.Globalization.CultureInfo.InvariantCulture);
            ReportingMethods.LogAssertionTrue(test, pendingPaymentsAfterSetup.Contains(paymentDate), $"Confirmed presence of the selected payment date ({paymentDate:MMMM d, yyyy}) in the pending payments list after setup");

            return dateSelected;
        }
        #endregion Services
    }
}