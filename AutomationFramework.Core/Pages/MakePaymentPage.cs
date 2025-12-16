using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class MakePaymentPage : BasePage
    {
        public MakePaymentPage(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement PageReady() => FindElement("Payment.PageReady", By.XPath("//span[contains(@id,'spidPaymentActivity') and contains(.,'Make')]"));
        public IWebElement DatePickerToggle() => FindElement("Payment.DatePickerToggle", By.CssSelector("input[matinput][aria-haspopup='dialog']"));
        public IWebElement CalendarOverlay() => FindElement("Payment.CalendarOverlay", By.XPath("//mat-calendar"));
        public IWebElement CalendarPeriodButton() => FindElement("Payment.CalendarPeriodButton", By.CssSelector("button.mat-calendar-period-button"));
        public IWebElement CalendarYear(string year) => FindElement("Payment.CalendarYear", By.XPath($"//mat-multi-year-view//div[normalize-space(text())='{year}']"));
        public IWebElement CalendarMonth(string month) => FindElement("Payment.CalendarMonth", By.XPath($"//mat-year-view//div[normalize-space(text())='{month}']"));
        public IWebElement CalendarDay(string day) => FindElement("Payment.CalendarDay", By.XPath($"//mat-month-view//div[normalize-space(text())='{day}']"));
        public IWebElement ScheduledPaymentModal() => FindElement("Payment.ScheduledPaymentModal", By.XPath("//mat-dialog-container"));
        public IWebElement Continue() => FindElement("Payment.Continue", By.XPath("//button//span[normalize-space(text())='Continue']"));
        public IWebElement LateFeeMessage() => FindElement("Payment.LateFeeMessage", By.XPath("//div[contains(translate(text(),'ABCDEFGHIJKLMNOPQRSTUVWXYZ','abcdefghijklmnopqrstuvwxyz'),'late fee')]"));
    }
}