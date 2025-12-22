using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public PaymentPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators/Locators.txt");
        }

        public void ContinueScheduledPaymentIfPresent()
        {
            if (Popup.IsPresent(_locators.GetBy("Dashboard.ContactPopup")))
            {
                Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactContinue"));
            }
        }

        public void OpenDatePicker()
        {
            Wait.UntilClickable(_locators.GetBy("Payment.DatePicker.Toggle")).Click();
        }

        public void SelectPaymentDate(string paymentDate)
        {
            var day = int.Parse(paymentDate.Split('-')[2]);
            Wait.UntilClickable(_locators.GetBy("Payment.Calendar.Day", day.ToString())).Click();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return Popup.IsPresent(_locators.GetBy("Payment.LateFee.Message"));
        }
    }
}