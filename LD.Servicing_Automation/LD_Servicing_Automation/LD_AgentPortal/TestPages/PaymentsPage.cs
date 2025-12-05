using System;
using OpenQA.Selenium;
using log4net;
using AventStack.ExtentReports;
using LD_AutomationFramework.Utilities;
using LD_AutomationFramework;
using LD_AutomationFramework.Pages;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using LD_AutomationFramework.Base;
using System.Linq;
using System.Reflection;
using LD_AutomationFramework.Config;
using static LD_AutomationFramework.Constants;


namespace LD_AgentPortal.Pages
{
    public class PaymentsPage
    {
        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;

        ReportLogger reportLogger { get; set; }



        public PaymentsPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            reportLogger = new ReportLogger(_driver);
        }

        #region Locators

        #region PaymentsTabLocators        

        #region PaymentSummaryLocators

        public By pastDuePaymentsLocBy = By.XPath("//div[contains(text(),'Past Due Payments')]//following-sibling::span");

        public By manageAutopayButtonLocBy = By.Id("btnManageAutopay");

        public By autopayStatusLocBy = By.XPath("//mello-serve-ui-automatic-payments//span[contains(@class,'rounded-circle')]//parent::h6");

        public By disabledManageAutopayButtonLocBy = By.XPath("//button[@id='btnManageAutopay' and @disabled='true']");

        #endregion PaymentSummaryLocators

        #region PendingPaymentsLocators

        public By makeAPaymentButtonLocBy = By.CssSelector("button[id*='btnMakePayment']");

        public By searchPaymentActivityButtonLocBy = By.CssSelector("button[id*='btnIdSearchShow']");

        public By makeAPaymentButtonDisabledLocBy = By.XPath("//button[contains(@id,'btnMakePayment') and @disabled='true']");

        public By makeAPaymentConfirmationPopupCancelButtonLocBy = By.Id("cancelBtn");

        public By makeAPaymentConfirmationPopupTextLocBy = By.CssSelector("mello-serve-ui-override-popup p");

        public By makeAPaymentConfirmationPopupConfirmButtonLocBy = By.Id("proceedBtn");

        #endregion PendingPaymentsLocators

        #endregion PaymentsTabLocators

        public By goBackToPaymentsLinkLocBy = By.Id("cancel-close-link");

        public By manageAutopayGridLocBy = By.CssSelector("mat-card[id]");

        public By accountDoesNotHaveAutopayMessageLocBy = By.XPath("//div[text()='Account does not have an Autopay setup']");

        public By setupAutopaybuttonLocBy = By.XPath("//span[contains(text(),'Setup Autopay')]//parent::button");

        public By unpaidPrincipalBalanceLocBy = By.Id("pIdPayUnpaid");

        public By authorizedByDropdownLocBy = By.XPath("//div[text()='Authorized By']//following-sibling::div//div[contains(@id,'mat-select')]");

        public string authorizedByDropdownValue = "//span[@class='mat-option-text']//span[contains(text(),'<BORROWERNAME>')]";

        public By authorizedByDropdownValueSelectedLocBy = By.CssSelector("mat-select[id='borrowerSelect'] span[class*='mat-select-min-line']");

        public By bankAccountDropdownLocBy = By.XPath("//mat-select[@id='bankAccountSelect']//div[contains(@id,'mat-select')]");

        public string bankAccountDropdownValue = "//span[@class='mat-option-text']//span[contains(text(),'<BANKACCOUNT>')]";

        public By allValuesInbankAccountDropdownLocBy = By.CssSelector("span[class='mat-option-text'] span");

        public By bankAccountDropdownValueSelectedLocBy = By.CssSelector("mat-select[id='bankAccountSelect'] span[class*='mat-select-min-line']");

        public string paymentDeletedTableRow = "//table[@id='tblIdPaymentDeleted']//tbody//tr[contains(.,'<CONFIRMATIONNUMBER>')]";

        public string deleteReasonTableRow = "//table[@id='tblIdPaymentDeleted']//tbody//tr[contains(.,'<CONFIRMATIONNUMBER>')]/following-sibling::tr//span[contains(@id,'delReasonChannelId')]";

        public By addAnAccountLinkLocBy = By.Id("add-account-link");

        public By accountNicknameLocBy = By.Id("accountNickNameInput");

        public By savingsRadioButtonLocBy = By.Id("SavingsButton");

        public By firstNameOnAccountTextboxLocBy = By.Id("firstNameInput");

        public By lastNameOnAccountTextboxLocBy = By.Id("lastNameInput");

        public By routingNumberTextboxLocBy = By.Id("routingNumberInput");

        public By accountNumberTextboxLocBy = By.Id("accountNumber");

        public By confirmAccountNumberTextboxLocBy = By.Id("accountNumberConfirm");

        public By addAccountButtonLocBy = By.Id("addAccountFormGroupBtn");

        public By paymentDatePickerIconLocBy = By.CssSelector("button[aria-label='Open calendar']");

        public By paymentDateTextboxWithDateSelectedLocBy = By.Id("paymtDateId");

        public string paymentDateTobeSelected = "td[aria-label='<DATETOBESELECTED>'] div[class*='mat-calendar-body-cell-content']";

        public By paymentAlreadyExistsAlertPopUpCloseIconLocBy = By.Id("btnClose");

        public By setupAutopayButtonInSetupAutopayScreenLocBy = By.XPath("//button//span[contains(text(),'Setup Autopay')]");

        public By confirmAutopayButtonLocBy = By.XPath("//span[contains(text(),'Confirm Autopay')]");

        public By continueButtonInConfirmAutopayScreenLocBy = By.Id("okBtn");

        public By activePaymentDateTobeSelected = By.CssSelector("td[class*='mat-calendar-body-active'] div[class*='mat-focus']");

        public By sectionRootLocBy = By.XPath("//*[contains(local-name(),'mello-serve-ui')]//*[.='Payment Summary']//parent::*");

        public string sectionTitleText = "//*[contains(local-name(),'mello-serve-ui')]//span[text()='<TITLE>']";

        public By notesImgLocBy = By.XPath("//img[@id='imgNotes']");

        public string buttonByText = "//button[contains(.,'<BUTTONNAME>')]";

        public By emptyPendingPaymentTableLocBy = By.XPath("//p[contains(text(),'There are currently no Pending Payments.')]");

        public By reasonForDeletionTextboxLocBy = By.XPath("//input[@matinput]");

        public By deleteButtonLocBy = By.Id("deleteBtn");

        public By deleteOkButtonLocBy = By.XPath("//span[text()='Ok']/ancestor::button");

        public By forbearancePopupTextLocBy = By.XPath("//p[contains(text(),'Active Forbearance Plan. Click')]");

        public By partialReInstatementRadioButtonLocBy = By.XPath("//mat-radio-button[@id='partialReinstatementRadioBtn']/label/span/input");

        public By matLabelAmountLocBy = By.XPath("//mat-label[contains(text(),'Amount (Monthly Payment:')]");

        public By amountMonthlyPaymentInputLocBy = By.Id("partialReinstatementInput");

        public By divTotalAmountLocBy = By.XPath("//div[text()='Total Payment']/following-sibling::div");

        public By paymentFormLocBy = By.Id("paymentForm");

        public By pastDueAmountRadioButtonLocBy = By.XPath("//span[contains(text(),'Past Due Amount')]/ancestor::mat-radio-button");

        public string labelPageHeaderTitleLocBy = "//form[@id='paymentForm']//span[text()='<TITLE>']";

        public string selectMatFormLabelNameLocBy = "//div[text()='<DROPDOWNLABEL>']/following-sibling::div//mat-form-field";

        public By otherBorrowerTextboxLocBy = By.XPath("//input[@id='otherBorrower']");

        public By lateFeesWarningTextLocBy = By.XPath("//p[contains(text(),'Any past due payments')]");

        public string paraByText = "//p[contains(text(),'<TEXT>')]";

        public string spanByText = "//span[text()='<TEXT>']";

        public string spanContainsByText = "//span[contains(text(),'<TEXT>')]";

        public string divByText = "//div[text()='<TEXT>']";

        public By disclosureLabelLocBy = By.XPath("//p[contains(text(),'I am required to read you a disclosure.')]/following-sibling::p");

        public By totalPaymentAmountTextLocBy = By.XPath("//mat-card[@id='cardIdPaymentReview']//p[@class='h4']");

        public By confirmationNumberTextLocBy = By.XPath("//div[text()='Confirmation Number']/following-sibling::div");

        public By paymentDateTextLocBy = By.XPath("//div[contains(text(),'Payment Date')]/following-sibling::div");

        public By tablePendingPaymentLocBy = By.Id("tblIdPaymentPending");

        public By pendingEditIconLocBy = By.XPath(".//img[@id='pendingEdit']/parent::div[contains(@class,'cursor-pointer')]");

        public By pendingDeleteIconLocBy = By.XPath(".//img[@id='pendingdelete']/parent::div[contains(@class,'cursor-pointer')]");

        public By tableRowLocBy = By.XPath(".//tbody//tr");

        public By noPendingPaymentsMessageLocBy = By.CssSelector("mello-serve-ui-payment-pending-details p[id='pIdNoPaymentAlert']");

        public By divDeleteMessageLocBy = By.XPath("//div[@class='my-3 ng-star-inserted']");

        public By spinnerLocBy = By.ClassName("spinner");

        public By tableDeletedPaymentLocBy = By.Id("tblIdPaymentDeleted");

        public By pastDueBannerLocBy = By.Id("PastDueAlertWhite");

        public By pastDueCurrentMonthDeleteDivLocBy = By.Id("pastDue");

        public By pastDueBannerTextLocBy = By.XPath("//span[contains(text(),'Past Due:')]");

        public By delinquentBannerTextLocBy = By.XPath("//span[contains(text(),'Delinquent:')]");

        public By paymentAlreadyExistsBannerLocBy = By.CssSelector("mat-error[id*='mat-error'] div[id*='paymentAlreadyExistsError']");

        public By monthlyPaymentPrepaidLocBy = By.CssSelector("div[id*='PaidOff']");

        public By yourLoanIsCurrentlyOnAnActiveTextLocBy = By.XPath("//span[contains(text(),'Your loan is currently on an Active')]");

        public By amountRadioButtonLocBy = By.XPath("//mat-radio-button[@id='partialReinstatementRadioBtn']");

        public By monthlyAmountTextboxLocBy = By.XPath("//input[@id='partialReinstatementInput']");

        public string radioButtonByText = "//mat-radio-button//label//*//span[contains(.,'<TEXT>')]/ancestor::mat-radio-button";

        public By overDuePastDueAmountSpanLocBy = By.XPath("//div[@id='pastdueAmountElmt']/mat-radio-group/span");

        public By additionalPrincipalTextboxLocBy = By.XPath("//input[@id='additionalPrincipalInput']");

        public By currentPaymentCheckboxLocBy = By.Id("currentPaymentCheckbox-input");

        public By currentPaymentCheckboxInputLocBy = By.XPath("//mat-checkbox[@id='currentPaymentCheckbox']/label/span/input");

        public By currentPaymentAmountLocBy = By.XPath("//mat-checkbox[@id='currentPaymentCheckbox']/../following-sibling::span");

        public By additionalPaymentCheckboxLocBy = By.Id("additionalPaymentCheckbox-input");

        public By overDueAddPrincipalDisabledTextboxLocBy = By.XPath("//input[@id='additionalPrincipalInput' and @disabled]");

        public By lateFeeAmountCheckboxLocBy = By.XPath("//mat-label[text()='Late Fee']/ancestor::mat-form-field/preceding-sibling::mat-checkbox//input");

        public By lateFeeAmountCheckboxCheckedLocBy = By.XPath("//mat-label[text()='Late Fee']/ancestor::mat-form-field/preceding-sibling::mat-checkbox//input[@aria-checked='true']");

        public By nSFFeeAmountCheckboxLocBy = By.XPath("//mat-label[text()='NSF Fee']/ancestor::mat-form-field/preceding-sibling::mat-checkbox//input");

        public By otherFeeAmountCheckboxLocBy = By.XPath("//mat-label[text()='Other Fees']/ancestor::mat-form-field/preceding-sibling::mat-checkbox//input");

        public By annualMaintenanceFeeAmountCheckboxLocBy = By.XPath("//mat-label[text()='Annual Maintenance Fee']/ancestor::mat-form-field/preceding-sibling::mat-checkbox//input");

        public By suspensePaymentCheckboxLocBy = By.CssSelector("mat-checkbox[id='suspenseBalanceCheckbox']");

        public By addToSuspenseBalanceTextboxLocBy = By.XPath("//input[@id='addSuspenseBalanceInput']");

        public By addToSuspenseBalanceDisabledTextboxLocBy = By.CssSelector("input[id = 'addSuspenseBalanceInput'][disabled]");

        public By addToSuspenceBalanceErrorMsgLocBy = By.Id("maxAddSuspenseBalanceErrorMesg");

        public By useSuspenseBalanceCheckboxLocBy = By.Id("useSuspenseBalanceCheckbox-input");

        public By useSuspenseBalanceDisabledCheckboxLocBy = By.CssSelector("input[id = 'useSuspenseBalanceCheckbox-input'][disabled]");

        public By useSuspenseBalanceAmountLocBy = By.XPath("//div[@id='useSuspenseBalanceBlock']//span[contains(@class,'float-right')]");

        public By lateFeeTextboxLocBy = By.XPath("//mat-label[text()='Late Fee']/ancestor::span/preceding-sibling::input[@id='feeInput']");

        public By nSFFeeTextboxLocBy = By.XPath("//mat-label[text()='NSF Fee']/ancestor::span/preceding-sibling::input[@id='feeInput']");

        public By otherFeeAmountTextboxLocBy = By.XPath("//mat-label[text()='Other Fees']/ancestor::span/preceding-sibling::input[@id='feeInput']");

        public By maxPrincipalErrorTextLocBy = By.Id("maxPrinciErrortext");

        public By totalPaymentGreaterUPBErrorTextLocBy = By.XPath("//ldsm-heloc-blue-banner-info-container[@type='payOffTotalAmount']");

        public By lateFeeTextboxHintLocBy = By.XPath("//mat-label[text()='Late Fee']/ancestor::mat-form-field//mat-hint");

        public By nSFFeeTextboxHintLocBy = By.XPath("//mat-label[text()='NSF Fee']/ancestor::mat-form-field//mat-hint");

        public By otherFeeAmountTextboxHintLocBy = By.XPath("//mat-label[text()='Other Fees']/ancestor::mat-form-field//mat-hint");

        public By otherFeesIconLocBy = By.XPath("//div[@id='otherFeeDescIcondiv']//img");

        public By otherFeesPopUpTextboxesLocBy = By.XPath("//input[contains(@id, 'otherFee')]");

        public By otherFeesPopUpTextboxHintLocBy = By.XPath("./ancestor::mat-form-field//mat-hint");

        public By otherFeesPopUpTextboxLabelLocBy = By.XPath("./ancestor::mat-form-field//mat-label");

        public By annualMaintenanceFeeTextboxLocBy = By.XPath("//mat-label[text()='Annual Maintenance Fee']/ancestor::span/preceding-sibling::input[@id='feeInput']");

        public By overDueAddEscrowDisabledTextboxLocBy = By.XPath("//input[@id='additionalEscrowInput'] and @disabled]");

        public By additionalEscrowTextboxLocBy = By.XPath("//input[@id='additionalEscrowInput']");

        public string divText = "//div[contains(text(),'<TEXT>')]";

        public By buttonYearMonthSelectorLocBy = By.XPath("//mat-calendar//button[@aria-label='Choose month and year']");

        public string buttonYear = "//mat-calendar//mat-multi-year-view//td[@aria-label='<YEAR>']";

        public string buttonMonth = "//mat-calendar//mat-year-view//td//div[contains(text(),'<MONTH>')]//parent::td";

        public string buttonDay = "//mat-calendar//mat-month-view//td//div[contains(text(),'<DAY>')]//parent::td";

        public By addPaymentCheckBoxLocBy = By.XPath("//mat-checkbox[@id='additionalPaymentCheckbox']/label/span/input");

        public By monthlyPaymentPastDueAmountSpanLocBy = By.XPath("//div[@id='amountCheckDiv']/div/div/following-sibling::span");

        public By monthlyPaymentPastDueAmountFieldTextLocBy = By.XPath("(//div[@id='amountCheckDiv']//*[@class and text()])[1]");

        public By pastDueAmountRadioButtonFieldTextLocBy = By.XPath("//div[@id='pastdueAmountElmt']//mat-radio-button");

        public By pastDueAmountTextLocBy = By.XPath("//div[@id='pastdueAmountElmt']//span[contains(@class,'semi-bold')]");

        public By delinquentDivLocBy = By.Id("pastDueDelinquent");

        public By pendingPaymentsDeleteImgLocBy = By.XPath("//img[@id='pendingdelete' and contains(@class,'ng-star-inserted')]");

        public By pendingPaymentsEditImgLocBy = By.XPath("//img[@id='pendingEdit' and contains(@class,'ng-star-inserted')]");

        public string pendingPaymentsEditImgDisabledLocBy = "//span[contains(text(),'<CONFIRMATIONNUMBER>')]//parent::td[contains(@class,'confirmationNumber')]//following-sibling::td//div//img[@id='pendingEdit' and not(contains(@class,'ng-star-inserted'))]";

        public By headerDetailsDivLocBy = By.XPath("//mello-serve-ui-loan-info-header//div[@class='row']");

        public By paymentSummaryPaymentBreakDownSectionLocBy = By.XPath("//h5[text()='Payment Breakdown']/following-sibling::div/div");

        public By paymentSummaryTaxesInsuranceLocBy = By.XPath("//span[@id='spidTaxInsurance']/parent::mat-card-title/parent::div/parent::mat-card-header/following-sibling::mat-card-content/div/div/div");

        public By closeTabDivLocBy = By.XPath("//div[contains(@class,'tab_notes_doc_btn') and contains(@class,'active')]");

        public By activeBankruptcySpanLocBy = By.XPath("//span[contains(text(),'Active Bankruptcy')]");

        public By disabledAuthorizedByMatSelectLocBy = By.XPath("//div[contains(text(),'Authorized By')]/following-sibling::div/mat-form-field/div/div/div/mat-select[@aria-disabled='true']");

        public By otpPaymentBannerLocBy = By.CssSelector("ldsm-otp-payment-payment-banner div");

        public By pastDueDivLocBy = By.Id("pastDue");

        public By nextPaymentInfoLocBy = By.XPath("//img[@mattooltipclass='next-payment-due-tooltip']");

        public By nextPaymentToolTipInfoLocBy = By.XPath("//div[contains(@class, 'next-payment-due-tooltip')]");

        public By prepaidpaymentHasBeeenMadeTextLocBy = By.XPath("//span[contains(text(),' Loan has been prepaid') or contains(text(),' Loan is prepaid') ]");

        public By monthlyPaymentPastDueCheckedAndDisabledLocBy = By.XPath("//input[@id='currentPaymentCheckbox-input' and @aria-checked='true' and @disabled]");

        public By monthlyPaymentCheckboxLocBy = By.Id("currentPaymentCheckbox-input");

        public By upcomingPaymentCheckboxLocBy = By.Id("upcomingPaymentCheckbox-input");

        public By monthlyPaymentAmountTextLocBy = By.XPath("//mat-checkbox[@id='currentPaymentCheckbox']/../following-sibling::span");

        public By feesSpanLocBy = By.XPath("//span[contains(text(),'FEES')]");

        public By feesSectorHiddenLocBy = By.XPath("//div[@id='FeeSector' and @hidden]");

        public By feesSectorNotHiddenLocBy = By.XPath("//div[@id='FeeSector' and not(@hidden)]");

        public By lateFeeAmountCheckboxDisabledCheckedLocBy = By.XPath("//mat-label[text()='Late Fee']/ancestor::ldsm-otp-payment-fee-checkbox-controller//mat-checkbox//input[@aria-checked='true' and @disabled]");

        public By nSFFeeAmountCheckboxDisabledCheckedLocBy = By.XPath("//mat-label[text()='NSF Fee']/ancestor::ldsm-otp-payment-fee-checkbox-controller//mat-checkbox//input[@aria-checked='true' and @disabled]");

        public By otherFeeAmountCheckboxDisabledCheckedLocBy = By.XPath("//mat-label[text()='Other Fees']/ancestor::ldsm-otp-payment-fee-checkbox-controller//mat-checkbox//input[@aria-checked='true' and @disabled]");

        public By annualMaintenanceFeeAmountCheckboxDisabledCheckedLocBy = By.XPath("//mat-label[text()='Annual Maintenance Fee']/ancestor::ldsm-otp-payment-fee-checkbox-controller//mat-checkbox//input[@aria-checked='true' and @disabled]");

        public By upcomingPaymentAmountTextLocBy = By.XPath("//mat-checkbox[@id='upcomingPaymentCheckbox']/../following-sibling::span");

        public By overDuePastDueAmountSpanTextLocBy = By.XPath("//div[@id='pastdueAmountElmt']/following-sibling::span");

        public By divPaymentsSubmitNoteLocBy = By.XPath("//div[contains(text(),' Payments must be submitted by')]");

        public By prePaymentCheckBoxLocBy = By.XPath("//mat-checkbox[@id='prePaymentCheckBox']//input");

        public By prePaymentCheckBoxAmountLocBy = By.XPath("//mat-checkbox[@id='prePaymentCheckBox']/parent::div/following-sibling::div/span");

        public string paymentBreakdownFeesByText = "//h5[text()='Payment Breakdown']/..//div[contains(.,'<FEES>')]/Span";

        public By softProcessStopsIndicatorMsgLocBy = By.Id("templateIdSoftProcessStopsIndicator");

        public By badCheckStopsIndicatorMsgLocBy = By.Id("templateIdBadCheckStopsIndicator");

        public By overridePopupMessageLocBy = By.XPath("//mello-serve-ui-override-popup//p");

        public By unpaidPrincipalBalanceAmountLocBy = By.XPath("//p[contains(text(),'Unpaid Principal Balance')]/following-sibling::p");

        public By emailChcekboxLocBy = By.XPath("//mat-checkbox[@id='emailSavedCheckbox']");

        public By inputEmailLocBy = By.XPath("//input[@id='Email']");

        public By methodDropdownLocBy = By.XPath("//mat-select[@formcontrolname='bankAccount']");

        public By methodDropDownOptionsLocBy = By.XPath("//mat-option//span/span");

        public string methodDropDownOptionText = "//mat-option//span[contains(text(),'<SELECTOPTIONTEXT>')]";

        public By bankAccountsTableEmptyTextLocBy = By.XPath("//table[@aria-label='Bank Accounts']/following-sibling::div");

        public By bankAccountsTableLocBy = By.XPath("//table[@aria-label='Bank Accounts']");

        public string bankAccountsTableColumnSelector = "//thead//th[contains(.,'<COLUMNNAME>')]/preceding-sibling::th";

        public string bankAccountsTableColumnElements = "//tbody//tr/td[<COLUMNNUMBER>]";

        #region RepaymentLocators

        public By activeRepaymentPlanAlertTextLocBy = By.XPath("//p[contains(text(),'Active Repayment Plan. Click')]");

        public By planBrokenOrDeletedAlertTextBy = By.XPath("//p[contains(text(),'Plan Broken or Deleted')]");

        public By repaymentPlanRadioButtonLocBy = By.XPath("//mat-radio-button[@id='repaymentPlanRadioBtn']/label/span/input");

        public By repaymentPlanRadioButtonIdLocBy = By.Id("repaymentPlanRadioBtn");

        public By pastDueRadioButtonLocBy = By.CssSelector("div[id='pastdueAmountElmt'] span[class*='mat-radio-outer-circle']");

        public By checkedAdditionalPaymentCheckboxLocBy = By.XPath("//mat-checkbox[@id='additionalPaymentsCheckBox']//input[@aria-checked='true']");

        public By additionalPaymentsCheckboxLocBy = By.XPath("//mat-checkbox[@id='additionalPaymentsCheckBox']//input");

        public By additionalPaymentTextLocBy = By.XPath("//input[@id='additionalPaymentsInput']");

        public By prepayCheckboxLocBy = By.XPath("//input[@id='prePaymentCheckBox-input']");

        public By lateFeeDateMsgDivLocBy = By.Id("latefeeInfoMsg1");

        public By divNextDueDateLocBy = By.XPath("//span[contains(text(), 'Next Due Date: ')]/parent::div/parent::div");

        public By nextDueBannerTextLocBy = By.XPath("//span[contains(text(),' Next Due Date:')]");

        public By paymentHasBeeenMadeTextLocBy = By.XPath("//span[contains(text(),'Payment has been made for')]");

        public By pastDueRadioButtonAmountTextLocBy = By.XPath("//div[@id='amountCheckDiv']//span[contains(@class,'semi-bold')] | //div[@id='pastdueAmountElmt']//span[contains(@class,'semi-bold')] | //div[@id='pastdueAmountElmt']/following-sibling::span[contains(@class,'semi-bold')]");

        public By pastDueAmountRadioButtonInputLocBy = By.CssSelector("mat-radio-button[id='pastDueAmountRadioBtn'] input");

        #endregion RepaymentLocators

        #endregion Locators

        #region Services

