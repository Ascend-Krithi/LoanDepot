using AventStack.ExtentReports;
using LD_AutomationFramework;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using log4net;
using OpenQA.Selenium;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace LD_AutomationFramework.Pages
{
    public class YopmailPage
    {
        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }
        ReportLogger reportLogger { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;
        public YopmailPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            reportLogger = new ReportLogger(_driver);
        }

        #region Locators        

        public By enterYourEmailInputlocBy = By.CssSelector("[id='login']");
        public By checkInboxButtonLocBy = By.CssSelector("[title='Check Inbox @yopmail.com']");
        public By verificationCodeTextContent = By.XPath("//span[contains(text(),'Your code is: ')]");
        public By refreshButtonLocBy = By.CssSelector("button[id='refresh']");
        public By menuIconLocBy = By.CssSelector("div[class='wminboxheader'] > div:nth-child(1) > button > i");
        public By emptyInboxLocBy = By.CssSelector("button[id='delall'] > i + span");
        public By mailContentTextLocBy = By.CssSelector("body[class='bodymail yscrollbar']");
        public string emailContentLocator = $"//body[@class='bodymail yscrollbar']//div[text()='EMAIL_TEXT_CONTENT_LOCATOR']";

        #endregion Locators

        #region Services

        /// <summary>
        /// To get the Verification Code from the Yopmail.com, or clear inbox if only clearing is required.
        /// </summary>
        /// <param name="emailId">string "autotest@yopmail.com"</param>
        /// <param name="isOnlyClearEmailsRequired">bool true to only clear inbox, false to get verification code</param>
        /// <param name="emailNotificationType">string notification type to get the content from Yopmail</param>
        /// <returns>Verification code or Email Content string or empty if only clearing inbox</returns>
        public string GetEmailContentFromYopmail(string emailId, bool isOnlyClearEmailsRequired = false, string emailNotificationType = Constants.EmailNotificationFromYopmail.VerificationCode)
        {
            string expectedEmailContent = string.Empty;
            try
            {
                string baseWindowHandle = _driver.CurrentWindowHandle.ToString();
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.open();");
                _driver.SwitchTo().Window(_driver.WindowHandles.Last());
                _driver.Navigate().GoToUrl("https://yopmail.com/");

                webElementExtensions.WaitForElementToBeEnabled(_driver, enterYourEmailInputlocBy, "Enter your Email Input Field", ConfigSettings.WaitTime, false);
                webElementExtensions.EnterText(_driver, enterYourEmailInputlocBy, emailId, false, "Enter your Email Input Field", isReportRequired: true);
                webElementExtensions.WaitForStalenessOfElement(_driver, checkInboxButtonLocBy, 2);
                webElementExtensions.ActionClick(_driver, checkInboxButtonLocBy, "Check Inbox Button", isReportRequired: true);
                webElementExtensions.WaitForElementToBeEnabled(_driver, refreshButtonLocBy, "Refresh Button", ConfigSettings.WaitTime);

                if (isOnlyClearEmailsRequired)
                {
                    // Just clear the inbox and return
                    webElementExtensions.ActionClick(_driver, menuIconLocBy, "Menu Icon", isReportRequired: true);
                    webElementExtensions.WaitForStalenessOfElement(_driver, emptyInboxLocBy, 2);
                    webElementExtensions.ActionClick(_driver, emptyInboxLocBy, "Empty Inbox", isReportRequired: true);
                    webElementExtensions.HandleAlert(_driver, true, ConfigSettings.WaitTime);
                    webElementExtensions.WaitForStalenessOfElement(_driver, menuIconLocBy, ConfigSettings.SmallWaitTime);
                    _driver.Close();
                    webElementExtensions.SwitchToFirstTab(_driver);
                    return string.Empty;
                }

                webElementExtensions.ActionClick(_driver, refreshButtonLocBy);
                webElementExtensions.SwitchToFrame(_driver, "ifmail");
                By emailContentLocBy = By.XPath(emailContentLocator.Replace("EMAIL_TEXT_CONTENT_LOCATOR", emailNotificationType));
                switch (emailNotificationType)
                {
                    case Constants.EmailNotificationFromYopmail.OTPSetup:
                        webElementExtensions.WaitForVisibilityOfElement(_driver, emailContentLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForStalenessOfElement(_driver, emailContentLocBy, 5);
                        reportLogger.TakeScreenshot(test, "OTP Setup Email Notification");
                        expectedEmailContent = webElementExtensions.GetElementText(_driver, mailContentTextLocBy, true);
                        break;
                    case Constants.EmailNotificationFromYopmail.OTPDelete:
                        webElementExtensions.WaitForVisibilityOfElement(_driver, emailContentLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForStalenessOfElement(_driver, emailContentLocBy, 5);
                        reportLogger.TakeScreenshot(test, "OTP Delete Email Notification");
                        expectedEmailContent = webElementExtensions.GetElementText(_driver, mailContentTextLocBy, true);
                        break;
                    case Constants.EmailNotificationFromYopmail.AutopaySetup:
                        webElementExtensions.WaitForVisibilityOfElement(_driver, emailContentLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForStalenessOfElement(_driver, emailContentLocBy, 5);
                        reportLogger.TakeScreenshot(test, "Autopay Setup Email Notification");
                        expectedEmailContent = webElementExtensions.GetElementText(_driver, mailContentTextLocBy, true);
                        break;
                    case Constants.EmailNotificationFromYopmail.AutopayDelete:
                        webElementExtensions.WaitForVisibilityOfElement(_driver, emailContentLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForStalenessOfElement(_driver, mailContentTextLocBy, 5);
                        reportLogger.TakeScreenshot(test, "Autopay Delete Email Notification");
                        expectedEmailContent = webElementExtensions.GetElementText(_driver, mailContentTextLocBy, true);
                        break;
                    case Constants.EmailNotificationFromYopmail.VerificationCode:
                        webElementExtensions.WaitForVisibilityOfElement(_driver, verificationCodeTextContent, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForStalenessOfElement(_driver, verificationCodeTextContent, 5);
                        string emailText = webElementExtensions.GetElementText(_driver, mailContentTextLocBy, true);
                        if (UtilAdditions.GetRootDirectory().Contains(Constants.SolutionProjectNames.MSP))
                            expectedEmailContent = Regex.Match(emailText, @"\d{6}").Value;
                        else
                            expectedEmailContent = Regex.Match(emailText, @"(?<=\b(?:Your code is|using the following code):\s*)\d{6}").Value;
                        test.Log(Status.Info, $"Verification code : {expectedEmailContent}");
                        reportLogger.TakeScreenshot(test, "Verification Code On Yopmail");
                        break;
                    case Constants.EmailNotificationFromYopmail.EmailTemplateYourAuthenticationCodeTextContent:
                        webElementExtensions.WaitForVisibilityOfElement(_driver, emailContentLocBy, ConfigSettings.WaitTime);
                        webElementExtensions.WaitForStalenessOfElement(_driver, mailContentTextLocBy, 5);
                        reportLogger.TakeScreenshot(test, "Email Template Text Content");
                        expectedEmailContent = webElementExtensions.GetElementText(_driver, mailContentTextLocBy, true);
                        break;
                    default:
                        test.Log(Status.Error, $"{emailNotificationType} is not in the required Email Notification Content type");
                        break;
                }

                webElementExtensions.SwitchToDefaultContent(_driver);
                webElementExtensions.ActionClick(_driver, menuIconLocBy, "Menu Icon", isReportRequired: true);
                webElementExtensions.WaitForStalenessOfElement(_driver, emptyInboxLocBy, 2);
                webElementExtensions.ActionClick(_driver, emptyInboxLocBy, "Empty Inbox", isReportRequired: true);
                webElementExtensions.HandleAlert(_driver, true, ConfigSettings.WaitTime);
                webElementExtensions.WaitForStalenessOfElement(_driver, menuIconLocBy, ConfigSettings.SmallWaitTime);
                _driver.Close();
                webElementExtensions.SwitchToTab(_driver, baseWindowHandle);
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Something happened while Getting the Verification Code from Yopmail: {e.Message}");
            }

            return !string.IsNullOrEmpty(expectedEmailContent) ? expectedEmailContent.Trim() : string.Empty;
        }

        #endregion Services
    }
}
