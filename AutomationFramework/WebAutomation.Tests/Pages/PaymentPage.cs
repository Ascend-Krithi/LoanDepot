using OpenQA.Selenium;
using WebAutomation.Core.Pages;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        public PaymentPage(IWebDriver driver) : base(driver) { }

        public void WaitForPaymentPageReady()
        {
            Wait.UntilVisible(By.CssSelector("span:contains('Make a Payment')"));
        }

        public void OpenDatePicker()
        {
            Wait.UntilClickable(By.CssSelector("mat-datepicker-toggle button")).Click();
        }

        public void SelectPaymentDate(string paymentDate)
        {
            // Assumes MM/dd/yyyy format
            var dt = System.DateTime.Parse(paymentDate);
            Wait.UntilClickable(By.CssSelector("button.mat-calendar-period-button")).Click();
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt.Year}']")).Click();
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt:MMM}']")).Click();
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt.Day}']")).Click();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return Driver.FindElements(By.Id("latefeeInfoMsg1")).Count > 0;
        }
    }
}