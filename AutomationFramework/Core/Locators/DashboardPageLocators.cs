using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class DashboardPageLocators
    {
        public const string LoanDropdownKey = "DashboardPage.LoanDropdown";
        public static readonly By LoanDropdown = By.XPath("//select[@id='loanType']");
        public static readonly By[] LoanDropdownAlternatives = {
            By.XPath("//select[contains(@name,'loan')]"),
            By.XPath("//select[contains(@class,'loan')]")
        };

        public const string LoanListKey = "DashboardPage.LoanList";
        public static readonly By LoanList = By.XPath("//ul[@id='loanList']/li");
        public static readonly By[] LoanListAlternatives = {
            By.XPath("//div[@class='loan-list']//li"),
            By.XPath("//li[contains(@class,'loan-item')]")
        };

        public const string PopupCloseKey = "DashboardPage.PopupClose";
        public static readonly By PopupClose = By.XPath("//button[@class='close']");
        public static readonly By[] PopupCloseAlternatives = {
            By.XPath("//button[contains(@aria-label,'Close')]"),
            By.XPath("//span[@class='close']")
        };

        public const string ChatPopupKey = "DashboardPage.ChatPopup";
        public static readonly By ChatPopup = By.XPath("//div[@id='chat-popup']");
        public static readonly By[] ChatPopupAlternatives = {
            By.XPath("//div[contains(@class,'chat-popup')]"),
            By.XPath("//iframe[contains(@src,'chat')]")
        };

        public const string DatePickerInputKey = "DashboardPage.DatePickerInput";
        public static readonly By DatePickerInput = By.XPath("//input[@id='mat-datepicker']");
        public static readonly By[] DatePickerInputAlternatives = {
            By.XPath("//input[contains(@class,'mat-datepicker')]"),
            By.XPath("//input[@type='date']")
        };

        public const string MessageKey = "DashboardPage.Message";
        public static readonly By Message = By.XPath("//div[@class='message']");
        public static readonly By[] MessageAlternatives = {
            By.XPath("//span[@class='message']"),
            By.XPath("//div[contains(@class,'msg')]")
        };
    }
}