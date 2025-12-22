using OpenQA.Selenium;
using WebAutomation.Core.Utilities;
using System;
using System.Globalization;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage
    {
        private readonly IWebDriver _driver;
        private readonly SmartWait _wait;

        public PaymentPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new SmartWait(driver);
        }

        public bool IsPageReady()
        {
            // Note: :contains is not supported in CSS, so use XPath for text match
            return _wait.UntilPresent(By.XPath("//span[contains(text(),'Make a Payment')]"), 10);
        }

        public void OpenDatePicker()
        {
            _wait.UntilClickable(By.CssSelector("mat-datepicker-toggle button")).Click();
            _wait.WaitForOverlay();
        }

        public bool IsDatePickerOpen()
        {
            return _wait.UntilPresent(By.CssSelector("mat-datepicker-content"), 5);
        }

        public void SelectPaymentDate(string date)
        {
            var dt = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var day = dt.Day.ToString();
            _wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and normalize-space(text())='{day}']")).Click();
            _wait.WaitForOverlayToClose();
        }

        public string GetSelectedPaymentDate()
        {
            var input = _driver.FindElement(By.CssSelector("input[formcontrolname='paymentDate']"));
            return input.GetAttribute("value");
        }

        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                var el = _driver.FindElement(By.Id("latefeeInfoMsg1"));
                return el.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}