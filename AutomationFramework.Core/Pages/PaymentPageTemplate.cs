using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class PaymentPageTemplate : BasePage
    {
        private readonly By paymentDatePicker = By.Id("paymentDatePicker");
        private readonly By lateFeeMessageArea = By.Id("lateFeeMessage");

        public void OpenPaymentDatePicker()
        {
            var picker = FindElement("PaymentPage.PaymentDatePicker", paymentDatePicker);
            picker.Click();
        }

        public void SelectPaymentDate(string paymentDate)
        {
            // Assume calendar widget logic here
            var dateElement = FindElement("PaymentPage.PaymentDate", By.XPath($"//td[@data-date='{paymentDate}']"));
            dateElement.Click();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return IsElementVisible(lateFeeMessageArea);
        }
    }
}