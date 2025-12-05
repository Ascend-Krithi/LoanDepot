using AventStack.ExtentReports;
using AventStack.ExtentReports.Model;
using iTextSharp.text.xml.simpleparser;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Config;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Utilities;
using log4net;
using Microsoft.SqlServer.Server;
using Microsoft.Testing.Platform.OutputDevice;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace LD_CustomerPortal.TestPages
{


    /// <summary>
    /// Class to handle all Payoff Quotes functionality
    /// </summary>
    public class PayoffQuotePage
    {

        IWebDriver _driver { get; set; }
        ExtentTest test { get; set; }
        ReportLogger reportLogger { get; set; }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        WebElementExtensionsPage webElementExtensions = null;
        CommonServicesPage commonServices = null;
        public PayoffQuotePage(IWebDriver _driver, ExtentTest test)
        {
            this._driver = _driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(_driver, test);
            commonServices = new CommonServicesPage(_driver, test);
            reportLogger = new ReportLogger(_driver);
        }

        #region Locators
        public By pageTitleLocBy = By.Id("payoffPrimaryHeader");
        public By deliveryLocBy = By.Id("selectMethodOfDelivery");
        public By addressLine1LocBy = By.Id("AddressLine1");
        public By addressLine2LocBy = By.Id("AddressLine2");
        public By cityLocBy = By.Id("cityName");
        public By stateLocBy = By.Id("State");
        public By zipCodeBy = By.Id("zipcode");
        public By btnIdRequestPayoffLocBy = By.Id("btnIdRequestPayoff");
        public By dateSelectedLocBy = By.Id("txtIdDate");
        string deliveryMethodItemPath = "//*[contains(@id,'mat-option')]/span[contains(.,'{0}')]";
        public By datePickerLocBy = By.Id("mat-datepicker-0");
        public By dialogHeaderLocBy = By.XPath("//*[contains(@id,'mat-dialog')]//h5");
        public By btnCloseDialogLocBy = By.XPath("//button[@aria-label='close']");
        public By payoffQuoteContentLocBy = By.Id("pIdSubheading");
        public By backToHomeLocBy = By.XPath("//button[contains(.,'Back to Home')]");
        public By payoffDescriptionLocBy = By.Id("payoffSecondaryDescription");
       
        #endregion

        #region methods
        /// <summary>
        /// Requests payoff quote for given method
        /// </summary>
        /// <param name="deliveryMethod">method to choose</param>
        /// <returns>list of available dates</returns>
        public List<string> RequestPayoffQuote(string deliveryMethod)
        {
            List<string> datesAvailable = null;
            try
            {

                webElementExtensions.ClickElement(_driver, deliveryLocBy);
                By itemLocBy = By.XPath(string.Format(deliveryMethodItemPath, deliveryMethod));
                webElementExtensions.ScrollIntoView(_driver, itemLocBy);
                webElementExtensions.WaitUntilElementIsClickable(_driver, itemLocBy);
                webElementExtensions.ActionClick(_driver, itemLocBy);
                ReportingMethods.Log(test, $"Payoff Quote : {deliveryMethod}");
                if (deliveryMethod != "US Postal Mail")
                {
                    datesAvailable = GetAllAvailableDates(deliveryMethod);
                }
                else
                {
                    reportLogger.TakeScreenshot(test, $"{deliveryMethod}");
                }
               
            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, $"Error");
                test.Log(Status.Error, $"Eror while performing action, {e.Message}");
            }
            return datesAvailable;

        }


        /// <summary>
        /// Method to select the available payment date
        /// </summary>
        /// <param name="deliveryMethod">delivey method</param>
        public List<string> GetAllAvailableDates (string deliveryMethod)
        {
            List<string> formattedDates = new List<string>();
            try
            {
                var isCalanderOpened = webElementExtensions.IsElementDisplayed(_driver, datePickerLocBy);

                if (!isCalanderOpened)
                {
                    webElementExtensions.WaitUntilElementIsClickable(_driver, commonServices.paymentDatePickerIconLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.paymentDatePickerIconLocBy);
                }
                webElementExtensions.WaitForElement(_driver,commonServices.datesAvailableToSelectOnCalndrLocBy, ConfigSettings.SmallWaitTime);
                if (webElementExtensions.IsElementEnabled(_driver, commonServices.prevMonthButtonCalanderLocBy))
                {
                    webElementExtensions.WaitUntilElementIsClickable(_driver, commonServices.prevMonthButtonCalanderLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.prevMonthButtonCalanderLocBy);
                }

                List<int> datesAvailable = _driver.FindElements(commonServices.datesAvailableToSelectOnCalndrLocBy).
                    Select(element => int.Parse(element.Text)).ToList();
                formattedDates= GetFormattedDates(datesAvailable);
                // choose the next month optionally to check next month
                if (webElementExtensions.IsElementEnabled(_driver,commonServices.nextMonthLocBy))
                {
                    webElementExtensions.WaitUntilElementIsClickable(_driver, commonServices.nextMonthButtonCalanderLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.nextMonthButtonCalanderLocBy);
                    datesAvailable = _driver.FindElements(commonServices.datesAvailableToSelectOnCalndrLocBy).
                        Select(element => int.Parse(element.Text)).ToList();
                    formattedDates.AddRange(GetFormattedDates(datesAvailable));
                    webElementExtensions.ActionClick(_driver, commonServices.prevMonthButtonCalanderLocBy);
                }
                if (datesAvailable.Count == 0)
                    log.Error("Failed while getting available dates in Date field as there are no dates available for selection.");

                else
                {
                    reportLogger.TakeScreenshot(test, $"Available Dates For Delivery Method: {deliveryMethod}");
                   _driver.ReportResult(test, formattedDates.Count > 0, "Successfully filtered available dates - " + string.Join(", ", formattedDates) + $" in Date field for {deliveryMethod}.", "Failed while filtering dates in Date field.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed while filtering dates in Date field: " + ex.Message);
            }

            return formattedDates;
        }
        /// <summary>
        /// Get Formatted Dates from eligible dates of Calander 
        /// </summary>
        /// <param name="datesAvailable">enabled dates, only days</param>
        /// <returns></returns>
        public List<string> GetFormattedDates(List<int> datesAvailable)
        {
             List<string> formattedDates = new List<string>();
            try
            {
                //Gets the current month and year showing in calander , as we get only days (int)
                string monthYearOnCalendar = webElementExtensions.GetElementText(_driver, commonServices.monthYearOnCalendarTextBy);
                //Format it to date so that we can get valid month and year
                DateTime parsedDate = DateTime.ParseExact(monthYearOnCalendar, "MMM yyyy", CultureInfo.InvariantCulture);
                if (datesAvailable.Count > 0)
                {
                    foreach (int date in datesAvailable)
                    {
                        DateTime specificDate = new DateTime(parsedDate.Year, parsedDate.Month, date);
                        string formattedDate = specificDate.ToString("M/d/yyyy h:mm:ss tt");
                        //format the dates to required format
                        formattedDate = webElementExtensions.DateTimeConverter(_driver, formattedDate, "m/d/yyyy to fullMonthName d, yyyy");
                        formattedDates.Add(formattedDate);
                    }
                }
            }
            catch (Exception e)
            {
                reportLogger.TakeScreenshot(test, $"Error");
                test.Log(Status.Error, $"Eror while formatting dates, {e.Message}");
            }
            return formattedDates;
        }

        /// <summary>
        /// Method to select the payment date
        /// </summary>
        /// <param name="date">Date value in the following format - June 15, 2024</param>
        /// <param name="isReportRequired">true/false</param>
        public void SelecDateFromCalanderForPayoffQuote(string date, bool selectNextMonth = false)
        {
            bool flag = false;
            By dateToBeSelectedLocBy = null;
            try
            {
                dateToBeSelectedLocBy = By.CssSelector(commonServices.paymentDateTobeSelected.Replace("<DATETOBESELECTED>", date));
                bool isDateShowing = webElementExtensions.IsElementDisplayed(_driver,dateToBeSelectedLocBy);
                if (selectNextMonth && !isDateShowing)
                {
                    webElementExtensions.WaitUntilElementIsClickable(_driver,commonServices.nextMonthButtonCalanderLocBy);
                    webElementExtensions.ActionClick(_driver, commonServices.nextMonthButtonCalanderLocBy);
                }
                webElementExtensions.WaitForVisibilityOfElement(_driver,dateToBeSelectedLocBy);
                webElementExtensions.ClickElement(_driver, dateToBeSelectedLocBy);
                webElementExtensions.WaitForElement(_driver,dateSelectedLocBy);
                var dateselected = webElementExtensions.GetElementText(_driver,dateSelectedLocBy);
                reportLogger.TakeScreenshot(test, $"Payoff Quote Date Selected");
            }
            catch (Exception ex)
            {
                log.Error("Failed while selecting date in Date field: " + ex.Message);
            }
               
        }


        #endregion
    }
}
