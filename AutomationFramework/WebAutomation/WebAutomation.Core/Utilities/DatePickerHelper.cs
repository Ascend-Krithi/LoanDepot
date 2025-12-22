using OpenQA.Selenium;
using System.Globalization;

namespace WebAutomation.Core.Utilities
{
    public class DatePickerHelper
    {
        private readonly IWebDriver _driver;
        private readonly SmartWait _wait;

        public DatePickerHelper(IWebDriver driver)
        {
            _driver = driver;
            _wait = new SmartWait(driver);
        }

        /// <summary>
        /// Selects a date from a Material UI date picker.
        /// </summary>
        /// <param name="date">Date in "MM/dd/yyyy" format.</param>
        public void SelectDate(string date)
        {
            var dt = DateTime.ParseExact(
                date,
                "MM/dd/yyyy",
                CultureInfo.InvariantCulture
            );

            // Open date picker
            _wait.UntilClickable(By.CssSelector("mat-datepicker-toggle button"))
                .Click();

            _wait.WaitForOverlay();

            // Switch to year selection view
            _wait.UntilClickable(By.CssSelector("button.mat-calendar-period-button"))
                .Click();
            
            // Wait for year view to be ready
            _wait.UntilVisible(By.CssSelector("mat-multi-year-view"));

            // Select year
            _wait.UntilClickable(
                By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()=' {dt.Year} ']"))
                .Click();

            // Select month
            _wait.UntilClickable(
                By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()=' {dt:MMM} ']"))
                .Click();

            // Select day
            _wait.UntilClickable(
                By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()=' {dt.Day} ']"))
                .Click();

            _wait.WaitForOverlayToClose();
        }
    }
}