        /// <summary>
        /// Method to select borrower name in Authorized By dropdown
        /// </summary>
        /// <param name="borrowerName">Borrower name/ Co-Borrower name/ Other</param>
        /// <param name="isReportRequired">True/False</param>
        public void SelectValueInAuthorizedByDropdown(string borrowerName, bool isReportRequired = true)
        {
            bool flag = false;
            By authorizedByDropdownValueLocBy = null;
            try
            {
                webElementExtensions.WaitForElement(_driver, authorizedByDropdownLocBy);
                webElementExtensions.ScrollIntoView(_driver, authorizedByDropdownLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, authorizedByDropdownLocBy);
                authorizedByDropdownValueLocBy = By.XPath(authorizedByDropdownValue.Replace("<BORROWERNAME>", borrowerName.ToUpper()));
                webElementExtensions.ActionClick(_driver, authorizedByDropdownValueLocBy);
                webElementExtensions.WaitForElement(_driver, authorizedByDropdownValueSelectedLocBy);
                if (_driver.FindElement(authorizedByDropdownValueSelectedLocBy).Text == borrowerName.ToUpper())
                    flag = true;
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
            }
            catch (Exception ex)
            {
                log.Error("Failed while selecting borrower name in Authorized By dropdown: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully selected borrower name - " + borrowerName + " in Authorized By dropdown.", "Failed while selecting borrower name - " + borrowerName + " in Authorized By dropdown.");
        }

        /// <summary>
        /// Method to verify borrower name selected in Authorized By dropdown
        /// </summary>
        /// <param name="borrowerName">Borrower name/ Co-Borrower name/ Other</param>
        /// <param name="isReportRequired">True/False</param>
        public void VerifyValueSelectedInAuthorizedByDropdown(string borrowerName, bool isReportRequired = true)
        {
            bool flag = false;
            try
            {
                webElementExtensions.WaitForElement(_driver, authorizedByDropdownLocBy);
                webElementExtensions.ScrollIntoView(_driver, authorizedByDropdownLocBy);
                webElementExtensions.WaitForElement(_driver, authorizedByDropdownValueSelectedLocBy);
                if (webElementExtensions.GetElementText(_driver, authorizedByDropdownValueSelectedLocBy) == borrowerName.ToUpper())
                    flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying the borrower name in Authorized By dropdown: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully verified that the selected borrower name is '" + borrowerName + "' in Authorized By dropdown.", "The borrower name selected in Authorized By dropdown is not '" + borrowerName + "'.");
        }

        /// <summary>
        /// Method to select the payment date
        /// </summary>
        /// <param name="date">Date value in the following format - June 15, 2024</param>
        /// <param name="isReportRequired">true/false</param>
        public void SelectPaymentDateInDateField(string date, bool isReportRequired = true)
        {
            bool flag = false;
            By paymentDateToBeSelectedLocBy = null;
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, paymentDatePickerIconLocBy, ConfigSettings.LongWaitTime);
                webElementExtensions.ClickElementUsingJavascript(_driver, paymentDatePickerIconLocBy);
                paymentDateToBeSelectedLocBy = By.CssSelector(paymentDateTobeSelected.Replace("<DATETOBESELECTED>", date));
                webElementExtensions.WaitForVisibilityOfElement(_driver, paymentDateToBeSelectedLocBy, ConfigSettings.LongWaitTime);
                webElementExtensions.ClickElement(_driver, paymentDateToBeSelectedLocBy);
                webElementExtensions.WaitForElement(_driver, paymentDateTextboxWithDateSelectedLocBy, ConfigSettings.LongWaitTime);
                if (!_driver.FindElement(paymentDateTextboxWithDateSelectedLocBy).GetAttribute("class").Contains("untouched"))
                    flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while selecting payment date in Date field: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully selected payment date - " + date + " in Date field.", "Failed while selecting payment date - " + date + " in Date field.");
        }

        /// <summary>
        /// Method to select the payment date
        /// </summary>
        /// <param name="date">Date value in the following format - June 15, 2024</param>
        /// <param name="isReportRequired">true/false</param>
        /// <returns>Payment date selected</returns>
        public string SelectEnabledPaymentDateInDateField(bool isReportRequired = true)
        {
            bool flag = false;
            string date = "";
            try
            {
                webElementExtensions.ActionClick(_driver, paymentDatePickerIconLocBy);
                webElementExtensions.ClickElement(_driver, activePaymentDateTobeSelected);
                webElementExtensions.WaitForElement(_driver, paymentDatePickerIconLocBy);
                if (!_driver.FindElement(paymentDateTextboxWithDateSelectedLocBy).GetAttribute("class").Contains("untouched"))
                    flag = true;
                date = webElementExtensions.GetElementAttribute(_driver, paymentDateTextboxWithDateSelectedLocBy, "min");
            }
            catch (Exception ex)
            {
                log.Error("Failed while selecting payment date in Date field: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully selected payment date - " + date + " in Date field.", "Failed while selecting payment date - " + date + " in Date field.");

            return Convert.ToDateTime(date).ToString("MMMM dd, yyyy");
        }

        /// <summary>
        /// Method to verify page or section displayed or not
        /// </summary>
        /// <param name="title">Payment Details Page title/Loan Summary section/....</param>
        public void VerifyPageOrSectionDisplayed(string title)
        {
            try
            {
                By pageLocBy = By.XPath(sectionTitleText.Replace("<TITLE>", title));
                webElementExtensions.WaitForElement(_driver, pageLocBy, ConfigSettings.LongWaitTime);
                webElementExtensions.MoveToElement(_driver, pageLocBy);
                if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, pageLocBy))
                    _driver.Navigate().Refresh();
                webElementExtensions.WaitForElement(_driver, pageLocBy, ConfigSettings.LongWaitTime);
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, pageLocBy), "Verify page or section displayed: " + title + ".");
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying page or section details: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to verify Angular Payment Details page displayed
        /// </summary>
        /// <param name="title">Angular Payment Details Page title</param>
        public void VerifyAngularPaymentDetailsPageDisplayed(string title)
        {
            By pageLocBy = By.XPath(sectionTitleText.Replace("<TITLE>", title));
            webElementExtensions.WaitForElement(_driver, pageLocBy, ConfigSettings.LongWaitTime);
            webElementExtensions.MoveToElement(_driver, pageLocBy);
            if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, pageLocBy))
                _driver.Navigate().Refresh();
            webElementExtensions.WaitForElement(_driver, pageLocBy, ConfigSettings.LongWaitTime);
            ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, pageLocBy), "Verify angular payment details page displayed");
            reportLogger.TakeScreenshot(test, "Angular Payment Details page");
        }

        /// <summary>
        /// Method to verify Payment Summary Details page displayed
        /// </summary>
        /// <param name="loanLevelData">Loan Level Data from DB</param>
        /// <param name="verifyOnlyLoanInfoHeaderSectionDetails">true/false</param>
        public void VerifyPaymentSummaryDetailsOnPaymentPage(Hashtable loanLevelData, bool verifyOnlyLoanInfoHeaderSectionDetails = false)
        {
            var headerSection = GetHeaderDetails();

            ReportingMethods.LogAssertionEqual(test, loanLevelData[Constants.LoanLevelDataColumns.BorrowerFullName].ToString().ToUpper(), headerSection["AccountHolderName"].ToUpper(), "Verify AccountHolderName field value");
            ReportingMethods.LogAssertionEqual(test, loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString().Trim(), headerSection["LoanNumber"], "Verify LoanNumber field value");

            string Address = Regex.Replace(loanLevelData[Constants.LoanLevelDataColumns.PropertyAddress].ToString().ToUpper().Trim(), @"\s+", " ") + ", " + loanLevelData[Constants.LoanLevelDataColumns.PropertyCity].ToString().ToUpper().Trim() + ", " + loanLevelData[Constants.LoanLevelDataColumns.PropertyState].ToString().ToUpper().Trim() + " " + loanLevelData[Constants.LoanLevelDataColumns.PropertyZip].ToString().Trim();
            ReportingMethods.LogAssertionEqual(test, Address.Trim(), headerSection["Address"].Trim(), "Verify Address field value");

            string unpaidprinciple = "$" + Convert.ToDouble(headerSection["Unpaid Principal Balance"].Replace("$", "")).ToString("N").Trim();
            ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.FirstPrincipalBalance]).ToString("N").Trim()}", unpaidprinciple, "Verify Unpaid Principal Balance field value");
            ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment]).ToString("N").Trim()}", headerSection["Next Payment"], "Verify Next Payment field value");
            bool pendingPayments = commonServices.GetPendingPaymentDetails(loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString());
            webElementExtensions.WaitForElement(_driver, autopayStatusLocBy);
            bool autopayFlag = webElementExtensions.VerifyElementText(_driver, autopayStatusLocBy, "Off");
            if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 1 && 
                webElementExtensions.IsElementDisplayedBasedOnCount(_driver, emptyPendingPaymentTableLocBy) && autopayFlag)
            {
                ReportingMethods.LogAssertionEqual(test, Convert.ToDateTime((loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate])).ToString("MM/dd/yyyy"), headerSection["Confirm Payment Arrangements"], "Verify Payment Due field value");
            }
            else
            {
                ReportingMethods.LogAssertionEqual(test, Convert.ToDateTime((loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate])).ToString("MM/dd/yyyy"), headerSection["Next Payment Due"], "Verify Next Payment Due field value");
            }
            DateTime currentTime = DateTime.Now;
            DateTime currentESTTime = TimeZoneInfo.ConvertTime(currentTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZones.EasternStandardTime));

            if (!verifyOnlyLoanInfoHeaderSectionDetails)
            {
                var lastpaymentdueSection = GetSubSectionValues("LAST PAYMENT DUE");
                ReportingMethods.LogAssertionEqual(test, loanLevelData[Constants.LoanLevelDataColumns.LastFullPaymentDate].ToString() == "" ? "-" : Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).AddMonths(-1).ToString("MM/dd/yyyy"), lastpaymentdueSection["Last Payment Due"], "Verify Last Payment Due field value");
                ReportingMethods.LogAssertionEqual(test, loanLevelData[Constants.LoanLevelDataColumns.LastFullPaymentDate].ToString() == "" ? "-" : Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.LastFullPaymentDate]).ToString("MM/dd/yyyy"), lastpaymentdueSection["Last Payment Paid On"], "Verify Last Payment Paid On field value");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.LastFullPaymentAmount].ToString() == "" ? "0.00" : loanLevelData[Constants.LoanLevelDataColumns.LastFullPaymentAmount]).ToString("N")}", lastpaymentdueSection["Last Payment"], "Verify Last Payment field value");

                var accountStandingSection = GetSubSectionValues("ACCOUNT STANDING");
                var accountStanding = Regex.Replace(loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().ToLower(), @"\b\w", match => match.Value.ToUpper());
                if (accountStanding == "On-Time")
                {
                    ReportingMethods.LogAssertionEqual(test, accountStanding.ToLower(), accountStandingSection["Account Standing"].ToLower(), "Verify Account Standing field value");
                }
                else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString() == "P")
                {
                    ReportingMethods.Log(test, "The Account Standing field value :" + accountStandingSection["Account Standing"]);
                }
                else
                {
                    ReportingMethods.LogAssertionEqual(test, accountStanding, accountStandingSection["Account Standing"], "Verify Account Standing field value");
                }

                ReportingMethods.LogAssertionEqual(test, $"{loanLevelData[Constants.LoanLevelDataColumns.LateChargeGraceDay].ToString()} Days", accountStandingSection["Grace Days"], "Verify Last Grace Days field value");
                ReportingMethods.LogAssertionEqual(test, $"{loanLevelData[Constants.LoanLevelDataColumns.DaysPastDue].ToString()} Days".Trim(), accountStandingSection["Days Delinquent"].Trim(), "Verify Delinquent Days field value");

                var paymentBreakdownSection = GetPaymentBreakDownValue();
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.PrincipalInterestAmount]).ToString("N")}", paymentBreakdownSection["Principal & Interest"], "Verify Principal & Interest field value");
                if (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.MiscellaneousAmount]) > 0)
                {
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.MiscellaneousAmount]).ToString("N")}", paymentBreakdownSection["Other"], "Verify Other value");
                }
                ReportingMethods.LogAssertionEqual(test, $"${(Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.CountyTax]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.CityTax])).ToString("N")}", paymentBreakdownSection["Property Taxes"], "Verify Property Taxes field value");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.HazardAmount]).ToString("N")}", paymentBreakdownSection["Homeowners Insurance"], "Verify Homeowners Insurance field value");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.MiMonthlyAmount]).ToString("N")}", paymentBreakdownSection["Mortgage Insurance"], "Verify Mortgage Insurance field value");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.OverShortAmount]).ToString("N")}", paymentBreakdownSection["Overage/Shortage"], "Verify Overage/Shortage field value");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.LienAmount]).ToString("N")}", paymentBreakdownSection["Lien"], "Verify Lien field value");
                if (Math.Abs(Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.ReplacementReserveAmount])) > 0)
                {
                    ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(loanLevelData[Constants.LoanLevelDataColumns.ReplacementReserveAmount]), paymentBreakdownSection["Buydown/Subsidy"], "Verify Buydown/Subsidy field value");
                }
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment]).ToString("N")}", paymentBreakdownSection["Total Monthly Payment"], "Verify Total Monthly Payment field value");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.AccruedLateChargeAmount]).ToString("N")}", paymentBreakdownSection["Late Charges"], "Verify Late Charges field value");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance]).ToString("N")}", paymentBreakdownSection["NSF Fees"], "Verify NSF Fees field value");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.OtherFees].ToString() == "" ? "0.00" : loanLevelData[Constants.LoanLevelDataColumns.OtherFees]).ToString("N")}", paymentBreakdownSection["Other Fees"], "Verify Other Fees field value");

                double TotalDue = 0.0;
                loanLevelData[Constants.LoanLevelDataColumns.OtherFees] = loanLevelData[Constants.LoanLevelDataColumns.OtherFees].ToString() == "" ? 0.0 : loanLevelData[Constants.LoanLevelDataColumns.OtherFees];

                if (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.ReplacementReserveAmount]) > 0)
                {
                    ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(loanLevelData[Constants.LoanLevelDataColumns.ReplacementReserveAmount]), paymentBreakdownSection["Buydown/Subsidy"], "Verify Buydown/Subsidy field value");
                    TotalDue = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.ReplacementReserveAmount]);
                }

                if (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.RecoverableCorpAdvBalance]) < 0)
                {
                    ReportingMethods.LogAssertionFalse(test, paymentBreakdownSection.Keys.Contains("Recoverable Corp Adv"), "Verify Recoverable Corp Adv field value not displayed when it's value < 0 ");
                    TotalDue = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.AccruedLateChargeAmount]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.OtherFees]);
                    if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) > 1)
                    {
                        TotalDue = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentBalance]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.AccruedLateChargeAmount]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.OtherFees]);
                    }
                }
                else
                {
                    ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(loanLevelData[Constants.LoanLevelDataColumns.RecoverableCorpAdvBalance]), paymentBreakdownSection["Recoverable Corp Adv"], "Verify Recoverable Corp Adv field value");

                    TotalDue = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.AccruedLateChargeAmount]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.OtherFees]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.RecoverableCorpAdvBalance]);

                    if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) > 1)
                    {
                        TotalDue = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentBalance]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.AccruedLateChargeAmount]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.OtherFees]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.RecoverableCorpAdvBalance]);
                    }
                }

                ReportingMethods.LogAssertionEqual(test, $"${TotalDue.ToString("N")}", paymentBreakdownSection["Total Due"], "Verify Total Due field value");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.SuspenseBalance]).ToString("N")}", paymentBreakdownSection["Suspense"], "Verify Suspense field value");
                ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount((Convert.ToDouble(TotalDue) - Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.SuspenseBalance]))), paymentBreakdownSection["Net Due"], "Verify Net Due field value");

                var taxesInsuranceSection = GetTaxesInsurance();
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.YtdTaxAmount]).ToString("N")}", "$" + Convert.ToDouble(taxesInsuranceSection["Taxes Paid YTD"].Replace("$", "")).ToString("N").Trim(), "Verify Taxes Paid YTD field value");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.YtdInsurancePaidAmount]).ToString("N")}", "$" + Convert.ToDouble(taxesInsuranceSection["Insurance Paid YTD"].Replace("$", "")).ToString("N").Trim(), "Verify Insurance Paid YTD field value");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.YtdInterestPaidAmount]).ToString("N")}", "$" + Convert.ToDouble(taxesInsuranceSection["Interest Paid YTD"].Replace("$", "")).ToString("N").Trim(), "Verify Interested Paid YTD field value");
            }
        }

        /// <summary>
        /// Method to get Angular page Header Details
        /// </summary>
        /// <returns>Angular page Header Details</returns>
        public Dictionary<string, string> GetHeaderDetails(bool isReportRequired = false)
        {
            bool flag = false;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
                webElementExtensions.ScrollIntoView(_driver, headerDetailsDivLocBy);
                var element = _driver.FindElement(headerDetailsDivLocBy);
                dic.Add("AccountHolderName", element.FindElement(By.XPath("./div[1]/div[1]")).Text.Trim());
                var elementtext = element.FindElement(By.XPath("./div[1]/div[2]")).Text.Trim();
                string[] elementArray = elementtext.Split('-');
                var loannumber = elementArray[0].Replace("Loan", "").Trim();
                var address = elementArray[1].Trim();
                dic.Add("LoanNumber", loannumber);
                dic.Add("Address", address);

                var elements = element.FindElements(By.XPath($"./div[2]/*"));
                int index = 1;

                foreach (var elem in elements)
                {
                    if (index < elements.Count)
                    {
                        var key = elem.FindElement(By.XPath("./div[2]")).Text.Trim();
                        var value = elem.FindElement(By.XPath("./div[1]")).Text.Trim();
                        dic.Add(key, value);
                        index = index + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting angular page header details: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully get angular page header details.", "Failed while getting angular page header details.");
            return dic;
        }

        /// <summary>
        /// Method to get Angular page Sub section Details
        /// </summary>
        /// <returns>Angular page Sub section Details</returns>
        public Dictionary<string, string> GetSubSectionValues(string subSection, bool isReportRequired = false)
        {

            bool flag = false;
            var dic = new Dictionary<string, string>();

            try
            {
                var sval = subSection.ToUpper();
                var elems = _driver.FindElements(By.XPath($"//h6[text()='{sval}']//following-sibling::div//div//div"));
                foreach (var elem in elems)
                {
                    var key = elem.Text.Trim();
                    var value = elem.FindElement(By.XPath("./following-sibling::h6")).Text.Trim();
                    dic.Add(key, value);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Sub section Details for " + subSection + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Sub section Details " + subSection, "Failed while getting Sub section Details for " + subSection);

            return dic;
        }

        /// <summary>
        /// Method to get OTP page Sub section Details
        /// </summary>
        /// <returns>OTP page Sub section Details</returns>
        public Dictionary<string, string> GetOTPSubSectionValues(string section, bool isReportRequired = false)
        {
            bool flag = false;
            var dic = new Dictionary<string, string>();
            try
            {
                if (_driver.FindElements(By.XPath($"//mat-card[contains(.,'{section}')]")).Count > 0)
                {
                    var sectionTotal = _driver.FindElement(By.XPath($"//mat-card[contains(.,'{section}')]"));
                    var sectionText = sectionTotal.FindElement(By.XPath($"./p[1]"));
                    var sectionValue = sectionTotal.FindElement(By.XPath($"./p[2]"));
                    dic.Add(sectionText.Text.Trim(), sectionValue.Text.Trim());

                    var elements = sectionTotal.FindElements(By.XPath($"./div"));
                    int index = 1;
                    foreach (var element in elements)
                    {
                        var key = element.FindElement(By.XPath($"./div[1]")).Text.Trim();
                        var value = element.FindElement(By.XPath($"./div[2]")).Text.Trim();
                        dic.Add(key, value);
                        index = index + 1;
                    }
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Sub section Details for " + section + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Sub section Details " + section, "Failed while getting Sub section Details for " + section);
            return dic;
        }

        /// <summary>
        /// Method to get OTP page Payment Breakdown Details
        /// </summary>
        /// <returns>Payment Breakdown Details</returns>
        public Dictionary<string, string> GetPaymentBreakDownValue(bool isReportRequired = false)
        {
            bool flag = false;
            var dic = new Dictionary<string, string>();

            try
            {
                WebElement thisElement = (WebElement)_driver.FindElement(sectionRootLocBy);
                webElementExtensions.ScrollIntoView(_driver, thisElement);
                var elems = thisElement.FindElements(paymentSummaryPaymentBreakDownSectionLocBy);
                foreach (var elem in elems)
                {
                    string[] elemText = Regex.Split(elem.Text, "\r\n");
                    var key = elemText[0].Trim();
                    if (key.Equals("ESCROW"))
                    {
                        foreach (var ele in elem.FindElements(By.XPath("./div[not(position()=1)]")))
                        {
                            string[] text = Regex.Split(ele.Text, "\r\n");
                            string val = text[1].Trim();
                            dic.Add(text[0].Trim(), text[1].Trim());
                        }
                    }
                    var value = "";
                    try
                    {
                        value = elem.FindElement(By.XPath("./span")).Text.Trim();

                    }
                    catch
                    {
                        value = "";
                    }
                    dic.Add(key, value);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Sub section Details for Payment Breakdown details " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Payment Breakdown details.", "Failed while getting Payment Breakdown details.");
            return dic;
        }

        /// <summary>
        /// Method to get OTP page Tax Insurance Details
        /// </summary>
        /// <returns>Tax Insurance Details</returns>
        public Dictionary<string, string> GetTaxesInsurance(bool isReportRequired = false)
        {
            bool flag = false;
            var dic = new Dictionary<string, string>();
            try
            {
                WebElement thisElement = (WebElement)_driver.FindElement(sectionRootLocBy);
                var elems = thisElement.FindElements(paymentSummaryTaxesInsuranceLocBy);
                foreach (var elem in elems)
                {
                    string[] elemText = Regex.Split(elem.Text, "\r\n");
                    var key = elemText[0].Trim();
                    var value = elem.FindElement(By.XPath("./following-sibling::h6")).Text.Trim();
                    dic.Add(key, value);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Tax Insurance details " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Tax Insurance details.", "Failed while getting Tax Insurance details.");
            return dic;
        }

        /// <summary>
        /// Method to Close Notes Tab
        /// </summary>
        public void CloseNotesTab()
        {
            webElementExtensions.ScrollIntoView(_driver, notesImgLocBy);
            if (_driver.FindElements(closeTabDivLocBy).Count > 0)
            {
                webElementExtensions.ActionClick(_driver, notesImgLocBy);
                ReportingMethods.Log(test, "Successfully Notes image button is clicked");
            }
        }

        /// <summary>
        /// Method to Delete All Existing Pending Payments
        /// </summary>
        /// <param name="reason">Delete Reason</param>
        public void DeleteAllExistingPendingPayments(string reason, bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                webElementExtensions.ScrollIntoView(_driver, By.XPath(buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)));
                if (_driver.FindElements(emptyPendingPaymentTableLocBy).Count == 0)
                {
                    webElementExtensions.ScrollIntoView(_driver, tablePendingPaymentLocBy);
                    var rows = _driver.FindElement(tablePendingPaymentLocBy).FindElements(tableRowLocBy);
                    foreach (var row in rows)
                    {
                        if (row.FindElements(pendingPaymentsDeleteImgLocBy).Count > 0 && row.Text.Contains(Constants.PortalNames.AgentPortal))
                        {
                            row.FindElement(pendingPaymentsDeleteImgLocBy).Click();
                            webElementExtensions.WaitForElement(_driver, reasonForDeletionTextboxLocBy);
                            webElementExtensions.EnterText(_driver, reasonForDeletionTextboxLocBy, reason);
                            webElementExtensions.WaitForElement(_driver, deleteButtonLocBy);
                            webElementExtensions.ActionClick(_driver, deleteButtonLocBy);
                            webElementExtensions.WaitForElement(_driver, deleteOkButtonLocBy);
                            webElementExtensions.ClickElementUsingJavascript(_driver, deleteOkButtonLocBy);
                        }

                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while deleting all existing Pending Payments " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got deleting all existing Pending Payments.", "Failed while getting  deleting all existing Pending Payments.");
        }

        /// <summary>
        /// Method to click Button by using Button Name
        /// </summary>
        /// <param name="buttonName">Button Name</param>
        /// <param name="message">Pass this when clicking on Alert buttons</param>
        public void ClickButtonUsingName(string buttonName, string message = "", bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                By btnLocBy = By.XPath(buttonByText.Replace("<BUTTONNAME>", buttonName));
                webElementExtensions.WaitUntilElementIsClickable(_driver, btnLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, btnLocBy, message == "" ? buttonName : message);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while clicking button named " + buttonName + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully clicked button named " + buttonName, "Failed while clicking button named " + buttonName);
        }

        /// <summary>
        /// Method to check Active Forbearance Plan Alert Displayed
        /// </summary>
        public void IsActiveForbearancePlanAlertDisplayed(bool isReportRequired = false)
        {

            bool flag = false;

            try
            {
                webElementExtensions.WaitForVisibilityOfElement(_driver, forbearancePopupTextLocBy);
                webElementExtensions.IsElementDisplayed(_driver, forbearancePopupTextLocBy, "Active Forbearance plan Alert");
                reportLogger.TakeScreenshot(test, "Active Forbearance plan Alert");
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while checking Active Forbearance Plan Alert Displayed" + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully checked Active Forbearance Plan Alert Displayed", "Failed while checking Active Forbearance Plan Alert Displayed");
        }

        /// <summary>
        /// Method to Verify Payment Breakdown Section Not Visible
        /// </summary>
        /// <param name="type">Payment flow - setup/Edit</param>
        public void VerifyPaymentBreakdownSectionNotVisible(string type = "setup")
        {
            var paymentbreakdownSection = GetSubSectionValues("Payment Breakdown");
            ReportingMethods.LogAssertionTrue(test, 0 == paymentbreakdownSection.Count, type == "setup" ? "Verify Payment Breakdown section is hidden" : "Verify " + type + " Payment Breakdown section is hidden");
        }

        /// <summary>
        /// Method to Verify OTP Screen Details
        /// </summary>
        /// <param name="loanLevelData">Loan level details from DB</param>
        /// <param name="planType">Active/Broken/Deleted</param>
        /// <param name="type">Payment flow - setup/Edit</param>
        public void VerifyOTPScreenDetails(Hashtable loanLevelData, string planType, string type = "setup")
        {

            var unPaidPrincipalBalanceSection = GetOTPSubSectionValues("Unpaid Principal Balance");
            ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.FirstPrincipalBalance]).ToString("N")}", unPaidPrincipalBalanceSection["Unpaid Principal Balance"], type == "setup" ? "Verify Unpaid Principal Balance section field value in OTP Screen" : "Verify " + type + " Unpaid Principal Balance section field value in OTP Screen");
            ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.YtdPrincipalPaidAmount]).ToString("N")}", unPaidPrincipalBalanceSection["Principal Paid"], type == "setup" ? "Verify Principal Paid field value in OTP Screen" : "Verify " + type + " Principal Paid field value in OTP Screen");
            ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.EscrowBalance]) - Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.EscrowAdvanceBalance])), unPaidPrincipalBalanceSection["Escrow Balance"], type == "setup" ? "Verify Escrow Balance field value in OTP Screen" : "Verify " + type + " Escrow Balance field value in OTP Screen");
            if (planType != "Active")
            {
                var paymentBreakDownSection = GetOTPSubSectionValues("Payment Breakdown");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment]).ToString("N")}", paymentBreakDownSection["Payment Breakdown"], type == "setup" ? "Verify Payment Breakdown section value in OTP Screen" : "Verify " + type + " Payment Breakdown section value in OTP Screen");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.PrincipalInterestAmount]).ToString("N")}", paymentBreakDownSection["Principal & Interest"], type == "setup" ? "Verify Principal & Interest field value in OTP Screen" : "Verify " + type + " Principal & Interest field value in OTP Screen");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TaxAndInsuranceAmount]).ToString("N")}", paymentBreakDownSection["Taxes &/or Insurance"], type == "setup" ? "Verify Taxes &/or Insurance field value in OTP Screen" : "Verify " + type + " Taxes &/or Insurance field value in OTP Screen");
                if (Math.Abs(Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.ReplacementReserveAmount])) > 0)
                {
                    ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(loanLevelData[Constants.LoanLevelDataColumns.ReplacementReserveAmount]), paymentBreakDownSection["Buydown/Subsidy"], type == "setup" ? "Verify Buydown/Subsidy field value in OTP Screen" : "Verify " + type + " Buydown/Subsidy field value in OTP Screen");
                }
            }
        }

        /// <summary>
        /// Method to  Verify Forbearance Plan Banner Details
        /// </summary>
        /// <param name="loanLevelData">Loan level details from DB</param>
        /// <param name="planType">Active/Broken/Deleted</param>
        /// <param name="type">Payment flow - setup</param>
        public void VerifyForbearancePlanBannerDetails(Hashtable loanLevelData, string planType, string type = "setup")
        {
            var difference = Convert.ToDateTime(DateTime.Now).Subtract(Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]));
            if (Convert.ToInt16(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) > 1)//Delinquent
            {
                VerifyBannerDelinquentDateDetails(loanLevelData, planType, type);
            }
            else
            {
                VerifyBannerPastDueDateDetails(loanLevelData, planType, type);
            }
        }

        /// <summary>
        /// Method to  Verify Banner Past Due Date Details
        /// </summary>
        /// <param name="loanLevelData">Loan level details from DB</param>
        /// <param name="planType">Active/Broken/Deleted</param>
        /// <param name="type">Payment flow - setup</param>
        public void VerifyBannerPastDueDateDetails(Hashtable loanLevelData, string planType = "", string expectedActiveBannerMessage = "", string type = "setup")
        {
            var pastDueDate = _driver.FindElement(pastDueBannerTextLocBy).Text.Trim().Replace("Past Due:", "").Trim();
            ReportingMethods.LogAssertionEqual(test, Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).ToString("MMM d, yyyy"), pastDueDate, type == "setup" ? "Verify Past Due in OTP Screen" : "Verify " + type + " Past Due in OTP Screen");
            if (planType == "Active")
            {
                var activeText = _driver.FindElement(yourLoanIsCurrentlyOnAnActiveTextLocBy).Text.Trim();
                ReportingMethods.LogAssertionContains(test, expectedActiveBannerMessage.Trim(), activeText.Trim(), type == "setup" ? "Verify OTP screen Active Plan Past Due Date details" : "Verify " + type + " OTP screen Active Plan Past Due Date details");
            }
        }

        /// <summary>
        /// Retrieves promise details for a given loan number.
        /// </summary>
        /// <param name="loanNumber">The loan number for which to retrieve promise details.</param>
        /// <param name="isReportRequired">Flag indicating whether a report is required.</param>
        /// <returns>A Hashtable containing the promise details.</returns>
        public Hashtable GetPromiseDetails(string loanNumber, bool isReportRequired = false)
        {
            bool flag = false;
            Hashtable hashData = new Hashtable();

            try
            {
                List<string> columnDataRequired = typeof(Constants.PromiseDataColumns).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(field => field.FieldType == typeof(string)).Select(field => (string)field.GetValue(null)).ToList();
                string query = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetActiveRepaymentPlanPromiseDetails));
                query = query.Replace("#", loanNumber);
                hashData = commonServices.ExecuteQueryAndGetDataFromDataBase(query, null, columnDataRequired).FirstOrDefault();
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting promise details" + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Promise details", "Failed while getting promise details");

            return hashData;
        }

        /// <summary>
        /// Method to  Verify  Plan Delinquent Details
        /// </summary>
        /// <param name="loanLevelData">Loan level details from DB</param>
        /// <param name="planType">Active/Broken/Deleted</param>
        /// <param name="type">Payment flow - setup</param>
        public void VerifyBannerDelinquentDateDetails(Hashtable loanLevelData, string planType, string type = "setup")
        {
            var delinquentduedate = _driver.FindElement(delinquentBannerTextLocBy).Text.Trim().Replace("Delinquent:", "").Trim();
            ReportingMethods.LogAssertionEqual(test, Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).ToString("MMM d, yyyy"), delinquentduedate, type == "setup" ? "Verify " + planType + " Delinquent Due in OTP Screen" : "Verify " + planType + " " + type + " Delinquent Due in OTP Screen");
        }

        /// <summary>
        /// Method to  Verify Banner Prepaid Details
        /// </summary>
        /// <param name="loanLevelData">Loan level details from DB</param>
        /// <param name="planType">Active/Broken/Deleted</param>
        /// <param name="type">Payment flow - setup</param>
        public void VerifyBannerPrepaidDateDetails(Hashtable loanLevelData, string planType, string type = "setup")
        {
            string nextDueDate = _driver.FindElement(nextDueBannerTextLocBy).Text.Trim();
            nextDueDate = nextDueDate.Replace("Next Due Date:", "").Trim();
            ReportingMethods.LogAssertionEqual(test, Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).ToString("MMM d, yyyy"), nextDueDate, type == "setup" ? "Verify " + planType + " Next Due in OTP Screen" : "Verify " + planType + " " + type + " Next Due in OTP Screen");
            var prepaidText = _driver.FindElement(prepaidpaymentHasBeeenMadeTextLocBy).Text.Trim();
            string Month = Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).AddMonths(-1).ToString("MMMM");
            int nextPaymentDueMonth = Convert.ToInt16(Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).ToString("MM"));
            int currentMonth = Convert.ToInt16(DateTime.Now.ToString("MM"));
            if (nextPaymentDueMonth > (currentMonth + 2))
            {
                ReportingMethods.LogAssertionContains(test, "Loan is prepaid through " + Month + " and only eligible for Additional Principal and Escrow Payments.", prepaidText.Trim(), type == "setup" ? "Verify OTP screen " + planType + "  Plan Payment has been made Month details" : "Verify " + planType + " " + type + " OTP screen Prepaid plan has been made Month details");
            }
            else
            {
                 ReportingMethods.LogAssertionContains(test, "Loan has been prepaid through " + Month + ".", prepaidText.Trim(), type == "setup" ? "Verify OTP screen " + planType + "  Plan Payment has been made Month details" : "Verify " + planType + " " + type + " OTP screen Prepaid plan has been made Month details");
            }
        }

        /// <summary>
        /// Method to Get Amount Monthly Payment Value
        /// </summary>
        /// <returns>Amount Monthly Payment Value Details</returns>
        public string GetAmountMonthlyPaymentValue(bool isReportRequired = false)
        {
            bool flag = false;
            string returnValue = "";
            try
            {
                var paymentAmount = _driver.FindElement(matLabelAmountLocBy).Text.Trim().Split(':');
                string amountMonthlyPayment = paymentAmount[1].Replace(")", "").Trim();
                returnValue = amountMonthlyPayment.ToString();
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Amount Monthly Payment Value" + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got  Amount Monthly Payment Value", "Failed while getting  Amount Monthly Payment Value");
            return returnValue;
        }

        /// <summary>
        /// Method to Enter Amount Monthly Payment Value
        /// </summary>
        /// <param name="amount">Amount details</param>
        /// <returns>Amount</returns>
        public string EnterAmountMonthlyPaymentValue(double amount)
        {
            webElementExtensions.EnterText(_driver, amountMonthlyPaymentInputLocBy, amount.ToString());
            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
            string amountEntered = commonServices.GetValueUsingJS(amountMonthlyPaymentInputLocBy);
            ReportingMethods.Log(test, "Successfully Amount Monthly Payment is entered " + amountEntered);
            return GetAmountText(divTotalAmountLocBy);
        }

        /// <summary>
        /// Method to get the Element Amount Text
        /// </summary>
        /// <param name="locator">Elment Locator - By Id, Css, Xpath</param>
        /// <returns>Amount Text Details</returns>
        public string GetAmountText(By locator, bool isReportRequired = false)
        {
            bool flag = false;
            string returnValue = "";
            try
            {
                double amount = Convert.ToDouble(webElementExtensions.GetElementText(_driver, locator).Replace("$", ""));
                returnValue = "$" + amount.ToString("N");
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Element Amount Text " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Element Amount Text.", "Failed while getting  Element Amount Text.");

            return returnValue;
        }

        /// <summary>
        /// Method to Verify Review And Confirmation Page Is Displayed
        /// </summary>
        /// <param name="type">Payment flow - setup/Edit</param>
        public void VerifyReviewAndConfirmationPageIsDisplayed(string type = "setup")
        {
            By locBy = By.XPath(paraByText.Replace("<TEXT>", "Does everything look okay?"));
            webElementExtensions.WaitForElement(_driver, locBy);
            ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, locBy), type == "setup" ? "Verify payment review page loaded" : "Verify " + type + " payment review page loaded");
            reportLogger.TakeScreenshot(test, "Review And Confirmation Page");
        }

        /// <summary>
        /// Method to Verify Total Amount In Payment Page
        /// </summary>
        /// <param name="totalPayment">Total Payment Amount</param>
        /// <param name="type">Payment flow - setup/Edit</param>
        public void VerifyTotalAmountInPaymentPage(string totalPayment, string type = "setup")
        {
            webElementExtensions.ActionClick(_driver, divTotalAmountLocBy);
            double actualTotalPayment = Convert.ToDouble(webElementExtensions.GetElementText(_driver, divTotalAmountLocBy).Replace("$", ""));
            ReportingMethods.LogAssertionEqual(test, totalPayment, "$" + actualTotalPayment.ToString("N"), type == "setup" ? "Verify Total Amount in payment page" : "Verify " + type + " Total Amount in payment page");
        }

        /// <summary>
        /// Method to Verify New Payment Added
        /// </summary>
        /// <param name="columnName">Pending Payments Table column name</param>
        /// <param name="text">Pending Payments Table cell text - like Confirmation Number</param>
        public void VerifyNewPaymentAdded(string columnName, string text)
        {
            webElementExtensions.WaitForElement(_driver, tablePendingPaymentLocBy);
            var rows = _driver.FindElement(tablePendingPaymentLocBy).FindElements(tableRowLocBy);
            int columnNum = _driver.FindElement(tablePendingPaymentLocBy).FindElements(By.XPath(".//thead//th[contains(.,'" + columnName + "')]/preceding-sibling::th")).Count + 1;

            foreach (var row in rows)
            {
                var rowText = row.Text;
                if (rowText.Contains(text))
                {
                    var value = row.FindElement(By.XPath("./td[" + columnNum + "]")).Text;
                    ReportingMethods.LogAssertionContains(test, text, value, "Verify New Payment added");
                    break;
                }
            }

        }

        /// <summary>
        /// Method to verify if pending payment already exists or not in the Pending Payments table
        /// </summary>
        /// <param name="columnName">Pending Payments Table column name</param>
        /// <param name="searchText">Pending Payments Table cell text - like Confirmation Number</param>
        /// <param name="isReportingRequired">true/false</param>
        public bool VerifyIfPendingPaymentAlreadyExists(string columnName, string searchText, bool isReportingRequired = false)
        {
            bool flag = false;
            var value = "";
            try
            {
                if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, noPendingPaymentsMessageLocBy))
                {
                    webElementExtensions.WaitForElement(_driver, tablePendingPaymentLocBy);
                    var rows = _driver.FindElement(tablePendingPaymentLocBy).FindElements(By.XPath(".//tbody//tr"));
                    int columnNum = _driver.FindElement(tablePendingPaymentLocBy).FindElements(By.XPath(".//thead//th[contains(.,'" + columnName + "')]/preceding-sibling::th")).Count + 1;

                    foreach (var row in rows)
                    {
                        var rowText = row.Text;
                        if (rowText.Contains(searchText))
                        {
                            value = row.FindElement(By.XPath("./td[" + columnNum + "]")).Text;
                            flag = true;
                            break;
                        }
                    }
                }
                else
                    flag = false;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying pending payment exists or not.");
            }
            if (isReportingRequired)
                _driver.ReportResult(test, flag, "Successfully verified that already pending payment exists for the given criteria and value: " + columnName + "- " + searchText + ".", "Failure - Pending payment do not exist for the given criteria and value: " + columnName + "- " + searchText + ".");
            return flag;
        }

        /// <summary>
        /// Method to Delete Newly Added Payment
        /// </summary>
        /// <param name="confirmationNumber">Confirmation Number</param>
        /// <param name="reason">Delete Reason</param>
        public void DeleteNewlyAddedPayment(string confirmationNumber, string reason)
        {
            var rows = _driver.FindElement(tablePendingPaymentLocBy).FindElements(By.XPath(".//tbody//tr"));
            bool flag = false;
            foreach (var row in rows)
            {
                webElementExtensions.ScrollIntoView(_driver, row);
                var rowText = row.Text;
                if (rowText.Contains(confirmationNumber))
                {
                    row.FindElement(pendingPaymentsDeleteImgLocBy).Click();
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    webElementExtensions.WaitForElement(_driver, reasonForDeletionTextboxLocBy);
                    webElementExtensions.EnterText(_driver, reasonForDeletionTextboxLocBy, reason);
                    webElementExtensions.WaitForElement(_driver, deleteButtonLocBy);
                    webElementExtensions.ClickElement(_driver, deleteButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    flag = true;
                    break;
                }
            }
            ReportingMethods.LogAssertionTrue(test, flag, "Verify New Payment deleted successfully");
        }

        /// <summary>
        /// Method to Verify Payment Deletion Message Is Displayed
        /// </summary>
        public void VerifyPaymentDeletionMessageIsDisplayed()
        {
            webElementExtensions.WaitForInvisibilityOfElement(_driver, spinnerLocBy, ConfigSettings.LongWaitTime);
            webElementExtensions.WaitForElement(_driver, deleteOkButtonLocBy, ConfigSettings.LongWaitTime);
            webElementExtensions.WaitForElement(_driver, divDeleteMessageLocBy, ConfigSettings.LongWaitTime);
            ReportingMethods.LogAssertionEqual(test, "Payment deletion successful.", _driver.FindElement(divDeleteMessageLocBy).Text, "Verify Payment deletion message displayed");
            webElementExtensions.ClickElement(_driver, deleteOkButtonLocBy);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
        }

        /// <summary>
        /// Method to Verify Newly Deleted Payment Not In Pending Payments Table
        /// </summary>
        /// <param name="confirmationNumber">Confirmation Number</param>
        public void VerifyNewlyDeletedPaymentNotInPendingPaymentsTable(string confirmationNumber)
        {
            bool flag;
            if (_driver.FindElements(emptyPendingPaymentTableLocBy).Count > 0)
            {
                flag = true;
            }
            else
            {
                webElementExtensions.WaitForElement(_driver, tablePendingPaymentLocBy);
                webElementExtensions.ScrollIntoView(_driver, tablePendingPaymentLocBy);
                var rows = _driver.FindElement(tablePendingPaymentLocBy).FindElements(By.XPath(".//tbody//tr"));
                flag = true;
                foreach (var row in rows)
                {
                    webElementExtensions.ScrollIntoView(_driver, row);
                    string rowText = row.Text;
                    if (rowText.Contains(confirmationNumber))
                    {
                        flag = false;
                        break;
                    }
                }
            }
            ReportingMethods.LogAssertionTrue(test, flag, "Verify Newly Deleted Payment not in Pending Payments table with Confirmation Number: " + confirmationNumber);
        }

        /// <summary>
        /// Method to Verify Newly Deleted Payment Added In Deleted Payments Table
        /// </summary>
        /// <param name="confirmationNumber">Confirmation Number</param>
        public void VerifyNewlyDeletedPaymentAddedInDeletedPaymentsTable(string confirmationNumber)
        {
            _driver.Navigate().Refresh();
            webElementExtensions.WaitForElement(_driver, tableDeletedPaymentLocBy, ConfigSettings.LoginWaitTime);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
            webElementExtensions.ScrollIntoView(_driver, tableDeletedPaymentLocBy);
            var rows = _driver.FindElement(tableDeletedPaymentLocBy).FindElements(By.XPath(".//tbody//tr"));
            bool flag = false;
            foreach (var row in rows)
            {
                var rowText = row.Text;
                if (rowText.Contains(confirmationNumber))
                {
                    flag = true;
                    break;
                }
            }
            ReportingMethods.LogAssertionTrue(test, flag, "Verify New Deleted Payment displayed in Deleted Payments table with Confirmation Number: " + confirmationNumber);
        }

        /// <summary>
        /// Method to Verify Newly Deleted Payment Reason Displayed In Deleted Payments Table
        /// </summary>
        /// <param name="confirmationNumber">Confirmation Number</param>
        /// <param name="reason">Delete Reason</param>
        public void VerifyNewlyDeletedPaymentReasonDisplayedInDeletedPaymentsTable(string confirmationNumber, string reason)
        {
            By paymentDeletedTableRowLocBy = By.XPath(paymentDeletedTableRow.Replace("<CONFIRMATIONNUMBER>", confirmationNumber));
            webElementExtensions.ScrollIntoView(_driver, paymentDeletedTableRowLocBy);
            webElementExtensions.WaitUntilElementIsClickable(_driver, paymentDeletedTableRowLocBy, ConfigSettings.LongWaitTime);
            webElementExtensions.ClickElementUsingJavascript(_driver, paymentDeletedTableRowLocBy);
            By deleteReasonLocBy = By.XPath(deleteReasonTableRow.Replace("<CONFIRMATIONNUMBER>", confirmationNumber));
            webElementExtensions.WaitForElement(_driver, deleteReasonLocBy, ConfigSettings.LongWaitTime);
            webElementExtensions.ScrollIntoView(_driver, deleteReasonLocBy);
            string deletedReasonText = _driver.FindElement(deleteReasonLocBy).Text;
            ReportingMethods.LogAssertionTrue(test, deletedReasonText.Contains(reason), "Verify Payment deletion reason message displayed");
        }

        /// <summary>
        /// Method to Edit Newly Added Payment
        /// </summary>
        /// <param name="confirmationNumber">Confirmation Number</param>
        public void EditNewlyAddedPayment(string confirmationNumber)
        {
            webElementExtensions.WaitForElement(_driver, tablePendingPaymentLocBy);
            var rows = _driver.FindElement(tablePendingPaymentLocBy).FindElements(By.XPath(".//tbody//tr"));
            try
            {
                bool flag = false;
                foreach (var row in rows)
                {
                    webElementExtensions.ScrollIntoView(_driver, row);
                    string rowText = row.Text;
                    if (rowText.Contains(confirmationNumber))
                    {
                        if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(pendingPaymentsEditImgDisabledLocBy.Replace("<CONFIRMATIONNUMBER>", confirmationNumber))))
                        {
                            IWebElement element = row.FindElement(pendingPaymentsEditImgLocBy);
                            webElementExtensions.ClickElementUsingJavascript(_driver, element);
                            ReportingMethods.LogAssertionTrue(test, true, "Verify New Payment Edit Clicked successfully");
                            flag = true;
                        }
                        else
                        {
                            ReportingMethods.LogAssertionTrue(test, false, "New payment with confirmation number - " + confirmationNumber + " got posted. Edit icon is disabled and so unable to proceed with further testing.");
                        }
                        break;
                    }
                }
                if (!flag)
                    ReportingMethods.LogAssertionTrue(test, flag, "Verify New Payment Edit Clicked successfully");
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// Method to verify "Override Required to Continue" span element is displayed
        /// </summary>
        public void VerifyOverrideRequiredToContinueBrokenOrDeletedPopupIsDisplayed()
        {
            By locBy = By.XPath(spanByText.Replace("<TEXT>", "Override Required to Continue"));
            webElementExtensions.WaitForElement(_driver, locBy);
            webElementExtensions.WaitForVisibilityOfElement(_driver, locBy);
            webElementExtensions.IsElementDisplayed(_driver, locBy, "Override Required to Continue");
        }

        /// <summary>
        /// Method to returns true if the radiobutton is checked or vice-versa
        /// </summary>
        /// <param name="locator">By Id, xpath</param>
        /// <returns>True/False</returns>
        public bool IsRadiobuttonSelected(By locator, bool isReportRequired = false)
        {

            bool flag = false;
            bool returnValue = false;

            try
            {
                webElementExtensions.WaitForElement(_driver, locator);
                if (_driver.FindElement(locator).GetAttribute("class").Contains("mat-radio-checked"))
                    returnValue = true;
                else
                    returnValue = false;
            }
            catch (Exception ex)
            {
                log.Error("Failed while checking radiobutton is checked " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully  checked radiobutton is checked.", "Failed while checking radiobutton is checked.");
            return returnValue;
        }

        /// <summary>
        /// Method to verify Delete Forbearence Payment Fields
        /// </summary>
        /// <param name="additionalPrincipalParam">Addtional Principal Amount</param>
        /// <param name="additionalEscrowAmountParam">Addtional Escrow Amount</param>
        /// <param name="additionalPrincipalEditParam">Addtional Principal Edit Amount</param>
        /// <param name="type">Payment flow Type -setup/Edit</param>
        /// <returns>Forbearence Payment Fields Details</returns>
        public Dictionary<string, string> VerifyDeleteForbearencePaymentFields(Hashtable loanLevelData, double additionalPrincipalParam, double additionalEscrowAmountParam, double additionalPrincipalEditParam = 0.0, string type = "setup")
        {
            bool radioButtonStatus = false;
            double totalPayment = 0.00;
            double defaultTotalPaymentAmount = 0.00;
            double pastDuePaymentAmount = 0.00;
            Dictionary<string, string> dic = new Dictionary<string, string>();

            if (Convert.ToInt16(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 1)
            {
                webElementExtensions.WaitForElement(_driver, currentPaymentCheckboxLocBy);
                if (webElementExtensions.GetElementAttribute(_driver, currentPaymentCheckboxLocBy, Constants.ElementAttributes.AriaChecked) == "true")
                {
                    radioButtonStatus = true;
                }
                if (radioButtonStatus == false)
                {
                    webElementExtensions.WaitForStalenessOfElement(_driver, currentPaymentCheckboxLocBy);
                    webElementExtensions.WaitForElement(_driver, currentPaymentCheckboxLocBy);
                    webElementExtensions.ActionClick(_driver, currentPaymentCheckboxLocBy, "Monthly Payment (Past Due)");
                    defaultTotalPaymentAmount = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", "").Trim());
                    pastDuePaymentAmount = Convert.ToDouble(_driver.FindElement(pastDueRadioButtonAmountTextLocBy).Text.Replace("$", "").Trim());
                    ReportingMethods.LogAssertionEqual(test, defaultTotalPaymentAmount.ToString(), pastDuePaymentAmount.ToString(), type == "setup" ? "Verify Initial Total Payment field value equals with Monthly Payment (Past Due) amount  when Monthly Payment (Past Due) radio button checked" : $"Verify " + type + " Initial Total Payment field value equals with Monthly Payment (Past Due) payment amount  when Monthly Payment (Past Due) radio button checked");
                }
                else
                {
                    defaultTotalPaymentAmount = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                }
            }
            else
            {
                By pastDueAmountLocBy = By.XPath(radioButtonByText.Replace("<TEXT>", "Past Due Amount"));
                webElementExtensions.WaitForElement(_driver, pastDueAmountLocBy);
                radioButtonStatus = IsRadiobuttonSelected(pastDueAmountLocBy);
                if (radioButtonStatus == false)
                {
                    webElementExtensions.WaitForStalenessOfElement(_driver, pastDueAmountLocBy);
                    webElementExtensions.WaitForElement(_driver, pastDueAmountLocBy);
                    webElementExtensions.ActionClick(_driver, pastDueAmountLocBy, "Past Due Amount");
                    defaultTotalPaymentAmount = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", "").Trim());
                    pastDuePaymentAmount = Convert.ToDouble(_driver.FindElement(pastDueRadioButtonAmountTextLocBy).Text.Replace("$", "").Trim());
                    ReportingMethods.LogAssertionEqual(test, defaultTotalPaymentAmount.ToString(), pastDuePaymentAmount.ToString(), type == "setup" ? "Verify Initial Total Payment field value equals with PastDue payment amount  when PastDue radio button checked" : $"Verify " + type + " Initial Total Payment field value equals with PastDue payment amount  when PastDue radio button checked");
                }
                else
                {
                    defaultTotalPaymentAmount = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                }
            }

            double lateFee = 0.00;
            double otherFee = 0.00;
            if (_driver.FindElements(lateFeeAmountCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
            {
                webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                webElementExtensions.ActionClick(_driver, lateFeeAmountCheckboxLocBy, "Late Fee Checkbox");
                lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (defaultTotalPaymentAmount + lateFee).ToString("N"), $"Verify Total Payment with LateFee calculation");
            }
            if (_driver.FindElements(otherFeeAmountCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
            {
                webElementExtensions.ActionClick(_driver, otherFeeAmountCheckboxLocBy, "Other Fee Checkbox");
                otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (defaultTotalPaymentAmount + lateFee + otherFee).ToString("N"), $"Verify Total Payment with OtherFee calculation");
            }

            double intialTotalPayment = Convert.ToDouble(defaultTotalPaymentAmount);
            double additionalPrincipalAmount = 0.00;
            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
            if (webElementExtensions.GetElementAttribute(_driver, additionalPaymentCheckboxLocBy, Constants.ElementAttributes.AriaChecked) != "true")
            {
                ReportingMethods.Log(test, "Verify initially Addtional Payment checkbox is not checked");
                webElementExtensions.ClickElementUsingJavascript(_driver, additionalPaymentCheckboxLocBy, "Addtional Payment checkbox");
            }
            if (type == "setup")
            {
                webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalParam * 100).ToString());
                ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalParam));
            }
            else
            {
                webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalEditParam * 100).ToString());
                ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalEditParam));
            }
            var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
            totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            additionalPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Replace("$", "").Trim());
            ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
            if (type.ToLower() == "edit")
            {
                additionalPrincipalAmount = Convert.ToDouble(commonServices.GetDollarFormattedAmount(additionalPrincipalEditParam - additionalPrincipalParam).ToString().Replace("$", ""));
            }
            ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + additionalPrincipalAmount + lateFee + otherFee).ToString("N"), type == "setup" ? $"Verify Total Payment with Additional Principal calculation" : $"Verify " + type + " Total Payment with Additional Principal calculation");

            double escrowAmount = 0.00;
            if (_driver.FindElements(additionalEscrowTextboxLocBy).Count > 0 && type.ToLower() == "setup")
            {
                webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowAmountParam * 100).ToString());
                ReportingMethods.Log(test, "Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowAmountParam));
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.Log(test, "Total Payment After Additional Escrow Amount Entered: " + "$" + totalPayment.ToString());
                var addescrowAmount = commonServices.GetValueUsingJS(additionalEscrowTextboxLocBy);
                escrowAmount = Convert.ToDouble(addescrowAmount.ToString().Trim().Replace("$", ""));
                if (type.ToLower() == "edit")
                {
                    escrowAmount = 0.00;
                }
                ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N").Trim(), "$" + (intialTotalPayment + additionalPrincipalAmount + escrowAmount + lateFee + otherFee).ToString("N").Trim(), type == "setup" ? $"Verify Total Payment with Additional Principal and Escrow calculation" : $"Verify " + type + " Total Payment with Additional Principal and Escrow calculation");
            }

            string addTotalAmount = "$" + totalPayment.ToString("N");
            dic.Add("AddTotalAmount", addTotalAmount);
            dic.Add("TotalPayment", "$" + totalPayment.ToString("N"));
            return dic;
        }

        /// <summary>
        /// Verify Current Month Forbearence Deleted Plan Payment Fields
        /// </summary>
        /// <param name="additionalPrincipalParam">Addtional Principal Amount</param>
        /// <param name="additionalEscrowAmountParam">Addtional Escrow Amount</param>
        /// <param name="additionalPrincipalEditParam">Addtional Principal Edit Amount</param>
        /// <param name="type">Payment flow - setup/Edit</param>
        /// <returns>total payment</returns>
        public string VerifyCurrentMonthForbearenceDeletedPlanPaymentFields(double additionalPrincipalParam, double additionalEscrowAmountParam, double additionalPrincipalEditParam = 0.0, string type = "setup")
        {
            double totalPayment = 0.00;
            string defaultTotalPaymentAmount = "";

            string monthlyPayment = GetAmountText(monthlyPaymentPastDueAmountSpanLocBy);
            defaultTotalPaymentAmount = GetAmountText(divTotalAmountLocBy);
            if (type.ToLower() != "edit")
            {
                ReportingMethods.LogAssertionEqual(test, monthlyPayment, defaultTotalPaymentAmount, type == "setup" ? "Verify Initial Total Payment field value equals with Deault Checked PastDue payment amount" : $"Verify " + type + " Initial Total Payment field value equals with Default Checked PastDue payment amount");
            }

            if (webElementExtensions.GetElementAttribute(_driver, addPaymentCheckBoxLocBy, "aria-checked") == "false")
            {
                webElementExtensions.ClickElementUsingJavascript(_driver, addPaymentCheckBoxLocBy, "Additional Payment (Principal Only, Escrow, etc.) CheckBox");
                defaultTotalPaymentAmount = GetAmountText(divTotalAmountLocBy);
            }
            else
            {
                defaultTotalPaymentAmount = GetAmountText(divTotalAmountLocBy);
            }

            double intialTotalPayment = Convert.ToDouble(defaultTotalPaymentAmount.Replace("$", ""));
            double additionalPrincipalAmount = 0.00;
            if (_driver.FindElement(additionalPrincipalTextboxLocBy).Enabled)
            {
                if (type == "setup")
                {
                    webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalParam * 100).ToString());
                    ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalParam));
                }
                else
                {
                    webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalEditParam * 100).ToString());
                    ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalEditParam));
                }
                var addPrincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);

                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                additionalPrincipalAmount = Convert.ToDouble(addPrincipalAmount.ToString().Trim().Replace("$", ""));
                ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
                if (type.ToLower() == "edit")
                {
                    additionalPrincipalAmount = Convert.ToDouble(commonServices.GetDollarFormattedAmount(additionalPrincipalEditParam - additionalPrincipalParam).ToString().Replace("$", ""));
                }
                ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + additionalPrincipalAmount).ToString("N"), type == "setup" ? $"Verify Total Payment with Additional Principal calculation" : $"Verify " + type + " Total Payment with Additional Principal calculation");
                reportLogger.TakeScreenshot(test, "Additional Principal");
            }
            double escrowAmount = 0.00;
            if (_driver.FindElements(additionalEscrowTextboxLocBy).Count > 0)
            {
                if (_driver.FindElement(additionalEscrowTextboxLocBy).Enabled)
                {
                    webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowAmountParam * 100).ToString());
                    ReportingMethods.Log(test, type == "setup" ? "" : "Edit " + "Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowAmountParam));

                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Additional Escrow Amount Entered: " + "$" + totalPayment.ToString());
                    var addEscrowAmount = commonServices.GetValueUsingJS(additionalEscrowTextboxLocBy);
                    escrowAmount = Convert.ToDouble(addEscrowAmount.ToString().Trim().Replace("$", ""));
                    if (type == "edit")
                    {
                        escrowAmount = 0.00;
                    }
                }
                ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N").Trim(), "$" + (intialTotalPayment + additionalPrincipalAmount + escrowAmount).ToString("N").Trim(), type == "setup" ? $"Verify Total Payment with Additional Principal and Escrow calculation" : $"Verify " + type + " Total Payment with Additional Principal and Escrow calculation");
            }
            return "$" + totalPayment.ToString("N");
        }

        #endregion Services

        #region OTP Services

        /// <summary>
        /// Method to Verify OTP Page Is Displayed
        /// </summary>
        /// <param name="type">Payment flow - setup/Edit</param>
        public void VerifyOTPPageIsDisplayed(string type = "setup")
        {
            webElementExtensions.WaitForElement(_driver, paymentFormLocBy);
            webElementExtensions.ScrollIntoView(_driver, paymentFormLocBy);
            webElementExtensions.IsElementDisplayed(_driver, paymentFormLocBy, type == "setup" ? "Main payment page is displayed" : type + "Main payment page is displayed");
        }

        /// <summary>
        /// Method to Verify Update Payment Button Is Disabled
        /// </summary>
        /// <param name="type">Payment flow - Edit/Update</param>
        public void VerifyUpdatePaymentButtonIsDisabled(string type = "setup")
        {
            By locBy = By.XPath(buttonByText.Replace("<BUTTONNAME>", "Update Payment"));
            webElementExtensions.WaitForElement(_driver, locBy);
            bool flag = webElementExtensions.IsElementEnabled(_driver, locBy);
            reportLogger.TakeScreenshot(test, "Update Payment button");
            ReportingMethods.LogAssertionFalse(test, flag, type == "" ? "Verify Update Payment button is disabled" : "Verify Edit Update Payment button is disabled");
        }

        /// <summary>
        /// Method to Verify Update Payment Button Is Enabled
        /// </summary>
        /// <param name="type">Payment flow - Edit/Update</param>
        public void VerifyUpdatePaymentButtonIsEnabled(string type = "setup")
        {
            By locBy = By.XPath(buttonByText.Replace("<BUTTONNAME>", "Update Payment"));
            webElementExtensions.WaitForElement(_driver, locBy);
            bool flag = webElementExtensions.IsElementEnabled(_driver, locBy);
            ReportingMethods.LogAssertionTrue(test, flag, type == "" ? "Verify Update Payment button is Enabled" : "Verify Edit Update Payment button is Enabled");
        }

        /// <summary>
        /// Method to Verify Make A Payment Button Is Disabled
        /// </summary>
        /// <param name="type">Payment flow - Edit/Update</param>
        public void VerifyMakeAPaymentButtonIsDisabled(string type = "setup")
        {
            By locBy = By.XPath(buttonByText.Replace("<BUTTONNAME>", "Make a Payment"));
            webElementExtensions.WaitForElement(_driver, locBy);
            bool flag = webElementExtensions.IsElementEnabled(_driver, locBy);
            ReportingMethods.LogAssertionFalse(test, flag, type == "setup" ? "Verify Make a Payment button is disabled" : "Verify Edit Make a Payment button is disabled");
        }

        /// <summary>
        /// Method to Verify Make A Payment Button Is Enabled
        /// </summary>
        /// <param name="type">Payment flow - Edit/Update</param>
        public void VerifyMakeAPaymentButtonIsEnabled(string type = "setup")
        {
            By locBy = By.XPath(buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment));
            webElementExtensions.WaitForElement(_driver, locBy);
            bool flag = webElementExtensions.IsElementEnabled(_driver, locBy);
            ReportingMethods.LogAssertionTrue(test, flag, type == "setup" ? "Verify Make a Payment button is enabled" : "Verify Edit Make a Payment button is enabled");
        }

        /// <summary>
        /// Method to Verify Apply Button Is Disabled
        /// </summary>
        /// <param name="type">Payment flow - Edit/Update</param>
        public void VerifyApplyButtonIsDisabled(string type = "setup")
        {
            By locBy = By.XPath(buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Apply));
            webElementExtensions.WaitForElement(_driver, locBy);
            bool flag = webElementExtensions.IsElementEnabled(_driver, locBy);
            ReportingMethods.LogAssertionFalse(test, flag, type == "setup" ? "Verify Apply button is disabled" : "Verify Edit Apply button is disabled");
        }

        /// <summary>
        /// Method to Verify Apply Button Is Enabled
        /// </summary>
        /// <param name="type">Payment flow - Edit/Update</param>
        public void VerifyApplyButtonIsEnabled(string type = "setup")
        {
            By locBy = By.XPath(buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.Apply));
            webElementExtensions.WaitForElement(_driver, locBy);
            bool flag = webElementExtensions.IsElementEnabled(_driver, locBy);
            ReportingMethods.LogAssertionTrue(test, flag, type == "setup" ? "Verify Apply button is enabled" : "Verify Edit Apply button is enabled");
        }

        /// <summary>
        /// Method to check OTP Page Title Is Displayed
        /// </summary>
        /// <param name="title">OTP Title</param>
        public void VerifyOTPPageTitleIsDisplayed(string title, bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                By elementLocBy = By.XPath(sectionTitleText.Replace("<TITLE>", title));
                webElementExtensions.WaitForElement(_driver, elementLocBy);
                webElementExtensions.ScrollIntoView(_driver, elementLocBy);
                webElementExtensions.IsElementDisplayed(_driver, elementLocBy, "page title is displayed as " + title);
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
        /// Method to Verify Late Fees Warning Msg Not Displayed
        /// </summary>
        public void VerifyLateFeesWarningMsgNotDisplayed()
        {
            ReportingMethods.LogAssertionFalse(test, webElementExtensions.IsElementDisplayed(_driver, lateFeesWarningTextLocBy), "verify late fees warning message is not displayed");
        }

        /// <summary>
        /// Method to Verify Edit Authorized By is disabled
        /// </summary>
        public void VerifyAuthorizedByIsDisabled()
        {
            ReportingMethods.LogAssertionTrue(test, _driver.FindElements(disabledAuthorizedByMatSelectLocBy).Count > 0, "Verify Edit Authorized By is disabled");
        }

        /// <summary>
        /// Method to Click on PartialReInstatement Radio Button Payment Page
        /// </summary>
        public void ClickAmountMonthlyPaymentRadioButton()
        {
            webElementExtensions.WaitUntilElementIsClickable(_driver, partialReInstatementRadioButtonLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, partialReInstatementRadioButtonLocBy, "Amount Monthly Payment radio button");
        }

        /// <summary>
        /// Method to verify Amount Monthly RadioButton is Selected By Default or Not
        /// </summary>
        public void VerifyAmountMonthlyRadioButtonSelectedByDefault()
        {
            webElementExtensions.WaitForVisibilityOfElement(_driver, amountRadioButtonLocBy);
            bool radiobuttonStatus = IsRadiobuttonSelected(amountRadioButtonLocBy);
            ReportingMethods.LogAssertionFalse(test, radiobuttonStatus, "Verify Amount Monthly radio button  is not selected by default");
        }

        /// <summary>
        /// Method to Enter Amount Monthly Payment Greater Than Unpaid Principal Balance
        /// </summary>
        public void EnterAmountMonthlyPaymentGreaterThanUnpaidPrincipalBalance()
        {
            var unPaidPrincipalBalanceSection = GetOTPSubSectionValues("Unpaid Principal Balance");
            string[] text = unPaidPrincipalBalanceSection["Unpaid Principal Balance"].ToString().Replace("$", "").Split('.');
            int amount = Convert.ToInt32(text[0].Replace(",", "")) * 100;
            amount = amount + Convert.ToInt32(text[1]) + 100;
            webElementExtensions.EnterText(_driver, monthlyAmountTextboxLocBy, amount.ToString());
            ReportingMethods.Log(test, "Entered Amount Monthly Payment field amount greater than Unpaid Principal Balance.");
        }

        /// <summary>
        /// Method  Enters Amount and Clicks Outside Amount Monthly Payment Field
        /// </summary>
        public void EnterAndClickOutsideAmountMonthlyPaymentField()
        {
            webElementExtensions.EnterText(_driver, monthlyAmountTextboxLocBy, "0000");
            webElementExtensions.EnterText(_driver, monthlyAmountTextboxLocBy, Keys.Tab);
            ReportingMethods.Log(test, "Clicked tab to get outside the Amount Monthly Payment field without entering amount");
        }

        /// <summary>
        /// Method to Verify the Inline Error Message is displayed
        /// </summary>
        /// <param name="inlineErrorMessage">Field Inline Error Message</param>
        public void VerifyInlineErrorMessageIsDisplayed(string inlineErrorMessage)
        {
            var locBy = _driver.FindElement(By.XPath(divText.Replace("<TEXT>", inlineErrorMessage)));
            ReportingMethods.LogAssertionContains(test, inlineErrorMessage, locBy.Text.Trim(), "Verify inline Error message " + inlineErrorMessage + " is displayed");
        }

        /// <summary>
        /// Method to Verify Amount Monthly Payment
        /// </summary>
        /// <param name="amountMonthlyPayment">Amount Monthly Payment</param>
        public void VerifyAmountMonthlyPayment(string amountMonthlyPayment)
        {
            ReportingMethods.LogAssertionEqual(test, amountMonthlyPayment, GetAmountMonthlyPaymentValue(), "Verify Edit Amount Monthly Payment");
        }

        /// <summary>
        /// Method to Verify DelinquentPaymentBalance Calculation Based on start Of month is Weekend Holiday Monday
        /// </summary>
        /// <param name="loanLevelData">Loan Level Data from DB</param>
        public void VerifyDelinquentPaymentBalanceCalculationBasedOnStartOfMonthIsWeekendHoliday(Hashtable loanLevelData)
        {
            DateTime date = DateTime.Now;
            List<DateTime> bankHolidays = commonServices.GetBankHolidays(Convert.ToString(date.Year));
            DateTime firstWorkingDate = commonServices.GetFirstWorkingDateOfMonth();
            string monthlyPaymentPastDueText = "";
            string monthlyPaymentPastDueAmount = "";
            string secondPastMonth = date.AddMonths(-2).ToString("MMM");
            string pastMonth = date.AddMonths(-1).ToString("MMM");
            string currentMonth = date.ToString("MMM");
            if ((date.Day == 1) && (date.DayOfWeek.ToString().ToLower() == "saturday" || date.DayOfWeek.ToString().ToLower() == "sunday" || bankHolidays.Contains(date)))
            {
                if (Convert.ToInt16(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 1)
                {
                    monthlyPaymentPastDueText = webElementExtensions.GetElementText(_driver, monthlyPaymentPastDueAmountFieldTextLocBy).Trim();
                    monthlyPaymentPastDueAmount = GetAmountText(monthlyPaymentPastDueAmountSpanLocBy);
                    ReportingMethods.LogAssertionEqual(test, "Monthly Payment (Past Due)", monthlyPaymentPastDueText, "Verify Monthly Payment (Past Due) field text");
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment]).ToString("N").Trim()}", monthlyPaymentPastDueAmount, "Verify Monthly Payment (Past Due) value");
                }
                else
                {
                    monthlyPaymentPastDueText = webElementExtensions.GetElementText(_driver, monthlyPaymentPastDueAmountFieldTextLocBy).Trim();
                    monthlyPaymentPastDueAmount = GetAmountText(monthlyPaymentPastDueAmountSpanLocBy);
                    string expectedMonthlyPaymentPastDueText = "Past Due Amount ( " + secondPastMonth + ". + " + pastMonth + ". Payments)".Trim();
                    ReportingMethods.LogAssertionEqual(test, expectedMonthlyPaymentPastDueText, monthlyPaymentPastDueText, "Verify Past Due Amount field text");
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentBalance]).ToString("N").Trim()}", monthlyPaymentPastDueAmount, "Verify Past Due Amount value");
                }
            }
            else if (date <= firstWorkingDate.AddDays(1).AddHours(10) || date.ToString("MMMM d, yyyy").Trim() == firstWorkingDate.ToString("MMMM d, yyyy").Trim())
            {
                if (Convert.ToInt16(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 1)
                {
                    monthlyPaymentPastDueText = webElementExtensions.GetElementText(_driver, monthlyPaymentPastDueAmountFieldTextLocBy).Trim();
                    monthlyPaymentPastDueAmount = GetAmountText(monthlyPaymentPastDueAmountSpanLocBy);
                    ReportingMethods.LogAssertionEqual(test, "Past Due Amount ( " + pastMonth + ". Payments)", monthlyPaymentPastDueText, "Verify Past Due Amount field text");
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment]).ToString("N").Trim()}", monthlyPaymentPastDueAmount, "Verify Past Due Amount value");
                }
                else
                {
                    monthlyPaymentPastDueText = webElementExtensions.GetElementText(_driver, pastDueAmountRadioButtonFieldTextLocBy).Trim().Replace("\r\n", " ");
                    monthlyPaymentPastDueAmount = GetAmountText(pastDueRadioButtonAmountTextLocBy);
                    ReportingMethods.LogAssertionEqual(test, "Past Due Amount ( " + secondPastMonth + " + " + pastMonth + ". Payments)", monthlyPaymentPastDueText, "Verify Past Due Amount field text");
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentBalance]).ToString("N").Trim()}", monthlyPaymentPastDueAmount, "Verify Past Due Amount value");
                }

            }
            else
            {
                if (Convert.ToInt16(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 2)
                {
                    monthlyPaymentPastDueText = webElementExtensions.GetElementText(_driver, monthlyPaymentPastDueAmountFieldTextLocBy).Trim();
                    monthlyPaymentPastDueAmount = GetAmountText(monthlyPaymentPastDueAmountSpanLocBy);
                    ReportingMethods.LogAssertionEqual(test, "Past Due Amount ( " + pastMonth + ". + " + currentMonth + ". Payments)", monthlyPaymentPastDueText, "Verify Past Due Amount field text");
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentBalance]).ToString("N").Trim()}", monthlyPaymentPastDueAmount, "Verify Past Due Amount value");
                }
                else
                {

                    monthlyPaymentPastDueText = webElementExtensions.GetElementText(_driver, pastDueAmountRadioButtonFieldTextLocBy).Trim().Replace("\r\n", " ");
                    monthlyPaymentPastDueAmount = GetAmountText(pastDueAmountTextLocBy);
                    ReportingMethods.LogAssertionEqual(test, "Past Due Amount (3 mo. -" + secondPastMonth + ". -" + currentMonth + ". Payments)", monthlyPaymentPastDueText, "Verify Past Due Amount field text");
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentBalance]).ToString("N").Trim()}", monthlyPaymentPastDueAmount, "Verify  Past Due Amount value");
                }
            }
        }

        /// <summary>
        /// Method to Verify User can Edit and Delete OTP based On 8.pm CST CutOff Time
        /// </summary>
        /// <param name="paymentDate">Payment Date of OTP</param>
        /// <param name="isReportRequired">Flag indicating whether a report is required</param>
        /// <returns>A Boolean value will be returned true when Edit and Delete buttons enabled status</returns>
        public bool VerifyUserCanEditAndDeleteOTPBasedOn8PMCSTCutOffTime(string paymentDate, bool isReportRequired = false)
        {
            bool flag = false;
            bool isEditBtnEnabled = false, isDeleteBtnEnabled = false;

            try
            {
                DateTime date = Convert.ToDateTime(paymentDate);
                paymentDate = date.ToString("MM/dd/yyyy");
                DateTime currentTime = DateTime.UtcNow;
                DateTime currentCSTTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, TimeZoneInfo.FindSystemTimeZoneById(TimeZones.CentralStandardTime));
                DateTime cutOffCSTTime = new DateTime(date.Year, date.Month, date.Day, 20, 00, 00);

                webElementExtensions.WaitForElement(_driver, tablePendingPaymentLocBy);
                webElementExtensions.ScrollIntoView(_driver, tablePendingPaymentLocBy);
                var rows = _driver.FindElement(tablePendingPaymentLocBy).FindElements(tableRowLocBy);
                foreach (var row in rows)
                {
                    var rowText = row.Text;
                    if (rowText.Contains(paymentDate))
                    {
                        if (row.FindElements(pendingEditIconLocBy).Count > 0)
                        {
                            isEditBtnEnabled = true;
                        }
                        else
                        {
                            isEditBtnEnabled = false;
                        }

                        if (row.FindElements(pendingDeleteIconLocBy).Count > 0)
                        {
                            isDeleteBtnEnabled = true;
                        }
                        else
                        {
                            isDeleteBtnEnabled = false;
                        }
                        break;
                    }
                }

                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verify user can Edit and Delete OTP before 8 PM CST on payment date " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully verified user can Edit and Delete OTP before 8 PM CST on payment date", "Failed while getting  verify user can Edit and Delete OTP before 8 PM CST on payment date ");
            return (isEditBtnEnabled && isDeleteBtnEnabled);
        }

        /// <summary>
        /// Method to Verify Payment Cut off Time Message
        /// </summary>
        public void VerifyPaymentCutoffTimeMessage()
        {
            DateTime cstTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 20, 0, 0);
            // Get the CST time zone
            TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

            string localTime = TimeZoneInfo.ConvertTime(cstTime, cstZone, TimeZoneInfo.Local).ToString("h:mm");

            Dictionary<string, string> timeZoneAbbreviations = new Dictionary<string, string>
            {
            { "Eastern Standard Time", "EST" },
            { "Central Standard Time", "CST" },
            { "Mountain Standard Time", "MST" },
            { "Pacific Standard Time", "PST" },
            { "India Standard Time", "IST" },
            { "UTC", "GMT" },
            // Add more time zones and their abbreviations as needed
            };
            string zone;
            string abbreviation;
            if (timeZoneAbbreviations.TryGetValue(TimeZoneInfo.Local.Id, out zone))
            {
                abbreviation = zone;
            }
            else
            {
                abbreviation = "";
            }

            string expected;

            if (TimeZoneInfo.Local.Id.Equals("India Standard Time") || TimeZoneInfo.Local.Id.Equals("UTC"))
            {
                expected = "Payments must be submitted by " + localTime + " a.m " + abbreviation + " to be posted on the same business day.";
            }
            else
            {
                expected = "Payments must be submitted by " + localTime + " p.m " + abbreviation + " to be posted on the same business day.";
            }

            var element = _driver.FindElement(divPaymentsSubmitNoteLocBy);
            ReportingMethods.LogAssertionEqual(test, expected, element.Text.Trim(), "Verify Payment Cutoff Time Message");
        }

        /// <summary>
        /// Simulates a click on the edit button and enters the provided email address into the appropriate field.
        /// </summary>
        /// <param name="email">The email address to be entered into the field.</param>
        public void ClickEditAndEnterMail(string email)
        {
            By editLocBy = By.XPath(spanByText.Replace("<TEXT>", "Edit"));
            webElementExtensions.ClickElement(_driver, editLocBy, "Edit Button");
            webElementExtensions.WaitForElement(_driver, emailChcekboxLocBy);
            var element = _driver.FindElement(emailChcekboxLocBy);
            ReportingMethods.LogAssertionTrue(test, element.GetAttribute(Constants.ElementAttributes.Class).Contains("mat-checkbox-checked"), "Verify Email Checkbox is already Checked");
            webElementExtensions.EnterText(_driver, inputEmailLocBy, email, true, "Email");
        }

        /// <summary>
        /// Method to Verify Delinquent Payment Fields
        /// </summary>
        /// <param name="additionalPrincipalParam">Addtional Principal Amount</param>
        /// <param name="additionalEscrowAmountParam">Addtional Escrow Amount</param>
        /// <param name="additionalPrincipalEditParam">Addtional Principal Edit Amount</param>
        /// <param name="additionalEscrowEditAmountParam">Addtional Escrow Edit Amount</param>
        /// <param name="type">Payment flow Type -setup/Edit</param>
        /// <returns>total payment</returns>
        public string VerifyOTPDelinquentPaymentFields(double additionalPrincipalParam, double additionalEscrowAmountParam, double additionalPrincipalEditParam = 0.0, double additionalEscrowEditAmountParam = 0.0, string type = "setup")
        {
            double totalPayment = 0.00, suspenseAmount = 0.0, lateFee = 0.00, otherFee = 0.00, delinquentPaymentAmount = 0.00, additionalPrincipalAmount = 0.00, escrowAmount = 0.00;
            bool flag = false;
            totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            delinquentPaymentAmount = Convert.ToDouble(GetAmountText(overDuePastDueAmountSpanTextLocBy).Replace("$", ""));

            if (IsRadiobuttonSelected(pastDueAmountRadioButtonInputLocBy) == false)
            {
                webElementExtensions.WaitForElement(_driver, pastDueAmountRadioButtonLocBy);
                webElementExtensions.ClickElement(_driver, pastDueAmountRadioButtonLocBy, "Past Due Amount RadioButton");
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (delinquentPaymentAmount).ToString("N"), $"Verify  Total Payment field value equals with PastDue payment amount  when PastDue radiobutton checked in OTP Screen");
            }

            if (_driver.FindElements(lateFeeAmountCheckboxLocBy).Count > 0)
            {
                flag = webElementExtensions.IsElementEnabled(_driver, additionalPrincipalTextboxLocBy);
                ReportingMethods.LogAssertionFalse(test, flag, "Verify Additional Principal field Initially Disabled");
                webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                flag = Convert.ToBoolean(webElementExtensions.GetElementAttribute(_driver, lateFeeAmountCheckboxLocBy, Constants.ElementAttributes.AriaChecked));
                if (flag == false)
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, lateFeeAmountCheckboxLocBy);
                    flag = Convert.ToBoolean(webElementExtensions.GetElementAttribute(_driver, lateFeeAmountCheckboxLocBy, Constants.ElementAttributes.AriaChecked));
                    if (flag)
                    {
                        ReportingMethods.Log(test, "Late Fee Checkbox is checked");
                        lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                    }
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (delinquentPaymentAmount + suspenseAmount + lateFee).ToString("N"), $"Verify Total Payment with Late Fee calculation");
                }
                flag = webElementExtensions.IsElementEnabled(_driver, additionalPrincipalTextboxLocBy);
                ReportingMethods.LogAssertionTrue(test, flag, "Verify Additional Principal field Enabled after selecting Late Fee");
            }

            if (_driver.FindElements(otherFeeAmountCheckboxLocBy).Count > 0)
            {
                flag = webElementExtensions.IsElementEnabled(_driver, additionalPrincipalTextboxLocBy);
                ReportingMethods.LogAssertionFalse(test, flag, "Verify Additional Principal field Initially Disabled");
                webElementExtensions.ScrollIntoView(_driver, otherFeeAmountCheckboxLocBy);
                if (IsRadiobuttonSelected(otherFeeAmountCheckboxLocBy) == false)
                {
                    webElementExtensions.ActionClick(_driver, otherFeeAmountCheckboxLocBy, "Other Fee Checkbox");
                    otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (delinquentPaymentAmount + suspenseAmount + lateFee + otherFee).ToString("N"), $"Verify Total Payment with OtherFee calculation");
                }
                flag = webElementExtensions.IsElementEnabled(_driver, additionalPrincipalTextboxLocBy);
                ReportingMethods.LogAssertionTrue(test, flag, "Verify Additional Principal field Enabled after selecting Other Fee");
            }

            webElementExtensions.IsElementEnabled(_driver, additionalPrincipalTextboxLocBy, "Additional Principal TextBox");

            if (_driver.FindElements(additionalPrincipalTextboxLocBy).Count > 0)
            {
                if (_driver.FindElement(additionalPrincipalTextboxLocBy).Enabled)
                {
                    if (type.ToLower().Trim() == "setup")
                    {
                        webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalParam * 100).ToString());
                        ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalParam));
                    }
                    else
                    {
                        webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalEditParam * 100).ToString());
                        ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalEditParam));
                    }
                    var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    additionalPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
                    if (type.ToLower() == "edit")
                    {
                        additionalPrincipalAmount = additionalPrincipalEditParam - additionalPrincipalParam;
                    }
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (delinquentPaymentAmount + suspenseAmount + lateFee + otherFee + additionalPrincipalAmount).ToString("N"), $"Verify Total Payment with Additional Principal calculation");
                }
            }

            if (_driver.FindElements(additionalEscrowTextboxLocBy).Count > 0)
            {
                if (_driver.FindElement(additionalEscrowTextboxLocBy).Enabled)
                {
                    if (type == "setup")
                    {
                        webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowAmountParam * 100).ToString());
                        ReportingMethods.Log(test, "Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowAmountParam));
                    }
                    else
                    {
                        webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowEditAmountParam * 100).ToString());
                        ReportingMethods.Log(test, "Edit Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowEditAmountParam));
                    }

                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Additional Escrow Amount Entered: " + "$" + totalPayment.ToString());
                    var addEscrowAmount = commonServices.GetValueUsingJS(additionalEscrowTextboxLocBy);
                    escrowAmount = Convert.ToDouble(addEscrowAmount.ToString().Trim().Replace("$", ""));
                    if (type.ToLower() == "edit")
                    {
                        escrowAmount = additionalEscrowEditAmountParam - additionalEscrowAmountParam;
                    }
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (delinquentPaymentAmount + suspenseAmount + lateFee + otherFee + additionalPrincipalAmount + escrowAmount).ToString("N"), $"Verify Total Payment with Additional Principal and Escrow calculation");
                }
            }
            return "$" + totalPayment.ToString("N");
        }

        #endregion OTP Services

        #region Payment Review Services

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
        /// Method to Click Confirm Button Payment Review Page
        /// </summary>
        public void ClickConfirmButtonPaymentReviewPage()
        {
            By locatorBy = By.XPath(buttonByText.Replace("<BUTTONNAME>", "Confirm Payment"));
            webElementExtensions.WaitForElement(_driver, locatorBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, locatorBy);
        }

        /// <summary>
        /// Method to Verify Updated Disclosure Statement In Review And Confirm Screen
        /// </summary>
        /// <param name="accountNumber">Account number details</param>
        /// <param name="accountFullName">Account Full name details</param>
        /// <param name="selectedDate">Selected Date</param>
        /// <param name="totalPayment">Total Payment Amount</param>
        /// <param name="Type">Payment flow - setup/Edit</param>
        public void VerifyUpdatedDisclosureStatementInReviewAndConfirmScreen(string accountNumber, string accountFullName, string selectedDate, string totalPayment, string type = "setup")
        {
            string lastfourAccountNumber = accountNumber.Substring(accountNumber.Length - 4);
            string date = Convert.ToDateTime(selectedDate).ToString("MM/dd/yyyy");
            string totalAmount = totalPayment;
            string amountInWords = webElementExtensions.NumberToCurrencyText(Convert.ToDecimal(totalAmount.Replace("$", "")));
            string disclosure = $"You are authorizing loanDepot to make a one-time debit from account ending in ****{lastfourAccountNumber}, in the amount of {totalAmount}, ({amountInWords}) on {date}. This account is in the name of {accountFullName.ToUpper()}. Do you attest that you are the authorized user on this account, and do you agree to this transaction?";
            string actualDisclosure = _driver.FindElement(disclosureLabelLocBy).Text;
            if (disclosure.Contains("One Cent)"))//adding this logic since it's displayed the same way in application[low priority defect as per stakeholders]
                disclosure = disclosure.Replace("One Cent)", "One Cents)");
            ReportingMethods.LogAssertionEqual(test, disclosure, actualDisclosure, type == "setup" ? "Verify Disclosure statement" : "Verify " + type + " Disclosure statement");
        }

        #endregion Payment Review Services

        #region Payment Confirmation Services

        /// <summary>
        /// Method to Verify Payment Confirmation Success Message Is Displayed
        /// </summary>
        /// <param name="message">Payment Confirmation Success Message</param>
        /// <param name="type">Payment flow - setup/Edit</param>
        public void VerifyPaymentConfirmationSuccessMessageIsDisplayed(string message, string type = "setup")
        {
            By locBy = By.XPath(spanByText.Replace("<TEXT>", message));
            webElementExtensions.WaitForVisibilityOfElement(_driver, locBy);
            ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, locBy), type == "setup" ? "Verify success message: " + message : "Verify " + type + " success message" + message);
            webElementExtensions.ScrollToTop(_driver);
            reportLogger.TakeScreenshot(test, "Payment Confirmation Success Message");
        }

        /// <summary>
        /// Method to Get Confirmation Number From Payments Confirmation Page
        /// </summary>
        /// <returns>Confirmation Number</returns>
        public string GetConfirmationNumberFromPaymentsConfirmationPage()
        {
            webElementExtensions.WaitForElement(_driver, confirmationNumberTextLocBy);
            var confirmationNumber = _driver.FindElement(confirmationNumberTextLocBy).Text.Trim();
            ReportingMethods.Log(test, "Confirmation Number: " + confirmationNumber);
            return confirmationNumber;
        }

        /// <summary>
        /// Method to Click On Go Back To Payments Link On Payment Review Page
        /// </summary>
        public void ClickOnGoBackToPaymentsLinkOnPaymentReviewPage()
        {
            By locBy = By.XPath(buttonByText.Replace("<BUTTONNAME>", "Go back to Payments"));
            webElementExtensions.WaitForElement(_driver, locBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, locBy);
        }

        #endregion Payment Confirmation Services

        #region Repayment Services

        /// <summary>
        /// Method to Verify Fields Broken/Delete of Repayment Plan
        /// </summary>
        /// <param name="additionalPrincipalParam">Additional Principal Amount</param>
        /// <param name="additionalEscrowAmountParam">Additional Escrow Amount</param>
        /// <param name="additionalPrincipalEditParam">Additional Principal Edit Amount</param>
        /// <param name="additionalEscrowEditAmountParam">Additional Escrow Edit Amount</param>
        /// <param name="type">Payment flow Type -setup/Edit</param>
        /// <returns>total payment</returns>
        public string VerifyRepaymentBrokenDeletePaymentFields(double additionalPrincipalParam, double additionalEscrowAmountParam, double additionalPrincipalEditParam = 0.0, double additionalEscrowEditAmountParam = 0.0, string type = "setup")
        {
            bool radiobuttonStatus;
            double totalPayment = 0.00;
            double additionalPrincipalAmount = 0.00;
            string defaulttotalpaymentamount = GetAmountText(divTotalAmountLocBy);
            double intialTotalPayment = Convert.ToDouble(defaulttotalpaymentamount.Replace("$", ""));
            ReportingMethods.Log(test, "Intial Total Payment is" + " " + defaulttotalpaymentamount);
            radiobuttonStatus = IsRadiobuttonSelected(pastDueRadioButtonLocBy);
            if (radiobuttonStatus == false)
            {
                ReportingMethods.Log(test, "Initial Past Due Amount Radio Button is not selected");
                webElementExtensions.WaitUntilElementIsClickable(_driver, pastDueRadioButtonLocBy);
                webElementExtensions.ActionClick(_driver, pastDueRadioButtonLocBy, "Past Due Amount radio button");
            }

            defaulttotalpaymentamount = GetAmountText(divTotalAmountLocBy);
            intialTotalPayment = Convert.ToDouble(defaulttotalpaymentamount.Replace("$", ""));
            totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));

            double suspenseAmount = 0.0;
            if (_driver.FindElements(suspensePaymentCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
            {
                webElementExtensions.ScrollIntoView(_driver, suspensePaymentCheckboxLocBy);
                if (IsRadiobuttonSelected(suspensePaymentCheckboxLocBy) == false)
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, suspensePaymentCheckboxLocBy, "Suspense Checkbox");
                    if (_driver.FindElements(addToSuspenseBalanceTextboxLocBy).Count > 0)
                    {
                        suspenseAmount = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, addToSuspenseBalanceTextboxLocBy).Replace("$", ""));
                    }
                    else
                    {
                        suspenseAmount = 0.0;
                    }
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + suspenseAmount).ToString("N"), $"Verify Total Payment with Suspense calculation");
                }
            }

            double lateFee = 0.0;
            if (_driver.FindElements(lateFeeAmountCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
            {
                webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                if (IsRadiobuttonSelected(lateFeeAmountCheckboxLocBy) == false)
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, lateFeeAmountCheckboxLocBy, "Late Fee Checkbox");
                    lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + (intialTotalPayment + suspenseAmount + lateFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with LateFee calculation");
                }
            }

            if (_driver.FindElements(additionalPrincipalTextboxLocBy).Count > 0)
            {
                webElementExtensions.ScrollIntoView(_driver, additionalPrincipalTextboxLocBy);
                if (_driver.FindElement(additionalPrincipalTextboxLocBy).Enabled)
                {
                    var addprincipal = _driver.FindElement(additionalPrincipalTextboxLocBy);
                    if (type == "setup")
                    {
                        webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalParam * 100).ToString());
                        ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalParam));
                    }
                    else
                    {
                        webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalEditParam * 100).ToString());
                        ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalEditParam));
                    }
                    var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    additionalPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
                    if (type.ToLower() == "edit")
                    {
                        additionalPrincipalAmount = additionalPrincipalEditParam - additionalPrincipalParam;
                    }
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + suspenseAmount + additionalPrincipalAmount + lateFee).ToString("N"), $"Verify Total Payment with Additional Principal calculation");

                }
            }
            double escrowAmount = 0.00;
            if (_driver.FindElements(additionalEscrowTextboxLocBy).Count > 0)
            {
                webElementExtensions.ScrollIntoView(_driver, additionalEscrowTextboxLocBy);
                if (_driver.FindElement(additionalEscrowTextboxLocBy).Enabled)
                {
                    var addescrow = _driver.FindElement(additionalEscrowTextboxLocBy);
                    if (type == "setup")
                    {
                        webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowAmountParam * 100).ToString());
                        ReportingMethods.Log(test, "Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowAmountParam));
                    }
                    else
                    {
                        webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowEditAmountParam * 100).ToString());
                        ReportingMethods.Log(test, "Edit Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowEditAmountParam));
                    }

                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Additional Escrow Amount Entered: " + "$" + totalPayment.ToString());
                    var addescrowAmount = commonServices.GetValueUsingJS(additionalEscrowTextboxLocBy);
                    escrowAmount = Convert.ToDouble(addescrowAmount.ToString().Trim().Replace("$", ""));
                    if (type.ToLower() == "edit")
                    {
                        escrowAmount = additionalEscrowEditAmountParam - additionalEscrowAmountParam;
                    }
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + suspenseAmount + additionalPrincipalAmount + escrowAmount + lateFee).ToString("N"), $"Verify Total Payment with Additional Principal and Escrow calculation");

                }
            }
            return "$" + totalPayment.ToString("N");
        }

        /// <summary>
        /// Method to Verify Fields of Active of Repayment Plan
        /// </summary>
        /// <param name="additionalPaymentsParam">Addtional Payments Amount</param>
        ///  <param name="additionalPaymentsEditParam">Addtional Payments Edit Amount</param>
        /// <param name="type">Payment flow Type -setup/Edit</param>
        /// <returns>total payment</returns>
        public string VerifyRepaymentPaymentFields(double additionalPaymentsParam, double additionalPaymentsEditParam = 0.0, string type = "setup")
        {
            bool radiobuttonStatus;
            double totalPayment = 0.00;
            double additionalPaymentsAmount = 0.00;
            type = type.ToLower() == "setup" ? "" : type;
            string defaultTotalPaymentAmount = GetAmountText(divTotalAmountLocBy);
            double intialTotalPayment = Convert.ToDouble(defaultTotalPaymentAmount.Replace("$", ""));
            radiobuttonStatus = IsRadiobuttonSelected(repaymentPlanRadioButtonIdLocBy);
            if (radiobuttonStatus == false)
            {
                webElementExtensions.ClickElementUsingJavascript(_driver, repaymentPlanRadioButtonLocBy, "Repayment Plan radio button");
            }
            if (_driver.FindElements(checkedAdditionalPaymentCheckboxLocBy).Count == 0)
            {
                ReportingMethods.Log(test, "Verify initially Addtional Payment checkbox is not checked");
                webElementExtensions.ClickElementUsingJavascript(_driver, additionalPaymentsCheckboxLocBy, "Addtional Payment checkbox");
            }
            var addprincipal = _driver.FindElement(additionalPaymentTextLocBy);
            addprincipal.Clear();
            additionalPaymentsAmount = additionalPaymentsParam;
            if (type.ToLower() == "edit") { additionalPaymentsAmount = additionalPaymentsEditParam; }
            addprincipal.SendKeys((additionalPaymentsAmount * 100).ToString());
            ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + additionalPaymentsAmount.ToString());
            var addprincipalAmount = commonServices.GetValueUsingJS(additionalPaymentTextLocBy);
            totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            additionalPaymentsAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
            if (type.ToLower() == "edit")
            {
                additionalPaymentsAmount = additionalPaymentsAmount - additionalPaymentsParam;
            }
            ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
            ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + additionalPaymentsAmount).ToString("N"), $"Verify " + type + " Total Payment with Additional Principal calculation");
            return "$" + totalPayment.ToString("N");
        }

        /// <summary>
        /// Verifies the banner details based on the loan level data, plan type, and type.
        /// </summary>
        /// <param name="loanLevelData">The loan level data as a Hashtable.</param>
        /// <param name="planType">The plan type as a string.</param>
        /// <param name="type">The type as a string. Default value is "setup".</param>
        public void VerifyBannerDetails(Hashtable loanLevelData, string planType = "", string type = "setup")
        {
            type = type.ToLower() == "setup" ? "" : type;
            int delinquentPaymentCount = Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]);

            if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 0)//On-Time
            {
                DateTime currentDate = DateTime.Now;
                DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);

                if (Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).ToString("yyyy-MM-dd") == firstDayOfNextMonth.ToString("yyyy-MM-dd"))//On-Time
                {
                    VerifyBannerOnTimeDateDetails(loanLevelData, planType, type);
                    webElementExtensions.VerifyElementColor(Constants.Colors.White, divNextDueDateLocBy, null, "Verify " + type + " " + planType + " Account Standing Banner details is White");
                }
                else
                {
                    VerifyBannerPrepaidDateDetails(loanLevelData, planType, type);
                    webElementExtensions.VerifyElementColor(Constants.Colors.Green, otpPaymentBannerLocBy, null, "Verify " + type + " " + planType + " Account Standing Banner details is Green");
                }
            }
            else if (delinquentPaymentCount == 1)//Past Due
            {
                DateTime date = DateTime.Now;
                List<DateTime> bankHolidays = commonServices.GetBankHolidays(Convert.ToString(date.Year));
                DateTime firstWorkingDate = commonServices.GetFirstWorkingDateOfMonth();
                if (date <= firstWorkingDate || date.ToString("MMMM d, yyyy").Trim() == firstWorkingDate.ToString("MMMM d, yyyy").Trim())
                {

                    if ((date.Day == 1) && (date.DayOfWeek.ToString().ToLower() == "saturday" || date.DayOfWeek.ToString().ToLower() == "sunday" || bankHolidays.Contains(date)))
                    {
                        VerifyBannerPastDueDateDetails(loanLevelData, planType, type);
                        webElementExtensions.VerifyElementColor(Constants.Colors.Orange, otpPaymentBannerLocBy, null, "Verify " + type + " " + planType + " Account Standing Banner details is Orange");
                    }
                    else
                    {
                        VerifyBannerDelinquentDateDetails(loanLevelData, planType, type);
                        webElementExtensions.VerifyElementColor(Constants.Colors.Red, delinquentDivLocBy, null, "Verify " + type + " " + planType + " Account Standing Banner details is Red");
                    }
                }
                else
                {
                    VerifyBannerPastDueDateDetails(loanLevelData, planType, type);
                    if (loanLevelData[Constants.LoanLevelDataColumns.RepayPlanStatusCode].ToString().ToUpper().Trim() == "A")
                    {
                        webElementExtensions.VerifyElementColor(Constants.Colors.Red, pastDueBannerLocBy, null, "Verify " + type + " " + planType + " Account Standing Banner details is Red");
                    }
                    else
                    {
                        webElementExtensions.VerifyElementColor(Constants.Colors.Orange, otpPaymentBannerLocBy, null, "Verify " + type + " " + planType + " Account Standing Banner details is Orange");
                    }
                }

            }
            else if (delinquentPaymentCount > 1)//Delinquent
            {
                VerifyBannerDelinquentDateDetails(loanLevelData, planType, type);
                webElementExtensions.VerifyElementColor(Constants.Colors.Red, delinquentDivLocBy, null, "Verify " + type + " " + planType + " Account Standing Banner details is Red");
            }
            else
            {
                ReportingMethods.Log(test, "Delinquent Payment Count: " + delinquentPaymentCount + " is not matched the conditions");
            }
        }

        /// <summary>
        /// Method to  Verify Banner On-Time Details
        /// </summary>
        /// <param name="loanLevelData">Loan level details from DB</param>
        /// <param name="planType">Active/Broken/Deleted</param>
        /// <param name="type">Payment flow - setup</param>
        public void VerifyBannerOnTimeDateDetails(Hashtable loanLevelData, string planType, string type = "setup")
        {
            string nextDueDate = _driver.FindElement(nextDueBannerTextLocBy).Text.Trim();
            nextDueDate = nextDueDate.Replace("Next Due Date:", "").Trim();
            ReportingMethods.LogAssertionEqual(test, Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).ToString("MMM d, yyyy"), nextDueDate, type == "setup" ? "Verify " + planType + " Next Due in OTP Screen" : "Verify " + planType + " " + type + " Next Due in OTP Screen");
            if (planType == "Active" || planType == "Completed")
            {
                var activeForbearance = _driver.FindElement(paymentHasBeeenMadeTextLocBy).Text.Trim();
                string Month = Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).AddMonths(-1).ToString("MMMM");
                ReportingMethods.LogAssertionContains(test, "Payment has been made for " + Month, activeForbearance.Trim(), type == "setup" ? "Verify OTP screen " + planType + "  Plan Next Due Date details" : "Verify " + planType + " " + type + " OTP screen Active Plan Next Due Date details");
            }
        }

        /// <summary>
        /// Method to Verify Fields of Completed Active Repayment Plan
        /// </summary>
        /// <param name="additionalPrincipalParam">Addtional Principal Amount</param>
        /// <param name="additionalEscrowAmountParam">Addtional Escrow Amount</param>
        /// <param name="additionalPrincipalEditParam">Addtional Principal Edit Amount</param>
        /// <param name="additionalEscrowEditAmountParam">Addtional Escrow Edit Amount</param>
        /// <param name="type">Payment flow Type -setup/Edit</param>
        /// <returns>total payment</returns>
        public string VerifyCompletedActiveRepaymentPaymentFields(Hashtable loanLevelData, double additionalPrincipalParam, double additionalEscrowAmountParam, double additionalPrincipalEditParam = 0.0, double additionalEscrowEditAmountParam = 0.0, string type = "setup")
        {
            bool radiobuttonStatus;
            double totalPayment = 0.00;
            double additionalPrincipalAmount = 0.00;
            double pastDueAmount = 0.00;
            double currentPaymentAmount = 0.00;
            double intialTotalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            ReportingMethods.Log(test, "Intial Total Payment is" + " " + "$" + intialTotalPayment);

            if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) > 1)
            {
                radiobuttonStatus = IsRadiobuttonSelected(pastDueAmountRadioButtonLocBy);
                if (radiobuttonStatus == false)
                {
                    ReportingMethods.Log(test, "Verify initially Past Due Amount Radio Button is not selected");
                    webElementExtensions.ScrollIntoView(_driver, pastDueAmountRadioButtonInputLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, pastDueAmountRadioButtonInputLocBy, "Repayment Plan radio button");
                    pastDueAmount = Convert.ToDouble(webElementExtensions.GetElementText(_driver, pastDueRadioButtonAmountTextLocBy).Replace("$", ""));
                    if (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.AccruedLateChargeAmount]) > 0 || Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.RecoverableCorpAdvBalance]) > 0 || Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance]) > 0)
                    {
                        webElementExtensions.IsElementDisabled(_driver, additionalPrincipalTextboxLocBy, "Intially Additional Principal TextBox");
                    }
                }
            }

            totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            double lateFee = 0.00;
            double otherFee = 0.00;
            if (_driver.FindElements(lateFeeAmountCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
            {
                webElementExtensions.ActionClick(_driver, lateFeeAmountCheckboxLocBy, "Late Fee Checkbox");
                lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.LogAssertionEqual(test, "$" + (intialTotalPayment + pastDueAmount + lateFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with LateFee calculation");
            }
            if (_driver.FindElements(otherFeeAmountCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
            {
                webElementExtensions.ActionClick(_driver, otherFeeAmountCheckboxLocBy, "Other Fee Checkbox");
                otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.LogAssertionEqual(test, "$" + (intialTotalPayment + pastDueAmount + lateFee + otherFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with OtherFee calculation");
            }

            if (webElementExtensions.GetElementAttribute(_driver, addPaymentCheckBoxLocBy, Constants.ElementAttributes.AriaChecked) == "false")
            {
                webElementExtensions.ClickElementUsingJavascript(_driver, addPaymentCheckBoxLocBy, "Additional Payment (Principal Only, Escrow, etc.) CheckBox");
            }

            webElementExtensions.IsElementEnabled(_driver, additionalPrincipalTextboxLocBy, "Additional Principal TextBox");

            if (_driver.FindElements(additionalPrincipalTextboxLocBy).Count > 0)
            {
                if (_driver.FindElement(additionalPrincipalTextboxLocBy).Enabled)
                {
                    if (type == "setup")
                    {
                        webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalParam * 100).ToString());
                        ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalParam));
                    }
                    else
                    {
                        webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalEditParam * 100).ToString());
                        ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalEditParam));
                    }
                    var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    additionalPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
                    if (type.ToLower() == "edit")
                    {
                        additionalPrincipalAmount = additionalPrincipalEditParam - additionalPrincipalParam;
                    }
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + pastDueAmount + lateFee + otherFee + additionalPrincipalAmount + currentPaymentAmount).ToString("N"), $"Verify Total Payment with Additional Principal calculation");
                }
            }
            double escrowAmount = 0.00;
            if (_driver.FindElements(additionalEscrowTextboxLocBy).Count > 0)
            {
                if (_driver.FindElement(additionalEscrowTextboxLocBy).Enabled)
                {
                    if (type == "setup")
                    {
                        webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowAmountParam * 100).ToString());
                        ReportingMethods.Log(test, "Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowAmountParam));
                    }
                    else
                    {
                        webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowEditAmountParam * 100).ToString());
                        ReportingMethods.Log(test, "Edit Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowEditAmountParam));
                    }

                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Additional Escrow Amount Entered: " + "$" + totalPayment.ToString());
                    var addEscrowAmount = commonServices.GetValueUsingJS(additionalEscrowTextboxLocBy);
                    escrowAmount = Convert.ToDouble(addEscrowAmount.ToString().Trim().Replace("$", ""));
                    if (type.ToLower() == "edit")
                    {
                        escrowAmount = additionalEscrowEditAmountParam - additionalEscrowAmountParam;
                    }
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + pastDueAmount + lateFee + otherFee + additionalPrincipalAmount + currentPaymentAmount + escrowAmount).ToString("N"), $"Verify Total Payment with Additional Principal and Escrow calculation");
                }
            }
            return "$" + totalPayment.ToString("N");
        }

        #endregion Repayment Services

        #region OTP Services

        /// <summary>
        /// Method to Verify Use Suspense Balance Is Negative
        /// </summary>
        /// <param name="type">Payment flow - Edit/Update</param>
        public void VerifyUseSuspenseBalanceIsNegative(string type = "setup")
        {
            bool flag = false;
            type = type.ToLower().Trim() == "setup" ? "" : type;
            string text = webElementExtensions.GetElementText(_driver, useSuspenseBalanceAmountLocBy);
            text = text != null || text != "" ? text : "0.0";
            if (Convert.ToDouble(text.Replace("$", "")) < 0.0)
            {
                flag = true;
            }
            ReportingMethods.LogAssertionTrue(test, flag, type == "" ? "Verify Use Suspense Balance Is Negative : " + text : "Verify Edit Use Suspense Balance Is Negative : " + text);
        }


        /// <summary>
        /// Verifies the calculation of the suspense amount based on the total payment amount and type.
        /// </summary>
        /// <param name="loanLevelData">Loan level details from DB</param>
        /// <param name="totalPaymentAmountt">The total payment amount as a string. Default is "$ 0.0".</param>
        /// <param name="type">The type of the operation. Default is "setup".</param>
        /// <returns>The calculated total payment amount as a formatted string.</returns>
        public string VerifySuspenseAmountCalculation(Hashtable loanLevelData, string totalPaymentAmountt = "$ 0.0", double addSuspenseValue = 0.0, string type = "setup")
        {
            double totalPayment = Convert.ToDouble(totalPaymentAmountt.Replace("$", "").Trim()), intialTotalPayment = 0.0, useSuspenseBalanceAmount = 0.0; bool flag = false;
            type = type.ToLower().Trim() == "setup" ? "" : type;
            if (_driver.FindElements(prePaymentCheckBoxLocBy).Count > 0)
            {
                webElementExtensions.ScrollIntoView(_driver, prePaymentCheckBoxLocBy);
                flag = Convert.ToBoolean(webElementExtensions.GetElementAttribute(_driver, prePaymentCheckBoxLocBy, Constants.ElementAttributes.AriaChecked));
                if (!flag)
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, prePaymentCheckBoxLocBy, "Prepay Checkbox");
                }
            }

            intialTotalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            ReportingMethods.Log(test, "Initial Total Payment : " + "$" + intialTotalPayment.ToString("N"));

            webElementExtensions.ScrollIntoView(_driver, suspensePaymentCheckboxLocBy);
            flag = _driver.FindElements(suspensePaymentCheckboxLocBy).Count > 0 ? true : false;
            _driver.ReportResult(test, flag,
                "Successfully verified that 'Suspense Payment' checkbox is displayed ",
                "Failure - 'Suspense Payment' checkbox is not displayed");
            webElementExtensions.ScrollIntoView(_driver, suspensePaymentCheckboxLocBy);
            webElementExtensions.ClickElement(_driver, suspensePaymentCheckboxLocBy, "Suspense Payment checkbox", true);

            if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.SuspenseBalance]) == 0)
            {
                flag = _driver.FindElements(addToSuspenseBalanceTextboxLocBy).Count > 0 ? true : false;
                _driver.ReportResult(test, flag, "Successfully verified that 'Add to Suspense Balance' Textbox is displayed", "Failure - 'Add to Suspense Balance' Textbox is not displayed");
                if (flag)
                {
                    if (type.ToLower().Trim() == "edit")
                    {
                        addSuspenseValue = addSuspenseValue + 5;
                    }
                    webElementExtensions.EnterText(_driver, addToSuspenseBalanceTextboxLocBy, (addSuspenseValue * 100).ToString());
                    ReportingMethods.Log(test, type + " Add to Suspense Balance Entered: " + "$" + Convert.ToDouble(addSuspenseValue));
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    if (type.ToLower().Trim() == "edit")
                    {
                        addSuspenseValue = 5;
                    }
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + addSuspenseValue).ToString("N"), $"Verify " + type.ToLower() == "setup" ? " " : type + "  Total Payment with add to Suspense Balance Entered calculation");
                }
            }
            if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.SuspenseBalance]) > 0)
            {
                flag = _driver.FindElements(useSuspenseBalanceCheckboxLocBy).Count > 0 ? true : false;
                _driver.ReportResult(test, flag, "Successfully verified that 'Use Suspense Balance' Checkbox is displayed", "Failure - 'Use Suspense Balance' Checkbox is not displayed");
                VerifyUseSuspenseBalanceIsNegative();
                webElementExtensions.ClickElementUsingJavascript(_driver, useSuspenseBalanceCheckboxLocBy, "Use Suspense Balance checkbox", true);
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                useSuspenseBalanceAmount = Convert.ToDouble(webElementExtensions.GetElementText(_driver, useSuspenseBalanceAmountLocBy).Replace("$", ""));
                ReportingMethods.LogAssertionEqual(test, "$" + (intialTotalPayment + useSuspenseBalanceAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify " + type.ToLower() == "setup" ? " " : type + "  Total Payment with Suspense Balance calculation");
            }
            return "$" + totalPayment.ToString("N");
        }

        /// <summary>
        /// Method to Verify Payment Fields
        /// </summary>
        /// <param name="additionalPrincipalParam">Addtional Principal Amount</param>
        /// <param name="additionalEscrowAmountParam">Addtional Escrow Amount</param>
        /// <param name="additionalPrincipalEditParam">Addtional Principal Edit Amount</param>
        /// <param name="additionalEscrowEditAmountParam">Addtional Escrow Edit Amount</param>
        /// <param name="type">Payment flow Type -setup/Edit</param>
        /// <returns>total payment</returns>
        public string VerifyOTPPaymentFields(Hashtable loanLevelData, double additionalPrincipalParam, double additionalEscrowAmountParam, double additionalPrincipalEditParam = 0.0, double additionalEscrowEditAmountParam = 0.0, string type = "setup")
        {
            bool radiobuttonStatus, flag;
            double totalPayment = 0.00;
            double additionalPrincipalAmount = 0.00;
            double pastDueAmount = 0.00, prepaidAmount = 0.00;
            double intialTotalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            ReportingMethods.Log(test, "Intial Total Payment is" + " " + "$" + intialTotalPayment);

            if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) > 1)
            {
                webElementExtensions.ScrollToTop(_driver);
                radiobuttonStatus = IsRadiobuttonSelected(pastDueAmountRadioButtonLocBy);
                if (radiobuttonStatus == false)
                {
                    ReportingMethods.LogAssertionFalse(test, radiobuttonStatus, "Verify Past Due Amount field Radio Button is not selected Initially");
                    webElementExtensions.WaitUntilElementIsClickable(_driver, pastDueAmountRadioButtonLocBy);
                    webElementExtensions.ClickElement(_driver, pastDueAmountRadioButtonLocBy, "Past Due radio button");
                    pastDueAmount = Convert.ToDouble(webElementExtensions.GetElementText(_driver, pastDueRadioButtonAmountTextLocBy).Replace("$", ""));
                }
                else { ReportingMethods.LogAssertionTrue(test, radiobuttonStatus, "Verify Past Due Amount field Radio Button is selected Initially"); }

                if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) < 3 && type.ToLower() == "setup")
                {
                    webElementExtensions.IsElementDisabled(_driver, additionalPrincipalTextboxLocBy, "Intially Additional Principal TextBox");
                }
            }

            if (_driver.FindElements(prepayCheckboxLocBy).Count > 0)//Prepaid
            {
                flag = Convert.ToBoolean(webElementExtensions.GetElementAttribute(_driver, prepayCheckboxLocBy, Constants.ElementAttributes.AriaChecked));
                if (flag == false)
                {
                    ReportingMethods.LogAssertionFalse(test, flag, "Verify initially Prepay Check Box is not checked");
                    webElementExtensions.ScrollIntoView(_driver, prepayCheckboxLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, prepayCheckboxLocBy, "Prepay Check Box");
                    prepaidAmount = Convert.ToDouble(webElementExtensions.GetElementText(_driver, prePaymentCheckBoxAmountLocBy).Replace("$", ""));
                }
            }

            totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            double lateFee = 0.00, otherFee = 0.00, nSFFee = 0.0;
            if (_driver.FindElements(lateFeeAmountCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
            {
                webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, lateFeeAmountCheckboxLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, lateFeeAmountCheckboxLocBy, "Late Fee Checkbox");
                webElementExtensions.WaitForVisibilityOfElement(_driver, lateFeeAmountCheckboxCheckedLocBy);
                lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.LogAssertionEqual(test, "$" + (intialTotalPayment + pastDueAmount + lateFee + prepaidAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with LateFee calculation");
            }
            if (_driver.FindElements(otherFeeAmountCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
            {
                webElementExtensions.ActionClick(_driver, otherFeeAmountCheckboxLocBy, "Other Fee Checkbox");
                otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.LogAssertionEqual(test, "$" + (intialTotalPayment + pastDueAmount + lateFee + otherFee + prepaidAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with OtherFee calculation");
            }
            if (_driver.FindElements(nSFFeeAmountCheckboxLocBy).Count > 0 && type.ToLower() == "setup")
            {
                webElementExtensions.ClickElementUsingJavascript(_driver, nSFFeeAmountCheckboxLocBy, "NSF Fee Checkbox");
                nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", ""));
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.LogAssertionEqual(test, "$" + (intialTotalPayment + pastDueAmount + lateFee + otherFee + nSFFee + prepaidAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with NSFFee calculation");
            }

            if (webElementExtensions.GetElementAttribute(_driver, addPaymentCheckBoxLocBy, Constants.ElementAttributes.AriaChecked) == "false")
            {
                webElementExtensions.ClickElementUsingJavascript(_driver, addPaymentCheckBoxLocBy, "Additional Payment (Principal Only, Escrow, etc.) CheckBox");
            }

            webElementExtensions.IsElementEnabled(_driver, additionalPrincipalTextboxLocBy, "Additional Principal TextBox");

            if (_driver.FindElements(additionalPrincipalTextboxLocBy).Count > 0)
            {
                webElementExtensions.ScrollIntoView(_driver, additionalPrincipalTextboxLocBy);
                if (_driver.FindElement(additionalPrincipalTextboxLocBy).Enabled)
                {
                    if (type == "setup")
                    {
                        webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalParam * 100).ToString());
                        ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalParam));
                    }
                    else
                    {
                        webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalEditParam * 100).ToString());
                        ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalEditParam));
                    }
                    var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    additionalPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
                    if (type.ToLower() == "edit")
                    {
                        additionalPrincipalAmount = additionalPrincipalEditParam - additionalPrincipalParam;
                    }
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + pastDueAmount + lateFee + otherFee + nSFFee + prepaidAmount + additionalPrincipalAmount).ToString("N"), $"Verify Total Payment with Additional Principal calculation");
                }
            }
            double escrowAmount = 0.00;
            if (_driver.FindElements(additionalEscrowTextboxLocBy).Count > 0)
            {
                if (_driver.FindElement(additionalEscrowTextboxLocBy).Enabled)
                {
                    if (type == "setup")
                    {
                        webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowAmountParam * 100).ToString());
                        ReportingMethods.Log(test, "Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowAmountParam));
                    }
                    else
                    {
                        webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowEditAmountParam * 100).ToString());
                        ReportingMethods.Log(test, "Edit Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowEditAmountParam));
                    }

                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Additional Escrow Amount Entered: " + "$" + totalPayment.ToString());
                    var addEscrowAmount = commonServices.GetValueUsingJS(additionalEscrowTextboxLocBy);
                    escrowAmount = Convert.ToDouble(addEscrowAmount.ToString().Trim().Replace("$", ""));
                    if (type.ToLower() == "edit")
                    {
                        escrowAmount = additionalEscrowEditAmountParam - additionalEscrowAmountParam;
                    }
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + pastDueAmount + lateFee + otherFee + nSFFee + prepaidAmount + additionalPrincipalAmount + escrowAmount).ToString("N"), $"Verify Total Payment with Additional Principal and Escrow calculation");
                }
            }
            return "$" + totalPayment.ToString("N");
        }

        /// <summary>
        /// Method to Make Additional Escrow Payment Only
        /// </summary>
        /// <param name="loanType">Loan Type - On-Time, Past Due, etc</param>
        /// <param name="additionalEscrowAmountParam">Addtional Escrow Amount</param>
        /// <param name="additionalEscrowEditAmountParam">Addtional Escrow Edit Amount</param>
        /// <param name="type">Payment flow Type -setup/Edit</param>
        /// <returns>total payment</returns>
        public string MakeAdditionalEscrowPaymentOnly(string loanType, double additionalEscrowAmountParam, double additionalEscrowEditAmountParam = 0.0, string type = "setup")
        {
            double totalPayment = 0.00, escrowAmount = 0.00;
            type = type.ToLower() == "setup" ? "" : type;
            double intialTotalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            ReportingMethods.Log(test, type + " Intial Total Payment is" + " " + "$" + intialTotalPayment);
            if (loanType == "On-time")
            {
                if (webElementExtensions.GetElementAttribute(_driver, currentPaymentCheckboxLocBy, Constants.ElementAttributes.AriaChecked) == "true")
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, currentPaymentCheckboxLocBy, "Current Payment CheckBox");
                }
                totalPayment = intialTotalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.Log(test, type + " Total Payment After Current Payment CheckBox unchecked " + "$" + totalPayment.ToString());
            }
            else if (loanType == "Past Due")
            {
                if (webElementExtensions.GetElementAttribute(_driver, monthlyPaymentCheckboxLocBy, Constants.ElementAttributes.AriaChecked) == "true")
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, monthlyPaymentCheckboxLocBy, "Monthly Payment (Past Due)");
                    ReportingMethods.LogAssertionTrue(test, _driver.FindElement(addPaymentCheckBoxLocBy).Enabled, type + " Verify Additional payment checkbox is enabled when the Monthly Payment (Past Due) checkbox is unchecked");
                }
                totalPayment = intialTotalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.Log(test, type + " Total Payment After Monthly Payment (Past Due) CheckBox unchecked " + "$" + totalPayment.ToString());
            }

            if (webElementExtensions.GetElementAttribute(_driver, addPaymentCheckBoxLocBy, Constants.ElementAttributes.AriaChecked) == "false")
            {
                webElementExtensions.ClickElementUsingJavascript(_driver, addPaymentCheckBoxLocBy, "Additional Payment (Principal Only, Escrow, etc.) CheckBox");
            }
            if (_driver.FindElement(additionalEscrowTextboxLocBy).Enabled)
            {
                if (type.ToLower() != "edit")
                {
                    webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowAmountParam * 100).ToString());
                    ReportingMethods.Log(test, "Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowAmountParam));
                }
                else
                {
                    webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowEditAmountParam * 100).ToString());
                    ReportingMethods.Log(test, "Edit Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowEditAmountParam));
                }

                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.Log(test, type + " Total Payment After Additional Escrow Amount Entered: " + "$" + totalPayment.ToString());
                var addEscrowAmount = commonServices.GetValueUsingJS(additionalEscrowTextboxLocBy);
                escrowAmount = Convert.ToDouble(addEscrowAmount.ToString().Trim().Replace("$", ""));
                if (type.ToLower() == "edit")
                {
                    escrowAmount = additionalEscrowEditAmountParam - additionalEscrowAmountParam;
                }
                ReportingMethods.LogAssertionEqual(test, "$" + (intialTotalPayment + escrowAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify " + type + " Total Payment with Escrow calculation");
            }
            totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            return "$" + totalPayment.ToString("N");
        }

        /// <summary>
        /// Method to verify Indicator message
        /// </summary>
        /// <param name="loanLevelData">Loan Level Data from DB</param>
        /// <param name="stopCodeType">Stop Code Type - Soft, Hard and Bad check stop code</param>
        public void VerifyIndicatorMessage(Hashtable loanLevelData, string stopCodeType)
        {
            List<string> actualIndicatorMsgList;
            string expectedIndicatorMsg = "";
            if (stopCodeType.ToLower().Trim() == "soft")
            {
                webElementExtensions.WaitForElement(_driver, softProcessStopsIndicatorMsgLocBy);
                webElementExtensions.ScrollIntoView(_driver, softProcessStopsIndicatorMsgLocBy);
                actualIndicatorMsgList = _driver.FindElements(softProcessStopsIndicatorMsgLocBy).Select(x => x.Text.Trim()).ToList();
            }
            else// Hard Or Bad Check
            {
                webElementExtensions.WaitForElement(_driver, badCheckStopsIndicatorMsgLocBy);
                webElementExtensions.ScrollIntoView(_driver, badCheckStopsIndicatorMsgLocBy);
                actualIndicatorMsgList = _driver.FindElements(badCheckStopsIndicatorMsgLocBy).Select(x => x.Text.Trim()).ToList();
            }

            if (loanLevelData[Constants.LoanLevelDataColumns.BadCheckStopCode].ToString().ToUpper() == "1")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.BadCheckStopCodeWhen1;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.BadCheckStopCode].ToString().ToUpper() == "4")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.BadCheckStopCodeWhen4;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.BadCheckStopCode].ToString().ToUpper() == "6")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.BadCheckStopCodeWhen6;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "3")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.SoftProcessStopCodeWhen3;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "B")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.SoftProcessStopCodeWhenB;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "H")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.SoftProcessStopCodeWhenH;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "L")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.SoftProcessStopCodeWhenL;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "N")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.SoftProcessStopCodeWhenN;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "2")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.HardProcessStopCodeWhen2;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "8")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.HardProcessStopCodeWhen8;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "A")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.HardProcessStopCodeWhenA;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "F")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.HardProcessStopCodeWhenF;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "M")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.HardProcessStopCodeWhenM;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "P")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.HardProcessStopCodeWhenP;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "R")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.HardProcessStopCodeWhenR;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "U")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.HardProcessStopCodeWhenU;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "W")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.HardProcessStopCodeWhenW;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "!")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.HardProcessStopCodeWhenExclamatory;
            }
            else if (loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper() == "9")
            {
                expectedIndicatorMsg = Constants.IndicatorMessage.HardProcessStopCodeWhen9;
            }

            ReportingMethods.LogAssertionListContains(test, new List<string> { expectedIndicatorMsg.Trim() }, actualIndicatorMsgList, "Verify Indicator Message when Stop Code is " + loanLevelData[Constants.LoanLevelDataColumns.ProcessStopCode].ToString().ToUpper());
        }

        /// <summary>
        /// Method to Verify Payment Fields and Fee Validations
        /// </summary>
        /// <param name="loanLevelData">Loan Level Data from DB</param>
        /// <param name="additionalPrincipalParam">Addtional Principal Amount</param>
        /// <param name="additionalEscrowAmountParam">Addtional Escrow Amount</param>
        /// <param name="additionalPrincipalEditParam">Addtional Principal Edit Amount</param>
        /// <param name="additionalEscrowEditAmountParam">Addtional Escrow Edit Amount</param>
        /// <param name="type">Payment flow Type -setup/Edit</param>
        /// <returns>total payment</returns>
        public string VerifyOTPPaymentFieldsAndFeeValidations(Hashtable loanLevelData, double additionalPrincipalParam, double additionalEscrowAmountParam, double additionalPrincipalEditParam = 0.0, double additionalEscrowEditAmountParam = 0.0, string type = "setup")
        {
            type = type.ToLower() == "setup" ? "" : type; bool radiobuttonStatus;
            double totalPayment = 0.00, additionalPrincipalAmount = 0.00, additionalEscrowAmount = 0.00, NSFFee = 0.00, lateFee = 0.00, otherFee = 0.00, pastDueAmount = 0.00, currentPaymentAmount = 0.00, monthlyPaymentPastDueAmount = 0.00, setupFeesAmount = 0.0;

            double intialTotalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            ReportingMethods.Log(test, "Intial Total Payment is" + " " + "$" + intialTotalPayment);

            if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 0)
            {
                currentPaymentAmount = Convert.ToDouble(GetAmountText(currentPaymentAmountLocBy).Replace("$", "").Trim());
            }
            else if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 1)
            {
                monthlyPaymentPastDueAmount = Convert.ToDouble(GetAmountText(monthlyPaymentAmountTextLocBy).Replace("$", "").Trim());
            }

            else if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) > 1)
            {

                By pastDueAmountRadioButtonLocBy = By.XPath(radioButtonByText.Replace("<TEXT>", "Past Due Amount"));
                webElementExtensions.ScrollIntoView(_driver, pastDueAmountRadioButtonLocBy);
                pastDueAmount = Convert.ToDouble(webElementExtensions.GetElementText(_driver, pastDueRadioButtonAmountTextLocBy).Replace("$", ""));
                radiobuttonStatus = IsRadiobuttonSelected(pastDueAmountRadioButtonLocBy);
                if (radiobuttonStatus == false)
                {
                    ReportingMethods.LogAssertionFalse(test, radiobuttonStatus, "Verify Past Due Amount field Radio Button is not selected Initially");
                    webElementExtensions.ClickElementUsingJavascript(_driver, pastDueRadioButtonLocBy, "Past Due radio button");
                }
                else
                {
                    ReportingMethods.LogAssertionTrue(test, radiobuttonStatus, "Verify Past Due Amount field Radio Button is selected Initially");
                }

                if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) < 3 && type.ToLower() != "edit")
                {
                    webElementExtensions.IsElementDisabled(_driver, additionalPrincipalTextboxLocBy, "Intially Additional Principal TextBox");
                }
            }
            if (webElementExtensions.GetElementAttribute(_driver, addPaymentCheckBoxLocBy, Constants.ElementAttributes.AriaChecked) == "false" && type.ToLower() != "edit")
            {
                webElementExtensions.WaitForElement(_driver, addPaymentCheckBoxLocBy);
                webElementExtensions.ScrollIntoView(_driver, addPaymentCheckBoxLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, addPaymentCheckBoxLocBy, "Additional Payment (Principal Only, Escrow, etc.) CheckBox");
            }

            Hashtable feeLevelData = commonServices.GetFeesWithLoanNumber(loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString()).FirstOrDefault();
            By feesErrorLocBy = By.XPath(paraByText.Replace("<TEXT>", Constants.ErrorMessages.PleaseEnterAnAmountEqualToOrLessThanTotalFeesDue));
            if (type.ToLower() == "edit")
            {
                if (_driver.FindElements(additionalPrincipalTextboxLocBy).Count > 0)
                {
                    additionalPrincipalAmount = Convert.ToDouble(commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy).ToString().Trim().Replace("$", ""));
                }
                if (_driver.FindElements(additionalEscrowTextboxLocBy).Count > 0)
                {
                    additionalEscrowAmount = Convert.ToDouble(commonServices.GetValueUsingJS(additionalEscrowTextboxLocBy).ToString().Trim().Replace("$", ""));
                }
                if (Convert.ToDouble(feeLevelData[Constants.FeesDataColumns.NsfFeeBalance]) > 0)
                {
                    NSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", "").Trim());
                }
                if (Convert.ToDouble(feeLevelData[Constants.FeesDataColumns.AccruedLateChargeAmount]) > 0)
                {
                    lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                }
                if (Convert.ToDouble(feeLevelData[Constants.FeesDataColumns.OtherFees]) > 0)
                {
                    otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                }
            }

            if (Convert.ToDouble(feeLevelData[Constants.FeesDataColumns.OtherFees]) > 0)
            {

                if (type.ToLower() != "edit")
                {
                    webElementExtensions.IsElementDisabled(_driver, additionalPrincipalTextboxLocBy, "Additional Principal TextBox when Other Fees Checkebox is not checked");
                    webElementExtensions.ScrollIntoView(_driver, otherFeeAmountCheckboxLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, otherFeeAmountCheckboxLocBy, "Other Fees Checkbox");
                }

                if (webElementExtensions.GetElementAttribute(_driver, otherFeeAmountTextboxLocBy, Constants.ElementAttributes.ReadOnly) == "true")
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, otherFeesIconLocBy, "Other Fee PopUp Icon");
                    double initialValue = 0.0;
                    webElementExtensions.WaitForVisibilityOfElement(_driver, otherFeesPopUpTextboxesLocBy);
                    var otherFeesTextboxes = _driver.FindElements(otherFeesPopUpTextboxesLocBy);
                    foreach (var otherFeesTextbox in otherFeesTextboxes)
                    {
                        string[] hintMsgArray = otherFeesTextbox.FindElement(otherFeesPopUpTextboxHintLocBy).Text.Split(':');
                        initialValue = Convert.ToDouble(hintMsgArray[1].Split('$')[1].Trim());
                        string otherFeeLabel = otherFeesTextbox.FindElement(otherFeesPopUpTextboxLabelLocBy).Text;
                        webElementExtensions.EnterText(_driver, otherFeesTextbox, ((initialValue + 1) * 100).ToString());
                        ReportingMethods.Log(test, "Other Fee PopUp Amount Entered: " + "$" + Convert.ToDouble(initialValue + 1));
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                        feesErrorLocBy = By.XPath(spanContainsByText.Replace("<TEXT>", Constants.ErrorMessages.PleaseEnterAnAmountEqualToOrLessThanTotalFeesDue));
                        webElementExtensions.WaitForVisibilityOfElement(_driver, feesErrorLocBy);
                        bool flag = webElementExtensions.IsElementDisplayed(_driver, feesErrorLocBy);
                        ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, feesErrorLocBy), "Verify PopUp 'Please enter an amount equal to or less than the total fees due.' message is displayed");
                        VerifyApplyButtonIsDisabled();

                        ReportingMethods.Log(test, "Enter Other Fee PopUp " + otherFeeLabel + " : " + "$" + Convert.ToDouble(1));
                        webElementExtensions.EnterText(_driver, otherFeesTextbox, (1 * 100).ToString());
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                        reportLogger.TakeScreenshot(test, "Other Fees Popup Hint Screenshot");
                        ReportingMethods.LogAssertionEqual(test, "Remaining " + otherFeeLabel + " Due: $" + Convert.ToDouble(initialValue - 1).ToString("N"), otherFeesTextbox.FindElement(otherFeesPopUpTextboxHintLocBy).Text.Trim(), "Verifying Other Fee PopUp Amount Hint");

                        webElementExtensions.EnterText(_driver, otherFeesTextbox, (initialValue * 100).ToString());
                        ReportingMethods.Log(test, "Other Fees PopUp Amount Entered: " + "$" + initialValue);
                        ReportingMethods.LogAssertionTrue(test, !webElementExtensions.IsElementDisplayed(_driver, feesErrorLocBy), "Verify PopUp 'Please enter an amount equal to or less than the total fees due.' message is not displayed");
                        VerifyApplyButtonIsEnabled();
                    }
                    ClickButtonUsingName(Constants.ButtonNames.Apply);
                    otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(feeLevelData[Constants.FeesDataColumns.OtherFees]).ToString("N")}", "$" + otherFee.ToString("N"), "Verify Other Fees value");

                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    if (type.ToLower() != "edit")
                    {
                        ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + monthlyPaymentPastDueAmount + pastDueAmount + NSFFee + lateFee + otherFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with OtherFee calculation");

                    }
                    else
                    {
                        ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + monthlyPaymentPastDueAmount + pastDueAmount + NSFFee + lateFee + otherFee + additionalPrincipalAmount + additionalEscrowAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with Edited OtherFee calculation");
                    }
                }
                else
                {
                    double otherAmount = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", "").Trim());
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(feeLevelData[Constants.FeesDataColumns.OtherFees]).ToString("N")}", "$" + otherFee.ToString("N"), "Verify Other Fees value");

                    webElementExtensions.EnterText(_driver, otherFeeAmountTextboxLocBy, ((otherAmount + 1) * 100).ToString());
                    ReportingMethods.Log(test, "Other Fee Amount Entered: " + "$" + Convert.ToDouble(otherAmount + 1));
                    By errorLocBy = By.XPath(spanByText.Replace("<TEXT>", Constants.ErrorMessages.PleaseEnterAnAmountEqualToOrLessThanTotalFeesDue));
                    ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, errorLocBy), "Verify 'Please enter an amount equal to or less than the total fees due.' message is displayed");
                    if (type.ToLower() == "edit") { VerifyUpdatePaymentButtonIsDisabled(); }
                    else { VerifyMakeAPaymentButtonIsDisabled(); }

                    webElementExtensions.EnterText(_driver, otherFeeAmountTextboxLocBy, (1 * 100).ToString());
                    ReportingMethods.Log(test, "Other Fee Amount Entered: " + "$" + Convert.ToDouble(1));
                    ReportingMethods.LogAssertionEqual(test, Constants.FeeHints.RemainingOtherFeesDue + Convert.ToDouble(otherAmount - 1).ToString("N"), webElementExtensions.GetElementText(_driver, otherFeeAmountTextboxHintLocBy).Trim(), "Verifying Other Fee Text box Hint");
                    reportLogger.TakeScreenshot(test, "Other Fees Hint Screenshot");

                    webElementExtensions.EnterText(_driver, otherFeeAmountTextboxLocBy, (otherAmount * 100).ToString());
                    ReportingMethods.Log(test, "Other Fees Amount Entered: " + "$" + Convert.ToDouble(otherAmount));
                    ReportingMethods.LogAssertionTrue(test, !webElementExtensions.IsElementDisplayed(_driver, errorLocBy), "Verify 'Please enter an amount equal to or less than the total fees due.' message is not displayed");
                    if (type.ToLower() == "edit") { VerifyUpdatePaymentButtonIsEnabled(); }
                    else { VerifyMakeAPaymentButtonIsEnabled(); }
                    otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    if (type.ToLower() != "edit")
                    {
                        ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + monthlyPaymentPastDueAmount + NSFFee + lateFee + otherFee + pastDueAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with OtherFee calculation");
                    }
                    else
                    {
                        ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + monthlyPaymentPastDueAmount + pastDueAmount + NSFFee + lateFee + otherFee + additionalEscrowAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with Edited OtherFee calculation");
                    }
                }
            }
            if (Convert.ToDouble(feeLevelData[Constants.FeesDataColumns.NsfFeeBalance]) > 0)
            {
                if (type.ToLower() != "edit")
                {
                    webElementExtensions.IsElementDisabled(_driver, additionalPrincipalTextboxLocBy, "Additional Principal TextBox when NSF Fee Checkebox is not checked");
                    webElementExtensions.ScrollIntoView(_driver, nSFFeeAmountCheckboxLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, nSFFeeAmountCheckboxLocBy, "NSF Fee Checkbox");
                }
                double nSFAmount = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", "").Trim());
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(feeLevelData[Constants.FeesDataColumns.NsfFeeBalance]).ToString("N")}", "$" + nSFAmount.ToString("N"), "Verify NSF Fees value");

                webElementExtensions.EnterText(_driver, nSFFeeTextboxLocBy, ((nSFAmount + 1) * 100).ToString());
                ReportingMethods.Log(test, "NSF Fee Amount Entered: " + "$" + Convert.ToDouble(nSFAmount + 1));
                feesErrorLocBy = By.XPath(paraByText.Replace("<TEXT>", Constants.ErrorMessages.PleaseEnterAnAmountEqualToOrLessThanTotalFeesDue));
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, feesErrorLocBy), "Verify 'Please enter an amount equal to or less than the total fees due.' message is displayed");
                if (type.ToLower() == "edit") { VerifyUpdatePaymentButtonIsDisabled(); }
                else { VerifyMakeAPaymentButtonIsDisabled(); }

                webElementExtensions.EnterText(_driver, nSFFeeTextboxLocBy, (1 * 100).ToString());
                ReportingMethods.Log(test, "NSF Fee Amount Entered: " + "$" + Convert.ToDouble(1));
                ReportingMethods.LogAssertionEqual(test, Constants.FeeHints.RemainingNSFFeeDue + Convert.ToDouble(nSFAmount - 1).ToString("N"), webElementExtensions.GetElementText(_driver, nSFFeeTextboxHintLocBy).Trim(), "Verifying NSF Fee Text box Hint");
                reportLogger.TakeScreenshot(test, "NSF Fees Hint Screenshot");

                webElementExtensions.EnterText(_driver, nSFFeeTextboxLocBy, (nSFAmount * 100).ToString());
                ReportingMethods.Log(test, "NSF Fee Amount Entered: " + "$" + Convert.ToDouble(nSFAmount));
                ReportingMethods.LogAssertionTrue(test, !webElementExtensions.IsElementDisplayed(_driver, feesErrorLocBy), "Verify 'Please enter an amount equal to or less than the total fees due.' message is not displayed");
                if (type.ToLower() == "edit") { VerifyUpdatePaymentButtonIsEnabled(); }
                else { VerifyMakeAPaymentButtonIsEnabled(); }
                NSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", ""));
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                if (type.ToLower() != "edit")
                {
                    ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + monthlyPaymentPastDueAmount + pastDueAmount + NSFFee + otherFee + lateFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with NSFFee calculation");
                }
                else
                {
                    ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + pastDueAmount + monthlyPaymentPastDueAmount + NSFFee + otherFee + additionalEscrowAmount + lateFee + otherFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with Edited NSFFee calculation");
                }
            }
            if (Convert.ToDouble(feeLevelData[Constants.FeesDataColumns.AccruedLateChargeAmount]) > 0)
            {
                if (type.ToLower() != "edit")
                {
                    webElementExtensions.IsElementDisabled(_driver, additionalPrincipalTextboxLocBy, "Additional Principal TextBox when Late Fee Checkebox is not checked");
                    webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, lateFeeAmountCheckboxLocBy, "Late Fee Checkbox");
                }
                double lateAmount = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", "").Trim());
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(feeLevelData[Constants.FeesDataColumns.AccruedLateChargeAmount]).ToString("N")}", "$" + lateAmount.ToString("N"), "Verify Late Fee value");

                webElementExtensions.EnterText(_driver, lateFeeTextboxLocBy, ((lateAmount + 1) * 100).ToString());
                ReportingMethods.Log(test, "Late Fee Amount Entered: " + "$" + Convert.ToDouble(lateAmount + 1));
                feesErrorLocBy = By.XPath(paraByText.Replace("<TEXT>", Constants.ErrorMessages.PleaseEnterAnAmountEqualToOrLessThanTotalFeesDue));
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, feesErrorLocBy), "Verify 'Please enter an amount equal to or less than the total fees due.' message is displayed");
                reportLogger.TakeScreenshot(test, "Late Fees Error Screenshot");
                if (type.ToLower() == "edit") { VerifyUpdatePaymentButtonIsDisabled(); }
                else { VerifyMakeAPaymentButtonIsDisabled(); }

                webElementExtensions.EnterText(_driver, lateFeeTextboxLocBy, (1 * 100).ToString());
                ReportingMethods.Log(test, "Late Fee Amount Entered: " + "$" + Convert.ToDouble(1));
                ReportingMethods.LogAssertionEqual(test, Constants.FeeHints.RemainingLateFeeDue + Convert.ToDouble(lateAmount - 1).ToString("N"), webElementExtensions.GetElementText(_driver, lateFeeTextboxHintLocBy).Trim(), "Verifying Late Fee Text box Hint");
                reportLogger.TakeScreenshot(test, "Late Fees Hint Screenshot");

                webElementExtensions.EnterText(_driver, lateFeeTextboxLocBy, (lateAmount * 100).ToString());
                ReportingMethods.Log(test, "Late Fee Amount Entered: " + "$" + Convert.ToDouble(lateAmount));
                ReportingMethods.LogAssertionTrue(test, !webElementExtensions.IsElementDisplayed(_driver, feesErrorLocBy), "Verify 'Please enter an amount equal to or less than the total fees due.' message is not displayed");
                if (type.ToLower() == "edit") { VerifyUpdatePaymentButtonIsEnabled(); }
                else { VerifyMakeAPaymentButtonIsEnabled(); }
                lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));

                if (type.ToLower() != "edit")
                {
                    ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + monthlyPaymentPastDueAmount + pastDueAmount + NSFFee + lateFee + otherFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with LateFee calculation");
                }
                else
                {
                    ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + NSFFee + lateFee + otherFee + pastDueAmount + monthlyPaymentPastDueAmount + additionalEscrowAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with Edited LateFee calculation");
                }
            }

            if (_driver.FindElements(additionalPrincipalTextboxLocBy).Count > 0)
            {
                webElementExtensions.ScrollIntoView(_driver, additionalPrincipalTextboxLocBy);
                if (_driver.FindElement(additionalPrincipalTextboxLocBy).Enabled)
                {
                    if (type.ToLower() != "edit")
                    {
                        webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalParam * 100).ToString());
                        ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalParam));
                    }
                    else
                    {
                        webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalEditParam * 100).ToString());
                        ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalEditParam));
                    }
                    var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    additionalPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));

                    if (type.ToLower() != "edit")
                    {
                        ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + monthlyPaymentPastDueAmount + NSFFee + lateFee + otherFee + additionalPrincipalAmount + pastDueAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with Additional Principal calculation");
                    }
                    else
                    {
                        ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + NSFFee + lateFee + otherFee + additionalPrincipalAmount + pastDueAmount + additionalEscrowAmount + monthlyPaymentPastDueAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with Additional Principal calculation");
                    }
                }
            }
            if (_driver.FindElements(additionalEscrowTextboxLocBy).Count > 0)
            {
                if (_driver.FindElement(additionalEscrowTextboxLocBy).Enabled)
                {
                    if (type.ToLower() != "edit")
                    {
                        webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowAmountParam * 100).ToString());
                        ReportingMethods.Log(test, "Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowAmountParam));
                    }
                    else
                    {
                        webElementExtensions.EnterText(_driver, additionalEscrowTextboxLocBy, (additionalEscrowEditAmountParam * 100).ToString());
                        ReportingMethods.Log(test, "Edit Additional Escrow Amount Entered: " + "$" + Convert.ToDouble(additionalEscrowEditAmountParam));
                    }

                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Additional Escrow Amount Entered: " + "$" + totalPayment.ToString());
                    var addEscrowAmount = commonServices.GetValueUsingJS(additionalEscrowTextboxLocBy);
                    additionalEscrowAmount = Convert.ToDouble(addEscrowAmount.ToString().Trim().Replace("$", ""));

                    if (type.ToLower() != "edit")
                    {
                        ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + monthlyPaymentPastDueAmount + pastDueAmount + NSFFee + lateFee + otherFee + additionalPrincipalAmount + additionalEscrowAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with Additional Principal and Escrow calculation");
                    }
                    else
                    {
                        ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + pastDueAmount + monthlyPaymentPastDueAmount + NSFFee + lateFee + otherFee + additionalPrincipalAmount + additionalEscrowAmount).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with Additional Principal and Escrow calculation");
                    }

                }
            }
            return "$" + totalPayment.ToString("N");
        }

        /// <summary>
        /// Method to get the Values from Method DropDown
        /// </summary>
        /// <returns>payment list(seperated by ';')</returns>
        public string GetMethodDropDownValues()
        {
            webElementExtensions.ScrollToTop(_driver);
            webElementExtensions.WaitForVisibilityOfElement(_driver, methodDropdownLocBy, ConfigSettings.LongWaitTime);
            webElementExtensions.ClickElement(_driver, methodDropdownLocBy);
            webElementExtensions.WaitUntilElementIsClickable(_driver, methodDropDownOptionsLocBy, ConfigSettings.LongWaitTime);
            var optionElements = _driver.FindElements(methodDropDownOptionsLocBy);
            string paymentList = "";
            if (optionElements.Count > 0)
            {
                string selectOptionText = "";
                foreach (var optionElement in optionElements)
                {
                    var Text = optionElement.Text.ToString().Split('-');
                    selectOptionText = optionElement.Text.ToString();
                    string OptionText = Text[Text.Length - 1];
                    OptionText = OptionText.Replace(")", ";");
                    paymentList = paymentList + OptionText;
                }
                string newPaymentList = paymentList.Replace(";", ", ");
                newPaymentList = newPaymentList.TrimEnd(',');
                ReportingMethods.Log(test, "The Bank Account Payment Method Last Four Digits are " + newPaymentList + ".");

                var optionLocator = By.XPath(methodDropDownOptionText.Replace("<SELECTOPTIONTEXT>", selectOptionText));
                webElementExtensions.ClickElementUsingJavascript(_driver, optionLocator);
                ReportingMethods.Log(test, "The Bank Account Payment Method  selected is " + selectOptionText + ".");
            }

            return paymentList;
        }

        /// <summary>
        /// Method to Verify Bank Accounts
        /// </summary>
        /// <param name="paymentList">Payment List(seperated by ';'</param>
        /// <param name="columnName">Bank Accounts Table Column Name</param>
        public void VerifyBankAccounts(string paymentList, string columnName)
        {
            if (paymentList == "")
            {
                ReportingMethods.LogAssertionEqual(test, _driver.FindElement(bankAccountsTableEmptyTextLocBy).Text,"There are no bank accounts to display.",
                "Verify Bank Account Payment methods are displayed");
                ReportingMethods.Log(test, "No Bank Account Payment Method Last Four Digits are  displayed.");
            }
            else
            {
                int columnNum = _driver.FindElement(bankAccountsTableLocBy).FindElements(By.XPath(bankAccountsTableColumnSelector.Replace("<COLUMNNAME>", columnName))).Count + 1;
                var elements = _driver.FindElement(bankAccountsTableLocBy).FindElements(By.XPath(bankAccountsTableColumnElements.Replace("<COLUMNNUMBER>", columnNum.ToString())));
                string currentPaymentList = "";
                foreach (var element in elements)
                {
                    var Text = element.Text.ToString().Replace("*", "");
                    currentPaymentList = currentPaymentList + Text + ";";
                }
                ReportingMethods.LogAssertionEqual(test, paymentList, currentPaymentList, "verify Bank Account Payment methods are displayed");
            }
        }

        #endregion OTP Services

        #region Heloc Services

        /// <summary>
        /// Method to verify Payment Summary Details page for Heloc Loan
        /// </summary>
        /// <param name="loanLevelData">Loan Level Data from DB</param>
        public void VerifyPaymentSummaryDetailsOnPaymentPageForHeloc(Hashtable loanLevelData)
        {
            APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData);
            var headerSection = GetHeaderDetails();
            ReportingMethods.LogAssertionEqual(test, loanLevelData[Constants.LoanLevelDataColumns.BorrowerFullName].ToString().ToUpper(), headerSection["AccountHolderName"].ToUpper(), "Verify AccountHolderName field value");
            ReportingMethods.LogAssertionEqual(test, loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString().Trim(), headerSection["LoanNumber"], "Verify LoanNumber field value");

            string Address = Regex.Replace(loanLevelData[Constants.LoanLevelDataColumns.PropertyAddress].ToString().ToUpper().Trim(), @"\s+", " ") + ", " + loanLevelData[Constants.LoanLevelDataColumns.PropertyCity].ToString().ToUpper().Trim() + ", " + loanLevelData[Constants.LoanLevelDataColumns.PropertyState].ToString().ToUpper().Trim() + " " + loanLevelData[Constants.LoanLevelDataColumns.PropertyZip].ToString().Trim();
            ReportingMethods.LogAssertionEqual(test, Address.Trim(), headerSection["Address"].Trim(), "Verify Address field value");

            string unpaidprinciple = "$" + Convert.ToDouble(headerSection["Unpaid Principal Balance"].Replace("$", "")).ToString("N").Trim();
            ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.FirstPrincipalBalance]).ToString("N").Trim()}", unpaidprinciple, "Verify Unpaid Principal Balance field value");

            var lastpaymentdueSection = GetSubSectionValues("LAST PAYMENT DUE");
            ReportingMethods.LogAssertionEqual(test, loanLevelData[Constants.LoanLevelDataColumns.LastFullPaymentDate].ToString() == "" ? "-" : Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).AddMonths(-1).ToString("MM/dd/yyyy"), lastpaymentdueSection["Last Payment Due"], "Verify Last Payment Due field value");
            ReportingMethods.LogAssertionEqual(test, loanLevelData[Constants.LoanLevelDataColumns.LastFullPaymentDate].ToString() == "" ? "-" : Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.LastFullPaymentDate]).ToString("MM/dd/yyyy"), lastpaymentdueSection["Last Payment Paid On"], "Verify Last Payment Paid On field value");
            ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.LastFullPaymentAmount].ToString() == "" ? "0.00" : loanLevelData[Constants.LoanLevelDataColumns.LastFullPaymentAmount]).ToString("N")}", lastpaymentdueSection["Last Payment"], "Verify Last Payment field value");

            var accountStandingSection = GetSubSectionValues("ACCOUNT STANDING");
            var accountStanding = Regex.Replace(loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().ToLower(), @"\b\w", match => match.Value.ToUpper());
            if (accountStanding == "On-Time")
            {
                ReportingMethods.LogAssertionEqual(test, accountStanding.ToLower(), accountStandingSection["Account Standing"].ToLower(), "Verify Account Standing field value");
            }
            else
            {
                ReportingMethods.LogAssertionEqual(test, accountStanding, accountStandingSection["Account Standing"], "Verify Account Standing field value");
            }
            ReportingMethods.LogAssertionEqual(test, $"{loanLevelData[Constants.LoanLevelDataColumns.LateChargeGraceDay].ToString()} Days", accountStandingSection["Grace Days"], "Verify Last Grace Days field value");
            ReportingMethods.LogAssertionEqual(test, $"{loanLevelData[Constants.LoanLevelDataColumns.DaysPastDue].ToString()} Days".Trim(), accountStandingSection["Days Delinquent"].Trim(), "Verify Delinquent Days field value");

            DateTime currentDate = DateTime.Now;
            DateTime helocDate = commonServices.GetHelocWorkingDate(15, 2);


            if (currentDate < helocDate && Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) < 1 && Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.FirstPrincipalBalance]) != 0)
            {
                ReportingMethods.Log(test, "Current day is : " + currentDate.ToString("MM/dd/yyyy") + " is before HELOC date : " + helocDate);
                ReportingMethods.LogAssertionEqual(test, Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).ToString("MM/dd/yyyy"), headerSection["Next Payment Due"], "Verify Next Payment Due field value");
                ReportingMethods.LogAssertionEqual(test, Constants.LabelNames.HelocNextPayment, headerSection["Next Payment"], "Verify Next Payment field value");
                webElementExtensions.ScrollIntoView(_driver, nextPaymentInfoLocBy);
                webElementExtensions.IsElementDisplayed(_driver, nextPaymentInfoLocBy, "Next Payment Info Icon");
                webElementExtensions.MoveToElement(_driver, nextPaymentInfoLocBy);
                reportLogger.TakeScreenshot(test, "Mouse Over on Next Payment Info Icon");
                string nextPaymentInfo = webElementExtensions.GetElementText(_driver, nextPaymentToolTipInfoLocBy);
                ReportingMethods.LogAssertionEqual(test, Constants.Messages.NextPaymentToolTipInfoHeloc, nextPaymentInfo, "Verify Next Payment Info Icon field value");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, paymentSummaryPaymentBreakDownSectionLocBy), "Payment Breakdown Message");
                ReportingMethods.LogAssertionEqual(test, Constants.Messages.PaymentBreakDownSectionMessageForHeloc, webElementExtensions.GetElementText(_driver, paymentSummaryPaymentBreakDownSectionLocBy), "Verify Payment Breakdown section message");
            }
            else if (currentDate < helocDate && Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.FirstPrincipalBalance]) == 0)
            {
                ReportingMethods.Log(test, "Current day is : " + currentDate.ToString("MM/dd/yyyy") + " is before HELOC date : " + helocDate);
                ReportingMethods.LogAssertionEqual(test, Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).ToString("MM/dd/yyyy"), headerSection["Next Payment Due"], "Verify Next Payment Due field value");
                ReportingMethods.LogAssertionEqual(test, Constants.LabelNames.HelocNextPayment, headerSection["Next Payment"], "Verify Next Payment field value");
                webElementExtensions.ScrollIntoView(_driver, nextPaymentInfoLocBy);
                webElementExtensions.IsElementDisplayed(_driver, nextPaymentInfoLocBy, "Next Payment Info Icon");
                webElementExtensions.MoveToElement(_driver, nextPaymentInfoLocBy);
                reportLogger.TakeScreenshot(test, "Mouse Over on Next Payment Info Icon");
                string nextPaymentInfo = webElementExtensions.GetElementText(_driver, nextPaymentToolTipInfoLocBy);
                ReportingMethods.LogAssertionEqual(test, Constants.Messages.NextPaymentToolTipInfoWhenUPBIsZeroHeloc, nextPaymentInfo, "Verify Next Payment Info Icon field value when UPB equal to Zero");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, paymentSummaryPaymentBreakDownSectionLocBy), "Payment Breakdown Message");
                ReportingMethods.LogAssertionEqual(test, Constants.Messages.PaymentBreakDownSectionMessageWhenUPBIsZeroForHeloc, webElementExtensions.GetElementText(_driver, paymentSummaryPaymentBreakDownSectionLocBy), "Verify Payment Breakdown section message when UPB equal to Zero");
            }
            else
            {
                DateTime fifteenthDate = new DateTime(currentDate.Year, currentDate.Month, 15, 00, 00, 00);
                List<DateTime> holidays = commonServices.GetBankHolidays();

                if (holidays.Contains(fifteenthDate))
                {
                    ReportingMethods.Log(test, "<b> 15th date is Bank Holiday and Current date is : " + currentDate.ToString("MM/dd/yyyy") + "</b>");
                }
                else
                {
                    ReportingMethods.Log(test, "<b> 15th date is : " + fifteenthDate.DayOfWeek.ToString() + " and Current date is : " + currentDate.ToString("MM/dd/yyyy") + "</b>");
                }
                bool pendingPayments = commonServices.GetPendingPaymentDetails(loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString());
                if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 1 && !pendingPayments)
                {
                    ReportingMethods.LogAssertionEqual(test, Convert.ToDateTime((loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate])).ToString("MM/dd/yyyy"), headerSection["Confirm Payment Arrangements"], "Verify Payment Due field value");
                }
                else
                {
                    ReportingMethods.LogAssertionEqual(test, Convert.ToDateTime((loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate])).ToString("MM/dd/yyyy"), headerSection["Next Payment Due"], "Verify Next Payment Due field value");
                }
                ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(helocLoanInfo.PaymentsDue.FirstOrDefault().TotalMonthlyPayment?.ToString("N").Trim()), headerSection["Next Payment"], "Verify Next Payment field value");
                var paymentBreakdownSection = GetPaymentBreakDownValue();
                ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(helocLoanInfo.PaymentsDue.FirstOrDefault().PrincipalAndInterest?.ToString("N")), paymentBreakdownSection["Principal & Interest"], "Verify Principal & Interest field value");
                ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(helocLoanInfo.PaymentsDue.FirstOrDefault().TotalMonthlyPayment?.ToString("N")), paymentBreakdownSection["Total Monthly Payment"], "Verify Total Monthly Payment field value");

                double TotalDue = 0.0;
                Dictionary<string, string> feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                if (feeDic.ContainsKey(Constants.FeeType.LateCharges))
                {
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(feeDic[Constants.FeeType.LateCharges]).ToString("N")}", paymentBreakdownSection[Constants.FeeType.LateCharges], "Verify Late Charges field value");
                    By locBy = By.XPath(paymentBreakdownFeesByText.Replace("<FEES>", Constants.FeeType.LateCharges));
                    bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.BrickRed, locBy, null, null, Constants.CssAttributes.Color);
                    _driver.ReportResult(test, flag, "Successfully verified that " + Constants.FeeType.LateCharges + " field value is 'Red' in color",
                                                        "Failure - " + Constants.FeeType.LateCharges + " field value is not Red in color");
                    TotalDue += Convert.ToDouble(feeDic[Constants.FeeType.LateCharges]);
                }
                if (helocLoanInfo.DelinquentPaymentBalance > 0)
                {
                    ReportingMethods.LogAssertionEqual(test, $"${helocLoanInfo.DelinquentPaymentBalance?.ToString("N")}", paymentBreakdownSection[Constants.FeeType.PastDuePayments], "Verify Past Due Payments field value");
                    By locBy = By.XPath(paymentBreakdownFeesByText.Replace("<FEES>", Constants.FeeType.PastDuePayments));
                    bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.BrickRed, locBy, null, null, Constants.CssAttributes.Color);
                    _driver.ReportResult(test, flag, "Successfully verified that " + Constants.FeeType.PastDuePayments + " field value is 'Red' in color",
                                                        "Failure - " + Constants.FeeType.PastDuePayments + " field value is not Red in color");
                    TotalDue += Convert.ToDouble(helocLoanInfo.DelinquentPaymentBalance.HasValue ? helocLoanInfo.DelinquentPaymentBalance : 0);
                }
                if (feeDic.ContainsKey(Constants.FeeType.NSFFees))
                {
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(feeDic[Constants.FeeType.NSFFees]).ToString("N")}", paymentBreakdownSection[Constants.FeeType.NSFFees], "Verify NSF Fees field value");
                    By locBy = By.XPath(paymentBreakdownFeesByText.Replace("<FEES>", Constants.FeeType.NSFFees));
                    bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.BrickRed, locBy, null, null, Constants.CssAttributes.Color);
                    _driver.ReportResult(test, flag, "Successfully verified that " + Constants.FeeType.NSFFees + " field value is 'Red' in color",
                                                        "Failure - " + Constants.FeeType.NSFFees + " field value is not Red in color");
                    TotalDue += Convert.ToDouble(feeDic[Constants.FeeType.NSFFees]);
                }
                if (feeDic.ContainsKey(Constants.FeeType.OtherFees))
                {
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]).ToString("N")}", paymentBreakdownSection[Constants.FeeType.OtherFees], "Verify Other Fees field value");
                    By locBy = By.XPath(paymentBreakdownFeesByText.Replace("<FEES>", Constants.FeeType.OtherFees));
                    bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.BrickRed, locBy, null, null, Constants.CssAttributes.Color);
                    _driver.ReportResult(test, flag, "Successfully verified that " + Constants.FeeType.OtherFees + " field value is 'Red' in color",
                                                        "Failure - " + Constants.FeeType.OtherFees + " field value is not Red in color");
                    TotalDue += Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
                }
                if (feeDic.ContainsKey(Constants.FeeType.AnnualMaintenanceFees))
                {
                    ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(feeDic[Constants.FeeType.AnnualMaintenanceFees]).ToString("N")}", paymentBreakdownSection[Constants.FeeType.AnnualMaintenanceFees], "Verify Other Fees field value");
                    By locBy = By.XPath(paymentBreakdownFeesByText.Replace("<FEES>", Constants.FeeType.AnnualMaintenanceFees));
                    bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.BrickRed, locBy, null, null, Constants.CssAttributes.Color);
                    _driver.ReportResult(test, flag, "Successfully verified that " + Constants.FeeType.AnnualMaintenanceFees + " field value is 'Red' in color",
                                                        "Failure - " + Constants.FeeType.AnnualMaintenanceFees + " field value is not Red in color");
                    TotalDue += Convert.ToDouble(feeDic[Constants.FeeType.AnnualMaintenanceFees]);
                }
                if (helocLoanInfo.DelinquentPaymentBalance == 0)
                {
                    TotalDue += Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment]);
                }
                ReportingMethods.LogAssertionEqual(test, $"${TotalDue.ToString("N")}", paymentBreakdownSection["Total Due"], "Verify Total Due field value");
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.SuspenseBalance]).ToString("N")}", paymentBreakdownSection["Suspense"], "Verify Suspense field value");
                ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount((Convert.ToDouble(TotalDue) - Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.SuspenseBalance]))), paymentBreakdownSection["Net Due"], "Verify Net Due field value");
            }

            var taxesInsuranceSection = GetTaxesInsurance();
            ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.YtdTaxAmount]).ToString("N")}", "$" + Convert.ToDouble(taxesInsuranceSection["Taxes Paid YTD"].Replace("$", "")).ToString("N").Trim(), "Verify Taxes Paid YTD field value");
            ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.YtdInsurancePaidAmount]).ToString("N")}", "$" + Convert.ToDouble(taxesInsuranceSection["Insurance Paid YTD"].Replace("$", "")).ToString("N").Trim(), "Verify Insurance Paid YTD field value");
            ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.YtdInterestPaidAmount]).ToString("N")}", "$" + Convert.ToDouble(taxesInsuranceSection["Interest Paid YTD"].Replace("$", "")).ToString("N").Trim(), "Verify Interested Paid YTD field value");
        }

        /// <summary>
        /// Method to verify Payment BreakDown Details page for Heloc Loan
        /// </summary>
        /// <param name="loanLevelData">Loan Level Data from DB</param>
        public void VerifyPaymentBreakDownDetailsOnPaymentPageForHeloc(Hashtable loanLevelData)
        {
            APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData);
            var paymentBreakdownSection = GetPaymentBreakDownValue();
            ReportingMethods.LogAssertionEqual(test, $"${helocLoanInfo.PaymentsDue.FirstOrDefault().PrincipalAndInterest?.ToString("N")}", paymentBreakdownSection["Principal & Interest"], "Verify Principal & Interest field value");
            ReportingMethods.LogAssertionEqual(test, $"${helocLoanInfo.PaymentsDue.FirstOrDefault().TotalMonthlyPayment?.ToString("N")}", paymentBreakdownSection["Total Monthly Payment"], "Verify Total Monthly Payment field value");

            double TotalDue = 0.0;
            Dictionary<string, string> feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
            if (feeDic.ContainsKey(Constants.FeeType.LateCharges))
            {
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(feeDic[Constants.FeeType.LateCharges]).ToString("N")}", paymentBreakdownSection[Constants.FeeType.LateCharges], "Verify Late Charges field value");
                By locBy = By.XPath(paymentBreakdownFeesByText.Replace("<FEES>", Constants.FeeType.LateCharges));
                bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.BrickRed, locBy, null, null, Constants.CssAttributes.Color);
                _driver.ReportResult(test, flag, "Successfully verified that " + Constants.FeeType.LateCharges + " field value is 'Red' in color",
                                                    "Failure - " + Constants.FeeType.LateCharges + " field value is not Red in color");
                TotalDue += Convert.ToDouble(feeDic[Constants.FeeType.LateCharges]);
            }
            if (helocLoanInfo.DelinquentPaymentBalance > 0)
            {
                ReportingMethods.LogAssertionEqual(test, $"${helocLoanInfo.DelinquentPaymentBalance?.ToString("N")}", paymentBreakdownSection[Constants.FeeType.PastDuePayments], "Verify Past Due Payments field value");
                By locBy = By.XPath(paymentBreakdownFeesByText.Replace("<FEES>", Constants.FeeType.PastDuePayments));
                bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.BrickRed, locBy, null, null, Constants.CssAttributes.Color);
                _driver.ReportResult(test, flag, "Successfully verified that " + Constants.FeeType.PastDuePayments + " field value is 'Red' in color",
                                                    "Failure - " + Constants.FeeType.PastDuePayments + " field value is not Red in color");
                TotalDue += Convert.ToDouble(helocLoanInfo.DelinquentPaymentBalance.HasValue ? helocLoanInfo.DelinquentPaymentBalance : 0);
            }
            if (feeDic.ContainsKey(Constants.FeeType.NSFFees))
            {
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(feeDic[Constants.FeeType.NSFFees]).ToString("N")}", paymentBreakdownSection[Constants.FeeType.NSFFees], "Verify NSF Fees field value");
                By locBy = By.XPath(paymentBreakdownFeesByText.Replace("<FEES>", Constants.FeeType.NSFFees));
                bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.BrickRed, locBy, null, null, Constants.CssAttributes.Color);
                _driver.ReportResult(test, flag, "Successfully verified that " + Constants.FeeType.NSFFees + " field value is 'Red' in color",
                                                    "Failure - " + Constants.FeeType.NSFFees + " field value is not Red in color");
                TotalDue += Convert.ToDouble(feeDic[Constants.FeeType.NSFFees]);
            }
            if (feeDic.ContainsKey(Constants.FeeType.OtherFees))
            {
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]).ToString("N")}", paymentBreakdownSection[Constants.FeeType.OtherFees], "Verify Other Fees field value");
                By locBy = By.XPath(paymentBreakdownFeesByText.Replace("<FEES>", Constants.FeeType.OtherFees));
                bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.BrickRed, locBy, null, null, Constants.CssAttributes.Color);
                _driver.ReportResult(test, flag, "Successfully verified that " + Constants.FeeType.OtherFees + " field value is 'Red' in color",
                                                    "Failure - " + Constants.FeeType.OtherFees + " field value is not Red in color");
                TotalDue += Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
            }
            if (feeDic.ContainsKey(Constants.FeeType.AnnualMaintenanceFees))
            {
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(feeDic[Constants.FeeType.AnnualMaintenanceFees]).ToString("N")}", paymentBreakdownSection[Constants.FeeType.AnnualMaintenanceFees], "Verify Other Fees field value");
                By locBy = By.XPath(paymentBreakdownFeesByText.Replace("<FEES>", Constants.FeeType.AnnualMaintenanceFees));
                bool flag = webElementExtensions.VerifyElementColor(Constants.Colors.BrickRed, locBy, null, null, Constants.CssAttributes.Color);
                _driver.ReportResult(test, flag, "Successfully verified that " + Constants.FeeType.AnnualMaintenanceFees + " field value is 'Red' in color",
                                                    "Failure - " + Constants.FeeType.AnnualMaintenanceFees + " field value is not Red in color");
                TotalDue += Convert.ToDouble(feeDic[Constants.FeeType.AnnualMaintenanceFees]);
            }
            if (helocLoanInfo.DelinquentPaymentBalance == 0)
            {
                TotalDue += Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment]);
            }
            if (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.RecoverableCorpAdvBalance]) < 0)
            {
                ReportingMethods.LogAssertionFalse(test, paymentBreakdownSection.Keys.Contains("Recoverable Corp Adv"), "Verify Recoverable Corp Adv field value not displayed when it's value < 0 ");
                TotalDue = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.AccruedLateChargeAmount]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.OtherFees]);
                if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) > 1)
                {
                    TotalDue = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentBalance]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.AccruedLateChargeAmount]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.OtherFees]);
                }
            }
            else
            {
                ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(loanLevelData[Constants.LoanLevelDataColumns.RecoverableCorpAdvBalance]), paymentBreakdownSection["Recoverable Corp Adv"], "Verify Recoverable Corp Adv field value");

                TotalDue = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.AccruedLateChargeAmount]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.OtherFees]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.RecoverableCorpAdvBalance]);

                if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) > 1)
                {
                    TotalDue = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentBalance]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.AccruedLateChargeAmount]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.OtherFees]) + Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.RecoverableCorpAdvBalance]);
                }
            }
            ReportingMethods.LogAssertionEqual(test, $"${TotalDue.ToString("N")}", paymentBreakdownSection["Total Due"], "Verify Total Due field value");
            ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.SuspenseBalance]).ToString("N")}", paymentBreakdownSection["Suspense"], "Verify Suspense field value");
            ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount((Convert.ToDouble(TotalDue) - Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.SuspenseBalance]))), paymentBreakdownSection["Net Due"], "Verify Net Due field value");
        }

        /// <summary>
        /// Method to Verify OTP Screen Details For Heloc
        /// </summary>
        /// <param name="loanLevelData">Loan level details from DB</param>
        /// <param name="type">Payment flow - setup/Edit</param>
        public void VerifyOTPScreenDetailsForHeloc(Hashtable loanLevelData, string type = "setup")
        {
            var unPaidPrincipalBalanceSection = GetOTPSubSectionValues("Unpaid Principal Balance");
            ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.FirstPrincipalBalance]).ToString("N")}", unPaidPrincipalBalanceSection["Unpaid Principal Balance"], type == "setup" ? "Verify Unpaid Principal Balance section field value in OTP Screen" : "Verify " + type + " Unpaid Principal Balance section field value in OTP Screen");
            ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.YtdPrincipalPaidAmount]).ToString("N")}", unPaidPrincipalBalanceSection["Principal Paid"], type == "setup" ? "Verify Principal Paid field value in OTP Screen" : "Verify " + type + " Principal Paid field value in OTP Screen");
            if (!(loanLevelData[Constants.LoanLevelDataColumns.LoanNumber]).ToString().StartsWith("9"))
            {
                ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.EscrowBalance]) - Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.EscrowAdvanceBalance])), unPaidPrincipalBalanceSection["Escrow Balance"], type == "setup" ? "Verify Escrow Balance field value in OTP Screen" : "Verify " + type + " Escrow Balance field value in OTP Screen");
            }
            DateTime currentDate = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
            DateTime firstDayOfNextSecondMonth = firstDayOfNextMonth.AddMonths(1);

            var paymentBreakDownSection = GetOTPSubSectionValues("Payment Breakdown");

            DateTime helocDate = commonServices.GetHelocWorkingDate(15, 2);
            if (currentDate < helocDate && Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 0)
            {
                ReportingMethods.LogAssertionEqual(test, "$0.00", paymentBreakDownSection["Payment Breakdown"], type == "setup" ? "Verify Payment Breakdown section value in OTP Screen" : "Verify " + type + " Payment Breakdown section value in OTP Screen");
                ReportingMethods.LogAssertionEqual(test, "$0.00", paymentBreakDownSection["Principal & Interest"], type == "setup" ? "Verify Principal & Interest field value in OTP Screen" : "Verify " + type + " Principal & Interest field value in OTP Screen");
                ReportingMethods.LogAssertionEqual(test, "$0.00", paymentBreakDownSection["Taxes &/or Insurance"], type == "setup" ? "Verify Taxes &/or Insurance field value in OTP Screen" : "Verify " + type + " Taxes &/or Insurance field value in OTP Screen");
            }
            else
            {
                APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData);
                Dictionary<string, string> feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(helocLoanInfo.PaymentsDue.FirstOrDefault().TotalMonthlyPayment?.ToString("N").Trim()), paymentBreakDownSection["Payment Breakdown"], type == "setup" ? "Verify Payment Breakdown section value in OTP Screen" : "Verify " + type + " Payment Breakdown section value in OTP Screen");
                ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(helocLoanInfo.PaymentsDue.FirstOrDefault().PrincipalAndInterest?.ToString("N").Trim()), paymentBreakDownSection["Principal & Interest"], type == "setup" ? "Verify Principal & Interest field value in OTP Screen" : "Verify " + type + " Principal & Interest field value in OTP Screen");
                ReportingMethods.LogAssertionEqual(test, commonServices.GetDollarFormattedAmount(helocLoanInfo.PaymentsDue.FirstOrDefault().TaxAndInsurance?.ToString("N").Trim()), paymentBreakDownSection["Taxes &/or Insurance"], type == "setup" ? "Verify Taxes &/or Insurance field value in OTP Screen" : "Verify " + type + " Taxes &/or Insurance field value in OTP Screen");
            }
        }

        /// <summary>
        /// Retrieves Heloc Loan Late Charge details for a given loan number.
        /// </summary>
        /// <param name="loanNumber">The loan number for which to retrieve Heloc Loan Late Charge details.</param>
        /// <param name="isReportRequired">Flag indicating whether a report is required.</param>
        /// <returns>A Hashtable containing the Heloc Loan Late Charge details.</returns>
        public string GetHelocLateCharges(string loanNumber, bool isReportRequired = false)
        {
            bool flag = false;
            Hashtable hashData = new Hashtable();
            string lateCharge = "";

            try
            {
                List<string> columnDataRequired = typeof(Constants.FeeDataColumns).GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(field => field.FieldType == typeof(string)).Select(field => (string)field.GetValue(null)).ToList();
                string query = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetLateChargesForHelocOTP)).Replace("ZONE", ConfigSettings.Environment.Equals("QA") ? "Z5" : "TA");
                query = query.Replace("#", loanNumber);
                hashData = commonServices.ExecuteQueryAndGetDataFromDataBase(query, null, columnDataRequired).FirstOrDefault();
                if (hashData != null)
                {
                    lateCharge = hashData[Constants.FeeDataColumns.TransactionAmount].ToString();
                }
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Heloc Loan Late Charge details" + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Heloc Loan Late Charge details", "Failed while getting Heloc Loan Late Charge details");

            return lateCharge;
        }

        /// <summary>
        /// Verifies the banner details for Heloc based on the loan level data, plan type, and type.
        /// </summary>
        /// <param name="loanLevelData">The loan level data as a Hashtable.</param>
        /// <param name="type">The type as a string. Default value is "setup".</param>
        public void VerifyBannerDetailsForHeloc(Hashtable loanLevelData, string type = "setup")
        {
            type = type.ToLower() == "setup" ? "" : type;
            if (loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Trim() == "Past Due")
            {
                VerifyBannerPastDueDateDetails(loanLevelData, "", type);
                webElementExtensions.VerifyElementColor(Constants.Colors.Orange, otpPaymentBannerLocBy, null, "Verify " + type + " Account Standing Banner details is Red");
            }
            else if (Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 0)//On-Time
            {
                DateTime currentDate = DateTime.Now;
                DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
                DateTime firstDayOfNextSecondMonth = firstDayOfNextMonth.AddMonths(1);

                if (Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).ToString("yyyy-MM-dd") == firstDayOfNextMonth.ToString("yyyy-MM-dd"))//On-Time
                {
                    VerifyBannerOnTimeDateDetails(loanLevelData, "", type);
                    webElementExtensions.VerifyElementColor(Constants.Colors.White, otpPaymentBannerLocBy, null, "Verify " + type + " Account Standing Banner details is White");
                }
                else if (Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).ToString("yyyy-MM-dd") == firstDayOfNextSecondMonth.ToString("yyyy-MM-dd"))//Prepaid
                {
                    VerifyBannerPrepaidDateDetails(loanLevelData, "", type);
                    webElementExtensions.VerifyElementColor(Constants.Colors.Green, otpPaymentBannerLocBy, null, "Verify " + type + " Account Standing Banner details is Green");
                }
            }
        }

        /// <summary>
        /// Method to Verify Fields of Heloc Payment Fields
        /// </summary>
        /// <param name="loanLevelData">Loan Level Data from DB</param>
        /// <param name="helocLoanInfo">Heloc Loan Info Data from API</param>
        /// <param name="additionalPaymentPrincipalParam">Addtional Payment Principal Amount</param>
        /// <param name="additionalPaymentPrincipalEditParam">Addtional Payment Principal Edit Amount</param>
        /// <param name="type">Payment flow Type -setup/Edit</param>
        /// <returns>total payment</returns>
        public string VerifyHelocPaymentFields(Hashtable loanLevelData, APIConstants.HelocLoanInfo helocLoanInfo, double additionalPaymentPrincipalParam = 0.0, double additionalPaymentPrincipalEditParam = 0.0, string type = "setup")
        {
            double totalPayment = 0.00, additionalPaymentPrincipalAmount = 0.00, lateFee = 0.00, nSFFee = 0.00, otherFees = 0.00, annualMaintenanceFee = 0.00, pastDueAmount = 0.00, upcomingPaymentAmount = 0.00, monthlyPaymentPastDueAmount = 0.00, currentPaymentAmount = 0.00;
            double intialTotalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", "")); bool flag;
            ReportingMethods.Log(test, "Intial Total Payment is" + " " + "$" + intialTotalPayment);
            DateTime currentDate = DateTime.Now, helocDate = commonServices.GetHelocWorkingDate(15, 2);

            if (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) > 2)
            {
                if (type.ToLower().Trim() == "setup") { webElementExtensions.IsElementDisplayed(_driver, matLabelAmountLocBy, "Amount (Monthly Payment)"); }
                string amountMonthlyPayment = "0.0"; amountMonthlyPayment = GetAmountMonthlyPaymentValue();
                string[] amountMonthlyPaymentArray = amountMonthlyPayment.Replace(",", "").Replace("$", "").Split('.');
                double addMonthlyAmount = Convert.ToDouble(amountMonthlyPaymentArray[0]) * 100;
                if (type.ToLower().Trim() == "setup") { ClickAmountMonthlyPaymentRadioButton(); }
                else { addMonthlyAmount = addMonthlyAmount + 10; }
                addMonthlyAmount = addMonthlyAmount + Convert.ToInt32(amountMonthlyPaymentArray[1]);
                amountMonthlyPayment = EnterAmountMonthlyPaymentValue(addMonthlyAmount);
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (Convert.ToDouble(amountMonthlyPayment.Replace("$", ""))).ToString("N"), type == "setup" ? $"Verify Total Payment with Amount(Monthly Payment)" : $"Verify " + type + " Total Payment with Amount(Monthly Payment)");
            }
            else if (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 2)
            {

                if (type == "setup")
                {
                    webElementExtensions.IsElementDisplayed(_driver, pastDueRadioButtonAmountTextLocBy, "Past Due Amount");
                    pastDueAmount = Convert.ToDouble(webElementExtensions.GetElementText(_driver, pastDueRadioButtonAmountTextLocBy).Replace("$", ""));
                    webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPaymentPrincipalParam * 100).ToString());
                    ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPaymentPrincipalParam));
                }
                else
                {
                    webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPaymentPrincipalEditParam * 100).ToString());
                    ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPaymentPrincipalEditParam));
                }
                Dictionary<string, string> feeDic = null;
                if (helocLoanInfo.Fees.Count > 0)
                {
                    feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                }
                pastDueAmount = Convert.ToDouble(webElementExtensions.GetElementText(_driver, pastDueRadioButtonAmountTextLocBy).Replace("$", ""));
                additionalPaymentPrincipalAmount = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, additionalPrincipalTextboxLocBy).Replace("$", ""));
                if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.LateCharges))
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, lateFeeAmountCheckboxLocBy, "Late Fee Checkbox");
                    lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                    double expectedLateFee = Convert.ToDouble(feeDic[Constants.FeeType.LateCharges]);
                    ReportingMethods.LogAssertionEqual(test, "$" + expectedLateFee.ToString("N"), "$" + lateFee.ToString("N"), "Verify Late Fees Text box amount is equal to HELOC API Late Fees.");
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + (pastDueAmount + additionalPaymentPrincipalAmount + lateFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with LateFee calculation");
                }
                if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.NSFFees))
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, nSFFeeAmountCheckboxLocBy, "NSF Fee Checkbox");
                    nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", ""));
                    double expectedNSFFee = Convert.ToDouble(feeDic[Constants.FeeType.NSFFees]);
                    ReportingMethods.LogAssertionEqual(test, "$" + expectedNSFFee.ToString("N"), "$" + nSFFee.ToString("N"), "Verify NSF Fees Text box amount is equal to HELOC API NSF Fees.");
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + (pastDueAmount + additionalPaymentPrincipalAmount + lateFee + nSFFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with NSF Fee calculation");
                }
                if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.OtherFees))
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, otherFeeAmountCheckboxLocBy, "Other Fee Checkbox");
                    otherFees = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                    double expectedOtherFees = Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
                    ReportingMethods.LogAssertionEqual(test, "$" + expectedOtherFees.ToString("N"), "$" + otherFees.ToString("N"), "Verify Other Fees Text box amount is equal to HELOC API Other Fees.");
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + (pastDueAmount + additionalPaymentPrincipalAmount + lateFee + nSFFee + otherFees).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with Other Fee calculation");
                }
                if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.AnnualMaintenanceFees))
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, annualMaintenanceFeeAmountCheckboxLocBy, "Annaul Maintenance Fee Checkbox");
                    annualMaintenanceFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, annualMaintenanceFeeAmountCheckboxLocBy).Replace("$", ""));
                    double expectedAnnaulMaintenanceFee = Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
                    ReportingMethods.LogAssertionEqual(test, "$" + expectedAnnaulMaintenanceFee.ToString("N"), "$" + annualMaintenanceFee.ToString("N"), "Verify Annaul Maintenance Fees Text box amount is equal to HELOC API Annaul Maintenance Fees.");
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + (intialTotalPayment + additionalPaymentPrincipalAmount + lateFee + nSFFee + otherFees + annualMaintenanceFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with Other Fee calculation");
                }

                var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                additionalPaymentPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
                ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
                ReportingMethods.LogAssertionEqual(test, "$" + (pastDueAmount + lateFee + nSFFee + otherFees + annualMaintenanceFee + additionalPaymentPrincipalAmount).ToString("N"), "$" + totalPayment.ToString("N"), type == "setup" ? $"Verify Total Payment with Additional Principal calculation" : $"Verify " + type + " Total Payment with Additional Principal calculation");
            }
            else if (currentDate > helocDate && Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) <= 1)
            {

                if (type == "setup")
                {
                    if (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 1)
                    {

                        monthlyPaymentPastDueAmount = Convert.ToDouble(GetAmountText(monthlyPaymentAmountTextLocBy).Replace("$", ""));
                        webElementExtensions.ActionClick(_driver, upcomingPaymentCheckboxLocBy, "Upcoming Payment Checkbox");
                        upcomingPaymentAmount = Convert.ToDouble(GetAmountText(upcomingPaymentAmountTextLocBy).Replace("$", ""));
                        ReportingMethods.Log(test, "Upcoming Payment Amount: " + "$" + upcomingPaymentAmount.ToString());
                        totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                        ReportingMethods.Log(test, "Total Payment After Upcoming Payment Check Box is checked: " + "$" + totalPayment.ToString("N"));
                        Dictionary<string, string> feeDic = null;
                        if (helocLoanInfo.Fees.Count > 0)
                        {
                            feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.LateCharges))
                        {
                            webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, lateFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Late Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " Late Fee Check Box not Checked intially and Disabled.");
                            lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                            double expectedLateFee = Convert.ToDouble(feeDic[Constants.FeeType.LateCharges]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedLateFee.ToString("N"), "$" + lateFee.ToString("N"), "Verify Late Fees Text box amount is equal to HELOC API Late Fees.");
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.NSFFees))
                        {
                            webElementExtensions.ScrollIntoView(_driver, nSFFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, nSFFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan NSF Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " NSF Fee Check Box not Checked intially and Disabled.");
                            nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", ""));
                            double expectedNSFFee = Convert.ToDouble(feeDic[Constants.FeeType.NSFFees]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedNSFFee.ToString("N"), "$" + nSFFee.ToString("N"), "Verify NSF Fees Text box amount is equal to HELOC API NSF Fees.");
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.OtherFees))
                        {
                            webElementExtensions.ScrollIntoView(_driver, otherFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, otherFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Other Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " Other Fee Check Box not Checked intially and Disabled.");
                            otherFees = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                            double expectedOtherFees = Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedOtherFees.ToString("N"), "$" + otherFees.ToString("N"), "Verify Other Fees Text box amount is equal to HELOC API Other Fees.");
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.AnnualMaintenanceFees))
                        {
                            webElementExtensions.ScrollIntoView(_driver, annualMaintenanceFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, annualMaintenanceFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Annaul Maintenance Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " Annaul Maintenance Fee Check Box not Checked intially and Disabled.");
                            annualMaintenanceFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, annualMaintenanceFeeTextboxLocBy).Replace("$", ""));
                            double expectedAnnaulMaintenanceFee = Convert.ToDouble(feeDic[Constants.FeeType.AnnualMaintenanceFees]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedAnnaulMaintenanceFee.ToString("N"), "$" + annualMaintenanceFee.ToString("N"), "Verify Annaul Maintenance Fees Text box amount is equal to HELOC API Annaul Maintenance Fees.");
                        }

                        ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (monthlyPaymentPastDueAmount + upcomingPaymentAmount + lateFee + nSFFee + otherFees + annualMaintenanceFee).ToString("N"), $"Verify Total Payment with Upcoming Payment calculation");
                        flag = _driver.FindElement(addPaymentCheckBoxLocBy).GetAttribute(Constants.ElementAttributes.OuterHTML).Contains(Constants.ElementAttributes.Disabled);
                        ReportingMethods.LogAssertionFalse(test, flag, "Verify " + type + " Additional Payment Check Box is enabled when Upcoming Payment Check Box is checked");
                    }
                    if (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 0)
                    {
                        currentPaymentAmount = Convert.ToDouble(GetAmountText(currentPaymentAmountLocBy).Replace("$", ""));
                        Dictionary<string, string> feeDic = null;
                        if (helocLoanInfo.Fees.Count > 0)
                        {
                            feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.LateCharges))
                        {
                            webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, lateFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Late Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " Late Fee Check Box not Checked intially and Disabled.");
                            lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                            double expectedLateFee = Convert.ToDouble(feeDic[Constants.FeeType.LateCharges]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedLateFee.ToString("N"), "$" + lateFee.ToString("N"), "Verify Late Fees Text box amount is equal to HELOC API Late Fees.");
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.NSFFees))
                        {
                            webElementExtensions.ScrollIntoView(_driver, nSFFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, nSFFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan NSF Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " NSF Fee Check Box not Checked intially and Disabled.");
                            nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", ""));
                            double expectedNSFFee = Convert.ToDouble(feeDic[Constants.FeeType.NSFFees]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedNSFFee.ToString("N"), "$" + nSFFee.ToString("N"), "Verify NSF Fees Text box amount is equal to HELOC API NSF Fees.");
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.OtherFees))
                        {
                            webElementExtensions.ScrollIntoView(_driver, otherFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, otherFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Other Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " Other Fee Check Box not Checked intially and Disabled.");
                            otherFees = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                            double expectedOtherFees = Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedOtherFees.ToString("N"), "$" + otherFees.ToString("N"), "Verify Other Fees Text box amount is equal to HELOC API Other Fees.");
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.AnnualMaintenanceFees))
                        {
                            webElementExtensions.ScrollIntoView(_driver, annualMaintenanceFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, annualMaintenanceFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Annaul Maintenance Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " Annaul Maintenance Fee Check Box not Checked intially and Disabled.");
                            annualMaintenanceFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, annualMaintenanceFeeAmountCheckboxLocBy).Replace("$", ""));
                            double expectedAnnaulMaintenanceFee = Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedAnnaulMaintenanceFee.ToString("N"), "$" + annualMaintenanceFee.ToString("N"), "Verify Annaul Maintenance Fees Text box amount is equal to HELOC API Annaul Maintenance Fees.");
                        }
                    }
                    if (_driver.FindElements(addPaymentCheckBoxLocBy).Count > 0)
                    {
                        webElementExtensions.ScrollIntoView(_driver, addPaymentCheckBoxLocBy);
                        webElementExtensions.ClickElementUsingJavascript(_driver, addPaymentCheckBoxLocBy, "Additional Payment CheckBox");
                    }
                    webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPaymentPrincipalParam * 100).ToString());
                    ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPaymentPrincipalParam));
                }
                else
                {
                    if (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 1)
                    {
                        monthlyPaymentPastDueAmount = Convert.ToDouble(GetAmountText(monthlyPaymentAmountTextLocBy).Replace("$", ""));
                        upcomingPaymentAmount = Convert.ToDouble(GetAmountText(upcomingPaymentAmountTextLocBy).Replace("$", ""));
                        Dictionary<string, string> feeDic = null;
                        if (helocLoanInfo.Fees.Count > 0)
                        {
                            feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.LateCharges))
                        {
                            webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, lateFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Late Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " Late Fee Check Box not Checked intially and Disabled.");
                            lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                            double expectedLateFee = Convert.ToDouble(feeDic[Constants.FeeType.LateCharges]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedLateFee.ToString("N"), "$" + lateFee.ToString("N"), "Verify Late Fees Text box amount is equal to HELOC API Late Fees.");
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.NSFFees))
                        {
                            webElementExtensions.ScrollIntoView(_driver, nSFFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, nSFFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan NSF Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " NSF Fee Check Box not Checked intially and Disabled.");
                            nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", ""));
                            double expectedNSFFee = Convert.ToDouble(feeDic[Constants.FeeType.NSFFees]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedNSFFee.ToString("N"), "$" + nSFFee.ToString("N"), "Verify NSF Fees Text box amount is equal to HELOC API NSF Fees.");
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.OtherFees))
                        {
                            webElementExtensions.ScrollIntoView(_driver, otherFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, otherFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Other Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " Other Fee Check Box not Checked intially and Disabled.");
                            otherFees = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                            double expectedOtherFees = Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedOtherFees.ToString("N"), "$" + otherFees.ToString("N"), "Verify Other Fees Text box amount is equal to HELOC API Other Fees.");
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.AnnualMaintenanceFees))
                        {
                            webElementExtensions.ScrollIntoView(_driver, annualMaintenanceFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, annualMaintenanceFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Annaul Maintenance Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " Annaul Maintenance Fee Check Box not Checked intially and Disabled.");
                            annualMaintenanceFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, annualMaintenanceFeeTextboxLocBy).Replace("$", ""));
                            double expectedAnnaulMaintenanceFee = Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedAnnaulMaintenanceFee.ToString("N"), "$" + annualMaintenanceFee.ToString("N"), "Verify Annaul Maintenance Fees Text box amount is equal to HELOC API Annaul Maintenance Fees.");
                        }

                        ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (monthlyPaymentPastDueAmount + upcomingPaymentAmount + lateFee + nSFFee + otherFees + annualMaintenanceFee).ToString("N"), $"Verify " + type + " Total Payment with Upcoming Payment calculation");
                    }
                    else if (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 0)
                    {
                        currentPaymentAmount = Convert.ToDouble(GetAmountText(currentPaymentAmountLocBy).Replace("$", ""));
                        webElementExtensions.WaitForElement(_driver, currentPaymentCheckboxInputLocBy);
                        flag = webElementExtensions.GetElementAttribute(_driver, currentPaymentCheckboxInputLocBy, Constants.ElementAttributes.AriaChecked) == "true" ? true : false;
                        ReportingMethods.LogAssertionTrue(test, flag, "Verify Current Payment checkbox is checked by default");
                        flag = _driver.FindElement(currentPaymentCheckboxInputLocBy).GetAttribute(Constants.ElementAttributes.OuterHTML).Contains(Constants.ElementAttributes.Disabled);
                        ReportingMethods.LogAssertionTrue(test, flag, "Verify Current Payment checkbox is Read Only");
                        if (webElementExtensions.GetElementAttribute(_driver, additionalPaymentCheckboxLocBy, Constants.ElementAttributes.AriaChecked) != "true")
                        {
                            ReportingMethods.Log(test, "Verify initially Addtional Payment checkbox is not checked");
                            webElementExtensions.ClickElementUsingJavascript(_driver, additionalPaymentCheckboxLocBy, "Addtional Payment checkbox");
                        }
                        webElementExtensions.IsElementDisplayed(_driver, additionalPrincipalTextboxLocBy, "Addtional Payment Textbox");
                        ReportingMethods.LogAssertionFalse(test, webElementExtensions.IsElementDisplayed(_driver, additionalEscrowTextboxLocBy), "Verify Escrow Amount Textbox is not displayed");

                        Dictionary<string, string> feeDic = null;
                        if (helocLoanInfo.Fees.Count > 0)
                        {
                            feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.LateCharges))
                        {
                            webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, lateFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Late Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " Late Fee Check Box not Checked intially and Disabled.");
                            lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                            double expectedLateFee = Convert.ToDouble(feeDic[Constants.FeeType.LateCharges]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedLateFee.ToString("N"), "$" + lateFee.ToString("N"), "Verify Late Fees Text box amount is equal to HELOC API Late Fees.");
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.NSFFees))
                        {
                            webElementExtensions.ScrollIntoView(_driver, nSFFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, nSFFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan NSF Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " NSF Fee Check Box not Checked intially and Disabled.");
                            nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", ""));
                            double expectedNSFFee = Convert.ToDouble(feeDic[Constants.FeeType.NSFFees]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedNSFFee.ToString("N"), "$" + lateFee.ToString("N"), "Verify Late Fees Text box amount is equal to HELOC API NSF Fees.");
                        }
                        if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.OtherFees))
                        {
                            webElementExtensions.ScrollIntoView(_driver, otherFeeAmountCheckboxLocBy);
                            flag = webElementExtensions.IsElementDisplayed(_driver, otherFeeAmountCheckboxDisabledCheckedLocBy);
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Other Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " Other Fee Check Box not Checked intially and Disabled.");
                            otherFees = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                            double expectedOtherFees = Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
                            ReportingMethods.LogAssertionEqual(test, "$" + expectedOtherFees.ToString("N"), "$" + lateFee.ToString("N"), "Verify Late Fees Text box amount is equal to HELOC API Other Fees.");
                        }
                    }
                    webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPaymentPrincipalEditParam * 100).ToString());
                    ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPaymentPrincipalEditParam));
                }
                var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
                totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                additionalPaymentPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
                ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
                ReportingMethods.LogAssertionEqual(test, "$" + (currentPaymentAmount + monthlyPaymentPastDueAmount + upcomingPaymentAmount + lateFee + nSFFee + otherFees + annualMaintenanceFee + additionalPaymentPrincipalAmount).ToString("N"), "$" + totalPayment.ToString("N"), type == "setup" ? $"Verify Total Payment with Additional Principal calculation" : $"Verify " + type + " Total Payment with Additional Principal calculation");
            }
            else if (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) <= 1)
            {
                DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                DateTime firstDayOfNextMonth = firstDayOfMonth.AddMonths(1);
                DateTime firstDayOfNextSecondMonth = firstDayOfNextMonth.AddMonths(1);
                Dictionary<string, string> feeDic = null;
                if (helocLoanInfo.Fees.Count > 0)
                {
                    feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
                }
                if (Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).ToString("yyyy-MM-dd") == firstDayOfNextMonth.ToString("yyyy-MM-dd") || Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 1)//On-Time  Or PastDue
                {
                    string amountMonthlyPayment = "0.0";
                    if (_driver.FindElements(matLabelAmountLocBy).Count > 0)
                    {
                        amountMonthlyPayment = GetAmountMonthlyPaymentValue();
                        string[] amountMonthlyPaymentArray = amountMonthlyPayment.Replace(",", "").Replace("$", "").Split('.');
                        double addMonthlyAmount = Convert.ToDouble(amountMonthlyPaymentArray[0]) * 100;
                        if (type.ToLower().Trim() == "setup")
                        {
                            ClickAmountMonthlyPaymentRadioButton();
                        }
                        else
                        {
                            addMonthlyAmount = addMonthlyAmount + 10;
                        }
                        addMonthlyAmount = addMonthlyAmount + Convert.ToInt32(amountMonthlyPaymentArray[1]);
                        amountMonthlyPayment = EnterAmountMonthlyPaymentValue(addMonthlyAmount);
                        totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                        if (type.ToLower().Trim() == "setup")
                        {
                            ReportingMethods.LogAssertionEqual(test, "$" + (intialTotalPayment + Convert.ToDouble(amountMonthlyPayment.Replace("$", ""))).ToString("N"), "$" + totalPayment.ToString("N"), type == "setup" ? $"Verify Total Payment with Amount(Monthly Payment)" : $"Verify " + type + " Total Payment with Amount(Monthly Payment)");
                        }
                        else
                        {
                            ReportingMethods.LogAssertionEqual(test, "$" + (Convert.ToDouble(amountMonthlyPayment.Replace("$", ""))).ToString("N"), "$" + totalPayment.ToString("N"), type == "setup" ? $"Verify Total Payment with Amount(Monthly Payment)" : $"Verify " + type + " Total Payment with Amount(Monthly Payment)");
                        }
                    }
                    if (_driver.FindElements(additionalPrincipalTextboxLocBy).Count > 0)
                    {
                        if (type == "setup")
                        {
                            webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPaymentPrincipalParam * 100).ToString());
                            ReportingMethods.Log(test, "Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPaymentPrincipalParam));
                        }
                        else
                        {
                            webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPaymentPrincipalEditParam * 100).ToString());
                            ReportingMethods.Log(test, "Edit Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPaymentPrincipalEditParam));
                        }
                        var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
                        totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                        additionalPaymentPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
                        ReportingMethods.Log(test, "Total Payment After Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
                        if (type.ToLower() == "edit")
                        {
                            additionalPaymentPrincipalAmount = Convert.ToDouble(commonServices.GetDollarFormattedAmount(additionalPaymentPrincipalEditParam - additionalPaymentPrincipalParam).ToString().Replace("$", ""));
                        }
                        ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + Convert.ToDouble(amountMonthlyPayment.Replace("$", "")) + additionalPaymentPrincipalAmount).ToString("N"), type == "setup" ? $"Verify Total Payment with Principal calculation" : $"Verify " + type + " Total Payment with Principal calculation");

                    }
                    if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.LateCharges))
                    {
                        flag = webElementExtensions.IsElementDisplayed(_driver, lateFeeAmountCheckboxDisabledCheckedLocBy);
                        if (flag)
                        {
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Late Fee Check Box is Checked intially and  Disabled.",
                                                                               "Failure - " + type + " Late Fee Check Box not Checked intially and Disabled.");
                        }
                        else
                        {
                            webElementExtensions.ClickElementUsingJavascript(_driver, lateFeeAmountCheckboxLocBy, "Late Fee Checkbox");
                        }
                        lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                        double expectedLateFee = Convert.ToDouble(feeDic[Constants.FeeType.LateCharges]);
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedLateFee.ToString("N"), "$" + lateFee.ToString("N"), "Verify Late Fees Text box amount is equal to HELOC API Late Fees.");
                        totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                        ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + Convert.ToDouble(amountMonthlyPayment.Replace("$", "")) + additionalPaymentPrincipalAmount + (flag != true ? lateFee : 0.0)).ToString("N"), $"Verify Total Payment with LateFee calculation");
                    }
                    if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.NSFFees))
                    {
                        flag = webElementExtensions.IsElementDisplayed(_driver, nSFFeeAmountCheckboxDisabledCheckedLocBy);
                        if (flag)
                        {
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan NSF Fee Check Box is Checked intially and  Disabled.",
                                                   "Failure - " + type + " NSF Fee Check Box not Checked intially and Disabled.");
                        }
                        else
                        {
                            webElementExtensions.ClickElementUsingJavascript(_driver, nSFFeeAmountCheckboxLocBy, "NSF Fee Checkbox");
                        }
                        nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", ""));
                        double expectedNSFFee = Convert.ToDouble(feeDic[Constants.FeeType.NSFFees]);
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedNSFFee.ToString("N"), "$" + nSFFee.ToString("N"), "Verify NSF Fees Text box amount is equal to HELOC API NSF Fees.");
                        totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                        ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + +Convert.ToDouble(amountMonthlyPayment.Replace("$", "")) + additionalPaymentPrincipalAmount + (flag != true ? lateFee : 0.0) + (flag != true ? nSFFee : 0.0)).ToString("N"), $"Verify Total Payment with NSF Fee calculation");
                    }
                    if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.OtherFees))
                    {
                        flag = webElementExtensions.IsElementDisplayed(_driver, otherFeeAmountCheckboxDisabledCheckedLocBy);
                        if (flag)
                        {
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Other Fee Check Box is Checked intially and  Disabled.",
                                                            "Failure - " + type + " Other Fee Check Box not Checked intially and Disabled.");
                        }
                        else
                        {
                            webElementExtensions.ClickElementUsingJavascript(_driver, otherFeeAmountCheckboxLocBy, "Other Fee Checkbox");
                        }
                        otherFees = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                        double expectedOtherFees = Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedOtherFees.ToString("N"), "$" + otherFees.ToString("N"), "Verify Other Fees Text box amount is equal to HELOC API Other Fees.");
                        totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                        ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + +Convert.ToDouble(amountMonthlyPayment.Replace("$", "")) + additionalPaymentPrincipalAmount + (flag != true ? lateFee : 0.0) + (flag != true ? nSFFee : 0.0) + (flag != true ? otherFees : 0.0)).ToString("N"), $"Verify Total Payment with Other Fee calculation");
                    }
                    if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.AnnualMaintenanceFees))
                    {
                        flag = webElementExtensions.IsElementDisplayed(_driver, annualMaintenanceFeeAmountCheckboxDisabledCheckedLocBy);
                        if (flag)
                        {
                            _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Annaul Maintenance Fee Check Box is Checked intially and  Disabled.",
                                                        "Failure - " + type + " Annaul Maintenance Fee Check Box not Checked intially and Disabled.");
                        }
                        else
                        {
                            webElementExtensions.ClickElementUsingJavascript(_driver, annualMaintenanceFeeAmountCheckboxLocBy, "Annaul Maintenance Fee Checkbox");
                        }
                        annualMaintenanceFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, annualMaintenanceFeeTextboxLocBy).Replace("$", ""));
                        double expectedAnnaulMaintenanceFee = Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedAnnaulMaintenanceFee.ToString("N"), "$" + annualMaintenanceFee.ToString("N"), "Verify Annaul Maintenance Fees Text box amount is equal to HELOC API Annaul Maintenance Fees.");
                        totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                        ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + +Convert.ToDouble(amountMonthlyPayment.Replace("$", "")) + additionalPaymentPrincipalAmount + (flag != true ? lateFee : 0.0) + (flag != true ? nSFFee : 0.0) + (flag != true ? otherFees : 0.0) + (flag != true ? annualMaintenanceFee : 0.0)).ToString("N"), $"Verify Total Payment with Other Fee calculation");
                    }
                }
                else if (Convert.ToDateTime(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate]).ToString("yyyy-MM-dd") == firstDayOfNextSecondMonth.ToString("yyyy-MM-dd"))//Prepaid
                {
                    if (type == "setup")
                    {
                        if (_driver.FindElements(addPaymentCheckBoxLocBy).Count > 0)
                        {
                            webElementExtensions.ScrollIntoView(_driver, addPaymentCheckBoxLocBy);
                            webElementExtensions.ClickElementUsingJavascript(_driver, addPaymentCheckBoxLocBy, "Additional Payment CheckBox");
                        }
                        webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPaymentPrincipalParam * 100).ToString());
                        ReportingMethods.Log(test, "Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPaymentPrincipalParam));
                    }
                    else
                    {
                        webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPaymentPrincipalEditParam * 100).ToString());
                        ReportingMethods.Log(test, "Edit Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPaymentPrincipalEditParam));
                    }
                    var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    additionalPaymentPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
                    if (type.ToLower() == "edit")
                    {
                        additionalPaymentPrincipalAmount = Convert.ToDouble(commonServices.GetDollarFormattedAmount(additionalPaymentPrincipalEditParam - additionalPaymentPrincipalParam).ToString().Replace("$", ""));
                    }
                    ReportingMethods.LogAssertionEqual(test, "$" + totalPayment.ToString("N"), "$" + (intialTotalPayment + additionalPaymentPrincipalAmount).ToString("N"), type == "setup" ? $"Verify Total Payment with Principal calculation" : $"Verify " + type + " Total Payment with Principal calculation");
                }
            }
            return "$" + totalPayment.ToString("N");
        }


        /// <summary>
        /// <param name="loanLevelData">Loan Level Data from DB</param>
        /// <param name="additionalPaymentsParam">Addtional Payments Amount</param>
        /// <param name="additionalEditPaymentsParam">Addtional Edit Payments Amount</param>
        /// <param name="type">Payment flow Type -setup/Edit</param>
        /// </summary>
        public string VerifyUpcomingPayment(Hashtable loanLevelData, double additionalPaymentsParam, double additionalEditPaymentsParam = 0.00, string type = "setup")
        {
            bool flag = false;
            type = type.ToLower().Trim() == "setup" ? "" : type;
            double additionalPrincipalAmount = 0.00, totalPayment = 0.00, monthlyPaymentAmount = 0.00, upcomingPaymentAmount = 0.00, lateFee = 0.0, nSFFee = 0.0, otherFee = 0.0, annualMaintenanceFee = 0.0;
            DateTime currentDate = DateTime.Now, helocDate = commonServices.GetHelocWorkingDate(15, 2);
            Hashtable pastAndUpcomingFeesDetails = commonServices.GetPastAndUpcomingFeesForHeloc(loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString()).FirstOrDefault();
            APIConstants.HelocLoanInfo helocLoanInfo = commonServices.GetHelocLoanInfo(loanLevelData);
            Dictionary<string, string> pastDueFeeDic = null;
            if (Convert.ToDateTime(helocLoanInfo.FeeBreakdown.FirstOrDefault().PaymentDueDate)
                .ToString("dd MMM yyyy") == new DateTime(currentDate.Year, currentDate.Month, 1).ToString("dd MMM yyyy"))
            {
                pastDueFeeDic = commonServices.GetHelocFees(helocLoanInfo.FeeBreakdown.FirstOrDefault().Fees);
            }
            Dictionary<string, string> upcomingFeeDic = null;
            if (currentDate > helocDate && Convert.ToDateTime(helocLoanInfo.FeeBreakdown.LastOrDefault().PaymentDueDate)
                .ToString("dd MMM yyyy") == new DateTime(currentDate.Year, currentDate.Month + 1, 1).ToString("dd MMM yyyy"))
            {
                upcomingFeeDic = commonServices.GetHelocFees(helocLoanInfo.FeeBreakdown.LastOrDefault().Fees);
            }

            monthlyPaymentAmount = Convert.ToDouble(GetAmountText(monthlyPaymentAmountTextLocBy).Replace("$", ""));
            ReportingMethods.Log(test, "Monthly Payment Amount: " + "$" + monthlyPaymentAmount.ToString());
            totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            if (type.ToLower() != "edit")
            {
                ReportingMethods.Log(test, "Initial Total Payment : " + "$" + totalPayment.ToString("N"));

                if (currentDate < helocDate)
                {
                    ReportingMethods.LogAssertionFalse(test, webElementExtensions.IsElementDisplayedBasedOnCount(_driver, upcomingPaymentCheckboxLocBy), "Verify Upcoming payment Check Box not displayed before HELOC date : "+ helocDate.ToString("dd MMM yyyy"));
                    if (pastDueFeeDic != null && pastDueFeeDic.ContainsKey(Constants.FeeType.LateCharges))
                    {
                        webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                        flag = webElementExtensions.IsElementDisplayed(_driver, lateFeeAmountCheckboxDisabledCheckedLocBy);
                        _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Late Fee Check Box is Checked intially and  Disabled.",
                                                        "Failure - " + type + " Late Fee Check Box not Checked intially and Disabled.");
                        lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                        double expectedLateFee = Convert.ToDouble(pastDueFeeDic[Constants.FeeType.LateCharges]);
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedLateFee.ToString("N"), "$" + lateFee.ToString("N"), "Verify Late Fees Text box amount is equal to HELOC API Late Fees.");
                    }
                    if (pastDueFeeDic != null && pastDueFeeDic.ContainsKey(Constants.FeeType.NSFFees))
                    {
                        webElementExtensions.ScrollIntoView(_driver, nSFFeeAmountCheckboxLocBy);
                        flag = webElementExtensions.IsElementDisplayed(_driver, nSFFeeAmountCheckboxDisabledCheckedLocBy);
                        _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan NSF Fee Check Box is Checked intially and  Disabled.",
                                                        "Failure - " + type + " NSF Fee Check Box not Checked intially and Disabled.");
                        nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", ""));
                        double expectedNSFFee = Convert.ToDouble(pastDueFeeDic[Constants.FeeType.NSFFees]);
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedNSFFee.ToString("N"), "$" + nSFFee.ToString("N"), "Verify NSF Fees Text box amount is equal to HELOC API NSF Fees.");
                    }
                    if (pastDueFeeDic != null && pastDueFeeDic.ContainsKey(Constants.FeeType.OtherFees))
                    {
                        webElementExtensions.ScrollIntoView(_driver, otherFeeAmountCheckboxLocBy);
                        flag = webElementExtensions.IsElementDisplayed(_driver, otherFeeAmountCheckboxDisabledCheckedLocBy);
                        _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Other Fee Check Box is Checked intially and  Disabled.",
                                                        "Failure - " + type + " Other Fee Check Box not Checked intially and Disabled.");
                        otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                        double expectedOtherFee = Convert.ToDouble(pastDueFeeDic[Constants.FeeType.OtherFees]);
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedOtherFee.ToString("N"), "$" + otherFee.ToString("N"), "Verify Other Fees Text box amount is equal to HELOC API Other Fees.");
                    }
                    if (pastDueFeeDic != null && pastDueFeeDic.ContainsKey(Constants.FeeType.AnnualMaintenanceFees))
                    {
                        webElementExtensions.ScrollIntoView(_driver, annualMaintenanceFeeAmountCheckboxLocBy);
                        flag = webElementExtensions.IsElementDisplayed(_driver, annualMaintenanceFeeAmountCheckboxDisabledCheckedLocBy);
                        _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Annual Maintenance Fee Check Box is Checked intially and  Disabled.",
                                                        "Failure - " + type + " Annual Maintenance Fee Check Box not Checked intially and Disabled.");
                        annualMaintenanceFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, annualMaintenanceFeeTextboxLocBy).Replace("$", ""));
                        double expectedAnnualMaintenanceFee = Convert.ToDouble(pastDueFeeDic[Constants.FeeType.AnnualMaintenanceFees]);
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedAnnualMaintenanceFee.ToString("N"), "$" + annualMaintenanceFee.ToString("N"), "Verify Annual Maintenance Fees Text box amount is equal to HELOC API Annual Maintenance Fees.");
                    }
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.LogAssertionEqual(test, "$" + (monthlyPaymentAmount + lateFee + nSFFee + otherFee + annualMaintenanceFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with Fees calculation");
                    if (_driver.FindElements(addPaymentCheckBoxLocBy).Count > 0)
                    {
                        flag = _driver.FindElement(addPaymentCheckBoxLocBy).GetAttribute(Constants.ElementAttributes.OuterHTML).Contains(Constants.ElementAttributes.Disabled);
                        if (!flag)
                        {
                            ReportingMethods.LogAssertionFalse(test, flag, "Verify " + type + " Additional Payment Check Box is enabled");
                            webElementExtensions.ScrollIntoView(_driver, addPaymentCheckBoxLocBy);
                            webElementExtensions.ClickElementUsingJavascript(_driver, addPaymentCheckBoxLocBy, "Addtional Payment checkbox");
                        }
                    }
                }
            }

            if (currentDate >= helocDate)
            {
                flag = Convert.ToBoolean(webElementExtensions.GetElementAttribute(_driver, upcomingPaymentCheckboxLocBy, Constants.ElementAttributes.AriaChecked));
                if (flag == false)
                {
                    ReportingMethods.LogAssertionFalse(test, flag, "Verify initially Upcoming Payment Check Box is not checked");
                    if (flag == false)
                    {

                        flag = _driver.FindElement(addPaymentCheckBoxLocBy).GetAttribute(Constants.ElementAttributes.OuterHTML).Contains(Constants.ElementAttributes.Disabled);
                        ReportingMethods.LogAssertionTrue(test, flag, "Verify " + type + " Additional Payment Check Box is disabled when Upcoming Payment Check Box is not checked");
                        if (pastDueFeeDic != null && (pastDueFeeDic.ContainsKey(Constants.FeeType.LateCharges) || pastDueFeeDic.ContainsKey(Constants.FeeType.NSFFees)
                           || pastDueFeeDic.ContainsKey(Constants.FeeType.OtherFees) || pastDueFeeDic.ContainsKey(Constants.FeeType.AnnualMaintenanceFees)))
                        {
                            flag = _driver.FindElements(feesSectorNotHiddenLocBy).Count() > 0 ? true : false;
                            ReportingMethods.LogAssertionTrue(test, flag, "Verify " + type + " Fees Section should be displayed when Past Fee present");
                        }
                    }
                }
                if (type.ToLower() != "edit")
                {
                    webElementExtensions.ActionClick(_driver, upcomingPaymentCheckboxLocBy, "Upcoming Payment Checkbox");
                    upcomingPaymentAmount = Convert.ToDouble(GetAmountText(upcomingPaymentAmountTextLocBy).Replace("$", ""));
                    ReportingMethods.Log(test, "Upcoming Payment Amount: " + "$" + upcomingPaymentAmount.ToString());
                    totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                    ReportingMethods.Log(test, "Total Payment After Upcoming Payment Check Box is checked: " + "$" + totalPayment.ToString("N"));
                    if (upcomingFeeDic != null && upcomingFeeDic.ContainsKey(Constants.FeeType.LateCharges))
                    {
                        webElementExtensions.WaitForVisibilityOfElement(_driver, lateFeeAmountCheckboxLocBy);
                        webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                        double expectedLateFee = Convert.ToDouble(upcomingFeeDic[Constants.FeeType.LateCharges]);
                        lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedLateFee.ToString("N"), "$" + lateFee.ToString("N"), "Verify Late Fees Text box amount is equal to HELOC API Late Fees.");
                    }
                    if (upcomingFeeDic != null && upcomingFeeDic.ContainsKey(Constants.FeeType.NSFFees))
                    {
                        webElementExtensions.ScrollIntoView(_driver, nSFFeeTextboxLocBy);
                        double expectedNSFFee = Convert.ToDouble(upcomingFeeDic[Constants.FeeType.NSFFees]);
                        nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", ""));
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedNSFFee.ToString("N"), "$" + nSFFee.ToString("N"), "Verify NSF Fees Text box amount is equal to HELOC API NSF Fees.");
                    }
                    if (upcomingFeeDic != null && upcomingFeeDic.ContainsKey(Constants.FeeType.OtherFees))
                    {
                        webElementExtensions.ScrollIntoView(_driver, otherFeeAmountTextboxLocBy);
                        double expectedOtherFee = Convert.ToDouble(upcomingFeeDic[Constants.FeeType.OtherFees]);
                        otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedOtherFee.ToString("N"), "$" + otherFee.ToString("N"), "Verify Other Fees Text box amount is equal to HELOC API Other Fees.");
                    }
                    if (upcomingFeeDic != null && upcomingFeeDic.ContainsKey(Constants.FeeType.AnnualMaintenanceFees))
                    {
                        webElementExtensions.ScrollIntoView(_driver, annualMaintenanceFeeTextboxLocBy);
                        double expectedAnnualMaintenanceFee = Convert.ToDouble(upcomingFeeDic[Constants.FeeType.AnnualMaintenanceFees]);
                        annualMaintenanceFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, annualMaintenanceFeeTextboxLocBy).Replace("$", ""));
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedAnnualMaintenanceFee.ToString("N"), "$" + annualMaintenanceFee.ToString("N"), "Verify Annual Maintenance Fees Text box amount is equal to HELOC API Annual Maintenance Fees.");
                    }
                    ReportingMethods.LogAssertionEqual(test, "$" + (monthlyPaymentAmount + upcomingPaymentAmount + lateFee + nSFFee + otherFee + annualMaintenanceFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify Total Payment with Upcoming Payment calculation");
                    flag = _driver.FindElement(addPaymentCheckBoxLocBy).GetAttribute(Constants.ElementAttributes.OuterHTML).Contains(Constants.ElementAttributes.Disabled);
                    ReportingMethods.LogAssertionFalse(test, flag, "Verify " + type + " Additional Payment Check Box is enabled when Upcoming Payment Check Box is checked");
                    webElementExtensions.ScrollIntoView(_driver, addPaymentCheckBoxLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, addPaymentCheckBoxLocBy, "Addtional Payment checkbox");
                }
                else
                {
                    upcomingPaymentAmount = Convert.ToDouble(GetAmountText(upcomingPaymentAmountTextLocBy).Replace("$", ""));
                    if (upcomingFeeDic != null && upcomingFeeDic.ContainsKey(Constants.FeeType.LateCharges))
                    {
                        webElementExtensions.WaitForVisibilityOfElement(_driver, lateFeeAmountCheckboxLocBy);
                        webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                        double expectedLateFee = Convert.ToDouble(upcomingFeeDic[Constants.FeeType.LateCharges]);
                        lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedLateFee.ToString("N"), "$" + lateFee.ToString("N"), "Verify " + type + " Late Fees Text box amount is equal to HELOC API Late Fees.");
                    }
                    if (upcomingFeeDic != null && upcomingFeeDic.ContainsKey(Constants.FeeType.NSFFees))
                    {
                        webElementExtensions.ScrollIntoView(_driver, nSFFeeTextboxLocBy);
                        double expectedNSFFee = Convert.ToDouble(upcomingFeeDic[Constants.FeeType.NSFFees]);
                        nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", ""));
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedNSFFee.ToString("N"), "$" + nSFFee.ToString("N"), "Verify " + type + "  NSF Fees Text box amount is equal to HELOC API NSF Fees.");
                    }
                    if (upcomingFeeDic != null && upcomingFeeDic.ContainsKey(Constants.FeeType.OtherFees))
                    {
                        webElementExtensions.ScrollIntoView(_driver, otherFeeAmountTextboxLocBy);
                        double expectedOtherFee = Convert.ToDouble(upcomingFeeDic[Constants.FeeType.OtherFees]);
                        otherFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedOtherFee.ToString("N"), "$" + otherFee.ToString("N"), "Verify " + type + "  Other Fees Text box amount is equal to HELOC API Other Fees.");
                    }
                    if (upcomingFeeDic != null && upcomingFeeDic.ContainsKey(Constants.FeeType.AnnualMaintenanceFees))
                    {
                        webElementExtensions.ScrollIntoView(_driver, annualMaintenanceFeeTextboxLocBy);
                        double expectedAnnualMaintenanceFee = Convert.ToDouble(upcomingFeeDic[Constants.FeeType.AnnualMaintenanceFees]);
                        annualMaintenanceFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, annualMaintenanceFeeTextboxLocBy).Replace("$", ""));
                        ReportingMethods.LogAssertionEqual(test, "$" + expectedAnnualMaintenanceFee.ToString("N"), "$" + annualMaintenanceFee.ToString("N"), "Verify " + type + "  Annual Maintenance Fees Text box amount is equal to HELOC API Annual Maintenance Fees.");
                    }
                }

                if (_driver.FindElements(additionalPrincipalTextboxLocBy).Count > 0)
                {
                    webElementExtensions.ScrollIntoView(_driver, additionalPrincipalTextboxLocBy);
                    if (_driver.FindElement(additionalPrincipalTextboxLocBy).Enabled)
                    {
                        var addprincipal = _driver.FindElement(additionalPrincipalTextboxLocBy);
                        if (type.ToLower() != "edit")
                        {
                            webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPaymentsParam * 100).ToString());
                            ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPaymentsParam));
                        }
                        else
                        {
                            webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalEditPaymentsParam * 100).ToString());
                            ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalEditPaymentsParam));
                        }
                        var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
                        totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
                        additionalPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
                        ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
                        ReportingMethods.LogAssertionEqual(test, "$" + (monthlyPaymentAmount + upcomingPaymentAmount + additionalPrincipalAmount + lateFee + nSFFee + otherFee + annualMaintenanceFee).ToString("N"), "$" + totalPayment.ToString("N"), $"Verify " + type.ToLower() == "setup" ? " " : type + "  Total Payment with Additional Principal calculation");
                    }
                    else
                    {
                        ReportingMethods.Log(test, "Additional Principal Amount Textbox is disabled.");
                    }
                }
            }
            return "$" + totalPayment.ToString("N");
        }

        /// <summary>
        /// Method to Verify Fields of Heloc Payment Fields
        /// </summary>
        /// <param name="loanLevelData">Loan Level Data from DB</param>
        /// <param name="helocLoanInfo">Heloc Loan Info Data from API</param>
        /// <param name="additionalPaymentPrincipalParam">Addtional Payment Principal Amount</param>
        /// <param name="additionalPaymentPrincipalEditParam">Addtional Payment Principal Edit Amount</param>
        /// <param name="type">Payment flow Type -setup/Edit</param>
        /// <returns>total payment</returns>
        public string VerifyHelocUIValidations(Hashtable loanLevelData, APIConstants.HelocLoanInfo helocLoanInfo, double additionalPrincipalParam, double additionalPrincipalEditParam, string type = "setup")
        {
            double totalPayment = 0.00, additionalPaymentPrincipalAmount = 0.00, lateFee = 0.00, nSFFee = 0.00, otherFees = 0.00, annualMaintenanceFee = 0.00, currentPaymentAmount = 0.00;
            double intialTotalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", "")); bool flag;
            ReportingMethods.Log(test, "Intial Total Payment is" + " " + "$" + intialTotalPayment);

            DateTime currentDate = DateTime.Now;
            DateTime helocDate = commonServices.GetHelocWorkingDate(15, 2);
            if (currentDate >= helocDate && Convert.ToInt32(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentCount]) == 0)
            {
                intialTotalPayment = 0.0;
                currentPaymentAmount = Convert.ToDouble(GetAmountText(currentPaymentAmountLocBy).Replace("$", ""));
                webElementExtensions.WaitForElement(_driver, currentPaymentCheckboxInputLocBy);
                flag = webElementExtensions.GetElementAttribute(_driver, currentPaymentCheckboxInputLocBy, Constants.ElementAttributes.AriaChecked) == "true" ? true : false;
                ReportingMethods.LogAssertionTrue(test, flag, "Verify Current Payment checkbox is checked by default");
                flag = _driver.FindElement(currentPaymentCheckboxInputLocBy).GetAttribute(Constants.ElementAttributes.OuterHTML).Contains(Constants.ElementAttributes.Disabled);
                ReportingMethods.LogAssertionTrue(test, flag, "Verify Current Payment checkbox is Read Only");

                if (webElementExtensions.GetElementAttribute(_driver, additionalPaymentCheckboxLocBy, Constants.ElementAttributes.AriaChecked) != "true")
                {
                    ReportingMethods.Log(test, "Verify initially Addtional Payment checkbox is not checked");
                    webElementExtensions.ClickElementUsingJavascript(_driver, additionalPaymentCheckboxLocBy, "Addtional Payment checkbox");
                }
            }

            webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (Constants.AmountValues.MaximumAdditionalPrincipalPaymentValue * 100).ToString());
            ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(Constants.AmountValues.MaximumAdditionalPrincipalPaymentValue));
            ReportingMethods.LogAssertionFalse(test, webElementExtensions.IsElementDisplayed(_driver, maxPrincipalErrorTextLocBy), "Verify Maximum Additional Principal Amount Error Text is not displayed");

            webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, ((Constants.AmountValues.MaximumAdditionalPrincipalPaymentValue + 1) * 100).ToString());
            ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble((Constants.AmountValues.MaximumAdditionalPrincipalPaymentValue + 1)));
            webElementExtensions.IsElementDisplayed(_driver, maxPrincipalErrorTextLocBy, "Maximum Additional Principal Amount Error Text");
            ReportingMethods.LogAssertionEqual(test, webElementExtensions.GetElementText(_driver, maxPrincipalErrorTextLocBy).Trim(), Constants.ErrorMessages.MaximumAdditionalPrincipalPaymentAcceptedIs25000, "Verify Maximum Additional Principal Amount Error Text");

            double uPB = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.FirstPrincipalBalance]);
            if (uPB > 0 && uPB < Convert.ToDouble(Constants.AmountValues.MaximumAdditionalPrincipalPaymentValue))
            {
                webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, ((uPB + 1) * 100).ToString());
                ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + uPB + 1);
            }
            totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            if (totalPayment > uPB && totalPayment <= Convert.ToDouble(Constants.AmountValues.MaximumAdditionalPrincipalPaymentValue))
            {
                webElementExtensions.IsElementDisplayed(_driver, totalPaymentGreaterUPBErrorTextLocBy, "Total Payment Amount Greater than Unpaid Principal Balance Error Text");
                ReportingMethods.LogAssertionEqual(test, webElementExtensions.GetElementText(_driver, totalPaymentGreaterUPBErrorTextLocBy).Trim(), Constants.ErrorMessages.TotalPaymentAmountGreaterThanUnpaidPrincipalBalance, "Verify Total Payment Amount Greater than Unpaid Principal Balance Error Text");
            }

            if (type == "setup")
            {
                webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalParam * 100).ToString());
                ReportingMethods.Log(test, "Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalParam));
            }
            else
            {
                webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, (additionalPrincipalEditParam * 100).ToString());
                ReportingMethods.Log(test, "Edit Additional Principal Amount Entered: " + "$" + Convert.ToDouble(additionalPrincipalEditParam));
            }

            Dictionary<string, string> feeDic = null;
            if (helocLoanInfo.Fees.Count > 0)
            {
                feeDic = commonServices.GetHelocFees(helocLoanInfo.Fees);
            }
            if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.LateCharges))
            {
                webElementExtensions.ScrollIntoView(_driver, lateFeeAmountCheckboxLocBy);
                flag = webElementExtensions.IsElementDisplayed(_driver, lateFeeAmountCheckboxDisabledCheckedLocBy);
                _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Late Fee Check Box is Checked intially and Disabled.",
                                                "Failure - " + type + " Late Fee Check Box not Checked intially and Disabled.");
                lateFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, lateFeeTextboxLocBy).Replace("$", ""));
                double expectedLateFee = Convert.ToDouble(feeDic[Constants.FeeType.LateCharges]);
                ReportingMethods.LogAssertionEqual(test, "$" + expectedLateFee.ToString("N"), "$" + lateFee.ToString("N"), "Verify Late Fees Text box amount is equal to HELOC API Late Fees.");
            }
            if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.NSFFees))
            {
                webElementExtensions.ScrollIntoView(_driver, nSFFeeAmountCheckboxLocBy);
                flag = webElementExtensions.IsElementDisplayed(_driver, nSFFeeAmountCheckboxDisabledCheckedLocBy);
                _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan NSF Fee Check Box is Checked intially and  Disabled.",
                                                "Failure - " + type + " NSF Fee Check Box not Checked intially and Disabled.");
                nSFFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, nSFFeeTextboxLocBy).Replace("$", ""));
                double expectedNSFFee = Convert.ToDouble(feeDic[Constants.FeeType.NSFFees]);
                ReportingMethods.LogAssertionEqual(test, "$" + expectedNSFFee.ToString("N"), "$" + nSFFee.ToString("N"), "Verify NSF Fee Text box amount is equal to HELOC API NSF Fees.");
            }
            if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.OtherFees))
            {
                webElementExtensions.ScrollIntoView(_driver, otherFeeAmountCheckboxLocBy);
                flag = webElementExtensions.IsElementDisplayed(_driver, otherFeeAmountCheckboxDisabledCheckedLocBy);
                _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Other Fees Check Box is Checked intially and  Disabled.",
                                                "Failure - " + type + " Other Fees Check Box not Checked intially and Disabled.");
                otherFees = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, otherFeeAmountTextboxLocBy).Replace("$", ""));
                double expectedOtherFees = Convert.ToDouble(feeDic[Constants.FeeType.OtherFees]);
                ReportingMethods.LogAssertionEqual(test, "$" + expectedOtherFees.ToString("N"), "$" + otherFees.ToString("N"), "Verify Other Fees Text box amount is equal to HELOC API Other Fees.");
            }
            if (feeDic != null && feeDic.ContainsKey(Constants.FeeType.AnnualMaintenanceFees))
            {
                webElementExtensions.ScrollIntoView(_driver, annualMaintenanceFeeAmountCheckboxLocBy);
                flag = webElementExtensions.IsElementDisplayed(_driver, annualMaintenanceFeeAmountCheckboxDisabledCheckedLocBy);
                _driver.ReportResult(test, flag, "Successfully verified " + type + " that Heloc plan Annual Maintenance Fee Check Box is Checked intially and  Disabled.",
                                                "Failure - " + type + " Annual Maintenance Check Box not Checked intially and Disabled.");
                annualMaintenanceFee = Convert.ToDouble(webElementExtensions.GetElementTextUsingJS(_driver, annualMaintenanceFeeTextboxLocBy).Replace("$", ""));
                double expectedAnnualMaintenanceFee = Convert.ToDouble(feeDic[Constants.FeeType.AnnualMaintenanceFees]);
                ReportingMethods.LogAssertionEqual(test, "$" + expectedAnnualMaintenanceFee.ToString("N"), "$" + annualMaintenanceFee.ToString("N"), "Verify Annual Maintenance Fee Text box amount is equal to HELOC API Other Fees.");
            }
            var addprincipalAmount = commonServices.GetValueUsingJS(additionalPrincipalTextboxLocBy);
            totalPayment = Convert.ToDouble(GetAmountText(divTotalAmountLocBy).Replace("$", ""));
            additionalPaymentPrincipalAmount = Convert.ToDouble(addprincipalAmount.ToString().Trim().Replace("$", ""));
            if (type != "setup")
            {
                additionalPaymentPrincipalAmount = additionalPrincipalEditParam - additionalPrincipalParam;
            }
            ReportingMethods.Log(test, "Total Payment After Additional Principal Amount Entered: " + "$" + totalPayment.ToString("N"));
            ReportingMethods.LogAssertionEqual(test, "$" + (intialTotalPayment + currentPaymentAmount + lateFee + nSFFee + otherFees + annualMaintenanceFee + additionalPaymentPrincipalAmount).ToString("N"), "$" + totalPayment.ToString("N"), type == "setup" ? $"Verify Total Payment with Additional Principal calculation" : $"Verify " + type + " Total Payment with Additional Principal calculation");

            return "$" + totalPayment.ToString("N");
        }

        #endregion Heloc Services

    }
}
