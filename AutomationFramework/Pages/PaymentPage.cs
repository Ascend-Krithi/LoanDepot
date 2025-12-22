using OpenQA.Selenium;

namespace AutomationFramework.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly By PageReady = By.CssSelector("span:contains('Make a Payment')");
        private readonly By DatePickerToggle = By.CssSelector("mat-datepicker-toggle button");
        private readonly string CalendarDayXpath = "//div[contains(@class,'mat-calendar-body-cell-content') and normalize-space(text())='{0}']";
        private readonly By LateFeeMessage = By.Id("latefeeInfoMsg1");
        private readonly By ScheduledPaymentPopup = By.CssSelector("mat-dialog-container");
        private readonly By ContinueButton = By.XPath("//button[normalize-space()='Continue']");

        public bool IsPageReady()
        {
            return IsElementVisible(PageReady);
        }

        public bool IsScheduledPaymentPopupDisplayed()
        {
            return IsElementVisible(ScheduledPaymentPopup);
        }

        public void ClickContinueOnScheduledPaymentPopup()
        {
            Click(ContinueButton);
        }

        public void OpenDatePicker()
        {
            Click(DatePickerToggle);
        }

        public bool IsCalendarDisplayed()
        {
            // Implement check for calendar widget
            return true; // Placeholder
        }

        public void SelectPaymentDate(string date)
        {
            // Assume date is in format "yyyy-MM-dd"
            var day = DateTime.Parse(date).Day.ToString();
            var calendarDay = By.XPath(string.Format(CalendarDayXpath, day));
            Click(calendarDay);
        }

        public bool IsDateSelected(string date)
        {
            // Implement check for selected date in field
            return true; // Placeholder
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return IsElementVisible(LateFeeMessage);
        }
    }
}