using AventStack.ExtentReports;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Utilities;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.XPath;
using static LD_AutomationFramework.APIConstants;
using Keys = OpenQA.Selenium.Keys;

namespace LD_AutomationFramework.Pages
{
    /// <summary>
    /// Bank Account Represents
    /// </summary>
    public class BankAccount
    {
        public string Nickname { get; set; }
        public string Name { get; set; }
        public string AcntType { get; set; }
        public string Routing { get; set; }
        public string AcntNumber { get; set; }
        public string BankName { get; set; }
        public bool Default { get; set; }

        public override string ToString()
        {
            return $"{Name}:{AcntType}:{AcntNumber}";
        }
    }
    public class CommonServicesPage
    {
        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }
        WebElementExtensionsPage webElementExtensions { get; set; }
        YopmailPage yopMail { get; set; }
        DBconnect dBconnect { get; set; }
        ReportLogger reportLogger { get; set; }

        public List<string> loanNumbersInUse = new List<string>();

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
        public CommonServicesPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            dBconnect = new DBconnect(test, "MelloServETL");
            reportLogger = new ReportLogger(_driver);            
        }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static bool successfulLoginDone = false;

        #region AgentPortalLocators

        public By apEnterLoanNumberInputFieldLocBy = By.Id("mat-input-0");
        public By apUsernameFieldLocBy = By.Name("loginfmt");
        public By useYourPasswordInsteadLinkLocBy = By.Id("idA_PWD_SwitchToPassword");
        public By apPasswordFieldLocBy = By.Name("passwd");
        public By apUsernameErrorMessageLocBy = By.Id("usernameError");
        public By apPasswordErrorMessageLocBy = By.Id("passwordError");
        public By apNextButtonLocBy = By.CssSelector("input[value='Next']");
        public By apSignInButtonLocBy = By.CssSelector("input[value='Sign in']");
        public By apStaySignedInNoButtonLocBy = By.CssSelector("input[value='No']");
        public By timeZoneValueInBorrowersLocalTimeLocBy = By.CssSelector("mello-serve-ui-loan-info-header div span[class='pl-1']");
        public By agentPortalPageHeaderLocBy = By.CssSelector("mello-serve-ui-header");
        public By somethingWrongMessageAlertPopupMainDivLocBy = By.CssSelector("div[id*='cdk-overlay']");
        public By pickAnAccountHeaderLocBy = By.Id("loginHeader");
        public By pickAnAccountPageSignedInUserSectionLocBy = By.CssSelector("div[data-test-id]");
        public By useAnotherAccountLocBy = By.Id("otherTileText");
        public string apNavigationTabsLocBy = "//div[@class='mat-tab-label-content' and text()='<TABNAME>']";

        #endregion AgentPortalLocators

        #region CustomerPortalLocators

        public By cpUsernameLocBy = By.Id("email");
        public By cpPasswordLocBy = By.Id("password");
        public By cpSignInbuttonLocBy = By.Id("next");
        public By cpLinkYourExistingLoanPage_LoanNumberFieldLocBy = By.Id("loanNumber");
        public By cpLinkYourExistingLoanPage_ZipcodeFieldLocBy = By.CssSelector("[formcontrolname='zipCode']");
        public By cpLinkYourExistingLoanPage_SsnNumberLocBy = By.CssSelector("[formcontrolname='ssn']");
        public By cpLinkYourExistingLoanPage_linkMyLoanButtonLocBy = By.XPath("//button[.='LINK MY LOAN']");
        public By cpLinkALoanPage_linkLoanButtonLocBy = By.XPath("//button[.='LINK LOAN']");
        public By cpMyLoansDropdownLocBy = By.CssSelector("div[id='navbarNavDropdown'] li button img");
        public By cpAddYourLoanButtonLocBy = By.CssSelector("ul[class*='navbar'] a[class*='dropdown-loanlink']");
        public By noThanksButtonLocBy = By.Id("no-thanks-link");
        public By chatBotCloseIconLocBy = By.CssSelector("button[aria-label='Hide ServisBOT Messenger']");
        public By spinnerLocBy = By.CssSelector("svg[class='spinner']");
        public By cpLoginScreenLocBy = By.Id("sign-up-sign-in");
        public By logoutDropdownArrowIconLocBy = By.XPath("//*[contains(text(),'arrow_drop_down')]");
        public By signOutOptionLocBy = By.XPath("//*[contains(text(),'Sign Out')]");
        public By nextMonthButtonCalanderLocBy = By.XPath("//button[@aria-label='Next month']");
        public By prevMonthButtonCalanderLocBy = By.XPath("//button[@aria-label='Previous month']");
        public By confirmOtpPaymentBtnLocBy = By.XPath("//span[contains(text(),'Confirm Payment')]");
        public By bankAccountsLocBy = By.XPath(".//mat-radio-button[contains(@class,'mat-radio-checked')]");
        public By cpSignUpNowLinkLocBy = By.CssSelector("[id='createAccount']");
        public By cpFirstNameTextInputLocBy = By.CssSelector("[id='extension_FirstName']");
        public By cpLastNameTextInputLocBy = By.CssSelector("[id='extension_LastName']");
        public By cpSendVerificationCodeButtonLocBy = By.CssSelector("[id='emailVerificationControl_but_send_code']");
        public By cpVerificationCodeInputLocBy = By.CssSelector("[id='emailVerificationCode']");
        public By cpVerifYCodeButtonLocBy = By.CssSelector("[id='emailVerificationControl_but_verify_code']");
        public By cpNewPasswordInputFieldLocBy = By.CssSelector("[id='newPassword']");
        public By cpReEnterNewPasswordInputFieldLocBy = By.CssSelector("[id='reenterPassword']");
        public By cpCreateAndSignInButtonLocBy = By.CssSelector("[id='continue']");
        public By cpForgotYourEmailLinkLocBy = By.CssSelector("[id='forgotEmailLink']");
        public By cpForgotYourPasswordLinkLocBy = By.CssSelector("[id='forgotPassword']");
        public By cpVerifyButtonOnForgotYourEmailPageLocBy = By.CssSelector("[id='Verify']");
        public By cpEmailOnPasswordResetPageLocBy = By.CssSelector("[id='email']");
        public By cpForgotEmailVerifiedPageLocBy = By.CssSelector("mello-serve-ui-email-found");
        public By cpSignInOnForgotEmailVerifiedPageButtonLocBy = By.CssSelector("[id='SignIn']");
        public By cpCreateYourPasswordContentLocBy = By.CssSelector("[id='forgot-password'] div[class*='text-center']");
        public string addAnAccountPopupErrorMsgs = "//*[text()='<ERROR>']";


        #endregion CustomerPortalLocators

        #region MSPLocators

        public By mspUsernameLocBy = By.Id("userNameInput");
        public By mspNewLoginScreenUsernameLocBy = By.Id("username_input");
        public By mspNewLoginScreenPasswordLocBy = By.Id("password_input1");
        public By mspNewLoginScreenNextButtonLocBy = By.Name("loginPageNextBtn");
        public By mspNextButtonLocBy = By.Id("nextButton");
        public By mspLoginButtonLocBy = By.Name("loginPageBtn");
        public By twoFAPasscodeInputFieldLocBy = By.Id("otpcode_input");
        public By mspClearSessionButtonLocBy = By.Id("btnClearSessn");
        public By mspClearSessionOkButtonLocBy = By.CssSelector("button[class*='messagebox_button_done']");
        public By userMenuLocBy = By.CssSelector("button[onclick*='userNameMenu']");
        public By logOffOptionLocBy = By.XPath("//div[text()='Log Off']");
        public By successfulLogoutMessageLocBy = By.XPath("//div[contains(text(),'You have successfully signed out of the application')]");
        public By directorOptionLocBy = By.XPath("//section[@id='servicing']//a[text()='Director']");

        #endregion MSPLocators

        #region CommonLocators

        public By loadingIconLocBy = By.CssSelector("mello-serve-ui-circle-spinner div[id='pause']");

        public By pushDeleteToMSPButtonLocBy = By.XPath("//mello-serve-ui-payment-summary-wrapper//mello-serve-ui-automatic-payments//button[@id='btnPushToDelete' and not(@disabled)]");

        public By pushDeleteToMSPButtonDisabledLocBy = By.XPath("//mello-serve-ui-payment-summary-wrapper//mello-serve-ui-automatic-payments//button[@id='btnPushToDelete' and @disabled]");

        public By deleteAutopayPopupTextLocBy = By.XPath("//mello-serve-ui-autopay-confirm-delete//div[text()='Please select a reason for deleting autopay.']");

        public By keepAutopayButtonLocBy = By.Id("cancelBtn");

        public string deleteReasonRadioButton = "//mat-radio-group[@aria-labelledby='Reason']//span[contains(text(),'<AUTOPAYDELETEREASON>')]";

        public By deleteButtonInDeleteAutpayPopupLocBy = By.XPath("//mello-serve-ui-autopay-confirm-delete//button[@id='deleteBtn']");

        public By disabledDeleteButtonInDeleteAutpayPopupLocBy = By.XPath("//mello-serve-ui-autopay-confirm-delete//button[@id='deleteBtn' and @disabled]");

        public By paymentSummarySectionLocBy = By.Id("matCardIdLoanInfo");

        public By manageAutopayButtonEnabledLocBy = By.XPath("//mello-serve-ui-automatic-payments//button[@id='btnManageAutopay' and not(@disabled)]");

        public By manageAutopayButtonDisabledLocBy = By.XPath("//mello-serve-ui-automatic-payments//button[@id='btnManageAutopay' and @disabled]");

        public By setupAutopaybuttonLocBy = By.XPath("//span[contains(text(),'Setup Autopay')]//parent::button");

        public By bankAccountDropdownLocBy = By.XPath("//mat-select[@id='bankAccountSelect']//div[contains(@id,'mat-select')]");

        public By bankAccountDropdownArrowIconLocBy = By.XPath("//mat-select[@id='bankAccountSelect']//div[contains(@id,'mat-select')]//following-sibling::div[contains(@class,'arrow')]");

        public By authorizedByDropdownLocBy = By.XPath("//div[text()='Authorized By']//following-sibling::div//div[contains(@id,'mat-select')]");

        public string authorizedByDropdownValue = "//span[@class='mat-option-text']//span[contains(text(),'<BORROWERNAME>')]";

        public By authorizedByDropdownValueSelectedLocBy = By.CssSelector("mat-select[id='borrowerSelect'] span[class*='mat-select-min-line']");

        public By authorizedByDropdownMainLocatorLocBy = By.Id("borrowerSelect");

        public By bankAccountSelectLocBy = By.Id("bankAccountSelect");

        public By emptyBankAccountDropdownLocBy = By.XPath("//label[@for='bankAccountSelect' and contains(@class,'empty')]//mat-label[text()='Please add a bank account for payment']");

        public string bankAccountDropdownValue = "//span[@class='mat-option-text']//span[contains(text(),'<BANKACCOUNT>')]";

        public By allValuesInbankAccountDropdownLocBy = By.CssSelector("span[class='mat-option-text'] span");

        public By bankAccountDropdownValueSelectedLocBy = By.CssSelector("mat-select[id='bankAccountSelect'] span[class*='mat-select-min-line']");

        public By manageBankAccountsLinkEnabledLocBy = By.XPath("//button[contains(.,'Manage Accounts') and not(@disabled)]");

        public By addAnAccountLinkLocBy = By.Id("add-account-link");

        public By accountNicknameLocBy = By.Id("accountNickNameInput");

        public By personalRadioButtonLocBy = By.Id("personalTypeButton");

        public By businessRadioButtonLocBy = By.Id("businessTypeButton");

        public By checkingRadioButtonLocBy = By.Id("accountTypeButton");

        public By savingsRadioButtonLocBy = By.Id("SavingsButton");

        public By firstNameOnAccountTextboxLocBy = By.Id("firstNameInput");

        public By lastNameOnAccountTextboxLocBy = By.Id("lastNameInput");

        public By routingNumberTextboxLocBy = By.Id("routingNumberInput");

        public By bankNameDisplayedBelowRoutingNumberFieldLocBy = By.XPath("//mat-hint//span[contains(text(),'Loan Depot')]");

        public By accountNumberTextboxLocBy = By.Id("accountNumber");

        public By confirmAccountNumberTextboxLocBy = By.Id("accountNumberConfirm");

        public By makeThisTheDefaultAccountCheckboxLocBy = By.Id("MakeDefaultAccountCheckbox");

        public By saveBankAccountCheckboxLocBy = By.Id("savedCheckbox");

        public By addAccountButtonLocBy = By.Id("addAccountFormGroupBtn");

        public string bankAccountsTableCellValues = "//mat-card[@id='cardIdManageAccounts']//table[@aria-label='Bank Accounts']//tr[contains(@class,'mat-row')][<ROWNUMBER>]//td[<COLUMNNUMBER>]";

        public By bankAccountsTableRows = By.XPath("//mat-card[@id='cardIdManageAccounts']//table[@aria-label='Bank Accounts']//tr[contains(@class,'mat-row')]");

        public By backToPaymentsButtonLocBy = By.CssSelector("div[class='container-lg'] a[class$='backtoInbox']");

        public By monthlyPaymentPlanAmountTextLocBy = By.CssSelector("[id='monthlyPaymentElmt'] + span"); //Need to be removed from CP Payments page

        public By amountRadioButtonLocBy = By.Id("partialReinstatementRadioBtn");

        public By amountInputFieldLocBy = By.Id("partialReinstatementInput");

        public By pastDueAmountLocBy = By.CssSelector("div[id='pastdueAmountElmt'] span[class*='right']");

        public By additionalPaymentCheckboxLocBy = By.Id("additionalPaymentCheckbox-input");

        public By additionalPrincipalTextboxLocBy = By.XPath("//input[@id='additionalPrincipalInput']");

        public By paymentDatePickerIconLocBy = By.CssSelector("button[aria-label='Open calendar']");

        public By paymentDateCalendarPopUpLocBy = By.CssSelector("mat-calendar[id*='mat-datepicker']");

        public string paymentDateTobeSelectedIsEnabledOrDisabledLocBy = "td[aria-label='<DATETOBESELECTED>']";

        public By paymentDateTextboxWithDateSelectedLocBy = By.Id("paymtDateId");

        public string paymentDateTobeSelected = "td[aria-label*='<DATETOBESELECTED>'] div[class*='mat-focus']";

        public By paymentDateAlreadySelected = By.XPath("//div[contains(@class,'mat-calendar-body-selected')]/..");

        public By addBankAccountPopUpLocBy = By.CssSelector("[id='addaNewBankAccountDivBlock']");

        public By cancelButtonAddAnAccountPopupLocBy = By.CssSelector("button[id='btnAddAcctCloseModal']");

        public By fccNotRequiredOptionLocBy = By.CssSelector("[id='ffcNoButton']");

        public By fccRequiredOptionLocBy = By.CssSelector("[id='ffcYesButton']");
        #region AutopayLocators

        public By additionalMonthlyPrincipalMaximumErrorMessageLocBy = By.Id("monthlyPaymentMaxPrinciErrortext");

        public By additionallyMonthlyPrincipalInputLocBy = By.Id("monthlyPaymentAddnPrinciInput");

        public By additionalMonthlyPrincipalCheckBoxInputLocBy = By.CssSelector("[formcontrolname='monthlyAdditionalPrincipalCheckbox'] input");

        public By additionalMonthlyPrincipalDisabledCheckBoxInputLocBy = By.CssSelector("[formcontrolname='monthlyAdditionalPrincipalCheckbox'] input[disabled]");

        public By additionalMonthlyPrincipalCheckBoxLabelLocBy = By.CssSelector("[formcontrolname='monthlyAdditionalPrincipalCheckbox'] label span");

        public By setupAutopayButtonInSetupAutopayScreenLocBy = By.XPath("//button//span[contains(text(),'Setup Autopay')]");

        public By confirmAutopayButtonLocBy = By.XPath("//span[contains(text(),'Confirm Autopay')]");

        public By continueButtonInConfirmAutopayScreenLocBy = By.Id("okBtn");

        public By manageAutoPayHeaderTextLocBy = By.Id("spidPaymentActivity");

        public By setupAutopayHeaderLocBy = By.CssSelector("[id='paymentForm'] span[id='spidPaymentActivity']");

        public By planForAutopayPaymentManageAutopayTextLocBy = By.Id("spIdFrequency_");

        public By nextDebitDateManageAutoPayTextLocBy = By.Id("sppaymentDate_");

        public By amountOnManageAutoPayTextLocBy = By.Id("spIdmonthlyPaymentAmount_");

        public By autopayEditIconLocBy = By.Id("linkIdActionEdit_");

        public By updateAutopayLocBy = By.XPath("//button//span[contains(text(),'Update Autopay')]");

        public By additionalPrincipalCheckboxInAutopayPageLocBy = By.CssSelector("mat-checkbox[formcontrolname='monthlyAdditionalPrincipalCheckbox']");

        public By additionalPrincipalInputFieldInAutopayPageLocBy = By.CssSelector("input[formcontrolname='monthlyAdditionalPrincipal']");

        #endregion AutopayLocators

        public By addlMonthlyPrincipalPaymentPlacedAfterPlanLocBy = By.XPath("//div[@id='monthlypaymentContainer']//following-sibling::div[@id='monthlyPaymentAddnPrincipalElement']");

        public By updatePaymentButtonLocBy = By.XPath("//*[text()='Update Payment']//ancestor::button");

        public By amountInAutopaySuccessfullyScheduledPageLocBy = By.CssSelector("ldsm-auto-payment-confirm div[id='print-section'] p[class*='semi-bold']");

        public By backToManageAutoPayButtonLocBy = By.Id("btnIdSearchSubmit");

        public By backtoAutopaySetupBtnLocBy = By.Id("backToAutopayLabelId");

        public By closeWindowButtonLocBy = By.XPath("//span[contains(text(),'Close Window')]");

        public string amountSectionInOtpSetupScreen = "//ldsm-otp-payment-amount-form-ctrl//*[contains(text(),'<AMOUNTSECTIONFIELDNAME>')]";

        public By feeSubSectionInAmountSectionOfOtpScreenLocBy = By.Id("FeeSector");

        public By lateFeeMessageInOtpScreenLocBy = By.Id("latefeeInfoMsg1");

        public By monthYearOnCalendarTextBy = By.CssSelector("span[id^='mat-calendar-button-']");

        public By datesAvailableToSelectOnCalndrLocBy = By.XPath("//td[contains(@class,'mat-calendar') and not(@aria-disabled)]/div[contains(@class,'content')]");

        public By accountNumberAlreadyExistsPopupLocBy = By.CssSelector("button[id='btnClose']");

        public string divLocatorWithSpecificText = "//div[text()='{0}']";

        public string spanByText = "//span[text()='<TEXT>']";

        public string divByText = "//div[text()='<TEXT>']";

        public By disclosureLabelLocBy = By.XPath("//p[contains(text(),'I am required to read you a disclosure.')]/following-sibling::p");

        public By confirmationNumberTextLocBy = By.XPath("//div[text()='Confirmation Number']/following-sibling::div");

        public By deleteIconManageBankAccountLocBy = By.CssSelector("td[class*='delete'] img");

        public string deleteButtonBasedOnAccountNumber = "//td[contains(text(),'<ACCOUNT_NUMBER>')]//following-sibling::td[contains(@class,'delete')]//img";

        public By deleteButtonOnDeleteBankAccountPopupLocBy = By.XPath("//button//span[text()='Delete']");

        public By closeButtonOnBankAccountCannotBeDeletedPopupLocBy = By.XPath("//button//span[text()='Close']");

        #region PaymentBreakdown

        public By paymentBreakdownLocBy = By.Id("pIdPayBreakdown");

        public By principalAndInterestLocBy = By.Id("divIdPayPrincipalInt");

        public By taxesAndOrInsuranceLocBy = By.Id("divIdTaxInsurance");

        #endregion PaymentBreakdown

        #region UnpaidPrincipalBalance

        public By unpaidPrincipalBalanceLocBy = By.Id("pIdPayUnpaid");

        public By principalPaidLocBy = By.Id("divIdPrincipal");

        public By escrowBalanceLocBy = By.Id("divIdEscrowBalance");

        #endregion UnpaidPrincipalBalance        

        public string buttonByText = "//button[contains(.,'<BUTTONNAME>')]";

        public string containsTextLocator = "//*[contains(.,'<LABELNAME>')]";

        public string linkByText = "//a[contains(.,'<TEXT>')]";

        public By oopsSomethingWentWrongPopupLocBy = By.XPath("//h5[text()='Oops! Something went wrong.']");

        public By leaveButtonLocBy = By.XPath("//span[text()='Leave']");

        #endregion CommonLocators

        #region DateLocators

        public By disabledPaymentDatesLocBy = By.XPath("//td[@role and @aria-disabled]");

        public By maxEnabledPaymentDateLocBy = By.XPath("//tr[last()]//td[@role='gridcell' and not(contains(@class,'mat-calendar-body-disabled'))][last()]");

        public By enabledPaymentDatesLocBy = By.XPath("//td[@role='gridcell' and not(contains(@class,'mat-calendar-body-disabled'))]");

        public By activePaymentDateTobeSelectedLocBy = By.XPath("//td[contains(@class,'mat-calendar-body-cell') and not(contains(@class, 'disabled'))]");

        public By monthYearSpanLocBy = By.XPath("//div[@class='mat-calendar-controls']//span[contains(@id,'mat-calendar-button')]");

        public By nextMonthLocBy = By.XPath("//button[@aria-label='Next month']");

        public By tablePendingPaymentLocBy = By.Id("tblIdPaymentPending");

        #endregion DateLocators

        #region Services

        #region LoginLogoutMethods

        ///<summary>
        ///Method to perform login to the application
        ///<param name="userName">username@domain.com</param>
        ///<param name="password">Password corresponding to the above username</param>
        ///</summary>
        public bool LoginToTheApplication(string username, string password)
        {
            bool flag = false;
            string rootDirectory = string.Empty;
            yopMail = new YopmailPage(_driver, test);
            try
            {
                rootDirectory = UtilAdditions.GetRootDirectory();
                if (rootDirectory.Contains(Constants.SolutionProjectNames.AgentPortal))
                {
                    webElementExtensions.EnterText(_driver, apUsernameFieldLocBy, username, true, "Username");
                    webElementExtensions.ActionClick(_driver, apNextButtonLocBy, "Next button.");
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, apUsernameErrorMessageLocBy))
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, apNextButtonLocBy);
                        if (username.Contains("test"))
                        {
                            webElementExtensions.ClickElement(_driver, useYourPasswordInsteadLinkLocBy);
                            webElementExtensions.WaitForVisibilityOfElement(_driver, apPasswordFieldLocBy);
                            webElementExtensions.EnterText(_driver, apPasswordFieldLocBy, password, true, "Password");
                            webElementExtensions.ActionClick(_driver, apSignInButtonLocBy, "Sign In button.");
                            if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, apPasswordErrorMessageLocBy))
                            {
                                if (webElementExtensions.WaitForInvisibilityOfElement(_driver, apSignInButtonLocBy))
                                {
                                    webElementExtensions.WaitUntilElementIsClickable(_driver, apStaySignedInNoButtonLocBy);
                                    webElementExtensions.MoveToElement(_driver, apStaySignedInNoButtonLocBy);
                                    flag = webElementExtensions.ClickElementUsingJavascript(_driver, apStaySignedInNoButtonLocBy, "No button of Stay signed In page.");
                                    webElementExtensions.WaitForInvisibilityOfElement(_driver, apStaySignedInNoButtonLocBy);
                                    if (_driver.FindElements(oopsSomethingWentWrongPopupLocBy).Count > 0)
                                    {
                                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                                        _driver.Navigate().Refresh();
                                    }
                                }
                            }
                        }
                    }
                    else
                        flag = true;
                }
                else if (rootDirectory.Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    webElementExtensions.WaitForElement(_driver, cpUsernameLocBy, ConfigSettings.WaitTime);
                    webElementExtensions.EnterText(_driver, cpUsernameLocBy, username, true, "Username");
                    webElementExtensions.WaitForElement(_driver, cpSignInbuttonLocBy);
                    webElementExtensions.EnterText(_driver, cpPasswordLocBy, password, true, "Password");
                    webElementExtensions.WaitForElement(_driver, cpSignInbuttonLocBy);
                    flag = webElementExtensions.ClickElementUsingJavascript(_driver, cpSignInbuttonLocBy, "Sign In button.");
                    reportLogger.TakeScreenshot(test, "'Login Page After Entering Credential'");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, cpSignInbuttonLocBy);
                }
                else if (rootDirectory.Contains(Constants.SolutionProjectNames.MSP))
                {
                    webElementExtensions.EnterText(_driver, mspUsernameLocBy, username, true);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, mspNextButtonLocBy);
                    webElementExtensions.ActionClick(_driver, mspNextButtonLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, mspNewLoginScreenUsernameLocBy);
                    webElementExtensions.EnterText(_driver, mspNewLoginScreenUsernameLocBy, username, true, "Username");
                    webElementExtensions.WaitForVisibilityOfElement(_driver, mspNewLoginScreenNextButtonLocBy);
                    webElementExtensions.ActionClick(_driver, mspNewLoginScreenNextButtonLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, mspNewLoginScreenPasswordLocBy);
                    webElementExtensions.EnterText(_driver, mspNewLoginScreenPasswordLocBy, password, true, "Password");
                    webElementExtensions.WaitForElement(_driver, mspLoginButtonLocBy);
                    webElementExtensions.ActionClick(_driver, mspLoginButtonLocBy, "Login button");
                    string twoFAPasscode = yopMail.GetEmailContentFromYopmail(username);
                    webElementExtensions.EnterText(_driver, twoFAPasscodeInputFieldLocBy, twoFAPasscode, true, "2 FA Passcode");
                    webElementExtensions.ActionClick(_driver, mspLoginButtonLocBy, "Login button");
                    flag = webElementExtensions.WaitForInvisibilityOfElement(_driver, mspLoginButtonLocBy);
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while logging in to the application: " + ex.Message);
            }
            finally
            {
                successfulLoginDone = flag;
                _driver.ReportResult(test, flag, "Successfully logged into the application with the username: " + @"<b>" + username + @"</b>" + ".", "Failed while logging into the application with the username:" + username + ".");
                if (!flag)
                    Assert.Fail("Failed while logging into the application.");
            }
            return flag;
        }

        ///<summary>
        ///Method to perform login to the Agent Portal application
        ///<param name="userName">username@domain.com</param>
        ///<param name="password">Password corresponding to the above username</param>
        ///</summary>
        public bool LoginToAgentPortalApplication(string username, string password)
        {
            bool flag = false;
            string rootDirectory = string.Empty;
            try
            {
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, logoutDropdownArrowIconLocBy))
                {
                    test.Log(Status.Info, "User already logged into Agent Portal application");
                    flag = true;
                }
                else
                {
                    webElementExtensions.EnterText(_driver, apUsernameFieldLocBy, username, true, "Username");
                    webElementExtensions.ActionClick(_driver, apNextButtonLocBy, "Next button.");
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, apUsernameErrorMessageLocBy))
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, apNextButtonLocBy);
                        if (username.Contains("test"))
                        {
                            webElementExtensions.ClickElement(_driver, useYourPasswordInsteadLinkLocBy);
                            webElementExtensions.WaitForVisibilityOfElement(_driver, apPasswordFieldLocBy);
                            webElementExtensions.EnterText(_driver, apPasswordFieldLocBy, password, true, "Password");
                            webElementExtensions.ActionClick(_driver, apSignInButtonLocBy, "Sign In button.");
                            if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, apPasswordErrorMessageLocBy))
                            {
                                if (webElementExtensions.WaitForInvisibilityOfElement(_driver, apSignInButtonLocBy))
                                {
                                    webElementExtensions.WaitUntilElementIsClickable(_driver, apStaySignedInNoButtonLocBy);
                                    webElementExtensions.MoveToElement(_driver, apStaySignedInNoButtonLocBy);
                                    webElementExtensions.ClickElementUsingJavascript(_driver, apStaySignedInNoButtonLocBy, "No button of Stay signed In page.");
                                    webElementExtensions.WaitForInvisibilityOfElement(_driver, apStaySignedInNoButtonLocBy);
                                    if (_driver.FindElements(oopsSomethingWentWrongPopupLocBy).Count > 0)
                                    {
                                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                                        _driver.Navigate().Refresh();
                                    }
                                    webElementExtensions.WaitForElement(_driver, logoutDropdownArrowIconLocBy);
                                    flag = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, logoutDropdownArrowIconLocBy);
                                }
                            }
                        }
                    }
                    _driver.ReportResult(test, flag, "Successfully logged into Agent Portal application with the username: " + @"<b>" + username + @"</b>" + ".", "Failed while logging into the Agent Portal application with the username:" + username + ".");
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while logging in to the Agent Portal application: " + ex.Message);
            }
            finally
            {
                successfulLoginDone = flag;                
            }
            return flag;
        }

        ///<summary>
        ///Method to logout of the application
        ///</summary>
        public void LogoutOfTheApplication()
        {
            bool flag = false, isReportRequired = true;
            string rootDirectory = string.Empty;
            try
            {
                rootDirectory = UtilAdditions.GetRootDirectory();
                if (rootDirectory.Contains(Constants.SolutionProjectNames.AgentPortal))
                {
                    webElementExtensions.WaitForElement(_driver, agentPortalPageHeaderLocBy);
                    if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, apUsernameFieldLocBy) && !successfulLoginDone)
                        isReportRequired = false;
                    else
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                        if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, oopsSomethingWentWrongPopupLocBy))
                        {
                            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                            webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                        }
                        webElementExtensions.WaitForElement(_driver, logoutDropdownArrowIconLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, logoutDropdownArrowIconLocBy);
                        webElementExtensions.ActionClick(_driver, logoutDropdownArrowIconLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                        if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, signOutOptionLocBy))
                        {
                            webElementExtensions.WaitForVisibilityOfElement(_driver, signOutOptionLocBy);
                        }
                        webElementExtensions.ClickElementUsingJavascript(_driver, signOutOptionLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                        webElementExtensions.WaitForElement(_driver, pickAnAccountHeaderLocBy);
                        flag = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, pickAnAccountHeaderLocBy) && successfulLoginDone;
                    }
                }
                else if (rootDirectory.Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, cpLoginScreenLocBy) && !successfulLoginDone)
                        isReportRequired = false;
                    else
                    {
                        webElementExtensions.WaitForElement(_driver, logoutDropdownArrowIconLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, logoutDropdownArrowIconLocBy);
                        webElementExtensions.ActionClick(_driver, logoutDropdownArrowIconLocBy);
                        webElementExtensions.WaitForElement(_driver, signOutOptionLocBy);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, signOutOptionLocBy);
                        webElementExtensions.ActionClick(_driver, signOutOptionLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, signOutOptionLocBy);
                        webElementExtensions.WaitForElement(_driver, cpLoginScreenLocBy);
                        flag = webElementExtensions.IsElementDisplayed(_driver, cpLoginScreenLocBy) && successfulLoginDone;
                        isReportRequired = false;
                    }
                }
                else if (rootDirectory.Contains(Constants.SolutionProjectNames.MSP))
                {
                    webElementExtensions.WaitForElement(_driver, userMenuLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, userMenuLocBy);
                    webElementExtensions.ClickElement(_driver, userMenuLocBy);
                    webElementExtensions.WaitForElement(_driver, logOffOptionLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, logOffOptionLocBy);
                    webElementExtensions.ActionClick(_driver, logOffOptionLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, logOffOptionLocBy);
                    webElementExtensions.WaitForElement(_driver, successfulLogoutMessageLocBy);
                    flag = webElementExtensions.IsElementDisplayed(_driver, successfulLogoutMessageLocBy) && successfulLoginDone;
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while logging out of the application: " + ex.Message);
            }
            finally
            {
                successfulLoginDone = false;//Resetting the login successful flag
                if (isReportRequired)
                    _driver.ReportResult(test, flag, "Successfully logged out of the application.", "Failed while logging out of the application.");
            }
        }

        /// <summary>
        /// Method to launch URL with a specific loan number
        /// </summary>
        /// <param name="loanLevelData">List of hashtables with loan level data from DB</param>
        /// <param name="rowNumber">DB table row number of the loan number selected from hashtable</param>
        /// <param name="isReportRequired">true/false</param>
        /// <returns></returns>
        public bool LaunchUrlWithLoanNumber(List<Hashtable> loanLevelData, int rowNumber = 0, bool isReportRequired = false)
        {
            bool flag = false, flag1 = false;
            string url = string.Empty, loanNumber = string.Empty;
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, oopsSomethingWentWrongPopupLocBy))
                {
                    webElementExtensions.ActionClick(_driver, oopsSomethingWentWrongPopupLocBy);
                }
                loanNumber = loanLevelData[rowNumber][Constants.LoanLevelDataColumns.LoanNumber].ToString();
                url = ConfigSettings.AppUrl + loanNumber;
                if (url.Contains("agent-dashboard/loansearch"))
                    url = url.Replace("agent-dashboard/loansearch", "agent/loan/loan-summary-details/");
                flag1 = loanNumbersInUse.Contains(loanNumber);
                if (!string.IsNullOrEmpty(loanNumber) && !flag1)
                {
                    _driver.Navigate().GoToUrl(url);
                    loanNumbersInUse.Add(loanNumber);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                    webElementExtensions.WaitForPageLoad(_driver);
                    if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, oopsSomethingWentWrongPopupLocBy))
                    {
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                        _driver.Navigate().Refresh();
                        _driver.Navigate().GoToUrl(url);
                    }
                    webElementExtensions.WaitForElement(_driver, timeZoneValueInBorrowersLocalTimeLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, timeZoneValueInBorrowersLocalTimeLocBy);
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, timeZoneValueInBorrowersLocalTimeLocBy))
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                    flag = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, timeZoneValueInBorrowersLocalTimeLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while launching the Url: " + ex.Message);
            }
            if (isReportRequired)
            {
                if (flag1)
                    test.Log(Status.Info, "The selected loan number - " + loanNumber + " is already in use.");
                else
                    _driver.ReportResult(test, flag, "Successfully launched the following URL with loan number - " + url + ".", "Failed while launching the following URL with loan number - " + url + ".");
            }
            return flag;
        }

        /// <summary>
        /// Method to launch URL with a specific loan number
        /// </summary>
        /// <param name="url">Agent Portal Url to be launched</param>
        /// <param name="isReportRequired">true/false</param>
        /// <returns></returns>
        public bool LaunchAgentPortalUrlWithLoanNumber(string url, bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, oopsSomethingWentWrongPopupLocBy))
                {
                    webElementExtensions.ActionClick(_driver, oopsSomethingWentWrongPopupLocBy);
                }
                if (url.Contains("agent-dashboard/loansearch"))
                    url = url.Replace("agent-dashboard/loansearch", "agent/loan/loan-summary-details/");
                _driver.Navigate().GoToUrl(url);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                webElementExtensions.WaitForPageLoad(_driver);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, oopsSomethingWentWrongPopupLocBy))
                {
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                    _driver.Navigate().Refresh();
                    _driver.Navigate().GoToUrl(url);
                }
                webElementExtensions.WaitForElement(_driver, timeZoneValueInBorrowersLocalTimeLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, timeZoneValueInBorrowersLocalTimeLocBy);
                if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, timeZoneValueInBorrowersLocalTimeLocBy))
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                flag = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, timeZoneValueInBorrowersLocalTimeLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);

            }
            catch (Exception ex)
            {
                log.Error("Failed while launching the Url: " + ex.Message);
            }
            if (isReportRequired)
            {
                _driver.ReportResult(test, flag, "Successfully launched the following URL with loan number - " + url + ".", "Failed while launching the following URL with loan number - " + url + ".");
            }
            return flag;
        }

        #endregion LoginLogoutMethods

        #region GetSqlDataMethods

        /// <summary>
        /// Method to retrieve loan level data from database
        /// </summary>
        /// <param name="query">Query to fetch data</param>
        /// <param name="dbName">Database name</param>
        /// <param name="columnDataRequired">Loan level column values required</param>
        /// <param name="usedLoanTestData">Already used data rows not ideal for testing</param>
        /// <param name="numberOfLoanDataRequired">Number of loan data required for testing</param>
        /// <returns></returns>
        public List<Hashtable> GetLoanDataFromDatabase(string query, string dbName = null, List<string> columnDataRequired = null, List<string> usedLoanTestData = null, int numberOfLoanDataRequired = 1, bool skipAssertEvenIfNoData = false)
        {
            bool flag = false;
            string loanNumber = string.Empty;
            Hashtable loanData = new Hashtable();
            List<Hashtable> loanDataList = new List<Hashtable>();

            try
            {
                if (!string.IsNullOrEmpty(dbName))
                    dBconnect = new DBconnect(test, dbName);

                DataTable allDataFromDB = dBconnect.ExecuteQuery(query);
                if (allDataFromDB.Rows.Count == 0)
                {
                    Assert.Fail();
                }
                else
                {
                    DataTable newTable = new DataTable();
                    DataRow[] orderRows = new DataRow[allDataFromDB.Rows.Count];
                    HashSet<int> uniqueNumbers = new HashSet<int>();
                    newTable = allDataFromDB.Clone();
                    //Make a new Random generator
                    Random rnd = new Random();
                    int count = 0;

                    //We'll use this to make sure we don't have a duplicate row
                    bool rowFound = false;
                    //index generation

                    while (uniqueNumbers.Count < allDataFromDB.Rows.Count)
                    {
                        int index = rnd.Next(0, allDataFromDB.Rows.Count);
                        if (!uniqueNumbers.Contains(index))
                        {
                            //use the index on the old table to get the random data, then put it into the new table.
                            foreach (DataRow row in newTable.Rows)
                            {
                                if (allDataFromDB.Rows[index] == row)
                                {
                                    //Oops, there's duplicate data already in the new table. We don't want this.
                                    rowFound = true;
                                    break;
                                }
                            }
                            if (!rowFound)
                                orderRows[count++] = allDataFromDB.Rows[index];
                            uniqueNumbers.Add(index);
                        }
                    }

                    foreach (DataRow dr in orderRows)
                    {
                        newTable.ImportRow(dr);
                    }

                    if (columnDataRequired == null)
                    {
                        columnDataRequired = typeof(Constants.LoanLevelDataColumns)
                        .GetFields(BindingFlags.Public | BindingFlags.Static)
                        .Where(field => field.FieldType == typeof(string))
                        .Select(field => (string)field.GetValue(null)).ToList();
                    }
                    foreach (DataRow row in newTable.Rows)
                    {
                        if (columnDataRequired.Contains("LoanNumber"))
                            loanNumber = row["LoanNumber"].ToString();
                        else
                            loanNumber = row["loan_number"].ToString();
                        if (!usedLoanTestData.Contains(loanNumber))
                        {
                            foreach (string column in columnDataRequired)
                            {
                                if (row.Table.Columns.Contains(column))
                                    loanData.Add(column, row[column]);
                            }
                            loanDataList.Add(loanData);
                            numberOfLoanDataRequired--;
                        }
                        if (numberOfLoanDataRequired == 0)
                            break;
                        else
                            loanData = new Hashtable();
                    }
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while logging getting loan data from DB: " + ex.Message);
            }
            if (!skipAssertEvenIfNoData)
            {
                if (loanDataList.Count == 0)
                {
                    test.Log(Status.Warning, "There is no test data available in " + ConfigSettings.Environment + " database for the given query - " + query + ".");
                    Assert.Fail();
                }
                else
                    _driver.ReportResult(test, flag, "Successfully retrieved " + loanDataList.Count + " set of loan level data from database.", "Failed while retrieving " + loanDataList.Count + " set of loan level data from database.");
            }

            return loanDataList;
        }

        /// <summary>
        /// Retrieves data from the database based on the provided query.
        /// </summary>
        /// <param name="query">The SQL query to execute.</param>
        /// <param name="dbName">The name of the database to connect to (optional).</param>
        /// <param name="columnDataRequired">A list of column names for which data is required (optional).</param>
        /// <returns>A list of Hashtables containing the retrieved data.</returns>
        public List<Hashtable> ExecuteQueryAndGetDataFromDataBase(string query, string dbName = null, List<string> columnDataRequired = null, bool isReported = false)
        {
            bool flag = false;
            Hashtable data = new Hashtable();
            List<Hashtable> dataList = new List<Hashtable>();
            try
            {
                if (!string.IsNullOrEmpty(dbName))
                    dBconnect = new DBconnect(test, dbName);

                DataTable allDataFromDB = dBconnect.ExecuteQuery(query);
                if (allDataFromDB.Rows.Count > 0)
                {
                    foreach (DataRow row in allDataFromDB.Rows)
                    {
                        foreach (string column in columnDataRequired)
                        {
                            data.Add(column, row[column]);
                        }
                        dataList.Add(data);
                    }
                }
                else
                {
                    foreach (string column in columnDataRequired)
                    {
                        data.Add(column, "-1");
                    }
                    dataList.Add(data);
                }

                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting data from DB: " + ex.Message);
            }
            if (isReported)
            {
                _driver.ReportResult(test, flag, "Successfully retrieved " + dataList.Count + " set of  data from database.", "Failed while retrieving " + data.Count + " set of data from database.");
            }
            return dataList;
        }

        #endregion GetSqlDataMethods

        #region CommonMethodsBasedOnFunctionality

        /// <summary>
        /// Method to select eligible date from Cal
        /// </summary>
        /// <param name="pendingPaymentDates">"1-1-2025"</param>
        /// <param name="isReportRequired">true/false</param>
        /// <param name="isSecondAvailableDateRequired">true/false</param>
        /// <param name="selectNextMonth">true/false</param>
        /// <returns></returns>
        public string SelectAnEligibleDateOnPaymentsPage(List<string> pendingPaymentDates, bool isReportRequired = false, string type = "setup", bool selectNextMonth = false, bool isScrollIntoViewRequired = true)
        {
            bool flag = false;
            string paymentDate = string.Empty;
            try
            {
                pendingPaymentDates = pendingPaymentDates.Select(date =>
                {
                    // Try parsing the date first
                    DateTime parsedDate;
                    return DateTime.TryParse(date, out parsedDate)
                        ? parsedDate.ToString("MMMM d, yyyy")
                        : date; // If already in correct format, keep it as is
                }).ToList();
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                var paymentDatesToEnabled = GetAvailablePaymentDateInDateField(isReportRequired, selectNextMonth);
                List<string> remainingDates = paymentDatesToEnabled.Except(pendingPaymentDates).ToList();
                if (remainingDates == null || remainingDates.Count == 0)
                {
                    test.Log(Status.Info, "No valid date found.");
                    if (type.Equals("setup"))
                    {
                        flag = false;
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Escape);
                        Assert.Fail("There was no other date available to select");
                        return null;
                    }
                    else
                    {
                        string alreadySelectedDate = webElementExtensions.GetElementAttribute(_driver, paymentDateAlreadySelected, "aria-label");
                        test.Log(Status.Info, $"There was no other date available to select, Selected Date remains Unchanged: {alreadySelectedDate}");
                        return alreadySelectedDate;
                    }
                }
                else
                {
                    paymentDate = remainingDates.First().ToString();
                    SelectPaymentDateInDateField(paymentDate, isReportRequired: true, selectNextMonth, isScrollIntoViewRequired: isScrollIntoViewRequired);
                    flag = true;
                    return paymentDate;
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while selecting eligible date: {e.Message}");
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, $"Succesfully selected an eligible date: {paymentDate} ", "Failed while trying to selecting date");
            return paymentDate;
        }

        /// <summary>
        /// Method to select the available payment date
        /// </summary>
        /// <param name="isReportRequired">true/false</param>
        public List<string> GetAvailablePaymentDateInDateField(bool isReportRequired = true, bool selectNextMonth = false)
        {
            List<string> formattedDates = new List<string>();
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, spinnerLocBy);
                webElementExtensions.ActionClick(_driver, paymentDatePickerIconLocBy);
                webElementExtensions.WaitForElement(_driver, datesAvailableToSelectOnCalndrLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForStalenessOfElement(_driver, datesAvailableToSelectOnCalndrLocBy, ConfigSettings.SmallWaitTime);
                List<int> datesAvailable = _driver.FindElements(datesAvailableToSelectOnCalndrLocBy).Select(element => int.Parse(element.Text)).ToList();

                webElementExtensions.WaitForElement(_driver, monthYearOnCalendarTextBy, ConfigSettings.SmallWaitTime);
                string monthYearOnCalendar = webElementExtensions.GetElementText(_driver, monthYearOnCalendarTextBy);
                DateTime parsedDate = DateTime.ParseExact(monthYearOnCalendar, "MMM yyyy", CultureInfo.InvariantCulture);
                reportLogger.TakeScreenshot(test, $"{monthYearOnCalendar} : Calendar");
                foreach (int date in datesAvailable)
                {
                    DateTime specificDate = new DateTime(parsedDate.Year, parsedDate.Month, date);
                    string formattedDate = specificDate.ToString("M/d/yyyy h:mm:ss tt");
                    formattedDate = webElementExtensions.DateTimeConverter(_driver, formattedDate, "m/d/yyyy to fullMonthName d, yyyy");
                    formattedDates.Add(formattedDate);
                }
                // choose the next month only if no dates available in current month and user asked to check next month
                if (datesAvailable.Count >= 0 && selectNextMonth)
                {
                    webElementExtensions.WaitUntilElementIsClickable(_driver, nextMonthButtonCalanderLocBy);
                    webElementExtensions.ActionClick(_driver, nextMonthButtonCalanderLocBy);
                    var newDates = _driver.FindElements(datesAvailableToSelectOnCalndrLocBy)
                      .Select(element => int.Parse(element.Text))
                      .ToList();
                    webElementExtensions.WaitForElement(_driver, monthYearOnCalendarTextBy, ConfigSettings.SmallWaitTime);
                    monthYearOnCalendar = webElementExtensions.GetElementText(_driver, monthYearOnCalendarTextBy);
                    reportLogger.TakeScreenshot(test, $"{monthYearOnCalendar} : Calendar");
                    parsedDate = DateTime.ParseExact(monthYearOnCalendar, "MMM yyyy", CultureInfo.InvariantCulture);

                    foreach (int date in newDates)
                    {
                        DateTime specificDate = new DateTime(parsedDate.Year, parsedDate.Month, date);
                        string formattedDate = specificDate.ToString("M/d/yyyy h:mm:ss tt");
                        formattedDate = webElementExtensions.DateTimeConverter(_driver, formattedDate, "m/d/yyyy to fullMonthName d, yyyy");
                        formattedDates.Add(formattedDate);
                    }
                    datesAvailable.AddRange(newDates);
                }
                if (datesAvailable.Count == 0)
                    log.Error("Failed while selecting payment dates in Date field as there are no dates available for selection.");
            }
            catch (Exception ex)
            {
                log.Error("Failed while filtering payment dates in Date field: " + ex.Message);
            }

            if (isReportRequired)
            {
                _driver.ReportResult(test, formattedDates.Count > 0, "Successfully filtered payment dates - " + string.Join(", ", formattedDates) + " in Date field.", "Failed while filtering payment dates in Date field.");
            }

            return formattedDates;
        }

        /// <summary>
        /// Method to navigate to Autopay screen for both Agent Portal and Customer Portal
        /// </summary>
        public void NavigateToSetupAutopayPage()
        {
            PaymentsPage apPayments = new PaymentsPage(_driver, test);
            try
            {
                webElementExtensions.WaitForElement(_driver, apPayments.goBackToPaymentsLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, setupAutopaybuttonLocBy);
                webElementExtensions.ActionClick(_driver, setupAutopaybuttonLocBy, "Setup Autopay button.");
                webElementExtensions.WaitForVisibilityOfElement(_driver, bankAccountDropdownLocBy);
            }
            catch (Exception ex)
            {
                log.Error("Failed while navigating to autopay screen: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to navigate to Edit Autopay screen for both Agent Portal and Customer Portal
        /// </summary>
        public void NavigateToEditAutopayPage()
        {
            PaymentsPage apPayments = new PaymentsPage(_driver, test);
            try
            {
                webElementExtensions.WaitForElement(_driver, apPayments.goBackToPaymentsLinkLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, autopayEditIconLocBy);
                webElementExtensions.ActionClick(_driver, autopayEditIconLocBy, "Edit Autopay icon", false, true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, bankAccountDropdownLocBy);
            }
            catch (Exception ex)
            {
                log.Error("Failed while navigating to edit autopay screen: " + ex.Message);
            }
        }


        /// <summary>
        /// Method to set up Autopay functionality for both Agent Portal and Customer Portal
        /// </summary>
        /// <param name="paymentDate">Payment date to be selected. If null, SelectAnEligibleDateOnPaymentsPage method would select the date</param>
        /// <param name="pendingPaymentDates">Payment Dates already having payment</param>
        /// <param name="borrowerName">borrower name [required only for agent portal]</param>
        /// <param name="isScrollIntoViewRequired">true/false</param>
        /// <param name="additionalPrincipal">Additional principal amount - 1.00/60.00/....</param>
        /// <param name="skipAutopayScreenNavigationCode">Pass 'true' if we do not need to execute the navigation to Set up Autopay page piece of code</param>
        /// <param name="skipBankAccountadditionrequired">Pass 'true' if we do not need to execute the bank account addition piece of code</param>
        public bool SetupAutopay(string paymentDate = null, List<string> pendingPaymentDates = null, string borrowerName = null, bool isScrollIntoViewRequired = true, string additionalPrincipal = null, bool skipAutopayScreenNavigationCode = false, bool skipBankAccountadditionrequired = false)
        {
            bool flag = false;
            PaymentsPage apPayments = new PaymentsPage(_driver, test);
            string randomNumber = string.Empty, bankAccountNumber = string.Empty, autopayAmountSetup = string.Empty;
            List<string> pendingPaymentDatesInMonthThreeLetterFormat = new List<string>();
            try
            {
                if (!skipAutopayScreenNavigationCode)
                    NavigateToSetupAutopayPage();
                if (!string.IsNullOrEmpty(borrowerName))
                {
                    webElementExtensions.WaitForElement(_driver, authorizedByDropdownLocBy);
                    apPayments.SelectValueInAuthorizedByDropdown(borrowerName);
                }
                else
                {
                    webElementExtensions.WaitForElement(_driver, bankAccountDropdownLocBy);
                }
                if (!skipBankAccountadditionrequired)
                    AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumber, true, true, true, isScrollIntoViewRequired);
                if (paymentDate == null)
                {
                    foreach (string dateValue in pendingPaymentDates)
                        pendingPaymentDatesInMonthThreeLetterFormat.Add(webElementExtensions.DateTimeConverter(_driver, dateValue, "m/d/yyyy to threeLetterMonthName d, yyyy"));
                    paymentDate = SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: isScrollIntoViewRequired);
                }
                else
                    SelectPaymentDateInDateField(paymentDate);

                if (!string.IsNullOrEmpty(additionalPrincipal) && webElementExtensions.IsElementDisplayedBasedOnCount(_driver, additionalPaymentCheckboxLocBy))
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, additionalPaymentCheckboxLocBy);
                    webElementExtensions.EnterText(_driver, additionalPrincipalTextboxLocBy, additionalPrincipal, true, "Additional Principal", true);
                }
                webElementExtensions.ClickElement(_driver, setupAutopayButtonInSetupAutopayScreenLocBy, "Setup Autopay button.");
                webElementExtensions.WaitForElement(_driver, confirmAutopayButtonLocBy);
                webElementExtensions.ScrollIntoView(_driver, confirmAutopayButtonLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, confirmAutopayButtonLocBy, "Confirm Autopay button.", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, continueButtonInConfirmAutopayScreenLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, continueButtonInConfirmAutopayScreenLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, closeWindowButtonLocBy);
                autopayAmountSetup = webElementExtensions.GetElementText(_driver, amountInAutopaySuccessfullyScheduledPageLocBy);
                reportLogger.TakeScreenshot(test, "Autopay successfully scheduled!");
                webElementExtensions.ClickElementUsingJavascript(_driver, backToManageAutoPayButtonLocBy, "Back To Manage AutoPay Button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, closeWindowButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                test.Log(Status.Info, $"Back to Manage Autopay Page.");
                if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, planForAutopayPaymentManageAutopayTextLocBy))
                    _driver.Navigate().Refresh();
                webElementExtensions.VerifyElementText(_driver, planForAutopayPaymentManageAutopayTextLocBy, "Monthly", "Autopay Payment Plan", true);
                webElementExtensions.VerifyElementText(_driver, nextDebitDateManageAutoPayTextLocBy, paymentDate, isReportRequired: true);
                webElementExtensions.VerifyElementText(_driver, amountOnManageAutoPayTextLocBy, autopayAmountSetup, isReportRequired: true);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, planForAutopayPaymentManageAutopayTextLocBy))
                    flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while setting up autopay: " + ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Method to perform edit autopay functionality for both Agent Portal and Customer Portal
        /// </summary>
        /// <param name="paymentDate">Payment date to be selected. If null, SelectAnEligibleDateOnPaymentsPage method would select the date</param>
        /// <param name="pendingPaymentDates">Payment Dates already having payment</param>
        /// <param name="borrowerName">borrower name [required only for agent portal]</param>
        /// <param name="isScrollIntoViewRequired">true/false</param>
        /// <param name="additionalPrincipal">Additional principal amount - 1.00/60.00/....</param>
        /// <param name="skipEditAutopayScreenNavigationCode">Pass 'true' if we do not need to execute the navigation to Set up Autopay page piece of code</param>
        /// <param name="skipNewBankAccountaddition">Pass 'true' if we do not need to execute the bank account addition piece of code</param>
        public bool EditAutopay(string paymentDate = null, List<string> pendingPaymentDates = null, string borrowerName = null, bool isScrollIntoViewRequired = true, string additionalPrincipal = null, bool skipEditAutopayScreenNavigationCode = false, bool skipNewBankAccountaddition = false)
        {
            bool flag = false;
            PaymentsPage apPayments = new PaymentsPage(_driver, test);
            string randomNumber = string.Empty, bankAccountNumber = string.Empty, autopayAmountSetup = string.Empty;
            List<string> pendingPaymentDatesInMonthThreeLetterFormat = new List<string>();
            try
            {
                if (!skipEditAutopayScreenNavigationCode)
                    NavigateToEditAutopayPage();

                webElementExtensions.WaitForElement(_driver, bankAccountDropdownLocBy);
                //verify Authorized By field and Update Autopay button are disabled
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.AgentPortal))
                    webElementExtensions.IsElementDisabled(_driver, authorizedByDropdownMainLocatorLocBy, "Authorized By");
                webElementExtensions.WaitForElement(_driver, updateAutopayLocBy);
                webElementExtensions.IsElementDisabled(_driver, updateAutopayLocBy, "Update Autopay");
                if (borrowerName != null)
                {
                    ReportingMethods.LogAssertionEqual(test, webElementExtensions.GetElementText(_driver, authorizedByDropdownValueSelectedLocBy), borrowerName.ToUpper(), "Verify Borrower name selected:");
                }
                if (!skipNewBankAccountaddition)
                    AddAnAccount(bankAccountName, firstName, lastName, personalOrBussiness, savings, routingNumber, accountNumberWhileEdit, true, true, true, isScrollIntoViewRequired);
                if (paymentDate == null)
                {
                    if (pendingPaymentDates != null)
                    {
                        foreach (string dateValue in pendingPaymentDates)
                            pendingPaymentDatesInMonthThreeLetterFormat.Add(webElementExtensions.DateTimeConverter(_driver, dateValue, "m/d/yyyy to threeLetterMonthName d, yyyy"));
                        paymentDate = SelectAnEligibleDateOnPaymentsPage(pendingPaymentDates, true, isScrollIntoViewRequired: isScrollIntoViewRequired);
                    }
                }
                else
                    SelectPaymentDateInDateField(paymentDate);

                if (!string.IsNullOrEmpty(additionalPrincipal) && webElementExtensions.IsElementDisplayedBasedOnCount(_driver, additionalPrincipalCheckboxInAutopayPageLocBy))
                {
                    webElementExtensions.ActionClick(_driver, additionalPrincipalCheckboxInAutopayPageLocBy);
                    webElementExtensions.EnterText(_driver, additionalPrincipalInputFieldInAutopayPageLocBy, additionalPrincipal, true, "Additional Principal", true);
                }
                webElementExtensions.ClickElement(_driver, updateAutopayLocBy, "Update Autopay button.");
                webElementExtensions.WaitForElement(_driver, confirmAutopayButtonLocBy);
                webElementExtensions.ScrollIntoView(_driver, confirmAutopayButtonLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, confirmAutopayButtonLocBy, "Confirm Autopay button.", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, closeWindowButtonLocBy);
                autopayAmountSetup = webElementExtensions.GetElementText(_driver, amountInAutopaySuccessfullyScheduledPageLocBy);
                reportLogger.TakeScreenshot(test, "Autopay successfully edited!");
                webElementExtensions.ClickElementUsingJavascript(_driver, backToManageAutoPayButtonLocBy, "Back To Manage AutoPay Button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, closeWindowButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, manageAutoPayHeaderTextLocBy, ConfigSettings.WaitTime);
                reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                test.Log(Status.Info, $"Back to Manage Autopay Page.");
                if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, planForAutopayPaymentManageAutopayTextLocBy))
                    _driver.Navigate().Refresh();
                webElementExtensions.VerifyElementText(_driver, planForAutopayPaymentManageAutopayTextLocBy, "Monthly", "Autopay Payment Plan", true);
                if (paymentDate != null)
                    webElementExtensions.VerifyElementText(_driver, nextDebitDateManageAutoPayTextLocBy, paymentDate, isReportRequired: true);
                webElementExtensions.VerifyElementText(_driver, amountOnManageAutoPayTextLocBy, autopayAmountSetup, isReportRequired: true);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, planForAutopayPaymentManageAutopayTextLocBy))
                    flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while editing autopay: " + ex.Message);
            }
            return flag;
        }


        /// <summary>
        /// Method to delete autopay [set up either from Agent Portal/Customer Portal] through Agent Portal
        /// </summary>
        /// <param name="loanNumber">Loan number for which autopay should be deleted</param>
        /// <param name="userAlreadyLoggedIn">Another user already logged in or not</param>
        public bool DeleteAutopayFromAgentPortal(string loanNumber, bool userAlreadyLoggedIn = false)
        {
            bool flag = false;
            string nodeValue = string.Empty, url = string.Empty;
            try
            {
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.AgentPortal))
                {
                    url = ConfigSettings.AppUrl;
                    if (userAlreadyLoggedIn)
                    {
                        LogoutOfTheApplication();
                        webElementExtensions.ActionClick(_driver, pickAnAccountPageSignedInUserSectionLocBy);
                        _driver.Navigate().GoToUrl(url);
                        webElementExtensions.WaitForElement(_driver, useAnotherAccountLocBy);
                        webElementExtensions.ActionClick(_driver, useAnotherAccountLocBy);
                    }
                }
                else
                {
                    string apConfigFile = Environment.CurrentDirectory.ToString() + "\\Config\\APConfig.xml";
                    apConfigFile = apConfigFile.Replace(Constants.SolutionProjectNames.CustomerPortal, Constants.SolutionProjectNames.AgentPortal);
                    using (var stream = new FileStream(apConfigFile, FileMode.Open))
                    {
                        XPathDocument doc = new XPathDocument(stream);
                        XPathNavigator nav = doc.CreateNavigator();

                        if (ConfigSettings.Environment == "QA")
                            nodeValue = "Portal/RunSettings/QAAppUrl";
                        else if (ConfigSettings.Environment == "SG")
                            nodeValue = "Portal/RunSettings/SGAppUrl";
                        XPathItem urlFromConfig = nav.SelectSingleNode(nodeValue);
                        url = urlFromConfig.Value;
                    }
                    webElementExtensions.OpenNewTabAndSwitch(_driver);
                    _driver.Navigate().GoToUrl(url);
                }
                LoginToAgentPortalApplication(Constants.DeleteAutopayCredentials.Username, new EncryptionManager(test).DecryptDataWithAes(Constants.DeleteAutopayCredentials.Password));
                LaunchAgentPortalUrlWithLoanNumber(url + loanNumber, true);
                NavigateToTabInAgentPortal(Constants.AgentPortalTabNames.PaymentsTab);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                webElementExtensions.WaitForElement(_driver, pushDeleteToMSPButtonLocBy);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, pushDeleteToMSPButtonLocBy))
                {
                    webElementExtensions.ActionClick(_driver, pushDeleteToMSPButtonLocBy, "Push Delete To MSP button.");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, deleteAutopayPopupTextLocBy);
                    webElementExtensions.WaitForElement(_driver, keepAutopayButtonLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, disabledDeleteButtonInDeleteAutpayPopupLocBy);
                    if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, disabledDeleteButtonInDeleteAutpayPopupLocBy))
                    {
                        By deleteReasonRadioButtonLocBy = By.XPath(deleteReasonRadioButton.Replace("<AUTOPAYDELETEREASON>", "I need to change my plan’s frequency."));
                        webElementExtensions.WaitForVisibilityOfElement(_driver, deleteReasonRadioButtonLocBy);
                        webElementExtensions.ActionClick(_driver, deleteReasonRadioButtonLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                        webElementExtensions.WaitForElement(_driver, deleteButtonInDeleteAutpayPopupLocBy);
                        if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, disabledDeleteButtonInDeleteAutpayPopupLocBy))
                        {
                            webElementExtensions.ActionClick(_driver, deleteButtonInDeleteAutpayPopupLocBy);
                        }
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                        webElementExtensions.WaitForElement(_driver, manageAutopayButtonEnabledLocBy);
                        webElementExtensions.WaitForElement(_driver, loadingIconLocBy);
                        webElementExtensions.WaitForElement(_driver, pushDeleteToMSPButtonDisabledLocBy);
                        if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, pushDeleteToMSPButtonDisabledLocBy))
                            flag = true;
                    }
                    _driver.ReportResult(test, flag, "Successfully deleted autopay for the loan - " + loanNumber + " from agent portal.", "Failed while deleting autopay for the loan - " + loanNumber + " from agent portal.");
                }
                else if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, pushDeleteToMSPButtonDisabledLocBy))
                    test.Log(Status.Info, "Autopay is already deleted for this loan.");
                else
                    test.Log(Status.Fail, "Enabled Push Delete To MSP button is not displayed to proceed further.");

                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal))
                {
                    _driver.Close();
                    webElementExtensions.SwitchToFirstTab(_driver);
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while performing autopay deletion: " + ex.Message);
            }            
            return flag;
        }

        /// <summary>
        /// Method to navigate to any tab in Agent Portal
        /// </summary>
        /// <param name="tabName">Payments/Loan Summary</param>
        /// <param name="isReportingRequired">true/false</param>
        public void NavigateToTabInAgentPortal(string tabName, bool isReportingRequired = true)
        {
            bool flag = false;
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, oopsSomethingWentWrongPopupLocBy))
                {
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                    _driver.Navigate().Refresh();
                }
                By tabLocBy = By.XPath(apNavigationTabsLocBy.Replace("<TABNAME>", tabName));
                webElementExtensions.WaitForElement(_driver, tabLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                if (!webElementExtensions.VerifyElementAttributeValue(_driver, tabLocBy, "aria-selected", "true"))
                {
                    webElementExtensions.MoveToElement(_driver, tabLocBy);
                    webElementExtensions.ScrollIntoView(_driver, tabLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, tabLocBy);
                }
                flag = true;
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
            }
            catch (Exception ex)
            {
                log.Error("Failed while navigating to tab: " + ex.Message);
            }
            if (isReportingRequired)
                _driver.ReportResult(test, flag, "Successfully navigated to " + tabName + " tab.", "Failed while navigating to " + tabName + " tab.");
        }

        /// <summary>
        /// Method to select bank account in Method dropdown
        /// </summary>
        /// <param name="bankAccountNickNameOrcheckingOrSavingsAccountType">Pass account nickname if it exists, else specify whether account is Savings or Checking</param>
        /// <param name="bankAccountNumber">Account number - 1234/45668/...</param>
        /// <param name="isReportRequired">True/False</param>
        public bool SelectValueInMethodDropdown(string bankAccountNickNameOrcheckingOrSavingsAccountType, string bankAccountNumber = Constants.BankAccountData.BankAccountNumber, bool isReportRequired = true)
        {
            bool flag = false;
            string bankAccountName = string.Empty;
            By bankAccountDropdownValueLocBy = null;
            List<IWebElement> allWebElementsInMethodDropdown = null;
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                bankAccountName = bankAccountNickNameOrcheckingOrSavingsAccountType + " (Loan Depot Test-" + bankAccountNumber.Substring(bankAccountNumber.Length - 4) + ")";
                if (ConfigSettings.Environment == "SG")
                    bankAccountName = bankAccountName.Replace("Test-", "Bank-");
                webElementExtensions.WaitForVisibilityOfElement(_driver, bankAccountDropdownLocBy);
                webElementExtensions.ScrollToTop(_driver);
                webElementExtensions.ActionClick(_driver, bankAccountDropdownLocBy);
                bankAccountDropdownValueLocBy = By.XPath(bankAccountDropdownValue.Replace("<BANKACCOUNT>", bankAccountName));
                webElementExtensions.WaitForElement(_driver, allValuesInbankAccountDropdownLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, allValuesInbankAccountDropdownLocBy);
                allWebElementsInMethodDropdown = _driver.FindElements(allValuesInbankAccountDropdownLocBy).ToList();
                foreach (IWebElement element in allWebElementsInMethodDropdown)
                {
                    if (element.Text.Contains(bankAccountNumber.Substring(bankAccountNumber.Length - 4)) &&
                        element.Text.Contains(bankAccountNickNameOrcheckingOrSavingsAccountType))
                    {
                        webElementExtensions.WaitForElementToBeEnabled(_driver, element, timeoutInSeconds: ConfigSettings.WaitTime, isScrollIntoViewRequired: false);
                        flag = webElementExtensions.ClickElementUsingJavascript(_driver, element);
                        webElementExtensions.WaitForVisibilityOfElement(_driver, bankAccountDropdownValueSelectedLocBy);
                        webElementExtensions.WaitForElement(_driver, bankAccountDropdownValueSelectedLocBy);
                        break;
                    }
                }
                if (!flag)
                    webElementExtensions.ActionClick(_driver, bankAccountDropdownLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
            }
            catch (Exception ex)
            {
                log.Error("Failed while selecting bank account in Method dropdown: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully selected bank account - " + bankAccountName + " in Method dropdown.", "Failed while selecting bank account - " + bankAccountName + " in Method dropdown.");
            return flag;
        }

        /// <summary>
        /// Method to verify if bank account already exists in Method dropdown
        /// </summary>
        /// <param name="bankAccountNickNameOrcheckingOrSavingsAccountType">Pass account nickname if it exists, else specify whether account is Savings or Checking</param>
        /// <param name="bankAccountNumber">Account number - 1234/45668/...</param>
        /// <param name="compareOnlySelectedValue">Pass true if only selected value needs to be verified</param>
        /// <param name="isReportRequired">True/False</param>
        public bool VerifyValueInMethodDropdown(string bankAccountNickNameOrcheckingOrSavingsAccountType, string bankAccountNumber = Constants.BankAccountData.BankAccountNumber, bool compareOnlySelectedValue = false, bool isReportRequired = false)
        {
            bool flag = false;
            By bankAccountDropdownValueLocBy = null;
            List<IWebElement> allWebElementsInMethodDropdown = null;
            string selectedValue = string.Empty, bankAccountName = string.Empty;
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                bankAccountName = bankAccountNickNameOrcheckingOrSavingsAccountType + " (Loan Depot Test-" + bankAccountNumber.Substring(bankAccountNumber.Length - 4) + ")";
                if (ConfigSettings.Environment == "SG")
                    bankAccountName = bankAccountName.Replace("Test-", "Bank-");
                if (compareOnlySelectedValue)
                {
                    webElementExtensions.WaitForVisibilityOfElement(_driver, bankAccountDropdownValueSelectedLocBy);
                    webElementExtensions.WaitForElement(_driver, bankAccountDropdownValueSelectedLocBy);
                    selectedValue = webElementExtensions.GetElementText(_driver, bankAccountDropdownValueSelectedLocBy);
                    if (selectedValue.Contains(bankAccountNumber.Substring(bankAccountNumber.Length - 4)) &&
                        selectedValue.Contains(bankAccountNickNameOrcheckingOrSavingsAccountType))
                        flag = true;
                }
                else
                {
                    webElementExtensions.WaitForVisibilityOfElement(_driver, bankAccountDropdownArrowIconLocBy);
                    webElementExtensions.WaitForElement(_driver, bankAccountDropdownArrowIconLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, bankAccountDropdownArrowIconLocBy);
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, emptyBankAccountDropdownLocBy))
                    {
                        webElementExtensions.WaitForElement(_driver, allValuesInbankAccountDropdownLocBy);
                        bankAccountDropdownValueLocBy = By.XPath(bankAccountDropdownValue.Replace("<BANKACCOUNT>", bankAccountName));
                        allWebElementsInMethodDropdown = _driver.FindElements(allValuesInbankAccountDropdownLocBy).ToList();
                        foreach (IWebElement element in allWebElementsInMethodDropdown)
                        {
                            string a = element.Text.TrimStart(' ').TrimEnd(' ');
                            if (element.Text.TrimStart(' ').TrimEnd(' ').Contains(bankAccountNumber.Substring(bankAccountNumber.Length - 4)))
                            {
                                flag = true;
                                break;
                            }
                        }
                        webElementExtensions.MoveToElement(_driver, bankAccountDropdownLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                    }
                    else
                    {
                        test.Log(Status.Fail, "There are no bank accounts available in drop down.");
                    }
                }
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying bank account in Method dropdown: " + ex.Message);
            }
            if (isReportRequired)
                if (compareOnlySelectedValue)
                    _driver.ReportResult(test, flag, "Successfully verified that the bank account selected in Method dropdown - '" + bankAccountName + "' is as expected.", "The bank account selected in application - '" + selectedValue + "' doesn't match with expected value - '" + bankAccountName + "'.");
                else
                    _driver.ReportResult(test, flag, "Successfully verified that the bank account - " + bankAccountName + " exists in Method dropdown.", "The bank account - " + bankAccountName + " doesn't exist in Method dropdown.");
            return flag;
        }

        /// <summary>
        /// Method to add a new bank account
        /// </summary>
        /// <param name="accountNickname">Account Nickname</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <param name="personalOrBusinessAccountType">Account type - Personal/Business</param>
        /// <param name="checkingOrSavingsAccountType">Account type - Checking/Savings</param>
        /// <param name="routingNumber">Routing number, eg:122199983</param>
        /// <param name="accountNumber">Account number, eg:123457899</param>
        /// <param name="saveBankAccount">true/false</param>
        /// <param name="makeTheAccountDefault">true/false</param>
        /// <param name="isReportRequired">True/False</param>
        public void AddAnAccount(string accountNickname = null, string firstName = "TestFN", string lastName = "TestLN", string personalOrBusinessAccountType = "personal", string checkingOrSavingsAccountType = "savings", string routingNumber = "122199983", string accountNumber = Constants.BankAccountData.BankAccountNumber, bool saveBankAccount = true, bool makeTheAccountDefault = true, bool isReportRequired = true, bool isScrollIntoViewRequired = true, bool isSelectBankAccountInTheEndRequired = false, string type = "setup")
        {
            bool flag = false, newAccountAdded = false;
            By locator = null;
            string accountNickNameOrAccountType = string.Empty;
            try
            {
                checkingOrSavingsAccountType = Char.ToUpper(checkingOrSavingsAccountType[0]) + checkingOrSavingsAccountType.Substring(1);
                personalOrBusinessAccountType = Char.ToUpper(personalOrBusinessAccountType[0]) + personalOrBusinessAccountType.Substring(1);
                accountNickNameOrAccountType = string.IsNullOrEmpty(accountNickname) ? checkingOrSavingsAccountType : accountNickname;

                if (!VerifyAccountExistsInBankAccountsPage(accountNickname, firstName, lastName, personalOrBusinessAccountType, checkingOrSavingsAccountType, routingNumber, accountNumber, false, isScrollIntoViewRequired))
                {
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, manageBankAccountsLinkEnabledLocBy))
                        webElementExtensions.WaitForElement(_driver, manageBankAccountsLinkEnabledLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                    webElementExtensions.WaitForElement(_driver, addAnAccountLinkLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, addAnAccountLinkLocBy);
                    if (!string.IsNullOrEmpty(accountNickname))
                        webElementExtensions.EnterText(_driver, accountNicknameLocBy, accountNickname, true, null, false, isScrollIntoViewRequired);
                    if (personalOrBusinessAccountType.ToLower().Equals("personal"))
                        locator = personalRadioButtonLocBy;
                    else if (personalOrBusinessAccountType.ToLower().Equals("business"))
                        locator = businessRadioButtonLocBy;
                    webElementExtensions.MoveToElement(_driver, locator);
                    webElementExtensions.ActionClick(_driver, locator);
                    if (checkingOrSavingsAccountType.ToLower().Equals("checking"))
                        locator = checkingRadioButtonLocBy;
                    else if (checkingOrSavingsAccountType.ToLower().Equals("savings"))
                        locator = savingsRadioButtonLocBy;
                    webElementExtensions.MoveToElement(_driver, locator);
                    webElementExtensions.ActionClick(_driver, locator);
                    webElementExtensions.EnterText(_driver, firstNameOnAccountTextboxLocBy, firstName, isScrollIntoViewRequired, null, false);
                    webElementExtensions.EnterText(_driver, lastNameOnAccountTextboxLocBy, lastName, isScrollIntoViewRequired, null, false);
                    webElementExtensions.EnterText(_driver, routingNumberTextboxLocBy, routingNumber, isScrollIntoViewRequired, null, false);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, bankNameDisplayedBelowRoutingNumberFieldLocBy);
                    webElementExtensions.EnterText(_driver, accountNumberTextboxLocBy, accountNumber, isScrollIntoViewRequired, null, false);
                    webElementExtensions.EnterText(_driver, confirmAccountNumberTextboxLocBy, accountNumber, isScrollIntoViewRequired, null, false);
                    if (!saveBankAccount)
                    {
                        webElementExtensions.MoveToElement(_driver, saveBankAccountCheckboxLocBy);
                        webElementExtensions.ActionClick(_driver, saveBankAccountCheckboxLocBy);
                    }
                    if (!makeTheAccountDefault)
                    {
                        webElementExtensions.MoveToElement(_driver, makeThisTheDefaultAccountCheckboxLocBy);
                        webElementExtensions.ActionClick(_driver, makeThisTheDefaultAccountCheckboxLocBy);
                    }

                    webElementExtensions.ClickElementUsingJavascript(_driver, addAccountButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, addAccountButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, addAnAccountLinkLocBy);
                    flag = VerifyValueInMethodDropdown(accountNickNameOrAccountType, accountNumber);
                    if (flag)
                    {
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                        SelectValueInMethodDropdown(accountNickNameOrAccountType, accountNumber, false);
                    }
                    newAccountAdded = true;
                }
                else
                {
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                    flag = SelectValueInMethodDropdown(accountNickNameOrAccountType, accountNumber, false);
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while adding a new bank account: " + ex.Message);
            }
            if (isReportRequired)
            {
                if (newAccountAdded)
                    _driver.ReportResult(test, flag, "Successfully added a new bank account having account number ending with - " + accountNumber.Substring(accountNumber.Length - 4) + ".", "Failed while adding a new bank account having account number ending with - " + accountNumber.Substring(accountNumber.Length - 4) + ".");
                else
                    _driver.ReportResult(test, flag, "Successfully selected existing bank account having account number ending with - " + accountNumber.Substring(accountNumber.Length - 4) + ".", "Failed while selecting existing bank account having account number ending with - " + accountNumber.Substring(accountNumber.Length - 4) + ".");
            }
            By manageAccountsLinkLocBy = By.XPath(buttonByText.Replace("<BUTTONNAME>", "Manage Accounts"));
            webElementExtensions.WaitForElement(_driver, manageAccountsLinkLocBy);
            if (isScrollIntoViewRequired)
                webElementExtensions.ScrollIntoView(_driver, manageAccountsLinkLocBy);
            webElementExtensions.ClickElementUsingJavascript(_driver, manageAccountsLinkLocBy);
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal.ToString()) && !type.Equals("setup"))
            {
                webElementExtensions.WaitForElementToBeEnabled(_driver, leaveButtonLocBy, isScrollIntoViewRequired: isScrollIntoViewRequired);
                webElementExtensions.ClickElementUsingJavascript(_driver, leaveButtonLocBy, "Leave Button");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, spinnerLocBy);
            }
            _driver.ReportResult(test, IsBankAccountAdded(accountNumber), "Successfully verified that the bank account exists in Manage Accounts page.", "Failed while verifying that the bank account exists in Manage Accounts page.");
            webElementExtensions.WaitForElementToBeEnabled(_driver, backToPaymentsButtonLocBy, "Back to Make Payments", isScrollIntoViewRequired: isScrollIntoViewRequired);
            webElementExtensions.ClickElementUsingJavascript(_driver, backToPaymentsButtonLocBy, "Back to Make Payments");
            webElementExtensions.WaitForInvisibilityOfElement(_driver, spinnerLocBy);
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal) && !(test.Model.FullName.ToString().Contains("AutoPay") || test.Model.FullName.ToString().Contains("Heloc")))
                webElementExtensions.WaitUntilUrlContains("payment");
            if (isSelectBankAccountInTheEndRequired)
                SelectValueInMethodDropdown(accountNickNameOrAccountType, accountNumber, false);
        }

        /// <summary>
        /// Method to verify if bank account exists or not in Bank Accounts page
        /// </summary>
        /// <param name="accountNickname">Account Nickname</param>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <param name="personalOrBusinessAccountType">Account type - Personal/Business</param>
        /// <param name="checkingOrSavingsAccountType">Account type - Checking/Savings</param>
        /// <param name="routingNumber">Routing number, eg:122199983</param>
        /// <param name="accountNumber">Account number, eg:123457899</param>
        /// <param name="isReportRequired">True/False</param>
        public bool VerifyAccountExistsInBankAccountsPage(string accountNickname, string firstName, string lastName, string personalOrBusinessAccountType, string checkingOrSavingsAccountType, string routingNumber, string accountNumber, bool isReportRequired = true, bool isScrollIntoViewRequired = true)
        {
            bool flag = false;
            int numberOfTableRowsCount = 0;
            By bankAccountsTableCellValuesLocBy = null;
            string accountDetails = string.IsNullOrEmpty(accountNickname) ? "|" : accountNickname + "|" + firstName.ToUpper() + " " + lastName.ToUpper() + "|" + personalOrBusinessAccountType + "/ " + checkingOrSavingsAccountType + "|" + routingNumber + "|" + "*******" + accountNumber.Substring(accountNumber.Length - 4) + "|Loan Depot Test",
                accountDetailsFromApplication = string.Empty;
            if (ConfigSettings.Environment.Equals("SG"))
                accountDetails = accountDetails.Replace("Test", "Bank");
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, manageBankAccountsLinkEnabledLocBy);
                webElementExtensions.WaitForElement(_driver, manageBankAccountsLinkEnabledLocBy);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, manageBankAccountsLinkEnabledLocBy))
                {
                    if (isScrollIntoViewRequired)
                        webElementExtensions.ScrollIntoView(_driver, manageBankAccountsLinkEnabledLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, manageBankAccountsLinkEnabledLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, manageBankAccountsLinkEnabledLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, backToPaymentsButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, backToPaymentsButtonLocBy);
                    numberOfTableRowsCount = _driver.FindElements(bankAccountsTableRows).Count;
                    for (int j = 1; j <= numberOfTableRowsCount; j++)
                    {
                        accountDetailsFromApplication = string.Empty;
                        for (int i = 1; i <= 6; i++)
                        {
                            if (!string.IsNullOrEmpty(accountDetailsFromApplication))
                                accountDetailsFromApplication = accountDetailsFromApplication + "|";
                            bankAccountsTableCellValuesLocBy = By.XPath(bankAccountsTableCellValues.Replace("<ROWNUMBER>", j.ToString()).Replace("<COLUMNNUMBER>", i.ToString()));
                            accountDetailsFromApplication = accountDetailsFromApplication + _driver.FindElement(bankAccountsTableCellValuesLocBy).Text.Trim();
                        }
                        if (string.Equals(accountDetails, accountDetailsFromApplication, StringComparison.OrdinalIgnoreCase))
                        {
                            flag = true;
                            break;
                        }
                    }
                    webElementExtensions.ClickElementUsingJavascript(_driver, backToPaymentsButtonLocBy, "Back to Make Payments");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                    if (isReportRequired)
                    {
                        _driver.ReportResult(test, flag, "Successfully verified that the bank account with details- " + accountDetails + " exists in the Bank accounts page.", "Failed while verifying that the bank account with details- " + accountDetails + " exists in the Bank accounts page.");
                    }
                }
                else
                {
                    test.Log(Status.Fail, "Manage Accounts link is not enabled to proceed further with Bank account verification.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying bank account exists or not: " + ex.Message);
            }
            return flag;
        }

        public bool IsBankAccountAdded(string accountNumber, string nickName = "", string nameOnAccount = "", string accountType = "", string routingNumber = "", string bankName = "", string defaultVal = "")
        {
            var dic = GetAccountTableAsDictionary();
            var found = true;
            var lastFourOfAccount = accountNumber.Substring(accountNumber.Length - 4);
            found = !(dic["Account no."].Contains($"*******{lastFourOfAccount}")) ? false :
                (!string.IsNullOrEmpty(nickName) && !(dic["Nick Name"].Contains(nickName))) ? false :
                (!string.IsNullOrEmpty(nameOnAccount) && !(dic["Name on Account"].Contains(nameOnAccount.ToUpper()))) ? false :
                (!string.IsNullOrEmpty(accountType) && !(dic["Account Type"].Contains(accountType))) ? false :
                (!string.IsNullOrEmpty(routingNumber) && !(dic["Routing no."].Contains(routingNumber))) ? false :
                (!string.IsNullOrEmpty(bankName) && !(dic["Bank Name"].Contains(bankName))) ? false :
                (!string.IsNullOrEmpty(defaultVal) && !(dic["Default"].Contains(defaultVal))) ? false :
                true;
            return found;
        }

        public Dictionary<string, List<string>> GetAccountTableAsDictionary()
        {
            var returnVal = new Dictionary<string, List<string>>();
            if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal.ToString()) && !(test.Model.FullName.ToString().Contains("AutoPay") || test.Model.FullName.ToString().Contains("ActiveForbearance") || test.Model.FullName.ToString().Contains("Heloc")))
                webElementExtensions.WaitUntilUrlContains("manage-accounts");

            var headers = _driver.FindElements(By.XPath((UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal)) ? "//div[@class='container-lg']//table//thead//th" : "//ldsm-manage-bank-accounts//table//thead//th"));
            foreach (var element in headers)
            {
                returnVal.Add(element.Text, new List<string>());
            }
            var rows = _driver.FindElements(By.XPath((UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal)) ? "//div[@class='container-lg']//table//tbody//tr" : "//ldsm-manage-bank-accounts//table//tbody//tr"));
            foreach (var element in rows)
            {
                var cols = element.FindElements(By.XPath((UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal)) ? ".//td" : ".//td"));
                returnVal["Nick Name"].Add(cols[0].Text);
                returnVal["Name on Account"].Add(cols[1].Text);
                returnVal["Account Type"].Add(cols[2].Text);
                returnVal["Routing no."].Add(cols[3].Text);
                returnVal["Account no."].Add(cols[4].Text);
                returnVal["Bank Name"].Add(cols[5].Text);
                var defaultValue = cols[6].FindElements(By.XPath(".//mat-radio-button[contains(@class,'mat-radio-checked')]")).Count == 0 ? "false" : "true";
                returnVal["Default"].Add(defaultValue);
            }

            return returnVal;
        }
        /// <summary>
        /// Returns all bank accounts linked to profile
        /// </summary>
        /// <returns></returns>

        public List<BankAccount> GetAllExisitingBankAccounts()
        {
            var returnVal = new List<BankAccount>();
            try
            {
                if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal.ToString()) && !(test.Model.FullName.ToString().Contains("AutoPay") || test.Model.FullName.ToString().Contains("ActiveForbearance")))
                    webElementExtensions.WaitUntilUrlContains("manage-accounts");

                var rows = _driver.FindElements(By.XPath((UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal)) ? "//div[@class='container-lg']//table//tbody//tr" : "//ldsm-manage-bank-accounts//table//tbody//tr"));
                foreach (var element in rows)
                {
                    var cols = element.FindElements(By.XPath((UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.CustomerPortal)) ? ".//td" : ".//td"));
                    var defaultValue = cols[6].FindElements(bankAccountsLocBy).Count == 0 ? false : true;
                    bool isdefault = defaultValue;
                    returnVal.Add(new BankAccount()
                    {
                        Nickname = cols[0].Text,
                        Name = cols[1].Text,
                        AcntType = cols[2].Text,
                        Routing = cols[3].Text,
                        AcntNumber = cols[4].Text,
                        BankName = cols[5].Text,
                        Default = isdefault
                    });
                }
                test.Log(Status.Info, $"Found {returnVal.Count} Bank accounts already configured ");
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting bank accounts: " + ex.Message);
            }

            return returnVal;
        }

        /// <summary>
        /// Method to verify amount entered in Monthly Payment input field
        /// </summary>
        /// <param name="amountMonthlyPayment">Amount Monthly Payment</param>
        public void VerifyAmountEnteredInMonthlyPaymentInputField(string amountMonthlyPaymentEntered)
        {
            ReportingMethods.LogAssertionEqual(test, amountMonthlyPaymentEntered, webElementExtensions.GetElementAttribute(_driver, amountInputFieldLocBy, "value").Trim(' '), "Verify amount entered in Monthly Payment input field");
        }

        /// <summary>
        /// Method to select the payment date
        /// </summary>
        /// <param name="date">Date value in the following format - June 15, 2024</param>
        /// <param name="isReportRequired">true/false</param>
        public void SelectPaymentDateInDateField(string date, bool isReportRequired = true, bool selectNextMonth = false, bool isScrollIntoViewRequired = true)
        {
            bool flag = false;
            By paymentDateToBeSelectedLocBy = null;
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                if (isScrollIntoViewRequired)
                    webElementExtensions.ScrollIntoView(_driver, paymentDatePickerIconLocBy);
                webElementExtensions.WaitUntilElementIsClickable(_driver, paymentDatePickerIconLocBy);

                if (_driver.FindElements(monthYearSpanLocBy).Count == 0)
                {
                    webElementExtensions.WaitUntilElementIsClickable(_driver, paymentDatePickerIconLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, paymentDatePickerIconLocBy);
                }
                paymentDateToBeSelectedLocBy = By.CssSelector(paymentDateTobeSelected.Replace("<DATETOBESELECTED>", date));
                bool isDateShowing = webElementExtensions.IsElementDisplayed(_driver, paymentDateToBeSelectedLocBy);
                if (selectNextMonth && !isDateShowing)
                {
                    webElementExtensions.WaitUntilElementIsClickable(_driver, nextMonthButtonCalanderLocBy);
                    webElementExtensions.ActionClick(_driver, nextMonthButtonCalanderLocBy);
                }
                webElementExtensions.WaitForVisibilityOfElement(_driver, paymentDateToBeSelectedLocBy);
                webElementExtensions.ClickElementUsingJavascript(_driver, paymentDateToBeSelectedLocBy);
                webElementExtensions.WaitForElement(_driver, paymentDatePickerIconLocBy);
                if (!_driver.FindElement(paymentDateTextboxWithDateSelectedLocBy).GetAttribute("class").Contains("untouched"))
                    flag = true;
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                reportLogger.TakeScreenshot(test, "Payment Date");
            }
            catch (Exception ex)
            {
                log.Error("Failed while selecting payment date in Date field: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully selected payment date - " + date + " in Date field.", "Failed while selecting payment date - " + date + " in Date field.");
        }

        /// <summary>
        /// Method to verify the payment date to be selected is enabled or not
        /// </summary>
        /// <param name="date">Date value in the following format - June 15, 2024</param>
        /// <param name="selectNextMonth">set to true if we want to check the date in next month calander</param>
        /// <param name="isReportRequired">true/false</param>
        /// <returns>true/false</returns>
        public bool VerifyPaymentDateToBeSelectedInDateFieldIsEnabled(string date, bool isReportRequired = true, bool selectNextMonth = false)
        {
            bool flag = false;
            By paymentDateLocBy = null;
            try
            {
                webElementExtensions.ActionClick(_driver, paymentDatePickerIconLocBy);
                webElementExtensions.WaitForElement(_driver, paymentDateCalendarPopUpLocBy);
                paymentDateLocBy = By.CssSelector(paymentDateTobeSelectedIsEnabledOrDisabledLocBy.Replace("<DATETOBESELECTED>", date));
                bool isDateShowing = webElementExtensions.IsElementDisplayed(_driver, paymentDateLocBy);
                if (selectNextMonth && !isDateShowing)
                {
                    webElementExtensions.WaitUntilElementIsClickable(_driver, nextMonthButtonCalanderLocBy);
                    webElementExtensions.ActionClick(_driver, nextMonthButtonCalanderLocBy);
                }
                if (!webElementExtensions.GetElementAttribute(_driver, paymentDateLocBy, Constants.ElementAttributes.Class).Contains("disabled"))
                    flag = true;
                reportLogger.TakeScreenshot(test, "Paymenr Date Calander");
                webElementExtensions.ActionClick(_driver, paymentDatePickerIconLocBy);
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying payment date in Date field: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully verified that payment date - " + date + " to be selected in Date field is enabled.", "Failed while verifying that payment date - " + date + " to be selected in Date field is enabled or not.");
            return flag;
        }

        public List<Hashtable> GetListOfLoanLevelData(string loanDetailsQuery, bool skipAssertEvenIfNoData = false)
        {
            List<string> usedLoanTestData = new List<string>();
            List<string> loanDataRequired = typeof(Constants.LoanLevelDataColumns)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(field => field.FieldType == typeof(string))
            .Select(field => (string)field.GetValue(null))
            .ToList();

            return GetLoanDataFromDatabase(loanDetailsQuery, null, loanDataRequired, usedLoanTestData, ConfigSettings.NumberOfLoanTestDataRequired, skipAssertEvenIfNoData);
        }

        /// <summary>
        /// Method to get the Element Amount Text using Java Script Executor
        /// </summary>
        /// <param name="locator">Element locator - By Id, css, xpath</param>
        /// <returns>Element value</returns>
        public string GetValueUsingJS(By locator, bool isReportRequired = false)
        {
            bool flag = false;
            string returnValue = "";
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)_driver;
                var elementText = js.ExecuteScript("return arguments[0].value", _driver.FindElement(locator));
                double amount = Convert.ToDouble(elementText.ToString().Replace("$", ""));
                returnValue = "$" + amount.ToString("N");
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Element Amount Text using Java Script " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Element Amount Text using Java Script.", "Failed while getting Element Amount Text using Java Script.");
            return returnValue;
        }

        /// <summary>
        /// Method to click on any element by using label name in application
        /// </summary>
        /// <param name="labelName">Label Name of any element. Can be button, link, dropdowns, icon, etc.</param>
        /// <param name="message">Message to be reported in extent report</param>
        public void ClickElementUsingTheLabelInApplication(string labelName, string message = "", bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                By containsTextLocatorLocBy = By.XPath(containsTextLocator.Replace("<LABELNAME>", labelName));
                webElementExtensions.WaitUntilElementIsClickable(_driver, containsTextLocatorLocBy);
                webElementExtensions.ScrollIntoView(_driver, containsTextLocatorLocBy);
                webElementExtensions.ClickElement(_driver, containsTextLocatorLocBy);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while clicking on the element - " + labelName + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully clicked on " + message == "" ? labelName : message, "Failed while clicking on " + message == "" ? labelName : message);
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
        /// Method to Click Confirm Button Payment Review Page
        /// </summary>
        public void ClickConfirmButtonPaymentReviewPage()
        {
            webElementExtensions.ScrollToBottom(_driver);
            By locatorBy = By.XPath(buttonByText.Replace("<BUTTONNAME>", "Confirm Payment"));
            webElementExtensions.WaitForElement(_driver, locatorBy);
            webElementExtensions.ClickElement(_driver, locatorBy);
        }

        /// <summary>
        /// Method to Verify Payment Confirmation Success Message Is Displayed
        /// </summary>
        /// <param name="message">Payment Confirmation Success Message</param>
        /// <param name="type">Payment flow - setup/Edit</param>
        public void VerifyPaymentConfirmationSuccessMessageIsDisplayed(string message, string type = "setup", bool isScrollIntoViewRequired = true)
        {
            By locBy = By.XPath(spanByText.Replace("<TEXT>", message));
            webElementExtensions.WaitForVisibilityOfElement(_driver, locBy);
            ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisplayed(_driver, locBy, isScrollIntoViewRequired: isScrollIntoViewRequired), type == "setup" ? "Verify success message " + message : "Verify " + type + " success message" + message);
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

        #endregion CommonMethodsBasedOnFunctionality

        #region Dates
        /// <summary>
        /// Verifies the enabled and disabled payment dates for a specific month.
        /// </summary>
        /// <param name="isReportRequired">Indicates whether a report is required.</param>
        public void VerifyEnabledDisabledPaymentDatesForMonth(bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                if (_driver.FindElements(monthYearSpanLocBy).Count == 0)
                {
                    webElementExtensions.ActionClick(_driver, paymentDatePickerIconLocBy);
                }

                DateTime currentDate = DateTime.Now;
                string[] displayedMonthYear = webElementExtensions.GetElementText(_driver, monthYearSpanLocBy).Split(' ');
                int advanceMonths = GetMonthNumber(displayedMonthYear[0].Trim()) - currentDate.Month;
                int advanceYears = Convert.ToInt32(displayedMonthYear[1].Trim()) - currentDate.Year;

                List<string> disabledDatesList = _driver.FindElements(disabledPaymentDatesLocBy).Select(x => x.GetAttribute(Constants.ElementAttributes.AriaLabel).Trim()).ToList();
                List<string> enabledDatesList = _driver.FindElements(enabledPaymentDatesLocBy).Select(x => x.GetAttribute(Constants.ElementAttributes.AriaLabel).Trim()).ToList();

                ReportingMethods.LogAssertionListEqual(test, GetDisabledDaysForMonth(advanceMonths, advanceYears), disabledDatesList, "Verify  disabled dates");
                ReportingMethods.LogAssertionListEqual(test, GetEnabledDaysForMonth(advanceMonths, advanceYears), enabledDatesList, "Verify enabled dates");

                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying enabled and disabled dates of month " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully verified enabled and disabled dates of month.", "Failed while verifying enabled and disabled dates of month.");
        }

        /// <summary>
        /// Retrieves the bank holidays for a specific year.
        /// </summary>
        /// <param name="year">The year for which to retrieve the bank holidays. Default value is "2025".</param>
        /// <param name="isReportRequired">Indicates whether a report is required. Default value is false.</param>
        /// <returns>A list of DateTime objects representing the bank holidays.</returns>
        public List<DateTime> GetBankHolidays(string year = "2025", bool isReportRequired = false)
        {
            bool flag = false;
            List<DateTime> days = new List<DateTime>();

            try
            {
                List<string> columnDataRequired = typeof(Constants.SettingDataColumns).GetFields(BindingFlags.Public | BindingFlags.Static)
               .Where(field => field.FieldType == typeof(string)).Select(field => (string)field.GetValue(null)).ToList();
                string query = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetSettingDetailsForBankHolidays));
                Hashtable hashData = ExecuteQueryAndGetDataFromDataBase(query, null, columnDataRequired).FirstOrDefault();
                string[] settingValue = hashData[Constants.SettingDataColumns.SettingValue].ToString().Replace("[", "").Replace("]", "").Split(',');
                foreach (string value in settingValue)
                {
                    string datevalue = value.Replace("\"", "").Trim();
                    if (!string.IsNullOrEmpty(datevalue) && value.Contains(year))
                    {
                        if (DateTime.TryParse(datevalue, out DateTime date))
                        {
                            days.Add(date);
                        }
                        else
                        {
                            log.Info("Failed while converting date " + datevalue);
                        }
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting holiday dates of the year " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got holiday dates of the year.", "Failed while getting holiday dates of the year.");

            // Log the bank holidays as a formatted string.
            test.Log(Status.Info, $"Bank Holidays: {string.Join(", ", days.Select(d => d.ToString("yyyy-MM-dd")))}");

            return days;
        }


        /// <summary>
        /// Retrieves the Setting value details from Settings table.
        /// </summary>
        /// <param name="settingKeyValue">The SettingKeyValue for which to retrieve Setting Details. Default value is "" for getting all Setting Details.</param>
        /// <param name="isReportRequired">Indicates whether a report is required. Default value is false.</param>
        /// <returns>A list of Hashtable objects representing the Setting Value Details.</returns>
        public List<Hashtable> GetSettingValueDetailsFromSettingsTable(string settingKeyValue = "", bool isReportRequired = false)
        {
            bool flag = false;
            List<Hashtable> settingValueData = new List<Hashtable>();

            try
            {
                List<string> columnDataRequired = typeof(Constants.SettingDataColumns).GetFields(BindingFlags.Public | BindingFlags.Static)
               .Where(field => field.FieldType == typeof(string)).Select(field => (string)field.GetValue(null)).ToList();
                string getSettingDetailsBySettingKeyValue = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetSettingDetailsBySettingKeyValue));
                settingValueData = ExecuteQueryAndGetDataFromDataBase(getSettingDetailsBySettingKeyValue.Replace("SETTINGKEYVALUE", settingKeyValue), null, columnDataRequired);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Setting Value Details for the Setting Key =" + settingKeyValue == "" ? "All" : settingKeyValue.Replace("_", " ") + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Setting Value Details for the Setting Key.", "Failed while getting Setting Value Details for the Setting Key.");

            return settingValueData;
        }

        /// <summary>
        /// Retrieves the Late Fee, NSF Fee, Other Fee value details from Fees table.
        /// </summary>
        /// <param name="loanNumber">The LoanNumber for which to retrieve Fees Details.</param>
        /// <param name="isReportRequired">Indicates whether a report is required. Default value is false.</param>
        /// <returns>A list of Hashtable objects representing the Fees Value Details.</returns>
        public List<Hashtable> GetFeesWithLoanNumber(string loanNumber, bool isReportRequired = false)
        {
            bool flag = false;
            List<Hashtable> feeValueData = new List<Hashtable>();

            try
            {
                List<string> columnDataRequired = typeof(Constants.FeesDataColumns).GetFields(BindingFlags.Public | BindingFlags.Static)
               .Where(field => field.FieldType == typeof(string)).Select(field => (string)field.GetValue(null)).ToList();
                string getSettingDetailsBySettingKeyValue = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetFeesWithLoanNumber));
                feeValueData = ExecuteQueryAndGetDataFromDataBase(getSettingDetailsBySettingKeyValue.Replace("LOANNUMBER", loanNumber), null, columnDataRequired);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Fees Value Details for the Loan Number =" + loanNumber + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Fees Value Details for the Loan Number.", "Failed while getting Fees Value Details for the Loan Number.");

            return feeValueData;
        }


        /// <summary>
        /// Retrieves the Fees value details from Fees table.
        /// </summary>
        /// <param name="loanNumber">The LoanNumberValue for which to retrieve Fees Details.</param>
        /// <param name="isReportRequired">Indicates whether a report is required. Default value is false.</param>
        /// <returns>A list of Fees value details from Fees table.</returns>
        public List<Hashtable> GetPastAndUpcomingFeesForHeloc(string loanNumber, bool isReportRequired = false)
        {
            bool flag = false;
            List<Hashtable> feesValueData = new List<Hashtable>();

            try
            {
                List<string> columnDataRequired = typeof(Constants.PastAndUpcomingFessDataColumns).GetFields(BindingFlags.Public | BindingFlags.Static)
               .Where(field => field.FieldType == typeof(string)).Select(field => (string)field.GetValue(null)).ToList();
                string getPastAndUpcomingFeesForHeloc = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetPastAndUpcomingFeesWithLoanNumberForHelocOTP));
                getPastAndUpcomingFeesForHeloc = getPastAndUpcomingFeesForHeloc.Replace("LESS_THEN", "<");
                getPastAndUpcomingFeesForHeloc = getPastAndUpcomingFeesForHeloc.Replace("#", loanNumber);
                feesValueData = ExecuteQueryAndGetDataFromDataBase(getPastAndUpcomingFeesForHeloc, null, columnDataRequired);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Fees Details for the loanNumber =" + loanNumber + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Fees Details for the loanNumber.", "Failed while  getting Fees Details for the loanNumber.");

            return feesValueData;
        }


        /// <summary>
        /// Retrieves the Pending Payment details from PaymentSetup table.
        /// </summary>
        /// <param name="loanNumber">The LoanNumberValue for which to retrieve PaymentSetup Details.</param>
        /// <param name="isReportRequired">Indicates whether a report is required. Default value is false.</param>
        /// <returns>A bool of Pending Payment value details from PaymentSetup table.</returns>
        public bool GetPendingPaymentDetails(string loanNumber, bool isReportRequired = false)
        {
            bool flag = false, pendingPaymentsCount = false;

            try
            {
                List<string> columnDataRequired = typeof(Constants.PaymentSetupDataColumns).GetFields(BindingFlags.Public | BindingFlags.Static)
               .Where(field => field.FieldType == typeof(string)).Select(field => (string)field.GetValue(null)).ToList();
                string getPendingPaymentDetailsWithLoanNumber = UtilAdditions.ReplaceSymbols(UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetPendingPaymentDetailsWithLoanNumber));
                getPendingPaymentDetailsWithLoanNumber = getPendingPaymentDetailsWithLoanNumber.Replace("#", loanNumber);
                List<Hashtable> pendingPaymentData = ExecuteQueryAndGetDataFromDataBase(getPendingPaymentDetailsWithLoanNumber, null, columnDataRequired);
                if (pendingPaymentData[0][Constants.PaymentSetupDataColumns.LoanNumber].ToString() != "-1")
                {
                    pendingPaymentsCount = true;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Pending Payment Details for the loanNumber =" + loanNumber + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Pending Payment Details for the loanNumber.", "Failed while  getting Pending Payment Details for the loanNumber.");

            return pendingPaymentsCount;
        }

        /// <summary>
        /// Retrieves a list of disabled days for a given month.
        /// </summary>
        /// <param name="advanceMonths">The number of months to advance from the current date.</param>
        /// <param name="advanceYears">The number of years to advance from the current date.</param>
        /// <param name="isReportRequired">Indicates whether a report is required.</param>
        /// <returns>A list of disabled days in the format "MMMM d, yyyy".</returns>
        public List<string> GetDisabledDaysForMonth(int advanceMonths = 0, int advanceYears = 0, bool isReportRequired = false)
        {
            bool flag = false;
            List<string> disabledDays = new List<string>();

            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime holidayChecktDate = DateTime.Now;
                currentDate = currentDate.AddMonths(advanceMonths);
                currentDate = currentDate.AddYears(advanceYears);
                int totaldays = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                List<DateTime> holidays = GetBankHolidays();

                for (int day = 0; day < totaldays; day++)
                {
                    DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

                    DateTime date = firstDayOfMonth.AddDays(day);
                    if (advanceMonths > 0)//In case of advance months satuday, sunday and holidays are disabled
                    {
                        holidayChecktDate = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
                        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || holidays.Contains(holidayChecktDate))
                        {
                            disabledDays.Add(date.ToString("MMMM d, yyyy").Trim());
                        }
                    }
                    else
                    {
                        holidayChecktDate = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
                        if (date <= currentDate || date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || holidays.Contains(holidayChecktDate))
                        {
                            if (date.Day == currentDate.Day)
                            {
                                DateTime currentTime = DateTime.UtcNow;
                                DateTime currentCSTTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
                                DateTime cutOffCSTTime = new DateTime(currentCSTTime.Year, currentCSTTime.Month, currentCSTTime.Day, 20, 00, 00);
                                if (currentCSTTime > cutOffCSTTime && (currentCSTTime.ToString("dd MMM yyyy") == currentDate.ToString("dd MMM yyyy")))
                                {
                                    disabledDays.Add(date.ToString("MMMM d, yyyy").Trim());
                                }
                                else
                                {
                                    holidayChecktDate = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
                                    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || holidays.Contains(holidayChecktDate))
                                    {
                                        disabledDays.Add(date.ToString("MMMM d, yyyy").Trim());
                                    }
                                }
                            }
                            else
                            {
                                disabledDays.Add(date.ToString("MMMM d, yyyy").Trim());
                            }
                        }

                    }

                }
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting disabled dates for month " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got disabled dates for month.", "Failed while getting disabled dates for month.");

            return disabledDays;
        }

        /// <summary>
        /// Retrieves a list of enabled days for a specific month.
        /// </summary>
        /// <param name="advanceMonths">The number of months to advance from the current date. Default is 0.</param>
        /// <param name="advanceYears">The number of years to advance from the current date.</param>
        /// <param name="isReportRequired">Indicates whether a report is required. Default is false.</param>
        /// <returns>A list of enabled days in the specified month.</returns>
        public List<string> GetEnabledDaysForMonth(int advanceMonths = 0, int advanceYears = 0, bool isReportRequired = false)
        {
            bool flag = false;
            List<string> workingDays = new List<string>();

            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime holidayChecktDate = DateTime.Now;
                currentDate = currentDate.AddMonths(advanceMonths);
                currentDate = currentDate.AddYears(advanceYears);
                int totalDaysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                List<DateTime> holidays = GetBankHolidays();

                if (advanceMonths == 0)
                {
                    DateTime currentTime = DateTime.UtcNow;
                    DateTime currentCSTTime = TimeZoneInfo.ConvertTimeFromUtc(currentTime, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
                    DateTime cutOffCSTTime = new DateTime(currentCSTTime.Year, currentCSTTime.Month, currentCSTTime.Day, 20, 00, 00);
                    if (currentCSTTime > cutOffCSTTime && currentCSTTime.Day == cutOffCSTTime.Day && currentCSTTime.Month == cutOffCSTTime.Month && (currentCSTTime.ToString("dd MMM yyyy") == currentDate.ToString("dd MMM yyyy")))
                    {
                        currentDate = currentDate.AddDays(1);
                    }

                    int remainingDays = totalDaysInMonth - currentDate.Day;
                    for (int day = 0; day <= remainingDays; day++)
                    {
                        DateTime date = currentDate.AddDays(day);
                        holidayChecktDate = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
                        if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && !holidays.Contains(holidayChecktDate))
                        {
                            workingDays.Add(date.ToString("MMMM d, yyyy").Trim());
                        }
                    }
                }
                else
                {
                    for (int day = 0; day < totalDaysInMonth; day++)
                    {
                        DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
                        DateTime date = firstDayOfMonth.AddDays(day);
                        holidayChecktDate = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
                        if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && !holidays.Contains(holidayChecktDate))
                        {
                            workingDays.Add(date.ToString("MMMM d, yyyy").Trim());
                        }
                    }
                }

                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting enabled dates of month " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got enabled dates of month.", "Failed while getting enabled dates of month.");

            return workingDays;
        }

        /// <summary>
        /// Verifies that the minimum pay date of the month is not a banking holiday.
        /// </summary>
        /// <param name="isReportRequired">Indicates whether a report is required.</param>
        public void VerifyMinimumPayDateOfMonthNotBankingHoliday(bool isReportRequired = true)
        {
            bool flag = false;
            try
            {
                if (_driver.FindElements(monthYearSpanLocBy).Count == 0)
                {
                    webElementExtensions.ActionClick(_driver, paymentDatePickerIconLocBy);
                }

                DateTime currentDate = DateTime.Now;
                string[] displayedMonthYear = webElementExtensions.GetElementText(_driver, monthYearSpanLocBy).Split(' ');
                int advanceMonths = GetMonthNumber(displayedMonthYear[0].Trim()) - currentDate.Month;
                int advanceYear = Convert.ToInt32(displayedMonthYear[1].Trim()) - currentDate.Year;
                string actualMinDate = webElementExtensions.GetElementAttribute(_driver, activePaymentDateTobeSelectedLocBy, Constants.ElementAttributes.AriaLabel);
                string expectedMinDate = GetEnabledDaysForMonth(advanceMonths, advanceYear).FirstOrDefault();
                ReportingMethods.LogAssertionEqual(test, expectedMinDate, actualMinDate, "Verify Minimum Pay Date of Month is not a Banking Holiday");
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying mimimum payment date in Date field: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully verified mimimum payment date in Date field.", "Failed while verifying mimimum payment date in Date field.");
        }

        /// <summary>
        /// Verifies if the maximum pay date is the last day of the month and not a banking holiday.
        /// </summary>
        /// <param name="advanceMonths">The number of months to advance from the current month. Default is 0.</param>
        /// <param name="advanceYear">The number of years to advance.</param>
        /// <param name="isReportRequired">Flag indicating whether to generate a report. Default is true.</param>
        public void VerifyMaximumPayDateIsLastDayOfMonthNotBankingHoliday(/*int advanceMonths = 0, int advanceYear = 0,*/ bool isReportRequired = true)
        {
            bool flag = false;
            try
            {
                if (_driver.FindElements(monthYearSpanLocBy).Count == 0)
                {
                    webElementExtensions.ActionClick(_driver, paymentDatePickerIconLocBy);
                }

                DateTime currentDate = DateTime.Now;
                string[] displayedMonthYear = webElementExtensions.GetElementText(_driver, monthYearSpanLocBy).Split(' ');
                int advanceMonths = GetMonthNumber(displayedMonthYear[0].Trim()) - currentDate.Month;
                int advanceYear = Convert.ToInt32(displayedMonthYear[1].Trim()) - currentDate.Year;

                string actualMaxDate = webElementExtensions.GetElementAttribute(_driver, maxEnabledPaymentDateLocBy, "aria-label");
                string expectedMaxDate = GetEnabledDaysForMonth(advanceMonths, advanceYear).LastOrDefault();
                ReportingMethods.LogAssertionEqual(test, expectedMaxDate, actualMaxDate, "Verify Maximum Pay Date is last day of Month is not a Banking Holiday");
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying maximum payment date in Date field: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully verified maximum payment date in Date field.", "Failed while verifying maximum payment date in Date field.");
        }

        /// <summary>
        /// Retrieves the month and year from a date picker.
        /// </summary>
        /// <param name="isReportRequired">Flag indicating whether a report is required.</param>
        /// <returns>A dictionary containing the month and year.</returns>
        public Dictionary<string, int> GetDatePickerMonthYear(bool isReportRequired = false)
        {
            bool flag = false;
            Dictionary<string, int> dic = new Dictionary<string, int>();
            try
            {
                if (_driver.FindElements(monthYearSpanLocBy).Count == 0)
                {
                    webElementExtensions.ActionClick(_driver, paymentDatePickerIconLocBy);
                }
                webElementExtensions.WaitForVisibilityOfElement(_driver, monthYearSpanLocBy);
                string[] displayedMonthYear = webElementExtensions.GetElementText(_driver, monthYearSpanLocBy).Split(' ');
                int month = GetMonthNumber(displayedMonthYear[0].Trim());
                int year = Convert.ToInt32(displayedMonthYear[1].Trim());

                DateTime currentDate = DateTime.Now;
                int monthDiff = month - currentDate.Month;
                int yearDiff = year - currentDate.Year;

                dic.Add("Month", monthDiff);
                dic.Add("Year", yearDiff);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting date picker month and year " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got date picker month and year.", "Failed while getting date picker month and year.");
            return dic;
        }

        /// <summary>
        /// Method to Get Working Date After 16th Date
        /// </summary>
        /// <param name="type">Payment flow - Setup/Edit</param>
        /// <param name="isReportRequired">True/False</param>
        /// <returns>Date in MMMM dd, yyyy format</returns>
        public string GetWorkingDateAfter16thDate(string type = "setup", bool isReportRequired = false)
        {
            bool flag = false;
            string value = "";

            try
            {
                if (_driver.FindElements(monthYearSpanLocBy).Count == 0)
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, paymentDatePickerIconLocBy);
                }

                DateTime currentDate = DateTime.Now;
                webElementExtensions.WaitForVisibilityOfElement(_driver, monthYearSpanLocBy);
                string[] displayedMonthYear = webElementExtensions.GetElementText(_driver, monthYearSpanLocBy).Split(' ');
                int advanceMonths = GetMonthNumber(displayedMonthYear[0].Trim()) - currentDate.Month;
                int advanceYear = Convert.ToInt32(displayedMonthYear[1].Trim()) - currentDate.Year;

                List<DateTime> bankHoliDays = GetBankHolidays(Convert.ToString(currentDate.Year + advanceYear));

                currentDate = currentDate.AddMonths(advanceMonths);
                currentDate = currentDate.AddYears(advanceYear);

                DateTime date;
                DateTime monthLastDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                bool isNextMonthEnabled = webElementExtensions.GetElementAttribute(_driver, nextMonthLocBy, "disabled") == "true" ? false : true;

                if (currentDate.Day > 16 && advanceMonths == 0)
                {
                    currentDate = currentDate.AddDays(0);
                }
                else if (currentDate.Day > 16 && advanceMonths > 0)
                {
                    currentDate = new DateTime(currentDate.Year, currentDate.Month, 16);
                }
                else
                {
                    currentDate = currentDate.AddDays(17 - currentDate.Day);
                }
                date = GetWorkingDay(currentDate, monthLastDate, bankHoliDays, isNextMonthEnabled);
                if (type.ToLower() == "edit")
                {
                    date = date.AddDays(1);
                    date = GetWorkingDay(date, monthLastDate, bankHoliDays, isNextMonthEnabled);
                }
                value = date.ToString("MMMM d, yyyy");
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting working date after 16th date of the month " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got  working date after 16th date of the month.", "Failed while getting  working date after 16th date of the month.");
            ReportingMethods.Log(test, "Working Date: " + value);
            return value;
        }

        /// <summary>
        /// Returns the corresponding month number for the given month name.
        /// </summary>
        /// <param name="monthname">The name of the month.</param>
        /// <returns>The corresponding month number. Returns 0 if the month name is invalid.</returns>
        public int GetMonthNumber(string monthname, bool isReportRequired = false)
        {
            bool flag = false;
            int month = 0;

            try
            {
                monthname = monthname.ToLower();
                if (monthname == "jan")
                {
                    month = 1;
                }
                else if (monthname == "feb")
                {
                    month = 2;
                }
                else if (monthname == "mar")
                {
                    month = 3;
                }
                else if (monthname == "apr")
                {
                    month = 4;
                }
                else if (monthname == "may")
                {
                    month = 5;
                }
                else if (monthname == "jun")
                {
                    month = 6;
                }
                else if (monthname == "jul")
                {
                    month = 7;
                }
                else if (monthname == "aug")
                {
                    month = 8;
                }
                else if (monthname == "sep")
                {
                    month = 9;
                }
                else if (monthname == "oct")
                {
                    month = 10;
                }
                else if (monthname == "nov")
                {
                    month = 11;
                }
                else if (monthname == "dec")
                {
                    month = 12;
                }
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting month number " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got month number.", "Failed while getting month number.");
            return month;
        }

        /// <summary>
        /// Method to Get Working Date
        /// </summary>
        /// <param name="type">The type of operation (default value is "setup").</param>
        /// <param name="advanceWorkingDays">The number of working days to advance from the current or base date.</param>
        /// <param name="isReportRequired">Indicates whether a report is required (default value is false).</param>
        /// <returns>The working date in the format "MMMM dd, yyyy".</returns>
        public string GetWorkingDate(string type = "setup", int advanceWorkingDays = 0, bool isReportRequired = false)
        {
            bool flag = false;
            string value = "";
            try
            {
                webElementExtensions.ScrollIntoView(_driver, paymentDatePickerIconLocBy);
                if (_driver.FindElements(monthYearSpanLocBy).Count == 0)
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, paymentDatePickerIconLocBy);
                }

                DateTime currentDate = DateTime.Now;
                webElementExtensions.WaitForVisibilityOfElement(_driver, monthYearSpanLocBy);
                string[] displayedMonthYear = webElementExtensions.GetElementText(_driver, monthYearSpanLocBy).Split(' ');
                int advanceMonths = GetMonthNumber(displayedMonthYear[0].Trim()) - currentDate.Month;
                int advanceYear = Convert.ToInt32(displayedMonthYear[1].Trim()) - currentDate.Year;
                List<DateTime> bankHoliDays = GetBankHolidays(Convert.ToString(currentDate.Year + advanceYear));

                currentDate = currentDate.AddMonths(advanceMonths);
                currentDate = currentDate.AddYears(advanceYear);
                if (advanceMonths > 0 || advanceYear > 0)
                {
                    currentDate = new DateTime(currentDate.Year, currentDate.Month, 1);
                }
                DateTime monthLastDate = new DateTime(currentDate.Year, currentDate.Month, DateTime.DaysInMonth(currentDate.Year, currentDate.Month));
                bool isNextMonthEnabled = webElementExtensions.GetElementAttribute(_driver, nextMonthLocBy, "disabled") == "true" ? false : true;

                DateTime workingDay = GetWorkingDay(currentDate, monthLastDate, bankHoliDays, isNextMonthEnabled);
                if (advanceWorkingDays > 0)
                {
                    workingDay = workingDay.AddDays(advanceWorkingDays);
                    workingDay = GetWorkingDay(workingDay, monthLastDate, bankHoliDays, isNextMonthEnabled);
                }
                if (type.ToLower() == "edit")
                {
                    workingDay = workingDay.AddDays(1);
                    workingDay = GetWorkingDay(workingDay, monthLastDate, bankHoliDays, isNextMonthEnabled);
                }

                value = workingDay.ToString("MMMM d, yyyy");
                ReportingMethods.Log(test, "Working Date: " + value);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting working date for the month " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got working date for the month.", "Failed while getting working date for the month.");
            return value;
        }

        /// <summary>
        /// Calculates the working day based on the given date and other parameters.
        /// </summary>
        /// <param name="date">The date for which the working day needs to be calculated.</param>
        /// <param name="monthLastDate">The last date of the month.</param>
        /// <param name="bankHolidays">A list of bank holidays.</param>
        /// <param name="isNextMonthEnabled">A flag indicating whether the calculation should consider the next month.</param>
        /// <param name="isReportRequired">A flag indicating whether a report is required.</param>
        /// <returns>The calculated working day.</returns>
        public DateTime GetWorkingDay(DateTime date, DateTime monthLastDate, List<DateTime> bankHolidays, bool isNextMonthEnabled, bool isReportRequired = false)
        {
            bool flag = false;
            DateTime workingDay = date;
            try
            {
                // Get the current system timezone
                TimeZoneInfo systemTimeZone = TimeZoneInfo.Local;

                // Get the CST timezone
                TimeZoneInfo cstTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

                // Get the current time in the CST timezone
                DateTime cstTime = TimeZoneInfo.ConvertTime(DateTime.Now, systemTimeZone, cstTimeZone);

                // Set the cutoff time to 8 PM in CST
                DateTime cutoffTime = new DateTime(cstTime.Year, cstTime.Month, cstTime.Day, 20, 0, 0);

                DateTime holidayCheckDate = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
                if (date.DayOfWeek.ToString().ToLower() == "saturday")
                {
                    workingDay = date.AddDays(2);
                    workingDay = GetWorkingDay(workingDay, monthLastDate, bankHolidays, isNextMonthEnabled);
                }
                else if (date.DayOfWeek.ToString().ToLower() == "sunday")
                {
                    workingDay = date.AddDays(1);
                    workingDay = GetWorkingDay(workingDay, monthLastDate, bankHolidays, isNextMonthEnabled);
                }
                else if (bankHolidays.Contains(holidayCheckDate))
                {
                    workingDay = date.AddDays(1);
                    workingDay = GetWorkingDay(workingDay, monthLastDate, bankHolidays, isNextMonthEnabled);
                }
                else if (DateTime.Compare(cstTime, cutoffTime) > 0 && DateTime.Now.ToString("dd MMM yyyy") == date.ToString("dd MMM yyyy") && (cstTime.ToString("dd MMM yyyy") == DateTime.Now.ToString("dd MMM yyyy")))
                {
                    workingDay = date.AddDays(1);
                    workingDay = GetWorkingDay(workingDay, monthLastDate, bankHolidays, isNextMonthEnabled);
                }
                else if (DateTime.Compare(cstTime, cutoffTime) > 0 && DateTime.Now.ToString("dd MMM yyyy") == date.ToString("dd MMM yyyy") && (cstTime.ToString("dd MMM yyyy") != DateTime.Now.ToString("dd MMM yyyy")))
                {
                    workingDay = cstTime.AddDays(1);
                    if (bankHolidays.Contains(holidayCheckDate))
                    {
                        workingDay = workingDay.AddDays(1);
                    }
                    if (workingDay.DayOfWeek.ToString().ToLower() == "saturday")
                    {
                        workingDay = workingDay.AddDays(2);
                    }
                    if (workingDay.DayOfWeek.ToString().ToLower() == "sunday")
                    {
                        workingDay = workingDay.AddDays(1);
                    }
                    if (workingDay > monthLastDate && !isNextMonthEnabled)
                    {
                        workingDay = monthLastDate;
                    }
                }
                else if (date > monthLastDate && !isNextMonthEnabled)
                {
                    workingDay = monthLastDate;
                }

                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting working date " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got working date.", "Failed while getting working date.");
            return workingDay;
        }

        /// <summary>
        /// Gets the Heloc working date based on the given parameters. E.g., Method to Get Second Working Date After 15th
        /// </summary>
        /// <param name="billDay">Heloc Bill Generation Day of Month in MSP.</param>
        /// <param name="businessDays">Business days to reflect Data in AP</param>
        /// <param name="isReportRequired">Indicates whether a report is required (default value is false).</param>
        /// <returns>The working date</returns>
        public DateTime GetHelocWorkingDate(int billDay, int businessDays = 0, bool isReportRequired = false)
        {
            bool flag = false;
            DateTime currentDate = DateTime.Now;
            DateTime date = new DateTime(currentDate.Year, currentDate.Month, billDay);
            try
            {
                List<DateTime> bankHoliDays = GetBankHolidays(Convert.ToString(currentDate.Year));
                for (int i = 0; i < businessDays; i++)
                {
                    if (i != 0) { date = date.AddDays(1); }
                    date = GetWorkingDay(date, bankHoliDays);
                }

                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Heloc working date for the month " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got Heloc working date for the month.", "Failed while Heloc getting working date for the month.");
            return date;
        }

        /// <summary>
        /// Calculates the working day based on the given date and other parameters.
        /// </summary>
        /// <param name="date">The date for which the working day needs to be calculated.</param>
        /// <param name="bankHolidays">A list of bank holidays.</param>
        /// <param name="isReportRequired">A flag indicating whether a report is required.</param>
        /// <returns>The calculated working day.</returns>
        public DateTime GetWorkingDay(DateTime date, List<DateTime> bankHolidays, bool isReportRequired = false)
        {
            bool flag = false;
            DateTime workingDay = date;
            try
            {

                DateTime holidayCheckDate = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
                if (date.DayOfWeek.ToString().ToLower() == "saturday")
                {
                    workingDay = date.AddDays(2);
                    workingDay = GetWorkingDay(workingDay, bankHolidays);
                }
                else if (date.DayOfWeek.ToString().ToLower() == "sunday")
                {
                    workingDay = date.AddDays(1);
                    workingDay = GetWorkingDay(workingDay, bankHolidays);
                }
                else if (bankHolidays.Contains(holidayCheckDate))
                {
                    workingDay = date.AddDays(1);
                    workingDay = GetWorkingDay(workingDay, bankHolidays);
                }
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting working date " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got working date.", "Failed while getting working date.");
            return workingDay;
        }

        /// <summary>
        /// Retrieves First Working Date Of Month for a specific month.
        /// </summary>
        /// <param name="advanceMonths">The number of months to advance from the current date. Default is 0.</param>
        /// <param name="advanceYears">The number of years to advance from the current date.</param>
        /// <param name="isReportRequired">Indicates whether a report is required. Default is false.</param>
        /// <returns>First Working Date Of Month in the specified month.</returns>
        public DateTime GetFirstWorkingDateOfMonth(int advanceMonths = 0, int advanceYears = 0, bool isReportRequired = false)
        {
            bool flag = false;
            DateTime currentDate = DateTime.Now;
            DateTime date = new DateTime(currentDate.Year + advanceYears, currentDate.Month + advanceMonths, currentDate.Day);
            try
            {
                List<DateTime> holidays = GetBankHolidays();
                int totalDaysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
                for (int day = 1; day <= totalDaysInMonth; day++)
                {
                    date = new DateTime(date.Year, date.Month, day);
                    if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && !holidays.Contains(date))
                    {
                        break;
                    }
                }
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting first working date of month " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully got first working date of month.", "Failed while getting first working date of month.");
            return date;
        }

        #endregion Dates

        /// <summary>
        /// Method to convert amount into correct amount format
        /// </summary>
        /// <param name="value">Postive or Negative Amount : 10, -999 </param>
        /// <returns> Positive: $10, Negative: -$999 </returns>
        public string GetDollarFormattedAmount(object value, bool isReportRequired = false)
        {
            bool flag = false;
            string amount = "";

            try
            {
                if (Convert.ToDouble(value) < 0.0)
                {
                    amount = value.ToString().Replace("-", "");
                    amount = "-" + $"${Convert.ToDouble(amount).ToString("N")}";
                }
                else
                {
                    amount = $"${Convert.ToDouble(value).ToString("N")}";
                }
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while converting amount: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully converting amount - " + value.ToString() + "in correct amount format.", "Failed while converting amount - " + value.ToString() + "in correct amount format.");

            return amount;
        }

        /// <summary>
        /// To get the Dates range in format "MMMM d, yyyy"
        /// </summary>
        /// <param name="startDate"> "2025-01-01" </param>
        /// <param name="endDate">"2025-01-15" </param>
        /// <returns></returns>
        public List<string> GetDatesInRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                return null;
            List<string> dateList = new List<string>();

            try
            {
                // Loop through each date from startDate to endDate (inclusive)
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    // Add the date formatted as "MMMM d, yyyy"
                    dateList.Add(date.ToString("MMMM d, yyyy"));
                }
            }
            catch (Exception ex)
            {
                test.Log(Status.Error, $"Something happened while getting the expected Enabled dates: <b>{ex.Message}</b>");
            }
            return dateList;
        }

        /// <summary>
        /// Method to calculate the date after a specified number of business days (including weekends)
        /// </summary>
        /// <param name="bankHolidayList">1-1-2025 and other holidays</param>
        /// <param name="businessDays">2</param>
        /// <returns></returns>
        public DateTime GetDateAfterBusinessDays(List<DateTime> bankHolidayList, DateTime currentDate, int businessDays = 2)
        {
            if (currentDate == null)
                currentDate = DateTime.Now.Date;  // Start from the current date (ignore time)
            int daysAdded = 0;

            // Loop until we have added the required number of business days
            while (daysAdded < businessDays)
            {
                currentDate = currentDate.AddDays(1);  // Move to the next day

                // Check if the current day is not a holiday
                if (!bankHolidayList.Contains(currentDate) && !(currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday))
                {
                    daysAdded++;  // Increment the business day count
                }
            }

            // Return the next day after we've counted the business days
            return currentDate;
        }

        /// <summary>
        /// To get the eligible Fridays for Autopay Biweekly payment setup
        /// </summary>
        /// <param name="currentDate">February 7, 2025</param>
        /// <param name="bankHolidays">February 17, 2025</param>
        /// <returns>List<string> February 7, 2025,February 13, 2025.....</returns>
        public static List<string> GetEligibleFridays(DateTime currentDate, List<DateTime> bankHolidays, string loanStatus = Constants.LoanStatus.Ontime)
        {
            var targetMonth = currentDate.Month;
            var targetYear = currentDate.Year;
            var fridays = Enumerable.Range(1, 31)
                .Select(i => new DateTime(targetYear, targetMonth, 1).AddDays(i - 1))
                .Where(d => d.DayOfWeek == DayOfWeek.Friday && d.Day != 1)
                .Take(2)
                .ToList();
            var nextMonthFridays = Enumerable.Range(1, 31)
                .Select(i => new DateTime((currentDate.Month == 12) ? currentDate.AddYears(1).Year : targetYear, currentDate.AddMonths(1).Month, 1).AddDays(i - 1))
                .Where(d => d.DayOfWeek == DayOfWeek.Friday && d.Day != 1)
                .Take(2)
                .ToList();
            fridays = fridays.Where(f => !bankHolidays.Contains(f)).ToList();
            if (fridays.Count == 0)
            {
                return null;
            }

            var firstWednesday = GetNthWeekdayOfMonth(currentDate, DayOfWeek.Wednesday, 1);
            var secondWednesday = GetNthWeekdayOfMonth(currentDate, DayOfWeek.Wednesday, 2);
            var pstTimeZone = TimeZoneInfo.FindSystemTimeZoneById(Constants.TimeZones.PacificStandardTime);

            var cutoffTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, 18, 0, 0);
            var firstWednesdayCutoff = firstWednesday.Date.AddHours(18);
            var secondWednesdayCutoff = secondWednesday.Date.AddHours(18);
            var thirdWednesdayCutoff = nextMonthFridays[0].AddDays(-2).Date.AddHours(18);
            if (currentDate < firstWednesdayCutoff)
            {
                if (loanStatus.Equals(Constants.LoanStatus.PrepaidOneMonth))
                {
                    return nextMonthFridays.Count == 2 ? nextMonthFridays.Select(f => f.ToString("MMMM d, yyyy")).ToList() : null;
                }
                else
                    return fridays.Count == 2 ? fridays.Select(f => f.ToString("MMMM d, yyyy")).ToList() : null;
            }
            else if (currentDate < secondWednesdayCutoff)
            {
                if (loanStatus.Equals(Constants.LoanStatus.PrepaidOneMonth))
                {
                    return nextMonthFridays.Count == 2 ? nextMonthFridays.Select(f => f.ToString("MMMM d, yyyy")).ToList() : null;
                }
                else if (loanStatus.Equals(Constants.LoanStatus.PrepaidOneMonth))
                    return nextMonthFridays.Count == 2 ? nextMonthFridays.Select(f => f.ToString("MMMM d, yyyy")).ToList() : null;
                else
                    return fridays.Count >= 2 ? new List<string> { fridays[1].ToString("MMMM d, yyyy") } : null;
            }
            else
            {
                if (loanStatus.Equals(Constants.LoanStatus.PrepaidOneMonth))
                    return nextMonthFridays.Count == 2 ? nextMonthFridays.Select(f => f.ToString("MMMM d, yyyy")).ToList() : null;
                else
                    return null;
            }
        }

        /// <summary>
        /// To get Nth Weekday of the month
        /// </summary>
        /// <param name="date">1/24/2025 10:42:15 AM</param>
        /// <param name="dayOfWeek">Wednesday</param>
        /// <param name="nth">5</param>
        /// <returns></returns>
        public static DateTime GetNthWeekdayOfMonth(DateTime date, DayOfWeek dayOfWeek, int nth)
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var firstDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            var targetDayOfWeek = (int)dayOfWeek;
            var daysToAdd = (targetDayOfWeek - firstDayOfWeek + 7) % 7;

            var firstTargetDay = firstDayOfMonth.AddDays(daysToAdd);
            return firstTargetDay.AddDays(7 * (nth - 1)); // nth occurrence
        }

        /// <summary>
        /// To get the expected Eligible dates for the FM / Heloc Autopay
        /// </summary>
        /// <param name="loanStatus">Ontime / PastDue / PrepaidOneMonth</param>
        /// <param name="autopayFrequency">Monthly / Biweekly</param>
        /// <returns></returns>
        public List<string> GetExpectedAvailableDates(string loanStatus, string autopayFrequency = Constants.AutopayPaymentFrequency.Monthly)
        {
            List<string> exptDates = new List<string>();

            List<DateTime> bankHolidayList = GetBankHolidays();

            bool isEligible = false;
            try
            {
                bool isBeforeCutOff = true;
                DateTime currentDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById(Constants.TimeZones.PacificStandardTime));
                int currentDay = currentDateTime.Day;
                DateTime cutOffTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, currentDay, 18, 0, 0);
                int lastDay = DateTime.DaysInMonth(currentDateTime.Year, currentDateTime.Month);
                int secondLastDay = lastDay - 1;

                if (exptDates != null)
                    exptDates.Clear();

                DateTime statusUpdateCutOff = new DateTime(currentDateTime.Year, currentDateTime.Month, 2, 0, 0, 0);

                switch (loanStatus)
                {
                    case Constants.LoanStatus.PastDue:
                        if (autopayFrequency.Equals(Constants.AutopayPaymentFrequency.Biweekly))
                        {
                            isEligible = false;
                            test.Log(Status.Info, "<b>No Dates Eligible for Autopay Biweekly Payment Setup for : " + loanStatus + "</b>");
                            break;
                        }
                        if (currentDay == 1 && currentDateTime < cutOffTime)
                        {
                            if (loanStatus.Equals(Constants.LoanStatus.PastDue))
                            {
                                test.Log(Status.Info, $"<font color='red'><b>No Eligible Dates Due to ETL Job and DB Sync Issue Current Time: {currentDateTime}</b></font>");
                                isEligible = false;
                            }
                        }
                        else if (currentDay == 1 && currentDateTime > cutOffTime)
                        {
                            if (loanStatus.Equals(Constants.LoanStatus.PastDue))
                            {
                                test.Log(Status.Info, $"<font color='red'><b>No Eligible Dates Due to ETL Job and DB Sync Issue Current Time: {currentDateTime}</b></font>");
                                isEligible = false;
                            }
                        }
                        else if (currentDay == 13)
                        {
                            if (currentDateTime > cutOffTime)
                            {
                                if (loanStatus.Equals(Constants.LoanStatus.PastDue))
                                {
                                    test.Log(Status.Info, $"<font color='red'><b>{loanStatus} is not Eligible for Autopay Setup as Today is 13th day of Month and we are After CutOff Time: {currentDateTime}</b></font>");
                                    isEligible = false;
                                }
                            }
                            else if (currentDateTime < cutOffTime)
                            {
                                isEligible = true;
                                isBeforeCutOff = true;
                                if (bankHolidayList.Contains(new DateTime(currentDateTime.Year, currentDateTime.Month, currentDay, 0, 0, 0)) || currentDateTime.DayOfWeek == DayOfWeek.Saturday || currentDateTime.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    isEligible = false;
                                    test.Log(Status.Info, $"<font color='red'><b> {loanStatus}  is not Eligible for Autopay Setup as Today's date falls on Either Weekend or Holiday : {currentDateTime} [{currentDateTime.DayOfWeek}] </b></font>");
                                }
                                else
                                {
                                    isEligible = true;
                                    exptDates = GetDatesInRange(new DateTime(currentDateTime.Year, currentDateTime.Month, 15), new DateTime(currentDateTime.Year, currentDateTime.Month, 15));
                                }

                            }
                        }
                        else if (currentDay > 13)
                        {
                            test.Log(Status.Info, $"<font color='red'><b> {loanStatus}  is not Eligible for Autopay Setup as Today's date is Greater than 13th day of Month:  {currentDateTime} </b></font>");
                            isEligible = false;
                        }
                        else
                        {
                            isBeforeCutOff = (currentDateTime < cutOffTime) ? true : false;
                            DateTime dateAfterTwoBusinessDays = (!isBeforeCutOff || bankHolidayList.Contains(new DateTime(currentDateTime.Year, currentDateTime.Month, currentDay, 0, 0, 0)) || (currentDateTime.DayOfWeek == DayOfWeek.Saturday || currentDateTime.DayOfWeek == DayOfWeek.Sunday)) ? GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 3) : GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 2);
                            exptDates = null;
                            exptDates = GetDatesInRange(dateAfterTwoBusinessDays, new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddDays(14).AddDays(1).AddTicks(-1));
                            if (exptDates == null || exptDates.Count == 0)
                            {
                                isEligible = false;
                                test.Log(Status.Info, $"<font color='red'><b> {loanStatus}  is not Eligible for Autopay Setup as Today's date : {currentDateTime} [{currentDateTime.DayOfWeek}] </b></font>");
                            }
                            else
                                isEligible = true;
                        }
                        break;

                    case Constants.LoanStatus.Ontime:
                        if (autopayFrequency.Equals(Constants.AutopayPaymentFrequency.Biweekly))
                        {
                            if (currentDay == 1)
                            {
                                test.Log(Status.Info, $"<font color='red'><b>No Eligible Dates Due to ETL Job and DB Sync Issue Current Time: {currentDateTime}</b></font>");
                                break;
                            }
                            List<string> eligibleFridays = GetEligibleFridays(currentDateTime, bankHolidayList, loanStatus);
                            if (eligibleFridays != null && eligibleFridays.Count > 0)
                            {
                                isEligible = true;
                                exptDates.AddRange(eligibleFridays);
                            }
                            else
                            {
                                isEligible = false;
                                test.Log(Status.Info, $"<font color='red'><b>No Eligible Dates Available for Autopay Biweekly Payment Setup on {currentDateTime} day {currentDateTime.DayOfWeek} for : " + loanStatus + "</b></font>");
                            }
                            break;
                        }

                        else
                        {
                            if (currentDateTime < cutOffTime)
                            {
                                if (currentDay == 1)
                                {

                                    DateTime dateAfterTwoBusinessDays = (bankHolidayList.Contains(new DateTime(currentDateTime.Year, currentDateTime.Month, currentDay, 0, 0, 0)) || currentDateTime.DayOfWeek == DayOfWeek.Saturday || currentDateTime.DayOfWeek == DayOfWeek.Sunday) ? GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 3) : GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 2);
                                    exptDates = GetDatesInRange(dateAfterTwoBusinessDays, new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddDays(15).AddTicks(-1));
                                    isEligible = true;
                                }

                                else if (currentDay == lastDay)
                                {
                                    DateTime dateAfterTwoBusinessDays = (bankHolidayList.Contains(new DateTime(currentDateTime.Year, currentDateTime.Month, currentDay, 0, 0, 0)) || currentDateTime.DayOfWeek == DayOfWeek.Saturday || currentDateTime.DayOfWeek == DayOfWeek.Sunday) ? GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 3) : GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 2);


                                    exptDates = GetDatesInRange(dateAfterTwoBusinessDays, new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(1).AddDays(15).AddTicks(-1));
                                    isEligible = true;

                                }
                                else
                                {
                                    isEligible = true; // Eligible - Ontime 1-15 next Month
                                    exptDates = GetDatesInRange(new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(1), new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(1).AddDays(14));
                                }
                            }
                            else if (currentDateTime > cutOffTime)
                            {
                                if (currentDay == secondLastDay)
                                {
                                    DateTime dateAfterTwoBusinessDays = GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 3);


                                    exptDates = GetDatesInRange(dateAfterTwoBusinessDays, new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(1).AddDays(15).AddTicks(-1));
                                    isEligible = true;
                                }
                                else if (currentDay == lastDay)
                                {
                                    DateTime dateAfterTwoBusinessDays = GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 3);


                                    exptDates = GetDatesInRange(dateAfterTwoBusinessDays, new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddDays(15).AddTicks(-1));
                                    isEligible = true;
                                }
                                else if (currentDay == 1)
                                {
                                    DateTime dateAfterTwoBusinessDays = GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 3);
                                    exptDates = GetDatesInRange(dateAfterTwoBusinessDays, new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddDays(15).AddTicks(-1));
                                    isEligible = true;
                                }
                                else
                                {
                                    isEligible = true; // Eligible - Ontime 1-15 next Month
                                    exptDates = GetDatesInRange(new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(1), new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(1).AddDays(14));
                                }
                            }
                        }
                        break;

                    case Constants.LoanStatus.PrepaidOneMonth:
                        if (autopayFrequency.Equals(Constants.AutopayPaymentFrequency.Biweekly))
                        {
                            if (currentDay == 1)
                            {
                                test.Log(Status.Info, $"<font color='red'><b>No Eligible Dates Due to ETL Job and DB Sync Issue Current Time: {currentDateTime}</b></font>");
                                break;
                            }
                            List<string> eligibleFridays = GetEligibleFridays(currentDateTime, bankHolidayList, Constants.LoanStatus.PrepaidOneMonth);

                            if (eligibleFridays != null && eligibleFridays.Count > 0)
                            {
                                isEligible = true;
                                exptDates.AddRange(eligibleFridays);

                            }
                            else
                            {
                                isEligible = false;
                                test.Log(Status.Info, $"<font color='red'><b>No Eligible Dates Available for Autopay Biweekly Payment Setup on {currentDateTime} day {currentDateTime.DayOfWeek} for : " + loanStatus + "</b></font>");
                            }
                            break;
                        }
                        else
                        {
                            isEligible = true;
                            exptDates = GetDatesInRange(new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(2), new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(2).AddDays(14));


                            if (currentDateTime < cutOffTime)
                            {
                                if (currentDay == 1)
                                {
                                    DateTime dateAfterTwoBusinessDays = (bankHolidayList.Contains(new DateTime(currentDateTime.Year, currentDateTime.Month, currentDay, 0, 0, 0)) || currentDateTime.DayOfWeek == DayOfWeek.Saturday || currentDateTime.DayOfWeek == DayOfWeek.Sunday) ? GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 3) : GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 2);
                                    exptDates = GetDatesInRange(new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(1), new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(1).AddDays(15).AddTicks(-1));
                                    isEligible = true;
                                }

                                else if (currentDay == lastDay)
                                {
                                    isBeforeCutOff = (currentDateTime < cutOffTime) ? true : false;
                                    DateTime dateAfterTwoBusinessDays = (!isBeforeCutOff || bankHolidayList.Contains(new DateTime(currentDateTime.Year, currentDateTime.Month, currentDay, 0, 0, 0)) || currentDateTime.DayOfWeek == DayOfWeek.Saturday || currentDateTime.DayOfWeek == DayOfWeek.Sunday) ? GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 3) : GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 2);

                                    //Eligible - Ontime - 2-15 for next month
                                    exptDates = GetDatesInRange(dateAfterTwoBusinessDays, new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(1).AddDays(15).AddTicks(-1));
                                    exptDates.AddRange(GetDatesInRange(new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(2), new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(2).AddDays(15).AddTicks(-1)));
                                    isEligible = true;

                                }
                                else
                                {
                                    isEligible = true; // Eligible - Ontime 1-15 next Month
                                }
                            }
                            else if (currentDateTime > cutOffTime)
                            {
                                if (currentDay == secondLastDay)
                                {
                                    isEligible = true;
                                }
                                else if (currentDay == lastDay)
                                {
                                    isEligible = true;
                                }
                                else if (currentDay == 1)
                                {
                                    DateTime dateAfterTwoBusinessDays = GetDateAfterBusinessDays(bankHolidayList, currentDateTime, 3); ;
                                    exptDates = GetDatesInRange(new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(1), new DateTime(currentDateTime.Year, currentDateTime.Month, 1).AddMonths(1).AddDays(15).AddTicks(-1));
                                    isEligible = true;
                                }
                                else
                                {
                                    isEligible = true; // Eligible - Ontime 1-15 next Month                                    
                                }
                            }
                            break;
                        }
                    case Constants.LoanStatus.PrepaidTwoMonth:
                        isEligible = false;
                        test.Log(Status.Info, $"<font color='red'><b>No Eligible Dates Available for Autopay Biweekly Payment Setup on {currentDateTime} day {currentDateTime.DayOfWeek} for : " + loanStatus + "</b></font>");
                        break;

                    default:
                        test.Log(Status.Error, $"{loanStatus} is not in the required loan status");
                        break;
                }

                // Log expected dates for each hour if eligible
                if (isEligible && exptDates != null && exptDates.Count > 0)
                {
                    test.Log(Status.Info, $"<b>Expected Eligible Dates for {loanStatus} for {autopayFrequency} on {currentDateTime} [{currentDateTime.DayOfWeek}] [PST Time zones] :\n " + string.Join(", ", exptDates) + "</b>");
                }
            }
            catch (Exception ex)
            {
                test.Log(Status.Error, "Failed while Getting actual Available dates: " + ex.Message);
            }
            return ((isEligible) ? exptDates : null);
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

        ///<summary>
        ///Method to Gets Heloc Loan Info
        ///</summary>
        /// <param name="loanLevelData">Loan Level Data from DB</param>
        public APIConstants.HelocLoanInfo GetHelocLoanInfo(Hashtable loanLevelData)
        {
            APIConstants.HelocLoanInfo helocLoanInfo = new APIConstants.HelocLoanInfo();
            try
            {
                string servicingLINCPortalURI = APIConstants.AgentPortalURIs.ServicingLINCPortalURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                RestManager restManager;
                restManager = new RestManager(servicingLINCPortalURI, test);
                JObject json = JObject.Parse(restManager.GetMethod(APIConstants.AgentPortalResourcePaths.BorrowerIVRDetailsPath, loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString()));
                string secretKey = json["secretKey"].ToString();
                string helocURI = APIConstants.AgentPortalURIs.HelocURI.Replace("ENVIRONMENT", ConfigSettings.Environment.ToLower());
                Dictionary<string, string> helocHeaders = new Dictionary<string, string>();
                helocHeaders.Add(APIConstants.AgentPortalHeadersName.loanNumber, loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString());
                helocHeaders.Add(APIConstants.AgentPortalHeadersName.encryptionValue, secretKey);
                helocHeaders.Add(APIConstants.AgentPortalHeadersName.ChannelId, APIConstants.AgentPortalHeadersValue.ChannelId);
                helocHeaders.Add(APIConstants.AgentPortalHeadersName.LoanConsumerType, APIConstants.AgentPortalHeadersValue.LoanConsumerType);
                restManager = new RestManager(helocURI, test);
                json = JObject.Parse(restManager.GetMethod(APIConstants.AgentPortalResourcePaths.HelocLoanInfoPath, loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString() + "/" + secretKey, false, "Basic", helocHeaders));
                helocLoanInfo = json.ToObject<APIConstants.HelocLoanInfo>();
                return helocLoanInfo;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Heloc Loan Info: " + ex.Message);
                return helocLoanInfo;
            }
        }

        ///<summary>
        ///Method to Gets Heloc Fees
        ///</summary>
        /// <param name="fees">Fees List from API</param>
        public Dictionary<string, string> GetHelocFees(List<Fees> fees)
        {
            Dictionary<string, string> feeDict = new Dictionary<string, string>();
            try
            {

                if (fees.Count > 0)
                {
                    foreach (APIConstants.Fees fee in fees)
                    {
                        if (fee.Description == "LATE CHARGE FEE")
                        {
                            feeDict.Add(Constants.FeeType.LateCharges, fee.Amount.ToString());
                        }
                        else if (fee.Description == "RETURN ITEM FEE")
                        {
                            feeDict.Add(Constants.FeeType.NSFFees, fee.Amount.ToString());
                        }
                        else if (fee.Description == "ANNUAL MAINT FEE")
                        {
                            feeDict.Add(Constants.FeeType.AnnualMaintenanceFees, fee.Amount.ToString());
                        }
                        else if (fee.Type == 3)
                        {
                            feeDict.Add(Constants.FeeType.OtherFees, fee.Amount.ToString());
                        }
                    }
                }

                return feeDict;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting Heloc Fees: " + ex.Message);
                return feeDict;
            }
        }

        /// <summary>
        /// Delete all added bank account except one in use
        /// </summary>
        /// <param name="isReportRequired">true/false</param>
        /// <param name="isScrollIntoViewRequired">true/false</param>
        public void DeleteAllAddedBankAccounts(bool isReportRequired = false, bool isScrollIntoViewRequired = false)
        {
            //Deleting all the Added Bank Accounts
            webElementExtensions.ClickElementUsingJavascript(_driver, manageBankAccountsLinkEnabledLocBy);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, manageBankAccountsLinkEnabledLocBy);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
            webElementExtensions.WaitForElement(_driver, backToPaymentsButtonLocBy);
            var deleteIconElements = _driver.FindElements(deleteIconManageBankAccountLocBy);
            reportLogger.TakeScreenshot(test, "Before Attempt to delete all the added Bank Accounts");
            foreach (var deleteIcon in deleteIconElements)
            {
                webElementExtensions.WaitForElementToBeEnabled(_driver, deleteIcon, isScrollIntoViewRequired: isScrollIntoViewRequired);
                webElementExtensions.ClickElementUsingJavascript(_driver, deleteIcon);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                webElementExtensions.WaitForElementToBeEnabled(_driver, deleteButtonOnDeleteBankAccountPopupLocBy, isScrollIntoViewRequired: isScrollIntoViewRequired, timeoutInSeconds: ConfigSettings.WaitTime);
                webElementExtensions.ClickElementUsingJavascript(_driver, deleteButtonOnDeleteBankAccountPopupLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
                if (webElementExtensions.WaitForElementToBeEnabled(_driver, closeButtonOnBankAccountCannotBeDeletedPopupLocBy, timeoutInSeconds: 2))
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, closeButtonOnBankAccountCannotBeDeletedPopupLocBy);
                }
            }
            reportLogger.TakeScreenshot(test, "After the attempt to deleting all the added Bank Accounts");
            webElementExtensions.ClickElementUsingJavascript(_driver, backToPaymentsButtonLocBy, "Back to Make Payments");
            webElementExtensions.WaitForInvisibilityOfElement(_driver, loadingIconLocBy);
        }

        #endregion Services
    }
}
