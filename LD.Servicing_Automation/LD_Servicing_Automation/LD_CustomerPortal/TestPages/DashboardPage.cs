using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using LD_CustomerPortal.TestPages;
using log4net;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LD_CustomerPortal.Pages
{
    /// <summary>
    /// Dashboard Page
    /// </summary>
    public class DashboardPage
    {
        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        WebElementExtensionsPage webElementExtensions = null;

        CommonServicesPage commonServices = null;

        PaymentsPage payments = null;

        FAQsPage fAQsPage = null;

        DBconnect dBconnect = null;

        ReportLogger reportLogger { get; set; }

        public DashboardPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            payments = new PaymentsPage(_driver, test);
            fAQsPage = new FAQsPage(_driver, test);
            reportLogger = new ReportLogger(_driver);
            dBconnect = new DBconnect(test, Constants.DBNames.MelloServETL);
        }

        #region Locators

        public By enterLoanNumberInputFieldLocBy = By.Id("mat-input-0");

        public By verifyCallerPopUpCloseIconLocBy = By.Id("btnIdClose");

        public By accountNotFoundMessageLocBy = By.XPath("//p[contains(text(),'Account Not Found')]");

        public By contactInfoPopup_UpdateLaterButtonLocBy = By.Id("btn-update-later");

        public By closeIconOnHelocEligiblePopupLocBy = By.CssSelector("[alt='closeIcon']");

        public By noThanksButtonLocBy = By.Id("no-thanks-link");

        public By chatBotCloseIconLocBy = By.CssSelector("button[aria-label='Minimize ServisBOT Messenger']");

        public By paymentDatePickerIconLocBy = By.CssSelector("button[aria-label='Open calendar']");

        public By paymentDateTextboxWithDateSelectedLocBy = By.Id("paymtDateId");

        public string serviceBotMessangerLocString = "//iframe[@id='servisbot-messenger-iframe' and contains(@style,'visibility: {0}')]";

        public By writeMessageTextInputFieldOnServiceChatBotLocBy = By.CssSelector("[data-testid='sb-textarea-default']");

        public By sendMessageButtonServiceChatBotLocBy = By.CssSelector("button[id='sb-textarea-submit-button']");

        public By sbActivityPendingLocBy = By.CssSelector("[class='sb-timeline-activity-indicator-paper sb-timeline-paper-wrapper']");

        public string serviceBotResponseString = "//article[text()='{0}']/ancestor::div[@class='sb-bubble-wrapper ltr sb-bubble-wrapper-out']/following-sibling::div//article[@aria-label='Chat message from bot']";

        public By loanDepotLogoLocBy = By.CssSelector("img[alt='loanDepot']");

        public By paymentActivityHeaderLocBy = By.Id("pIdPaymentActivity");

        public By manageAutopayButtonLocBy = By.Id("linkManageAutopay");

        public By manageAutopayOnOffButtonLocBy = By.CssSelector("span img[alt$=' Autopay']");

        public By updateLaterButtonLocBy = By.XPath("//button[contains(text(),'Update Later')]");

        public By paymentIneligibleMessageLocBy = By.Id("loanNotEligibleMsg");

        public By continueButtonLocBy = By.XPath("//button//span[text()='Continue']");

        public By scheduledPaymentIsProcessingPopupHeaderLocBy = By.XPath("//div[text()='Scheduled Payment is Processing']");

        public By makeAPaymentButtonLocBy = By.CssSelector("button[class*='payment']");

        public By confirmationNumbersInPendinPaymentSectionLocBy = By.CssSelector("span[id='confirmationNumber_']");

        public By cancelButtonLinkMyLoanPopupLocBy = By.Id("btnIdCloseModal");

        public By pendingPaymentDatesTextLocBy = By.CssSelector("span[id='pendingPaymentDate']");

        public By autopayPendingPaymentDatesTextLocBy = By.CssSelector("span[id='sppaymentDate_']");

        public By accountStandingStatusLocBy = By.CssSelector("[id^='spIdStatus'] > h6");

        public By paymentFormPageLocBy = By.Id("paymentForm");

        public By selectedLoanLocBy = By.XPath("//*[@id='navbarNavDropdown']//*[contains(text(),' | Account -')]");

        public By remindMeLaterPaperLessPageLinkLocBy = By.Id("remind-me-later-link");

        public By loanStatusTextLocBy = By.CssSelector("[id*='spIdStatus'] h6");

        public By messagesLinkLocBy = By.XPath("//span[text()='Messages']");

        public By statusOfActiveForbearanceLocBy = By.XPath("//*[contains(text(),'Active Forbearance')]");

        public By statusOfActiveRepayLocBy = By.XPath("//*[contains(text(),'Active Repayment Plan')]");

        public By statusOfModificationTrail = By.XPath("//*[contains(text(),'Modification Trial Plan')]");

        public By principalAndInterestAmountValueTextLocBy = By.Id("divIdPaymentInterest");

        public By taxAndInsuranceAmountValueTextLocBy = By.Id("divIdPaymentEscrow");

        public By feesAmountValueTextLocBy = By.Id("divIdPaymentFees");

        public By subsidyTextLocBy = By.Id("divIdbuyDownSubsidy");

        public By totalAmountDueValueTextLocBy = By.CssSelector("[id='divIdAmountDue']");

        public By currentAmountDueInHeaderValueTextLocBy = By.CssSelector("[id^='hIdCurrentPaymentDue']");

        public By btnMakePaymentLocBy = By.Id("btnIdMakePayment");

        //button for heloc make a payment 
        public By btnHelocMakeApaymentLocBy = By.XPath("//button[contains(.,'Make a Payment')]");

        public By verifyYourIdentityPopupLocBy = By.CssSelector("div[class='cdk-overlay-pane mfa-popup']");


        public By receiveCodeViaEmailButtonLocBy = By.XPath("//ldsm-mfa//span[text()='Receive Code Via Email']");

        public By verificationOTPInputFieldLocBy = By.Id("otp");

        public By verifyCodeButtonLocBy = By.Id("VerifyCodeBtn");

        public By verifiedMFAPopUpLocBy = By.CssSelector("mello-serve-ui-mfa-alert-message");

        public By statementsLinkLocBy = By.XPath("//a[contains(text(),'Statements & Documents')]");

        public By settingsBtnLocBy = By.XPath("//button[contains(text(),'Settings')]");

        public By helpAndSupportLocBy = By.XPath("//button[contains(text(),'Help and Support')]");

        public By faqAnchorLinkLocBy = By.XPath("//a[@href='/faqs' and text()='Frequently Asked Questions']");

        public By tutorialLinkLocBy = By.XPath("//a[contains(text(),'Tutorial')]");

        public By managePaperlessLocBy = By.XPath("//a[contains(text(),'Manage Paperless')]");

        public By payoffQuoteLocBy = By.Id("btnPayoffQuote");

        public string currentBalanceValueTextBy = "//h6[text()='Current Balance']<TEXT>";

        public By escrowBalanceValueTextLocBy = By.XPath("//p[text()='Escrow Balance']/following-sibling::p[contains(@class,'text-grey')]");

        public By nextAmountDueValueTextLocBy = By.CssSelector("h4[id^='hIdCurrentPaymentDue']");

        public By accountStandingValueTextLocBy = By.XPath("//*[contains(@id,'spIdStatus')]//*[self::h4 or self::h6]");

        public By accountStandingForHelocLoansTextLocBy = By.XPath("//div[text()='Account Standing']/following-sibling::div/span");

        public By currentPaymentDueDateValueTextLocBy = By.CssSelector("div[id='lastPaidOff'] p");

        public string availableCreditLineValueTextBy = "//h6[text()='Available Credit Line']<TEXT>";

        public By availableCreditLineProgressBarValueLocBy = By.CssSelector("[class^='mat-progress-bar-primary']");

        public By paymentInfoIconLocBy = By.CssSelector("[id='paymentInfo']");

        public By paymentInfoExplanationPopupTextsLocBy = By.CssSelector("mhp-heloc-payment-explanation");

        public By nextPaymentDueDateForHelocLoansTextLocBy = By.XPath("//mhp-heloc-make-a-payment//div[contains(text(),'Due on ')]");

        public By prodsSectionLocBy = By.XPath("//div[contains(text(),'New Products & Services')]");

        public string productsPath = "//span[contains(text(),'{0}')]";

        public By learnMoreBtnLocBy = By.XPath("//*[contains(@id,'mat-dialog')]//button[contains(.,'Learn More')]");

        public By cancelBtnLocBy = By.XPath("//*[contains(@id,'mat-dialog')]//button[contains(.,'Cancel')]");

        public By saveBtnLocBy = By.XPath("//*[contains(@id,'mat-dialog')]//button[contains(.,'Save')]");

        public By continueBtnLocBy = By.XPath("//*[contains(@id,'mat-dialog')]//button[contains(.,'Continue')]");

        public By dialogLocBy = By.XPath("//*[contains(@id,'mat-dialog')]");

        public By dialogCloseBtnLocBy = By.XPath("//*[contains(@id,'mat-dialog')]//button");

        string loanDropDownItemPath = "//*[contains(text(),'ACCOUNT - {0}')]//parent::div";

        public By helpSupportLinkLocBy = By.XPath("//a[text()='Help and Support']");

        public By naihcSecLocBy = By.XPath("//*[@id='templateIdNAIHC']//p");

        public By naihcLinkLocBy = By.XPath("//*[@id='templateIdNAIHC']//a");

        public By paperlessDescriptionLocBy = By.Id("pIdPaperlessDetails");

        public By managePaperlessLinkLocBy = By.XPath("//*[contains(text(),'Enable Paperless')]");

        public By multiFactorAuthenticatorCodeTooltipIconLocBy = By.CssSelector("img[class^='mat-tooltip-trigger ']");

        public By multiFactorAuthenticatorCodeTooltipTextContentLocBy = By.CssSelector("mat-tooltip-component");

        public By profileAvatarIconLocBy = By.CssSelector("img[alt='avatar']");

        public By editButtonOnContactInformationLocBy = By.CssSelector("span[id='ciIdbtnEdit']");

        public By homePhoneInputFieldOnContactInfoPageLocBy = By.CssSelector("input[formcontrolname='telecomNumber']");

        public By cityInputFieldOnCantactInfoPageLocBy = By.CssSelector("input[formcontrolname='cityName']");

        public By saveChangesButtonOnContactInfoPageLocBy = By.CssSelector("span[id='ciIdSaveChanges']");

        public By faqLinkOnVerifyYourIdentityPopupLocBy = By.CssSelector("div[class='cdk-overlay-pane mfa-popup'] a[href='/faqs']");

        public By faqHeaderTextOnFaqPageLocBy = By.XPath("//h3[text()='Frequently Asked Questions']");

        public By licensingLinkLocBy = By.XPath("//a[contains(text(),'Licensing Information')]");

        public By widgetPrivacyLinkLocBy = By.XPath("//*[contains(text(),'Read comehome')]//a[contains(text(),'Privacy Policy')]");

        public By footerPrivacyLinkLocBy = By.XPath("//*[contains(text(),'loanDepot')]//a[contains(text(),'Privacy Policy')]");

        public By termsLinkLocBy = By.XPath("//a[contains(text(),'Terms of Use')]");

        public By sendUsASceureMessageLinkOnContactUsCardLocBy = By.XPath("//button[contains(text(),'Send us a secure message')]");

        public By loanNotEligibleMsgLocBy = By.Id("loanNotEligibleMsg");

        public By helocLoanStandingLocBy = By.XPath("//div[text()='Account Standing']/parent::div");

        public By helocNotEligibleMsgLocBy = By.XPath("//*[contains(text(),'Your HELOC account is currently ineligible for online payments')]");

        public By requestFundsButtonLocBy = By.CssSelector("mhp-heloc-draw-funds [id='matCardIdMakeAPayment'] button");

        public By requestFundsAdvanceRequestFromLineOfCreditTextContentLocBy = By.CssSelector("mhp-heloc-draw-funds [id='matCardIdMakeAPayment']");

        public By drawFundRequestedSuccessAlertLocBy = By.CssSelector("div[class*='alert-success']");

        // Login MFA        
        public By loginMFAPopUpLocBy = By.CssSelector("div[class$='mfa-login-popup']");

        public By emailDropdownSelectLoginMFAPopUpLocBy = By.CssSelector("mat-select[id = 'email']");

        public By receiveCodeViaEmailButtonLoginMFAPopUpLocBy = By.XPath("//span[text()='Receive Code Via Email']");

        public By rememberMeCheckBoxLoginMFAPopUpLocBy = By.CssSelector("mat-checkbox[formcontrolname='rememberMe']");

        public By selectEmailOptionLoginMFAPopUpLocBy = By.CssSelector("div[id='email-panel'] mat-option span[class='mat-option-text']");

        public By cancelButtonLoginMGAPopUpLocBy = By.CssSelector("[id='cancelBtn']");

        public By phoneSmsDropdownSelectLoginMFAPopUpLocBy = By.CssSelector("mat-select[id = 'cellPhoneSms']");

        public By selectPhoneSmsOptionLoginMFAPopUpLocBy = By.CssSelector("[id='cellPhoneSms-panel']");

        public By phoneCallDropdownSelectLoginMFAPopUpLocBy = By.CssSelector("[id='cellPhoneCall']");

        public By selectPhoneCallOptionLoginMFAPopUpLocBy = By.CssSelector("[id='cellPhoneCall-panel']");

        #endregion Locators

        #region Services

        /// <summary>
        /// Method to close all pop ups and after linking new loan
        /// </summary>
        /// <param name="isReportRequired">True/False</param>
        public void ClosePopUpsAfterLinkingNewLoan(bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, contactInfoPopup_UpdateLaterButtonLocBy, ConfigSettings.SmallWaitTime);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, contactInfoPopup_UpdateLaterButtonLocBy))
                {
                    reportLogger.TakeScreenshot(test, "Contact Info Update Popup");
                    flag = webElementExtensions.ActionClick(_driver, contactInfoPopup_UpdateLaterButtonLocBy, "Update Later Button on Contact Info Pop up", isReportRequired: isReportRequired);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, contactInfoPopup_UpdateLaterButtonLocBy);
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while closing the pop ups in the application: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully closed all pop ups in the application.", "Failed while closing the pop ups in the application.");
        }

        /// <summary>
        /// Method to close Heloc pop ups and after linking new loan
        /// </summary>
        /// <param name="loanNumber">LoanNumber</param>
        public void CloseHelocEligiblePopup(string loanNumber)
        {
            bool flag = false;
            string flagID = string.Empty;
            try
            {
                string queryForFlagID = UtilAdditions.GetExtractedXmlData(Constants.DBQueryFileName.LoanDetails, Constants.DBQueries.GetFlagIDForASpecificLoanNumber).Replace("LN_NUMBER", loanNumber);
                DBconnect dBconnect = new DBconnect(test, Constants.DBNames.MelloServ);
                flagID = dBconnect.ExecuteQuery(queryForFlagID).ToString();

                if (flagID.Equals("2"))
                {
                    reportLogger.TakeScreenshot(test, "Heloc Eligible Pop Up");
                    flag = webElementExtensions.ActionClick(_driver, closeIconOnHelocEligiblePopupLocBy, "Close Icon on Heloc Eligible Pop up");
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, closeIconOnHelocEligiblePopupLocBy);
                    _driver.ReportResult(test, flag, "Successfully closed the Heloc pop up which was supposed to get displayed for this loan number - " + loanNumber + ".", "Failed while handling the heloc pop up for this loan number - " + loanNumber + ".");
                }
                else
                {
                    flag = !webElementExtensions.IsElementDisplayedBasedOnCount(_driver, closeIconOnHelocEligiblePopupLocBy);
                    _driver.ReportResult(test, flag, "Successfully verified that Heloc pop up should not be displayed for this loan number - " + loanNumber + ".", "Failed while verifying that heloc pop up should not be displayed for this loan number - " + loanNumber + ".");
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while closing the Heloc pop up in the application: " + ex.Message);
            }
        }


        /// <summary>
        /// Method to handle Paper Less Page
        /// </summary>
        /// <param name="isReportRequired">flag = true / flase</param>
        public void HandlePaperLessPage(bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.WaitForStalenessOfElement(_driver, noThanksButtonLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForElement(_driver, noThanksButtonLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForStalenessOfElement(_driver, noThanksButtonLocBy, ConfigSettings.SmallWaitTime);
                flag = webElementExtensions.IsElementDisplayed(_driver, noThanksButtonLocBy, isScrollIntoViewRequired: false);
                if (flag)
                {
                    webElementExtensions.ActionClick(_driver, remindMeLaterPaperLessPageLinkLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, noThanksButtonLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                }
            }
            catch (Exception e)
            {
                log.Error($"Failed while handling Paperless page: {e.Message}");
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully closed Paperless Page", "Failed while closing the Paperless Page");
        }

        /// <summary>
        /// Method to Close the Service chatbot
        /// </summary>
        /// <param name="isReportRequired">flag = true / false</param>
        public void HandleServiceChatBot()
        {
            bool flag = false;
            try
            {
                webElementExtensions.WaitForElementToBeEnabled(_driver, By.XPath(string.Format(serviceBotMessangerLocString, "visible")));
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(string.Format(serviceBotMessangerLocString, "visible"))))
                {
                    webElementExtensions.SwitchToFrame(_driver, Constants.CustomerPortalFrameNames.ServiceBotFrame);
                    reportLogger.TakeScreenshot(test, "Service Chat Bot");
                    webElementExtensions.WaitForElement(_driver, chatBotCloseIconLocBy);
                    webElementExtensions.ActionClick(_driver, chatBotCloseIconLocBy, "Chat Bot Close Icon", isReportRequired: true);
                    flag = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(string.Format(serviceBotMessangerLocString, "hidden")));
                    webElementExtensions.SwitchToDefaultContent(_driver);
                    _driver.ReportResult(test, true, "Successfully closed service chatbot window in the application.", "");
                }
                else
                    test.Log(Status.Info, "Warning!!! - Service Chatbot did not Pop up");
            }
            catch (Exception ex)
            {
                log.Error("Failed while closing the Service Chatbot in the application: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to Verify the Chatbot Feature
        /// </summary>
        /// <param name="loanInfo">Hashtable loanInforamtion</param>
        public void VerifyAPIUtterencesInServiceChatBot(Hashtable loanInfo)
        {
            bool flag = false;
            try
            {
                webElementExtensions.WaitForElementToBeEnabled(_driver, By.XPath(string.Format(serviceBotMessangerLocString, "visible")));
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, By.XPath(string.Format(serviceBotMessangerLocString, "visible"))))
                {
                    webElementExtensions.SwitchToFrame(_driver, Constants.CustomerPortalFrameNames.ServiceBotFrame);
                    reportLogger.TakeScreenshot(test, "Service Chat Bot");

                    webElementExtensions.EnterText(_driver, writeMessageTextInputFieldOnServiceChatBotLocBy, "When is my next payment due?", false, "Write Message Input Field on Chat Bot", true);
                    webElementExtensions.ActionClick(_driver, sendMessageButtonServiceChatBotLocBy, "Send message Button On Service Chat Bot", isReportRequired: true);
                    webElementExtensions.WaitForStalenessOfElement(_driver, sendMessageButtonServiceChatBotLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, sbActivityPendingLocBy, ConfigSettings.LongWaitTime);
                    By responseLocBy = By.XPath(string.Format(serviceBotResponseString, "When is my next payment due?"));
                    string actualResponse = webElementExtensions.GetElementText(_driver, responseLocBy, true);
                    decimal totalPayment = Convert.ToDecimal(loanInfo[Constants.LoanLevelDataColumns.TotalMonthlyPayment]);
                    decimal accruedLateChargeAmount = Convert.ToDecimal(loanInfo[Constants.LoanLevelDataColumns.AccruedLateChargeAmount]);
                    decimal nsfFeeAmount = Convert.ToDecimal(loanInfo[Constants.LoanLevelDataColumns.NsfFeeBalance]);
                    decimal otherFeeAmount = Convert.ToDecimal(loanInfo[Constants.LoanLevelDataColumns.OtherFees]);
                    var expectedValue = "$" + (totalPayment + accruedLateChargeAmount + nsfFeeAmount + otherFeeAmount).ToString("N");
                    ReportingMethods.LogAssertionTrue(test, actualResponse.Contains(expectedValue), $"Verify for {"When is my next payment due?"} response contains: {expectedValue}");
                    expectedValue = DateTime.Parse(loanInfo[Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("M/d/yyyy");
                    ReportingMethods.LogAssertionTrue(test, actualResponse.Contains(expectedValue), $"Verify for {"When is my next payment due?"} response contains: {expectedValue}");
                    reportLogger.TakeScreenshot(test, "Service Chat Bot with: When is my next payment due? Response");
                    webElementExtensions.EnterText(_driver, writeMessageTextInputFieldOnServiceChatBotLocBy, "how much escrow funds", false, "Write Message Input Field on Chat Bot", true);
                    webElementExtensions.ActionClick(_driver, sendMessageButtonServiceChatBotLocBy, "Send message Button On Service Chat Bot", isReportRequired: true);
                    webElementExtensions.WaitForStalenessOfElement(_driver, sendMessageButtonServiceChatBotLocBy, ConfigSettings.SmallWaitTime);
                    responseLocBy = By.XPath(string.Format(serviceBotResponseString, "how much escrow funds"));
                    actualResponse = webElementExtensions.GetElementText(_driver, responseLocBy, true);
                    double calculatedValue = Convert.ToDouble(loanInfo[Constants.LoanLevelDataColumns.EscrowBalance])
                       - Convert.ToDouble(loanInfo[Constants.LoanLevelDataColumns.EscrowAdvanceBalance]);

                    string expectedEscrowBalance = (calculatedValue < 0)
                        ? $"-${Math.Abs(calculatedValue).ToString("N2")}"
                        : $"${calculatedValue.ToString("N2")}";
                    ReportingMethods.LogAssertionTrue(test, actualResponse.Contains(expectedEscrowBalance), $"Verify for {"how much escrow funds"} response contains: {expectedEscrowBalance}");
                    reportLogger.TakeScreenshot(test, "Service Chat Bot with: how much escrow funds Response ");

                    webElementExtensions.EnterText(_driver, writeMessageTextInputFieldOnServiceChatBotLocBy, "Account Details", false, "Write Message Input Field on Chat Bot", true);
                    webElementExtensions.ActionClick(_driver, sendMessageButtonServiceChatBotLocBy, "Send message Button On Service Chat Bot", isReportRequired: true);
                    webElementExtensions.WaitForStalenessOfElement(_driver, sendMessageButtonServiceChatBotLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, sbActivityPendingLocBy, ConfigSettings.LongWaitTime);
                    responseLocBy = By.XPath(string.Format(serviceBotResponseString, "Account Details"));
                    var elements = _driver.FindElements(responseLocBy);

                    // Combine the text of all elements into one string
                    actualResponse = string.Join(" ", elements.Select(e => e.Text));        //escrowBalance = Convert.ToDecimal(loanInfo[Constants.LoanLevelDataColumns.EscrowBalance]);
                    expectedValue = "$" + Convert.ToDecimal(loanInfo[Constants.LoanLevelDataColumns.OriginalMortgageAmount]).ToString("N");
                    ReportingMethods.LogAssertionTrue(test, actualResponse.Contains(expectedValue), $"Verify for {"Account Details"} response contains: {expectedValue}");
                    expectedValue = DateTime.Parse(loanInfo[Constants.LoanLevelDataColumns.OriginationDate].ToString()).ToString("M/d/yyyy");
                    ReportingMethods.LogAssertionTrue(test, actualResponse.Contains(expectedValue), $"Verify for {"Account Details"} response contains: {expectedValue}");
                    string raw = loanInfo[Constants.LoanLevelDataColumns.InterestRates]?.ToString();
                    expectedValue = (decimal.Parse(raw) * 100).ToString("0.###") + "%";
                    ReportingMethods.LogAssertionTrue(test, actualResponse.Contains(expectedValue), $"Verify for {"Account Details"} response contains: {expectedValue}");
                    expectedValue = DateTime.Parse(loanInfo[Constants.LoanLevelDataColumns.MaturityDate].ToString()).ToString("M/d/yyyy");
                    ReportingMethods.LogAssertionTrue(test, actualResponse.Contains(expectedValue), $"Verify for {"Account Details"} response contains: {expectedValue}");
                    reportLogger.TakeScreenshot(test, "Service Chat Bot with: Account Detail");

                    webElementExtensions.SwitchToDefaultContent(_driver);
                    _driver.ReportResult(test, true, "Successfully closed service chatbot window in the application.", "");
                }
                else
                    test.Log(Status.Info, "Warning!!! - Service Chatbot did not Pop up");
            }
            catch (Exception ex)
            {
                log.Error("Failed while closing the Service Chatbot in the application: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to verify if the loan number in application is linked to the user account or not
        /// </summary>
        /// <param name="loanNumber">Loan number</param>
        /// <param name="propertyZipCode">Zip code</param>
        /// <param name="borrowerSsn">SSN</param>
        /// <param name="isReportRequired">true/false</param>
        /// <returns>true/false</returns>
        public bool VerifyIfLinkingOfLoanNumberIsSuccessful(string loanNumber, bool checkManageAutopayToggleOff = true, bool isReportRequired = false, bool checkManageAutopayPage = true, bool checkMakeAPaymentButtonEnabled = false, bool checkMakeAOneTimePaymentPage = false)
        {
            bool flag = false;
            try
            {
                test.Log(Status.Info, $"<b><u>Started checking if the linked loan is usefull for the execution or not</u></b>");
                if (checkManageAutopayToggleOff)
                {
                    if (webElementExtensions.VerifyElementAttributeValue(_driver, manageAutopayOnOffButtonLocBy, "alt", "Off Autopay")) //"Off Autopay" "On Autopay")
                    {
                        flag = true;
                        test.Log(Status.Info, "Manage Autopay toggle is <b>turnned OFF</b>");
                        reportLogger.TakeScreenshot(test, "Manage Autopay toggle is <b>turnned OFF</b>");
                        //webElementExtensions.ScrollToTop(_driver);

                        if (checkMakeAPaymentButtonEnabled)
                        {
                            By btnLocBy = By.XPath(commonServices.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment));
                            flag = webElementExtensions.IsElementEnabled(_driver, (loanNumber.StartsWith("9")) ? btnLocBy : btnMakePaymentLocBy, isScrollIntoViewRequired: false);
                            test.Log(Status.Info, (flag) ? "Make a Payment Button is enabled" : "Make a Payment Button is Disabled");
                            if (flag)
                            {
                                if (checkMakeAOneTimePaymentPage)
                                {
                                    bool willScheduledPaymentInProgressPopAppear = webElementExtensions.IsElementDisplayed(_driver, confirmationNumbersInPendinPaymentSectionLocBy, isScrollIntoViewRequired: false);
                                    webElementExtensions.ClickElementUsingJavascript(_driver, btnMakePaymentLocBy);
                                    HandlePaperLessPage();
                                    if (willScheduledPaymentInProgressPopAppear)
                                        payments.AcceptScheduledPaymentIsProcessingPopUp();
                                    flag = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.addAnAccountLinkLocBy);
                                    reportLogger.TakeScreenshot(test, "Make a One-Time Payment", true);
                                    webElementExtensions.ScrollToTop(_driver);
                                    webElementExtensions.WaitUntilElementIsClickable(_driver, loanDepotLogoLocBy, ConfigSettings.SmallWaitTime);
                                    webElementExtensions.ClickElementUsingJavascript(_driver, loanDepotLogoLocBy, "Loan Depot Logo", isReportRequired: true);
                                    webElementExtensions.WaitUntilUrlContains("/dashboard");
                                }
                            }
                        }

                        if (checkManageAutopayPage)
                        {
                            webElementExtensions.WaitUntilElementIsClickable(_driver, manageAutopayButtonLocBy, ConfigSettings.SmallWaitTime);
                            webElementExtensions.ClickElement(_driver, manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
                            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                            HandlePaperLessPage();
                            webElementExtensions.WaitForVisibilityOfElement(_driver, payments.backToAccountSummaryLinkLocBy);
                            if (!webElementExtensions.IsElementDisabled(_driver, payments.expandArrowLocBy, isScrollIntoViewRequired: false))
                            {
                                if (webElementExtensions.IsElementEnabled(_driver, payments.setupAutopaybuttonLocBy, isScrollIntoViewRequired: false))
                                {
                                    test.Log(Status.Info, "Setup Autopay Button on Manage Autopay Page is <b>Enabled</b>");
                                    flag = true;
                                    test.Info($"Found desired loan!!!!!!!!!!!!!!!!!!!!!");
                                }
                                else
                                {
                                    test.Log(Status.Info, "Setup Autopay Button on Manage Autopay Page is <b>Disabled</b>");
                                    flag = false;
                                    test.Info($"Did not Found desired loan!!!!!!!!!!!!!!!!!!!!!!");
                                }
                            }
                            reportLogger.TakeScreenshot(test, "Manage Autopay Page");
                            webElementExtensions.WaitUntilElementIsClickable(_driver, loanDepotLogoLocBy, ConfigSettings.SmallWaitTime);
                            webElementExtensions.ClickElementUsingJavascript(_driver, payments.backToAccountSummaryLinkLocBy, "Loan Depot Logo", isReportRequired: true);
                            HandlePaperLessPage();
                            webElementExtensions.WaitUntilUrlContains("/dashboard");
                        }
                    }
                    else
                    {
                        test.Log(Status.Info, "Manage Autopay toggle is <b>turnned ON</b>");
                        flag = false;
                        test.Info($"Did not Found desired loan!!!!!!!!!!!!!!!!!!!!!");
                    }
                }
                else
                {
                    flag = true;
                    test.Info($"Found desired loan!!!!!!!!!!!!!!!!!!!!!");
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while linking the loan number to the user account: " + ex.Message);
            }
            if (isReportRequired && flag)
                _driver.ReportResult(test, flag, "Successfully verified that the loan number - " + loanNumber + " is linked to the account.", "Failed while linking the loan number - " + loanNumber + " to the account.");
            return flag;
        }

        /// <summary>
        /// Method to verify if the loan number has autopay setup already
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsLoanHasAutopayToggleON()
        {
            bool flag = false;

            test.Log(Status.Info, $"<b><u>Started checking if the linked loan is usefull for the execution or not</u></b>");

            string attribute_value = webElementExtensions.GetElementAttribute(_driver, manageAutopayOnOffButtonLocBy, "alt");
            test.Log(Status.Info, $"Manage Autopay Toggle is {attribute_value}");

            return attribute_value.IndexOf("ON", 0, StringComparison.OrdinalIgnoreCase) != -1;
        }

        /// <summary>
        /// Method to link my loan
        /// </summary>
        /// <param name="loanNumber">LoanNumber</param>
        /// <param name="propertyZipCode">Property Zip Code</param>
        /// <param name="borrowerSsn">Borrower SSn</param>
        /// <returns></returns>
        public void LinkLoan(string loanNumber, string propertyZipCode, string borrowerSsn, bool eligibleForAutopay = true, bool isReportRequired = false, string emailID = "SCRUBBED.EMAIL@NOTREAL.NET", bool isVerifyMFARequired = false)
        {
            bool flag = false;
            try
            {

                borrowerSsn = borrowerSsn.Substring(borrowerSsn.Length - 4);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                test.Log(Status.Info, $"<b><u>Started Linking the Loan Process</u></b>");
                if (_driver.Url.Contains("/dashboard"))
                {
                    webElementExtensions.WaitUntilElementIsClickable(_driver, commonServices.cpMyLoansDropdownLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.cpMyLoansDropdownLocBy, "My Dashboard Loans");
                    webElementExtensions.WaitUntilElementIsClickable(_driver, commonServices.cpAddYourLoanButtonLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.cpAddYourLoanButtonLocBy, "Add Your Loan");
                }
                test.Log(Status.Info, $"Loan Number: <b>{loanNumber}</b><br>Property Zip Code: <b>{propertyZipCode}</b><br>Last 4 digits of SSN: <b>{borrowerSsn}</b>");
                webElementExtensions.EnterText(_driver, commonServices.cpLinkYourExistingLoanPage_LoanNumberFieldLocBy, loanNumber, true, "Loan Number", true);
                webElementExtensions.EnterText(_driver, commonServices.cpLinkYourExistingLoanPage_ZipcodeFieldLocBy, propertyZipCode, true, "Property Zip Code", true);
                webElementExtensions.EnterText(_driver, commonServices.cpLinkYourExistingLoanPage_SsnNumberLocBy, borrowerSsn, true, "Last 4 digits of SSN", true);

                if (_driver.Url.Contains("/dashboard"))
                {

                    dBconnect.UpdateBorrowerEmailID(loanNumber, emailID);
                    flag = webElementExtensions.ActionClick(_driver, commonServices.cpLinkALoanPage_linkLoanButtonLocBy, "Link Loan button", true);
                    if (isVerifyMFARequired)
                        VerifyMFAAuthentication(loanNumber);
                    else
                        ByPassMFAAuthentication(loanNumber);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.cpLinkALoanPage_linkLoanButtonLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, makeAPaymentButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                }

                else
                {

                    dBconnect.UpdateBorrowerEmailID(loanNumber, emailID);
                    flag = webElementExtensions.ActionClick(_driver, commonServices.cpLinkYourExistingLoanPage_linkMyLoanButtonLocBy, "Link My Loan button", true);
                    if (isVerifyMFARequired)
                        VerifyMFAAuthentication(loanNumber);
                    else
                        ByPassMFAAuthentication(loanNumber);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.cpLinkYourExistingLoanPage_linkMyLoanButtonLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, makeAPaymentButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                }
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while Linking Loan. Error : <b>{e.Message}</b>");
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully Linked Loan", "Failed while Linking Loan.");
        }

        /// <summary>
        /// Method the check if the below Keys value is Null or not
        /// </summary>
        /// <param name="loanLevelData">Data related to a particular loan</param>
        /// <returns></returns>
        public bool WillPaperLessPageAppear(Hashtable loanLevelData, bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                var keysToCheck = new HashSet<string>
            {
            Constants.LoanLevelDataColumns.EConsent1098Flag.ToString(),
            Constants.LoanLevelDataColumns.EConsent1098Date.ToString(),
            Constants.LoanLevelDataColumns.EConsentBillingFlag.ToString(),
            Constants.LoanLevelDataColumns.EConsentBillingDate.ToString(),
            Constants.LoanLevelDataColumns.EConsentArmChangeNoticeFlag.ToString(),
            Constants.LoanLevelDataColumns.EConsentArmChangeNoticeDate.ToString(),
            Constants.LoanLevelDataColumns.EConsentEscrowAnalysisFlag.ToString(),
            Constants.LoanLevelDataColumns.EConsentEscrowAnalysisDate.ToString()
            };
                foreach (var key in keysToCheck)
                {
                    var value = loanLevelData[key];
                    if (!(loanLevelData.ContainsKey(key) && loanLevelData[key].ToString().Equals("")))
                    {
                        flag = false;
                        return flag;
                    }
                }
            }
            catch (Exception ex)
            {
                test.Log(Status.Error, $"Something happened while Checking if the Paperless Page will be displayed or not Error : <b>{ex.Message}</b>");
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully Checked if the Paperless Page will be displayed or not", "Failed while Checking if the Paperless Page will be displayed or not");
            return flag = true;
        }

        /// <summary>
        /// Get the last 4 digit loan number selected in dropdown list
        /// </summary>
        /// <returns></returns>
        public string GetLastFourDigitsFromLoanSelected()
        {

            string selected_loan = webElementExtensions.GetElementText(_driver, selectedLoanLocBy).Trim();
            return selected_loan.Substring(selected_loan.Length - 4);
        }

        /// <summary>
        /// Method to verify if the Autopay Delete Button is Enabeled
        /// </summary>
        /// <returns>true/false</returns>
        public bool IsLoanHasAutopayDeleteButtonEnabled()
        {
            bool flag = false;
            test.Log(Status.Info, $"<b><u>Started checking if the linked loan is usefull for the execution or not</u></b>");
            webElementExtensions.WaitUntilElementIsClickable(_driver, manageAutopayButtonLocBy, ConfigSettings.SmallWaitTime);
            webElementExtensions.ActionClick(_driver, manageAutopayButtonLocBy, "Manage Autopay button.", isReportRequired: true);
            webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
            webElementExtensions.WaitForVisibilityOfElement(_driver, payments.deleteIconManageAutopayLocBy);
            string classValue = webElementExtensions.GetElementAttribute(_driver, payments.deleteIconManageAutopayLocBy, "class");
            flag = !classValue.Contains("cursor-default");
            test.Log(Status.Info, $"Manage Autopay delete button  is " + ((flag) ? "Enabled" : "Disabled"));
            webElementExtensions.WaitUntilElementIsClickable(_driver, loanDepotLogoLocBy, ConfigSettings.SmallWaitTime);
            webElementExtensions.ClickElementUsingJavascript(_driver, loanDepotLogoLocBy, "Loan Depot Logo", isReportRequired: true);
            webElementExtensions.WaitUntilUrlContains("/dashboard");
            return flag;
        }

        /// <summary>
        /// Validate amounts showing in dashboard payment summary section with database amounts
        /// </summary>
        /// <param name="principalAndInterestDb">string "418.34"</param>
        /// <param name="taxAndInsuranceDb">string "418.35"</param>
        /// <param name="amountDueCalculated">string "418.36"</param>
        public void VerifyPaymentSummary(Hashtable loanLevelData)
        {
            try
            {
                //Values from UI
                string principalAndInterest = webElementExtensions.GetElementText(_driver, principalAndInterestAmountValueTextLocBy);
                string taxAndInsurance = webElementExtensions.GetElementText(_driver, taxAndInsuranceAmountValueTextLocBy);
                string fees = webElementExtensions.GetElementText(_driver, feesAmountValueTextLocBy);
                string subsidy = webElementExtensions.GetElementText(_driver, subsidyTextLocBy);
                string totalAmountDue = webElementExtensions.GetElementText(_driver, totalAmountDueValueTextLocBy);
                string amountDue = webElementExtensions.GetElementText(_driver, currentAmountDueInHeaderValueTextLocBy);

                reportLogger.TakeScreenshot(test, "Payment Summary");

                //assertions
                ReportingMethods.LogAssertionEqual(test, Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.PrincipalInterestAmount]).ToString("C"), principalAndInterest, $"Verify Principal and Interest Amount on Dashboard Page");
                ReportingMethods.LogAssertionEqual(test, Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TaxAndInsuranceAmount]).ToString("C"), taxAndInsurance, $"Verify Tax and Insurance Amount on Dashboard Page");
                /* commented to code for fees but it will be needed in future once bug related to fees is fixed */
                //ReportingMethods.LogAssertionEqual(test, fees, $"${feesDb}", $"Fees Amount not mathcing , expected : {feesDb}, actual : {fees}");

                // Determine if the loan status is delinquent
                bool isDelinquent = loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus]
                                        .ToString()
                                        .Equals(Constants.LoanStatus.Delinquent);

                // Define the amounts to be added
                double delinquentPaymentBalance = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.DelinquentPaymentBalance]);
                double accruedLateChargeAmount = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.AccruedLateChargeAmount]);
                double nsfFeeBalance = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance]);
                double recoverableCorpAdvBalance = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.RecoverableCorpAdvBalance]);
                double otherFeeBalance = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.OtherFees]);
                var subsidyAmountCalculated = CalculateSubsidyAmount(loanLevelData);

                // If delinquent, calculate the total for delinquent balance, otherwise use the regular monthly payment
                double expAmountDue = isDelinquent
                                    ? delinquentPaymentBalance + accruedLateChargeAmount + nsfFeeBalance + recoverableCorpAdvBalance + otherFeeBalance
                                    : Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.TotalMonthlyPayment])
                                        + accruedLateChargeAmount + nsfFeeBalance + otherFeeBalance;

                // Format the amount as currency
                string amountDueCalculated = expAmountDue.ToString("C");
                if (!subsidyAmountCalculated.Equals("0.00"))
                    ReportingMethods.LogAssertionEqual(test, $"-${subsidyAmountCalculated}", subsidy, $"Verify Subsidy Amount on Dashboard Page");
                ReportingMethods.LogAssertionEqual(test, amountDueCalculated, amountDue, $"Verify Amount Due on Dashboard Page");
                ReportingMethods.LogAssertionEqual(test, totalAmountDue, amountDue, "Verify Total Amount Due on Dashboard Page");
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while verifying Payment Summary Error : <b>{e.Message}</b>");
            }
        }

        /// <summary>
        /// Calculate expected subsidy Amount 
        /// </summary>
        /// <param name="loanLevelData">loan's record from db</param>
        public string CalculateSubsidyAmount(Hashtable loanLevelData)
        {
            DateTime dueDate = DateTime.Parse(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString());
            DateTime today = DateTime.Now.Date;


            int numberOfMonths = Math.Abs(today.Year - dueDate.Year) * 12 + (today.Month - dueDate.Month) + 1;
            int remMonths = 0;
            double subsidyCalculated = 0;
            var reserveBal = Double.Parse(loanLevelData[Constants.LoanLevelDataColumns.ReplacementReserveBalance].ToString());
            var period1 = int.Parse(loanLevelData[Constants.LoanLevelDataColumns.SubsidyPeriod1Months].ToString());
            var period2 = int.Parse(loanLevelData[Constants.LoanLevelDataColumns.SubsidyPeriod2Months].ToString());
            var period3 = int.Parse(loanLevelData[Constants.LoanLevelDataColumns.SubsidyPeriod3Months].ToString());
            var period4 = int.Parse(loanLevelData[Constants.LoanLevelDataColumns.SubsidyPeriod4Months].ToString());
            var period5 = int.Parse(loanLevelData[Constants.LoanLevelDataColumns.SubsidyPeriod5Months].ToString());

            var subsidyAmount1 = Double.Parse(loanLevelData[Constants.LoanLevelDataColumns.SubsidyAmount1].ToString());
            var subsidyAmount2 = Double.Parse(loanLevelData[Constants.LoanLevelDataColumns.SubsidyAmount2].ToString());
            var subsidyAmount3 = Double.Parse(loanLevelData[Constants.LoanLevelDataColumns.SubsidyAmount3].ToString());
            var subsidyAmount4 = Double.Parse(loanLevelData[Constants.LoanLevelDataColumns.SubsidyAmount4].ToString());
            var subsidyAmount5 = Double.Parse(loanLevelData[Constants.LoanLevelDataColumns.SubsidyAmount5].ToString());

            //Calculate the subsidy based on period and amount is respective period for number of months required
            if (period1 >= numberOfMonths)
            {
                subsidyCalculated = numberOfMonths * subsidyAmount1;
            }

            else
            {
                remMonths = numberOfMonths - period1;
                subsidyCalculated = period1 * subsidyAmount1;

                if (period2 >= remMonths)
                {
                    subsidyCalculated += remMonths * subsidyAmount2;
                }
                else
                {
                    remMonths = remMonths - period2;
                    subsidyCalculated += period2 * subsidyAmount2;

                    if (period3 >= remMonths)
                    {
                        subsidyCalculated += remMonths * subsidyAmount3;
                    }
                    else
                    {
                        remMonths = remMonths - period3;
                        subsidyCalculated += period3 * subsidyAmount3;

                        if (period4 >= remMonths)
                        {
                            subsidyCalculated += remMonths * subsidyAmount4;
                        }
                        else
                        {
                            remMonths = remMonths - period4;
                            subsidyCalculated += period4 * subsidyAmount4;

                            if (period5 >= remMonths)
                            {
                                subsidyCalculated += remMonths * subsidyAmount5;
                            }
                        }
                    }
                }
            }

            //return subsidyCalculated or reserveBal whichever is lower

            var calculatedSubsidy = subsidyCalculated <= reserveBal ? subsidyCalculated : reserveBal;

            test.Log(Status.Info, $"Subsidy Calculated based on DB Amounts : {calculatedSubsidy}$");
            return subsidyCalculated.ToString("N");

        }

        /// <summary>
        /// To By pass the MFA Authentication while linking the loan
        /// </summary>
        /// <param name="loanNumber">string "1040628842"</param>
        public void ByPassMFAAuthentication(string loanNumber)
        {
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, receiveCodeViaEmailButtonLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.ActionClick(_driver, receiveCodeViaEmailButtonLocBy, "Receive Code Via Email Button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, receiveCodeViaEmailButtonLocBy, ConfigSettings.MediumWaitTime);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, verificationOTPInputFieldLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.EnterText(_driver, verificationOTPInputFieldLocBy, Constants.MFAByPassCode.ByPassMFACode);
                webElementExtensions.WaitForElementToBeEnabled(_driver, verifyCodeButtonLocBy, "Verify Button", isScrollIntoViewRequired: true);
                webElementExtensions.ActionClick(_driver, verifyCodeButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, verifyCodeButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, verifiedMFAPopUpLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                webElementExtensions.ActionClick(_driver, continueButtonLocBy, "Continue Button", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while Bypassing MFA authentication Error : <b>{e.Message}</b>");
            }
        }

        /// <summary>
        /// Clicks on Request Payoff quote
        /// </summary>
        public void RequestPayoffQuote()
        {
            try
            {
                //There are two buttons present with same id and properties, iterate over to find the right one
                reportLogger.TakeScreenshot(test, "Request Payoff Quote");
                var requestQuote = _driver.FindElements(payoffQuoteLocBy)
                    .Where(e => e.Enabled == true && e.Displayed == true).FirstOrDefault();
                webElementExtensions.ClickElementUsingJavascript(_driver, requestQuote);
                test.Log(Status.Info, "Clciked on Request Payoff Quote Button");
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while Requesting payoff quote <b>{e.Message}</b>");
            }


        }
        /// <summary>
        /// Method to verify Payment Summary Details page displayed
        /// </summary>
        /// <param name="loanLevelData">Loan Level Data from DB</param>
        public void VerifyPaymentSummaryDetailsOnDashboardPage(Hashtable loanLevelData)
        {
            try
            {
                string xpathCondition = string.Empty;

                if (loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus]
                    .ToString()
                    .Equals(Constants.LoanStatus.Delinquent.ToString()))
                {
                    xpathCondition = "/following-sibling::h4";
                }
                else if (loanLevelData[Constants.LoanLevelDataColumns.NsfFeeBalance].ToString().Equals("0.00")
                    || loanLevelData[Constants.LoanLevelDataColumns.AccruedLateChargeAmount].ToString().Equals("0.00")
                    || loanLevelData[Constants.LoanLevelDataColumns.OtherFees].ToString().Equals("0.00"))
                {
                    xpathCondition = "/following-sibling::h4";
                }
                else
                {
                    xpathCondition = "/../following-sibling::div/p";
                }

                // Replace the placeholder in the XPath string
                By loc = By.XPath(currentBalanceValueTextBy.Replace("<TEXT>", xpathCondition));
                string currentBalance = webElementExtensions.GetElementText(_driver, loc);
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.FirstPrincipalBalance]).ToString("N").Trim()}", currentBalance, "Verify Current Balance on Dashboard Page");

                string escrowBalance = webElementExtensions.GetElementText(_driver, escrowBalanceValueTextLocBy);
                double calculatedValue = Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.EscrowBalance])
                       - Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.EscrowAdvanceBalance]);

                string expectedEscrowBalance = (calculatedValue < 0)
                    ? $"-${Math.Abs(calculatedValue).ToString("N2")}"
                    : $"${calculatedValue.ToString("N2")}";

                ReportingMethods.LogAssertionEqual(test, expectedEscrowBalance, escrowBalance, "Verify Escrow Balance on Dashboard Page");

                if (!(DateTime.Now.Day <= 15 && loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus].Equals("Past Due")))
                {
                    string accountStanding = webElementExtensions.GetElementText(_driver, accountStandingValueTextLocBy);
                    ReportingMethods.LogAssertionEqual(test, (loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus]).Equals("On-time") ? Constants.LoanStatus.Ontime : (loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus]).Equals("Past Due") ? Constants.LoanStatus.PastDue : Constants.LoanStatus.Delinquent, accountStanding, "Verify Account Standing on Dashboard page");
                }


                string currentPaymentDueDate = webElementExtensions.GetElementText(_driver, currentPaymentDueDateValueTextLocBy);
                DateTime date = DateTime.Parse(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString());
                ReportingMethods.LogAssertionEqual(test, "Current payment due date " + date.ToString("MMM d, yyyy"), currentPaymentDueDate, "Verify Current Payment Due Date field value");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.VerifyElementColor(Constants.Colors.Purple, By.XPath(commonServices.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)), null, "Verify Make A Payment Button color is Purple", isScrollIntoViewRequired: false, isScrollToTopRequired: true), "Verify the Color of Make a Payment Button on Dashboard Page");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementEnabled(_driver, By.XPath(commonServices.buttonByText.Replace("<BUTTONNAME>", Constants.ButtonNames.MakeAPayment)), "Make A Payment", false), "Verify Is Make A Payment Enabled");

                VerifyPaymentSummary(loanLevelData);
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while Payment Summary Details On Dashboard Page Error : <b>{e.Message}</b>");
            }
        }

        /// <summary>
        /// Verify Dashboard Page for Heloc Autopay Loans
        /// </summary>
        /// <param name="loanLevelData">Loan Level Data from DB</param>
        /// <param name="isReportRequired"></param>
        /// <returns></returns>
		public void VerifyPaymentSummaryDetailsForHelocLoanOnDashboardPage(Hashtable loanLevelData, APIConstants.HelocLoanInfo helocLoanInfo, decimal totalFees, bool isReportRequired = false)
        {

            bool flag = false;
            try
            {
                string xpathCondition = string.Empty, xpathForTextValueAgainstFieldName = string.Empty;
                xpathCondition = "/following-sibling::h4";
                xpathForTextValueAgainstFieldName = "//div[text()='<TEXT>']/following-sibling::div";

                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                reportLogger.TakeScreenshot(test, "Dashboard Page with Heloc Loan");
                By loc = By.XPath(xpathForTextValueAgainstFieldName.Replace("<TEXT>", "Account Standing"));
                string accountStanding = webElementExtensions.GetElementText(_driver, loc);
                string expAccountStanding = string.Empty;
                if (loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus].Equals("Past Due"))
                {
                    if (DateTime.Now.Day >= 15)
                    {
                        expAccountStanding = Constants.LoanStatus.PastDue;
                        ReportingMethods.LogAssertionEqual(test, expAccountStanding, accountStanding.Trim(), "Verify Account Standing on Dashboard Page");
                    }
                }
                else if (loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus].Equals("On-time") || loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus].Equals(Constants.LoanStatus.Delinquent))
                {
                    expAccountStanding = ((loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus].Equals("On-time"))
                    ? Constants.LoanStatus.Ontime : Constants.LoanStatus.Delinquent);
                    ReportingMethods.LogAssertionEqual(test, expAccountStanding, accountStanding.Trim(), "Verify Account Standing on Dashboard Page");
                }

                loc = By.XPath(currentBalanceValueTextBy.Replace("<TEXT>", xpathCondition));
                string currentBalance = webElementExtensions.GetElementText(_driver, loc);
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(helocLoanInfo.CurrentBalance).ToString("N").Trim()}", currentBalance, "Verify Current Balance on Dashboard Page");

                loc = By.XPath(availableCreditLineValueTextBy.Replace("<TEXT>", xpathCondition));
                string availableCreditLineBalance = webElementExtensions.GetElementText(_driver, loc);
                string expAvailableCreditLineBalance = Convert.ToDouble(helocLoanInfo.AvailableHELOCAmount).ToString("N").Trim();
                ReportingMethods.LogAssertionEqual(test, $"${expAvailableCreditLineBalance}", availableCreditLineBalance, "Verify Available Credit Line Balance on Dashboard Page");

                loc = By.XPath(xpathForTextValueAgainstFieldName.Replace("<TEXT>", "Credit Limit"));
                string creditLineTextValue = webElementExtensions.GetElementText(_driver, loc).Trim();
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(helocLoanInfo.HelocLimitAmount).ToString("N").Trim()}", creditLineTextValue, "Verify Credit Line Balance on Dashboard Page");

                loc = By.XPath(xpathForTextValueAgainstFieldName.Replace("<TEXT>", "YTD Interest Paid"));
                string YTDInterestPaidTextValue = webElementExtensions.GetElementText(_driver, loc).Trim();
                ReportingMethods.LogAssertionEqual(test, $"${Convert.ToDouble(helocLoanInfo.YtdInterestPaid).ToString("N").Trim()}", YTDInterestPaidTextValue, "Verify YTD Interest Paid Balance on Dashboard Page");

                loc = By.XPath(xpathForTextValueAgainstFieldName.Replace("<TEXT>", "Interest Rate"));
                string interestRateTextValue = webElementExtensions.GetElementText(_driver, loc).Trim();
                string expInterestRate = (Convert.ToDouble(loanLevelData[Constants.LoanLevelDataColumns.InterestRates]) * 100).ToString("0.#######") + "%";
                ReportingMethods.LogAssertionEqual(test, expInterestRate, interestRateTextValue, "Verify Interest Rates on Dashboard Page");

                DBconnect dBconnect = new DBconnect(test, "MelloServETL");
                string expectedLastPaymentDate = DateTime.Parse(dBconnect.GetLastPaymentDate(loanLevelData[Constants.LoanLevelDataColumns.LoanNumber].ToString())).ToString("MMM d, yyyy");
                loc = By.XPath(xpathForTextValueAgainstFieldName.Replace("<TEXT>", "Last Payment Date"));
                string lastPaymentDateTextValue = webElementExtensions.GetElementText(_driver, loc).Trim();
                ReportingMethods.LogAssertionEqual(test, expectedLastPaymentDate, lastPaymentDateTextValue, "Verify Last Payment Date on Dashboard Page");

                string nextPaymentDueDate = webElementExtensions.GetElementText(_driver, nextPaymentDueDateForHelocLoansTextLocBy);
                string expNextPaymentDueDate = DateTime.Parse(loanLevelData[Constants.LoanLevelDataColumns.NextPaymentDueDate].ToString()).ToString("MMM d, yyyy");
                ReportingMethods.LogAssertionContains(test, expNextPaymentDueDate, nextPaymentDueDate, "Verify Next Payment Due Date on Dashboard Page");

                webElementExtensions.MoveToElement(_driver, paymentInfoIconLocBy);
                webElementExtensions.WaitForVisibilityOfElement(_driver, paymentInfoExplanationPopupTextsLocBy);
                string paymentInfoExplanationPopUpTextContent = webElementExtensions.GetElementText(_driver, paymentInfoExplanationPopupTextsLocBy);


                string exptPrincipalAndInterestBalance = $"${Convert.ToDouble(helocLoanInfo.NextPaymentDue).ToString("N").Trim()}";
                string exptFees = $"${Convert.ToDouble(totalFees).ToString("N").Trim()}";
                string exptNextAmountDue = $"${(Convert.ToDouble(helocLoanInfo.NextPaymentDue) + Convert.ToDouble(totalFees)).ToString("N").Trim()}";
                if (helocLoanInfo.IsBillGenerated || loanLevelData[Constants.LoanLevelDataColumns.DaysPastDueStatus].ToString().Equals(Constants.LoanStatus.PastDue))
                {
                    if (test.Model.FullName.Contains("HelocOTPPastDue") || test.Model.FullName.Contains("HelocOTPDelinquent"))
                    {
                        var payment = (helocLoanInfo.PaymentsDue).Sum(paymentsDue => (Convert.ToDouble(paymentsDue.PrincipalAndInterest)));
                        exptPrincipalAndInterestBalance = "$" + payment.ToString("N").Trim();
                        exptNextAmountDue = "$" + (payment + Convert.ToDouble(totalFees)).ToString("N").Trim();
                    }

                    ReportingMethods.LogAssertionContains(test, exptPrincipalAndInterestBalance, paymentInfoExplanationPopUpTextContent, "Verify Principal and Interest Balance on Explanation Pop Up on Dashboard Page");
                    ReportingMethods.LogAssertionContains(test, exptFees, paymentInfoExplanationPopUpTextContent, "Verify Fees on Explanation Pop Up on Dashboard Page");
                    ReportingMethods.LogAssertionContains(test, exptNextAmountDue, paymentInfoExplanationPopUpTextContent, "Verify Next Amount Due on Explanation Pop Up on Dashboard Page");
                }
                else
                {
                    if (test.Model.FullName.Contains("HelocOTPDelinquent"))
                    {
                        var payment = (helocLoanInfo.PaymentsDue).Sum(paymentsDue => (Convert.ToDouble(paymentsDue.PrincipalAndInterest)));
                        exptPrincipalAndInterestBalance = "$" + payment.ToString("N").Trim();
                        exptNextAmountDue = "$" + (payment + Convert.ToDouble(totalFees)).ToString("N").Trim();
                        ReportingMethods.LogAssertionContains(test, exptPrincipalAndInterestBalance, paymentInfoExplanationPopUpTextContent, "Verify Principal and Interest Balance on Explanation Pop Up on Dashboard Page");
                        ReportingMethods.LogAssertionContains(test, exptFees, paymentInfoExplanationPopUpTextContent, "Verify Fees on Explanation Pop Up on Dashboard Page");
                        ReportingMethods.LogAssertionContains(test, exptNextAmountDue, paymentInfoExplanationPopUpTextContent, "Verify Next Amount Due on Explanation Pop Up on Dashboard Page");
                    }
                    else
                    {
                        ReportingMethods.LogAssertionContains(test, $"Principal and Interest\r\n${0.00.ToString("N").Trim()}", paymentInfoExplanationPopUpTextContent, "Verify Principal and Interest Balance on Explanation Pop Up on Dashboard Page");
                        ReportingMethods.LogAssertionContains(test, $"Fees\r\n${0.00.ToString("N").Trim()}", paymentInfoExplanationPopUpTextContent, "Verify Fees on Explanation Pop Up on Dashboard Page");
                        ReportingMethods.LogAssertionContains(test, $"Next Amount Due\r\n${0.00.ToString("N").Trim()}", paymentInfoExplanationPopUpTextContent, "Verify Next Amount Due on Explanation Pop Up on Dashboard Page");
                    }
                }
                reportLogger.TakeScreenshot(test, "Explanation Pop Up on Dashboard Page");
            }
            catch (Exception ex)
            {
                log.Error("Failed while checking radiobutton is checked " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully  checked radiobutton is checked.", "Failed while checking radiobutton is checked.");
        }

        /// <summary>
        /// Open the product page in new tab and validates it's url
        /// </summary>
        /// <param name="productName">name of product</param>
        public void CheckProduct(string productName)
        {
            bool isProductPageOpened = false;
            try
            {
                By productLocBy = By.XPath(string.Format(productsPath, productName));
                var item = _driver.FindElements(productLocBy).Where(e => e.Displayed).FirstOrDefault();

                if (item != null)
                {
                    //Open product page
                    var tabsBefore = _driver.WindowHandles;
                    var mainHandle = _driver.CurrentWindowHandle;
                    webElementExtensions.ScrollIntoView(_driver, prodsSectionLocBy);
                    reportLogger.TakeScreenshot(test, $"Dashboard : {productName}");
                    webElementExtensions.ClickElementUsingJavascript(_driver, item);
                    webElementExtensions.WaitForElement(_driver, learnMoreBtnLocBy);
                    reportLogger.TakeScreenshot(test, $"Product Dialog : {productName}");
                    webElementExtensions.ClickElement(_driver, learnMoreBtnLocBy);
                    webElementExtensions.WaitForPageLoad(_driver);
                    webElementExtensions.WaitForNewTabWindow(_driver, tabsBefore.Count + 1, ConfigSettings.LongWaitTime);
                    var tabsAfter = _driver.WindowHandles;
                    ReportingMethods.LogAssertionTrue(test, tabsAfter.Count > tabsBefore.Count, $"{productName} Product Page should be opened in new tab");
                    if (tabsAfter.Count > 0)
                    {
                        foreach (var tab in tabsAfter)
                        {
                            try
                            {

                                if (tab != mainHandle)
                                {
                                    webElementExtensions.SwitchToTab(_driver, tab);
                                    webElementExtensions.WaitForPageLoad(_driver);
                                    isProductPageOpened = true;
                                    reportLogger.TakeScreenshot(test, $"{productName} Product Page in new Tab");

                                    switch (productName)
                                    {

                                        case "Solar":
                                            ReportingMethods.LogAssertionContains(test, "www.lumio.com", _driver.Url, "Documents Url should have borrowed Documents");
                                            break;
                                        case "Home Renovation":
                                            ReportingMethods.LogAssertionContains(test, "portal.bosscathome.com/mellohome/repairs", _driver.Url, "Documents Url should have borrowed Documents");
                                            break;
                                        case "Home Maintenance":
                                            ReportingMethods.LogAssertionContains(test, "portal.bosscathome.com/mellohome/maintenance", _driver.Url, "Documents Url should have borrowed Documents");
                                            break;
                                        default:
                                            ReportingMethods.LogAssertionFalse(test, true, $"Unsopported {productName}");
                                            break;

                                    }
                                    _driver.Close();
                                }
                            }
                            catch { }
                        }
                        webElementExtensions.SwitchToTab(_driver, mainHandle);
                    }
                    ReportingMethods.LogAssertionTrue(test, isProductPageOpened, $"{productName} Page should be Opened");

                }
            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, $"Error");
                test.Log(Status.Error, $"Eror while checking Products and Services {e.Message}");
            }
        }

        /// <summary>
        /// Switch to the other loan if already linked
        /// </summary>
        /// <param name="lastFourDigitLoanNumber">last 4 digits of required loan</param>
        public void SwitchToLinkedLoan(string lastFourDigitLoanNumber)
        {
            string selectedLoan = GetLastFourDigitsFromLoanSelected();
            if (lastFourDigitLoanNumber.Equals(selectedLoan, StringComparison.OrdinalIgnoreCase))
                return;
            else
            {
                webElementExtensions.WaitUntilElementIsClickable(_driver, commonServices.cpMyLoansDropdownLocBy);
                webElementExtensions.ActionClick(_driver, commonServices.cpMyLoansDropdownLocBy, "My Dashboard Loans");
                webElementExtensions.WaitUntilElementIsClickable(_driver, commonServices.cpAddYourLoanButtonLocBy);
                By requiredLinkedLoanPathLocBy = By.XPath(string.Format(loanDropDownItemPath, lastFourDigitLoanNumber));
                webElementExtensions.WaitUntilElementIsClickable(_driver, requiredLinkedLoanPathLocBy);
                webElementExtensions.ActionClick(_driver, requiredLinkedLoanPathLocBy, $"Select Linked Loan {lastFourDigitLoanNumber}");
                webElementExtensions.WaitUntilUrlContains("dashboard");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.WaitTime);
                log.Info($"Switched to Loan-{lastFourDigitLoanNumber}");
                reportLogger.TakeScreenshot(test, "Selected Loan");
            }
        }

        /// <summary>
        /// To verify the MFA Authentication while linking the loan
        /// </summary>
        /// <param name="loanNumber">string "1040628842"</param>
        public void VerifyMFAAuthentication(string loanNumber)
        {
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, verifyYourIdentityPopupLocBy, ConfigSettings.SmallWaitTime);

                if (_driver.Url.Contains("/dashboard") || _driver.Url.Contains("/contact-information"))
                {
                    webElementExtensions.WaitUntilElementIsClickable(_driver, faqLinkOnVerifyYourIdentityPopupLocBy);
                    webElementExtensions.ActionClick(_driver, faqLinkOnVerifyYourIdentityPopupLocBy, "FAQ Link On Verify Your Identity", isReportRequired: true);
                    webElementExtensions.WaitForNewTabWindow(_driver, 1, ConfigSettings.SmallWaitTime);
                    webElementExtensions.SwitchTabs(_driver, 1);
                    webElementExtensions.WaitForStalenessOfElement(_driver, faqHeaderTextOnFaqPageLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                    webElementExtensions.WaitUntilUrlContains("/faqs");
                    ReportingMethods.LogAssertionContains(test, "/faqs", _driver.Url.ToString(), $"Verify if the UR contains <b>/faqs<b>");
                    reportLogger.TakeScreenshot(test, "FAQs Page");
                    FAQsPage fAQsPage = new FAQsPage(_driver, test);
                    fAQsPage.VerifyTheTextContentForSpecificGeneralQuestionTopic("What is multi-factor authentication?");
                    _driver.Close();
                    webElementExtensions.SwitchTabs(_driver, 0);
                }

                webElementExtensions.ActionClick(_driver, cancelBtnLocBy, "Cancel Button on Verifyyour Identity Popup", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, verifyYourIdentityPopupLocBy, ConfigSettings.SmallWaitTime);
                if (_driver.Url.Contains("/dashboard"))
                    webElementExtensions.ActionClick(_driver, commonServices.cpLinkALoanPage_linkLoanButtonLocBy, "Link Loan button", true);
                else if (_driver.Url.Contains("/contact-information"))
                    webElementExtensions.ActionClick(_driver, saveChangesButtonOnContactInfoPageLocBy, "Save Changes button on Contact Infor Page button", true);
                else
                    webElementExtensions.ActionClick(_driver, commonServices.cpLinkYourExistingLoanPage_linkMyLoanButtonLocBy, "Link My Loan button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, verifyYourIdentityPopupLocBy, ConfigSettings.SmallWaitTime);
                string textContentOfVerifyYourIdentityPopup = webElementExtensions.GetElementText(_driver, verifyYourIdentityPopupLocBy, true);
                if (_driver.Url.Contains("/dashboard"))
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.VerifyYourIdentityPopupForOtherThanFirstLoanTextContent, textContentOfVerifyYourIdentityPopup, "Verify Text Content of Verify Your Identity Popup on Dashboard Page");
                else if (_driver.Url.Contains("/contact-information"))
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.VerifyYourIdentityPopupOnContactInfoPageTextContent, textContentOfVerifyYourIdentityPopup, "Verify Text Content of Verify Your Identity Popup On Contact Info Page");
                else
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.VerifyYourIdentityPopupTextContent, textContentOfVerifyYourIdentityPopup, "Verify Text Content of Verify Your Identity Popup");

                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementEnabled(_driver, receiveCodeViaEmailButtonLocBy, isScrollIntoViewRequired: false), "Verify Receive Code Via Email Button is enabled or not");
                reportLogger.TakeScreenshot(test, "Verify your Identity Popup");
                if (!_driver.Url.Contains("/dashboard") && !_driver.Url.Contains("/contact-information"))
                {
                    webElementExtensions.MoveToElement(_driver, multiFactorAuthenticatorCodeTooltipIconLocBy);
                    webElementExtensions.WaitForStalenessOfElement(_driver, multiFactorAuthenticatorCodeTooltipTextContentLocBy, 2);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, multiFactorAuthenticatorCodeTooltipTextContentLocBy);
                    string tooltipTextContent = webElementExtensions.GetElementText(_driver, multiFactorAuthenticatorCodeTooltipTextContentLocBy, true);
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.MultiFactorAuthenticatorCodeTooltipTextContent, tooltipTextContent, "Verify Text of Multi-Factor Authenticator Code tooltip");
                    reportLogger.TakeScreenshot(test, "Verify Your Identity - Multi-Factor Authenticator Code tooltip");
                }
                webElementExtensions.ActionClick(_driver, receiveCodeViaEmailButtonLocBy, isReportRequired: true);

                webElementExtensions.WaitForInvisibilityOfElement(_driver, receiveCodeViaEmailButtonLocBy, ConfigSettings.MediumWaitTime);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, verifyYourIdentityPopupLocBy, ConfigSettings.SmallWaitTime);
                string textContentOfVerifyYourIdentityVerificationCodeInputPopup = webElementExtensions.GetElementText(_driver, verifyYourIdentityPopupLocBy, true);
                if (_driver.Url.Contains("/dashboard"))
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.VerifyYourIdentifyVerificationCodeInputForOtherThanFirstLoanPopupTextContent, textContentOfVerifyYourIdentityVerificationCodeInputPopup, "Verify Your Identity Popup Text Content");
                else if (_driver.Url.Contains("/contact-information"))
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.VerifyYourIdentifyVerificationCodeInputOnContactInfoPopupTextContent, textContentOfVerifyYourIdentityVerificationCodeInputPopup, "Verify Your Identity Popup Text Content");
                else
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.VerifyYourIdentifyVerificationCodeInputPopupTextContent, textContentOfVerifyYourIdentityVerificationCodeInputPopup, "Verify Your Identity Popup Text Content");
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, verifyCodeButtonLocBy, isScrollIntoViewRequired: false), "Verify button should be disabled");
                reportLogger.TakeScreenshot(test, "Verify Your Identify Verification Code Input Popup");
                if (!_driver.Url.Contains("/dashboard") && !_driver.Url.Contains("/contact-information"))
                {
                    webElementExtensions.MoveToElement(_driver, multiFactorAuthenticatorCodeTooltipIconLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, multiFactorAuthenticatorCodeTooltipTextContentLocBy);
                    string tooltipTextContent1 = webElementExtensions.GetElementText(_driver, multiFactorAuthenticatorCodeTooltipTextContentLocBy, true);
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.MultiFactorAuthenticatorCodeTooltipTextContent, tooltipTextContent1, "Verify Text of Multi-Factor Authenticator Code tooltip");
                    reportLogger.TakeScreenshot(test, "Verify Your Identity - Multi-Factor Authenticator Code tooltip");
                }

                webElementExtensions.WaitForVisibilityOfElement(_driver, verificationOTPInputFieldLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.EnterText(_driver, verificationOTPInputFieldLocBy, "02202", isScrollIntoViewRequired: false, "Verification OTP Input Field");
                webElementExtensions.WaitForElementToBeDisabled(_driver, verifyCodeButtonLocBy, "Verify Button", isScrollIntoViewRequired: true);
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementDisabled(_driver, verifyCodeButtonLocBy, isScrollIntoViewRequired: false), "Verify button should be Disabled if user enters code less then 6 digits");
                reportLogger.TakeScreenshot(test, "Verify Your Identify Verification Code Input Popup");
                webElementExtensions.EnterText(_driver, verificationOTPInputFieldLocBy, Constants.MFAByPassCode.ByPassMFACode, isScrollIntoViewRequired: false, "Verification OTP Input Field");
                webElementExtensions.WaitForElementToBeEnabled(_driver, verifyCodeButtonLocBy, "Verify Button", isScrollIntoViewRequired: true);
                ReportingMethods.LogAssertionTrue(test, webElementExtensions.IsElementEnabled(_driver, verifyCodeButtonLocBy, isScrollIntoViewRequired: false), "Verify button should be Enabled if user enters 6 digits code");
                reportLogger.TakeScreenshot(test, "Verify Your Identify Verification Code Input Popup");

                webElementExtensions.ActionClick(_driver, verifyCodeButtonLocBy, "Verify Code Button", false, true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, verifyCodeButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, verifiedMFAPopUpLocBy);
                string verifiedPopupTextContent = webElementExtensions.GetElementText(_driver, verifiedMFAPopUpLocBy, true);
                if (_driver.Url.Contains("/contact-information"))
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.VerifiedMFAAuthenticationPopupOnContactInfoPageTextContent, verifiedPopupTextContent, "Verify verified Popup for the MFA Successful Authentication");
                else
                    ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.VerifiedMFAAuthenticationPopupTextContent, verifiedPopupTextContent, "Verify verified Popup for the MFA Successful Authentication");
                reportLogger.TakeScreenshot(test, "Verified! Popup");
                webElementExtensions.ActionClick(_driver, continueButtonLocBy, "Continue Button", isReportRequired: true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, editButtonOnContactInformationLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.ActionClick(_driver, editButtonOnContactInformationLocBy, "Edit button on Contact Information Page");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);

            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while Bypassing MFA authentication Error : <b>{e.Message}</b>");
            }
        }
        /// <summary>
        /// Verifies the tutorial link
        /// </summary>
        public void VerifyMobileTutorial()
        {
            bool isVideoLinkOpened = false;
            try
            {
                var tabsBefore = _driver.WindowHandles;
                var mainHandle = _driver.CurrentWindowHandle;
                webElementExtensions.ScrollIntoView(_driver, helpAndSupportLocBy);
                webElementExtensions.ClickElement(_driver, helpAndSupportLocBy);
                reportLogger.TakeScreenshot(test, $"Help and Support");
                webElementExtensions.ClickElementUsingJavascript(_driver, tutorialLinkLocBy);
                webElementExtensions.WaitForPageLoad(_driver);
                webElementExtensions.WaitForNewTabWindow(_driver, tabsBefore.Count + 1, ConfigSettings.LongWaitTime);
                var tabsAfter = _driver.WindowHandles;
                ReportingMethods.LogAssertionTrue(test, tabsAfter.Count > tabsBefore.Count, $"Tutorial Page should be opened in new tab");
                if (tabsAfter.Count > 0)
                {
                    foreach (var tab in tabsAfter)
                    {
                        try
                        {

                            if (tab != mainHandle)
                            {
                                webElementExtensions.SwitchToTab(_driver, tab);
                                webElementExtensions.WaitForPageLoad(_driver);
                                reportLogger.TakeScreenshot(test, $"Tutorial Page in new Tab");
                                ReportingMethods.LogAssertionContains(test, "https://share.vidyard.com/watch", _driver.Url, "Url should have https://share.vidyard.com/watch");
                                isVideoLinkOpened = true;
                                _driver.Close();
                            }
                        }
                        catch (Exception ex)
                        {
                            //Printing in logs is enough and no need to fail the test since somwtimes haidden tabs causingthe failure while switching
                            ReportingMethods.Log(test, $"Exception while switching {ex.Message}");
                        }
                    }
                    webElementExtensions.SwitchToTab(_driver, mainHandle);
                }
                ReportingMethods.LogAssertionTrue(test, isVideoLinkOpened, $"Tutorial Page should be Opened");

            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, $"Error");
                test.Log(Status.Error, $"Eror while checking Tutorial {e.Message}");
            }


        }

        /// <summary>
        /// Open the NAIHC Assisatnce in new tab and validates it's url
        /// </summary>
        public void VerifyNAIHC_Assistance()
        {

            var naihcDescription = webElementExtensions.GetElementText(_driver, naihcSecLocBy);
            ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.NAIHCDescription, naihcDescription, "NAIHC Descprition Comparison");

            VerifyLink(naihcLinkLocBy, "NAIHC", "https://naihc.net/tribal-housing-assistance-resource-hub");
        }


        /// <summary>
        /// Clicks the paperless settings link on dashboard page 
        /// </summary>
        public void ClickPaperlessSettings()
        {
            try
            {
                var item = _driver.FindElements(managePaperlessLinkLocBy).Where(e => e.Displayed).FirstOrDefault();
                if (item != null)
                {
                    webElementExtensions.ScrollIntoView(_driver, item);
                    reportLogger.TakeScreenshot(test, $"Dashboard : Manage Paperless Section");
                    var naihcDescription = webElementExtensions.GetElementText(_driver, paperlessDescriptionLocBy);
                    ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.PaperlessDashboardDescription, naihcDescription, "Paperless Descprition Comparison");
                    webElementExtensions.ClickElementUsingJavascript(_driver, item);
                    webElementExtensions.WaitForPageLoad(_driver);
                    webElementExtensions.WaitUntilUrlContains("manage-paperless");

                }
            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, $"Error");
                test.Log(Status.Error, $"Eror while checking Manmage Paperless , {e.Message}");
            }
        }

        /// <summary>
        /// Verify the given link in page with repsective url open in new tab
        /// </summary>
        /// <param name="linkItem">link control</param>
        /// <param name="linkName">Name of the Link</param>
        /// <param name="url">expected url</param>
        public void VerifyLink(By linkItem, string linkName, string url)
        {
            bool isPageOpened = false;
            try
            {
                var item = _driver.FindElements(linkItem).Where(e => e.Displayed).FirstOrDefault();
                if (item != null)
                {
                    var tabsBefore = _driver.WindowHandles;
                    var mainHandle = _driver.CurrentWindowHandle;
                    //webElementExtensions.ScrollIntoView(_driver, item);
                    webElementExtensions.MoveToElement(_driver, item);
                    reportLogger.TakeScreenshot(test, $"Dashboard : {linkName}");
                    webElementExtensions.ClickElementUsingJavascript(_driver, item);
                    webElementExtensions.WaitForPageLoad(_driver);
                    webElementExtensions.WaitForNewTabWindow(_driver, tabsBefore.Count + 1, ConfigSettings.LongWaitTime);
                    var tabsAfter = _driver.WindowHandles;
                    ReportingMethods.LogAssertionTrue(test, tabsAfter.Count > tabsBefore.Count, $"{linkName} should be opened in new tab");
                    if (tabsAfter.Count > 0)
                    {
                        foreach (var tab in tabsAfter)
                        {
                            try
                            {

                                if (tab != mainHandle)
                                {
                                    webElementExtensions.SwitchToTab(_driver, tab);
                                    webElementExtensions.WaitForPageLoad(_driver);
                                    isPageOpened = true;
                                    reportLogger.TakeScreenshot(test, $"{linkName} Page in new Tab");
                                    ReportingMethods.LogAssertionContains(test, url, _driver.Url, $"{linkName} Url should be {url}");
                                    _driver.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                //Printing in logs is enough and no need to fail the test since somwtimes haidden tabs causingthe failure while switching
                                ReportingMethods.Log(test, $"Exception while switching {ex.Message}");
                            }
                        }
                        webElementExtensions.SwitchToTab(_driver, mainHandle);
                    }
                    ReportingMethods.LogAssertionTrue(test, isPageOpened, $"{linkName} Page should be Opened");

                }
            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, $"Error");
                test.Log(Status.Error, $"Eror while checking {linkName} , {e.Message}");
            }
        }

        /// <summary>
        /// To Validate the Manage Autopay Badge
        /// </summary>
        /// <param name="expectedStatus"> "On Autopay"/"Off Autopay"</param>
        public void ValidateAutopayBadge(string expectedStatus)
        {
            webElementExtensions.ActionClick(_driver, loanDepotLogoLocBy, "Loan Depot Logo");
            HandlePaperLessPage();
            webElementExtensions.WaitUntilUrlContains("/dashboard");

            if (webElementExtensions.VerifyElementAttributeValue(_driver, manageAutopayOnOffButtonLocBy, "alt", expectedStatus, true))
            {
                test.Pass($"Manage Autopay Badge is {expectedStatus}");
            }
            else
            {
                test.Fail($"Manage Autopay Badge is NOT {expectedStatus}");
            }
        }

        /// <summary>
        /// To Cancel the Loan Link Popup
        /// </summary>
        public void CancelLoanLinkPopup()
        {
            webElementExtensions.ScrollIntoView(_driver, cancelButtonLinkMyLoanPopupLocBy);
            webElementExtensions.ClickElement(_driver, cancelButtonLinkMyLoanPopupLocBy);
        }

        /// <summary>
        /// Verify Advance Request from Line of Credit Text content when for Active/Inactive Heloc Draw
        /// </summary>
        /// <param name="isRequesFundsButtonEnabled">bool true/false</param>
        public void VerifyAdvanceRequestFromLineOfCreditTextContent(bool isRequesFundsButtonEnabled)
        {
            try
            {
                reportLogger.TakeScreenshot(test, "Dashboard Page");
                webElementExtensions.WaitForVisibilityOfElement(_driver, requestFundsAdvanceRequestFromLineOfCreditTextContentLocBy, ConfigSettings.SmallWaitTime);
                string actTextContent = webElementExtensions.GetElementText(_driver, requestFundsAdvanceRequestFromLineOfCreditTextContentLocBy);
                if (isRequesFundsButtonEnabled)
                    ReportingMethods.LogAssertionEqual(test, "Advance Request from Line of Credit\r\nRequest Funds", actTextContent, "Verify Advance Request from Line of Credit Text content when for Active Heloc Draw");
                else
                    ReportingMethods.LogAssertionEqual(test, "", actTextContent, "Verify Advance Request from Line of Credit Text content when for Inactive Heloc Draw");
            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, $"Error");
                test.Log(Status.Error, $"Eror while checking Verifying the Advance Request from Line of Credit Text content , {e.Message}");
            }
        }

        /// <summary>
        /// To handle / verify Login MFA Verification Popup
        /// </summary>
        /// <param name="loanNumber">"1040258327"</param>
        /// <param name="isRememberMyDeviceCheckRequired">true / false</param>
        public void HandleLoginMFA(string loanNumber, bool isRememberMyDeviceCheckRequired = false)
        {
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForStalenessOfElement(_driver, loginMFAPopUpLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForElement(_driver, loginMFAPopUpLocBy, ConfigSettings.WaitTime);

                string mfaPopUpBeforeRequestingVerificationCodeTextContent = webElementExtensions.GetElementText(_driver, loginMFAPopUpLocBy, true);

                webElementExtensions.ActionClick(_driver, phoneSmsDropdownSelectLoginMFAPopUpLocBy, "Phone Number Dropdown for Receiving code using Phone Sms", isReportRequired: true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, selectPhoneSmsOptionLoginMFAPopUpLocBy);
                string phoneNumbersDropdownForCodeViaPhoneSms = webElementExtensions.GetElementText(_driver, selectPhoneSmsOptionLoginMFAPopUpLocBy, true);
                ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.LoginMFAPopPleaseSelectPhoneNumberInfoText, phoneNumbersDropdownForCodeViaPhoneSms, "Phone Number Dropdown for Receiving code using Phone Sms Info Text");
                ReportingMethods.LogAssertionContains(test, "***-***-1111", phoneNumbersDropdownForCodeViaPhoneSms, "Phone Number Dropdown for Receiving code using Phone Sms Phone Number Option Text");

                webElementExtensions.ActionClick(_driver, emailDropdownSelectLoginMFAPopUpLocBy, "Email Options Drop down");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, selectPhoneSmsOptionLoginMFAPopUpLocBy);
                webElementExtensions.ActionClick(_driver, phoneCallDropdownSelectLoginMFAPopUpLocBy, "Phone Number Dropdown for Receiving code using Phone Call", isReportRequired: true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, selectPhoneSmsOptionLoginMFAPopUpLocBy);
                string phoneNumbersDropdownForCodeViaPhoneCall = webElementExtensions.GetElementText(_driver, selectPhoneCallOptionLoginMFAPopUpLocBy, true);
                ReportingMethods.LogAssertionContains(test, "***-***-1111", phoneNumbersDropdownForCodeViaPhoneCall, "Phone Number Dropdown for Receiving code using Phone Call Phone Number Option Text");

                reportLogger.TakeScreenshot(test, "Login MFA Popup Before sending verification code text content");
                ReportingMethods.LogAssertionEqual(test, mfaPopUpBeforeRequestingVerificationCodeTextContent, Constants.CustomerPortalTextMessages.LoginMFAPopUpBeforeRequestingCodeTextContent, "Login MFA Popup Before sending verification code text content");

                webElementExtensions.ActionClick(_driver, emailDropdownSelectLoginMFAPopUpLocBy, "Email Options Drop down");
                webElementExtensions.ActionClick(_driver, emailDropdownSelectLoginMFAPopUpLocBy, "Email Options Drop down", isReportRequired: true);
                webElementExtensions.ActionClick(_driver, selectEmailOptionLoginMFAPopUpLocBy, "Email Option", isReportRequired: true);


                webElementExtensions.WaitForElementToBeEnabled(_driver, receiveCodeViaEmailButtonLoginMFAPopUpLocBy, "Receive Code Via Email Button", ConfigSettings.SmallWaitTime);
                webElementExtensions.ActionClick(_driver, receiveCodeViaEmailButtonLoginMFAPopUpLocBy, "Receive Code Via Email Button", true);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, receiveCodeViaEmailButtonLoginMFAPopUpLocBy, ConfigSettings.MediumWaitTime);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);

                string mfaPopUpAfterRequestingVerificationCodeTextContent = webElementExtensions.GetElementText(_driver, loginMFAPopUpLocBy, true);
                reportLogger.TakeScreenshot(test, "Login MFA after sending verification code");
                ReportingMethods.LogAssertionEqual(test, mfaPopUpAfterRequestingVerificationCodeTextContent, Constants.CustomerPortalTextMessages.LoginMFAPopUpAfterRequestingCodeTextContent, "Login MFA after sending verification code");
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.WaitForVisibilityOfElement(_driver, verificationOTPInputFieldLocBy, ConfigSettings.SmallWaitTime);
                webElementExtensions.EnterText(_driver, verificationOTPInputFieldLocBy, Constants.MFAByPassCode.ByPassMFACode);
                webElementExtensions.WaitForElementToBeEnabled(_driver, verifyCodeButtonLocBy, "Verify Button", isScrollIntoViewRequired: true);
                if (isRememberMyDeviceCheckRequired)
                    webElementExtensions.ActionClick(_driver, rememberMeCheckBoxLoginMFAPopUpLocBy, "Remember My Device Checkbox", isReportRequired: true);

                webElementExtensions.ActionClick(_driver, verifyCodeButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, verifyCodeButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy, ConfigSettings.SmallWaitTime);
                ReportingMethods.Log(test, $"MFA verification handled successfully for Loan Number: {loanNumber}");
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while Bypassing MFA authentication Error : <b>{e.Message}</b>");
            }
        }
    }
    #endregion Services
}

