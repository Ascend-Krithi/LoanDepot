using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using log4net;
using OpenQA.Selenium;
using System;
using System.Linq;

namespace LD_CustomerPortal.TestPages
{


    /// <summary>
    /// Class to handle all Payoff Quotes functionality
    /// </summary>
    public class PaperlessSettingsPage
    {

        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }
        ReportLogger reportLogger { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;
        public PaperlessSettingsPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            reportLogger = new ReportLogger(_driver);
        }

        #region Locators
        public By pageTitleLocBy = By.XPath("//div[contains(text(),'Manage Paperless')]");
        public By toggleSelectAllLocBy = By.XPath("//*[@formcontrolname='isSelectAll']//*[@type='checkbox']");
        public By toggleYearEndLocBy = By.XPath("//*[@formcontrolname='isTaxPaperless']//*[@type='checkbox']");
        public By toggleBillingLocBy = By.XPath("//*[@formcontrolname='isBillingPaperless']//*[@type='checkbox']");
        public By toggleEscrowLocBy = By.XPath("//*[@formcontrolname='isEscrowPaperless']//*[@type='checkbox']");
        public By toggleOtherLettersLocBy = By.XPath("//*[@formcontrolname='isOtherLettersPaperless']//*[@type='checkbox']");
        public By allTogglesLocBy = By.XPath("//*[@type='checkbox']");
        public By btnSaveChangesLocBy = By.Id("saveChangesPaperlessBtn");
        public By paperlessDescriptionLocBy = By.XPath("//div[contains(text(),'paperless settings may take up to 24 hours')]");
        public By errMsgLocBy = By.XPath("//*[contains(@id,'mat-dialog')]//*[contains(text(),'Please try again later')]");
        public By toggleDescriptionLocBy = By.XPath("//div[contains(text(),'toggles to select the documents')]");
        public By dialog = By.XPath("//*[contains(@id,'mat-dialog')]");
        public By successTitleLocBy = By.XPath("//*[contains(@id,'mat-dialog')]//div[@class='h5']");
        public By successDesclaimer = By.XPath("//*[contains(@id,'mat-dialog')]//div[contains(text(),'paperless settings may take up to 24 hours')]");
        public By btnSubmitLocBy = By.Id("btnSubmit");
        public By termConditionsLocBy = By.Id("mat-checkbox-1-input");
        public By btnOkLocBy = By.XPath("//*[contains(@id,'mat-dialog')]//button[contains(.,'Ok')]");
        public By btnCloseLocBy = By.Id("btnClose");


        #endregion

        #region methods
        /// <summary>
        /// Verify select toggle functionality         
        /// </summary>
        public void VerifySelectAllButtonFunctionality()
        {
            try
            {
                var isSelected = _driver.FindElement(toggleSelectAllLocBy).Selected;
                webElementExtensions.ClickElementUsingJavascript(_driver, toggleSelectAllLocBy);
                webElementExtensions.WaitForPageLoad(_driver);
                var allStatus = _driver.FindElements(allTogglesLocBy)
                .ToList().TrueForAll(e => e.Selected == !isSelected);
                reportLogger.TakeScreenshot(test, $"Select All : {!isSelected}");
                isSelected = _driver.FindElement(toggleSelectAllLocBy).Selected;
                ReportingMethods.LogAssertionTrue(test, allStatus, "Selecting/Deselecting All should select/deselect all other options");
                if (!isSelected)
                {
                    webElementExtensions.ClickElementUsingJavascript(_driver, toggleYearEndLocBy);
                }

                var isEnabled = !webElementExtensions.IsElementDisabled(_driver, btnSaveChangesLocBy);
                ReportingMethods.LogAssertionTrue(test, isEnabled, "Save Chnages Button Should be enabled after changes");
            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, $"Error");
                test.Log(Status.Error, $"Eror while performing action, {e.Message}");
            }

        }
        /// <summary>
        /// Checks all downstrea,m steps from Save 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public bool VerifySaveChanges()
        {
            bool isSaveSuccessful = false;
            try
            {
                //Save Button Click
                webElementExtensions.ClickElementUsingJavascript(_driver, btnSaveChangesLocBy);
                //Term and Conditions Dialog
                webElementExtensions.WaitForVisibilityOfElement(_driver, dialog);
                webElementExtensions.WaitForVisibilityOfElement(_driver, termConditionsLocBy);
                reportLogger.TakeScreenshot(test, $"Dialog After Save");
                var isSubmitEnabled = webElementExtensions.IsElementEnabled(_driver, btnSubmitLocBy);
                ReportingMethods.LogAssertionFalse(test, isSubmitEnabled, "Submit Button Should be disabled default");
                webElementExtensions.ClickElementUsingJavascript(_driver, termConditionsLocBy);
                webElementExtensions.WaitForElementToBeEnabled(_driver, btnSubmitLocBy);
                isSubmitEnabled = webElementExtensions.IsElementEnabled(_driver, btnSubmitLocBy);
                ReportingMethods.LogAssertionTrue(test, isSubmitEnabled, "Submit Button Should be Enabled");
                reportLogger.TakeScreenshot(test, $"Select Terms");
                webElementExtensions.ClickElement(_driver, btnSubmitLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, btnSubmitLocBy);
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitForPageLoad(_driver);
                //Success Confirmation Dialog
                webElementExtensions.WaitUntilElementIsClickable(_driver, btnCloseLocBy);
                reportLogger.TakeScreenshot(test, $"Confirmation Dialog");
                var title = webElementExtensions.GetElementText(_driver, successTitleLocBy);
                if (title.Contains("Changes Successfully Saved"))
                {
                    ReportingMethods.LogAssertionContains(test, "Changes Successfully Saved", title, "Tile should contain Success");
                    var desclaimer = webElementExtensions.GetElementText(_driver, successDesclaimer);
                    ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalTextMessages.PaperlessDescription, desclaimer, "Verify Disclaimer");
                    webElementExtensions.ClickElement(_driver, btnCloseLocBy);
                    reportLogger.TakeScreenshot(test, $"Close Success Dialog");
                    isSaveSuccessful = true;
                }
                else
                {
                    var errMsg = webElementExtensions.GetElementText(_driver, errMsgLocBy);
                    ReportingMethods.LogAssertionContains(test, Constants.CustomerPortalErrorMsgs.PaperlessErrorMessage, errMsg, "Verify Error Message");
                    webElementExtensions.ClickElement(_driver, btnOkLocBy);
                    reportLogger.TakeScreenshot(test, $"Close Error Dialog");
                    test.Log(Status.Warning, "<b>This loan doesn't exists in MSP</b>");
                    isSaveSuccessful = false;

                }
                //Navigate back to dsahboard
                webElementExtensions.WaitForInvisibilityOfElement(_driver, commonServices.spinnerLocBy);
                webElementExtensions.WaitUntilUrlContains("dashboard");
                return isSaveSuccessful;
            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, $"Error");
                test.Log(Status.Error, $"Eror while performing action, {e.Message}");
                return isSaveSuccessful;
            }


        }
        #endregion
    }
}
