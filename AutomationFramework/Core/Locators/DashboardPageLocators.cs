using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class DashboardPageLocators
    {
        public const string LoanDropdownKey = "DashboardPage.LoanDropdown";
        public static readonly By LoanDropdown = By.XPath("//select[@id='loanType']");
        public static readonly By[] LoanDropdownAlternatives = {
            By.CssSelector("select[name='loan']"),
            By.XPath("//select[contains(@aria-label, 'Loan')]")
        };

        public const string LoanListKey = "DashboardPage.LoanList";
        public static readonly By LoanList = By.XPath("//ul[@id='loanList']/li");
        public static readonly By[] LoanListAlternatives = {
            By.CssSelector("ul.loan-list > li"),
            By.XPath("//li[contains(@class, 'loan-item')]")
        };

        public const string PopupCloseKey = "DashboardPage.PopupClose";
        public static readonly By PopupClose = By.XPath("//button[@class='close-popup']");
        public static readonly By[] PopupCloseAlternatives = {
            By.CssSelector("button[aria-label='Close']"),
            By.XPath("//button[contains(text(),'Close')]")
        };

        public const string ChatPopupKey = "DashboardPage.ChatPopup";
        public static readonly By ChatPopup = By.XPath("//div[@id='chat-popup']");
        public static readonly By[] ChatPopupAlternatives = {
            By.CssSelector("div.chat-popup"),
            By.XPath("//div[contains(@class, 'chat-popup')]")
        };

        public const string DatePickerInputKey = "DashboardPage.DatePickerInput";
        public static readonly By DatePickerInput = By.XPath("//input[@id='mat-datepicker']");
        public static readonly By[] DatePickerInputAlternatives = {
            By.CssSelector("input.mat-datepicker-input"),
            By.XPath("//input[contains(@aria-label, 'Date')]")
        };

        public const string MessageBannerKey = "DashboardPage.MessageBanner";
        public static readonly By MessageBanner = By.XPath("//div[@id='messageBanner']");
        public static readonly By[] MessageBannerAlternatives = {
            By.CssSelector("div.banner-message"),
            By.XPath("//div[contains(@class, 'message-banner')]")
        };
    }
}