using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using AventStack.ExtentReports;
using LD_AutomationFramework.Pages;
using LD_AutomationFramework.Base;
using LD_AutomationFramework.Utilities;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// Page Object for Make Payment Page (source: Servicing _ loanDepot_Date Selection for Payment.html)
    /// </summary>
    public class MakePaymentPage
    {
        private IWebDriver _driver;
        private ExtentTest test;
        private WebElementExtensionsPage webElementExtensions;

        public MakePaymentPage(IWebDriver driver, ExtentTest test)
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
        }

        #region Locators
        // Source mapping: Servicing _ loanDepot_Date Selection for Payment.html
        public By pageReadyLocBy = By.XPath("//span[contains(@id,'spidPaymentActivity') and contains(.,'Make')]"); // Payment.PageReady
        public By datePickerToggleLocBy = By.CssSelector("input[matinput][aria-haspopup='dialog']"); // Payment.DatePickerToggle
        public By calendarOverlayLocBy = By.XPath("//mat-calendar"); // Payment.CalendarOverlay
        public By calendarPeriodButtonLocBy = By.CssSelector("button.mat-calendar-period-button"); // Payment.CalendarPeriodButton
        public By calendarYearLocBy(string year) => By.XPath($"//mat-multi-year-view//div[normalize-space(text())='{year}']"); // Payment.CalendarYear
        public By calendarMonthLocBy(string month) => By.XPath($"//mat-year-view//div[normalize-space(text())='{month}']"); // Payment.CalendarMonth
        public By calendarDayLocBy(string day) => By.XPath($"//mat-month-view//div[normalize-space(text())='{day}']"); // Payment.CalendarDay
        public By scheduledPaymentModalLocBy = By.XPath("//mat-dialog-container"); // Payment.ScheduledPaymentModal
        public By continueLocBy = By.XPath("//button//span[normalize-space(text())='Continue']"); // Payment.Continue
        public By lateFeeMessageLocBy = By.XPath("//div[contains(translate(text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),'late fee')]"); // Payment.LateFeeMessage
        #endregion

        #region Methods
        public void WaitForPaymentPage()
        {
            webElementExtensions.WaitForElement(_driver, pageReadyLocBy, ConfigSettings.WaitTime);
            test.Log(AventStack.ExtentReports.Status.Info, "Make Payment page loaded (spidPaymentActivity span present)");
        }

        public void SelectPaymentDate(string year, string month, string day)
        {
            webElementExtensions.Click(_driver, datePickerToggleLocBy, ConfigSettings.SmallWaitTime);
            webElementExtensions.WaitForElement(_driver, calendarOverlayLocBy, ConfigSettings.SmallWaitTime);
            webElementExtensions.Click(_driver, calendarPeriodButtonLocBy, ConfigSettings.SmallWaitTime);
            webElementExtensions.Click(_driver, calendarYearLocBy(year), ConfigSettings.SmallWaitTime);
            webElementExtensions.Click(_driver, calendarMonthLocBy(month), ConfigSettings.SmallWaitTime);
            webElementExtensions.Click(_driver, calendarDayLocBy(day), ConfigSettings.SmallWaitTime);
            test.Log(AventStack.ExtentReports.Status.Info, $"Selected payment date: {month} {day}, {year}");
        }

        public void ClickContinue()
        {
            webElementExtensions.Click(_driver, continueLocBy, ConfigSettings.SmallWaitTime);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked Continue button on payment modal");
        }

        public bool IsPaymentScreenDisplayed()
        {
            return webElementExtensions.IsElementDisplayed(_driver, pageReadyLocBy);
        }
        #endregion
    }
}
