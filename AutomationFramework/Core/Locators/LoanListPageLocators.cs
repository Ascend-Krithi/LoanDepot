using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class LoanListPageLocators
    {
        public const string LoanRows = "LoanListPage.LoanRows";
        public const string LoanRowByName = "LoanListPage.LoanRowByName";
        public const string DatePickerInput = "LoanListPage.DatePickerInput";
        public const string DatePickerDay = "LoanListPage.DatePickerDay";
        public const string PopupDialog = "LoanListPage.PopupDialog";
        public const string PopupClose = "LoanListPage.PopupClose";

        public static readonly By[] LoanRowsAlternatives = new By[]
        {
            By.CssSelector("table.loan-list tbody tr"),
            By.XPath("//table[contains(@class,'loan-list')]//tr")
        };

        public static By[] GetLoanRowByNameAlternatives(string loanName)
        {
            return new By[]
            {
                By.XPath($"//tr[td[contains(text(),'{loanName}')]]"),
                By.CssSelector($"tr[data-loan-name='{loanName}']")
            };
        }

        public static readonly By[] DatePickerInputAlternatives = new By[]
        {
            By.CssSelector("input[matdatepicker]"),
            By.CssSelector("input[type='date']")
        };

        public static By[] GetDatePickerDayAlternatives(string day)
        {
            return new By[]
            {
                By.XPath($"//mat-calendar//td[.='{day}']"),
                By.XPath($"//td[contains(@class,'mat-calendar-body-cell') and text()='{day}']")
            };
        }

        public static readonly By[] PopupDialogAlternatives = new By[]
        {
            By.CssSelector(".mat-dialog-container"),
            By.XPath("//div[contains(@class,'mat-dialog-container')]")
        };

        public static readonly By[] PopupCloseAlternatives = new By[]
        {
            By.CssSelector(".mat-dialog-container button.close"),
            By.XPath("//div[contains(@class,'mat-dialog-container')]//button[contains(@class,'close')]")
        };

        public static Dictionary<string, By[]> GetLocators()
        {
            return new Dictionary<string, By[]>
            {
                { LoanRows, LoanRowsAlternatives },
                { DatePickerInput, DatePickerInputAlternatives },
                { PopupDialog, PopupDialogAlternatives },
                { PopupClose, PopupCloseAlternatives }
            };
        }
    }
}