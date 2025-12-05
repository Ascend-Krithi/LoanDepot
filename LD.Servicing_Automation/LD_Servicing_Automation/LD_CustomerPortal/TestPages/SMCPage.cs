using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Pages;
using log4net;
using OpenQA.Selenium;
using System;

namespace LD_CustomerPortal.Pages
{
    public class SMCPage
    {
        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }
        ReportLogger reportLogger { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;
        public SMCPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            reportLogger = new ReportLogger(_driver);
        }

        #region Locators        

        public By smcMessageTemplateContentTestLocBy = By.CssSelector("mello-serve-ui-secure-message-center");

        public By newMessageButtonOnSMCPageLocBy = By.XPath("//span[text()=' New Message ']");

        public By needAnEvenQuickerResponsePopupLocBy = By.CssSelector("mat-dialog-container mello-serve-ui-servis-bot");

        public By continueWithSecureMessageCenterLinkOnNeedAnEvenQuickerResponsePopupLocBy = By.XPath("//span[text()='Continue with Secure Message Center']");

        public By letsChatButtonOnNeedAnEvenQuickerResponsePopupLocBy = By.XPath("//button[contains(@class,'m-2 mat-primary')]/span");

        public By sendUpASecureMessagePopupLocBy = By.CssSelector("mello-serve-ui-secure-msg-container div[class$='secure-Msg-Container']");

        public By whatCanWeHelpYouWithOnSendUsASecureMessagePopupLocBy = By.CssSelector("mat-label[id^='lblIdType']");

        public By optionsOnSendUsASecureMessagePopupLocBy = By.CssSelector("div[id='ddlIdType-panel'] mat-option");

        public By messageTextInputBoxLocBy = By.CssSelector("textarea[id='txtIdMessage']");

        public By submitButtonOnSendUsASecureMessageLocBy = By.CssSelector("button[type='submit']");

        public By messageSentPopupLocBy = By.CssSelector("mello-serve-ui-msg-success");

        public By closeIconOnMessageSentPopUpLocBy = By.XPath("//mat-icon[text()=' close ']");

        public By sentMessagesTabLocBy = By.XPath("//div[text()='SENT MESSAGES']");

        public By refreshButtonLocBy = By.XPath("//mat-label[text()='Refresh']");
        #endregion Locators

        #region Services

        /// <summary>
        /// To Get the SMC message template content
        /// </summary>
        /// <param name="smc">"SM203"</param>
        /// <param name="autopayType">"Monthly/Bi-weekly"</param>
        public string GetSMCTemplateContent(string smc, string autopayType)
        {
            string title = string.Empty, messageTextContent = string.Empty;
            try
            {
                By firstNotificationLocBy = null;
                switch (smc)
                {
                    case Constants.SMCCode.SM203:
                        title = " loanDepot Authorized Recurring Automatic Payment - SM203 ";
                        break;
                    case Constants.SMCCode.SM256:
                        title = " loanDepot Cancellation Notice of Recurring Automatic Payment Plan - SM256 ";
                        break;
                    case Constants.SMCCode.SM257:
                        title = " loanDepot Authorized Automatic Recurring Payment - SM257 ";
                        break;
                    case Constants.SMCCode.SM258:
                        title = " loanDepot One Time Payment Notice - SM258 ";
                        break;
                    case Constants.SMCCode.SM260:
                        title = " loanDepot Cancellation Notice - SM260 ";
                        break;
                }
                firstNotificationLocBy = By.XPath(string.Format(commonServices.divLocatorWithSpecificText, title));
                webElementExtensions.WaitForVisibilityOfElement(_driver, firstNotificationLocBy);
                webElementExtensions.ActionClick(_driver, firstNotificationLocBy, $"First SMC Message with {smc}", false, true);
                webElementExtensions.WaitForVisibilityOfElement(_driver, firstNotificationLocBy);
                messageTextContent = webElementExtensions.GetElementText(_driver, smcMessageTemplateContentTestLocBy, true);
                reportLogger.TakeScreenshot(test, "Template Content");
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while Getting the SMC Message Template Content: {e.Message}");
            }
            return messageTextContent;
        }

        #endregion Services
    }
}
