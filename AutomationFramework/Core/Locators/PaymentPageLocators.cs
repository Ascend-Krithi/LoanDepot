using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class PaymentPageLocators
    {
        public const string PaymentDropdown = "PaymentPage.PaymentDropdown";
        public static readonly By PaymentDropdownLocator = By.XPath("//mat-select[@id='paymentType']");
        public static readonly By[] PaymentDropdownAlternatives = {
            By.CssSelector("mat-select#paymentType"),
            By.XPath("//select[@name='paymentType']")
        };

        public const string DatePickerInput = "PaymentPage.DatePickerInput";
        public static readonly By DatePickerInputLocator = By.XPath("//input[@id='mat-datepicker']");
        public static readonly By[] DatePickerInputAlternatives = {
            By.CssSelector("input.mat-datepicker-input"),
            By.XPath("//input[contains(@placeholder,'Date')]")
        };

        public const string SubmitButton = "PaymentPage.SubmitButton";
        public static readonly By SubmitButtonLocator = By.XPath("//button[@id='submitPayment']");
        public static readonly By[] SubmitButtonAlternatives = {
            By.CssSelector("button[type='submit']"),
            By.XPath("//button[contains(text(),'Submit')]")
        };
    }
}