using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class DashboardPageLocators
    {
        public const string WelcomeMessage = "DashboardPage.WelcomeMessage";
        public const string LoanListDropdown = "DashboardPage.LoanListDropdown";
        public const string ChatPopup = "DashboardPage.ChatPopup";
        public const string ChatPopupClose = "DashboardPage.ChatPopupClose";

        public static readonly By[] WelcomeMessageAlternatives = new By[]
        {
            By.CssSelector(".welcome-message"),
            By.XPath("//h1[contains(text(),'Welcome')]")
        };

        public static readonly By[] LoanListDropdownAlternatives = new By[]
        {
            By.Id("loanList"),
            By.CssSelector("mat-select[formcontrolname='loan']")
        };

        public static readonly By[] ChatPopupAlternatives = new By[]
        {
            By.Id("chat-popup"),
            By.CssSelector(".chat-popup")
        };

        public static readonly By[] ChatPopupCloseAlternatives = new By[]
        {
            By.CssSelector(".chat-popup .close"),
            By.XPath("//div[@id='chat-popup']//button[contains(@class,'close')]")
        };

        public static Dictionary<string, By[]> GetLocators()
        {
            return new Dictionary<string, By[]>
            {
                { WelcomeMessage, WelcomeMessageAlternatives },
                { LoanListDropdown, LoanListDropdownAlternatives },
                { ChatPopup, ChatPopupAlternatives },
                { ChatPopupClose, ChatPopupCloseAlternatives }
            };
        }
    }
}