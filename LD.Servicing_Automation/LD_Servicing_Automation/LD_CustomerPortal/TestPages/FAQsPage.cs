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
    /// Class to handle FAQs
    /// </summary>
    public class FAQsPage
    {

        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }
        ReportLogger reportLogger { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;
        public FAQsPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            reportLogger = new ReportLogger(_driver);
        }

        #region Locators
        public By faqSearchInputLocBy = By.Id("faqSearch");
        public By faqs = By.XPath("//*[contains(@id,'mat-expansion-panel-header')]");
        public By faqSections = By.XPath("//p[contains(@class,'ng-star-inserted')]");
        public string topicHeaderLinkLoc = "//mat-panel-title[contains(@class, 'panel-header-title accheader') and contains(text(), '<TEXT>')]";
        public By whatIsMultiFactorAuthenticationTextContentTextLocBy = By.XPath("//mat-expansion-panel-header//following-sibling::div");

        #endregion

        #region methods
        /// <summary>
        /// search by topic
        /// </summary>
        /// <param name="topic">topic to search</param>
        public void SearchByTopic(string topic)
        {
            try
            {
                webElementExtensions.WaitUntilElementIsClickable(_driver, faqSearchInputLocBy);
                reportLogger.TakeScreenshot(test, $"FAQ Page");

            var sections = _driver.FindElements(faqSections).Select(s => s.Text).ToList();
            ReportingMethods.LogAssertionTrue(test, sections.Count == 4, $"Number of FAQS section  should be 4 ,<br> {string.Join(",<br> ", sections)}");

                var topics = _driver.FindElements(faqs).Select(f => f.Text).ToList();
                ReportingMethods.LogAssertionTrue(test, topics.Count == 43, $"Number of FAQS should be 43 ,<br> {string.Join(",<br> ", topics)}");

                ReportingMethods.Log(test, $"Searching the FAQ with topic '{topic}'");

                webElementExtensions.ClickElement(_driver, faqSearchInputLocBy);
                webElementExtensions.EnterText(_driver, faqSearchInputLocBy, topic, false, isClickRequired: true);


                reportLogger.TakeScreenshot(test, $"Search Topic in FAQ Page");
                topics = _driver.FindElements(faqs).Select(f => f.Text).ToList();
                bool allFaqsFiltered = topics.TrueForAll(f => f.IndexOf(topic, StringComparison.OrdinalIgnoreCase) > 0);
                ReportingMethods.LogAssertionTrue(test, allFaqsFiltered, $"All filtered FAQS should have search topic '{topic}': <br> {string.Join(",<br> ", topics)}");
            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, $"Failed Search by {topic} in FAQ");
                test.Log(Status.Error, $"Failed while searching by Topic in FAQ with {e.Message}");
            }
        }

        /// <summary>
        /// Verify the text content of the specific Question on FAQs Page
        /// </summary>
        /// <param name="topic">"What is multi-factor authentication?"</param>
        public void VerifyTheTextContentForSpecificGeneralQuestionTopic(string topic = "What is multi-factor authentication?")
        {
            try
            {
                ReportingMethods.Log(test, $"Searching the FAQ with topic '{topic}'");
                webElementExtensions.EnterText(_driver, faqSearchInputLocBy, topic, false, isClickRequired: true);
                reportLogger.TakeScreenshot(test, $"Search Topic in FAQ Page");
                string b = string.Format(topic, topicHeaderLinkLoc);
                webElementExtensions.WaitForVisibilityOfElement(_driver, By.XPath(topicHeaderLinkLoc.Replace("<TEXT>", topic)));

                webElementExtensions.ActionClick(_driver, By.XPath(topicHeaderLinkLoc.Replace("<TEXT>", topic)));
                webElementExtensions.WaitForStalenessOfElement(_driver, whatIsMultiFactorAuthenticationTextContentTextLocBy, 2);
                webElementExtensions.WaitForVisibilityOfElement(_driver, whatIsMultiFactorAuthenticationTextContentTextLocBy);
                string actTextContent = webElementExtensions.GetElementText(_driver, whatIsMultiFactorAuthenticationTextContentTextLocBy);
                ReportingMethods.LogAssertionEqual(test, Constants.CustomerPortalTextMessages.whatIsMultiFactorAuthenticationTextContentOnFaqsPage, actTextContent, $"Verify {topic} Text Content");
                reportLogger.TakeScreenshot(test, $"{topic} Text Content");
            }
            catch (Exception e)
            {
                test.Log(Status.Error, $"Failed while verifying the text content for the {topic} Error: {e.Message}");
            }
        }

        #endregion
    }
}
