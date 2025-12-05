using AutoIt;
using AventStack.ExtentReports;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Utilities;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace LD_AutomationFramework.Pages
{
    public class WebElementExtensionsPage
    {
        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }
        CommonServicesPage commonServices { get; set; }
        public WebElementExtensionsPage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
        }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Locators



        #endregion Locators

        #region Services
        /// <summary>
        /// Wait for new tas to be opned until active tabs/windows count is >= expectedTabsCount
        /// </summary>
        /// <param name="_driver"></param>
        /// <param name="expectedTabsCount">expected count</param>
        /// <param name="waitTime">waittime</param>
        /// <param name="pollingInterval">interval</param>

        public void WaitForNewTabWindow(IWebDriver _driver, int expectedTabsCount, int waitTime = 0, int pollingInterval = 1)
        {
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.PollingInterval = TimeSpan.FromSeconds(pollingInterval);
            if (waitTime == 0)
            {
                wait.Timeout = TimeSpan.FromSeconds(ConfigSettings.WaitTime);
            }
            else
            {
                wait.Timeout = TimeSpan.FromSeconds(waitTime);
            }

            try
            {
                wait.Until(d => d.WindowHandles.Count >= expectedTabsCount);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// Switch to Window based on handle 
        /// </summary>
        /// <param name="_driver">Driver reference</param>
        /// <param name="windowHandle">window handle</param>

        public bool SwitchToTab(IWebDriver _driver, string windowHandle)
        {
            try
            {
                _driver.SwitchTo().Window(windowHandle);
                return true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while switching tab: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Method to wait until webElement is located using 'FindElement' method ignoring the exceptions until wait time set
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="locator">By.Id/By.Xpath/..</param>
        /// <param name="waitTime">wait time in seconds</param>
        /// <returns>true/false</returns>
        public bool WaitForElement(IWebDriver _driver, By locator, int waitTime = 0)
        {
            bool flag = false;
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            if (waitTime == 0)
            {
                wait.Timeout = TimeSpan.FromSeconds(ConfigSettings.WaitTime);
            }
            else
            {
                wait.Timeout = TimeSpan.FromSeconds(waitTime);
            }

            try
            {
                wait.Until(x =>
                {
                    try
                    {
                        flag = true;
                        return x.FindElement(locator);
                    }
                    catch (Exception ex)
                    {
                        flag = false;
                        if (ex.Message.Contains("no such window"))
                            _driver.Navigate().Refresh();
                        wait.IgnoreExceptionTypes(typeof(NoSuchElementException), typeof(NoSuchWindowException), typeof(StaleElementReferenceException), typeof(ElementNotInteractableException));
                        return null;
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Method to wait until page is loaded.
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="waitTime">wait time in seconds</param>
        /// <returns>true/false</returns>
        public bool WaitForPageLoad(IWebDriver _driver, int waitTime = 0, int pollingInterval = 1)
        {
            bool flag = false;
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.PollingInterval = TimeSpan.FromSeconds(pollingInterval);
            if (waitTime == 0)
            {
                wait.Timeout = TimeSpan.FromSeconds(ConfigSettings.WaitTime);
            }
            else
            {
                wait.Timeout = TimeSpan.FromSeconds(waitTime);
            }

            try
            {
                wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Method to wait for title of driver contains expected string
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="waitTime">wait time in seconds</param>
        /// <param name="pollingInterval">polling time in seconds</param>
        /// <returns>true/false</returns>
        public void WaitForTitleContains(IWebDriver _driver, string title, int waitTime = 0, int pollingInterval = 1)
        {
            bool flag = false;
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.PollingInterval = TimeSpan.FromSeconds(pollingInterval);
            if (waitTime == 0)
            {
                wait.Timeout = TimeSpan.FromSeconds(ConfigSettings.WaitTime);
            }
            else
            {
                wait.Timeout = TimeSpan.FromSeconds(waitTime);
            }

            try
            {
                wait.Until(d => (string.IsNullOrEmpty(d.Title) && d.Title.Contains(title)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        /// <summary>
        /// Method to wait for title of driver not contains expected string
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="waitTime">wait time in seconds</param>
        /// <param name="pollingInterval">polling time in seconds</param>
        /// <returns>true/false</returns>
        public void WaitForTitleNotContains(IWebDriver _driver, string title, int waitTime = 0, int pollingInterval = 1)
        {
            bool flag = false;
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            wait.PollingInterval = TimeSpan.FromSeconds(pollingInterval);
            if (waitTime == 0)
            {
                wait.Timeout = TimeSpan.FromSeconds(ConfigSettings.WaitTime);
            }
            else
            {
                wait.Timeout = TimeSpan.FromSeconds(waitTime);
            }

            try
            {
                wait.Until(d => (!string.IsNullOrEmpty(d.Title) && !d.Title.Contains(title)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        /// <summary>
        /// Method to wait until webElement is invisible
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="locator">By.Id/By.Xpath/..</param>
        /// <param name="waitTime">wait time in seconds</param>
        /// <returns>true/false</returns>
        public bool WaitForInvisibilityOfElement(IWebDriver _driver, By locator, int waitTime = 0)
        {
            bool flag = false;
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            if (waitTime == 0)
            {
                wait.Timeout = TimeSpan.FromSeconds(ConfigSettings.WaitTime);
            }
            else
            {
                wait.Timeout = TimeSpan.FromSeconds(waitTime);
            }

            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locator));
                flag = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Method to wait until webElement is visible
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="locator">By.Id/By.Xpath/..</param>
        /// <param name="waitTime">wait time in seconds</param>
        public void WaitForVisibilityOfElement(IWebDriver _driver, By locator, int waitTime = 0)
        {
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            if (waitTime == 0)
            {
                wait.Timeout = TimeSpan.FromSeconds(ConfigSettings.WaitTime);
            }
            else
            {
                wait.Timeout = TimeSpan.FromSeconds(waitTime);
            }

            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Method to wait until all elements are present
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="locator">By.Id/By.Xpath/..</param>
        /// <param name="waitTime">wait time in seconds</param>
        public void WaitForStalenessOfElement(IWebDriver _driver, By locator, int waitTime = 0)
        {
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            if (waitTime == 0)
            {
                wait.Timeout = TimeSpan.FromSeconds(ConfigSettings.WaitTime);
            }
            else
            {
                wait.Timeout = TimeSpan.FromSeconds(waitTime);
            }

            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(_driver.FindElement(locator)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Method to wait until webElement is clickable
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="locator">By.Id/By.Xpath/..</param>
        /// <param name="waitTime">wait time in seconds</param>
        public void WaitUntilElementIsClickable(IWebDriver _driver, By locator, int waitTime = 0)
        {
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            if (waitTime == 0)
            {
                wait.Timeout = TimeSpan.FromSeconds(ConfigSettings.WaitTime);
            }
            else
            {
                wait.Timeout = TimeSpan.FromSeconds(waitTime);
            }

            try
            {
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Method to verify whether an webElement is displayed or not.
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="locator">By.Id, xpath, etc</param>
        /// <param name="elementName">Web Element name</param>
        public bool IsElementDisplayed(IWebDriver _driver, By locator, string elementName = null, bool isScrollIntoViewRequired = true, bool isScrollToTopRequired = false)
        {
            bool flag = false;
            try
            {
                if (isScrollIntoViewRequired)
                    ScrollIntoView(_driver, locator);
                if (isScrollToTopRequired)
                    ScrollToTop(_driver);
                var element = _driver.FindElement(locator);
                flag = element.Displayed;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying whether webElement is displayed or not: " + ex.Message);
            }
            if (!string.IsNullOrEmpty(elementName))
                _driver.ReportResult(test, flag, "Successfully verified that the webElement - " + elementName + " is displayed.", "The webElement - " + elementName + " is not displayed.");
            return flag;
        }

        /// <summary>
        /// Method to verify whether an webElement is displayed or not based on the count.
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="locator">By.Id, xpath, etc</param>
        /// <param name="elementName">Web Element name</param>
        public bool IsElementDisplayedBasedOnCount(IWebDriver _driver, By locator, string elementName = null)
        {
            bool flag = false;
            int count = 0;
            try
            {
                count = _driver.FindElements(locator).Count;
                flag = count > 0;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying whether webElement is displayed or not: " + ex.Message);
            }
            if (!string.IsNullOrEmpty(elementName))
                _driver.ReportResult(test, flag, "Successfully verified that " + elementName + " is displayed.", "The " + elementName + " is not displayed.");
            return flag;
        }

        /// <summary>
        /// Method to verify actual and expected values of a web Element's attribute
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="locator">By Locator</param>
        /// <param name="attributeName">disabled, isSeletced, Class, etc.</param>
        /// <param name="expectedValue">Value to be verified</param>
        /// <param name="isReportRequired">true/false</param>
        /// <returns></returns>
        public bool VerifyElementAttributeValue(IWebDriver _driver, By locator, string attributeName, string expectedValue, bool isReportRequired = false)
        {
            bool flag = false;
            string elementValue = string.Empty;
            try
            {
                elementValue = _driver.FindElement(locator).GetAttribute(attributeName);
                if (elementValue == expectedValue)
                    flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying actual and expected attribute values of the webElement : " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully verified that the expected and actual values of the webElement attribute - " + attributeName + " and value - " + expectedValue + "matches.", "The expected and actual values of the webElement attribute - " + attributeName + " and value - " + expectedValue + "did not match.");
            return flag;
        }

        /// <summary>
        /// Method to get the Css attribute value of a web Element
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="locator">By Locator</param>
        /// <param name="attributeName">background-color, font-family, etc.</param>
        /// <param name="isReportRequired">true/false</param>
        /// <returns></returns>
        public string GetCssAttributeValueOfElement(IWebDriver _driver, By locator, string attributeName, bool isReportRequired = false)
        {
            bool flag = false;
            string cssValue = string.Empty;
            try
            {
                cssValue = _driver.FindElement(locator).GetCssValue(attributeName);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while retrieving the css attribute value of the webElement : " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully retrieved the value of the webElement's css attribute.<br />Attribute name - '" + attributeName + "'.<br />Attribute value - '" + cssValue + "'.", "Failed while retrieving the value of the webElement Css attribute.<br />Attribute name - '" + attributeName + "'.");
            return cssValue;
        }


        /// <summary>
        /// Method to verify actual and expected values of a web Element's text, optionally we can also check contains instead of equals
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="locator">By Locator</param>
        /// <param name="expectedValue">Value to be verified</param>
        /// <param name="locatorName">The name of the field/web Element in UI</param>
        /// <param name="isReportRequired">true/false</param>
        /// <param name="checkContains">false</param>
        /// <returns></returns>
        public bool VerifyElementText(IWebDriver _driver, By locator, string expectedValue, string locatorName = null, bool isReportRequired = false, bool checkContains = false)
        {
            bool flag = false;
            string elementText = string.Empty;
            try
            {
                WaitForElement(_driver, locator);
                elementText = _driver.FindElement(locator).Text.Trim(' ');

                if (checkContains)
                    flag = elementText.Contains(expectedValue);

                else
                    flag = elementText == expectedValue;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying actual and expected attribute values of the webElement : " + ex.Message);
            }
            if (isReportRequired)
                if (locatorName != null)
                    _driver.ReportResult(test, flag, "Successfully verified that the actual UI data of " + locatorName + " matches with the expected value - " + expectedValue + ".", "The actual and expected text value did not match. Actual - " + elementText + ", Expected - " + expectedValue + ".");
                else
                    _driver.ReportResult(test, flag, "Successfully verified that the text in application matches with the expected value - " + expectedValue + ".", "The actual and expected text value did not match. Actual - " + elementText + ", Expected - " + expectedValue + ".");
            return flag;
        }

        /// <summary>
        /// Method to get a web Element's text
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="locator">By Locator</param>
        /// <param name="isReportRequired">true/false</param>
        /// <returns></returns>
        public string GetElementText(IWebDriver _driver, By locator, bool isReportRequired = false)
        {
            bool flag = false;
            string elementText = string.Empty;
            try
            {
                WaitForVisibilityOfElement(_driver, locator);
                elementText = _driver.FindElement(locator).Text;
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while retrieving web Element's text : " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully retrieved the webelement text - " + elementText + ".", "Failed to retrieve the web Element text.");
            return elementText;
        }

        /// <summary>
        /// Method to switch to a frame using name or ID
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="frameNameOrID">Frame Name or ID</param>
        public void SwitchToFrame(IWebDriver _driver, string frameNameOrID)
        {
            try
            {
                _driver.SwitchTo().Frame(frameNameOrID);
            }
            catch (Exception ex)
            {
                log.Error("Failed while switching to the frame - " + frameNameOrID + ": " + ex.Message);
            }
        }

        /// <summary>
        /// Method to switch back to frame default content
        /// </summary>
        /// <param name="_driver">Web driver</param>
        public void SwitchToDefaultContent(IWebDriver _driver)
        {
            try
            {
                _driver.SwitchTo().DefaultContent();
            }
            catch (Exception ex)
            {
                log.Error("Failed while switching to frame default content: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to switch browser tabs
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="tabNumber">Tab number starting with 0, 1, 2...</param>
        public void SwitchTabs(IWebDriver _driver, int tabNumber)
        {
            try
            {
                IList<string> windowHandles = new List<string>(_driver.WindowHandles);
                _driver.SwitchTo().Window(windowHandles[tabNumber]);
            }
            catch (Exception ex)
            {
                log.Error("Failed while switching between tabs: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to open a new tab and switch
        /// </summary>
        /// <param name="_driver">Web driver</param>
        public void OpenNewTabAndSwitch(IWebDriver _driver)
        {
            try
            {
                _driver.SwitchTo().NewWindow(WindowType.Tab);
            }
            catch (Exception ex)
            {
                log.Error("Failed while opening a new tab and switching to it: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to switch to first tab
        /// </summary>
        /// <param name="_driver">Web driver</param>
        public void SwitchToFirstTab(IWebDriver _driver)
        {
            try
            {
                _driver.SwitchTo().Window(_driver.WindowHandles.First());
            }
            catch (Exception ex)
            {
                log.Error("Failed while switching to first tab: " + ex.Message);
            }
        }

        ///<summary>
        ///Method to enter text in a text field
        ///<param name="locator">Locator of the input text field - By.Id/By.XPath/...</param>
        ///<param name="textToBeEntered">The text to be entered in the input field</param>
        ///</summary>
        public bool EnterText(IWebDriver _driver, By locator, string textToBeEntered, bool isScrollIntoViewRequired = true, string controlName = null, bool isReportRequired = false, bool scrollToTopRequired = false, bool isClickRequired = false)
        {
            bool flag = false;
            IWebElement element = null;
            Actions action = new Actions(_driver);
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);

            try
            {
                WaitForElement(_driver, locator);
                element = _driver.FindElement(locator);
                if (isScrollIntoViewRequired)
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                wait.Until(ExpectedConditions.ElementIsVisible(locator)).Clear();
                if (isClickRequired)
                    element.Click();
                element.SendKeys(textToBeEntered);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while entering text in the input text field webElement: " + ex.Message);
            }
            if (isReportRequired)
            {
                if (controlName != null)
                    _driver.ReportResult(test, flag, "Successfully entered the value - " + textToBeEntered + " in " + controlName + " field.", "Failed while entering the text value - " + textToBeEntered + " in " + controlName + " field.");
                else
                    _driver.ReportResult(test, flag, "Successfully entered the value - " + textToBeEntered + " in the input text field.", "Failed while entering the text value - " + textToBeEntered + " in the input text field.");
            }
            return flag;
        }

        ///<summary>
        ///Method to enter text in a text field
        ///<param name="element">Element of the input text field - IWebElement/...</param>
        ///<param name="textToBeEntered">The text to be entered in the input field</param>
        ///</summary>
        public bool EnterText(IWebDriver _driver, IWebElement element, string textToBeEntered, bool isScrollIntoViewRequired = true, string controlName = null, bool isReportRequired = false, bool scrollToTopRequired = false, bool isClickRequired = false)
        {
            bool flag = false;
            Actions action = new Actions(_driver);
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);

            try
            {
                if (isScrollIntoViewRequired)
                    ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                element.Clear();
                if (isClickRequired)
                    element.Click();
                element.SendKeys(textToBeEntered);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while entering text in the input text field webElement: " + ex.Message);
            }
            if (isReportRequired)
            {
                if (controlName != null)
                    _driver.ReportResult(test, flag, "Successfully entered the value - " + textToBeEntered + " in " + controlName + " field.", "Failed while entering the text value - " + textToBeEntered + " in " + controlName + " field.");
                else
                    _driver.ReportResult(test, flag, "Successfully entered the value - " + textToBeEntered + " in the input text field.", "Failed while entering the text value - " + textToBeEntered + " in the input text field.");
            }
            return flag;
        }

        ///<summary>
        ///Method to move control to a web Element and then perform click using Actions class
        ///<param name="locator">Locator of the Element to be clicked - By.Id/By.XPath/...</param>
        ///<param name="webElementName">Field name in application to click</param>
        ///<param name="doubleClick">Pass true if web Element should be double clicked</param>
        ///</summary>
        public bool ActionClick(IWebDriver _driver, By locator, string webElementName = null, bool doubleClick = false, bool isReportRequired = false)
        {
            bool flag = false;
            Actions action = new Actions(_driver);
            try
            {
                IWebElement element = _driver.FindElement(locator);
                if (doubleClick)
                    action.MoveToElement(element).DoubleClick().Build().Perform();
                else
                    action.MoveToElement(element).Click().Build().Perform();
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while clicking on the web Element in the application: " + ex.Message);
            }
            if (isReportRequired)
            {
                if (webElementName != null)
                    _driver.ReportResult(test, flag, "Successfully clicked on the - " + webElementName + ".", "Failed while clicking on the - " + webElementName + ".");
                else
                    _driver.ReportResult(test, flag, "Successfully clicked on the web webElement having locator as " + locator + ".", "Failed while clicking on the web Element having locator as " + locator + ".");
            }
            return flag;
        }

        ///<summary>
        ///Method to move control to a web Element and then perform click
        ///<param name="locator">Locator of the webElement to be clicked - By.Id/By.XPath/...</param>
        ///<param name="webElementName">Field name in application to click</param>
        ///</summary>
        public bool ClickElement(IWebDriver _driver, By locator, string webElementName = null, bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                WaitForElement(_driver, locator);
                IWebElement element = _driver.FindElement(locator);
                element.Click();
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while clicking on the web Element in the application: " + ex.Message);
            }
            if (isReportRequired)
            {
                if (webElementName != null)
                    _driver.ReportResult(test, flag, "Successfully clicked on the web Element - " + webElementName + ".", "Failed while clicking on the web Element - " + webElementName + ".");
                else
                    _driver.ReportResult(test, flag, "Successfully clicked on the web Element having locator as " + locator + ".", "Failed while clicking on the web Element having locator as " + locator + ".");
            }
            return flag;
        }

        ///<summary>
        ///Method to click a web Element using Javascript executor
        ///<param name="locator">Locator of the web Element - By.Id/By.XPath/...</param>
        ///<param name="webElementName">Web Element being clicked</param>
        ///<param name="isReportRequired">true/false</param>
        ///</summary>
        public bool ClickElementUsingJavascript(IWebDriver _driver, By locator, string webElementName = null, bool isReportRequired = false)
        {
            bool flag = false;
            IWebElement element = null;

            try
            {
                element = _driver.FindElement(locator);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while clicking on the web Element in the application: " + ex.Message);
            }
            if (isReportRequired)
            {
                if (webElementName != null)
                    _driver.ReportResult(test, flag, "Successfully clicked on " + webElementName + ".", "Failed while clicking on " + webElementName + ".");
                else
                    _driver.ReportResult(test, flag, "Successfully clicked on the web webElement having locator as " + locator + ".", "Failed while clicking on the web webElement having locator as " + locator + ".");
            }
            return flag;
        }

        ///<summary>
        ///Method to move control to a web webElement
        ///<param name="locator">Locator of the webElement to be clicked - By.Id/By.XPath/...</param>
        ///</summary>
        public void MoveToElement(IWebDriver _driver, By locator)
        {
            Actions action = new Actions(_driver);
            try
            {
                IWebElement element = _driver.FindElement(locator);
                action.MoveToElement(element).Build().Perform();
            }
            catch (Exception ex)
            {
                log.Error("Failed while moving to and highlighting the web webElement in the application: " + ex.Message);
            }
        }

        ///<summary>
        ///Method to move control to a web webElement
        ///<param name="element">webelemnt reference</param>
        ///</summary>
        public void MoveToElement(IWebDriver _driver, IWebElement element)
        {
            Actions action = new Actions(_driver);
            try
            {
                action.MoveToElement(element).Build().Perform();
            }
            catch (Exception ex)
            {
                log.Error("Failed while moving to and highlighting the web webElement in the application: " + ex.Message);
            }
        }

        ///<summary>
        ///Method to send keystrokes from the keyboard        
        ///<param name="keyStrokeToBeSimulated">Pass the key stroke to be simulated like Enter/Delete/Shift/....</param>
        ///<param name="isReportRequired">true/false</param>
        ///</summary>
        public bool SendKeyStrokesFromKeyboard(IWebDriver _driver, string keyStrokeToBeSimulated, bool isReportRequired = false)
        {
            bool flag = false;
            Actions action = new Actions(_driver);
            try
            {
                action.SendKeys(keyStrokeToBeSimulated).Build().Perform();
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while performing keystrokes in the application: " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully send the following key stroke - " + keyStrokeToBeSimulated + ".", "Failed while sending the following key stroke - " + keyStrokeToBeSimulated + ".");
            return flag;
        }

        ///<summary>
        ///Method to scroll a web webElement into view
        ///<param name="locator">Locator of the web webElement - By.Id/By.XPath/...</param>
        ///</summary>
        public bool ScrollIntoView(IWebDriver _driver, By locator)
        {
            bool flag = false;
            IWebElement element = null;

            try
            {
                element = _driver.FindElement(locator);
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while scrolling the web webElement into view: " + ex.Message);
            }
            return flag;
        }

        ///<summary>
        ///Method to scroll a web webElement into view
        ///<param name="element">web webElement - Located By.Id/By.XPath/...</param>
        ///</summary>
        public bool ScrollIntoView(IWebDriver _driver, IWebElement element)
        {
            bool flag = false;

            try
            {

                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while scrolling the web webElement into view: " + ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Method to verify the webElement color using By locator or button name. Pass either locator parameter or buttonName.
        /// </summary>
        /// <param name="expectedColorValue">Color in Css</param>
        /// <param name="locator">Element Locator - By Id, Css, Xpath</param>
        /// <param name="buttonName">Make a payment/Setup Autopay/etc.</param>        
        /// <param name="message">Verification message</param>
        /// <param name="cssColorAttribute">background-color/color/etc.</param>
        /// <returns>true/false</returns>
        public bool VerifyElementColor(string expectedColorValue, By locator = null, string buttonName = null, string message = null, string cssColorAttribute = Constants.CssAttributes.BackgroundColor, bool isScrollIntoViewRequired = true, bool isScrollToTopRequired = false)
        {
            string actualColorValue = string.Empty;
            commonServices = new CommonServicesPage(_driver, test);
            try
            {
                if (locator == null && buttonName != null)
                    locator = By.XPath(commonServices.buttonByText.Replace("<BUTTONNAME>", buttonName));
                if (isScrollIntoViewRequired)
                    ScrollIntoView(_driver, locator);
                if (isScrollToTopRequired)
                    ScrollToTop(_driver);
                actualColorValue = GetCssAttributeValueOfElement(_driver, locator, cssColorAttribute).Trim();
                if (message != null)
                    ReportingMethods.LogAssertionEqual(test, expectedColorValue, actualColorValue, message);
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying webElement color: " + ex.Message);
            }
            return actualColorValue.Equals(expectedColorValue);
        }

        /// <summary>
        /// Method to extract PDF data to a string
        /// </summary>
        public void PdfValidation()
        {
            string userFolderPath = Environment.GetEnvironmentVariable("USERPROFILE");
            string pdfUrl = _driver.FindElement(By.CssSelector("ld-pdf-viewer embed")).GetAttribute("src");
            string pdfName = pdfUrl.Split('/')[3];

            ((IJavaScriptExecutor)_driver).ExecuteScript("window.open();");
            _driver.SwitchTo().Window(_driver.WindowHandles.Last());
            _driver.Navigate().GoToUrl(pdfUrl);
            Thread.Sleep(2000);
            Actions builder = new Actions(_driver);
            for (int i = 0; i < 7; i++)
            {
                builder.SendKeys(Keys.Tab).Build().Perform();
                builder.SendKeys(Keys.Enter).Build().Perform();
            }
            int aiDialogHandle = AutoItX.WinWaitActive("Save As", "", 5);
            if (aiDialogHandle > 0)
            {
                AutoItX.ControlClick("Save As", "&Save", "[CLASS:Button; INSTANCE:2]");
            }
            _driver.Navigate().GoToUrl(pdfUrl);
            Thread.Sleep(2000);
            AutoItX.ControlClick("Save As", "Cancel", "[CLASS:Button; INSTANCE:3]");

            using (var client = new WebClient())
            {
                string abc = "file:///" + userFolderPath.Replace("\\", "/") + "/Downloads/" + pdfName + ".pdf";
                byte[] data = client.DownloadData(abc);
                string currentText = string.Empty, pdfContent = string.Empty;
                PdfReader pdfReader = new PdfReader(data);
                for (int page = 1; page <= pdfReader.NumberOfPages; page++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                    currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

                    currentText = Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8,
                        Encoding.Default.GetBytes(currentText)));
                }
                pdfContent = currentText;
                pdfReader.Close();
            }
        }

        /// <summary>
        /// Method to convert date time to different formats
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="date">input value</param>
        /// <param name="format">format to which date should be converted</param>
        /// <returns>required date time format as string</returns>
        public string DateTimeConverter(IWebDriver _driver, string date, string format)
        {
            string returnValue = null;
            int month = 0, year = 0, day = 0;
            string month_name = string.Empty;
            DateTime dateTime = DateTime.Now;
            try
            {
                switch (format)
                {
                    case "m/d/yyyy to fullMonthName d, yyyy":
                        date = date.Split(' ')[0];
                        day = Convert.ToInt32(date.Split('/')[1]);
                        month = Convert.ToInt32(date.Split('/')[0]);
                        year = Convert.ToInt32(date.Split('/')[2]);
                        dateTime = new DateTime(year, month, day);
                        month_name = dateTime.ToString("MMMM");
                        returnValue = month_name + " " + day + ", " + year;
                        break;

                    case "m/d/yyyy to threeLetterMonthName d, yyyy":
                        date = date.Split(' ')[0];
                        day = Convert.ToInt32(date.Split('/')[1]);
                        month = Convert.ToInt32(date.Split('/')[0]);
                        year = Convert.ToInt32(date.Split('/')[2]);
                        dateTime = new DateTime(year, month, day);
                        month_name = dateTime.ToString("MMM");
                        returnValue = month_name + " " + day + ", " + year;
                        break;

                    case "mm/dd/yy to m/d/yyyy":
                        if (date.Contains(' '))
                            date = date.Split(' ')[0];
                        day = Convert.ToInt32(date.Split('/')[1]);
                        month = Convert.ToInt32(date.Split('/')[0]);
                        year = Convert.ToInt32(date.Split('/')[2]);
                        year = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(year);
                        returnValue = new DateTime(year, month, day).ToString();
                        break;

                    default: break;
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while converting date to the required format: " + format + " - " + ex.Message);
            }
            return returnValue;
        }

        /// <summary>
        /// Method to calculate difference in months between two dates
        /// </summary>
        /// <param name="startDate">dd/m/yyyy format. Keep start date always earlier than or equal to endDate for consistent calculation</param>
        /// <param name="endDate">dd/m//yyyy format</param>
        /// <returns>Month difference</returns>
        public int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int months = 0;
            try
            {
                if (startDate > endDate)
                {
                    DateTime temp = startDate;
                    startDate = endDate;
                    endDate = temp;
                }

                months = (endDate.Year - startDate.Year) * 12;
                months += endDate.Month - startDate.Month;

                // Adjust if the day of the month in startDate is later than in endDate
                // This ensures only full months are counted or a partial month is excluded
                if (endDate.Day < startDate.Day)
                {
                    months--;
                }
            }
            catch(Exception ex)
            {
                log.Error("Failed while calculating month difference: " + ex.Message);
            }
            return months;
        }

        /// <summary>
        /// Method to generate a random number
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="minimum">Minimum value limit</param>
        /// <param name="maximum">Maximum value limit</param>
        /// <returns>random number generated</returns>
        public string RandomNumberGenerator(IWebDriver _driver, int minimum, int maximum)
        {
            int returnValue = 0;
            try
            {
                Random rnd = new Random();
                returnValue = rnd.Next(minimum, maximum);
            }
            catch (Exception ex)
            {
                log.Error("Failed while generating random number: " + ex.Message);
            }
            return returnValue.ToString();
        }

        /// <summary>
        /// Method to generate a random number
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="minimum">Minimum value limit</param>
        /// <param name="maximum">Maximum value limit</param>
        /// <returns>random number generated</returns>
        public string RandomNumberGenerator(IWebDriver _driver, double minimum, double maximum)
        {
            double returnValue = 0.00;
            try
            {
                Random random = new Random();
                returnValue = random.NextDouble() * (maximum - minimum) + minimum;
            }
            catch (Exception ex)
            {
                log.Error("Failed while generating random number: " + ex.Message);
            }
            return returnValue.ToString("0.00");
        }

        /// <summary>
        /// Method to verify whether an webElement is enabled or not.
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="locator">By.Id, xpath, etc</param>
        /// <param name="elementName">Web webElement name</param>
        public bool IsElementEnabled(IWebDriver _driver, By locator, string elementName = null, bool isScrollIntoViewRequired = true)
        {
            bool flag = false;
            try
            {
                if (isScrollIntoViewRequired)
                    ScrollIntoView(_driver, locator);
                var element = _driver.FindElement(locator);
                flag = element.Enabled;
            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying whether webElement is enabled or not: " + ex.Message);
            }
            if (!string.IsNullOrEmpty(elementName))
                _driver.ReportResult(test, flag, "Successfully verified that the webElement - " + elementName + " is enabled.", "The webElement - " + elementName + " is not displayed.");
            return flag;
        }

        /// <summary>
        /// Method to verify whether an webElement is disabled or not.
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="locator">By.Id, xpath, etc</param>
        /// <param name="elementName">Web webElement name</param>
        public bool IsElementDisabled(IWebDriver _driver, By locator, string elementName = null, bool isScrollIntoViewRequired = true)
        {
            bool flag = false;
            try
            {
                if (isScrollIntoViewRequired)
                    ScrollIntoView(_driver, locator);
                var element = _driver.FindElement(locator);
                //Few angular webcontrols do not expore enabled property hence combining with attribute value
                var classAttribute = element.GetAttribute("class");

                if (!element.Enabled || classAttribute.Contains("disabled"))
                {
                    flag = true;
                }

            }
            catch (Exception ex)
            {
                log.Error("Failed while verifying whether webElement is disabled or not: " + ex.Message);
            }
            if (!string.IsNullOrEmpty(elementName))
                _driver.ReportResult(test, flag, "Successfully verified that the webElement - " + elementName + " is disabled.", "The webElement - " + elementName + " is not disabled.");
            return flag;
        }

        ///<summary>
        ///Method to click a web webElement using Javascript executor
        ///<param name="element">web webElement - Located By.Id/By.XPath/...</param>
        ///<param name="webElementName">Web webElement being clicked</param>
        ///<param name="isReportRequired">true/false</param>
        ///</summary>
        public bool ClickElementUsingJavascript(IWebDriver _driver, IWebElement element, string webElementName = null, bool isReportRequired = false)
        {
            bool flag = false;

            try
            {

                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", element);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while clicking on the web Element in the application: " + ex.Message);
            }
            if (isReportRequired)
            {
                if (webElementName != null)
                    _driver.ReportResult(test, flag, "Successfully clicked on the web Element - " + webElementName + ".", "Failed while clicking on the web Element - " + webElementName + ".");
                else
                    _driver.ReportResult(test, flag, "Successfully clicked on the web Element " + element.Text + ".", "Failed while clicking on the web Element having locator as " + element.Text + ".");
            }
            return flag;
        }

        /// <summary>
        /// Method to Get Element Background Color
        /// </summary>
        /// <param name="_driver">web driver</param>
        /// <param name="locator">By.Id, xpath, etc</param>
        /// <returns></returns>
        public string GetBackgroundColor(IWebDriver _driver, By locator)
        {
            var bgColor = "";
            try
            {
                var element = _driver.FindElement(locator);
                bgColor = element.GetCssValue("background-color");
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting color value: " + ex.Message);
            }
            return bgColor.ToString().Trim();
        }

        /// <summary>
        /// Method to get a web Element's Attribute
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="locator">By Locator</param>
        /// <param name="attributeName">Attribute name</param>
        /// <param name="isReportRequired">true/false</param>
        /// <returns></returns>
        public string GetElementAttribute(IWebDriver _driver, By locator, string attributeName, bool isReportRequired = false)
        {
            bool flag = false;
            string attributeText = string.Empty;
            try
            {
                attributeText = _driver.FindElement(locator).GetAttribute(attributeName);
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while retrieving web Element's attribute : " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, "Successfully retrieved the web Element attribute - " + attributeText + ".", "Failed to retrieve the web Element attribute.");
            return attributeText;
        }

        public string NumberToCurrencyText(decimal number)
        {

            // Round the value just in case the decimal value is longer than two digits
            number = decimal.Round(number, 2, MidpointRounding.ToEven);

            string wordNumber = string.Empty;

            // Divide the number into the whole and fractional part strings
            string[] arrNumber = number.ToString().Split('.');

            // Get the whole number text
            long wholePart = long.Parse(arrNumber[0]);
            string strWholePart = NumberToText(wholePart);

            // For amounts of zero dollars show 'No Dollars...' instead of 'Zero Dollars...'
            wordNumber = (wholePart == 0 ? "No" : strWholePart);// + (wholePart == 1 ? " and " : " and ");

            // If the array has more than one webElement then there is a fractional part otherwise there isn't
            // just add 'No Cents' to the end
            if (arrNumber.Length > 1)
            {
                // If the length of the fractional webElement is only 1, add a 0 so that the text returned isn't,
                // 'One', 'Two', etc but 'Ten', 'Twenty', etc.
                long fractionPart = long.Parse((arrNumber[1].Length == 1 ? arrNumber[1] + "0" : arrNumber[1]));
                if (fractionPart > 0)
                {
                    string strFarctionPart = NumberToText(fractionPart);
                    wordNumber += " And " + (fractionPart == 0 ? " No" : strFarctionPart) + (fractionPart == 1 ? " Cent" : " Cents");
                }
            }
            else
                wordNumber += "No Cents";

            return wordNumber;
        }

        public static string NumberToText(long number)
        {
            StringBuilder wordNumber = new StringBuilder();

            string[] powers = new string[] { "Thousand ", "Million ", "Billion " };
            string[] tens = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
            string[] ones = new string[] { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten",
                                       "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

            if (number == 0) { return "Zero"; }
            if (number < 0)
            {
                wordNumber.Append("Negative ");
                number = -number;
            }

            long[] groupedNumber = new long[] { 0, 0, 0, 0 };
            int groupIndex = 0;

            while (number > 0)
            {
                groupedNumber[groupIndex++] = number % 1000;
                number /= 1000;
            }

            for (int i = 3; i >= 0; i--)
            {
                long group = groupedNumber[i];

                if (group >= 100)
                {
                    wordNumber.Append(ones[group / 100 - 1] + " Hundred ");
                    group %= 100;

                    if (group == 0 && i > 0)
                        wordNumber.Append(powers[i - 1]);
                }

                if (group >= 20)
                {
                    if ((group % 10) != 0)
                        wordNumber.Append(tens[group / 10 - 2] + " " + ones[group % 10 - 1] + " ");
                    else
                        wordNumber.Append(tens[group / 10 - 2] + " ");
                }
                else if (group > 0)
                    wordNumber.Append(ones[group - 1] + " ");

                if (group != 0 && i > 0)
                    wordNumber.Append(powers[i - 1]);
            }

            return wordNumber.ToString().Trim();
        }

        /// <summary>
        /// Retrieves the text of an webElement using JavaScript.
        /// </summary>
        /// <param name="_driver">The WebDriver instance.</param>
        /// <param name="locator">The locator to find the webElement.</param>
        /// <param name="scrollIntoView">Determines whether to scroll the webElement into view before retrieving the text. Default is true.</param>
        /// <param name="controlName">The name of the control. Used for reporting purposes.</param>
        /// <param name="isReportRequired">Determines whether to generate a report. Default is false.</param>
        /// <returns>The text of the webElement.</returns>
        public string GetElementTextUsingJS(IWebDriver _driver, By locator, bool scrollIntoView = true, string controlName = null, bool isReportRequired = false)
        {
            bool flag = false;
            IWebElement element = null;
            Actions action = new Actions(_driver);
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(_driver);
            string elementText = string.Empty;
            try
            {
                WaitForElement(_driver, locator);
                element = _driver.FindElement(locator);
                if (scrollIntoView)
                    elementText = ((IJavaScriptExecutor)_driver).ExecuteScript("return arguments[0].value", element).ToString().Trim();
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while getting text field webElement: " + ex.Message);
            }
            if (isReportRequired)
            {
                if (controlName != null)
                    _driver.ReportResult(test, flag, "Successfully got the " + controlName + " field value - " + elementText, "Failed while getting the " + controlName + " field value - " + elementText);
                else
                    _driver.ReportResult(test, flag, "Successfully got the value - " + elementText, "Failed while getting the value - " + elementText);
            }
            return elementText;
        }

        /// <summary>
        /// Method to wait until specific keyword is found in the Application URL
        /// </summary>
        /// <param name="keywordToBeSearchedInUrl">Application URL</param>
        /// <param name="isReportRequired">true / false</param>
        public void WaitUntilUrlContains(string keywordToBeSearchedInUrl, bool isReportRequired = false)
        {
            bool flag = false;
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(ConfigSettings.LongWaitTime));
                wait.Until(ExpectedConditions.UrlContains(keywordToBeSearchedInUrl));
                flag = true;
            }
            catch (Exception ex)
            {
                log.Error($"Failed while waiting for URL to contain '{keywordToBeSearchedInUrl}': " + ex.Message);
            }
            if (isReportRequired)
                _driver.ReportResult(test, flag, $"Successfull Verified Application URL contains '{keywordToBeSearchedInUrl}'", $"Failed while waiting for Application Url to contain '{keywordToBeSearchedInUrl}'");
        }

        /// <summary>
        /// Method to scroll to top of the page
        /// </summary>
        /// <param name="_driver">WebDriver</param>
        public void ScrollToTop(IWebDriver _driver)
        {
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, 0);");
            }
            catch (Exception ex)
            {
                log.Error("Failed while scrolling the web Element to Top of the page: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to scroll to bottom of the page
        /// </summary>
        /// <param name="_driver">WebDriver</param>
        public void ScrollToBottom(IWebDriver _driver)
        {
            try
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            }
            catch (Exception ex)
            {
                log.Error("Failed while scrolling the web Element to the bottom of the page: " + ex.Message);
            }
        }

        /// <summary>
        /// Method to wait for an webElement to become disabled.
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="locator">By.Id, XPath, etc.</param>
        /// <param name="elementName">Web Element name</param>
        /// <param name="timeoutInSeconds">Timeout in seconds to wait for the webElement to be disabled</param>
        public bool WaitForElementToBeDisabled(IWebDriver _driver, By locator, string elementName = null, int timeoutInSeconds = 10, bool isScrollIntoViewRequired = true)
        {
            bool flag = false;
            try
            {
                if (isScrollIntoViewRequired)
                    ScrollIntoView(_driver, locator);

                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(driver =>
                {
                    var webElement = driver.FindElement(locator);
                    return !webElement.Enabled;
                });

                var element = _driver.FindElement(locator);
                flag = !element.Enabled;
            }
            catch (Exception ex)
            {
                log.Error("Failed while waiting for the webElement to be disabled: " + ex.Message);
            }

            if (!string.IsNullOrEmpty(elementName))
                _driver.ReportResult(test, flag, "Successfully verified that the webElement - " + elementName + " is disabled.", "The webElement - " + elementName + " is not disabled.");

            return flag;
        }

        /// <summary>
        /// Method to wait for an webElement to become enabled.
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="locator">By.Id, XPath, etc.</param>
        /// <param name="elementName">Web webElement name</param>
        /// <param name="timeoutInSeconds">Timeout in seconds to wait for the webElement to be enabled</param>
        public bool WaitForElementToBeEnabled(IWebDriver _driver, By locator, string elementName = null, int timeoutInSeconds = 10, bool isScrollIntoViewRequired = true)
        {
            bool flag = false;
            try
            {
                if (isScrollIntoViewRequired)
                    ScrollIntoView(_driver, locator);

                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));

                var element = _driver.FindElement(locator);
                flag = element.Enabled;
            }
            catch (Exception ex)
            {
                log.Error("Failed while waiting for the webElement to be enabled: " + ex.Message);
            }

            if (!string.IsNullOrEmpty(elementName))
                _driver.ReportResult(test, flag, "Successfully verified that the webElement - " + elementName + " is enabled.", "The webElement - " + elementName + " is not enabled.");

            return flag;
        }

        /// <summary>
        /// Method to wait for an IWebElement to become enabled.
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="element">IWebElement</param>
        /// <param name="elementName">Web element name</param>
        /// <param name="timeoutInSeconds">Timeout in seconds to wait for the webElement to be enabled</param>
        public bool WaitForElementToBeEnabled(IWebDriver _driver, IWebElement element, string elementName = null, int timeoutInSeconds = 10, bool isScrollIntoViewRequired = true)
        {
            bool flag = false;
            try
            {
                // Scroll into view if required
                if (isScrollIntoViewRequired)
                    ScrollIntoView(_driver, element);

                // Wait until the element is clickable
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));

                // Check if the element is enabled
                flag = element.Enabled;
            }
            catch (Exception ex)
            {
                log.Error("Failed while waiting for the webElement to be enabled: " + ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Method to wait for an IWebElement to become disabled.
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="element">IWebElement</param>
        /// <param name="elementName">Web element name</param>
        /// <param name="timeoutInSeconds">Timeout in seconds to wait for the webElement to be disabled</param>
        public bool WaitForElementToBeDisabled(IWebDriver _driver, IWebElement element, string elementName = null, int timeoutInSeconds = 10, bool isScrollIntoViewRequired = true)
        {
            bool flag = false;
            try
            {
                // Scroll into view if required
                if (isScrollIntoViewRequired)
                    ScrollIntoView(_driver, element);

                // Wait until the element is disabled
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(driver =>
                {
                    // Check if the element is disabled
                    return !element.Enabled;
                });

                // Check the final state of the element
                flag = !element.Enabled;
            }
            catch (Exception ex)
            {
                log.Error("Failed while waiting for the webElement to be disabled: " + ex.Message);
            }
            return flag;
        }

        /// <summary>
        /// Method to handle a browser alert by accepting or dismissing it.
        /// </summary>
        /// <param name="_driver">Web driver</param>
        /// <param name="acceptAlert">True to accept the alert, false to dismiss</param>
        /// <param name="timeoutInSeconds">Timeout in seconds to wait for the alert</param>
        /// <returns>True if the alert was successfully handled, false otherwise</returns>
        public bool HandleAlert(IWebDriver _driver, bool acceptAlert = true, int timeoutInSeconds = 10)
        {
            bool flag = false;
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutInSeconds));
                wait.Until(driver =>
                {
                    try
                    {
                        // Try switching to alert to ensure it's present
                        IAlert alrt = driver.SwitchTo().Alert();
                        return true;
                    }
                    catch (NoAlertPresentException)
                    {
                        return false;
                    }
                });

                IAlert alert = _driver.SwitchTo().Alert();

                if (acceptAlert)
                {
                    alert.Accept();
                    log.Info("Alert accepted.");
                }
                else
                {
                    alert.Dismiss();
                    log.Info("Alert dismissed.");
                }

                flag = true;
            }
            catch (NoAlertPresentException)
            {
                log.Warn("No alert was present to handle.");
            }
            catch (Exception ex)
            {
                log.Error("Failed while handling the alert: " + ex.Message);
            }

            return flag;
        }

        /// <summary>
        /// Validate PaymentId/ProfileId Guid is valid guid or not
        /// </summary>
        /// <param name="profileId"></param>
        /// <returns></returns>

        public bool IsValidGuid(string profileId)
        {
            bool IsGuid = false;
            try
            {
                Regex guidRegExp = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
                IsGuid = guidRegExp.IsMatch(profileId);
                if (IsGuid) return true;
            }
            catch (Exception ex)
            {
                log.Error("Failed while validating Guid is in correct format or not: " + ex.Message);
            }
            return IsGuid;
        }

        #endregion Services
    }
}
