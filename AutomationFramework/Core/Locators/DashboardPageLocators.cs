using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class DashboardPageLocators
    {
        public const string LoanDropdownKey = "Dashboard.LoanDropdown";
        public static readonly By LoanDropdown = By.XPath("//select[@id='loanType']");
        public static readonly By[] LoanDropdownAlternatives = {
            By.CssSelector("select.loan-type"),
            By.XPath("//select[contains(@name,'loan')]")
        };

        public const string LoanListKey = "Dashboard.LoanList";
        public static readonly By LoanList = By.XPath("//ul[@id='loanList']/li");
        public static readonly By[] LoanListAlternatives = {
            By.CssSelector("ul.loan-list > li"),
            By.XPath("//div[@class='loan-list']//li")
        };

        public const string ChatPopupKey = "Dashboard.ChatPopup";
        public static readonly By ChatPopup = By.XPath("//div[@id='chatPopup']");
        public static readonly By[] ChatPopupAlternatives = {
            By.CssSelector("div.chat-popup"),
            By.XPath("//div[contains(@class,'chat-popup')]")
        };

        public const string DatePickerKey = "Dashboard.DatePicker";
        public static readonly By DatePicker = By.XPath("//input[@id='mat-datepicker']");
        public static readonly By[] DatePickerAlternatives = {
            By.CssSelector("input.mat-datepicker-input"),
            By.XPath("//input[contains(@class,'mat-datepicker')]")
        };
    }
}