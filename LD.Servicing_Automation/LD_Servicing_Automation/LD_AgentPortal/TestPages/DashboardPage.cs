using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using log4net;
using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static LD_AutomationFramework.Constants;

namespace LD_AgentPortal.Pages
{
    public class DashboardPage
    {
        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }

        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;

        PaymentsPage payments = null;
        public List<string> loanNumbersInUse = new List<string>();

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DashboardPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            payments = new PaymentsPage(_driver, test);
        }

        #region Locators

        #region LoanSearchPageLocators

        public By toolsAndResourcesHeaderLocBy = By.XPath("//mello-serve-ui-nav//h3[text()='Tools and Resources']");

        public By toolsAndResourcesMenuListLocBy = By.CssSelector("mello-serve-ui-nav a span");

        public string toolsAndResourcesMenuItemLocBy = "//mello-serve-ui-nav//mat-list-item[<MENUNUMBER>]//a//span";

        public string toolsAndResourcesMenuListHyperlinkLocBy = "//span[text()='<MENUNAME>']//parent::a";

        public By userNameFieldInAiqPageLocBy = By.Name("userId");

        public By menuBarInProctorPageLocBy = By.Id("navbarSupportedContent");

        public By melloLogoInWorkdayPageLocBy = By.Id("bannerLogo");

        public By enterLoanNumberInputFieldLocBy = By.XPath("//input[contains(@data-placeholder,'Enter Loan Number')]");

        public By verifyCallerButtonLocBy = By.Id("btnVerifyCaller");

        public By viewAccountButtonLocBy = By.Id("btnViewAccount");

        public By verifyCallerPopUpCloseIconLocBy = By.Id("btnIdClose");

        public By firstNameInAdvSearchLocBy = By.CssSelector("input[formcontrolname='firstName']");

        public By lastNameInAdvSearchLocBy = By.CssSelector("input[formcontrolname='lastName']");

        public By phoneInAdvSearchLocBy = By.CssSelector("input[formcontrolname='phoneNumber']");

        public By emailAddressInAdvSearchLocBy = By.CssSelector("input[formcontrolname='email']");

        public By last4SsnInAdvSearchLocBy = By.CssSelector("input[formcontrolname='ssn']");

        public By streetAddress_PropertyAddInAdvSearchLocBy = By.CssSelector("input[formcontrolname='propertyAddress']");

        public By aptUnitNumber_PropertyAddInAdvSearchLocBy = By.CssSelector("input[formcontrolname='propertyUnitNumber']");

        public By city_PropertyAddInAdvSearchLocBy = By.CssSelector("input[formcontrolname='propertyCity']");

        public By state_PropertyAddInAdvSearchLocBy = By.CssSelector("input[formcontrolname='propertyState']");

        public By zipCode_PropertyAddInAdvSearchLocBy = By.CssSelector("input[formcontrolname='propertyZipCode']");

        public By streetAddress_MailingAddInAdvSearchLocBy = By.CssSelector("input[formcontrolname='mailingAddress']");

        public By aptUnitNumber_MailingAddInAdvSearchLocBy = By.CssSelector("input[formcontrolname='mailingUnitNumber']");

        public By city_MailingAddInAdvSearchLocBy = By.CssSelector("input[formcontrolname='mailingCity']");

        public By state_MailingAddInAdvSearchLocBy = By.CssSelector("input[formcontrolname='mailingState']");

        public By zipCode_MailingAddInAdvSearchLocBy = By.CssSelector("input[formcontrolname='mailingZipCode']");

        public By resetFormButtonLocBy = By.XPath("//span[text()='Reset Form']");

        public By searchForCustomerButtonLocBy = By.XPath("//button[not(@disabled)]//span[contains(text(),'Search for Customer')]");

        public By searchForCustomerButtonDisabledLocBy = By.XPath("//button[@disabled]//span[contains(text(),'Search for Customer')]");

        public By loanNumberInSearchResultsPopupLocBy = By.XPath("//mello-serve-ui-advanced-search-result//span[contains(text(),'Loan')]");

        public By viewAccountButtonInSearchResultsPopupLocBy = By.XPath("//button[contains(text(),'View Account')]");

        public By verifyCallerButtonInSearchResultsPopupLocBy = By.XPath("//button[contains(text(),'Verify Caller')]");

        #endregion LoanSearchPageLocators

        #region DashboardCommonLocators        

        public By uiBannersInDashboardLocBy = By.CssSelector("mello-serve-ui-banners");

        public string apNavigationTabsLocBy = "//div[@class='mat-tab-label-content' and text()='<TABNAME>']";

        public By backToSearchButtonLocBy = By.Id("cancel-close-link");

        public By timeZoneValueInBorrowersLocalTimeLocBy = By.XPath("//mello-serve-ui-loan-info-header//div[contains(@class,'col')][2]//div[4]");

        #endregion DashboardCommonLocators

        #region LoanSummaryTab

        public By loanSummaryHeaderLabelLocBy = By.Id("spidPaymentActivity");

        public By loanNumberInLoanSummaryTabLocBy = By.XPath("//div[text()='Loan Number']//following-sibling::h6");

        public By propertyAddressLocBy = By.XPath("//h6[text()='Property Address']//following-sibling::div[1]//h6");

        public string loanSummaryDetailsCommonLocatorLocBy = "//div[text()='<FIELDNAME>']//following-sibling::h6";

        #endregion LoanSummaryTab

        #region NotesSection

        public By notesSectionFirstMessageLocBy = By.XPath("//tr[1]//div[@class='notes_dv']");

        #endregion NotesSection

        #endregion Locators

        #region Services

        /// <summary>
        /// Method to perform quick search with a specific loan number in landing page
        /// </summary>
        /// <param name="loanLevelData">List of hashtables with loan level data from DB</param>
        /// <param name="rowNumber">DB table row number of the loan number selected from hashtable</param>
        /// <param name="isReportRequired">true/false</param>
        /// <returns></returns>
        public bool QuickSearchForLoanNumber(List<Hashtable> loanLevelData, int rowNumber = 0, bool isReportRequired = false)
        {
            bool flag = false, alreadyUsedLoan = false;
            string url = string.Empty, loanNumber = string.Empty;
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.oopsSomethingWentWrongPopupLocBy))
                {
                    webElementExtensions.ActionClick(_driver, commonServices.oopsSomethingWentWrongPopupLocBy);
                }
                loanNumber = loanLevelData[rowNumber][Constants.LoanLevelDataColumns.LoanNumber].ToString();

                alreadyUsedLoan = loanNumbersInUse.Contains(loanNumber);
                if (!string.IsNullOrEmpty(loanNumber) && !alreadyUsedLoan)
                {
                    webElementExtensions.EnterText(_driver, enterLoanNumberInputFieldLocBy, loanNumber, true, "Loan Number search", true);
                    webElementExtensions.ScrollIntoView(_driver, verifyCallerButtonLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, verifyCallerButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, viewAccountButtonLocBy);
                    webElementExtensions.ActionClick(_driver, verifyCallerButtonLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, verifyCallerPopUpCloseIconLocBy);

                    //Close the Verify caller pop up
                    webElementExtensions.ClickElement(_driver, verifyCallerPopUpCloseIconLocBy, "Verify Caller pop up Close icon", true);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, verifyCallerPopUpCloseIconLocBy);
                    webElementExtensions.WaitForElement(_driver, backToSearchButtonLocBy);
                    webElementExtensions.WaitForElement(_driver, loanSummaryHeaderLabelLocBy);

                    loanNumbersInUse.Add(loanNumber);
                    webElementExtensions.WaitForPageLoad(_driver);
                    if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.oopsSomethingWentWrongPopupLocBy))
                    {
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                        webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                        _driver.Navigate().Refresh();
                    }
                    webElementExtensions.WaitForElement(_driver, timeZoneValueInBorrowersLocalTimeLocBy);
                    webElementExtensions.WaitForVisibilityOfElement(_driver, timeZoneValueInBorrowersLocalTimeLocBy);
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, timeZoneValueInBorrowersLocalTimeLocBy))
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                    flag = webElementExtensions.IsElementDisplayedBasedOnCount(_driver, timeZoneValueInBorrowersLocalTimeLocBy);
                    webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while searching for the loan number: " + ex.Message);
            }
            if (isReportRequired)
            {
                if (alreadyUsedLoan)
                    test.Log(Status.Info, "The selected loan number - " + loanNumber + " is already in use.");
                else
                    _driver.ReportResult(test, flag, "Successfully searched for the loan number - " + loanNumber + ".", "Failed while searching for the loan number - " + loanNumber + ".");
            }
            return flag;
        }

        /// <summary>
        /// Method to navigate to any tab
        /// </summary>
        /// <param name="tabName">Payments/Loan Summary</param>
        public void NavigateToTab(string tabName)
        {
            bool flag = false;
            try
            {
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, commonServices.oopsSomethingWentWrongPopupLocBy))
                {
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Tab);
                    webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);
                    _driver.Navigate().Refresh();
                }
                By tabLocBy = By.XPath(apNavigationTabsLocBy.Replace("<TABNAME>", tabName));
                webElementExtensions.WaitForElement(_driver, tabLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                if (!webElementExtensions.VerifyElementAttributeValue(_driver, tabLocBy, "aria-selected", "true"))
                {
                    webElementExtensions.MoveToElement(_driver, tabLocBy);
                    webElementExtensions.ScrollIntoView(_driver, tabLocBy);
                    webElementExtensions.ClickElementUsingJavascript(_driver, tabLocBy);
                }
                flag = true;
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
            }
            catch (Exception ex)
            {
                log.Error("Failed while navigating to tab: " + ex.Message);
            }
            _driver.ReportResult(test, flag, "Successfully navigated to " + tabName + " tab.", "Failed while navigating to " + tabName + " tab.");
        }

        /// <summary>
        /// Method to verify if the loan number in application is eligible for autopay or not
        /// </summary>
        /// <param name="isReportRequired">true/false</param>
        /// <returns></returns>
        public bool VerifyIfLoanNumberIsEligibleForAutopay(bool isReportRequired = false)
        {
            bool flag = false;
            int count = 0;
            try
            {
                #region CommentedCode
                //--------Below lines are commented due to a functionality change. After confirmation, the code will be removed---------
                ////Enter the loan number in the Loan Search input field
                //webElementExtensions.EnterText(_driver, dashboard.enterLoanNumberInputFieldLocBy, loanNumber, true, "Loan Number search.");
                //webElementExtensions.ScrollIntoView(_driver, dashboard.verifyCallerButtonLocBy);
                //webElementExtensions.WaitForElement(_driver, dashboard.viewAccountButtonLocBy);
                //webElementExtensions.SendKeyStrokesFromKeyboard(_driver, Keys.Enter);

                ////Close the Verify caller pop up
                //webElementExtensions.ClickElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy, "Verify Caller pop up Close icon.");
                //webElementExtensions.WaitForInvisibilityOfElement(_driver, dashboard.verifyCallerPopUpCloseIconLocBy);
                //webElementExtensions.WaitForElement(_driver, dashboard.backToSearchButtonLocBy);
                //webElementExtensions.WaitForElement(_driver, dashboard.loanSummaryHeaderLabelLocBy);
                #endregion CommentedCode

                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                count = _driver.FindElements(uiBannersInDashboardLocBy).Count();
                if (count > 0)
                    webElementExtensions.WaitForElement(_driver, uiBannersInDashboardLocBy);
                commonServices.NavigateToTabInAgentPortal(Constants.AgentPortalTabNames.PaymentsTab, false);
                webElementExtensions.WaitForElement(_driver, payments.manageAutopayButtonLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.manageAutopayButtonLocBy))
                {
                    //Condition check 1 - Manage Pay button is enabled or not
                    if (!webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.disabledManageAutopayButtonLocBy))
                    {
                        ////Condition check 2 - Whether autopay was setup recently or not 
                        webElementExtensions.ActionClick(_driver, payments.manageAutopayButtonLocBy, "Manage Autopay button.");
                        webElementExtensions.WaitForElement(_driver, payments.goBackToPaymentsLinkLocBy);
                        webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                        if (webElementExtensions.VerifyElementAttributeValue(_driver, payments.manageAutopayGridLocBy, "id", "showStartPaymentButton"))
                            flag = true;
                        if (webElementExtensions.IsElementDisplayedBasedOnCount(_driver, payments.goBackToPaymentsLinkLocBy))
                            webElementExtensions.ActionClick(_driver, payments.goBackToPaymentsLinkLocBy);
                    }
                }
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying if loan number is eligible for Autopay: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully verified that the loan number is eligible for setting up autopay.", "The loan number is not eligible for setting up autopay.");
            return flag;
        }

        /// <summary>
        /// Method to verify the loan summary section data
        /// </summary>
        /// <param name="loanSummaryDetails">List data in the format - "Loan Number|1022533665", "Type of Mortgage|Conventional"</param>
        public void VerifyLoanSummaryDetails(List<string> loanSummaryDetails)
        {
            string valueFromDb = string.Empty, actualValue = string.Empty, fieldName = string.Empty;
            try
            {
                if (loanSummaryDetails != null)
                {
                    test.Log(Status.Info, "***** Verification of Loan Summary section data *****");
                    foreach (string data in loanSummaryDetails)
                    {
                        fieldName = data.Split('|')[0];
                        valueFromDb = data.Split('|')[1];
                        if (fieldName.Equals("Property Address"))
                            actualValue = webElementExtensions.GetElementText(_driver, propertyAddressLocBy);
                        else if (fieldName.Equals("Interest Rate"))
                        {
                            actualValue = webElementExtensions.GetElementText(_driver, By.XPath(loanSummaryDetailsCommonLocatorLocBy.Replace("<FIELDNAME>", fieldName))).Replace("%", "");
                            actualValue = (Convert.ToDecimal(actualValue) / 100).ToString("F7");
                        }
                        else if (fieldName.Equals("Principal & Interest") || fieldName.Equals("Taxes & Insurance") || fieldName.Equals("Total Monthly PITI") || fieldName.Equals("Escrow Balance") || fieldName.Equals("Suspense Balance") || fieldName.Equals("Res Escrow Balance") || fieldName.Equals("Corp Adv Balance"))
                        {
                            actualValue = webElementExtensions.GetElementText(_driver, By.XPath(loanSummaryDetailsCommonLocatorLocBy.Replace("<FIELDNAME>", fieldName)));
                            valueFromDb = "$" + Convert.ToDouble(valueFromDb).ToString("N");
                        }
                        else
                            actualValue = webElementExtensions.GetElementText(_driver, By.XPath(loanSummaryDetailsCommonLocatorLocBy.Replace("<FIELDNAME>", fieldName)));

                        if (valueFromDb.Equals(actualValue))
                            _driver.ReportResult(test, true, "Verification Success for " + fieldName + ". Actual value - " + actualValue + ", DB value - " + valueFromDb + ".", "");
                        else
                            _driver.ReportResult(test, false, "", "Verification failed for " + fieldName + ". Actual value - " + actualValue + ", DB value - " + valueFromDb + ".");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying loan summary details -" + ex.Message);
            }

        }

        #endregion Services

        /// <summary>
        /// Verifies if the loan number is eligible for defaults in the past months.
        /// </summary>
        /// <param name="defaultsPlan">Defaults plan - Forberance, Repayments</param>
        /// <param name="isReportRequired">Optional parameter to indicate if a report is required. Default value is false.</param>
        /// <returns>True if the loan number is eligible for defaults, otherwise false.</returns>
        public bool VerifyIfLoanNumberIsEligibleForDefaultsPastMonths(string defaultsPlan, bool isReportRequired = false)
        {
            bool flag = false;
            int count = 0;
            try
            {

                count = _driver.FindElements(uiBannersInDashboardLocBy).Count();
                if (count > 0)
                    webElementExtensions.WaitForElement(_driver, uiBannersInDashboardLocBy);
                By paymentsTabLocBy = By.XPath(apNavigationTabsLocBy.Replace("<TABNAME>", Constants.AgentPortalTabNames.PaymentsTab));
                webElementExtensions.WaitForVisibilityOfElement(_driver, paymentsTabLocBy);
                if (!webElementExtensions.VerifyElementAttributeValue(_driver, paymentsTabLocBy, "aria-selected", "true"))
                    webElementExtensions.ActionClick(_driver, paymentsTabLocBy, "Payments tab.");
                webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                if (_driver.FindElement(payments.makeAPaymentButtonLocBy).GetCssValue("background-color").ToString().Trim() == Constants.Colors.Orange)
                    flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying if loan number is eligible for " + defaultsPlan + ": " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully verified that the loan number in application is eligible for " + defaultsPlan + ".", "Failed while verifying that the loan number in application is eligible for " + defaultsPlan + ".");
            return flag;
        }

        /// <summary>
        /// Verifies if the loan number is eligible for the current month.
        /// </summary>
        /// <param name="isReportRequired">Optional parameter to indicate if a report is required. Default value is false.</param>
        /// <returns>True if the loan number is eligible, otherwise false.</returns>
        public bool VerifyIfLoanNumberIsEligibleForCurrentMonth(bool isReportRequired = false)
        {
            bool flag = false;
            int count = 0;
            try
            {

                count = _driver.FindElements(uiBannersInDashboardLocBy).Count();
                if (count > 0)
                    webElementExtensions.WaitForElement(_driver, uiBannersInDashboardLocBy);
                By paymentsTabLocBy = By.XPath(apNavigationTabsLocBy.Replace("<TABNAME>", Constants.AgentPortalTabNames.PaymentsTab));
                webElementExtensions.WaitForElement(_driver, paymentsTabLocBy);
                if (!webElementExtensions.VerifyElementAttributeValue(_driver, paymentsTabLocBy, "aria-selected", "true"))
                    webElementExtensions.ActionClick(_driver, paymentsTabLocBy, "Payments tab.");
                webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                if (_driver.FindElement(payments.makeAPaymentButtonLocBy).GetCssValue("background-color").ToString().Trim() == Constants.Colors.Purple)
                    flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying if loan number is eligible: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully verified that the loan number in application is eligible for Forbearance.", "Failed while verifying that the loan number in application is eligible for Forbearance.");
            return flag;
        }

        /// <summary>
        /// Verifies if the loan number is eligible for OTP
        /// </summary>
        /// <param name="isReportRequired">Optional parameter to indicate if a report is required. Default value is false.</param>
        /// <returns>True if the loan number is eligible, otherwise false.</returns>
        public bool VerifyIfLoanNumberIsEligibleForOTP(bool isReportRequired = false)
        {
            bool flag = false;
            int count = 0;
            try
            {

                count = _driver.FindElements(uiBannersInDashboardLocBy).Count();
                if (count > 0)
                    webElementExtensions.WaitForElement(_driver, uiBannersInDashboardLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.loadingIconLocBy);
                By paymentsTabLocBy = By.XPath(apNavigationTabsLocBy.Replace("<TABNAME>", Constants.AgentPortalTabNames.PaymentsTab));
                webElementExtensions.WaitForElement(_driver, paymentsTabLocBy);
                if (!webElementExtensions.VerifyElementAttributeValue(_driver, paymentsTabLocBy, "aria-selected", "true"))
                    webElementExtensions.ActionClick(_driver, paymentsTabLocBy, "Payments tab.");
                webElementExtensions.WaitForElement(_driver, payments.makeAPaymentButtonLocBy);
                webElementExtensions.ScrollIntoView(_driver, payments.makeAPaymentButtonLocBy);
                if (_driver.FindElement(payments.makeAPaymentButtonLocBy).GetCssValue("background-color").ToString().Trim() == Constants.Colors.Purple)
                    flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying if loan number is eligible: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully verified that the loan number in application is eligible.", "Failed while verifying that the loan number in application is eligible.");
            return flag;
        }

        /// <summary>
        /// Method to verify messages in Notes section of Dashboard page
        /// </summary>
        /// <param name="dateTimeUtc">Pass the utc date time value captured at the time of payment addition/edit/delete</param>
        /// <param name="type">add/edit/delete. Based on this functionality, messages change</param>
        /// <param name="confirmationNumber">Example - 6869290CHP</param>
        /// <param name="paymentDate">OTP screen payment date entered</param>
        /// <param name="paymentAmount">OTP screen payment amount entered</param>
        /// <param name="accountNumber">Account number details</param>
        /// <param name="userName">login username which would be the same value as agent name</param>
        /// <param name="borrowerName">if type = delete, pass borrower name</param>
        /// <param name="deleteReason">if type = delete, pass payment delete reason</param>
        public void VerifyMessagesInNotesSectionOfDashboardPage(DateTime dateTimeUtc, string type = "add", string confirmationNumber = null, string paymentDate = null, string paymentAmount = null, string accountNumber = null, string userName = null, string borrowerName = null, string deleteReason = null)
        {
            string expectedNote = string.Empty, accountNum = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(accountNumber))
                    accountNum = accountNumber.Substring(accountNumber.Length - 4);                
                string agentName = userName.Split('@')[0].ToUpper().Replace('_', ' ');
                
                if (type.ToLower().Equals("add"))
                {
                    DateTime currentCSTDateTime = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUtc, TimeZoneInfo.FindSystemTimeZoneById(TimeZones.CentralStandardTime));
                    string formattedCurrentCSTDateTime = currentCSTDateTime.ToString("MM/dd/yyyy hh:mmtt");
                    string date = Convert.ToDateTime(paymentDate).ToString("MM/dd/yyyy");
                    expectedNote = $"AGENT OTP PAYMENT TRACKING: {confirmationNumber} POST {date} AMOUNT {paymentAmount} BANK INFO - LOAN DEPOT BANK ******{accountNum} TAKEN AT {formattedCurrentCSTDateTime} CST BY AGENT {agentName}";
                }
                else if (type.ToLower().Equals("delete"))
                    expectedNote = $"ONETIME PAYMENT CONFIRMATION # {confirmationNumber} DRAFT AMOUNT {paymentAmount} CANCELLED BY AGENT: {agentName} REQUESTED BY {borrowerName.ToUpper()} REASON: {deleteReason.ToUpper()}";
                
                string actualNote = _driver.FindElement(notesSectionFirstMessageLocBy).Text;
                actualNote = Regex.Replace(actualNote, @"\s+", " ").Trim();
                ReportingMethods.LogAssertionEqual(test, expectedNote, actualNote, "Verify newly added message in Notes section");
            }
            catch(Exception ex)
            {
                log.Error("Failed while verifing messages in Notes section:" + ex.Message);
            }
        }
    }
}
