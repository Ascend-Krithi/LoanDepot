using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class PaymentPageLocators
    {
        public const string AmountInputKey = "PaymentPage.AmountInput";
        public static readonly By AmountInput = By.XPath("//input[@id='amount']");
        public static readonly By[] AmountInputAlternatives = {
            By.CssSelector("input[name='amount']"),
            By.XPath("//input[contains(@placeholder, 'Amount')]")
        };

        public const string PayButtonKey = "PaymentPage.PayButton";
        public static readonly By PayButton = By.XPath("//button[@id='payBtn']");
        public static readonly By[] PayButtonAlternatives = {
            By.CssSelector("button.pay"),
            By.XPath("//button[contains(text(),'Pay')]")
        };
    }
}