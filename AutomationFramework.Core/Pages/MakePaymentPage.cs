using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Pages;
using System;

namespace AutomationFramework.Core.Pages
{
    public class MakePaymentPage : BasePage
    {
        public static readonly By PageReady = By.XPath("//span[contains(@id,'spidPaymentActivity') and contains(.,'Make')]");
        public static readonly By DatePickerToggle = By.CssSelector("input[matinput][aria-haspopup='dialog']");
        public static readonly By CalendarOverlay = By.XPath("//mat-calendar");
        public static readonly By CalendarPeriodButton = By.CssSelector("button.mat-calendar-period-button");
        public static readonly string CalendarYearFormat = "//mat-multi-year-view//div[normalize-space(text())='{0}']";
        public static readonly string CalendarMonthFormat = "//mat-year-view//div[normalize-space(text())='{0}']";
        public static readonly string CalendarDayFormat = "//mat-month-view//div[normalize-space(text())='{0}']";
        public static readonly By ScheduledPaymentModal = By.XPath("//mat-dialog-container");
        public static readonly By Continue = By.XPath("//button//span[normalize-space(text())='Continue']");
        public static readonly By LateFeeMessage = By.XPath("//div[contains(translate(text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),'late fee')]");

        public MakePaymentPage(SelfHealingWebDriver driver) : base(driver) { }

        public void WaitForPageReady()
        {
            FindElement("Payment.PageReady", PageReady);
        }

        public void OpenDatePicker()
        {
            FindElement("Payment.DatePickerToggle", DatePickerToggle).Click();
        }

        public void SelectCalendarYear(string year)
        {
            FindElement("Payment.CalendarPeriodButton", CalendarPeriodButton).Click();
            var locator = By.XPath(string.Format(CalendarYearFormat, year));
            FindElement("Payment.CalendarYear", locator).Click();
        }

        public void SelectCalendarMonth(string month)
        {
            var locator = By.XPath(string.Format(CalendarMonthFormat, month));
            FindElement("Payment.CalendarMonth", locator).Click();
        }

        public void SelectCalendarDay(string day)
        {
            var locator = By.XPath(string.Format(CalendarDayFormat, day));
            FindElement("Payment.CalendarDay", locator).Click();
        }

        public void ClickContinue()
        {
            try
            {
                var modal = FindElement("Payment.ScheduledPaymentModal", ScheduledPaymentModal, 3);
                if (modal != null && modal.Displayed)
                {
                    FindElement("Payment.Continue", Continue).Click();
                }
            }
            catch
            {
                // Modal not present, nothing to do
            }
        }

        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                var element = FindElement("Payment.LateFeeMessage", LateFeeMessage, 3);
                return element != null && element.Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}