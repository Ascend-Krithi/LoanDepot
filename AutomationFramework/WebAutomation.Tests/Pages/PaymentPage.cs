using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Utilities;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _locators;
        private readonly DatePickerHelper _datePickerHelper;

        public PaymentPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators/Locators.txt");
            _datePickerHelper = new DatePickerHelper(driver);
        }

        public void OpenDatePicker()
        {
            Wait.UntilClickable(_locators.GetBy("Payment.DatePicker.Toggle")).Click();
        }

        public void SelectPaymentDate(string paymentDate)
        {
            _datePickerHelper.SelectDate(paymentDate);
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return Popup.IsPresent(_locators.GetBy("Payment.LateFee.Message"));
        }
    }
}